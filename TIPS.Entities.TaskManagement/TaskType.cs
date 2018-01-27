using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.TaskManagement
{
   public class TaskType
    {
        [DataMember]
        public virtual long TaskTypeId { get; set; }
        [DataMember]
        public virtual string TaskTypeCode { get; set; }
        [DataMember]
        public virtual string TaskTypeName { get; set; }
    }
}
