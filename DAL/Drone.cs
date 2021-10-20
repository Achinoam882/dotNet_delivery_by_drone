using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using IDAL.DO;
namespace IDAL
{
    namespace DO
    {
          public struct Drone
          {
              public int Id { get; set; }
              public string Model { get; set; }
              public WeightCategories MaxWeight { get; set; }
              public  DroneStatuses  Status { get; set; }
              public double Battery { get; set; }
              public override string ToString()
              {
                return string.Format ("Id is:{0}\t  Model of the drone is:{1}\t   MaxWeight is:{2}\t " +
                    " Drone status is:{3}\t  Battery level is:{4}\t",Id, Model, MaxWeight, Status, Battery);
              }

        }
    }

}