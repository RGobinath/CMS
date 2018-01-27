using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities
{
    public class IssueCountReportByIssueGroup_SP
    {
        [DataMember]
        public virtual Int64 Id { get; set; }
        [DataMember]
        public virtual string BranchCode { get; set; }
        [DataMember]
        public virtual string IssueGroup { get; set; }
        [DataMember]
        public virtual Int64 Logged { get; set; }
        [DataMember]
        public virtual Int64 Completed { get; set; }
        [DataMember]
        public virtual Int64 NonCompleted { get; set; }
        [DataMember]
        public virtual Int64 ResolveIssue { get; set; }
        [DataMember]
        public virtual Int64 ApproveIssue { get; set; }
        [DataMember]
        public virtual Int64 Complete { get; set; }
    }
}