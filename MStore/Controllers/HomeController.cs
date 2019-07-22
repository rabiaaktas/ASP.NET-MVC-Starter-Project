using MStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MStore.Controllers
{
    public class HomeController : Controller
    {
        private formdbEntities db = new formdbEntities();
        public ActionResult Index()
        {
            var albums = getTopSellingAlbums(5);
            return View(albums);
        }
       // [Authorize(Roles = "Admin")]

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private List<Album> getTopSellingAlbums(int count)
        {
            return db.Album.OrderByDescending(x => x.OrderDetails.Count())
                .Take(count).ToList();
        }
    }
}