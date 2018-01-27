using System;

namespace TIPS.Entities.EnquiryEntities
{
    public class FollowUpDetails
    {
        public virtual long FollowUpDetailsId { get; set; }
        public virtual long EnquiryDetailsId { get; set; }
        public virtual string FollowUpRemarks { get; set; }
        public virtual DateTime? StaffFollowUpDate { get; set; }
        public virtual DateTime? NextFollowUpDate { get; set; }
        public virtual string StaffName { get; set; }
    }
}