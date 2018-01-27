using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.TransportEntities
{
    public class FitnessCertificate
    {

        public virtual long Id { get; set; }

        public virtual long RefId { get; set; }

        public virtual long VehicleId { get; set; }

        public virtual string Type { get; set; }

        public virtual string VehicleNo { get; set; }

        public virtual DateTime? FCDate { get; set; }

        public virtual DateTime? NextFCDate { get; set; }

        public virtual long FCCost { get; set; }

        public virtual string Description { get; set; }

        public virtual string FCWorkCarriedAt { get; set; }

        public virtual string RTO { get; set; }

        public virtual string Driver { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime CreatedDate { get; set; }

        public virtual string FCertificate { get; set; }

        public virtual DateTime? FCTaxValidUpto { get; set; }
    }
}
