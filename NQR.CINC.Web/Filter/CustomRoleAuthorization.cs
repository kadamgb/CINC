using System;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;

namespace NQR.CINC.Web.Filters
{
    ///  Redirect user to friendly error page if unauthorized. Capture controller and action where error occured
    /// <summary>
    /// Custom Authorization Filter for MVC Controllers. 
    /// Security trimming and visibility of the nodes will be controlled using roles and permissions assigned to the current logged in user.
    /// The security trimming will be done using a custom authorization filter that inherits from AuthorizationFilter
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class CustomRoleAuthorizationAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        private string controller;
        private string action;

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //Retrive controller and action
            controller = filterContext.RouteData.Values.ContainsKey("controller") ? filterContext.RouteData.Values["controller"].ToString() : String.Empty;
            action = filterContext.RouteData.Values.ContainsKey("action") ? filterContext.RouteData.Values["action"].ToString() : String.Empty;

            base.OnAuthorization(filterContext);
        }

        /// Added null checks
        /// <summary>
        /// Core Authorization logic
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorized = false;

            if (httpContext == null)
            {                
                return authorized;
            }

            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null && !authTicket.Expired)
                {
                    var roles = authTicket.UserData.Split(',');
                    HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new FormsIdentity(authTicket), roles);
                }
                authorized = true;
            }
            else
            {
                authorized=false;
            }
            return authorized;
            //check roles
            //if (HttpContext.Current != null && HttpContext.Current.Items.Contains("Roles"))
            //{
            //    //Get all application level roles
            //    applicationRoles = HttpContext.Current.Items["Roles"] as string[];

            //    //if user does not belong to any roles in SharePoint
            //    if (applicationRoles == null || applicationRoles.GetLength(0) <= 0)
            //    {
            //        //httpContext.Items.Add(IS_AUTHORIZED, authorized);
            //       // EspHelpers.LogUnAuthorizedAccess("User does not belong to any role in SharePoint - " + controller + "." + action);
            //        return authorized;
            //    }
            //    else
            //    {
            //        //check if roles exists in web.config
            //        if (ConfigurationManager.AppSettings.AllKeys.Contains(Roles))
            //        {
            //            //allowable roles will be passed as comma seperated values. convert to string[]
            //            allowableRoles = ConfigurationManager.AppSettings[Roles].ToString().Split(',');

            //            if (applicationRoles != null)
            //            {
            //                //if user belongs to atleast one SharePoint role, check if user is a member of a role that is allowed
            //                foreach (string role in applicationRoles)
            //                {
            //                    string trimmedRole = role.Trim();

            //                    // if(trimmedRole contains _ then remove guid before _ and compaire it with allowble roles)
            //                    if (trimmedRole.Contains("_"))
            //                    {
            //                        //Input: e1878836-7950-48d3-b331-16705a822877_Experts or  e1878836-7950-48d3-b331-16705a822877_LeadTO
            //                        //Output: USA _Experts		or _LeadTO

            //                        //Remove characters before character “_”
            //                        trimmedRole = trimmedRole.Substring(trimmedRole.IndexOf('_'));
            //                    }

            //                    if (allowableRoles.Contains(trimmedRole))
            //                    {
            //                        authorized = true;
            //                        //httpContext.Items.Add(IS_AUTHORIZED, authorized);

            //                        return authorized;
            //                    }
            //                }
            //            }

            //            //if we have reached this point, then user is not a member of allowable roles
            //            //EspHelpers.LogUnAuthorizedAccess("User is not a member of any allowable role - " + controller + "." + action);
            //            return authorized;
            //        }
            //        else
            //        {
            //            // If roles does not exist in web.config, then disallow user
            //            authorized = false;
            //           // EspHelpers.LogUnAuthorizedAccess("Role specified (" + Roles + ") does not exist in web.config - " + controller + "." + action);
            //            return authorized;
            //        }
            //    }
            //}
            //else
            //{
            //    //not authorized      
            //    //httpContext.Items.Add(IS_AUTHORIZED, authorized);
            //   // EspHelpers.LogUnAuthorizedAccess("ApplicationIdentity not initialized correctly with Roles - " + controller + "." + action);
            //    return authorized;
            //}
        }

        // Redirect to nucleus signin url Redirect to the signin url logic changed
        /// <summary>
        /// Handles un-authorized requests by redirecting them to Error/unauthorized view
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);

            try
            {
                //string signInUrl = WebConfigurationManager.AppSettings["signIn"];
                string target = filterContext.RequestContext.HttpContext.Request.Url.AbsoluteUri;
               // string signInUrl = Helpers.ConstructSignInUrl(target);
                string signInUrl = "/login/login";
                //Redirect user to signin in page
                filterContext.Result = new RedirectResult(signInUrl);
            }
            catch (Exception ex)
            {
                //for any exception encountered while trying to construct target url, simply redirect the user to default page             
                string signInUrl = "/login/login";//WebConfigurationManager.AppSettings["signIn"];
                filterContext.Result = new RedirectResult(signInUrl);
            }
        }

    }
}