using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.QAEntities
{
   public  class QALikes
    {
       public virtual long Id { get; set; }
       public virtual long AnswerId {get;set;}
       public virtual string LikedBy { get; set; }
       public virtual DateTime LikeDate { get; set; }
    }
}
