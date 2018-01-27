using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StoreEntities
{
    [DataContract]
   public class StockTransaction
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string TransactionCode { get; set; }
        [DataMember]
        public virtual string Store { get; set; }
        [DataMember]
        public virtual long ItemId { get; set; }
        [DataMember]
        public virtual string Units { get; set; }
        [DataMember]
        public virtual DateTime TransactionDate { get; set; }
        [DataMember]
        public virtual string TransactionBy { get; set; }
        [DataMember]
        public virtual string TransactionType { get; set; }
        [DataMember]
        public virtual int? Qty { get; set; }
        [DataMember]
        public virtual int DamagedQty { get; set; }
        //[DataMember]
        //public virtual string DamagedRemarks { get; set; }
        [DataMember]
        public virtual string TransactionComments { get; set; }
        [DataMember]
        public virtual string RequiredForStore { get; set; }
        [DataMember]
        public virtual string RequiredFromStore { get; set; }
    }
}
