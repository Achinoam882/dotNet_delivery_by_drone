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

            public int ChargeSlots { get; set; }//מספר עמדות טעינה פנויות

            /*  public override string ToString()
              {
                  return string.Format("Id is:{0,-14}\tName of Base Station is:{1,-14}\tLongitude is:{2,-14}\t " +
                      "Latitude is:{3,-14}\tNumber of Charge Slots is:{4,-14}\t", Id, Name, Longitude, Latitude, ChargeSlots);
              }
            */
            public override string ToString()
            {
                return this.ToStringProperty();
            }

        }
    }
}
