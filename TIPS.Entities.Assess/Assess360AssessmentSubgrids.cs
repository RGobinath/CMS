using System;
using System.Runtime.Serialization;
namespace TIPS.Entities.Assess
{
    [DataContract]
    public class Assess360AssessmentSubgrids
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual int AssessCompGroup { get; set; }
        [DataMember]
        public virtual string Staff { get; set; }
        [DataMember]
        public virtual string StudentName { get; set; }
        [DataMember]
        public virtual string Subject { get; set; }

        [DataMember]
        public virtual string IDNo { get; set; }

        [DataMember]
        public virtual string AssignmentName { get; set; }
        [DataMember]
        public virtual string Mark { get; set; }
        [DataMember]
        public virtual string MarksOutOff { get; set; }

        [DataMember]
        public virtual string EnteredBy { get; set; }

        [DataMember]
        public virtual DateTime DateCreated { get; set; }

        [DataMember]
        public virtual string ConsolidatedMarks { get; set; }

       
        
    }
}
