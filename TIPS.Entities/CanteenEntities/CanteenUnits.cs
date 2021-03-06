﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.CanteenEntities
{
     [DataContract]
   public class CanteenUnits
    {
         [DataMember]
         public virtual long Id { get; set; }
         [DataMember]
         public virtual string Units { get; set; }
         [DataMember]
         public virtual string UnitCode { get; set; }
    }
}
