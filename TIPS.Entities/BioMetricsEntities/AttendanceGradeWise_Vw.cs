using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.BioMetricsEntities
{
    public class AttendanceGradeWise_Vw
    {
        public virtual long Id { get; set; }
        public virtual long DeviceId { get; set; }
        public virtual string DeviceFName { get; set; }
        public virtual DateTime LogDate { get; set; }
        public virtual long P1 { get; set; }
        public virtual string P1In { get; set; }
        public virtual string P1Out { get; set; }
        public virtual long P2 { get; set; }
        public virtual string P2In { get; set; }
        public virtual string P2Out { get; set; }
        public virtual long P3 { get; set; }
        public virtual string P3In { get; set; }
        public virtual string P3Out { get; set; }
        public virtual long P4 { get; set; }
        public virtual string P4In { get; set; }
        public virtual string P4Out { get; set; }
        public virtual long P5 { get; set; }
        public virtual string P5In { get; set; }
        public virtual string P5Out { get; set; }
        public virtual long P6 { get; set; }
        public virtual string P6In { get; set; }
        public virtual string P6Out { get; set; }
        public virtual long P7 { get; set; }
        public virtual string P7In { get; set; }
        public virtual string P7Out { get; set; }
        public virtual long P8 { get; set; }
        public virtual string P8In { get; set; }
        public virtual string P8Out { get; set; }
        public virtual long P9 { get; set; }
        public virtual string P9In { get; set; }
        public virtual string P9Out { get; set; }

        public virtual string Period1 { get; set; }
        public virtual string Period2 { get; set; }
        public virtual string Period3 { get; set; }
        public virtual string Period4 { get; set; }
        public virtual string Period5 { get; set; }
        public virtual string Period6 { get; set; }
        public virtual string Period7 { get; set; }
        public virtual string Period8 { get; set; }
        public virtual string Period9 { get; set; }
    }
}
