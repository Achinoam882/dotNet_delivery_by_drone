using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using DAL;
namespace Dal
{
    class DalXml : IDal
    {
        public static string DroneXml = @"DroneXml.xml";
        public static string BaseStationXml = @"BaseStationXml.xml";
        public static string CustomerXml = @"CustomerXml.xml";
        public static string ParcelXml = @"ParcelXml.xml";
        public static string DroneChargeXml = @"DroneChargeXml.xml";
        public static string userPath = @"User.xml";


        #region Singelton

        static DalXml()// static ctor to ensure instance init is done just before first usage
        {
           //DataSource.Initialize();
        }

        private DalXml() //private  
        {
            List<DroneCharge> droneCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
            foreach (var item in droneCharge)
            {
                MoreChargeSlots(item.StationId);
            }
            droneCharge.Clear();
            XMLTools.SaveListToXMLSerializer(droneCharge, DroneChargeXml);
        }

        internal static DalXml Instance { get; } = new DalXml();// The public Instance property to use

        #endregion Singelton
        public double[] RequestPowerbyDrone()
        {

            List<string> config = XMLTools.LoadListFromXMLSerializer<string>(@"DalXmlConfig.xml");
            double[] temp = { double.Parse(config[0]), double.Parse(config[1]), double.Parse(config[2]),
                     double.Parse(config[3]),double.Parse(config[4])};
            return temp;
        } 
        #region Parcel
        public int SetParcel(Parcel newParcel)
        {
            List<Parcel> parcel = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);

            List<string> config = XMLTools.LoadListFromXMLSerializer<string>(@"DalXmlConfig.xml");
            int CountIdPackage = int.Parse(config[5]);
            newParcel.Id = CountIdPackage++;
            config[5] = CountIdPackage.ToString();
            XMLTools.SaveListToXMLSerializer<string>(config, @"DalXmlConfig.xml");

            parcel.Add(newParcel);
            XMLTools.SaveListToXMLSerializer(parcel, ParcelXml);
            return newParcel.Id; //Returns the id of the current Parcel.
            
        }
        public void SetParcelToDrone(int ParcelId, int droneId)
        {
            List<Parcel> parcel = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            int indexaforParcel = parcel.FindIndex(x => x.Id == ParcelId);
            if (indexaforParcel == -1)
                throw new NonExistingObjectException();
            Parcel temp = parcel[indexaforParcel];
            temp.DroneId = droneId;
            temp.Scheduled = DateTime.Now;
            parcel[indexaforParcel] = temp;
            XMLTools.SaveListToXMLSerializer(parcel, ParcelXml);
        }

        public void PickUpParcel(int ParcelId)
        {
            List<Parcel> parcel = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);

            //Update the package.
            int indexaforParcel = parcel.FindIndex(x => x.Id == ParcelId);

            if (indexaforParcel == -1)
                throw new NonExistingObjectException();

            Parcel temp = parcel[indexaforParcel];
            temp.PickUp = DateTime.Now;
            parcel[indexaforParcel] = temp;

            XMLTools.SaveListToXMLSerializer(parcel, ParcelXml);
        }

        public void DeliverToCustomer(int ParcelId)
        {
            List<Parcel> parcel = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);

            int indexaforParcel = parcel.FindIndex(x => x.Id == ParcelId);

            if (indexaforParcel == -1)
                throw new NonExistingObjectException();

            Parcel temp = parcel[indexaforParcel];
            temp.Delivered = DateTime.Now;
            parcel[indexaforParcel] = temp;
            XMLTools.SaveListToXMLSerializer(parcel, ParcelXml);
        }

        public Parcel GetParcel(int ID)
        {
            List<Parcel> parcel = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            if (!parcel.Exists(x => x.Id == ID))
            {
                throw new NonExistingObjectException();
            }
            return parcel.Find(x => x.Id == ID);
        }

        public IEnumerable<Parcel> GetParcelList(Predicate<Parcel> prdicat = null)
        {
            List<Parcel> parcel = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            //return DataSource.ParcelsList.FindAll(x => prdicat == null ? true : prdicat(x));
            return parcel.Where(x => prdicat == null ? true : prdicat(x));
        }

        public void DeleteParcel(int ParcelId) 
        {
            List<Parcel> parcel = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);

            int index = parcel.FindIndex(x => x.Id == ParcelId);
            if (index == -1)
            {
                throw new NonExistingObjectException();
            }
            parcel.RemoveAt(index); //else

            XMLTools.SaveListToXMLSerializer(parcel, ParcelXml);
        }
        public IEnumerable<Parcel> GetFreeParcelList()
        {
            List<Parcel> Listparcel = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            return Listparcel.Where(x => x.DroneId == 0);
        }

        #endregion Parcel
        #region Customers
        public void SetCustomer(Customer newCustomer)
        {
            XElement element = XMLTools.LoadListFromXMLElement(CustomerXml);

            XElement customer = (from cus in element.Elements()
                                 where cus.Element("Id").Value == newCustomer.Id.ToString()
                                 select cus).FirstOrDefault();
            if (customer != null)
            {
                throw new AddExistingObjectException();
            }

            XElement CustomerElement= new XElement("Customer",
                                 new XElement("Id", newCustomer.Id),
                                 new XElement("Name", newCustomer.Name),
                                 new XElement("PhoneNumber", newCustomer.PhoneNumber),
                                 new XElement("Longitude", newCustomer.Longitude),
                                 new XElement("Latitude", newCustomer.Latitude));

            element.Add(CustomerElement);
            XMLTools.SaveListToXMLElement(element, CustomerXml);
        }
        public void UpDateCustomer(Customer newCustomer)
        {
            XElement element = XMLTools.LoadListFromXMLElement(CustomerXml);

            XElement customer = (from cus in element.Elements()
                                 where cus.Element("Id").Value == newCustomer.Id.ToString()
                                 select cus).FirstOrDefault();
            if (customer == null)
            {
                throw new NonExistingObjectException();
            }

            customer.Element("Id").Value = newCustomer.Id.ToString();
            customer.Element("Name").Value = newCustomer.Name;
            customer.Element("PhoneNumber").Value = newCustomer.PhoneNumber;
            customer.Element("Longitude").Value = newCustomer.Longitude.ToString();
            customer.Element("Latitude").Value = newCustomer.Latitude.ToString();

            XMLTools.SaveListToXMLElement(element, CustomerXml);


        }

         

        public Customer GetCustomer(int ID)
        {
            XElement element = XMLTools.LoadListFromXMLElement(CustomerXml);

            Customer customer = (from cus in element.Elements()
                                 where int.Parse(cus.Element("Id").Value) == ID
                                 select new Customer()
                                 {
                                     Id = int.Parse(cus.Element("Id").Value),
                                     Name = cus.Element("Name").Value,
                                     PhoneNumber = cus.Element("PhoneNumber").Value,
                                     Longitude = double.Parse(cus.Element("Longitude").Value),
                                     Latitude = double.Parse(cus.Element("Latitude").Value)
                                 }).FirstOrDefault();

            if (customer.Id != 1)
            {
                return customer;
            }
            else
            {
                throw new NonExistingObjectException();
            }
        }

        public IEnumerable<Customer> GetCustomerList()
        {
            XElement element = XMLTools.LoadListFromXMLElement(CustomerXml);
            IEnumerable<Customer> customer = from cus in element.Elements()
                                             select new Customer()
                                             {
                                                 Id = int.Parse(cus.Element("Id").Value),
                                                 Name = cus.Element("Name").Value,
                                                 PhoneNumber = cus.Element("PhoneNumber").Value,
                                                 Longitude = double.Parse(cus.Element("Longitude").Value),
                                                 Latitude = double.Parse(cus.Element("Latitude").Value)
                                             };
            return customer.Select(item => item);
        }

        #endregion Customers
        #region BaseStations
        public void SetBaseStation(BaseStation newbaseStation)
        {
            List<BaseStation> ListbaseStations = XMLTools.LoadListFromXMLSerializer<BaseStation>(BaseStationXml);
            if (ListbaseStations.Exists(x => x.Id == newbaseStation.Id))
            {
                throw new AddExistingObjectException();
            }
            ListbaseStations.Add(newbaseStation);
            XMLTools.SaveListToXMLSerializer(ListbaseStations, BaseStationXml);
            


        }

        public void UpDateBaseStation(BaseStation newBaseStation)
        {
            List<BaseStation> ListbaseStations = XMLTools.LoadListFromXMLSerializer<BaseStation>(BaseStationXml);
            if (!ListbaseStations.Exists(x => x.Id == newBaseStation.Id))
            {
                throw new NonExistingObjectException();
            }
            ListbaseStations[ListbaseStations.FindIndex(x => x.Id == newBaseStation.Id)] = newBaseStation; 
            XMLTools.SaveListToXMLSerializer(ListbaseStations, BaseStationXml);
        }

        public void LessChargeSlots(int baseStationId)
        {
            List<BaseStation> ListbaseStations = XMLTools.LoadListFromXMLSerializer<BaseStation>(BaseStationXml);
            //BaseStation update.
            int indexaforBaseStationId = ListbaseStations.FindIndex(x => x.Id == baseStationId);
            if (indexaforBaseStationId != -1)
            {
                BaseStation BaseStationTemp = ListbaseStations[indexaforBaseStationId];
                BaseStationTemp.ChargeSlots--;
                ListbaseStations[indexaforBaseStationId] = BaseStationTemp;
                XMLTools.SaveListToXMLSerializer(ListbaseStations, BaseStationXml);
            }



        }

        public void MoreChargeSlots(int baseStationId)
        {
            List<BaseStation> ListbaseStations = XMLTools.LoadListFromXMLSerializer<BaseStation>(BaseStationXml);
            //BaseStation update.
            int indexaBaseStationId = ListbaseStations.FindIndex(x => x.Id == baseStationId);
            
                BaseStation temp = ListbaseStations[indexaBaseStationId];
                temp.ChargeSlots++;
                ListbaseStations[indexaBaseStationId] = temp;
                XMLTools.SaveListToXMLSerializer(ListbaseStations, BaseStationXml);
            
        }

        public BaseStation GetBaseStation(int ID)
        {
            List<BaseStation> ListbaseStations = XMLTools.LoadListFromXMLSerializer<BaseStation>(BaseStationXml);
            if (!ListbaseStations.Exists(x => x.Id == ID))
            {
                throw new NonExistingObjectException();
            }
            return ListbaseStations.Find(x => x.Id == ID);
        }
        public IEnumerable<BaseStation> GetBaseStationFreeChargeSlots()
        {
            List<BaseStation> ListbaseStations = XMLTools.LoadListFromXMLSerializer<BaseStation>(BaseStationXml);
            return ListbaseStations.Where(x => x.ChargeSlots > 0)/*.ToList()*/;

        }


        public IEnumerable<BaseStation> GetBaseStationList(Predicate<BaseStation> predicate = null)
        {
            List<BaseStation> ListbaseStations = XMLTools.LoadListFromXMLSerializer<BaseStation>(BaseStationXml);
            return ListbaseStations.Where(x => predicate == null ? true : predicate(x));
        }

        #endregion BaseStations
        #region Drones
        public void SetDrone(Drone newDrone)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            if (drones.Exists(x => x.Id == newDrone.Id))
            {
                throw new AddExistingObjectException();
            }
            drones.Add(newDrone);
            XMLTools.SaveListToXMLSerializer(drones, DroneXml);
        }

        public void UpDateDrone(Drone newDrone)
        {
            List<Drone> Listdrones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            if (!Listdrones.Exists(x => x.Id == newDrone.Id))
            {
                throw new NonExistingObjectException();
            }
            Listdrones[Listdrones.FindIndex(x => x.Id == newDrone.Id)] = newDrone;
            XMLTools.SaveListToXMLSerializer(Listdrones, DroneXml);
        }

        public Drone GetDrone(int ID)
        {
            List<Drone> Listdrones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            if (!Listdrones.Exists(x => x.Id == ID))
            {
                throw new NonExistingObjectException();
            }
            return Listdrones.Find(x => x.Id == ID);
        }
        public void SendDroneToCharge(int droneId,int baseStationId)
        {
            List<DroneCharge> droneCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
            droneCharge.Add(new DroneCharge()
            {
                StationId = baseStationId,
                DroneId = droneId,
                TimeDroneInCharging = DateTime.Now
            });
            XMLTools.SaveListToXMLSerializer(droneCharge, DroneChargeXml);
        }

        public void ReleaseFromCharging(int droneId)
        {
            List<DroneCharge> droneCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
            droneCharge.RemoveAt(droneCharge.FindIndex(x => x.DroneId == droneId));
            XMLTools.SaveListToXMLSerializer(droneCharge, DroneChargeXml);
        }

        public IEnumerable<Drone> GetDroneList()
        {
            List<Drone> Listdrones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            return Listdrones.Select(item => item);

        }
        #endregion Drones
        #region DroneCharge
   

        public DroneCharge GetDroneCharge(int droneID)
        {
            List<DroneCharge> droneCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
            if (!droneCharge.Exists(x => x.DroneId == droneID))
            {
                throw new NonExistingObjectException();
            }
            return droneCharge.Find(x => x.DroneId == droneID);
        }

        public IEnumerable<DroneCharge> GetChargeSlotsList(Predicate<DroneCharge> predicate = null)
        {

            List<DroneCharge> droneCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);

            return droneCharge.Where(x => predicate == null ? true : predicate(x));

        }

        #endregion DroneCharge
        #region User
        /// <summary>
        /// returns a user with the name(id) that we are looking for
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUser(string id)
        {
            XElement UserRootElem = XMLTools.LoadListFromXMLElement(userPath);
          User user = (from u in UserRootElem.Elements()
                            where u.Element("UserName").Value == id && Boolean.Parse(u.Element("DelUser").Value) == false
                            select new User()
                            {
                                Id= int.Parse(u.Element("Id").Value),
                                UserName = u.Element("UserName").Value,
                                Salt = int.Parse(u.Element("Salt").Value),
                                HashedPassword = u.Element("HashedPassword").Value,
                               AllowingAccess = Boolean.Parse(u.Element("AllowingAccess").Value),
                                //password = u.Element("password").Value,
                                DelUser = Boolean.Parse(u.Element("DelUser").Value)
                            }
           ).FirstOrDefault();

            if (user != null)
             //  throw new NullReferenceException: ('Object reference not set to an instance of an object.'
                return user;
            throw new DO.NonExistingObjectException($"The user name {id} does not exist");

        }
        /// <summary>
        /// returns a list of all users in the xml file
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DO.User> GetUsers()
        {
            XElement UserRootElem = XMLTools.LoadListFromXMLElement(userPath);

            return (from u in UserRootElem.Elements()
                    where Boolean.Parse(u.Element("DelUser").Value) == false
                    select new User
                    {
                        Id = int.Parse(u.Element("Id").Value),
                        UserName = u.Element("UserName").Value,
                        Salt = int.Parse(u.Element("Salt").Value),
                        HashedPassword = u.Element("HashedPassword").Value,
                        AllowingAccess = Boolean.Parse(u.Element("AllowingAccess").Value),
                        password = u.Element("password").Value,
                        DelUser = Boolean.Parse(u.Element("DelUser").Value)
                    }
                   );

        }
        /// <summary>
        /// adds a new user
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(DO.User user)
        {
            XElement UserRootElem = XMLTools.LoadListFromXMLElement(userPath);
            XElement user1 = (from u in UserRootElem.Elements()
                              where u.Element("UserName").Value == user.UserName
                              select u).FirstOrDefault();

            if (user1 != null)
            {
                if (Boolean.Parse(user1.Element("DelUser").Value) == true)
                {
                    user1.Element("DelUser").Value = false.ToString();
                    XMLTools.SaveListToXMLElement(UserRootElem, userPath);

                    return;
                }
                else
                    throw new DO.NonExistingObjectException($"The user name {user.UserName} does not exist");
            }
            XElement UserElem = new XElement("User",
                                    new XElement("Id", user.Id),
                                   new XElement("UserName", user.UserName),
                                   new XElement("Salt", user.Salt),
                                   new XElement("HashedPassword", user.HashedPassword),
                                   new XElement("AllowingAccess", user.AllowingAccess),
                                   new XElement("password", user.password),
                                   new XElement("DelUser", user.DelUser)

                                   );

            UserRootElem.Add(UserElem);
            XMLTools.SaveListToXMLElement(UserRootElem, userPath);

        }
        /// <summary>
        /// updates a user
        /// </summary>
        /// <param name="user"></param>
        public void UpdatUser(DO.User user)
        {
            XElement UserRootElem = XMLTools.LoadListFromXMLElement(userPath);
            XElement user1 = (from u in UserRootElem.Elements()
                              where u.Element("UserName").Value == user.UserName
                              select u).FirstOrDefault();

            if (user1 != null)
            {
                if (Boolean.Parse(user1.Element("DelUser").Value) == false)
                {
                    user1.Element("Id").Value = user.Id.ToString();
                    user1.Element("UserName").Value = user.UserName;
                    user1.Element("Salt").Value = user.Salt.ToString();
                    user1.Element("HashedPassword").Value = user.HashedPassword;
                    user1.Element("AllowingAccess").Value = user.AllowingAccess.ToString();
                    user1.Element("password").Value = user.password;
                    user1.Element("DelUser").Value = user.DelUser.ToString();
                    XMLTools.SaveListToXMLElement(UserRootElem, userPath);
                }
                else
                    throw new DO.NonExistingObjectException($"The user name {user.UserName} does not exist");
            }
            else
                throw new DO.NonExistingObjectException($"The user name {user.UserName} does not exist");
        }
        /// <summary>
        /// deletes a user
        /// </summary>
        /// <param name="user"></param>
        public void DeleteUser(DO.User user)
        {
            XElement UserRootElem = XMLTools.LoadListFromXMLElement(userPath);
            XElement user1 = (from u in UserRootElem.Elements()
                              where u.Element("UserName").Value == user.UserName
                              select u).FirstOrDefault();

            if (user1 != null)
            {
                if (Boolean.Parse(user1.Element("DelUser").Value) == false)
                {
                    user1.Element("DelUser").Value = true.ToString();
                    XMLTools.SaveListToXMLElement(UserRootElem, userPath);
                }
                else
                    throw new DO.NonExistingObjectException($"The user name {user.UserName} is already deleted");
            }
            else
                throw new DO.NonExistingObjectException($"The user name {user.UserName} does not exist");
        }
        #endregion
    }
}

