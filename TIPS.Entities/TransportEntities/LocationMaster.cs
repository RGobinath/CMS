using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.TransportEntities
{
    public class LocationMaster
    {
        public virtual long Id { get; set; }
        public virtual string LocationId { get; set; }
        public virtual string Campus { get; set; }
        public virtual string LocationName { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual string ModifiedBy { get; set; }

        public virtual string CreatedUserName { get; set; }
        public virtual string ModifiedByUserName { get; set; }

        //Added by Gobi
        public virtual long ReportCount { get; set; }
    }
}
