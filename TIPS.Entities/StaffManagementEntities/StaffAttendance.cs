using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.StaffManagementEntities
{
    public class StaffAttendance
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
        [DataMember]
        public virtual DateTime? AttendanceDate { get; set; }
        [DataMember]
        public virtual DateTime? LogIn { get; set; }
        [DataMember]
        public virtual DateTime? LogOut { get; set; }
        [DataMember]
        public virtual string LogInIPAddress { get; set; }
        [DataMember]
        public virtual string LogOutIPAddress { get; set; }
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
