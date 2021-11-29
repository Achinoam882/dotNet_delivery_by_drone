using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL;
using IBL.BO;



namespace ConsoleUI_BL
{
    class Program
    {
        #region main options

        #region insert options
        /// <summary>
        /// The function handles various addition options.
        /// </summary>
        /// <param name="dal">DalObject object that is passed as a parameter to enable the functions in the DalObject class</param>
        static public void AddOptions(IBL.IBL bl)
        {

            Add add;
            int number = 0;
            Console.WriteLine(@"
choose:
0- To add a base station to the list, 
1- To add a drone to the list,
2- To add a new customer to the list,
3- To add a package for delivery");
            while (!int.TryParse(Console.ReadLine(), out number));
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
                   while(!int.TryParse(Console.ReadLine(), out idStation));
                    Console.WriteLine(" please enter the name of the new station");
                    name = Console.ReadLine();
                    Console.WriteLine(" please enter the amount of charge slots in the new station");
                    while(! int.TryParse(Console.ReadLine(), out chargeSlots));
                    Console.WriteLine(" please enter the latitude of the new station");
                    while(!double.TryParse(Console.ReadLine(), out latitude));
                    Console.WriteLine(" please enter the longitude of the new station");
                    while(! double.TryParse(Console.ReadLine(), out longitude));
                    Location newLocation = new Location { Latitude = latitude, Longitude = longitude };
                    BaseStation newBaseStation = new BaseStation
                    {
                        Id = idStation,
                        Name = name,
                        BaseStationLocation= newLocation,
                        FreeChargeSlots = chargeSlots,
                        DroneChargingList = new List<DroneCharging>()
                    };
                    try
                    {
                        bl.SetBaseStation(newBaseStation);
                    }
                    catch (AddingProblemException ex)
                    {
                        Console.WriteLine(ex);
                    }
                    break;


                case Add.Drone:
                    int idDrone, firstChargingStation;
                    string model;
                    int maxWeight;
                    //DroneStatuses status;
                    //double battery;
                    Console.WriteLine(@"
You have chosen to add a new  drone,
please enter the id of the new drone");
                    while (!int.TryParse(Console.ReadLine(), out idDrone));
                    Console.WriteLine(" please enter the model of the new drone");
                    model = Console.ReadLine();
                    Console.WriteLine(" please enter the weight of the new drone:0 for light,1 for medium,2 for heavy");
                   while(! int.TryParse(Console.ReadLine(), out maxWeight));
                    Console.WriteLine("please enter the Station number  to put the drone for charging");
                    while (!int.TryParse(Console.ReadLine(), out firstChargingStation));
                    DroneToList newDrone = new DroneToList
                    {
                        Id = idDrone,
                        Model = model,
                        MaxWeight =(WeightCategories)maxWeight,
                        
                     
                    };
                    try
                    {
                        bl.SetDrone(newDrone, firstChargingStation);
                    }
                    catch (AddingProblemException ex)
                    {
                        Console.WriteLine(ex);
                    }
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
                   while(! int.TryParse(Console.ReadLine(), out idCustomer));
                    Console.WriteLine("please enter the name of the new customer");
                    nameCustomer = Console.ReadLine();
                    Console.WriteLine("please enter the phone number of the new customer");
                    phoneNumber = Console.ReadLine();
                    Console.WriteLine("please enter the latitude of the new customer");
                   while(! double.TryParse(Console.ReadLine(), out latitudeCustomer));
                    Console.WriteLine("please enter the longitude of the new customer");
                    while(!double.TryParse(Console.ReadLine(), out longitudeCustomer));
                    Location customerLocation = new Location { Latitude = latitudeCustomer, Longitude = longitudeCustomer };
                    Customer newCustomer = new Customer
                    {
                        Id = idCustomer,
                        Name = nameCustomer,
                        CustomerLocation=customerLocation,
                        PhoneNumber = phoneNumber,
                    };
                    try
                    {
                        bl.SetCustomer(newCustomer);
                    }
                    catch (AddingProblemException ex)
                    {
                        Console.WriteLine(ex);
                    }
                    break;

                case Add.ParcelDelivery:
                    int senderId, targetId, weightParcel, newPriority;
                    string senderName, targetName;
                    Console.WriteLine(@"
You have chosen to add a new parcel to delivery,
please enter the id of the sender");
                    while(!int.TryParse(Console.ReadLine(), out senderId));
                    Console.WriteLine("Next Please enter the name of the customer:");
                    senderName = Console.ReadLine();
                    Console.WriteLine("please enter the id of the target");
                    while(!int.TryParse(Console.ReadLine(), out targetId));
                    Console.WriteLine("Next Please enter the name of the customer:");
                    targetName = Console.ReadLine();
                    Console.WriteLine(" please enter the weight of the new parcel:0 for light,1 for medium,2 for heavy");
                    while(!int.TryParse(Console.ReadLine(), out weightParcel));
                    Console.WriteLine(" please enter the priority of the new parcel:0 for regular,1 for fast,2 for urgent");
                    while(!int.TryParse(Console.ReadLine(), out newPriority));
                    CustomerParcel newCustomerParcelSender = new CustomerParcel {Id= senderId,Name = senderName };
                    CustomerParcel newCustomerParcelTarget = new CustomerParcel { Id = targetId, Name = targetName };
                    Parcel newParcel = new Parcel
                    {
                        Sender = newCustomerParcelSender,
                        Receiving = newCustomerParcelTarget, 
                        Weight = (WeightCategories)weightParcel,
                        Priority = (Priorities)newPriority,                      
                    };
                    try
                    {
                        bl.SetParcel(newParcel);
                    }
                    catch (AddingProblemException ex)
                    {
                        Console.WriteLine(ex);
                    }
                    break;
                default:
                    Console.WriteLine(@"you entered a wrong number.
please try again");
                    break;
            }

        }
        #endregion insert options

        #region update options
        /// <summary>
        /// The function handles various update options.
        /// </summary>
        /// <param name="dal">DalObject object that is passed as a parameter to enable the functions in the DalObject class</param>
        static public void UpDateOptions(IBL.IBL bl)
        {
            Update update;
            int number = 0;
            Console.WriteLine(@"
choose:
0- To update a drone name, 
1- To  update a base station,
2- To  update a customer, 
3- To charge  a drone, 
4- To stop charging drone
5-To update a parcel to drone
6- To pick up parcel by drone
7-To deliver parcel by drone");
            while (!int.TryParse(Console.ReadLine(), out number));
            update = (Update)number;
            int customerId, baseStationId, droneId;
            string newDroneModel,baseStationName, customerName, phoneNumber,chargeSlots;
            TimeSpan time;
            switch (update)
            {
                case Update.DroneName:
                    Console.WriteLine("please enter the id of the drone");
                   while(! int.TryParse(Console.ReadLine(), out droneId));
                    Console.WriteLine("please enter the new model name of the drone");
                    newDroneModel = Console.ReadLine();
                    try
                    {
                        bl.ChangeDroneModel(droneId, newDroneModel);
                    }
                    catch (UpdateProblemException ex)
                    {
                        Console.WriteLine(ex);
                    }
                    break;
                case Update.BaseStationUpDate:
                    Console.WriteLine("please enter base station ID to update");
                    while (!int.TryParse(Console.ReadLine(), out baseStationId));
                    Console.WriteLine("please enter the updated base station name if there isnt send an empty line");
                    baseStationName = Console.ReadLine();//אם נשלח ריק השדה לא מתעדכן
                    Console.WriteLine("please enter the updated amount of  Charge slots if there isnt send an empty line");
                    chargeSlots= Console.ReadLine();//אם נשלח ריק השדה לא מתעדכן
                    try
                    {
                        bl.UpdateBaseStaison(baseStationId, baseStationName, chargeSlots);
                    }
                    catch (UpdateProblemException ex)
                    {
                        Console.WriteLine(ex);
                    }
                    break;
                case Update.CustomerUpDate:
                    Console.WriteLine("please enter the updated id of the customer ");
                    while(!int.TryParse(Console.ReadLine(), out customerId));
                    Console.WriteLine("please enter the updated customer's name if there isnt send an empty line");
                    customerName = Console.ReadLine();//אם נשלח ריק השדה לא מתעדכן
                    Console.WriteLine("please enter the  updated customer's phone number if there isnt send an empty line");
                    phoneNumber=Console.ReadLine();//אם נשלח ריק השדה לא מתעדכן
                    try
                    {
                        bl.UpdateCustomer(customerId, customerName, phoneNumber);
                    }
                    catch (UpdateProblemException ex)
                    {
                        Console.WriteLine(ex);
                    }
                    break;
                case Update.InChargingDrone:
                    Console.WriteLine("please enter the id of the drone");
                   while(! int.TryParse(Console.ReadLine(), out droneId));
                    try
                    {
                        bl.SendDroneToCharge(droneId);
                    }
                    catch (UpdateProblemException ex)
                    {
                        Console.WriteLine(ex);
                    }
                    break;
                case Update.OutChargingDrone:
                    Console.WriteLine("please enter the id of the drone");
                    while(!int.TryParse(Console.ReadLine(), out droneId));
                    Console.WriteLine("Please enter the length of time the drone has been charging:");
                    TimeSpan.TryParse(Console.ReadLine(), out time);
                    try
                    {
                        bl.ReleaseFromCharging(droneId, time);
                    }
                    catch (UpdateProblemException ex)
                    {
                        Console.WriteLine(ex);
                    }
                    break;
                case Update.AssignDrone:
                    Console.WriteLine("please enter the id of the drone");
                    while (!int.TryParse(Console.ReadLine(), out droneId)) ;
                    try
                    {
                        bl.AssignParcelToDrone(droneId);
                    }
                    catch (UpdateProblemException ex)
                    {
                        Console.WriteLine(ex);
                    }
                    break;
                case Update.PickUp:
                    Console.WriteLine("please enter the id of the drone");
                    while (!int.TryParse(Console.ReadLine(), out droneId)) ;
                    try
                    { 
                        bl.PickUpParcelByDrone(droneId);
                    }
                    catch (UpdateProblemException ex)
                    {
                        Console.WriteLine(ex);
                    }
                    break;
                case Update.Deliver:
                    Console.WriteLine("please enter the id of the drone");
                    while (!int.TryParse(Console.ReadLine(), out droneId)) ;
                    try
                    {
                        bl.DroneDeliverParcel(droneId);
                    }
                    catch (UpdateProblemException ex)
                    {
                        Console.WriteLine(ex);
                    }
                    break;
                default:
                    Console.WriteLine(@"you entered a wrong number.
please choose again");
                    break;
            }
        }
        #endregion update options

        #region display options
        /// <summary>
        /// The function handles display options.
        /// </summary>
        /// <param name="dal">DalObject object that is passed as a parameter to enable the functions in the DalObject class</param>
        static public void DisplayOptions(IBL.IBL bl)
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
            while (!int.TryParse(Console.ReadLine(), out number));
            display = (Display)number;
            switch (display)
            {
                case Display.ViewBaseStation:
                    Console.WriteLine("please enter the id of the base station ");
                    while (!int.TryParse(Console.ReadLine(), out idForAllObjects));
                    try 
                    { 
                    Console.WriteLine(bl.GetBaseStation(idForAllObjects).ToString());
                    }
                    catch (GetDetailsProblemException ex)
                    {
                        Console.WriteLine(ex);
                    }
                    break;
                case Display.ViewDrone:
                    Console.WriteLine("please enter the id of the drone ");
                    while (!int.TryParse(Console.ReadLine(), out idForAllObjects));
                    try
                    {
                        Console.WriteLine(bl.GetDrone(idForAllObjects).ToString());
                    }
                    catch (GetDetailsProblemException ex)
                    {
                        Console.WriteLine(ex);
                    }
                    break;
                case Display.ViewCustomer:
                    Console.WriteLine("please enter the id of the customer ");
                    while (!int.TryParse(Console.ReadLine(), out idForAllObjects));
                    try
                    {
                        Console.WriteLine(bl.GetCustomer(idForAllObjects).ToString());
                    }
                    catch (GetDetailsProblemException ex)
                    {
                        Console.WriteLine(ex);
                    }
                    break;
                case Display.ViewParcel:
                    Console.WriteLine("please enter the id of the parcel ");
                    while (!int.TryParse(Console.ReadLine(), out idForAllObjects));
                    try
                    {
                        Console.WriteLine(bl.GetParcel(idForAllObjects).ToString());
                    }
                    catch (GetDetailsProblemException ex)
                    {
                        Console.WriteLine(ex);
                    }
                    break;
                default:
                    Console.WriteLine(@"you entered a wrong number.
please try again");
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
        public static void PrintList<T>(List<T> PrintList) where T : class
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
        static public void DisplayListsOptions(IBL.IBL bl)
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
                    PrintList(bl.GetBaseStationList().ToList());                     
                    break;
                case ViewList.ListDrone:
                    PrintList(bl.GetDroneList().ToList());  
                    break;
                case ViewList.ListCustomer:
                    PrintList( bl.GetCustomerList().ToList()) ;
                    break;
                case ViewList.ListParcel:
                    PrintList(bl.GetParcelList().ToList()) ;                     
                    break;
                case ViewList.ListFreeParcel:
                    PrintList(bl.GetParcelList(x => x.Status == ParcelStatus.Requested).ToList());
                    break;
                case ViewList.ListFreeChargeSlotsInBaseStation:
                    PrintList(bl.GetBaseStationList(x => x.FreeChargeSlots > 0).ToList());
                    break;
                default:
                    Console.WriteLine(@"you entered a wrong number.
please try again");
                    break;
            }


        }
        #endregion display lists options


        #endregion main options
        static void Main(string[] args)
       {
            
            IBL.IBL BlObject = new IBL.BL();
            int number = 0;
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
                        AddOptions(BlObject);
                        break;
                    case Choice.Update:
                        UpDateOptions(BlObject);
                        break;
                    case Choice.Display:
                        DisplayOptions(BlObject);
                        break;
                    case Choice.ViewLists:
                        DisplayListsOptions(BlObject);
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
    
