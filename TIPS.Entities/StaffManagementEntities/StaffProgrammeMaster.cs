using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.StaffManagementEntities
{
    public class StaffProgrammeMaster
    {
        public virtual long StaffProgrammeMatserId { get; set; }
        public virtual string Campus { get; set; }
        public virtual string StaffType { get; set; }
        public virtual string ProgrammeName { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual bool IsActive { get; set; }
    }
}
