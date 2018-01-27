using System.Runtime.Serialization;
using System;
namespace TIPS.Entities.Assess.ReportCardClasses
{
    [DataContract]
    public class CoScholasticMaster
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
         public virtual string CoScholasticName { get; set; }
         [DataMember]
         public virtual string CoScholasticSection { get; set; }
        //added for Reportcard for only show 
        /// <summary>
        /// Created details and Modified details
        /// </summary>
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime? ModifiedDate { get; set; }
    }
}
