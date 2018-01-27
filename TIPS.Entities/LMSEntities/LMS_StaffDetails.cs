using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.LMSEntities
{
    public class LMS_StaffDetails
    {
        public virtual long Id { get; set; }
        public virtual long StaffPreRegNumber { get; set; }
        public virtual string StaffIdNumber { get; set; }
        public virtual string StaffName { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Subject { get; set; }
        public virtual string Section { get; set; }
        public virtual string AcademicYear { get; set; }   
        public virtual string UserId { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
    }
}
