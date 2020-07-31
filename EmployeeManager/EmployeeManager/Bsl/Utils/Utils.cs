using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Bsl.Utils
{
    public class Utils
    {
        public static string GetMd5x2(string str)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            bytes = provider.ComputeHash(bytes);
            StringBuilder builder = new StringBuilder();
            foreach (byte num in bytes)
            {
                builder.Append(num.ToString("x2").ToLower());
            }
            return builder.ToString();
        }

    }
}
