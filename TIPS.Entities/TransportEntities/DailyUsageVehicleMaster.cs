using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.TransportEntities
{
    public class DailyUsageVehicleMaster
    {
        [DataMember]
        public virtual int Id { get; set; }
        [DataMember]
        public virtual int VehicleTypeId { get; set; }
        [DataMember]
        public virtual string Type { get; set; }
        [DataMember]
        public virtual string VehicleNo { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Purpose { get; set; }
        [DataMember]
        public virtual string FuelType { get; set; }

        [DataMember]
        public virtual string EngineType { get; set; }
        [DataMember]
        public virtual string EngineNumber { get; set; }

        [DataMember]
        public virtual DateTime? FirstRegisteredDate { get; set; }

        public virtual string Make { get; set; }
        public virtual string ChassisNo { get; set; }
        public virtual string BHP { get; set; }
        public virtual string CC { get; set; }
        public virtual string WheelBase { get; set; }
        public virtual string UnladenWeight { get; set; }
        public virtual string Color { get; set; }
        public virtual string GVW { get; set; }
        public virtual string Address { get; set; }
        public virtual string RCAttachment { get; set; }

        public virtual string Model { get; set; }
        [DataMember]
        public virtual bool IsActive { get; set; }

        [DataMember]
        public virtual NewVehicleTypeMaster NewVehicleTypeMaster { get; set; } 
    }
}
