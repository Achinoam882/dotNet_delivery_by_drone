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
        /// The function adds a drone to the list of drones.
        /// </summary>
        /// <param name="newDrone"></param>
        public void SetDrone(int id,string model,WeightCategories maxWeight,DroneStatuses status,double battery)
        {
            DataSource.droneList.Add( new Drone()
            {
                Id = id,
                Model = model,
                MaxWeight = maxWeight,
                Status = status,
                Battery = battery,
            });

        }
        /// <summary>
        /// The function adds a station to the list of Basestations.
        /// </summary>
        /// <param name="newbaseStation"></param>
        public void SetBaseStation(int id, string name, double longitude, double latitude, int chargeSlots)
        {
            DataSource.BaseStationList.Add(new BaseStation()
            {
                Id=id,
                Name=name,
                Longitude=longitude,
                Latitude=latitude,
                ChargeSlots=chargeSlots,
            });

        }
        /// <summary>
        /// The function adds a customer to the list of customers.
        /// </summary>
        /// <param name="newCustomer"></param>
        public void SetCustomer(int id, string name, double longitude, double latitude, string phoneNumber)
        {
            DataSource.CustomerList.Add(new Customer()
            {
                Id = id,
                Name = name,
                Longitude = longitude,
                Latitude=latitude,
                PhoneNumber=phoneNumber,
            }) ;
            
        }
        /// <summary>
        /// The function adds a Parcel to the list of Parcels.
        /// </summary>
        /// <param name="newParcel"></param>
        /// <returns></returns>
        public int SetParcel(int senderId, int targetId, WeightCategories weightParcel, Priorities priority)
        {
            DataSource.ParcelList.Add(new Parcel()
            {
                Id = DataSource.Config.IdParcel++,
                SenderId = senderId,
                TargetId = targetId,
                Weight = weightParcel,
                Priority = priority,
                Requested = DateTime.Now,
            }) ;
            return DataSource.Config.IdParcel;
       
        }
        /// <summary>
        /// The function assigns a package to the drone.
        /// </summary>
        /// <param name="parcelId">Id of Parcel</param>
        /// <param name="droneId">Id of drone</param>
        public void SetParcelToDrone(int parcelId, int droneId)
        {
            int parcelIndex = DataSource.ParcelList.FindIndex(x => x.Id == parcelId);
            Parcel temp = DataSource.ParcelList[parcelIndex];
            temp.DroneId = droneId;
            temp.Scheduled = DateTime.Now;
            DataSource.ParcelList[parcelIndex] = temp;

            int droneIndex = DataSource.droneList.FindIndex(x => x.Id == droneId);
            Drone droneTemp = DataSource.droneList[droneIndex];
            droneTemp.Status = (DroneStatuses)2;
            DataSource.droneList[droneIndex] = droneTemp;
        }
        /// <summary>
        /// picked up package by the drone.
        /// </summary>
        /// <param name="parcelId">Id of Parcel</param>
        public void PickUpParcel(int parcelId)
        {
            int parcelIndex = DataSource.ParcelList.FindIndex(x => x.Id == parcelId);
            Parcel temp = DataSource.ParcelList[parcelIndex];
            temp.PickUp = DateTime.Now;
            DataSource.ParcelList[parcelIndex] = temp;
        }
        /// <summary>
        /// delivery package to the customer.
        /// </summary>
        /// <param name="parcelId">Id of Parcel</param>
        public void DeliverToCustomer(int parcelId)
        {
            int parcelIndex = DataSource.ParcelList.FindIndex(x => x.Id == parcelId);
            Parcel temp = DataSource.ParcelList[parcelIndex];
            temp.Delivered= DateTime.Now;
            DataSource.ParcelList[parcelIndex] = temp;
           // DataSource.ParcelList[parcelIndex].Delivered= DateTime.Now;
        }
        /// <summary>
        /// sending drone for charging at BaseStation.
        /// </summary>
        /// <param name="baseStationId">Id of baseStation</param>
        /// <param name="droneId">Id of drone</param>
        public void SendDroneToCharge(int droneId,int baseStationId)
        {
            int droneIndex = DataSource.droneList.FindIndex(x => x.Id == droneId);
            Drone droneTemp = DataSource.droneList[droneIndex];
            droneTemp.Status = (DroneStatuses)1;
            DataSource.droneList[droneIndex] = droneTemp;

            int BaseStationIndex = DataSource.BaseStationList.FindIndex(x => x.Id == baseStationId);
            BaseStation temp = DataSource.BaseStationList[BaseStationIndex];
            temp.ChargeSlots--;
            DataSource.BaseStationList[BaseStationIndex] = temp;

            DataSource.droneChargeList.Add(new DroneCharge()
            {
                DroneId = droneId,
                StationId= baseStationId,
            }) ;            

        }
        /// <summary>
        /// release drone from charging at BaseStation.
        /// </summary>
        /// <param name="droneId">Id of drone</param>
        public void ReleaseFromCharging(int droneId)
        {
            //Drone update.
            int droneIndex = DataSource.droneList.FindIndex(x => x.Id == droneId);
            Drone droneTemp = DataSource.droneList[droneIndex];
            droneTemp.Status = (DroneStatuses)0;
            DataSource.droneList[droneIndex] = droneTemp;
            //find the Station Id and remove from the droneChargeList.
            int chargeSlotIndex = DataSource.droneChargeList.FindIndex(x => x.DroneId == droneId);
            int baseStationId = DataSource.droneChargeList[chargeSlotIndex].StationId;
            DataSource.droneChargeList.Remove(DataSource.droneChargeList.Find(x => x.DroneId == droneId));
            //BaseStation update.
            int BaseStationIndex = DataSource.BaseStationList.FindIndex(x => x.Id == baseStationId);
            BaseStation temp = DataSource.BaseStationList[BaseStationIndex];
            temp.ChargeSlots++;
            DataSource.BaseStationList[BaseStationIndex] = temp;
        }
        /// <summary>
        /// The function returns the selected base station.
        /// </summary>
        /// <param name="idForAllObjects">Id of a selected BaseStation </param>
        /// <returns> return empty ubjact if its not there</returns>
        public BaseStation GetBaseStation(int idForAllObjects)
        {
            int BaseStationIndex = DataSource.BaseStationList.FindIndex(x => x.Id == idForAllObjects);
            return DataSource.BaseStationList[BaseStationIndex];
            
        }
        /// <summary>
        /// The function returns the selected Drone.
        /// </summary>
        /// <param name="idForAllObjects">Id of a selected Drone</param>
        /// <returns>return empty ubjact if its not there</returns>
        public Drone GetDrone(int idForAllObjects)
        {
            int DroneIndex = DataSource.droneList.FindIndex(x => x.Id == idForAllObjects);
            return DataSource.droneList[DroneIndex];

        }

        /// <summary>
        /// The function returns the selected Customer.
        /// </summary>
        /// <param name="idForAllObjects">Id of a selected Customer</param>
        /// <returns>return empty ubjact if its not there</returns>
        public Customer  GetCustomer( int idForAllObjects)
        {
            int CustomerIndex = DataSource.CustomerList.FindIndex(x => x.Id == idForAllObjects);
            return DataSource.CustomerList[CustomerIndex];

        }
        /// <summary>
        /// The function returns the selected Parcel.
        /// </summary>
        /// <param name="idForAllObjects">Id of a selected Parcel</param>
        /// <returns>return empty ubjact if its not there</returns>
        public Parcel GetParcel(int idForAllObjects)
        {
            int ParcelIndex = DataSource.ParcelList.FindIndex(x => x.Id == idForAllObjects);
            return DataSource.ParcelList[ParcelIndex];
        }
        /// <summary>
        /// The function returns an array of all base stations.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public List<BaseStation> GetBaseStationList()
        {
            List<BaseStation> temp = new List<BaseStation>();
            for(int i=0;i<DataSource.BaseStationList.Count();i++)
            {
                temp.Add(DataSource.BaseStationList[i]);
            }
            return temp;
        }
        /// <summary>
        /// The function returns an array of all Drone.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public List <Drone> GetDroneList()
        {
            List<Drone> temp = new List<Drone>();
            for (int i = 0; i < DataSource.droneList.Count(); i++)
            {
                temp.Add(DataSource.droneList[i]);
            }
            return temp;

        }
        /// <summary>
        /// The function returns an array of all Customer.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public List <Customer> GetCustomerList()
        {
            List<Customer> temp = new List<Customer>();
            for (int i = 0; i < DataSource.CustomerList.Count(); i++)
            {
                temp.Add(DataSource.CustomerList[i]);
            }
            return temp;

        }
        /// <summary>
        /// The function returns an array of all Parcel.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public List <Parcel> GetParcelList()
        {
            List<Parcel> temp = new List<Parcel>();
            for (int i = 0; i < DataSource.ParcelList.Count(); i++)
            {
                temp.Add(DataSource.ParcelList[i]);
            }
            return temp;
        }
        /// <summary>
        /// The function returns an array of all packages not associated with the Drone.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public List <Parcel> GetFreeParcelList()
        {
            List<Parcel> temp = new List<Parcel>();
            for (int i = 0; i < DataSource.ParcelList.Count(); i++)
            {
                if(DataSource.ParcelList[i].DroneId==0)
                {
                    temp.Add(DataSource.ParcelList[i]);
                }
            }
            return temp;

        }
        /// <summary>
        /// The function returns base stations with free charge positions.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public List <BaseStation> GetBaseStationFreeChargeSlots()
        {
            List<BaseStation> temp = new List<BaseStation>();
            for (int i = 0; i < DataSource.BaseStationList.Count(); i++)
            {
                if(DataSource.BaseStationList[i].ChargeSlots>0)
                {
                    temp.Add(DataSource.BaseStationList[i]);
                }
            }

            return temp;
        }

    }
}
