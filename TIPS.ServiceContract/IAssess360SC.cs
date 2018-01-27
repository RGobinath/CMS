using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TIPS.Entities.Assess;
using System.ServiceModel;
namespace TIPS.ServiceContract
{
    [ServiceContract]
    public interface IAssess360SC
    {
        [OperationContract]
        Assess360 GetAssess360ById(long Id);
        [OperationContract]
        long SaveOrUpdateAssess360(Assess360 Assess360);
        [OperationContract]
        long SaveOrUpdateAssess360Component(Assess360Component Assess360Component);
        [OperationBehavior]
        Assess360Component GetAssess360Component(long Id);
        [OperationContract]
        Dictionary<long, IList<Assess360>> GetAssess360ListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);
        [OperationContract]
        Dictionary<long, IList<AssessCompMaster>> GetAssess360CompMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);
        [OperationContract]
        IList<AssessCompMaster> GetAssess360CompMasterListByName(string AssessCompGroup);
        [OperationContract]
        Dictionary<long, IList<StaffMaster>> GetStaffMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);
        [OperationContract]
        Dictionary<long, IList<SubjectMaster>> GetSubjectMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);
        [OperationContract]
        Dictionary<long, IList<StudentDetailsView>> GetStudentDetailsViewListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);
        [OperationContract]
        Dictionary<long, IList<Assess360Component>> GetAssess360ComponentListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);
        [OperationContract]
        Dictionary<long, IList<AssessGroupMaster>> GetAssessGroupMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);
        #region new subject based semester marks
        [OperationContract]
        Dictionary<long, IList<SubjectStudentTemplate>> GetSubjectMarksViewListWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);
        [OperationContract]
        Dictionary<long, IList<SubjectMarks>> GetSubjectMarksViewListWithCriteria1(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);
        [OperationContract]
        SubjectMarks CheckExistdatainSubjectMarks(long Id);
        [OperationContract]
        long CreateOrUpdateSubjectMarks(SubjectMarks sub);
        [OperationContract]
        Dictionary<long, IList<StudentFinalResult_vw>> GetStudentFinalResultWidthSubjectWiseList(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);
        [OperationContract]
        string GetComponentWiseConsolidatedMarksForAStudent(long Assess360Id);
        #endregion new subject based semester marks
    }
}
