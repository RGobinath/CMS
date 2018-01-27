using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.CommunictionEntities
{
    [DataContract]
    public class SMSRecipientsInfo
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
        public virtual string NewId { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual bool IsHosteller { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string AdmissionStatus { get; set; }
        [DataMember]
        public virtual string MotherMobile_NO { get; set; }
        [DataMember]
        public virtual string FatherMobile_NO { get; set; }
        [DataMember]
        public virtual string FamilyDetailType { get; set; }
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
        public virtual long IdKeyValue { get; set; }
        [DataMember]
        public virtual string SentSMSStatusWithTid { get; set; }
        [DataMember]
        public virtual string SentSMSReportsWithStatus { get; set; }
        [DataMember]
        public virtual string VanNo { get; set; }

        public virtual long ReportCount { get; set; }

        [DataMember]
        public virtual DateTime AppliedDate { get; set; }
    }
}
