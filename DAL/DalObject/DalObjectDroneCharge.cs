using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject :IDal
    {
      #region to Get Charge Slots List
        public IEnumerable<DroneCharge>GetChargeSlotsList(Predicate<DroneCharge> DroneChargepredicate = null)
        {
            return DataSource.DroneChargeList.FindAll(x => DroneChargepredicate == null ? true : DroneChargepredicate(x)).ToList();
        }
        #endregion Get Charge Slots List



        //#region to update a Drone Charge
        //public void UpDateDroneCharge(DroneCharge NewDroneCharge)
        //{
        //    DataSource.DroneList[DataSource.DroneList.FindIndex(x => x.Id == NewDroneCharge.Id)] = NewDroneCharge;
        //}
        //#endregion to update a Drone Charge

    }
}
