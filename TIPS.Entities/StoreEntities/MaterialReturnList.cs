using System;
using System.Text;
using System.Collections.Generic;

namespace TIPS.Entities.StoreEntities
{
    public class MaterialReturnList {
        public virtual int Id { get; set; }
        public virtual int? MatRetId { get; set; }
        public virtual string MaterialGroup { get; set; }
        public virtual string MaterialSubGroup { get; set; }
        public virtual string Material { get; set; }
        public virtual string Units { get; set; }
        public virtual int? ReturnQty { get; set; }
        public virtual decimal? TotalPrice { get; set; }
    }
}
