using EmployeeManager.Bsl.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Bsl.Identity
{
    public class PasswordHasherOverride<TUser> : PasswordHasher<TUser> where TUser : AppUser
    {
        public override PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            if (hashedPassword == null) { throw new ArgumentNullException(nameof(hashedPassword)); }
            if (providedPassword == null) { throw new ArgumentNullException(nameof(providedPassword)); }

            var hashedProvidedPassword = Utils.Utils.GetMd5x2(providedPassword);

            if (hashedPassword != hashedProvidedPassword)
            {
                return PasswordVerificationResult.Failed;
            }
            else
            {
                return PasswordVerificationResult.Success;
            }
        }
    }
}
