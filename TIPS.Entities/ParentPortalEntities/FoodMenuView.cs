using System.Runtime.Serialization;
using System;

namespace TIPS.Entities.ParentPortalEntities
{
    [DataContract]
    public class FoodMenuView
    {
        [DataMember]
        public virtual Int64 Id { get; set; }
        [DataMember]
        public virtual Int64 FMWeekId { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Months { get; set; }
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
        public virtual string Notes { get; set; }
        [DataMember]
        public virtual string Performer { get; set; }
        [DataMember]
        public virtual string CreatedOn { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }

    }
}
