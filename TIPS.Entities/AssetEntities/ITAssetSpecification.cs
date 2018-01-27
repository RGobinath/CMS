using System;
using System.Runtime.Serialization;
namespace TIPS.Entities.AssetEntities
{
    [DataContract]
    public class ITAssetSpecification
    {
        [DataMember]
        public virtual long Spec_Id { get; set; }
        [DataMember]
        public virtual string Specification { get; set; }
        [DataMember]
        public virtual string Description { get; set; }
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
