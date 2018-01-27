using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.StaffManagementEntities
{
    public class StaffBDayWishesMaster
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string GreetingCardImageURL { get; set; }
        [DataMember]
        public virtual string GreetingMessage { get; set; }
        [DataMember]
        public virtual bool IsUsed { get; set; }
        [DataMember]
        public virtual string Subject { get; set; }
    }
}
