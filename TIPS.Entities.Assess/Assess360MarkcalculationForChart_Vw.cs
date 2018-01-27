using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.Assess
{
    public class Assess360MarkcalculationForChart_Vw
    {
        public virtual long Id { get; set; }
        public virtual string RequestNo { get; set; }
        public virtual long StudentId { get; set; }
        public virtual string IdNo { get; set; }
        public virtual string Name { get; set; }
        public virtual string Initial { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Section { get; set; }
        public virtual string AcademicYear { get; set; }
        public virtual long Assess360Id { get; set; }
        public virtual string GroupName { get; set; }
        public virtual DateTime IncidentDate { get; set; }
        public virtual decimal Mark { get; set; }
        public virtual string Subject { get; set; }
        public virtual string AssignmentName { get; set; }
        public virtual decimal MarksOutOff { get; set; }
        public virtual string Semester { get; set; }
        public virtual int MonthRslt { get; set; }
        public virtual decimal CalculatedMarks { get; set; }
    }
}
