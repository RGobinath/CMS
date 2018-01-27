using System;
using System.Text;
using System.Collections.Generic;


namespace TIPS.Entities {
    
    public class WeeklyIssueStatusReport_vw {
        public virtual int? Id { get; set; }
        public virtual string BranchCode { get; set; }
        public virtual int? Logged { get; set; }
        public virtual int? Completed { get; set; }
        public virtual int? NonCompleted { get; set; }
    }
}
