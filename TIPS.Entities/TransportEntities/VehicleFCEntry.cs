using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.TransportEntities
{
    public class VehicleFCEntry
    {
        [DataMember]
        public virtual long FCId { get; set; }
        [DataMember]
        public virtual long VehicleId { get; set; }
        [DataMember]
        public virtual string VehicleNo { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Vendor { get; set; }
        [DataMember]
        public virtual string InvoiceNo { get; set; }
        [DataMember]
        public virtual string Description { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime? ModifiedDate { get; set; }
        [DataMember]
        public virtual DateTime? EntryDate { get; set; }
        [DataMember]
        public virtual long VehicleCostId { get; set; }
    }
}
