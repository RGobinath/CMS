using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.BioMetricsEntities
{
    public class Staff_DeviceLogSummaryStatus_sp
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string IdNumber { get; set; }
        public virtual string Campus { get; set; }
        public virtual long PreRegNum { get; set; }
        public virtual string UserId { get; set; }
        public virtual DateTime? AttendanceDate { get; set; }
        public virtual long EmployeeId { get; set; }
        public virtual DateTime? LogDate { get; set; }
        public virtual string INOUT { get; set; }
    }
}
