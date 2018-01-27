using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.Entities.Assess.ReportCardClasses;

namespace TIPS.ServiceContract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IReportCardService" in both code and config file together.
    [ServiceContract]
    public interface IReportCardService
    {
        [OperationContract]
        long SaveOrUpdateMYPReportCard(RptCardMYP RptCardMYP);
        [OperationContract]
        RptCardMYP GetMYPReportCard(long Id);

        [OperationContract]
        long SaveOrUpdatePYPReportCard(RptCardPYP RptCardPYP);
        [OperationContract]
        RptCardPYP GetPYPReportCard(long Id);

        [OperationContract]
        long SaveOrUpdateRptCardFocus(RptCardFocus RptCardFocus);
        [OperationContract]
        RptCardFocus GetRptCardFocusById(long RptCardFocusId);
        [OperationContract]
        RptCardFocus GetRptCardFocusByGradeCampusSem(string Grade, string Campus, long Semester, string AcademicYear);

        [OperationContract]
        Dictionary<long, IList<RptCardInBoxView>> GetRepCardInBoxListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);
        [OperationContract]
        Dictionary<long, IList<StudentDtlsView>> GetStudentDtlsViewListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, string name, string[] values, Dictionary<string, object> criteria);
        #region Start PYP 1 to 5 Grades
        [OperationContract]
        long SaveOrUpdatePYPFirstGrade(RptCardPYPFirstGrade RptPYP);
        [OperationContract]
        RptCardPYPFirstGrade GetPYPFirstGrade(long Id);
        [OperationContract]
        Dictionary<long, IList<RptCardPYPFirstGrade>> GetRepCardFirstGradeListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);

        #endregion End PYP 1 to 5 Grades
    }
}