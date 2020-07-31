using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Helper
{
    public class MyCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            //context.RedirectUri = "/login?returnUrl=" + context.HttpContext.Request.Path.Value;
            context.RedirectUri = "/";
            return base.RedirectToLogin(context);
        }
        
    }
}
