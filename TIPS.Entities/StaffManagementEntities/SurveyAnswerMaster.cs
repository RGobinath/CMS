using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.StaffManagementEntities
{
    public class SurveyAnswerMaster
    {
        public virtual long SurveyAnswerId { get; set; }
        public virtual string SurveyAnswer { get; set; }
        public virtual long SurveyMark { get; set; }
        public virtual bool IsPositive { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual SurveyQuestionMaster SurveyQuestionMaster { get; set; }
    }
}
