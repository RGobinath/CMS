using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StaffManagementEntities
{
    [DataContract]
    public class StaffDetailsEdit
    {
        [DataMember]
        public virtual Int64 Id { get; set; }
        [DataMember]
        public virtual Int32 StaffTableId { get; set; }
        [DataMember]
        public virtual Int32 PreRegNum { get; set; }
        [DataMember]
        public virtual string StaffUserName { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
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
        public virtual string NoticePeriod { get; set; }
        [DataMember]
        public virtual string ExpectedSalary { get; set; }
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
    }
}
