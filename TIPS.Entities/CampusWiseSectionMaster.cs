using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities
{
    public class CampusWiseSectionMaster
    {
        public virtual long CampusWiseSectionMasterId { get; set; }
        public virtual string Section { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual string UpdateBy { get; set; }
        public virtual DateTime UpdateDate { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual AcademicyrMaster AcademicyrMaster { get; set; }
        public virtual CampusMaster CampusMaster { get; set; }
        public virtual CampusGradeMaster CampusGradeMaster { get; set; }

    }
}
