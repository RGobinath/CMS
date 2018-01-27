using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.EnquiryEntities
{
   public class EnquiryFollowUp_vw
    {
        public virtual long EnquiryDetailsId { get; set; }
        public virtual string StudentName { get; set; }
        public virtual string Program { get; set; }
        public virtual string Course { get; set; }
        public virtual string CourseType { get; set; }
        public virtual string Campus { get; set; } // Centre
        public virtual string FollowUpRemarks { get; set; }
        public virtual DateTime? StaffFollowUpDate { get; set; }
        public virtual DateTime? NextFollowUpDate { get; set; }
        public virtual Int32 Count { get; set; }

        public virtual string EnquiryDetailsCode { get; set; }
        public virtual string ParentName { get; set; }
        public virtual string EnquirerMobileNo { get; set; }
        public virtual string EnquirerEmailId { get; set; }


    }
}
