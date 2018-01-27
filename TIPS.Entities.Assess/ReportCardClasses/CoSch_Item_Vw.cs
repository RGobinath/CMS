using System.Runtime.Serialization;
using System;
namespace TIPS.Entities.Assess.ReportCardClasses
{
    [DataContract]
    public class CoSch_Item_Vw
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
        public virtual long CoScholasticCode { get; set; }
        [DataMember]
        public virtual string CoScholasticSection { get; set; }
        [DataMember]
        public virtual string CoScholasticName { get; set; }
        [DataMember]
        public virtual string Parameter { get; set; }
        [DataMember]
        public virtual string DescriptiveIndicator { get; set; }
        [DataMember]
        public virtual string CoSchCriteriaCode { get; set; }
        [DataMember]
        public virtual string CoSchGrade { get; set; }
    }
}
