using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace DO
        
    {
        public struct Customer
        {
            public int Id { get; set; } //customer Id

            public string Name { get; set; } //Customer Name

            public string PhoneNumber { get; set; } //Phone number of customer

            public double Longitude { get; set; }

            public double Latitude { get; set; }
           
            public override string ToString()
            {
                return this.ToStringProperty();
            }

        }
    }

