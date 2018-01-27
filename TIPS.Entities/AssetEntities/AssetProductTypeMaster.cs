using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.AssetEntities
{
    public class AssetProductTypeMaster
    {
        [DataMember]
        public virtual long AssetProductTypeMasterId { get; set; }
        [DataMember]
        public virtual AssetProductMaster AssetProductMaster { get; set; }
        [DataMember]
        public virtual string ProductType { get; set; }
        [DataMember]
        public virtual bool IsActive { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime? ModifiedDate { get; set; }
    }
}
