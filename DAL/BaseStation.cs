using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public struct BaseStation
    {
        public int id;
        public string Name { get; set; }
        public double longitute;
        public double latitude;
        public int chargeslots;
    }
}
