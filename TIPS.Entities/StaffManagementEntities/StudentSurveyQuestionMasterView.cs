using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.StaffManagementEntities
{
    [DataContract]
    public class StudentSurveyQuestionMasterView
    {
        [DataMember]
        public virtual long SurveyQuestionId { get; set; }
        [DataMember]
        public virtual long StudentSurveyGroupId { get; set; }
        [DataMember]
        public virtual long StudentSurveyQuestionId { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }        
        [DataMember]
        public virtual string StudentSurveyGroup { get; set; }
        [DataMember]
        public virtual string StudentSurveyQuestion { get; set; }
        [DataMember]
        public virtual bool IsActive { get; set; }        
    }
}
