using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StoreEntities
{
    [DataContract]
   public class StoreMaster
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Store { get; set; }
        [DataMember]
        public virtual string MainStore { get; set; }
  
    }
}
