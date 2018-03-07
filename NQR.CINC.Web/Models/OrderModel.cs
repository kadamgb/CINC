using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NQR.CINC.Web.Models
{
    public class OrderModel
    {
        public string OrderID { get; set; }
        public string CustomerName { get; set; }
        public string ShipperCity { get; set; }
        public bool IsShipped { get; set; }      
    }
}