using System;

namespace TIPS.Entities.EnquiryEntities
{
    public class EnquiryDetailGridView
    {
        public virtual long EnquiryDetailsId { get; set; }
        public virtual long EnquiryId { get; set; }
        public virtual string Board { get; set; }
        public virtual string EnquirerMobileNo { get; set; }
        public virtual string EnquirerEmailId { get; set; }
        public virtual DateTime? EnquiredDate { get; set; }
        public virtual string ParentName { get; set; }
        public virtual string EnquiryDetailsCode { get; set; }
        public virtual string StudentName { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual string Campus { get; set; }
        public virtual string CourseProgram { get; set; }
        public virtual string EnquiryStatus { get; set; }
        public virtual DateTime? FollowupDate { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
    }
}
