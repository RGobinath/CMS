using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TIPS.Entities;
using PersistenceFactory;
using System.Collections;
using System.Data;
using TIPS.Entities.StaffEntities;
using TIPS.Entities.LMSEntities;

namespace TIPS.Component
{
    public class MasterDataBC
    {
        PersistenceServiceFactory PSF = null;
        public MasterDataBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }

        public Dictionary<long, IList<StudentDetails>> GetStudentDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<StudentDetails>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<StudentDetailsVw>> GetStudentDetailsVwListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<StudentDetailsVw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<StudFamilyDetailsEmail>> GetStudFamilyDetailsEmailListForAStudent(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<StudFamilyDetailsEmail>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<StudFamilyDetailsEmail>> GetStudFamilyDetailsEmailListForAppnos(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria, string colName, string[] values)
        {
            try
            {
                criteria.Add(colName,values);
                return PSF.GetListWithExactSearchCriteriaCount<StudFamilyDetailsEmail>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateStudentDetailsMasterList(StudentDetails sdm)
        {
            try
            {
                if (sdm != null)
                    PSF.SaveOrUpdate<StudentDetails>(sdm);
                else { throw new Exception("CallManagement is required and it cannot be null.."); }
                return sdm.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<IssueGroupMaster>> GetIssueGroupListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<IssueGroupMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IssueTypeMaster> GetIssueType(string IssueGroup) //Administrative
        {

            try
            {
                return PSF.GetListByName<IssueTypeMaster>("from " + typeof(IssueTypeMaster) + " where IssueGroup ='" + IssueGroup + "'");
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public IList<Grade> GetGradeByCampus(string Campus) //Administrative
        {

            try
            {
                return PSF.GetListByName<Grade>("from " + typeof(Grade) + " where Campus ='" + Campus + "'");
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public DataTable GetTotalIssueCountPerDay(string TotalCountPerDay1) //Administrative
        {

            try
            {
                return PSF.ExecuteSqlUsingSQLCommand(TotalCountPerDay1);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public DataSet GetTotalIssueCountPerDay(string[] TotalCountPerDay1) //Administrative
        {

            try
            {
                return PSF.ExecuteSqlUsingSQLCommand(TotalCountPerDay1);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public IList<StaffIssueTypeMaster> GetStaffIssueTypeById(string IssueGroup) //Administrative
        {
            try
            {
                return PSF.GetListByName<StaffIssueTypeMaster>("from " + typeof(StaffIssueTypeMaster) + " where IssueGroup ='" + IssueGroup + "'");
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public Dictionary<long, IList<StaffIssueGroupMaster>> GetStaffIssueGroupListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<StaffIssueGroupMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<LMS_StaffStudentDetails_Vw>> GetStaffandStudentDetailsListwithCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<LMS_StaffStudentDetails_Vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
