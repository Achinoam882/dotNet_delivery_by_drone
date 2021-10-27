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
        static public void AddOptions(DalObject.DalObject dal)
        {
            Add add;
            int number = 0;
            Console.WriteLine(@"choose: 0-To add a base station to the list, 
                                        1-To add a drone to the list,
                                        2-To add a new customer to the list,
                                        3-To add a package for shipment");
            int.TryParse(Console.ReadLine(), out number);
                add = (Add)number;
            switch (add)
            {
                case Add.basestation:
                    int idStation, chargeSlots; 
                    string name;
                    double longitude, latitude;
                    Console.WriteLine(@"You have chosen to add a new base station,
                                        please enter the id of the new station");
                    int.TryParse(Console.ReadLine(), out idStation);
                    Console.WriteLine(@"please enter the name of the new station");
                    name = Console.ReadLine();
                    Console.WriteLine(@"please enter the latitude of the new station");
                    double.TryParse(Console.ReadLine(), out latitude);
                    Console.WriteLine(@"please enter the longitude of the new station");
                    double.TryParse(Console.ReadLine(), out longitude);
                    Console.WriteLine(@"please enter the amount of charge slots in the new station");
                    int.TryParse(Console.ReadLine(), out chargeSlots);
                    dal.SetBaseStation(idStation, name, longitude, latitude, chargeSlots);
                    break;
                case Add.drone:
                    int idDrone;
                    string model;
                    WeightCategories maxWeight;
                    DroneStatuses status;
                    double battery;
                    Console.WriteLine(@"You have chosen to add a new drone,
                                        please enter the id of the new drone");
                    int.TryParse(Console.ReadLine(), out idDrone);
                    Console.WriteLine(@"please enter the model of the new drone");
                    model = Console.ReadLine();
                    Console.WriteLine(@"please enter the maxWeight of the new drone: 0- for light,1- for medium,2- for heavy ");
                    WeightCategories.TryParse(Console.ReadLine(), out maxWeight);
                    Console.WriteLine(@"please enter the status of the new drone:0- for free,1- for inMaintenance,2- for busy");
                    DroneStatuses.TryParse(Console.ReadLine(), out status);
                    Console.WriteLine(@"please enter the charge level of the battery");
                    double.TryParse(Console.ReadLine(), out battery);
                    dal.SetDrone(idDrone, model, maxWeight, status, battery);
                    break;
                case Add.newcustomer:
                    int idCustomer;
                    string nameCustomer, phoneNum;
                    double longitudeCustomer, latitudeCustomer;
                    Console.WriteLine(@"You have chosen to add a new customer,
                                        please enter the id of the customer");
                    int.TryParse(Console.ReadLine(), out idCustomer);
                    Console.WriteLine(@"please enter the name of the new customer");
                    nameCustomer = Console.ReadLine();
                    Console.WriteLine(@"please enter the latitude of the new customer");
                    double.TryParse(Console.ReadLine(), out latitudeCustomer);
                    Console.WriteLine(@"please enter the longitude of the new customer");
                    double.TryParse(Console.ReadLine(), out longitudeCustomer);
                    Console.WriteLine(@"please enter the phone number of the new customer");
                    phoneNum = Console.ReadLine();
                    dal.SetCustomer(idCustomer, nameCustomer, longitudeCustomer, latitudeCustomer, phoneNum);
                    break;
                case Add.parcelDelivery:
                    int senderId, targetId;
                    WeightCategories weight;
                    Priorities priority;
                    Console.WriteLine(@"You have chosen to add a new parcel to delivey,
                                        Please enter the sender ID number:");
                    int.TryParse(Console.ReadLine(), out senderId);
                    Console.WriteLine(@"Please enter the target ID number:");
                    int.TryParse(Console.ReadLine(), out targetId);
                    Console.WriteLine(@"please enter the maxWeight of the new parcel: 0- for light,1- for medium,2- for heavy ");
                    WeightCategories.TryParse(Console.ReadLine(), out weight);
                    Console.WriteLine(@"please enter the priority of the new parcel: 0- for regular,1- for fast,2- for urgent ");
                    Priorities.TryParse(Console.ReadLine(), out priority);
                    dal.SetParcel(senderId, targetId, weight, priority);
                    break;
                default:
                    break;
            }
        }
        static public void UpdateOptions(DalObject.DalObject dal)
        {
            Update update;
            int number = 0;
            Console.WriteLine(@"choose: 0-update a parcel to drone, 
                                        1-To pick up parcel,
                                        2-To deliver parcel,
                                        3-To charg drone,
                                        4-To stop charging drone ");
            int.TryParse(Console.ReadLine(), out number);
            update = (Update)number;
            int idParcel,droneId, baseStationId;
            switch (update)
            {
                case Update.parcelToDrone:
                    Console.WriteLine(@"please enter the parcel id: ");
                    int.TryParse(Console.ReadLine(), out idParcel);
                    Console.WriteLine(@"please enter the drone id: ");
                    int.TryParse(Console.ReadLine(), out droneId);
                    dal.SetParcelToDrone(idParcel, droneId);
                    break;
                case Update.pickUp:
                    Console.WriteLine(@"please enter the parcel id: ");
                    int.TryParse(Console.ReadLine(), out idParcel);
                    dal.PickUpParcel(idParcel);
                    break;
                case Update.deliverd:
                    Console.WriteLine(@"please enter the parcel id: ");
                    int.TryParse(Console.ReadLine(), out idParcel);
                    dal.DeliverToCustomer(idParcel);
                    break;
                case Update.incharging:
                    Console.WriteLine("please enter Drone ID:");
                    int.TryParse(Console.ReadLine(), out droneId);
                    Console.WriteLine("please choose baseStationId ID from the List below:");
                    List<BaseStation>FreeChargeSlotsInBaseStation = new List<BaseStation>();
                    FreeChargeSlotsInBaseStation = dal.GetBaseStationFreeChargeSlots();
                    for(int i=0;i< FreeChargeSlotsInBaseStation.Count();i++)
                    {
                        Console.WriteLine(FreeChargeSlotsInBaseStation[i].ToString());
                    }
                    int.TryParse(Console.ReadLine(), out baseStationId);
                    dal.SendDroneToCharge(droneId, baseStationId);

                    break;
                case Update.outcharging:
                    Console.WriteLine(@"please enter the drone id: ");
                    int.TryParse(Console.ReadLine(), out droneId);
                    dal.ReleaseFromeCharging(droneId);
                    break;
                default:
                    break;
            }
        }
       static public void DisPlayOptions(DalObject.DalObject dal)
        {
            int number = 0, idForViewObject;
            Display display;
            Console.WriteLine(@"choose: 0-To view base station, 
                                        1-To view drone,
                                        2-To view customers,
                                        3-To view package"
                              );
            int.TryParse(Console.ReadLine(), out number);
            display = (Display)number;
            switch (display)
            {
                case Display.viewBaseStation:
                    Console.WriteLine(@"Insert ID number of base station:");
                    int.TryParse(Console.ReadLine(), out idForViewObject);
                    Console.WriteLine(dal.GetBaseStation(idForViewObject).ToString());
                    break;
                case Display.viewDrone:
                    Console.WriteLine(@"Insert ID number of drone:");
                    int.TryParse(Console.ReadLine(), out idForViewObject);
                    Console.WriteLine(dal.GetDrone(idForViewObject).ToString());
                    break;
                case Display.viewCustomers:
                    Console.WriteLine(@"Insert ID number of customer:");
                    int.TryParse(Console.ReadLine(), out idForViewObject);
                    Console.WriteLine(dal.GetCustomer(idForViewObject).ToString());
                    break;
                case Display.viewPackage:
                    Console.WriteLine(@"Insert ID number of package:");
                    int.TryParse(Console.ReadLine(), out idForViewObject);
                    Console.WriteLine(dal.GetParcel(idForViewObject).ToString());

                    break;
                default:
                    break;
            }
        }
        static public void DisplayList(DalObject.DalObject dal)
        {
            DisplayLists displayLists;
            int number = 0;
            Console.WriteLine(@"choose: 0-To view base stations list, 
                                        1-To view drone list,
                                        2-To view customers list,
                                        3-To view package list"
                              );
            int.TryParse(Console.ReadLine(), out number);
            displayLists = (DisplayLists)number;
            switch (displayLists)
            {
                case DisplayLists.ListOfBaseStation:
                    List<BaseStation> displayBaseList = new List<BaseStation>();
                    displayBaseList = dal.GetBaseStationList();
                    for(int i=0; i< displayBaseList.Count();i++)
                    {
                        Console.WriteLine(displayBaseList[i].ToString());
                    }
                    break;
                case DisplayLists.ListOfDroned:
                    List<Drone> displayDroneList = new List<Drone>();
                    displayDroneList = dal.GetDroneList();
                    for(int i=0; i<displayDroneList.Count();i++)
                    {
                        Console.WriteLine(displayDroneList[i].ToString());
                    }

                    break;
                case DisplayLists.ListOfCustomer:
                    List<Customer> displayCustomerList = new List<Customer>();
                    displayCustomerList = dal.GetCustomerList();
                    for (int i = 0; i < displayCustomerList.Count(); i++)
                    {
                        Console.WriteLine(displayCustomerList[i].ToString());
                    }
                    break;
                case DisplayLists.ListOfPackage:
                    List<Parcel> displayParcelList = new List<Parcel>();
                    displayParcelList = dal.GetParcelList();
                    for (int i = 0; i < displayParcelList.Count(); i++)
                    {
                        Console.WriteLine(displayParcelList[i].ToString());
                    }
                    break;
                case DisplayLists.ListOfFreePackage:
                    List<Parcel> displayParcelWithoutDrone = new List<Parcel>();
                    displayParcelWithoutDrone = dal.GetParcelWithoutDrone();
                    for (int i = 0; i < displayParcelWithoutDrone.Count(); i++)
                    {
                        Console.WriteLine(displayParcelWithoutDrone[i].ToString());
                    }
                    break;
                case DisplayLists.ListOfBaseStationsWithFreeChargSlots:
                    List<BaseStation> displayBaseStationWithFreeChargSlots = new List<BaseStation>();
                    displayBaseStationWithFreeChargSlots = dal.GetBaseStationFreeChargeSlots();
                    for (int i = 0; i < displayBaseStationWithFreeChargSlots.Count(); i++)
                    {
                        Console.WriteLine(displayBaseStationWithFreeChargSlots[i].ToString());
                    }
                    break;
                default:
                    break;
            }

        }
        static void Main(string[] args)
        {
            DalObject.DalObject dalObject = new DalObject.DalObject();
        Choice choice;
        int number = 0;
            do
            {
                Console.WriteLine(@"choose: 0-To add, 
                                            1-To update,
                                            2-To display,
                                            3-To view lists,
                                            4-To exit");

                while (!int.TryParse(Console.ReadLine(), out number)) ;
                choice = (Choice)number;
                switch (choice)
                {
                    case Choice.add: AddOptions(dalObject);
               
                        break;
                    case Choice.update: UpdateOptions(dalObject);
                        break;
                    case Choice.display: DisPlayOptions(dalObject);
                        break;
                    case Choice.viewLists:DisplayList(dalObject);
                        break;
                    case Choice.exit:
                        Console.WriteLine("Have a good day");
                        break;
                    default:
                        break;
                }
            } while (!(number == 4));

        }
    }
}
