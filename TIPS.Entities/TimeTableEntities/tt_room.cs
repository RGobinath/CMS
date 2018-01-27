using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIPS.Entities.TimeTableEntities
{
    public class tt_room
    {
        public virtual string room_id { get; set; }
        public virtual string room_category_id { get; set; }
        public virtual string room_name { get; set; }
        public virtual string description { get; set; }

    }
}
