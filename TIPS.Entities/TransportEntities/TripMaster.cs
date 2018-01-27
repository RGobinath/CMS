using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.TransportEntities
{
    public class TripMaster
    {
        public virtual long TripId { get; set; }

        public virtual string TripName { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime CreatedDate { get; set; }

        public virtual string ModifiedBy { get; set; }

        public virtual DateTime ModifiedDate { get; set; }

        public virtual bool IsActive { get; set; }

    }
}
