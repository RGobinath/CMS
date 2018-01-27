using System.Runtime.Serialization;
using System;


namespace TIPS.Entities.AdmissionEntities
{
    public class StudentTemplateView
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
        [DataMember]     
        public virtual string Name { get; set; }
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
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string NewId { get; set; }
        [DataMember]
        public virtual string DOB { get; set; }
        [DataMember]
        public virtual bool Transport { get; set; }
        [DataMember]
        public virtual bool Food { get; set; }
        [DataMember]
        public virtual string BoardingType { get; set; }
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
        public virtual string FoodPreference { get; set; }

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
        public virtual string EntryFrom { get; set; }

        /// Written by felix kinoniya
        public virtual bool IsPromotionOrTransfer { get; set; }

        //added by micheal
        [DataMember]
        public virtual string Initial { get; set; }
        [DataMember]
        public virtual string MobileNo { get; set; }
        [DataMember]
        public virtual string RouteId { get; set; }
        [DataMember]
        public virtual string LocationName { get; set; }
        //Added by Gobi for Route Configuration report
        [DataMember]
        public virtual string RouteAllocation { get; set; }

    }
}
