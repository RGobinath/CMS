using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.Assess
{
    public class StudHWAccuracy_Old
    {
        public virtual Int32 Id { get; set; }
        public virtual string Subject { get; set; }
        public virtual decimal Mark { get; set; }
        public virtual Int32 Assess360Id { get; set; }
    }
}
