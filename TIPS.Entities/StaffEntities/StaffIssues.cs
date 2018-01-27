using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StaffEntities
{
    [DataContract]
    public class StaffIssues
    {        
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string IssueGroup { get; set; }
        [DataMember]
        public virtual string IssueType { get; set; }
        [DataMember]
        public virtual string Description { get; set; }
        [DataMember]
        public virtual string IssueNumber { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime CreatedDate { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual string ActivityFullName { get; set; }
        [DataMember]
        public virtual string UserRoleName { get; set; }
        [DataMember]
        public virtual long InstanceId { get; set; }
        [DataMember]
        public virtual string BranchCode { get; set; }
        [DataMember]
        public virtual string DeptCode { get; set; }
        [DataMember]
        public virtual bool IsIssueCompleted { get; set; }
        [DataMember]
        public virtual string Resolution { get; set; }
        [DataMember]
        public virtual TimeSpan? DifferenceInHours { get; set; }
        [DataMember]
        public virtual int Hours { get; set; }
        [DataMember]
        public virtual string CreatedUserName { get; set; }
        //Added By Gobinath For Ticket-377
        [DataMember]
        public virtual string StaffName { get; set; }
        ///Assigning Issue to User for
        [DataMember]
        public virtual long ActivityId { get; set; }
        //Added By Prabakaran
        [DataMember]
        public virtual DateTime? DueDate { get; set; }
        [DataMember]
        public virtual DateTime? AssignedDate { get; set; }
    }
}
