using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace TIPS.Entities.QAEntities
{
    public class QAAnswers
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long QuestionId { get; set; }
        [DataMember]
        public virtual string Answer { get; set; }
        [DataMember]
        public virtual string AnsweredBy { get; set; }
        [DataMember]
        public virtual string StaffName { get; set; }
        [DataMember]
        public virtual DateTime AnsweredDate { get; set; }
        [DataMember]
        public virtual DateTime LastModifiedDate { get; set; }
        public virtual string Campus { get; set; }
        public virtual long Likes { get; set; }
    }
}

