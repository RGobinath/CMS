using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.AssetEntities
{
    public class ITAccessories
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual CampusMaster CampusMaster { get; set; }
        [DataMember]
        public virtual AssetProductMaster AssetProductMaster { get; set; }
        [DataMember]
        public virtual AssetProductTypeMaster AssetProductTypeMaster { get; set; }
        [DataMember]
        public virtual ITAccessoriesBrandMaster ITAccessoriesBrandMaster { get; set; }
        [DataMember]
        public virtual ITAccessoriesModelMaster ITAccessoriesModelMaster { get; set; }
        [DataMember]
        public virtual long Quantity { get; set; }
        [DataMember]
        public virtual decimal Amount { get; set; }
        [DataMember]
        public virtual string Warranty { get; set; }
        [DataMember]
        public virtual AssetInvoiceDetails AssetInvoiceDetails  { get; set; }
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
