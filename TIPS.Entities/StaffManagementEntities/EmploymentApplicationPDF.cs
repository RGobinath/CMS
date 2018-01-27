using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StaffManagementEntities
{
    public class EmploymentApplicationPDF
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string TipsLogo { get; set; }
        [DataMember]
        public virtual string NaceLogo { get; set; }
        [DataMember]
        public virtual string StaffPhoto { get; set; }
        [DataMember]
        public virtual Int32 PreRegNum { get; set; }
        [DataMember]
        public virtual string Designation { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string Gender { get; set; }
        [DataMember]
        public virtual string DOB { get; set; }
        [DataMember]
        public virtual string PhoneNo { get; set; }
        [DataMember]
        public virtual string Alt_PhoneNo { get; set; }
        [DataMember]
        public virtual string NativeState { get; set; }
        [DataMember]
        public virtual string EmailId { get; set; }
        [DataMember]
        public virtual string BGRP { get; set; }
        [DataMember]
        public virtual string MaritalStatus { get; set; }
        [DataMember]
        public virtual string Spoken_LanguagesKnown { get; set; }
        [DataMember]
        public virtual string Written_LanguagesKnown { get; set; }
        [DataMember]
        public virtual string FatherName { get; set; }
        [DataMember]
        public virtual string FatherOccupation { get; set; }
        [DataMember]
        public virtual string SpouseName { get; set; }
        [DataMember]
        public virtual string SpouseOccupation { get; set; }
        [DataMember]
        public virtual string CorrespondenceAddress { get; set; }
        [DataMember]
        public virtual string PermanantAddress { get; set; }
        [DataMember]
        public virtual string EmrgcyContPrsn { get; set; }
        [DataMember]
        public virtual string EmrgcyContNumber { get; set; }
        [DataMember]
        public virtual IList<StaffFamilyDetails> StaffFamilyDetailsList { get; set; }
        [DataMember]
        public virtual IList<StaffQualification> StaffQualificationDetailsList { get; set; }
        [DataMember]
        public virtual IList<StaffExperience> StaffExperienceDetailsList { get; set; }
        [DataMember]
        public virtual string TotYrOfExp { get; set; }
        [DataMember]
        public virtual string TotYrOfTeachExp { get; set; }
        [DataMember]
        public virtual string SyllabusHandled { get; set; }
        [DataMember]
        public virtual string SubjectsTaught { get; set; }
        [DataMember]
        public virtual string GradesTaught { get; set; }
        [DataMember]
        public virtual string Achievments { get; set; }
        [DataMember]
        public virtual string ExpectedSalary { get; set; }
        [DataMember]
        public virtual string LastDrawnGrossSalary { get; set; }
        [DataMember]
        public virtual string LastDrawnNettSalary { get; set; }
        [DataMember]
        public virtual string JoiningDateOrDays { get; set; }
        //Other Details
        [DataMember]
        public virtual string AnyOtherSignificant { get; set; }
        [DataMember]
        public virtual string SpecialInterests { get; set; }
        [DataMember]
        public virtual string HowYouKnowVacancy { get; set; }
        //[DataMember]
        //public virtual bool BeenShortListedBefore { get; set; }
        [DataMember]
        public virtual string BeenShortListedBefore { get; set; }
        [DataMember]
        public virtual string ShortlistedWhy { get; set; }
        //[DataMember]
        //public virtual bool RelativeWorkingWithUs { get; set; }
        [DataMember]
        public virtual string RelativeWorkingWithUs { get; set; }
        [DataMember]
        public virtual string RelativeDetails { get; set; }
        [DataMember]
        public virtual string CommitTimeWithTIPS { get; set; }
        [DataMember]
        public virtual string CareerGrowthExpectation { get; set; }
        [DataMember]
        public virtual string WillingToTravel { get; set; }
        [DataMember]
        public virtual string WillingForRelocation { get; set; }
        [DataMember]
        public virtual IList<StaffReferenceDetails> StaffReferenceDetailsList { get; set; }
        [DataMember]
        public virtual string CurrentDate { get; set; }
    }
}
