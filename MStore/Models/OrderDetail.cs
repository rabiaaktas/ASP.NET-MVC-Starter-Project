//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MStore.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderDetail
    {
        public int OrderDetailID { get; set; }
        public int orderId { get; set; }
        public int albumId { get; set; }
        public int quantity { get; set; }
        public decimal unitPrice { get; set; }
    
        public virtual Album Album { get; set; }
        public virtual Order Order { get; set; }
    }
}
