using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.TransportEntities
{
    [DataContract]
    public class VehicleDistanceCovered_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual decimal KMIn { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string VehicleNo { get; set; }
        [DataMember]
        public virtual decimal KMOut { get; set; }
        [DataMember]
        public virtual string FuelType { get; set; }
        [DataMember]
        public virtual string Type { get; set; }
        [DataMember]
        public virtual int VehicleId { get; set; }
    }
}
