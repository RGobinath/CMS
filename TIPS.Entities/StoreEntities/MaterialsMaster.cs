﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StoreEntities
{
   [DataContract]
   public class MaterialsMaster
    {
         [DataMember]
         public virtual long Id { get; set; }
         [DataMember]
         public virtual long MaterialGroupId { get; set; }
         [DataMember]
         public virtual long MaterialSubGroupId { get; set; }
         [DataMember]
         public virtual string Material { get; set; }
         [DataMember]
         public virtual string UnitCode { get; set; }
         [DataMember]
         public virtual string ItemCode { get; set; }
         [DataMember]
         public virtual string Notes { get; set; }
         [DataMember]
         public virtual string ItemLocation { get; set; }
         [DataMember]
         public virtual bool IsActive { get; set; }
    }
}
