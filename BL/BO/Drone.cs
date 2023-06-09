﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Drone
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public WeightCategories MaxWeight { get; set; }//light,medium,heavy
        public  DroneStatuses  Status { get; set; } //free,inMaintenance,busy
        public double Battery { get; set; }
        public ParcelInTransfer DroneParcel { get; set; }
        public Location DroneLocation { get; set; }
        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}
