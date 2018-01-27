using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.BioMetricsEntities
{
    public class Employees
    {
        public virtual long EmployeeId { get; set; }
        public virtual string EmployeeName { get; set; }
        public virtual string EmployeeCode { get; set; }
        public virtual Int32 RecordStatus { get; set; }

    }
}
