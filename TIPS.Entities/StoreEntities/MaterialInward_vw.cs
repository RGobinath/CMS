using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StoreEntities
{
    [DataContract]
    public class MaterialInward_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string InwardNumber { get; set; }
        [DataMember]
        public virtual string ProcessedBy { get; set; }
        [DataMember]
        public virtual string UserRole { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual DateTime CreatedDate { get; set; }
        [DataMember]
        public virtual string PONumber { get; set; }
        [DataMember]
        public virtual string Supplier { get; set; }
        [DataMember]
        public virtual string SuppRefNo { get; set; }
        [DataMember]
        public virtual string ReceivedBy { get; set; }
        [DataMember]
        public virtual DateTime ReceivedDateTime { get; set; }
        [DataMember]
        public virtual string Store { get; set; }
        [DataMember]
        public virtual string InvoiceNumber { get; set; }
        [DataMember]
        public virtual string InvoiceAmount { get; set; }
        [DataMember]
        public virtual DateTime InvoiceDate { get; set; }
        [DataMember]
        public virtual string DCNumber { get; set; }
        [DataMember]
        public virtual DateTime DCDate { get; set; }
        //[DataMember]
        //public virtual DateTime PODate { get; set; }
        [DataMember]
        public virtual long TotalCount { get; set; }
    }
}
