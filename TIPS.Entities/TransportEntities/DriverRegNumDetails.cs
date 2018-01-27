using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.TransportEntities
{
        [DataContract]
    public class DriverRegNumDetails
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual Int32 PreRegNum { get; set; }
        [DataMember]
        public virtual string Date { get; set; }
        [DataMember]
        public virtual string Time { get; set; }
     
    }
}
