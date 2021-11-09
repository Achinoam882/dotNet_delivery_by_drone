using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
namespace IDAL
{
    interface IDal
    {
        /// The function adds a drone to the list of drones.
        /// </summary>
        /// <param name="newDrone"></param>
        public void SetDrone(int id, string model, WeightCategories maxWeight/*,DroneStatuses status*/, double battery);

        /// <summary>
        /// The function adds a station to the list of Basestations.
        /// </summary>
        /// <param name="newbaseStation"></param>
        public void SetBaseStation(int id, string name, double longitude, double latitude, int chargeSlots);

        /// <summary>
        /// The function adds a customer to the list of customers.
        /// </summary>
        /// <param name="newCustomer"></param>
        public void SetCustomer(int id, string name, double longitude, double latitude, string phoneNumber);

    
    /// <summary>
    /// The function adds a Parcel to the list of Parcels.
    /// </summary>
    /// <param name="newParcel"></param>
    /// <returns></returns>
    public int SetParcel(int senderId, int targetId, WeightCategories weightParcel, Priorities priority);

    
    /// <summary>
    /// The function assigns a package to the drone.
    /// </summary>
    /// <param name="parcelId">Id of Parcel</param>
    /// <param name="droneId">Id of drone</param>
    public void SetParcelToDrone(int parcelId, int droneId);


        /// <summary>
        /// picked up package by the drone.
        /// </summary>
        /// <param name="parcelId">Id of Parcel</param>
        public void PickUpParcel(int parcelId);

        /// <summary>
        /// delivery package to the customer.
        /// </summary>
        /// <param name="parcelId">Id of Parcel</param>
        public void DeliverToCustomer(int parcelId);

        /// <summary>
        /// sending drone for charging at BaseStation.
        /// </summary>
        /// <param name="baseStationId">Id of baseStation</param>
        /// <param name="droneId">Id of drone</param>
        public void SendDroneToCharge(int droneId, int baseStationId);

        /// <summary>
        /// release drone from charging at BaseStation.
        /// </summary>
        /// <param name="droneId">Id of drone</param>
        public void ReleaseFromCharging(int droneId);


        /// <summary>
        /// The function returns the selected base station.
        /// </summary>
        /// <param name="idForAllObjects">Id of a selected BaseStation </param>
        /// <returns> return empty ubjact if its not there</returns>
        public BaseStation GetBaseStation(int idForAllObjects);

        /// <summary>
        /// The function returns the selected Drone.
        /// </summary>
        /// <param name="idForAllObjects">Id of a selected Drone</param>
        /// <returns>return empty ubjact if its not there</returns>
        public Drone GetDrone(int idForAllObjects);

        /// <summary>
        /// The function returns the selected Customer.
        /// </summary>
        /// <param name="idForAllObjects">Id of a selected Customer</param>
        /// <returns>return empty ubjact if its not there</returns>
        public Customer GetCustomer(int idForAllObjects);


        /// <summary>
        /// The function returns the selected Parcel.
        /// </summary>
        /// <param name="idForAllObjects">Id of a selected Parcel</param>
        /// <returns>return empty ubjact if its not there</returns>
        public Parcel GetParcel(int idForAllObjects);

        /// <summary>
        /// The function returns an array of all base stations.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public IEnumerable<BaseStation> GetBaseStationList();

        /// <summary>
        /// The function returns an array of all Drone.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public IEnumerable<Drone> GetDroneList();

        /// <summary>
        /// The function returns an array of all Customer.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public IEnumerable<Customer> GetCustomerList();

        /// <summary>
        /// The function returns an array of all Parcel.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public IEnumerable<Parcel> GetParcelList();

        /// <summary>
        /// The function returns an array of all packages not associated with the Drone.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public IEnumerable<Parcel> GetFreeParcelList();

        /// <summary>
        /// The function returns base stations with free charge positions.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public IEnumerable<BaseStation> GetBaseStationFreeChargeSlots();




    }
}
