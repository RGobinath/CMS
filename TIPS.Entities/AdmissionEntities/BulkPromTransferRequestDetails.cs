using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.AdmissionEntities
{
    [DataContract]
    public class BulkPromTransferRequestDetails
    {
        public virtual long BulkPromTransferRequestId { get; set; }
        public virtual string RequestName { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Section { get; set; }
        public virtual string Status { get; set; }
        public virtual bool IsSaveList { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual bool IsPromotionOrTransfer { get; set; }
    }
}
