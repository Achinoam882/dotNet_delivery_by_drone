﻿using System;
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

        public static double Available ;
        public static double LightWeightCarrier ;
        public static double MediumWeightCarrier ;
        public static double HeavyWeightCarrier ;
        public static double DroneChargingRate ;

        public List<DroneToList> dronesListBL;
        DalApi.IDal dalObject;

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
            dalObject = DalFactory.GetDal("DalObject");

            double[] arrDrone = dalObject.RequestPowerbyDrone();
            Available = arrDrone[0];
            LightWeightCarrier = arrDrone[1];
            MediumWeightCarrier = arrDrone[2];
            HeavyWeightCarrier = arrDrone[3];
            DroneChargingRate = arrDrone[4];

            // Convert a layer dronelist to a logical layer drone list
            List<DO.Drone> dronesDal = dalObject.GetDroneList().ToList();
            dronesListBL = new List<DroneToList>();
            foreach (var item in dronesDal)
            {
                dronesListBL.Add(new DroneToList { Id = item.Id, Model = item.Model,
                    MaxWeight = (WeightCategories)item.MaxWeight
                });
            }

            //creating a customer list 
            List<Customer> customerListBL = new List<Customer>();
            List<DO.Customer> customerDal = dalObject.GetCustomerList().ToList();
            foreach (var item in customerDal)
            {
                customerListBL.Add(new Customer
                {
                    Id = item.Id,
                    Name = item.Name,
                    PhoneNumber = item.PhoneNumber,
                    CustomerLocation = new Location() { Longitude = item.Longitude, Latitude = item.Latitude },
                });
            }
            //creating a basestationlist list
            List<BaseStation> baseStationBL = new List<BaseStation>();
            List<DO.BaseStation> baseStationDal = dalObject.GetBaseStationList().ToList();
            foreach (var item in baseStationDal)
            {
               
                baseStationBL.Add(new BaseStation
                {
                    Id = item.Id,
                    Name = item.Name,
                    FreeChargeSlots = item.ChargeSlots,
                    BaseStationLocation = new Location() { Longitude = item.Longitude, Latitude = item.Latitude },
                    DroneChargingList = new List<DroneCharging>()
                });
            }

            //creating a parcel list of parcel fro the data layer that were set to a drone already
            List<DO.Parcel> parcelListDal = dalObject.GetParcelList(x => x.DroneId != 0).ToList();




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
                        BaseStation baseStation = baseStationBL[random.Next(0, baseStationBL.Count)];
                        item.DroneLocation = baseStation.BaseStationLocation;
                        dalObject.SendDroneToCharge(item.Id, baseStation.Id);
                        dalObject.LessChargeSlots(baseStation.Id);
                        item.Battery = random.Next(0, 21);
                    }
                    if (item.Status == DroneStatuses.Free)
                    {
                        List<DO.Parcel> parcelListDeliveredByThisDrone = parcelListDal.FindAll(x => x.DroneId == item.Id && x.Delivered != DateTime.MinValue);
                        if (parcelListDeliveredByThisDrone.Any())
                        {
                           
                            item.DroneLocation = customerListBL.Find(x => x.Id == parcelListDeliveredByThisDrone[random.Next(0, parcelListDeliveredByThisDrone.Count)].TargetId).CustomerLocation;
                            double batteryUse = mindistanceBetweenLocationBaseStation(baseStationBL, item.DroneLocation).Item2 * Available;
                            item.Battery = (float)((float)(random.NextDouble() * (100 - batteryUse)) + batteryUse);

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
