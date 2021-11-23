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
        public IEnumerable<DroneCharging> DroneChargingList { get; set; }
        public override string ToString()
        {
            return this.ToStringProperty();
        }

    }
}
