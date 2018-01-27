using System;
namespace TIPS.Entities.Assess.ReportCardClasses
{
    public class RptCardInBoxView
    {
        public virtual long Id { get; set; }
        public virtual string IdNo { get; set; }
        public virtual string RequestNo { get; set; }
        public virtual string Name { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Section { get; set; }
        public virtual string Grade { get; set; }
        public virtual long StudentId { get; set; }
        public virtual long PreRegNum { get; set; }
        public virtual long Semester { get; set; }
        public virtual string AcademicYear { get; set; }
        public virtual string TeacherName { get; set; }
        public virtual string RptCardStatus { get; set; }
        public virtual string Second_Language { get; set; }
        public virtual DateTime? RptDate { get; set; }
        public virtual string Initial { get; set; }
    }
}
