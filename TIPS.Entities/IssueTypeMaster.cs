using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities
{
   [DataContract]
    public class IssueTypeMaster
    {
       [DataMember]
       public virtual long FormId { get; set; }
       [DataMember]
       public virtual string FormCode { get; set; }
       [DataMember]
       public virtual string IssueGroup { get; set; }
       [DataMember]
       public virtual string IssueType { get; set; }
    }
}
