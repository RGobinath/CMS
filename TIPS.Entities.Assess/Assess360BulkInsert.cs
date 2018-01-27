using System.Collections.Generic;
using System.Text;
using System;
using System.Runtime.Serialization;

namespace TIPS.Entities.Assess
{
    [DataContract]
    public class Assess360BulkInsert
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long Assess360Id { get; set; }
        [DataMember]
        public virtual long A360CompId { get; set; }
        [DataMember]
        public virtual int AssessCompGroup { get; set; }
        [DataMember]
        public virtual bool? IsCredit { get; set; }
        [DataMember]
        public virtual string GroupName { get; set; }
        [DataMember]
        public virtual DateTime? IncidentDate { get; set; }
        [DataMember]
        public virtual DateTime? DateCreated { get; set; }
        [DataMember]
        public virtual decimal? Mark { get; set; }
        [DataMember]
        public virtual string Description { get; set; }
        [DataMember]
        public virtual string Subject { get; set; }
        [DataMember]
        public virtual string Staff { get; set; }
        [DataMember]
        public virtual string AssignmentName { get; set; }
        [DataMember]
        public virtual string MarksOutOff { get; set; }
        [DataMember]
        public virtual string EnteredBy { get; set; }
        [DataMember]
        public virtual DateTime? DateModified { get; set; }
        [DataMember]
        public virtual string RequestNo { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string IdNo { get; set; }
        [DataMember]
        public virtual string ConsolidatedMarks { get; set; }
        [DataMember]
        public virtual string AssessmentType { get; set; }
        [DataMember]
        public virtual string IncidentDateString { get; set; }
        [DataMember]
        public virtual string Semester { get; set; }
    }
}