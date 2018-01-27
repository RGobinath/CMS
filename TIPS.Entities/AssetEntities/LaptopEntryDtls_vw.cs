using System;
using System.Runtime.Serialization;

namespace TIPS.Entities.AssetEntities
{
    [DataContract]
    public class LaptopEntryDtls_vw
    {
        [DataMember]
        public virtual long AssetDet_Id { get; set; }
        [DataMember]
        public virtual long InvoiceDetailsId { get; set; }
        [DataMember]
        public virtual string AssetCode { get; set; }
        [DataMember]
        public virtual string AssetType { get; set; }
        [DataMember]
        public virtual string Make { get; set; }
        [DataMember]
        public virtual string Model { get; set; }
        [DataMember]
        public virtual string SerialNo { get; set; }
        [DataMember]
        public virtual string LTSize { get; set; }
        [DataMember]
        public virtual string OperatingSystemDtls { get; set; }
        [DataMember]
        public virtual string TransactionType { get; set; }
        [DataMember]
        public virtual string CurrentCampus { get; set; }
        [DataMember]
        public virtual string ReceivedCampus { get; set; }
        [DataMember]
        public virtual string ReceivedGrade { get; set; }
        [DataMember]
        public virtual string NewId { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual long VendorId { get; set; }
        [DataMember]
        public virtual string InvoiceNo { get; set; }
        [DataMember]
        public virtual DateTime? InvoiceDate { get; set; }
        [DataMember]
        public virtual long TotalAsset { get; set; }
        [DataMember]
        public virtual long AssetCount { get; set; }
        [DataMember]
        public virtual decimal Amount { get; set; }
    }
}
