using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StaffManagementEntities
{
    public class StaffTraining
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual Int32 PreRegNum { get; set; }
        [DataMember]
        public virtual string Particulars { get; set; }
        [DataMember]
        public virtual string Place { get; set; }
        [DataMember]
        public virtual string Date { get; set; }
        [DataMember]
        public virtual string SponsoredBy { get; set; }
    }
}
