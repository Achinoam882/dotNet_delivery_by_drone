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
        #region add base station
        /// <summary>
        /// The function adds a station to the list of Basestations.
        /// </summary>
        /// <param name="newbaseStation"></param>
        public void SetBaseStation(BaseStation newBaseStation)
        {
            //exception of the logical layer//name??
            if (newBaseStation.Id < 0)
                throw new AddingProblemException("ID cant be a negative number");
            if (newBaseStation.FreeChargeSlots < 0)
                throw new AddingProblemException("free charge slots cant be a negative number");
            if (newBaseStation.BaseStationLocation.Latitude < 34.3 || newBaseStation.BaseStationLocation.Longitude > 35.5)
                throw new AddingProblemException("the location that was chosen isnt in the country");
            DO.BaseStation baseStationDal = new DO.BaseStation()
            {
                Id = newBaseStation.Id,
                ChargeSlots = newBaseStation.FreeChargeSlots,
                Name = newBaseStation.Name,
                Longitude = newBaseStation.BaseStationLocation.Longitude,
                Latitude = newBaseStation.BaseStationLocation.Latitude,

            };
            try
            {
                dalObject.SetBaseStation(baseStationDal);
            }
            catch (DO.AddExistingObjectException )
            {
                throw new AddingProblemException("Base station excits already");

            }
        }
        #endregion add base station

        #region update Base Staison
        /// <summary>
        /// The function update the name and charge slots of station .
        /// </summary>
      
        public void UpdateBaseStaison(int baseStationId, string baseStationName, string chargeSlots)
        {
            DO.BaseStation newBaseStation = new DO.BaseStation();
            try
            {
                newBaseStation = dalObject.GetBaseStation(baseStationId);
                if (baseStationName != "")
                {
                    newBaseStation.Name = baseStationName;
                }
            }
            catch (DO.NonExistingObjectException)
            {
                throw new UpdateProblemException("ID base station  doesnt exists in the system");
            }
           
            if (chargeSlots != "")
                {
                int chargeSlotsTottal, usedChargeSlotsTottal;
                while (!int.TryParse(chargeSlots, out chargeSlotsTottal)) ;
                    usedChargeSlotsTottal = dalObject.GetChargeSlotsList(x => x.StationId == baseStationId).ToList().Count;
                    if ((chargeSlotsTottal - usedChargeSlotsTottal) < 0)
                    {
                        throw new UpdateProblemException("number of charging slotes were not received");
                    }
                    newBaseStation.ChargeSlots = chargeSlotsTottal - usedChargeSlotsTottal;
                }
            dalObject.UpDateBaseStation(newBaseStation);

        }

        #endregion update Base Staison

        #region display base station
        /// <summary>
        /// The function return a base station  .
        /// </summary>
        /// <param name="idForDisplayBaseStation">id of station to display</param>
        public BaseStation GetBaseStation(int idForDisplayBaseStation)
        {
            DO.BaseStation dalBase = new DO.BaseStation();
            try
            {
                dalBase = dalObject.GetBaseStation(idForDisplayBaseStation);
            }
            catch (DO.NonExistingObjectException)
            {
                throw new GetDetailsProblemException("ID  doesnt exists");
            }
            //Location dalBaseStationLoc = new Location() { Longitude = dalBase.Longitude, Latitude = dalBase.Latitude };
            BaseStation blBaseStation = new BaseStation()
            {
                Id = dalBase.Id,
                Name = dalBase.Name,
                BaseStationLocation = new Location() { Longitude = dalBase.Longitude, Latitude = dalBase.Latitude },
                FreeChargeSlots = dalBase.ChargeSlots,
            };
            blBaseStation.DroneChargingList=from item in dalObject.GetChargeSlotsList(i => i.StationId == idForDisplayBaseStation)
                                            select new DroneCharging { Id = item.DroneId, Battery = dronesListBL.Find(x => x.Id == item.DroneId).Battery };
            return blBaseStation;
        }
        #endregion display base station

        #region display base station list
        /// <summary>
        /// The function return a list of  base stations  .
        /// </summary>
        public IEnumerable<BaseStationToList> GetBaseStationList(Predicate<BaseStationToList> predicate = null)
        {
            IEnumerable<BaseStationToList> baseStationBL = from item in dalObject.GetBaseStationList()
                                                           select new BaseStationToList()
                                                           {
                                                               Id = item.Id,
                                                               Name = item.Name,
                                                               FreeChargeSlots = item.ChargeSlots,
                                                               TakenChargeSlots = dalObject.GetChargeSlotsList(x => x.StationId == item.Id).Count()
                                                           };
            return baseStationBL.Where(x => predicate == null ? true : predicate(x));
     }
        #endregion display base station list

        

    }
}
