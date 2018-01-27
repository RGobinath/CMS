using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.WidgetEntities
{
   public class SubWidget : ISubWidget
    {
        public string Topic { get; set; }
        public string TabBuild { get; set; }
        public string Description { get; set; }
    }
}
