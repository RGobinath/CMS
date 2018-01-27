using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using TIPS.Entities;
using TIPS.Entities.TicketingSystem;
namespace TIPS.ServiceContract
{
    [ServiceContract]
    public interface IProcessFlowSC
    {
        [OperationContract]
        long CreateCallManagement(CallManagement CallManagement);
        [OperationContract]
        long StartCallManagement(Entities.CallManagement CallManagement, string Template, string userId);
        [OperationContract]
        bool CreateInformationActivity(CallManagement CallManagement, string Template, string userId, string ActivityName, string Notification, bool isHosteller);
        [OperationContract]
        bool CompleteInformationActivity(CallManagement CallManagement, string Template, string userId, string ActivityName);
        [OperationContract]
        bool CompleteActivityCallManagement(CallManagement CallManagement, string Template, string userId, string ActivityName, bool isRejection);
        [OperationContract]
        bool BulkCompleteActivityCallManagement(long[] ActivityId, string Template, string userId);
        [OperationContract]
        long CreateActivity(Activity Activity);
        [OperationContract]
        ProcessInstance GetProcessInstanceById(long Id);
        [OperationContract]
        WorkFlowStatus GetWorkFlowStatusById(long Id);
        [OperationContract]
        Activity GetActivityById(long Id);
        [OperationContract]
        CallManagement GetCallManagementById(long Id);
        [OperationContract]
        Dictionary<long, IList<ProcessInstance>> GetProcessInstanceListWithsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria);
        [OperationContract]
        Dictionary<long, IList<Entities.CallMgmntPIView>> GetProcessInstanceViewListWithsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria);
        [OperationContract]
        Dictionary<long, IList<WorkFlowStatus>> GetWorkFlowStatusListWithsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria);
        [OperationContract]
        Dictionary<long, IList<CallMgmntActivity>> GetActivityListWithsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria, string ColumnName, string[] values, string[] alias);
        [OperationContract]
        bool AssignActivity(long activityId, string userId);
        [OperationContract]
        bool AssignActivityCheckBeforeAssigning(long activityId, string userId);
        [OperationContract]
        Dictionary<long, IList<TicketSystemActivity>> GetTicketSystemActivityListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, string ColumnName, string[] values, string[] alias);
        [OperationContract]
        string StartETicketingSystem(TicketSystem TicketSystem, string Template, string userId);
    }
}
