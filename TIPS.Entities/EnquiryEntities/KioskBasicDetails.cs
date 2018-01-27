using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.EnquiryEntities
{
   public  class KioskBasicDetails
    {
        public virtual long Id { get; set; }
        public virtual long? StudentId { get; set; }
        public virtual string StudentName { get; set; }
        public virtual string AcademicYear{ get; set; }
        public virtual string Campus { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Section { get; set; }
        public virtual string Phone { get; set; }
        public virtual string EmailId { get; set; }//VisitPurpose
        public virtual string VisitPurpose { get; set; }
    }
}
