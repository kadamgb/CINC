using Newtonsoft.Json;
using NQR.CINC.Web.Filters;
using NQR.CINC.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NQR.CINC.Web.Controllers
{
    public class HomeController : Controller
    {
        //private string _secretKey = System.Configuration.ConfigurationManager.AppSettings.Get("SecretKey");      // secret key to identify client application e.g. mvc or wpf or mobile 
        //private string _oathBaseURL = System.Configuration.ConfigurationManager.AppSettings.Get("OathBaseUrl");  // Othorization server url
        private string _webApiBaseUrl = System.Configuration.ConfigurationManager.AppSettings.Get("WebApiBaseUrl"); // webApi server url     
        [CustomRoleAuthorizationAttribute(Roles = "Manager")]
        public ActionResult Index()
        {
            string userName = User.Identity.Name;
            var httpClient = new HttpClient();
            string accessToken = Convert.ToString(Session["AccessToken"]);
            httpClient.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", accessToken));
            try
            {
                HttpResponseMessage response = httpClient.GetAsync(_webApiBaseUrl + "api/account/all").Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    List<RegisterUserViewModel> userList = JsonConvert.DeserializeObject<List<RegisterUserViewModel>>(data);
                    //orderModel = JsonConvert.DeserializeObject<List<OrderModel>>(orders);
                    ViewData["activelinkId"] = "dashboard";
                    return View(userList);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return View("Error");
        }

        public ActionResult About()
        {           
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}