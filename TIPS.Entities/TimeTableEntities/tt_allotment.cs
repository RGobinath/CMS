using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIPS.Entities.TimeTableEntities
{
    public class tt_allotment
    {
        public virtual long allotment_id { get; set; }
        public virtual long division_id { get; set; }
        public virtual long subject_id { get; set; }
        public virtual long teacher_id { get; set; }
        public virtual Int32 no_of_periods { get; set; }
        public virtual Int32 remaining_periods { get; set; }
        public virtual bool charge { get; set; }
        public virtual string combined_details_id { get; set; }
        public virtual string lab_division_id { get; set; }
        public virtual string bifurcation_details_id { get; set; }
        public virtual Int32 continous_periods { get; set; }
        public virtual string description { get; set; }
    }
}
