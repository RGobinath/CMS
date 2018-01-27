using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StoreEntities
{
    [DataContract]
    public class MaterialGroupSupplier
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long SupplierId { get; set; }
        [DataMember]
        public virtual string MaterialGroup { get; set; }
    }
}
