using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.StaffManagementEntities
{
    public class StaffHolidaysMaster
    {
        public virtual long StaffHolidaysMaster_Id { get; set; }
        public virtual string Campus { get; set; }
        public virtual long Year { get; set; }
        public virtual string Month { get; set; }
        public virtual long NoOfHolidays { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual long MonthNumber { get; set; }
        public virtual string Descriptions { get; set; }
        public virtual string HolidayDate { get; set; }
        public virtual string HolidayType { get; set; }
    }
}
