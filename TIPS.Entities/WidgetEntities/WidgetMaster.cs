using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.WidgetEntities
{
    public class WidgetMaster
    {
        public long Id { get; set; }

        public string WidgetModule { get; set; }

        public string RoleCode { get; set; }
        public string Description { get; set; }

        public long SordOrder { get; set; }
        public string ClassName { get; set; }
        public string WidgetName { get; set; }
        public string DivId { get; set; }
        public string CloseId { get; set; }
        public string FooterText { get; set; }

        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
