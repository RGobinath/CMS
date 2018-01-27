using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.Assess.ReportCardClasses
{
    [DataContract]
    public class ReportCardRequest
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string RequestNo{get;set;}
        [DataMember]
        public virtual string Campus{get;set;}
        [DataMember]
        public virtual string Grade{get;set;}
        [DataMember]
        public virtual string Section{get;set;}
        [DataMember]
        public virtual string GroupName { get; set; }
        [DataMember]
        public virtual string AcademicYear{get;set;}
        [DataMember]
        public virtual long TofWorkingDayT1 { get; set; }
        [DataMember]
        public virtual long TofWorkingDayT2 { get; set; }
        [DataMember]
        public virtual DateTime CreatedDate{get;set;}
        [DataMember]
        public virtual string CreatedBy{get;set;}
        [DataMember]
        public virtual string ModifiedBy{get;set;}
        [DataMember]
        public virtual DateTime ModifiedDate { get; set; }

        //forUse Model
        [DataMember]
        public virtual string Subject { get; set; }
          [DataMember]
        public virtual string Language { get; set; }
        
        
    }
}
