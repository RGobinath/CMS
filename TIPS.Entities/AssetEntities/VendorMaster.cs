using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.AssetEntities
{
    public class VendorMaster
    {
        [DataMember]
        public virtual long VendorId { get; set; }
        [DataMember]
        public virtual string VendorCode { get; set; }
        [DataMember]
        public virtual string VendorName { get; set; }
        [DataMember]
        public virtual string VendorType { get; set; }
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
