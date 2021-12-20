using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using IDAL.DO;

    namespace DO
    {
          public struct Drone
          {
              public int Id { get; set; }
              public string Model { get; set; }
              public WeightCategories MaxWeight { get; set; }//light,medium,heavy
           // public  DroneStatuses  StNatus { get; set; } //free,inMaintenance,busy
           // public double Battery { get; set; }
             //public override string ToString()
             // {
             //   return string.Format ("Id is:{0,-14}\t Model of the drone is:{1,-14}\t MaxWeight is:{2,-14}\t" +
             //       "/*/* Drone status is:{3,-14}\t*/ Battery level is:{4,-14}\t*/",Id, Model, MaxWeight/*, Status, Battery*/);
             // }
               public override string ToString()
               {
                 return this.ToStringProperty();
               }
 
        }
    }

