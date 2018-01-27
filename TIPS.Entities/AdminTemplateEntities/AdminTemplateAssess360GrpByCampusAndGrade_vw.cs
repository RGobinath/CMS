using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.AdminTemplateEntities
{
    public class AdminTemplateAssess360GrpByCampusAndGrade_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual decimal GetMarks { get; set; }
        [DataMember]
        public virtual long Rank { get; set; }

        [DataMember]
        public virtual string Mark { get; set; }
        [DataMember]
        public virtual string RankName { get; set; }
    }
}
