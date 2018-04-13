using Newtonsoft.Json;
using NQR.CINC.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace NQR.CINC.Web.Controllers
{
    public class PatientController : Controller
    {
        private string _webApiBaseUrl = System.Configuration.ConfigurationManager.AppSettings.Get("WebApiBaseUrl"); // Web Api server url
        // GET: Patient
        public ActionResult Index()
        {
            HttpClient httpClient = new HttpClient();
            string accessToken = Convert.ToString(Session["AccessToken"]);
            httpClient.DefaultRequestHeaders.Add("Autorization", string.Format("Bearer {0}", accessToken));
            HttpResponseMessage response = httpClient.GetAsync(_webApiBaseUrl + "api/patient/all").Result;
            if(response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                List<PatientModel> patientList = JsonConvert.DeserializeObject<List<PatientModel>>(data);
                ViewData["activelinkId"] = "patient";
                return View(patientList);
            }          
            return View("Error");
        }


        /// <summary>
        /// Create/Add new Patient
        /// </summary>
        /// <returns></returns>                
        public ActionResult Create()
        {
            ViewData["activelinkId"] = "patient";
            return View();
        }
        [HttpPost]
        public ActionResult Create(PatientModel patientModel)
        {

            ViewData["activelinkId"] = "patient";
            return View();
        }

    }
}