using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities
{
   public class IssueGroupMaster
    {
        [DataMember]
        public virtual Int64 Id { get; set; }
        [DataMember]
        public virtual string FormCode { get; set; }
        [DataMember]
        public virtual string IssueGroup { get; set; }
        [DataMember]
        public virtual string ParentCode { get; set; }
    }
}
