using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersistenceFactory;
using TIPS.Entities;
using TIPS.Entities.WidgetEntities;

namespace TIPS.Component
{
    public class WidgetBC
    {
        PersistenceServiceFactory PSF = null;
        public WidgetBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }
        public Dictionary<long, IList<UserAppRole>> GetAppRoleForAnUserListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchArrayCriteriaCount<UserAppRole>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Dictionary<long, IList<WidgetUserConfig>> GetUserWidgetConfigWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchArrayCriteriaCount<WidgetUserConfig>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Dictionary<long, IList<WidgetMaster>> GetWidgetMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchArrayCriteriaCount<WidgetMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Dictionary<long, IList<CallManagementView>> GetIssueListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<CallManagementView>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Added By Micheal


        public WidgetUserConfig GetWidgetUserConfigById(long Id)
        {
            try
            {
                WidgetUserConfig WidgetUsr = null;
                if (Id > 0)
                    WidgetUsr = PSF.Get<WidgetUserConfig>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return WidgetUsr;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long SaveOrUpdateWidgetusrConfig(WidgetUserConfig widusr)
        {

            try
            {
                if (widusr != null)
                    PSF.SaveOrUpdate<WidgetUserConfig>(widusr);
                return widusr.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


    }
}
