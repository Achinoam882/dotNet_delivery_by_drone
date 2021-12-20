﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;
namespace DalObject
{

    /// <summary>
    /// matods that use from the main 
    /// </summary>
    sealed partial class DalObject :IDal

    {

        #region singelton


        internal static readonly IDal instance = new DalObject();
        
        static DalObject() { }
        DalObject()
        {
            DataSource.Initialize();
        }
        public static IDal Instance { get {return instance;} }

        #endregion

        /// <summary>
        /// Default constructor.
        /// </summary>
       
        public double[] RequestPowerbyDrone()
        {
            double[] temp = { DataSource.Config.Available, DataSource.Config.LightWeightCarrier, 
            DataSource.Config.MediumWeightCarrier, DataSource.Config.HeavyWeightCarrier, 
            DataSource.Config.DroneChargingRate };
            return temp;
        }
        








    }
}
