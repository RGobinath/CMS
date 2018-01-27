using System.Collections.Generic;
using System.Text;
using System;
using System.Runtime.Serialization;

namespace TIPS.Entities.Assess
{
    [DataContract]
    public class AssessCompMaster
    {
        [DataMember]
        public virtual int Id { get; set; }
        [DataMember]
        public virtual string AssessCompGroup { get; set; }
        [DataMember]
        public virtual string CompName { get; set; }
        [DataMember]
        public virtual bool? IsCredit { get; set; }
        [DataMember]
        public virtual string Mark { get; set; }
        [DataMember]
        public virtual int GroupId { get; set; } 
    }
}
