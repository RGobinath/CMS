using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.AssetEntities
{
    [DataContract]
    public class AssetDetailsTemplate
    {
        [DataMember]
        public virtual long Asset_Id { get; set; }
        [DataMember]
        public virtual string AssetCode { get; set; }
        [DataMember]
        public virtual string Location { get; set; }
        [DataMember]
        public virtual string SerialNumber { get; set; }
        [DataMember]
        public virtual string Specifications { get; set; }
        //[DataMember]
        //public virtual string Descriptions { get; set; }
        [DataMember]
        public virtual string SpecificationsDetails { get; set; }
        [DataMember]
        public virtual string AssetType { get; set; }
        ////added by Thamizhmani
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime? ModifiedDate { get; set; }
        [DataMember]
        public virtual bool IsSubAsset { get; set; }

        //public virtual CampusMaster CampusMaster { get; set; }
        public virtual List<string> specList { get; set; }
        

    }
}
