using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.TransportEntities
{
    public class VehicleMaintance_Vw
    {
         public virtual int Id { get; set; }
        public virtual int VehicleId { get; set; }
        public virtual string Campus { get; set; }
        public virtual string VehicleNo { get; set; }
        public virtual string VehicleMaintenanceType { get; set; }
        public virtual string VehicleBreakdownLocation { get; set; }
        public virtual DateTime? VehicleDateOfBreakdown { get; set; }
        public virtual DateTime? VehiclePlannedDateOfService { get; set; }
        public virtual DateTime? VehicleActualDateOfService { get; set; }
        public virtual string VehicleServiceProvider { get; set; }
        public virtual decimal? VehicleSeviceCost { get; set; }
        public virtual string VehicleServiceBillNo { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string VehicleSparePartsUsed { get; set; }
        public virtual string VM_SparePartsUsedfile { get; set; }
    
    }
}
