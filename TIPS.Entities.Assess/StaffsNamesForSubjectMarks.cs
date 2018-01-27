using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.Assess
{
   public class StaffsNamesForSubjectMarks
    {
        public virtual long Id { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Section { get; set; }
        public virtual string StaffName { get; set; }
        public virtual DateTime Date { get; set; }
    }
}
