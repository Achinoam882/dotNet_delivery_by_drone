using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public static class DeepCopyUtilities
    {
        /// <summary>
        /// Deep copying
        /// The function contains a loop that passes over all the Properties in variable S and copies the Properties to T
        /// In case T does not have the Propertie it continues in a loop
        /// in case it does have it it copies the Properties by setValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static void CopyPropertiesTo<T, S>(this S from, T to)
        {
            foreach (PropertyInfo propTo in to.GetType().GetProperties())
            {
                PropertyInfo propFrom = typeof(S).GetProperty(propTo.Name);
                if (propFrom == null)
                    continue;
                var value = propFrom.GetValue(from, null);
                if (value is ValueType || value is string)
                    propTo.SetValue(to, value);
            }
        }
        public static object CopyPropertiesToNew<S>(this S from, Type type)
        {
            ///Activator
            ///Creates an instance of the specified type using the constructor that best matches the specified parameters.
            ///Contains methods to create types of objects locally or remotely
            ///or obtain references to existing remote objects. This class cannot be inherited.
            object to = Activator.CreateInstance(type); // new object of Type
            from.CopyPropertiesTo(to);
            return to;
        }
       
     

    }
}
