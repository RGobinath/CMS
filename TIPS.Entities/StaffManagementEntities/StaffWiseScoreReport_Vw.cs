using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.StaffManagementEntities
{
    public class StaffWiseScoreReport_Vw
    {
        public virtual long Id { get; set; }
        public virtual long PreRegNum { get; set; }
        public virtual string Name { get; set; }
        public virtual string IdNumber { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Subject { get; set; }
        public virtual string AcademicYear { get; set; }
        //public virtual string Semester { get; set; }
        public virtual string Month { get; set; }
        //public virtual int Entered { get; set; }
        //public virtual int TotalStudents { get; set; }
        public virtual int A { get; set; }
        public virtual int B { get; set; }
        public virtual int C { get; set; }
        public virtual int D { get; set; }
        public virtual int E { get; set; }
        public virtual int F { get; set; }

    }
}
