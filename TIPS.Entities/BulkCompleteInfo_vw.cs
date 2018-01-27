using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities
{
    [DataContract]
    public class BulkCompleteInfo
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string ActivityName { get; set; }
        [DataMember]
        public virtual bool Completed { get; set; }
        [DataMember]
        public virtual long ProcessRefId { get; set; }
        [DataMember]
        public virtual long InstanceId { get; set; }
        [DataMember]
        public virtual string IssueNumber { get; set; }
        [DataMember]
        public virtual string ActivityFullName { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual string Email { get; set; }
        [DataMember]
        public virtual bool IsIssueCompleted { get; set; }
        [DataMember]
        public virtual bool IsInformation { get; set; }
        [DataMember]
        public virtual DateTime? ActionDate { get; set; }
       
    }
}
