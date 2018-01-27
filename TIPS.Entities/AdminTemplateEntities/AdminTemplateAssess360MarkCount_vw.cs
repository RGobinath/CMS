using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.AdminTemplateEntities
{
    public class AdminTemplateAssess360MarkCount_vw
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
        public virtual long Below75 { get; set; }
        [DataMember]
        public virtual long MeritList { get; set; }
        [DataMember]
        public virtual long HighAcheiversClub { get; set; }
        [DataMember]
        public virtual long ChairmanAward { get; set; }
    }
}
