using System;
using System.Text;
using System.Collections.Generic;


namespace TIPS.Entities.TransportEntities {
    
    public class DriverOTAllowance_vw {
        public virtual int? Id { get; set; }
        public virtual string Campus { get; set; }
        public virtual string DriverName { get; set; }
        public virtual string DriverIdNo { get; set; }
        public virtual int Month { get; set; }
        public virtual int Year { get; set; }
        public virtual int Evening { get; set; }
        public virtual decimal? EveningAllowance { get; set; }
        public virtual int Night { get; set; }
        public virtual decimal? NightAllowance { get; set; }
        public virtual int OutStation { get; set; }
        public virtual decimal? OutStationAllowance { get; set; }
        public virtual int Holiday { get; set; }
        public virtual decimal? HolidayAllowance { get; set; }
        public virtual int Remedial { get; set; }
        public virtual decimal? RemedialAllowance { get; set; }
        public virtual int TotalOTCount { get; set; }
        public virtual decimal? TotalAllowance { get; set; }
    }
}
