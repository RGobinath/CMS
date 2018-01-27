using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.TransportEntities
{
    public class VehicleReport
    {
        public virtual int Id { get; set; }
        public virtual int VehicleTypeId { get; set; }
        public virtual int VehicleId { get; set; }
        public virtual string Type { get; set; }
        public virtual string VehicleNo { get; set; }
        public virtual string Campus { get; set; }
        public virtual decimal DistanceCovered { get; set; }
        public virtual decimal? FuelConsumed { get; set; }
        public virtual decimal? FuelCost { get; set; }
        public virtual decimal? Mileage { get; set; }
        public virtual decimal? FC { get; set; }
        public virtual decimal? Insurance { get; set; }
        public virtual decimal? MechanicalMaintenance { get; set; }
        public virtual decimal? ACMaintenance { get; set; }
        public virtual decimal? ElectricalMaintenance { get; set; }
        public virtual decimal? BodyMaintenance { get; set; }
        public virtual decimal? TyreMaintenance { get; set; }

        [DataMember]
        public virtual VehicleTypeMaster VehicleTypeMaster { get; set; } 

    }
}
