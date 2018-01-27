using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StoreEntities
{
     [DataContract]
   public class UOM_ConversionMatrix
    {
         [DataMember]
         public virtual int Id { get; set; }
         [DataMember]
         public virtual int BaseQuantity { get; set; }
         [DataMember]
         public virtual string BaseUnit { get; set; }
         [DataMember]
         public virtual int ConversionQuantity { get; set; }
         [DataMember]
         public virtual string ConversionUnit { get; set; }
    }
}
