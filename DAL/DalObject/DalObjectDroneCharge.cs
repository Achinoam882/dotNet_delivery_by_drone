using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;

namespace DalObject
{
    partial class DalObject :IDal
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

        #region print drone  charge
        /// <summary>
        /// The function returns the selected Drone charge.
        /// </summary>
        /// <param name="idForAllObjects">Id of a selected Drone</param>
        /// <returns>return empty ubjact if its not there</returns>
        public DroneCharge GetDroneCharge(int idForAllObjects)
        {
            if (!(DataSource.DroneChargeList.Exists(x => x.DroneId == idForAllObjects)))
            {
                throw new NonExistingObjectException();
            }
            return DataSource.DroneChargeList.Find(x => x.DroneId == idForAllObjects);

        }
        #endregion print drone charge

    }
}
