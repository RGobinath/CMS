using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace TIPS.Entities
{
    [DataContract]
    public class Application
    {
        [DataMember]
        public virtual Int32 Id { get; set; }
        [DataMember]
        public virtual string AppCode { get; set; }
        [DataMember]
        public virtual string AppName { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime? ModifiedDate { get; set; }

    }
}
