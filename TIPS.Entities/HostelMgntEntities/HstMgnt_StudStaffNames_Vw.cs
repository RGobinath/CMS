using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.HostelMgntEntities
{
    public class HstMgnt_StudStaffNames_Vw
    {
        public virtual Int64 Id { get; set; }
        public virtual string Name { get; set; }
        public virtual Int64 StudStfID { get; set; }
        public virtual string IdNum { get; set; }
        public virtual string Campus { get; set; }
    }
}
