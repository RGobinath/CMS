﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.StaffManagementEntities
{
    [DataContract]
    public class StudentSurveyQuestionMaster
    {
        [DataMember]
        public virtual long StudentSurveyQuestionId { get; set; }
        [DataMember]
        public virtual string StudentSurveyQuestion { get; set; }
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
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }

        public virtual StudentSurveyGroupMaster StudentSurveyGroupMaster { get; set; }
    }
}
