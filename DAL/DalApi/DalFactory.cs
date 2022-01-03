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
            
            Type type = Type.GetType($"Dal.{dalPkg},{dalPkg}", true);
            if (type == null)
                throw new DalConfigException($"Class {dalPkg} was not found in the {dalPkg}.dll");
            IDal dal = (IDal)type.GetProperty("Instance",
                BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
            if(dal==null)
                throw new DalConfigException($"Class {dalPkg} is not a singleton or wrong propertry name for Instance");
            return dal;

        }
        //switch (dltype)
        //{
        //    case "DalObject":
        //        return Dal.DalObject.Instance;
        //        //return DalObject.DalXml.Instance;

        //        //case "DalXml":
        //        //    return new DalXml;

        //}
        //throw new DalTypeCantBeShowen();
    }
}
