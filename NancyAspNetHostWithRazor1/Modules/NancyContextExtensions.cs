using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Nancy;
using Nancy.Owin;

namespace NancyAspNetHosOwinIdentity.Modules
{
    public static class NancyContextExtensions
    {
        // wrapper extension to basically mirror Katana's
        public static IOwinContext GetOwinContext(this NancyContext context)
        {
            object requestEnvironment;
            context.Items.TryGetValue(NancyMiddleware.RequestEnvironmentKey, out requestEnvironment);
            var environment = requestEnvironment as IDictionary<string, object>;
            return environment != null ? new OwinContext(environment) : null;
        }


    }
}