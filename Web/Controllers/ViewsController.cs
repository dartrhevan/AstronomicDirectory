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
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Web.Models.DataAccessMySqlProvider;

namespace Web.Controllers
{
    public class ViewsController : Controller
    {
        public IActionResult PlanetView(int starId, int planetId)
        {
            using (var db = new AstronomicDirectoryDbContext())
            {
                var planet = db.Planets.First(p => p.Id == planetId && p.StarId == starId);
                //foreach (var dbMoon in db.Moons/*.Where(pl => pl.PlanetId == planetId)*/)
                //{ }
                db.Moons.Load();

                return View(planet);
            }
        }

        public IActionResult MoonView(int moonId)
        {
            using (var db = new AstronomicDirectoryDbContext())
            {
                return View(db.Moons.Find(moonId));
            }
        }

        public ActionResult GetImage(int id)
        {
            using (var db = new AstronomicDirectoryDbContext())
            {
                return base.File(db.Stars.Find(id).Photo ?? new byte[] { }, "image/jpeg");
            }
        }

        public ActionResult GetPlanetImage(int id)
        {
            using (var db = new AstronomicDirectoryDbContext())
            {
                return base.File(db.Planets.Find(id).Photo ?? new byte[] { }, "image/jpeg");
            }
        }

        public IActionResult StarView(int id)
        {
            using (var db = new AstronomicDirectoryDbContext())
            {
                var dbs = db.Stars.Find(id);
                //dbs.Planets = new Collection<DBPlanet>(db.Planets.Where(pl => pl.StarId == dbs.Id).ToList());
                db.Planets.Load();
                //foreach (var planet in db.Planets/*.Where(pl => pl.StarId == dbs.Id)*/)
                //{ }
                return View(dbs);
            }
        }

        public ActionResult Print(int id)
        {
            using (var db = new AstronomicDirectoryDbContext())
            {
                var star = db.Stars.Include(s1 => s1.Planets).First(s1 => s1.Id == id);
                db.Moons.Load();
                IWorkbook workbook =
                    new XSSFWorkbook(System.IO.File.OpenRead("template.xlsx"));

                var sheet = workbook.GetSheetAt(0);

                sheet.GetRow(1).Cells[1].SetCellValue(star.Name);

                PrintStar(star, sheet);

                var ms = new MemoryStream();
                workbook.Write(ms);

                ms.Position = 0;

                return base.File(ms, "application/octet-stream", "star" + id + ".xlsx");
            }
        }

        private static void PrintStar(DBStar star, ISheet sheet)
        {
            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
            {
                IRow row = sheet.GetRow(i);
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
                            //{
                                PrintPlanets(star, sheet, i++, row, j, cell, planet);
                            //    i++;
                            //}

                            break;
                        }

                        if (cell.StringCellValue == "$Moons")
                        {
                            foreach (var moon in star.Planets.SelectMany(p => p.Moons))
                                //{
                                PrintMoons(star, sheet, i++, row, j, cell, moon);
                            //    i++;
                            //}

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
            cell.SetCellValue(moon.HasAtmosphere);/*
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
            using (var db = new AstronomicDirectoryDbContext())
            {
                var dbs = db.Stars.Find(id);
                db.Planets.Load();
                db.Moons.Load();
                var xml = new XmlSerializer(typeof(Star));
                var str = new MemoryStream();
                xml.Serialize(str, dbs.ToStar());
                return File(str.ToArray(), "application/xml", dbs.Name + ".star");
            }
        }

        [HttpPost]
        public IActionResult StarView(IFormFile star)
        {
            var xml = new XmlSerializer(typeof(Star));
            var fs = star.OpenReadStream();
            var st = (Star)xml.Deserialize(fs);
            fs.Close();
            var dbs = new DBStar(st);
            using (var db = new AstronomicDirectoryDbContext())
            {
                //if (db.Stars.FirstOrDefault(s => s.Name == st.Name) == null)
                //{
                db.Stars.Add(dbs);
                db.Planets.AddRange(dbs.Planets);
                db.Moons.AddRange(dbs.Planets.SelectMany(pl => pl.Moons));
                //foreach (var pl in dbs.Planets)
                //    if(pl.Moons != null)

                db.SaveChanges();
                //}

            }
            return View(dbs);
        }

    }
}