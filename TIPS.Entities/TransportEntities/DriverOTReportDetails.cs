using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.TransportEntities
{
    public class DriverOTReportDetails
    {
        [DataMember]
        public virtual long Id { get; set; }

        [DataMember]
        public virtual string Campus { get; set; }
        
        [DataMember]
        public virtual string DriverIdNo { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        
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
        [DataMember]
        public virtual string OTType { set; get; }
        [DataMember]
        public virtual long OTCount { set; get; }
        [DataMember]
        public virtual string TotalAllowance { set; get; }
        [DataMember]
        public virtual string Evening { set; get; }
        [DataMember]
        public virtual string RemidialTrip { set; get; }
        [DataMember]
        public virtual string HolidayTrip { set; get; }
        [DataMember]
        public virtual string NightTrip { set; get; }
        
       
    }
}
