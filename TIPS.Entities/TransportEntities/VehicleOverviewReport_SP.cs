using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.TransportEntities
{
    public class VehicleOverviewReport_SP
    {
        public virtual long Id { get; set; }
        public virtual string VehicleNo { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Purpose { get; set; }
        public virtual string FuelType { get; set; }
        public virtual string Type { get; set; }
        public virtual long VehicleTypeId { get; set; }
        public virtual string VehicleType { get; set; }
        public virtual long TypeOrder { get; set; }
        public virtual long TotalNoOfTrip { get; set; }
        public virtual long TotalDistance { get; set; }
        public virtual decimal Fuel { get; set; }
        public virtual decimal FC { get; set; }
        public virtual decimal DriverOt { get; set; }
        public virtual decimal HelperOt { get; set; }
        public virtual decimal Others { get; set; }
        public virtual decimal Maintenance { get; set; }
        public virtual decimal Service { get; set; }
        public virtual decimal Expenses { get; set; }
    }
}
