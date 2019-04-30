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

            using (var db = new AstronomicDirectoryDbContext())
            {
                var dbs = new DBStar(st);
                //var dbs = new DbSpaceShip()
                //{
                //    Name = ship.Name,
                //    Build = ship.Build,
                //    Photo = ship.Photo,
                //};
                //dbs.Journal = new Collection<DbFlight>();
                //foreach (var flight in ship.Journal)
                //{
                //    dbs.Journal.Add(new DbFlight()
                //    {
                //        Crew = flight.Crew,
                //        From = flight.From,
                //        Passengers = flight.Passengers,
                //        To = flight.To
                //    });
                //}

                db.Stars.Add(dbs);
                db.SaveChanges();
            }

            return View(st);
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

        public IActionResult GetImage(byte[] photo)
        {
            //var img = Image.FromStream(new MemoryStream(arr));
            var str = new MemoryStream(photo);
            
            return File(str, "image/jpeg");
        }

        //public FileResult Download()
        //{
        //    string file_path = "~/Files/Form.rar";
        //    // Тип файла - content-type
        //    string file_type = "";
        //    // Имя файла - необязательно
        //    string file_name = "Form.rar";
        //    return File(file_path, file_type, file_name);
        //}
        [HttpPost]
        public IActionResult PlanetView(Planet planet)
        {
            return View(planet);
        }

        //public static byte[] PlanetToBytes(Planet pl)
        //{
        //    var xml = new XmlSerializer(typeof(Planet));
        //    var fs = new MemoryStream();
        //    xml.Serialize(fs, pl);
        //    //var st = (Star)xml.Deserialize(fs);
        //    return fs.ToArray();
        //}

        //static Planet PlanetFromBytes(byte[] arr)
        //{
        //    var xml = new XmlSerializer(typeof(Planet));
        //    var fs = new MemoryStream(arr);
        //    return xml.Deserialize(fs) as Planet;
        //}
    }
}
