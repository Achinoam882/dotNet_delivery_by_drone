using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IBL;
//using IDAL;
//using IDAL.DO;
namespace IBL
{
    public partial class BL : IBL
    {
        #region finding the nearest base station to the location
        /// <summary>
        /// The function calculates the distance between a particular location and base stations.
        /// </summary>
        /// <param name="baseStationBL">baseStationBL List</param>
        /// <param name="location">location</param>
        /// <returns>The location of the base station closest to the location and the min distance</returns>
        private (Location, double) mindistanceBetweenLocationBaseStation(List<BaseStation> baseStationBL, Location location)
        {
            List<double> distance = new List<double>();
            foreach (var item in baseStationBL)
            {
                distance.Add(GetDistance(location, item.BaseStationLocation));
            }
            double minimumDistance = distance.Min();
            return (baseStationBL[distance.FindIndex(x => x == minimumDistance)].BaseStationLocation, minimumDistance);

        }
        #endregion finding the nearest base station to the location


        #region  calculating distance between  2 locations

        /// <summary>
        /// A function that calculates the distance between points
        /// </summary>
        /// <param name="location1">location1</param>
        /// <param name="location2">location2</param>
        /// <returns>the distence between the points</returns>
        private double GetDistance(Location location1, Location location2)
        {
            //For the calculation we calculate the earth into a circle (ellipse) Divide its 360 degrees by half
            //180 for each longitude / latitude and then make a pie on each half to calculate the radius for
            //the formula below
            var num1 = location1.Longitude * (Math.PI / 180.0);
            var d1 = location1.Latitude * (Math.PI / 180.0);
            var num2 = location2.Longitude * (Math.PI / 180.0) - num1;
            var d2 = location2.Latitude * (Math.PI / 180.0);

            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);


            return (double)(6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))));
        }
        #endregion   calculating distance between  2 location


        #region Calculate Electricity Use
        /// <summary>
        /// The function calculates the Electricity Use .
        /// </summary>
        /// <param name="weight">WeightCategories</param>
        /// <param name="distance1">double</param>
        ///  <param name="distance2">double</param>
        /// <returns>The electric use</returns>
        private double CalculateElectricityUse(WeightCategories weight, double distance1, double distance2)
        {
            double electricityUse = distance2 * Available;
            WeightCategories weightOfParcel = weight;
            switch (weightOfParcel)
            {
                case WeightCategories.Light:
                    electricityUse += distance1 * LightWeightCarrier;
                    break;
                case WeightCategories.Medium:
                    electricityUse += distance1 * MediumWeightCarrier;
                    break;
                case WeightCategories.Heavy:
                    electricityUse += distance1 * HeavyWeightCarrier;
                    break;
                default:
                    break;
            }
            return electricityUse;
        }
        #endregion Calculate Electricity Use

        public double Available;
        public double LightWeightCarrier;
        public double MediumWeightCarrier;
        public double HeavyWeightCarrier;
        public double DroneChargingRate;

        public List<DroneToList> dronesListBL;
        IDAL.IDal dalObject;

        //Create a Random object to be used to draw the battery status and Location of the drones.
        Random random = new Random(DateTime.Now.Millisecond);
        #region constractor
        public BL()
        {
            //Creates an object that will serve as an access point to methods in DAL.
            dalObject = new DalObject.DalObject();

            double[] arrDrone = dalObject.RequestPowerbyDrone();
            Available = arrDrone[0];
            LightWeightCarrier = arrDrone[1];
            MediumWeightCarrier = arrDrone[2];
            HeavyWeightCarrier = arrDrone[3];
            DroneChargingRate = arrDrone[4];

            // Convert a layer dronelist to a logical layer drone list
            List<IDAL.DO.Drone> dronesDal = dalObject.GetDroneList().ToList();
            dronesListBL = new List<DroneToList>();
            foreach (var item in dronesDal)
            {
                dronesListBL.Add(new DroneToList { Id = item.Id, Model = item.Model,
                    MaxWeight = (WeightCategories)item.MaxWeight
                });
            }

            //creating a customer list 
            List<Customer> customerListBL = new List<Customer>();
            List<IDAL.DO.Customer> customerDal = dalObject.GetCustomerList().ToList();
            foreach (var item in customerDal)
            {
                Location location = new Location { Latitude = item.Latitude, Longitude = item.Longitude };
                customerListBL.Add(new Customer
                {
                    Id = item.Id,
                    Name = item.Name,
                    PhoneNumber = item.PhoneNumber,
                    CustomerLocation = location
                });
            }
            //creating a basestationlist list
            List<BaseStation> baseStationBL = new List<BaseStation>();
            List<IDAL.DO.BaseStation> baseStationDal = dalObject.GetBaseStationList().ToList();
            foreach (var item in baseStationDal)
            {
                Location location = new Location { Latitude = item.Latitude, Longitude = item.Longitude };
                baseStationBL.Add(new BaseStation
                {
                    Id = item.Id,
                    Name = item.Name,
                    FreeChargeSlots = item.ChargeSlots,
                    BaseStationLocation = location
                });
            }

            //creating a parcel list of parcel fro the data layer that were set to a drone already
            List<IDAL.DO.Parcel> parcelListDal = dalObject.GetParcelList(x => x.DroneId != 0).ToList();




            foreach (var item in dronesListBL)
            {
                int index = parcelListDal.FindIndex(x => x.DroneId == item.Id && x.Delivered == DateTime.MinValue);
                if (index != -1)
                {
                    item.Status = DroneStatuses.Busy;//if the parcel was set to a drone but wasnt delivered so the drone status is bust
                    Location locationSender = customerListBL.Find(x => x.Id == parcelListDal[index].SenderId).CustomerLocation;//creating the location of the sender of the parcel that was set to a drond but not delivered yet
                                                                                                                               //Distance between sender and receiver.
                    Location locationReceiver = customerListBL.Find(x => x.Id == parcelListDal[index].TargetId).CustomerLocation;
                    double distanceBetweenSenderAndReceiver = GetDistance(locationSender, locationReceiver);

                    //Distance between the receiver and the nearest base station.
                    double DistanceBetweenReceiverAndNearestBaseStation = mindistanceBetweenLocationBaseStation(baseStationBL, locationReceiver).Item2;
                    double electricityUse = CalculateElectricityUse((WeightCategories)parcelListDal[index].Weight, distanceBetweenSenderAndReceiver, DistanceBetweenReceiverAndNearestBaseStation);

                    if (parcelListDal[index].PickUp == DateTime.MinValue)//if the parcel wasnt pickedup yet
                    {
                        item.DroneLocation = mindistanceBetweenLocationBaseStation(baseStationBL, locationSender).Item1;
                        electricityUse += GetDistance(item.DroneLocation, locationSender) * Available; //
                    }
                    else
                    {
                        item.DroneLocation = locationSender;
                    }
                    // random number battery status between minimum charge to make the shipment and full charge.     
                    item.Battery = ((float)((float)random.NextDouble() * (100 - electricityUse)) + electricityUse);

                }
                else//if the drone isnt busy
                {
                    item.Status = (DroneStatuses)random.Next(0, 2);
                    if (item.Status == DroneStatuses.InMaintenance)
                    {
                        item.DroneLocation = baseStationBL[random.Next(0, dronesListBL.Count)].BaseStationLocation;
                        item.Battery = random.Next(0, 21);
                        //senddronetocharge?dronecharging?
                    }
                    if (item.Status == DroneStatuses.Free)
                    {
                        List<IDAL.DO.Parcel> parcelListDeliveredByThisDrone = parcelListDal.FindAll(x => x.DroneId == item.Id && x.Delivered != DateTime.MinValue);
                        if (parcelListDeliveredByThisDrone.Count != 0)
                        {
                            int i = random.Next(0, parcelListDeliveredByThisDrone.Count);
                            item.DroneLocation = customerListBL.Find(x => x.Id == parcelListDeliveredByThisDrone[i].TargetId).CustomerLocation;
                            double batteryUse = mindistanceBetweenLocationBaseStation(baseStationBL, item.DroneLocation).Item2 * Available;
                            item.Battery = (float)((float)(random.NextDouble() * (100 - batteryUse)) + batteryUse);

                        }

                    }


                }
            }




        }
        #endregion constractor

        #region add options
        #region add base station
        /// <summary>
        /// The function adds a station to the list of Basestations.
        /// </summary>
        /// <param name="newbaseStation"></param>
        public void SetBaseStation(BaseStation newBaseStation)
        {
            //exception of the logical layer//name??
            if (newBaseStation.Id < 0)
                throw new AddingProblemException("קוד תחנה לא יכול להיות מספר שלילי");
            if (newBaseStation.FreeChargeSlots < 0)
                throw new AddingProblemException("מספר עמדות פנויות לא יכול להיות מספר שלילי");
            if (newBaseStation.BaseStationLocation.Longitude < 34.3 || newBaseStation.BaseStationLocation.Latitude > 35.5)
                throw new AddingProblemException("המיקום שנבחר לא נמצא בגבולות הארץ");
            IDAL.DO.BaseStation baseStationDal = new IDAL.DO.BaseStation()
            {
                Id = newBaseStation.Id,
                ChargeSlots = newBaseStation.FreeChargeSlots,
                Name = newBaseStation.Name,
                Longitude = newBaseStation.BaseStationLocation.Longitude,
                Latitude = newBaseStation.BaseStationLocation.Latitude,

            };
            try
            {
                dalObject.SetBaseStation(baseStationDal);
            }
            catch (IDAL.DO.AddExistingObjectException ex)
            {
                throw new AddingProblemException("קוד תחנה זה כבר קיים במערכת", ex);

            }
        }
        #endregion add base station


        #region add drone
        /// The function adds a drone to the list of drones.
        /// </summary>
        /// <param name="newDrone"></param>
        public void SetDrone(DroneToList newDrone, int firstChargingStation)
        {
            //exception of the logical layer//Model???
            if (newDrone.Id < 0)
                throw new AddingProblemException(" מספר סידורי זה לא יכול להיות מספר שלילי");
            if (newDrone.MaxWeight < (WeightCategories)0 || newDrone.MaxWeight > (WeightCategories)2)
                throw new AddingProblemException("משקל לא תקין");
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
                throw new AddingProblemException("קוד  זה כבר קיים במערכת", ex);
            }
            newDrone.Battery = random.Next(20, 41);
            newDrone.Status = DroneStatuses.InMaintenance;
            newDrone.DroneLocation.Latitude = dalObject.GetBaseStation(firstChargingStation).Latitude;
            newDrone.DroneLocation.Longitude = dalObject.GetBaseStation(firstChargingStation).Longitude;
            dronesListBL.Add(newDrone);
            //האם לשלוח רחפן לטעינה???
        }
        #endregion add drone

        #region  add customer
        /// <summary>
        /// The function adds a customer to the list of customers.
        /// </summary>
        /// <param name="newCustomer"></param>
        public void SetCustomer(Customer newCustomer)
        {

            //exception of the logical layer//name,phonenumber???
            if (newCustomer.Id < 0)
                throw new AddingProblemException(" תהודת זהות  לא יכול להיות מספר שלילי");
            if (newCustomer.CustomerLocation.Longitude < 34.3 || newCustomer.CustomerLocation.Latitude > 35.5)
                throw new AddingProblemException("המיקום שנבחר לא נמצא בגבולות הארץ");

            IDAL.DO.Customer CustomerDal = new IDAL.DO.Customer()
            {
                Id = newCustomer.Id,
                Name = newCustomer.Name,
                PhoneNumber = newCustomer.PhoneNumber,
                Longitude = newCustomer.CustomerLocation.Longitude,
                Latitude = newCustomer.CustomerLocation.Latitude,

            };
            try
            {
                dalObject.SetCustomer(CustomerDal);
            }
            catch (IDAL.DO.AddExistingObjectException ex)
            {
                throw new AddingProblemException("קוד  זה כבר קיים במערכת", ex);

            }
        }
        #endregion  add customer

        #region add parcel
        /// <summary>
        /// The function adds a Parcel to the list of Parcels.
        /// </summary>
        /// <param name="newParcel"></param>
        /// <returns></returns>
        public void SetParcel(Parcel newParcel)
        {

            //exception of the logical layer//name,phonenumber???
            if (newParcel.Sender.Id < 0 || newParcel.Receiving.Id < 0)
                throw new AddingProblemException(" תהודת זהות  לא יכול להיות מספר שלילי");
            if (newParcel.Weight < (WeightCategories)0 || newParcel.Weight > (WeightCategories)2)
                throw new AddingProblemException("משקל לא תקין");
            if (newParcel.Priority < (Priorities)0 || newParcel.Priority > (Priorities)2)
                throw new AddingProblemException("מס שהוכנס לא תקין");
            IDAL.DO.Parcel ParcelDal = new IDAL.DO.Parcel()
            {
                Id = newParcel.Id,
                SenderId = newParcel.Sender.Id,
                TargetId = newParcel.Receiving.Id,
                Weight = (IDAL.DO.WeightCategories)newParcel.Weight,
                Priority = (IDAL.DO.Priorities)newParcel.Priority,
                DroneId = 0,/*?????*/
                Requested = DateTime.Now
            };
            try
            {
                dalObject.SetParcel(ParcelDal);
            }
            catch (IDAL.DO.AddExistingObjectException ex)
            {
                throw new AddingProblemException("קוד  זה כבר קיים במערכת", ex);

            }


        }
        #endregion  add customer
        #endregion add options

        

        #region update options
        #region update Drone Model
        public void ChangeDroneModel(int droneId, string newDroneModel)
        {
            //בדיקת תקינות MODEL?
            try
            {
                dronesListBL.Find(x => x.Id == droneId).Model = newDroneModel;//update bl

            }
            catch (IDAL.DO.NonExistingObjectException ex)
            {
                throw new UpdateProblemException("קוד  זה לא קיים במערכת", ex);

            }
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
        }
        #endregion update Drone Model

        #region update Base Staison
        public void UpdateBaseStaison(int baseStationId, string baseStationName, string chargeSlots)
        {
            try
            {
                int chargeSlotsTottal, usedChargeSlotsTottal;
                IDAL.DO.BaseStation newBaseStation = dalObject.GetBaseStation(baseStationId);
                if (baseStationName != "")
                    newBaseStation.Name = baseStationName;
                if (chargeSlots != "")
                {
                    while (!int.TryParse(chargeSlots, out chargeSlotsTottal)) ;
                    usedChargeSlotsTottal = dalObject.GetChargeSlotsList(x => x.StationId == baseStationId).ToList().Count;
                    if ((chargeSlotsTottal - usedChargeSlotsTottal)< 0)
                    {
                        throw new UpdateProblemException("מספר עמדות טעינה לא התקבל");
                    }
                    newBaseStation.ChargeSlots = chargeSlotsTottal - usedChargeSlotsTottal;
                    dalObject.UpdateBaseStation(newBaseStation);

                }                     
            }
            catch (IDAL.DO.NonExistingObjectException ex)
            {
                throw new UpdateProblemException("קוד תחנה זה לא קיים במערכת", ex);
            }
        }

        #endregion update Base Staison

        #region Update Customer
        public void UpdateCustomer(int customerId,string customerName,string phoneNumber)
        {
               try
                {
                    IDAL.DO.Customer newCustomer = dalObject.GetCustomer(customerId);
                    if (customerName != "")
                        newCustomer.Name = customerName;
                    if (phoneNumber != "")
                        newCustomer.PhoneNumber = phoneNumber;
                dalObject.UpDateCustomer(newCustomer);
            }
            catch (IDAL.DO.NonExistingObjectException ex)
               {
                throw new UpdateProblemException("קוד  זה לא קיים במערכת", ex);
               }
        }

        #endregion Update Customer
        #region Send Drone To Charge
        public void SendDroneToCharge(int droneId)
        {
            try
            {
                IDAL.DO.DroneCharge newDroneCharge = dalObject.(droneId);
            }
            catch (IDAL.DO.NonExistingObjectException ex)
            {
                throw new UpdateProblemException("קוד  זה לא קיים במערכת", ex);
            } 
        }
        #endregion Send Drone To Charge



















        #endregion update Drone Model






        #endregion update options
    }

}
