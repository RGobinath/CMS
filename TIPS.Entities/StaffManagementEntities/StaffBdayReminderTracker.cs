using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StaffManagementEntities
{
   public class StaffBdayReminderTracker
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string SentList { get; set; }
        [DataMember]
        public virtual string CheckDate { get; set; }
        [DataMember]
        public virtual bool IsSent { get; set; }
        [DataMember]
        public virtual string RemainderType { get; set; }
    }
}
