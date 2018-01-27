using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TIPS.Entities.StaffEntities;
using PersistenceFactory;
using TIPS.Entities.StaffManagementEntities;

namespace TIPS.Component
{
   public class StaffIssuesBC
    {
       PersistenceServiceFactory PSF = null;
       public StaffIssuesBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }
       public long CreateOrUpdateStaffManagement(StaffIssues si)
       {
           try
           {
               if (si != null)
                   PSF.SaveOrUpdate<StaffIssues>(si);
               else { throw new Exception("StaffIssues is required and it cannot be null.."); }
               return si.Id;
           }
           catch (Exception)
           {

               throw;
           }
       }

       public Dictionary<long, IList<StaffIssues>> GetStaffIssueManagementListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
       {
           try
           {
               Dictionary<long, IList<StaffIssues>> retValue = new Dictionary<long, IList<StaffIssues>>();
               return PSF.GetListWithSearchCriteriaCount<StaffIssues>(page, pageSize, sortType, sortBy, criteria);
           }

           catch (Exception)
           {

               throw;
           }
       }

       public StaffIssues GetStaffIssuesById(long Id)
       {
           try
           {
               StaffIssues si = null;
               if (Id > 0)
                   si = PSF.Get<StaffIssues>(Id);
               else { throw new Exception("Id is required and it cannot be 0"); }
               return si;
           }
           catch (Exception)
           {

               throw;
           }
       }
       #region Added By Prabakaran
       public Dictionary<long, IList<StaffIssueManagementReport_vw>> GetStaffIssueManagementReport_vwListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
       {
           try
           {
               Dictionary<long, IList<StaffIssueManagementReport_vw>> retValue = new Dictionary<long, IList<StaffIssueManagementReport_vw>>();
               return PSF.GetListWithSearchCriteriaCount<StaffIssueManagementReport_vw>(page, pageSize, sortType, sortBy, criteria);
           }

           catch (Exception)
           {

               throw;
           }
       }
       #endregion
       #region Added By Gobinath
       public StaffDetails GetStaffDetailsByUserId(string UserId)
       {
           try
           {
               StaffDetails si = null;
               if (!string.IsNullOrEmpty(UserId))
                   si = PSF.Get<StaffDetails>("StaffUserName", UserId);
               else { throw new Exception("Id is required and it cannot be 0"); }
               return si;
           }
           catch (Exception)
           {

               throw;
           }
       }
       #endregion
    }
}
