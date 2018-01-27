using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.QAEntities
{
    public class QAProcessLog
    {




        public virtual long Id { get; set; }
        public virtual DateTime ProcessedDate { get; set; }
        public virtual string ProcessedBy { get; set; }
        public virtual string IsProcessed { get; set; }
    }
}
