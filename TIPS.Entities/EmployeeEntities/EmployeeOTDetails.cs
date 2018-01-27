using System;
using System.Text;
using System.Collections.Generic;


namespace TIPS.Entities.EmployeeEntities {
    
    public class EmployeeOTDetails {
        public virtual long Id { get; set; }
        public virtual long PreRegNum { get; set; }
        public virtual string Campus { get; set; }
        public virtual string EmployeeName { get; set; }
        public virtual string EmployeeIdNo { get; set; }
        public virtual DateTime OTDate { get; set; }
        public virtual string OTType { get; set; }
        public virtual decimal? Allowance { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
    }
}
