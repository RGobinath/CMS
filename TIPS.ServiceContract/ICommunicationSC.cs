using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TIPS.Entities.CommunictionEntities;

namespace TIPS.ServiceContract
{
   public interface ICommunicationSC
    {
       Dictionary<long, IList<ComposeEmailInfo>> GetComposeEmailInfoListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria);
    }
}
