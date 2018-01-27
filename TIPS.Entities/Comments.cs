using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities
{
     [DataContract]
   public class Comments
    {
        [DataMember]
        public virtual long CommentId { get; set; }
        [DataMember]
        public virtual long EntityRefId { get; set; }
        [DataMember]
        public virtual DateTime CommentedOn { get; set; }
        [DataMember]
        public virtual string CommentedBy { get; set; }
        [DataMember]
        public virtual string RejectionComments { get; set; }
        [DataMember]
        public virtual string ResolutionComments { get; set; }
        [DataMember]
        public virtual string AppName { get; set; }
    }
}
