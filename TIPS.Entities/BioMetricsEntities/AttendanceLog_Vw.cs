using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TIPS.Entities.BioMetricsEntities
{
    [DataContract]
    public class AttendanceLog_Vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string EmployeeName { get; set; }
        [DataMember]
        public virtual long EmployeeCode { get; set; }
        [DataMember]
        public virtual string Gender { get; set; }
        [DataMember]
        public virtual string CompanyFName { get; set; }
        [DataMember]
        public virtual long CompanyId { get; set; }
        [DataMember]
        public virtual string DepartmentFName { get; set; }
        [DataMember]
        public virtual long DepartmentId { get; set; }
        [DataMember]
        public virtual string Designation { get; set; }
        [DataMember]
        public virtual string CategoryName { get; set; }
        [DataMember]
        public virtual long CategoryId { get; set; }
        [DataMember]
        public virtual string EmployementType { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual string ShiftGroupName { get; set; }
        [DataMember]
        public virtual long ShiftGroupId { get; set; }
        [DataMember]
        public virtual string ShiftType { get; set; }
        [DataMember]
        public virtual string MultiShiftGroupFName { get; set; }
        [DataMember]
        public virtual long MultiShiftGroupId { get; set; }
        [DataMember]
        public virtual DateTime AttendanceDate { get; set; }
        [DataMember]
        public virtual string AttendanceStatus { get; set; }
        [DataMember]
        public virtual string InTime { get; set; }
        [DataMember]
        public virtual string OutTime { get; set; }
    }
}
