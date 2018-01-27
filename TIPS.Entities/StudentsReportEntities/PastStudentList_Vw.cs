using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.StudentsReportEntities
{
    public class PastStudentList_Vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual Int64 Flag { get; set; }
        [DataMember]
        public virtual string AcademicYear {get;set;}
        [DataMember]
        public virtual Int64 Boys { get; set; }
        [DataMember]
        public virtual Int64 Girls { get; set; }
        [DataMember]
        public virtual Int64 Total { get; set; }

        
    }
}
