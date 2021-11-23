using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class CustomerToList
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }
        public int ParcelProvided { get; set; }
        public int Parcelsnet { get; set; }//not provided
        public int ParcelReceived { get; set; }
        public int ParcelOnTheWay { get; set; }//on the way to customer
        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}
