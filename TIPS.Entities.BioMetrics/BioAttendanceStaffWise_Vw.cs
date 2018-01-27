using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TIPS.Entities.BioMetrics
{
    public class BioAttendanceStaffWise_Vw
    {
        public virtual long Id { get; set; }
        public virtual string UserId { get; set; }
        public virtual DateTime LogDate { get; set; }
        public virtual long P1 { get; set; }
        public virtual string P1Grade { get; set; }
        public virtual DateTime P1In { get; set; }
        public virtual DateTime P1Out { get; set; }
        public virtual long P2 { get; set; }
        public virtual string P2Grade { get; set; }
        public virtual DateTime P2In { get; set; }
        public virtual DateTime P2Out { get; set; }
        public virtual long P3 { get; set; }
        public virtual string P3Grade { get; set; }
        public virtual DateTime P3In { get; set; }
        public virtual DateTime P3Out { get; set; }
        public virtual long P4 { get; set; }
        public virtual string P4Grade { get; set; }
        public virtual DateTime P4In { get; set; }
        public virtual DateTime P4Out { get; set; }
        public virtual long P5 { get; set; }
        public virtual string P5Grade { get; set; }
        public virtual DateTime P5In { get; set; }
        public virtual DateTime P5Out { get; set; }
        public virtual long P6 { get; set; }
        public virtual string P6Grade { get; set; }
        public virtual DateTime P6In { get; set; }
        public virtual DateTime P6Out { get; set; }
        public virtual long P7 { get; set; }
        public virtual string P7Grade { get; set; }
        public virtual DateTime P7In { get; set; }
        public virtual DateTime P7Out { get; set; }
        public virtual long P8 { get; set; }
        public virtual string P8Grade { get; set; }
        public virtual DateTime P8In { get; set; }
        public virtual DateTime P8Out { get; set; }
        public virtual long P9 { get; set; }
        public virtual string P9Grade { get; set; }
        public virtual DateTime P9In { get; set; }
        public virtual DateTime P9Out { get; set; }
    }
}