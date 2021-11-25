using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
   public class BaseStation
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int FreeChargeSlots { get; set; }//מספר עמדות טעינה פנויות
        public Location BaseStationLocation { get; set; }
        public List<DroneCharging> DroneChargingList { get; set; }
        public override string ToString()
        {
            return string.Format("Id is:{0}\nName of Base Station is:{1}\nNumber of Charge Slots is:{2}\nlocation:{3}\n", Id, Name, FreeChargeSlots, BaseStationLocation)
                + "Drone in charge slots:" +String.Join(Environment.NewLine, DroneChargingList)+"\n";
        }



        
        

    }
}
