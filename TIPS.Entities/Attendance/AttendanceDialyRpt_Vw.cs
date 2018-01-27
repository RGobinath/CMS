using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.Attendance
{
  public  class AttendanceDialyRpt_Vw
    {
        public virtual long Id { get; set; }
        public virtual long PreRegNum { get; set; }
        public virtual string Name { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Section { get; set; }
        public virtual string AcademicYear  { get; set; }
        public virtual string Status_Flag { get; set; }
    }
}
