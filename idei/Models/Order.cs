using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace idei.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public String UserId { get; set; }
        public ApplicationUser User { get; set; }
        public System.DateTime OrderDate { get; set; }
        public virtual ICollection<OrderList> OrderLists { get; set; }
        //private decimal total;
        //public decimal Total
        //{
        //    get
        //    {
        //        return total;
        //    }
        //    set
        //    {
        //        foreach (OrderList o in OrderLists)
        //        {
        //            total += o.UnitPrice * o.Quantity;
        //        }
        //    }
        //}
        //public decimal getTotal()
        //{
        //    decimal total = 0;
        //    foreach (OrderList o in OrderLists)
        //    {
        //        total = o.UnitPrice * o.Quantity;
        //    }
        //    return total;
        //}

    }
}