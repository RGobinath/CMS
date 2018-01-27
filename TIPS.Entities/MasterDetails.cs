using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace TIPS.Entities
{
    [DataContract]
    public class MasterDetails
    {
        [DataMember]
        public virtual int Id { get; set; }
        [DataMember]
        public virtual string Masters { get; set; }
    }
}
