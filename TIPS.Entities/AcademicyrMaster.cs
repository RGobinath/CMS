using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities
{
    public class AcademicyrMaster
    {
        [DataMember]
        public virtual long FormId { get; set; }
        [DataMember]
        public virtual string FormCode { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string AcademicYeardesc { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime CreatedDate { get; set; }
        [DataMember]
        public virtual string UpdateBy { get; set; }
        [DataMember]
        public virtual DateTime UpdateDate { get; set; }
        [DataMember]
        public virtual bool IsActive { get; set; }
    }
}
