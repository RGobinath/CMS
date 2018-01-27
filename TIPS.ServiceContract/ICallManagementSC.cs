using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TIPS.Entities;

namespace TIPS.ServiceContract
{
    public interface ICallManagementSC
    {
        long CreateOrUpdateCallManagement(CallManagement CallManagement);
        CallManagement GetCallManagementById(long Id);
        //Dictionary<long, IList<CallManagement>> GetCallManagementListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria,string[] alias);
        Dictionary<long, IList<CallManagement>> GetCallManagementListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria);
       
    }
}
