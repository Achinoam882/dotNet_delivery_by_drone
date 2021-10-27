using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IDAL
{
    namespace DO
    {
        public struct Parcel
        {
            public int Id { get; set; }

            public int SenderId { get; set; }

            public int TargetId { get; set; }

            public WeightCategories Weight { get; set; }

            public Priorities Priority { get; set; }

            public int DroneId { get; set; }

            public DateTime Scheduled { get; set; }

            public DateTime PickUp { get; set; }

            public DateTime Delivered { get; set; }

            public DateTime Requested { get; set; }
            public override string ToString()
            {
                return string.Format("Id is:{0,-14}\tSender Id is:{1,-14}\tTarget Id is:{2,-14}\t " +
                    " Drone Weight is:{3,-14}\tPriority is:{4,-14}\tDrone Id is:{5,-14}\tScheduled time is:{6,-14}\t " +
                    " PickUp time is:{7,-14}\tDelivered time is:{8,-14}\tRequested time is:{9,-14}\t", Id, SenderId,
                    TargetId, Weight, Priority, DroneId, Scheduled, PickUp, Delivered, Requested);
            }

        }
    }
}
