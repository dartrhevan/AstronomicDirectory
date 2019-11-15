using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AstronomicDirectory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Web.Models;
using Web.Models.DataAccessMySqlProvider;

namespace Web.Controllers
{
    public class ViewsController : Controller
    {
        private readonly AstronomicDirectoryDbContext db = new AstronomicDirectoryDbContext();

        public IActionResult PlanetView(int starId, int planetId)
        {
            var planet = db.Planets.First(p => p.Id == planetId && p.StarId == starId);
            //foreach (var dbMoon in db.Moons/*.Where(pl => pl.PlanetId == planetId)*/)
            //{ }
            db.Moons.Load();
            return View(planet);
        }

        public IActionResult MoonView(int moonId) => View(db.Moons.Find(moonId));

        public ActionResult GetImage(int id)
        {

            var dbStar = db.Stars.Find(id);

            var contents = dbStar.Photo ?? new byte[0];
            return base.File(contents, "image/jpeg");
        }

        public ActionResult GetPlanetImage(int id)
        {
            byte[] contents;
            var flag = HttpContext.Session.Get("imgflag");

            if (flag != null && flag[0] == 1)
            {
                var fileContents = HttpContext.Session.Get("img");
                contents = fileContents;
                HttpContext.Session.Set("imgflag", new byte[1] {0});
            }
            else
            {
                db.Planets.Load();
                var planet = db.Planets.Find(id);
                contents = planet.Photo ?? new byte[0];
            }
            
            return base.File(contents, "image/jpeg");
        }

        public ActionResult GetMoonImage(int id)
        {
            byte[] contents;

            var flag = HttpContext.Session.Get("imgflag");
            if (flag != null && flag[0] == 1)
            {
                var fileContents = HttpContext.Session.Get("img");
                contents = fileContents;
                HttpContext.Session.Set("imgflag", new byte[1] {0});
            }
            else
            {
                var moon = db.Moons.Find(id);
                contents = moon.Photo ?? new byte[0];
            }

            return base.File(contents, "image/jpeg");
        }


        public IActionResult StarView(int id)
        {
            var dbs = db.Stars.Find(id);
            //dbs.Planets = new Collection<DBPlanet>(db.Planets.Where(pl => pl.StarId == dbs.Id).ToList());
            db.Planets.Load();
            //foreach (var planet in db.Planets/*.Where(pl => pl.StarId == dbs.Id)*/)
            //{ }
            return View(dbs);
        }

        public ActionResult Print(int id)
        {
            var star = db.Stars.Include(s1 => s1.Planets).First(s1 => s1.Id == id);
            db.Moons.Load();

            //var p = HttpContext.Current.Server.MapPath("template.xlsx");

            IWorkbook workbook = new XSSFWorkbook(System.IO.File.OpenRead("template.xlsx"));
            var sheet = workbook.GetSheetAt(0);
            sheet.GetRow(1).Cells[1].SetCellValue(star.Name);
            PrintStar(star, sheet);
            var ms = new MemoryStream();
            workbook.Write(ms);
            ms.Position = 0;
            return base.File(ms, "application/octet-stream", "star" + id + ".xlsx");
        }

        private static void PrintStar(DBStar star, ISheet sheet)
        {
            for (var i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++) //Read Excel File
            {
                var row = sheet.GetRow(i);
                if (row == null) continue;
                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                var lastCellNum = row.LastCellNum;
                for (int j = row.FirstCellNum; j < lastCellNum; j++)
                {
                    var cell = row.GetCell(j);
                    if (cell != null)
                    {
                        if (cell.StringCellValue == "$PropRow")
                        {
                            PrintStarProp(star, sheet, ref i, row, j, cell);
                            break;
                        }

                        if (cell.StringCellValue == "$Planets")
                        {
                            foreach (var planet in star.Planets)
                                PrintPlanets(star, sheet, i++, row, j, cell, planet);
                            break;
                        }

                        if (cell.StringCellValue == "$Moons")
                        {
                            foreach (var moon in star.Planets.SelectMany(p => p.Moons))
                                PrintMoons(star, sheet, i++, row, j, cell, moon);
                            break;
                        }
                    }
                }
            }
        }

        private static void PrintMoons(DBStar star, ISheet sheet, int i, IRow row, int j, ICell cell, DBMoon moon)
        {
            row = sheet.GetRow(i);
            cell = row.GetCell(j);
            cell.SetCellValue(moon.Name);
            cell = row.GetCell(j + 1) ?? row.CreateCell(j + 1);
            cell.SetCellValue(moon.MiddleDistanceValue +
                              moon.MiddleDistanceUnit.ToString());
            cell = row.GetCell(j + 2) ?? row.CreateCell(j + 2);
            cell.SetCellValue(moon.Radius);
            cell = row.GetCell(j + 3) ?? row.CreateCell(j + 3);
            cell.SetCellValue(moon.PlanetOwner);
            cell = row.GetCell(j + 4) ?? row.CreateCell(j + 4);
            cell.SetCellValue(moon.Temperature);
            cell = row.GetCell(j + 5) ?? row.CreateCell(j + 5);
            cell.SetCellValue(moon.InventingDate);
            cell.CellStyle.DataFormat = 14;

            cell = row.GetCell(j + 6) ?? row.CreateCell(j + 6);
            cell.SetCellValue(moon.HasAtmosphere); /*
            if (planet != star.Planets.Last())
                row = sheet.CopyRow(i, i + 1);*/
        }


        private static void PrintStarProp(DBStar star, ISheet sheet, ref int i, IRow row, int j, ICell cell)
        {
            cell.SetCellValue("Дата открытия");
            cell = row.GetCell(j + 1) ?? row.CreateCell(j + 1);
            cell.SetCellValue(star.InventingDate);
            cell.CellStyle.DataFormat = 14;

            row = sheet.CopyRow(i, i + 1);
            i++;
            cell = row.GetCell(j) ?? row.CreateCell(j);
            cell.SetCellValue("Галактика");
            row.CreateCell(j + 1).SetCellValue(star.Galaxy ?? "");
            row = sheet.CreateRow(i++);
            cell = row.GetCell(j) ?? row.CreateCell(j);
            cell.SetCellValue("Радиус");
            row.CreateCell(j + 1).SetCellValue(star.Radius);
            row = sheet.CreateRow(i++);

            cell = row.GetCell(j) ?? row.CreateCell(j);
            cell.SetCellValue("Температура");
            row.CreateCell(j + 1).SetCellValue(star.Temperature);
            row = sheet.CreateRow(i++);

            cell = row.GetCell(j) ?? row.CreateCell(j);
            cell.SetCellValue("Среднее расстояние");
            row.CreateCell(j + 1).SetCellValue(star.MiddleDistanceValue);
            row = sheet.CreateRow(i++);
        }

        private static void PrintPlanets(DBStar star, ISheet sheet, int i, IRow row, int j, ICell cell, DBPlanet planet)
        {
            row = sheet.GetRow(i);
            cell = row.GetCell(j);
            cell.SetCellValue(planet.Name);
            cell = row.GetCell(j + 1) ?? row.CreateCell(j + 1);
            cell.SetCellValue(planet.MiddleDistanceValue +
                              planet.MiddleDistanceUnit.ToString());
            cell = row.GetCell(j + 2) ?? row.CreateCell(j + 2);
            cell.SetCellValue(planet.Radius);
            cell = row.GetCell(j + 3) ?? row.CreateCell(j + 3);
            cell.SetCellValue(planet.Star);
            cell = row.GetCell(j + 4) ?? row.CreateCell(j + 4);
            cell.SetCellValue(planet.Temperature);
            cell = row.GetCell(j + 5) ?? row.CreateCell(j + 5);
            cell.SetCellValue(planet.Type.ToString());
            cell = row.GetCell(j + 6) ?? row.CreateCell(j + 6);
            cell.SetCellValue(planet.InventingDate);
            cell.CellStyle.DataFormat = 14;

            cell = row.GetCell(j + 7) ?? row.CreateCell(j + 7);
            cell.SetCellValue(planet.HasAtmosphere);
            if (planet != star.Planets.Last())
                row = sheet.CopyRow(i, i + 1);
        }

        public FileContentResult DownloadStar(int id)
        {
            var dbs = db.Stars.Find(id);
            db.Planets.Load();
            db.Moons.Load();
            var xml = new XmlSerializer(typeof(Star));
            var str = new MemoryStream();
            xml.Serialize(str, dbs.ToStar());
            return File(str.ToArray(), "application/xml", dbs.Name + ".star");
        }

        [HttpPost]
        public IActionResult StarView(IFormFile star)
        {
            var xml = new XmlSerializer(typeof(Star));
            var fs = star.OpenReadStream();
            var st = (Star) xml.Deserialize(fs);
            fs.Close();
            var dbs = new DBStar(st);
            //if (db.Stars.FirstOrDefault(s => s.Name == st.Name) == null)
            //{
            db.Stars.Add(dbs);
            db.Planets.AddRange(dbs.Planets);
            db.Moons.AddRange(dbs.Planets.SelectMany(pl => pl.Moons));
            //foreach (var pl in dbs.Planets)
            //    if(pl.Moons != null)

            db.SaveChanges();
            //}

            return View(dbs);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            db.Dispose();
        }

        
        public string StarListJson()
        {
            var list = db.Stars.Select(s => new StarLite(s.Id, s.Name, s.Galaxy)).ToList();
            return JsonConvert.SerializeObject(list);
        }

        public string StarJson(int id)
        {
            var star = db.Stars.Find(id);
            return JsonConvert.SerializeObject(star);
        }
        //public class StarList
        //{
        //    public List<StarLite> ArrayOfStarLite { get; set; }
        //}

        public string StarListXml()
        {
            var list = db.Stars.Select(s => new StarLite(s.Id, s.Name, s.Galaxy)).ToList();
            var xml = new XmlSerializer(typeof(List<StarLite>));
            var str = new StringWriter();
            xml.Serialize(str, list);
            return str.ToString();
        }

        public string StarXml(int id)
        {
            db.Planets.Load();
            db.Moons.Load();
            var star = db.Stars.Find(id); 
            var xml = new XmlSerializer(typeof(Star));
            var str = new StringWriter();
            var st = star.ToStar();
            xml.Serialize(str, st);
            return str.ToString();
        }

        [HttpPost]
        public async Task StarViews(/*string s*/)
        {
            var xml = new XmlSerializer(typeof(Star));
            var requestBody = HttpContext.Request.Body;
            var st = (Star)xml.Deserialize(requestBody/*new StringReader(s)*/);
            //var st = JsonConvert.DeserializeObject<Star>(s);
            var dbs = new DBStar(st);            
            using (var db = new AstronomicDirectoryDbContext())
            {
                //if (db.Stars.FirstOrDefault(s => s.Name == st.Name) == null)
                await db.Stars.AddAsync(dbs);
                await db.Planets.AddRangeAsync(dbs.Planets);
                await db.Moons.AddRangeAsync(dbs.Planets.SelectMany(pl => pl.Moons));
                //foreach (var pl in dbs.Planets)
                //    if(pl.Moons != null)
                await db.SaveChangesAsync();
            }
        }

        public class StarPair
        {
            public int Id { get; set; }
            public Star Star { get; set; }
        }

        public string test()
        {
            var str = new StringWriter();
            var tup = new StarPair() { Id = 2, Star = new Star() { Name = "st" } };
            var xml = new XmlSerializer(typeof(StarPair));
            xml.Serialize(str, tup);
            return str.ToString();
        }

        [HttpPost]
        public void EditStar()
        {
            var xml = new XmlSerializer(typeof(StarPair));
            var requestBody = HttpContext.Request.Body;
            var st = (StarPair)xml.Deserialize(requestBody/*new StringReader(s)*/);
            //var st = JsonConvert.DeserializeObject<Star>(s);
            var dbs = new DBStar(st.Star);
            dbs.Id = st.Id;
            using (var db = new AstronomicDirectoryDbContext())
            {
                db.Entry(dbs).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}