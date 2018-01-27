using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.Assess
{
   public class Assess360StudentMarks
    {
       [DataMember]
       public virtual decimal ConsolidatedMarks { get; set; }
       [DataMember]
       public virtual decimal Character { get; set; }
       [DataMember]
       public virtual decimal AttPunc { get; set; }
       [DataMember]
       public virtual decimal HomeComp { get; set; }
       [DataMember]
       public virtual decimal HomeAccu { get; set; }
       [DataMember]
       public virtual decimal WeekTest { get; set; }
       [DataMember]
       public virtual decimal SLC { get; set; }
       [DataMember]
       public virtual decimal TermAssess { get; set; }

    }
}
