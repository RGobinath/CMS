using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.CommunictionEntities
{
    [DataContract]
    public class StaffSMSRecipientsInfo
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long SMSComposeId { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
        [DataMember]
        public virtual string IdNumber { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Department { get; set; }
        [DataMember]
        public virtual string Designation { get; set; }
       
        [DataMember]
        public virtual string Staff_Mobile { get; set; }
        
        [DataMember]
        public virtual bool SendSMS { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual DateTime? RecipientsCreatedDate { get; set; }
        [DataMember]
        public virtual DateTime? RecipientsModifiedDate { get; set; }
        [DataMember]
        public virtual string ExceptionErrMsg { get; set; }
        [DataMember]
        public virtual string MobileNumber { get; set; }
        [DataMember]
        public virtual string SentSMSStatusWithTid { get; set; }
        [DataMember]
        public virtual string SentSMSReportsWithStatus { get; set; }
    

        public virtual long ReportCount { get; set; }

    }
}
