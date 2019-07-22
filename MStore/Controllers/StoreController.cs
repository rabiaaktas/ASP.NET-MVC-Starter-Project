using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MStore.Components;
using MStore.Models;

namespace MStore.Controllers
{
   
    public class StoreController : Controller
    {
        private formdbEntities db = new formdbEntities();


        // GET: Store
        //[AllowAnonymous]
        public ActionResult Index()
        {
            var lang = RouteData.Values["language"].ToString();
            if(lang == "tr-TR")
            {
                lang = "tr";
            }
            var listItems = db.Album.Where(x => x.lang == lang).ToList();
            return View(listItems);
        }

        public ActionResult Browse(string genre)
        {
            //var id = RouteData.Values["id"].ToString();
            var lang = RouteData.Values["language"].ToString();
            if (lang == "tr-TR")
            {
                lang = "tr";
            }
            var albums = db.Album.Where(x => x.lang == lang).Where(x => x.Genre1.Name == genre).ToList();
            return View(albums);
        }
        //Get details about product
        public ActionResult Details(int id)
        {
            // db.Album.Find(id); Böyle de kulanılır.
            var lang = RouteData.Values["language"].ToString();
            if (lang == "tr-TR")
            {
                lang = "tr";
            }
           
            var album = db.Album.Where(x => x.AlbumID == id)
                .Where(x => x.lang == lang).FirstOrDefault();

            return View(album);
        }
        //Düzenlenecek değerler.
        [HttpGet]
       //[SessionAuthorization]  // --- Bunu yazarsak authorize olmadığı için 403 error döndürür. Çalıştırmaz.
       
        public ActionResult Edit()
        {
            Album album = new Album();
            var lang = RouteData.Values["language"].ToString();
            if (lang == "tr-TR")
            {
                lang = "tr";
            }
            if (RouteData.Values["id"] != null)
            {
                var idE = Convert.ToInt32(RouteData.Values["id"]);
                if (idE > 0)
                {
                    var editInf = db.Album.Where(x => x.AlbumID == idE).Where(x => x.lang == lang).FirstOrDefault();
                    album = editInf;
                }

            }
            return View(album);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Album album)
        {
            var getAl = db.Album.Where(x => x.AlbumID == album.AlbumID).FirstOrDefault();
            getAl.Title = album.Title;
            getAl.Price = album.Price;
            getAl.Genre1.Name = album.Genre1.Name;
            getAl.Price = album.Price;
            getAl.Artist.Name = album.Artist.Name;
            getAl.lang = album.lang;

            //db.Album.Add(getAl);
            db.SaveChanges();
            return RedirectToAction("Index", "Store");
        }

        [HttpGet]
        public ActionResult Create()
        {
            var artists = db.Artist.ToList();
            ViewBag.artists = artists;
            return View();
        }
        [HttpPost]
        public ActionResult Create(Album album)
        {
            //Album insertedAlbum = new Album();
            //if (album != null)
            //{
            //    insertedAlbum.Title = album.Title;
            //    if(album != null)
            //    {
            //        //var artist = db.Artist.Where(x => x.Name == album.Artist.Name).FirstOrDefault();
            //        //insertedAlbum.ArtistId = artist.ArtistID;
            //        //var genre = db.Genre1.Where(x => x.Name == album.Artist.Name).FirstOrDefault();
            //        //insertedAlbum.Genre1.GenreID = genre.GenreID;

            //    }
            db.Album.Add(album);
            db.SaveChanges();

            return RedirectToAction("Details", "Store", new { id = album.AlbumID });

        }

       //Get /en/Store/Delete/1
       [HttpGet]
        public ActionResult Delete(int id)
        {
            var lang = RouteData.Values["language"].ToString();
            if (lang == "tr-TR")
            {
                lang = "tr";
            }
            Album album = db.Album.Where(x => x.lang == lang).Where(x => x.AlbumID == id).FirstOrDefault();

            return View(album);

        }

        [HttpPost]
        public ActionResult DeleteConf(int id)
        {
            var deleted = db.Album.Find(id);
            db.Album.Remove(deleted);
            db.SaveChanges();
            return RedirectToAction("Index","Store");

        }
        [ChildActionOnly]
        public ActionResult GenreMenu()
        {
            var genres = db.Genre1.ToList();
            return PartialView(genres);
        }

    }
}
