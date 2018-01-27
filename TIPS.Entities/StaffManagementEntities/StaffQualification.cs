using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StaffManagementEntities
{
    public class StaffQualification
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual Int32 PreRegNum { get; set; }
        [DataMember]
        public virtual string Course { get; set; }
        [DataMember]
        public virtual string Board { get; set; }
        [DataMember]
        public virtual string Institute { get; set; }
        [DataMember]
        public virtual string YearOfComplete { get; set; }
        [DataMember]
        public virtual string MajorSubjects { get; set; }
        [DataMember]
        public virtual string Percentage { get; set; }

    }
}
