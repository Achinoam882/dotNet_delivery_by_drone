using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class CustomerToList
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }
        public int ParcelProvided { get; set; }//מס חבילות ששולחו וסופקו
        public int Parcelsnet { get; set; }//not provided מס חבילות ששולחו ועוד לא סופקו
        public int ParcelReceived { get; set; } //מס מס חבילות שקיבל
        public int ParcelOnTheWay { get; set; }//on the way to customer מס חבילות בדרך אליו
        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}
