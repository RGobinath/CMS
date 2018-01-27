using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.StaffManagementEntities
{
    public class Staff_AttendanceOpeningBalanceGenerateCount
    {
        public virtual long Id { get; set; }
        public virtual long LastMonth { get; set; }
        public virtual long LastYear { get; set; }
        public virtual long CurrentMonth { get; set; }
        public virtual long CurrentYear { get; set; }
        public virtual long TotalGenerateCount { get; set; }
        public virtual long ConsolidateCount { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
    }
}
