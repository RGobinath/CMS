using System;
using System.Text;
using System.Collections.Generic;


namespace TIPS.Entities.TransportEntities {
    
    public class VehicleBodyMaintenance {
        public virtual int Id { get; set; }
        public virtual int VehicleId { get; set; }
        public virtual string VehicleNo { get; set; }
        public virtual string BTypeOfBody { get; set; }
        public virtual DateTime? BDateOfRepair { get; set; }
        public virtual string BTypeOfRepair { get; set; }
        public virtual string BPartsRequired { get; set; }
        public virtual string BServiceProvider { get; set; }
        public virtual decimal? BServiceCost { get; set; }
        public virtual string BBillNo { get; set; }
        public virtual string BDescription { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string BM_SparePartsUsedfile { get; set; }
    }
}
