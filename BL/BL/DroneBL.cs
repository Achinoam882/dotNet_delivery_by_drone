using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BlApi;

namespace BL
{
    public partial class BL
    {
        #region add drone


        /// <summary>
        ///  The function adds a drone to the list of drones.
        /// </summary>
        /// <param name="newDrone"></param>
        /// <param name="firstChargingStation"></param>
        public void SetDrone(DroneToList newDrone, int firstChargingStation)
        {
            //exception of the logical layer//
            if (newDrone.Id < 0)
                throw new AddingProblemException(" this number cant be  negative ");
            if (newDrone.MaxWeight < (WeightCategories)0 || newDrone.MaxWeight > (WeightCategories)2)
                throw new AddingProblemException("Improper weight");

            try
            {
                dalObject.GetBaseStation(firstChargingStation);
            }
            catch (DO.NonExistingObjectException)
            {
                throw new AddingProblemException("ID  Of BaseStation doesnt exists");
            }
            if (dalObject.GetBaseStation(firstChargingStation).ChargeSlots == 0)
                throw new AddingProblemException("no free charge slots");
            DO.Drone DroneDal = new DO.Drone()
            {
                Id = newDrone.Id,
                Model = newDrone.Model,
                MaxWeight = (DO.WeightCategories)newDrone.MaxWeight,

            };
            try
            {
                dalObject.SetDrone(DroneDal);
            }
            catch (DO.AddExistingObjectException)
            {
                throw new AddingProblemException("ID already exists");
            }
            newDrone.Battery = random.Next(20, 41);
            newDrone.Status = DroneStatuses.InMaintenance;
            Location location = new Location()
            {
             Longitude = dalObject.GetBaseStation(firstChargingStation).Longitude,
            Latitude = dalObject.GetBaseStation(firstChargingStation).Latitude 
            };

        
            newDrone.DroneLocation=location;          
            dalObject.LessChargeSlots(firstChargingStation);
            dalObject.SendDroneToCharge(newDrone.Id,firstChargingStation);
            dronesListBL.Add(newDrone);

        }
        #endregion add drone

        #region update Drone Model
        /// <summary>
        ///  The function updates the model of the drone.
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="newDroneModel"></param>
        public void ChangeDroneModel(int droneId, string newDroneModel)
        {
           
            try
            {
                DO.Drone newDrone = dalObject.GetDrone(droneId);
                newDrone.Model = newDroneModel;
                dalObject.UpDateDrone(newDrone);
            }
            catch (DO.NonExistingObjectException )
            {
                throw new UpdateProblemException("ID doesnt exists in the system");

            }
            if (!dronesListBL.Exists(x => x.Id == droneId))
                throw new UpdateProblemException("ID  doesnt exists in the system");
            dronesListBL.Find(x => x.Id == droneId).Model = newDroneModel;//update bl
        }
        #endregion update Drone Model

        #region Send Drone To Charge
        /// <summary>
        /// sending drone for charging at BaseStation.
        /// </summary>
        /// <param name="droneId">Id of drone</param>
        public void SendDroneToCharge(int droneId)
        {

            DroneToList droneToCharge = dronesListBL.Find(x => x.Id == droneId);
            if (droneToCharge == default)//if there is no drone with that id in the drone to list
                throw new UpdateProblemException("ID drone doesnt exists in the system");
            if (droneToCharge.Status != DroneStatuses.Free)//if the drone isnt free
                throw new UpdateProblemException("not possible to send a drone for charging if its not free ");
            IEnumerable<DO.BaseStation> BaseStationDal = dalObject.GetBaseStationList(x => x.ChargeSlots > 0);/*(x => x.ChargeSlots > 0).ToList();*///to filter the basesattion with free chargeslots

            IEnumerable<BaseStation> BaseStationBl = from item in BaseStationDal
                                                     select new BaseStation()
                                                     {
                                                         Id = item.Id,
                                                         Name = item.Name,
                                                         FreeChargeSlots = item.ChargeSlots,
                                                         BaseStationLocation = new Location() { Longitude = item.Longitude, Latitude = item.Latitude }
                                                     };
            if (!BaseStationBl.Any())//no frre stations with charge slots
                throw new UpdateProblemException("no base station with free charge slots");
           // Location closeBaseStation= (mindistanceBetweenLocationBaseStation(BaseStationBl, droneToCharge.DroneLocation).Item1);
            int stationDistance =(int)(mindistanceBetweenLocationBaseStation(BaseStationBl, droneToCharge.DroneLocation).Item2);
            int batteryUse =(int)(stationDistance * Available);
            if (droneToCharge.Battery - batteryUse < 0)
                throw new UpdateProblemException("not possible to send a drone to be charged");
            droneToCharge.Status = DroneStatuses.InMaintenance;
            droneToCharge.DroneLocation = mindistanceBetweenLocationBaseStation(BaseStationBl, droneToCharge.DroneLocation).Item1;
            droneToCharge.Battery -= batteryUse;
            dalObject.LessChargeSlots(BaseStationBl.First(x => x.BaseStationLocation.Latitude == droneToCharge.DroneLocation.Latitude&& x.BaseStationLocation.Longitude == droneToCharge.DroneLocation.Longitude).Id);
            dalObject.SendDroneToCharge(droneId, BaseStationBl.First(x => x.BaseStationLocation.Latitude == droneToCharge.DroneLocation.Latitude && x.BaseStationLocation.Longitude == droneToCharge.DroneLocation.Longitude).Id);         
        }
        #endregion Send Drone To Charge

        #region Release Drone  From Charging
        /// <summary>
        /// release drone from charging at BaseStation.
        /// </summary>
        /// <param name="droneId">Id of drone</param>
        public void ReleaseFromCharging(int droneId)
        {
            DroneToList droneToCharge = dronesListBL.Find(x => x.Id == droneId);
            if (droneToCharge == default)//if there is no drone with that id in the drone to list
                throw new UpdateProblemException("ID  doesnt exists");
            if (droneToCharge.Status != DroneStatuses.InMaintenance)//if the drone isnt free
                throw new UpdateProblemException("not possible to release  a drone from charging if it is not in maintenance");
            TimeSpan timeCharging = (TimeSpan)(DateTime.Now - dalObject.GetDroneCharge(droneId).TimeDroneInCharging);   
            int batteryLevel = (int)(((timeCharging.TotalHours * DroneChargingRate) + droneToCharge.Battery));
            if (batteryLevel > 100) { batteryLevel = 100; }
            droneToCharge.Battery = batteryLevel;
            droneToCharge.Status = DroneStatuses.Free;
            dalObject.MoreChargeSlots(dalObject.GetDroneCharge(droneId).StationId);
            dalObject.ReleaseFromCharging(droneId);
            
}
        #endregion Release Drone  From Charging

        #region display drone
        /// <summary>
        /// The function return an exists drone 
        /// </summary>
        /// <param name="idForDisplayDrone">Id of drone</param>
        public Drone GetDrone(int idForDisplayDrone)
        {
            DroneToList droneToList = dronesListBL.Find(x => x.Id == idForDisplayDrone);
            if (droneToList == default)
                throw new GetDetailsProblemException("Drone doesnt exists in the system");
            Drone displayDrone = new Drone()
            {
                Id = droneToList.Id,
                Battery = droneToList.Battery,
                DroneLocation = droneToList.DroneLocation,
                MaxWeight = droneToList.MaxWeight,
                Model = droneToList.Model,
                Status = droneToList.Status,
                DroneParcel=new ParcelInTransfer()

            };
            if (droneToList.Status == DroneStatuses.Busy )
            {
                DO.Parcel DalParcel = dalObject.GetParcel(droneToList.NumParcelTransfer);
                DO.Customer DalSender = dalObject.GetCustomer(DalParcel.SenderId);
                DO.Customer DalReciver = dalObject.GetCustomer(DalParcel.TargetId);

                Location locationOfSender = new Location() { Longitude = DalSender.Longitude, Latitude = DalSender.Latitude };
                Location locationOfReciver = new Location() { Longitude = DalReciver.Longitude, Latitude = DalReciver.Latitude };

                //sender
                displayDrone.DroneParcel.Sender = new CustomerParcel() { Id = DalParcel.SenderId, Name = DalSender.Name };
                displayDrone.DroneParcel.CollectionLocation = locationOfSender;


                // reciver
                displayDrone.DroneParcel.Receiving = new CustomerParcel() { Id = DalReciver.Id, Name = DalReciver.Name };
                displayDrone.DroneParcel.DeliveryDestination = locationOfReciver;
                //if (DalParcel.PickUp == null)
                //    displayDrone.DroneParcel.TransportDistance = GetDistance(displayDrone.DroneLocation, locationOfSender);
                //else
                //    displayDrone.DroneParcel.TransportDistance = GetDistance(locationOfSender, locationOfReciver);

                displayDrone.DroneParcel.TransportDistance = DalParcel.PickUp == null ? GetDistance(displayDrone.DroneLocation, locationOfSender) : GetDistance(locationOfSender, locationOfReciver);
                displayDrone.DroneParcel.Weight = (WeightCategories)DalParcel.Weight;
                displayDrone.DroneParcel.Priority = (Priorities)DalParcel.Priority;
                displayDrone.DroneParcel.Id = DalParcel.Id;
                displayDrone.DroneParcel.ParcelStatus = DalParcel.PickUp != null ? true : false;
                //if (DalParcel.PickUp != null)
                //   displayDrone.DroneParcel.ParcelStatus = true;
                //else
                //    displayDrone.DroneParcel.ParcelStatus = false;
            }

            return displayDrone;
        }
        #endregion display drone

        #region display drone list
        /// <summary>
        /// The function return a list of drone
        /// </summary>
        public IEnumerable<DroneToList> GetDroneList(Predicate<DroneToList> predicate = null)
        {
            return dronesListBL.FindAll(x => predicate == null ? true : predicate(x));
        }
        #endregion display drone list

        #region Drone Simulator
      
        /// The function start the simulation
        /// </summary>
        public void SimulatorOn(int droneId, Action ToReportProgress, Func<bool> IsTimeFinish)
        {
            new Simulator(this, droneId, ToReportProgress, IsTimeFinish);
        }
        #endregion Drone Simulator
    }
}
