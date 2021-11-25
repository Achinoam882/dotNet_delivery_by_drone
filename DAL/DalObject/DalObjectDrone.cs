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
        #region add drone
        /// The function adds a drone to the list of drones.
        /// </summary>
        /// <param name="newDrone"></param>
        public void SetDrone(Drone newDrone)
        {

            if (DataSource.DroneList.FindIndex(x => x.Id == newDrone.Id) != -1)
            {
                throw new AddExistingObjectException();
            }
            DataSource.DroneList.Add(newDrone);
        }
        #endregion add drone

        #region print drone
        /// <summary>
        /// The function returns the selected Drone.
        /// </summary>
        /// <param name="idForAllObjects">Id of a selected Drone</param>
        /// <returns>return empty ubjact if its not there</returns>
        public Drone GetDrone(int idForAllObjects)
        {
            if (!(DataSource.DroneList.Exists(x => x.Id == idForAllObjects)))
            {
                throw new NonExistingObjectException();
            }
            return DataSource.DroneList.Find(x => x.Id == idForAllObjects);

        }
        #endregion print drone

        #region to send a drone to charge
        /// <summary>
        /// sending drone for charging at BaseStation.
        /// </summary>
        /// <param name="baseStationId">Id of baseStation</param>
        /// <param name="droneId">Id of drone</param>
        public void SendDroneToCharge(int droneId, int baseStationId)
        {
            if (DataSource.DroneList.FindIndex(x => x.Id == droneId) == -1)
            {
                throw new NonExistingObjectException();
            }
            if (DataSource.BaseStationList.FindIndex(x => x.Id == baseStationId) == -1)
            {
                throw new NonExistingObjectException();
            }
            DataSource.DroneChargeList.Add(new DroneCharge()
            {
                StationId = baseStationId,
                DroneId = droneId
            });

        }

        #endregion to send a drone to charge

        #region to release a drone from charging
        /// <summary>
        /// release drone from charging at BaseStation.
        /// </summary>
        /// <param name="droneId">Id of drone</param>
        public void ReleaseFromCharging(int droneId)
        {
            //Drone update.
            if (!(DataSource.DroneList.Exists(x => x.Id == droneId)))
            {
                throw new NonExistingObjectException();
            }
            int droneIndex = DataSource.DroneList.FindIndex(x => x.Id == droneId);
            Drone droneTemp = DataSource.DroneList[droneIndex];
            //droneTemp.Status = (DroneStatuses)0;
            DataSource.DroneList[droneIndex] = droneTemp;
            //find the Station Id and remove from the droneChargeList.
            int chargeSlotIndex = DataSource.DroneChargeList.FindIndex(x => x.DroneId == droneId);
            int baseStationId = DataSource.DroneChargeList[chargeSlotIndex].StationId;
            DataSource.DroneChargeList.Remove(DataSource.DroneChargeList.Find(x => x.DroneId == droneId));
            //BaseStation update.
            if (!(DataSource.BaseStationList.Exists(x => x.Id == baseStationId)))
            {
                throw new NonExistingObjectException();
            }
            //int BaseStationIndex = DataSource.BaseStationList.FindIndex(x => x.Id == baseStationId);
            //BaseStation temp = DataSource.BaseStationList[BaseStationIndex];
            //temp.ChargeSlots++;
            //DataSource.BaseStationList[BaseStationIndex] = temp;
        }
        #endregion to release a drone from charging

        #region  drone list
        /// <summary>
        /// The function returns an array of all Drone.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public IEnumerable<Drone> GetDroneList()
        {
            return DataSource.DroneList.Take(DataSource.DroneList.Count).ToList();

        }
        #endregion  drone list

        #region to update a drone
        public void UpDateDrone(Drone newDrone)
        {
             DataSource.DroneList[DataSource.DroneList.FindIndex(x => x.Id == newDrone.Id)] = newDrone;
        }
        #endregion to update a drone

    }
}
