using System;
using System.Runtime.Serialization;
namespace TIPS.Entities.Assess
{
    [DataContract]
    public class Assess360_ARC
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string RequestNo { get; set; }
        [DataMember]
        public virtual DateTime? DateCreated { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual string UserRole { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual long StudentId { get; set; }
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
        public virtual string TotalMarks { get; set; }
        [DataMember]
        public virtual string ObtainedMarks { get; set; }
        [DataMember]
        public virtual string Subject { get; set; }
        [DataMember]
        public virtual bool IsActive { get; set; }
        //Added By Gobi
        [DataMember]
        public virtual string Initial { get; set; }
        public virtual string CreatedUserName { get; set; }
    }
}
