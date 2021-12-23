using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;



namespace DalApi
{
  public  interface IDal
    {
       
        /// The function adds a drone to the list of drones.
        /// </summary>
        /// <param name="newDrone"></param>
       void SetDrone(Drone newDrone);

        /// <summary>
        /// The function adds a station to the list of Basestations.
        /// </summary>
        /// <param name="newbaseStation"></param>
        void SetBaseStation(BaseStation newBaseStation);

        /// <summary>
        /// The function adds a customer to the list of customers.
        /// </summary>
        /// <param name="newCustomer"></param>
        void SetCustomer(Customer newCustomer);

    
    /// <summary>
    /// The function adds a Parcel to the list of Parcels.
    /// </summary>
    /// <param name="newParcel"></param>
    /// <returns></returns>
    int SetParcel(Parcel newParcel);

    
    /// <summary>
    /// The function assigns a package to the drone.
    /// </summary>
    /// <param name="parcelId">Id of Parcel</param>
    /// <param name="droneId">Id of drone</param>
    void SetParcelToDrone(int parcelId, int droneId);


        /// <summary>
        /// picked up package by the drone.
        /// </summary>
        /// <param name="parcelId">Id of Parcel</param>
        void PickUpParcel(int parcelId);

        /// <summary>
        /// delivery package to the customer.
        /// </summary>
        /// <param name="parcelId">Id of Parcel</param>
       void DeliverToCustomer(int parcelId);

        /// <summary>
        /// sending drone for charging at BaseStation.
        /// </summary>
        /// <param name="baseStationId">Id of baseStation</param>
        /// <param name="droneId">Id of drone</param>
         void SendDroneToCharge(int droneId, int baseStationId);

        /// <summary>
        /// release drone from charging at BaseStation.
        /// </summary>
        /// <param name="droneId">Id of drone</param>
        void ReleaseFromCharging(int droneId);


        /// <summary>
        /// The function returns the selected base station.
        /// </summary>
        /// <param name="idForAllObjects">Id of a selected BaseStation </param>
        /// <returns> return empty ubjact if its not there</returns>
         BaseStation GetBaseStation(int idForAllObjects);

        /// <summary>
        /// The function returns the selected Drone.
        /// </summary>
        /// <param name="idForAllObjects">Id of a selected Drone</param>
        /// <returns>return empty ubjact if its not there</returns>
         Drone GetDrone(int idForAllObjects);

        /// <summary>
        /// The function returns the selected Customer.
        /// </summary>
        /// <param name="idForAllObjects">Id of a selected Customer</param>
        /// <returns>return empty ubjact if its not there</returns>
        Customer GetCustomer(int idForAllObjects);


        /// <summary>
        /// The function returns the selected Parcel.
        /// </summary>
        /// <param name="idForAllObjects">Id of a selected Parcel</param>
        /// <returns>return empty ubjact if its not there</returns>
      Parcel GetParcel(int idForAllObjects);

        /// <summary>
        /// The function returns an array of all base stations.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        IEnumerable<BaseStation> GetBaseStationList(Predicate<BaseStation> BaseStationpredicate = null);

        /// <summary>
        /// The function returns an array of all Drone.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        IEnumerable<Drone> GetDroneList();

        /// <summary>
        /// The function returns an array of all Customer.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        IEnumerable<Customer> GetCustomerList();

        /// <summary>
        /// The function returns an array of all Parcel.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
         IEnumerable<Parcel> GetParcelList(Predicate<Parcel> parcelpredicate = null);

        /// <summary>
        /// The function returns an array of all packages not associated with the Drone.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        IEnumerable<Parcel> GetFreeParcelList();

        /// <summary>
        /// The function returns base stations with free charge positions.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        IEnumerable<BaseStation> GetBaseStationFreeChargeSlots();


        public IEnumerable<DroneCharge> GetChargeSlotsList(Predicate<DroneCharge> DroneChargepredicate = null);

        public void UpDateBaseStation(BaseStation newBaseStation);
        public  double[] RequestPowerbyDrone();
        public void UpDateDrone(Drone newDrone);
        public void UpDateCustomer(Customer newCustomer);
        public void UpDateParcel(Parcel newParcel);
        public void LessChargeSlots(int droneId);
        public void MoreChargeSlots(int baseStationId);
        public DroneCharge GetDroneCharge(int idForAllObjects);
        public void DeleteParcel(int parcelId);



    }
}
