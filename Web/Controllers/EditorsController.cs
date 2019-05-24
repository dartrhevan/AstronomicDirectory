using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using AstronomicDirectory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Models.DataAccessMySqlProvider;

namespace Web.Controllers
{
    public class EditorsController : Controller
    {
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

        public IActionResult PlanetEditor(string name, string owner, string gal)
        {
            var l = new List<DBMoon>();
            SaveToSession(l, null, "moons");
            return View("PlanetEditor", Tuple.Create(name, owner, gal));
        }

        public IActionResult MoonEditor(string name, string owner, string gal) =>
            View(Tuple.Create(name, owner, gal));


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
            var xml = new XmlSerializer(typeof(List<DBPlanet>));
            var stream = new MemoryStream(HttpContext.Session.Get("planets"));
            var planets = xml.Deserialize(stream) as List<DBPlanet>;
            foreach (var planet in planets)
                dbs.Planets.Add(planet);
            using (var db = new AstronomicDirectoryDbContext())
            {
                db.Stars.Add(dbs);
                db.Planets.AddRange(dbs.Planets);
                db.Moons.AddRange(dbs.Planets.SelectMany(pl => pl.Moons));
                db.SaveChanges();
                HttpContext.Session.Set("img", ph);
                //HttpContext.Session.Set("imgflag", new byte[1]{1});
            }
            return View("~/Views/Views/StarView.cshtml", dbs);
        }

        [HttpPost]
        public IActionResult AddMoon(IFormFile Photo, string Name, string Galaxy, uint Radius, uint Temperature, DateTime Date, uint Dist, string Unit)
        {
            byte[] ph = new byte[0];
            if (Photo != null)
            {
                var str = Photo.OpenReadStream();
                ph = new byte[str.Length];
                str.Read(ph, 0, ph.Length);
                str.Close();
            }
            var xml = new XmlSerializer(typeof(List<DBMoon>));
            var stream = new MemoryStream(HttpContext.Session.Get("moons"));
            var moons = xml.Deserialize(stream) as List<DBMoon>;
            using (var db = new AstronomicDirectoryDbContext())
            {
                var moon = new DBMoon(Date, ph, Name, new Distance(Dist, StringToUnit(Unit)), Radius, false, PlanetType.Gas, "", Galaxy, Temperature);
                moons.Add(moon);
                //moon.Id = -1;
                SaveToSession(moons, xml, "moons");
                HttpContext.Session.Set("img", ph);
                HttpContext.Session.Set("imgflag", new byte[1] { 1 });

                return View("~/Views/Views/MoonView.cshtml", moon);
            }
        }


        [HttpPost]
        public IActionResult AddPlanet(IFormFile Photo, string Name, string Galaxy, uint Radius, uint Temperature, DateTime Date, uint Dist, string Unit, bool atm, bool type)
        {
            byte[] ph = new byte[0];
            if (Photo != null)
            {
                var str = Photo.OpenReadStream();
                ph = new byte[str.Length];
                str.Read(ph, 0, ph.Length);
                str.Close();
            }
            var xml = new XmlSerializer(typeof(List<DBPlanet>));
            var stream = new MemoryStream(HttpContext.Session.Get("planets"));
            var planets = xml.Deserialize(stream) as List<DBPlanet>;
            using (var db = new AstronomicDirectoryDbContext())
            {
                var planet = new DBPlanet(Date, ph, Name, new Distance(Dist, StringToUnit(Unit)), Radius, atm, type?PlanetType.Tought : PlanetType.Gas, "", Galaxy, Temperature);
                var mx = new XmlSerializer(typeof(List<DBMoon>));
                var st = new MemoryStream(HttpContext.Session.Get("moons"));
                var moons = mx.Deserialize(st) as List<DBMoon>;
                planet.Moons = new Collection<DBMoon>(moons);
                planets.Add(planet);
                SaveToSession(planets, xml);
                HttpContext.Session.Set("img", ph);
                HttpContext.Session.Set("imgflag", new byte[1] { 1 });

                //HttpContext.Response.WriteAsync(@"onload = ""window.close();""");

                //planet.Id = -1;
                return View("~/Views/Views/PlanetView.cshtml", planet);
            }
        }

        public IActionResult StarEditor()
        {
            var l = new List<DBPlanet>();
            SaveToSession(l);
            var star = new DBStar();
            return View(star);
        }

        private void SaveToSession(object l, XmlSerializer x = null, string key = "planets")
        {
            var xml = x ?? new XmlSerializer(l.GetType());
            var str = new MemoryStream();
            xml.Serialize(str, l);
            HttpContext.Session.Set(key, str.ToArray());
        }
    }
}