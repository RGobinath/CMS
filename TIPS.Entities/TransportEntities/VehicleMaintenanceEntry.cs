using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.TransportEntities
{
    public class VehicleMaintenanceEntry
    {
        [DataMember]
        public virtual long MaintenanceId { get; set; }
        [DataMember]
        public virtual long VehicleId { get; set; }
        [DataMember]
        public virtual string VehicleNo { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual bool AC { get; set; }
        [DataMember]
        public virtual bool Battery { get; set; }
        [DataMember]
        public virtual bool Tyre { get; set; }
        [DataMember]
        public virtual string SupplierName { get; set; }
        [DataMember]
        public virtual string InvoiceNo { get; set; }
        [DataMember]
        public virtual long Amount { get; set; }
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
        public virtual DateTime? VehicleTravelDate { get; set; }
        [DataMember]
        public virtual long VehicleCostId { get; set; }
        [DataMember]
        public virtual bool MechanicalMaintenance { get; set; }
        [DataMember]
        public virtual bool ElectricalMaintenance { get; set; }
        [DataMember]
        public virtual bool BodyMaintenance { get; set; }

    }
}
