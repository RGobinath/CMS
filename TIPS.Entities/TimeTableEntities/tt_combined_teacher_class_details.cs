using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIPS.Entities.TimeTableEntities
{
    public class tt_combined_teacher_class_details
    {
        public virtual string combined_details_id { get; set; }
        public virtual string combined_master_id { get; set; }
        public virtual string division_id { get; set; }
        public virtual string teacher_id { get; set; }
        public virtual string subject_id { get; set; }
        public virtual string description { get; set; }

    }
}
