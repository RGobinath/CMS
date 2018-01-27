using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StudentsReportEntities
{
    public class MISMonthlyReport_Vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual long Flag { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual long Boys { get; set; }
        [DataMember]
        public virtual long Girls { get; set; }
        [DataMember]
        public virtual long Total { get; set; }
        [DataMember]
        public virtual DateTime CreatedDateNew { get; set; }
    }
}
