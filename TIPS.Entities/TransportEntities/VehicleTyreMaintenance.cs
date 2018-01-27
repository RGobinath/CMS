using System;
using System.Text;
using System.Collections.Generic;


namespace TIPS.Entities.TransportEntities {
    
    public class VehicleTyreMaintenance {
        public virtual int Id { get; set; }
        public virtual int VehicleId { get; set; }
        public virtual string VehicleNo { get; set; }
        public virtual string TyreMaintenanceType { get; set; }
        public virtual string TyreLocation { get; set; }
        public virtual string TypeOfTyre { get; set; }
        public virtual string TyreMake { get; set; }
        public virtual string TyreModel { get; set; }
        public virtual string TyreSize { get; set; }
        public virtual DateTime? TyreAssignedDate { get; set; }
        public virtual decimal? TyreMilometerReading { get; set; }
        public virtual string TyreReasonForRemoving { get; set; }
        
        public virtual decimal? TyreCost { get; set; }
        public virtual string TyreNo { get; set; }
        public virtual string TyreBillNo { get; set; }
        public virtual DateTime? TyreDateOfAlignment { get; set; }
        public virtual DateTime? TyreDateOfRotation { get; set; }
        public virtual DateTime? TyreDateOfWheelService { get; set; }
        public virtual decimal? TyreServiceCost { get; set; }
        public virtual string TyreMaintenanceBillNo { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }

        public virtual DateTime? TyreDateOfService { get; set; }
        public virtual string TyreServiceProvider { get; set; }
        public virtual decimal? CostOfService { get; set; }
        public virtual string TyreServicedBy { get; set; }
        public virtual string TyreServiceBillNo { get; set; }

    }
}
