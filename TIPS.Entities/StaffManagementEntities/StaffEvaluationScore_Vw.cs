using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.StaffManagementEntities
{
    public class StaffEvaluationScore_Vw
    {
        [DataMember]
        public virtual long Id {get;set;}
        [DataMember]
        public virtual long PreRegNum {get;set;}
        [DataMember]
        public virtual string Name {get;set;}
        [DataMember]
        public virtual string IdNumber {get;set;}
        [DataMember]
        public virtual string Campus {get;set;}
        [DataMember]
        public virtual string Grade {get;set;}
        [DataMember]
        public virtual string Section {get;set;}
        [DataMember]
        public virtual string AcademicYear {get;set;}
        [DataMember]
        public virtual string Subject {get;set;}
        //[DataMember]
        //public virtual string Semester {get;set;}
        [DataMember]
        public virtual string Month { get; set; }
        [DataMember]
        public virtual long Entered{get;set;}            
        [DataMember]
        public virtual long TotalStudents{get;set;}            
        [DataMember]
        public virtual long TotalScore{get;set;}            
    }
}