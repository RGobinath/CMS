using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.Entities.StaffEntities;
using TIPS.Component;
using TIPS.Entities.StaffManagementEntities;

namespace TIPS.ServiceContract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "StaffIssuesService" in code, svc and config file together.
    public class StaffIssuesService : IStaffIssuesSC
    {
        public long CreateOrUpdateStaffIssues(StaffIssues si)
        {
            try
            {
                StaffIssuesBC sibc = new StaffIssuesBC();
                sibc.CreateOrUpdateStaffManagement(si);
                return si.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<StaffIssues>> GetStaffIssueManagementListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StaffIssuesBC StaffIssuesBC = new StaffIssuesBC();
                return StaffIssuesBC.GetStaffIssueManagementListWithPaging(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public StaffIssues GetStaffIssuesById(long Id)
        {
            try
            {
                StaffIssuesBC sibc = new StaffIssuesBC();
                return sibc.GetStaffIssuesById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #region Added By Prabakran
        public Dictionary<long, IList<StaffIssueManagementReport_vw>> GetStaffIssueManagementReport_vwListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StaffIssuesBC StaffIssuesBC = new StaffIssuesBC();
                return StaffIssuesBC.GetStaffIssueManagementReport_vwListWithPaging(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        #region Added By Gobinath
        public StaffDetails GetStaffDetailsByUserId(string UserId)
        {
            try
            {
                StaffIssuesBC sibc = new StaffIssuesBC();
                return sibc.GetStaffDetailsByUserId(UserId);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
    }
}
