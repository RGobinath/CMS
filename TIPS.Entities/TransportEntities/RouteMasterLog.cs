using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.TransportEntities
{
    public class RouteMasterLog
    {
        public virtual long RouteLogId { get; set; }
        public virtual long Id { get; set; }
        public virtual string RouteId { get; set; }
        public virtual string RouteNo { get; set; }
        public virtual string VehicleNo { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Source { get; set; }
        public virtual string Destination { get; set; }
        public virtual string Via { get; set; }
        public virtual string ToRouteNo { get; set; }
        public virtual string ToVehicleNo { get; set; }
        public virtual string ToCampus { get; set; }
        public virtual string ToSource { get; set; }
        public virtual string ToDestination { get; set; }
        public virtual string ToVia { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
    }
}
