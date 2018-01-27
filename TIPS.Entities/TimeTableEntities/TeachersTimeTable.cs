using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIPS.Entities.TimeTableEntities
{
    public class TeachersTimeTable
    {
        public virtual long DayId { get; set; }
        public virtual string DayName { get; set; }
        public virtual string First_Period { get; set; }
        public virtual string Secnd_Period { get; set; }
        public virtual string Third_Period { get; set; }
        public virtual string Fourth_Period { get; set; }
        public virtual string Fifth_Period { get; set; }
        public virtual string Sixth_Period { get; set; }
        public virtual string Sevent_Period { get; set; }

        public virtual string First_Prd_color { get; set; }
        public virtual string Secnd_Prd_color { get; set; }
        public virtual string Third_Prd_color { get; set; }
        public virtual string Fourth_Prd_color { get; set; }
        public virtual string Fifth_Prd_color { get; set; }
        public virtual string Sixth_Prd_color { get; set; }
        public virtual string Sevent_Prd_color { get; set; }
    }
}
