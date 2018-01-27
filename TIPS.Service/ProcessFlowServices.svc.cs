using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.ServiceContract;
using TIPS.Component;
using TIPS.Entities;
using TIPS.Entities.StaffEntities;
using TIPS.Entities.TicketingSystem;
using TIPS.Entities.HRManagementEntities;
using TIPS.Entities.TaskManagement;
namespace TIPS.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ProcessFlowServices" in code, svc and config file together.
    public class ProcessFlowServices : IProcessFlowSC
    {

        public long CreateCallManagement(Entities.CallManagement CallManagement)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                ProcessFlowBC.CreateCallManagement(CallManagement);
                return CallManagement.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public long StartCallManagement(Entities.CallManagement CallManagement, string Template, string userId)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                ProcessFlowBC.StartCallManagement(CallManagement, Template, userId);
                return CallManagement.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool CreateInformationActivity(CallManagement CallManagement, string Template, string userId, string ActivityName, string Notification, bool isHosteller)
        {
            try
            {
                bool retValue = false;
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                ProcessFlowBC.CreateInformationActivity(CallManagement, Template, userId, ActivityName, Notification, isHosteller);
                retValue = true;
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        
        public bool CompleteInformationActivity(CallManagement CallManagement, string Template, string userId, string ActivityName)
        {
            try
            {
                bool retValue = false;
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                ProcessFlowBC.CompleteInformationActivity(CallManagement, Template, userId, ActivityName);
                retValue = true;
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public bool CompleteActivityCallManagement(Entities.CallManagement CallManagement, string Template, string userId, string ActivityName, bool isRejection)
        {
            try
            {
                bool retValue = false;
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                ProcessFlowBC.CompleteActivityCallManagement(CallManagement, Template, userId, ActivityName, isRejection);
                retValue = true;
                return retValue;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }
        public bool BulkCompleteActivityCallManagement(long[] ActivityId, string Template, string userId) 
        {
            try
            {
                bool retValue = false;
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                ProcessFlowBC.BulkCompleteActivityCallManagement(ActivityId, Template, userId);
                retValue = true;
                return retValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool BulkInfoCompleteActivityCallManagement(long[] ActivityIds, string Template, string userId)
        {
            try
            {
                bool retValue = false;
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                ProcessFlowBC.BulkInfoCompleteActivityCallManagement(ActivityIds, Template, userId);
                retValue = true;
                return retValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public long CreateActivity(Entities.Activity Activity)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.CreateActivity(Activity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool AssignActivity(long activityId, string userId)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.AssignActivity(activityId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool AssignActivityCheckBeforeAssigning(long activityId, string userId)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.AssignActivityCheckBeforeAssigning(activityId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Entities.ProcessInstance GetProcessInstanceById(long Id)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.GetProcessInstanceById(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Entities.WorkFlowStatus GetWorkFlowStatusById(long Id)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.GetWorkFlowStatusById(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Entities.Activity GetActivityById(long Id)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.GetActivityById(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Entities.CallManagement GetCallManagementById(long Id)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.GetCallManagementById(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<Entities.ProcessInstance>> GetProcessInstanceListWithsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.GetProcessInstanceListWithsearchCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<Entities.CallMgmntPIView>> GetProcessInstanceViewListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.GetProcessInstanceViewListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<Entities.WorkFlowStatus>> GetWorkFlowStatusListWithsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.GetWorkFlowStatusListWithsearchCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<CallMgmntActivity>> GetActivityListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, string ColumnName, string[] values, string[] alias)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.GetActivityListWithsearchCriteria(page, pageSize, sortBy, sortType,  criteria, ColumnName, values, alias);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<TicketSystemActivity>> GetTicketSystemActivityListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, string ColumnName, string[] values, string[] alias)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.GetTicketSystemActivityListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria, ColumnName, values, alias);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public long StartStaffIssueManagement(StaffIssues StaffIssues, string Template, string userId)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                ProcessFlowBC.StartStaffIssueManagement(StaffIssues, Template, userId);
                return StaffIssues.Id;
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("Cannot insert duplicate key row in object 'dbo.QUEUE' with"))
                {
                    throw new FaultException("Cannot insert duplicate key in Queue Name.");
                }
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<StaffIssues>> GetStaffViewListWithsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.GetStaffViewListWithsearchCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<StaffMgmntActivity>> GetStaffIssueActivityListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, string ColumnName, string[] values, string[] alias)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.GetStaffIssueActivityListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria, ColumnName, values, alias);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool CompleteActivityStaffIssueManagement(StaffIssues StaffIssues, string Template, string userId, string ActivityName, bool isRejection)
        {
            try
            {
                bool retValue = false;
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                ProcessFlowBC.CompleteActivityStaffIssueManagement(StaffIssues, Template, userId, ActivityName, isRejection);
                retValue = true;
                return retValue;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public bool BulkIssueCompleteActivityStaffManagement(long[] ActivityId, string Template, string userId)
        {
            try
            {
                bool retValue = false;
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                ProcessFlowBC.BulkIssueCompleteActivityStaffManagement(ActivityId, Template, userId);
                retValue = true;
                return retValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool AssignStaffActivity(long activityId, string userId)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.AssignStaffActivity(activityId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<StaffMgmntActivity>> GetStaffIssueActivityListWithsearchCriteriaOnly(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> Criteria)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.GetStaffIssueActivityListWithsearchCriteriaOnly(page, pageSize, sortType, sortBy, Criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        
        public string StartETicketingSystem(TicketSystem TicketSystem, string Template, string userId)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.StartETicketingSystem(TicketSystem, Template, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public string ReopenActivityTicketSystem(TicketSystem TicketSystem, string Template, string userId, string ActivityName, bool isRejection)
        {
            try
            {
                //bool retValue = false;
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.ReopenActivityTicketSystem(TicketSystem, Template, userId, ActivityName, isRejection);
                //retValue = true;
                //return retValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool CompleteActivityTicketSystem(TicketSystem TicketSystem, string Template, string userId, string ActivityName, bool isRejection)
        {
            try
            {
                bool retValue = false;
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                ProcessFlowBC.CompleteActivityTicketSystem(TicketSystem, Template, userId, ActivityName, isRejection);
                retValue = true;
                return retValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public long StartHRManagement(LeaveRequest lrequest, string Template, string userId)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                ProcessFlowBC.StartHRManagement(lrequest, Template, userId);
                return lrequest.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool BulkCompleteActivityHRManagement(long[] ActivityId, string Template, string userId)
        {
            try
            {
                bool retValue = false;
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                ProcessFlowBC.BulkCompleteActivityHRManagement(ActivityId, Template, userId);
                retValue = true;
                return retValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool CompleteActivityHRManagement(Entities.HRManagementEntities.LeaveRequest HRManagement, string Template, string userId, string Report, string CheckAppRole, string UserIdDetails, string ActivityName, bool isRejection)
        {
            try
            {
                bool retValue = false;
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                ProcessFlowBC.CompleteActivityHRManagement(HRManagement, Template, userId, Report, CheckAppRole, UserIdDetails, ActivityName, isRejection);
                retValue = true;
                return retValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<HRMgmntActivity>> GetHRActivityListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, string ColumnName, string[] values, string[] alias)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.GetHRActivityListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria, ColumnName, values, alias);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        //Bank Account Details
        public long StartBankHRManagement(BankAccount account, string Template, string userId)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                ProcessFlowBC.StartBankHRManagement(account, Template, userId);
                return account.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool CompleteBankActivityHRManagement(Entities.HRManagementEntities.BankAccount account, string Template, string userId, string ActivityName, bool isRejection)
        {
            try
            {
                bool retValue = false;
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                ProcessFlowBC.CompleteBankActivityHRManagement(account, Template, userId, ActivityName, isRejection);
                retValue = true;
                return retValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<BankMgmntActivity>> GetBankHRActivityListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, string ColumnName, string[] values, string[] alias)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.GetBankHRActivityListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria, ColumnName, values, alias);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool BankAccountBulkCompleteActivity(long[] ActivityId, string Template, string userId)
        {
            try
            {
                bool retValue = false;
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                ProcessFlowBC.BankAccountBulkCompleteActivity(ActivityId, Template, userId);
                retValue = true;
                return retValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }


        // Certificate Request Details...
        public long StartCertificateHRManagement(CertificateRequest Crequest, string Template, string userId)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                ProcessFlowBC.StartCertificateHRManagement(Crequest, Template, userId);
                return Crequest.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool CompleteCertificateActivityHRManagement(Entities.HRManagementEntities.CertificateRequest Crequest, string Template, string userId, string ActivityName, bool isRejection)
        {
            try
            {
                bool retValue = false;
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                ProcessFlowBC.CompleteCertificateActivityHRManagement(Crequest, Template, userId, ActivityName, isRejection);
                retValue = true;
                return retValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<CertificateMgmntActivity>> GetCertificateHRActivityListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, string ColumnName, string[] values, string[] alias)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.GetCertificateHRActivityListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria, ColumnName, values, alias);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool CertificateBulkCompleteActivityHRManagement(long[] ActivityId, string Template, string userId)
        {
            try
            {
                bool retValue = false;
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                ProcessFlowBC.CertificateBulkCompleteActivityHRManagement(ActivityId, Template, userId);
                retValue = true;
                return retValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }


        // Salary Advance Request Details...
        public long StartSalaryAdvanceHRManagement(SalaryAdvance Sadvance, string Template, string userId)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                ProcessFlowBC.StartSalaryAdvanceHRManagement(Sadvance, Template, userId);
                return Sadvance.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool CompleteSalaryAdvanceActivityHRManagement(Entities.HRManagementEntities.SalaryAdvance Sadvance, string Template, string userId, string ActivityName, bool isRejection)
        {
            try
            {
                bool retValue = false;
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                ProcessFlowBC.CompleteSalaryAdvanceActivityHRManagement(Sadvance, Template, userId, ActivityName, isRejection);
                retValue = true;
                return retValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<SalaryAdvanceMgmntActivity>> GetSalaryAdvanceHRActivityListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, string ColumnName, string[] values, string[] alias)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.GetSalaryAdvanceHRActivityListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria, ColumnName, values, alias);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool SalaryAdvanceBulkCompleteActivityHRManagement(long[] ActivityId, string Template, string userId)
        {
            try
            {
                bool retValue = false;
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                ProcessFlowBC.SalaryAdvanceBulkCompleteActivityHRManagement(ActivityId, Template, userId);
                retValue = true;
                return retValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool MoveBackToAvailable(long activityId)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.MoveBackToAvailable(activityId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<CallMgmntActivity>> GetActivityListWithsearchCriteriaLikeSearch(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, string ColumnName, string[] values, string[] alias)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.GetActivityListWithsearchCriteriaLikeSearch(page, pageSize, sortBy, sortType, criteria, ColumnName, values, alias);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public string StartETaskSystem(TaskSystem TaskSystem, string Template, string userId)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.StartETaskingSystem(TaskSystem, Template, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool CompleteActivityTaskSystem(TaskSystem TaskSystem, string Template, string userId, string ActivityName, bool isRejection)
        {
            try
            {
                bool retValue = false;
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                ProcessFlowBC.CompleteActivityTaskSystem(TaskSystem, Template, userId, ActivityName, isRejection);
                retValue = true;
                return retValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        

        public Dictionary<long, IList<TaskSystemActivity>> GetTaskSystemActivityListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, string ColumnName, string[] values, string[] alias)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.GetTaskSystemActivityListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria, ColumnName, values, alias);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<Activity>> GetActivityWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, string ColumnName, string[] values, string[] alias)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.GetActivityWithsearchCriteria(page, pageSize, sortBy, sortType, criteria, ColumnName, values, alias);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<StaffActivities>> GetStaffActivityWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, string ColumnName, string[] values, string[] alias)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.GetStaffActivityWithsearchCriteria(page, pageSize, sortBy, sortType, criteria, ColumnName, values, alias);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #region Added By Prabakaran
        public Dictionary<long, IList<Entities.CallMgmntPIView>> GetProcessInstanceViewListWithExactandLikesearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.GetProcessInstanceViewListWithExactandLikesearchCriteria(page, pageSize, sortBy, sortType, criteria,likecriteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #endregion
        #region Added By Prabakaran
        public StaffActivities GetStaffActivitiesById(long Id)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.GetStaffActivitiesById(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }       
        public StaffActivities GetStaffActivities(long InstanceId, long ProcessRefId)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.GetStaffActivities(InstanceId, ProcessRefId);
            }
            catch (Exception)
            {

                throw;
            }
            finally { }
        }
        public long CreateOrUpdateStaffActivities(StaffActivities staffactivities)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                ProcessFlowBC.CreateOrUpdateStaffActivities(staffactivities);
                return staffactivities.Id;
            }
            catch (Exception)
            {

                throw;
            }
            finally { }
        }
        //public StaffActivities GetStaffActivitiesByActivityId(long AcitivityId)
        //{
        //    try
        //    {
        //        ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
        //        return ProcessFlowBC.GetStaffActivitiesByActivityId(AcitivityId);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    finally { }
        //}
        //public StaffIssues GetStaffIssuesById(long Id)
        //{
        //    try
        //    {
        //        ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
        //        return ProcessFlowBC.GetStaffIssuesById(Id);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    finally { }
        //}
        public StaffActivities GetStaffActivitiesByActivityId(long AcitivityId)
        {
            try
            {
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.GetStaffActivitiesByActivityId(AcitivityId);
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
                ProcessFlowBC ProcessFlowBC = new ProcessFlowBC();
                return ProcessFlowBC.GetStaffIssuesById(Id);
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
