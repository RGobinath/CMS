using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StoreEntities
{
    [DataContract]
    public class SkuList
    {
        [DataMember]
        public virtual long SkuId { get; set; }
        [DataMember]
        public virtual long MaterialRefId { get; set; }
        [DataMember]
        public virtual string MaterialGroup { get; set; }
        [DataMember]
        public virtual string MaterialSubGroup { get; set; }
        [DataMember]
        public virtual string Material { get; set; }
        [DataMember]
        public virtual string OrderedUnits { get; set; }
        [DataMember]
        public virtual int? OrderQty { get; set; }
        [DataMember]
        public virtual int? ReceivedQty { get; set; }
        [DataMember]
        public virtual int? DamagedQty { get; set; }
        [DataMember]
        public virtual int? DamagelessQty { get; set; }
        [DataMember]
        public virtual int? IssuedQty { get; set; }
        [DataMember]
        public virtual int? StockAvailableQty { get; set; }
        [DataMember]
        public virtual string IssuedStatus { get; set; }
        [DataMember]
        public virtual string DamageDescription { get; set; }
        [DataMember]
        public virtual string ReceivedUnits { get; set; }
        [DataMember]
        public virtual decimal? UnitPrice { get; set; }
        [DataMember]
        public virtual decimal Tax { get; set; }
        [DataMember]
        public virtual decimal Discount { get; set; }
        [DataMember]
        public virtual decimal? TotalPrice { get; set; }

        [DataMember]
        public virtual string OldPrices { get; set; }
        

    }
}
