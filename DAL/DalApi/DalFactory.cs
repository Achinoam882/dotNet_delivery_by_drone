using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using System.Threading.Tasks;
using DO;

namespace DalApi
{
    /// <summary>
    /// Static Factory class for creating Dal tier implementation object according to
    /// configuration in file config.xml
    /// </summary>
    public static class DalFactory
    {
        /// <summary>
        /// The function creates Dal tier implementation object according to Dal type
        /// as appears in "dal" element in the configuration file config.xml.<br/>
        /// The configuration file also includes element "dal-packages" with list
        /// of available packages (.dll files) per Dal type.<br/>
        /// Each Dal package must use "Dal" namespace and it must include internal access
        /// singleton class with the same name as package's name.<br/>
        /// The singleton class must include public static property called "Instance"
        /// which must contain the single instance of the class.
        /// </summary>
        /// <returns>Dal tier implementation object</returns>

        public static IDal GetDal()
        {
            // get dal implementation name from config.xml according to <data> element
            string dalType = DalConfig.DalName;
            // get dal implementation name from config.xml according to <data> element
            string dalPkg = DalConfig.DalPachages[dalType];
            if (dalPkg == null)
                throw new DalConfigException($"Package {dalType} is not found in packages list in dal-config.xml");
            try { Assembly.Load(dalPkg); }
            catch (Exception)
            { throw new DalConfigException("Failed to load the dal-config.xml file"); }
            // Get concrete Dal implementation's class metadata object
            // 1st element in the list inside the string is full class name:
            //    namespace = "Dal" or as specified in the "namespace" attribute in the config file,
            //    class name = package name or as specified in the "class" attribute in the config file
            //    the last requirement (class name = package name) is not mandatory in general - but this is the way it
            //    is configured per the implementation here, otherwise we'd need to add class name in addition to package
            //    name in the config.xml file - which is clearly a good option.
            //    NB: the class may not be public - it will still be found... Our approach that the implemntation class
            //        should hold "internal" access permission (which is actually the default access permission)
            // 2nd element is the package name = assembly name (as above)
            Type type = Type.GetType($"Dal.{dalPkg},{dalPkg}", true);
            if (type == null)
                throw new DalConfigException($"Class {dalPkg} was not found in the {dalPkg}.dll");
            IDal dal = (IDal)type.GetProperty("Instance",
                BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
            if(dal==null)
                throw new DalConfigException($"Class {dalPkg} is not a singleton or wrong propertry name for Instance");
            return dal;

        }
    }
}
