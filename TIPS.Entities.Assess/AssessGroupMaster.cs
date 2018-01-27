using System.Collections.Generic;
using System.Text;
using System;


namespace TIPS.Entities.Assess
{

    public class AssessGroupMaster
    {
        public virtual long Id { get; set; }
        public virtual string GroupName { get; set; }
        public virtual string GroupCode { get; set; }
        public virtual string Description { get; set; }
        public virtual int Weightage { get; set; }
        public virtual string Grade { get; set; }
    }
}
