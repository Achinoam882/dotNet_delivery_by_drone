using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
   public class Parcel
    {
        public int Id { get; set; }
        public CustomerParcel Sender { get; set; }//(לקוח בחבילה(השולח  
       public  CustomerParcel Receiving { get; set; }//(לקוח בחבילה(המקבל
        public WeightCategories Weight { get; set; }//light,medium,heavy
        public Priorities Priority { get; set; }//regular, fast, urgent
        public DroneParcel DroneAtParcel { get; set; }//רחפן בחבילה
        public DateTime? Scheduled { get; set; }//זמן שיוך החבילה לרחפן

        public DateTime? PickUp { get; set; }///  זמן איסוף חבילה מהשולח (מהרחפן

        public DateTime? Delivered { get; set; }//זמן הגעת החבילה למקבל,זמן אספקה

        public DateTime? Requested { get; set; } //זמן יצירת חבילה למשלוח
        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}
