using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.HostelMgntEntities
{
    public class HstlMgmt_BedMaster
    {
        public virtual Int64 BedMst_Id { get; set; }
        public virtual Int64 RoomMst_Id { get; set; }
        public virtual Int64 HstlMst_Id { get; set; }
        public virtual string BedNumber { get; set; }
        public virtual bool IsAllocate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateModified { get; set; }
    }
}
