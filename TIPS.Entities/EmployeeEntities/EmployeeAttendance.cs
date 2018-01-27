using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.EmployeeEntities
{
    public class EmployeeAttendance
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string EmployeeName { get; set; }
        [DataMember]
        public virtual string EmployeeIdNo { get; set; }
        [DataMember]
        public virtual DateTime AbsentDate { get; set; }
        [DataMember]
        public virtual string AbsentType { get; set; }
        [DataMember]
        public virtual DateTime? TimeIn { get; set; }
        [DataMember]
        public virtual DateTime? TimeOut { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime CreatedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime ModifiedDate { get; set; }
        [DataMember]
        public virtual string UserRole { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }

        [DataMember]
        public virtual string EntryFrom { get; set; }

   
    }
}
