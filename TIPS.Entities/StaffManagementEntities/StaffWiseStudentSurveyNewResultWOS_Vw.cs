using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.StaffManagementEntities
{
    public class StaffWiseStudentSurveyNewResultWOS_Vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual long StaffPreRegNumber { get; set; }
        [DataMember]
        public virtual string StaffName { get; set; }
        [DataMember]
        public virtual string Subject { get; set; }
        [DataMember]
        public virtual long QuestionCount { get; set; }
        [DataMember]
        public virtual long StudentCount { get; set; }
        [DataMember]
        public virtual decimal Score { get; set; }
        [DataMember]
        public virtual decimal Average { get; set; }

        [DataMember]
        public virtual string CategoryName { get; set; }
    }
}

