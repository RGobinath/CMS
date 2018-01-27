using System;
using System.Text;
using System.Collections.Generic;

namespace TIPS.Entities.TransportEntities
{
    public class VehicleElectricalMaintenance
    {
        public virtual int Id { get; set; }
        public virtual int VehicleId { get; set; }
        public virtual string VehicleNo { get; set; }
        public virtual DateTime? EDateOfService { get; set; }
        public virtual string EServiceProvider { get; set; }
        public virtual decimal? EServiceCost { get; set; }
        public virtual string EBillNo { get; set; }
        public virtual string ESparePartsUsed { get; set; }
        public virtual string EDescription { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string EM_SparePartsUsedfile { get; set; }
    }
}
