using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BlApi;
using DalApi;
//using IDAL;
//using IDAL.DO;



namespace BL
{
    public partial class BL : BlApi.IBL
    {



        #region finding the nearest base station to the location
        /// <summary>
        /// The function calculates the distance between a particular location and base stations.
        /// </summary>
        /// <param name="baseStationBL">baseStationBL List</param>
        /// <param name="location">location</param>
        /// <returns>The location of the base station closest to the location and the min distance</returns>
        internal (Location, double) mindistanceBetweenLocationBaseStation(IEnumerable<BaseStation> baseStationBL, Location location)
        {
            IEnumerable<Location> distance = from item in baseStationBL
                                             orderby GetDistance(location, item.BaseStationLocation)
                                             select item.BaseStationLocation;

            Location minimumDistance = distance.First();
            return (minimumDistance, GetDistance(location, minimumDistance));




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
                                                                                                                                   
                                                                                                                                  
            return ((double)(6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))))) / 1000;
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
            int electricityUse = (int)(distance2 * Available);
            WeightCategories weightOfParcel = weight;
            switch (weightOfParcel)
            {
                case WeightCategories.Light:
                    electricityUse +=(int)( distance1 * LightWeightCarrier);
                    break;
                case WeightCategories.Medium:
                    electricityUse += (int)(distance1 * MediumWeightCarrier);
                    break;
                case WeightCategories.Heavy:
                    electricityUse += (int)(distance1 * HeavyWeightCarrier);
                    break;
                default:
                    break;
            }
            return electricityUse;
        }
        #endregion Calculate Electricity Use

        public static double Available ;
        public static double LightWeightCarrier ;
        public static double MediumWeightCarrier ;
        public static double HeavyWeightCarrier ;
        public static double DroneChargingRate ;

        public List<DroneToList> dronesListBL;
       internal DalApi.IDal dalObject;

        //Create a Random object to be used to draw the battery status and Location of the drones.
        Random random = new Random(DateTime.Now.Millisecond);




        #region singelton


         static readonly IBL instance = new BL();

        static BL() { }

        public static IBL Instance { get => instance; }

        #endregion

        #region constractor
         BL()
        {
            //Creates an object that will serve as an access point to methods in DAL.
            dalObject = DalFactory.GetDal();

            double[] arrDrone = dalObject.RequestPowerbyDrone();
            Available = arrDrone[0];
            LightWeightCarrier = arrDrone[1];
            MediumWeightCarrier = arrDrone[2];
            HeavyWeightCarrier = arrDrone[3];
            DroneChargingRate = arrDrone[4];

            // Convert a layer dronelist to a logical layer drone list
            IEnumerable<DO.Drone> dronesDal = dalObject.GetDroneList().ToList();
            dronesListBL = (from item in dronesDal
                            select new DroneToList
                            {
                                Id = item.Id,
                                Model = item.Model,
                                MaxWeight = (WeightCategories)item.MaxWeight
                            }).ToList();

            //creating a customer list 

            IEnumerable<DO.Customer> customerDal = dalObject.GetCustomerList().ToList();
List<Customer> customerListBL = (from item in customerDal
                                             select new Customer()
                                             {
                                                 Id = item.Id,
                                                 Name = item.Name,
                                                 PhoneNumber = item.PhoneNumber,
                                                 CustomerLocation = new Location() { Longitude = item.Longitude, Latitude = item.Latitude },
                                             }).ToList();
            //creating a basestationlist list

            IEnumerable<DO.BaseStation> baseStationDal = dalObject.GetBaseStationList().ToList();
            List<BaseStation> baseStationBL = (from item in baseStationDal
                                                     select new BaseStation()
                                                     {
                                                         Id = item.Id,
                                                         Name = item.Name,
                                                         FreeChargeSlots = item.ChargeSlots,
                                                         BaseStationLocation = new Location() { Longitude = item.Longitude, Latitude = item.Latitude },
                                                         DroneChargingList = new List<DroneCharging>()
                                                     }).ToList();


            //creating a parcel list of parcel fro the data layer that were set to a drone already
           List<DO.Parcel> parcelListDal = dalObject.GetParcelList(x => x.DroneId != 0).ToList();




            foreach (var item in dronesListBL)
            {
                
                int index = parcelListDal.FindIndex(x => x.DroneId == item.Id && x.Delivered == null);
                if (index != -1)
                {
                    item.Status = DroneStatuses.Busy;//if the parcel was set to a drone but wasnt delivered so the drone status is bust
                    Location locationSender = customerListBL.First(x => x.Id == parcelListDal[index].SenderId).CustomerLocation;//creating the location of the sender of the parcel that was set to a drond but not delivered yet
                                                                                                                               //Distance between sender and receiver.
                    Location locationReceiver = customerListBL.First(x => x.Id == parcelListDal[index].TargetId).CustomerLocation;
                    double distanceBetweenSenderAndReceiver = GetDistance(locationSender, locationReceiver);

                    //Distance between the receiver and the nearest base station.
                    double DistanceBetweenReceiverAndNearestBaseStation = mindistanceBetweenLocationBaseStation(baseStationBL, locationReceiver).Item2;
                    int electricityUse = (int)(CalculateElectricityUse((WeightCategories)parcelListDal[index].Weight, distanceBetweenSenderAndReceiver, DistanceBetweenReceiverAndNearestBaseStation));

                    if (parcelListDal[index].PickUp == null)//if the parcel wasnt pickedup yet
                    {
                        item.DroneLocation = mindistanceBetweenLocationBaseStation(baseStationBL, locationSender).Item1;
                        electricityUse +=(int)( GetDistance(item.DroneLocation, locationSender) * Available); //
                    }
                    else
                    {
                        item.DroneLocation = locationSender;
                    }
                    // random number battery status between minimum charge to make the shipment and full charge.     
                    item.Battery =(int)( ((float)((float)random.NextDouble() * (100 - electricityUse)) + electricityUse));
                    item.NumParcelTransfer = parcelListDal[index].Id;

                }
                else//if the drone isnt busy
                {
                    item.Status = (DroneStatuses)random.Next(0, 2);
                    if (item.Status == DroneStatuses.InMaintenance)
                    {
                        BaseStation baseStation = baseStationBL[random.Next(0, baseStationBL.Count)];
                        item.DroneLocation = baseStation.BaseStationLocation;
                        dalObject.SendDroneToCharge(item.Id, baseStation.Id);
                        dalObject.LessChargeSlots(baseStation.Id);
                        item.Battery = random.Next(0, 21);
                    }
                    if (item.Status == DroneStatuses.Free)
                    {
                        List<DO.Parcel> DeliveredByThisDroneparcelList = parcelListDal.FindAll(x => x.DroneId == item.Id && x.Delivered != null);
                        if (DeliveredByThisDroneparcelList.Any())
                        {
                            int TargetId = DeliveredByThisDroneparcelList[random.Next(0, DeliveredByThisDroneparcelList.Count)].TargetId;
                            item.DroneLocation = customerListBL.First(x => x.Id == TargetId).CustomerLocation;
                            int batteryUse =(int) (mindistanceBetweenLocationBaseStation(baseStationBL, item.DroneLocation).Item2 * Available);
                            item.Battery =(int) ((float)((float)(random.NextDouble() * (100 - batteryUse)) + batteryUse));

                        }
                        else //if the List is empty.
                        {
                            item.DroneLocation = baseStationBL[random.Next(0, baseStationBL.Count)].BaseStationLocation;
                            item.Battery = random.Next(0, 101);
                        }

                    }


                }
            }




        }
        #endregion constractor
    }


}
