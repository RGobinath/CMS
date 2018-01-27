using System;
using System.Runtime.Serialization;

namespace TIPS.Entities.StoreEntities
{
    [DataContract]
    public class MaterialIssueList_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long IssNoteId { get; set; }
        [DataMember]
        public virtual string IssNoteNumber { get; set; }
        [DataMember]
        public virtual long MRLId { get; set; }
        [DataMember]
        public virtual int IssueQty { get; set; }
        [DataMember]
        public virtual int TotalIssued { get; set; }
        [DataMember]
        public virtual DateTime IssueDate { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual string IssuedBy { get; set; }
    }
}
