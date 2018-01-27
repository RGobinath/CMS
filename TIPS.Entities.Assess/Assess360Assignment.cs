using System;
using System.Runtime.Serialization;
namespace TIPS.Entities.Assess
{
    [DataContract]
    public class Assess360Assignment
    {
        [DataMember]
        public virtual long Id { get; set; }
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
        public virtual int AssessCompGroup { get; set; }
        [DataMember]
        public virtual string AssignmentName { get; set; }
        [DataMember]
        public virtual decimal? Mark { get; set; }
        [DataMember]
        public virtual string Staff { get; set; }
        [DataMember]
        public virtual int TotalStudents { get; set; }
        [DataMember]
        public virtual int Entered { get; set; }
        [DataMember]
        public virtual int NotEntered { get; set; }
        [DataMember]
        public virtual string EnteredBy { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string Semester { get; set; }
    }
}
