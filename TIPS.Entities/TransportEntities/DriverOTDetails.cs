using System;
using System.Text;
using System.Collections.Generic;


namespace TIPS.Entities.TransportEntities {
    
    public class DriverOTDetails {
        public virtual int? Id { get; set; }
        public virtual string Campus { get; set; }
        public virtual string DriverName { get; set; }
        public virtual string DriverIdNo { get; set; }
        public virtual DateTime OTDate { get; set; }
        public virtual string OTType { get; set; }
        public virtual decimal? Allowance { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
    }
}
