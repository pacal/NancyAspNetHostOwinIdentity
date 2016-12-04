using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Nancy;
using Nancy.Security;

namespace NancyAspNetHosOwinIdentity.Modules.SecureApp
{
    public class SecureAppModule : NancyModule
    {
        public SecureAppModule() : base("/secure")
        {
            this.RequiresMSOwinAuthentication();
            Get["/"] = o => "This is a secured area which requires login";

        }
    }
}