using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StoreEntities
{
    [DataContract]
   public class StoreStockBalance
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Store { get; set; }
        [DataMember]
        public virtual long ItemId { get; set; }
        [DataMember]
        public virtual int AMonth{ get; set; }
        [DataMember]
        public virtual int AYear { get; set; }
        [DataMember]
        public virtual int ClosingBalance { get; set; }
       
    }
}
