using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;
using DalObject;
namespace Dal
{

    /// <summary>
    /// matods that use from the main 
    /// </summary>
    sealed partial class DalObject :IDal

    {

        #region singelton


        static readonly IDal instance = new DalObject();
        
        static DalObject() { }
       private DalObject()
        {
            DataSource.Initialize();
        }
        static IDal Instance { get {return instance;} }

        #endregion


        #region Request Power by Drone
        /// <summary>
        /// The funcation return an arr of weight carrier of parcel
        /// </summary>
        public double[] RequestPowerbyDrone()
        {
            double[] temp = { DataSource.Config.Available, DataSource.Config.LightWeightCarrier, 
            DataSource.Config.MediumWeightCarrier, DataSource.Config.HeavyWeightCarrier, 
            DataSource.Config.DroneChargingRate };
            return temp;
        }
        #endregion








    }
}
