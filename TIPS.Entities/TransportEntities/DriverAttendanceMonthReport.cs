using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace TIPS.Entities.TransportEntities
{
    [DataContract]
    public class DriverAttendanceMonthReport
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
        [DataMember]
        public virtual string DriverName { get; set; }
        [DataMember]
        public virtual string DriverIdNo { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual Int32 Leave { get; set; }
        [DataMember]
        public virtual Int32 Absent { get; set; }
        [DataMember]
        public virtual Int32 TotalLandA { get; set; }
        [DataMember]
        public virtual Int32 AbMonth { get; set; }
        [DataMember]
        public virtual Int32 AbYear { get; set; }
        [DataMember]
        public virtual string AbsentDate { get; set; }
        [DataMember]
        public virtual Int32 TotalWorkingDay { get; set; }
        [DataMember]
        public virtual Int32 NoOfPre { get; set; }
    }
}
