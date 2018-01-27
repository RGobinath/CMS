using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using TIPS.Component;
using TIPS.Entities;
using TIPS.ServiceContract;
using TIPS.Entities.WidgetEntities;

namespace TIPS.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class WidgetService
    {
        public Dictionary<long, IList<WidgetUserConfig>> GetUserWidgetConfigWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    WidgetBC widgetBC = new WidgetBC();
                    return widgetBC.GetUserWidgetConfigWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally { }
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
                try
                {
                    WidgetBC WidgetBC = new WidgetBC();
                    return WidgetBC.GetWidgetMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally { }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Dictionary<long, IList<CallManagementView>> GetIssueDetailsListWithPaging(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {

            try
            {
                WidgetBC WidgetBC = new WidgetBC();
                return WidgetBC.GetIssueListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }

        }


        #region Added By Micheal
        public WidgetUserConfig GetWidgetUsrConfigById(long id)
        {
            try
            {
                WidgetBC widgetbc = new WidgetBC();
                return widgetbc.GetWidgetUserConfigById(id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion

        public long SaveOrUpdateWidgetusrConfig(WidgetUserConfig widusr)
        {
            try
            {
                WidgetBC WidgetBC = new WidgetBC();
                return WidgetBC.SaveOrUpdateWidgetusrConfig(widusr);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

    }
}
