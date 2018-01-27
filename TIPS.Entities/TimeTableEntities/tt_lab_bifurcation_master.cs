using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIPS.Entities.TimeTableEntities
{
    public class tt_lab_bifurcation_master
    {
        public virtual string bifurcation_master_id { get; set; }
        public virtual string division_id { get; set; }
        public virtual string continous_periods { get; set; }
        public virtual string no_of_periods { get; set; }

    }
}
