using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace BO
{
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
