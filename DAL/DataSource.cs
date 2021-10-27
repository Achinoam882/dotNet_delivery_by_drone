using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using System.ComponentModel;

namespace DalObject
{
    /// <summary>
    /// Contains boot data and list structure
    /// </summary>
    public static class DataSource
    {
        /// <summary> A static Random that sets the random to select a millisecond to repel collisions </summary>
        internal static Random r = new Random();
        /// <summary> list of drones </summary>
        internal static List< Drone> droneList = new List<Drone>();
        /// <summary>list of customers</summary>
        internal static List<Customer> CustomerList = new List<Customer>();
        /// <summary>list of Base Station </summary>
        internal static List<BaseStation> BaseStationList = new List<BaseStation>();
        /// <summary> list of parcels </summary>
        internal static List<Parcel> ParcelList = new List<Parcel>();
        /// <summary> list of drone Charge  </summary>
        internal static List<DroneCharge> droneChargeList = new List<DroneCharge>();


        internal class Config
        {
            public static int IdParcel = 1;
        }
       
        public static void Initialize()
        {
            //initialization of 2 base station in two random cities
            BaseStationList.Add(new BaseStation()
            {

                Id = r.Next(100000000, 999999999),
                Name = "Eilat",
                Latitude = 35.789900,
                Longitude = 33.268437,
                ChargeSlots = r.Next(5, 10)
            });
            BaseStationList.Add(new BaseStation()
            {

                    Id = r.Next(100000000, 999999999),
                    Name = "Tel Aviv",
                    Latitude = 35.239900,
                    Longitude = 33.678437,
                    ChargeSlots = r.Next(5, 10)
             });
            //initialization of 5 Drones with different and random values.
            string[] ModelArr = new string[5] { "GS18", "TS60", "P50", "AS81", "BC55" };
                for(int i=0;i<5; i++)
                {
                droneList.Add( new Drone()
                    {
                        Id = r.Next(100000000, 999999999),
                        Model = ModelArr[i],
                        MaxWeight = (WeightCategories)r.Next(0,3),//0=light,1=medium,2=heavy
                    Status = (DroneStatuses)r.Next(0, 3),//0=free, 1=inMaintenance, 2=busy
                    Battery = r.Next(0,100),

                    });

                }
            //initialization of 10 customers with different and random values.
            string[] CustomerNameArr = new string[10] { "Yaron", "Moshe", "Malka", "Daniel", "Joey", "David", "Tamar", "Rafael", "Michael", "Reena" };
                for (int i = 0; i < 10; i++)
                {
                CustomerList.Add(new Customer()
                    {
                        Id = r.Next(100000000, 999999999),
                        Name = CustomerNameArr[i],
                        PhoneNumber="0"+r.Next(50,60)+"-"+ r.Next(0000000,9999999),
                        Longitude= r.Next(31,36)+r.NextDouble(),
                        Latitude = r.Next(31, 36) + r.NextDouble(),
                    });

                }
            //initialization of 10 parcels with different and random values.
            for (int i = 0; i < 10; i++)
                {
                ParcelList.Add(new Parcel()
                {
                    Id = Config.IdParcel++,
                    SenderId = CustomerList[r.Next(0, 5)].Id,
                    TargetId = CustomerList[r.Next(5, 10)].Id,
                    Weight = (WeightCategories)r.Next(0, 3),
                    Priority = (Priorities)r.Next(0, 3),
                    DroneId = 0,


                //r.Next(1000,9999),
                Requested = new DateTime(2021, 7, r.Next(1, 7))
                });
                }



         }
        
    }
}
