using System.Runtime.Serialization;
using System;
namespace TIPS.Entities.Assess.ReportCardClasses
{
    [DataContract]
    public class CoScholasticStudentDetails_Vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long RptId { get; set; }
        [DataMember]
        public virtual string NewId { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
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
        public virtual string CoScholasticCriteria { get; set; }
        [DataMember]
        public virtual decimal? MaxMarks { get; set; }

        //added for Reportcard for only show 
        /// <summary>
        /// Created details and Modified details
        /// </summary>
        [DataMember]
        public virtual long RptRequestId { get; set; }
        [DataMember]
        public virtual long EditRowId { get; set; }
        [DataMember]
        public virtual string ObtainMarks { get; set; }
        [DataMember]
        public virtual string CoSholasticGrade { get; set; }
        [DataMember]
        public virtual DateTime CreatedDate { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime ModifiedDate { get; set; }
        
    }
}
