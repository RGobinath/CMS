using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using TIPS.Entities.AdmissionEntities;
using TIPS.Entities.StaffManagementEntities;

namespace TIPS.Entities.AssetEntities
{
    [DataContract]
    public class SubAssetDetails_vw
    {
        [DataMember]
        public virtual long AssetDet_Id { get; set; }
        [DataMember]
        public virtual string AssetCode { get; set; }
        [DataMember]
        public virtual string AssetType { get; set; }
        [DataMember]
        public virtual string Model { get; set; }
        [DataMember]
        public virtual string Make { get; set; }
        [DataMember]
        public virtual string Location { get; set; }
        [DataMember]
        public virtual string SerialNo { get; set; }
        [DataMember]
        public virtual string SpecificationsDetails { get; set; }
        [DataMember]
        public virtual long Asset_Id { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual CampusMaster CampusMaster { get; set; }
        public virtual List<string> specList { get; set; }

        public virtual AssetDetailsTransaction AssetDetailsTransaction { get; set; }
        public virtual ITAssetServiceDetails ITAssetServiceDetails { get; set; }
        public virtual ITAssetScrapDetails ITAssetScrapDetails { get; set; }

        [DataMember]
        public virtual string CurrentCampus { get; set; }
        [DataMember]
        public virtual string CurrentLocation { get; set; }
        [DataMember]
        public virtual bool IsActive { get; set; }
        [DataMember]
        public virtual string TransactionType { get; set; }
        [DataMember]
        public virtual DateTime? InstalledOn { get; set; }
        [DataMember]
        public virtual string EngineerName { get; set; }
        [DataMember]
        public virtual string FromCampus { get; set; }
        [DataMember]
        public virtual string UserType { get; set; }
        [DataMember]
        public virtual long IdNum { get; set; }
        [DataMember]
        public virtual long InvoiceDetailsId { get; set; }
        public virtual StudentTemplateView StudentTemplateView { get; set; }
        public virtual StaffDetailsView StaffDetailsView { get; set; }
        [DataMember]
        public virtual string FromBlock { get; set; }
        [DataMember]
        public virtual string CurrentBlock { get; set; }
        [DataMember]
        public virtual bool IsStandBy { get; set; }
        public virtual ITAssetDetailsTransactionHistory AssetHistory { get; set; }
        [DataMember]
        public virtual TimeSpan? DifferenceInDays { get; set; }
        [DataMember]
        public virtual string Warranty { get; set; }
        [DataMember]
        public virtual decimal Amount { get; set; }
        [DataMember]
        public virtual bool IsExpired { get; set; }
        //for Sub Asset
        //public virtual AssetDetailsTemplate AssetDetailsTemplate { get; set; }
        [DataMember]
        public virtual bool IsSubAssetRequired { get; set; }
        [DataMember]
        public virtual bool IsSubAsset { get; set; }
        [DataMember]
        public virtual string SubAssetType { get; set; }
        [DataMember]
        public virtual long AssetRefId { get; set; }
    }
}
