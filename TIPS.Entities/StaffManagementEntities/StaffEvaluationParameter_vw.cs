using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.StaffManagementEntities
{
    public class StaffEvaluationParameter_vw
    {
        [DataMember]
        public virtual long Id {get;set;}
        [DataMember]
        public virtual long StaffEvaluationParameterId {get;set;}
        [DataMember]
        public virtual long StaffEvaluationCategoryId {get;set;}
        [DataMember]
        public virtual string Campus {get;set;}
        [DataMember]
        public virtual string Grade {get;set;}
        [DataMember]
        public virtual string AcademicYear {get;set;}
        [DataMember]
        public virtual string Month { get; set; }
        [DataMember]
        public virtual string CategoryName {get;set;}
        [DataMember]
        public virtual bool IsActive {get;set;}
        [DataMember]
        public virtual string StaffEvaluationParameters{get;set;}
        [DataMember]
        public virtual bool IsPositive { get; set; }   
    }
}
