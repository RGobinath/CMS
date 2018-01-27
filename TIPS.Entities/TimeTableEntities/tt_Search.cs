using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIPS.Entities.TimeTableEntities
{
    public class tt_Search
    {
        public virtual long Id { get; set; }
        public virtual long SearchId { get; set; }
        public virtual string SearchCampus { get; set; }
        public virtual string SearchGrade { get; set; }
        public virtual string SearchSection { get; set; }
    
    }
}
