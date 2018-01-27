using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.StaffEntities
{
    public class StaffIssueManagementReport_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Performer { get; set; }
        [DataMember]
        public virtual long ProcessRefId { get; set; }
        [DataMember]
        public virtual long InstanceId { get; set; }
        [DataMember]
        public virtual long Available { get; set; }
        [DataMember]
        public virtual long Assigned { get; set; }
        [DataMember]
        public virtual long Resolved { get; set; }
        [DataMember]
        public virtual string IssueNumber { get; set; }
        [DataMember]
        public virtual string BranchCode { get; set; }
        [DataMember]
        public virtual string IssueGroup { get; set; }
        [DataMember]
        public virtual string IssueType { get; set; }
        [DataMember]
        public virtual string Description { get; set; }
        [DataMember]
        public virtual string Resolution { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual bool IsIssueCompleted { get; set; }
        [DataMember]
        public virtual DateTime? DueDate { get; set; }
        [DataMember]
        public virtual DateTime? AssignedDate { get; set; }
        [DataMember]
        public virtual DateTime? ModifiedDate { get; set; }
        [DataMember]
        public virtual TimeSpan? DifferenceInHours { get; set; }
        [DataMember]
        public virtual TimeSpan? DifferenceInHours1 { get; set; }
        [DataMember]
        public virtual TimeSpan? DifferenceInHours2 { get; set; }
        [DataMember]
        public virtual long TotalCount { get; set; }

    }
}
