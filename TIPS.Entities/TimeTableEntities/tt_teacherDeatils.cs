using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.TimeTableEntities
{
   public class tt_teacherDeatils
    {
        public virtual string Gender { get; set; }
        public virtual string TeacherName { get; set; }
        public virtual Int32 max_periods { get; set; }
        public virtual Int32 rem_periods { get; set; }
    }
}
