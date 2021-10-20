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
                return string.Format("Id is:{0}\t  Name of the customer is:{1}\t   PhoneNumber is:{2}\t " +
                    " Longitude is:{3}\t  Latitude is:{4}\t", Id, Name, PhoneNumber, Longitude, Latitude);
            }

        }
    }
}
