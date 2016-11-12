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
using Nancy.Routing;
using Nancy.Security;
using NancyAspNetHosOwinIdentity.Identity;
using NancyAspNetHosOwinIdentity.Modules.Identity.Models;


namespace NancyAspNetHosOwinIdentity.Modules.Identity
{

    public class AccounAuthModule : NancyModule
    {
        public AccounAuthModule() : base("/account")
        {     
            Post["/login"] = Login;
            Post["/login/forgot"] = o => "email-> send reset";
            Post["/login/reset"] = o => "Emailguid, username, pw from the reset page";
            Delete["/logout"] = Logout;

            Post["/register"] = Register;
        }

        private object Login(dynamic paramters)
        {
            var response = new Response();
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            var authenticationManager = this.Context.GetAuthenticationManager();

            var lvm = this.Bind<vm_LoginViewModel>();
            var foundUser = userManager.FindByName(lvm.UserName);

            bool goodpw = false;
            if (foundUser != null)
            {
                goodpw = userManager.CheckPassword(foundUser, lvm.Password);
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
                response = this.Response.AsJson(new { httpStatusCode = HttpStatusCode.Unauthorized,
                    message = "Invalid UserName or Password.", HttpStatusCode.Unauthorized});
            }
            

            return response;
        }

        private object Register(dynamic paramters)
        {
            var reg = this.Bind<vm_RegisterViewModel>();
            Response response;
            if (reg != null)
            {
                var userStore = new UserStore<IdentityUser>();
                var userManager = new UserManager<IdentityUser>(userStore);
                var authenticationManager = this.Context.GetAuthenticationManager();

                var identUser = new ApplicationUser()
                {
                    UserName = reg.Email,
                    Id = Guid.NewGuid().ToString(),
                    Email = reg.Email,
                    PasswordHash = userManager.PasswordHasher.HashPassword(reg.Password),                                        
                };

                var createTask = userManager.Create(identUser);
                if (createTask.Succeeded)
                {
                    var createdUser = userManager.CreateIdentity(identUser, DefaultAuthenticationTypes.ApplicationCookie);
                    //http://stackoverflow.com/questions/26373614/asp-net-identity-authenticationmanager-vs-signinmanager-and-cookie-expiration
                    authenticationManager.SignIn(new AuthenticationProperties() {IsPersistent = false,}, createdUser);
                    response = this.Response.AsJson(
                            new
                            {
                                httpStatusCode = HttpStatusCode.Created,
                                message = $"User: {identUser.Email} has been successfully registered."
                            }, HttpStatusCode.Created);


                }
                else
                {
                    // TODO set messate for creteTask.message
                    response = this.Response.AsJson(
                            new
                            {
                                httpStatusCode = HttpStatusCode.InternalServerError,
                                message = "There was an error creating the user."
                            }, HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                response = this.Response.AsJson(
                   new
                   {
                       httpStatusCode = HttpStatusCode.NotAcceptable,
                       message = "Could not bind to viewmodel."
                   }, HttpStatusCode.NotAcceptable);

            }

            return response;
        }

        private object Logout(dynamic paramters)
        {
            var authenticationManager = this.Context.GetAuthenticationManager();
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie,
                                  DefaultAuthenticationTypes.ExternalCookie);

            return this.Response.AsJson(new
            {
                httpStatusCode = HttpStatusCode.OK,
                message = "You have ben successfully logged out."
            }, HttpStatusCode.OK);
        }

    }

    public class UsersManagementModule : NancyModule
    {
        public UsersManagementModule() : base("/users")
        {

        }

    }
}