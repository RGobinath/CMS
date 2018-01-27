using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.LMSEntities
{
    public class LMS_StaffStudentDetails_Vw
    {
        public virtual long Id { get; set; }
        public virtual long PreRegNumber { get; set; }
        public virtual string IdNumber { get; set; }
        public virtual string Name { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Subject { get; set; }
        public virtual string Section { get; set; }
        public virtual string AcademicYear { get; set; }   
        public virtual bool IsActive { get; set; }
        public virtual string UserType { get; set; }
    }
}
