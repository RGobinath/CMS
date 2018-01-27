using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.StaffManagementEntities
{
    public class CLDetailsMaster
    {
        public virtual long CLDetails_Id { get; set; }
        public virtual long PreRegNum { get; set; }
        public virtual long Month { get; set; }
        public virtual long Year { get; set; }
        public virtual decimal OpeningBalance { get; set; }
        public virtual long AllotedCL { get; set; }
        public virtual decimal CLInHands { get; set; }
        public virtual decimal? ClosingBalance { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string Status { get; set; }
        public virtual DateTime? DateOfLongLeaveAndResigned { get; set; }
        public virtual string Remark { get; set; }

    }
}
