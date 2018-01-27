using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.Entities.TaskManagement;
using TIPS.Component.TaskManagement;
using TIPS.Entities.TicketingSystem;

namespace TIPS.Service.TaskManagement
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TaskSystemService" in code, svc and config file together.
    public class TaskSystemService : ITaskSystemService
    {
        public TaskSystem GetTaskSystemById(long Id)
        {
            try
            {
                TaskSystemBC TaskSystemBC = new TaskSystemBC();
                return TaskSystemBC.GetTaskSystemById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public string SaveOrUpdateTaskSystem(TaskSystem TaskSystem)
        {
            try
            {
                TaskSystemBC TaskSystemBC = new TaskSystemBC();
                return TaskSystemBC.SaveOrUpdateTaskSystem(TaskSystem);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<TaskSystem>> GetTaskSystemBCListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TaskSystemBC TaskSystemBC = new TaskSystemBC();
                return TaskSystemBC.GetTaskSystemBCListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<Module>> GetModuleListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TaskSystemBC TaskSystemBC = new TaskSystemBC();
                return TaskSystemBC.GetModuleListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<Priority>> GetPriorityListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TaskSystemBC TaskSystemBC = new TaskSystemBC();
                return TaskSystemBC.GetPriorityListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<TaskStatus>> GetTaskStatusListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TaskSystemBC TaskSystemBC = new TaskSystemBC();
                return TaskSystemBC.GetTaskStatusListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<TaskType>> GetTaskTypeListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TaskSystemBC TaskSystemBC = new TaskSystemBC();
                return TaskSystemBC.GetTaskTypeListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<Severity>> GetSeverityListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TaskSystemBC TaskSystemBC = new TaskSystemBC();
                return TaskSystemBC.GetSeverityListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long CreateOrUpdateTaskComments(TaskComments TaskComments)
        {
            try
            {
                TaskSystemBC TaskSystemBC = new TaskSystemBC();
                return TaskSystemBC.CreateOrUpdateTaskComments(TaskComments);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public TaskComments GetTaskCommentsById(long Id)
        {
            try
            {
                TaskSystemBC TaskSystemBC = new TaskSystemBC();
                return TaskSystemBC.GetTaskCommentsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public IList<TaskComments> GetTaskCommentsByTaskId(long TaskId)
        {
            try
            {
                TaskSystemBC TaskSystemBC = new TaskSystemBC();
                return TaskSystemBC.GetTaskCommentsByTaskId(TaskId);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<TaskComments>> GetTaskCommentsListWithPaging(int? page, int? pagesize, string sortby, string sorttype, Dictionary<string, object> criteria)
        {
            try
            {
                TaskSystemBC TaskSystemBC = new TaskSystemBC();
                return TaskSystemBC.GetTaskCommentsListWithPaging(page, pagesize, sortby, sorttype, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public bool EditTaskNote(long id, string note)
        {
            try
            {
                TaskSystemBC TaskSystemBC = new TaskSystemBC();
                return TaskSystemBC.EditTaskNote(id, note);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public bool DeleteTaskComments(long[] Ids)
        {
            try
            {
                TaskSystemBC TaskSystemBC = new TaskSystemBC();
                return TaskSystemBC.DeleteTaskComments(Ids);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public bool UpdateTaskStatus(long TaskId, string TaskStatus)
        {
            try
            {
                TaskSystemBC TaskSystemBC = new TaskSystemBC();
                return TaskSystemBC.UpdateTaskStatus(TaskId, TaskStatus);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
    }
}
