using System;
using System.Runtime.Serialization;

namespace TIPS.Entities.ParentPortalEntities
{
    public class NoteAttachment
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual Int64 NotePreId { get; set; }
        [DataMember]
        public virtual byte[] Attachment { get; set; }
        [DataMember]
        public virtual string AttachmentName { get; set; }
    }
}
