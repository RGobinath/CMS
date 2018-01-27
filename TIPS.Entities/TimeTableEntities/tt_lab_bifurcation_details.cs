using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIPS.Entities.TimeTableEntities
{
    public class tt_lab_bifurcation_details
    {
        public virtual string bifurcation_details_id { get; set; }
        public virtual string bifurcation_master_id { get; set; }
        public virtual string lab_division_id { get; set; }
        public virtual string subject_id { get; set; }
        public virtual string teacher_id { get; set; }
        public virtual string description { get; set; }
    }
}
