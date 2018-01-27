using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using TIPS.Entities.AdmissionEntities;

namespace TIPS.Entities.TransportEntities
{
    public class RouteConfiguration
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long RouteMasterId { get; set; }
        [DataMember]
        public virtual string LocationName { get; set; }
        [DataMember]
        public virtual string LocationDetails { get; set; }
        [DataMember]
        public virtual long StopOrderNumber { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime? ModifiedDate { get; set; }
        [DataMember]
        public virtual IList<RouteStudentConfigurationPDF_vw> routeStudConfigPdfList { get; set; }
    }
}
