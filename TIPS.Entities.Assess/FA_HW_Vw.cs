using System.Runtime.Serialization;

namespace TIPS.Entities.Assess
{
    public class FA_HW_Vw
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
        public virtual decimal FAJan { get; set; }
        [DataMember]
        public virtual decimal FAFeb { get; set; }
        [DataMember]
        public virtual decimal FAMar { get; set; }
        [DataMember]
        public virtual decimal FAApr { get; set; }
        [DataMember]
        public virtual decimal FAMay { get; set; }
        [DataMember]
        public virtual decimal FAJun { get; set; }
        [DataMember]
        public virtual decimal FAJul { get; set; }
        [DataMember]
        public virtual decimal FAAug { get; set; }
        [DataMember]
        public virtual decimal FASep { get; set; }
        [DataMember]
        public virtual decimal FANov { get; set; }
        [DataMember]
        public virtual decimal FAOct { get; set; }
        [DataMember]
        public virtual decimal FADec { get; set; }
        [DataMember]
        public virtual decimal HWJan { get; set; }
        [DataMember]
        public virtual decimal HWFeb { get; set; }
        [DataMember]
        public virtual decimal HWMar { get; set; }
        [DataMember]
        public virtual decimal HWApr { get; set; }
        [DataMember]
        public virtual decimal HWMay { get; set; }
        [DataMember]
        public virtual decimal HWJun { get; set; }
        [DataMember]
        public virtual decimal HWJul { get; set; }
        [DataMember]
        public virtual decimal HWAug { get; set; }
        [DataMember]
        public virtual decimal HWSep { get; set; }
        [DataMember]
        public virtual decimal HWNov { get; set; }
        [DataMember]
        public virtual decimal HWOct { get; set; }
        [DataMember]
        public virtual decimal HWDec { get; set; }
    }
}