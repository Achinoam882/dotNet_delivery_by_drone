using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

    namespace BO
    {
        public static class ToStringObj
        {
            public static string ToStringProperty<T>(this T t)
            {
                string str = "";
                foreach (PropertyInfo item in t.GetType().GetProperties())
                    str += "\n" + item.Name + ": " + item.GetValue(t, null);
                return str;
            }
        }
    }   

