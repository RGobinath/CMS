using System;
using System.Runtime.Serialization;

namespace TIPS.Entities.EnquiryEntities
{
    public class EnquiryCourseBatchTiming
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Batch { get; set; }
        [DataMember]
        public virtual string Timing { get; set; }
        [DataMember]
        public virtual long EnquiryCourseId { get; set; }
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
        [DataMember]
        public virtual long BatchId { get; set; }
        [DataMember]
        public virtual long TimingId { get; set; }
    }
}

   