using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.TransportEntities
{
    [DataContract]
    public class FuelRefilDetails_Vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual decimal LastMilometerReading { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string VehicleNo { get; set; }
        [DataMember]
        public virtual decimal CurrentMilometerReading { get; set; }
        [DataMember]
        public virtual string FuelType { get; set; }
        [DataMember]
        public virtual int VehicleId { get; set; }
        //[DataMember]
        //public virtual decimal CurrentMiloMeterReading1 { get; set; }
        //[DataMember]
        //public virtual decimal LastMiloMeterReading1 { get; set; }
    }
}
