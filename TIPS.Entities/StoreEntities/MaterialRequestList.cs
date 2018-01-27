using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StoreEntities
{
    [DataContract]
    public class MaterialRequestList
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long MatReqRefId { get; set; }
        [DataMember]
        public virtual string RequiredFor { get; set; }
        [DataMember]
        public virtual string RequiredForGrade { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string Material { get; set; }
        [DataMember]
        public virtual string MaterialGroup { get; set; }
        [DataMember]
        public virtual string MaterialSubGroup { get; set; }
        [DataMember]
        public virtual int? Quantity { get; set; }
        [DataMember]
        public virtual DateTime? RequiredDate { get; set; }
        [DataMember]
        public virtual string RequestType { get; set; }
        [DataMember]
        public virtual string Units { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual int? ApprovedQty { get; set; }
        [DataMember]
        public virtual int? IssuedQty { get; set; }
        [DataMember]
        public virtual int ClosingBalance { get; set; }
        [DataMember]
        public virtual decimal UnitPrice{ get; set; }
        [DataMember]
        public virtual int MaterialPriceMasterListId { get; set; }
        [DataMember]
        public virtual long InwardId { get; set; }

        [DataMember]
        public virtual string InwardIds { get; set; }
        [DataMember]
        public virtual string AvailableQtys { get; set; }
        [DataMember]
        public virtual string UnitPrices { get; set; }
        [DataMember]
        public virtual string Taxes { get; set; }
        [DataMember]
        public virtual string Discounts { get; set; }
    }
}
