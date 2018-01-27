using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.BioMetricsEntities
{
    public class Staff_AttendanceLongleaveAndResignedReport_vw
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string IdNumber { get; set; }
        public virtual long PreRegNum { get; set; }
        public virtual string Month { get; set; }
        public virtual long Year { get; set; }
        public virtual string Status { get; set; }
        public virtual DateTime? DateOfLongLeaveAndResigned { get; set; }
    }
}
