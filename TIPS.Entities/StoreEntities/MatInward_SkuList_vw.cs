using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StoreEntities
{
    [DataContract]
    public class MatInward_SkuList_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string InwardNumber { get; set; }
        [DataMember]
        public virtual string Supplier { get; set; }
        [DataMember]
        public virtual string SuppRefNo { get; set; }
        [DataMember]
        public virtual DateTime InvoiceDate { get; set; }
        [DataMember]
        public virtual DateTime ReceivedDateTime { get; set; }
        [DataMember]
        public virtual string ReceivedBy { get; set; }
        [DataMember]
        public virtual DateTime CreatedDate { get; set; }
        [DataMember]
        public virtual string MaterialGroup { get; set; }
        [DataMember]
        public virtual string MaterialSubGroup { get; set; }
        [DataMember]
        public virtual string Material { get; set; }
        [DataMember]
        public virtual int? OrderQty { get; set; }
        [DataMember]
        public virtual string OrderedUnits { get; set; }
        [DataMember]
        public virtual int? ReceivedQty { get; set; }
        [DataMember]
        public virtual string ReceivedUnits { get; set; }
        [DataMember]
        public virtual string DamageDescription { get; set; }

        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Store { get; set; }
        [DataMember]
        public virtual decimal? UnitPrice { get; set; }
        [DataMember]
        public virtual decimal? TotalPrice { get; set; }
    }
}
