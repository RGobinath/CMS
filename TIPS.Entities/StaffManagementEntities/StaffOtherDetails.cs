using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StaffManagementEntities
{
    public class StaffOtherDetails
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual Int32 PreRegNum { get; set; }
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
    }
}
