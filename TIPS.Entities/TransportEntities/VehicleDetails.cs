using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.TransportEntities
{
    public class VehicleDetails
    {
       [DataMember]
       public virtual int Id { get; set; }
       [DataMember]
       public virtual int VehicleTypeId { get; set; }
       [DataMember]
       public virtual string Type { get; set; }
       [DataMember]
       public virtual string VehicleNo { get; set; }
       [DataMember]
       public virtual string Campus { get; set; }
       [DataMember]
       public virtual string Purpose { get; set; }
       [DataMember]
       public virtual string DriverName { get; set; }
       [DataMember]
       public virtual string FuelType { get; set; }
       [DataMember]
        //Added by anto
       public virtual bool IsActive { get; set; }
    }
}
