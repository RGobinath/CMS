using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.Entities;
using TIPS.Component;
using System.Collections;
using System.Data;
using TIPS.Entities.StaffEntities;

namespace TIPS.ServiceContract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MasterDataService" in code, svc and config file together.
    public class MasterDataService : IMasterDataServiceSC
    {
        public Dictionary<long, IList<StudentDetails>> GetStudentDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby,  Dictionary<string, object> criteria)
        {
            try
            {
                MasterDataBC MasterDataBC = new MasterDataBC();
                    return MasterDataBC.GetStudentDetailsListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateStudentDetailsMasterList(Entities.StudentDetails student)
        {
            try
            {
                MasterDataBC StudentBC = new MasterDataBC();
                StudentBC.CreateOrUpdateStudentDetailsMasterList(student);
                return student.Id;
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

        public Dictionary<long, IList<IssueGroupMaster>> GetIssueGroupListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MasterDataBC MasterDataBC = new MasterDataBC();
                return MasterDataBC.GetIssueGroupListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<IssueTypeMaster> GetIssueTypeById(string IssueGroup)
        {
            try
            {
                MasterDataBC MasterDataBC = new MasterDataBC();
                return MasterDataBC.GetIssueType(IssueGroup);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public IList<Grade> GetGradeByCampus(string Campus)
        {
            try
            {
                MasterDataBC MasterDataBC = new MasterDataBC();
                return MasterDataBC.GetGradeByCampus(Campus);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public DataTable GetTotalIssueCountPerDay(string TotalCountPerDay1)
        {
            try
            {
                MasterDataBC MasterDataBC = new MasterDataBC();
                return MasterDataBC.GetTotalIssueCountPerDay(TotalCountPerDay1);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public DataSet GetTotalIssueCountPerDay(string[] TotalCountPerDay1)
        {
            try
            {
                MasterDataBC MasterDataBC = new MasterDataBC();
                return MasterDataBC.GetTotalIssueCountPerDay(TotalCountPerDay1);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<StaffIssueGroupMaster>> GetStaffIssueGroupListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MasterDataBC MasterDataBC = new MasterDataBC();
                return MasterDataBC.GetStaffIssueGroupListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<StaffIssueTypeMaster> GetStaffIssueTypeById(string IssueGroup)
        {
            try
            {
                MasterDataBC MasterDataBC = new MasterDataBC();
                return MasterDataBC.GetStaffIssueTypeById(IssueGroup);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
    }
}
