using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StaffEntities
{
   [DataContract]
    public class StaffIssueTypeMaster
    {
       [DataMember]
       public virtual long Id { get; set; }
       [DataMember]
       public virtual string FormCode { get; set; }
       [DataMember]
       public virtual string IssueGroup { get; set; }
       [DataMember]
       public virtual string IssueType { get; set; }
    }
}
