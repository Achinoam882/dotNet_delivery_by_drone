using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IDAL.DO;
namespace DalObject
{
    public class DalObject
    {
        public DalObject()
        {
            DataSource.Initialize();
        }
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
      
        public void PickUpParcel(int parcelId)
        {
            int parcelIndex = DataSource.ParcelList.FindIndex(x => x.Id == parcelId);
            Parcel temp = DataSource.ParcelList[parcelIndex];
            temp.PickUp = DateTime.Now;
            DataSource.ParcelList[parcelIndex] = temp;
        }
        public void DeliverToCustomer(int parcelId)
        {
            int parcelIndex = DataSource.ParcelList.FindIndex(x => x.Id == parcelId);
            Parcel temp = DataSource.ParcelList[parcelIndex];
            temp.Delivered= DateTime.Now;
            DataSource.ParcelList[parcelIndex] = temp;
           // DataSource.ParcelList[parcelIndex].Delivered= DateTime.Now;
        }
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
        public void ReleaseFromCharging(int droneId)
        {
            int droneIndex = DataSource.droneList.FindIndex(x => x.Id == droneId);
            Drone droneTemp = DataSource.droneList[droneIndex];
            droneTemp.Status = (DroneStatuses)0;
            DataSource.droneList[droneIndex] = droneTemp;

            int chargeSlotIndex = DataSource.droneChargeList.FindIndex(x => x.DroneId == droneId);
            int baseStationId = DataSource.droneChargeList[chargeSlotIndex].StationId;
            DataSource.droneChargeList.Remove(DataSource.droneChargeList.Find(x => x.DroneId == droneId));

            int BaseStationIndex = DataSource.BaseStationList.FindIndex(x => x.Id == baseStationId);
            BaseStation temp = DataSource.BaseStationList[BaseStationIndex];
            temp.ChargeSlots++;
            DataSource.BaseStationList[BaseStationIndex] = temp;
        }
        public BaseStation GetBaseStation(int idForAllObjects)
        {
            int BaseStationIndex = DataSource.BaseStationList.FindIndex(x => x.Id == idForAllObjects);
            return DataSource.BaseStationList[BaseStationIndex];
            
        }
        public Drone GetDrone(int idForAllObjects)
        {
            int DroneIndex = DataSource.droneList.FindIndex(x => x.Id == idForAllObjects);
            return DataSource.droneList[DroneIndex];

        }
         public Customer  GetCustomer( int idForAllObjects)
        {
            int CustomerIndex = DataSource.CustomerList.FindIndex(x => x.Id == idForAllObjects);
            return DataSource.CustomerList[CustomerIndex];

        }
        public Parcel GetParcel(int idForAllObjects)
        {
            int ParcelIndex = DataSource.ParcelList.FindIndex(x => x.Id == idForAllObjects);
            return DataSource.ParcelList[ParcelIndex];
        }
       public List<BaseStation> GetBaseStationList()
        {
            List<BaseStation> temp = new List<BaseStation>();
            for(int i=0;i<DataSource.BaseStationList.Count();i++)
            {
                temp.Add(DataSource.BaseStationList[i]);
            }
            return temp;
        }
        public List <Drone> GetDroneList()
        {
            List<Drone> temp = new List<Drone>();
            for (int i = 0; i < DataSource.droneList.Count(); i++)
            {
                temp.Add(DataSource.droneList[i]);
            }
            return temp;

        }
        public List <Customer> GetCustomerList()
        {
            List<Customer> temp = new List<Customer>();
            for (int i = 0; i < DataSource.CustomerList.Count(); i++)
            {
                temp.Add(DataSource.CustomerList[i]);
            }
            return temp;

        }
        public List <Parcel> GetParcelList()
        {
            List<Parcel> temp = new List<Parcel>();
            for (int i = 0; i < DataSource.ParcelList.Count(); i++)
            {
                temp.Add(DataSource.ParcelList[i]);
            }
            return temp;
        }
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
