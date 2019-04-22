using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AstronomicDirectory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Web.Models;

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
            return base.View(st);
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
    }
}
