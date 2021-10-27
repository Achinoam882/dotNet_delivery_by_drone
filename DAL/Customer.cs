using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IDAL
{
    namespace DO

    {
        public struct Customer
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string PhoneNumber { get; set; }

            public double Longitude { get; set; }

            public double Latitude { get; set; }
            public override string ToString()
            {
                return string.Format("Id is:{0,-14}\tName of the customer is:{1,-14}\tPhoneNumber is:{2,-14}\tLongitude is:{3,-14}\tLatitude is:{4,-14}\t", Id,Name,PhoneNumber,Longitude,Latitude);
            }

        }
    }
}
