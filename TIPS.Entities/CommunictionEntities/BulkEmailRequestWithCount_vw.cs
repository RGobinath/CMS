using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.CommunictionEntities
{
    public class BulkEmailRequestWithCount_vw
    {
        public virtual long Id { get; set; }
        public virtual string BulkReqId { get; set; }
        public virtual string UserId { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Grade { get; set; }
        public virtual string AcademicYear { get; set; }
        public virtual bool Father { get; set; }
        public virtual bool Mother { get; set; }
        public virtual bool General { get; set; }
        public virtual string Subject { get; set; }
        public virtual bool Attachment { get; set; }
        public virtual string Message { get; set; }
        public virtual string Status { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual long InProgress { get; set; }
        public virtual long InvalidMail { get; set; }
        public virtual long Sent { get; set; }
        public virtual long NotSent { get; set; }
        public virtual long Total { get; set; }
        public virtual long IdKeyValue { get; set; }
    }
}
