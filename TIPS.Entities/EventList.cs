using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities
{
    [DataContract]
    public class EventList
    {
        [DataMember]
        public virtual long EventListId { get; set; }
        [DataMember]
        public virtual long EventId { get; set; }
        [DataMember]
        public virtual string EventListDescription { get; set; }
        [DataMember]
        public virtual string EventOrder { get; set; }
        [DataMember]
        public virtual bool IsActive { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime CreatedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime ModifiedDate { get; set; }
    }
}
