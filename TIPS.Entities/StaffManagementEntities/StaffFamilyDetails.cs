using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StaffManagementEntities
{
    public class StaffFamilyDetails
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual Int32 PreRegNum { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string Occupation { get; set; }
        [DataMember]
        public virtual string Age { get; set; }
        [DataMember]
        public virtual string Relationship { get; set; }
    }
}
