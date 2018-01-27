using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.Attendance
{
    [DataContract]
    public class MonthMasterForAttendance
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string MonthName { get; set; }
        [DataMember]
        public virtual string MonthCode { get; set; }
        [DataMember]
        public virtual string Code { get; set; }
        
    }
}
