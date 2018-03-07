using NQR.CINC.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Claims;
using NQR.CINC.Services.UserService;
using NQR.CINC.Services.Infrastructure;
using System.Web;

namespace NQR.CINC.Web.Controllers
{
    public class LoginController : BaseMvcController
    {
        private string _secretKey = System.Configuration.ConfigurationManager.AppSettings.Get("SecretKey");      // secret key to identify client application e.g. mvc or wpf or mobile 
        private string _oathBaseURL = System.Configuration.ConfigurationManager.AppSettings.Get("OathBaseUrl");  // Othorization server url
        private string _webApiBaseUrl = System.Configuration.ConfigurationManager.AppSettings.Get("WebApiBaseUrl"); // webApi server url

        private IUserRepository _userRepository;
        public LoginController(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _userRepository = unitOfWork.UserRepositoryInstance;
        }


        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(UserModel user)
        {
            // call audience : this code will check for which client is sending request like WPF app or mvc app or some other app
            //var audienceClient = new HttpClient();
            //var data = new List<KeyValuePair<string, string>>
            //                            {
            //                                new KeyValuePair<string, string>("Name", secretKey)
            //                            };

            //var contnt = new FormUrlEncodedContent(data);

            //var audresponse = await audienceClient.PostAsync(_oathBaseURL+"api/audience", contnt);

            //if (audresponse.IsSuccessStatusCode)
            //{
            //    string stateInfo = audresponse.Content.ReadAsStringAsync().Result;

            //}
            // get token 
            AccessToken token = new AccessToken();
            var client = new HttpClient();
            var inputData = new List<KeyValuePair<string, string>>
                                        {
                                            new KeyValuePair<string, string>("grant_type", "password"),
                                            new KeyValuePair<string, string>("username", user.UserName),
                                            new KeyValuePair<string, string>("password", user.Password),
                                            new KeyValuePair<string, string>("client_id", _secretKey)
                                        };

            var content = new FormUrlEncodedContent(inputData);
            HttpResponseMessage response;
            try
            {
                response = await client.PostAsync(_oathBaseURL + "oauth2/token", content);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (response.IsSuccessStatusCode)
            {
                string stateInfo = response.Content.ReadAsStringAsync().Result;
                token = JsonConvert.DeserializeObject<AccessToken>(stateInfo);
            }
            else
            {
                return View();
            }            

            Session["AccessToken"] = token.access_token;

            // here getting all roles of user by his username(email) to authorize him when redirect to home controller
            //List<string> roles = _userRepository.GetUserRoles(user.UserName);
            string roles = "Manager";

            FormsAuthentication.SetAuthCookie(user.UserName, false);

            var authTicket = new FormsAuthenticationTicket(1, user.UserName, DateTime.Now, DateTime.Now.AddMinutes(20), false, roles);
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            HttpContext.Response.Cookies.Add(authCookie);
            Session["CurrentLoggedInUser"] = user.UserName;
            return RedirectToAction("index", "Home");
        }
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            { 
                var client = new HttpClient();
                var inputData = new List<KeyValuePair<string, string>>
                                        {
                                            new KeyValuePair<string, string>("UserName", model.UserName),
                                            new KeyValuePair<string, string>("Password", model.Password),
                                            new KeyValuePair<string, string>("ConfirmPassword", model.ConfirmPassword),
                                            new KeyValuePair<string, string>("Email", model.Email),
                                            new KeyValuePair<string, string>("Phone", model.Phone)
                                        };

                var content = new FormUrlEncodedContent(inputData);

                var response = await client.PostAsync(_webApiBaseUrl + "api/account/register", content);

                if (response.IsSuccessStatusCode)
                {

                    return RedirectToAction("Login");
                }

            }
         
            return View(model);
        }
        public ActionResult LogOut()
        {            
            Session.Clear();                  
            FormsAuthentication.SignOut();          
            return RedirectToAction("login", "login");
        }
    }
}