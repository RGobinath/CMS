using System.Runtime.Serialization;
using System;

namespace TIPS.Entities.TransportEntities
{
    public class VehicleFuelReport_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Type { get; set; }
        [DataMember]
        public virtual string VehicleNo { get; set; }
        [DataMember]
        public virtual int Month { get; set; }
        [DataMember]
        public virtual int Year { get; set; }
        [DataMember]
        public virtual decimal FuelQty { get; set; }
        [DataMember]
        public virtual decimal TotalPrice { get; set; }
        public virtual DateTime ? LastFilledDate { get; set; }
    }
}
