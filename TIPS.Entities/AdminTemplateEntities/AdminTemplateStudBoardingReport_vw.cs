﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.AdminTemplateEntities
{
    public class AdminTemplateStudBoardingReport_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string BoardingType { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual long Count { get; set; }
    }
}
