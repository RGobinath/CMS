using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.AdminTemplateEntities
{
    public class AdminTemplateStudentsAcademicYearWiseCount_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string Boys { get; set; }
        [DataMember]
        public virtual string Girls { get; set; }
        [DataMember]
        public virtual long Total { get; set; }
    }
}
