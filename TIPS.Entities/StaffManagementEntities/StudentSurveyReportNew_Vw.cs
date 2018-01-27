using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.StaffManagementEntities
{
    public class StudentSurveyReportNew_Vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long StaffEvaluationCategoryId { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string CategoryName { get; set; }
        [DataMember]
        public virtual DateTime EvaluationDate { get; set; }
        [DataMember]
        public virtual long StaffPreRegNumber { get; set; }
        [DataMember]
        public virtual string StaffName { get; set; }
        [DataMember]
        public virtual string Subject { get; set; }
        [DataMember]
        public virtual Int64 StudentCount { get; set; }
        [DataMember]
        public virtual Int64 QuestionCount { get; set; }
        [DataMember]
        public virtual decimal Score { get; set; }
        [DataMember]
        public virtual decimal Average { get; set; }
    }
}
