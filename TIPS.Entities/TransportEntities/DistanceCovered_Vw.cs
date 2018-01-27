using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.TransportEntities
{
    public class DistanceCovered_Vw
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
        public virtual decimal DistanceCovered { get; set; }
        [DataMember]
        public virtual DateTime? LastTripDate { get; set; }

    }
}
