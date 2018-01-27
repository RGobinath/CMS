using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.Entities;

namespace TIPS.ServiceContract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IActivitiesService" in both code and config file together.
    
    public interface IActivitiesServiceSC
    {
        long CreateOrUpdateActivity(Activity Activity);
        Activity GetActivityById(long Id);
        //Dictionary<long, IList<CallManagement>> GetCallManagementListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria,string[] alias);
        Dictionary<long, IList<Activity>> GetActivityListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria);
    }
}
