using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
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

            IDAL.DO.Customer CustomerDal = new IDAL.DO.Customer()
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
            catch (IDAL.DO.AddExistingObjectException )
            {
                throw new AddingProblemException("ID already exists");

            }
        }
        #endregion  add customer


        #region Update Customer

        public void UpdateCustomer(int customerId, string customerName, string phoneNumber)
        {
            try
            {
                IDAL.DO.Customer newCustomer = dalObject.GetCustomer(customerId);
                if (customerName != "")
                    newCustomer.Name = customerName;
                if (phoneNumber != "")
                    newCustomer.PhoneNumber = phoneNumber;
                dalObject.UpDateCustomer(newCustomer);
            }
            catch (IDAL.DO.NonExistingObjectException )
            {
                throw new UpdateProblemException("ID  doesnt exists in the system");
            }
        }

        #endregion Update Customer


        #region display customer
        public Customer GetCustomer(int idForDisplayCustomer)
        {
            IDAL.DO.Customer dalCustomer = new IDAL.DO.Customer();
            try
            {
                dalCustomer = dalObject.GetCustomer(idForDisplayCustomer);
            }
            catch (IDAL.DO.NonExistingObjectException)
            {
                throw new GetDetailsProblemException("ID customer doesnt exists in the system");
            }
            Location dalocation = new Location() { Latitude = dalCustomer.Latitude, Longitude = dalCustomer.Longitude };
            Customer DisPlayCustomer = new Customer()
            {
                Id = dalCustomer.Id,
                Name = dalCustomer.Name,
                PhoneNumber = dalCustomer.PhoneNumber,
                CustomerLocation = dalocation,
                ParcelFromCustomer = new List<ParcelAtCustomer>(),
                ParcelToCustomer = new List<ParcelAtCustomer>(),
            };
            List<IDAL.DO.Parcel> dalParcelList = dalObject.GetParcelList(i => i.SenderId == idForDisplayCustomer).ToList();
            foreach (var item in dalParcelList)
            {
                CustomerParcel customerParcel = new CustomerParcel() { Id = item.TargetId, Name = dalObject.GetCustomer(item.TargetId).Name };
                ParcelAtCustomer parcelAtCustomer = new ParcelAtCustomer()
                {
                    Id = item.Id,
                    Priority = (Priorities)item.Priority,
                    Weight = (WeightCategories)item.Weight,
                    OtherSide = customerParcel
                };
                if (item.Delivered != DateTime.MinValue)
                    parcelAtCustomer.Status = ParcelStatus.Delivered;
                else if (item.PickUp != DateTime.MinValue)
                    parcelAtCustomer.Status = ParcelStatus.PickUp;
                else if (item.Scheduled != DateTime.MinValue)
                    parcelAtCustomer.Status = ParcelStatus.Scheduled;
                else
                    parcelAtCustomer.Status = ParcelStatus.Requested;

                DisPlayCustomer.ParcelFromCustomer.Add(parcelAtCustomer);

            }
            List<IDAL.DO.Parcel> dalSentParcelList = dalObject.GetParcelList(i => i.TargetId == idForDisplayCustomer).ToList();
            foreach (var item in dalSentParcelList)
            {
                CustomerParcel customerParcel = new CustomerParcel() { Id = item.SenderId, Name = dalObject.GetCustomer(item.SenderId).Name };
                ParcelAtCustomer parcelAtCustomer = new ParcelAtCustomer()
                {
                    Id = item.Id,
                    Priority = (Priorities)item.Priority,
                    Weight = (WeightCategories)item.Weight,
                    OtherSide = customerParcel,
                };
                if (item.Delivered != DateTime.MinValue)
                    parcelAtCustomer.Status = ParcelStatus.Delivered;
                else if (item.PickUp != DateTime.MinValue)
                    parcelAtCustomer.Status = ParcelStatus.PickUp;
                else if (item.Scheduled != DateTime.MinValue)
                    parcelAtCustomer.Status = ParcelStatus.Scheduled;
                else
                    parcelAtCustomer.Status = ParcelStatus.Requested;

                DisPlayCustomer.ParcelToCustomer.Add(parcelAtCustomer);

            }
            return DisPlayCustomer;
        }
        #endregion display customer


        #region display customer list
        public IEnumerable<CustomerToList> GetCustomerList(Predicate<CustomerToList> predicate = null)
        {
            List<CustomerToList> customerBL = new List<CustomerToList>();
            List<IDAL.DO.Customer> holdDalCustomer = dalObject.GetCustomerList().ToList();
            // run on all the customer list and put the correct info into   
            foreach (var item in holdDalCustomer)
            {
                customerBL.Add(new CustomerToList
                {
                    Id = item.Id,
                    Name = item.Name,
                    PhoneNumber = item.PhoneNumber,
                    ParcelProvided = dalObject.GetParcelList
                    (x => x.Delivered != DateTime.MinValue && x.SenderId == item.Id).ToList().Count,

                    Parcelsnet = dalObject.GetParcelList
                    (x => x.PickUp != DateTime.MinValue && x.Delivered == DateTime.MinValue && x.SenderId == item.Id).ToList().Count,

                    ParcelReceived = dalObject.GetParcelList
                    (x => x.Delivered != DateTime.MinValue && x.TargetId == item.Id).ToList().Count,

                    ParcelOnTheWay = dalObject.GetParcelList
                    (x => x.PickUp != DateTime.MinValue && x.Delivered == DateTime.MinValue && x.TargetId == item.Id).ToList().Count,
                });
            }

            return customerBL.FindAll(x => predicate == null ? true : predicate(x));
        }
        #endregion display customer list
    }
}
