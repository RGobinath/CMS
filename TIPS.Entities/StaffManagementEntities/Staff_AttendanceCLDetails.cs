using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.StaffManagementEntities
{
    public class Staff_AttendanceCLDetails
    {
        public virtual long Staff_AttendanceCLBalance_Id { get; set; }
        public virtual long PreRegNum { get; set; }
        public virtual long Month { get; set; }
        public virtual long Year { get; set; }
        public virtual decimal OpeningCLBalance { get; set; }
        public virtual decimal AllotedCL { get; set; }
        public virtual decimal TotalAvailableBalane { get; set; }
        public virtual decimal? NoOfLeavesTaken { get; set; }
        public virtual decimal? ClosingBalance { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual decimal? LeaveToBeCalculated { get; set; }
        public virtual string Remarks { get; set; }
    }
}
