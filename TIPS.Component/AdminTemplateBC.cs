using PersistenceFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TIPS.Entities.AdminTemplateEntities;

namespace TIPS.Component
{
    public class AdminTemplateBC
    {
        PersistenceServiceFactory PSF = null;
        public AdminTemplateBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }
        #region Dashboard
        public Dictionary<long, IList<AdminTemplateDashboardIndex_vw>> GetAdminTemplateDashboardIndex_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<AdminTemplateDashboardIndex_vw>(page, pageSize, sortType, sortby, criteria);
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
                return PSF.GetListWithEQSearchCriteriaCount<AdminTemplateStudents_vw>(page, pageSize, sortType, sortby, criteria);
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
                return PSF.GetListWithEQSearchCriteriaCount<AdminTemplateStaffDetails_vw>(page, pageSize, sortType, sortby, criteria);
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
                return PSF.GetListWithEQSearchCriteriaCount<AdminTemplateStaffStatus_vw>(page, pageSize, sortType, sortby, criteria);
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
                return PSF.GetListWithEQSearchCriteriaCount<AdminTemplateStudentsAcademicYearWiseCount_vw>(page, pageSize, sortType, sortby, criteria);
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
                return PSF.GetListWithEQSearchCriteriaCount<AdminTemplateAdmissionStatusReport_vw>(page, pageSize, sortType, sortby, criteria);
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
                return PSF.GetListWithEQSearchCriteriaCount<AdminTemplateStudBoardingReport_vw>(page, pageSize, sortType, sortby, criteria);
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
                return PSF.GetListWithEQSearchCriteriaCount<AdminTemplateEmailReport_vw>(page, pageSize, sortType, sortby, criteria);
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
                return PSF.GetListWithEQSearchCriteriaCount<AdminTemplateSMSReport_vw>(page, pageSize, sortType, sortby, criteria);
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
                return PSF.GetListWithEQSearchCriteriaCount<AdminTemplateUsersTypeReport_vw>(page, pageSize, sortType, sortby, criteria);
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
                return PSF.GetListWithEQSearchCriteriaCount<AdminTemplateAssess360MarkCount_vw>(page, pageSize, sortType, sortby, criteria);
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
                return PSF.GetListWithEQSearchCriteriaCount<AdminTemplateLoginHistory_vw>(page, pageSize, sortType, sortby, criteria);
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
                return PSF.GetListWithEQSearchCriteriaCount<AdminTemplateStaffGroupByDept_vw>(page, pageSize, sortType, sortby, criteria);
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
                return PSF.GetListWithEQSearchCriteriaCount<AdminTemplateBrowserWiseReport_vw>(page, pageSize, sortType, sortby, criteria);
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
                return PSF.GetListWithEQSearchCriteriaCount<AdminTemplateUsersReport_vw>(page, pageSize, sortType, sortby, criteria);
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
                return PSF.GetListWithEQSearchCriteriaCount<AdminTemplateStaffsGrpByDeptAndCampus_vw>(page, pageSize, sortType, sortby, criteria);
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
                return PSF.GetListWithEQSearchCriteriaCount<AdminTemplateAssess360GrpByCampusAndGrade_vw>(page, pageSize, sortType, sortby, criteria);
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
        //        return PSF.GetListWithSearchCriteriaCountWithNotEqStringProperty<AdminTemplateTransportReportByCampus_vw>(page, pageSize, sortType, sortby, criteria, ColumnName, Values);
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
                return PSF.GetListWithEQSearchCriteriaCount<AdminTemplateTransportReportByCampus_vw>(page, pageSize, sortType, sortby, criteria);
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
                return PSF.GetListWithEQSearchCriteriaCount<AdminTemplateStudentTemplate_vw>(page, pageSize, sortType, sortby, criteria);
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
                return PSF.GetListWithEQSearchCriteriaCount<AdminTemplateStudentsCountGroupByGrade_vw>(page, pageSize, sortType, sortby, criteria);
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
                return PSF.GetListWithEQSearchCriteriaCount<AdminTemplateStudCountGrpByCampusGradeSection_vw>(page, pageSize, sortType, sortby, criteria);
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
                return PSF.GetListWithEQSearchCriteriaCount<AdminTemplateCampusWiseIssueCount_vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
