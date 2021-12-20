using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalApi
{
    public static class DalFactory
    {
        public static IDal GetDal(string dltype)
        {
            switch (dltype)
            {
                case "DalObject":
                    return DalObject.DalObject.Instance;
                //case "DalXml":
                //    return new DalXml;

            }
            throw new DalTypeCantBeShowen();


        }
    }
}
