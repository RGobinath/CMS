using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.StaffManagementEntities
{
    [DataContract]
    public class StudentSurveyAnswerMaster
    {
        [DataMember]
        public virtual long StudentSurveyAnswerId { get; set; }
        [DataMember]
        public virtual string StudentSurveyAnswer { get; set; }
        [DataMember]
        public virtual long StudentSurveyMark { get; set; }
        [DataMember]
        public virtual bool IsPositive { get; set; }
        [DataMember]
        public virtual bool IsActive { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime? ModifiedDate { get; set; }

        public virtual StudentSurveyQuestionMaster StudentSurveyQuestionMaster { get; set; }
    }
}
