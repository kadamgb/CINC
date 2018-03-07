using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NQR.CINC.Api.Controllers
{
    [RoutePrefix("api/Orders")]
    public class OrdersController : ApiController
    {
        //added the “Authorize” attribute so if you tried to issue HTTP GET request to the end point “http://localhost:port/api/orders” 
        //you will receive HTTP status code 401 unauthorized because the request you send till this moment doesn’t contain valid authorization header
        [Authorize]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(Order.CreateOrders());
        }
    }
    #region Helpers
    public class Order
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; }
        public string ShipperCity { get; set; }
        public Boolean IsShipped { get; set; }

        public static List<Order> CreateOrders()
        {
            List<Order> OrderList = new List<Order>
            {
                new Order {OrderID = 10248, CustomerName = "TATA", ShipperCity = "Mumbai", IsShipped = true },
                new Order {OrderID = 10249, CustomerName = "BIRLA", ShipperCity = "London", IsShipped = false},
                new Order {OrderID = 10250,CustomerName = "Ambani", ShipperCity = "Gujarat", IsShipped = false },
                new Order {OrderID = 10251,CustomerName = "Kadam", ShipperCity = "Satara", IsShipped = false},
                new Order {OrderID = 10252,CustomerName = "Adani", ShipperCity = "Delhi", IsShipped = true}
            };

            return OrderList;
        }
    }
    #endregion
}
