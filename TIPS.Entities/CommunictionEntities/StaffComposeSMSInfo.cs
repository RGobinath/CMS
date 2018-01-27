using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.CommunictionEntities
{
    [DataContract]
    public class StaffComposeSMSInfo
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string SMSReqId { get; set; }
        [DataMember]
        public virtual bool Staff { get; set; }
         [DataMember]
        public virtual string Message { get; set; }
        [DataMember]
        public virtual bool IsSaveList { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual bool BulkSMSSent { get; set; }
        [DataMember]
        public virtual bool Suspended { get; set; }
        [DataMember]
        public virtual string ReasonForSuspend { get; set; }
        [DataMember]
        public virtual string SMSTemplate { get; set; }
        [DataMember]
        public virtual string SMSTemplateValue { get; set; }
        [DataMember]
        public virtual string Info { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
       public virtual string Campus { get; set; }
        [DataMember]
        public virtual string IdNumber { get; set; }
        [DataMember]
        public virtual string Department { get; set; }
        [DataMember]
        public virtual string Designation { get; set; }
   
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime CreatedDate { get; set; }
        [DataMember]
        public virtual DateTime ModifiedDate { get; set; }

        [DataMember]
        public virtual long Sent { get; set; }
        [DataMember]
        public virtual long Failed { get; set; }
        [DataMember]
        public virtual long NotValid { get; set; }
        [DataMember]
        public virtual long UnDelivered { get; set; }
        [DataMember]
        public virtual long Total { get; set; }
 
    }
}
