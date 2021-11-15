using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    /// <summary> enum for Priorities of the parcel </summary>
    public enum Priorities { Regular, Fast, Urgent }
    /// <summary> enum for Weight Categories of the parcel and the drone </summary>
    public enum WeightCategories { Light, Medium, Heavy }
    /// <summary> enum  for Drone Statuses  of the  drone </summary>
    public enum DroneStatuses { Free,InMaintenance,Busy}
    
    
    public enum ParcelStatus { Requested,Scheduled, PickUp, Delivered }//נוצר, שויך, נאסף, סופק
    /// <summary> enum for Statuses of the parcel </summary>
    public enum Choice { Add, Update, Display, ViewLists, Exit }
    /// <summary> enum for main choices</summary>

    public enum Add { Basestation,Drone,NewCustomer,ParcelDelivery }
    /// <summary> enum for InsertrOption</summary>

 
    public enum Update {DroneName, BaseStationUpDate, CustomerUpDate, InChargingDrone, OutChargingDrone, AssignDrone, PickUp, Deliver}
    ///<summary> enum for UpdatesOption</summary>

   
    public enum Display { ViewBaseStation, ViewDrone, ViewCustomer, ViewParcel }
    /// <summary>enum for DisplaySingleOption </summary>


    public enum ViewList { ListBaseStation, ListDrone, ListCustomer, ListParcel, ListFreeParcel, ListFreeChargeSlotsInBaseStation }
    /// <summary>enum for DisplayListOption </summary>


}
