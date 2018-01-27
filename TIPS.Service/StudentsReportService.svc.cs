using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.Component;
using TIPS.Entities;
using TIPS.Entities.ReportEntities;
using TIPS.Entities.StudentsReportEntities;

namespace TIPS.ServiceContract
{
    public class StudentsReportService : IStudentsReportSC
    {
        StudentsReportBC StdRpt = new StudentsReportBC();

        #region MIS Report
        public Dictionary<long, IList<MISReport_vw>> GetMISReport_vwListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return StdRpt.GetMISReport_vwListWithPaging(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<MISMailMaster>> GetMISMailMasterListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return StdRpt.GetMISMailMasterListWithPaging(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<MISMailStatus>> GetMISMailStatusListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return StdRpt.GetMISMailStatusListWithPaging(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<CampusEmailId>> GetCampusEmailId(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return StdRpt.GetCampusEmailId(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long SaveOrUpdateMISMailStatusDetails(MISMailStatus Status)
        {
            try
            {
                return StdRpt.SaveOrUpdateMISMailStatusDetails(Status);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public MISMailStatus GetMISMailStatusDetailsById(long Id)
        {
            try
            {
                StudentsReportService SRS = new StudentsReportService();
                if (Id > 0)
                {
                    return StdRpt.GetMISMailStatusDetailsById(Id);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<MISOverAllReport_vw>> GetMISOverAllReport_vwListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return StdRpt.GetMISOverAllReport_vwListWithPaging(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        #region MIS MAIL MASTER
        public long SaveOrUpdateMISMailMasterDetails(MISMailMaster MailMaster)
        {
            try
            {
                StdRpt.SaveOrUpdateMISMailMasterDetails(MailMaster);
                return MailMaster.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public MISMailMaster GetMISMailMasterDetailsById(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    return StdRpt.GetMISMailMasterDetailsById(Id);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public long GetDeleteMISMailMasterById(MISMailMaster MailMaster)
        {
            try
            {
                if (MailMaster.Id > 0)
                {
                    StdRpt.GetDeleteMISMailMasterrowById(MailMaster);
                    return MailMaster.Id;
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<CampusMaster>> GetCampusMasterListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return StdRpt.GetCampusMasterListWithPaging(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        #endregion

        #region DetailedAdmissionReport
        public Dictionary<long, IList<DetailedAdmissionReport_vw>> DetailedAdmissionReport_vwListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return StdRpt.DetailedAdmissionReport_vwListWithLikeAndExcactSerachCriteria(page, pageSize, sortby, sortType, exctcriteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        #region AdmissionStatusWiseReport
        public Dictionary<long, IList<AdmissionStatusReport_vw>> GetAdmissionStatusListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return StdRpt.GetAdmissionStatusListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        #region MIS Consolidate Report
        public Dictionary<long, IList<MISDateWiseReport_vw>> GetMISDateWiseReport_vwListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return StdRpt.GetMISDateWiseReport_vwListWithPaging(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        #region Check StudentReport Services
        public bool SendEmailFromWindowsService(string Campus)
        {
            bool ret = false;
            ret = StdRpt.SendEmailFromWindowsService(Campus);
            return ret;
        }
        public bool SendMISConsolidateReport()
        {
            bool ret = false;
            ret = StdRpt.SendMISConsolidateReport();
            return ret;
        }
        public bool SendAdmissionReport()
        {
            bool ret = false;
            ret = StdRpt.SendAdmissionReport();
            return ret;
        }
        #endregion

        #region PastStudentService
        public Dictionary<long, IList<PastStudentList_Vw>> GetPastStudentRecords_VwCountWithPagingAndCriteria(int? page, int rows, string sortBy, string sortType, Dictionary<string, object> searchCriteria)
        {
            try
            {
                return StdRpt.GetPastStudentRecords_VwCountWithPagingAndCriteria(page, rows, sortBy, sortType, searchCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion

        public Dictionary<long, IList<MISMonthlyReport_Vw>> GetMISMonthlyReport_vwListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return StdRpt.GetMISMonthlyReport_vwListWithPaging(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        #region PageHistoryReport by Karthy
        public long SaveOrUpdatePageHistory(PageHistory pageHistory)
        {
            try
            {
                StdRpt.SaveOrUpdatePageHistory(pageHistory);
                return pageHistory.PageHistory_Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }

        }
        public Dictionary<long, IList<PageHistoryReport>> GetPageHistoryReportListWithPagingAndCriteria(int? page, int? pagesize, string sortby, string sorttype, Dictionary<string, object> criteria)
        {
            try
            {
                return StdRpt.GetPageHistoryReportListWithPagingAndCriteria(page, pagesize, sortby, sorttype, criteria);
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region CampusWiseUsageModule
        public Dictionary<long, IList<CampusWiseUsageModule>> GetCampusWiseUsageModuleListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return StdRpt.GetCampusWiseUsageModuleListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateCampusWiseUsageModule(CampusWiseUsageModule CampusWiseUsageModule)
        {
            try
            {
                StdRpt.SaveOrUpdateCampusWiseUsageModule(CampusWiseUsageModule);
                return CampusWiseUsageModule.CampusWiseUsageModule_Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }

        }
        public CampusWiseUsageModule GetCampusWiseUsageModuleById(long Id)
        {
            try
            {
                return StdRpt.GetCampusWiseUsageModuleById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long SaveOrUpdatecampusWiseUsageModule(CampusWiseUsageModule campusWiseUsageModule)
        {
            try
            {
                StdRpt.SaveOrUpdatecampusWiseUsageModule(campusWiseUsageModule);
                return campusWiseUsageModule.CampusWiseUsageModule_Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }

        }
        public bool DeleteCampusWiseUsageModule(long Id)
        {
            try
            {
                StdRpt.DeleteCampusWiseUsageModule(Id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #endregion
        public Dictionary<long, IList<CampusWiseModuleUsageReport_vw>> GetCampusWiseModuleUsageReport_vwListWithPagingAndCriteria(int? page, int? pagesize, string sortby, string sorttype, Dictionary<string, object> criteria)
        {
            try
            {
                return StdRpt.GetCampusWiseModuleUsageReport_vwListWithPagingAndCriteria(page, pagesize, sortby, sorttype, criteria);
            }
            catch (Exception)
            {
                throw;
            }

        }
        #region CampusWiseUsageModule_vw
        public Dictionary<long, IList<CampusWiseUsageModule_vw>> GetCampusWiseUsageModule_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return StdRpt.GetCampusWiseUsageModule_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    
    }
}
