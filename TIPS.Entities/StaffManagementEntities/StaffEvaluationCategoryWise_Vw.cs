using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.StaffManagementEntities
{
    public class StaffEvaluationCategoryWise_Vw
    {
        public virtual long Id { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Section { get; set; }
        public virtual long StaffEvaluationCategoryId { get; set; }
        public virtual string CategoryName { get; set; }
        public virtual string StaffName { get; set; }
        public virtual long StaffPreRegNum { get; set; }
        public virtual string AcademicYear { get; set; }
        public virtual string Subject { get; set; }
        //public virtual string Semester { get; set; }
        public virtual string Month { get; set; }
        public virtual int Score { get; set; }
        public virtual int MaxScore { get; set; }
        public virtual int AvgScore { get; set; }
    }
}
