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
              public int Id { get; set; } //Drone Id
              public string Model { get; set; } //Drone name(model)
              public WeightCategories MaxWeight { get; set; }//light,medium,heavy
               public override string ToString()
               {
                 return this.ToStringProperty();
               }
 
        }
    }

