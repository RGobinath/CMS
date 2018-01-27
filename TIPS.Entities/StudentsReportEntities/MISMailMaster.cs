using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.StudentsReportEntities
{
    [DataContract]
    public class MISMailMaster
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string EmailId { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string EmailType { get; set; }
        [DataMember]
        public virtual string SentCategory { get; set; }
        [DataMember]
        public virtual bool IsMailNeeded { get; set; }
    }
}
