﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
   public class BaseStationToList
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int FreeChargeSlots { get; set; }//מספר עמדות טעינה פנויות 
        public int TakenChargeSlots { get; set; }//  תפוסות מספר עמדות טעינה 
        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}
