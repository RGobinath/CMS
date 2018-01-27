using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.StoreEntities
{
    public class SkuList_vw
    {
        public virtual long Id { get; set; }
        public virtual long SkuId { get; set; }
        public virtual long MaterialRefId { get; set; }
        public virtual string MaterialGroup { get; set; }
        public virtual string MaterialSubGroup { get; set; }
        public virtual string Material { get; set; }
        public virtual string OrderedUnits { get; set; }
        public virtual int OrderQty { get; set; }
        public virtual int ReceivedQty { get; set; }
        public virtual int DamagedQty { get; set; }
        public virtual int DamagelessQty { get; set; }
        public virtual int IssuedQty { get; set; }
        public virtual int StockAvailableQty { get; set; }
        public virtual string IssuedStatus { get; set; }
        public virtual string DamageDescription { get; set; }
        public virtual string ReceivedUnits { get; set; }
        public virtual decimal UnitPrice { get; set; }
        public virtual decimal Tax { get; set; }
        public virtual decimal Discount { get; set; }
        public virtual decimal TotalPrice { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Status { get; set; }
    }
}
