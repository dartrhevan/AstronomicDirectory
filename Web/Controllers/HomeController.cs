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
            //if(st.Photo != null)
            //    HttpContext.Session.Set("photo", st.Photo);


            var dbs = new DBStar(st);
            using (var db = new AstronomicDirectoryDbContext())
            {
                db.Stars.Add(dbs);
                db.SaveChanges();
            }

            return View(dbs);
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
            //var img = Image.FromStream(new MemoryStream(arr));
            //var photo = HttpContext.Session.Get("photo");

            //var str = new MemoryStream(photo);
            using (var db = new AstronomicDirectoryDbContext())
            {
                return base.File(db.Stars.Find(id).Photo, "image/jpeg");
            }
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
