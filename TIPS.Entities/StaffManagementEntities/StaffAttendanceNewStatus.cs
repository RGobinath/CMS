using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.StaffManagementEntities
{
    public class StaffAttendanceNewStatus
    {
        public virtual long StaffStatus_Id { get; set; }
        public virtual string StaffName { get; set; }
        public virtual string IdNumber { get; set; }
        public virtual long PreRegNum { get; set; }
        public virtual string StaffStatus { get; set; }
        public virtual DateTime? DateOfLongLeaveAndResigned { get; set; }
        public virtual string ToDateOfLongLeaveAndResigned { get; set; }
        public virtual string Remarks { get; set; }
        public virtual DateTime? DateOfCreated { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
    }
}
