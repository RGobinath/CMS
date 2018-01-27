using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.CommunictionEntities
{
    [DataContract]
    public class ComposeEmailInfo
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
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string AdmissionStatus { get; set; }
        [DataMember]
        public virtual string StudentName { get; set; }
        [DataMember]
        public virtual string NewId { get; set; }
        [DataMember]
        public virtual string FeeStructYear { get; set; }
        [DataMember]
        public virtual string IsHosteller { get; set; }
        [DataMember]
        public virtual bool Father { get; set; }
        [DataMember]
        public virtual bool Mother { get; set; }
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
        public virtual string Status { get; set; }
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
        [DataMember]
        public virtual string VanNo { get; set; }
        [DataMember]
        public virtual long IdKeyValue { get; set; }
        [DataMember]
        public virtual DateTime? AppliedFromDate { get; set; }
        [DataMember]
        public virtual DateTime? AppliedToDate { get; set; }
        [DataMember]
        public virtual string MailDate { get; set; }
        [DataMember]
        public virtual string ExpiryDate { get; set; }
        [DataMember]
        public virtual bool StudentPortal { get; set; }
        [DataMember]
        public virtual bool ParentPortal { get; set; }

    }
}
