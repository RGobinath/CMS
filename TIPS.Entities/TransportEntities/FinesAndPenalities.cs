using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.TransportEntities
{
    public class FinesAndPenalities
    {
        public virtual long Id { get; set; }

        public virtual int RefId { get; set; }

        public virtual long VehicleId { get; set; }
        public virtual string Type { get; set; }
        public virtual string VehicleNo { get; set; }

        public virtual DateTime? PenalityDate { get; set; }

        public virtual string PenalityArea { get; set; }

        public virtual string PenalityReason { get; set; }

        public virtual long PenalityRupees { get; set; }

        public virtual DateTime? PenalityDueDate { get; set; }

        public virtual string PenalityPaidBy { get; set; }

        public virtual DateTime? CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string DriverName { get; set; }
    }
}
