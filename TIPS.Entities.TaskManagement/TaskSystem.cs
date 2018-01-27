using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.TaskManagement
{
   public class TaskSystem
    {
        public virtual long Id { get; set; }
        public virtual string TaskNo { get; set; }
      //  public virtual string Module { get; set; }
        public virtual string TaskType { get; set; }
        public virtual string Severity { get; set; }
        public virtual string Priority { get; set; }
        public virtual string Status { get; set; }
        public virtual string TaskStatus { get; set; }
        public virtual string Comments { get; set; }
        public virtual string Summary { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string Reporter { get; set; }
        public virtual string AssignedTo { get; set; }
        public virtual long InstanceId { get; set; }
        [DataMember]
        public virtual string BranchCode { get; set; }
        [DataMember]
        public virtual string DeptCode { get; set; }
        public virtual bool IsTaskCompleted { get; set; }
        [DataMember]
        public virtual TimeSpan? DifferenceInHours { get; set; }
    }
}
