using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities
{
    public class CampusEmailId
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Server { get; set; }
        [DataMember]
        public virtual string EmailId { get; set; }
        [DataMember]
        public virtual string Password { get; set; }
        [DataMember]
        public virtual string FBLink { get; set; }
        [DataMember]
        public virtual string AlternateEmailId { get; set; }
        [DataMember]
        public virtual string AlternateEmailIdPassword { get; set; }
        [DataMember]
        public virtual string PhoneNumber { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual DateTime? ModifiedDate { get; set; }
        [DataMember]
        public virtual string Address { get; set; }
        [DataMember]
        public virtual string WebSiteName { get; set; }
        [DataMember]
        public virtual string SchoolName { get; set; }
        [DataMember]
        public virtual string PinCode { get; set; }     
        [DataMember]
        public virtual string SenderID { get; set; }

        [DataMember]
        public virtual string BulkEmailId { get; set; }
        [DataMember]
        public virtual string BulkEmailIdPassword { get; set; }
        [DataMember]
        public virtual string BulkEmailIdHost { get; set; }
        [DataMember]
        public virtual Int32 BulkEmailIdPort { get; set; }
        [DataMember]
        public virtual string AlternateBulkEmailId { get; set; }
        [DataMember]
        public virtual string AlternateBulkEmailIdPassword { get; set; }
        [DataMember]
        public virtual string AlternateBulkEmailIdHost { get; set; }
        [DataMember]
        public virtual Int32 AlternateBulkEmailIdPort { get; set; }
        
    }
}
