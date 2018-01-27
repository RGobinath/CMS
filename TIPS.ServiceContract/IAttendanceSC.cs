using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Linq;
using System.Text;
using TIPS.Entities;
using TIPS.Entities.Attendance;
using TIPS.Entities.AdmissionEntities;

namespace TIPS.ServiceContract
{
    [ServiceContract()]
    public interface IAttendanceSC
    {
        [OperationContract]
        Dictionary<long, IList<UserAppRole>> GetAppRoleForAnUserListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);
        [OperationContract]
        Dictionary<long, IList<StudentAttendanceView>> GetStudentListListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);
        [OperationContract]
        Dictionary<long, IList<Attendance>> GetStudentTemplateForAnAttendanceListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);
        [OperationContract]
        Dictionary<long, IList<Holidays>> GetHolidaysListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);
        [OperationContract]
        Dictionary<long, IList<Attendance>> GetAbsentListForAnAttendanceListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);
        [OperationContract]
        long CreateOrUpdateAttendanceList(Attendance att);
        [OperationContract]
        Attendance GetAttentanceById(long Id);
        [OperationContract]
        long DeleteAttendancevalue(Attendance att);
        [OperationContract]
        long CreateOrUpdateHolyDays(Holidays holy);
        [OperationContract]
        Dictionary<long, IList<FamilyDetails>> GetFamilyDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria);
        [OperationContract]
        long CreateOrUpdateSMSLog(SMS sms);
    }
}
