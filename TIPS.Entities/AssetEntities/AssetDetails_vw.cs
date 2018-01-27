using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.AssetEntities
{
    public class AssetDetails_vw
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
        public virtual long PreRegNum { get; set; }
        [DataMember]
        public virtual string FromBlock { get; set; }
        [DataMember]
        public virtual string CurrentBlock { get; set; }
        [DataMember]
        public virtual bool IsStandBy { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
    }
}
