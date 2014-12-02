using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace idei.Models
{
    public class OrderList
    {
        public int OrderListId { get; set; }
        public int OrderId { get; set; }
        public int RecordId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public virtual Record Record { get; set; }
        public virtual Order Order { get; set; }
    }
}