using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.AdminTemplateEntities
{
    public class AdminTemplateTransportReportByCampus_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual long DriverCount { get; set; }
        [DataMember]
        public virtual long RouteCount { get; set; }
        [DataMember]
        public virtual long LocationCount { get; set; }
        [DataMember]
        public virtual long VehiclesCount { get; set; }
    }
}
