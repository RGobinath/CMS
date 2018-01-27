using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StoreEntities
{
    [DataContract]
   public class StockAudit
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long StockId { get; set; }
        [DataMember]
        public virtual DateTime CreateDate { get; set; }
        [DataMember]
        public virtual DateTime ModifiedDate { get; set; }
        [DataMember]
        public virtual int OldCount { get; set; }
        [DataMember]
        public virtual int Credit { get; set; }
        [DataMember]
        public virtual int Debit { get; set; }
        [DataMember]
        public virtual int Total { get; set; }
    }
}
