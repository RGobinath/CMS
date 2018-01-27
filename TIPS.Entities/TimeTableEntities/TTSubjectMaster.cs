using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.TimeTableEntities
{
    public class TTSubjectMaster
    {
        public virtual long Id { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Section { get; set; }
        public virtual string Subject { get; set; }
        public virtual string AcademicYear { get; set; }
    }
}
