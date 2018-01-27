using System.Runtime.Serialization;

namespace TIPS.Entities.Assess
{
    public class FormattiveAssessment_Vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long AssessId { get; set; }
        [DataMember]
        public virtual long StudentId { get; set; }
        [DataMember]
        public virtual string IdNo { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string Subject { get; set; }
        [DataMember]
        public virtual decimal Jan { get; set; }
        [DataMember]
        public virtual decimal Feb { get; set; }
        [DataMember]
        public virtual decimal Mar { get; set; }
        [DataMember]
        public virtual decimal Apr { get; set; }
        [DataMember]
        public virtual decimal May { get; set; }
        [DataMember]
        public virtual decimal Jun { get; set; }
        [DataMember]
        public virtual decimal Jul { get; set; }
        [DataMember]
        public virtual decimal Aug { get; set; }
        [DataMember]
        public virtual decimal Sep { get; set; }
        [DataMember]
        public virtual decimal Nov { get; set; }
        [DataMember]
        public virtual decimal Oct { get; set; }
        [DataMember]
        public virtual decimal Dec { get; set; }
    }
}