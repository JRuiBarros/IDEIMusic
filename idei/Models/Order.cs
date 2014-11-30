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
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

    }
}