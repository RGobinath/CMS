using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities
{
    public class TIPSMaster
    {

        public virtual long Id { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Section { get; set; }
        public virtual string AcademicYear { get; set; }
        public virtual Int32 NumStud { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }

    }
}
