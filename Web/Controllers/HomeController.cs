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

        [HttpPost]
        public IActionResult AddStar(IFormFile Photo, string Name, string Galaxy, uint Radius, uint Temperature, DateTime Date, uint Dist, string Unit)
        {
            var str = Photo.OpenReadStream();
            var ph = new byte[str.Length];
            str.Read(ph, 0, ph.Length);
            var dbs = new DBStar(Date, ph, Name, new Distance(Dist, Unit[0] == 'A' ? UnitType.AstronomicUnits : UnitType.LightYears), Radius, Temperature, Galaxy);
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

            str.Close();
            return View("~/Views/Home/StarView.cshtml", dbs);
        }
    }
}
