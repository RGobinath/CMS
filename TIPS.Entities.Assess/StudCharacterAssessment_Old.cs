using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.Assess
{
    public class StudCharacterAssessment_Old
    {
        public virtual int Id { get; set; }
        public virtual long Assess360Id { get; set; }
        public virtual decimal? Credit { get; set; }
        public virtual decimal? Debit { get; set; }
        public virtual decimal? FinalMark { get; set; }
    }
}
