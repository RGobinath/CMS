﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.AdminTemplateEntities
{
    public class AdminTemplateStudentsCountGroupByGrade_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual long Boys { get; set; }
        [DataMember]
        public virtual long Girls { get; set; }
        [DataMember]
        public virtual long Total { get; set; }
        [DataMember]
        public virtual long ReportCount { get; set; }
    }
}
