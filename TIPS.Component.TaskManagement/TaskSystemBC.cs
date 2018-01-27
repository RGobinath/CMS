using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersistenceFactory;
using TIPS.Entities.TaskManagement;
using TIPS.Entities.TicketingSystem;

namespace TIPS.Component.TaskManagement
{
    public class TaskSystemBC
    {
         PersistenceServiceFactory PSF = null;
         public TaskSystemBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }
        public TaskSystem GetTaskSystemById(long Id)
        {
            try
            {
                return PSF.Get<TaskSystem>(Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string SaveOrUpdateTaskSystem(TaskSystem TaskSystem)
        {
            try
            {
                if (TaskSystem.Id > 0)
                {
                    PSF.SaveOrUpdate<TaskSystem>(TaskSystem);
                }
                else
                {
                    string TaskNo = "ETask-" + TaskSystem.Id;
                    TaskSystem.TaskNo = TaskNo;
                    PSF.Save<TaskSystem>(TaskSystem);
                }
                return TaskSystem.TaskNo;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<TaskSystem>> GetTaskSystemBCListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<TaskSystem>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<Module>> GetModuleListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<Module>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<Priority>> GetPriorityListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<Priority>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<TaskStatus>> GetTaskStatusListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<TaskStatus>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<TaskType>> GetTaskTypeListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<TaskType>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<Severity>> GetSeverityListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<Severity>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateTaskComments(TaskComments TaskComments)
        {
            try
            {
                if (TaskComments != null)
                    PSF.SaveOrUpdate<TaskComments>(TaskComments);
                else { throw new Exception("Comments is required and it cannot be null.."); }
                return TaskComments.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool EditTaskNote(long id, string note)
        {
            try
            {
                if (id > 0)
                {
                    TaskComments TaskComments = PSF.Get<TaskComments>(id);
                    TaskComments.Note = note;
                    PSF.Update<TaskComments>(TaskComments);
                    return true;
                }

                else return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteTaskComments(long[] Ids)
        {
            try
            {
                if (Ids != null && Ids.Length > 0)
                {
                    IList<TaskComments> list = PSF.GetListByIds<TaskComments>(Ids);
                    PSF.DeleteAll<TaskComments>(list);
                    return true;
                }

                else return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public TaskComments GetTaskCommentsById(long Id)
        {
            try
            {
                TaskComments TaskComments = null;
                if (Id > 0)
                    TaskComments = PSF.Get<TaskComments>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return TaskComments;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IList<TaskComments> GetTaskCommentsByTaskId(long TaskId)
        {
            try
            {
                IList<TaskComments> TaskComments = null;
                if (TaskId > 0)
                    TaskComments = PSF.GetListById<TaskComments>("TaskId", TaskId);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return TaskComments;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<TaskComments>> GetTaskCommentsListWithPaging(int? page, int? pagesize, string sortby, string sorttype, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<TaskComments>> retValue = new Dictionary<long, IList<TaskComments>>();
                return PSF.GetListWithExactSearchCriteriaCount<TaskComments>(page, pagesize, sortby, sorttype, criteria);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool UpdateTaskStatus(long TaskId, string TaskStatus)
        {
            try
            {
                if (TaskId > 0)
                {
                    TaskSystem TaskSystem = PSF.Get<TaskSystem>(TaskId);
                    TaskSystem.TaskStatus = TaskStatus;
                    PSF.Update<TaskSystem>(TaskSystem);
                    return true;
                }

                else return false;
            }
            catch (Exception)
            { throw; }
        }
    }
}
