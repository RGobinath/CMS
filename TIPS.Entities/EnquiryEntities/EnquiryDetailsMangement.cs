using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections;

namespace TIPS.Entities.EnquiryEntities
{
    public class EnquiryDetailsMangement
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Centre { get; set; }
        [DataMember]
        public virtual string Program { get; set; }
        [DataMember]
        public virtual string Course { get; set; }
        [DataMember]
        public virtual string CourseType { get; set; }
        [DataMember]
        public virtual string Batch { get; set; }
        [DataMember]
        public virtual string Timing { get; set; }
        [DataMember]
        public virtual long ThejoProgramId { get; set; }
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
        public virtual EnquiryDetailsList EnquiryDetailsList { get; set; }
    }
}
