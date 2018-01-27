using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using TIPS.Entities;
using TIPS.Entities.AdmissionEntities;


namespace TIPS.Entities.StaffManagementEntities
{
    [DataContract]
    public class StaffDetails
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual Int32 PreRegNum { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string StaffUserName { get; set; }
        [DataMember]
        public virtual string IdNumber { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Designation { get; set; }
        [DataMember]
        public virtual string Department { get; set; }
        [DataMember]
        public virtual string DateOfJoin { get; set; }
        [DataMember]
        public virtual string ReportingManager { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual string StaffType { get; set; }
        [DataMember]
        public virtual string DOB { get; set; }
        [DataMember]
        public virtual Int32 Age { get; set; }
        [DataMember]
        public virtual string BGRP { get; set; }
        [DataMember]
        public virtual string Gender { get; set; }
        [DataMember]
        public virtual string EmailId { get; set; }
        [DataMember]
        public virtual string PhoneNo { get; set; }
        [DataMember]
        public virtual string FatherName { get; set; }
        [DataMember]
        public virtual string SpouseName { get; set; }
        [DataMember]
        public virtual string MotherName { get; set; }
        [DataMember]
        public virtual string EmergencyContactPerson { get; set; }
        [DataMember]
        public virtual string EmergencyContactNo { get; set; }
        [DataMember]
        public virtual string InsuranceDetails { get; set; }
        [DataMember]
        public virtual string MaritalStatus { get; set; }
        [DataMember]
        public virtual string NativeState { get; set; }
        [DataMember]
        public virtual string ChildrenIfAny { get; set; }
        [DataMember]
        public virtual string LanguagesKnown { get; set; }
        [DataMember]
        public virtual string BankName { get; set; }
        [DataMember]
        public virtual string BankAccountNumber { get; set; }
        [DataMember]
        public virtual string PermanantAddress { get; set; }
        [DataMember]
        public virtual string AlternateAddress { get; set; }
        [DataMember]
        public virtual string Add1 { get; set; }
        [DataMember]
        public virtual string Add2 { get; set; }
        [DataMember]
        public virtual string City { get; set; }
        [DataMember]
        public virtual string State { get; set; }
        [DataMember]
        public virtual string Country { get; set; }
        [DataMember]
        public virtual string Pin { get; set; }
        //[DataMember]
        //public virtual IList<FileUpload> FileUploadList { get; set; }
        [DataMember]
        public virtual string PFNo { get; set; }
        [DataMember]
        public virtual string ESINo { get; set; }
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

        /// <summary>
        /// ///////////////
        /// </summary>

        [DataMember]
        public virtual string Achievments { get; set; }
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
        public virtual string CreatedDate { get; set; }
        [DataMember]
        public virtual string CreatedTime { get; set; }

        //Added By Gobi For Automatic B'Day Wishes
        [DataMember]
        public virtual bool IsBDayWishNeed { get; set; }

        [DataMember]
        public virtual IList<StaffQualification> StaffQualificationList { get; set; }

        [DataMember]
        public virtual IList<StaffExperience> StaffExperienceList { get; set; }

        //[DataMember]
        //public virtual IList<StaffOtherDetails> StaffOtherList { get; set; }

        [DataMember]
        public virtual IList<StaffTraining> StaffTrainingList { get; set; }

        //[DataMember]
        //public virtual IList<UploadedFiles> UploadedFilesList { get; set; }
        [DataMember]
        public virtual IList<UploadedFilesView> UploadedFilesList { get; set; }
        [DataMember]
        public virtual string DocCheck { get; set; }
        [DataMember]
        public virtual string QualCheck { get; set; }

        public virtual long ReportCount { get; set; }

        //Added By Gobi For Employment Form 
        [DataMember]
        public virtual string AltPhoneNo { get; set; }
        [DataMember]
        public virtual string Written_LanguagesKnown { get; set; }
        [DataMember]
        public virtual string FatherOccupation { get; set; }
        [DataMember]
        public virtual string SpouseOccupation { get; set; }
        //Family Details
        [DataMember]
        public virtual string FamilyName { get; set; }
        [DataMember]
        public virtual string FamilyOccupation { get; set; }
        [DataMember]
        public virtual string FamilyAge { get; set; }
        [DataMember]
        public virtual string FamilyRelationship { get; set; }
        [DataMember]
        public virtual IList<StaffFamilyDetails> StaffFamilyDetailsList { get; set; }
        [DataMember]
        public virtual string ExpectedSalary { get; set; }
        [DataMember]
        public virtual string LastDrawnGrossSalary { get; set; }
        [DataMember]
        public virtual string LastDrawnNettSalary { get; set; }
        [DataMember]
        public virtual string JoiningDateOrDays { get; set; }
        //Reference Details
        [DataMember]
        public virtual string RefName { get; set; }
        [DataMember]
        public virtual string RefContactNo { get; set; }
        [DataMember]
        public virtual string RefHowKnow { get; set; }
        [DataMember]
        public virtual string RefHowLongKnow { get; set; }
        [DataMember]
        public virtual IList<StaffReferenceDetails> StaffReferenceDetailsList { get; set; }
        //Added By Prabakaran
        [DataMember]
        public virtual string StaffGroup { get; set; }
        [DataMember]
        public virtual string StaffSubGroup { get; set; }
        [DataMember]
        public virtual string OfficialEmailId { get; set; }
        [DataMember]
        public virtual string TempIdNumber{ get; set; }

        public string Course { get; set; }
        public DateTime DateofRedsn { get; set; }
        public string CurDesg { get; set; }
        public DateTime Letterdate { get; set; }
        public DateTime DateofRelv { get; set; }
        public string RSCertfy { get; set; }
        public double AmountSalryApr { get; set; }
        public DateTime DateofSalryApr { get; set; }
        public double SalaryCerAmount { get; set; }
        public string AmtInWords { get; set; }
        public double PFAmount { get; set; }
        public DateTime DateofTrsfr { get; set; }
        public string TransferedCampus { get; set; }

        public string Honor { get; set; }
        public string WorkingType { get; set; }
        [DataMember]
        public virtual StaffProgrammeMaster StaffProgrammeMaster { get; set; }
        [DataMember]
        public virtual bool IsReportingManager { get; set; }
        [DataMember]
        public virtual string StaffCategoryForAttendane { get; set; }
        // Added by Naveen on 14 Nov 2017
        [DataMember]
        public virtual DateTime? ModifiedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime? DateOfLongLeaveAndResigned { get; set; }
        [DataMember]
        public virtual string Remarks { get; set; }
        [DataMember]
        public virtual string CurrentStatus { get; set; }
        public string LocationPreferred { get; set; }
        public string InterestedArea { get; set; }
        public string PositionorDepartmentapplied { get; set; }

    }
}
