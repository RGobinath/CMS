using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.CommunictionEntities
{
    public class StaffBulkSMSRequestReport_vw
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
