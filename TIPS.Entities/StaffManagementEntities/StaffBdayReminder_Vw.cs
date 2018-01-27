using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StaffManagementEntities
{
     [DataContract]
    public class StaffBdayReminder_Vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string IdNumber { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Designation { get; set; }
        [DataMember]
        public virtual string EmailId { get; set; }
        [DataMember]
        public virtual string PhoneNo { get; set; }
        [DataMember]
        public virtual DateTime DofB { get; set; }
        [DataMember]
        public virtual long  BirthDay { get; set; }
        [DataMember]
        public virtual long BirthMonth { get; set; }
        [DataMember]
        public virtual long BirthYear { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
     }
}
