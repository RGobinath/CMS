using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.CommunictionEntities
{
    [DataContract]
    public class StaffRecipientsEmailInfo
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long ComposeId { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Department { get; set; }
        [DataMember]
        public virtual string Designation { get; set; }

        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual string IdNumber { get; set; }

        [DataMember]
        public virtual string EmailId { get; set; }

        [DataMember]
        public virtual bool SendMail { get; set; }

        [DataMember]
        public virtual DateTime? RecipientsCreatedDate { get; set; }

        [DataMember]
        public virtual DateTime? RecipientsModifiedDate { get; set; }

        [DataMember]
        public virtual long IdKeyValue { get; set; }



    }
}
