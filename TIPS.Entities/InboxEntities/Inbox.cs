using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.InboxEntities
{
    public class Inbox
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string UserId { get; set; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Module { get; set; }
        [DataMember]
        public virtual long RefNumber { get; set; }
        [DataMember]
        public virtual string InformationFor { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual DateTime CreatedDate { get; set; }
        [DataMember]
        public virtual bool IsRead { get; set; }
    }
}
