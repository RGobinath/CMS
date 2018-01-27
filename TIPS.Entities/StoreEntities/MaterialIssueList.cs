using System;
using System.Runtime.Serialization;

namespace TIPS.Entities.StoreEntities
{
    [DataContract]
    public class MaterialIssueList
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long MRLId { get; set; }
        [DataMember]
        public virtual int RequestQty { get; set; }
        [DataMember]
        public virtual int ApprovedQty { get; set; }
        [DataMember]
        public virtual int PrevIssdQty { get; set; }
        [DataMember]
        public virtual int IssueQty { get; set; }
        [DataMember]
        public virtual int TotalIssued { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual long IssNoteId { get; set; }

        [DataMember]
        public virtual decimal UnitPrice { get; set; }
        [DataMember]
        public virtual decimal TotalPrice { get; set; }

        //[DataMember]
        //public virtual int MaterialPriceMasterListId { get; set; }

        [DataMember]
        public virtual string InwardIds { get; set; }

        [DataMember]
        public virtual string Material { get; set; }
        
    }
}
