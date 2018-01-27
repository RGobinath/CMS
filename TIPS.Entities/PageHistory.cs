using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities
{
   public class PageHistory
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string UserId { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string SessionId { get; set; }
        [DataMember]
        public virtual DateTime VisitedTime { get; set; }
        [DataMember]
        public virtual string Action { get; set; }
        [DataMember]
        public virtual string Controller { get; set; }
        [DataMember]
        public virtual long ExecutionTime { get; set; }

        public long PageHistory_Id { get; set; }
    }
}
