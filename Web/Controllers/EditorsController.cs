using System;
using System.Collections.Generic;
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
        public IActionResult PlanetEditor(string name)
        {
            //var pl = new DBPlanet();
            //st.Planets.Add(pl);
            return View(name);
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
            var xml = new XmlSerializer(typeof(List<DBPlanet>));
            var stream = new MemoryStream(HttpContext.Session.Get("planets"));
            //xml.Serialize(str, l);
            var planets = xml.Deserialize(stream) as List<DBPlanet>;
            foreach (var planet in planets)
                dbs.Planets.Add(planet);
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

            return View("~/Views/Views/StarView.cshtml", dbs);
        }

        [HttpPost]
        public IActionResult AddPlanet(IFormFile Photo, string Name, string Galaxy, uint Radius, uint Temperature, DateTime Date, uint Dist, string Unit)
        {
            byte[] ph = new byte[0];
            if (Photo != null)
            {
                var str = Photo.OpenReadStream();
                ph = new byte[str.Length];
                str.Read(ph, 0, ph.Length);
                str.Close();
            }

            //var l = new List<DBPlanet>();
            var xml = new XmlSerializer(typeof(List<DBPlanet>));
            var stream = new MemoryStream(HttpContext.Session.Get("planets"));
            //xml.Serialize(str, l);
            var planets = xml.Deserialize(stream) as List<DBPlanet>;

            using (var db = new AstronomicDirectoryDbContext())
            {
                //var st = db.Stars.Find(StId);
                var planet = new DBPlanet(Date, ph, Name, new Distance(Dist, StringToUnit(Unit)), Radius, false, PlanetType.Gas, "", Galaxy, Temperature);
                planets.Add(planet);

                //var l = new List<DBPlanet>();
                //var xml = new XmlSerializer(l.GetType());
                //var str = new MemoryStream();
                //xml.Serialize(str, planets);
                //HttpContext.Session.Set("planets", str.ToArray());
                SaveToSession(planets, xml);

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

        private void SaveToSession(List<DBPlanet> l, XmlSerializer x = null)
        {
            var xml = x ?? new XmlSerializer(l.GetType());
            var str = new MemoryStream();
            xml.Serialize(str, l);
            HttpContext.Session.Set("planets", str.ToArray());
        }

    }


}