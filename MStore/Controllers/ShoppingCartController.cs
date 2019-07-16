using MStore.Models;
using MStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        private formdbEntities db = new formdbEntities();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                cartTotal = cart.GetTotal()
            };
            
            return View(viewModel);
        }

        public ActionResult addToCart(int id)
        {
            var album = db.Album.Where(x => x.AlbumID == id).FirstOrDefault();
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.AddToCart(album);
            return RedirectToAction("Index");

        }
        [HttpPost]
        public ActionResult removeFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            var removed = db.Cart.Single(x => x.recordID == id).Album.Title;
            int itemCount =  cart.removeFromCart(id);
            var removeModel = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(removed) + "has been removed.",
                CartCount = cart.GetCount(),
                CartTotal = cart.GetTotal(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(removeModel);
        }

        public ActionResult cartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            ViewData["cartCount"] = cart.GetCount();
            return PartialView("cartSummary");
        }


    }
}