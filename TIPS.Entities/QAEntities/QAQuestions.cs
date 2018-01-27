using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace TIPS.Entities.QAEntities
{
    [DataContract]
    public class QAQuestions
    {
        [DataMember]
        public virtual long Id { get; set; }
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
        public virtual Char IsAnswered { get; set; }
        [DataMember]
        public virtual string AssignedTo { get; set; }
        [DataMember]
        public virtual long EscalationPeriod { get; set; }
        [DataMember]
        public virtual string QuestionTitle { get; set; }
        public virtual long Replies { get; set; }
        public virtual long Views { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Section { get; set; }
        public virtual string QuestionRefId { get; set; }
        public virtual DateTime ExpiryDate { get; set; }
        public virtual string Status { get; set; }
        public virtual string ExpiryStatus { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
    }
}
