using System;
using System.Text;
using System.Collections.Generic;


namespace TIPS.Entities.TransportEntities {
    
    public class Permit {
        public virtual int Id { get; set; }
        public virtual int? VehicleId { get; set; }
        public virtual string VehicleNo { get; set; }
        public virtual string PermitNo { get; set; }
        public virtual string ValidIn { get; set; }
        public virtual DateTime? ValidFrom { get; set; }
        public virtual DateTime? ValidTo { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
    }
}
