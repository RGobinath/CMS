using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.TaskManagement
{
    public class TaskStatus
    {
        [DataMember]
        public virtual long StatusId { get; set; }
        [DataMember]
        public virtual string StatusCode { get; set; }
        [DataMember]
        public virtual string StatusName { get; set; }
    }
}
