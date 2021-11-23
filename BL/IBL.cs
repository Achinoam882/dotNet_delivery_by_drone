using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
   public interface IBL
    {
        /// <summary>
        /// The function adds a station to the list of Basestations.
        /// </summary>
        /// <param name="newbaseStation"></param>
        public void SetBaseStation(BaseStation newBaseStation);
        /// <summary>
        /// The function adds a drone to the list of drones.
        /// </summary>
        /// <param name="newDrone"></param>
        public void SetDrone(DroneToList newDrone, int firstChargingStation);


       
        /// <summary>
        /// The function adds a customer to the list of customers.
        /// </summary>
        /// <param name="newCustomer"></param>
        public void SetCustomer(Customer newCustomer);


        /// <summary>
        /// The function adds a Parcel to the list of Parcels.
        /// </summary>
        /// <param name="newParcel"></param>
      
        public void SetParcel(Parcel newParcel);
        public void ChangeDroneModel(int droneId, string newDroneModel);
        public void UpdateBaseStaison(int baseStationId, string baseStationName, string chargeSlots);
        public void UpdateCustomer(int customerId, string customerName, string phoneNumber);
        public void SendDroneToCharge(int droneId);
        public void ReleaseFromCharging(int droneId, DateTime time);
        public void AssignParcelToDrone(int droneId);
        public IEnumerable<BaseStationToList> GetBaseStationList(Predicate<BaseStationToList> predicate = null);
        public BaseStation GetBaseStation(int idForDisplayBaseStation);
        public Drone GetDrone(int idForDisplayDrone);
        public IEnumerable<ParcelToList> GetParcelList(Predicate<ParcelToList> predicate = null);
        public Parcel GetParcel(int idForDisplayParcel);
        public Customer GetCustomer(int idForDisplayCustomer);
        public IEnumerable<CustomerToList> GetCustomerList(Predicate<CustomerToList> predicate = null);
        public IEnumerable<DroneToList> GetDroneList(Predicate<DroneToList> predicate = null);
      //  private List<IDAL.DO.Parcel> ParcelHighestPriorityList(DroneToList drone);
       // private List<IDAL.DO.Parcel> FindingHeaviestList(List<IDAL.DO.Parcel> parcels, DroneToList myDrone);
       // private bool DistancePossibleForDrone(IDAL.DO.Parcel item, DroneToList drone);
       // private IDAL.DO.Parcel MinDistaceFromDroneToParcel(List<IDAL.DO.Parcel> heaviest, Location location);
        public void PickUpParcelByDrone(int droneId);
        public void DroneDeliverParcel(int droneId);


    }
}
