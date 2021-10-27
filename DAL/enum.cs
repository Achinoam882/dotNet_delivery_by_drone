using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        /// <summary> enum for Priorities of the parcel </summary>
        public enum Priorities { regular, fast, urgent}
        /// <summary> enum for Weight Categories of the parcel and the drone </summary>
        public enum WeightCategories { light,medium,heavy}
        /// <summary> enum  for Drone Statuses  of the  drone </summary>
        public enum DroneStatuses { free,inMaintenance,busy}
        /// <summary>enum for the first dialog </summary>
        public enum Choice { add,update,display,viewLists,exit}
        /// <summary> enum for InsertrOption</summary>
        public enum Add { basestation,drone,newcustomer,parcelDelivery }
        /// <summary> enum for UpdatesOption</summary>
        public enum Update { parcelToDrone,pickUp,delivered,inCharging,outCharging}
        /// <summary>enum for DisplaySingleOption </summary>
        public enum Display {viewBaseStation,viewDrone,viewCustomer,viewParcel}
        /// <summary>enum for DisplayListOption </summary>
        public enum ViewList {listBaseStation , listDrone, listCustomer,listParcel, listFreeParcel, listFreeChargeSlotsInBaseStation}


    }
}
