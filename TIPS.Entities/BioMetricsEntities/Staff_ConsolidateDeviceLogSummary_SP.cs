using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.BioMetricsEntities
{
    public class Staff_ConsolidateDeviceLogSummary_SP
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
        [DataMember]
        public virtual string IdNumber { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string StaffCategoryForAttendane { get; set; }
        [DataMember]
        public virtual string Department { get; set; }
        [DataMember]
        public virtual string Designation { get; set; }
        [DataMember]
        public virtual string Programme { get; set; }
        [DataMember]
        public virtual long TotalDays { get; set; }
        [DataMember]
        public virtual decimal TotalWorkedDays { get; set; }
        [DataMember]
        public virtual decimal NoOfLeaveTaken { get; set; }
        [DataMember]
        public virtual long TotalWorkedHours { get; set; }
        [DataMember]
        public virtual long TotalWorkedMinutes { get; set; }
        [DataMember]
        public virtual long TotalWorkedSeconds { get; set; }
        [DataMember]
        public virtual string TotalWorkedHoursMinutesAndseconds { get; set; }
        //[DataMember]
        //public virtual DateTime? TotalWorkedHoursInDateTimeFormat { get; set; }
        [DataMember]
        public virtual decimal OpeningBalance { get; set; }
        [DataMember]
        public virtual decimal ClosingBalance { get; set; }
        //[DataMember]
        //public virtual long AllotedCL { get; set; }
        [DataMember]
        public virtual decimal TotalAvailableBalance { get; set; }
        [DataMember]
        public virtual decimal LeaveTobeCalculated { get; set; }
        [DataMember]
        public virtual string CurrentStatus { get; set; }
        [DataMember]
        public virtual DateTime? DateOfLongLeaveAndResigned { get; set; }
    }
}
