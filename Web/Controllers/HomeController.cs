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
using Web.Models.DataAccessMySqlProvider;

namespace Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly AstronomicDirectoryDbContext db = new AstronomicDirectoryDbContext();

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Delete(int id)
        {
            //Book b = new Book { Id = id };
            db.Entry(db.Stars.Find(id)).State = EntityState.Deleted;
            db.SaveChanges();

            return RedirectToAction("Index");
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
        
        public IActionResult List()
        {
            List<DBStar> list = db.Stars.ToList();
            return View(list);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            db.Dispose();
        }


    }
}
