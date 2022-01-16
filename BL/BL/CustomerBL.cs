using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BL
{
    public partial class BL
    {
        #region  add customer
        /// <summary>
        /// The function adds a customer to the list of customers.
        /// </summary>
        /// <param name="newCustomer"></param>
        public void SetCustomer(Customer newCustomer)
        {

            //exception of the logical layer//name,phonenumber???
            if (newCustomer.Id < 0)
                throw new AddingProblemException(" ID cant be a negative number");
            if (newCustomer.CustomerLocation.Longitude < 34.3 || newCustomer.CustomerLocation.Latitude > 35.5)
                throw new AddingProblemException("the location that was chosen isnt in the country");

            DO.Customer CustomerDal = new DO.Customer()
            {
                Id = newCustomer.Id,
                Name = newCustomer.Name,
                PhoneNumber = newCustomer.PhoneNumber,
                Longitude = newCustomer.CustomerLocation.Longitude,
                Latitude = newCustomer.CustomerLocation.Latitude,

            };
            try
            {
                dalObject.SetCustomer(CustomerDal);
            }
            catch (DO.AddExistingObjectException )
            {
                throw new AddingProblemException("ID already exists");

            }
        }
        #endregion  add customer


        #region Update Customer
        /// <summary>
        /// The function update name and phone number customer.
        /// </summary>
       
        public void UpdateCustomer(int customerId, string customerName, string phoneNumber)
        {
            try
            {
                DO.Customer newCustomer = dalObject.GetCustomer(customerId);
                if (customerName != "")
                    newCustomer.Name = customerName;
                if (phoneNumber != "")
                    newCustomer.PhoneNumber = phoneNumber;
                dalObject.UpDateCustomer(newCustomer);
            }
            catch (DO.NonExistingObjectException )
            {
                throw new UpdateProblemException("ID  doesnt exists in the system");
            }
        }

        #endregion Update Customer


        #region display customer
        /// <summary>
        /// The function return an exists customer .
        /// </summary>
        /// <param name="idForDisplayCustomer"> id of customer</param>

        public Customer GetCustomer(int idForDisplayCustomer)
        {
            DO.Customer dalCustomer = new DO.Customer();
            try
            {
                dalCustomer = dalObject.GetCustomer(idForDisplayCustomer);
            }
            catch (DO.NonExistingObjectException)
            {
                throw new GetDetailsProblemException("ID customer doesnt exists in the system");
            }
            // Location dalocation = new Location() { Latitude = dalCustomer.Latitude, Longitude = dalCustomer.Longitude };
            Customer DisPlayCustomer = new Customer()
            {
                Id = dalCustomer.Id,
                Name = dalCustomer.Name,
                PhoneNumber = dalCustomer.PhoneNumber,
                CustomerLocation = new Location() { Latitude = dalCustomer.Latitude, Longitude = dalCustomer.Longitude },
                // ParcelFromCustomer = new List<ParcelAtCustomer>(),
                //ParcelToCustomer = new List<ParcelAtCustomer>(),
            };
            IEnumerable<DO.Parcel> dalParcelList = dalObject.GetParcelList(i => i.SenderId == idForDisplayCustomer);
            DisPlayCustomer.ParcelFromCustomer = (from item in dalParcelList
                                                 select new ParcelAtCustomer()
                                                 {
                                                     Id = item.Id,
                                                     Priority = (Priorities)item.Priority,
                                                     Weight = (WeightCategories)item.Weight,
                                                     Status = item.Delivered != null ? ParcelStatus.Delivered : item.PickUp != null ? ParcelStatus.PickUp :
                                                     item.Scheduled != null ? ParcelStatus.Scheduled : ParcelStatus.Requested,
                                                     OtherSide = new CustomerParcel() { Id = item.TargetId, Name = dalObject.GetCustomer(item.TargetId).Name },


                                                 }).ToList();
            IEnumerable<DO.Parcel> dalSentParcelList = dalObject.GetParcelList(i => i.TargetId == idForDisplayCustomer);
            DisPlayCustomer.ParcelToCustomer = (from item in dalSentParcelList
                                               select new ParcelAtCustomer()
                                               {
                                                   Id = item.Id,
                                                   Priority = (Priorities)item.Priority,
                                                   Weight = (WeightCategories)item.Weight,
                                                   Status = item.Delivered != null ? ParcelStatus.Delivered : item.PickUp != null ? ParcelStatus.PickUp :
                                                     item.Scheduled != null ? ParcelStatus.Scheduled : ParcelStatus.Requested,
                                                   OtherSide = new CustomerParcel() { Id = item.SenderId, Name = dalObject.GetCustomer(item.SenderId).Name },
                                               }).ToList();
            return DisPlayCustomer;

        }
        #endregion display customer


        #region display customer list
        /// <summary>
        /// The function return list of customers .
        /// </summary>
        
        // run on all the customer list and put the correct info into  
        public IEnumerable<CustomerToList> GetCustomerList(Predicate<CustomerToList> predicate = null)
            {
            
            IEnumerable<DO.Customer> DalCustomers = dalObject.GetCustomerList();
                IEnumerable<CustomerToList> BLcustomers = from item in DalCustomers
                                                          select new CustomerToList()
                                                          {
                                                              Id = item.Id,
                                                              Name = item.Name,
                                                              PhoneNumber = item.PhoneNumber,
                                                              ParcelProvided = dalObject.GetParcelList(x => x.Delivered != null && x.SenderId == item.Id).Count(),
                                                              Parcelsnet = dalObject.GetParcelList(x => x.PickUp != null && x.Delivered == null && x.SenderId == item.Id).Count(),
                                                              ParcelReceived = dalObject.GetParcelList(x => x.Delivered != null && x.TargetId == item.Id).ToList().Count(),
                                                              ParcelOnTheWay = dalObject.GetParcelList(x => x.PickUp != null && x.Delivered == null && x.TargetId == item.Id).Count(),
                                                          };
                
          

            return BLcustomers.Where(x => predicate == null ? true : predicate(x));
            }
        #endregion display customer list
    }
}
