using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.TransportEntities
{
    public class RouteMaster
    {
        public virtual long Id { get; set; }

        public virtual string RouteId { get; set; }

        public virtual string RouteNo { get; set; }

        public virtual string Campus { get; set; }

        public virtual string Source { get; set; }
        public virtual string VehicleNo { get; set; }
        public virtual string Destination { get; set; }

        public virtual string Via { get; set; }

        public virtual decimal Distance { get; set; }

        public virtual string District { get; set; }

        public virtual string State { get; set; }

        public virtual string Country { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual string ModifiedBy { get; set; }

        //Added by Gobi
        public virtual long ReportCount { get; set; }
        public virtual string LocationName { get; set; }
        public virtual long StopOrderNumber { get; set; }
        public virtual long NoOfStudList { get; set; }
    }
}
