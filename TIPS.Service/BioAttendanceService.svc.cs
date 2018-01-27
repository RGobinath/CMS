using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.Component;
using TIPS.Entities.BioMetricsEntities;

namespace TIPS.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BioAttendanceService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select BioAttendanceService.svc or BioAttendanceService.svc.cs at the Solution Explorer and start debugging.
    public class BioAttendanceService : IBioAttendanceService
    {
        BioAttendanceBC bioMetricBc = new BioAttendanceBC();
        public void DoWork()
        {
        }
        public Dictionary<long, IList<AttendanceLog_Vw>> GetAttendanceLogListWithCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                
                return bioMetricBc.GetAttendanceLogListWithCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<AttendanceGradeWise_Vw>> GetDailyReportGradeWiseListWithCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return bioMetricBc.GetDailyReportGradeWiseListWithCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<Devices>> GetDevicesListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return bioMetricBc.GetDevicesListWithPagingAndCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<BioAttendanceStaffWise_Vw>> GetDailyReportStaffGradeWiseListWithCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return bioMetricBc.GetDailyReportStaffGradeWiseListWithCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<Employees>> GetEmployeesListWithLikeandEQSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> Exactcriteria, Dictionary<string, object> Likecriteria)
        {
            try
            {
                return bioMetricBc.GetEmployeesListWithLikeandEQSearch(page, pageSize, sortby, sortType, Exactcriteria, Likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<StaffInOutSummary>> GetStaffInOutSummaryList(string BasetableName)
        {
            try
            {
                BioAttendanceBC bioMetric = new BioAttendanceBC();
                return bioMetric.GetStaffInOutSummaryList(BasetableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Consolidate Report
        public Dictionary<long, IList<Staff_ConsolidateDeviceLogSummary_SP>> GetStaffConsolidateDeviceLogSummaryDetails(string spCampus, string spIdNumber, string spName, string spStaffType, string spStaffProgramme, string spNewStatus, string spAttendanceFromDate, string spAttendanceToDate)
        {
            try
            {
                BioAttendanceBC bioMetric = new BioAttendanceBC();
                return bioMetricBc.GetStaffConsolidateDeviceLogSummaryDetails(spCampus, spIdNumber, spName, spStaffType, spStaffProgramme, spNewStatus, spAttendanceFromDate, spAttendanceToDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Dictionary<long, IList<Staff_DeviceLogSummarySP>> GetStaffDeviceLogSummaryDetails(string spCampus, string spIdNumber, string spName, string spStaffType, string spAttendanceFromDate, string spAttendanceToDate)
        {
            try
            {
                BioAttendanceBC bioMetric = new BioAttendanceBC();
                return bioMetricBc.GetStaffDeviceLogSummaryDetails(spCampus, spIdNumber, spName, spStaffType, spAttendanceFromDate, spAttendanceToDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Dictionary<long, IList<Staff_AttendanceRegisterReport>> GetStaffConsolidateDeviceLogSummaryDetailsForResgister(string spCampus, string spIdNumber, string spName, string spStaffType, string spStaffProgramme, string spNewStatus, string spAttendanceFromDate, string spAttendanceToDate)
        {
            try
            {
                BioAttendanceBC bioMetric = new BioAttendanceBC();
                return bioMetricBc.GetStaffConsolidateDeviceLogSummaryDetailsForResgister(spCampus, spIdNumber, spName, spStaffType, spStaffProgramme, spNewStatus, spAttendanceFromDate, spAttendanceToDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Staff Attendnace Onduty Report
        public Dictionary<long, IList<Staff_AttendanceOnduty_vw>> GetStaffAttendanceOnDutyViewListWithCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                BioAttendanceBC bioMetric = new BioAttendanceBC();
                return bioMetric.GetStaffAttendanceOnDutyViewListWithCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion

        #region Staff Attendance Longleave and Regined Report
        public Dictionary<long, IList<Staff_AttendanceLongleaveAndResignedReport_vw>> GetStaffAttendanceLongleaveAndReginedViewListWithCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                BioAttendanceBC bioMetric = new BioAttendanceBC();
                return bioMetric.GetStaffAttendanceLongLeaveAndReginedViewListWithCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        public Dictionary<long, IList<Staff_DeviceLogSummaryStatus_sp>> GetStaffDeviceLogStatusSummaryDetails(string spCampus, string spIdNumber, string spName, string spStaffType, string spAttendanceFromDate, string spAttendanceToDate)
        {
            try
            {
                BioAttendanceBC bioMetric = new BioAttendanceBC();
                return bioMetricBc.GetStaffDeviceLogStatusSummaryDetails(spCampus, spIdNumber, spName, spStaffType, spAttendanceFromDate, spAttendanceToDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
