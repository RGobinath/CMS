using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace TIPS.Entities.AdmissionEntities
{
    public class RelievingReasonMaster
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string RelievingReason { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime CreatedDate { get; set; }
        [DataMember]
        public virtual DateTime ModifiedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
    }
}

