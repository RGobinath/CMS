using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.HRManagementEntities
{
    [DataContract]
   public class RequestNumber
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long Count { get; set; }

    }
}
