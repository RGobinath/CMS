using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities
{
    public class CoOrdinatorsContactInfo
    {
        [DataMember]
        public virtual int Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Server { get; set; }
        [DataMember]
        public virtual string Category { get; set; }
        [DataMember]
        public virtual string EmailId { get; set; }
        [DataMember]
        public virtual string MobileNo { get; set; }
    }
}
