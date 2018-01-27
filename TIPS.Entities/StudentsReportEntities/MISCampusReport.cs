using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.StudentsReportEntities
{
    public class MISCampusReport
    {
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string ShowGrade { get; set; }
        [DataMember]
        public virtual string AcdYr { get; set; }
        [DataMember]
        public virtual long Boys { get; set; }
        [DataMember]
        public virtual long Girls { get; set; }
        [DataMember]
        public virtual long Total { get; set; }
        [DataMember]
        public virtual long OverAllTotal { get; set; }
        [DataMember]
        public virtual IList<MISCampusReport> MISCampusReportList { get; set; }
    }
}
