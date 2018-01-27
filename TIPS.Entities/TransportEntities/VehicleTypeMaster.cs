using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.TransportEntities
{
   public class VehicleTypeMaster
    {
       [DataMember]
       public virtual int Id { get; set; }
       [DataMember]
       public virtual string VehicleType { get; set; }
      
    }
}
