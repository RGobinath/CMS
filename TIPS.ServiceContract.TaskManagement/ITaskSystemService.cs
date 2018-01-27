using System.Collections.Generic;
using System.ServiceModel;
using TIPS.Entities.TaskManagement;
using TIPS.Entities.TicketingSystem;

namespace TIPS.Service.TaskManagement
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITaskSystemService" in both code and config file together.
    [ServiceContract]
    public interface ITaskSystemService
    {
        [OperationContract]
        TaskSystem GetTaskSystemById(long Id);
        [OperationContract]
        string SaveOrUpdateTaskSystem(TaskSystem TaskSystem);
        [OperationContract]
        Dictionary<long, IList<TaskSystem>> GetTaskSystemBCListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);
        [OperationContract]
        Dictionary<long, IList<Module>> GetModuleListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);
        [OperationContract]
        Dictionary<long, IList<Priority>> GetPriorityListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);
        [OperationContract]
        Dictionary<long, IList<TaskStatus>> GetTaskStatusListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);
        [OperationContract]
        Dictionary<long, IList<TaskType>> GetTaskTypeListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);
        [OperationContract]
        Dictionary<long, IList<Severity>> GetSeverityListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);
        [OperationContract]
        long CreateOrUpdateTaskComments(TaskComments TaskComments);
        [OperationContract]
        TaskComments GetTaskCommentsById(long Id);
        [OperationContract]
        IList<TaskComments> GetTaskCommentsByTaskId(long TaskId);
        [OperationContract]
        Dictionary<long, IList<TaskComments>> GetTaskCommentsListWithPaging(int? page, int? pagesize, string sortby, string sorttype, Dictionary<string, object> criteria);
    }
}
