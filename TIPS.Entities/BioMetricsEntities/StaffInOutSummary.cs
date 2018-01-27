using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TIPS.Entities.BioMetricsEntities
{
    [DataContract]
    public class StaffInOutSummary
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string IdNumber { get; set; }
        [DataMember]
        public virtual string Designation { get; set; }
        [DataMember]
        public virtual long EmployeeID { get; set; }
        [DataMember]
        public virtual string EmployeeName { get; set; }
        [DataMember]
        public virtual string EmployeeIdNumber { get; set; }
        [DataMember]
        public virtual long DeviceId { get; set; }
        [DataMember]
        public virtual string UserId { get; set; }
        [DataMember]
        public virtual DateTime LogDate { get; set; }
        [DataMember]
        public virtual DateTime InTime { get; set; }
        [DataMember]
        public virtual DateTime OutTime { get; set; }

    }
}


