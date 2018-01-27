using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.AdmissionEntities
{
    public class EmailAttachment
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual Int32 PreRegNum { get; set; }
        [DataMember]
        public virtual byte[] Attachment { get; set; }
        [DataMember]
        public virtual string AttachmentName { get; set; }
        [DataMember]
        public virtual string AppName { get; set; }
    }
}
