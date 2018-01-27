using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.TransportEntities
{
   public class VehicleDistanceCovered
    {
       [DataMember]
       public virtual int Id { get; set; }
       [DataMember]
       public virtual string RefNo { get; set; }
       [DataMember]
       public virtual int RefId { get; set; }
       [DataMember]
       public virtual string Type { get; set; }
       [DataMember]
       public virtual int VehicleId { get; set; }
       [DataMember]
       public virtual DateTime? CreatedDate { get; set; }
       [DataMember]
       public virtual string CreatedBy { get; set; }
       [DataMember]
       public virtual DateTime? ModifiedDate { get; set; }
       [DataMember]
       public virtual string ModifiedBy { get; set; }
       [DataMember]
       public virtual string Campus { get; set; }
       [DataMember]
       public virtual string UserRole { get; set; }
       [DataMember]
       public virtual string Status { get; set; }
       //[DataMember]
       //public virtual DateTime? TripDate { get; set; }
       [DataMember]
       public virtual string Route { get; set; }
       [DataMember]
       public virtual string VehicleNo { get; set; }
       [DataMember]
       public virtual string DriverName { get; set; }
       [DataMember]
       public virtual string Source { get; set; }
       [DataMember]
       public virtual string Destination { get; set; }
       [DataMember]
       public virtual string Purpose { get; set; }
       [DataMember]
       public virtual DateTime? OutDateTime { get; set; }
       [DataMember]
       public virtual decimal KMOut { get; set; }
       [DataMember]
       public virtual DateTime? InDateTime { get; set; }
       [DataMember]
       public virtual decimal KMIn { get; set; }
       [DataMember]
       public virtual decimal DistanceCovered { get; set; }
       [DataMember]
       public virtual bool IsAnyService { get; set; }
       [DataMember]
       public virtual string ServiceCenterName { get; set; }
       [DataMember]
       public virtual bool IsKMReseted { get; set; }
       [DataMember]
       public virtual decimal KMResetValue { get; set; }
       [DataMember]
       public virtual string PurposeType { get; set; }
       [DataMember]
       public virtual string VehicleType { get; set; }
       [DataMember]
       public virtual TimeSpan? DifferenceInHours { get; set; }
       
    }
}
