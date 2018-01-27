using System;
using System.Runtime.Serialization;

namespace TIPS.Entities.ParentPortalEntities
{
    [DataContract]
    public class NoteEmailLog 
    {
        [DataMember]
        public virtual Int64 Id { get; set; }

        [DataMember]
        public virtual string EmailTo { get; set; }

        [DataMember]
        public virtual string EmailCC { get; set; }

        [DataMember]
        public virtual string EmailBCC { get; set; }

        [DataMember]
        public virtual Int64 BCC_Count { get; set; }

        [DataMember]
        public virtual string Subject { get; set; }

        [DataMember]
        public virtual string Message { get; set; }

        [DataMember]
        public virtual string Attachment { get; set; }

        [DataMember]
        public virtual string EmailDateTime { get; set; }

        [DataMember]
        public virtual string Module { get; set; }
    }
}
