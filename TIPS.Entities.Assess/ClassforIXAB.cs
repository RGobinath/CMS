using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.Assess
{
    [DataContract]
    public class ClassforIXAB
    {
        [DataMember]
        public virtual long Id { set; get; }
        [DataMember]
        public virtual string Subject { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime CreatedDate { get; set; }
        [DataMember]
        public virtual DateTime ModifiedDate { get; set; }


        [DataMember]
        public virtual string Marks { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }


       
    }
}
