using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.StoreEntities
{
    [DataContract]
    public class StoreReportForAdminTemplate_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string RequiredForCampus { get; set; }
        [DataMember]
        public virtual long Count { get; set; }
    }
}
