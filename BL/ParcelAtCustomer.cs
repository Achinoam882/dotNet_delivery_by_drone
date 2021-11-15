using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelAtCustomer
    {
        public int Id { get; set; }

        public ParcelStatus Status{ get; set; }//נוצר, שויך, נאסף, סופק

        public WeightCategories Weight { get; set; }//light,medium,heavy

        public Priorities Priority { get; set; }//regular, fast, urgent
         public CustomerParcel OtherSide   { get; set; }
    }
}
