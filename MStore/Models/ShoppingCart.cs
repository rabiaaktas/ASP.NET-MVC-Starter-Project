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
        //helper for calling shopping cart
        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }
       
        public void AddToCart(Album album)
        {
            //get the specifed cart and check the album existence.
            var cartItem = db.Cart.SingleOrDefault(c => c.cartID == shoppingCartID && c.AlbumId == album.AlbumID);
            if (cartItem == null)
            {
                //add new item to cart.
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
                //if the album exists than add one to its count.
                cartItem.count++;
            }
            db.SaveChanges();
        }


        public int removeFromCart(int id)
        {
            var removedItem = db.Cart.Where(x => x.cartID == shoppingCartID).Where(x => x.recordID == id).SingleOrDefault();
            int itemCount = 0;
            if(removedItem != null)
            {
                if(removedItem.count > 1)
                {
                    removedItem.count--;
                    itemCount = removedItem.count;
                }

                else
                {
                    db.Cart.Remove(removedItem);
                }
                db.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = db.Cart.Where(x => x.cartID == shoppingCartID);
            foreach(var item in cartItems)
            {
                db.Cart.Remove(item);
            }
            db.SaveChanges();
        }

        public List<Cart> GetCartItems()
        {
            return db.Cart.Where(x => x.cartID == shoppingCartID).ToList();
        }
        //get the count for each item in card and sum them up
        public int GetCount()
        {
            int? count = (from cartItems in db.Cart
                          where cartItems.cartID == shoppingCartID
                          select (int?)cartItems.count).Sum();
            return count ?? 0;
        }

        public decimal GetTotal()
        {

            decimal? total = (from cartItems in db.Cart
                              where cartItems.cartID == shoppingCartID
                              select (int?)cartItems.count *
                              cartItems.Album.Price).Sum();

            return total ?? decimal.Zero;
        }

        public int CreateOrder(Order order)
        {
            decimal totalOrder = 0;
            var cartItems = GetCartItems();

            foreach(var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    albumId = item.AlbumId,
                    orderId = order.OrderID,
                    unitPrice = item.Album.Price,
                    quantity = item.count
                };
                totalOrder += (item.count * item.Album.Price);
                db.OrderDetail.Add(orderDetail);
            }
            order.total = totalOrder;
            db.SaveChanges();
            EmptyCart();
            return order.OrderID;
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

        public void MigrateCart(string userName)
        {
            var shoppingCart = db.Cart.Where(
                c => c.cartID == shoppingCartID);

            foreach (Cart item in shoppingCart)
            {
                item.cartID = userName;
            }
            db.SaveChanges();
        }
    }
}