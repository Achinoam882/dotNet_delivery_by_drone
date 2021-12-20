using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using IDAL.DO;
namespace DalObject
{ 

    /// <summary>
    /// matods that use from the main 
    /// </summary>
    public partial class DalObject :IDal
    {

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DalObject()
        {
            DataSource.Initialize();
        }
        public double[] RequestPowerbyDrone()
        {
            double[] temp = { DataSource.Config.Available, DataSource.Config.LightWeightCarrier, 
            DataSource.Config.MediumWeightCarrier, DataSource.Config.HeavyWeightCarrier, 
            DataSource.Config.DroneChargingRate };
            return temp;
        }
       







    }
}
