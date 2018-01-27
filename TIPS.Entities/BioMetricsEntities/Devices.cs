using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.BioMetricsEntities
{
    public class Devices
    {
        [DataMember]
        public virtual long DeviceId { get; set; }
        [DataMember]
        public virtual string DeviceFName { get; set; }
        [DataMember]
        public virtual string DeviceSName { get; set; }
        [DataMember]
        public virtual Int32 IsRealTime { get; set; }

    }
}
