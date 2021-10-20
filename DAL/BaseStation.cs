using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IDAL
{
    namespace DO
    {
       public struct BaseStation
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public double Longitude { get; set; }

            public double Latitude { get; set; }

            public int ChargeSlots { get; set; }

            public override string ToString()
            {
                return string.Format("Id is:{0}\t  Name of Base Station is:{1}\t   Longitude is:{2}\t " +
                    "Latitude is:{3}\t  Number of Charge Slots is:{4}\t", Id, Name, Longitude, Latitude, ChargeSlots);
            }


        }
    }
}
