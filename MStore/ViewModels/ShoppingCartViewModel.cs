using MStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MStore.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<Cart> CartItems { get; set; }
        public decimal cartTotal { get; set; }
    }
}