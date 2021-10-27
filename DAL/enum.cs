using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public enum Priorities { regular, fast, urgent}
        public enum WeightCategories { light,medium,heavy}
        public enum DroneStatuses { free,inMaintenance,busy}
        public enum Choice { add, update, display, viewLists, exit }

        public enum Add { basestation,drone,newcustomer, parcelDelivery}
        public enum Update {parcelToDrone, pickUp, deliverd, incharging, outcharging }
        public enum Display {viewBaseStation,viewDrone,viewCustomers,viewPackage}
        public enum DisplayLists {ListOfBaseStation, ListOfDroned, ListOfCustomer,
            ListOfPackage, ListOfFreePackage, ListOfBaseStationsWithFreeChargSlots
        }
    }
}
