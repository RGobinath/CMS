using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.Assess.ReportCardClasses
{
    public class ReportCardCBSECo_Sch_LitAndSkills
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long RptId { get; set; }
        [DataMember]
        public virtual long RptRequestId { get; set; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
        [DataMember]
        public virtual string NewId { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string Subject { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string Term { get; set; }

        [DataMember]
        public virtual string LndCS_1 { get; set; }
        [DataMember]
        public virtual string LndCS_2 { get; set; }
        [DataMember]
        public virtual string LndCS_3 { get; set; }
        [DataMember]
        public virtual string LndCS_4 { get; set; }
        [DataMember]
        public virtual string LndCS_5 { get; set; }
        [DataMember]
        public virtual string LndCS_6 { get; set; }
        [DataMember]
        public virtual string LndCS_7 { get; set; }
        [DataMember]
        public virtual string LndCS_8 { get; set; }
        [DataMember]
        public virtual string LndCS_9 { get; set; }
        [DataMember]
        public virtual string LndCS_10 { get; set; }
        [DataMember]
        public virtual string LndCS_Total { get; set; }
        [DataMember]
        public virtual string LndCS_Average { get; set; }
        [DataMember]
        public virtual string LndCS_Grade { get; set; }
    }
}
