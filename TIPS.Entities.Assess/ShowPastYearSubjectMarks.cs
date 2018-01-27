using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.Assess
{
    [DataContract]
    public class ShowPastYearSubjectMarks
    {
        [DataMember]
        public virtual long Id { set; get; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string Subject { get; set; }
        [DataMember]
        public virtual string Semester { get; set; }
        [DataMember]
        public virtual string Test1 { get; set; }
        [DataMember]
        public virtual string Test2 { get; set; }
        [DataMember]
        public virtual string Test3 { get; set; }
        [DataMember]
        public virtual string Project { get; set; }
        [DataMember]
        public virtual string FATotal { get; set; }
        [DataMember]
        public virtual string SATotal { get; set; }
        [DataMember]
        public virtual string Outof { get; set; }
        [DataMember]
        public virtual string Fa1andFa2 { get; set; }
        [DataMember]
        public virtual string SEMGrade { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }

        //[DataMember]
        //public virtual DateTime? CreatedDate { get; set; }
        //[DataMember]
        //public virtual DateTime? ModifiedDate { get; set; }

        [DataMember]
        public virtual string SecondLanguage { get; set; }


    }
}
