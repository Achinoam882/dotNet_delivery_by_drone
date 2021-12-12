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
        #region add parcel
        /// <summary>
        /// The function adds a Parcel to the list of Parcels.
        /// </summary>
        /// <param name="newParcel"></param>
        /// <returns></returns>
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
            catch (IDAL.DO.NonExistingObjectException)
            {
                throw new AddingProblemException("ID doesnt exists in the system");

            }
            IDAL.DO.Parcel ParcelDal = new IDAL.DO.Parcel()
            {
                //Id = newParcel.Id,
                SenderId = newParcel.Sender.Id,
                TargetId = newParcel.Receiving.Id,
                Weight = (IDAL.DO.WeightCategories)newParcel.Weight,
                Priority = (IDAL.DO.Priorities)newParcel.Priority,
                DroneId = 0,
                Requested = DateTime.Now
            };
           
                dalObject.SetParcel(ParcelDal);
            
           


        }
        #endregion  add customer

        #region Assign Parcel To Drone
        public void AssignParcelToDrone(int droneId)
        {
            DroneToList drone = dronesListBL.Find(x => x.Id == droneId);
            if (drone == default)//if there is no drone with that id in the drone to list
                throw new UpdateProblemException("ID doesnt exists in the system");
            if (drone.Status != DroneStatuses.Free)//if the drone isnt free
                throw new UpdateProblemException("not possible to assign a drone to a packadge if its not free ");

            List<IDAL.DO.Parcel> highestPriority = ParcelHighestPriorityList(drone);
            List<IDAL.DO.Parcel> heaviest = FindingHeaviestList(highestPriority, drone);
            if (!heaviest.Any())
            {
                throw new UpdateProblemException("no parcels waiting to be assign");
            }
            IDAL.DO.Parcel closestParcel = MinDistaceFromDroneToParcel(heaviest, drone.DroneLocation);
            drone.Status = DroneStatuses.Busy;
            drone.NumParcelTransfer = closestParcel.Id;
            dalObject.SetParcelToDrone(closestParcel.Id, droneId);
            
        }
        #endregion Assign Parcel To Drone

        #region Assign Parcel To Drone functions
        #region Highest Priority List
        /// <summary>
        /// The function finds a list of the most urgent parcels.
        /// </summary>
        /// <param name="drone">drone object</param>
        /// <returns>list of the most urgent packages</returns>
        private List<IDAL.DO.Parcel> ParcelHighestPriorityList(DroneToList drone)
        {
            List<IDAL.DO.Parcel> allParcels = dalObject.GetParcelList(x => x.DroneId == 0).ToList();
            List<IDAL.DO.Parcel> regularList = new List<IDAL.DO.Parcel>();//creata regular priority list
            List<IDAL.DO.Parcel> fastList = new List<IDAL.DO.Parcel>();//creata fastpriority list
            List<IDAL.DO.Parcel> urgentList = new List<IDAL.DO.Parcel>();//creata urgent priority list
            foreach (var item in allParcels)
            {
                if (drone.MaxWeight >= (WeightCategories)item.Weight && DistancePossibleForDrone(item, drone))
                {
                    switch ((Priorities)item.Priority)
                    {
                        case Priorities.Regular:
                            regularList.Add(item);
                            break;
                        case Priorities.Fast:
                            fastList.Add(item);
                            break;
                        case Priorities.Urgent:
                            urgentList.Add(item);
                            break;
                        default:
                            break;
                    }

                }
            }
            return (urgentList.Any() ? urgentList : fastList.Any() ? fastList : regularList);
        }
        #endregion Highest Priority List

        #region Highest Weight List
        /// <summary>
        /// The function finds a list of the heaviest parcels for the drone with the heights priority.
        /// </summary>
        /// <param name="parcels">list of the most urgent parcels</param>
        /// <param name="drone">drone object</param>
        /// <returns></returns>
        private List<IDAL.DO.Parcel> FindingHeaviestList(List<IDAL.DO.Parcel> allParcels, DroneToList drone)
        {
            List<IDAL.DO.Parcel> lightList = new List<IDAL.DO.Parcel>();//creata light weight list
            List<IDAL.DO.Parcel> mediumList = new List<IDAL.DO.Parcel>();//creata medium weight list
            List<IDAL.DO.Parcel> heavyList = new List<IDAL.DO.Parcel>();//creata heavy weight list
            foreach (var item in allParcels)
            {
                switch ((WeightCategories)item.Weight)
                {
                    case WeightCategories.Light:
                        lightList.Add(item);
                        break;
                    case WeightCategories.Medium:
                        mediumList.Add(item);
                        break;
                    case WeightCategories.Heavy:
                        heavyList.Add(item);
                        break;
                    default:
                        break;
                }
            }
            return (heavyList.Any() ? heavyList : mediumList.Any() ? mediumList : lightList);


        }
        #endregion  Highest Weight List

        #region possible distace for drone

        /// <summary>
        /// The function calculates if the drone can take parcel with distance issues.
        /// </summary>
        /// <param name="item">list of the most urgent parcels</param>
        /// <param name="drone">drone object</param>
        /// <returns></returns>

        private bool DistancePossibleForDrone(IDAL.DO.Parcel item, DroneToList drone)
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

            
            List<BaseStation> blBaseStationList = new List<BaseStation>();
            List<IDAL.DO.BaseStation> baseStationsDal = dalObject.GetBaseStationList().ToList();
            foreach (var x in baseStationsDal)
            {
                blBaseStationList.Add(new BaseStation
                {
                    Id = x.Id,
                    Name = x.Name,
                    FreeChargeSlots = x.ChargeSlots,
                    BaseStationLocation = new Location() { Longitude = x.Longitude, Latitude = x.Latitude }
                });
            }
            batteryUse += mindistanceBetweenLocationBaseStation(blBaseStationList, GetCustomer(item.TargetId).CustomerLocation).Item2*Available;
            if (drone.Battery - batteryUse < 0) 
            { 
                return false; 
            }
            else 
               return true;
        }
        #endregion possible distace for drone

        #region Minimum Distace From Drone To Parcel
        /// <summary>
        /// The function calculates the closest parcel to a drone.
        /// </summary>
        /// <param name="heaviest">list of the most urgent  and heaviest parcels</param>
        /// <param name="location">location of the drone</param>
        /// <returns></returns>
        private IDAL.DO.Parcel MinDistaceFromDroneToParcel(List<IDAL.DO.Parcel> heaviest, Location location)
        {
            List<double> distanceList = new List<double>();
            foreach (var x in heaviest)
            {
                Location senderLocation = GetCustomer(x.SenderId).CustomerLocation;
                distanceList.Add(GetDistance(location, senderLocation));
            }
            double mindistance = distanceList.Min();
            return heaviest[distanceList.FindIndex(dis => dis == mindistance)];
        }
        #endregion Minimum Distace From Drone To Parcel

        #endregion Assign Parcel To Drone functions

        #region  picking up a parcel by drone
        /// <summary>
        /// picked up package by the drone.
        /// </summary>
        /// <param name="droneId">Id of Parcel</param>

        public void PickUpParcelByDrone(int droneId)
        {

            DroneToList drone = dronesListBL.Find(x => x.Id == droneId);
            IDAL.DO.Parcel myParcel = dalObject.GetParcel(drone.NumParcelTransfer);
            if (drone == default)//if there is no drone with that id in the drone to list
                throw new UpdateProblemException("ID doesnt exists in the system");
            if (drone.NumParcelTransfer == 0)//if the drone isnt free
                throw new UpdateProblemException("not possible to pick up a package that has not been assign to a drone");
            if (myParcel.PickUp != null)
                throw new UpdateProblemException("not possible to pick up a package that has already been picked up");
            Location senderLocation = GetCustomer(myParcel.SenderId).CustomerLocation;
            drone.Battery-= GetDistance(drone.DroneLocation, senderLocation) * Available;
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
            IDAL.DO.Parcel myParcel = dalObject.GetParcel(drone.NumParcelTransfer);
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
                        drone.Battery -= GetDistance(targetLocation, drone.DroneLocation) * LightWeightCarrier;
                        break;
                    case WeightCategories.Medium:
                        drone.Battery -= GetDistance(targetLocation, drone.DroneLocation) * MediumWeightCarrier;
                        break;
                    case WeightCategories.Heavy:
                        drone.Battery -= GetDistance( targetLocation, drone.DroneLocation) * HeavyWeightCarrier;
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
        public Parcel GetParcel(int idForDisplayParcel)
        {
            IDAL.DO.Parcel dalParcel = new IDAL.DO.Parcel();
            try
            {
                dalParcel = dalObject.GetParcel(idForDisplayParcel);
            }
            catch (IDAL.DO.NonExistingObjectException)
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
            if (disPlayParcel.Scheduled != null)
            {
                DroneParcel droneInThePackage = new DroneParcel()
                {
                    Id = droneToList.Id,
                    Battery = droneToList.Battery,
                    DroneLocationNow = droneToList.DroneLocation
                };
                disPlayParcel.DroneAtParcel = droneInThePackage;
            }

            return disPlayParcel;
        }
        #endregion display parcel

        #region display parcel list
        public IEnumerable<ParcelToList> GetParcelList(Predicate<ParcelToList> predicate = null)
        {
            List<ParcelToList> parcels = new List<ParcelToList>();
            List<IDAL.DO.Parcel> holdDalParcels = dalObject.GetParcelList().ToList();

            foreach (var item in holdDalParcels)
            {
                ParcelStatus currentStatus;
                if (item.Delivered != null)
                    currentStatus = ParcelStatus.Delivered;
                else if (item.PickUp != null)
                    currentStatus = ParcelStatus.PickUp;
                else if (item.Scheduled != null)
                    currentStatus = ParcelStatus.Scheduled;
                else
                    currentStatus = ParcelStatus.Requested;

                parcels.Add(new ParcelToList
                {
                    Id = item.Id,
                    Weight = (WeightCategories)item.Weight,
                    Priority = (Priorities)item.Priority,
                    Sender = dalObject.GetCustomer(item.SenderId).Name,
                    Receiving = dalObject.GetCustomer(item.TargetId).Name,
                    Status = currentStatus
                });
            }

            return parcels.FindAll(x => predicate == null ? true : predicate(x));
        }

        #endregion display parcel list
    }
}
