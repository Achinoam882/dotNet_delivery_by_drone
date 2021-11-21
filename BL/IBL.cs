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


    }
}
