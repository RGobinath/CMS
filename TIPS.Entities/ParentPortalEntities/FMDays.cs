using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections;

namespace TIPS.Entities.ParentPortalEntities
{
    [DataContract]
    public class FMDays
    {
        [DataMember]
        public virtual Int64 Id { get; set; }
        [DataMember]
        public virtual Int64 FMWeekId { get; set; }

        [DataMember]
        public virtual string Week { get; set; }
        [DataMember]
        public virtual string Day { get; set; }
        [DataMember]
        public virtual string BreakFast { get; set; }
        [DataMember]
        public virtual string LunchNonVeg { get; set; }
        [DataMember]
        public virtual string LunchVeg { get; set; }
        [DataMember]
        public virtual string Snacks { get; set; }

        [DataMember]
        public virtual IEnumerable Week1 { get; set; }
        [DataMember]
        public virtual IEnumerable Day1 { get; set; }
        [DataMember]
        public virtual IEnumerable BreakFast1 { get; set; }
        [DataMember]
        public virtual IEnumerable LunchNonVeg1 { get; set; }
        [DataMember]
        public virtual IEnumerable LunchVeg1 { get; set; }
        [DataMember]
        public virtual IEnumerable Snacks1 { get; set; }
    }
}
