using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.AssetEntities
{
    public class STAssetDetailsTransactionHistory
    {
        [DataMember]
        public virtual long History_Id { get; set; }
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
        public virtual string TransactionType { get; set; }

        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual STAssetDetails STAssetDetails { get; set; }
        [DataMember]
        public virtual long TransactionType_Id { get; set; }        
        [DataMember]
        public virtual long IdNum { get; set; }
        [DataMember]
        public virtual long InvoiceDetailsId { get; set; }
        [DataMember]
        public virtual string Warranty { get; set; }
        [DataMember]
        public virtual decimal Amount { get; set; }
        [DataMember]
        public virtual long AssetRefId { get; set; }
        [DataMember]
        public virtual bool IsSubAsset { get; set; }
        [DataMember]
        public virtual string ReceivedAcademicYr { get; set; }

    }
}
