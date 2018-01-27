using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.EnquiryEntities
{
    public class KioskEnquiryDetails
    {
        public virtual long Enq_Id { get; set; }
        public virtual string StudentName { get; set; }
        public virtual string FatherName { get; set; }
        public virtual string Campus { get; set; }
        public virtual string MotherName { get; set; }
        public virtual string AcademicYear { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Gender { get; set; }
        public virtual string DOB { get; set; }
        public virtual string ParentName { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Address { get; set; }
        public virtual string Locality { get; set; }
        public virtual string EmailId { get; set; }
        public virtual string Enq_Number { get; set; }
    }
}
