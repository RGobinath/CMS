using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.AssetEntities
{
    public class AssetInvoiceDetails_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long AssetDet_Id { get; set; }
        [DataMember]
        public virtual long InvoiceDetailsId { get; set; }
        [DataMember]
        public virtual string InvoiceNo { get; set; }
        [DataMember]
        public virtual string DocumentType { get; set; }
        [DataMember]
        public virtual string DocumentName { get; set; }
        [DataMember]
        public virtual string DocumentSize { get; set; }
        [DataMember]
        public virtual byte[] DocumentData { get; set; }
        [DataMember]
        public virtual DateTime UploadedDate { get; set; }
        [DataMember]
        public virtual DateTime InvoiceDate { get; set; }
        [DataMember]
        public virtual string Warranty { get; set; }
        [DataMember]
        public virtual long TotalAsset { get; set; }
        [DataMember]
        public virtual VendorMaster VendorMaster { get; set; }
        [DataMember]
        public virtual decimal Amount { get; set; }
        [DataMember]
        public virtual bool IsExpired { get; set; }
    }
}
