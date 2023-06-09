﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;
using Dal;
using System.ComponentModel;
using DalObject;

namespace Dal
{
    /// <summary>
    /// Contains boot data and list structure
    /// </summary>
      static class DataSource
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
        public static List<User> Users;
        #endregion lists of structs


       
        internal class Config
        {
            public static double Available = 0.5;
            public static double LightWeightCarrier = 0.6;
            public static double MediumWeightCarrier = 0.7;
            public static double HeavyWeightCarrier = 0.800;
            public static double DroneChargingRate = 300;
            public static int IdParcel = 1;

        }

        public static void Initialize()
        {
            #region User initialization
            /// <summary>
            /// Add an users to the list
            /// </summary>
            Users = new List<User>
            {
                new User{Id=209624931,AllowingAccess=true,DelUser=false,UserName="m",Salt=12122,HashedPassword= Tools.hashPassword(12122+"3")},
                new User{Id=206464067,AllowingAccess=true,DelUser=false,UserName="Achinoam",Salt=1234213,HashedPassword= Tools.hashPassword(1234213+"Achinoam123")},
                new User{Id=0,AllowingAccess=false,DelUser=false,UserName="mosait",Salt=12342,HashedPassword= Tools.hashPassword(12342+"mo123")}
            };
            #endregion User initialization
            #region base station initialization
            //initialization of 2 base station in two random cities
            BaseStationList.Add(new BaseStation()
            {

                Id = /*R.Next(100000000, 999999999),*/ 10,
                Name = "beit shemesh",
                Latitude = 31.78272,
                Longitude = 35.18722,
                ChargeSlots = /*R.Next(5, 10)*/6
            });
            BaseStationList.Add(new BaseStation()
            { 
                    Id = /*R.Next(100000000, 999999999)*/11,
                    Name = "jerusalem",
                    Latitude = 31.783333,
                    Longitude = 35.316667,
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
                    Latitude  =  (float)((float)(R.NextDouble()*(31.783333 - 31.78272))+ 31.78272),
                    Longitude = (float)((float)(R.NextDouble() * (35.316667 - 35.18722)) + 35.18722)
                    

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
