﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace DalApi
{
    /// <summary>
    /// Class for processing confing.xml file and getting from there
    /// information which is relevant for initialization of DalApi
    /// </summary>
    class DalConfig
    {
        internal static string DalName;
        internal static Dictionary<string, string> DalPachages;
        static DalConfig()
        {
            XElement dalConfig = XElement.Load(@"xml\dal-config.xml");
            DalName = dalConfig.Element("dal").Value;
            DalPachages = (from pkg in dalConfig.Element("dal-packages").Elements()
                           select pkg).ToDictionary(p => "" + p.Name, p => p.Value);
        }
    }
    /// <summary>
    /// Represents errors during DalApi initialization
    /// </summary>
    public class DalConfigException : Exception
    {
        public DalConfigException(string msg) : base(msg) { }
        public DalConfigException(string msg, Exception ex) : base(msg, ex) { }

    }


}

