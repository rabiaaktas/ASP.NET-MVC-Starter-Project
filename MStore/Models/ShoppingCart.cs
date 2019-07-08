using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MStore.Models;

namespace MStore.Models
{
    public class ShoppingCart
    {
        formdbEntities db = new formdbEntities();
        string shoppingCartID { get; set; }
        public const string cartSessionKey = "CartID";

        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.shoppingCartID = cart.GetCartId(context);
            return cart;

        }

        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public void AddToCart(Album album)
        {
            var cartItem = db.Cart.SingleOrDefault(c => c.cartID == shoppingCartID && c.AlbumId == album.AlbumID);
            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    AlbumId = album.AlbumID,
                    cartID = shoppingCartID,
                    count = 1,
                    createdDate = DateTime.Now.Date
                };
                db.Cart.Add(cartItem);

            }
            else
            {
                cartItem.count++;
            }
            db.SaveChanges();
        }




    

        public string GetCartId(HttpContextBase context)
        {
            if(context.Session[cartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[cartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    context.Session[cartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[cartSessionKey].ToString();

        }
    }
}