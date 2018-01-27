using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StoreEntities
{
    [DataContract]
    public class MaterialIssueNote
    {
        [DataMember]
        public virtual long IssNoteId { get; set; }
        [DataMember]
        public virtual string IssNoteNumber { get; set; }
        [DataMember]
        public virtual DateTime CreatedDate { get; set; }
        [DataMember]
        public virtual long RequestId { get; set; }
        [DataMember]
        public virtual string RequestNumber { get; set; }
        [DataMember]
        public virtual DateTime? RequestedDate { get; set; }
        [DataMember]
        public virtual string ProcessedBy { get; set; }
        [DataMember]
        public virtual string UserRole { get; set; }
        [DataMember]
        public virtual string RequestStatus { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string RequiredForCampus { get; set; }
        [DataMember]
        public virtual string DeliveredThrough { get; set; }
        [DataMember]
        public virtual string DeliveryDetails { get; set; }
        [DataMember]
        public virtual DateTime? DeliveryDate { get; set; }
        [DataMember]
        public virtual DateTime? IssueDate { get; set; }
        [DataMember]
        public virtual string IssuedBy { get; set; }
        [DataMember]
        public virtual string Store { get; set; }
        [DataMember]
        public virtual string RequestorDescription { get; set; }
        [DataMember]
        public virtual string Department { get; set; }
        [DataMember]
        public virtual string RequiredForStore { get; set; }
        [DataMember]
        public virtual string RequiredFromStore { get; set; }
        [DataMember]
        public virtual string DCNumber { get; set; }

        [DataMember]
        public virtual string CreatedUserName { get; set; }
    }
}
