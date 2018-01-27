using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.BioMetricsEntities
{
    public class Staff_AutoEmailNotificationLogForAttendance
    {
        [DataMember]
        public virtual long Staff_AutoEmailNotificationLogForAttendance_Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string MailFor { get; set; }
        [DataMember]
        public virtual string AttendanceDate { get; set; }
        [DataMember]
        public virtual DateTime? LoggedDateAndTime { get; set; }
    }
}
