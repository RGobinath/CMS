using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using TIPS.Entities.AdmissionEntities;

namespace TIPS.Entities.TransportEntities
{
    [DataContract]
    public class DriverMaster
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long DriverRegNo { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Dob { get; set; }
        [DataMember]
        public virtual long Age { get; set; }
        [DataMember]
        public virtual string ContactNo { get; set; }
        [DataMember]
        public virtual string PresentAddress { get; set; }
        [DataMember]
        public virtual string PermanentAddress { get; set; }
       
        [DataMember]
        public virtual string DriverIdNo { get; set; }
        [DataMember]
        public virtual string LicenseNo { get; set; }

        [DataMember]
        public virtual DateTime CreatedDate { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }

        [DataMember]
        public virtual DateTime ModifiedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual string BatchNo { get; set; }
        [DataMember]
        public virtual DateTime? LicenseValDate { get; set; }
        [DataMember]
        public virtual DateTime? NonTraLicenseValDate { get; set; }
        [DataMember]
        public virtual string DriverPhoto { get; set; }
        [DataMember]
        public virtual string Designation { get; set; }
        [DataMember]
        public virtual bool IsActive { get; set; }
        //Newly Added for Enhancing the system
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual DateTime DateOfJoin { get; set; }
        [DataMember]
        public virtual string PFNo { get; set; }
        [DataMember]
        public virtual string ESINo { get; set; }
        [DataMember]
        public virtual string ReportingManager { get; set; }
        [DataMember]
        public virtual string StaffType { get; set; }
        [DataMember]
        public virtual string Gender { get; set; }
        [DataMember]
        public virtual string TotalYearsOfExp { get; set; }
        [DataMember]
        public virtual string BGRP { get; set; }
        [DataMember]
        public virtual long ReportCount { get; set; }
        [DataMember]
        public virtual string MaritalStatus { get; set; }
        [DataMember]
        public virtual string PhoneNo { get; set; }
        [DataMember]
        public virtual string AltPhoneNo { get; set; }
        [DataMember]
        public virtual string EmailId { get; set; }
        [DataMember]
        public virtual string FatherName { get; set; }
        [DataMember]
        public virtual string SpouseName { get; set; }
        [DataMember]
        public virtual string MotherName { get; set; }
        [DataMember]
        public virtual string FatherOccupation { get; set; }
        [DataMember]
        public virtual string SpouseOccupation { get; set; }
        [DataMember]
        public virtual string EmergencyContactPerson { get; set; }
        [DataMember]
        public virtual string EmergencyContactNo { get; set; }
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
        public virtual string InsuranceDetails { get; set; }
        [DataMember]
        public virtual bool IsRegistered { get; set; }

        [DataMember]
        public virtual IList<UploadedFilesView> UploadedFilesList { get; set; }
        [DataMember]
        public virtual long JoiningSalary { get; set; }
        [DataMember]
        public virtual string NominatedBy { get; set; }
        [DataMember]
        public virtual string Relationship { get; set; }
        [DataMember]
        public virtual string LastWorked { get; set; }
        [DataMember]
        public virtual string WorkedLocation { get; set; }
        [DataMember]
        public virtual IList<DriverFamilyDetails> DriverFamilyDetailsList { get; set; } 
    }
}
