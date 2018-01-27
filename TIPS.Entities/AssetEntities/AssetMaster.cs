using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.AssetEntities
{
    public class AssetMaster
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Asset { get; set; }
        [DataMember]
        public virtual string AssetColor { get; set; }
        [DataMember]
        public virtual string AssetType { get; set; }
    }
}
