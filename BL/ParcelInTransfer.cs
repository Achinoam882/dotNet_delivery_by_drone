using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
   public class ParcelInTransfer
    {
        public int Id { get; set; }
        public bool ParcelStatus { get; set; }
        public Priorities Priority { get; set; }//regular, fast, urgent
        public WeightCategories Weight { get; set; }//light,medium,heavy
        public CustomerParcel Sender { get; set; }
        public CustomerParcel Receiving { get; set; }
        public Location CollectionLocation { get; set; }//מיקום איסוף
        public Location DeliveryDestination { get; set; }//יעד אספקה
        public int TransportDistance { get; set; }

    }
}
