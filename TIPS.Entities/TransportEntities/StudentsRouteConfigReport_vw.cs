using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.TransportEntities
{
    public class StudentsRouteConfigReport_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual long TransportRequiredMale { get; set; }
        [DataMember]
        public virtual long TransportRequiredFemale { get; set; }
        [DataMember]
        public virtual long TransportRequired { get; set; }
        [DataMember]
        public virtual long TransportNotRequiredMale { get; set; }
        [DataMember]
        public virtual long TransportNotRequiredFeMale { get; set; }
        [DataMember]
        public virtual long TransportNotRequired { get; set; }
        [DataMember]
        public virtual long RouteAllocatedMale { get; set; }
        [DataMember]
        public virtual long RouteAllocatedFeMale { get; set; }
        [DataMember]
        public virtual long RouteAllocated { get; set; }
        [DataMember]
        public virtual long RouteNotAllocatedMale { get; set; }
        [DataMember]
        public virtual long RouteNotAllocatedFeMale { get; set; }
        [DataMember]
        public virtual long RouteNotAllocated { get; set; }
    }
}
