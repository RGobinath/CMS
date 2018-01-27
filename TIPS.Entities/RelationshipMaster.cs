using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities
{
    public class RelationshipMaster
    {
        [DataMember]
        public virtual long FormId { get; set; }
        [DataMember]
        public virtual string FormCode { get; set; }
        [DataMember]
        public virtual string Relationships { get; set; }
        [DataMember]
        public virtual string Module { get; set; }
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
