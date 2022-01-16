using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BL
{
    public partial class BL
    {
        #region add parcel
        /// <summary>
        /// The function adds a Parcel to the list of Parcels.
        /// </summary>
        /// <param name="newParcel"></param>
        public void SetParcel(Parcel newParcel)
        {

            //exception of the logical layer
            if (newParcel.Sender.Id < 0 || newParcel.Receiving.Id < 0)
                throw new AddingProblemException(" ID cant be a negative number");
            if (newParcel.Weight < (WeightCategories)0 || newParcel.Weight > (WeightCategories)2)
                throw new AddingProblemException("Improper weight");
            if (newParcel.Priority < (Priorities)0 || newParcel.Priority > (Priorities)2)
                throw new AddingProblemException("Improper number");
            try
            {
                dalObject.GetCustomer(newParcel.Sender.Id);
                dalObject.GetCustomer(newParcel.Receiving.Id);
            }
            catch (DO.NonExistingObjectException)
            {
                throw new AddingProblemException("ID doesnt exists in the system");

            }
            DO.Parcel ParcelDal = new DO.Parcel()
            {
                SenderId = newParcel.Sender.Id,
                TargetId = newParcel.Receiving.Id,
                Weight = (DO.WeightCategories)newParcel.Weight,
                Priority = (DO.Priorities)newParcel.Priority,
                DroneId = 0,
                Scheduled=null,
                PickUp=null,
                Delivered=null,
                Requested = DateTime.Now
            };
           
                dalObject.SetParcel(ParcelDal);
            
           


        }
        #endregion  add customer

        #region Assign Parcel To Drone
        /// <summary>
        /// The function assign pacrel  to possible drone.
        /// </summary>
        /// <param name="droneId">id of drone</param>
        public void AssignParcelToDrone(int droneId)
        {
            DroneToList drone = dronesListBL.Find(x => x.Id == droneId);
            if (drone == default)//if there is no drone with that id in the drone to list
                throw new UpdateProblemException("ID doesnt exists in the system");
            if (drone.Status != DroneStatuses.Free)//if the drone isnt free
                throw new UpdateProblemException("not possible to assign a drone to a packadge if its not free ");

           


            IEnumerable<DO.Parcel> ReadyToAsiggenParcels = from item in dalObject.GetParcelList(x => x.DroneId == 0 && drone.MaxWeight >= (WeightCategories)x.Weight && DistancePossibleForDrone(x, drone))
                                                 orderby item.Priority descending, item.Weight descending, GetDistance(GetCustomer(item.SenderId).CustomerLocation, drone.DroneLocation)
                                                 select item;
            if (!ReadyToAsiggenParcels.Any())
            {
                throw new UpdateProblemException("no suitable parcels waiting to be assign");
            }
            DO.Parcel closestParcel = ReadyToAsiggenParcels.First();
           // DO.Parcel closestParcel = MinDistaceFromDroneToParcel(tempParcels, drone.DroneLocation);
            drone.Status = DroneStatuses.Busy;
            drone.NumParcelTransfer = closestParcel.Id;
            dalObject.SetParcelToDrone(closestParcel.Id, droneId);
            
        }
        #endregion Assign Parcel To Drone

        

       

        #region possible distace for drone

        /// <summary>
        /// The function calculates if the drone can take parcel with distance issues.
        /// </summary>
        /// <param name="item">list of the most urgent parcels</param>
        /// <param name="drone">drone object</param>
        /// <returns></returns>

        private bool DistancePossibleForDrone(DO.Parcel item, DroneToList drone)
        {
            double batteryUse = GetDistance(drone.DroneLocation, GetCustomer(item.SenderId).CustomerLocation) * Available;
            double distanceSenderTarget = GetDistance(GetCustomer(item.SenderId).CustomerLocation, GetCustomer(item.TargetId).CustomerLocation);
            switch ((WeightCategories)item.Weight)
            {
                case WeightCategories.Light:
                    batteryUse += distanceSenderTarget * LightWeightCarrier;
                    break;
                case WeightCategories.Medium:
                    batteryUse += distanceSenderTarget * MediumWeightCarrier;
                    break;
                case WeightCategories.Heavy:
                    batteryUse += distanceSenderTarget * HeavyWeightCarrier;
                    break;
                default:
                    break;
            }

            if (drone.Battery - batteryUse < 0) 
            { 
                return false;   
            }

            
           
            IEnumerable<DO.BaseStation> baseStationsDal = dalObject.GetBaseStationList();
            IEnumerable<BaseStation> blBaseStationList = from x in baseStationsDal
                                                         select new BaseStation() {
                                                             Id = x.Id,
                                                             Name = x.Name,
                                                             FreeChargeSlots = x.ChargeSlots,
                                                             BaseStationLocation = new Location() { Longitude = x.Longitude, Latitude = x.Latitude } };
            batteryUse += mindistanceBetweenLocationBaseStation(blBaseStationList, GetCustomer(item.TargetId).CustomerLocation).Item2*Available;
            if (drone.Battery - batteryUse < 0) 
            { 
                return false; 
            }
            else 
               return true;
        }
        #endregion possible distace for drone

    


        #region  picking up a parcel by drone
        /// <summary>
        /// picked up package by the drone.
        /// </summary>
        /// <param name="droneId">Id of Parcel</param>

        public void PickUpParcelByDrone(int droneId)
        {

            DroneToList drone = dronesListBL.Find(x => x.Id == droneId);
           DO.Parcel myParcel = dalObject.GetParcel(drone.NumParcelTransfer);
            if (drone == default)//if there is no drone with that id in the drone to list
                throw new UpdateProblemException("ID doesnt exists in the system");
            if (drone.NumParcelTransfer == 0)//if the drone isnt free
                throw new UpdateProblemException("not possible to pick up a package that has not been assign to a drone");
            if (myParcel.PickUp != null)
                throw new UpdateProblemException("not possible to pick up a package that has already been picked up");
            Location senderLocation = GetCustomer(myParcel.SenderId).CustomerLocation;
            drone.Battery-=(int) (GetDistance(drone.DroneLocation, senderLocation) * Available);
            drone.DroneLocation = senderLocation;
            dalObject.PickUpParcel(myParcel.Id);
            
        }
        #endregion  picking up a parcel by drone

        #region  drone deliver  parcel to customer
        /// <summary>
        /// delivery package to the customer.
        /// </summary>
        /// <param name="droneId">Id of Parcel</param>
        public void DroneDeliverParcel(int droneId)
        {

            DroneToList drone = dronesListBL.Find(x => x.Id == droneId);
            DO.Parcel myParcel = dalObject.GetParcel(drone.NumParcelTransfer);
            if (drone == default)//if there is no drone with that id in the drone to list
                throw new UpdateProblemException("ID  doesnt exists in the system");
            if (drone.NumParcelTransfer == 0)//if the drone isnt free
                throw new UpdateProblemException("not possible to deliver a package that has not  been assign to a drone");
            if (myParcel.PickUp == null)
                throw new UpdateProblemException("not possible to deliver a package that has not  been picked up ");
            if (myParcel.Delivered != null)
                throw new UpdateProblemException("not possible to deliver a package that has already been delivered ");
            if (myParcel.PickUp != null && myParcel.Delivered == null)
            {
                Location targetLocation = GetCustomer(myParcel.TargetId).CustomerLocation;
                switch ((WeightCategories)myParcel.Weight)
                {
                    case WeightCategories.Light:
                        drone.Battery -=( int)(GetDistance(targetLocation, drone.DroneLocation) * LightWeightCarrier);
                        break;
                    case WeightCategories.Medium:
                        drone.Battery -= (int)(GetDistance(targetLocation, drone.DroneLocation) * MediumWeightCarrier);
                        break;
                    case WeightCategories.Heavy:
                        drone.Battery -= (int)(GetDistance( targetLocation, drone.DroneLocation) * HeavyWeightCarrier);
                        break;
                    default:
                        break;
                }

                drone.DroneLocation = targetLocation;
                drone.Status = DroneStatuses.Free;
                drone.NumParcelTransfer = 0;
                dalObject.DeliverToCustomer(myParcel.Id);
               
            }

        }
        #endregion  drone deliver  parcel to customer

        #region display parcel
        /// <summary>
        /// The function return an exists parcel.
        /// </summary>
        public Parcel GetParcel(int idForDisplayParcel)
        {
            DO.Parcel dalParcel = new DO.Parcel();
            try
            {
                dalParcel = dalObject.GetParcel(idForDisplayParcel);
            }
            catch (DO.NonExistingObjectException)
            {
                throw new GetDetailsProblemException("ID  doesnt exists in the system");
            }

            DroneToList droneToList = dronesListBL.Find(x => x.Id == dalParcel.DroneId);
            CustomerParcel senderInDelivery = new CustomerParcel() { Id = dalParcel.SenderId, Name = dalObject.GetCustomer(dalParcel.SenderId).Name };
            CustomerParcel reciverInDelivery = new CustomerParcel() { Id = dalParcel.TargetId, Name = dalObject.GetCustomer(dalParcel.TargetId).Name };
            Parcel disPlayParcel = new Parcel()
            {
                Id = dalParcel.Id,
                Weight = (WeightCategories)dalParcel.Weight,
                Priority = (Priorities)dalParcel.Priority,
                Sender = senderInDelivery,
                Receiving = reciverInDelivery,
                Requested = dalParcel.Requested,
                Scheduled = dalParcel.Scheduled,
                PickUp = dalParcel.PickUp,
                Delivered = dalParcel.Delivered
            };
            if (disPlayParcel.Scheduled != null&& disPlayParcel.Delivered==null)
            {
                DroneParcel droneAssignToParcel = new DroneParcel()
                {
                    Id = droneToList.Id,
                    Battery = droneToList.Battery,
                    DroneLocationNow = droneToList.DroneLocation
                };
                disPlayParcel.DroneAtParcel = droneAssignToParcel;
            }

            return disPlayParcel;
        }
        #endregion display parcel

        #region display parcel list
        /// <summary>
        /// The function return a list of parcel.
        /// </summary>
        public IEnumerable<ParcelToList> GetParcelList(Predicate<ParcelToList> predicate = null)
        {
 
            IEnumerable<DO.Parcel> holdDalParcels = dalObject.GetParcelList();
            IEnumerable<ParcelToList> parcels = from item in holdDalParcels
                                                select new ParcelToList()
                                                {
                                                    Id = item.Id,
                                                    Weight = (WeightCategories)item.Weight,
                                                    Priority = (Priorities)item.Priority,
                                                    Sender = dalObject.GetCustomer(item.SenderId).Name,
                                                    Receiving = dalObject.GetCustomer(item.TargetId).Name,
                                                    Status= item.Delivered != null?  ParcelStatus.Delivered:item.PickUp != null? ParcelStatus.PickUp: item.Scheduled != null? 
                                                    ParcelStatus.Scheduled: ParcelStatus.Requested
                                                };

            return parcels.Where(x => predicate == null ? true : predicate(x));
        }

        #endregion display parcel list


        #region delete parcel

        /// <summary>
        /// The function delete an exists parcel.
        /// </summary>
        public void DeleteParcel(int ParcelId)
        {
            DO.Parcel myParcel = dalObject.GetParcel(ParcelId);
            if (myParcel.Scheduled!=null)
            {
                throw new UpdateProblemException("not possible to delete a package that has already been assigned ");
               
            }
            try
            {
                dalObject.DeleteParcel(ParcelId);
            }
            catch(DO.NonExistingObjectException)
            {
                throw new UpdateProblemException("ID  doesnt exists in the system");
            }
        }
        #endregion delete parcel
    }
}
