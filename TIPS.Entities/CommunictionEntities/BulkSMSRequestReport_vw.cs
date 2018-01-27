using System;
using System.Runtime.Serialization;

namespace TIPS.Entities.CommunictionEntities
{
    [DataContract]
    public class BulkSMSRequestReport_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long SMSComposeId { get; set; }
        [DataMember]
        public virtual long Total { get; set; }
        [DataMember]
        public virtual long Sent { get; set; }
        [DataMember]
        public virtual long NotValid { get; set; }
        [DataMember]
        public virtual long Failed { get; set; }
        [DataMember]
        public virtual long DNDApplied { get; set; }
        [DataMember]
        public virtual long NotDelivered { get; set; }
    }
}
