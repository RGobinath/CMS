using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TIPS.Entities.StaffManagementEntities;

namespace TIPS.Entities.TransportEntities
{
    public class VehicleCostDetails
    {

        public virtual long VehicleCostId { get; set; }

        public virtual long VehicleId { get; set; }

        public virtual string Campus { get; set; }

        public virtual string TypeOfTrip { get; set; }

        public virtual DateTime VehicleTravelDate { get; set; }

        public virtual string VehicleNo { get; set; }

        public virtual DriverMaster DriverMaster { get; set; }

        public virtual StaffDetails StaffDetails { get; set; }

        public virtual string VehicleRoute { get; set; }

        public virtual long StartKmrs { get; set; }

        public virtual long EndKmrs { get; set; }

        public virtual long Distance { get; set; }

        public virtual decimal DriverOt { get; set; }

        public virtual decimal HelperOt { get; set; }

        public virtual decimal Diesel { get; set; }

        public virtual decimal Maintenance { get; set; }

        public virtual decimal Service { get; set; }

        public virtual decimal FC { get; set; }

        public virtual decimal Others { get; set; }

        public virtual long HelperId { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime CreatedDate { get; set; }

        public virtual string ModifiedBy { get; set; }

        public virtual DateTime ModifiedDate { get; set; }

        //public virtual long DriverMasterId { get; set; }

        //public virtual long HelperId { get; set; }
    }
    public class VehicleCostDetailsPDF
    {
        public virtual IList<VehicleCostDetails> VehicleCostDetailsList { get; set; }
    }
}
