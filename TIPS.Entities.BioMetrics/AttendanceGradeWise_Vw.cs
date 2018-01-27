using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.BioMetrics
{
    public class AttendanceGradeWise_Vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string DeviceFName { get; set; }
        [DataMember]
        public virtual DateTime LogDate { get; set; }
        public virtual long P1 { get; set; }
        public virtual DateTime P1In { get; set; }
        public virtual DateTime P1Out { get; set; }
        public virtual long P2 { get; set; }
        public virtual DateTime P2In { get; set; }
        public virtual DateTime P2Out { get; set; }
        public virtual long P3 { get; set; }
        public virtual DateTime P3In { get; set; }
        public virtual DateTime P3Out { get; set; }
        public virtual long P4 { get; set; }
        public virtual DateTime P4In { get; set; }
        public virtual DateTime P4Out { get; set; }
        public virtual long P5 { get; set; }
        public virtual DateTime P5In { get; set; }
        public virtual DateTime P5Out { get; set; }
        public virtual long P6 { get; set; }
        public virtual DateTime P6In { get; set; }
        public virtual DateTime P6Out { get; set; }
        public virtual long P7 { get; set; }
        public virtual DateTime P7In { get; set; }
        public virtual DateTime P7Out { get; set; }
        public virtual long P8 { get; set; }
        public virtual DateTime P8In { get; set; }
        public virtual DateTime P8Out { get; set; }
        public virtual long P9 { get; set; }
        public virtual DateTime P9In { get; set; }
        public virtual DateTime P9Out { get; set; }
    }
}
