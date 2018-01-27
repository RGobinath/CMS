using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TIPS.Service;
using TIPS.Entities.WidgetEntities;
using TIPS.Entities;
using TIPS.Entities.Assess.ReportCardClasses;
using TIPS.ServiceContract;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TIPS.Entities.AdmissionEntities;
using TIPS.Entities.StaffManagementEntities;
using CustomAuthentication;
using TIPS.Entities.StoreEntities;
using TIPS.Entities.Attendance;
using TIPS.Entities.Assess;
using TIPS.Entities.ParentPortalEntities;
using TIPS.Entities.TransportEntities;


namespace CMS.Controllers
{
    public class WidgetController : BaseController
    {
        string policyName = "WidgetPolicy";
        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
        //
        // GET: /Widget/
        WidgetService ws = new WidgetService();
        UserService us = new UserService();
        Dictionary<string, object> criteria = new Dictionary<string, object>();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult WidgetBoard(long? Count)
        {
            try
            {
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    criteria.Clear();
                    criteria.Add("UserId", userId);
                    Dictionary<long, IList<UserAppRole>> UserRole = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                    if (UserRole != null && UserRole.Count > 0 && UserRole.FirstOrDefault().Key > 0)
                    {
                        var UserRoleList = (from u in UserRole.First().Value
                                            where u.RoleCode != null
                                            select new { u.AppCode }).Distinct().ToArray();

                        ArrayList Role = new ArrayList();
                        foreach (var item in UserRoleList)
                        {
                            Role.Add(item.AppCode);
                        }
                        criteria.Clear();
                        criteria.Add("UserId", userId);
                        criteria.Add("RoleCode", Role);
                        Dictionary<long, IList<WidgetUserConfig>> widgetConfig = ws.GetUserWidgetConfigWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                        if (widgetConfig != null && widgetConfig.Count > 0 && widgetConfig.FirstOrDefault().Key > 0)
                        {
                            var WidgetList = (from u in widgetConfig.First().Value
                                              where u.WidgetMaster != null && u.IsActive == true
                                              select new { u.WidgetMaster, u.WidgetName, u.DivId, u.CloseId }).Distinct().ToArray();

                            if (WidgetList != null)
                            {
                                List<IWidget> Board = new List<IWidget>();
                                foreach (var item in WidgetList)
                                {
                                    //var HeaderText = item.WidgetMaster;
                                    Board.Add(
                                    new WidgetBoard()
                                    {
                                        SortOrder = 1,
                                        ClassName = "high",
                                        HeaderText = item.WidgetMaster,//+ "-Management",
                                        FooterText = "Footer",
                                        WidgetName = item.WidgetName,
                                        DivId = item.DivId,
                                        CloseId = item.CloseId,
                                    });
                                }
                                if (Count == null)
                                    ViewBag.count = 3;
                                else
                                    ViewBag.count = Count;
                                ViewBag.Widgets = Board;
                                ViewBag.btnview = Board.Count;
                            }
                        }
                        else
                        {
                            ViewBag.Widgets = null;
                            return View();

                        }
                    }
                    else return null;
                    return View();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public JsonResult JqgridWidgets(int rows, string sidx, string sord, int? page = 1)
        {


            var jsonData = new
            {
                total = 1, // we'll implement later
                page = 5,
                records = 4, // implement later
                rows = new[]{
                new {id = 1, cell = new[] {"1", "John", "Male","12"}},
                new {id = 2, cell = new[] {"2", "Mani", "Male","17"}},
                new {id = 3, cell = new[] {"3", "Alex", "Male","90"}},
                new {id = 4, cell = new[] {"4", "Rani", "Female","23"}}
                }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        #region New Added
        public ActionResult FillWidgets()
        {

            try
            {
                MastersService mssvc = new MastersService();
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    criteria.Add("UserId", userId);
                    Dictionary<long, IList<UserAppRole>> UserRole = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                    if (UserRole != null && UserRole.Count > 0 && UserRole.FirstOrDefault().Key > 0)
                    {
                        var UserRoleList = (from u in UserRole.First().Value
                                            where u.RoleCode != null
                                            select new { u.AppCode }).Distinct().ToArray();

                        ArrayList Role = new ArrayList();

                        //string[] Role = new string[1];
                        //int i = 0;
                        foreach (var item in UserRoleList)
                        {
                            Role.Add(item.AppCode);
                           // i++;
                        }
                        string[] myRole = (string[])Role.ToArray(typeof(string));
                        criteria.Clear();
                        //criteria.Add("UserId", userId);
                        criteria.Add("RoleCode", myRole);
                        Dictionary<long, IList<WidgetMaster>> WidgetList = ws.GetWidgetMasterListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                        var Widgetname = (
                              from items in WidgetList.FirstOrDefault().Value
                              select new
                              {
                                  Text = items.WidgetModule,
                                  Value = items.RoleCode,
                              }).Distinct().ToList();
                        return Json(Widgetname, JsonRequestBehavior.AllowGet);
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }

        }

        public ActionResult AddWidgets(string widgets)
        {
            string userId = ValidateUser();
            if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
            else
            {
                WidgetUserConfig widusr = new WidgetUserConfig();
                criteria.Clear();
                criteria.Add("UserId", userId);
                Dictionary<long, IList<WidgetUserConfig>> widgetConfigure = ws.GetUserWidgetConfigWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                if (widgetConfigure != null && widgetConfigure.FirstOrDefault().Value != null && widgetConfigure.FirstOrDefault().Value.Count > 0)
                {
                    foreach (var items in widgetConfigure.First().Value)
                    {
                        widusr = ws.GetWidgetUsrConfigById(items.Id);
                        widusr.IsActive = false;
                        ws.SaveOrUpdateWidgetusrConfig(widusr);
                    }
                }
                string[] Widgetsarr = null;
                if (widgets != "null" && widgets != null)
                {
                    Widgetsarr = widgets.Split(',');
                }
                for (int i = 0; i < Widgetsarr.Length; i++)
                {
                    criteria.Clear();
                    criteria.Add("UserId", userId);
                    criteria.Add("RoleCode", Widgetsarr[i]);
                    Dictionary<long, IList<WidgetUserConfig>> widgetConfig = ws.GetUserWidgetConfigWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                    if (widgetConfig != null && widgetConfig.FirstOrDefault().Value != null && widgetConfig.FirstOrDefault().Value.Count > 0)
                    {
                        foreach (var items in widgetConfig.First().Value)
                        {
                            widusr = ws.GetWidgetUsrConfigById(items.Id);
                            widusr.IsActive = true;
                            ws.SaveOrUpdateWidgetusrConfig(widusr);
                        }
                    }
                    else
                    {
                        WidgetUserConfig widusrconfig = new WidgetUserConfig();
                        criteria.Clear();
                        criteria.Add("RoleCode", Widgetsarr[i]);
                        Dictionary<long, IList<WidgetMaster>> WidgetList = ws.GetWidgetMasterListWithPagingAndCriteria(null, 10000, null, null, criteria);
                        if (WidgetList != null && WidgetList.FirstOrDefault().Value != null && WidgetList.FirstOrDefault().Value.Count > 0)
                        {
                            foreach (var item in WidgetList.First().Value)
                            {
                                //if(item.RoleCode=="STR")
                                widusrconfig.UserId = userId;
                                widusrconfig.UserCode = "1";
                                widusrconfig.WidgetMaster = item.Description;
                                widusrconfig.WidgetName = item.WidgetName;
                                widusrconfig.RoleCode = item.RoleCode;
                                widusrconfig.IsActive = true;
                                widusrconfig.DivId = item.DivId;
                                widusrconfig.CloseId = item.CloseId;
                                widusrconfig.CreatedBy = userId;
                                widusrconfig.CreatedDate = DateTime.Now;
                                ws.SaveOrUpdateWidgetusrConfig(widusrconfig);
                            }
                        }
                    }
                }
                return Json(null, JsonRequestBehavior.AllowGet);//RedirectToAction("WidgetBoard");
            }
        }
        #endregion

        public ActionResult Test()
        {

            return View();
        }

        public JsonResult Studentlistjqgrid(string PreRegNum, string Name, string Campus, string AdmissionStatus, int? rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                criteria.Clear();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(PreRegNum))
                    criteria.Add("PreRegNum", Convert.ToInt64(PreRegNum));
                if (!string.IsNullOrEmpty(Name))
                    criteria.Add("Name", Name);
                if (!string.IsNullOrEmpty(Campus))
                    criteria.Add("Campus", Campus);
                if (!string.IsNullOrEmpty(AdmissionStatus))
                    criteria.Add("AdmissionStatus", AdmissionStatus);

                Dictionary<long, IList<StudentTemplate>> StudentList = ads.GetStudentDetailsListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (StudentList != null && StudentList.FirstOrDefault().Value != null && StudentList.FirstOrDefault().Value.Count > 0)
                {
                    long totalrecords = StudentList.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in StudentList.FirstOrDefault().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                                        items.Id.ToString(),
                            items.PreRegNum.ToString(),
                            //items.Name,
                            String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '#' onclick = 'getdata("+"\"" + items.Id + "\"" + ")' >{0}</a>",items.Name),
                            items.Campus,
                            items.AdmissionStatus,
                            }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
        }

        public JsonResult Stafflistjqgrid(string Name, string IdNumber, string Campus, string Designation, int? rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                criteria.Clear();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(Name))
                    criteria.Add("Name", Name);
                if (!string.IsNullOrEmpty(IdNumber))
                    criteria.Add("IdNumber", IdNumber);
                if (!string.IsNullOrEmpty(Campus))
                    criteria.Add("Campus", Campus);
                if (!string.IsNullOrEmpty(Designation))
                    criteria.Add("Designation", Designation);
                Dictionary<long, IList<StaffDetails>> StaffList = sms.GetStaffDetailsListWithPaging(page - 1, rows, sidx, sord, criteria);
                if (StaffList != null && StaffList.FirstOrDefault().Value != null && StaffList.FirstOrDefault().Value.Count > 0)
                {
                    long totalrecords = StaffList.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in StaffList.FirstOrDefault().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                                        items.Id.ToString(),
                                         String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '/StaffManagement/ApplicationForm?id="+items.Id+"'  >{0}</a>",items.Name),
                            //items.Name,
                            items.IdNumber,
                            items.Campus,
                            items.Designation,
                            }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
        }

        public JsonResult Issuejqgrid(string IssueNumber, string StudentName, string BranchCode, string IssueType, int? rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                ProcessFlowServices pfs = new ProcessFlowServices(); // TODO: Initialize to an appropriate value
                string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                UserService us = new UserService();
                Dictionary<long, IList<UserAppRole>> userAppRole = new Dictionary<long, IList<UserAppRole>>();
                Dictionary<string, object> criteriaUserAppRole = new Dictionary<string, object>();
                criteriaUserAppRole.Add("UserId", userid);
                criteriaUserAppRole.Add("AppCode", "ISM");
                userAppRole = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteriaUserAppRole);
                if (userAppRole != null && userAppRole.Count > 0 && userAppRole.First().Key > 0)
                {
                    int count = userAppRole.First().Value.Count;
                    //if it has values then for each concatenate APP+ROLE 
                    string[] userRoles = new string[count];
                    string[] deptCodeArr = new string[count];
                    string[] brnCodeArr = new string[count];
                    int i = 0;
                    // Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    foreach (UserAppRole uar in userAppRole.First().Value)
                    {
                        string deptCode = uar.DeptCode;
                        string branchCode = uar.BranchCode;
                        if (!string.IsNullOrWhiteSpace(uar.AppCode.Trim()) && !string.IsNullOrWhiteSpace(uar.RoleCode.Trim()))
                        {
                            userRoles[i] = uar.AppCode.Trim() + uar.RoleCode.Trim();
                        }
                        if (!string.IsNullOrEmpty(deptCode))
                        {
                            deptCodeArr[i] = deptCode;
                        }
                        if (!string.IsNullOrEmpty(branchCode))
                        {
                            brnCodeArr[i] = branchCode;
                        }
                        i++;
                    }
                    brnCodeArr = brnCodeArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    deptCodeArr = deptCodeArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    criteria.Add("BranchCode", brnCodeArr);
                    criteria.Add("Available", true);
                    string[] alias = new string[1];
                    alias[0] = "CallManagementView";
                    sidx = "CallManagementView." + sidx;
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (!string.IsNullOrEmpty(StudentName))
                        criteria.Add("CallManagementView.StudentName", StudentName);
                    if (!string.IsNullOrEmpty(IssueNumber))
                        criteria.Add("CallManagementView.IssueNumber", IssueNumber);
                    if (!string.IsNullOrEmpty(BranchCode))
                        criteria.Add("CallManagementView.BranchCode", BranchCode);
                    if (!string.IsNullOrEmpty(IssueType))
                        criteria.Add("CallManagementView.IssueType", IssueType);



                    Dictionary<long, IList<CallMgmntActivity>> ActivitiesList = pfs.GetActivityListWithsearchCriteria(page - 1, rows, sidx, sord, criteria, "AppRole", userRoles, alias);
                    if (ActivitiesList != null && ActivitiesList.Count > 0)
                    {
                        foreach (CallMgmntActivity pi in ActivitiesList.First().Value)
                        {
                            pi.ProcessInstance.DifferenceInHours = DateTime.Now - pi.ProcessInstance.DateCreated;
                        }
                        long totalRecords = ActivitiesList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        //string date = DateTime.Now.Date.ToString();
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows = (
                                 from items in ActivitiesList.First().Value
                                 select new
                                 {
                                     i = items.Id,
                                     cell = new string[] 
                                   {   
                                       items.Id.ToString(), 
                                       "<a href='/Home/CallRegister?id=" + items.CallManagementView.Id + "&activityId=" + items.Id + "&activityName=" + items.ActivityName + "&activityFullName=" + items.ActivityFullName + "'>" + items.CallManagementView.IssueNumber + "</a>",
                                       items.CallManagementView.StudentName,
                                        items.CallManagementView.BranchCode,
                                       items.CallManagementView.IssueType,
                                   }
                                 })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;//return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
        }

        public JsonResult Attendancejqgrid(string NewId, string Name, string Grade, int? rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                AttendanceService ats = new AttendanceService();
                criteria.Clear();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(NewId))
                    criteria.Add("NewId", NewId);
                if (!string.IsNullOrEmpty(Name))
                    criteria.Add("Name", Name);
                if (!string.IsNullOrEmpty(Grade))
                    criteria.Add("Grade", Grade);
                Dictionary<long, IList<StudentAttendanceView>> AttendanceList = ats.GetStudentListListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                //Dictionary<long, IList<StudentAttendanceView>> AttendanceList = ats.GetAttendanceDetailsListWithPaging(page - 1, rows, sidx, sord, criteria);
                if (AttendanceList != null && AttendanceList.FirstOrDefault().Value != null && AttendanceList.FirstOrDefault().Value.Count > 0)
                {
                    long totalrecords = AttendanceList.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in AttendanceList.FirstOrDefault().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                            items.NewId,
                            items.Name,
                            items.Grade,
                            }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
        }

        public JsonResult Assess360jqgrid(string Name, string Section, string Grade, string ConsolidatedMarks, int? rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Assess360Service AssSrvc = new Assess360Service();
                sord = sord == "desc" ? "Desc" : "Asc";
                criteria.Clear();
                if (!string.IsNullOrEmpty(Name))
                    criteria.Add("Name", Name);
                if (!string.IsNullOrEmpty(Section))
                    criteria.Add("Section", Section);
                if (!string.IsNullOrEmpty(Grade))
                    criteria.Add("Grade", Grade);
                if (!string.IsNullOrEmpty(ConsolidatedMarks))
                    criteria.Add("ConsolidatedMarks", ConsolidatedMarks);
                Dictionary<long, IList<Assess360>> AssessList = AssSrvc.GetAssess360ListWithPagingAndCriteriaWithAlias(page - 1, rows, sidx, sord, null, null, criteria, null);//AssSrvc.GetAssessDetailsListWithPaging(page - 1, rows, sord, sidx, criteria);
                if (AssessList != null && AssessList.FirstOrDefault().Value != null && AssessList.FirstOrDefault().Value.Count > 0)
                {
                    long totalrecords = AssessList.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in AssessList.FirstOrDefault().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                                        items.Id.ToString(),
                                        items.RequestNo,
                            items.Name,
                            items.Section,
                           // items.Grade,
                            items.ConsolidatedMarks,
                            }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
        }

        public JsonResult GeneralNotifyjqgrid(string Topic, string PublishTo, string NoteType, string Campus, int? rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                ParentPortalService pps = new ParentPortalService();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(Topic))
                    criteria.Add("Topic", Topic);
                if (!string.IsNullOrEmpty(PublishTo))
                    criteria.Add("PublishTo", PublishTo);
                if (!string.IsNullOrEmpty(NoteType))
                    criteria.Add("NoteType", NoteType);
                if (!string.IsNullOrEmpty(Campus))
                    criteria.Add("Campus", Campus);
                Dictionary<long, IList<Notification>> nDet = pps.GetValuesFromNotification(page - 1, rows, sidx, sord, criteria);
                if (nDet != null && nDet.FirstOrDefault().Value != null && nDet.FirstOrDefault().Value.Count > 0)
                {
                    long totalrecords = nDet.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in nDet.FirstOrDefault().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                                        items.Id.ToString(),
                            //items.Topic,
                              "<a href='/Notify/ViewNotifications?npreid=" + items.NotePreId + "' style='color:blue;'>"+items.Topic+"</a>",
                            items.PublishTo,
                            items.NoteType,
                            items.Campus,
                            }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
        }

        public JsonResult IndividualNotifyjqgrid(string PreRegNum, string Name, string Grade, string Campus, int? rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                sord = sord == "desc" ? "Desc" : "Asc";
                criteria.Clear();
                if (!string.IsNullOrEmpty(PreRegNum))
                    criteria.Add("PreRegNum", Convert.ToInt64(PreRegNum));
                if (!string.IsNullOrEmpty(Name))
                    criteria.Add("Name", Name);
                if (!string.IsNullOrEmpty(Grade))
                    criteria.Add("Grade", Grade);
                if (!string.IsNullOrEmpty(Campus))
                    criteria.Add("Campus", Campus);
                Dictionary<long, IList<StudentTemplateView>> nDet = ads.GetStudentTemplateViewListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (nDet != null && nDet.FirstOrDefault().Value != null && nDet.FirstOrDefault().Value.Count > 0)
                {
                    long totalrecords = nDet.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in nDet.FirstOrDefault().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                            items.PreRegNum.ToString(),
                            items.Name,
                            items.Grade,
                            items.Campus,
                            }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
        }

        public JsonResult MaterialInwardjqgrid(string InwardNumber, string TotalCount, string Supplier, string ReceivedBy, int? rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StoreService ss = new StoreService();
                sord = sord == "desc" ? "Desc" : "Asc";
                criteria.Clear();
                if (!string.IsNullOrEmpty(InwardNumber))
                    criteria.Add("InwardNumber", InwardNumber);
                if (!string.IsNullOrEmpty(TotalCount))
                    criteria.Add("TotalCount", Convert.ToInt64(TotalCount));
                if (!string.IsNullOrEmpty(Supplier))
                    criteria.Add("Supplier", Supplier);
                if (!string.IsNullOrEmpty(ReceivedBy))
                    criteria.Add("ReceivedBy", ReceivedBy);

                Dictionary<long, IList<MaterialInward_vw>> MaterialInwardlist = ss.GetMaterialInwardlistWithPagingAndCriteria_vw(page - 1, rows, sidx, sord, criteria);
                if (MaterialInwardlist != null && MaterialInwardlist.FirstOrDefault().Value != null && MaterialInwardlist.FirstOrDefault().Value.Count > 0)
                {
                    long totalrecords = MaterialInwardlist.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in MaterialInwardlist.FirstOrDefault().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                                        items.Id.ToString(),
                            items.InwardNumber,
                            items.TotalCount.ToString(),
                            items.Supplier,
                            items.ReceivedBy,
                            items.Status,
                            }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
        }

        public JsonResult MaterialOutwardjqgrid(string IssNoteNumber, string RequiredForCampus, string IssuedBy, string DeliveredThrough, int? rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StoreService ss = new StoreService();
                sord = sord == "desc" ? "Desc" : "Asc";
                criteria.Clear();
                if (!string.IsNullOrEmpty(IssNoteNumber))
                    criteria.Add("IssNoteNumber", IssNoteNumber);
                if (!string.IsNullOrEmpty(RequiredForCampus))
                    criteria.Add("RequiredForCampus", RequiredForCampus);
                if (!string.IsNullOrEmpty(IssuedBy))
                    criteria.Add("IssuedBy", IssuedBy);
                if (!string.IsNullOrEmpty(DeliveredThrough))
                    criteria.Add("DeliveredThrough", DeliveredThrough);
                Dictionary<long, IList<MaterialIssueNote>> MaterialIssueNotelist = ss.GetMaterialMaterialIssueNoteListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (MaterialIssueNotelist != null && MaterialIssueNotelist.FirstOrDefault().Value != null && MaterialIssueNotelist.FirstOrDefault().Value.Count > 0)
                {
                    long totalrecords = MaterialIssueNotelist.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in MaterialIssueNotelist.FirstOrDefault().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                                        items.IssNoteId.ToString(),
                            items.IssNoteNumber,
                            items.RequiredForCampus,
                            items.IssuedBy,
                            items.DeliveredThrough,
                            }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
        }

        public JsonResult TransportDetailsjqgrid(string VehicleNo, string Campus, int? rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                TransportService ts = new TransportService();
                sord = sord == "desc" ? "Desc" : "Asc";
                criteria.Clear();
                if (!string.IsNullOrEmpty(VehicleNo))
                    criteria.Add("VehicleNo", VehicleNo);
                if (!string.IsNullOrEmpty(Campus))
                    criteria.Add("Campus", Campus);
                string[] alias = new string[1];
                alias[0] = "VehicleTypeMaster";
                Dictionary<long, IList<VehicleTypeAndSubType>> VehicleSubTypeMaster = ts.GetVehicleTypeAndSubTypeListWithsearchCriteriaLikeSearch(page - 1, rows, sord, sidx, criteria, alias);//ts.GetVehicleTypeAndSubTypeListWithPaging(page - 1, rows, sidx, sord, criteria);
                if (VehicleSubTypeMaster != null && VehicleSubTypeMaster.FirstOrDefault().Value != null && VehicleSubTypeMaster.FirstOrDefault().Value.Count > 0)
                {
                    long totalrecords = VehicleSubTypeMaster.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in VehicleSubTypeMaster.FirstOrDefault().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                            
                            items.VehicleNo,
                            items.Campus,
                            }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
        }
    }
}
