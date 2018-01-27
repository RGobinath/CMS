using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;


namespace TIPS.Entities.AdmissionEntities
{
    public class StudentTemplate
    {
        [DataMember]
        public virtual long Id { get; set; }

        [DataMember]
        public virtual long PreRegNum { get; set; }

        //[Required]
        [DataMember]     
        public virtual string Name { get; set; }

        //[Required]
        [DataMember]
        public virtual string Gender { get; set; }

        [DataMember]
        public virtual string FeeStructYear { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual string Status1 { get; set; }
        [DataMember]
        public virtual string AdmissionStatus { get; set; }
        [DataMember]
        public virtual string EnquiryLocationFrom { get; set; }


        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string NewId { get; set; }
        //[Required]
        [DataMember]
        public virtual string DOB { get; set; }
        //[DataMember]
        //public virtual string Photo { get; set; }

        //[Required]
        [DataMember]
        public virtual bool Transport { get; set; }

        //[Required]
        [DataMember]
        public virtual bool Food { get; set; }

        //[Required]
        [DataMember]
        public virtual string BoardingType { get; set; }

        //[DataMember]
        //public virtual string BirthCert { get; set; }
        [DataMember]
        public virtual bool EducationGoalYesorNo { get; set; }

        [DataMember]
        public virtual string EducationGoals { get; set; }

        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string JoiningAcademicYear { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }

        [DataMember]
        public virtual string Grade { get; set; }

        [DataMember]
        public virtual string ApplicationNo { get; set; }

        [DataMember]
        public virtual string BGRP { get; set; }

        [DataMember]
        public virtual string AnnualIncome { get; set; }

        [DataMember]
        public virtual string EmailId { get; set; }

        [DataMember]
        public virtual string Email { get; set; }

        [DataMember]
        public virtual string Subject { get; set; }

        [DataMember]
        public virtual bool Father { get; set; }

        [DataMember]
        public virtual bool Mother { get; set; }

        [DataMember]
        public virtual bool General { get; set; }

        [DataMember]
        public virtual string LanguagesKnown { get; set; }
        [DataMember]
        public virtual string CreatedDate { get; set; }
        [DataMember]
        public virtual DateTime CreatedDateNew { get; set; }
        [DataMember]
        public virtual string CreatedTime { get; set; }
        [DataMember]
        public virtual bool IsHosteller { get; set; }

        [DataMember]
        public virtual string VanNo { get; set; }

        [DataMember]
        public virtual IList<PaymentDetails> PaymentDetailsList { get; set; }
        [DataMember]
        public virtual IList<AddressDetails> AddressDetailsList { get; set; }
        [DataMember]
        public virtual IList<FamilyDetails> FamilyDetailsList { get; set; }        
        [DataMember]
        public virtual IList<PastSchoolDetails> PastSchoolDetailsList { get; set; }        
        [DataMember]
        public virtual IList<DocumentDetails> DocumentDetailsList { get; set; }
        [DataMember]
        public virtual IList<UploadedFiles> UploadedFilesList { get; set; }
        [DataMember]
        public virtual IList<ApproveAssign> ApproveAssignList { get; set; }

        //added by felix kinoniya
        public virtual long BulkPromTransferRequestId { get; set; }

        //added by micheal
        [DataMember]
        public virtual string Initial { get; set; }
        [DataMember]
        public virtual string MobileNo { get; set; }
        //Added by Gobi
        public virtual string SecondLanguage { get; set; }
        //added by felix kinoniya
        public virtual string BulkPreRegNum { get; set; }

        public virtual long ReportCount { get; set; }
        [DataMember]
        public virtual string FoodPreference { get; set; }

        [DataMember]
        public virtual string OperationalYear { get; set; }
        //For Transport Route Configuration Added by Gobi
        [DataMember]
        public virtual string LocationName { get; set; }
        [DataMember]
        public virtual string LocationTamilDescription { get; set; }
        [DataMember]
        public virtual string RouteId { get; set; }
        [DataMember]
        public virtual string LocationOtherDetails { get; set; }

        //Added by Mic for Locality Planner
        [DataMember]
        public virtual string Locality { get; set; }
        [DataMember]
        public virtual string Place { get; set; }
        /// Added by Anto
        /// 
        [DataMember]
        public virtual string Kilometer { get; set; }
        [DataMember]
        public virtual string Nationality { get; set; }
        [DataMember]
        public virtual string EntryFrom { get; set; }
        [DataMember]
        public virtual string PickUpTime { get; set; }
        [DataMember]
        public virtual string DropTime { get; set; }
        [DataMember]
        public virtual string DeviceType { get; set; }
    }
}
