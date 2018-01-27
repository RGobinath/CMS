using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.ServiceContract;
using TIPS.Entities;
using TIPS.Component;
using System.Data;

namespace TIPS.ServiceContract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CallManagementService" in code, svc and config file together.
    public class CallManagementService : ICallManagementSC
    {

        public long CreateOrUpdateCallManagement(CallManagement cm)
        {
            try
            {
                CallManagementBC CallManagementBC = new CallManagementBC();
                CallManagementBC.CreateOrUpdateCallManagement(cm);
                return cm.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }
        public CallManagement GetCallManagementById(long Id)
        {
            try
            {
                CallManagementBC CallManagementBC = new CallManagementBC();
                return CallManagementBC.GetCallManagementById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<CallManagement>> GetCallManagementListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                CallManagementBC CallManagementBC = new CallManagementBC();
                return CallManagementBC.GetCallManagementListWithPaging(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<BulkCompleteInfo>> GetInformationListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                CallManagementBC CallManagementBC = new CallManagementBC();
                return CallManagementBC.GetInformationListWithsearchCriteria(page, pageSize,sortBy ,sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        

        public Dictionary<long, IList<CallManagementChart>> GetCallManagementListWithPagingChart(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                CallManagementBC CallManagementBC = new CallManagementBC();
                return CallManagementBC.GetCallManagementListWithPagingChart(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<CallManagementDashboard>> GetCallManagementListWithPagingDashboard(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                CallManagementBC CallManagementBC = new CallManagementBC();
                return CallManagementBC.GetCallManagementListWithPagingDashboard(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        #region Monthly wise Issue count report

        public Dictionary<long, IList<IssueCountReportView>> GetIssueCountListWithPagingAndCriteria_vw(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                CallManagementBC CmBC = new CallManagementBC();
                return CmBC.GetIssueCountListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion

        public DataTable GetStatusWiseIssueCount(string strQry1)
        {
            try
            {
                CallManagementBC CallManagementBC = new CallManagementBC();
                return CallManagementBC.GetStatusWiseIssueCount(strQry1);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }


        public Dictionary<long, IList<Activity>> GetCallManagementActivityList(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                CallManagementBC CallManagementBC = new CallManagementBC();
                return CallManagementBC.GetCallManagementActivityList(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #region Added By Prabakaran CallManagement
        public Dictionary<long, IList<CallManagementHistory>> GetCallManagementHistoryListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                CallManagementBC CallManagementBC = new CallManagementBC();
                return CallManagementBC.GetCallManagementHistoryListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria, likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public CallManagement GetCallManagementByIssueNumber(string Id)
        {
            try
            {
                CallManagementBC CallManagementBC = new CallManagementBC();
                return CallManagementBC.GetCallManagementByIssueNumber(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public bool DeleteCallManagement(long Id)
        {
            try
            {
                CallManagementBC CallManagementBC = new CallManagementBC();
                CallManagementBC.DeleteCallManagementById(Id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public CallManagementHistory GetCallManagementHistoryByCallManagementId(long Id)
        {
            try
            {
                CallManagementBC CallManagementBC = new CallManagementBC();
                return CallManagementBC.GetCallManagementHistoryByCallManagementId(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public CallManagementHistory GetCallManagementHistoryById(long Id)
        {
            try
            {
                CallManagementBC CallManagementBC = new CallManagementBC();
                return CallManagementBC.GetCallManagementHistoryById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long CreateOrUpdateCallManagementHistory(CallManagementHistory cmh)
        {
            try
            {
                CallManagementBC CallManagementBC = new CallManagementBC();
                CallManagementBC.CreateOrUpdateCallManagementHistory(cmh);
                return cmh.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }
        public IList<Activity> GetActivityByProcessRefId(long Id)
        {
            try
            {
                CallManagementBC CallManagementBC = new CallManagementBC();
                return CallManagementBC.GetActivityByProcessRefId(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long CreateOrUpdateActivity(Activity activity)
        {
            try
            {
                CallManagementBC CallManagementBC = new CallManagementBC();
                CallManagementBC.CreateOrUpdateActivity(activity);
                return activity.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #endregion
        #region IssueCountReportByCampus By Prabakaran
        public Dictionary<long, IList<IssueCountReportByCampus_SP>> GetIssueCountReportByCampus_SPList(string Campus,DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                CallManagementBC CallManagementBC = new CallManagementBC();
                return CallManagementBC.GetIssueCountReportByCampus_SPList(Campus, FromDate, ToDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region IssueCountReportByIssueGroup By Prabakaran
        public Dictionary<long, IList<IssueCountReportByIssueGroup_SP>> GetIssueCountReportByIssueGroup_SPList(string Campus,string IssueGroup, DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                CallManagementBC CallManagementBC = new CallManagementBC();
                return CallManagementBC.GetIssueCountReportByIssueGroup_SPList(Campus,IssueGroup, FromDate, ToDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region PerformerWiseCountReport By Prabakaran
        public Dictionary<long, IList<PerformerWiseIssueCountReport_SP>> GetPerformerWiseIssueCountReport_SPList(string BranchCode,string Performer, DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                CallManagementBC CallManagementBC = new CallManagementBC();
                return CallManagementBC.GetPerformerWiseIssueCountReport_SPList(BranchCode,Performer, FromDate, ToDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
