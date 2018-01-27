using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StoreEntities
{
    [DataContract]
    public class MatReq_ReqList_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string RequestNumber { get; set; }
        [DataMember]
        public virtual string RequiredForCampus { get; set; }
        [DataMember]
        public virtual string ProcessedBy { get; set; }
        [DataMember]
        public virtual DateTime? RequestedDate { get; set; }
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
        public virtual DateTime? RequiredDate { get; set; }
        [DataMember]
        public virtual int? Quantity { get; set; }
        [DataMember]
        public virtual int? ApprovedQty { get; set; }
        [DataMember]
        public virtual int? IssuedQty { get; set; }
        [DataMember]
        public virtual string Status { get; set; }


    }
}
