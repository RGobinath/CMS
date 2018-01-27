using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.TransportEntities
{
    public class VehicleCostDetailsReport_sp
    {
        public virtual long Id { get; set; }
        public virtual string Campus { get; set; }
        public virtual string TripCount { get; set; }
        public virtual string VehicleNo { get; set; }
        public virtual decimal DriverOt { get; set; }
        public virtual decimal HelperOt { get; set; }
        public virtual decimal Diesel { get; set; }
        public virtual decimal Maintenance { get; set; }
        public virtual decimal Service { get; set; }
        public virtual decimal FC { get; set; }
        public virtual decimal Others { get; set; }
    }
}
