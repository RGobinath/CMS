using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersistenceFactory;
using TIPS.Entities;
using System.Data;

namespace TIPS.Component
{
  public class IssueCountBC
    {
       PersistenceServiceFactory PSF = null;
       public IssueCountBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }
       
    }
}
