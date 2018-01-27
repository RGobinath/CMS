using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace TIPS.Entities.AssetEntities
{
    [DataContract]
    public class ITAssetServiceDetails
    {
        [DataMember]
        public virtual long AssetService_Id { get; set; }
        [DataMember]
        public virtual string DCNo { get; set; }
        [DataMember]
        public virtual DateTime? DCDate { get; set; }
        [DataMember]
        public virtual string Problem { get; set; }
        [DataMember]
        public virtual string PhysicalCondition { get; set; }
        [DataMember]
        public virtual string EngineerName { get; set; }
        [DataMember]
        public virtual long PendingAge { get; set; }
        [DataMember]
        public virtual string Vendor { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime? ModifiedDate { get; set; }
        [DataMember]
        public virtual AssetDetails AssetDetails { get; set; }
        [DataMember]
        public virtual string FromCampus { get; set; }
        [DataMember]
        public virtual string FromBlock { get; set; }
        [DataMember]
        public virtual string FromLocation { get; set; }
        [DataMember]
        public virtual DateTime? ExpectedDate { get; set; }
        [DataMember]
        public virtual DateTime? InwardDate { get; set; }
        [DataMember]
        public virtual long InvoiceDetailsId { get; set; }
        [DataMember]
        public virtual string Warranty { get; set; }
        [DataMember]
        public virtual decimal Amount { get; set; }
        [DataMember]
        public virtual bool IsExpired { get; set; }
        [DataMember]
        public virtual Int64 AssetRefId { get; set; }
        [DataMember]
        public virtual bool IsSubAsset { get; set; }
    }
}
