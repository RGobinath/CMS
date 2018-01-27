using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StoreEntities
{
     [DataContract]
   public class StoreUnits
    {
         [DataMember]
         public virtual long Id { get; set; }
         [DataMember]
         public virtual string Units { get; set; }
         [DataMember]
         public virtual string UnitCode { get; set; }
    }
}
