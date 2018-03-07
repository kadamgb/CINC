﻿using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using NQR.CINC.Entities.EntityFramework;
using NQR.CINC.Services.UserService;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NQR.CINC.Authorization.Providers
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            string symmetricKeyAsBase64 = string.Empty;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                context.SetError("invalid_clientId", "client_Id is not set");
                //return Task.FromResult<object>(null);
                return;
            }

            var audience = AudiencesStore.FindAudience(context.ClientId);

            if (audience == null)
            {
                context.SetError("invalid_clientId", string.Format("Invalid client_id '{0}'", context.ClientId));
                //return Task.FromResult<object>(null);
                return;
            }
            
            context.Validated();
            //return Task.FromResult<object>(null);
            return;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            //Dummy check here, you need to do your DB checks against membership system http://bit.ly/SPAAuthCode
            //if (context.UserName != context.Password)
            //{
            //    context.SetError("invalid_grant", "The user name or password is incorrect");
            //    //return;
            //    return Task.FromResult<object>(null);
            //}

            

            using (UserRepository _repo = new UserRepository())
            {                
                
                User user=  _repo.UnitOfWorkInstance.UserRepositoryInstance.GetUserByEmail(context.UserName);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
            }

            var identity = new ClaimsIdentity("JWT");

            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Manager"));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Supervisor"));

            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                         "audience", (context.ClientId == null) ? string.Empty : context.ClientId
                    }
                });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
            //return Task.FromResult<object>(null);
            return;
        }
    }
}