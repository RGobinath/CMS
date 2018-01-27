using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.AdmissionEntities
{
    [DataContract]
    public class BulkPromTransfer
    {
        public virtual long   BulkPromTransferId { get; set; }
        public virtual long BulkPromTransferRequestId { get; set; }
        public virtual string RequestName { get; set; }
        public virtual string NewId { get; set; }
        public virtual string AfterCampus { get; set; }
        public virtual string AfterGrade { get; set; }
        public virtual string AfterSection { get; set; }
        public virtual string AfterNewId { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Section { get; set; }
        public virtual string Type { get; set; }
        public virtual string ApplicationNo { get; set; }
        public virtual long PreRegNum { get; set; }
        public virtual string Name { get; set; }
        public virtual string Gender { get; set; }
        public virtual string FeeStructYear { get; set; }
        public virtual string AcademicYear { get; set; }
        public virtual string AfterAcademicYear { get; set; }
        public virtual bool IsHosteller { get; set; }
        public virtual string AdmissionStatus { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual bool IsPromotionOrTransfer { get; set; }
    }
}
