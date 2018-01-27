using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.WidgetEntities
{
    public class WidgetUserConfig
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public string UserCode { get; set; }
        public string WidgetMaster { get; set; }
        public string WidgetName{ get; set; }
        public string RoleCode { get; set; }
        public string DivId { get; set; }
        public string CloseId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
