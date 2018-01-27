using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace TIPS.Entities.EmployeeEntities
{
    [DataContract]
   public class EmployeeAttendanceReport
    {
        [DataMember]
        public virtual long Id { get; set; }
        
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual Int64 PreRegNum { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string IdNumber { get; set; }
        [DataMember]
        public virtual DateTime AbsentDate { get; set; }
        [DataMember]
        public virtual string AbsentType { get; set; }
        [DataMember]
        public virtual Int32 AbsentCountList { get; set; }
        [DataMember]
        public virtual Int32 LeaveCountList { get; set; }
        [DataMember]
        public virtual Int32 noofpre { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual bool IsActive { get; set; }
        [DataMember]
        public virtual string Designation { get; set; }
        // Attendance Date List Details
        [DataMember]
        public virtual string Date1 { set; get; }
        [DataMember]
        public virtual string Date2 { set; get; }
        [DataMember]
        public virtual string Date3 { set; get; }
        [DataMember]
        public virtual string Date4 { set; get; }
        [DataMember]
        public virtual string Date5 { set; get; }
        [DataMember]
        public virtual string Date6 { set; get; }
        [DataMember]
        public virtual string Date7 { set; get; }
        [DataMember]
        public virtual string Date8 { set; get; }
        [DataMember]
        public virtual string Date9 { set; get; }
        [DataMember]
        public virtual string Date10 { set; get; }
        [DataMember]
        public virtual string Date11 { set; get; }
        [DataMember]
        public virtual string Date12 { set; get; }
        [DataMember]
        public virtual string Date13 { set; get; }
        [DataMember]
        public virtual string Date14 { set; get; }
        [DataMember]
        public virtual string Date15 { set; get; }
        [DataMember]
        public virtual string Date16 { set; get; }
        [DataMember]
        public virtual string Date17 { set; get; }
        [DataMember]
        public virtual string Date18 { set; get; }
        [DataMember]
        public virtual string Date19 { set; get; }
        [DataMember]
        public virtual string Date20 { set; get; }
        [DataMember]
        public virtual string Date21 { set; get; }
        [DataMember]
        public virtual string Date22 { set; get; }
        [DataMember]
        public virtual string Date23 { set; get; }
        [DataMember]
        public virtual string Date24 { set; get; }
        [DataMember]
        public virtual string Date25 { set; get; }
        [DataMember]
        public virtual string Date26 { set; get; }
        [DataMember]
        public virtual string Date27 { set; get; }
        [DataMember]
        public virtual string Date28 { set; get; }
        [DataMember]
        public virtual string Date29 { set; get; }
        [DataMember]
        public virtual string Date30 { set; get; }
        [DataMember]
        public virtual string Date31 { set; get; }
        public virtual int HolidayCountList { get; set; }
        public virtual long numofworkdays { get; set; }
        public virtual Decimal percround { get; set; }
        public virtual Decimal Percentage { get; set; }
        public virtual int totalWorkingday { get; set; }
        public virtual int totalHoliday { get; set; }
        public virtual int totalAttendance { get; set; }
        public virtual double totalPercentage { get; set; }
        /// For Monthly Driver Attendance
        [DataMember]
        public virtual Int32 Leave { get; set; }
        [DataMember]
        public virtual Int32 Absent { get; set; }
        [DataMember]
        public virtual string FromDate { get; set; }
        [DataMember]
        public virtual string ToDate { get; set; }
        [DataMember]
        
        public virtual Int32 TotalLandA { get; set; }
        [DataMember]
        public virtual Int32 AbMonth { get; set; }
        [DataMember]
        public virtual Int32 AbYear { get; set; }
        [DataMember]
        public virtual Int32 TotalWorkingDay { get; set; }
        [DataMember]
        public virtual Int32 NoOfPre { get; set; }
        [DataMember]
        public virtual string WorkingType { get; set; }
    }
}
