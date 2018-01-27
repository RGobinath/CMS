using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StoreEntities
{
    [DataContract]
    public class MatIssNote_RequestList_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string IssNoteNumber { get; set; }
        [DataMember]
        public virtual long RequestId { get; set; }
        [DataMember]
        public virtual string IssuedBy { get; set; }
        [DataMember]
        public virtual DateTime? IssueDate { get; set; }
        [DataMember]
        public virtual string RequiredForCampus { get; set; }
        [DataMember]
        public virtual string DeliveredThrough { get; set; }
        [DataMember]
        public virtual string DeliveryDetails { get; set; }
        [DataMember]
        public virtual string RequestType { get; set; }
        [DataMember]
        public virtual string RequiredForGrade { get; set; }
        [DataMember]
        public virtual string RequiredFor { get; set; }
        [DataMember]
        public virtual string MaterialGroup { get; set; }
        [DataMember]
        public virtual string MaterialSubGroup { get; set; }
        [DataMember]
        public virtual string Material { get; set; }
        [DataMember]
        public virtual string Units { get; set; }
        [DataMember]
        public virtual int? IssueQty { get; set; }

        [DataMember]
        public virtual string ProcessedBy { get; set; }
        [DataMember]
        public virtual string RequiredForStore{ get; set; }

        [DataMember]
        public virtual decimal TotalPrice { get; set; }

       
    }
}
