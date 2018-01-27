using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.InboxEntities
{
    public class InboxCount_Vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string UserId { get; set; }
        [DataMember]
        public virtual string UnreadCount { get; set; }
    }
}
