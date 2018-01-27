using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StoreEntities
{
    [DataContract]
    public class MaterialRequest
    {
        [DataMember]
        public virtual long Id { get; set; }
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
        public virtual TimeSpan? DifferenceInHours { get; set; }
        [DataMember]
        public virtual int Hours { get; set; }
        [DataMember]
        public virtual long InstanceId { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string RequestorDescription { get; set; }
        [DataMember]
        public virtual string RequiredForCampus { get; set; }
        [DataMember]
        public virtual string ApproverComments { get; set; }
        [DataMember]
        public virtual string Department { get; set; }
        [DataMember]
        public virtual string RequiredForStore { get; set; }
        [DataMember]
        public virtual string RequiredFromStore { get; set; }
        [DataMember]
        public virtual string CreatedUserName { get; set; }
        [DataMember]
        public virtual string UserName { get; set; }

    }
}
