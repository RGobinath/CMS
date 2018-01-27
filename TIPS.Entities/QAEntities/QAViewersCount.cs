using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.QAEntities
{
    public class QAViewersCount
    {
        public virtual long Id { get; set; }
      
        public virtual long QuestionId { get; set; }
        public virtual long ViewCount { get; set; }
    }
}
