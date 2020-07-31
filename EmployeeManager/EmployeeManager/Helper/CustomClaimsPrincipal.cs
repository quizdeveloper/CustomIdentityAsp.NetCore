using EmployeeManager.Bsl.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmployeeManager.Helper
{
    public class CustomClaimsPrincipal : UserClaimsPrincipalFactory<AppUser>
    {
        private readonly UserManager<AppUser> _userManger;
        public CustomClaimsPrincipal(UserManager<AppUser> userManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
            _userManger = userManager;
        }

        public override async Task<ClaimsPrincipal> CreateAsync(AppUser user)
        {
            try
            {
                var principal = await base.CreateAsync(user);
                ((ClaimsIdentity)principal.Identity).AddClaims(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Email", user.Email),
                    new Claim("DisplayName", user.FullName ?? user.Email),
                    new Claim("Avatar", user.Avatar ?? string.Empty),
                });
                return principal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
