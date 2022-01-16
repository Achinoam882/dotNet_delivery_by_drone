using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
   public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

       public string PhoneNumber { get; set; }

        public Location CustomerLocation { get; set; }

        public IEnumerable<ParcelAtCustomer> ParcelFromCustomer { get; set; }
        public IEnumerable<ParcelAtCustomer> ParcelToCustomer { get; set; }
        public override string ToString()
        {
            return string.Format("Id is:{0}\nName of customer is:{1}\nPhone Number :{2}\nlocation:{3}\n", Id, Name, PhoneNumber, CustomerLocation)
                + "Parcel From Customer:\n" + String.Join(Environment.NewLine, ParcelFromCustomer) +
                "Parcel To Customer:\n" + String.Join(Environment.NewLine, ParcelToCustomer) + "\n";
        }
    }
}
