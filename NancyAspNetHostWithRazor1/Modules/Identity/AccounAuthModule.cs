using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;
using Nancy.Security;
using NancyAspNetHosOwinIdentity.Modules.Identity.Models;


namespace NancyAspNetHosOwinIdentity.Modules.Identity
{

    public class AccounAuthModule : NancyModule
    {
        public AccounAuthModule() : base("/account")
        {
            //Get["/login"] = paramaters => "Get Login page";
            Post["/login"] = Login;
            Post["/login/forgot"] = o => "email-> send reset";
            Post["/login/reset"] = o => "Emailguid, username, pw from the reset page";
            Delete["/Logout"] = o => "Signout user";

            Post["/register"] = o => "register view model";
        }

        private object Login(dynamic paramters)
        {
            var response = new Response();
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            var authenticationManager = this.Context.GetAuthenticationManager();

            var reg = this.Bind<vm_LoginViewModel>();
            var foundUser = userManager.FindByName(reg.UserName);

            bool goodpw = false;
            if (foundUser != null)
            {
                goodpw = userManager.CheckPassword(foundUser, reg.Password);
            }

            if (goodpw)
            {
                var createdUser = userManager.CreateIdentity(foundUser, DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false, RedirectUri = "/Secure" }, createdUser);
                response.StatusCode = HttpStatusCode.Accepted;
            }
            else
            {
                response.StatusCode = HttpStatusCode.Forbidden;
                response = this.Response.AsJson(new {message = "Invalid UserName or Password.", HttpStatusCode.Unauthorized});
            }
            

            return response;
        }

    }

    public class UsersManagementModule : NancyModule
    {
        public UsersManagementModule() : base("/users")
        {

        }

    }
}