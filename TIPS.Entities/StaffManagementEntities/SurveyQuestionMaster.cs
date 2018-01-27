using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.StaffManagementEntities
{
    [DataContract]
    public class SurveyQuestionMaster
    {
        [DataMember]
        public virtual long SurveyQuestionId { get; set; }
        [DataMember]
        public virtual string SurveyQuestion { get; set; }
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
        public virtual SurveyGroupMaster SurveyGroupMaster { get; set; }
    }
}
