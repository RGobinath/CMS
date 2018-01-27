using System;
using System.Runtime.Serialization;

namespace TIPS.Entities.EnquiryEntities
{
    public class EnquiryCourse
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string CourseCode { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string Subjects { get; set; }
        [DataMember]
        public virtual string Board { get; set; }
        [DataMember]
        public virtual long EnquiryId { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime? ModifiedDate { get; set; }
    }
}
