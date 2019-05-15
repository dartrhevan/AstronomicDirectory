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