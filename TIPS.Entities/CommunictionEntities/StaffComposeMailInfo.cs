using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.CommunictionEntities
{
    [DataContract]
    public class StaffComposeMailInfo
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string BulkReqId { get; set; }
        [DataMember]
        public virtual string UserId { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
    
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string IdNumber { get; set; }
        [DataMember]
        public virtual string Department { get; set; }
        [DataMember]
        public virtual string Designation { get; set; }
        [DataMember]
        public virtual bool General { get; set; }
        [DataMember]
        public virtual string Subject { get; set; }
        [DataMember]
        public virtual bool Attachment { get; set; }
        [DataMember]
        public virtual string Message { get; set; }
        [DataMember]
        public virtual bool IsSaveList { get; set; }
        [DataMember]
        public virtual string Suspend { get; set; }
        
        [DataMember]
        public virtual string Reason { get; set; }
        [DataMember]
        public virtual string AlternativeEmailId { get; set; }
        [DataMember]
        public virtual string AlternatPassword { get; set; }
        [DataMember]
        public virtual bool BulkEmailAdded { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime CreatedDate { get; set; }
        [DataMember]
        public virtual DateTime ModifiedDate { get; set; }
      
    }
}
