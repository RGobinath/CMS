using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.Assess.ReportCardClasses
{
    public class RptCardFocus
    {
        public virtual long RptCardFocusId { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Grade { get; set; }
        public virtual long Semester { get; set; }
        public virtual string AcademicYear { get; set; }
        public virtual string English { get; set; }
        public virtual string Hindi { get; set; }
        public virtual string French { get; set; }
        public virtual string Maths { get; set; }
        public virtual string Physics { get; set; }
        public virtual string Chemistry { get; set; }
        public virtual string Biology { get; set; }
        public virtual string HistoryGeography { get; set; }
        public virtual string ICT { get; set; }
        public virtual string Robotics { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }

        public virtual string spark { get; set; }
    }
}
