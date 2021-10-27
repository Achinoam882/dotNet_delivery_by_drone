using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IDAL.DO;
using System.ComponentModel;

namespace ConsoleUI
{
    class Program
    {
        /// <summary>
        /// The function handles various addition options.
        /// </summary>
        /// <param name="dal">DalObject object that is passed as a parameter to enable the functions in the DalObject class</param>
        static public void AddOptions( DalObject.DalObject dal)
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
                case Add.basestation:
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
                    dal.SetBaseStation(idStation, name, longitude, latitude, chargeSlots);
                    break;
                case Add.drone:
                    int idDrone;
                    string model;
                    WeightCategories maxWeight;
                    DroneStatuses status;
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
                    DroneStatuses.TryParse(Console.ReadLine(), out status);
                    Console.WriteLine("please enter the charge level of  the battery");
                    double.TryParse(Console.ReadLine(), out battery);
                    dal.SetDrone(idDrone, model, maxWeight, status, battery);
                    break;
                case Add.newcustomer:
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
                    phoneNumber= Console.ReadLine();
                    Console.WriteLine("please enter the latitude of the new customer");
                    double.TryParse(Console.ReadLine(), out latitudeCustomer);
                    Console.WriteLine("please enter the longitude of the new customer");
                    double.TryParse(Console.ReadLine(), out longitudeCustomer);
                    dal.SetCustomer(idCustomer, nameCustomer, longitudeCustomer, latitudeCustomer, phoneNumber);
                    break;
                case Add.parcelDelivery:
                    int senderId, targetId;
                    WeightCategories weightParcel;
                    Priorities Priority;
                    Console.WriteLine(@"
You have chosen to add a new parcel to delivery,
please enter the id of the sender");
                    int.TryParse(Console.ReadLine(), out senderId);
                    Console.WriteLine("please enter the id of the target");
                    int.TryParse(Console.ReadLine(), out targetId);
                    Console.WriteLine(" please enter the weight of the new parcel:0 for light,1 for medium,2 for heavy");
                    WeightCategories.TryParse(Console.ReadLine(), out weightParcel);
                    Console.WriteLine(" please enter the priority of the new parcel:0 for regular,1 for fast,2 for urgent");
                    Priorities.TryParse(Console.ReadLine(), out Priority);
                    int counterParcel = dal.SetParcel(senderId, targetId, weightParcel, Priority);
                    break;
                default:
                    break;
            }

        }
        /// <summary>
        /// The function handles various update options.
        /// </summary>
        /// <param name="dal">DalObject object that is passed as a parameter to enable the functions in the DalObject class</param>
        static public void UpDateOptions(DalObject.DalObject dal)
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
                case Update.parcelToDrone:
                    Console.WriteLine("please enter the id of the parcel");
                    int.TryParse(Console.ReadLine(), out parcelId);
                    Console.WriteLine("please enter the id of the drone");
                    int.TryParse(Console.ReadLine(), out droneId);
                    dal.SetParcelToDrone(parcelId, droneId);
                    break;
                case Update.pickUp:
                    Console.WriteLine("please enter the id of the parcel");
                    int.TryParse(Console.ReadLine(), out parcelId);
                    dal.PickUpParcel(parcelId);
                    break;
                case Update.delivered:
                    Console.WriteLine("please enter the id of the parcel");
                    int.TryParse(Console.ReadLine(), out parcelId);
                    dal.DeliverToCustomer(parcelId);
                    break;
                case Update.inCharging:
                    Console.WriteLine("please enter the id of the drone");
                    int.TryParse(Console.ReadLine(), out droneId);
                    Console.WriteLine("please select a base station id");
                    List<BaseStation> FreeChargeSlotsInBaseStation = new List<BaseStation>();
                    FreeChargeSlotsInBaseStation = dal.GetBaseStationFreeChargeSlots();
                    for (int i= 0; i < FreeChargeSlotsInBaseStation.Count(); i++)
                    {
                        Console.WriteLine(FreeChargeSlotsInBaseStation[i].ToString());
                    }
                    int.TryParse(Console.ReadLine(), out baseStationId);
                    dal.SendDroneToCharge(droneId,baseStationId);
                    break;
                case Update.outCharging:
                    Console.WriteLine("please enter the id of the drone");
                    int.TryParse(Console.ReadLine(), out droneId);
                    dal.ReleaseFromCharging(droneId);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// The function handles display options.
        /// </summary>
        /// <param name="dal">DalObject object that is passed as a parameter to enable the functions in the DalObject class</param>
        static public void DisplayOptions(DalObject.DalObject dal)
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
                case Display.viewBaseStation:
                    Console.WriteLine("please enter the id of the base station ");
                    int.TryParse(Console.ReadLine(), out idForAllObjects);
                    Console.WriteLine(dal.GetBaseStation(idForAllObjects).ToString());
                    break;
                case Display.viewDrone:
                    Console.WriteLine("please enter the id of the drone ");
                    int.TryParse(Console.ReadLine(), out idForAllObjects);
                    Console.WriteLine(dal.GetDrone(idForAllObjects).ToString());
                    break;
                case Display.viewCustomer:
                    Console.WriteLine("please enter the id of the customer ");
                    int.TryParse(Console.ReadLine(), out idForAllObjects);
                    Console.WriteLine(dal.GetCustomer(idForAllObjects).ToString());
                    break;
                case Display.viewParcel:
                    Console.WriteLine("please enter the id of the parcel ");
                    int.TryParse(Console.ReadLine(), out idForAllObjects);
                    Console.WriteLine(dal.GetParcel(idForAllObjects).ToString());
                    break;
                default:
                    break;
            }

        }
        /// <summary>
        /// The function handles list view options.
        /// </summary>
        /// <param name="dal">DalObject object that is passed as a parameter to enable the functions in the DalObject class</param>
        static public void DisplayListsOptions(DalObject.DalObject dal)
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
                case ViewList.listBaseStation:
                    List<BaseStation> BaseStationList = new List<BaseStation>();
                    BaseStationList=dal.GetBaseStationList();
                    for(int i=0;i< BaseStationList.Count();i++)
                    {
                        Console.WriteLine(BaseStationList[i].ToString());
                    }
                    break;
                case ViewList.listDrone:
                    List<Drone> DroneList = new List<Drone>();
                    DroneList = dal.GetDroneList();
                    for (int i = 0; i < DroneList.Count(); i++)
                    {
                        Console.WriteLine(DroneList[i].ToString());
                    }
                    break;
                case ViewList.listCustomer:
                    List<Customer> CustomerList = new List<Customer>();
                    CustomerList = dal.GetCustomerList();
                    for (int i = 0; i < CustomerList.Count(); i++)
                    {
                        Console.WriteLine(CustomerList[i].ToString());
                    }
                    break;
                case ViewList.listParcel:
                    List<Parcel> ParcelList = new List<Parcel>();
                    ParcelList = dal.GetParcelList();
                    for (int i = 0; i < ParcelList.Count(); i++)
                    {
                        Console.WriteLine(ParcelList[i].ToString());
                    }
                    break;
                case ViewList.listFreeParcel:
                    List<Parcel> FreeParcelList = new List<Parcel>();
                    FreeParcelList = dal.GetFreeParcelList();
                    for (int i = 0; i < FreeParcelList.Count(); i++)
                    {
                        Console.WriteLine(FreeParcelList[i].ToString());
                    }
                    break;
                case ViewList.listFreeChargeSlotsInBaseStation:
                    List<BaseStation> FreeChargeSlotsInBaseStationList = new List<BaseStation>();
                    FreeChargeSlotsInBaseStationList = dal.GetBaseStationFreeChargeSlots();
                    for (int i = 0; i < FreeChargeSlotsInBaseStationList.Count(); i++)
                    {
                        Console.WriteLine(FreeChargeSlotsInBaseStationList[i].ToString());
                    }
                    break;
                default:
                    break;
            }
 

        }
        static void Main(string[] args)
        {
            DalObject.DalObject dalObject = new DalObject.DalObject();
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
                    case Choice.add:
                        AddOptions(dalObject);
                        break;
                    case Choice.update:
                        UpDateOptions(dalObject);
                        break;
                    case Choice.display:
                        DisplayOptions(dalObject);
                        break;
                    case Choice.viewLists:
                        DisplayListsOptions(dalObject);
                        break;
                    case Choice.exit:
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
