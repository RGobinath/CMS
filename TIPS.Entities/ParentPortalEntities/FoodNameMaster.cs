using System.Runtime.Serialization;
using System;

namespace TIPS.Entities.ParentPortalEntities
{
    [DataContract]
    public class FoodNameMaster
    {
        [DataMember]
        public virtual Int64 Id { get; set; }
        [DataMember]
        public virtual string FoodName { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string FoodType { get; set; }
        [DataMember]
        public virtual string Performer { get; set; }
        [DataMember]
        public virtual string CreatedOn { get; set; }
    }
}
