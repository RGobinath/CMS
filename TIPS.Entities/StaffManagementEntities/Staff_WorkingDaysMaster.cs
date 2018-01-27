using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.StaffManagementEntities
{
    public class Staff_WorkingDaysMaster
    {
        [DataMember]
        public virtual long Staff_WorkingDaysMaster_Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string StaffType { get; set; }
        [DataMember]
        public virtual long Month { get; set; }
        [DataMember]
        public virtual long Year { get; set; }
        [DataMember]
        public virtual long NoOfworkingDays { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime? ModifiedDate { get; set; }
    }
}
