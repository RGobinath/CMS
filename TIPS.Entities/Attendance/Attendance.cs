using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.Attendance
{
    [DataContract]
   public class Attendance
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        //[DataMember]
        //public virtual long AttId { get; set; }
        [DataMember]
        public virtual DateTime? AbsentDate { get; set; }
        [DataMember]
        public virtual bool IsAbsent { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual string UserRole { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }

       [DataMember]
        public virtual string EntryFrom { get; set; }

       [DataMember]
       public virtual string AttendanceType { get; set; }
       [DataMember]
       public virtual string AbsentCategory { get; set; }
    }
}
