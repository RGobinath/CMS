using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.StaffManagementEntities
{
    public class StaffEvaluationParameter
    {
        [DataMember]
        public virtual long StaffEvaluationParameterId { get; set; }
        [DataMember]
        public virtual long StaffEvaluationCategoryId { get; set; }
        [DataMember]
        public virtual string StaffEvaluationParameters { get; set; }
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
        public virtual bool IsPositive { get; set; }

    }
}
