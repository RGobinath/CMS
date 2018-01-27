using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.TransportEntities
{
    public class Student_Route_Configuration
    {
        [DataMember]
        public virtual long RouteMasterId { get; set; }
        [DataMember]
        public virtual string RouteStudCode { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string RouteNo { get; set; }
        [DataMember]
        public virtual string Source { get; set; }
        [DataMember]
        public virtual string Destination { get; set; }
    }
}
