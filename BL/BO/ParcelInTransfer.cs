using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
   public class ParcelInTransfer
    {
        public int Id { get; set; }
        public bool ParcelStatus { get; set; }//if delivery true if pickup false
        public Priorities Priority { get; set; }//regular, fast, urgent
        public WeightCategories Weight { get; set; }//light,medium,heavy
        public CustomerParcel Sender { get; set; }
        public CustomerParcel Receiving { get; set; }
        public Location CollectionLocation { get; set; }//מיקום איסוף
        public Location DeliveryDestination { get; set; }//יעד אספקה
        public double TransportDistance { get; set; }
        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}
