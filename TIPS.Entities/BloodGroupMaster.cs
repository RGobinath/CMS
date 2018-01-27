using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities
{
    public class BloodGroupMaster
    {
        [DataMember]
        public virtual long FormId { get; set; }
        [DataMember]
        public virtual string FormCode { get; set; }
        [DataMember]
        public virtual string BloodGroup { get; set; }
        [DataMember]
        public virtual string BloodGroupdesc { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual string CreatedDate { get; set; }
        [DataMember]
        public virtual string UpdatedBy { get; set; }
        [DataMember]
        public virtual string UpdatedDate { get; set; }

    }
}
