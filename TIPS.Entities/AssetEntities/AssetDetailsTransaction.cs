using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.AssetEntities
{
    [DataContract]
    public class AssetDetailsTransaction
    {
        [DataMember]
        public virtual long AssetTrans_Id { get; set; }
        [DataMember]
        public virtual long AssetDet_Id { get; set; }
        [DataMember]
        public virtual string AssetCode { get; set; }
        [DataMember]
        public virtual string FromCampus { get; set; }
        [DataMember]
        public virtual string FromBlock { get; set; }
        [DataMember]
        public virtual string FromLocation { get; set; }
        
        [DataMember]
        public virtual string ToCampus { get; set; }
        [DataMember]
        public virtual string ToBlock { get; set; }
        [DataMember]
        public virtual string ToLocation { get; set; }
        [DataMember]
        public virtual string Description { get; set; }
        [DataMember]
        public virtual string EngineerName { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime? ModifiedDate { get; set; }
        [DataMember]
        public virtual DateTime? InstalledOn { get; set; }
        [DataMember]
        public virtual Int64 AssetRefId { get; set; }
        [DataMember]
        public virtual bool IsSubAsset { get; set; }
    }
}
