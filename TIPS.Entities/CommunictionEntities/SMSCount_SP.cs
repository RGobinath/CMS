using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.CommunictionEntities
{
    public class SMSCount_SP
    {
        public virtual long Id { get; set; }
        public virtual string Campus { get; set; }
        public virtual long Sent { get; set; }
        public virtual long Failed { get; set; }
        public virtual long NotDelivered { get; set; }
        public virtual long NotValid { get; set; }
        public virtual long DNDApplied { get; set; }
        public virtual long Total { get; set; }        
    }
}
