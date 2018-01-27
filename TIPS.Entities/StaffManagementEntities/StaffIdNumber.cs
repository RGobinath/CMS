using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StaffManagementEntities
{
    [DataContract]
    public class StaffIdNumber
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual Int32 StaffIdnumber { get; set; }
    }
}
