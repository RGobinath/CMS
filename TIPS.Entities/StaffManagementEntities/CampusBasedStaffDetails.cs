using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.StaffManagementEntities
{
    public class CampusBasedStaffDetails
    {
        public virtual long Id { get; set; }
        public virtual long StaffPreRegNumber { get; set; }
        public virtual string StaffName { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Subject { get; set; }
        public virtual string Section { get; set; }
        public virtual string AcademicYear { get; set; }   
        public virtual string UserId { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual string StaffType { get; set; }

        // Added by Gobi for Staff_AttendanceReportConfiguration CRUD Operation
        public virtual string Department { get; set; }
        public virtual string SubDepartment { get; set; }
        public virtual string Programme { get; set; }
        public virtual string Designation { get; set; }
        public virtual string ReportingLevel { get; set; }
        public virtual string ReportingDesignation { get; set; }
        // For Search only
        public virtual string ConfigurationStatus { get; set; }
        public virtual string ReportingHeads { get; set; }
    }
}
