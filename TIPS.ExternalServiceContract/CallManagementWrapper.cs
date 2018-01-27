using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TIPS.Entities;

namespace TIPS.ExternalServiceContract
{
    public class CallManagementWrapper
    {
        public long count { get; set; }
        public IList<CallManagementView> list { get; set; }
    }
}
