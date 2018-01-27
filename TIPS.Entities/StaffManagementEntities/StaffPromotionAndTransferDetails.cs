using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.StaffManagementEntities
{
    public class StaffPromotionAndTransferDetails
    {
        public virtual long Id { get; set; }
        public virtual long StaffID { get; set; }
        public virtual string StaffName { get; set; }
        public virtual string BeforeCampus { get; set; }
        public virtual string AfterCampus { get; set; }
        public virtual string BeforeDesignation { get; set; }
        public virtual string AfterDesignation { get; set; }
        public virtual string BeforeDepartment { get; set; }
        public virtual string AfterDepartment { get; set; }        
        public virtual string CreatedBy { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
    }
}
