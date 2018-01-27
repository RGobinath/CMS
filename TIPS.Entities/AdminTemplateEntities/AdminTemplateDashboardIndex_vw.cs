using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.AdminTemplateEntities
{
    public class AdminTemplateDashboardIndex_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string UserCount { get; set; }
        [DataMember]
        public virtual string StaffCount { get; set; }
        [DataMember]
        public virtual string CampusCount { get; set; }
        [DataMember]
        public virtual string StudentsCount { get; set; }
        [DataMember]
        public virtual string ViewersCount { get; set; }
    }
}
