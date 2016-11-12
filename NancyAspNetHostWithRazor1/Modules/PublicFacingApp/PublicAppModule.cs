using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;

namespace NancyAspNetHosOwinIdentity.Modules.PublicFacingApp
{

    public class PublicAppModule : NancyModule
    {
        public PublicAppModule() : base("/public")
        {
            Get["/"] = paramaters => "public data";
        }
    }
}