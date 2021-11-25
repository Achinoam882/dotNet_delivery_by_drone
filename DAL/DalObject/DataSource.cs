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
        #region lists of structs
        /// <summary> A static Random that sets the random to select a millisecond to repel collisions </summary>
        internal static Random R  = new Random(DateTime.Now.Millisecond);
        /// <summary> list of drones </summary>
        internal static List< Drone> DroneList = new List<Drone>();
        /// <summary>list of customers</summary>
        internal static List<Customer> CustomerList = new List<Customer>();
        /// <summary>list of Base Station </summary>
        internal static List<BaseStation> BaseStationList = new List<BaseStation>();
        /// <summary> list of parcels </summary>
        internal static List<Parcel> ParcelList = new List<Parcel>();
        /// <summary> list of drone Charge  </summary>
        internal static List<DroneCharge> DroneChargeList = new List<DroneCharge>();
        #endregion lists of structs

        internal class Config
        {
            public static int IdParcel = 1;
            public static double Available=2.00;
            public static double LightWeightCarrier=3.00;
            public static double MediumWeightCarrier=4.00;
            public static double HeavyWeightCarrier=5.000;
            public static double DroneChargingRate=2.5;

        }
       
        public static void Initialize()
        {
            #region base station initialization
            //initialization of 2 base station in two random cities
            BaseStationList.Add(new BaseStation()
            {

                Id = /*R.Next(100000000, 999999999),*/ 10,
                Name = "Eilat",
                Latitude = 35.789900,
                Longitude = 33.268437,
                ChargeSlots = /*R.Next(5, 10)*/6
            });
            BaseStationList.Add(new BaseStation()
            { 
                    Id = /*R.Next(100000000, 999999999)*/11,
                    Name = "Tel Aviv",
                    Latitude = 35.239900,
                    Longitude = 33.678437,
                    ChargeSlots = /*R.Next(5, 10)*/ 10
             });
            #endregion base station initialization


            #region drone initialization
            //initialization of 5 Drones with different and random values.
          
            string[] ModelArr = new string[5] { "GS18", "TS60", "P50", "AS81", "BC55" };
                for(int i=0;i<5; i++)
                {
                DroneList.Add( new Drone()
                    {
                        Id = R.Next(100000000, 999999999),
                        Model = ModelArr[i],
                        MaxWeight = (WeightCategories)R.Next(0,3),//0=light,1=medium,2=heavy
                   

                    });

                }
            #endregion drone initialization


            #region customer initialization
            //initialization of 10 customers with different and random values.

            string[] CustomerNameArr = new string[10] { "Yaron", "Moshe", "Malka", "Daniel", "Joey", "David", "Tamar", "Rafael", "Michael", "Reena" };
                for (int i = 0; i < 10; i++)
                {
                CustomerList.Add(new Customer()
                    {
                        Id = R.Next(100000000, 999999999),
                        Name = CustomerNameArr[i],
                        PhoneNumber="0"+R.Next(50,60)+"-"+ R.Next(0000000,9999999),
                        Longitude= R.Next(31,36)+R.NextDouble(),
                        Latitude = R.Next(31, 36) + R.NextDouble(),
                    });

                }
            #endregion customer initialization


            #region parcel initialization
            //initialization of 10 parcels with different and random values.

            for (int i = 0; i < 10; i++)
                {
                ParcelList.Add(new Parcel()
                {
                    Id = Config.IdParcel++,
                    SenderId = CustomerList[R.Next(0, 5)].Id,
                    TargetId = CustomerList[R.Next(5, 10)].Id,
                    Weight = (WeightCategories)R.Next(0, 3),
                    Priority = (Priorities)R.Next(0, 3),
                    DroneId = 0,


                //r.Next(1000,9999),
                Requested =  DateTime.Now
                });
                }
            #endregion parcel initialization



        }

    }
}
