using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.HRManagementEntities
{
    [DataContract]
   public class CertificateRequest
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string RequestNo { get; set; }
        [DataMember]
        public virtual DateTime CreateDate { get; set; }
        [DataMember]
        public virtual string ProcessedBy { get; set; }
        [DataMember]
        public virtual string UserRole { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual string ActivityFullName { get; set; }
        [DataMember]
        public virtual string StaffName { get; set; }
        [DataMember]
        public virtual string StaffIdNumber { get; set; }
        //[DataMember]
        //public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Department { get; set; }
        [DataMember]
        public virtual string CertificateType { get; set; }
        [DataMember]
        public virtual string DateOfIssue { get; set; }
        [DataMember]
        public virtual string Description { get; set; }
        //[DataMember]
        //public virtual string FileType { get; set; }
        [DataMember]
        public virtual long InstanceId { get; set; }
        [DataMember]
        public virtual string Performer { get; set; }
        [DataMember]
        public virtual string BranchCode { get; set; }
        [DataMember]
        public virtual bool IsIssueCompleted { get; set; }

        [DataMember]
        public virtual string ResolveComments { get; set; }
        [DataMember]
        public virtual string RejectionDetails { get; set; }


        [DataMember]
        public virtual TimeSpan? DifferenceInHours { get; set; }

        // Demo
        [DataMember]
        public virtual string DeptCode { get; set; }
    }
}
