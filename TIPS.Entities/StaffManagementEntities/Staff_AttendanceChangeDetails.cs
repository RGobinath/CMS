using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.StaffManagementEntities
{
    public class Staff_AttendanceChangeDetails
    {
        [DataMember]
        public virtual long Staff_AttendanceChangeDetails_Id { get; set; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
        [DataMember]
        public virtual long Month { get; set; }
        [DataMember]
        public virtual long Year { get; set; }
        [DataMember]
        public virtual long TotalNoOfDaysWorkedByLogs { get; set; }
        [DataMember]
        public virtual decimal TotalNoOfDaysWorkedByChange { get; set; }
        [DataMember]
        public virtual long AllowedCasualLeaves { get; set; }
        [DataMember]
        public virtual long TotalNoOfLeavesTakenByLogs { get; set; }
        [DataMember]
        public virtual decimal TotalNoOfLeavesTakenByChange { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime? ModifiedDate { get; set; }
        [DataMember]
        public virtual bool IsActive { get; set; }
        [DataMember]
        public virtual decimal OnDuty { get; set; }
        [DataMember]
        public virtual decimal NoOfPermissionsTaken { get; set; }
        [DataMember]
        public virtual decimal NoOfLeavesCalculatedByPermissions { get; set; }
        [DataMember]
        public virtual long NoOfHolidays { get; set; }
        [DataMember]
        public virtual decimal CasualLeavesTaken { get; set; }
        [DataMember]
        public virtual decimal NoOfLeaveToBeCalculated { get; set; }
    }
}
