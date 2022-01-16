using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace DO
    {
       public struct BaseStation
        {
            public int Id { get; set; } //baseStation Id

            public string Name { get; set; } //Base Station Name

            public double Longitude { get; set; }

            public double Latitude { get; set; }

            public int ChargeSlots { get; set; }//Num of  Free charg slots

          
            public override string ToString()
            {
                return this.ToStringProperty();
            }

        }
    }

