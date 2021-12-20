using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
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
            //exception of the logical layer//Model???
            if (newDrone.Id < 0)
                throw new AddingProblemException(" מספר סידורי זה לא יכול להיות מספר שלילי");
            if (newDrone.MaxWeight < (WeightCategories)0 || newDrone.MaxWeight > (WeightCategories)2)
                throw new AddingProblemException("משקל לא תקין");
            if (dalObject.GetBaseStation(firstChargingStation).ChargeSlots == 0)
                throw new AddingProblemException("אין עמדות טעינה פנויות");
            IDAL.DO.Drone DroneDal = new IDAL.DO.Drone()
            {
                Id = newDrone.Id,
                Model = newDrone.Model,
                MaxWeight = (IDAL.DO.WeightCategories)newDrone.MaxWeight,

            };
            try
            {
                dalObject.SetDrone(DroneDal);
            }
            catch (IDAL.DO.AddExistingObjectException ex)
            {
                throw new AddingProblemException("קוד זה כבר קיים במערכת", ex);
            }
            newDrone.Battery = random.Next(20, 41);
            newDrone.Status = DroneStatuses.InMaintenance;
            newDrone.DroneLocation.Latitude = dalObject.GetBaseStation(firstChargingStation).Latitude;
            newDrone.DroneLocation.Longitude = dalObject.GetBaseStation(firstChargingStation).Longitude;
            dronesListBL.Add(newDrone);
            dalObject.LessChargeSlots(firstChargingStation);
            dalObject.SendDroneToCharge(firstChargingStation, newDrone.Id);

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
                IDAL.DO.Drone newDrone = dalObject.GetDrone(droneId);
                newDrone.Model = newDroneModel;
                dalObject.UpDateDrone(newDrone);
            }
            catch (IDAL.DO.NonExistingObjectException ex)
            {
                throw new UpdateProblemException("קוד  זה לא קיים במערכת", ex);

            }
            if (!dronesListBL.Exists(x => x.Id == droneId))
                throw new UpdateProblemException("קוד זה לא קיים");
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
                throw new UpdateProblemException("קוד  זה לא קיים במערכת");
            if (droneToCharge.Status != DroneStatuses.Free)//if the drone isnt free
                throw new UpdateProblemException("אין אפשרות לשלוח רחפן לטעינה שאינו פנוי");
            List<IDAL.DO.BaseStation> BaseStationDal = dalObject.GetBaseStationFreeChargeSlots().ToList();/*(x => x.ChargeSlots > 0).ToList();*///to filter the basesattion with free chargeslots
            if (BaseStationDal.Count == 0)//no frre stations with charge slots
                throw new UpdateProblemException("לא קיים  תחנה במערכת עם עמדות טעינה פנויות");
            List<BaseStation> BaseStationBl = new List<BaseStation>();
            foreach (var item in BaseStationDal)
            {
                BaseStationBl.Add(new BaseStation
                {
                    Id = item.Id,
                    Name = item.Name,
                    FreeChargeSlots = item.ChargeSlots,
                    BaseStationLocation = new Location() { Longitude = item.Longitude, Latitude = item.Latitude },
                    DroneChargingList = new List<DroneCharging>()
                });
            }

            double stationDistance = mindistanceBetweenLocationBaseStation(BaseStationBl, droneToCharge.DroneLocation).Item2;
            double batteryUse = stationDistance * Available;
            if (droneToCharge.Battery - batteryUse < 0)
                throw new UpdateProblemException("אין אפשרות לשלוח רחפן לטעינה");
            droneToCharge.Status = DroneStatuses.InMaintenance;
            droneToCharge.DroneLocation = mindistanceBetweenLocationBaseStation(BaseStationBl, droneToCharge.DroneLocation).Item1;
            droneToCharge.Battery -= batteryUse;
            dalObject.LessChargeSlots(BaseStationBl.Find(x => x.BaseStationLocation == droneToCharge.DroneLocation).Id);
            try
            {
                dalObject.SendDroneToCharge(droneId, BaseStationBl.Find(x => x.BaseStationLocation == droneToCharge.DroneLocation).Id);
            }
            catch (IDAL.DO.NonExistingObjectException )
            {
                throw new UpdateProblemException("קוד  זה לא קיים במערכת");

            }

        }
        #endregion Send Drone To Charge

        #region Release Drone  From Charging
        /// <summary>
        /// release drone from charging at BaseStation.
        /// </summary>
        /// <param name="droneId">Id of drone</param>
        public void ReleaseFromCharging(int droneId, DateTime time)
        {
            DroneToList droneToCharge = dronesListBL.Find(x => x.Id == droneId);
            if (droneToCharge == default)//if there is no drone with that id in the drone to list
                throw new UpdateProblemException("קוד  זה לא קיים במערכת");
            if (droneToCharge.Status != DroneStatuses.InMaintenance)//if the drone isnt free
                throw new UpdateProblemException("אין אפשרות לשחרר רחפן מטעינה שאינו בתחזוקה");
            double batteryLevel = (((time.Hour + (time.Minute) % 60 + (time.Second) % 3600) * DroneChargingRate) + droneToCharge.Battery);
            if (batteryLevel > 100) { batteryLevel = 100; }
            droneToCharge.Battery = batteryLevel;
            droneToCharge.Status = DroneStatuses.Free;
            dalObject.MoreChargeSlots(dalObject.GetDroneCharge(droneId).StationId);
            dalObject.ReleaseFromCharging(droneId);
        }
        #endregion Release Drone  From Charging

        #region display drone
        public Drone GetDrone(int idForDisplayDrone)
        {
            DroneToList droneToList = dronesListBL.Find(x => x.Id == idForDisplayDrone);
            if (droneToList == default)
                throw new GetDetailsProblemException("רחפן לא קיים");
            Drone displayDrone = new Drone()
            {
                Id = droneToList.Id,
                Battery = droneToList.Battery,
                DroneLocation = droneToList.DroneLocation,
                MaxWeight = droneToList.MaxWeight,
                Model = droneToList.Model,
                Status = droneToList.Status
            };
            if (droneToList.Status == DroneStatuses.Busy)
            {
                IDAL.DO.Parcel DalParcel = dalObject.GetParcel(droneToList.NumParcelTransfer);
                IDAL.DO.Customer DalSender = dalObject.GetCustomer(DalParcel.SenderId);
                IDAL.DO.Customer DalReciver = dalObject.GetCustomer(DalParcel.TargetId);

                Location locationOfSender = new Location() { Longitude = DalSender.Longitude, Latitude = DalSender.Latitude };
                Location locationOfReciver = new Location() { Longitude = DalReciver.Longitude, Latitude = DalReciver.Latitude };

                //sender
                displayDrone.DroneParcel.Sender.Id = DalParcel.SenderId;
                displayDrone.DroneParcel.Sender.Name = DalSender.Name;
                displayDrone.DroneParcel.CollectionLocation = locationOfSender;

                // reciver
                displayDrone.DroneParcel.Receiving.Id = DalReciver.Id;
                displayDrone.DroneParcel.Receiving.Name = DalReciver.Name;
                displayDrone.DroneParcel.DeliveryDestination = locationOfReciver;
                displayDrone.DroneParcel.TransportDistance = GetDistance(locationOfSender, locationOfReciver);
                displayDrone.DroneParcel.Weight = (WeightCategories)DalParcel.Weight;
                displayDrone.DroneParcel.Priority = (Priorities)DalParcel.Priority;
                displayDrone.DroneParcel.Id = DalParcel.Id;
                if (DalParcel.PickUp != DateTime.MinValue)

                    displayDrone.DroneParcel.ParcelStatus = true;

                else
                    displayDrone.DroneParcel.ParcelStatus = false;
            }
            return displayDrone;
        }
        #endregion display drone

        #region display drone list
        public IEnumerable<DroneToList> GetDroneList(Predicate<DroneToList> predicate = null)
        {
            return dronesListBL.FindAll(x => predicate == null ? true : predicate(x));
        }
        #endregion display drone list
    }
}
