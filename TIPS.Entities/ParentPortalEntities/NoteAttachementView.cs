using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.ParentPortalEntities
{
    public class NoteAttachmentView
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual Int64 NotePreId { get; set; }
        [DataMember]
        public virtual string AttachmentName { get; set; }
    }
}
