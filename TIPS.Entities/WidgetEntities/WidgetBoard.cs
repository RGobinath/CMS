using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.WidgetEntities
{
   public class WidgetBoard : IWidget
    {
        public long SortOrder { get; set; }
        public string ClassName { get; set; }
        public string FooterText { get; set; }
        public string HeaderText { get; set; }
        public string WidgetName { get; set; }
        public string DivId { get; set; }
        public string CloseId { get; set; }
    }
}
