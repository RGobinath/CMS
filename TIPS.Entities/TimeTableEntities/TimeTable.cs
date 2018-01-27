using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.TimeTableEntities
{
    public class TimeTable
    {
        public virtual long Id { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Section { get; set; }
        public virtual string Day { get; set; }
        public virtual string Period1 { get; set; }
        public virtual string Period1Caption { get; set; }
        public virtual string Period1Icon { get; set; }

        public virtual string Period2 { get; set; }
        public virtual string Period2Caption { get; set; }
        public virtual string Period2Icon { get; set; }

        public virtual string Period3 { get; set; }
        public virtual string Period3Caption { get; set; }
        public virtual string Period3Icon { get; set; }

        public virtual string Period4 { get; set; }
        public virtual string Period4Caption { get; set; }
        public virtual string Period4Icon { get; set; }

        public virtual string Period5 { get; set; }
        public virtual string Period5Caption { get; set; }
        public virtual string Period5Icon { get; set; }

        public virtual string Period6 { get; set; }
        public virtual string Period6Caption { get; set; }
        public virtual string Period6Icon { get; set; }

        public virtual string Period7 { get; set; }
        public virtual string Period7Caption { get; set; }
        public virtual string Period7Icon { get; set; }

        public virtual string Period8 { get; set; }
        public virtual string Period8Caption { get; set; }
        public virtual string Period8Icon { get; set; }

        public virtual string Period9 { get; set; }
        public virtual string Period9Caption { get; set; }
        public virtual string Period9Icon { get; set; }

        public virtual string Period10 { get; set; }
        public virtual string Period10Caption { get; set; }
        public virtual string Period10Icon { get; set; }
    }
}
