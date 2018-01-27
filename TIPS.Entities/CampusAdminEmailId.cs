using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities
{
    public class CampusAdminEmailId
    {
        [DataMember]
        public virtual int Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Server { get; set; }
        [DataMember]
        public virtual string EmailId { get; set; }
        [DataMember]
        public virtual string Password { get; set; }
    }
}
