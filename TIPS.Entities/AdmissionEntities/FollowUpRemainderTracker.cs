using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace TIPS.Entities.AdmissionEntities
{
    public class FollowUpRemainderTracker
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string SentList { get; set; }
        [DataMember]
        public virtual string CheckDate { get; set; }
        [DataMember]
        public virtual bool IsSent { get; set; }
         [DataMember]
        public virtual string RemainderType { get; set; }
    }
}
