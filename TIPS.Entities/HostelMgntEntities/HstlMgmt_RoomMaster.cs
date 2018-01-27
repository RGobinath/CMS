using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.HostelMgntEntities
{
    public class HstlMgmt_RoomMaster
    {
        public virtual long RoomMst_Id { get; set; }
        public virtual string RoomNumber { get; set; }
        public virtual long HstlMst_Id { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? DateModified { get; set; }
    }
}
