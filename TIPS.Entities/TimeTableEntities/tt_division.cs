using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIPS.Entities.TimeTableEntities
{
    public class tt_division
    {
        public virtual long division_id { get; set; }
        public virtual long room_id { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Section { get; set; }
        public virtual Int32 order_no { get; set; }
        public virtual string description { get; set; }
        public virtual string division_color { get; set; }

    }
}
