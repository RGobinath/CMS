using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities
{
    public class CampusMaster
    {
        [DataMember]
        public virtual long FormId { get; set; }
        [DataMember]
        public virtual string FormCode { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string ShowName { get; set; }
        [DataMember]
        public virtual string Code { get; set; }
        [DataMember]
        public virtual string Location { get; set; }
        [DataMember]
        public virtual string Country { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual string CreatedDate { get; set; }
        [DataMember]
        public virtual string UpdatedBy { get; set; }
        [DataMember]
        public virtual string UpdatedDate { get; set; }
        [DataMember]
        public virtual long Flag { get; set; }
    }
}
