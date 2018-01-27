using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIPS.Entities.TimeTableEntities
{
    public class tt_SubjectTeacherPeriodCount
    {
        public virtual string Subject { get; set; }
        public virtual long TeacherId { get; set; }
        public virtual string TeacherCode { get; set; }
        public virtual string TeacherName { get; set; }
        public virtual long PeriodCount { get; set; }
        public virtual long PeriodRemainingCount { get; set; }
    }
}
