using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.TransportEntities
{
    public class DailyUsageVehicleMaster_vw
    {
        public virtual long ViewId { get; set; }
        public virtual int Id { get; set; }
        public virtual int VehicleTypeId { get; set; }
        public virtual string VehicleSubType { get; set; }
        public virtual string VehicleNo { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Purpose { get; set; }
        public virtual string FuelType { get; set; }
        public virtual string EngineType { get; set; }
        public virtual int NoofSeats { get; set; }
        public virtual int NoofAxle { get; set; }
        public virtual DateTime FirstRegisteredDate { get; set; }
        public virtual string RegistrationNo { get; set; }
        public virtual string Make { get; set; }
        public virtual string Type { get; set; }
        public virtual string ChassisNo { get; set; }
        public virtual string EngineNumber { get; set; }
        public virtual string BHP { get; set; }
        public virtual string CC { get; set; }
        public virtual string WheelBase { get; set; }
        public virtual string UnladenWeight { get; set; }
        public virtual string Color { get; set; }
        public virtual string GVW { get; set; }
        public virtual string Address { get; set; }
        public virtual string RCAttachment { get; set; }
        public virtual string Model { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string Status { get; set; }
        public virtual bool IsGPSDeviceInstalled { get; set; }
        public virtual long GPS_TrackingDeviceMaster_Id { get; set; }
        public virtual DateTime? VehicleTravelDate { get; set; }
        public virtual long Rank { get; set; }
        public virtual VehicleTypeMaster VehicleTypeMaster { get; set; } 
    }
}
