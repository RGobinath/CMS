using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities
{
    [DataContract]
    public class CallManagementChart
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long ProcessRefId { get; set; }
        [DataMember]
        public virtual string IssueGroup { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual string Performer { get; set; }
        [DataMember]
        public virtual bool IsInformation { get; set; }
        [DataMember]
        public virtual TimeSpan? DifferenceInHours { get; set; }
        [DataMember]
        public virtual int Hours { get; set; }
        [DataMember]
        public virtual DateTime IssueDate { get; set; }
        [DataMember]
        public virtual string InformationFor { get; set; }
        [DataMember]
        public virtual string BranchCode { get; set; }
    }
}
