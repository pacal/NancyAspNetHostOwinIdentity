using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NancyAspNetHosOwinIdentity.Modules.Identity.Models
{
    public class vm_LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RembmerMe { get; set; }
    }

    public class vm_RegisterViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }


}
