using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities
{
    [DataContract]
    public class Event
    {
        [DataMember]
        public virtual long EventId { get; set; }
        [DataMember]
        public virtual string EventTitle { get; set; }
        [DataMember]
        public virtual string EventDescription { get; set; }
        [DataMember]
        public virtual string EventFor { get; set; }
        [DataMember]
        public virtual string EventType { get; set; }
        [DataMember]
        public virtual string EventCreatedBy { get; set; }
        [DataMember]
        public virtual long EventListCount { get; set; }
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
