using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.Entities;
using TIPS.Entities.AdmissionEntities;
using System.Data;

namespace TIPS.ServiceContract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAdmissionManagement" in both code and config file together.
    public interface IAdmissionManagementSC
    {
        long CreateOrUpdateCallManagement(AdmissionManagement am);
        AdmissionManagement GetAdmissionManagementById(long ApplicationNo);
        Dictionary<long, IList<AdmissionManagement>> GetAdmissionManagementListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria);
        UploadedFiles GetUploadedFilesByIdWithDirectQuery(long Id);
        IList<UploadedFiles> GetUploadedFilesByPreRegNum(long PreRegNum, string flag);
        DataTable GetDashBoardStudentList(string StrSql);

    }
}
