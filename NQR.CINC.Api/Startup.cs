using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using NQR.CINC.DependencyInjection.DependencyResolution;
using Owin;
using StructureMap;
using System.Threading.Tasks;
using System.Web.Http;
//the “assembly” attribute which states which class to fire on start-up. 
[assembly: OwinStartup(typeof(NQR.CINC.Api.Startup))]
namespace NQR.CINC.Api
{
   
    public class Startup
    {
        private string _oathBaseURL = System.Configuration.ConfigurationManager.AppSettings.Get("OathBaseUrl");  // Othorization server url
        /// <summary>
        /// The “Configuration” method accepts parameter of type “IAppBuilder” this parameter will be supplied by the host at run-time.
        /// This “app” parameter is an interface which will be used to compose the application for our Owin server.
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {

            HttpConfiguration config = new HttpConfiguration();

            // start: below container added by Ganesh kadam
            IContainer container = IoC.Initialize();
            config.DependencyResolver = new StructureMapWebApiDependencyResolver(container);
            // end: below container added by Ganesh kadam

            config.MapHttpAttributeRoutes();

            ConfigureOAuth(app);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.UseWebApi(config);
        }
        public void ConfigureOAuth(IAppBuilder app)
        {
            // TODO : below issuer,audience and secrete key keep in web.config   
            var _issuer = "http://localhost:65010/";
            var audience = "CINCMVCApplication";
            var secret = TextEncodings.Base64Url.Decode("IxrAjDoa2FqElO7IhrSrUJELhUckePEPVpaePlS_Xaw");
            // below sceret key is generated from 'Ganeshkadam100' string and with help of website http://hash.online-convert.com/sha1-generator
            //var secret = TextEncodings.Base64Url.Decode("Sm3KAUMNQLTTZW4xMYIR3RrgAU0=");
            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audience },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(_issuer, secret)
                    },
                    Provider = new OAuthBearerAuthenticationProvider
                    {
                        OnValidateIdentity = context =>
                        {
                            context.Ticket.Identity.AddClaim(new System.Security.Claims.Claim("newCustomClaim", "newValue"));
                            return Task.FromResult<object>(null);
                        }
                    }
                });

        }
    }
}