using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.BioMetricsEntities
{
    public class Staff_AttendanceReportConfigurationByStaffs
    {
        [DataMember]
        public virtual long Staff_AttendanceReportConfig_Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual long StaffPreRegNum { get; set; }
        [DataMember]
        public virtual long ReportingHeadPreRegNum { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime? ModifiedDate { get; set; }
    }
}
