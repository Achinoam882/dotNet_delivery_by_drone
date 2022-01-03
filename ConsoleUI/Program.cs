using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using IDAL;
using DO;
using DalApi;
using System.ComponentModel;

namespace ConsoleUI
{
    class Program
    {
        #region main options

        #region insert options
        /// <summary>
        /// The function handles various addition options.
        /// </summary>
        /// <param name="dal">DalObject object that is passed as a parameter to enable the functions in the DalObject class</param>
        static public void AddOptions(DalApi.IDal dal)
        {
            Add add;
            int number = 0;
            Console.WriteLine(@"
choose:
0- To add a base station to the list, 
1- To add a drone to the list,
2- To add a new customer to the list,
3- To add a package for shipment");
            int.TryParse(Console.ReadLine(), out number);
            add = (Add)number;
            switch (add)
            {
                case Add.Basestation:
                    int idStation, chargeSlots;
                    double latitude, longitude;
                    string name;
                    Console.WriteLine(@"
You have chosen to add a new  base station,
please enter the id of the new station");
                    int.TryParse(Console.ReadLine(), out idStation);
                    Console.WriteLine(" please enter the name of the new station");
                    name = Console.ReadLine();
                    Console.WriteLine(" please enter the latitude of the new station");
                    double.TryParse(Console.ReadLine(), out latitude);
                    Console.WriteLine(" please enter the longitude of the new station");
                    double.TryParse(Console.ReadLine(), out longitude);
                    Console.WriteLine(" please enter the amount of charge slots in the new station");
                    int.TryParse(Console.ReadLine(), out chargeSlots);
                    BaseStation newBaseStation = new BaseStation
                    {
                        Id = idStation,
                        Name = name,
                        Longitude = longitude,
                        Latitude = latitude,
                        ChargeSlots = chargeSlots,
                    };
                    dal.SetBaseStation(newBaseStation);
                    break;
                case Add.Drone:
                    int idDrone;
                    string model;
                    WeightCategories maxWeight;
                    //DroneStatuses status;
                    double battery;
                    Console.WriteLine(@"
You have chosen to add a new  drone,
please enter the id of the new drone");
                    int.TryParse(Console.ReadLine(), out idDrone);
                    Console.WriteLine(" please enter the model of the new drone");
                    model = Console.ReadLine();
                    Console.WriteLine(" please enter the weight of the new drone:0 for light,1 for medium,2 for heavy");
                    WeightCategories.TryParse(Console.ReadLine(), out maxWeight);
                    Console.WriteLine(" please enter the status of the new drone:0 for free,1 for inMaintenance,2 for busy");
                    //DroneStatuses.TryParse(Console.ReadLine(), out status);
                    Console.WriteLine("please enter the charge level of  the battery");
                    double.TryParse(Console.ReadLine(), out battery);
                    Drone newDrone = new Drone
                    {
                        Id = idDrone,
                        Model = model,
                        MaxWeight = maxWeight,
                        //Status = status,
                        //Battery = battery,
                    };
                    dal.SetDrone( newDrone);
                    break;
                case Add.NewCustomer:
                    int idCustomer;
                    string nameCustomer;
                    string phoneNumber;
                    double longitudeCustomer;
                    double latitudeCustomer;
                    Console.WriteLine(@"
You have chosen to add a new customer,
please enter the id of the new customer");
                    int.TryParse(Console.ReadLine(), out idCustomer);
                    Console.WriteLine("please enter the name of the new customer");
                    nameCustomer = Console.ReadLine();
                    Console.WriteLine("please enter the phone number of the new customer");
                    phoneNumber = Console.ReadLine();
                    Console.WriteLine("please enter the latitude of the new customer");
                    double.TryParse(Console.ReadLine(), out latitudeCustomer);
                    Console.WriteLine("please enter the longitude of the new customer");
                    double.TryParse(Console.ReadLine(), out longitudeCustomer);
                    Customer newCustomer = new Customer { 
                    Id = idCustomer,
                    Name = nameCustomer,
                    Longitude = longitudeCustomer,
                    Latitude = latitudeCustomer,
                    PhoneNumber = phoneNumber,};
                    dal.SetCustomer(newCustomer);
                    break;
                case Add.ParcelDelivery:
                    int senderId, targetId;
                    WeightCategories weightParcel;
                    Priorities newPriority;
                    Console.WriteLine(@"
You have chosen to add a new parcel to delivery,
please enter the id of the sender");
                    int.TryParse(Console.ReadLine(), out senderId);
                    Console.WriteLine("please enter the id of the target");
                    int.TryParse(Console.ReadLine(), out targetId);
                    Console.WriteLine(" please enter the weight of the new parcel:0 for light,1 for medium,2 for heavy");
                    WeightCategories.TryParse(Console.ReadLine(), out weightParcel);
                    Console.WriteLine(" please enter the priority of the new parcel:0 for regular,1 for fast,2 for urgent");
                    Priorities.TryParse(Console.ReadLine(), out newPriority);
                    Parcel newParcel = new Parcel
                    {
                        SenderId = senderId,
                        TargetId = targetId,
                        Weight = weightParcel,
                        Priority = newPriority,
                        //Requested = DateTime.Now,
                    };
                    int counterParcel = dal.SetParcel(newParcel);
                    break;
                default:
                    break;
            }

        }
        #endregion insert options

        #region update options
        /// <summary>
        /// The function handles various update options.
        /// </summary>
        /// <param name="dal">DalObject object that is passed as a parameter to enable the functions in the DalObject class</param>
        static public void UpDateOptions(DalApi.IDal dal)
        {
            Update update;
            int number = 0;
            Console.WriteLine(@"
choose:
0- To update a parcel to drone, 
1- To pick up parcel,
2- To deliver parcel, 
3- To charge drone, 
4- To stop charging drone");
            int.TryParse(Console.ReadLine(), out number);
            update= (Update)number;
            int parcelId, baseStationId, droneId;
            switch (update)
            {
                case Update.ParcelToDrone:
                    Console.WriteLine("please enter the id of the parcel");
                    int.TryParse(Console.ReadLine(), out parcelId);
                    Console.WriteLine("please enter the id of the drone");
                    int.TryParse(Console.ReadLine(), out droneId);
                    dal.SetParcelToDrone(parcelId, droneId);
                    break;
                case Update.PickUp:
                    Console.WriteLine("please enter the id of the parcel");
                    int.TryParse(Console.ReadLine(), out parcelId);
                    dal.PickUpParcel(parcelId);
                    break;
                case Update.Delivered:
                    Console.WriteLine("please enter the id of the parcel");
                    int.TryParse(Console.ReadLine(), out parcelId);
                    dal.DeliverToCustomer(parcelId);
                    break;
                case Update.InCharging:
                    Console.WriteLine("please enter the id of the drone");
                    int.TryParse(Console.ReadLine(), out droneId);
                    Console.WriteLine("please select a base station id");
                    foreach (BaseStation item in (List<BaseStation>)dal.GetBaseStationFreeChargeSlots())
                        Console.WriteLine(item.ToString());
                    int.TryParse(Console.ReadLine(), out baseStationId);
                    dal.SendDroneToCharge(droneId,baseStationId);
                    break;
                case Update.OutCharging:
                    Console.WriteLine("please enter the id of the drone");
                    int.TryParse(Console.ReadLine(), out droneId);
                    dal.ReleaseFromCharging(droneId);
                    break;
                default:
                    break;
            }
        }
        #endregion update options

        #region display options
        /// <summary>
        /// The function handles display options.
        /// </summary>
        /// <param name="dal">DalObject object that is passed as a parameter to enable the functions in the DalObject class</param>
        static public void DisplayOptions(DalApi.IDal dal)
        {
            int idForAllObjects;
            Display display;
            int number = 0;
            Console.WriteLine(@"
choose: 
0- To view Base Station , 
1- To view Drone,
2- To view Customer, 
3- To view Parcel");
            int.TryParse(Console.ReadLine(), out number);
            display = (Display)number;
            switch (display)
            {
                case Display.ViewBaseStation:
                    Console.WriteLine("please enter the id of the base station ");
                    int.TryParse(Console.ReadLine(), out idForAllObjects);
                    Console.WriteLine(dal.GetBaseStation(idForAllObjects).ToString());
                    break;
                case Display.ViewDrone:
                    Console.WriteLine("please enter the id of the drone ");
                    int.TryParse(Console.ReadLine(), out idForAllObjects);
                    Console.WriteLine(dal.GetDrone(idForAllObjects).ToString());
                    break;
                case Display.ViewCustomer:
                    Console.WriteLine("please enter the id of the customer ");
                    int.TryParse(Console.ReadLine(), out idForAllObjects);
                    Console.WriteLine(dal.GetCustomer(idForAllObjects).ToString());
                    break;
                case Display.ViewParcel:
                    Console.WriteLine("please enter the id of the parcel ");
                    int.TryParse(Console.ReadLine(), out idForAllObjects);
                    Console.WriteLine(dal.GetParcel(idForAllObjects).ToString());
                    break;
                default:
                    break;
            }

        }
        #endregion display  options

        #region display lists options
        /// <summary>
        /// The function prints the data in the list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listToPrint"></param>
        public static void PrintList<T>(List<T> PrintList) where T : struct
        {
            foreach (T item in PrintList)
            {
                Console.WriteLine(item);
            }
        }
        /// <summary>
        /// The function handles list view options.
        /// </summary>
        /// <param name="dal">DalObject object that is passed as a parameter to enable the functions in the DalObject class</param>
        static public void DisplayListsOptions(DalApi.IDal dal)
        {
            ViewList viewList;
            int number = 0;
            Console.WriteLine(@"
choose:
0- To view list of Base Stations, 
1- To view list of Drons,
2- To view list of Customers, 
3- To view list of Parcels,
4- To view list of free parcels,
5- To view list of free charge slots in base station");
            int.TryParse(Console.ReadLine(), out number);
            viewList = (ViewList)number;
            switch (viewList)
            {
                case ViewList.ListBaseStation:
                    PrintList(dal.GetBaseStationList().ToList());
                    break;
                case ViewList.ListDrone:
                    PrintList(dal.GetDroneList().ToList());
                    break;
                case ViewList.ListCustomer:
                    PrintList(dal.GetCustomerList().ToList());
                    break;
                case ViewList.ListParcel:
                    PrintList(dal.GetParcelList().ToList());
                    break;
                case ViewList.ListFreeParcel:
                    PrintList(dal.GetFreeParcelList().ToList());
                    break;
                case ViewList.ListFreeChargeSlotsInBaseStation:
                    PrintList(dal.GetBaseStationFreeChargeSlots().ToList());
                    break;
                default:
                    break;
            }
 

        }
        #endregion display lists options


        #endregion main options

        static void Main(string[] args)
        {
            //IDal dalObject = new DalObject.DalObject();
            IDal dalObject = DalFactory.GetDal();


            int number=0;
            Choice choice;
            do
            {
                Console.WriteLine(@"please choose :
0-To add,
1-To update,
2-To display,
3-To view lists,
4-To exit");
                while (!int.TryParse(Console.ReadLine(), out number)) ;
                choice = (Choice)number;
                switch (choice)
                {
                    case Choice.Add:
                        AddOptions(dalObject);
                        break;
                    case Choice.Update:
                        UpDateOptions(dalObject);
                        break;
                    case Choice.Display:
                        DisplayOptions(dalObject);
                        break;
                    case Choice.ViewLists:
                        DisplayListsOptions(dalObject);
                        break;
                    case Choice.Exit:
                        Console.WriteLine("Have a great day");
                        break;
                    default:
                        break;
                }

            }
           while (number != 4);
        }
    }
}
