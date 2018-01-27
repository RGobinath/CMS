using PersistenceFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using TIPS.Entities.BioMetricsEntities;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TIPS.Entities.StaffManagementEntities;
using System.Globalization;

namespace TIPS.Component
{
    public class BioAttendanceBC
    {
        PSFAnotherDatabase AnotherDbPsfObj = null;
        StaffManagementBC staffMngmntBC = new StaffManagementBC();
        public BioAttendanceBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            AnotherDbPsfObj = new PSFAnotherDatabase(Assembly, ConfigurationManager.AppSettings["AttendanceConString"].ToString());
        }
        // PersistenceServiceFactory PSF = null;
        // public BioAttendanceBC()
        //{
        //    List<String> Assembly = new List<String>();
        //    Assembly.Add("TIPS.Entities");
        //    Assembly.Add("TIPS.Entities.Assess");
        //    Assembly.Add("TIPS.Entities.TicketingSystem");
        //    Assembly.Add("TIPS.Entities.TaskManagement");
        //    PSF = new PersistenceServiceFactory(Assembly);
        //}

        public Dictionary<long, IList<AttendanceLog_Vw>> GetAttendanceLogListWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AnotherDbPsfObj.GetListWithLikeSearchCriteriaCount<AttendanceLog_Vw>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Dictionary<long, IList<AttendanceGradeWise_Vw>> GetDailyReportGradeWiseListWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AnotherDbPsfObj.GetListWithExactSearchCriteriaCount<AttendanceGradeWise_Vw>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<Devices>> GetDevicesListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AnotherDbPsfObj.GetListWithExactSearchCriteriaCount<Devices>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<BioAttendanceStaffWise_Vw>> GetDailyReportStaffGradeWiseListWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AnotherDbPsfObj.GetListWithExactSearchCriteriaCount<BioAttendanceStaffWise_Vw>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<Employees>> GetEmployeesListWithLikeandEQSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> Exactcriteria, Dictionary<string, object> Likecriteria)
        {
            try
            {
                return AnotherDbPsfObj.GetListWithExactAndLikeSearchCriteriaWithCount<Employees>(page, pageSize, sortType, sortby, Exactcriteria, Likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StaffInOutSummary>> GetStaffInOutSummaryList(string BasetableName)
        {
            try
            {
                //if (!string.IsNullOrWhiteSpace(ReportDate))
                //{
                //    //IFormatProvider culture = new CultureInfo("en-US", true);
                //    //DateTime dateVal = DateTime.ParseExact(ReportDate, "yyyy-MM-dd", culture);
                //    //ReportDate = dateVal.ToString();

                //    string[] sa = ReportDate.Split('/');
                //    string strNew = sa[2] + "-" + sa[1] + "-" + sa[0] + " 00:00:00";
                //    ReportDate = strNew;
                //}
                return AnotherDbPsfObj.ExecuteStoredProcedurewithOptionalParametersByDictonary<StaffInOutSummary>("GetStaffInOutSummaryList",
                         new[] { new SqlParameter("BaseTableName", BasetableName)
                    });

            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Consolidate Report
        public Dictionary<long, IList<Staff_ConsolidateDeviceLogSummary_SP>> GetStaffConsolidateDeviceLogSummaryDetails(string spCampus, string spIdNumber, string spName, string spStaffType, string spStaffProgramme, string spNewstatus, string spAttendanceFromDate, string spAttendanceToDate)
        {
            try
            {
                DateTime spAttendanceFromDateTime = new DateTime();
                DateTime spAttendanceToDateTime = new DateTime();
                //DateTime DeviceLogDateTime = new DateTime();
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                //spAttendanceFromDateTime = DateTime.Parse(spAttendanceFromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                //spAttendanceToDateTime = DateTime.Parse(spAttendanceToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                spAttendanceFromDateTime = DateTime.ParseExact(spAttendanceFromDate, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                spAttendanceToDateTime = DateTime.ParseExact(spAttendanceToDate, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                DateTime today = new DateTime();
                string DeviceLogTableName = string.Empty;
                if (!string.IsNullOrEmpty(spAttendanceFromDate) && !string.IsNullOrEmpty(spAttendanceToDate))
                {
                    string[] AttArray = spAttendanceFromDate.Split('/');
                    DeviceLogTableName = "DeviceLogs_" + spAttendanceFromDateTime.Month + "_" + spAttendanceFromDateTime.Year;
                }
                else
                    DeviceLogTableName = "DeviceLogs_" + today.ToString("MM").Substring(1, 1) + "_" + today.ToString("yyyy");

                return AnotherDbPsfObj.ExecuteStoredProcedurewithOptionalParametersByDictonary<Staff_ConsolidateDeviceLogSummary_SP>("GetConsolidateDeviceLogSummaryList",
                       new[] {
                           //new SqlParameter("spAttendanceDateInString", stringAttendanceDate),
                           new SqlParameter("spDeviceLogTableName", DeviceLogTableName)
                           ,new SqlParameter("spCampus", spCampus)
                           ,new SqlParameter("spIdNumber", spIdNumber)
                           ,new SqlParameter("spName", spName)
                           ,new SqlParameter("spStaffCategoryForAttendane", spStaffType)
                           ,new SqlParameter("spStaffProgramme", spStaffProgramme)
                           ,new SqlParameter("spCurrentStatus",spNewstatus)
                           ,new SqlParameter("spAttendanceFromDate", spAttendanceFromDateTime)
                           ,new SqlParameter("spAttendanceToDate", spAttendanceToDateTime)
                    });
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public Dictionary<long, IList<Staff_AttendanceRegisterReport>> GetStaffConsolidateDeviceLogSummaryDetailsForResgister(string spCampus, string spIdNumber, string spName, string spStaffType, string spStaffProgramme, string spNewstatus, string spAttendanceFromDate, string spAttendanceToDate)
        {
            try
            {
                DateTime spAttendanceFromDateTime = new DateTime();
                DateTime spAttendanceToDateTime = new DateTime();
                //DateTime DeviceLogDateTime = new DateTime();
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                //spAttendanceFromDateTime = DateTime.Parse(spAttendanceFromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                //spAttendanceToDateTime = DateTime.Parse(spAttendanceToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                spAttendanceFromDateTime = DateTime.ParseExact(spAttendanceFromDate, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                spAttendanceToDateTime = DateTime.ParseExact(spAttendanceToDate, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                DateTime today = new DateTime();
                string DeviceLogTableName = string.Empty;
                if (!string.IsNullOrEmpty(spAttendanceFromDate) && !string.IsNullOrEmpty(spAttendanceToDate))
                {
                    string[] AttArray = spAttendanceFromDate.Split('/');
                    DeviceLogTableName = "DeviceLogs_" + spAttendanceFromDateTime.Month + "_" + spAttendanceFromDateTime.Year;
                }
                else
                    DeviceLogTableName = "DeviceLogs_" + today.ToString("MM").Substring(1, 1) + "_" + today.ToString("yyyy");

                return AnotherDbPsfObj.ExecuteStoredProcedurewithOptionalParametersByDictonary<Staff_AttendanceRegisterReport>("GetConsolidateDeviceLogSummaryListForRegister",
                       new[] {
                           //new SqlParameter("spAttendanceDateInString", stringAttendanceDate),
                           new SqlParameter("spDeviceLogTableName", DeviceLogTableName)
                           ,new SqlParameter("spCampus", spCampus)
                           ,new SqlParameter("spIdNumber", spIdNumber)
                           ,new SqlParameter("spName", spName)
                           ,new SqlParameter("spStaffCategoryForAttendane", spStaffType)
                           ,new SqlParameter("spStaffProgramme", spStaffProgramme)
                           ,new SqlParameter("spCurrentStatus",spNewstatus)
                           ,new SqlParameter("spAttendanceFromDate", spAttendanceFromDateTime)
                           ,new SqlParameter("spAttendanceToDate", spAttendanceToDateTime)
                    });
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        #endregion
        public Dictionary<long, IList<Staff_DeviceLogSummarySP>> GetStaffDeviceLogSummaryDetails(string spCampus, string spIdNumber, string spName, string spStaffType, string spAttendanceFromDate, string spAttendanceToDate)
        {
            try
            {
                DateTime spAttendanceFromDateTime = new DateTime();
                DateTime spAttendanceToDateTime = new DateTime();
                //DateTime DeviceLogDateTime = new DateTime();
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                spAttendanceFromDateTime = DateTime.Parse(spAttendanceFromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                spAttendanceToDateTime = DateTime.Parse(spAttendanceToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);

                DateTime today = new DateTime();
                string DeviceLogTableName = string.Empty;
                if (!string.IsNullOrEmpty(spAttendanceFromDate) && !string.IsNullOrEmpty(spAttendanceToDate))
                {
                    string[] AttArray = spAttendanceFromDate.Split('/');
                    DeviceLogTableName = "DeviceLogs_" + spAttendanceFromDateTime.Month + "_" + spAttendanceFromDateTime.Year;
                }
                else
                    DeviceLogTableName = "DeviceLogs_" + today.ToString("MM").Substring(1, 1) + "_" + today.ToString("yyyy");

                return AnotherDbPsfObj.ExecuteStoredProcedurewithOptionalParametersByDictonary<Staff_DeviceLogSummarySP>("GetDeviceLogSummaryList",
                       new[] {
                           //new SqlParameter("spAttendanceDateInString", stringAttendanceDate),
                           new SqlParameter("spDeviceLogTableName", DeviceLogTableName)
                           ,new SqlParameter("spCampus", spCampus)
                           ,new SqlParameter("spIdNumber", spIdNumber)
                           ,new SqlParameter("spName", spName)
                           ,new SqlParameter("spStaffType", spStaffType)
                           ,new SqlParameter("spAttendanceFromDate", spAttendanceFromDateTime)
                           ,new SqlParameter("spAttendanceToDate", spAttendanceToDateTime)
                    });
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }


        //#region Automatic Email to Reporting Managers
        ////public bool SendLateComersAndEarlyGoersListToReportingHeads()
        ////{
        ////    try
        ////    {
        ////        bool IsMailSent = false;
        ////        return IsMailSent;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        ////        throw ex;
        ////    }
        ////}
        //public string SendLateComersAndEarlyGoersListToReportingHeads1()
        //{
        //    string MailFor = string.Empty;
        //    DateTime todayDateAndTime = DateTime.Now;
        //    string stringAttendanceDate = todayDateAndTime.ToString("dd/MM/yyyy");
        //    string[] AttArray = stringAttendanceDate.Split('-');
        //    if (AttArray != null && AttArray.Length > 0)
        //        stringAttendanceDate = AttArray[0] + "/" + AttArray[1] + "/" + AttArray[2];
        //    return stringAttendanceDate;
        //}

        //public bool SendLateComersAndEarlyGoersListToReportingHeads()
        //{
        //    try
        //    {
        //        bool IsMailSent = false;
        //        bool IsMailLogged = false;
        //        string MailFor = string.Empty;
        //        DateTime todayDateAndTime = DateTime.Now;
        //        string stringAttendanceDate = todayDateAndTime.ToString("dd/MM/yyyy");
        //        string[] AttArray = stringAttendanceDate.Split('-');
        //        if (AttArray != null && AttArray.Length > 0)
        //            stringAttendanceDate = AttArray[0] + "/" + AttArray[1] + "/" + AttArray[2];
        //        DateTime requiredTime = new DateTime();
        //        string Attendance = string.Format("{0:dd/MM/yyyy}", stringAttendanceDate);
        //        IList<StaffDetailsView> MailSendStaffList = new List<StaffDetailsView>();
        //        IList<Staff_AttendanceReportConfiguration> Staff_AttendanceReportConfigurationList = new List<Staff_AttendanceReportConfiguration>();
        //        Staff_AttendanceReportConfigurationList = staffMngmntBC.GetStaff_AttendanceReportConfigurationList();
        //        if (Staff_AttendanceReportConfigurationList != null && Staff_AttendanceReportConfigurationList.Count > 0)
        //        {
        //            var CampusArray = (from u in Staff_AttendanceReportConfigurationList
        //                               select u.Campus).Distinct().ToArray();
        //            foreach (var CampusItem in CampusArray)
        //            {
        //                if (todayDateAndTime >= (Convert.ToDateTime(Attendance + " " + "09:00:00")))
        //                {

        //                    MailFor = "LateComers";
        //                    IsMailLogged = staffMngmntBC.IsMailLogged(CampusItem, "LateComers", stringAttendanceDate);
        //                    if (IsMailLogged == false)
        //                    {
        //                        requiredTime = Convert.ToDateTime(Attendance + " " + "09:00:00");
        //                        MailSendStaffList = GetNotReachedStaffDetailsList(CampusItem, stringAttendanceDate, requiredTime);
        //                        if (MailSendStaffList != null && MailSendStaffList.Count > 0)
        //                            IsMailSent = SendMailToReportingManagers(MailFor, MailSendStaffList);
        //                        if (IsMailSent == true)
        //                            staffMngmntBC.SaveIntoMailLog(CampusItem, MailFor, stringAttendanceDate);
        //                    }
        //                }
        //                if (todayDateAndTime >= (Convert.ToDateTime(Attendance + " " + "15:00:00")))
        //                {
        //                    MailFor = "EarlyGoers";
        //                    IsMailLogged = staffMngmntBC.IsMailLogged(CampusItem, MailFor, stringAttendanceDate);
        //                    if (IsMailLogged == false)
        //                    {
        //                        requiredTime = Convert.ToDateTime(Attendance + " " + "15:00:00");
        //                        MailSendStaffList = GetEarlyOutStaffDetailsList(CampusItem, stringAttendanceDate, requiredTime);
        //                        if (MailSendStaffList != null && MailSendStaffList.Count > 0)
        //                            IsMailSent = SendMailToReportingManagers(MailFor, MailSendStaffList);
        //                        if (IsMailSent == true)
        //                            staffMngmntBC.SaveIntoMailLog(CampusItem, MailFor, stringAttendanceDate);
        //                    }
        //                }
        //            }
        //        }
        //        return IsMailSent;
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}
        //private bool SendMailToReportingManagers(string MailFor, IList<StaffDetailsView> MailSendStaffList)
        //{
        //    try
        //    {
        //        bool IsMailSent = false;
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        Dictionary<long, IList<Staff_AttendanceReportConfiguration>> ConfigurationDictionaryData = staffMngmntBC.GetStaff_AttendanceReportConfigurationDetailsListWithPaging(0, 99999, string.Empty, string.Empty, criteria);
        //        var ConfigurationsGroupByList = ConfigurationDictionaryData.FirstOrDefault().Value
        //                           .GroupBy(ac => new
        //                           {
        //                               ac.Campus,
        //                               ac.Programme,
        //                               ac.Department,
        //                               ac.SubDepartment,
        //                               ac.Designation,
        //                               ac.ReportingDesignation
        //                           })
        //                           .Select(ac => new Staff_AttendanceReportConfiguration
        //                           {
        //                               Campus = ac.Key.Campus,
        //                               Programme = ac.Key.Programme,
        //                               Department = ac.Key.Department,
        //                               SubDepartment = ac.Key.SubDepartment,
        //                               Designation = ac.Key.Designation,
        //                               ReportingDesignation = ac.Key.ReportingDesignation
        //                           }).ToList();
        //        if (ConfigurationsGroupByList != null && ConfigurationsGroupByList.Count > 0)
        //        {

        //            foreach (Staff_AttendanceReportConfiguration ConfigItem in ConfigurationsGroupByList)
        //            {
        //                IList<StaffDetailsView> SortedStaffList = new List<StaffDetailsView>();
        //                SortedStaffList = (from u in MailSendStaffList
        //                                   where
        //                                    u.Campus == ConfigItem.Campus
        //                                   && u.Programme == ConfigItem.Programme
        //                                   && u.Department == ConfigItem.Department
        //                                   && u.SubDepartment == ConfigItem.SubDepartment
        //                                   && u.Designation == ConfigItem.Designation
        //                                   select u).Distinct().ToList();
        //                if (SortedStaffList != null && SortedStaffList.Count > 0)
        //                {
        //                    var ReportingManagersMailIds = GetReportingManagersEMailIdByStaff_AttendanceReportConfiguration(ConfigItem);
        //                    if (!string.IsNullOrEmpty(ConfigItem.Campus) && !string.IsNullOrEmpty(ConfigItem.Programme) && !string.IsNullOrEmpty(ConfigItem.Department) && !string.IsNullOrEmpty(ConfigItem.Designation))
        //                    {
        //                        IsMailSent = BuildEmailBodyWithContentAndSendEmail(SortedStaffList, ReportingManagersMailIds, MailFor, ConfigItem.Campus, ConfigItem.Department);
        //                    }
        //                }
        //            }
        //        }
        //        return IsMailSent;
        //    }
        //    catch (Exception ex)
        //    {
        //        //ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}
        //private bool BuildEmailBodyWithContentAndSendEmail(IList<StaffDetailsView> StaffDetailsList, string[] ReportingManagersMailIds, string MailFor, string Campus, string Department)
        //{
        //    try
        //    {
        //        AdmissionManagementBC admsnBC = new AdmissionManagementBC();
        //        string RecipientInfo = "", Subject = "", MailBody = "";
        //        MailBody = admsnBC.GetMailTemplateBody();
        //        RecipientInfo = "";
        //        if (MailFor == "LateComers")
        //            Subject = "Late comers ( or ) Absentees list @ 9.05Am";
        //        else if (MailFor == "EarlyGoers")
        //            Subject = "Early Goers before 3PM from the campus";
        //        else { }
        //        int StaffCount = 0;
        //        string Content = string.Empty;
        //        if (!string.IsNullOrEmpty(Department) && Department == "Academics")
        //        {
        //            Content = "<br /><table class='tftable' border='1'><tr><th>S.No</th><th>Staff Name</th><th>Staff Id Number</th><th>Programme</th><th>Department</th><th>Designation</th><th>Grade</th><th>Section</th><th>Subject</th></tr>";
        //            foreach (var StaffObjItem in StaffDetailsList)
        //            { StaffCount++; Content = Content + "<tr><td>" + StaffCount + "</td><td>" + StaffObjItem.Name + "</td><td>" + StaffObjItem.IdNumber + "</td><td>" + StaffObjItem.Programme + "</td><td>" + StaffObjItem.Department + "</td><td>" + StaffObjItem.Designation + "</td><td></td><td></td><td></td></tr>"; Campus = StaffObjItem.Campus; }
        //        }
        //        else if (!string.IsNullOrEmpty(Department) && Department != "Academics")
        //        {
        //            Content = "<br /><table class='tftable' border='1'><tr><th>S.No</th><th>Staff Name</th><th>Staff Id Number</th><th>Programme</th><th>Department</th><th>Designation</th></tr>";
        //            foreach (var StaffObjItem in StaffDetailsList)
        //            { StaffCount++; Content = Content + "<tr><td>" + StaffCount + "</td><td>" + StaffObjItem.Name + "</td><td>" + StaffObjItem.IdNumber + "</td><td>" + StaffObjItem.Programme + "</td><td>" + StaffObjItem.Department + "</td><td>" + StaffObjItem.Designation + "</td></tr>"; Campus = StaffObjItem.Campus; }
        //        }
        //        string BodyofMail = "";
        //        BodyofMail = BodyofMail + Content;
        //        bool IsSentMail = false;
        //        IsSentMail = staffMngmntBC.SendMailFunctionForBioMetric(BodyofMail, Campus, MailBody, Subject, RecipientInfo, ReportingManagersMailIds);
        //        return IsSentMail;
        //    }
        //    catch (Exception ex)
        //    {
        //        //ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}
        //private string[] GetReportingManagersEMailIdByStaff_AttendanceReportConfiguration(Staff_AttendanceReportConfiguration ConfigObj)
        //{
        //    try
        //    {
        //        IList<StaffDetailsView> ReportingManagersStaffsList = new List<StaffDetailsView>();
        //        ReportingManagersStaffsList = staffMngmntBC.GetStaffDetailsListByCampusProgrammeDepartmentAndDesignation(ConfigObj.Campus, ConfigObj.Programme, ConfigObj.Department, ConfigObj.ReportingDesignation);
        //        var ReportingManagersMailIds = (from u in ReportingManagersStaffsList
        //                                        where !string.IsNullOrEmpty(u.TempEmailId)
        //                                        select u.TempEmailId.Trim()).Distinct().ToArray();
        //        return ReportingManagersMailIds;
        //    }
        //    catch (Exception ex)
        //    {
        //        //ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}

        //private IList<Staff_DeviceLogSummarySP> GetStaffDetailsWhoseAreCome(string Campus, string stringAttendanceDate, DateTime? RequiredTime)
        //{
        //    try
        //    {
        //        IList<Staff_DeviceLogSummarySP> ReachedStaffDetails = new List<Staff_DeviceLogSummarySP>();
        //        Dictionary<long, IList<Staff_DeviceLogSummarySP>> TotaldeviceSummaryDetails = null;
        //        TotaldeviceSummaryDetails = GetStaffDeviceLogSummaryDetailsByAttendanceLogDate(stringAttendanceDate);
        //        if (TotaldeviceSummaryDetails != null && TotaldeviceSummaryDetails.FirstOrDefault().Key > 0)
        //        {
        //            ReachedStaffDetails = (from u in TotaldeviceSummaryDetails.FirstOrDefault().Value
        //                                   where u.LogInTime < RequiredTime.Value
        //                                   select u).Distinct().ToList();
        //        }
        //        return ReachedStaffDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        //ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}

        //private IList<Staff_DeviceLogSummarySP> GetStaffDetailsWhoseAreEarlyOut(string Campus, string stringAttendanceDate, DateTime? RequiredTime)
        //{
        //    try
        //    {
        //        IList<Staff_DeviceLogSummarySP> ReachedStaffDetails = new List<Staff_DeviceLogSummarySP>();
        //        Dictionary<long, IList<Staff_DeviceLogSummarySP>> TotaldeviceSummaryDetails = null;
        //        TotaldeviceSummaryDetails = GetStaffDeviceLogSummaryDetailsByAttendanceLogDate(stringAttendanceDate);
        //        if (TotaldeviceSummaryDetails != null && TotaldeviceSummaryDetails.FirstOrDefault().Key > 0)
        //        {
        //            ReachedStaffDetails = (from u in TotaldeviceSummaryDetails.FirstOrDefault().Value
        //                                   where u.LogOutTime < RequiredTime.Value
        //                                   select u).Distinct().ToList();
        //        }
        //        return ReachedStaffDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        //ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}
        //public IList<StaffDetailsView> GetNotReachedStaffDetailsList(string Campus, string stringAttendanceDate, DateTime? RequiredTime)
        //{
        //    try
        //    {
        //        IList<StaffDetailsView> StaffListWhoseAreNotReachedSchool = new List<StaffDetailsView>();
        //        IList<Staff_DeviceLogSummarySP> StaffDetailsWhoseAreCome = GetStaffDetailsWhoseAreCome(Campus, stringAttendanceDate, RequiredTime);
        //        if (StaffDetailsWhoseAreCome != null && StaffDetailsWhoseAreCome.Count > 0)
        //        {
        //            long[] PresentStaffsPreRegNum = new long[StaffDetailsWhoseAreCome.Count];
        //            PresentStaffsPreRegNum = (from u in StaffDetailsWhoseAreCome
        //                                      select u.PreRegNum).Distinct().ToArray();
        //            Dictionary<long, IList<StaffDetailsView>> StaffDetailsByCampus = GetStaffDetailsByCampus(Campus);
        //            if (StaffDetailsByCampus != null && StaffDetailsByCampus.FirstOrDefault().Key > 0)
        //            {
        //                long[] TotalStaffsPreRegNum = (from u in StaffDetailsByCampus.FirstOrDefault().Value
        //                                               where u.PreRegNum > 0
        //                                               select Convert.ToInt64(u.PreRegNum)).Distinct().ToArray();
        //                if (TotalStaffsPreRegNum != null && TotalStaffsPreRegNum.Length > 0 && PresentStaffsPreRegNum.Length > 0)
        //                {
        //                    var NotReachedStaffsPreRegNum = TotalStaffsPreRegNum.Except(PresentStaffsPreRegNum).Distinct().ToArray();
        //                    if (NotReachedStaffsPreRegNum != null && NotReachedStaffsPreRegNum.Length > 0)
        //                        StaffListWhoseAreNotReachedSchool = staffMngmntBC.GetStaffDetailsListByPreRegNum(NotReachedStaffsPreRegNum);
        //                }
        //            }
        //        }
        //        return StaffListWhoseAreNotReachedSchool;
        //    }
        //    catch (Exception ex)
        //    {
        //        //ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}

        //public IList<StaffDetailsView> GetEarlyOutStaffDetailsList(string Campus, string stringAttendanceDate, DateTime? RequiredTime)
        //{
        //    try
        //    {
        //        IList<StaffDetailsView> StaffListWhoseAreEarlyOut = new List<StaffDetailsView>();
        //        IList<Staff_DeviceLogSummarySP> StaffDeviceSummaryListWhoseAreEarlyOut = GetStaffDetailsWhoseAreEarlyOut(Campus, stringAttendanceDate, RequiredTime);
        //        if (StaffDeviceSummaryListWhoseAreEarlyOut != null && StaffDeviceSummaryListWhoseAreEarlyOut.Count > 0)
        //        {
        //            long[] PresentStaffsPreRegNum = new long[StaffDeviceSummaryListWhoseAreEarlyOut.Count];
        //            PresentStaffsPreRegNum = (from u in StaffDeviceSummaryListWhoseAreEarlyOut
        //                                      where u.PreRegNum > 0
        //                                      select u.PreRegNum).Distinct().ToArray();
        //            if (PresentStaffsPreRegNum != null && PresentStaffsPreRegNum.Length > 0)
        //                StaffListWhoseAreEarlyOut = staffMngmntBC.GetStaffDetailsListByPreRegNum(PresentStaffsPreRegNum);
        //        }
        //        return StaffListWhoseAreEarlyOut;
        //    }
        //    catch (Exception ex)
        //    {
        //        //ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}
        //private Dictionary<long, IList<StaffDetailsView>> GetStaffDetailsByCampus(string Campus)
        //{
        //    try
        //    {
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        criteria.Add("Campus", Campus);
        //        criteria.Add("Status", "Registered");
        //        return staffMngmntBC.GetStaffDetailsViewListWithPaging(null, 999999, string.Empty, string.Empty, criteria);
        //    }
        //    catch (Exception ex)
        //    {
        //        //ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}
        //public Dictionary<long, IList<Staff_DeviceLogSummarySP>> GetStaffDeviceLogSummaryDetailsByAttendanceLogDate(string stringAttendanceDate)
        //{
        //    try
        //    {
        //        DateTime today = new DateTime();
        //        string DeviceLogTableName = string.Empty;
        //        if (!string.IsNullOrEmpty(stringAttendanceDate))
        //        {
        //            string[] AttArray = stringAttendanceDate.Split('/');
        //            DeviceLogTableName = "DeviceLogs_" + AttArray[1].Substring(1, 1) + "_" + AttArray[2];
        //        }
        //        else
        //            DeviceLogTableName = "DeviceLogs_" + today.ToString("MM").Substring(1, 1) + "_" + today.ToString("yyyy");

        //        return AnotherDbPsfObj.ExecuteStoredProcedurewithOptionalParametersByDictonary<Staff_DeviceLogSummarySP>("GetDeviceLogSummaryList",
        //               new[] {
        //                   new SqlParameter("spAttendanceDateInString", stringAttendanceDate)
        //                   ,new SqlParameter("spDeviceLogTableName", DeviceLogTableName)
        //                   ,new SqlParameter("spCampus", string.Empty)
        //                   ,new SqlParameter("spIdNumber", string.Empty)
        //                   ,new SqlParameter("spName", string.Empty)
        //            });
        //    }
        //    catch (Exception ex)
        //    {
        //        //ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}
        //public Dictionary<long, IList<Staff_DeviceLogSummarySP>> GetStaffDeviceLogSummaryDetails(string stringAttendanceDate, string Campus, string IdNumber, string Name)
        //{
        //    try
        //    {
        //        DateTime today = new DateTime();
        //        string DeviceLogTableName = string.Empty;
        //        if (!string.IsNullOrEmpty(stringAttendanceDate))
        //        {
        //            string[] AttArray = stringAttendanceDate.Split('/');
        //            DeviceLogTableName = "DeviceLogs_" + AttArray[1].Substring(1, 1) + "_" + AttArray[2];
        //        }
        //        else
        //            DeviceLogTableName = "DeviceLogs_" + today.ToString("MM").Substring(1, 1) + "_" + today.ToString("yyyy");

        //        return AnotherDbPsfObj.ExecuteStoredProcedurewithOptionalParametersByDictonary<Staff_DeviceLogSummarySP>("GetDeviceLogSummaryList",
        //               new[] {
        //                   new SqlParameter("spAttendanceDateInString", stringAttendanceDate)
        //                   ,new SqlParameter("spDeviceLogTableName", DeviceLogTableName)
        //                   ,new SqlParameter("spCampus", Campus)
        //                   ,new SqlParameter("spIdNumber", IdNumber)
        //                   ,new SqlParameter("spName", Name)
        //            });
        //    }
        //    catch (Exception ex)
        //    {
        //        //ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}
        ////public bool GetStaffDeviceLogSummaryDetails1(string stringAttendanceDate, string Campus, string IdNumber, string Name)
        ////{
        ////    try
        ////    {
        ////        bool ret = false;
        ////        //DateTime today = new DateTime();
        ////        string DeviceLogTableName = string.Empty;
        ////        if (!string.IsNullOrEmpty(stringAttendanceDate))
        ////        {
        ////            //string[] AttArray = stringAttendanceDate.Split('/');
        ////            //DeviceLogTableName = "DeviceLogs_" + AttArray[1].Substring(1, 1) + "_" + AttArray[2];
        ////            //if (DeviceLogTableName == "DeviceLogs_6_2017")
        ////                ret = true;
        ////            //else
        ////            //    ret = false;
        ////        }
        ////        return ret;
        ////        //DateTime today = new DateTime();
        ////        //string DeviceLogTableName = string.Empty;
        ////        //if (!string.IsNullOrEmpty(stringAttendanceDate))
        ////        //{
        ////        //    string[] AttArray = stringAttendanceDate.Split('/');
        ////        //    DeviceLogTableName = "DeviceLogs_" + AttArray[1].Substring(1, 1) + "_" + AttArray[2];
        ////        //}
        ////        //else
        ////        //    DeviceLogTableName = "DeviceLogs_" + today.ToString("MM").Substring(1, 1) + "_" + today.ToString("yyyy");

        ////        //return AnotherDbPsfObj.ExecuteStoredProcedurewithOptionalParametersByDictonary<Staff_DeviceLogSummarySP>("GetDeviceLogSummaryList",
        ////        //       new[] {
        ////        //           new SqlParameter("spAttendanceDateInString", stringAttendanceDate)
        ////        //           ,new SqlParameter("spDeviceLogTableName", DeviceLogTableName)
        ////        //           ,new SqlParameter("spCampus", Campus)
        ////        //           ,new SqlParameter("spIdNumber", IdNumber)
        ////        //           ,new SqlParameter("spName", Name)
        ////        //    });
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        //ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        ////        throw ex;
        ////    }
        ////}

        //#endregion
        #region Staff Attendance Onduty Reports
        public Dictionary<long, IList<Staff_AttendanceOnduty_vw>> GetStaffAttendanceOnDutyViewListWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AnotherDbPsfObj.GetListWithExactSearchCriteriaCount<Staff_AttendanceOnduty_vw>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Staff Attendance Longleave and Regined Report
        public Dictionary<long, IList<Staff_AttendanceLongleaveAndResignedReport_vw>> GetStaffAttendanceLongLeaveAndReginedViewListWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AnotherDbPsfObj.GetListWithExactSearchCriteriaCount<Staff_AttendanceLongleaveAndResignedReport_vw>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        public Dictionary<long, IList<Staff_DeviceLogSummaryStatus_sp>> GetStaffDeviceLogStatusSummaryDetails(string spCampus, string spIdNumber, string spName, string spStaffType, string spAttendanceFromDate, string spAttendanceToDate)
        {
            try
            {
                DateTime spAttendanceFromDateTime = new DateTime();
                DateTime spAttendanceToDateTime = new DateTime();
                //DateTime DeviceLogDateTime = new DateTime();
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                //spAttendanceFromDateTime = DateTime.Parse(spAttendanceFromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                //spAttendanceToDateTime = DateTime.Parse(spAttendanceToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                spAttendanceFromDateTime = DateTime.ParseExact(spAttendanceFromDate, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                spAttendanceToDateTime = DateTime.ParseExact(spAttendanceToDate, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                DateTime today = new DateTime();
                string DeviceLogTableName = string.Empty;
                if (!string.IsNullOrEmpty(spAttendanceFromDate) && !string.IsNullOrEmpty(spAttendanceToDate))
                {
                    string[] AttArray = spAttendanceFromDate.Split('/');
                    DeviceLogTableName = "DeviceLogs_" + spAttendanceFromDateTime.Month + "_" + spAttendanceFromDateTime.Year;
                }
                else
                    DeviceLogTableName = "DeviceLogs_" + today.ToString("MM").Substring(1, 1) + "_" + today.ToString("yyyy");

                return AnotherDbPsfObj.ExecuteStoredProcedurewithOptionalParametersByDictonary<Staff_DeviceLogSummaryStatus_sp>("GetDeviceLogStatusSummaryList",
                       new[] {
                           //new SqlParameter("spAttendanceDateInString", stringAttendanceDate),
                           new SqlParameter("spDeviceLogTableName", DeviceLogTableName)
                           ,new SqlParameter("spCampus", spCampus)
                           ,new SqlParameter("spIdNumber", spIdNumber)
                           ,new SqlParameter("spName", spName)
                           ,new SqlParameter("spStaffType", spStaffType)
                           ,new SqlParameter("spAttendanceFromDate", spAttendanceFromDateTime)
                           ,new SqlParameter("spAttendanceToDate", spAttendanceToDateTime)
                    });
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
    }
}

