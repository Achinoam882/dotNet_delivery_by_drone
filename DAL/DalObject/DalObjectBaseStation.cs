using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using IDAL;

namespace DalObject
{
    public partial class DalObject:IDal
    {
        #region add base station
        /// <summary>
        /// The function adds a station to the list of Basestations.
        /// </summary>
        /// <param name="newbaseStation"></param>
        public void SetBaseStation(BaseStation newBaseStation)
        {
            if (DataSource.BaseStationList.FindIndex(x => x.Id == newBaseStation.Id) != -1)
            {
                throw new AddExistingObjectException();
            }
            DataSource.BaseStationList.Add(newBaseStation);
        }
        #endregion add base station


        #region print base station
        /// <summary>
        /// The function returns the selected base station.
        /// </summary>
        /// <param name="idForAllObjects">Id of a selected BaseStation </param>
        /// <returns> return empty ubjact if its not there</returns>
        public BaseStation GetBaseStation(int idForAllObjects)
        {
            if (!(DataSource.BaseStationList.Exists(x => x.Id == idForAllObjects)))
            {
                throw new NonExistingObjectException();
            }
            return DataSource.BaseStationList.Find(x => x.Id == idForAllObjects);
        }
        #endregion print base station


        #region print base station list
        /// <summary>
        /// The function returns an array of all base stations.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public IEnumerable<BaseStation> GetBaseStationList()
        {
            return DataSource.BaseStationList.Take(DataSource.BaseStationList.Count).ToList();
        }
        #endregion add base station list


        #region print free charge slots base station list
        /// <summary>
        /// The function returns base stations with free charge positions.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public IEnumerable<BaseStation> GetBaseStationFreeChargeSlots()
        {
            return DataSource.BaseStationList.FindAll(x => x.ChargeSlots > 0).ToList();
        }
        #endregion print free charge slots base station list


        #region to update a BaseStation
        public void UpDateBaseStation(BaseStation newBaseStation)
        {
            DataSource.BaseStationList[DataSource.BaseStationList.FindIndex(x => x.Id == newBaseStation.Id)] = newBaseStation;
        }
        #endregion to update a BaseStation
    }
}
