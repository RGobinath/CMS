using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace TIPS.Entities.AdmissionEntities
{
    public class EmailLog
    {
        [DataMember]
        public virtual long Id { get; set; }

        [DataMember]
        public virtual string EmailTo { get; set; }
        [DataMember]
        public virtual string EmailFrom { get; set; }

        [DataMember]
        public virtual string EmailCC { get; set; }

        [DataMember]
        public virtual string EmailBCC { get; set; }

        [DataMember]
        public virtual int BCC_Count { get; set; }

        //[Required]
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
        [DataMember]
        public virtual bool IsSent { get; set; }
        [DataMember]
        public virtual string ActualException { get; set; }
        [DataMember]
        public virtual string NewId { get; set; }
        [DataMember]
        public virtual string StudName { get; set; }
    }
}
