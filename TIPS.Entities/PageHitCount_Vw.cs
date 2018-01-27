using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace TIPS.Entities
{
    public class PageHitCount_Vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Action { get; set; }
        [DataMember]
        public virtual string Controller { get; set; }
        [DataMember]
        public virtual long HitCount { get; set; }
    }
}
