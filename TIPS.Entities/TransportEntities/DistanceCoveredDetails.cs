using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.TransportEntities
{
   public class DistanceCoveredDetails
    {
       [DataMember]
       public virtual int Id { get; set; }
       [DataMember]
       public virtual string RefNo { get; set; }
       [DataMember]
       public virtual string ProcessedBy { get; set; }
       [DataMember]
       public virtual string UserRole { get; set; }
       [DataMember]
       public virtual string Status { get; set; }
       [DataMember]
       public virtual string Campus { get; set; }
       [DataMember]
       public virtual DateTime CreatedDate { get; set; }
       [DataMember]
       public virtual string Description { get; set; }
    }
}
