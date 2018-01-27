using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TIPS.Entities.TransportEntities;

namespace TIPS.ServiceContract
{
   public interface ITransportServiceSc
    {
       Dictionary<long, IList<VehicleTypeMaster>> GetVehicleTypeMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);
    }
}
