using System;
using System.Collections;

namespace TIPS.Entities.EnquiryEntities
{
    public class EnquiryDetails
    {
        public virtual long EnquiryDetailsId { get; set; }
        public virtual string Board { get; set; }

        public virtual string EnquirerMobileNo { get; set; }
        public virtual string EnquirerEmailId { get; set; }
        public virtual string EnquirerComments { get; set; }
        public virtual DateTime? EnquiredDate { get; set; }
        public virtual string EnquiryStatus { get; set; }
        public virtual string AdmittedRefNo { get; set; }
        public virtual DateTime? AdmittedDate { get; set; }
        public virtual string EnquiryDetailsCode { get; set; }
        public virtual string ParentName { get; set; }
        public virtual DateTime? DOB { get; set; }
        public virtual int? Age { get; set; }
        public virtual string KnownThrough { get; set; }
        public virtual DateTime? FollowupDate { get; set; }
        public virtual string Enrolled { get; set; }
        public virtual string LanguagesKnown { get; set; }
        public virtual string Gender { get; set; }
        public virtual string StudentName { get; set; }
        public virtual string Initial { get; set; }
        public virtual string Campus { get; set; }
        public virtual IEnumerable Subjects { get; set; }
        public virtual string EnquiryThrough { get; set; }
        public virtual string Location { get; set; }
        public virtual bool? SendMessage { get; set; }

        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        
        public virtual string School { get; set; }
        /// <summary>
        /// newly added
        /// </summary>
        /// 
        public virtual string ReciveEmail { get; set; }
        public virtual string Program { get; set; }
        public virtual string Course { get; set; }
        public virtual string CourseType { get; set; }
        public virtual string Batch { get; set; }
        public virtual string Timing { get; set; }
        public virtual string Phone { get; set; }
        public virtual string FollowupDate1 { get; set; }
        public virtual string Subject { get; set; }
        public virtual string EmailContent { get; set; }
        public virtual string CourseEntryId { get; set; }
        public virtual string Grade { get; set; }

    }
}