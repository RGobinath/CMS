using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.WidgetEntities
{
    public interface IWidget
    {
        long SortOrder { get; set; }
        string ClassName { get; set; }
        string FooterText { get; set; }
        string HeaderText { get; set; }
        string WidgetName { get; set; }
        string DivId { get; set; }
        string CloseId { get; set; }
    }
}
