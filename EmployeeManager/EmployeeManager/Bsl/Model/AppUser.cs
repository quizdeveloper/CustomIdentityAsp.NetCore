using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace EmployeeManager.Bsl.Model
{
    public class AppUser : IIdentity
    {
        public virtual int Id { get; set; }
        private string _userName;
        private string _email;

        public virtual string UserName
        {
            set => _userName = value;
            get => _userName?.ToLower();
        }

        public virtual string Email
        {
            set => _email = value;
            get => _email?.ToLower();
        }

        public string PassWord { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Avatar { get; set; }

        public string AuthenticationType { get; set; }

        public bool IsAuthenticated { get; set; }

        public string Name { get; set; }
    }
}
