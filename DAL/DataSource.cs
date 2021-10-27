using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using System.ComponentModel;

namespace DalObject
{
    public static  class  DataSource
    {
       
        internal static Random r = new Random();
        internal static List<Drone> droneList = new List<Drone>();
        internal static List<BaseStation> baseStationsList= new List<BaseStation>();
        internal static List<Customer> customersList = new List<Customer>();
        internal static List<Parcel> parcelsList = new List<Parcel>();
        internal static List<DroneCharge> droneChargeList = new List<DroneCharge>();

        internal class Config
        {
            public static int CountIdPackage = 1;
        }

        public static void Initialize()
        {
            baseStationsList.Add(new BaseStation()
            {
                Id = r.Next(100000000, 999999999),
                Name = "Eilat",
                Longitude = 32.021679,
                Latitude = 34.789990
            });

            baseStationsList.Add(new BaseStation()
            {
                Id = r.Next(100000000, 999999999),
                Name = "Tel Aviv",
                Longitude = 35.021679,
                Latitude = 36.789990,
                ChargeSlots = r.Next(5, 10)
            });
            string[] ModelArr = new string[5] { "GS18", "TS60", "P50", "AS81", "BC55" };
            for (int i = 0; i < 5; i++)
            {
                droneList.Add(new Drone()
                {
                    Id = r.Next(100000000, 999999999),
                    Model = ModelArr[i],
                    MaxWeight = (WeightCategories)r.Next(0, 3),
                    Status = (DroneStatuses)r.Next(0, 3),
                    Battery = r.Next(0, 100),
                });
            }
            string[] CustomerNameArr = new string[10] { "Yaron", "Moshe", "Malka", "Daniel", "Joey", "David",
                "Tamar", "Rafael", "Michael", "Reena" };
            for (int i = 0; i < 10; i++)
            {
                customersList.Add(new Customer()
                {
                    Id = r.Next(100000000, 999999999),
                    Name = CustomerNameArr[i],
                    PhoneNumber = "0" + r.Next(50, 60) + "-" + r.Next(0000000, 9999999),
                    Longitude = r.Next(31, 36) + r.NextDouble(),
                    Latitude = r.Next(31, 36) + r.NextDouble()
                });
            }
            for (int i = 0; i < 10; i++)
            {
                parcelsList.Add(new Parcel()
                {
                    Id = Config.CountIdPackage++,
                    SenderId = customersList[r.Next(0, 5)].Id,
                    TargetId = customersList[r.Next(5, 10)].Id,
                    Weight = (WeightCategories)r.Next(0, 3),
                    Priority = (Priorities)r.Next(0, 3),
                    DroneId = 0,
                    Requested = new DateTime(2021, 6, r.Next(1, 7))
                });
            }
        }
    }
    
}
