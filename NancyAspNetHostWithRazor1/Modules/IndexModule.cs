using System.Collections.Generic;
using Microsoft.Owin.Security;
using Nancy;
using Nancy.Owin;
using Nancy.Security;
using NancyAspNetHosOwinIdentity.Identity;

namespace NancyAspNetHosOwinIdentity.Modules
{
    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = parameters =>
            {

                var b = this.Context.GetOwinContext();
                
                var env = GetOwinEnvironmentValue<IDictionary<string, object>>(this.Context.Items, NancyMiddleware.RequestEnvironmentKey);

                if (env == null)
                {
                    return "Not running on owin host";
                }

                var requestMethod = GetOwinEnvironmentValue<string>(env, "owin.RequestMethod");
                var requestPath = GetOwinEnvironmentValue<string>(env, "owin.RequestPath");
                var owinVersion = GetOwinEnvironmentValue<string>(env, "owin.Version");
                var statusMessage = string.Format("You made a {0} request to {1} which runs on OWIN {2}.", requestMethod, requestPath, owinVersion);

                IAuthenticationManager authenticationManager = this.Context.GetAuthenticationManager();



                return View["index"];
            };
        }

        private static T GetOwinEnvironmentValue<T>(IDictionary<string, object> env, string name, T defaultValue = default(T))
        {
            object value;
            return env.TryGetValue(name, out value) && value is T ? (T)value : defaultValue;
        }
    }
}