using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.BioMetricsEntities
{
    public class Staff_DeviceLogSummarySP
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string UserId { get; set; }
        [DataMember]
        public virtual long EmployeeId { get; set; }
        [DataMember]
        public virtual DateTime? AttendanceDate { get; set; }
        [DataMember]
        public virtual DateTime? LogInTime { get; set; }
        [DataMember]
        public virtual DateTime? LogOutTime { get; set; }
        [DataMember]
        public virtual string WorkingHours { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
        [DataMember]
        public virtual string IdNumber { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string StaffType { get; set; }
        [DataMember]
        public virtual string Programme { get; set; }
        [DataMember]
        public virtual string Department { get; set; }
        [DataMember]
        public virtual string Designation { get; set; }
        [DataMember]
        public virtual string LogsType { get; set; }

    }
}
