using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.HostelMgntEntities
{
    public class HostelMasters_Vw
    {
        public virtual Int64 HstlMst_Id { get; set; }
        public virtual string HostelName { get; set; }
        public virtual string HostelType { get; set; }
        public virtual string Campus { get; set; }
        public virtual Int64 Rooms { get; set; }
        public virtual Int64 Beds { get; set; }
        public virtual string Floor { get; set; }
        public virtual Int64 CaptyUtilised { get; set; }
        public virtual Int64 AvibleCapty { get; set; }
        public virtual string InCharge { get; set; }



    }
}

