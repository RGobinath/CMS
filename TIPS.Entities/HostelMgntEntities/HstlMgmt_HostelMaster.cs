using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.HostelMgntEntities
{
    public class HstlMgmt_HostelMaster
    {
        public virtual long HstlMst_Id { get; set; }
        public virtual string HostelName { get; set; }
        public virtual string HostelType { get; set; }
        public virtual string Floor { get; set; }
        public virtual string Campus { get; set; }
        public virtual string InCharge { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? DateCreated { get; set; }
        public virtual DateTime? DateModified { get; set; }
    }
}
