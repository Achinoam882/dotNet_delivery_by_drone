using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
namespace DalObject
{
    /// <summary>
    /// matods that use from the main 
    /// </summary>
    public class DalObject
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public DalObject()
        {
            DataSource.Initialize();
        }
        /// <summary>
        /// The function adds a drone to the list of drones.
        /// </summary>
        /// <param name="newDrone"></param>
        public void SetDrone(int id,string model, WeightCategories maxWeight,DroneStatuses status,double battery)
        {
           DataSource.droneList .Add(new Drone(){
                Id = id,
                Model = model,
                MaxWeight = maxWeight,
                Status = status,
                Battery = battery
            });

        }
        /// <summary>
        /// The function adds a station to the list of Basestations.
        /// </summary>
        /// <param name="newbaseStation"></param>
        public void SetBaseStation(int id, string name,double longitude,double latitude,int chargeSlots)
        {

            DataSource.baseStationsList.Add(new BaseStation()
            {
                Id = id,
                Name = name,
                Longitude = longitude,
                Latitude = latitude,
                ChargeSlots = chargeSlots

            });
        }
        /// <summary>
        /// The function adds a customer to the list of customers.
        /// </summary>
        /// <param name="newCustomer"></param>
        public void SetCustomer(int id, string name, double longitude, double latitude,string phoneNum) 
        {
            DataSource.customersList.Add(new Customer()
            {
                Id = id,
                Name = name,
                PhoneNumber = phoneNum,
                Latitude = latitude,
                Longitude = longitude
            });
        }
        /// <summary>
        /// The function adds a Parcel to the list of Parcels.
        /// </summary>
        /// <param name="newParcel"></param>
        /// <returns></returns>
        public void SetParcel(int senderId, int targetId, WeightCategories weight, Priorities priority)
        {
            DataSource.parcelsList.Add(new Parcel()
            {

                Id = DataSource.Config.CountIdPackage++,
                SenderId = senderId,
                TargetId = targetId,
                Weight = weight,
                Priority = priority,
                Requested = DateTime.Now
            }); 
        }
        /// <summary>
        /// The function assigns a package to the drone.
        /// </summary>
        /// <param name="ParcelId">Id of Parcel</param>
        /// <param name="droneId">Id of drone</param>
        public void SetParcelToDrone(int idParcel,int droneId)
        {
            //Update the package.
            int parcelIndex = DataSource.parcelsList.FindIndex(x => x.Id == idParcel);
            Parcel temp = DataSource.parcelsList[parcelIndex];
            temp.DroneId = droneId;
            temp.Scheduled = DateTime.Now;
            DataSource.parcelsList[parcelIndex] = temp;
            //update the drone
            int droneIndex = DataSource.droneList.FindIndex(x => x.Id == droneId);
            Drone droneTemp = DataSource.droneList[droneIndex];
            //droneTemp.Status = (DroneStatuses)2; //busy
            droneTemp.Status = DroneStatuses.busy;
            DataSource.droneList[droneIndex] = droneTemp;
        }
        /// <summary>
        /// picked up package by the drone
        /// </summary>
        /// <param name="idParcel"> Id of the parcel</param>
        public void PickUpParcel(int idParcel)
        {
            int parcelIndex = DataSource.parcelsList.FindIndex(x => x.Id == idParcel);
            Parcel temp = DataSource.parcelsList[parcelIndex];
            temp.PickUp = DateTime.Now;
            DataSource.parcelsList[parcelIndex] = temp;
        }
        /// <summary>
        /// delivery package to the customer.
        /// </summary>
        /// <param name="ParcelId">Id of Parcel</param>
        public void DeliverToCustomer(int idParcel)
        {
            int parcelIndex = DataSource.parcelsList.FindIndex(x => x.Id == idParcel);
            Parcel temp = DataSource.parcelsList[parcelIndex];
            temp.Delivered = DateTime.Now;
            DataSource.parcelsList[parcelIndex] = temp;
        }
        /// <summary>
        /// sending drone for charging at BaseStation.
        /// </summary>
        /// <param name="baseStationId">Id of baseStation</param>
        /// <param name="droneId">Id of drone</param>
        public void SendDroneToCharge(int droneId, int baseStationId)
        {
            //update drone
            int droneIndex = DataSource.droneList.FindIndex(x => x.Id == droneId);
            Drone droneTemp = DataSource.droneList[droneIndex];
            droneTemp.Status = (DroneStatuses)1; //inMaintenance
            DataSource.droneList[droneIndex] = droneTemp;
            //Add a droneCharge to the list
            DataSource.droneChargeList.Add(new DroneCharge { DroneId = droneId, StationId = baseStationId });
            //BaseStation update
            int BaseStationIndex = DataSource.baseStationsList.FindIndex(x => x.Id == droneId);
            BaseStation temp = DataSource.baseStationsList[BaseStationIndex];
            temp.ChargeSlots--;
            DataSource.baseStationsList[BaseStationIndex] = temp;
        }
        /// <summary>
        /// release drone from charging at BaseStation.
        /// </summary>
        /// <param name="droneId">Id of drone</param>
        public void ReleaseFromeCharging(int droneId)
        {
            //update drone to be a free
            int droneIndex = DataSource.droneList.FindIndex(x => x.Id == droneId);
            Drone droneTemp = DataSource.droneList[droneIndex];
            droneTemp.Status = (DroneStatuses)0; //inMaintenance
            DataSource.droneList[droneIndex] = droneTemp;
            // find the Station Id and remove from the droneChargeList.
            int droneChargeIndex = DataSource.droneChargeList.FindIndex(x => x.DroneId == droneId);
            int baseStationId = DataSource.droneChargeList[droneChargeIndex].StationId;
            DataSource.droneChargeList.Remove(DataSource.droneChargeList.Find(x => x.DroneId == droneId));
            //BaseStation update.
            int BaseStationIndex = DataSource.baseStationsList.FindIndex(x => x.Id == baseStationId);
            BaseStation temp = DataSource.baseStationsList[BaseStationIndex];
            temp.ChargeSlots++;
            DataSource.baseStationsList[BaseStationIndex] = temp;
        }
        /// <summary>
        /// The function returns the selected base station.
        /// </summary>
        /// <param name="ID">Id of a selected BaseStation </param>
        /// <returns> return empty ubjact if its not there</returns>
        public BaseStation GetBaseStation(int idBaseStation)
        {
            int BaseStationlIndex = DataSource.baseStationsList.FindIndex(x => x.Id == idBaseStation);
            return DataSource.baseStationsList[BaseStationlIndex];

        }
        /// <summary>
        /// The function returns the selected Drone.
        /// </summary>
        /// <param name="ID">Id of a selected Drone</param>
        /// <returns>return empty ubjact if its not there</returns>
        public Drone GetDrone(int idDrone)
        {
            int DroneIndex = DataSource.droneList.FindIndex(x => x.Id == idDrone);
            return DataSource.droneList[DroneIndex];
        }
        /// <summary>
        /// The function returns the selected Customer.
        /// </summary>
        /// <param name="ID">Id of a selected Customer</param>
        /// <returns>return empty ubjact if its not there</returns>
        public Customer  GetCustomer(int idCustomer)
        {
            int CustomerIndex = DataSource.customersList.FindIndex(x => x.Id == idCustomer);
            return DataSource.customersList[CustomerIndex];
        }
        /// <summary>
        /// The function returns the selected Parcel.
        /// </summary>
        /// <param name="ID">Id of a selected Parcel</param>
        /// <returns>return empty ubjact if its not there</returns>
        public Parcel GetParcel(int idParcel)
        {
            int ParcelIndex = DataSource.parcelsList.FindIndex(x => x.Id == idParcel);
            return DataSource.parcelsList[ParcelIndex];
        }
        /// <summary>
        /// The function returns an array of all base stations.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public List<BaseStation>  GetBaseStationList()
        {
            List<BaseStation> temp = new List<BaseStation>();
            for(int i=0;i<DataSource.baseStationsList.Count;i++)
            {
                //temp[i] = DataSource.baseStationsList[i];
                temp.Add(DataSource.baseStationsList[i]);
            }
            return temp;
        }
        /// <summary>
        /// The function returns an array of all Drone.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public List<Drone> GetDroneList()
        {
            List<Drone> temp = new List<Drone>();
            for (int i = 0; i < DataSource.droneList.Count; i++)
            {
                // temp[i] = DataSource.droneList[i];
                temp.Add(DataSource.droneList[i]);
            }
            return temp;
        }
        /// <summary>
        /// The function returns an array of all Customer.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public List<Customer> GetCustomerList()
        {
            List<Customer> temp = new List<Customer>();
            for (int i = 0; i < DataSource.customersList.Count; i++)

                //temp[i] = DataSource.customersList[i];
                temp.Add(DataSource.customersList[i]);

            return temp;
        }
        /// <summary>
        /// The function returns an array of all Parcel.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public List<Parcel> GetParcelList()
        {
            List<Parcel> temp = new List<Parcel>();
            for (int i = 0; i < DataSource.parcelsList.Count; i++)
                temp.Add(DataSource.parcelsList[i]);
            return temp;
        }
        /// <summary>
        /// The function returns an list of all packages not associated with the Drone.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public List<Parcel>  GetParcelWithoutDrone()
        {
            List<Parcel> temp = new List<Parcel>();
            for(int i=0;i<DataSource.parcelsList.Count;i++)
            {
                if (DataSource.parcelsList[i].DroneId == 0)
                    temp.Add(DataSource.parcelsList[i]);
            }
            return temp;
        }
        /// <summary>
        /// The function returns base stations with free charge positions.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public List<BaseStation> GetBaseStationFreeChargeSlots()
        {
            List<BaseStation> temp = new List<BaseStation>();
            for(int i=0; i<DataSource.baseStationsList.Count;i++)
            {
                if (DataSource.baseStationsList[i].ChargeSlots > 0)
                    // temp[i] = DataSource.baseStationsList[i];
                    temp.Add(DataSource.baseStationsList[i]);
            }
            return temp;
        }
    }
}
