using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StaffManagementEntities
{
    public class StaffReferenceDetails
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual Int32 PreRegNum { get; set; }
        [DataMember]
        public virtual string RefName { get; set; }
        [DataMember]
        public virtual string RefContactNo { get; set; }
        [DataMember]
        public virtual string RefHowKnow { get; set; }
        [DataMember]
        public virtual string RefHowLongKnow { get; set; }
    }
}
