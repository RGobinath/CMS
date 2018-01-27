using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.Component;
using TIPS.Entities.AdminTemplateEntities;

namespace TIPS.ServiceContract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AdminTemplateService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AdminTemplateService.svc or AdminTemplateService.svc.cs at the Solution Explorer and start debugging.
    public class AdminTemplateService : IAdminTemplateService
    {
        #region Dashboard Index
        public Dictionary<long, IList<AdminTemplateDashboardIndex_vw>> GetAdminTemplateDashboardIndex_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdminTemplateBC ATBC = new AdminTemplateBC();
                return ATBC.GetAdminTemplateDashboardIndex_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Students
        public Dictionary<long, IList<AdminTemplateStudents_vw>> GetAdminTemplateStudents_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdminTemplateBC ATBC = new AdminTemplateBC();
                return ATBC.GetAdminTemplateStudents_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Staff
        public Dictionary<long, IList<AdminTemplateStaffDetails_vw>> GetAdminTemplateStaffDetails_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdminTemplateBC ATBC = new AdminTemplateBC();
                return ATBC.GetAdminTemplateStaffDetails_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<AdminTemplateStaffStatus_vw>> GetAdminTemplateStaffStatus_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdminTemplateBC ATBC = new AdminTemplateBC();
                return ATBC.GetAdminTemplateStaffStatus_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Admission
        public Dictionary<long, IList<AdminTemplateStudentsAcademicYearWiseCount_vw>> GetAdminTemplateStudentsAcademicYearWiseCount_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdminTemplateBC ATBC = new AdminTemplateBC();
                return ATBC.GetAdminTemplateStudentsAcademicYearWiseCount_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<AdminTemplateAdmissionStatusReport_vw>> GetAdminTemplateAdmissionStatusReport_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdminTemplateBC ATBC = new AdminTemplateBC();
                return ATBC.GetAdminTemplateAdmissionStatusReport_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<AdminTemplateStudBoardingReport_vw>> GetAdminTemplateStudBoardingReport_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdminTemplateBC ATBC = new AdminTemplateBC();
                return ATBC.GetAdminTemplateStudBoardingReport_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Email
        public Dictionary<long, IList<AdminTemplateEmailReport_vw>> GetAdminTemplateEmailReport_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdminTemplateBC ATBC = new AdminTemplateBC();
                return ATBC.GetAdminTemplateEmailReport_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region SMS
        public Dictionary<long, IList<AdminTemplateSMSReport_vw>> GetAdminTemplateSMSReport_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdminTemplateBC ATBC = new AdminTemplateBC();
                return ATBC.GetAdminTemplateSMSReport_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Users

        public Dictionary<long, IList<AdminTemplateUsersTypeReport_vw>> GetAdminTemplateUsersTypeReport_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdminTemplateBC ATBC = new AdminTemplateBC();
                return ATBC.GetAdminTemplateUsersTypeReport_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Newly Added
        public Dictionary<long, IList<AdminTemplateAssess360MarkCount_vw>> GetAdminTemplateAssess360MarkCount_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdminTemplateBC ATBC = new AdminTemplateBC();
                return ATBC.GetAdminTemplateAssess360MarkCount_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<AdminTemplateLoginHistory_vw>> GetAdminTemplateLoginHistory_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdminTemplateBC ATBC = new AdminTemplateBC();
                return ATBC.GetAdminTemplateLoginHistory_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<AdminTemplateStaffGroupByDept_vw>> GetAdminTemplateStaffGroupByDept_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdminTemplateBC ATBC = new AdminTemplateBC();
                return ATBC.GetAdminTemplateStaffGroupByDept_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<AdminTemplateBrowserWiseReport_vw>> GetAdminTemplateBrowserWiseReport_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdminTemplateBC ATBC = new AdminTemplateBC();
                return ATBC.GetAdminTemplateBrowserWiseReport_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<AdminTemplateUsersReport_vw>> GetAdminTemplateUsersReport_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdminTemplateBC ATBC = new AdminTemplateBC();
                return ATBC.GetAdminTemplateUsersReport_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Added on 03/11/2014
        public Dictionary<long, IList<AdminTemplateStaffsGrpByDeptAndCampus_vw>> GetAdminTemplateStaffsGrpByDeptAndCampus_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdminTemplateBC ATBC = new AdminTemplateBC();
                return ATBC.GetAdminTemplateStaffsGrpByDeptAndCampus_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Added on 04/11/2014
        public Dictionary<long, IList<AdminTemplateAssess360GrpByCampusAndGrade_vw>> GetAdminTemplateAssess360GrpByCampusAndGrade_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdminTemplateBC ATBC = new AdminTemplateBC();
                return ATBC.GetAdminTemplateAssess360GrpByCampusAndGrade_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Added on 05/11/2014
        //public Dictionary<long, IList<AdminTemplateTransportReportByCampus_vw>> GetAdminTemplateTransportReportByAllCampus_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, string ColumnName, string[] Values)
        //{
        //    try
        //    {
        //        AdminTemplateBC ATBC = new AdminTemplateBC();
        //        return ATBC.GetAdminTemplateTransportReportByAllCampus_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria, ColumnName, Values);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public Dictionary<long, IList<AdminTemplateTransportReportByCampus_vw>> GetAdminTemplateTransportReportByCampus_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdminTemplateBC ATBC = new AdminTemplateBC();
                return ATBC.GetAdminTemplateTransportReportByCampus_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public Dictionary<long, IList<AdminTemplateStudentTemplate_vw>> GetAdminTemplateStudentTemplate_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdminTemplateBC ATBC = new AdminTemplateBC();
                return ATBC.GetAdminTemplateStudentTemplate_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<AdminTemplateStudentsCountGroupByGrade_vw>> GetAdminTemplateStudentsCountGroupByGrade_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdminTemplateBC ATBC = new AdminTemplateBC();
                return ATBC.GetAdminTemplateStudentsCountGroupByGrade_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<AdminTemplateStudCountGrpByCampusGradeSection_vw>> GetAdminTemplateStudCountGrpByCampusGradeSection_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdminTemplateBC ATBC = new AdminTemplateBC();
                return ATBC.GetAdminTemplateStudCountGrpByCampusGradeSection_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region Campus Wise Issue Count
        public Dictionary<long, IList<AdminTemplateCampusWiseIssueCount_vw>> GetAdminTemplateCampusWiseIssueCount_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdminTemplateBC ATBC = new AdminTemplateBC();
                return ATBC.GetAdminTemplateCampusWiseIssueCount_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

    }
}
