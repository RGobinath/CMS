using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.QAEntities
{
    public class QADetails
    {
        //Vw_GetQADetails
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long QuestionId { get; set; }
        [DataMember]
        public virtual string Subject { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string Question { get; set; }
        [DataMember]
        public virtual string RaisedBy { get; set; }
        [DataMember]
        public virtual string StudentName { get; set; }
        [DataMember]
        public virtual DateTime RaisedDate { get; set; }
        [DataMember]
        public virtual string IsAnswered { get; set; }
        [DataMember]
        public virtual string AssignedTo { get; set; }
        [DataMember]
        public virtual long EscalationPeriod { get; set; }
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
        public virtual long Replies { get; set; }
        public virtual long Views { get; set; }
        public virtual string QuestionTitle { get; set; }
        public virtual string Section { get; set; }
    }
}
