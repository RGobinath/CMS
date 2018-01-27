using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.QAEntities
{
   public  class QADashboard
    {
       public virtual long Id { get; set; }
       public virtual string Grade { get; set; }
       public virtual long NoOfQuestions { get; set; }
       public virtual string Campus { get; set; }
       public virtual string UserId { get; set; }
       public virtual long InboxCount { get; set; }
       public virtual long CampusCount { get; set; }
       public virtual long AllCampusCount { get; set; }
       public virtual long Success { get; set; }
       public virtual long Failiure { get; set; }
    }
}
