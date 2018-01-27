using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.WidgetEntities
{
   public interface ISubWidget
   {
       string Topic { get; set; }
       string TabBuild { get; set; }
       string Description { get; set; }
    }
}
