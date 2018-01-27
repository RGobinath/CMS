using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.StaffManagementEntities
{
    public class CandidateDtls
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
        [DataMember]
        public virtual string Salutation { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string Gender { get; set; }
        [DataMember]
        public virtual string DOB { get; set; }
        [DataMember]
        public virtual Int32 Age { get; set; }
        [DataMember]
        public virtual string BGRP { get; set; }
        [DataMember]
        public virtual string MaritalStatus { get; set; }
        [DataMember]
        public virtual string PhoneNo { get; set; }
        [DataMember]
        public virtual string AltPhoneNo { get; set; }
        [DataMember]
        public virtual string EmailId { get; set; }
        [DataMember]
        public virtual string EmergencyContactPerson { get; set; }
        [DataMember]
        public virtual string EmergencyContactNo { get; set; }
        [DataMember]
        public virtual string NativeCountry { get; set; }
        [DataMember]
        public virtual string NativeState { get; set; }
        [DataMember]
        public virtual string NativeCity { get; set; }
        [DataMember]
        public virtual string FatherName { get; set; }
        [DataMember]
        public virtual string FatherOccupation { get; set; }
        [DataMember]
        public virtual string SpouseName { get; set; }
        [DataMember]
        public virtual string SpouseOccupation { get; set; }
        [DataMember]
        public virtual string LanguagesKnown { get; set; }
        [DataMember]
        public virtual string Written_LanguagesKnown { get; set; }
        [DataMember]
        public virtual string AadhaarNo { get; set; }
        [DataMember]
        public virtual string LocationPreferred { get; set; }
        [DataMember]
        public virtual string InterestedArea { get; set; }
        [DataMember]
        public virtual string Qualification { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Designation { get; set; }
        [DataMember]
        public virtual string Department { get; set; }
        [DataMember]
        public virtual string AlternateAddress { get; set; }
        [DataMember]
        public virtual string PermanantAddress { get; set; }
        [DataMember]
        public virtual string JobResponsibility { get; set; }
        [DataMember]
        public virtual string Achievments { get; set; }
        [DataMember]
        public virtual string InterviewMode { get; set; }
        [DataMember]
        public virtual string TotalYearsOfExp { get; set; }
        [DataMember]
        public virtual string TotalYearsOfTeachingExp { get; set; }
        [DataMember]
        public virtual string SyllabusHandled { get; set; }
        [DataMember]
        public virtual string SubjectsTaught { get; set; }
        [DataMember]
        public virtual string GradesTaught { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual string AnyOtherSignificant { get; set; }
        [DataMember]
        public virtual string SpecialInterests { get; set; }
        [DataMember]
        public virtual string HowYouKnowVacancy { get; set; }
        [DataMember]
        public virtual bool BeenShortListedBefore { get; set; }
        [DataMember]
        public virtual string ShortlistedWhy { get; set; }
        [DataMember]
        public virtual bool RelativeWorkingWithUs { get; set; }
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
        public virtual string LastDrawnGrossSalary { get; set; }
        [DataMember]
        public virtual string LastDrawnNettSalary { get; set; }
        [DataMember]
        public virtual string ExpectedSalary { get; set; }
        [DataMember]
        public virtual string JoiningDateOrDays { get; set; }
        [DataMember]
        public virtual string TempId{ get; set; }
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime? ModifiedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual string FamilyDetails { get; set; }
        [DataMember]
        public virtual string AppliedFor { get; set; }
        [DataMember]
        public virtual string QualificationDetails { get; set; }
        [DataMember]
        public virtual string ExperienceDetails { get; set; }
        [DataMember]
        public virtual string ReferenceDetails { get; set; }
        [DataMember]
        public virtual string EmergencyContactRelationship { get; set; }
        [DataMember]
        public virtual string PANNo { get; set; }

    }
}
