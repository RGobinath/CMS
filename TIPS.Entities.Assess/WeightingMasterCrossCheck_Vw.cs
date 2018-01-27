using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.Assess
{
    [DataContract]
    public class WeightingMasterCrossCheck_Vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string Subject { get; set; }
        [DataMember]
        public virtual string ComponentTitle { get; set; }
        [DataMember]
        public virtual decimal TotalMarks { get; set; }
        [DataMember]
        public virtual decimal Weightings { get; set; }
        [DataMember]
        public virtual long AssignmentId { get; set; }
        [DataMember]
        public virtual string AssignmentType { get; set; }
        [DataMember]
        public virtual bool IsMatched { get; set; }
    }
}
