using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.AssetEntities
{
    [DataContract]
    public class AssetInvoiceDetails
    {
        [DataMember]
        public virtual long InvoiceDetailsId { get; set; }
        //[DataMember]
        //public virtual long VendorId         {get;set;}
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
        public virtual DateTime? InvoiceDate { get; set; }
        //[DataMember]
        //public virtual string Warranty { get; set; }
        [DataMember]
        public virtual long TotalAsset { get; set; }
        [DataMember]
        public virtual long AssetCount { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime? ModifiedDate { get; set; }
        [DataMember]
        public virtual bool IsActive { get; set; }
        public virtual VendorMaster VendorMaster { get; set; }
        [DataMember]
        public virtual  decimal Amount { get; set; }
    }
}
