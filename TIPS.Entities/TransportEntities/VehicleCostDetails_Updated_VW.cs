using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.TransportEntities
{
    public class VehicleCostDetails_Updated_VW
    {
        public virtual long VehicleCostId { get; set; }
        public virtual long VehicleId { get; set; }
        public virtual string Campus { get; set; }
        public virtual string TypeOfTrip { get; set; }
        public virtual DateTime VehicleTravelDate { get; set; }
        public virtual string VehicleNo { get; set; }
        public virtual long DriverMasterId { get; set; }
        public virtual long HelperId { get; set; }
        public virtual string VehicleRoute { get; set; }
        public virtual decimal StartKmrs { get; set; }
        public virtual decimal EndKmrs { get; set; }
        public virtual decimal DriverOt { get; set; }
        public virtual decimal HelperOt { get; set; }
        public virtual decimal Diesel { get; set; }
        public virtual decimal Maintenance { get; set; }
        public virtual decimal Service { get; set; }
        public virtual decimal FC { get; set; }
        public virtual decimal Others { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual long Rank { get; set; }
        public virtual string EntryType { get; set; }
    }
}
