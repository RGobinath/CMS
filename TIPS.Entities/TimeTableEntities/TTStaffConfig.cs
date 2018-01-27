using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.TimeTableEntities
{
    public class TTStaffConfig
    {
        public virtual long Id { get; set; }
        public virtual long StaffId { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Class { get; set; }
        public virtual string Section { get; set; }
        public virtual string Subject { get; set; }
        public virtual string StaffName { get; set; }
        public virtual string LessonsPerWeek { get; set; }
        public virtual string ClassContinueity { get; set; }
    }
}
