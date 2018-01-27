using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.StaffManagementEntities
{
    public class CampusBasedStaffDetails_Vw
    {
        public virtual long Id { get; set; }
        public virtual long StaffPreRegNumber { get; set; }
        public virtual string StaffName { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Department { get; set; }
        public virtual string SubDepartment { get; set; }
        public virtual string Programme { get; set; }
        public virtual string Designation { get; set; }
        public virtual string StaffType { get; set; }
        public virtual string AcademicYear { get; set; }
        public virtual string ConfigurationStatus { get; set; }
        public virtual string ReportingHeads { get; set; }
    }
}
