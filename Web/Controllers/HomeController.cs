using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AstronomicDirectory;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Web.Models;
using Web.Models.DataAccessPostgreSqlProvider;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
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

        public IActionResult StarEditor()
        {
            var star = new DBStar();
            return View(star);
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

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Uploading()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult GetImage(int id)
        {
            using (var db = new AstronomicDirectoryDbContext())
            {
                return base.File(db.Stars.Find(id).Photo??new byte[] {}, "image/jpeg");
            }
        }

        public ActionResult GetPlanetImage(int id)
        {
            using (var db = new AstronomicDirectoryDbContext())
            {
                return base.File(db.Planets.Find(id).Photo ?? new byte[] { }, "image/jpeg");
            }
        }


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

        public IActionResult List()
        {

            List<DBStar> list;
            using (var db = new AstronomicDirectoryDbContext())
            {
                list = db.Stars.ToList();
            }

            return View(list);
        }

        static UnitType StringToUnit(string s)
        {
            switch (s[0])
            {
                case 'K':
                    return UnitType.Kilometers;
                case 'A':
                    return UnitType.AstronomicUnits;
                case 'L':
                    return UnitType.LightYears;
                default:
                    throw new ArgumentException();
            }
        }

        public IActionResult PlanetEditor(DBStar st)
        {
            //var pl = new DBPlanet();
            //st.Planets.Add(pl);
            return View(st);
        }

        [HttpPost]
        public IActionResult AddStar(IFormFile Photo, string Name, string Galaxy, uint Radius, uint Temperature, DateTime Date, uint Dist, string Unit)
        {
            byte[] ph = new byte[0];
            if (Photo != null)
            {
                var str = Photo.OpenReadStream();
                ph = new byte[str.Length];
                str.Read(ph, 0, ph.Length);
                str.Close();
            }

            var dbs = new DBStar(Date, ph, Name, new Distance(Dist, StringToUnit(Unit)), Radius, Temperature, Galaxy);
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
            return View("~/Views/Home/StarView.cshtml", dbs);
        }

        [HttpPost]
        public IActionResult AddPlanet(int StId, IFormFile Photo, string Name, string Galaxy, uint Radius, uint Temperature, DateTime Date, uint Dist, string Unit)
        {
            byte[] ph = new byte[0];
            if (Photo != null)
            {
                var str = Photo.OpenReadStream();
                ph = new byte[str.Length];
                str.Read(ph, 0, ph.Length);
                str.Close();
            }

            using (var db = new AstronomicDirectoryDbContext())
            {
                var st = db.Stars.Find(StId);
                var planet = new DBPlanet(Date, ph, Name, new Distance(Dist, StringToUnit(Unit)), Radius, false, PlanetType.Gas, st) {StarId = StId};
                return View("~/Views/Home/PlanetView.cshtml", planet);
            }
        }
    }
}
