using System.Runtime.Serialization;
namespace TIPS.Entities.EnquiryEntities
{
    [DataContract]
    public class StudentDtlsView
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string IdNo { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string PreRegNum { get; set; }
        [DataMember]
        public virtual bool IsHosteller { get; set; }
        [DataMember]
        public virtual string ApplicationNo { get; set; }
        [DataMember]
        public virtual string AdmissionStatus { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string FeeStructYear { get; set; }
        [DataMember]
        public virtual string BoardingType { get; set; }
    }
}