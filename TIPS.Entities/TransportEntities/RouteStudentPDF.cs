using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using TIPS.Entities.TransportEntities;

namespace TIPS.Entities.TransportEntities
{
    public class RouteStudentPDF
    {
        [DataMember]
        public virtual string TipsLogo { get; set; }
        [DataMember]
        public virtual string NaceLogo { get; set; }
        [DataMember]
        public virtual string LocationName { get; set; }
        [DataMember]
        public virtual string RouteId { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Today { get; set; }
        [DataMember]
        public virtual IList<RouteConfiguration> RouteConfigList { get; set; }
       
    }
}
