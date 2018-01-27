using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIPS.Entities.TimeTableEntities
{
    public class tt_days
    {
        public virtual long day_id { get; set; }
        public virtual string day_name { get; set; }
        public virtual string day_code { get; set; }
        public virtual DateTime day_date { get; set; }

    }
}
