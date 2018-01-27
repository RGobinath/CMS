using System;
using System.Text;
using System.Collections.Generic;


namespace TIPS.Entities.Assess
{

    public class NotUsedAssignmentList
    {
        public virtual int Id { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Subject { get; set; }
        public virtual string AssignmentName { get; set; }
    }
}
