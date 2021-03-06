using System;
using System.Text;
using System.Collections.Generic;


namespace TIPS.Entities.TransportEntities {
    
    public class VehicleACMaintenance {
        public virtual int Id { get; set; }
        public virtual int VehicleId { get; set; }
        public virtual string VehicleNo { get; set; }
        public virtual string ACMaintenanceType { get; set; }
        public virtual string ACBreakdownLocation { get; set; }
        public virtual DateTime? ACDateOfBreakdown { get; set; }
        public virtual string ACModel { get; set; }
        public virtual string ACMaintenanceDescription { get; set; }
        public virtual DateTime? ACPlannedDateOfService { get; set; }
        public virtual DateTime? ACActualDateOfService { get; set; }
        public virtual string ACServiceProvider { get; set; }
        public virtual decimal? ACServiceCost { get; set; }
        public virtual string ACServiceBillNo { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string ACSparePartsUsed { get; set; }
        public virtual string AM_SparePartsUsedfile { get; set; }
    }
}
