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
                return string.Format("Id is:{0}\t Sender Id is:{1}\t  Target Id is:{2}\t " +
                    " Drone Weight is:{3}\t  Priority is:{4}\t  Drone Id is:{5}\t  Scheduled time is:{6}\t " +
                    " PickUp time is:{7}\t  Delivered time is:{8}\t  Requested time is:\t"  , Id, SenderId,
                    TargetId, Weight, Priority, DroneId, Scheduled, PickUp, Delivered, Requested);
            }

        }
    }
}
