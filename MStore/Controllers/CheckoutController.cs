using MStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MStore.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
    // GET: Checkout
    private formdbEntities db = new formdbEntities();
    const string promoCode = "FREE";
        public ActionResult AddressAndPayment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection formCollection)
        {
            var order = new Order();
            TryUpdateModel(order);
            try
            {
                //if(string.Equals(Values["promoCode"],promoCode,StringComparison.OrdinalIgnoreCase) == false)
                //{
                //    return View(order);
                //}
                //else
                //{
                    var cart = ShoppingCart.GetCart(this.HttpContext);
                    order.username = User.Identity.Name;
                    order.orderDate = DateTime.Now;
                    order.total = cart.GetTotal();
                    db.Order.Add(order);
                    db.SaveChanges();
                   
                    cart.CreateOrder(order);
                    return RedirectToAction("Complete", new { id = order.OrderID });
                //}
            }
            catch
            {
                return View(order);
            }
        }

        public ActionResult Complete()
        {
            int i = Convert.ToInt32(RouteData.Values["id"]);
            var order = db.Order.Where(x => x.OrderID == i).FirstOrDefault();
            return View(order);            
        }

    }
}


