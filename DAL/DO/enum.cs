using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace DO
    {
        /// <summary> enum for Priorities of the parcel </summary>
        public enum Priorities { Regular, Fast, Urgent}
        /// <summary> enum for Weight Categories of the parcel and the drone </summary>
        public enum WeightCategories { Light,Medium,Heavy}
        /// <summary> enum  for Drone Statuses  of the  drone </summary>
        //public enum DroneStatuses { free,inMaintenance,busy}
        ///// <summary>enum for the first dialog </summary>
        public enum Choice { Add,Update,Display,ViewLists,Exit}
        /// <summary> enum for InsertrOption</summary>
        public enum Add { Basestation,Drone,NewCustomer,ParcelDelivery }
        /// <summary> enum for UpdatesOption</summary>
        public enum Update { ParcelToDrone,PickUp,Delivered,InCharging,OutCharging}
        /// <summary>enum for DisplaySingleOption </summary>
        public enum Display {ViewBaseStation,ViewDrone,ViewCustomer,ViewParcel}
        /// <summary>enum for DisplayListOption </summary>
        public enum ViewList {ListBaseStation , ListDrone, ListCustomer,ListParcel, ListFreeParcel, ListFreeChargeSlotsInBaseStation}


    }

