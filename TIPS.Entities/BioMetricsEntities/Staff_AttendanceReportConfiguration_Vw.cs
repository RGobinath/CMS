using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.BioMetricsEntities
{
    public class Staff_AttendanceReportConfiguration_Vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long StaffPreRegNum { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Department { get; set; }
        [DataMember]
        public virtual string Designation { get; set; }
        [DataMember]
        public virtual long Staff_AttendanceReportConfig_Id { get; set; }
        [DataMember]
        public virtual long ReportingHeadPreRegNum { get; set; }
        [DataMember]
        public virtual string ReportingHeadDesignation { get; set; }
        [DataMember]
        public virtual string ReportingHeadName { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime? ModifiedDate { get; set; }
    }
}
