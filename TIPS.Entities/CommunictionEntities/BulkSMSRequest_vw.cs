using System;
using System.Runtime.Serialization;

namespace TIPS.Entities.CommunictionEntities
{
    [DataContract]
    public class BulkSMSRequest_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string NewId { get; set; }
        [DataMember]
        public virtual bool IsHosteller { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string AdmissionStatus { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
       [DataMember]
        public virtual string FamilyDetailType { get; set; }
        [DataMember]
        public virtual string MobileNumber { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual DateTime SMSRecipientCreatedDate { get; set; }
        [DataMember]
        public virtual DateTime SMSRecipientModifiedDate { get; set; }
        [DataMember]
        public virtual long IdKeyValue { get; set; }
        [DataMember]
        public virtual string SentSMSStatusWithTid { get; set; }
        [DataMember]
        public virtual string SentSMSReportsWithStatus { get; set; }
        [DataMember]
        public virtual string VanNo { get; set; }
        [DataMember]
        public virtual DateTime CreatedDateNew { get; set; }
    }
}
