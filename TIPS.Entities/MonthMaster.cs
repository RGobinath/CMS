using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities
{
    public class MonthMaster
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string MonthName { get; set; }
        [DataMember]
        public virtual int MonthValue { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime CreatedDate { get; set; }
        [DataMember]
        public virtual string UpdateBy { get; set; }
        [DataMember]
        public virtual string UpdateDate { get; set; }
    }
}
