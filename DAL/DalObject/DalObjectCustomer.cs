using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject :IDal
    {
        #region  add customer
        /// <summary>
        /// The function adds a customer to the list of customers.
        /// </summary>
        /// <param name="newCustomer"></param>
        public void SetCustomer(Customer newCustomer)
        {
            if (DataSource.CustomerList.FindIndex(x => x.Id == newCustomer.Id) != -1)
            {
                throw new AddExistingObjectException();
            }
            DataSource.CustomerList.Add(newCustomer);
        }
        #endregion  add customer

        #region print customer
        /// <summary>
        /// The function returns the selected Customer.
        /// </summary>
        /// <param name="idForAllObjects">Id of a selected Customer</param>
        /// <returns>return empty ubjact if its not there</returns>
        public Customer GetCustomer(int idForAllObjects)
        {
            if (!(DataSource.CustomerList.Exists(x => x.Id == idForAllObjects)))
            {
                throw new NonExistingObjectException();
            }
            return DataSource.CustomerList.Find(x => x.Id == idForAllObjects);
        }
        #endregion print customer

        #region customer list
        /// <summary>
        /// The function returns an array of all Customer.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public IEnumerable<Customer> GetCustomerList()
        {
            return DataSource.CustomerList.Take(DataSource.CustomerList.Count).ToList();
        }
        #endregion customer list

    }
}
