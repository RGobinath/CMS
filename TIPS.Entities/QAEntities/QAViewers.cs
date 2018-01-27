using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.QAEntities
{
    public class QAViewers
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long QuestionId { get; set; }
        [DataMember]
        public virtual string ViewedBy { get; set; }
        [DataMember]
        public virtual DateTime ViewedDate { get; set; }
    }
}
