using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BlApi
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
      
         void SetParcel(Parcel newParcel);
        /// <summary>
        ///  The function updates the model of the drone.
        /// </summary>
        /// <param name="droneId">id of drone </param>
        /// <param name="newDroneModel">model drone</param>
        void ChangeDroneModel(int droneId, string newDroneModel);
        /// <summary>
        /// The function update the name and charge slots of station .
        /// </summary>
        void UpdateBaseStaison(int baseStationId, string baseStationName, string chargeSlots);
        /// <summary>
        /// The function update the name and phone number of customer .
        /// </summary>
        void UpdateCustomer(int customerId, string customerName, string phoneNumber);
        /// <summary>
        /// sending drone for charging at BaseStation.
        /// </summary>
        /// <param name="droneId">Id of drone</param>
        void SendDroneToCharge(int droneId);
        /// <summary>
        /// release drone from charging at BaseStation.
        /// </summary>
        /// <param name="droneId">Id of drone</param>
        void ReleaseFromCharging(int droneId);
        /// <summary>
        /// The function assign pacrel  to possible drone.
        /// </summary>
        /// <param name="droneId">id of drone</param>
        void AssignParcelToDrone(int droneId);
        /// <summary>
        /// The function return a list of base station.
        /// </summary>
        IEnumerable<BaseStationToList> GetBaseStationList(Predicate<BaseStationToList> predicate = null);
        /// <summary>
        /// The function return an exists  base station object.
        /// </summary>
        /// <param name="idForDisplayBaseStation">Id of station</param>
        BaseStation GetBaseStation(int idForDisplayBaseStation);
        /// <summary>
        /// The function return an exists  drone object.
        /// </summary>
        /// <param name="idForDisplayDrone">Id of drone</param>
        Drone GetDrone(int idForDisplayDrone);
        /// <summary>
        /// The function return a list of parcel.
        /// </summary>
        IEnumerable<ParcelToList> GetParcelList(Predicate<ParcelToList> predicate = null);
        /// <summary>
        /// The function return an exists parcel.
        /// </summary>
        Parcel GetParcel(int idForDisplayParcel);
        /// <summary>
        /// The function return an exists customer.
        /// </summary>
        /// <param name="idForDisplayCustomer">Id of customer</param>
        Customer GetCustomer(int idForDisplayCustomer);
        /// <summary>
        /// The function return a list of customers.
        /// </summary>
        IEnumerable<CustomerToList> GetCustomerList(Predicate<CustomerToList> predicate = null);
        /// <summary>
        /// The function return a list of drones.
        /// </summary>
        IEnumerable<DroneToList> GetDroneList(Predicate<DroneToList> predicate = null);
        /// <summary>
        /// picked up package by the drone.
        /// </summary>
        /// <param name="droneId">Id of drone</param>
        void PickUpParcelByDrone(int droneId);
        /// <summary>
        /// delivery package to the customer.
        /// </summary>
        /// <param name="droneId">Id of Parcel</param>
        void DroneDeliverParcel(int droneId);

        /// <summary>
        /// The function delete an exists parcel.
        /// </summary>
        public void DeleteParcel(int ParcelId);
        /// <summary>
        /// The function receives an ID number and returns the corresponding object
        /// </summary>
        /// <param name="id">id of user</param>
        /// <returns>user object</returns>

        public User GetUser(string id);
        /// <summary>
        /// The function returns all user
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetUsers();
        /// <summary>
        /// The function Receives an object user to Deletion
        /// </summary>
        /// <param name="user">user object for delete</param>
        public void DeleteUser(User user);
        /// The function receives a User for updating
        /// </summary>
        /// <param name="user">user object for update</param>
        public void UpdateUser(User user);
        /// <summary>
        /// The function Receives an object user and enters the database
        /// </summary>
        /// <param name="user">user object </param>
        public void AddUser(User user);
        /// <summary>
        /// The function start the simulator
        /// </summary>
        public void SimulatorOn(int droneId, Action ToReportProgress, Func<bool> IsTimeFinish);
    }
}
