using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.BioMetricsEntities
{
    public class Staff_AttendanceConfigurationsReport_Vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string IdNumber { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Designation { get; set; }
        [DataMember]
        public virtual string Department { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual string StaffType { get; set; }
        [DataMember]
        public virtual string StaffUserName { get; set; }
        [DataMember]
        public virtual string IdNumberToEmployeeCode { get; set; }
        [DataMember]
        public virtual long StaffBioMetricId { get; set; }
        [DataMember]
        public virtual string IsHavingICMSAccount { get; set; }
        [DataMember]
        public virtual string IsAttendanceConfigured { get; set; }
        [DataMember]
        public virtual string IsAttendanceMappedInICMS { get; set; }
        [DataMember]
        public virtual long EmployeeId { get; set; }
        [DataMember]
        public virtual string StaffCategoryForAttendane { get; set; }
    }
}
