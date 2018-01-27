using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.TimeTableEntities
{
    public class TTStaffMaster
    {
        public virtual long Id { get; set; }
        public virtual string Campus { get; set; }
        public virtual string StaffName { get; set; }
        public virtual string Grade { get; set; }
    }
}
