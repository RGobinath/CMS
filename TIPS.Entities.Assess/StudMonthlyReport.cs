using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.Assess
{
    [DataContract]
    public class StudMonthlyReport
    {
        [DataMember]
        public virtual int Id { get; set; }
        [DataMember]
        public virtual string StudName { get; set; }
        [DataMember]
        public virtual DateTime? ReportGenDate { get; set; }
        [DataMember]
        public virtual int Month { get; set; }
        [DataMember]
        public virtual Int32 Year { get; set; }
        [DataMember]
        public virtual string Character { get; set; }
        [DataMember]
        public virtual string AttPunctuality { get; set; }
        [DataMember]
        public virtual string HwCompletion { get; set; }
        [DataMember]
        public virtual string HwAccuracy { get; set; }
        [DataMember]
        public virtual string WkChapterTests { get; set; }
        [DataMember]
        public virtual string SLCParentAssessment { get; set; }
        [DataMember]
        public virtual string TermAssessment { get; set; }
    }
}
