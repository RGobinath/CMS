using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities
{
    public class PerformerWiseIssueCountReport_SP
    {
        [DataMember]
        public virtual Int64 Id { get; set; }
        [DataMember]
        public virtual string BranchCode { get; set; }
        [DataMember]
        public virtual string Performer { get; set; }
        [DataMember]
        public virtual Int64 Resolved { get; set; }
        [DataMember]
        public virtual Int64 Assigned { get; set; }
        [DataMember]
        public virtual Int64 Rejected { get; set; }
        [DataMember]
        public virtual Int64 Completed { get; set; }        
    }
}
