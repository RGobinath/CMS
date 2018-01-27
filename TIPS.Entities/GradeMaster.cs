using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities
{
    [DataContract]
    public class GradeMaster
    {
        [DataMember]
        public virtual long FormId { get; set; }
        [DataMember]
        public virtual string FormCode { get; set; }
        [DataMember]
        public virtual int grad { get; set; }
        [DataMember]
        public virtual string gradcod { get; set; }
        [DataMember]
        public virtual string graddesc { get; set; }
        [DataMember]
        public virtual string Code { get; set; }
        [DataMember]
        public virtual string Code1 { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual string CreatedDate { get; set; }
        [DataMember]
        public virtual string UpdatedBy { get; set; }
        [DataMember]
        public virtual string UpdatedDate { get; set; }
        //[DataMember]
        //public virtual string Campus { get; set; }

    }
}
