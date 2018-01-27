using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities
{
    [DataContract]
    public class StaffDepartmentMaster
    {
        [DataMember]
        public virtual Int64 Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Department { get; set; }
        [DataMember]
        public virtual string GroupName { get; set; }
    }
}
