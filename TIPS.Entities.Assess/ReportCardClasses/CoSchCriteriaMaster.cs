using System.Runtime.Serialization;
using System;
namespace TIPS.Entities.Assess.ReportCardClasses
{
    [DataContract]
    public class CoSchCriteriaMaster
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long ParentRefId { get; set; }
        [DataMember]
        public virtual long RefId { get; set; }
        [DataMember]
        public virtual string CoScholasticCriteria { get; set; }
        [DataMember]
        public virtual decimal TotalMarks { get; set; }

        //added for Reportcard for only show 
        /// <summary>
        /// Created details and Modified details
        /// </summary>
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
