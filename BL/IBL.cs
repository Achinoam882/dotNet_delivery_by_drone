using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
   public interface IBL
    {

        public  BaseStation GetBaseStation(int idForDisplayBaseStation);
        public Drone GetDrone(int idForDisplayDrone);
        public Customer GetCustomer(int idForDisplayObject);
        public Customer GetParcel(int idForDisplayObject);
        public IEnumerable<CustomerToList> GetCustomerList(Predicate<CustomerToList> predicate = null);
        
    }
}
