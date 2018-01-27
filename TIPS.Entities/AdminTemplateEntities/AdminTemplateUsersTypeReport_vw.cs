using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.AdminTemplateEntities
{
    public class AdminTemplateUsersTypeReport_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string UserType { get; set; }
        [DataMember]
        public virtual long Count { get; set; }
    }
}
