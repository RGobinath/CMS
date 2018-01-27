using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.Assess
{
    public class StudCopiedhomework
    {
        public virtual long Id { get; set; }
        public virtual long Assess360Id { get; set; }
        public virtual int AssessCompGroup { get; set; }
        public virtual bool IsCredit { get; set; }
        public virtual decimal? Mark { get; set; }
        public virtual decimal? MarksOutOff { get; set; }
    }
}
