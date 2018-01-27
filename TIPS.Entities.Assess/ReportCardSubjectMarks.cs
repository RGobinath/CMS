
using System;
namespace TIPS.Entities.Assess
{
    public class ReportCardSubjectMarks
    {
        public virtual string SubjectName { get; set; }
        public virtual decimal SubjectEffort { get; set; }
        public virtual string SubjectGEffort { get; set; }
        public virtual decimal SubjectFA { get; set; }
        public virtual string SubjectGFA { get; set; }
        public virtual decimal SubjectSA { get; set; }
        public virtual string SubjectGSA { get; set; }
        public virtual decimal SubjectOverAll { get; set; }
        public virtual string SubjectGOverAll { get; set; }
        public virtual string SubjectTerm { get; set; }
        public virtual Int32 SubjectStarGrade { get; set; }
        public virtual string Semester { get; set; }
        public virtual Int32 subjectstar { get; set; }
    }
}
