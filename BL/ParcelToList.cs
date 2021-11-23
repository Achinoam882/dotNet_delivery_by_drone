using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelToList
    {
        public int Id { get; set; }
        public string Sender { get; set; }//שם לקוח שולח
        public string Receiving { get; set; }//שם לקוח מקבל
        public WeightCategories Weight { get; set; }//light,medium,heavy
        public Priorities Priority { get; set; }//regular, fast, urgent
        public ParcelStatus Status { get; set; }//נוצר, שויך, נאסף, סופק
        public override string ToString()
        {
            return this.ToStringProperty();
        }

    }
}
