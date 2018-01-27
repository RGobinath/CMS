using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.Assess
{
    [DataContract]
    public class RptCardAchievement
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long RefId { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string SemesterType { get; set; }
        [DataMember]
        public virtual string AchievementType { get; set; }
        [DataMember]
        public virtual string AchievementDescription { get; set; }
        [DataMember]
        public virtual string Awarded { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime? ModifiedDate { get; set; }
    }
}
