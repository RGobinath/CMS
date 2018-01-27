using System.Runtime.Serialization;
using System;

namespace TIPS.Entities.TransportEntities
{
    public class VehicleDistanceCoveredReport_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Type { get; set; }
        [DataMember]
        public virtual string VehicleNo { get; set; }
        [DataMember]
        public virtual int Month { get; set; }
        [DataMember]
        public virtual int Year { get; set; }
        [DataMember]
        public virtual decimal DistanceCovered { get; set; }
        [DataMember]
        public virtual DateTime? LastTripDate { get; set; }

    }
}
