using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Collections;
using System.Configuration;
using TIPS.Service;
using TIPS.Entities.StaffManagementEntities;
using System.Globalization;
using TIPS.Entities.BioMetricsEntities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TIPS.ServiceContract;
using TIPS.Entities;
using TIPS.Component;

namespace CMS.Controllers
{
    public class BioMetricAttendanceController : BaseController
    {
        //
        // GET: /BioMetricUserDetails/

        BioAttendanceService bioMetricSvc = new BioAttendanceService();
       

        public ActionResult BioAttendance()
        {
            try
            {
                string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "");
                throw ex;
            }

        }

        public ActionResult AttendanceLogJqGrid(string ExportType, string EmployeeName, string CompanyCode, string AttendanceDate, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                BioAttendanceService bioMetric = new BioAttendanceService();
                sord = sord == "desc" ? "Desc" : "Asc";
                //UserServiceReference.UserServiceSCClient client = new UserServiceReference.UserServiceSCClient();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(EmployeeName)) { criteria.Add("EmployeeName", EmployeeName); }
                if (!string.IsNullOrEmpty(CompanyCode) && Convert.ToInt64(CompanyCode) > 0)
                { criteria.Add("CompanyId", Convert.ToInt64(CompanyCode)); }
                if (!string.IsNullOrEmpty(AttendanceDate))
                {
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    var Datevalue = DateTime.Parse(AttendanceDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    criteria.Add("AttendanceDate", Datevalue);
                }
                Dictionary<long, IList<AttendanceLog_Vw>> BioAttList = bioMetric.GetAttendanceLogListWithCriteria(page - 1, rows, sidx, sord, criteria);
                //IList<UserInfo> UserInfoList = client.UserInfo(Name, rows, sidx, sord, page - 1).ToList<UserInfo>();
                if (BioAttList != null && BioAttList.Count > 0)
                {
                    if (ExportType == "Excel")
                    {
                        base.ExptToXL(BioAttList.First().Value.ToList(), "AttendanceLogDetails", (items => new
                        {
                            items.EmployeeName,
                            items.Gender,
                            CampusName = items.CompanyFName,
                            items.DepartmentFName,
                            items.Designation,
                            items.EmployementType,
                            items.Status,
                            AttendanceDate = items.AttendanceDate.ToString("dd'/'MM'/'yyyy"),
                            items.AttendanceStatus
                        }));
                        return new EmptyResult();

                    }
                    else
                    {
                        long totalrecords = BioAttList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)5);
                        var AttLst = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in BioAttList.FirstOrDefault().Value
                                    select new
                                 {
                                     cell = new string[] 
                                         {
                                             items.Id.ToString(),
                                             items.EmployeeName,
                                             items.Gender,
                                             items.CompanyFName,
                                             items.DepartmentFName,
                                             items.Designation,
                                             items.EmployementType,
                                             items.Status,
                                             items.AttendanceDate.ToString("dd'/'MM'/'yyyy"),
                                             items.AttendanceStatus
                                         }
                                 })
                        };
                        return Json(AttLst, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult DailyReportGradeWise()
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("IsRealTime", 1);
                Dictionary<long, IList<Devices>> DevicesList = bioMetricSvc.GetDevicesListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                ViewBag.devices = DevicesList.First().Value;
                string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "");
                throw ex;
            }
        }

        public ActionResult DailyReportGradeWiseJqGrid(string ExportType, string Grade, long deviceId, string frmDate, string toDate, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("DeviceId", deviceId);
                if (!string.IsNullOrEmpty(frmDate) && !string.IsNullOrEmpty(toDate))
                {
                    frmDate = frmDate.Trim();
                    toDate = toDate.Trim();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    //string To = string.Format("{0:MM/dd/yyyy}", DateTime.Now);
                    DateTime[] fromto = new DateTime[2];
                    fromto[0] = DateTime.Parse(frmDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    fromto[1] = DateTime.Parse(toDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);// Convert.ToDateTime(To + " " + "23:59:59");
                    criteria.Add("LogDate", fromto);
                }
                Dictionary<long, IList<AttendanceGradeWise_Vw>> BioAttList = bioMetricSvc.GetDailyReportGradeWiseListWithCriteria(page - 1, rows, sidx, sord, criteria);
                if (BioAttList != null && BioAttList.Count > 0)
                {
                    if (Grade == "IX")
                    {
                        if (ExportType == "Excel")
                        {
                            base.ExptToXL(BioAttList.First().Value.ToList(), "AttendanceLogDetails", (items => new
                            {

                                items.DeviceFName,
                                LogDate = items.LogDate.ToString("dd'/'MM'/'yyyy"),
                                P1 = items.Period1 = items.P1 > 0 ? "P1In(" + items.P1In + ")" : "-",
                                P2 = items.Period2 = items.P2 > 0 ? "P2In(" + items.P2In + ")" : "-",
                                P3 = items.Period3 = items.P3 > 0 ? "P3In(" + items.P3In + ")" : "-",
                                P4 = items.Period4 = items.P4 > 0 ? "P4In(" + items.P4In + ")" : "-",
                                P5 = items.Period5 = items.P5 > 0 ? "P5In(" + items.P5In + ")" : "-",
                                P6 = items.Period6 = items.P6 > 0 ? "P6In(" + items.P6In + ")" : "-",
                                P7 = items.Period7 = items.P7 > 0 ? "P7In(" + items.P7In + ")" : "-",
                                P8 = items.Period8 = items.P8 > 0 ? "P8In(" + items.P8In + ")" : "-",
                                P9 = items.Period9 = items.P9 > 0 ? "P9In(" + items.P9In + ")" : "-",
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalrecords = BioAttList.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)5);
                            var AttLst = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,

                                rows = (from items in BioAttList.FirstOrDefault().Value
                                        select new
                                        {
                                            cell = new string[] 
                                         {
                                             items.Id.ToString(),
                                             items.DeviceFName,
                                items.LogDate.ToString("dd'/'MM'/'yyyy"),
                                items.Period1=items.P1>0?"P1In("+items.P1In+")":"-",
                                items.Period2=items.P2>0?"P2In("+items.P2In+")":"-",
                                items.Period3=items.P3>0?"P3In("+items.P3In+")":"-",
                                items.Period4=items.P4>0?"P4In("+items.P4In+")":"-",
                                items.Period5=items.P5>0?"P5In("+items.P5In+")":"-",
                                items.Period6=items.P6>0?"P6In("+items.P6In+")":"-",
                                items.Period7=items.P7>0?"P7In("+items.P7In+")":"-",
                                items.Period8=items.P8>0?"P8In("+items.P8In+")":"-",
                                items.Period9=items.P9>0?"P9In("+items.P9In+")":"-",
                                         }
                                        })
                            };
                            return Json(AttLst, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        if (ExportType == "Excel")
                        {
                            base.ExptToXL(BioAttList.First().Value.ToList(), "AttendanceLogDetails", (items => new
                            {
                                items.DeviceFName,
                                LogDate = items.LogDate.ToString("dd'/'MM'/'yyyy"),
                                P1 = items.Period1 = items.P1 > 0 ? "P1In(" + items.P1In + ")" : "-",
                                P2 = items.Period2 = items.P2 > 0 ? "P2In(" + items.P2In + ")" : "-",
                                P3 = items.Period3 = items.P3 > 0 ? "P3In(" + items.P3In + ")" : "-",
                                P4 = items.Period4 = items.P4 > 0 ? "P4In(" + items.P4In + ")" : "-",
                                P5 = items.Period5 = items.P5 > 0 ? "P5In(" + items.P5In + ")" : "-",
                                P6 = items.Period6 = items.P6 > 0 ? "P6In(" + items.P6In + ")" : "-",
                                P7 = items.Period7 = items.P7 > 0 ? "P7In(" + items.P7In + ")" : "-",
                            }));
                            return new EmptyResult();

                        }
                        else
                        {
                            long totalrecords = BioAttList.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)5);
                            var AttLst = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,

                                rows = (from items in BioAttList.FirstOrDefault().Value
                                        select new
                                        {
                                            cell = new string[] 
                                         {
                                             items.Id.ToString(),
                                             items.DeviceFName,
                                items.LogDate.ToString("dd'/'MM'/'yyyy"),
                                 items.Period1=items.P1>0?"P1In("+items.P1In+")":"-",
                                items.Period2=items.P2>0?"P2In("+items.P2In+")":"-",
                                items.Period3=items.P3>0?"P3In("+items.P3In+")":"-",
                                items.Period4=items.P4>0?"P4In("+items.P4In+")":"-",
                                items.Period5=items.P5>0?"P5In("+items.P5In+")":"-",
                                items.Period6=items.P6>0?"P6In("+items.P6In+")":"-",
                                items.Period7=items.P7>0?"P7In("+items.P7In+")":"-",
                                         }
                                        })
                            };
                            return Json(AttLst, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult DailyReportStaffGradeWise()
        {
            try
            {
                string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "");
                throw ex;
            }
        }

        public ActionResult DailyReportStaffGradeWiseJqGrid(string ExportType, long? userId, string Category, string frmDate, string toDate, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (userId != null && userId > 0) { criteria.Add("UserId", userId ?? 0); }
                if (!string.IsNullOrEmpty(Category))
                { //criteria.Add("UserId", userId); 
                }
                //criteria.Add("",Category);
                if (!string.IsNullOrEmpty(frmDate) && !string.IsNullOrEmpty(toDate))
                {
                    frmDate = frmDate.Trim();
                    toDate = toDate.Trim();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    //string To = string.Format("{0:MM/dd/yyyy}", DateTime.Now);
                    DateTime[] fromto = new DateTime[2];
                    fromto[0] = DateTime.Parse(frmDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    fromto[1] = DateTime.Parse(toDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);// Convert.ToDateTime(To + " " + "23:59:59");
                    criteria.Add("LogDate", fromto);
                }
                Dictionary<long, IList<BioAttendanceStaffWise_Vw>> BioAttStaffList = bioMetricSvc.GetDailyReportStaffGradeWiseListWithCriteria(page - 1, rows, sidx, sord, criteria);
                if (BioAttStaffList != null && BioAttStaffList.Count > 0)
                {
                    if (ExportType == "Excel")
                    {
                        base.ExptToXL(BioAttStaffList.First().Value.ToList(), "StaffAttendanceLogDetails", (items => new
                        {
                            items.UserId,
                            items.EmployeeName,
                            LogDate = items.LogDate.ToString("dd'/'MM'/'yyyy"),
                            P1 = items.Period1 = items.P1 > 0 ? items.P1Grade + "(" + items.P1In + ")" : "-",
                            P2 = items.Period2 = items.P2 > 0 ? items.P2Grade + "(" + items.P2In + ")" : "-",
                            P3 = items.Period3 = items.P3 > 0 ? items.P3Grade + "(" + items.P3In + ")" : "-",
                            P4 = items.Period4 = items.P4 > 0 ? items.P4Grade + "(" + items.P4In + ")" : "-",
                            P5 = items.Period5 = items.P5 > 0 ? items.P5Grade + "(" + items.P5In + ")" : "-",
                            P6 = items.Period6 = items.P6 > 0 ? items.P6Grade + "(" + items.P6In + ")" : "-",
                            P7 = items.Period7 = items.P7 > 0 ? items.P7Grade + "(" + items.P7In + ")" : "-",
                            P8 = items.Period8 = items.P8 > 0 ? items.P8Grade + "(" + items.P8In + ")" : "-",
                            P9 = items.Period9 = items.P9 > 0 ? items.P9Grade + "(" + items.P9In + ")" : "-",
                        }));
                        return new EmptyResult();
                    }
                    else
                    {
                        long totalrecords = BioAttStaffList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)5);
                        var AttLst = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in BioAttStaffList.FirstOrDefault().Value
                                    select new
                                    {
                                        cell = new string[] 
                                         {
                                             items.Id.ToString(),
                                             items.UserId.ToString(),
                                             items.EmployeeName,
                                             items.LogDate.ToString("dd'/'MM'/'yyyy"),
                                             items.Period1=items.P1>0?items.P1Grade+"("+items.P1In+")":"-",
                                             items.Period2=items.P2>0?items.P2Grade+"("+items.P2In+")":"-",
                                             items.Period3=items.P3>0?items.P3Grade+"("+items.P3In+")":"-",
                                             items.Period4=items.P4>0?items.P4Grade+"("+items.P4In+")":"-",
                                             items.Period5=items.P5>0?items.P5Grade+"("+items.P5In+")":"-",
                                             items.Period6=items.P6>0?items.P6Grade+"("+items.P6In+")":"-",
                                             items.Period7=items.P7>0?items.P7Grade+"("+items.P7In+")":"-",
                                             items.Period8=items.P8>0?items.P8Grade+"("+items.P8In+")":"-",
                                             items.Period9=items.P9>0?items.P9Grade+"("+items.P9In+")":"-",
                                         }
                                    })
                        };
                        return Json(AttLst, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult EmployeePopup()
        {
            return PartialView();
        }

        public ActionResult GetBioEmployeeDetails(string StaffName, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                UserService us = new UserService();
                Dictionary<string, object> EQcriteria = new Dictionary<string, object>();
                Dictionary<string, object> Likecriteria = new Dictionary<string, object>();
                Likecriteria.Add("EmployeeName", StaffName);
                Dictionary<long, IList<Employees>> EmployeeesList = bioMetricSvc.GetEmployeesListWithLikeandEQSearch(0, 9999, string.Empty, string.Empty, EQcriteria, Likecriteria);
                if (EmployeeesList != null && EmployeeesList.Count > 0)
                {
                    long totalRecords = EmployeeesList.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows = (
                        from items in EmployeeesList.FirstOrDefault().Value
                        select new
                        {
                            cell = new string[] {
                           items.EmployeeId.ToString(),
                           items.EmployeeName,
                           items.EmployeeCode
                            }
                        })
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, "Assess360Policy"); }
        }

        public ActionResult StaffIOSummary()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw ex;
            }
        }

        public ActionResult GetStaffInOutSummaryJqGrid(string date, string campus, string staffName, string staffId, string Type, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("logOut", "Account");
                else
                {

                    BioAttendanceService bioMetric = new BioAttendanceService();
                    Dictionary<long, IList<StaffInOutSummary>> StaffInOutSummaryList = bioMetric.GetStaffInOutSummaryList("DeviceLogs_");
                    if (Type != "Excel")
                    {
                        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                        DateTime attdate = new DateTime();
                        if (!string.IsNullOrEmpty(date))
                            attdate = DateTime.Parse(date, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        if (StaffInOutSummaryList != null && StaffInOutSummaryList.Count > 0 && StaffInOutSummaryList.First().Key > 0)
                        {
                            long totalRecords = StaffInOutSummaryList.First().Key;
                            int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                            var jsonData = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalRecords,
                                rows = (from items in StaffInOutSummaryList.First().Value
                                        where (string.IsNullOrEmpty(campus) || items.Campus == campus) &&
                                        (string.IsNullOrEmpty(staffId) || items.IdNumber == staffId) &&
                                        (string.IsNullOrEmpty(staffName) || items.Name == staffName) &&
                                        (string.IsNullOrEmpty(date) || items.LogDate == attdate)
                                        select new
                                        {
                                            cell = new string[]{
                                                items.Id.ToString(),
                                        items.Campus,
                                        items.Name.ToString(),
                                        items.IdNumber,
                                        items.Designation,
                                        items.EmployeeID.ToString(),
                                        items.EmployeeName,
                                        items.EmployeeIdNumber,
                                        items.DeviceId.ToString(),
                                        items.UserId,
                                        items.LogDate.ToString("dd'/'MM'/'yyyy"),
                                        items.InTime.ToShortTimeString() !="00:00" ? items.InTime.ToShortTimeString() : " ",
                                        items.OutTime.ToShortTimeString() !="00:00" ? items.OutTime.ToShortTimeString() : " "
                                            }
                                        })
                            };
                            return Json(jsonData, JsonRequestBehavior.AllowGet);
                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //List<string> lstHeader = new List<string>() { "Id", "Staff ID", "Name", "Campus", "Attendance Date", "Start Time From", "Start Time", "EndTime", "EndTimeTo", "LogIn", "LogOut",  };

                        //var ReportList = StaffInOutSummaryList.First().Value.ToList();

                        //base.NewExportToExcel(ReportList, "Staff_Attendance_Report", (items => new
                        //{
                        //    items.Id,
                        //    items.StaffDetails_Id,
                        //    items.Name,
                        //    items.Campus,
                        //    AttendanceDate = items.AttendanceDate.ToShortDateString(),
                        //    StartTimefrom = items.StartTimeFrom.ToShortTimeString(),
                        //    StartTime = items.StartTime.ToShortTimeString(),
                        //    EndTime = items.EndTime.ToShortTimeString(),
                        //    EndTimeTo = items.EndTimeTo.ToShortTimeString(),
                        //    LogIn = items.LogIn.ToShortTimeString() != "00:00" ? items.LogIn.ToShortTimeString() : " ",
                        //    LogOut = items.LogOut.ToShortTimeString() != "00:00" ? items.LogOut.ToShortTimeString() : " ",
                        //    items.WorkedHour,
                        //    items.WorkedMinute,
                        //    items.PunchLog
                        //}), lstHeader);
                        return new EmptyResult();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }


        public DateTime[] getFromTodateByMonthAndYear(string MonthAndYear)
        {
            string[] MonthAndYearArray = new string[2];
            int SearchMonth = 0;
            int SearchYear = 0;
            if (!string.IsNullOrEmpty(MonthAndYear))
            {
                MonthAndYearArray = MonthAndYear.Split('-');
                if (!string.IsNullOrEmpty(MonthAndYearArray[0]) && !string.IsNullOrEmpty(MonthAndYearArray[1]))
                {
                    SearchMonth = Convert.ToInt32(MonthAndYearArray[0]);
                    SearchYear = Convert.ToInt32(MonthAndYearArray[1]);
                }
            }
            DateTime baseDate = DateTime.Now;
            var today = baseDate;
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime[] fromto = new DateTime[2];
            switch (MonthAndYearArray[0])
            {
                case "01":
                    DateTime janFirst = new DateTime(SearchYear, 1, 1);
                    DateTime janLast = new DateTime(SearchYear, 1, 31);
                    string janfrom = string.Format("{0:MM/dd/yyyy}", janFirst);
                    string janto = string.Format("{0:MM/dd/yyyy}", janLast);
                    fromDate = Convert.ToDateTime(janfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(janto + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;

                case "02":
                    DateTime febFirst = new DateTime(SearchYear, 2, 1);
                    DateTime febLast = new DateTime(SearchYear, 2, 28);
                    string febfrom = string.Format("{0:MM/dd/yyyy}", febFirst);
                    string febto = string.Format("{0:MM/dd/yyyy}", febLast);
                    fromDate = Convert.ToDateTime(febfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(febto + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;
                case "03":
                    DateTime marFirst = new DateTime(SearchYear, 3, 1);
                    DateTime marLast = new DateTime(SearchYear, 3, 31);
                    string marfrom = string.Format("{0:MM/dd/yyyy}", marFirst);
                    string marto = string.Format("{0:MM/dd/yyyy}", marLast);
                    fromDate = Convert.ToDateTime(marfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(marto + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;
                case "04":
                    DateTime aprilFirst = new DateTime(SearchYear, 4, 1);
                    DateTime aprilLast = new DateTime(SearchYear, 4, 30);
                    string aprilfrom = string.Format("{0:MM/dd/yyyy}", aprilFirst);
                    string aprilto = string.Format("{0:MM/dd/yyyy}", aprilLast);
                    fromDate = Convert.ToDateTime(aprilfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(aprilto + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;
                case "05":
                    DateTime mayFirst = new DateTime(SearchYear, 5, 1);
                    DateTime mayLast = new DateTime(SearchYear, 5, 31);
                    string mayfrom = string.Format("{0:MM/dd/yyyy}", mayFirst);
                    string mayto = string.Format("{0:MM/dd/yyyy}", mayLast);
                    fromDate = Convert.ToDateTime(mayfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(mayto + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;
                case "06":
                    DateTime juneFirst = new DateTime(SearchYear, 6, 1);
                    DateTime juneLast = new DateTime(SearchYear, 6, 30);
                    string junefrom = string.Format("{0:MM/dd/yyyy}", juneFirst);
                    string juneto1 = string.Format("{0:MM/dd/yyyy}", juneLast);
                    fromDate = Convert.ToDateTime(junefrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(juneto1 + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;
                case "07":
                    DateTime julyFirst = new DateTime(SearchYear, 7, 1);
                    DateTime julyLast = new DateTime(SearchYear, 7, 31);
                    string julyfrom = string.Format("{0:MM/dd/yyyy}", julyFirst);
                    string julyto = string.Format("{0:MM/dd/yyyy}", julyLast);
                    fromDate = Convert.ToDateTime(julyfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(julyto + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;
                case "08":
                    DateTime augFirst = new DateTime(SearchYear, 8, 1);
                    DateTime augLast = new DateTime(SearchYear, 8, 30);
                    string augfrom = string.Format("{0:MM/dd/yyyy}", augFirst);
                    string augto = string.Format("{0:MM/dd/yyyy}", augLast);
                    fromDate = Convert.ToDateTime(augfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(augto + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;
                case "09":
                    DateTime sepFirst = new DateTime(SearchYear, 9, 1);
                    DateTime sepLast = new DateTime(SearchYear, 9, 31);
                    string sepfrom = string.Format("{0:MM/dd/yyyy}", sepFirst);
                    string septo = string.Format("{0:MM/dd/yyyy}", sepLast);
                    fromDate = Convert.ToDateTime(sepfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(septo + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;
                case "10":
                    DateTime octFirst = new DateTime(SearchYear, 10, 1);
                    DateTime octLast = new DateTime(SearchYear, 10, 30);
                    string octfrom = string.Format("{0:MM/dd/yyyy}", octFirst);
                    string octto = string.Format("{0:MM/dd/yyyy}", octLast);
                    fromDate = Convert.ToDateTime(octfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(octto + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;
                case "11":
                    DateTime novFirst = new DateTime(SearchYear, 11, 1);
                    DateTime novLast = new DateTime(SearchYear, 11, 31);
                    string novfrom = string.Format("{0:MM/dd/yyyy}", novFirst);
                    string novto = string.Format("{0:MM/dd/yyyy}", novLast);
                    fromDate = Convert.ToDateTime(novfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(novto + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;
                case "12":
                    DateTime decFirst = new DateTime(SearchYear, 6, 1);
                    DateTime decLast = new DateTime(SearchYear, 6, 30);
                    string decfrom = string.Format("{0:MM/dd/yyyy}", decFirst);
                    string decto = string.Format("{0:MM/dd/yyyy}", decLast);
                    fromDate = Convert.ToDateTime(decfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(decto + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;
            }
            return fromto;
        }
        public ActionResult StaffInOutSummaryJqGrid(string Campus, string StaffType, string IdNumber, string StaffName, string AttendanceFromDate, string AttendanceToDate, string MonthYear, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                BioAttendanceService bioMetric = new BioAttendanceService();
                
               
                DateTime StartDate = new DateTime();
                DateTime ToDate = new DateTime();
                if (!string.IsNullOrEmpty(MonthYear))
                {
                    DateTime[] fromto = new DateTime[2];
                    fromto = getFromTodateByMonthAndYear(MonthYear);
                    AttendanceFromDate = fromto[0].ToString("dd/MM/yyyy");
                    AttendanceToDate = fromto[1].ToString("dd/MM/yyyy");
                }
                if (!string.IsNullOrEmpty(AttendanceFromDate) && !string.IsNullOrEmpty(AttendanceToDate))
                {
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    StartDate = DateTime.Parse(AttendanceFromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    ToDate = DateTime.Parse(AttendanceToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                }
                var SearchDateList = Enumerable.Range(0, 1 + ToDate.Subtract(StartDate).Days).Select(offset => StartDate.AddDays(offset)).ToArray();
                var MonthAndYearGroup = (from p in SearchDateList
                                         group p by new { month = p.Date.Month, year = p.Date.Year } into d
                                         select d).ToList();
                List<Staff_DeviceLogSummarySP> TotalLogCollections = new List<Staff_DeviceLogSummarySP>();
                IList<Staff_DeviceLogSummarySP> DeviceLogsList = new List<Staff_DeviceLogSummarySP>();
                for (int i = 0; i < MonthAndYearGroup.Count; i++)
                {
                    var SingleMonthYear = (from u in MonthAndYearGroup[i] select u).ToList();
                    Dictionary<long, IList<Staff_DeviceLogSummarySP>> BioMetricAttendanceDetails = null;
                    BioMetricAttendanceDetails = bioMetric.GetStaffDeviceLogSummaryDetails(Campus, IdNumber, StaffName, StaffType, SingleMonthYear[0].Date.ToString("dd/MM/yyyy 00:00:01"), SingleMonthYear[SingleMonthYear.Count - 1].Date.ToString("dd/MM/yyyy 23:59:59"));
                    if (BioMetricAttendanceDetails != null && BioMetricAttendanceDetails.FirstOrDefault().Key > 0)
                    {
                        var SinleMonthLogList = (from u in BioMetricAttendanceDetails.FirstOrDefault().Value
                                                 where u.AttendanceDate != null
                                                 select u).ToList();
                        TotalLogCollections.AddRange(SinleMonthLogList);
                    }
                }
                if (ExportType == "Excel")
                {
                    string ExcelFileName = "Staff_IO_Summary_On_" + DateTime.Now.ToString("dd/MM/yyyy");
                    //var List = DeviceLogsList.First().Value.ToList();
                    ExptToXL(TotalLogCollections, ExcelFileName, (items => new
                    {
                        items.Campus,
                        items.Name,
                        items.IdNumber,
                        AttendanceDate = items.AttendanceDate != null ? items.AttendanceDate.Value.ToString("dd/MM/yyyy") : string.Empty,
                        Group = items.Programme,
                        items.Designation,
                        LogInTime = items.LogInTime != null ? items.LogInTime.Value.ToString("HH:mm:ss") : "00:00:00",
                        LogOutTime = items.LogOutTime != null ? items.LogOutTime.Value.ToString("HH:mm:ss") : "00:00:00",
                        items.WorkingHours
                    }));
                    return new EmptyResult();
                }
                else
                {
                    //IList<UserInfo> UserInfoList = client.UserInfo(Name, rows, sidx, sord, page - 1).ToList<UserInfo>();
                    if (TotalLogCollections != null && TotalLogCollections.Count > 0)
                    {
                        long totalrecords = TotalLogCollections.Count;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var AttLst = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in TotalLogCollections
                                    select new
                                    {
                                        cell = new string[] 
                                         {
                                             items.Id.ToString(),
                                             items.Campus,
                                             items.Name,
                                             items.IdNumber,
                                             items.AttendanceDate!=null?items.AttendanceDate.Value.ToString("dd/MM/yyyy"):string.Empty,
                                             items.Department,
                                             items.Programme,
                                             items.Designation,
                                             items.LogInTime!=null?items.LogInTime.Value.ToString("HH:mm:ss"):"00:00:00",
                                             items.LogOutTime!=null?items.LogOutTime.Value.ToString("HH:mm:ss"):"00:00:00",
                                             items.WorkingHours
                                         }
                                    })
                        };
                        return Json(AttLst, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var AssLst = new { rows = (new { cell = new string[] { } }) };
                        return Json(AssLst, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region Staff Consolidate Attendance Summary
        public ActionResult StaffConsolidateSummary()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw ex;
            }
        }
        public ActionResult StaffConsolidateSummaryJqGrid(string Campus, string StaffType, string Programme, string IdNumber, string StaffName, string StaffStatus, string AttendanceFromDate, string AttendanceToDate, string MonthYear, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                BioAttendanceService bioMetric = new BioAttendanceService();
                long NoOfDaysWeekOff = Convert.ToInt64(ConfigurationManager.AppSettings["NoOfDaysWeekOff"]);
                DateTime StartDate = new DateTime();
                DateTime ToDate = new DateTime();
                if (!string.IsNullOrEmpty(MonthYear))
                {
                    DateTime[] fromto = new DateTime[2];
                    fromto = getFromTodateByMonthAndYear(MonthYear);
                    AttendanceFromDate = fromto[0].ToString("dd/MM/yyyy");
                    AttendanceToDate = fromto[1].ToString("dd/MM/yyyy");
                }
                if (!string.IsNullOrEmpty(AttendanceFromDate) && !string.IsNullOrEmpty(AttendanceToDate))
                {
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    StartDate = DateTime.Parse(AttendanceFromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    ToDate = DateTime.Parse(AttendanceToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                }
                var SearchDateList = Enumerable.Range(0, 1 + ToDate.Subtract(StartDate).Days).Select(offset => StartDate.AddDays(offset)).ToArray();
                var MonthAndYearGroup = (from p in SearchDateList
                                         group p by new { month = p.Date.Month, year = p.Date.Year } into d
                                         select d).ToList();
                List<Staff_ConsolidateDeviceLogSummary_SP> TotalLogCollections = new List<Staff_ConsolidateDeviceLogSummary_SP>();
                IList<Staff_ConsolidateDeviceLogSummary_SP> DeviceLogsList = new List<Staff_ConsolidateDeviceLogSummary_SP>();
                IList<Staff_ConsolidateDeviceLogSummary_SP> ConsolidateLogList = new List<Staff_ConsolidateDeviceLogSummary_SP>();

                for (int i = 0; i < MonthAndYearGroup.Count; i++)
                {
                    var SingleMonthYear = (from u in MonthAndYearGroup[i] select u).ToList();
                    Dictionary<long, IList<Staff_ConsolidateDeviceLogSummary_SP>> BioMetricAttendanceDetails = null;
                    BioMetricAttendanceDetails = bioMetric.GetStaffConsolidateDeviceLogSummaryDetails(Campus, IdNumber, StaffName, StaffType, Programme, StaffStatus, SingleMonthYear[0].Date.ToString("dd/MM/yyyy 00:00:01"), SingleMonthYear[SingleMonthYear.Count - 1].Date.ToString("dd/MM/yyyy 23:59:59"));
                    if (BioMetricAttendanceDetails != null && BioMetricAttendanceDetails.FirstOrDefault().Key > 0)
                    {
                        var SinleMonthLogList = (from u in BioMetricAttendanceDetails.FirstOrDefault().Value
                                                 //where u.AttendanceDate != null
                                                 select u).ToList();
                        TotalLogCollections.AddRange(SinleMonthLogList);
                    }
                }
                if (TotalLogCollections != null && TotalLogCollections.Count > 0)
                {
                    ConsolidateLogList = (from c in TotalLogCollections
                                          group c by new
                                          {
                                              c.PreRegNum,
                                              c.Name,
                                              c.IdNumber,
                                              c.Campus,
                                              c.StaffCategoryForAttendane,
                                              c.Department,
                                              c.Designation,
                                              c.Programme,
                                              c.TotalDays,
                                          } into gcs
                                          select new Staff_ConsolidateDeviceLogSummary_SP()
                                          {

                                              PreRegNum = gcs.Key.PreRegNum,
                                              Name = gcs.Key.Name,
                                              IdNumber = gcs.Key.IdNumber,
                                              Campus = gcs.Key.Campus,
                                              StaffCategoryForAttendane = gcs.Key.StaffCategoryForAttendane,
                                              Department = gcs.Key.Department,
                                              Designation = gcs.Key.Designation,
                                              Programme = gcs.Key.Programme,
                                              //TotalDays = gcs.Sum(x => x.TotalDays),
                                              TotalDays = gcs.Key.TotalDays,
                                              TotalWorkedDays = (gcs.Sum(x => x.TotalWorkedDays)) + NoOfDaysWeekOff,
                                              TotalWorkedHoursMinutesAndseconds = BulidTimeFormatString(gcs.Sum(x => x.TotalWorkedHours), gcs.Sum(x => x.TotalWorkedMinutes), gcs.Sum(x => x.TotalWorkedSeconds))
                                          }).ToList();
                }
                if (ExportType == "Excel")
                {
                    string ExcelFileName = "Staff_Consolidate_Summary_From_" + AttendanceFromDate + "_To_" + AttendanceToDate + "_On_" + DateTime.Now.ToString("dd/MM/yyyy");
                    ExptToXL(ConsolidateLogList, ExcelFileName, (items => new
                    {
                        items.Campus,
                        items.Name,
                        items.IdNumber,
                        Group = items.Programme,
                        items.Designation,
                        TotalDays = items.TotalDays.ToString(),
                        TotalWorkingDays = items.TotalWorkedDays.ToString(),
                        NoOfDaysLeaveTaken = (items.TotalDays - items.TotalWorkedDays).ToString(),
                        TotalWorkedHours = items.TotalWorkedHoursMinutesAndseconds
                    }));
                    return new EmptyResult();
                }
                else
                {
                    if (ConsolidateLogList != null && ConsolidateLogList.Count > 0)
                    {
                        long totalrecords = ConsolidateLogList.Count;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var AttLst = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in ConsolidateLogList
                                    select new
                                    {
                                        cell = new string[] 
                                         {
                                             items.Id.ToString(),
                                             items.Campus,
                                             items.Name,
                                             items.IdNumber,
                                             items.Department,
                                             items.StaffCategoryForAttendane,
                                             items.Programme,
                                             items.Designation,
                                             items.TotalDays.ToString(),
                                             items.TotalWorkedDays.ToString(),
                                             (items.TotalDays-items.TotalWorkedDays).ToString(),
                                             items.TotalWorkedHoursMinutesAndseconds,
                                             //!string.IsNullOrEmpty(items.TotalWorkedHours)?items.TotalWorkedHours:"00:00:00:000",
                                             //items.TotalWorkedHoursInDateTimeFormat!=null?items.TotalWorkedHoursInDateTimeFormat.Value.ToString("HH:mm:ss"):string.Empty
                                             "<img src='/Images/History.png ' id='ImgHistory' onclick=\"ShowStaffAttendanceDetails('" + items.IdNumber +"','"+AttendanceFromDate+"','"+AttendanceToDate+"');\" />"
                                         }
                                    })
                        };
                        return Json(AttLst, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var AssLst = new { rows = (new { cell = new string[] { } }) };
                        return Json(AssLst, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string BulidTimeFormatString(long Hours, long Minutes, long Seconds)
        {
            string TimeFormat = string.Empty;
            Minutes += Seconds / 60;
            Seconds %= 60;
            Hours += Minutes / 60;
            Minutes %= 60;
            TimeFormat = string.Format("{0:00}:{1:00}:{2:00}", Hours, Minutes, Seconds);
            return TimeFormat;
        }
        public ActionResult ShowStaffAttendanceDetails(string IdNumber, string AttendanceFromDate, string AttendanceToDate)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ViewBag.IdNumber = IdNumber;
                    ViewBag.AttendanceFromDate = AttendanceFromDate;
                    ViewBag.AttendanceToDate = AttendanceToDate;
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw ex;
            }
        }
        #endregion
        #region Staff Daily Attendance Summary
        public ActionResult StaffDailyAttendanceIOSummaryJqGrid(string IdNumber, string AttendanceFromDate, string AttendanceToDate, string MonthYear, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                BioAttendanceService bioMetric = new BioAttendanceService();
                DateTime StartDate = new DateTime();
                DateTime ToDate = new DateTime();
                if (!string.IsNullOrEmpty(MonthYear))
                {
                    DateTime[] fromto = new DateTime[2];
                    fromto = getFromTodateByMonthAndYear(MonthYear);
                    AttendanceFromDate = fromto[0].ToString("dd/MM/yyyy");
                    AttendanceToDate = fromto[1].ToString("dd/MM/yyyy");
                }
                if (!string.IsNullOrEmpty(AttendanceFromDate) && !string.IsNullOrEmpty(AttendanceToDate))
                {
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    StartDate = DateTime.Parse(AttendanceFromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    ToDate = DateTime.Parse(AttendanceToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                }
                var SearchDateList = Enumerable.Range(0, 1 + ToDate.Subtract(StartDate).Days).Select(offset => StartDate.AddDays(offset)).ToArray();
                var MonthAndYearGroup = (from p in SearchDateList
                                         group p by new { month = p.Date.Month, year = p.Date.Year } into d
                                         select d).ToList();
                List<Staff_DeviceLogSummarySP> TotalLogCollections = new List<Staff_DeviceLogSummarySP>();
                IList<Staff_DeviceLogSummarySP> DeviceLogsList = new List<Staff_DeviceLogSummarySP>();
                IList<Staff_DeviceLogSummarySP> ConsolidateLogList = new List<Staff_DeviceLogSummarySP>();
                for (int i = 0; i < MonthAndYearGroup.Count; i++)
                {
                    var SingleMonthYear = (from u in MonthAndYearGroup[i] select u).ToList();
                    Dictionary<long, IList<Staff_DeviceLogSummarySP>> BioMetricAttendanceDetails = null;
                    BioMetricAttendanceDetails = bioMetric.GetStaffDeviceLogSummaryDetails(string.Empty, IdNumber, string.Empty, string.Empty, SingleMonthYear[0].Date.ToString("dd/MM/yyyy 00:00:01"), SingleMonthYear[SingleMonthYear.Count - 1].Date.ToString("dd/MM/yyyy 23:59:59"));
                    if (BioMetricAttendanceDetails != null && BioMetricAttendanceDetails.FirstOrDefault().Key > 0)
                    {
                        var SinleMonthLogList = (from u in BioMetricAttendanceDetails.FirstOrDefault().Value
                                                 where u.AttendanceDate != null
                                                 select u).ToList();
                        TotalLogCollections.AddRange(SinleMonthLogList);
                    }
                }
                if (TotalLogCollections != null && TotalLogCollections.Count > 0)
                {
                    ConsolidateLogList = (from c in TotalLogCollections
                                          group c by new
                                          {
                                              c.PreRegNum,
                                              c.Name,
                                              c.IdNumber,
                                              c.Campus,
                                              c.StaffType,
                                              c.Department,
                                              c.Designation,
                                              c.Programme,
                                              c.AttendanceDate,
                                              c.LogInTime,
                                              c.LogOutTime,
                                              c.WorkingHours
                                          } into gcs
                                          select new Staff_DeviceLogSummarySP()
                                          {
                                              PreRegNum = gcs.Key.PreRegNum,
                                              Name = gcs.Key.Name,
                                              IdNumber = gcs.Key.IdNumber,
                                              Campus = gcs.Key.Campus,
                                              StaffType = gcs.Key.StaffType,
                                              Department = gcs.Key.Department,
                                              Designation = gcs.Key.Designation,
                                              Programme = gcs.Key.Programme,
                                              AttendanceDate = gcs.Key.AttendanceDate,
                                              LogInTime = gcs.Key.LogInTime,
                                              LogOutTime = gcs.Key.LogOutTime,
                                              WorkingHours = gcs.Key.WorkingHours


                                          }).ToList();
                }
                if (ExportType == "Excel")
                {
                    string ExcelFileName = "Staff_Daily_IO_Summary_From_" + AttendanceFromDate + "_To_" + AttendanceToDate + "_On_" + DateTime.Now.ToString("dd/MM/yyyy");
                    ExptToXL(TotalLogCollections, ExcelFileName, (items => new
                    {
                        items.Campus,
                        items.Name,
                        items.IdNumber,
                        AttendanceDate = items.AttendanceDate != null ? items.AttendanceDate.Value.ToString("dd/MM/yyyy") : string.Empty,
                        Group = items.Programme,
                        items.Designation,
                        LogInTime = items.LogInTime != null ? items.LogInTime.Value.ToString("HH:mm:ss") : "00:00:00",
                        LogOutTime = items.LogOutTime != null ? items.LogOutTime.Value.ToString("HH:mm:ss") : "00:00:00",
                        items.WorkingHours
                    }));
                    return new EmptyResult();
                }
                else
                {
                    //IList<UserInfo> UserInfoList = client.UserInfo(Name, rows, sidx, sord, page - 1).ToList<UserInfo>();
                    if (ConsolidateLogList != null && ConsolidateLogList.Count > 0)
                    {
                        long totalrecords = ConsolidateLogList.Count;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var AttLst = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in ConsolidateLogList
                                    select new
                                    {
                                        cell = new string[] 
                                         {
                                             items.Id.ToString(),
                                             items.Campus,
                                             items.Name,
                                             items.IdNumber,
                                             items.AttendanceDate!=null?items.AttendanceDate.Value.ToString("dd/MM/yyyy"):string.Empty,
                                             items.Department,
                                             items.Programme,
                                             items.Designation,
                                             items.LogInTime!=null?items.LogInTime.Value.ToString("HH:mm:ss"):"00:00:00",
                                             items.LogOutTime!=null?items.LogOutTime.Value.ToString("HH:mm:ss"):"00:00:00",
                                             items.WorkingHours
                                         }
                                    })
                        };
                        return Json(AttLst, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var AssLst = new { rows = (new { cell = new string[] { } }) };
                        return Json(AssLst, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Staff Consolidate Attendance Summary New

        public ActionResult StaffConsolidateSummaryNew()
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                UserService us = new UserService();
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    criteria.Add("UserId", userId);
                    criteria.Add("AppCode", "STFIOSUMRY");
                    Dictionary<long, IList<UserAppRole>> listObj = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    var RoleArray = (from u in listObj.FirstOrDefault().Value
                                     where u.UserId == userId
                                     select u.RoleCode).ToArray();
                    var DeptArray = (from item in listObj.FirstOrDefault().Value
                                     where item.UserId == userId
                                     select item.DeptCode).ToArray();
                    var BranchArray = (from a in listObj.FirstOrDefault().Value
                                       where a.UserId == userId
                                       select a.BranchCode).ToArray();
                    if (RoleArray != null && DeptArray != null && DeptArray.Contains("FEES / FINANCE"))
                    {
                        ViewBag.Flag = "Show-Finance";
                        ViewBag.Campus = BranchArray[0];
                    }
                    else if (RoleArray != null && DeptArray != null && DeptArray.Contains("HR"))
                    {
                        ViewBag.Flag = "Show-HR";
                        ViewBag.Campus = BranchArray[0];
                    }
                    else if (RoleArray != null && RoleArray.Contains("Bio-All"))
                    {
                        ViewBag.Flag = "Show-ALL";
                    }
                    else
                    {
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw ex;
            }
        }


        //public ActionResult StaffConsolidateSummaryNewJqGrid(string Campus, string StaffType, string Programme, string IdNumber, string StaffName, string AttendanceFromDate, string AttendanceToDate, string MonthYear, string ExportType, string OnDuty, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        BioAttendanceService bioMetric = new BioAttendanceService();
        //        long NoOfDaysWeekOff = Convert.ToInt64(ConfigurationManager.AppSettings["NoOfDaysWeekOff"]);
        //        DateTime StartDate = new DateTime();
        //        DateTime ToDate = new DateTime();
        //        if (!string.IsNullOrEmpty(MonthYear))
        //        {
        //            DateTime[] fromto = new DateTime[2];
        //            fromto = getFromTodateByMonthAndYear(MonthYear);
        //            AttendanceFromDate = fromto[0].ToString("dd/MM/yyyy");
        //            AttendanceToDate = fromto[1].ToString("dd/MM/yyyy");
        //        }
        //        if (!string.IsNullOrEmpty(AttendanceFromDate) && !string.IsNullOrEmpty(AttendanceToDate))
        //        {
        //            IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
        //            StartDate = DateTime.Parse(AttendanceFromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
        //            ToDate = DateTime.Parse(AttendanceToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
        //        }
        //        var SearchDateList = Enumerable.Range(0, 1 + ToDate.Subtract(StartDate).Days).Select(offset => StartDate.AddDays(offset)).ToArray();
        //        var MonthAndYearGroup = (from p in SearchDateList
        //                                 group p by new { month = p.Date.Month, year = p.Date.Year } into d
        //                                 select d).ToList();
        //        List<Staff_ConsolidateDeviceLogSummary_SP> TotalLogCollections = new List<Staff_ConsolidateDeviceLogSummary_SP>();
        //        IList<Staff_ConsolidateDeviceLogSummary_SP> DeviceLogsList = new List<Staff_ConsolidateDeviceLogSummary_SP>();
        //        IList<Staff_ConsolidateDeviceLogSummary_SP> ConsolidateLogList = new List<Staff_ConsolidateDeviceLogSummary_SP>();
        //        for (int i = 0; i < MonthAndYearGroup.Count; i++)
        //        {
        //            var SingleMonthYear = (from u in MonthAndYearGroup[i] select u).ToList();
        //            Dictionary<long, IList<Staff_ConsolidateDeviceLogSummary_SP>> BioMetricAttendanceDetails = null;
        //            BioMetricAttendanceDetails = bioMetric.GetStaffConsolidateDeviceLogSummaryDetails(Campus, IdNumber, StaffName, StaffType, Programme, SingleMonthYear[0].Date.ToString("dd/MM/yyyy 00:00:01"), SingleMonthYear[SingleMonthYear.Count - 1].Date.ToString("dd/MM/yyyy 23:59:59"));
        //            if (BioMetricAttendanceDetails != null && BioMetricAttendanceDetails.FirstOrDefault().Key > 0)
        //            {
        //                var SinleMonthLogList = (from u in BioMetricAttendanceDetails.FirstOrDefault().Value
        //                                         //where u.AttendanceDate != null
        //                                         select u).ToList();
        //                TotalLogCollections.AddRange(SinleMonthLogList);
        //            }
        //        }
        //        if (TotalLogCollections != null && TotalLogCollections.Count > 0)
        //        {
        //            ConsolidateLogList = (from c in TotalLogCollections
        //                                  group c by new
        //                                  {
        //                                      c.PreRegNum,
        //                                      c.Name,
        //                                      c.IdNumber,
        //                                      c.Campus,
        //                                      c.StaffType,
        //                                      c.Department,
        //                                      c.Designation,
        //                                      c.Programme,
        //                                      c.TotalDays,
        //                                      // c.TotalWorkedDays,
        //                                      //c.NoOfLeaveTaken,
        //                                      //c.OpeningBalance,
        //                                      //c.ClosingBalance,
        //                                      //c.Remarks,
        //                                      //c.OnDuty,
        //                                      //c.NoOfPermissionsTaken,
        //                                      //c.NoOfLeavesCalculatedByPermissions

        //                                  } into gcs
        //                                  select new Staff_ConsolidateDeviceLogSummary_SP()
        //                                  {
        //                                      PreRegNum = gcs.Key.PreRegNum,
        //                                      Name = gcs.Key.Name,
        //                                      IdNumber = gcs.Key.IdNumber,
        //                                      Campus = gcs.Key.Campus,
        //                                      StaffType = gcs.Key.StaffType,
        //                                      Department = gcs.Key.Department,
        //                                      Designation = gcs.Key.Designation,
        //                                      Programme = gcs.Key.Programme,
        //                                      //TotalDays = gcs.Sum(x => x.TotalDays),
        //                                      TotalDays = gcs.Key.TotalDays,
        //                                      TotalWorkedDays = (gcs.Sum(x => x.TotalWorkedDays)) + NoOfDaysWeekOff,
        //                                      //TotalWorkedDays=gcs.Key.TotalWorkedDays,
        //                                      //TotalWorkedDays = CalcuateWorkedDays(gcs.Key.PreRegNum, AttendanceToDate, ((gcs.Sum(x => x.TotalWorkedDays)) + NoOfDaysWeekOff)),
        //                                      //NoOfLeaveTaken =  gcs.Key.NoOfLeaveTaken - NoOfDaysWeekOff,
        //                                      NoOfLeaveTaken = CalcuateLeaveTaken(gcs.Key.PreRegNum, AttendanceToDate, (gcs.FirstOrDefault().NoOfLeaveTaken - NoOfDaysWeekOff)),
        //                                      TotalWorkedHoursMinutesAndseconds = BulidTimeFormatStringNew(gcs.Sum(x => x.TotalWorkedHours), gcs.Sum(x => x.TotalWorkedMinutes), gcs.Sum(x => x.TotalWorkedSeconds)),
        //                                      Remarks = gcs.FirstOrDefault().Remarks,
        //                                      OpeningBalance = gcs.FirstOrDefault().OpeningBalance,
        //                                      ClosingBalance = gcs.FirstOrDefault().ClosingBalance,
        //                                      OnDuty = gcs.FirstOrDefault().OnDuty,
        //                                      NoOfPermissionsTaken = gcs.FirstOrDefault().NoOfPermissionsTaken,
        //                                      NoOfLeavesCalculatedByPermissions = gcs.FirstOrDefault().NoOfLeavesCalculatedByPermissions
        //                                  }).ToList();
        //        }
        //        if (ExportType == "Excel")
        //        {
        //            string ExcelFileName = "Staff_Consolidate_Summary_From_" + AttendanceFromDate + "_To_" + AttendanceToDate + "_On_" + DateTime.Now.ToString("dd/MM/yyyy");
        //            ExptToXL(ConsolidateLogList, ExcelFileName, (items => new
        //            {
        //                items.Campus,
        //                items.Name,
        //                ID_Number = items.IdNumber,
        //                Group = items.Programme,
        //                items.Designation,
        //                //TotalDays = items.TotalDays.ToString(),
        //                Total_No_Of_Days_Worked = items.TotalWorkedDays.ToString(),
        //                Total_No_Of_Days_Leave_Taken = items.NoOfLeaveTaken,
        //                //NoOfDaysLeaveTaken = (items.TotalDays - items.TotalWorkedDays).ToString(),
        //                //TotalWorkedHours = items.TotalWorkedHoursMinutesAndseconds,
        //                Opening_Balance = items.OpeningBalance,
        //                Closing_Balance = items.ClosingBalance,
        //                //items.Remarks,
        //                //items.OnDuty,
        //                //items.NoOfPermissionsTaken,
        //                //items.NoOfLeavesCalculatedByPermissions

        //            }));
        //            return new EmptyResult();
        //        }
        //        else
        //        {
        //            if (ConsolidateLogList != null && ConsolidateLogList.Count > 0)
        //            {
        //                long totalrecords = ConsolidateLogList.Count;
        //                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                var AttLst = new
        //                {
        //                    total = totalPages,
        //                    page = page,
        //                    records = totalrecords,

        //                    rows = (from items in ConsolidateLogList
        //                            select new
        //                            {
        //                                cell = new string[] 
        //                                 {
        //                                     items.Id.ToString(),
        //                                     items.Campus,
        //                                     items.Name,
        //                                     items.IdNumber,
        //                                     items.Department,
        //                                     items.StaffType,
        //                                     items.Programme,
        //                                     items.Designation,
        //                                     items.TotalDays.ToString(),
        //                                     CalcuateWorkedDays(items.PreRegNum,AttendanceToDate,items.TotalWorkedDays).ToString(),
        //                                     items.NoOfLeaveTaken.ToString(),
        //                                  //  (items.TotalDays-items.TotalWorkedDays).ToString(),
        //                                     items.TotalWorkedHoursMinutesAndseconds,
        //                                     //!string.IsNullOrEmpty(items.TotalWorkedHours)?items.TotalWorkedHours:"00:00:00:000",
        //                                     //items.TotalWorkedHoursInDateTimeFormat!=null?items.TotalWorkedHoursInDateTimeFormat.Value.ToString("HH:mm:ss"):string.Empty
        //                                      "<img src='/Images/History.png ' id='ImgHistory' onclick=\"ShowStaffAttendanceDetailsNew('" + items.IdNumber +"','"+AttendanceFromDate+"','"+AttendanceToDate+"');\" />",
        //                                    "<Button class='btn btn-primary' id='EditHistory'  onclick=\"EditStaffAttendanceDetails( '" + items.PreRegNum +"','"+items.TotalWorkedDays+"','"+items.StaffType+"','"+ AttendanceToDate+ "','"+ items.TotalDays+"','"+ items.Campus +"','"+ items.NoOfLeaveTaken +"');\" >Edit</Button>",
        //                                    //"<img src='/Images/edit.png ' width='25' height='14' id='EditHistory' onclick=\"EditStaffAttendanceDetails('" + items.PreRegNum +"','"+items.TotalWorkedDays+"','"+items.StaffType+"');\" />",
        //                                    items.OpeningBalance.ToString(),
        //                                    items.ClosingBalance.ToString(),
        //                                    items.Remarks,
        //                                    items.OnDuty.ToString(),
        //                                    items.NoOfPermissionsTaken.ToString(),
        //                                    items.NoOfLeavesCalculatedByPermissions.ToString()
        //                                 }
        //                            })
        //                };
        //                return Json(AttLst, JsonRequestBehavior.AllowGet);
        //            }
        //            else
        //            {
        //                var AssLst = new { rows = (new { cell = new string[] { } }) };
        //                return Json(AssLst, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public ActionResult StaffConsolidateSummaryNewJqGrid(string Campus, string StaffType, string Programme, string IdNumber, string StaffName, string AttendanceFromDate, string AttendanceToDate, string MonthYear, string ExportType, string OnDuty, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        BioAttendanceService bioMetric = new BioAttendanceService();
        //        long NoOfDaysWeekOff = Convert.ToInt64(ConfigurationManager.AppSettings["NoOfDaysWeekOff"]);
        //        DateTime StartDate = new DateTime();
        //        DateTime ToDate = new DateTime();
        //        if (!string.IsNullOrEmpty(MonthYear))
        //        {
        //            DateTime[] fromto = new DateTime[2];
        //            fromto = getFromTodateByMonthAndYear(MonthYear);
        //            AttendanceFromDate = fromto[0].ToString("dd/MM/yyyy");
        //            AttendanceToDate = fromto[1].ToString("dd/MM/yyyy");
        //        }
        //        if (!string.IsNullOrEmpty(AttendanceFromDate) && !string.IsNullOrEmpty(AttendanceToDate))
        //        {
        //            IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
        //            //StartDate = DateTime.Parse(AttendanceFromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
        //            StartDate = DateTime.ParseExact(AttendanceFromDate, "dd/MM/yyyy",CultureInfo.InvariantCulture);
        //            ToDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //            //ToDate = DateTime.Parse(AttendanceToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
        //        }
        //        var SearchDateList = Enumerable.Range(0, 1 + ToDate.Subtract(StartDate).Days).Select(offset => StartDate.AddDays(offset)).ToArray();
        //        var MonthAndYearGroup = (from p in SearchDateList
        //                                 group p by new { month = p.Date.Month, year = p.Date.Year } into d
        //                                 select d).ToList();
        //        List<Staff_ConsolidateDeviceLogSummary_SP> TotalLogCollections = new List<Staff_ConsolidateDeviceLogSummary_SP>();
        //        IList<Staff_ConsolidateDeviceLogSummary_SP> DeviceLogsList = new List<Staff_ConsolidateDeviceLogSummary_SP>();
        //        IList<Staff_ConsolidateDeviceLogSummary_SP> ConsolidateLogList = new List<Staff_ConsolidateDeviceLogSummary_SP>();
        //        for (int i = 0; i < MonthAndYearGroup.Count; i++)
        //        {
        //            var SingleMonthYear = (from u in MonthAndYearGroup[i] select u).ToList();
        //            Dictionary<long, IList<Staff_ConsolidateDeviceLogSummary_SP>> BioMetricAttendanceDetails = null;
        //            BioMetricAttendanceDetails = bioMetric.GetStaffConsolidateDeviceLogSummaryDetails(Campus, IdNumber, StaffName, StaffType, Programme, SingleMonthYear[0].Date.ToString("dd/MM/yyyy 00:00:01"), SingleMonthYear[SingleMonthYear.Count - 1].Date.ToString("dd/MM/yyyy 23:59:59"));
        //            if (BioMetricAttendanceDetails != null && BioMetricAttendanceDetails.FirstOrDefault().Key > 0)
        //            {
        //                var SinleMonthLogList = (from u in BioMetricAttendanceDetails.FirstOrDefault().Value
        //                                         //where u.AttendanceDate != null
        //                                         select u).ToList();
        //                TotalLogCollections.AddRange(SinleMonthLogList);
        //            }
        //        }
        //        if (TotalLogCollections != null && TotalLogCollections.Count > 0)
        //        {
        //            ConsolidateLogList = (from c in TotalLogCollections
        //                                  group c by new
        //                                  {
        //                                      c.PreRegNum,
        //                                      c.Name,
        //                                      c.IdNumber,
        //                                      c.Campus,
        //                                      c.StaffType,
        //                                      c.Department,
        //                                      c.Designation,
        //                                      c.Programme,
        //                                      c.TotalDays,
        //                                      // c.TotalWorkedDays,
        //                                      //c.NoOfLeaveTaken,
        //                                      //c.OpeningBalance,
        //                                      //c.ClosingBalance,
        //                                      //c.Remarks,
        //                                      //c.OnDuty,
        //                                      //c.NoOfPermissionsTaken,
        //                                      //c.NoOfLeavesCalculatedByPermissions

        //                                  } into gcs
        //                                  select new Staff_ConsolidateDeviceLogSummary_SP()
        //                                  {
        //                                      PreRegNum = gcs.Key.PreRegNum,
        //                                      Name = gcs.Key.Name,
        //                                      IdNumber = gcs.Key.IdNumber,
        //                                      Campus = gcs.Key.Campus,
        //                                      StaffType = gcs.Key.StaffType,
        //                                      Department = gcs.Key.Department,
        //                                      Designation = gcs.Key.Designation,
        //                                      Programme = gcs.Key.Programme,
        //                                      //TotalDays = gcs.Sum(x => x.TotalDays),
        //                                      TotalDays = gcs.Key.TotalDays,
        //                                      TotalWorkedDays = (gcs.Sum(x => x.TotalWorkedDays)) + NoOfDaysWeekOff,
        //                                      //TotalWorkedDays=gcs.Key.TotalWorkedDays,
        //                                      //TotalWorkedDays = CalcuateWorkedDays(gcs.Key.PreRegNum, AttendanceToDate, ((gcs.Sum(x => x.TotalWorkedDays)) + NoOfDaysWeekOff)),
        //                                      //NoOfLeaveTaken =  gcs.Key.NoOfLeaveTaken - NoOfDaysWeekOff,
        //                                      //NoOfLeaveTaken = CalcuateLeaveTaken(gcs.Key.PreRegNum, AttendanceToDate, (gcs.FirstOrDefault().NoOfLeaveTaken - NoOfDaysWeekOff)),
        //                                      NoOfLeaveTaken = gcs.FirstOrDefault().NoOfLeaveTaken,
        //                                      TotalWorkedHoursMinutesAndseconds = BulidTimeFormatStringNew(gcs.Sum(x => x.TotalWorkedHours), gcs.Sum(x => x.TotalWorkedMinutes), gcs.Sum(x => x.TotalWorkedSeconds)),
        //                                      Remarks = gcs.FirstOrDefault().Remarks,
        //                                      OpeningBalance = gcs.FirstOrDefault().OpeningBalance,
        //                                      ClosingBalance = gcs.FirstOrDefault().ClosingBalance,
        //                                      OnDuty = gcs.FirstOrDefault().OnDuty,
        //                                      NoOfPermissionsTaken = gcs.FirstOrDefault().NoOfPermissionsTaken,
        //                                      NoOfLeavesCalculatedByPermissions = gcs.FirstOrDefault().NoOfLeavesCalculatedByPermissions
        //                                  }).ToList();
        //        }
        //        if (ExportType == "Excel")
        //        {
        //            string ExcelFileName = "Staff_Consolidate_Summary_From_" + AttendanceFromDate + "_To_" + AttendanceToDate + "_On_" + DateTime.Now.ToString("dd/MM/yyyy");
        //            ExptToXL(ConsolidateLogList, ExcelFileName, (items => new
        //            {
        //                items.Campus,
        //                items.Name,
        //                ID_Number = items.IdNumber,
        //                Group = items.Programme,
        //                items.Designation,
        //                //TotalDays = items.TotalDays.ToString(),
        //                Total_No_Of_Days_Worked = CalcuateWorkedDays(items.PreRegNum, AttendanceToDate, items.TotalWorkedDays).ToString(),
        //                No_Of_Days_Leave_Calculated = LeaveToBeCalculated(items.Campus, items.PreRegNum, AttendanceToDate, (items.TotalDays - items.TotalWorkedDays)).ToString(),
        //                //NoOfDaysLeaveTaken = (items.TotalDays - items.TotalWorkedDays).ToString(),
        //                //TotalWorkedHours = items.TotalWorkedHoursMinutesAndseconds,
        //                Opening_Balance = items.OpeningBalance,
        //                Closing_Balance = ClosingBalance(items.Campus, items.PreRegNum, AttendanceToDate, CalcuateLeaveTaken(items.PreRegNum, AttendanceToDate, (items.TotalDays - items.TotalWorkedDays)), CalcuateWorkedDays(items.PreRegNum, AttendanceToDate, items.TotalWorkedDays)).ToString(),
        //                //items.Remarks,
        //                //items.OnDuty,
        //                //items.NoOfPermissionsTaken,
        //                //items.NoOfLeavesCalculatedByPermissions

        //            }));
        //            return new EmptyResult();
        //        }
        //        else
        //        {
        //            if (ConsolidateLogList != null && ConsolidateLogList.Count > 0)
        //            {
        //                long totalrecords = ConsolidateLogList.Count;
        //                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                var AttLst = new
        //                {
        //                    total = totalPages,
        //                    page = page,
        //                    records = totalrecords,

        //                    rows = (from items in ConsolidateLogList
        //                            select new
        //                            {
        //                                cell = new string[] 
        //                                 {
        //                                     items.Id.ToString(),
        //                                     items.Campus,
        //                                     items.Name,
        //                                     items.IdNumber,
        //                                     items.Department,
        //                                     items.StaffType,
        //                                     items.Programme,
        //                                     items.Designation,
        //                                     items.TotalDays.ToString(),
        //                                     CalcuateWorkedDays(items.PreRegNum,AttendanceToDate,items.TotalWorkedDays).ToString(),
        //                                     //items.NoOfLeaveTaken.ToString(),
        //                                     //CalcuateLeaveTaken(items.PreRegNum, AttendanceToDate, (items.TotalDays-items.TotalWorkedDays)).ToString(),
        //                                     LeaveToBeCalculated(items.Campus,items.PreRegNum,AttendanceToDate,(items.TotalDays-items.TotalWorkedDays)).ToString(),
        //                                     //(items.TotalDays-items.TotalWorkedDays).ToString(),
        //                                     items.TotalWorkedHoursMinutesAndseconds,
        //                                     //!string.IsNullOrEmpty(items.TotalWorkedHours)?items.TotalWorkedHours:"00:00:00:000",
        //                                     //items.TotalWorkedHoursInDateTimeFormat!=null?items.TotalWorkedHoursInDateTimeFormat.Value.ToString("HH:mm:ss"):string.Empty
        //                                      "<img src='/Images/History.png ' id='ImgHistory' onclick=\"ShowStaffAttendanceDetailsNew('" + items.IdNumber +"','"+AttendanceFromDate+"','"+AttendanceToDate+"');\" />",
        //                                    "<Button class='btn btn-primary' id='EditHistory'  onclick=\"EditStaffAttendanceDetails( '" + items.PreRegNum +"','"+ CalcuateWorkedDays(items.PreRegNum,AttendanceToDate,items.TotalWorkedDays)+"','"+items.StaffType+"','"+ AttendanceToDate+ "','"+ items.TotalDays+"','"+ items.Campus +"','"+ CalcuateLeaveTaken(items.PreRegNum, AttendanceToDate, (items.TotalDays-items.TotalWorkedDays)) +"');\" >Edit</Button>",
        //                                    //"<img src='/Images/edit.png ' width='25' height='14' id='EditHistory' onclick=\"EditStaffAttendanceDetails('" + items.PreRegNum +"','"+items.TotalWorkedDays+"','"+items.StaffType+"');\" />",
        //                                    items.OpeningBalance.ToString(),
        //                                    items.ClosingBalance.ToString(),
        //                                    items.Remarks,
        //                                    items.OnDuty.ToString(),
        //                                    items.NoOfPermissionsTaken.ToString(),
        //                                    items.NoOfLeavesCalculatedByPermissions.ToString()
        //                                 }
        //                            })
        //                };
        //                return Json(AttLst, JsonRequestBehavior.AllowGet);
        //            }
        //            else
        //            {
        //                var AssLst = new { rows = (new { cell = new string[] { } }) };
        //                return Json(AssLst, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public ActionResult StaffConsolidateSummaryNewJqGrid(string Campus, string StaffType, string Programme, string IdNumber, string StaffName, string StaffStatus, string searchStstus, string AttendanceFromDate, string AttendanceToDate, string MonthYear, string ExportType, string Rept, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {

                BioAttendanceService bioMetric = new BioAttendanceService();
                long NoOfDaysWeekOff = Convert.ToInt64(ConfigurationManager.AppSettings["NoOfDaysWeekOff"]);
                DateTime StartDate = new DateTime();
                DateTime ToDate = new DateTime();
                if (!string.IsNullOrEmpty(MonthYear))
                {
                    string[] AttendanceDate = MonthYear.Split('-');
                    if (Convert.ToInt32(AttendanceDate[0]) == 3)
                    {
                        var FebMonthDays = DateTime.DaysInMonth(Convert.ToInt32(AttendanceDate[1]), Convert.ToInt32(AttendanceDate[0]) - 1);
                        if (FebMonthDays == 28)
                        {
                            AttendanceToDate = "28" + "/" + String.Join("/", AttendanceDate).ToString();
                            ToDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            var month = new DateTime(Convert.ToInt32(AttendanceDate[1]), Convert.ToInt32(AttendanceDate[0]), 1);
                            var LastMonth = month.AddMonths(-1);
                            AttendanceFromDate = "01" + "/" + LastMonth.ToString("MM") + "/" + LastMonth.Year;
                            StartDate = DateTime.ParseExact(AttendanceFromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            AttendanceToDate = "28" + "/" + String.Join("/", AttendanceDate).ToString();
                            ToDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            var month = new DateTime(Convert.ToInt32(AttendanceDate[1]), Convert.ToInt32(AttendanceDate[0]), 1);
                            var LastMonth = month.AddMonths(-1);
                            AttendanceFromDate = "29" + "/" + LastMonth.ToString("MM") + "/" + LastMonth.Year;
                            StartDate = DateTime.ParseExact(AttendanceFromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                    }
                    else
                    {
                        AttendanceToDate = "28" + "/" + String.Join("/", AttendanceDate).ToString();
                        ToDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        var month = new DateTime(Convert.ToInt32(AttendanceDate[1]), Convert.ToInt32(AttendanceDate[0]), 1);
                        var LastMonth = month.AddMonths(-1);
                        AttendanceFromDate = "29" + "/" + LastMonth.ToString("MM") + "/" + LastMonth.Year;
                        StartDate = DateTime.ParseExact(AttendanceFromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                }
                if (!string.IsNullOrEmpty(AttendanceFromDate) && !string.IsNullOrEmpty(AttendanceToDate))
                {
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    //StartDate = DateTime.Parse(AttendanceFromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    //ToDate = DateTime.Parse(AttendanceToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    StartDate = DateTime.ParseExact(AttendanceFromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    ToDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (string.IsNullOrEmpty(StaffType))
                {
                    string tempStaffType = "Teaching,Non Teaching-Admin";
                    string[] arrayStaffType = tempStaffType.Split(',');
                    StaffType = arrayStaffType[0] + "','" + arrayStaffType[1];
                }
                else
                {
                    string[] arrStaffType = StaffType.Split(',');
                    if (arrStaffType.Length == 1)
                    {
                        //StaffType = StaffType;
                    }
                    else
                    {
                        StaffType = arrStaffType[0] + "','" + arrStaffType[1];
                    }
                }
                if (string.IsNullOrEmpty(StaffStatus))
                {
                    string tempStatus = "Registered,LongLeave,Others,Resigned";
                    string[] arrStatus = tempStatus.Split(',');
                    StaffStatus = arrStatus[0] + "','" + arrStatus[1] + "','" + arrStatus[2] + "','" + arrStatus[3];

                }
                else
                {
                    string[] arrayStatus = StaffStatus.Split(',');
                    if (arrayStatus.Length == 1)
                    {
                        // StaffStatus = StaffStatus;
                    }
                    else
                    {
                        StaffStatus = arrayStatus[0] + "','" + arrayStatus[1] + "','" + arrayStatus[2] + "','" + arrayStatus[3];
                    }
                }

                var SearchDateList = Enumerable.Range(0, 1 + ToDate.Subtract(StartDate).Days).Select(offset => StartDate.AddDays(offset)).ToArray();
                var MonthAndYearGroup = (from p in SearchDateList
                                         group p by new { month = p.Date.Month, year = p.Date.Year } into d
                                         select d).ToList();
                List<Staff_ConsolidateDeviceLogSummary_SP> TotalLogCollections = new List<Staff_ConsolidateDeviceLogSummary_SP>();
                List<Staff_ConsolidateDeviceLogSummary_SP> TotalLogCollectionsForResigned = new List<Staff_ConsolidateDeviceLogSummary_SP>();
                IList<Staff_ConsolidateDeviceLogSummary_SP> DeviceLogsList = new List<Staff_ConsolidateDeviceLogSummary_SP>();
                IList<Staff_ConsolidateDeviceLogSummary_SP> ConsolidateLogList = new List<Staff_ConsolidateDeviceLogSummary_SP>();
                IList<Staff_ConsolidateDeviceLogSummary_SP> ConsolidateLogListNew = new List<Staff_ConsolidateDeviceLogSummary_SP>();
                IList<Staff_ConsolidateDeviceLogSummary_SP> FinalConsolidateList = new List<Staff_ConsolidateDeviceLogSummary_SP>();
                for (int i = 0; i < MonthAndYearGroup.Count; i++)
                {
                    var SingleMonthYear = (from u in MonthAndYearGroup[i] select u).ToList();
                    Dictionary<long, IList<Staff_ConsolidateDeviceLogSummary_SP>> BioMetricAttendanceDetails = null;
                    BioMetricAttendanceDetails = bioMetric.GetStaffConsolidateDeviceLogSummaryDetails(Campus, IdNumber, StaffName, StaffType, Programme, StaffStatus, SingleMonthYear[0].Date.ToString("dd/MM/yyyy 00:00:01"), SingleMonthYear[SingleMonthYear.Count - 1].Date.ToString("dd/MM/yyyy 23:59:59"));
                    if (BioMetricAttendanceDetails != null && BioMetricAttendanceDetails.FirstOrDefault().Key > 0)
                    {
                        var SinleMonthLogList = (from u in BioMetricAttendanceDetails.FirstOrDefault().Value
                                                 where u.CurrentStatus == "Registered" || u.CurrentStatus == "LongLeave"
                                                 select u).ToList();
                        TotalLogCollections.AddRange(SinleMonthLogList);
                    }
                    var SingMonthListForResigned = (from item in BioMetricAttendanceDetails.FirstOrDefault().Value
                                                    where item.CurrentStatus == "Resigned"
                                                    select item).ToList();
                    TotalLogCollectionsForResigned.AddRange(SingMonthListForResigned);

                }
                FinalConsolidateList = ConsolidateLogListForAttendance(TotalLogCollections, TotalLogCollectionsForResigned, AttendanceFromDate, AttendanceToDate);

                if (ExportType == "ExcelForHR")
                {
                    string ExcelFileName = "Staff_Consolidate_Summary_From_For_HR_" + AttendanceFromDate + "_To_" + AttendanceToDate + "_On_" + DateTime.Now.ToString("dd/MM/yyyy");
                    ExptToXL(FinalConsolidateList, ExcelFileName, (items => new
                    {
                        items.Campus,
                        items.Name,
                        ID_Number = items.IdNumber,
                        Group = items.Programme,
                        items.Designation,
                        Staff_Status = items.CurrentStatus == "Registered" ? "Active" : items.CurrentStatus,
                        Total_No_Of_Days_Present = CalcuateWorkedDays(items.PreRegNum, AttendanceToDate, items.TotalWorkedDays).ToString(),
                        Total_No_Of_Days_Absent = (items.TotalDays - CalcuateWorkedDays(items.PreRegNum, AttendanceToDate, items.TotalWorkedDays)).ToString(),
                        No_Of_Days_Leave_Taken = LeaveToBeCalculated(items.Campus, items.PreRegNum, AttendanceToDate, (items.TotalDays - items.TotalWorkedDays)).ToString(),
                        Opening_Balance = OpeningBalance(items.PreRegNum, AttendanceToDate),
                        Closing_Balance = ClosingBalance(items.Campus, items.PreRegNum, AttendanceToDate, CalcuateLeaveTaken(items.PreRegNum, AttendanceToDate, (items.TotalDays - items.TotalWorkedDays)), CalcuateWorkedDays(items.PreRegNum, AttendanceToDate, items.TotalWorkedDays)).ToString(),
                        Remarks = GetRemarks(items.PreRegNum, AttendanceToDate)

                    }));
                    return new EmptyResult();
                }
                else if (ExportType == "ExcelForFinance")
                {
                    string ExcelFileName = "Staff_Consolidate_Summary_From_For_Finance_" + AttendanceFromDate + "_To_" + AttendanceToDate + "_On_" + DateTime.Now.ToString("dd/MM/yyyy");
                    ExptToXL(FinalConsolidateList, ExcelFileName, (items => new
                    {
                        items.Campus,
                        items.Name,
                        ID_Number = items.IdNumber,
                        Staff_Status = items.CurrentStatus == "Registered" ? "Active" : items.CurrentStatus,
                        No_Of_Days_Leave_Taken = LeaveToBeCalculated(items.Campus, items.PreRegNum, AttendanceToDate, (items.TotalDays - items.TotalWorkedDays)).ToString(),
                    }));
                    return new EmptyResult();
                }
                else
                {
                    if (FinalConsolidateList != null && FinalConsolidateList.Count > 0)
                    {
                        long totalrecords = ConsolidateLogList.Count;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var AttLst = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in FinalConsolidateList
                                    select new
                                    {
                                        cell = new string[] 
                                         {
                                             items.Id.ToString(),
                                             items.Campus,
                                             items.Name,
                                             items.IdNumber,
                                             items.Department,
                                             items.StaffCategoryForAttendane,
                                             items.Programme,
                                             items.Designation,
                                             items.CurrentStatus == "Registered"?"Active":items.CurrentStatus,
                                             items.TotalDays.ToString(),
                                             CalcuateWorkedDays(items.PreRegNum,AttendanceToDate,items.TotalWorkedDays).ToString(),
                                             (items.TotalDays-CalcuateWorkedDays(items.PreRegNum,AttendanceToDate,items.TotalWorkedDays)).ToString(),
                                             LeaveToBeCalculated(items.Campus,items.PreRegNum,AttendanceToDate,(items.TotalDays - items.TotalWorkedDays)).ToString(),
                                             items.TotalWorkedHoursMinutesAndseconds,
                                             OpeningBalance(items.PreRegNum, AttendanceToDate).ToString(),
                                             ClosingBalance(items.Campus, items.PreRegNum, AttendanceToDate, CalcuateLeaveTaken(items.PreRegNum, AttendanceToDate, (items.TotalDays - items.TotalWorkedDays)), CalcuateWorkedDays(items.PreRegNum, AttendanceToDate, items.TotalWorkedDays)).ToString(),
                                             "<img src='/Images/History.png ' id='ImgHistory' onclick=\"ShowStaffAttendanceDetailsNew('" + items.IdNumber +"','"+AttendanceFromDate+"','"+AttendanceToDate+"');\" />",
                                             "<Button class='btn btn-primary' id='EditHistory'  onclick=\"EditStaffAttendanceDetails( '" + items.PreRegNum +"','"+ CalcuateWorkedDays(items.PreRegNum,AttendanceToDate,items.TotalWorkedDays)+"','"+items.StaffCategoryForAttendane+"','"+ AttendanceToDate+ "','"+ items.TotalDays+"','"+ items.Campus +"','"+ CalcuateLeaveTaken(items.PreRegNum, AttendanceToDate, (items.TotalDays-items.TotalWorkedDays))  +"');\" >Edit</Button>",
                                             items.TotalAvailableBalance.ToString(),
                                             "<Button class='btn btn-primary btn-xs' height='14' width='25'  id='EditStaffStaffChange'  onclick=\"EditStaffStatusChange( '" + items.PreRegNum +"','"+ items.Campus +"','"+ MonthYear +"','"+ AttendanceToDate + "');\" >Edit Status</Button>",
                                            
                                         }
                                    })
                        };
                        return Json(AttLst, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var AssLst = new { rows = (new { cell = new string[] { } }) };
                        return Json(AssLst, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string BulidTimeFormatStringNew(long Hours, long Minutes, long Seconds)
        {
            string TimeFormat = string.Empty;
            Minutes += Seconds / 60;
            Seconds %= 60;
            Hours += Minutes / 60;
            Minutes %= 60;
            TimeFormat = string.Format("{0:00}:{1:00}:{2:00}", Hours, Minutes, Seconds);
            return TimeFormat;
        }
        public ActionResult ShowStaffAttendanceDetailsNew(string IdNumber, string AttendanceFromDate, string AttendanceToDate)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ViewBag.IdNumber = IdNumber;
                    ViewBag.AttendanceFromDate = AttendanceFromDate;
                    ViewBag.AttendanceToDate = AttendanceToDate;
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw ex;
            }
        }
        //public ActionResult EditStaffBioAttendanceDetails(long PreRegNum, decimal TotalWorkedDays, string StaffType, string AttendanceToDate, long TotalDays, string Campus, decimal NoOfLeaveTaken)
        //{
        //    try
        //    {

        //        BioAttendanceService bioMetric = new BioAttendanceService();
        //        StaffManagementService smsServiceObj = new StaffManagementService();
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            ViewBag.NoOfDaysLeaveTaken = NoOfLeaveTaken;
        //            ViewBag.NoOfworkingDays = TotalDays;
        //            ViewBag.TotalNoOfDaysWorked = TotalWorkedDays;
        //            ViewBag.PreRegNum = PreRegNum;
        //            decimal NoOfHolidays = 0;
        //            DateTime AttendanceDate = new DateTime();
        //            AttendanceDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //            long Month = AttendanceDate.Month;
        //            long Year = AttendanceDate.Year;
        //            ViewBag.AttendanceMonth = Month;
        //            ViewBag.AttendanceYear = Year;
        //            decimal NoOfLeaveTakenAbs = System.Math.Abs(NoOfLeaveTaken);
        //            StaffHolidaysMaster StaffHolidaysObj = new StaffHolidaysMaster();
        //            StaffHolidaysObj = smsServiceObj.GetStaffHolidaysMasterByAcademicYearAndMonthCampus(Year, Month, Campus);
        //            NoOfHolidays = StaffHolidaysObj != null ? StaffHolidaysObj.NoOfHolidays : 0;
        //            ViewBag.Holidays = NoOfHolidays;
        //            ViewBag.TotalNoOfWorkedDaysByHolidays = TotalWorkedDays + NoOfHolidays;
        //            ViewBag.TotalNoOfLeaveTakenByHolidays = NoOfLeaveTakenAbs - NoOfHolidays;


        //            Staff_AttendanceCLDetails StaffAttendanceClObj = new Staff_AttendanceCLDetails();
        //            StaffAttendanceClObj = smsServiceObj.GetStaff_AttendanceCLDetailsByPreRegNum(Month, Year, PreRegNum);
        //            if (StaffAttendanceClObj != null)
        //            {
        //                ViewBag.OpeningBalance = StaffAttendanceClObj.OpeningCLBalance;
        //                ViewBag.AllotedCL = StaffAttendanceClObj.AllotedCL;
        //                ViewBag.TotalAvailableBalance = StaffAttendanceClObj.TotalAvailableBalane;
        //                // decimal NoOfLeaveTakenAbs = System.Math.Abs(NoOfLeaveTaken);
        //                var LeaveToBeCalculated = StaffAttendanceClObj.TotalAvailableBalane - (NoOfLeaveTakenAbs - NoOfHolidays);
        //                ViewBag.LeaveToBeCalculated = LeaveToBeCalculated;
        //                var LeaveTaken = (TotalDays - TotalWorkedDays) - NoOfHolidays;
        //                if (LeaveTaken < StaffAttendanceClObj.TotalAvailableBalane)
        //                {
        //                    ViewBag.LeaveToBeCalculated = 0;
        //                    ViewBag.ClosingBalance = LeaveToBeCalculated;
        //                }
        //                else
        //                {
        //                    ViewBag.LeaveToBeCalculated = LeaveToBeCalculated;
        //                    ViewBag.ClosingBalance = 0;
        //                }


        //            }

        //            return View();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "AttendancePolicy");
        //        throw ex;
        //    }
        //}
        //public ActionResult EditStaffBioAttendanceDetails(long PreRegNum, decimal TotalWorkedDays, string StaffType, string AttendanceToDate, long TotalDays, string Campus, decimal NoOfLeaveTaken)
        //{
        //    try
        //    {

        //        BioAttendanceService bioMetric = new BioAttendanceService();
        //        StaffManagementService smsServiceObj = new StaffManagementService();
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            ViewBag.NoOfDaysLeaveTaken = NoOfLeaveTaken;
        //            ViewBag.NoOfworkingDays = TotalDays;
        //            ViewBag.TotalNoOfDaysWorked = TotalWorkedDays;
        //            ViewBag.PreRegNum = PreRegNum;
        //            decimal NoOfHolidays = 0;
        //            DateTime AttendanceDate = new DateTime();
        //            AttendanceDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //            long Month = AttendanceDate.Month;
        //            long Year = AttendanceDate.Year;
        //            ViewBag.AttendanceMonth = Month;
        //            ViewBag.AttendanceYear = Year;
        //            decimal NoOfLeaveTakenAbs = System.Math.Abs(NoOfLeaveTaken);
        //            StaffHolidaysMaster StaffHolidaysObj = new StaffHolidaysMaster();
        //            StaffHolidaysObj = smsServiceObj.GetStaffHolidaysMasterByAcademicYearAndMonthCampus(Year, Month, Campus);
        //            NoOfHolidays = StaffHolidaysObj != null ? StaffHolidaysObj.NoOfHolidays : 0;
        //            ViewBag.Holidays = NoOfHolidays;
        //            ViewBag.TotalNoOfWorkedDaysByHolidays = TotalWorkedDays + NoOfHolidays;
        //            ViewBag.TotalNoOfLeaveTakenByHolidays = NoOfLeaveTakenAbs - NoOfHolidays;
        //            if ((TotalWorkedDays + NoOfHolidays) < 26)
        //            {
        //                Staff_AttendanceCLDetails StaffAttendanceClObj = new Staff_AttendanceCLDetails();
        //                StaffAttendanceClObj = smsServiceObj.GetStaff_AttendanceCLDetailsByPreRegNum(Month, Year, PreRegNum,true);
        //                if (StaffAttendanceClObj != null)
        //                {
        //                    ViewBag.OpeningBalance = StaffAttendanceClObj.OpeningCLBalance;
        //                    ViewBag.AllotedCL = StaffAttendanceClObj.AllotedCL;
        //                    ViewBag.TotalAvailableBalance = StaffAttendanceClObj.TotalAvailableBalane;
        //                    // decimal NoOfLeaveTakenAbs = System.Math.Abs(NoOfLeaveTaken);
        //                    var LeaveToBeCalculated = StaffAttendanceClObj.TotalAvailableBalane - (NoOfLeaveTakenAbs - NoOfHolidays);
        //                    ViewBag.LeaveToBeCalculated = LeaveToBeCalculated;
        //                    var LeaveTaken = (TotalDays - TotalWorkedDays) - NoOfHolidays;
        //                    if (LeaveTaken < StaffAttendanceClObj.TotalAvailableBalane)
        //                    {
        //                        ViewBag.LeaveToBeCalculated = 0;
        //                        ViewBag.ClosingBalance = LeaveToBeCalculated;
        //                    }
        //                    else
        //                    {
        //                        ViewBag.LeaveToBeCalculated = LeaveToBeCalculated;
        //                        ViewBag.ClosingBalance = 0;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                //StaffHolidaysMaster StaffHolidaysObj = new StaffHolidaysMaster();
        //                //StaffHolidaysObj = smsServiceObj.GetStaffHolidaysMasterByAcademicYearAndMonthCampus(Year, Month, Campus);
        //                //NoOfHolidays = StaffHolidaysObj != null ? StaffHolidaysObj.NoOfHolidays : 0;
        //                //ViewBag.Holidays = NoOfHolidays;
        //                ViewBag.TotalNoOfWorkedDaysByHolidays = TotalWorkedDays + NoOfHolidays;
        //                ViewBag.TotalNoOfLeaveTakenByHolidays = 0;
        //                Staff_AttendanceCLDetails StaffAttendanceClObj = new Staff_AttendanceCLDetails();
        //                StaffAttendanceClObj = smsServiceObj.GetStaff_AttendanceCLDetailsByPreRegNum(Month, Year, PreRegNum,true);
        //                if (StaffAttendanceClObj != null)
        //                {
        //                    ViewBag.OpeningBalance = StaffAttendanceClObj.OpeningCLBalance;
        //                    ViewBag.AllotedCL = StaffAttendanceClObj.AllotedCL;
        //                    ViewBag.TotalAvailableBalance = StaffAttendanceClObj.TotalAvailableBalane;
        //                    ViewBag.LeaveToBeCalculated = 0;
        //                    ViewBag.ClosingBalance = StaffAttendanceClObj.TotalAvailableBalane;
        //                }

        //            }
        //            return View();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "AttendancePolicy");
        //        throw ex;
        //    }
        //}
        public ActionResult EditStaffBioAttendanceDetails(long PreRegNum, decimal TotalWorkedDays, string StaffType, string AttendanceToDate, long TotalDays, string Campus, decimal NoOfLeaveTaken)
        {
            try
            {

                BioAttendanceService bioMetric = new BioAttendanceService();
                StaffManagementService smsServiceObj = new StaffManagementService();
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ViewBag.NoOfDaysLeaveTaken = NoOfLeaveTaken;
                    ViewBag.NoOfworkingDays = TotalDays;
                    ViewBag.TotalNoOfDaysWorked = TotalWorkedDays;
                    ViewBag.PreRegNum = PreRegNum;
                    decimal NoOfHolidays = 0;
                    DateTime AttendanceDate = new DateTime();
                    AttendanceDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    long Month = AttendanceDate.Month;
                    long Year = AttendanceDate.Year;
                    ViewBag.AttendanceMonth = Month;
                    ViewBag.AttendanceYear = Year;
                    Staff_AttendanceChangeDetails StaffChangeDetailsObj = new Staff_AttendanceChangeDetails();
                    StaffChangeDetailsObj = smsServiceObj.GetStaff_AttendanceChangeDetailsByPreRegNumAndMonthYear(PreRegNum, Month, Year, true);
                    if (StaffChangeDetailsObj == null)
                    {

                        decimal NoOfLeaveTakenAbs = System.Math.Abs(NoOfLeaveTaken);
                        StaffHolidaysMaster StaffHolidaysObj = new StaffHolidaysMaster();
                        StaffHolidaysObj = smsServiceObj.GetStaffHolidaysMasterByAcademicYearAndMonthCampus(Year, Month, Campus);
                        NoOfHolidays = StaffHolidaysObj != null ? StaffHolidaysObj.NoOfHolidays : 0;
                        ViewBag.Holidays = NoOfHolidays;
                        ViewBag.TotalNoOfWorkedDaysByHolidays = TotalWorkedDays + NoOfHolidays;
                        ViewBag.TotalNoOfLeaveTakenByHolidays = (TotalDays - TotalWorkedDays - NoOfHolidays);
                        if ((TotalWorkedDays + NoOfHolidays) <= 26)
                        {
                            CLDetailsMaster CLDetailsMasterClObj = new CLDetailsMaster();
                            CLDetailsMasterClObj = smsServiceObj.GetCLDetailsMasterByPreRegNum(Month, Year, PreRegNum);
                            if (CLDetailsMasterClObj != null)
                            {
                                ViewBag.OpeningBalance = CLDetailsMasterClObj.OpeningBalance;
                                ViewBag.AllotedCL = CLDetailsMasterClObj.AllotedCL;
                                ViewBag.TotalAvailableBalance = CLDetailsMasterClObj.CLInHands;
                                // decimal NoOfLeaveTakenAbs = System.Math.Abs(NoOfLeaveTaken);
                                var LeaveToBeCalculated = CLDetailsMasterClObj.CLInHands - ((TotalDays - TotalWorkedDays) - NoOfHolidays);
                                ViewBag.LeaveToBeCalculated = LeaveToBeCalculated;
                                var LeaveTaken = (TotalDays - TotalWorkedDays) - NoOfHolidays;
                                if (LeaveTaken < CLDetailsMasterClObj.CLInHands)
                                {
                                    ViewBag.LeaveToBeCalculated = 0;
                                    ViewBag.ClosingBalance = LeaveToBeCalculated;
                                }
                                else
                                {
                                    ViewBag.LeaveToBeCalculated = LeaveToBeCalculated;
                                    ViewBag.ClosingBalance = 0;
                                }
                            }
                        }
                        else
                        {
                            //StaffHolidaysMaster StaffHolidaysObj = new StaffHolidaysMaster();
                            //StaffHolidaysObj = smsServiceObj.GetStaffHolidaysMasterByAcademicYearAndMonthCampus(Year, Month, Campus);
                            //NoOfHolidays = StaffHolidaysObj != null ? StaffHolidaysObj.NoOfHolidays : 0;
                            //ViewBag.Holidays = NoOfHolidays;
                            ViewBag.TotalNoOfWorkedDaysByHolidays = TotalWorkedDays + NoOfHolidays;
                            ViewBag.TotalNoOfLeaveTakenByHolidays = 0;
                            CLDetailsMaster CLDetailsMasterClObj = new CLDetailsMaster();
                            CLDetailsMasterClObj = smsServiceObj.GetCLDetailsMasterByPreRegNum(Month, Year, PreRegNum);
                            if (CLDetailsMasterClObj != null)
                            {
                                ViewBag.OpeningBalance = CLDetailsMasterClObj.OpeningBalance;
                                ViewBag.AllotedCL = CLDetailsMasterClObj.AllotedCL;
                                ViewBag.TotalAvailableBalance = CLDetailsMasterClObj.CLInHands;
                                ViewBag.LeaveToBeCalculated = 0;
                                ViewBag.ClosingBalance = CLDetailsMasterClObj.CLInHands;
                            }

                        }
                    }
                    else
                    {
                        ViewBag.TotalNoOfWorkedDaysByHolidays = StaffChangeDetailsObj.TotalNoOfDaysWorkedByChange;
                        ViewBag.TotalNoOfLeaveTakenByHolidays = StaffChangeDetailsObj.TotalNoOfLeavesTakenByChange;
                        ViewBag.OnDuty = StaffChangeDetailsObj.OnDuty;
                        ViewBag.PermissionInHours = StaffChangeDetailsObj.NoOfPermissionsTaken;
                        ViewBag.TotalNoOfPermission = StaffChangeDetailsObj.NoOfLeavesCalculatedByPermissions;
                        StaffHolidaysMaster StaffHolidaysObj = new StaffHolidaysMaster();
                        StaffHolidaysObj = smsServiceObj.GetStaffHolidaysMasterByAcademicYearAndMonthCampus(Year, Month, Campus);
                        NoOfHolidays = StaffHolidaysObj != null ? StaffHolidaysObj.NoOfHolidays : 0;
                        ViewBag.Holidays = NoOfHolidays;
                        CLDetailsMaster CLDetailsMasterClObj = new CLDetailsMaster();
                        CLDetailsMasterClObj = smsServiceObj.GetCLDetailsMasterByPreRegNum(Month, Year, PreRegNum);
                        if (CLDetailsMasterClObj != null)
                        {
                            ViewBag.OpeningBalance = CLDetailsMasterClObj.OpeningBalance;
                            ViewBag.AllotedCL = CLDetailsMasterClObj.AllotedCL;
                            ViewBag.TotalAvailableBalance = CLDetailsMasterClObj.CLInHands;
                        }
                        Staff_AttendanceCLDetails StaffAttendanceCLDetailsOBj = new Staff_AttendanceCLDetails();
                        StaffAttendanceCLDetailsOBj = smsServiceObj.GetStaff_AttendanceCLDetailsByPreRegNum(Month, Year, PreRegNum, true);
                        if (StaffAttendanceCLDetailsOBj != null)
                        {

                            ViewBag.ClosingBalance = StaffAttendanceCLDetailsOBj.ClosingBalance;
                            ViewBag.LeaveToBeCalculated = StaffAttendanceCLDetailsOBj.LeaveToBeCalculated;
                            ViewBag.Remarks = StaffAttendanceCLDetailsOBj.Remarks;
                        }
                        ViewBag.Exist = 1;
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw ex;
            }
        }
        public decimal CalcuateWorkedDays(long PreRegNum, string AttendanceToDate, decimal TotalNoOfWorkedDays)
        {
            StaffManagementService smsServiceObj = new StaffManagementService();
            DateTime AttendanceDate = new DateTime();
            AttendanceDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            long Month = AttendanceDate.Month;
            long Year = AttendanceDate.Year;
            Staff_AttendanceChangeDetails obj = new Staff_AttendanceChangeDetails();
            obj = smsServiceObj.GetStaff_AttendanceChangeDetailsByPreRegNumAndMonthYear(PreRegNum, Month, Year, true);
            if (obj == null)
            {
                return TotalNoOfWorkedDays;
            }
            else
            {
                return obj.TotalNoOfDaysWorkedByChange;
            }

        }
        public decimal CalcuateLeaveTaken(long PreRegNum, string AttendanceToDate, decimal NoOfLeaveTaken)
        {
            StaffManagementService smsServiceObj = new StaffManagementService();
            DateTime AttendanceDate = new DateTime();
            AttendanceDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            long Month = AttendanceDate.Month;
            long Year = AttendanceDate.Year;
            Staff_AttendanceCLDetails StaffAttendanceClObj = new Staff_AttendanceCLDetails();
            StaffAttendanceClObj = smsServiceObj.GetStaff_AttendanceCLDetailsByPreRegNum(Month, Year, PreRegNum, true);
            if (NoOfLeaveTaken < 0)
            {
                return 0;
            }
            if (StaffAttendanceClObj == null)
            {

                return NoOfLeaveTaken;
            }
            else if (StaffAttendanceClObj.ModifiedDate == null)
            {
                return NoOfLeaveTaken;
            }
            else
            {

                return StaffAttendanceClObj.LeaveToBeCalculated ?? 0;
            }
        }
        #endregion
        #region Staff Daily Attendance Summary New
        public ActionResult StaffDailyAttendanceIOSummaryNewJqGrid(string IdNumber, string AttendanceFromDate, string AttendanceToDate, string MonthYear, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                BioAttendanceService bioMetric = new BioAttendanceService();
                DateTime StartDate = new DateTime();
                DateTime ToDate = new DateTime();
                if (!string.IsNullOrEmpty(MonthYear))
                {
                    DateTime[] fromto = new DateTime[2];
                    fromto = getFromTodateByMonthAndYear(MonthYear);
                    AttendanceFromDate = fromto[0].ToString("dd/MM/yyyy");
                    AttendanceToDate = fromto[1].ToString("dd/MM/yyyy");
                }
                if (!string.IsNullOrEmpty(AttendanceFromDate) && !string.IsNullOrEmpty(AttendanceToDate))
                {
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    //StartDate = DateTime.Parse(AttendanceFromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    //ToDate = DateTime.Parse(AttendanceToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    StartDate = DateTime.ParseExact(AttendanceFromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    ToDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                var SearchDateList = Enumerable.Range(0, 1 + ToDate.Subtract(StartDate).Days).Select(offset => StartDate.AddDays(offset)).ToArray();
                var MonthAndYearGroup = (from p in SearchDateList
                                         group p by new { month = p.Date.Month, year = p.Date.Year } into d
                                         select d).ToList();
                List<Staff_DeviceLogSummarySP> TotalLogCollections = new List<Staff_DeviceLogSummarySP>();
                IList<Staff_DeviceLogSummarySP> DeviceLogsList = new List<Staff_DeviceLogSummarySP>();
                IList<Staff_DeviceLogSummarySP> ConsolidateLogList = new List<Staff_DeviceLogSummarySP>();
                for (int i = 0; i < MonthAndYearGroup.Count; i++)
                {
                    var SingleMonthYear = (from u in MonthAndYearGroup[i] select u).ToList();
                    Dictionary<long, IList<Staff_DeviceLogSummarySP>> BioMetricAttendanceDetails = null;
                    BioMetricAttendanceDetails = bioMetric.GetStaffDeviceLogSummaryDetails(string.Empty, IdNumber, string.Empty, string.Empty, SingleMonthYear[0].Date.ToString("dd/MM/yyyy 00:00:01"), SingleMonthYear[SingleMonthYear.Count - 1].Date.ToString("dd/MM/yyyy 23:59:59"));
                    if (BioMetricAttendanceDetails != null && BioMetricAttendanceDetails.FirstOrDefault().Key > 0)
                    {
                        var SinleMonthLogList = (from u in BioMetricAttendanceDetails.FirstOrDefault().Value
                                                 where u.AttendanceDate != null
                                                 select u).ToList();
                        TotalLogCollections.AddRange(SinleMonthLogList);
                    }
                }
                if (TotalLogCollections != null && TotalLogCollections.Count > 0)
                {
                    ConsolidateLogList = (from c in TotalLogCollections
                                          group c by new
                                          {
                                              c.PreRegNum,
                                              c.Name,
                                              c.IdNumber,
                                              c.Campus,
                                              c.StaffType,
                                              c.Department,
                                              c.Designation,
                                              c.Programme,
                                              c.AttendanceDate,
                                              c.LogInTime,
                                              c.LogOutTime,
                                              c.WorkingHours
                                          } into gcs
                                          select new Staff_DeviceLogSummarySP()
                                          {
                                              PreRegNum = gcs.Key.PreRegNum,
                                              Name = gcs.Key.Name,
                                              IdNumber = gcs.Key.IdNumber,
                                              Campus = gcs.Key.Campus,
                                              StaffType = gcs.Key.StaffType,
                                              Department = gcs.Key.Department,
                                              Designation = gcs.Key.Designation,
                                              Programme = gcs.Key.Programme,
                                              AttendanceDate = gcs.Key.AttendanceDate,
                                              LogInTime = gcs.Key.LogInTime,
                                              LogOutTime = gcs.Key.LogOutTime,
                                              WorkingHours = gcs.Key.WorkingHours


                                          }).ToList();
                }
                if (ExportType == "Excel")
                {
                    string ExcelFileName = "Staff_Daily_IO_Summary_From_" + AttendanceFromDate + "_To_" + AttendanceToDate + "_On_" + DateTime.Now.ToString("dd/MM/yyyy");
                    ExptToXL(TotalLogCollections, ExcelFileName, (items => new
                    {
                        items.Campus,
                        items.Name,
                        items.IdNumber,
                        AttendanceDate = items.AttendanceDate != null ? items.AttendanceDate.Value.ToString("dd/MM/yyyy") : string.Empty,
                        Group = items.Programme,
                        items.Designation,
                        LogInTime = items.LogInTime != null ? items.LogInTime.Value.ToString("HH:mm:ss") : "00:00:00",
                        LogOutTime = items.LogOutTime != null ? items.LogOutTime.Value.ToString("HH:mm:ss") : "00:00:00",
                        items.WorkingHours
                    }));
                    return new EmptyResult();
                }
                else
                {
                    //IList<UserInfo> UserInfoList = client.UserInfo(Name, rows, sidx, sord, page - 1).ToList<UserInfo>();
                    if (ConsolidateLogList != null && ConsolidateLogList.Count > 0)
                    {
                        long totalrecords = ConsolidateLogList.Count;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var AttLst = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in ConsolidateLogList
                                    select new
                                    {
                                        cell = new string[] 
                                         {
                                             items.Id.ToString(),
                                             items.Campus,
                                             items.Name,
                                             items.IdNumber,
                                             items.AttendanceDate!=null?items.AttendanceDate.Value.ToString("dd/MM/yyyy"):string.Empty,
                                             items.Department,
                                             items.Programme,
                                             items.Designation,
                                             items.LogInTime!=null?items.LogInTime.Value.ToString("HH:mm:ss"):"00:00:00",
                                             items.LogOutTime!=null?items.LogOutTime.Value.ToString("HH:mm:ss"):"00:00:00",
                                             items.WorkingHours,
                                             "<img src='/Images/History.png ' id='ImgHistory' onclick=\"ShowStaffAttendanceInOutStatusNew('" + items.IdNumber +"','"+AttendanceFromDate+"','"+AttendanceToDate+"','"+ items.AttendanceDate.Value.ToString("dd/MM/yyyy") +"');\" />",
                                         }
                                    })
                        };
                        return Json(AttLst, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var AssLst = new { rows = (new { cell = new string[] { } }) };
                        return Json(AssLst, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        public decimal LeaveToBeCalculated(string Campus, long PreRegNum, string AttendanceToDate, decimal NoOfLeaveTaken)
        {

            decimal NoOfHolidays = 0;
            StaffManagementService smsServiceObj = new StaffManagementService();
            DateTime AttendanceDate = new DateTime();
            AttendanceDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            long Month = AttendanceDate.Month;
            long Year = AttendanceDate.Year;
            Staff_AttendanceCLDetails StaffAttendanceClObj = new Staff_AttendanceCLDetails();
            StaffAttendanceClObj = smsServiceObj.GetStaff_AttendanceCLDetailsByPreRegNum(Month, Year, PreRegNum, true);
            StaffHolidaysMaster StaffHolidaysObj = new StaffHolidaysMaster();
            StaffHolidaysObj = smsServiceObj.GetStaffHolidaysMasterByAcademicYearAndMonthCampus(Year, Month, Campus);
            Staff_AttendanceChangeDetails Staff_AttendanceChangeDetailsobj = smsServiceObj.GetStaff_AttendanceChangeDetailsByPreRegNumAndMonthYear(PreRegNum, Month, Year, true);
            NoOfHolidays = StaffHolidaysObj != null ? StaffHolidaysObj.NoOfHolidays : 0;
            if (Staff_AttendanceChangeDetailsobj != null)
            {
                return StaffAttendanceClObj.LeaveToBeCalculated ?? 0;
            }
            else
            {
                if (NoOfLeaveTaken < 0)
                {
                    return 0;
                }
                if (StaffAttendanceClObj != null)
                {

                    var LeaveTaken = StaffAttendanceClObj.TotalAvailableBalane - (NoOfLeaveTaken - NoOfHolidays);
                    if (LeaveTaken > 0)
                        return 0;
                    else return LeaveTaken;
                }
                else
                {
                    return 0;
                }
            }
        }
        public decimal ClosingBalance(string Campus, long PreRegNum, string AttendanceToDate, decimal NoOfLeaveTaken, decimal NoOfWorkedDays)
        {
            decimal NoOfHolidays = 0;
            StaffManagementService smsServiceObj = new StaffManagementService();
            DateTime AttendanceDate = new DateTime();
            AttendanceDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            long Month = AttendanceDate.Month;
            long Year = AttendanceDate.Year;
            Staff_AttendanceCLDetails StaffAttendanceClObj = new Staff_AttendanceCLDetails();
            StaffAttendanceClObj = smsServiceObj.GetStaff_AttendanceCLDetailsByPreRegNum(Month, Year, PreRegNum, true);
            StaffHolidaysMaster StaffHolidaysObj = new StaffHolidaysMaster();
            StaffHolidaysObj = smsServiceObj.GetStaffHolidaysMasterByAcademicYearAndMonthCampus(Year, Month, Campus);
            NoOfHolidays = StaffHolidaysObj != null ? StaffHolidaysObj.NoOfHolidays : 0;
            Staff_AttendanceChangeDetails Staff_AttendanceChangeDetailsobj = smsServiceObj.GetStaff_AttendanceChangeDetailsByPreRegNumAndMonthYear(PreRegNum, Month, Year, true);
            if (Staff_AttendanceChangeDetailsobj != null)
            {
                return StaffAttendanceClObj.ClosingBalance ?? 0;
            }
            else
            {
                if (NoOfWorkedDays + NoOfHolidays <= 26)
                {

                    if (StaffAttendanceClObj != null)
                    {
                        var LeaveToBeCalculated = StaffAttendanceClObj.TotalAvailableBalane - (NoOfLeaveTaken - NoOfHolidays);
                        var LeaveTaken = NoOfLeaveTaken - NoOfHolidays;
                        if (LeaveTaken < StaffAttendanceClObj.TotalAvailableBalane)
                        {
                            return LeaveToBeCalculated;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {

                    return StaffAttendanceClObj != null ? StaffAttendanceClObj.TotalAvailableBalane : 0;
                }
            }
        }
        public decimal OpeningBalance(long PreRegNum, string AttendanceToDate)
        {
            StaffManagementService smsServiceObj = new StaffManagementService();
            DateTime AttendanceDate = new DateTime();
            AttendanceDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            long Month = AttendanceDate.Month;
            long Year = AttendanceDate.Year;
            Staff_AttendanceCLDetails StaffAttendanceClObj = new Staff_AttendanceCLDetails();
            StaffAttendanceClObj = smsServiceObj.GetStaff_AttendanceCLDetailsByPreRegNum(Month, Year, PreRegNum, true);

            if (StaffAttendanceClObj != null)
            {
                return StaffAttendanceClObj.TotalAvailableBalane;
            }
            else
            {
                return 0;
            }
        }
        public string GetRemarks(long PreRegNum, string AttendanceToDate)
        {
            StaffManagementService smsServiceObj = new StaffManagementService();
            DateTime AttendanceDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            long Month = AttendanceDate.Month;
            long Year = AttendanceDate.Year;
            Staff_AttendanceCLDetails StaffAttendanceCLObj = new Staff_AttendanceCLDetails();
            StaffAttendanceCLObj = smsServiceObj.GetStaff_AttendanceCLDetailsByPreRegNum(Month, Year, PreRegNum, true);
            return StaffAttendanceCLObj != null ? StaffAttendanceCLObj.Remarks : null;
        }
        public ActionResult ColumnHide(string AttendanceDate, string MonthYear)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                string[] MonthAndYearArray = new string[2];
                if (!string.IsNullOrEmpty(MonthYear))
                {
                    MonthAndYearArray = MonthYear.Split('-');
                    long SearchMonth = Convert.ToInt64(MonthAndYearArray[0]);
                    long SearchYear = Convert.ToInt64(MonthAndYearArray[1]);
                    Staff_AttendanceOpeningBalanceGenerateCount obj = new Staff_AttendanceOpeningBalanceGenerateCount();
                    obj = sms.GetStaffAttendanceOpeningBalanceGenerateCountByMonth(SearchMonth, SearchYear);
                    CLDetailsMaster ExctObj = new CLDetailsMaster();
                    ExctObj = sms.GetCLDetailsMasterByMonthYear(SearchMonth, SearchYear);
                    //Staff_AttendanceOpeningBalanceGenerateCount Extobj = new Staff_AttendanceOpeningBalanceGenerateCount();
                    //Extobj = sms.GetStaffAttendanceOpeningBalanceGenerateCountByMonthAndYear(SearchMonth, SearchMonth);
                    if (obj != null)
                    {
                        if (ExctObj == null)
                        {
                            return Json("Hide", JsonRequestBehavior.AllowGet);
                        }
                        return Json("Hide", JsonRequestBehavior.AllowGet);
                    }
                    if (ExctObj == null)
                    {
                        return Json("Hide", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Show", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    DateTime Date = DateTime.ParseExact(AttendanceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    long Month = Date.Month;
                    long Year = Date.Year;
                    Staff_AttendanceOpeningBalanceGenerateCount obj = new Staff_AttendanceOpeningBalanceGenerateCount();
                    obj = sms.GetStaffAttendanceOpeningBalanceGenerateCountByMonth(Month, Year);
                    CLDetailsMaster ExctObj = new CLDetailsMaster();
                    ExctObj = sms.GetCLDetailsMasterByMonthYear(Month, Year);
                    if (obj != null)
                    {
                        if (ExctObj == null)
                        {
                            return Json("Hide", JsonRequestBehavior.AllowGet);
                        }
                        return Json("Hide", JsonRequestBehavior.AllowGet);
                    }
                    if (ExctObj == null)
                    {
                        return Json("Hide", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Show", JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(null, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                throw;
            }

        }
        public ActionResult CalculateClosingBalanceGenerate(string MonthYear)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                string UserId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(UserId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    DateTime ToDate = new DateTime();
                    DateTime StartDate = new DateTime();
                    DateTime? FromDate = null;
                    long NoOfDaysWeekOff = Convert.ToInt64(ConfigurationManager.AppSettings["NoOfDaysWeekOff"]); IList<CLDetailsMaster> cldList = new List<CLDetailsMaster>();
                    IList<Staff_AttendanceCLDetails> AttClDetailsList = new List<Staff_AttendanceCLDetails>();
                    string[] AttMonthYear = MonthYear.Split('-');
                    FromDate = DateTime.ParseExact(MonthYear, "MM-yyyy", CultureInfo.InvariantCulture);
                    DateTime AttendanceDate = new DateTime();
                    AttendanceDate = DateTime.ParseExact(Convert.ToString(FromDate), "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    long Month = AttendanceDate.Month;
                    long year = AttendanceDate.Year;
                    CLDetailsMaster ClDetailsMasterObj = new CLDetailsMaster();
                    ClDetailsMasterObj = sms.GetCLDetailsMasterByMonthYear(Month, year);
                    if (ClDetailsMasterObj != null)
                    {
                        return Json("Failed", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //GeneratExcelReort(MonthYear);                        
                        string Months = Convert.ToString(AttendanceDate.AddMonths(-1));
                        DateTime LastMonth = DateTime.ParseExact(Months, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        long LstMonth = LastMonth.Month;
                        long LstYear = LastMonth.Year;
                        criteria.Add("Month", LstMonth);
                        criteria.Add("Year", LstYear);
                        Dictionary<long, IList<CLDetailsMaster>> ClDetailsListObj = sms.GetCLDetailsMasterListWithPagingAndCriteria(0, 99999, null, null, criteria);
                        if (ClDetailsListObj != null && ClDetailsListObj.FirstOrDefault().Key > 0)
                        {
                            var CLDetailsMasterListObj = (from u in ClDetailsListObj.FirstOrDefault().Value
                                                          where u.PreRegNum > 0
                                                          select u).Distinct().ToList();
                            criteria.Clear();
                            foreach (var item in CLDetailsMasterListObj)
                            {
                                List<Staff_ConsolidateDeviceLogSummary_SP> TotalLogCollections = new List<Staff_ConsolidateDeviceLogSummary_SP>();
                                IList<Staff_ConsolidateDeviceLogSummary_SP> DeviceLogsList = new List<Staff_ConsolidateDeviceLogSummary_SP>();
                                IList<Staff_ConsolidateDeviceLogSummary_SP> ConsolidateLogList = new List<Staff_ConsolidateDeviceLogSummary_SP>();
                                List<Staff_ConsolidateDeviceLogSummary_SP> TotalLogCollectionsForResigned = new List<Staff_ConsolidateDeviceLogSummary_SP>();
                                IList<Staff_ConsolidateDeviceLogSummary_SP> FinalConsolidateList = new List<Staff_ConsolidateDeviceLogSummary_SP>();
                                // StaffDetails StaffDetailsObj = new StaffDetails();
                                criteria.Add("PreRegNum", Convert.ToInt32(item.PreRegNum));
                                Dictionary<long, IList<StaffDetails>> StaffDetailsObjList = sms.GetStaffDetailsListWithPaging(0, 99999, null, null, criteria);
                                if (StaffDetailsObjList != null && StaffDetailsObjList.FirstOrDefault().Key > 0)
                                {
                                    var StaffDetailsObj = (from u in StaffDetailsObjList.FirstOrDefault().Value
                                                           where u.PreRegNum == item.PreRegNum && u.CurrentStatus == "Registered" || u.CurrentStatus == "LongLeave"
                                                           select u).Distinct().ToList();
                                    if (StaffDetailsObj != null && StaffDetailsObj.Count() > 0)
                                    {
                                        CLDetailsMaster clDetailsExtObj = new CLDetailsMaster();
                                        Staff_AttendanceCLDetails StaffAttClDetailsExtObj = new Staff_AttendanceCLDetails();
                                        clDetailsExtObj.PreRegNum = StaffDetailsObj.FirstOrDefault().PreRegNum;
                                        clDetailsExtObj.Month = Month;
                                        clDetailsExtObj.Year = year;
                                        clDetailsExtObj.Status = StaffDetailsObj.FirstOrDefault().CurrentStatus;
                                        if (item.ClosingBalance != null)
                                        {
                                            clDetailsExtObj.OpeningBalance = item.ClosingBalance ?? 0;
                                        }
                                        else
                                        {
                                            var m = new DateTime(Convert.ToInt32(AttMonthYear[1]), Convert.ToInt32(AttMonthYear[0]), 1);
                                            var Mm = m.AddMonths(-2);
                                            var FebMonthDays = DateTime.DaysInMonth(Convert.ToInt32(AttMonthYear[1]), Convert.ToInt32(Mm.ToString("MM")));
                                            //var FebMonthDays = DateTime.DaysInMonth(Convert.ToInt32(AttMonthYear[1]), Convert.ToInt32(AttMonthYear[0]) - 2);
                                            string AttendanceFromDate = string.Empty;
                                            string AttendanceToDate = string.Empty;
                                            if (Convert.ToInt32(AttMonthYear[0]) == 4)
                                            {
                                                if (FebMonthDays == 28)
                                                {
                                                    var Lstpremonth = new DateTime(Convert.ToInt32(AttMonthYear[1]), Convert.ToInt32(AttMonthYear[0]), 1);
                                                    var PreviousMonth = Lstpremonth.AddMonths(-1);
                                                    AttendanceToDate = "28" + "/" + PreviousMonth.ToString("MM") + "/" + PreviousMonth.Year;
                                                    ToDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                    var month = new DateTime(Convert.ToInt32(AttMonthYear[1]), Convert.ToInt32(AttMonthYear[0]), 1);
                                                    var PreMonth = month.AddMonths(-2);
                                                    AttendanceFromDate = "01" + "/" + PreMonth.ToString("MM") + "/" + PreMonth.Year;
                                                    StartDate = DateTime.ParseExact(AttendanceFromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                }
                                                else
                                                {
                                                    var Lstpremonth = new DateTime(Convert.ToInt32(AttMonthYear[1]), Convert.ToInt32(AttMonthYear[0]), 1);
                                                    var PreviousMonth = Lstpremonth.AddMonths(-1);
                                                    AttendanceToDate = "28" + "/" + PreviousMonth.ToString("MM") + "/" + PreviousMonth.Year;
                                                    ToDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                    var month = new DateTime(Convert.ToInt32(AttMonthYear[1]), Convert.ToInt32(AttMonthYear[0]), 1);
                                                    var PreMonth = month.AddMonths(-2);
                                                    AttendanceFromDate = "29" + "/" + PreMonth.ToString("MM") + "/" + PreMonth.Year;
                                                    StartDate = DateTime.ParseExact(AttendanceFromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                }
                                            }
                                            else
                                            {
                                                var Lstpremonth = new DateTime(Convert.ToInt32(AttMonthYear[1]), Convert.ToInt32(AttMonthYear[0]), 1);
                                                var PreviousMonth = Lstpremonth.AddMonths(-1);
                                                AttendanceToDate = "28" + "/" + PreviousMonth.ToString("MM") + "/" + PreviousMonth.Year;
                                                ToDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                var month = new DateTime(Convert.ToInt32(AttMonthYear[1]), Convert.ToInt32(AttMonthYear[0]), 1);
                                                var PreMonth = month.AddMonths(-2);
                                                AttendanceFromDate = "29" + "/" + PreMonth.ToString("MM") + "/" + PreMonth.Year;
                                                StartDate = DateTime.ParseExact(AttendanceFromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                            }
                                            if (!string.IsNullOrEmpty(AttendanceFromDate) && !string.IsNullOrEmpty(AttendanceToDate) && string.IsNullOrEmpty(MonthYear))
                                            {
                                                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                                                StartDate = DateTime.ParseExact(AttendanceFromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                ToDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                            }
                                            var SearchDateList = Enumerable.Range(0, 1 + ToDate.Subtract(StartDate).Days).Select(offset => StartDate.AddDays(offset)).ToArray();
                                            var MonthAndYearGroup = (from p in SearchDateList
                                                                     group p by new { month = p.Date.Month, year = p.Date.Year } into d
                                                                     select d).ToList();
                                            string tempStaffType = "Teaching,Non Teaching-Admin";
                                            string[] arrayStaffType = tempStaffType.Split(',');
                                            string StaffType = arrayStaffType[0] + "','" + arrayStaffType[1];
                                            string tempStatus = "Registered,LongLeave,Others,Resigned";
                                            string[] arrStatus = tempStatus.Split(',');
                                            string StaffStatus = arrStatus[0] + "','" + arrStatus[1] + "','" + arrStatus[2] + "','" + arrStatus[3];
                                            for (int i = 0; i < MonthAndYearGroup.Count; i++)
                                            {
                                                var SingleMonthYear = (from u in MonthAndYearGroup[i] select u).ToList();
                                                Dictionary<long, IList<Staff_ConsolidateDeviceLogSummary_SP>> ConsolidateList = null;
                                                ConsolidateList = bioMetricSvc.GetStaffConsolidateDeviceLogSummaryDetails(StaffDetailsObj.FirstOrDefault().Campus, StaffDetailsObj.FirstOrDefault().IdNumber, string.Empty, "Teaching','Non Teaching-Admin", string.Empty, "Registered','LongLeave','Others','Resigned", SingleMonthYear[0].Date.ToString("dd/MM/yyyy 00:00:01"), SingleMonthYear[SingleMonthYear.Count - 1].Date.ToString("dd/MM/yyyy 23:59:59"));
                                                if (ConsolidateList != null && ConsolidateList.FirstOrDefault().Key > 0)
                                                {
                                                    var SinleMonthLogList = (from u in ConsolidateList.FirstOrDefault().Value
                                                                             where u.CurrentStatus == "Registered" || u.CurrentStatus == "LongLeave"
                                                                             select u).ToList();
                                                    TotalLogCollections.AddRange(SinleMonthLogList);
                                                }
                                                var SingMonthListForResigned = (from a in ConsolidateList.FirstOrDefault().Value
                                                                                where a.CurrentStatus == "Resigned"
                                                                                select a).ToList();
                                                TotalLogCollectionsForResigned.AddRange(SingMonthListForResigned);
                                            }
                                            FinalConsolidateList = ConsolidateLogListForAttendance(TotalLogCollections, TotalLogCollectionsForResigned, AttendanceFromDate, AttendanceToDate);
                                            if (FinalConsolidateList != null && FinalConsolidateList.Count > 0)
                                            {
                                                clDetailsExtObj.OpeningBalance = ClosingBalance(StaffDetailsObj.FirstOrDefault().Campus, StaffDetailsObj.FirstOrDefault().PreRegNum, AttendanceToDate, CalcuateLeaveTaken(StaffDetailsObj.FirstOrDefault().PreRegNum, AttendanceToDate, (FinalConsolidateList.FirstOrDefault().TotalDays - FinalConsolidateList.FirstOrDefault().TotalWorkedDays)),
                                                                                    CalcuateWorkedDays(StaffDetailsObj.FirstOrDefault().PreRegNum, AttendanceToDate, FinalConsolidateList.FirstOrDefault().TotalWorkedDays));
                                            }
                                            else
                                            {
                                                if (item.ClosingBalance != null)
                                                {
                                                    clDetailsExtObj.OpeningBalance = item.ClosingBalance ?? 0;
                                                }
                                                else
                                                {
                                                    clDetailsExtObj.OpeningBalance = item.CLInHands;
                                                }
                                            }
                                        }
                                        clDetailsExtObj.AllotedCL = (Month == 11) ? 0 : (Month == 12) ? 0 : 1;
                                        clDetailsExtObj.CLInHands = clDetailsExtObj.OpeningBalance + clDetailsExtObj.AllotedCL;
                                        clDetailsExtObj.CreatedBy = UserId;
                                        clDetailsExtObj.CreatedDate = DateTime.Now;
                                        clDetailsExtObj.IsActive = true;
                                        StaffAttClDetailsExtObj.PreRegNum = StaffDetailsObj.FirstOrDefault().PreRegNum;
                                        StaffAttClDetailsExtObj.Month = Month;
                                        StaffAttClDetailsExtObj.Year = year;
                                        StaffAttClDetailsExtObj.OpeningCLBalance = clDetailsExtObj.OpeningBalance;
                                        StaffAttClDetailsExtObj.AllotedCL = (Month == 11) ? 0 : (Month == 12) ? 0 : 1;
                                        StaffAttClDetailsExtObj.TotalAvailableBalane = StaffAttClDetailsExtObj.OpeningCLBalance + StaffAttClDetailsExtObj.AllotedCL;
                                        StaffAttClDetailsExtObj.IsActive = true;
                                        StaffAttClDetailsExtObj.CreatedBy = UserId;
                                        StaffAttClDetailsExtObj.CreatedDate = DateTime.Now;
                                        cldList.Add(clDetailsExtObj);
                                        AttClDetailsList.Add(StaffAttClDetailsExtObj);
                                    }
                                }
                                criteria.Remove("PreRegNum");
                            }
                            if (cldList.Count > 0 && AttClDetailsList.Count > 0)
                            {

                                sms.SaveOrUpdateCLDetailsMasterByList(cldList);
                                sms.SaveOrUpdateStaffAttendanceCLDetailsByList(AttClDetailsList);
                                string AttendanceFromDate = string.Empty;
                                string AttendanceToDate = string.Empty;
                                Staff_AttendanceOpeningBalanceGenerateCount obj = new Staff_AttendanceOpeningBalanceGenerateCount();
                                BioAttendanceService bioMetric = new BioAttendanceService();
                                if (!string.IsNullOrEmpty(MonthYear))
                                {
                                    string[] AttDate = MonthYear.Split('-');

                                    if (Convert.ToInt32(AttDate[0]) == 3)
                                    {
                                        var m = new DateTime(Convert.ToInt32(AttMonthYear[1]), Convert.ToInt32(AttMonthYear[0]), 1);
                                        var Mm = m.AddMonths(-1);
                                        var FebMonthDays = DateTime.DaysInMonth(Convert.ToInt32(AttMonthYear[1]), Convert.ToInt32(Mm.ToString("MM")));
                                        if (FebMonthDays == 28)
                                        {
                                            AttendanceToDate = "28" + "/" + FebMonthDays.ToString("MM") + "/" + Mm.Year;
                                            ToDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                            var month = new DateTime(Convert.ToInt32(AttDate[1]), Convert.ToInt32(AttDate[0]), 1);
                                            var LMonth = month.AddMonths(-1);
                                            AttendanceFromDate = "01" + "/" + LMonth.ToString("MM") + "/" + LMonth.Year;
                                            StartDate = DateTime.ParseExact(AttendanceFromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                        }
                                        else
                                        {
                                            AttendanceToDate = "28" + "/" + FebMonthDays.ToString("MM") + "/" + Mm.Year;
                                            ToDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                            var month = new DateTime(Convert.ToInt32(AttDate[1]), Convert.ToInt32(AttDate[0]), 1);
                                            var LMonth = month.AddMonths(-1);
                                            AttendanceFromDate = "29" + "/" + LMonth.ToString("MM") + "/" + LMonth.Year;
                                            StartDate = DateTime.ParseExact(AttendanceFromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                        }
                                    }
                                    else
                                    {
                                        var month = new DateTime(Convert.ToInt32(AttDate[1]), Convert.ToInt32(AttDate[0]), 1);
                                        AttendanceToDate = "28" + "/" + month.ToString("MM") + "/" + month.Year;
                                        ToDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                        var LMonth = month.AddMonths(-1);
                                        AttendanceFromDate = "29" + "/" + LMonth.ToString("MM") + "/" + LMonth.Year;
                                        StartDate = DateTime.ParseExact(AttendanceFromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    }

                                }
                                if (!string.IsNullOrEmpty(AttendanceFromDate) && !string.IsNullOrEmpty(AttendanceToDate) && string.IsNullOrEmpty(MonthYear))
                                {
                                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                                    StartDate = DateTime.ParseExact(AttendanceFromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    ToDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                }
                                string tempStaffType = "Teaching,Non Teaching-Admin";
                                string[] arrayStaffType = tempStaffType.Split(',');
                                string StaffType = arrayStaffType[0] + "','" + arrayStaffType[1];
                                string tempStatus = "Registered,LongLeave,Others,Resigned";
                                string[] arrStatus = tempStatus.Split(',');
                                string StaffStatus = arrStatus[0] + "','" + arrStatus[1] + "','" + arrStatus[2] + "','" + arrStatus[3];
                                var SearchDateList = Enumerable.Range(0, 1 + ToDate.Subtract(StartDate).Days).Select(offset => StartDate.AddDays(offset)).ToArray();
                                var MonthAndYearGroup = (from p in SearchDateList
                                                         group p by new { month = p.Date.Month, year = p.Date.Year } into d
                                                         select d).ToList();
                                List<Staff_ConsolidateDeviceLogSummary_SP> TotalLogCollections = new List<Staff_ConsolidateDeviceLogSummary_SP>();
                                List<Staff_ConsolidateDeviceLogSummary_SP> TotalLogCollectionsForResigned = new List<Staff_ConsolidateDeviceLogSummary_SP>();
                                IList<Staff_ConsolidateDeviceLogSummary_SP> FinalConsolidateList = new List<Staff_ConsolidateDeviceLogSummary_SP>();
                                for (int i = 0; i < MonthAndYearGroup.Count; i++)
                                {
                                    var SingleMonthYear = (from u in MonthAndYearGroup[i] select u).ToList();
                                    Dictionary<long, IList<Staff_ConsolidateDeviceLogSummary_SP>> BioMetricAttendanceDetails = null;
                                    BioMetricAttendanceDetails = bioMetric.GetStaffConsolidateDeviceLogSummaryDetails(string.Empty, string.Empty, string.Empty, StaffType, string.Empty, StaffStatus, SingleMonthYear[0].Date.ToString("dd/MM/yyyy 00:00:01"), SingleMonthYear[SingleMonthYear.Count - 1].Date.ToString("dd/MM/yyyy 23:59:59"));
                                    if (BioMetricAttendanceDetails != null && BioMetricAttendanceDetails.FirstOrDefault().Key > 0)
                                    {
                                        var SinleMonthLogList = (from u in BioMetricAttendanceDetails.FirstOrDefault().Value
                                                                 where u.CurrentStatus == "Registered" || u.CurrentStatus == "LongLeave"
                                                                 select u).ToList();
                                        TotalLogCollections.AddRange(SinleMonthLogList);
                                    }
                                    var SingMonthListForResigned = (from a in BioMetricAttendanceDetails.FirstOrDefault().Value
                                                                    where a.CurrentStatus == "Resigned"
                                                                    select a).ToList();

                                    TotalLogCollectionsForResigned.AddRange(SingMonthListForResigned);
                                }

                                FinalConsolidateList = ConsolidateLogListForAttendance(TotalLogCollections, TotalLogCollectionsForResigned, AttendanceFromDate, AttendanceToDate);
                                obj.ConsolidateCount = FinalConsolidateList.Count;
                                obj.TotalGenerateCount = cldList.Count;
                                obj.LastMonth = LstMonth;
                                obj.LastYear = LstYear;
                                obj.CurrentMonth = Month;
                                obj.CurrentYear = year;
                                obj.CreatedBy = UserId;
                                obj.CreatedDate = DateTime.Now;
                                obj.ModifiedBy = UserId;
                                obj.ModifiedDate = DateTime.Now;
                                sms.SaveOrUpdateStaffAttendanceOpeningBalanceGenerateCount(obj);
                                return Json("Success", JsonRequestBehavior.AllowGet);
                            }
                        }
                        return Json("AttGeneratFailed", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw ex;
            }
        }
        #region Closing Balance View
        public ActionResult ClosingBalanceView(string AttendanceFromDate, string AttendanceToDate, string MonthYear)
        {
            try
            {
                string UserId = base.ValidateUser();
                if (string.IsNullOrEmpty(UserId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw;
            }
        }
        public ActionResult StaffClosingBalanceJqGrid(string Campus, string StaffType, string Programme, string IdNumber, string StaffName, string StaffStatus, string AttendanceFromDate, string AttendanceToDate, string MonthYear, string ExportType, string Rept, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {

                BioAttendanceService bioMetric = new BioAttendanceService();
                long NoOfDaysWeekOff = Convert.ToInt64(ConfigurationManager.AppSettings["NoOfDaysWeekOff"]);
                DateTime StartDate = new DateTime();
                DateTime ToDate = new DateTime();
                if (string.IsNullOrEmpty(Campus))
                {
                    string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                    MastersService ms = new MastersService();
                    UserService us = new UserService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("UserId", userid);
                    criteria.Add("AppCode", "STFIOSUMRY");
                    string[] BranchList = new string[0] { };
                    Dictionary<long, IList<UserAppRole>> listObj = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    if (listObj != null && listObj.FirstOrDefault().Key > 0)
                    {
                        BranchList = (from item in listObj.FirstOrDefault().Value
                                      where item.UserId == userid && item.DeptCode == "FEES / FINANCE" || item.DeptCode == "HR"
                                      select item.BranchCode).ToArray();
                        var Rolearr = (from item in listObj.FirstOrDefault().Value
                                       select item.RoleCode).ToArray();
                        if (Rolearr != null && Rolearr.Contains("Bio-All"))
                        {

                        }
                        else if (BranchList != null && BranchList.Count() > 0)
                        {
                            Campus = Convert.ToString(BranchList[0]);
                        }
                    }

                }
                if (!string.IsNullOrEmpty(MonthYear))
                {
                    string[] AttendanceDate = MonthYear.Split('-');
                    if (Convert.ToInt32(AttendanceDate[0]) == 3)
                    {
                        var FebMonthDays = DateTime.DaysInMonth(Convert.ToInt32(AttendanceDate[1]), Convert.ToInt32(AttendanceDate[0]) - 1);
                        if (FebMonthDays == 28)
                        {
                            AttendanceToDate = "28" + "/" + String.Join("/", AttendanceDate).ToString();
                            ToDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            var month = new DateTime(Convert.ToInt32(AttendanceDate[1]), Convert.ToInt32(AttendanceDate[0]), 1);
                            var LastMonth = month.AddMonths(-1);
                            AttendanceFromDate = "01" + "/" + LastMonth.ToString("MM") + "/" + LastMonth.Year;
                            StartDate = DateTime.ParseExact(AttendanceFromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            AttendanceToDate = "28" + "/" + String.Join("/", AttendanceDate).ToString();
                            ToDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            var month = new DateTime(Convert.ToInt32(AttendanceDate[1]), Convert.ToInt32(AttendanceDate[0]), 1);
                            var LastMonth = month.AddMonths(-1);
                            AttendanceFromDate = "29" + "/" + LastMonth.ToString("MM") + "/" + LastMonth.Year;
                            StartDate = DateTime.ParseExact(AttendanceFromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                    }
                    else
                    {
                        AttendanceToDate = "28" + "/" + String.Join("/", AttendanceDate).ToString();
                        ToDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        var month = new DateTime(Convert.ToInt32(AttendanceDate[1]), Convert.ToInt32(AttendanceDate[0]), 1);
                        var LastMonth = month.AddMonths(-1);
                        AttendanceFromDate = "29" + "/" + LastMonth.ToString("MM") + "/" + LastMonth.Year;
                        StartDate = DateTime.ParseExact(AttendanceFromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                }
                if (!string.IsNullOrEmpty(AttendanceFromDate) && !string.IsNullOrEmpty(AttendanceToDate))
                {
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    StartDate = DateTime.ParseExact(AttendanceFromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    ToDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (string.IsNullOrEmpty(StaffType))
                {
                    string tempStaffType = "Teaching,Non Teaching-Admin";
                    string[] arrayStaffType = tempStaffType.Split(',');
                    StaffType = arrayStaffType[0] + "','" + arrayStaffType[1];
                }
                else
                {
                    string[] arrStaffType = StaffType.Split(',');
                    if (arrStaffType.Length == 1)
                    {
                        // StaffType = StaffType;
                    }
                    else
                    {
                        StaffType = arrStaffType[0] + "','" + arrStaffType[1];
                    }
                }
                if (string.IsNullOrEmpty(StaffStatus))
                {
                    string tempStatus = "Registered,LongLeave,Others";
                    string[] arrStatus = tempStatus.Split(',');
                    StaffStatus = arrStatus[0] + "','" + arrStatus[1] + "','" + arrStatus[2];

                }
                else
                {
                    string[] arrayStatus = StaffStatus.Split(',');
                    if (arrayStatus.Length == 1)
                    {
                        //StaffStatus = StaffStatus;
                    }
                    else
                    {
                        StaffStatus = arrayStatus[0] + "','" + arrayStatus[1] + "','" + arrayStatus[2];
                    }
                }
                var SearchDateList = Enumerable.Range(0, 1 + ToDate.Subtract(StartDate).Days).Select(offset => StartDate.AddDays(offset)).ToArray();
                var MonthAndYearGroup = (from p in SearchDateList
                                         group p by new { month = p.Date.Month, year = p.Date.Year } into d
                                         select d).ToList();
                List<Staff_ConsolidateDeviceLogSummary_SP> TotalLogCollections = new List<Staff_ConsolidateDeviceLogSummary_SP>();
                IList<Staff_ConsolidateDeviceLogSummary_SP> DeviceLogsList = new List<Staff_ConsolidateDeviceLogSummary_SP>();
                IList<Staff_ConsolidateDeviceLogSummary_SP> ConsolidateLogList = new List<Staff_ConsolidateDeviceLogSummary_SP>();
                for (int i = 0; i < MonthAndYearGroup.Count; i++)
                {
                    var SingleMonthYear = (from u in MonthAndYearGroup[i] select u).ToList();
                    Dictionary<long, IList<Staff_ConsolidateDeviceLogSummary_SP>> BioMetricAttendanceDetails = null;
                    BioMetricAttendanceDetails = bioMetric.GetStaffConsolidateDeviceLogSummaryDetails(Campus, IdNumber, StaffName, StaffType, Programme, StaffStatus, SingleMonthYear[0].Date.ToString("dd/MM/yyyy 00:00:01"), SingleMonthYear[SingleMonthYear.Count - 1].Date.ToString("dd/MM/yyyy 23:59:59"));
                    if (BioMetricAttendanceDetails != null && BioMetricAttendanceDetails.FirstOrDefault().Key > 0)
                    {
                        var SinleMonthLogList = (from u in BioMetricAttendanceDetails.FirstOrDefault().Value
                                                 //where u.AttendanceDate != null
                                                 select u).ToList();
                        TotalLogCollections.AddRange(SinleMonthLogList);
                    }
                }
                if (TotalLogCollections != null && TotalLogCollections.Count > 0)
                {
                    ConsolidateLogList = (from c in TotalLogCollections
                                          group c by new
                                          {
                                              c.PreRegNum,
                                              c.Name,
                                              c.IdNumber,
                                              c.Campus,
                                              c.StaffCategoryForAttendane,
                                              c.Department,
                                              c.Designation,
                                              c.Programme,
                                              c.CurrentStatus,
                                              c.TotalDays,
                                          } into gcs
                                          select new Staff_ConsolidateDeviceLogSummary_SP()
                                          {
                                              PreRegNum = gcs.Key.PreRegNum,
                                              Name = gcs.Key.Name,
                                              IdNumber = gcs.Key.IdNumber,
                                              Campus = gcs.Key.Campus,
                                              StaffCategoryForAttendane = gcs.Key.StaffCategoryForAttendane,
                                              Department = gcs.Key.Department,
                                              Designation = gcs.Key.Designation,
                                              Programme = gcs.Key.Programme,
                                              CurrentStatus = gcs.Key.CurrentStatus,
                                              TotalDays = gcs.Key.TotalDays,
                                              TotalWorkedDays = (gcs.Sum(x => x.TotalWorkedDays)) + NoOfDaysWeekOff,
                                              TotalWorkedHoursMinutesAndseconds = BulidTimeFormatStringNew(gcs.Sum(x => x.TotalWorkedHours), gcs.Sum(x => x.TotalWorkedMinutes), gcs.Sum(x => x.TotalWorkedSeconds)),
                                          }).ToList();
                }
                if (ExportType == "Excel")
                {
                    DateTime AttendanceDate = new DateTime();
                    AttendanceDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    long Month = AttendanceDate.Month;
                    long Year = AttendanceDate.Year;
                    DateTime dtDate = new DateTime(Convert.ToInt32(Year), Convert.ToInt32(Month), 1);
                    string MonthName = dtDate.ToString("MMMM");
                    string ExcelFileName = "Staff_Attendance_Report_" + MonthName + "_" + Year + "_On_" + DateTime.Now.ToString("dd/MM/yyyy");
                    ExptToXL(ConsolidateLogList, ExcelFileName, (items => new
                    {
                        items.Name,
                        ID_Number = items.IdNumber,
                        Total_No_Of_Days_Present = items.TotalWorkedDays.ToString(),
                        Total_No_Of_Days_Absent = (items.TotalDays - items.TotalWorkedDays).ToString(),
                        Leave_Closing_Balance = ClosingBalance(items.Campus, items.PreRegNum, AttendanceToDate, CalcuateLeaveTaken(items.PreRegNum, AttendanceToDate, (items.TotalDays - items.TotalWorkedDays)), CalcuateWorkedDays(items.PreRegNum, AttendanceToDate, items.TotalWorkedDays)).ToString(),
                    }));
                    return new EmptyResult();
                }
                else
                {
                    if (ConsolidateLogList != null && ConsolidateLogList.Count > 0)
                    {
                        long totalrecords = ConsolidateLogList.Count;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var AttLst = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in ConsolidateLogList
                                    select new
                                    {
                                        cell = new string[] 
                                         {
                                             items.Id.ToString(),
                                             items.Name,
                                             items.IdNumber,
                                             items.TotalDays.ToString(),
                                             items.TotalWorkedDays.ToString(),
                                             (items.TotalDays-items.TotalWorkedDays).ToString(),
                                             ClosingBalance(items.Campus, items.PreRegNum, AttendanceToDate, CalcuateLeaveTaken(items.PreRegNum, AttendanceToDate, (items.TotalDays - items.TotalWorkedDays)), CalcuateWorkedDays(items.PreRegNum, AttendanceToDate, items.TotalWorkedDays)).ToString(),
                                             items.CurrentStatus
                                            
                                         }
                                    })
                        };
                        return Json(AttLst, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var AssLst = new { rows = (new { cell = new string[] { } }) };
                        return Json(AssLst, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult AttendanceGenerateCheck(string AttendanceToDate, string MonthYear)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                string UserId = base.ValidateUser();
                if (string.IsNullOrEmpty(UserId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (!string.IsNullOrEmpty(AttendanceToDate))
                    {
                        DateTime AttendanceDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        long Month = AttendanceDate.Month;
                        long Year = AttendanceDate.Year;
                        CLDetailsMaster obj = new CLDetailsMaster();
                        obj = sms.GetCLDetailsMasterByMonthYear(Month, Year);
                        if (obj != null)
                        {
                            return Json("Success", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json("Failed", JsonRequestBehavior.AllowGet);
                            //return View();
                        }
                    }
                    if (!string.IsNullOrEmpty(MonthYear))
                    {
                        string[] AttendnaceDate = MonthYear.Split('-');
                        long Month = Convert.ToInt64(AttendnaceDate[0]);
                        long Year = Convert.ToInt64(AttendnaceDate[1]);
                        CLDetailsMaster obj = new CLDetailsMaster();
                        obj = sms.GetCLDetailsMasterByMonthYear(Month, Year);
                        if (obj != null)
                            return Json("Success", JsonRequestBehavior.AllowGet);
                        else
                            return Json("Failed", JsonRequestBehavior.AllowGet);
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw;
            }
        }
        #endregion
        #region Staff Details Status Update
        public ActionResult EditStaffStatusChange(long PreRegNum, string Campus, string MonthYear, string AttendanceDate)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                string UserId = base.ValidateUser();
                if (string.IsNullOrEmpty(UserId)) return RedirectToAction("LoggOff", "Account");
                else
                {
                    StaffDetailsView obj = new StaffDetailsView();
                    obj = sms.GetStaffDetailsViewByStatus(PreRegNum);
                    if (obj != null)
                    {
                        if (obj.CurrentStatus == "Registered")
                        {
                            string Status = "Active";
                            ViewBag.Status = Status;
                        }
                        ViewBag.DateOfLongLeaveAndResigned = obj.DateOfLongLeaveAndResigned;
                    }
                    ViewBag.MonthYear = MonthYear;
                    ViewBag.AttendanceDate = AttendanceDate;
                    ViewBag.PreRegNum = PreRegNum;
                    ViewBag.Campus = Campus;
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw;
            }
        }
        public ActionResult SaveStaffDetailsStatus(StaffAttendanceNewStatus ExtObj, string Status, DateTime DateOfLongLeaveAndResigned, string Remarks, long PreRegNum, string MonthYear, string AttendanceDate)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                UserService us = new UserService();
                string UserId = base.ValidateUser();
                if (string.IsNullOrEmpty(UserId)) return RedirectToAction("LoggOff", "Account");
                else
                {
                    if (!string.IsNullOrEmpty(AttendanceDate))
                    {
                        DateTime LeaveDate = DateTime.ParseExact(AttendanceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        long Month = LeaveDate.Month;
                        long Year = LeaveDate.Year;
                        CLDetailsMaster ClDetailsObj = new CLDetailsMaster();
                        StaffDetailsView StaffDetailsObj = new StaffDetailsView();
                        StaffAttendanceNewStatus StaffAttStatusObj = new StaffAttendanceNewStatus();
                        User ObjUser = new TIPS.Entities.User();
                        StaffDetailsObj = sms.GetStaffDetailsViewByStatus(Convert.ToInt32(PreRegNum));
                        ClDetailsObj = sms.GetCLDetailsMasterByPreRegNum(Month, Year, PreRegNum);
                        StaffAttStatusObj = sms.GetStaffAttendanceNewStatusByPreRegNum(PreRegNum);
                        ObjUser = us.GetUserByEmployeeId(StaffDetailsObj.IdNumber);
                        if (StaffDetailsObj != null)
                        {
                            StaffDetailsObj.CurrentStatus = Status == "Rejoining" ? "Registered" : Status;
                            StaffDetailsObj.ModifiedBy = UserId;
                            StaffDetailsObj.ModifiedDate = DateTime.Now;
                            StaffDetailsObj.DateOfLongLeaveAndResigned = DateOfLongLeaveAndResigned;
                            StaffDetailsObj.Remarks = Remarks;
                            StaffDetailsObj.Status = UpdateStaffStatus(Status);
                            sms.CreateOrUpdateStaffDetailsView(StaffDetailsObj);
                            if (ObjUser != null)
                            {
                                if (Status == "LongLeave")
                                {
                                    ObjUser.IsActive = false;
                                    us.CreateOrUpdateUser(ObjUser);
                                }
                                else if (Status == "Resigned")
                                {
                                    ObjUser.IsActive = false;
                                    us.CreateOrUpdateUser(ObjUser);
                                }
                                else if (Status == "Rejoing")
                                {
                                    ObjUser.IsActive = true;
                                    us.CreateOrUpdateUser(ObjUser);
                                }
                            }
                            if (ClDetailsObj != null)
                            {
                                ClDetailsObj.Status = Status;
                                ClDetailsObj.DateOfLongLeaveAndResigned = DateOfLongLeaveAndResigned;
                                ClDetailsObj.Remark = Remarks;
                                sms.SaveOrUpdateCLDetailsMaster(ClDetailsObj);
                            }
                            if (StaffAttStatusObj == null)
                            {
                                ExtObj.StaffName = StaffDetailsObj.Name;
                                ExtObj.IdNumber = StaffDetailsObj.IdNumber;
                                ExtObj.PreRegNum = PreRegNum;
                                ExtObj.StaffStatus = Status;
                                ExtObj.DateOfLongLeaveAndResigned = DateOfLongLeaveAndResigned;
                                ExtObj.ToDateOfLongLeaveAndResigned = "Till Date";
                                ExtObj.Remarks = Remarks;
                                ExtObj.DateOfCreated = DateTime.Now;
                                ExtObj.CreatedBy = UserId;
                                ExtObj.ModifiedDate = DateTime.Now;
                                ExtObj.ModifiedBy = UserId;
                                sms.SaveOrUpdateStaffAttendanceNewStatus(ExtObj);

                            }
                            else
                            {
                                ExtObj.StaffName = StaffDetailsObj.Name;
                                ExtObj.IdNumber = StaffDetailsObj.IdNumber;
                                ExtObj.PreRegNum = PreRegNum;
                                ExtObj.StaffStatus = Status;
                                ExtObj.DateOfLongLeaveAndResigned = DateOfLongLeaveAndResigned;
                                ExtObj.ToDateOfLongLeaveAndResigned = "Till Date";
                                StaffAttStatusObj.ToDateOfLongLeaveAndResigned = DateOfLongLeaveAndResigned.ToString("dd/MM/yyyy");
                                ExtObj.Remarks = Remarks;
                                ExtObj.DateOfCreated = DateTime.Now;
                                ExtObj.CreatedBy = UserId;
                                ExtObj.ModifiedDate = DateTime.Now;
                                ExtObj.ModifiedBy = UserId;
                                StaffAttStatusObj.ModifiedDate = DateTime.Now;
                                StaffAttStatusObj.ModifiedBy = UserId;
                                sms.SaveOrUpdateStaffAttendanceNewStatus(ExtObj);
                                sms.SaveOrUpdateStaffAttendanceNewStatus(StaffAttStatusObj);
                            }
                            return Json("Updated", JsonRequestBehavior.AllowGet);
                        }

                    }
                    if (!string.IsNullOrEmpty(MonthYear))
                    {
                        string[] ArrMonthYear = MonthYear.Split('-');
                        long Month = Convert.ToInt64(ArrMonthYear[0]);
                        long Year = Convert.ToInt64(ArrMonthYear[1]);
                        CLDetailsMaster ClDetailsObj = new CLDetailsMaster();
                        StaffDetailsView StaffDetailsObj = new StaffDetailsView();
                        StaffAttendanceNewStatus StaffAttStatusObj = new StaffAttendanceNewStatus();
                        User ObjUser = new TIPS.Entities.User();
                        StaffDetailsObj = sms.GetStaffDetailsViewByStatus(Convert.ToInt32(PreRegNum));
                        ClDetailsObj = sms.GetCLDetailsMasterByPreRegNum(Month, Year, PreRegNum);
                        StaffAttStatusObj = sms.GetStaffAttendanceNewStatusByPreRegNum(PreRegNum);
                        ObjUser = us.GetUserByEmployeeId(StaffDetailsObj.IdNumber);
                        if (StaffDetailsObj != null)
                        {
                            StaffDetailsObj.CurrentStatus = Status;
                            StaffDetailsObj.ModifiedBy = UserId;
                            StaffDetailsObj.ModifiedDate = DateTime.Now;
                            StaffDetailsObj.DateOfLongLeaveAndResigned = DateOfLongLeaveAndResigned;
                            StaffDetailsObj.Remarks = Remarks;
                            StaffDetailsObj.Status = UpdateStaffStatus(Status);
                            sms.CreateOrUpdateStaffDetailsView(StaffDetailsObj);
                            if (ObjUser != null)
                            {
                                if (Status == "LongLeave")
                                {
                                    ObjUser.IsActive = false;
                                    us.CreateOrUpdateUser(ObjUser);
                                }
                                else if (Status == "Resigned")
                                {
                                    ObjUser.IsActive = false;
                                    us.CreateOrUpdateUser(ObjUser);
                                }
                                else if (Status == "Rejoing")
                                {
                                    ObjUser.IsActive = true;
                                    us.CreateOrUpdateUser(ObjUser);
                                }
                            }
                            if (ClDetailsObj != null)
                            {
                                ClDetailsObj.Status = Status;
                                ClDetailsObj.DateOfLongLeaveAndResigned = DateOfLongLeaveAndResigned;
                                ClDetailsObj.Remark = Remarks;
                                sms.SaveOrUpdateCLDetailsMaster(ClDetailsObj);
                            }
                            if (StaffAttStatusObj == null)
                            {
                                ExtObj.StaffName = StaffDetailsObj.Name;
                                ExtObj.IdNumber = StaffDetailsObj.IdNumber;
                                ExtObj.PreRegNum = PreRegNum;
                                ExtObj.StaffStatus = Status;
                                ExtObj.DateOfLongLeaveAndResigned = DateOfLongLeaveAndResigned;
                                ExtObj.ToDateOfLongLeaveAndResigned = "Till Date";
                                ExtObj.Remarks = Remarks;
                                ExtObj.DateOfCreated = DateTime.Now;
                                ExtObj.CreatedBy = UserId;
                                ExtObj.ModifiedDate = DateTime.Now;
                                ExtObj.ModifiedBy = UserId;
                                sms.SaveOrUpdateStaffAttendanceNewStatus(ExtObj);
                            }
                            else
                            {
                                ExtObj.StaffName = StaffDetailsObj.Name;
                                ExtObj.IdNumber = StaffDetailsObj.IdNumber;
                                ExtObj.PreRegNum = PreRegNum;
                                ExtObj.StaffStatus = Status;
                                ExtObj.DateOfLongLeaveAndResigned = DateOfLongLeaveAndResigned;
                                ExtObj.ToDateOfLongLeaveAndResigned = "Till Date";
                                StaffAttStatusObj.ToDateOfLongLeaveAndResigned = DateOfLongLeaveAndResigned.ToString("dd/MM/yyyy");
                                ExtObj.Remarks = Remarks;
                                ExtObj.DateOfCreated = DateTime.Now;
                                ExtObj.CreatedBy = UserId;
                                ExtObj.ModifiedDate = DateTime.Now;
                                ExtObj.ModifiedBy = UserId;
                                sms.SaveOrUpdateStaffAttendanceNewStatus(ExtObj);
                                sms.SaveOrUpdateStaffAttendanceNewStatus(StaffAttStatusObj);
                            }
                            return Json("Updated", JsonRequestBehavior.AllowGet);
                        }

                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw;
            }
        }
        public string UpdateStaffStatus(string Status)
        {
            try
            {
                if (Status == "Registered")
                {
                    return "Registered";
                }
                else if (Status == "LongLeave")
                {
                    return "Inactive";
                }
                else if (Status == "Resigned")
                {
                    return "Inactive";
                }
                else if (Status == "Rejoing")
                {
                    return "Registered";
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw ex;
            }
        }

        #endregion
        #region Staff Attendance Onduty Report
        public ActionResult StaffAttendanceOndutyView()
        {
            try
            {
                string UserId = base.ValidateUser();
                if (string.IsNullOrEmpty(UserId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region BreadCrumb
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    #endregion
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendnacePolicy");
                throw ex;
            }
        }
        public ActionResult StaffAttendanceOndutyReportListJqGrid(Staff_AttendanceOnduty_vw StaffAttendanceOndutyObj, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                BioAttendanceService bioMetric = new BioAttendanceService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(StaffAttendanceOndutyObj.Month))
                {
                    criteria.Add("Month", StaffAttendanceOndutyObj.Month);
                }
                if (StaffAttendanceOndutyObj.Year > 0)
                {
                    criteria.Add("Year", StaffAttendanceOndutyObj.Year);
                }
                Dictionary<long, IList<Staff_AttendanceOnduty_vw>> StaffAttendanceOndutyList = bioMetric.GetStaffAttendanceOnDutyViewListWithCriteria(page - 1, rows, sidx, sord, criteria);
                if (StaffAttendanceOndutyList != null && StaffAttendanceOndutyList.FirstOrDefault().Key > 0)
                {
                    if (ExportType == "Excel")
                    {
                        string ExcelFileName = "Staff_Attendance_Onuty_Report_On" + " " + DateTime.Now.ToString("dd/MM/yyyy");
                        var List = StaffAttendanceOndutyList.First().Value.ToList();
                        ExptToXL(List, ExcelFileName, (items => new
                        {
                            items.Name,
                            items.IdNumber,
                            items.Month,
                            items.Year,
                            items.OnDuty,
                        }));
                        return new EmptyResult();
                    }
                    else
                    {
                        long totalrecords = StaffAttendanceOndutyList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in StaffAttendanceOndutyList.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[]{
                                     items.Id.ToString(),
                                     items.Name,
                                     items.IdNumber,
                                     items.PreRegNum.ToString(),
                                     items.Month,
                                     items.Year.ToString(),
                                     items.OnDuty.ToString()

                                       }
                                    })

                        };
                        return Json(jsondat, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        #endregion
        #region Staff Attendance Longleave And Resigned Report
        public ActionResult StaffAttendanceLongleaveAndResignedView()
        {
            try
            {
                string UserId = base.ValidateUser();
                if (string.IsNullOrEmpty(UserId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region BreadCrumb
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    #endregion
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw ex;
            }
        }
        public ActionResult StaffAttendanceLongLeaveAndResignedListJqGrid(Staff_AttendanceLongleaveAndResignedReport_vw obj, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                BioAttendanceService bioMetric = new BioAttendanceService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(obj.Month))
                {
                    criteria.Add("Month", obj.Month);
                }
                if (obj.Year > 0)
                {
                    criteria.Add("Year", obj.Year);
                }
                if (!string.IsNullOrEmpty(obj.Status))
                {
                    criteria.Add("Status", obj.Status);
                }
                Dictionary<long, IList<Staff_AttendanceLongleaveAndResignedReport_vw>> StaffAttendanceLongleaveAndResignedList = bioMetric.GetStaffAttendanceLongleaveAndReginedViewListWithCriteria(page - 1, rows, sidx, sord, criteria);
                if (StaffAttendanceLongleaveAndResignedList != null && StaffAttendanceLongleaveAndResignedList.FirstOrDefault().Key > 0)
                {
                    if (ExportType == "Excel")
                    {
                        string ExcelFileName = "Staff_Attendance_Longleave_And_Regined_Report_On" + " " + DateTime.Now.ToString("dd/MM/yyyy");
                        var List = StaffAttendanceLongleaveAndResignedList.First().Value.ToList();
                        ExptToXL(List, ExcelFileName, (items => new
                        {
                            items.Name,
                            items.IdNumber,
                            items.Month,
                            items.Year,
                            items.Status,
                        }));
                        return new EmptyResult();
                    }
                    else
                    {
                        long totalrecords = StaffAttendanceLongleaveAndResignedList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in StaffAttendanceLongleaveAndResignedList.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[]{
                                     items.Id.ToString(),
                                     items.Name,
                                     items.IdNumber,
                                     items.PreRegNum.ToString(),
                                     items.Month,
                                     items.Year.ToString(),
                                     items.Status,
                                     items.DateOfLongLeaveAndResigned!=null?items.DateOfLongLeaveAndResigned.Value.ToString("dd/MM/yyyy"):null
                                       }
                                    })

                        };
                        return Json(jsondat, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }

            }
            catch(Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        #endregion
        #region Staff Bio Attendance Device log Details
        public ActionResult ShowStaffAttendanceInOutStatusNew(string IdNumber, string AttendanceDate)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ViewBag.IdNumber = IdNumber;
                    ViewBag.AttendanceDate = AttendanceDate;
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw ex;
            }
        }
        public ActionResult StaffDailyAttendanceIOSummaryStatusJqGrid(string IdNumber, string AttendanceFromDate, string AttendanceToDate, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                BioAttendanceService bioMetric = new BioAttendanceService();
                DateTime StartDate = new DateTime();
                DateTime ToDate = new DateTime();
                if (!string.IsNullOrEmpty(AttendanceFromDate) && !string.IsNullOrEmpty(AttendanceToDate))
                {
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    //StartDate = DateTime.Parse(AttendanceFromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    //ToDate = DateTime.Parse(AttendanceToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    StartDate = DateTime.ParseExact(AttendanceFromDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    ToDate = DateTime.ParseExact(AttendanceToDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                }
                var SearchDateList = Enumerable.Range(0, 1 + ToDate.Subtract(StartDate).Days).Select(offset => StartDate.AddDays(offset)).ToArray();
                var MonthAndYearGroup = (from p in SearchDateList
                                         group p by new { month = p.Date.Month, year = p.Date.Year } into d
                                         select d).ToList();
                List<Staff_DeviceLogSummaryStatus_sp> TotalLogCollections = new List<Staff_DeviceLogSummaryStatus_sp>();
                IList<Staff_DeviceLogSummaryStatus_sp> DeviceLogsList = new List<Staff_DeviceLogSummaryStatus_sp>();
                IList<Staff_DeviceLogSummaryStatus_sp> ConsolidateLogList = new List<Staff_DeviceLogSummaryStatus_sp>();
                for (int i = 0; i < MonthAndYearGroup.Count; i++)
                {
                    var SingleMonthYear = (from u in MonthAndYearGroup[i] select u).ToList();
                    Dictionary<long, IList<Staff_DeviceLogSummaryStatus_sp>> BioMetricAttendanceDetails = null;
                    BioMetricAttendanceDetails = bioMetric.GetStaffDeviceLogStatusSummaryDetails(string.Empty, IdNumber, string.Empty, string.Empty, SingleMonthYear[0].Date.ToString("dd/MM/yyyy 00:00:01"), SingleMonthYear[SingleMonthYear.Count - 1].Date.ToString("dd/MM/yyyy 23:59:59"));
                    if (BioMetricAttendanceDetails != null && BioMetricAttendanceDetails.FirstOrDefault().Key > 0)
                    {
                        var SinleMonthLogList = (from u in BioMetricAttendanceDetails.FirstOrDefault().Value
                                                 where u.AttendanceDate != null
                                                 select u).ToList();
                        TotalLogCollections.AddRange(SinleMonthLogList);
                    }
                }
                if (TotalLogCollections != null && TotalLogCollections.Count > 0)
                {
                    ConsolidateLogList = (from c in TotalLogCollections
                                          group c by new
                                          {
                                              c.PreRegNum,
                                              c.Name,
                                              c.IdNumber,
                                              c.Campus,
                                              c.AttendanceDate,
                                              c.LogDate,
                                              c.INOUT
                                          } into gcs
                                          select new Staff_DeviceLogSummaryStatus_sp()
                                          {
                                              PreRegNum = gcs.Key.PreRegNum,
                                              Name = gcs.Key.Name,
                                              IdNumber = gcs.Key.IdNumber,
                                              Campus = gcs.Key.Campus,
                                              AttendanceDate = gcs.Key.AttendanceDate,
                                              LogDate = gcs.Key.LogDate,
                                              INOUT = gcs.Key.INOUT,
                                          }).ToList();
                }
                if (ExportType == "Excel")
                {
                    string ExcelFileName = "Staff_Daily_IO_Status_Summary_From_" + AttendanceFromDate + "_To_" + AttendanceToDate + "_On_" + DateTime.Now.ToString("dd/MM/yyyy");
                    ExptToXL(TotalLogCollections, ExcelFileName, (items => new
                    {
                        items.Campus,
                        items.Name,
                        items.IdNumber,
                        AttendanceDate = items.AttendanceDate != null ? items.AttendanceDate.Value.ToString("dd/MM/yyyy") : string.Empty,
                        LogTime = items.LogDate != null ? items.LogDate.Value.ToString("HH:mm:ss") : "00:00:00",
                        Log_Status = items.INOUT
                    }));
                    return new EmptyResult();
                }
                else
                {

                    if (ConsolidateLogList != null && ConsolidateLogList.Count > 0)
                    {
                        long totalrecords = ConsolidateLogList.Count;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var AttLst = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in ConsolidateLogList
                                    select new
                                    {
                                        cell = new string[] 
                                         {
                                             items.Id.ToString(),
                                             items.Campus,
                                             items.Name,
                                             items.IdNumber,
                                             items.AttendanceDate!=null ? items.AttendanceDate.Value.ToString("dd/MM/yyyy"):string.Empty,
                                             items.LogDate!=null ? items.LogDate.Value.ToString("HH:mm:ss"):"00:00:00",
                                             items.INOUT
                                         }
                                    })
                        };
                        return Json(AttLst, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var AssLst = new { rows = (new { cell = new string[] { } }) };
                        return Json(AssLst, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw ex;
            }
        }
        #endregion
        public IList<Staff_ConsolidateDeviceLogSummary_SP> ConsolidateLogListForAttendance(List<Staff_ConsolidateDeviceLogSummary_SP> TotalLogCollections, List<Staff_ConsolidateDeviceLogSummary_SP> TotalLogCollectionsForResigned, string AttendanceFromDate, string AttendanceToDate)
        {
            long NoOfDaysWeekOff = Convert.ToInt64(ConfigurationManager.AppSettings["NoOfDaysWeekOff"]);
            //List<Staff_ConsolidateDeviceLogSummary_SP> TotalLogCollectionsForResigned = new List<Staff_ConsolidateDeviceLogSummary_SP>();
            IList<Staff_ConsolidateDeviceLogSummary_SP> DeviceLogsList = new List<Staff_ConsolidateDeviceLogSummary_SP>();
            IList<Staff_ConsolidateDeviceLogSummary_SP> ConsolidateLogList = new List<Staff_ConsolidateDeviceLogSummary_SP>();
            IList<Staff_ConsolidateDeviceLogSummary_SP> ConsolidateLogListNew = new List<Staff_ConsolidateDeviceLogSummary_SP>();
            IList<Staff_ConsolidateDeviceLogSummary_SP> FinalConsolidateList = new List<Staff_ConsolidateDeviceLogSummary_SP>();
            if (TotalLogCollections != null && TotalLogCollections.Count > 0)
            {
                ConsolidateLogList = (from c in TotalLogCollections
                                      group c by new
                                      {
                                          c.PreRegNum,
                                          c.Name,
                                          c.IdNumber,
                                          c.Campus,
                                          c.StaffCategoryForAttendane,
                                          c.Department,
                                          c.Designation,
                                          c.Programme,
                                          c.CurrentStatus,
                                          c.TotalDays,

                                      } into gcs
                                      select new Staff_ConsolidateDeviceLogSummary_SP()
                                      {
                                          PreRegNum = gcs.Key.PreRegNum,
                                          Name = gcs.Key.Name,
                                          IdNumber = gcs.Key.IdNumber,
                                          Campus = gcs.Key.Campus,
                                          StaffCategoryForAttendane = gcs.Key.StaffCategoryForAttendane,
                                          Department = gcs.Key.Department,
                                          Designation = gcs.Key.Designation,
                                          Programme = gcs.Key.Programme,
                                          CurrentStatus = gcs.Key.CurrentStatus,
                                          TotalDays = gcs.Key.TotalDays,
                                          TotalWorkedDays = (gcs.Sum(x => x.TotalWorkedDays)) + NoOfDaysWeekOff,
                                          NoOfLeaveTaken = gcs.FirstOrDefault().NoOfLeaveTaken,
                                          TotalWorkedHoursMinutesAndseconds = BulidTimeFormatStringNew(gcs.Sum(x => x.TotalWorkedHours), gcs.Sum(x => x.TotalWorkedMinutes), gcs.Sum(x => x.TotalWorkedSeconds)),
                                          OpeningBalance = gcs.FirstOrDefault().OpeningBalance,
                                          ClosingBalance = gcs.FirstOrDefault().ClosingBalance,
                                          TotalAvailableBalance = gcs.FirstOrDefault().TotalAvailableBalance

                                      }).ToList();

                if (TotalLogCollectionsForResigned != null && TotalLogCollectionsForResigned.Count > 0)
                {
                    ConsolidateLogListNew = (from c in TotalLogCollectionsForResigned
                                             where Convert.ToDateTime(AttendanceFromDate) <= c.DateOfLongLeaveAndResigned
                                             && Convert.ToDateTime(AttendanceToDate) >= c.DateOfLongLeaveAndResigned
                                             group c by new
                                             {
                                                 c.PreRegNum,
                                                 c.Name,
                                                 c.IdNumber,
                                                 c.Campus,
                                                 c.StaffCategoryForAttendane,
                                                 c.Department,
                                                 c.Designation,
                                                 c.Programme,
                                                 c.CurrentStatus,
                                                 c.TotalDays,

                                             } into gcs
                                             select new Staff_ConsolidateDeviceLogSummary_SP()
                                             {
                                                 PreRegNum = gcs.Key.PreRegNum,
                                                 Name = gcs.Key.Name,
                                                 IdNumber = gcs.Key.IdNumber,
                                                 Campus = gcs.Key.Campus,
                                                 StaffCategoryForAttendane = gcs.Key.StaffCategoryForAttendane,
                                                 Department = gcs.Key.Department,
                                                 Designation = gcs.Key.Designation,
                                                 Programme = gcs.Key.Programme,
                                                 CurrentStatus = gcs.Key.CurrentStatus,
                                                 TotalDays = gcs.Key.TotalDays,
                                                 TotalWorkedDays = (gcs.Sum(x => x.TotalWorkedDays)) + NoOfDaysWeekOff,
                                                 NoOfLeaveTaken = gcs.FirstOrDefault().NoOfLeaveTaken,
                                                 TotalWorkedHoursMinutesAndseconds = BulidTimeFormatStringNew(gcs.Sum(x => x.TotalWorkedHours), gcs.Sum(x => x.TotalWorkedMinutes), gcs.Sum(x => x.TotalWorkedSeconds)),
                                                 OpeningBalance = gcs.FirstOrDefault().OpeningBalance,
                                                 ClosingBalance = gcs.FirstOrDefault().ClosingBalance,
                                                 TotalAvailableBalance = gcs.FirstOrDefault().TotalAvailableBalance

                                             }).ToList();
                }
                FinalConsolidateList = ConsolidateLogList.Concat(ConsolidateLogListNew).ToList();
                return FinalConsolidateList;
            }
            return null;
        }
        #region Staff Attendance Register Report
        public ActionResult StaffAttendanceRegisterReport()
        {
            try
            {
                string UserId = base.ValidateUser();
                if (string.IsNullOrEmpty(UserId)) return RedirectToAction("logOff", "Account");
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolidace");
                throw;
            }
        }
        public ActionResult StaffAttendanceRegisterReportGrid(string Campus, string StaffType, string Programme, string IdNumber, string StaffName, string StaffStatus, string searchStstus, string MonthYear, string AttendanceFromDate, string AttendanceToDate, string ExportType, string Rept, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                BioAttendanceService bioMetric = new BioAttendanceService();
                StaffManagementService smsObj = new StaffManagementService();
                long NoOfDaysWeekOff = Convert.ToInt64(ConfigurationManager.AppSettings["NoOfDaysWeekOff"]);
                DateTime StartDate = new DateTime();
                DateTime ToDate = new DateTime();
                if (string.IsNullOrEmpty(Campus))
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string[] AttendanceDate = MonthYear.Split('-');
                    if (!string.IsNullOrEmpty(MonthYear))
                    {
                        //string[] AttendanceDate = MonthYear.Split('-');
                        if (Convert.ToInt32(AttendanceDate[0]) == 3)
                        {
                            var FebMonthDays = DateTime.DaysInMonth(Convert.ToInt32(AttendanceDate[1]), Convert.ToInt32(AttendanceDate[0]) - 1);
                            if (FebMonthDays == 28)
                            {
                                AttendanceToDate = "28" + "/" + String.Join("/", AttendanceDate).ToString();
                                ToDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                var month = new DateTime(Convert.ToInt32(AttendanceDate[1]), Convert.ToInt32(AttendanceDate[0]), 1);
                                var LastMonth = month.AddMonths(-1);
                                AttendanceFromDate = "01" + "/" + LastMonth.ToString("MM") + "/" + LastMonth.Year;
                                StartDate = DateTime.ParseExact(AttendanceFromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                AttendanceToDate = "28" + "/" + String.Join("/", AttendanceDate).ToString();
                                ToDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                var month = new DateTime(Convert.ToInt32(AttendanceDate[1]), Convert.ToInt32(AttendanceDate[0]), 1);
                                var LastMonth = month.AddMonths(-1);
                                AttendanceFromDate = "29" + "/" + LastMonth.ToString("MM") + "/" + LastMonth.Year;
                                StartDate = DateTime.ParseExact(AttendanceFromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                        }
                        else
                        {
                            AttendanceToDate = "28" + "/" + String.Join("/", AttendanceDate).ToString();
                            ToDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            var month = new DateTime(Convert.ToInt32(AttendanceDate[1]), Convert.ToInt32(AttendanceDate[0]), 1);
                            var LastMonth = month.AddMonths(-1);
                            AttendanceFromDate = "29" + "/" + LastMonth.ToString("MM") + "/" + LastMonth.Year;
                            StartDate = DateTime.ParseExact(AttendanceFromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                    }
                    if (!string.IsNullOrEmpty(AttendanceFromDate) && !string.IsNullOrEmpty(AttendanceToDate))
                    {
                        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                        StartDate = DateTime.ParseExact(AttendanceFromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        ToDate = DateTime.ParseExact(AttendanceToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (string.IsNullOrEmpty(StaffType))
                    {
                        string tempStaffType = "Teaching,Non Teaching-Admin";
                        string[] arrayStaffType = tempStaffType.Split(',');
                        StaffType = arrayStaffType[0] + "','" + arrayStaffType[1];
                    }
                    else
                    {
                        string[] arrStaffType = StaffType.Split(',');
                        if (arrStaffType.Length == 1)
                        {
                            //StaffType = StaffType;
                        }
                        else
                        {
                            StaffType = arrStaffType[0] + "','" + arrStaffType[1];
                        }
                    }
                    if (string.IsNullOrEmpty(StaffStatus))
                    {
                        string tempStatus = "Registered,LongLeave,Others,Resigned";
                        string[] arrStatus = tempStatus.Split(',');
                        StaffStatus = arrStatus[0] + "','" + arrStatus[1] + "','" + arrStatus[2] + "','" + arrStatus[3];

                    }
                    else
                    {
                        string[] arrayStatus = StaffStatus.Split(',');
                        if (arrayStatus.Length == 1)
                        {
                            // StaffStatus = StaffStatus;
                        }
                        else
                        {
                            StaffStatus = arrayStatus[0] + "','" + arrayStatus[1] + "','" + arrayStatus[2] + "','" + arrayStatus[3];
                        }
                    }

                    var SearchDateList = Enumerable.Range(0, 1 + ToDate.Subtract(StartDate).Days).Select(offset => StartDate.AddDays(offset)).ToArray();
                    var MonthAndYearGroup = (from p in SearchDateList
                                             group p by new { month = p.Date.Month, year = p.Date.Year } into d
                                             select d).ToList();
                    List<Staff_AttendanceRegisterReport> TotalLogCollections = new List<Staff_AttendanceRegisterReport>();
                    List<Staff_AttendanceRegisterReport> TotalLogCollectionsForResigned = new List<Staff_AttendanceRegisterReport>();
                    IList<Staff_AttendanceRegisterReport> FinalConsolidateList = new List<Staff_AttendanceRegisterReport>();
                    List<Staff_DeviceLogSummarySP> LogCollections = new List<Staff_DeviceLogSummarySP>();
                    IList<Staff_DeviceLogSummarySP> DeviceLogsList = new List<Staff_DeviceLogSummarySP>();
                    IList<Staff_DeviceLogSummarySP> ConsolidateLogList = new List<Staff_DeviceLogSummarySP>();
                    for (int i = 0; i < MonthAndYearGroup.Count; i++)
                    {
                        var SingleMonthYear = (from u in MonthAndYearGroup[i] select u).ToList();
                        Dictionary<long, IList<Staff_DeviceLogSummarySP>> BioMetricAttendanceDetails = null;
                        BioMetricAttendanceDetails = bioMetric.GetStaffDeviceLogSummaryDetails(Campus, IdNumber, StaffName, string.Empty, SingleMonthYear[0].Date.ToString("dd/MM/yyyy 00:00:01"), SingleMonthYear[SingleMonthYear.Count - 1].Date.ToString("dd/MM/yyyy 23:59:59"));
                        if (BioMetricAttendanceDetails != null && BioMetricAttendanceDetails.FirstOrDefault().Key > 0)
                        {
                            var SinleMonthLogList = (from u in BioMetricAttendanceDetails.FirstOrDefault().Value
                                                     where u.AttendanceDate != null
                                                     select u).ToList();
                            LogCollections.AddRange(SinleMonthLogList);
                        }
                    }
                    if (LogCollections != null && LogCollections.Count > 0)
                    {
                        ConsolidateLogList = (from c in LogCollections
                                              group c by new
                                              {
                                                  c.PreRegNum,
                                                  c.Name,
                                                  c.IdNumber,
                                                  c.Campus,
                                                  c.AttendanceDate,
                                              } into gcs
                                              select new Staff_DeviceLogSummarySP()
                                              {
                                                  PreRegNum = gcs.Key.PreRegNum,
                                                  Name = gcs.Key.Name,
                                                  IdNumber = gcs.Key.IdNumber,
                                                  Campus = gcs.Key.Campus,
                                                  AttendanceDate = gcs.Key.AttendanceDate,

                                              }).ToList();
                    }
                    // var BioMetricAttendanceDetails = "";
                    for (int i = 0; i < MonthAndYearGroup.Count; i++)
                    {
                        var SingleMonthYear = (from u in MonthAndYearGroup[i] select u).ToList();
                        Dictionary<long, IList<Staff_AttendanceRegisterReport>> BioMetricAttendanceDetails = null;
                        BioMetricAttendanceDetails = bioMetric.GetStaffConsolidateDeviceLogSummaryDetailsForResgister(Campus, IdNumber, StaffName, StaffType, Programme, StaffStatus, SingleMonthYear[0].Date.ToString("dd/MM/yyyy 00:00:01"), SingleMonthYear[SingleMonthYear.Count - 1].Date.ToString("dd/MM/yyyy 23:59:59"));
                        if (BioMetricAttendanceDetails != null && BioMetricAttendanceDetails.FirstOrDefault().Key > 0)
                        {
                            var SinleMonthLogList = (from u in BioMetricAttendanceDetails.FirstOrDefault().Value
                                                     where u.CurrentStatus == "Registered" || u.CurrentStatus == "LongLeave"
                                                     select u).ToList();
                            TotalLogCollections.AddRange(SinleMonthLogList);
                        }
                        var SingMonthListForResigned = (from item in BioMetricAttendanceDetails.FirstOrDefault().Value
                                                        where item.CurrentStatus == "Resigned"
                                                        select item).ToList();
                        TotalLogCollectionsForResigned.AddRange(SingMonthListForResigned);

                    }
                    FinalConsolidateList = ConsolidateLogListForAttendanceForRegister(TotalLogCollections, TotalLogCollectionsForResigned, AttendanceFromDate, AttendanceToDate);
                    long[] list = (from a in ConsolidateLogList select a.PreRegNum).ToArray();
                    List<Staff_DeviceLogSummarySP> AttDate = ConsolidateLogList.ToList();
                    foreach (Staff_AttendanceRegisterReport obj in FinalConsolidateList)
                    {
                        Dictionary<string, object> criteria = new Dictionary<string, object>();
                        string[] Attemonthyear = MonthYear.Split('-');
                        var Month = Convert.ToInt64(Attemonthyear[0]);
                        var Year = Convert.ToInt64(Attemonthyear[1]);
                        criteria.Add("HolidayType", "Holiday");
                        criteria.Add("Campus", Campus);
                        Dictionary<long, IList<StaffHolidaysMaster>> StaffHolidaysMasterList = smsObj.GetStaffHolidaysMasterListWithPagingAndCriteria(page - 1, rows, string.Empty, string.Empty, criteria);
                        if (StaffHolidaysMasterList != null && StaffHolidaysMasterList.FirstOrDefault().Key > 0)
                        {
                            var Holiday = (from holi in StaffHolidaysMasterList.FirstOrDefault().Value
                                           where holi.Campus == Campus && holi.MonthNumber == Month && holi.HolidayDate != null
                                           select holi).Distinct().ToList();
                            obj.Date1 = "<b style='color:Red'>A</b>"; obj.Date2 = "<b style='color:Red'>A</b>"; obj.Date3 = "<b style='color:Red'>A</b>"; obj.Date4 = "<b style='color:Red'>A</b>"; obj.Date5 = "<b style='color:Red'>A</b>"; obj.Date6 = "<b style='color:Red'>A</b>"; obj.Date7 = "<b style='color:Red'>A</b>"; obj.Date8 = "<b style='color:Red'>A</b>"; obj.Date9 = "<b style='color:Red'>A</b>"; obj.Date10 = "<b style='color:Red'>A</b>"; obj.Date11 = "<b style='color:Red'>A</b>"; obj.Date12 = "<b style='color:Red'>A</b>"; obj.Date13 = "<b style='color:Red'>A</b>"; obj.Date14 = "<b style='color:Red'>A</b>"; obj.Date15 = "<b style='color:Red'>A</b>";
                            obj.Date16 = "<b style='color:Red'>A</b>"; obj.Date17 = "<b style='color:Red'>A</b>"; obj.Date18 = "<b style='color:Red'>A</b>"; obj.Date19 = "<b style='color:Red'>A</b>"; obj.Date20 = "<b style='color:Red'>A</b>"; obj.Date21 = "<b style='color:Red'>A</b>"; obj.Date22 = "<b style='color:Red'>A</b>"; obj.Date23 = "<b style='color:Red'>A</b>"; obj.Date24 = "<b style='color:Red'>A</b>"; obj.Date25 = "<b style='color:Red'>A</b>"; obj.Date26 = "<b style='color:Red'>A</b>"; obj.Date27 = "<b style='color:Red'>A</b>"; obj.Date28 = "<b style='color:Red'>A</b>"; obj.Date29 = "<b style='color:Red'>A</b>"; obj.Date30 = "<b style='color:Red'>A</b>";
                            obj.Date31 = "<b style='color:Red'>A</b>";
                            foreach (StaffHolidaysMaster shm in Holiday)
                            {
                                if (Holiday.Count() > 0)
                                {
                                    var hadate = Holiday.FirstOrDefault().HolidayDate.Split(',');
                                    for (var j = 0; j < hadate.Length; j++)
                                    {
                                        switch (hadate[j].Substring(0, 2))
                                        {
                                            case "01": { if (ExportType == "Excel")obj.Date1 = "H"; else { obj.Date1 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "02": { if (ExportType == "Excel")obj.Date2 = "H"; else { obj.Date2 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "03": { if (ExportType == "Excel")obj.Date3 = "H"; else { obj.Date3 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "04": { if (ExportType == "Excel")obj.Date4 = "H"; else { obj.Date4 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "05": { if (ExportType == "Excel")obj.Date5 = "H"; else { obj.Date5 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "06": { if (ExportType == "Excel")obj.Date6 = "H"; else { obj.Date6 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "07": { if (ExportType == "Excel")obj.Date7 = "H"; else { obj.Date7 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "08": { if (ExportType == "Excel")obj.Date8 = "H"; else { obj.Date8 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "09": { if (ExportType == "Excel")obj.Date9 = "H"; else { obj.Date9 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "10": { if (ExportType == "Excel")obj.Date10 = "H"; else { obj.Date10 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "11": { if (ExportType == "Excel")obj.Date11 = "H"; else { obj.Date11 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "12": { if (ExportType == "Excel")obj.Date12 = "H"; else { obj.Date12 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "13": { if (ExportType == "Excel")obj.Date13 = "H"; else { obj.Date13 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "14": { if (ExportType == "Excel")obj.Date14 = "H"; else { obj.Date14 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "15": { if (ExportType == "Excel")obj.Date15 = "H"; else { obj.Date15 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "16": { if (ExportType == "Excel")obj.Date16 = "H"; else { obj.Date16 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "17": { if (ExportType == "Excel")obj.Date17 = "H"; else { obj.Date17 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "18": { if (ExportType == "Excel")obj.Date18 = "H"; else { obj.Date18 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "19": { if (ExportType == "Excel")obj.Date19 = "H"; else { obj.Date19 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "20": { if (ExportType == "Excel")obj.Date20 = "H"; else { obj.Date20 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "21": { if (ExportType == "Excel")obj.Date21 = "H"; else { obj.Date21 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "22": { if (ExportType == "Excel")obj.Date22 = "H"; else { obj.Date22 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "23": { if (ExportType == "Excel")obj.Date23 = "H"; else { obj.Date23 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "24": { if (ExportType == "Excel")obj.Date24 = "H"; else { obj.Date24 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "25": { if (ExportType == "Excel")obj.Date25 = "H"; else { obj.Date25 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "26": { if (ExportType == "Excel")obj.Date26 = "H"; else { obj.Date26 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "27": { if (ExportType == "Excel")obj.Date27 = "H"; else { obj.Date27 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "28": { if (ExportType == "Excel")obj.Date28 = "H"; else { obj.Date28 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "29": { if (ExportType == "Excel")obj.Date29 = "H"; else { obj.Date29 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "30": { if (ExportType == "Excel")obj.Date30 = "H"; else { obj.Date30 = "<b style='color:Blue'>H</b>"; } } break;
                                            case "31": { if (ExportType == "Excel")obj.Date31 = "H"; else { obj.Date31 = "<b style='color:Blue'>H</b>"; } } break;
                                            default: break;
                                        }
                                    }
                                }
                            }
                        }
                        criteria.Clear();
                        criteria.Add("HolidayType", "WeekOff");
                        criteria.Add("Campus", Campus);
                        Dictionary<long, IList<StaffHolidaysMaster>> StaffHolidaysMasterListObj = smsObj.GetStaffHolidaysMasterListWithPagingAndCriteria(page - 1, rows, string.Empty, string.Empty, criteria);
                        if (StaffHolidaysMasterListObj != null && StaffHolidaysMasterListObj.FirstOrDefault().Key > 0)
                        {
                            var HolidayForWeekOff = (from holi in StaffHolidaysMasterListObj.FirstOrDefault().Value
                                                     where holi.Campus == Campus && holi.MonthNumber == Month && holi.HolidayDate != null
                                                     select holi).Distinct().ToList();
                            foreach (StaffHolidaysMaster shm in HolidayForWeekOff)
                            {
                                if (HolidayForWeekOff.Count() > 0)
                                {
                                    var hadateweekoff = HolidayForWeekOff.FirstOrDefault().HolidayDate.Split(',');
                                    for (var j = 0; j < hadateweekoff.Length; j++)
                                    {
                                        switch (hadateweekoff[j].Substring(0, 2))
                                        {
                                            case "01": { if (ExportType == "Excel")obj.Date1 = "W"; else { obj.Date1 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "02": { if (ExportType == "Excel")obj.Date2 = "W"; else { obj.Date2 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "03": { if (ExportType == "Excel")obj.Date3 = "W"; else { obj.Date3 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "04": { if (ExportType == "Excel")obj.Date4 = "W"; else { obj.Date4 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "05": { if (ExportType == "Excel")obj.Date5 = "W"; else { obj.Date5 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "06": { if (ExportType == "Excel")obj.Date6 = "W"; else { obj.Date6 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "07": { if (ExportType == "Excel")obj.Date7 = "W"; else { obj.Date7 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "08": { if (ExportType == "Excel")obj.Date8 = "W"; else { obj.Date8 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "09": { if (ExportType == "Excel")obj.Date9 = "W"; else { obj.Date9 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "10": { if (ExportType == "Excel")obj.Date10 = "W"; else { obj.Date10 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "11": { if (ExportType == "Excel")obj.Date11 = "W"; else { obj.Date11 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "12": { if (ExportType == "Excel")obj.Date12 = "W"; else { obj.Date12 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "13": { if (ExportType == "Excel")obj.Date13 = "W"; else { obj.Date13 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "14": { if (ExportType == "Excel")obj.Date14 = "W"; else { obj.Date14 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "15": { if (ExportType == "Excel")obj.Date15 = "W"; else { obj.Date15 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "16": { if (ExportType == "Excel")obj.Date16 = "W"; else { obj.Date16 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "17": { if (ExportType == "Excel")obj.Date17 = "W"; else { obj.Date17 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "18": { if (ExportType == "Excel")obj.Date18 = "W"; else { obj.Date18 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "19": { if (ExportType == "Excel")obj.Date19 = "W"; else { obj.Date19 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "20": { if (ExportType == "Excel")obj.Date20 = "W"; else { obj.Date20 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "21": { if (ExportType == "Excel")obj.Date21 = "W"; else { obj.Date21 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "22": { if (ExportType == "Excel")obj.Date22 = "W"; else { obj.Date22 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "23": { if (ExportType == "Excel")obj.Date23 = "W"; else { obj.Date23 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "24": { if (ExportType == "Excel")obj.Date24 = "W"; else { obj.Date24 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "25": { if (ExportType == "Excel")obj.Date25 = "W"; else { obj.Date25 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "26": { if (ExportType == "Excel")obj.Date26 = "W"; else { obj.Date26 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "27": { if (ExportType == "Excel")obj.Date27 = "W"; else { obj.Date27 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "28": { if (ExportType == "Excel")obj.Date28 = "W"; else { obj.Date28 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "29": { if (ExportType == "Excel")obj.Date29 = "W"; else { obj.Date29 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "30": { if (ExportType == "Excel")obj.Date30 = "W"; else { obj.Date30 = "<b style='color:Blue'>W</b>"; } } break;
                                            case "31": { if (ExportType == "Excel")obj.Date31 = "W"; else { obj.Date31 = "<b style='color:Blue'>W</b>"; } } break;
                                            default: break;
                                        }
                                    }
                                }
                            }
                        }
                        List<string> value = getAllSundays(Convert.ToInt32(Year));
                        var lastdateofmonth = "28" + "/" + String.Join("/", Attemonthyear).ToString();
                        var day = lastdateofmonth.Split('/');
                        var lastday = day[0];
                        var currentMnthHolidays = (from v in value
                                                   where v.Substring(3, 2) == Convert.ToString(Month)
                                                       && Convert.ToInt64(v.Substring(0, 2)) <= Convert.ToInt64(lastday)
                                                   select v);
                        int Hcount = 0;
                        foreach (string s in currentMnthHolidays)
                        {

                            switch (s.Substring(0, 2))
                            {
                                case "01": { if (ExportType == "Excel")obj.Date1 = "H"; else { obj.Date1 = "<b style='color:blue'>H</b>"; } } break;
                                case "02": { if (ExportType == "Excel")obj.Date2 = "H"; else { obj.Date2 = "<b style='color:blue'>H</b>"; } } break;
                                case "03": { if (ExportType == "Excel")obj.Date3 = "H"; else { obj.Date3 = "<b style='color:blue'>H</b>"; } } break;
                                case "04": { if (ExportType == "Excel")obj.Date4 = "H"; else { obj.Date4 = "<b style='color:blue'>H</b>"; } } break;
                                case "05": { if (ExportType == "Excel")obj.Date5 = "H"; else { obj.Date5 = "<b style='color:blue'>H</b>"; } } break;
                                case "06": { if (ExportType == "Excel")obj.Date6 = "H"; else { obj.Date6 = "<b style='color:blue'>H</b>"; } } break;
                                case "07": { if (ExportType == "Excel")obj.Date7 = "H"; else { obj.Date7 = "<b style='color:blue'>H</b>"; } } break;
                                case "08": { if (ExportType == "Excel")obj.Date8 = "H"; else { obj.Date8 = "<b style='color:blue'>H</b>"; } } break;
                                case "09": { if (ExportType == "Excel")obj.Date9 = "H"; else { obj.Date9 = "<b style='color:blue'>H</b>"; } } break;
                                case "10": { if (ExportType == "Excel")obj.Date10 = "H"; else { obj.Date10 = "<b style='color:blue'>H</b>"; } } break;
                                case "11": { if (ExportType == "Excel")obj.Date11 = "H"; else { obj.Date11 = "<b style='color:blue'>H</b>"; } } break;
                                case "12": { if (ExportType == "Excel")obj.Date12 = "H"; else { obj.Date12 = "<b style='color:blue'>H</b>"; } } break;
                                case "13": { if (ExportType == "Excel")obj.Date13 = "H"; else { obj.Date13 = "<b style='color:blue'>H</b>"; } } break;
                                case "14": { if (ExportType == "Excel")obj.Date14 = "H"; else { obj.Date14 = "<b style='color:blue'>H</b>"; } } break;
                                case "15": { if (ExportType == "Excel")obj.Date15 = "H"; else { obj.Date15 = "<b style='color:blue'>H</b>"; } } break;
                                case "16": { if (ExportType == "Excel")obj.Date16 = "H"; else { obj.Date16 = "<b style='color:blue'>H</b>"; } } break;
                                case "17": { if (ExportType == "Excel")obj.Date17 = "H"; else { obj.Date17 = "<b style='color:blue'>H</b>"; } } break;
                                case "18": { if (ExportType == "Excel")obj.Date18 = "H"; else { obj.Date18 = "<b style='color:blue'>H</b>"; } } break;
                                case "19": { if (ExportType == "Excel")obj.Date19 = "H"; else { obj.Date19 = "<b style='color:blue'>H</b>"; } } break;
                                case "20": { if (ExportType == "Excel")obj.Date20 = "H"; else { obj.Date20 = "<b style='color:blue'>H</b>"; } } break;
                                case "21": { if (ExportType == "Excel")obj.Date21 = "H"; else { obj.Date21 = "<b style='color:blue'>H</b>"; } } break;
                                case "22": { if (ExportType == "Excel")obj.Date22 = "H"; else { obj.Date22 = "<b style='color:blue'>H</b>"; } } break;
                                case "23": { if (ExportType == "Excel")obj.Date23 = "H"; else { obj.Date23 = "<b style='color:blue'>H</b>"; } } break;
                                case "24": { if (ExportType == "Excel")obj.Date24 = "H"; else { obj.Date24 = "<b style='color:blue'>H</b>"; } } break;
                                case "25": { if (ExportType == "Excel")obj.Date25 = "H"; else { obj.Date25 = "<b style='color:blue'>H</b>"; } } break;
                                case "26": { if (ExportType == "Excel")obj.Date26 = "H"; else { obj.Date26 = "<b style='color:blue'>H</b>"; } } break;
                                case "27": { if (ExportType == "Excel")obj.Date27 = "H"; else { obj.Date27 = "<b style='color:blue'>H</b>"; } } break;
                                case "28": { if (ExportType == "Excel")obj.Date28 = "H"; else { obj.Date28 = "<b style='color:blue'>H</b>"; } } break;
                                case "29": { if (ExportType == "Excel")obj.Date29 = "H"; else { obj.Date29 = "<b style='color:blue'>H</b>"; } } break;
                                case "30": { if (ExportType == "Excel")obj.Date30 = "H"; else { obj.Date30 = "<b style='color:blue'>H</b>"; } } break;
                                case "31": { if (ExportType == "Excel")obj.Date31 = "H"; else { obj.Date31 = "<b style='color:blue'>H</b>"; } } break;
                                default: break;
                            }
                            Hcount = Hcount + 1;
                        }

                        var month = new DateTime(Convert.ToInt32(Attemonthyear[1]), Convert.ToInt32(Attemonthyear[0]), 1);
                        var LastMonthDate = month.AddMonths(-1);
                        var Date = DateTime.ParseExact(Convert.ToString(LastMonthDate), "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        var lDate = Date.ToString("dd/MM/yyyy");
                        var LastMonthArr = lDate.Split('-');
                        var LastMonth = LastMonthArr[1];
                        var lastMnthHolidays = (from v in value where v.Substring(3, 2) == Convert.ToString(LastMonth) select v);
                        int Pcount = 0;
                        foreach (string s in currentMnthHolidays)
                        {

                            switch (s.Substring(0, 2))
                            {
                                case "01": { if (ExportType == "Excel")obj.Date1 = "H"; else { obj.Date1 = "<b style='color:blue'>H</b>"; } } break;
                                case "02": { if (ExportType == "Excel")obj.Date2 = "H"; else { obj.Date2 = "<b style='color:blue'>H</b>"; } } break;
                                case "03": { if (ExportType == "Excel")obj.Date3 = "H"; else { obj.Date3 = "<b style='color:blue'>H</b>"; } } break;
                                case "04": { if (ExportType == "Excel")obj.Date4 = "H"; else { obj.Date4 = "<b style='color:blue'>H</b>"; } } break;
                                case "05": { if (ExportType == "Excel")obj.Date5 = "H"; else { obj.Date5 = "<b style='color:blue'>H</b>"; } } break;
                                case "06": { if (ExportType == "Excel")obj.Date6 = "H"; else { obj.Date6 = "<b style='color:blue'>H</b>"; } } break;
                                case "07": { if (ExportType == "Excel")obj.Date7 = "H"; else { obj.Date7 = "<b style='color:blue'>H</b>"; } } break;
                                case "08": { if (ExportType == "Excel")obj.Date8 = "H"; else { obj.Date8 = "<b style='color:blue'>H</b>"; } } break;
                                case "09": { if (ExportType == "Excel")obj.Date9 = "H"; else { obj.Date9 = "<b style='color:blue'>H</b>"; } } break;
                                case "10": { if (ExportType == "Excel")obj.Date10 = "H"; else { obj.Date10 = "<b style='color:blue'>H</b>"; } } break;
                                case "11": { if (ExportType == "Excel")obj.Date11 = "H"; else { obj.Date11 = "<b style='color:blue'>H</b>"; } } break;
                                case "12": { if (ExportType == "Excel")obj.Date12 = "H"; else { obj.Date12 = "<b style='color:blue'>H</b>"; } } break;
                                case "13": { if (ExportType == "Excel")obj.Date13 = "H"; else { obj.Date13 = "<b style='color:blue'>H</b>"; } } break;
                                case "14": { if (ExportType == "Excel")obj.Date14 = "H"; else { obj.Date14 = "<b style='color:blue'>H</b>"; } } break;
                                case "15": { if (ExportType == "Excel")obj.Date15 = "H"; else { obj.Date15 = "<b style='color:blue'>H</b>"; } } break;
                                case "16": { if (ExportType == "Excel")obj.Date16 = "H"; else { obj.Date16 = "<b style='color:blue'>H</b>"; } } break;
                                case "17": { if (ExportType == "Excel")obj.Date17 = "H"; else { obj.Date17 = "<b style='color:blue'>H</b>"; } } break;
                                case "18": { if (ExportType == "Excel")obj.Date18 = "H"; else { obj.Date18 = "<b style='color:blue'>H</b>"; } } break;
                                case "19": { if (ExportType == "Excel")obj.Date19 = "H"; else { obj.Date19 = "<b style='color:blue'>H</b>"; } } break;
                                case "20": { if (ExportType == "Excel")obj.Date20 = "H"; else { obj.Date20 = "<b style='color:blue'>H</b>"; } } break;
                                case "21": { if (ExportType == "Excel")obj.Date21 = "H"; else { obj.Date21 = "<b style='color:blue'>H</b>"; } } break;
                                case "22": { if (ExportType == "Excel")obj.Date22 = "H"; else { obj.Date22 = "<b style='color:blue'>H</b>"; } } break;
                                case "23": { if (ExportType == "Excel")obj.Date23 = "H"; else { obj.Date23 = "<b style='color:blue'>H</b>"; } } break;
                                case "24": { if (ExportType == "Excel")obj.Date24 = "H"; else { obj.Date24 = "<b style='color:blue'>H</b>"; } } break;
                                case "25": { if (ExportType == "Excel")obj.Date25 = "H"; else { obj.Date25 = "<b style='color:blue'>H</b>"; } } break;
                                case "26": { if (ExportType == "Excel")obj.Date26 = "H"; else { obj.Date26 = "<b style='color:blue'>H</b>"; } } break;
                                case "27": { if (ExportType == "Excel")obj.Date27 = "H"; else { obj.Date27 = "<b style='color:blue'>H</b>"; } } break;
                                case "28": { if (ExportType == "Excel")obj.Date28 = "H"; else { obj.Date28 = "<b style='color:blue'>H</b>"; } } break;
                                case "29": { if (ExportType == "Excel")obj.Date29 = "H"; else { obj.Date29 = "<b style='color:blue'>H</b>"; } } break;
                                case "30": { if (ExportType == "Excel")obj.Date30 = "H"; else { obj.Date30 = "<b style='color:blue'>H</b>"; } } break;
                                case "31": { if (ExportType == "Excel")obj.Date31 = "H"; else { obj.Date31 = "<b style='color:blue'>H</b>"; } } break;
                                default: break;
                            }
                            Pcount = Pcount + 1;
                        }
                        if (list.Contains(obj.PreRegNum))
                        {
                            var PresentDate = (from d in AttDate
                                               where d.AttendanceDate != null && d.PreRegNum == obj.PreRegNum &&
                                                Convert.ToDateTime(AttendanceFromDate) <= d.AttendanceDate
                                                 && Convert.ToDateTime(AttendanceToDate) >= d.AttendanceDate
                                               select d).ToArray();
                            for (var i = 0; i < PresentDate.Length; i++)
                            {
                                string PreDate = PresentDate[i].AttendanceDate.Value.ToString("dd");
                                switch (PreDate)
                                {
                                    case "1":
                                    case "01": { if (ExportType == "Excel")obj.Date1 = "<b style='color:Green'>P</b>"; else { obj.Date1 = "<b style='color:Green'>P</b>"; } } break;
                                    case "2":
                                    case "02": { if (ExportType == "Excel")obj.Date2 = "<b style='color:Green'>P</b>"; else { obj.Date2 = "<b style='color:Green'>P</b>"; } } break;
                                    case "3":
                                    case "03": { if (ExportType == "Excel")obj.Date3 = "<b style='color:Green'>P</b>"; else { obj.Date3 = "<b style='color:Green'>P</b>"; } } break;
                                    case "4":
                                    case "04": { if (ExportType == "Excel")obj.Date4 = "<b style='color:Green'>P</b>"; else { obj.Date4 = "<b style='color:Green'>P</b>"; } } break;
                                    case "5":
                                    case "05": { if (ExportType == "Excel")obj.Date5 = "<b style='color:Green'>P</b>"; else { obj.Date5 = "<b style='color:Green'>P</b>"; } } break;
                                    case "6":
                                    case "06": { if (ExportType == "Excel")obj.Date6 = "<b style='color:Green'>P</b>"; else { obj.Date6 = "<b style='color:Green'>P</b>"; } } break;
                                    case "7":
                                    case "07": { if (ExportType == "Excel")obj.Date7 = "<b style='color:Green'>P</b>"; else { obj.Date7 = "<b style='color:Green'>P</b>"; } } break;
                                    case "8":
                                    case "08": { if (ExportType == "Excel")obj.Date8 = "<b style='color:Green'>P</b>"; else { obj.Date8 = "<b style='color:Green'>P</b>"; } } break;
                                    case "9":
                                    case "09": { if (ExportType == "Excel")obj.Date9 = "<b style='color:Green'>P</b>"; else { obj.Date9 = "<b style='color:Green'>P</b>"; } } break;
                                    case "10": { if (ExportType == "Excel")obj.Date10 = "<b style='color:Green'>P</b>"; else { obj.Date10 = "<b style='color:Green'>P</b>"; } } break;
                                    case "11": { if (ExportType == "Excel")obj.Date11 = "<b style='color:Green'>P</b>"; else { obj.Date11 = "<b style='color:Green'>P</b>"; } } break;
                                    case "12": { if (ExportType == "Excel")obj.Date12 = "<b style='color:Green'>P</b>"; else { obj.Date12 = "<b style='color:Green'>P</b>"; } } break;
                                    case "13": { if (ExportType == "Excel")obj.Date13 = "<b style='color:Green'>P</b>"; else { obj.Date13 = "<b style='color:Green'>P</b>"; } } break;
                                    case "14": { if (ExportType == "Excel")obj.Date14 = "<b style='color:Green'>P</b>"; else { obj.Date14 = "<b style='color:Green'>P</b>"; } } break;
                                    case "15": { if (ExportType == "Excel")obj.Date15 = "<b style='color:Green'>P</b>"; else { obj.Date15 = "<b style='color:Green'>P</b>"; } } break;
                                    case "16": { if (ExportType == "Excel")obj.Date16 = "<b style='color:Green'>P</b>"; else { obj.Date16 = "<b style='color:Green'>P</b>"; } } break;
                                    case "17": { if (ExportType == "Excel")obj.Date17 = "<b style='color:Green'>P</b>"; else { obj.Date17 = "<b style='color:Green'>P</b>"; } } break;
                                    case "18": { if (ExportType == "Excel")obj.Date18 = "<b style='color:Green'>P</b>"; else { obj.Date18 = "<b style='color:Green'>P</b>"; } } break;
                                    case "19": { if (ExportType == "Excel")obj.Date19 = "<b style='color:Green'>P</b>"; else { obj.Date19 = "<b style='color:Green'>P</b>"; } } break;
                                    case "20": { if (ExportType == "Excel")obj.Date20 = "<b style='color:Green'>P</b>"; else { obj.Date20 = "<b style='color:Green'>P</b>"; } } break;
                                    case "21": { if (ExportType == "Excel")obj.Date21 = "<b style='color:Green'>P</b>"; else { obj.Date21 = "<b style='color:Green'>P</b>"; } } break;
                                    case "22": { if (ExportType == "Excel")obj.Date22 = "<b style='color:Green'>P</b>"; else { obj.Date22 = "<b style='color:Green'>P</b>"; } } break;
                                    case "23": { if (ExportType == "Excel")obj.Date23 = "<b style='color:Green'>P</b>"; else { obj.Date23 = "<b style='color:Green'>P</b>"; } } break;
                                    case "24": { if (ExportType == "Excel")obj.Date24 = "<b style='color:Green'>P</b>"; else { obj.Date24 = "<b style='color:Green'>P</b>"; } } break;
                                    case "25": { if (ExportType == "Excel")obj.Date25 = "<b style='color:Green'>P</b>"; else { obj.Date25 = "<b style='color:Green'>P</b>"; } } break;
                                    case "26": { if (ExportType == "Excel")obj.Date26 = "<b style='color:Green'>P</b>"; else { obj.Date26 = "<b style='color:Green'>P</b>"; } } break;
                                    case "27": { if (ExportType == "Excel")obj.Date27 = "<b style='color:Green'>P</b>"; else { obj.Date27 = "<b style='color:Green'>P</b>"; } } break;
                                    case "28": { if (ExportType == "Excel")obj.Date28 = "<b style='color:Green'>P</b>"; else { obj.Date28 = "<b style='color:Green'>P</b>"; } } break;
                                    case "29": { if (ExportType == "Excel")obj.Date29 = "<b style='color:Green'>P</b>"; else { obj.Date29 = "<b style='color:Green'>P</b>"; } } break;
                                    case "30": { if (ExportType == "Excel")obj.Date30 = "<b style='color:Green'>P</b>"; else { obj.Date30 = "<b style='color:Green'>P</b>"; } } break;
                                    case "31": { if (ExportType == "Excel")obj.Date31 = "<b style='color:Green'>P</b>"; else { obj.Date31 = "<b style='color:Green'>P</b>"; } } break;
                                    default: break;
                                }
                            }
                        }
                    }
                    if (ExportType == "Excel")
                    {
                        DateTime dtDate = new DateTime(Convert.ToInt32(AttendanceDate[1]), Convert.ToInt32(AttendanceDate[0]), 1);
                        string MonthName = dtDate.ToString("MMMM");
                        if (AttendanceDate[0] == "01" || AttendanceDate[0] == "05" || AttendanceDate[0] == "07" || AttendanceDate[0] == "08" || AttendanceDate[0] == "10" || AttendanceDate[0] == "12")
                        {
                            string headerTable = @"<Table border='1px' cellpadding='9' cellspacing='0'><tr><td colspan='2'><b>Campus - " + Campus + "</td></b><td colspan='2'><b>Month - " + MonthName + "</b></td><td colspan='2'><b> Year - " + AttendanceDate[1] + "<b></td> <td colspan='29' align='center' style='font-size: large;'>The Indian Public School</td></tr></b></Table>";
                            ExptToXLWithHeader(FinalConsolidateList, "Staff_Attendnace_Report", (items => new
                            {
                                items.Name,
                                items.IdNumber,
                                items.Date29,
                                items.Date30,
                                items.Date31,
                                items.Date1,
                                items.Date2,
                                items.Date3,
                                items.Date4,
                                items.Date5,
                                items.Date6,
                                items.Date7,
                                items.Date8,
                                items.Date9,
                                items.Date10,
                                items.Date11,
                                items.Date12,
                                items.Date13,
                                items.Date14,
                                items.Date15,
                                items.Date16,
                                items.Date17,
                                items.Date18,
                                items.Date19,
                                items.Date20,
                                items.Date21,
                                items.Date22,
                                items.Date23,
                                items.Date24,
                                items.Date25,
                                items.Date26,
                                items.Date27,
                                items.Date28,
                                Total_No_Of_Days_Present = items.TotalWorkedDays,
                                No_Of_Days_Leave_Taken = (items.TotalDays - items.TotalWorkedDays)
                            }), headerTable);
                            return new EmptyResult();
                        }
                        if (Convert.ToInt32(AttendanceDate[0]) == 3)
                        {
                            var FebMonthDays = DateTime.DaysInMonth(Convert.ToInt32(AttendanceDate[1]), Convert.ToInt32(AttendanceDate[0]) - 1);
                            if (FebMonthDays == 28)
                            {
                                string headerTable = @"<Table border='1px' cellpadding='9' cellspacing='0'><tr><td colspan='2'><b>Campus - " + Campus + "</b></td><td colspan='2'><b>Month - " + MonthName + "<b></td><td colspan='2'><b>Year - " + AttendanceDate[1] + "</b></td> <td colspan='29' align='center' style='font-size: large;'>The Indian Public School</td></tr></b></Table>";
                                ExptToXLWithHeader(FinalConsolidateList, "Staff_Attendnace_Report", (items => new
                                {
                                    items.Name,
                                    items.IdNumber,
                                    items.Date1,
                                    items.Date2,
                                    items.Date3,
                                    items.Date4,
                                    items.Date5,
                                    items.Date6,
                                    items.Date7,
                                    items.Date8,
                                    items.Date9,
                                    items.Date10,
                                    items.Date11,
                                    items.Date12,
                                    items.Date13,
                                    items.Date14,
                                    items.Date15,
                                    items.Date16,
                                    items.Date17,
                                    items.Date18,
                                    items.Date19,
                                    items.Date20,
                                    items.Date21,
                                    items.Date22,
                                    items.Date23,
                                    items.Date24,
                                    items.Date25,
                                    items.Date26,
                                    items.Date27,
                                    items.Date28,
                                    Total_No_Of_Days_Present = items.TotalWorkedDays,
                                    No_Of_Days_Leave_Taken = (items.TotalDays - items.TotalWorkedDays)
                                }), headerTable);
                                return new EmptyResult();
                            }
                            else
                            {
                                string headerTable = @"<Table border='1px' cellpadding='9' cellspacing='0'><tr><td colspan='2'><b>Campus - " + Campus + "</b></td><td colspan='2'><b>Month - " + MonthName + "</b></td> <td colspan='2'><b>Year - " + AttendanceDate[1] + "</b></td> <td colspan='29' align='center' style='font-size: large;'>The Indian Public School</td></tr></b></Table>";
                                ExptToXLWithHeader(FinalConsolidateList, "Staff_Attendnace_Report", (items => new
                                {
                                    items.Name,
                                    items.IdNumber,
                                    items.Date29,
                                    items.Date1,
                                    items.Date2,
                                    items.Date3,
                                    items.Date4,
                                    items.Date5,
                                    items.Date6,
                                    items.Date7,
                                    items.Date8,
                                    items.Date9,
                                    items.Date10,
                                    items.Date11,
                                    items.Date12,
                                    items.Date13,
                                    items.Date14,
                                    items.Date15,
                                    items.Date16,
                                    items.Date17,
                                    items.Date18,
                                    items.Date19,
                                    items.Date20,
                                    items.Date21,
                                    items.Date22,
                                    items.Date23,
                                    items.Date24,
                                    items.Date25,
                                    items.Date26,
                                    items.Date27,
                                    items.Date28,
                                    Total_No_Of_Days_Present = items.TotalWorkedDays,
                                    No_Of_Days_Leave_Taken = (items.TotalDays - items.TotalWorkedDays)
                                }), headerTable);
                                return new EmptyResult();
                            }
                        }
                        else
                        {
                            string headerTable = @"<Table border='1px' cellpadding='9' cellspacing='0'><tr><td colspan='2'><b>Campus - " + Campus + "<b></td><td colspan='2'><b>Month - " + MonthName + "</b></td><td colspan='2'><b>Year - " + AttendanceDate[1] + "</b></td> <td colspan='29' align='center' style='font-size: large;'>The Indian Public School</td></tr></b></Table>";
                            ExptToXLWithHeader(FinalConsolidateList, "Staff_Attendnace_Report", (items => new
                            {
                                items.Name,
                                items.IdNumber,
                                items.Date29,
                                items.Date30,
                                items.Date31,
                                items.Date1,
                                items.Date2,
                                items.Date3,
                                items.Date4,
                                items.Date5,
                                items.Date6,
                                items.Date7,
                                items.Date8,
                                items.Date9,
                                items.Date10,
                                items.Date11,
                                items.Date12,
                                items.Date13,
                                items.Date14,
                                items.Date15,
                                items.Date16,
                                items.Date17,
                                items.Date18,
                                items.Date19,
                                items.Date20,
                                items.Date21,
                                items.Date22,
                                items.Date23,
                                items.Date24,
                                items.Date25,
                                items.Date26,
                                items.Date27,
                                items.Date28,
                                Total_No_Of_Days_Present = items.TotalWorkedDays,
                                No_Of_Days_Leave_Taken = (items.TotalDays - items.TotalWorkedDays)
                            }), headerTable);
                            return new EmptyResult();
                        }
                    }
                    long totalrecords = FinalConsolidateList.Count;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in FinalConsolidateList
                                select new
                                {
                                    i = items.Id,
                                    cell = new string[] {
                                                      items.Id.ToString(),
                               items.Name,items.IdNumber,
                               items.Date29,items.Date30,items.Date31,items.Date1,items.Date2,items.Date3,items.Date4,items.Date5,items.Date6,items.Date7,items.Date8,items.Date9,items.Date10,items.Date11,items.Date12,items.Date13,items.Date14,items.Date15,items.Date16,items.Date17,items.Date18,items.Date19,
                               items.Date20,items.Date21,items.Date22,items.Date23,items.Date24,items.Date25,items.Date26,items.Date27,items.Date28,
                               items.TotalWorkedDays.ToString(),(items.TotalDays - items.TotalWorkedDays).ToString()
                            }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw;
            }
        }
        public List<string> getAllSundays(int Year)
        {
            List<string> strDates = new List<string>();
            for (int month = 1; month <= 12; month++)
            {
                DateTime dt = new DateTime(Year, month, 1);
                int firstSundayOfMonth = (int)dt.DayOfWeek;
                if (firstSundayOfMonth != 0)
                {
                    dt = dt.AddDays((6 - firstSundayOfMonth) + 1);
                }
                while (dt.Month == month)
                {
                    strDates.Add(dt.ToString("dd/MM/yyyy"));
                    dt = dt.AddDays(7);
                }
            }
            return strDates;
        }
        #endregion
        public IList<Staff_AttendanceRegisterReport> ConsolidateLogListForAttendanceForRegister(List<Staff_AttendanceRegisterReport> TotalLogCollections, List<Staff_AttendanceRegisterReport> TotalLogCollectionsForResigned, string AttendanceFromDate, string AttendanceToDate)
        {
            long NoOfDaysWeekOff = Convert.ToInt64(ConfigurationManager.AppSettings["NoOfDaysWeekOff"]);
            //List<Staff_ConsolidateDeviceLogSummary_SP> TotalLogCollectionsForResigned = new List<Staff_ConsolidateDeviceLogSummary_SP>();
            IList<Staff_AttendanceRegisterReport> DeviceLogsList = new List<Staff_AttendanceRegisterReport>();
            IList<Staff_AttendanceRegisterReport> ConsolidateLogList = new List<Staff_AttendanceRegisterReport>();
            IList<Staff_AttendanceRegisterReport> ConsolidateLogListNew = new List<Staff_AttendanceRegisterReport>();
            IList<Staff_AttendanceRegisterReport> FinalConsolidateList = new List<Staff_AttendanceRegisterReport>();
            if (TotalLogCollections != null && TotalLogCollections.Count > 0)
            {
                ConsolidateLogList = (from c in TotalLogCollections
                                      group c by new
                                      {
                                          c.PreRegNum,
                                          c.Name,
                                          c.IdNumber,
                                          c.Campus,
                                          c.CurrentStatus,
                                          c.TotalDays,

                                      } into gcs
                                      select new Staff_AttendanceRegisterReport()
                                      {
                                          PreRegNum = gcs.Key.PreRegNum,
                                          Name = gcs.Key.Name,
                                          IdNumber = gcs.Key.IdNumber,
                                          Campus = gcs.Key.Campus,
                                          CurrentStatus = gcs.Key.CurrentStatus,
                                          TotalDays = gcs.Key.TotalDays,
                                          TotalWorkedDays = (gcs.Sum(x => x.TotalWorkedDays)) + NoOfDaysWeekOff,
                                          NoOfLeaveTaken = gcs.FirstOrDefault().NoOfLeaveTaken,

                                      }).ToList();

                if (TotalLogCollectionsForResigned != null && TotalLogCollectionsForResigned.Count > 0)
                {
                    ConsolidateLogListNew = (from c in TotalLogCollectionsForResigned
                                             where Convert.ToDateTime(AttendanceFromDate) <= c.DateOfLongLeaveAndResigned
                                             && Convert.ToDateTime(AttendanceToDate) >= c.DateOfLongLeaveAndResigned
                                             group c by new
                                             {
                                                 c.PreRegNum,
                                                 c.Name,
                                                 c.IdNumber,
                                                 c.Campus,
                                                 c.CurrentStatus,
                                                 c.TotalDays,

                                             } into gcs
                                             select new Staff_AttendanceRegisterReport()
                                             {
                                                 PreRegNum = gcs.Key.PreRegNum,
                                                 Name = gcs.Key.Name,
                                                 IdNumber = gcs.Key.IdNumber,
                                                 Campus = gcs.Key.Campus,
                                                 CurrentStatus = gcs.Key.CurrentStatus,
                                                 TotalDays = gcs.Key.TotalDays,
                                                 TotalWorkedDays = (gcs.Sum(x => x.TotalWorkedDays)) + NoOfDaysWeekOff,
                                                 NoOfLeaveTaken = gcs.FirstOrDefault().NoOfLeaveTaken,

                                             }).ToList();
                }

                // FinalConsolidateList = ConsolidateLogListNew.Concat(ConsolidateLogList).ToList();
                FinalConsolidateList = ConsolidateLogList.Concat(ConsolidateLogListNew).ToList();
                return FinalConsolidateList;
            }
            return null;
        }
    }
}
