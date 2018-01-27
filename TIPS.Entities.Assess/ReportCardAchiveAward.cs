using System;
namespace TIPS.Entities.Assess
{
    public class ReportCardAchiveAward
    {
        public virtual string Achievement { get; set; }
        public virtual string AchievementDescription { get; set; }
        public virtual string Award { get; set; }
        public virtual string AwardDescription { get; set; }
        public virtual string Semester { get; set; }
    }
}
