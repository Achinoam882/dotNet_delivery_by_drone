using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Security.Cryptography;

namespace DO
{
    /// <summary>
    /// Auxiliary static class
    /// Contains a function hashPassword for the user class
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// The function gets a string which is the password and the salt
        /// The hash is used as a unique value 
        /// </summary>
        /// <param name="passwordWithSalt"></param>
        /// <returns></returns>
        public static string hashPassword(string passwordWithSalt)
        {
            SHA512 shaM = new SHA512Managed();
            return Convert.ToBase64String(shaM.ComputeHash(Encoding.UTF8.GetBytes(passwordWithSalt)));
        }
    }
}
