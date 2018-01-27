using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIPS.Entities.TimeTableEntities
{
    public class tt_teacher
    {
        public virtual long teacher_id { get; set; }
        public virtual string teacher_code { get; set; }
        public virtual string teacher_name { get; set; }
        public virtual Int32 max_periods { get; set; }
        public virtual Int32 order_no { get; set; }

        public virtual string gender { get; set; }
        public virtual string address { get; set; }
        public virtual string phoneno { get; set; }
        public virtual string email_id { get; set; }

        public virtual string teacher_colour { get; set; }
        public virtual string description { get; set; }
        public virtual Int32 rem_periods { get; set; }
    }
}
