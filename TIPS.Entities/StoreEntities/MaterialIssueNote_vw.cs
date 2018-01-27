using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StoreEntities
{
    [DataContract]
    public class MaterialIssueNote_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long IssNoteId { get; set; }
        [DataMember]
        public virtual long MRLId { get; set; }
        [DataMember]
        public virtual int RequestQty { get; set; }
        [DataMember]
        public virtual int ApprovedQty { get; set; }
        [DataMember]
        public virtual int IssueQty { get; set; }
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
        public virtual string Units { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual DateTime? RequiredDate { get; set; }
        [DataMember]
        public virtual string RequestType { get; set; }
    }
}
