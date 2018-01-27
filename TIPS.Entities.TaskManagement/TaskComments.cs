using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.TaskManagement
{
    public class TaskComments
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long TaskId { get; set; }
        [DataMember]
        public virtual string CommentedBy { get; set; }
        [DataMember]
        public virtual DateTime? CommentedOn { get; set; }
        [DataMember]
        public virtual string RejectionComments { get; set; }
        [DataMember]
        public virtual string ResolutionComments { get; set; }
        [DataMember]
        public virtual string Note { get; set; }
    }
}
