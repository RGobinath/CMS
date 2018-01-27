using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.Assess
{
    public class ReportCardFromAssess360VItoVIII
    {
        public virtual long StudentId { get; set; }
        public virtual string IdNo { get; set; }
        public virtual string Name { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Section { get; set; }
        public virtual string Grade { get; set; }
        public virtual string AcademicYear { get; set; }
        public virtual string Term { get; set; }
        public virtual long AssessId { get; set; }
        public virtual string Assess360Marks { get; set; }
        public virtual decimal SemIOverAll { get; set; }
        public virtual decimal SemIIOverAll { get; set; }
        public virtual decimal SemIIIOverAll { get; set; }
        /// <summary>
        /// For English As Second Language
        /// </summary>
        public IList<ReportCardSubjectMarks> RptMarks { get; set; }
        public IList<ReportCardAchiveAward> RptAchiveAward { get; set; }
    }
}
