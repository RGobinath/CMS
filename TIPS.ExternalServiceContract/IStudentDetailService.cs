using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TIPS.Entities;
using System.ServiceModel;
namespace TIPS.ExternalServiceContract
{
    [ServiceContract]
    public interface IStudentDetailService
    {
        [OperationContract]
        bool ValidateWebServiceUser(string uid, string pwd, string roleCode);
        [OperationContract]
        Dictionary<long, IList<StudentDetailsVw>> GetStudentDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, string uid, string pwd, string roleCode, Dictionary<string, object> criteria);
        [OperationContract]
        IList<StudentDetailsVw> GetStudentDetailsListWithPagingWithoutCount(int? page, int? pageSize, string sortby, string sortType, string uid, string pwd, string roleCode, Dictionary<string, object> criteria);
        [OperationContract]
        Dictionary<long, IList<StudentDetailsVw>> GetStudentDetailsListWithPagingCriteriaEnum(int? page, int? pageSize, string sortby, string sortType, string uid, string pwd, string roleCode, Dictionary<StudentDetails_Enum, object> criteria);
        [OperationContract]
        IList<StudFamilyDetailsEmail> GetStudFamilyDetailsEmailListForAStudent(string AppNo, string uid, string pwd, string roleCode);
        [OperationContract]
        Dictionary<long, IList<StudFamilyDetailsEmail>> GetStudFamilyDetailsEmailListForAppnos(string[] AppNo, string uid, string pwd, string roleCode);
        [OperationContract]
        Dictionary<long, IList<StudFamilyDetailsEmail>> GetStudFamilyDetailsEmailListWithFamilyType(string[] AppNo, FamilyTypeEnum FamilyTypeEnum, string uid, string pwd, string roleCode);
        [OperationContract]
        Dictionary<long, IList<StudFamilyDetailsEmail>> GetStudFamilyDtlsEmailLstWithFamilyTypeForIdNo(string[] IdNo, FamilyTypeEnum FamilyTypeEnum, string uid, string pwd, string roleCode);
    }
}
