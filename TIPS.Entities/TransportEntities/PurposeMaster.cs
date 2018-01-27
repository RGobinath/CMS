using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.TransportEntities
{
    public class PurposeMaster
    {
        public virtual long Purpose_Id { get; set; }
        public virtual string PurposeName { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual bool IsActive { get; set; }
    }
}
