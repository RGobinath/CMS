using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities
{
   public class DocumentReport_Vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
        [DataMember]
        public virtual long UploadedfilesId { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string DocumentType { get; set; }
        [DataMember]
        public virtual string DocumentName { get; set; }
        [DataMember]
        public virtual string IdNum { get; set; }
        [DataMember]
        public virtual string IsDocumentAvailable { get; set; }
        [DataMember]
        public virtual string Type { get; set; }
        [DataMember]
        public virtual string EmailId { get; set; }
        [DataMember]
        public virtual string PhoneNo { get; set; }
        [DataMember]
        public virtual string PermanantAddress { get; set; }
        [DataMember]
        public virtual string Designation { get; set; }
        [DataMember]
        public virtual string PFNo { get; set; }
        [DataMember]
        public virtual string BankAccountNumber { get; set; }
        [DataMember]
        public virtual string ESINo { get; set; }
        [DataMember]
        public virtual string DOB { get; set; }
        [DataMember]
        public virtual Int32 Age { get; set; }
    }
}
