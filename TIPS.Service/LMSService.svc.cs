using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.Component;
using TIPS.Entities.LMSEntities;

namespace TIPS.ServiceContract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "LMSService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select LMSService.svc or LMSService.svc.cs at the Solution Explorer and start debugging.
    public class LMSService : ILMSService
    {
        public Dictionary<long, IList<LMS_StaffStudentDetails_Vw>> GetStaffandStudentDetailsListwithCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MasterDataBC MasterDataBC = new MasterDataBC();
                return MasterDataBC.GetStaffandStudentDetailsListwithCriteria(page, pageSize, sortBy ,sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
