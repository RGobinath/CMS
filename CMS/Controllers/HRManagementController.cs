using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TIPS.Entities;
using TIPS.Entities.HRManagementEntities;
using TIPS.Service;
using TIPS.ServiceContract;

namespace CMS.Controllers
{
    public class HRManagementController : BaseController
    {

        HRManagementService Hrms = new HRManagementService();
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Activities(long Id)
        {
            try
            {
                ViewBag.Id = Id;
                return PartialView();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
        public ActionResult LoadUserControl(string id, long? ActivityId)
        {
            try
            {
                ViewBag.Id = ActivityId;
                return PartialView(id);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }

        public ActionResult ActOnLeaveRequest(long? Id, long? activityId, string activityName, string ActivityFullName)
        {
            LeaveRequest lrequest = new LeaveRequest();
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ViewBag.title = "LeaveRequest";
                    UserService us = new UserService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("UserId", userId);
                    criteria.Add("AppCode", "HRM");
                    Dictionary<long, IList<UserAppRole>> UserAppRoleList = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    var UserDetails = (from u in UserAppRoleList.First().Value
                                       select new { u.RoleName }).ToArray();


                    if ((Id != null) && (Id > 0))
                    {
                        string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                        ProcessFlowServices pfs = new ProcessFlowServices();
                        pfs.AssignActivity((Convert.ToInt64(activityId)), userid);

                        LeaveRequest getlrid = Hrms.GetLeaveRequestById(Convert.ToInt64(Id));
                        getlrid.Status = !string.IsNullOrWhiteSpace(activityName) ? activityName : getlrid.Status;
                        getlrid.ActivityFullName = !string.IsNullOrWhiteSpace(ActivityFullName) ? ActivityFullName : getlrid.ActivityFullName;
                        ViewBag.RequestNum = getlrid.RequestNo;
                        ViewBag.Campus = getlrid.BranchCode;
                        string x = getlrid.DateApplyingForLeave;
                        string[] Leave = x.Split(new char[] { '-' });
                        getlrid.DateFrom = Leave[0];
                        getlrid.DateTo = Leave[1];
                        ViewBag.Details = 5;


                        string ReportMng = null;
                        StaffDetailsHRM Staff = Hrms.GetStaffById(getlrid.CreatedBy);
                        string ReportMngname = Staff.ReportingManager;
                        if (!string.IsNullOrEmpty(ReportMngname))
                        {
                            StaffDetailsHRM Staffusername = Hrms.GetStaffusername(ReportMngname);
                            ReportMng = Staffusername.StaffUserName;
                        }
                        else
                        {
                            ReportMng = Staff.ReportingManager;
                        }
                        Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                        criteria1.Add("UserId", ReportMng);
                        criteria1.Add("AppCode", "HRM");
                        Dictionary<long, IList<UserAppRole>> UserAppRoleList2 = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 100, "", "", criteria1);
                        var ListCount = (from u in UserAppRoleList2.First().Value
                                         select new { u.AppCode, u.RoleCode }).ToArray();
                        string CheckAppRole = ListCount[0].AppCode + ListCount[0].RoleCode;
                        if (CheckAppRole == "HRMHEAD") { ViewBag.HRApprole = "8"; }

                        return View(getlrid);
                    }
                    else
                    {
                        StaffDetailsHRM Staff = Hrms.GetStaffById(userId);
                        lrequest.ProcessedBy = userId;
                        lrequest.CreateDate = DateTime.Now;
                        lrequest.UserRole = UserDetails[0].RoleName;
                        lrequest.Status = "Opened";
                        lrequest.ActivityFullName = "Opened";
                        lrequest.StaffName = Staff.Name;
                        lrequest.StaffIdNumber = Staff.IdNumber;
                        lrequest.BranchCode = Staff.Campus.ToUpper();
                        lrequest.ReportingManager = Staff.ReportingManager;
                        ViewBag.Reply = 1;
                        return View(lrequest);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }

        public ActionResult ShowLeaveRequestDetails(long? Id)
        {
            try
            {
                LeaveRequest getlrid = Hrms.GetLeaveRequestById(Convert.ToInt64(Id));
                string DateDetails = getlrid.DateApplyingForLeave;
                string[] Leave = DateDetails.Split(new char[] { '-' });
                getlrid.DateFrom = Leave[0];
                getlrid.DateTo = Leave[1];
                return View(getlrid);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }

        }
        public ActionResult LeaveRequestGrid(int? rows, string sidx = "Id", string sord = "Desc", int? page = 1)
        {
            try
            {
                TIPS.Service.UserService us = new UserService();
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {

                    StaffDetailsHRM Staff = Hrms.GetStaffById(userId);
                    if (Staff == null) { ViewBag.Name = "6"; }
                    else
                    {
                        string ReportMng = Staff.ReportingManager;

                        Dictionary<string, object> criteria2 = new Dictionary<string, object>();
                        criteria2.Add("UserId", userId);
                        criteria2.Add("AppCode", "HRM");
                        Dictionary<long, IList<UserAppRole>> UserAppRoleList2 = us.GetAppRoleForAnUserListWithPagingAndCriteriaWithLikeSearch(0, 9999, string.Empty, string.Empty, criteria2);
                        var ListCount2 = (from u in UserAppRoleList2.First().Value
                                          select new { u.AppCode, u.RoleCode }).ToArray();
                        string CheckAppRole = ListCount2[0].AppCode + ListCount2[0].RoleCode;
                        if ((ReportMng == null) && (CheckAppRole == "HRMSTAFF"))
                        {
                            ViewBag.flag = "1";
                        }
                        Dictionary<string, object> criteria = new Dictionary<string, object>();
                        criteria.Add("UserId", userId);
                        criteria.Add("AppCode", "HRM");
                        criteria.Add("RoleCode", "HEAD");
                        Dictionary<long, IList<UserAppRole>> UserAppRoleList = us.GetAppRoleForAnUserListWithPagingAndCriteriaWithLikeSearch(0, 9999, string.Empty, string.Empty, criteria);
                        var ListCount = (from u in UserAppRoleList.First().Value
                                         select new { u.AppCode, u.RoleCode }).ToList();

                        if (ListCount.Count >= 1)
                        {
                            ViewBag.count = "1";
                            ViewBag.HrHead = "4";
                        }
                        else
                        {
                            ViewBag.count = "0";
                        }

                        Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                        criteria1.Add("UserId", userId);
                        criteria1.Add("AppCode", "HRM");
                        criteria1.Add("RoleCode", "RM");
                        Dictionary<long, IList<UserAppRole>> UserAppRoleList1 = us.GetAppRoleForAnUserListWithPagingAndCriteriaWithLikeSearch(0, 9999, string.Empty, string.Empty, criteria1);
                        var ListCount1 = (from u in UserAppRoleList1.First().Value
                                          select new { u.AppCode, u.RoleCode }).ToList();
                        if (ListCount1.Count >= 1)
                        {
                            ViewBag.count1 = "1";
                        }
                        else
                        {
                            ViewBag.count1 = "0";
                        }
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
        public ActionResult LeaveRequestJqGrid(long? Id, string ddlSearchBy, string txtSearch, string fromDate, string status, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ProcessFlowServices pfs = new ProcessFlowServices(); // TODO: Initialize to an appropriate value
                    //pass userid and get the list of roles from user service GetAppRoleForAnUserListWithPagingAndCriteria
                    string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();

                    UserService us = new UserService();
                    Dictionary<long, IList<UserAppRole>> userAppRole = new Dictionary<long, IList<UserAppRole>>();
                    Dictionary<string, object> criteriaUserAppRole = new Dictionary<string, object>();
                    criteriaUserAppRole.Add("UserId", userid);
                    //criteria.Add("ProcessedBy", userid);
                    criteriaUserAppRole.Add("AppCode", "HRM");
                    userAppRole = us.GetAppRoleForAnUserListWithPagingAndCriteriaWithLikeSearch(0, 1000, string.Empty, string.Empty, criteriaUserAppRole);
                    if (userAppRole != null && userAppRole.Count > 0 && userAppRole.First().Key > 0)
                    {
                        int count = userAppRole.First().Value.Count;
                        //if it has values then for each concatenate APP+ROLE 
                        string[] userRoles = new string[count];
                        string[] deptCodeArr = new string[count];
                        string[] brnCodeArr = new string[count];
                        string[] rolecodeArr = new string[count];


                        int i = 0;
                        Dictionary<string, object> criteria = new Dictionary<string, object>();
                        foreach (UserAppRole uar in userAppRole.First().Value)
                        {
                            string deptCode = uar.DeptCode;
                            string branchCode = uar.BranchCode;
                            string roleCode = uar.RoleCode;


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
                            if (!string.IsNullOrEmpty(roleCode))
                            {
                                rolecodeArr[i] = roleCode;
                            }

                            i++;
                        }
                        brnCodeArr = brnCodeArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                        deptCodeArr = deptCodeArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                        criteria.Add("BranchCode", brnCodeArr);
                        //if (status != "Assigned")
                        //    criteria.Add("DeptCode", deptCodeArr);
                        if (!string.IsNullOrEmpty(txtSearch))
                        {
                            txtSearch.Trim();
                            if (ddlSearchBy == "CreateDate")
                            {
                                var issuedate = Convert.ToDateTime(txtSearch);
                                criteria.Add("LeaveRequest." + ddlSearchBy.Trim(), issuedate);
                            }
                            else
                            {
                                criteria.Add("LeaveRequest." + ddlSearchBy.Trim(), txtSearch);
                            }
                        }
                        if ((!string.IsNullOrEmpty(fromDate) && !(string.IsNullOrEmpty(fromDate.Trim()))))
                        {
                            fromDate = fromDate.Trim();
                            string To = string.Format("{0:MM/dd/yyyy}", DateTime.Now);
                            DateTime ToDate = Convert.ToDateTime(To + " " + "23:59:59");
                            DateTime fdate = DateTime.Now;
                            DateTime tdate = DateTime.Now;
                            DateTime[] fromto = new DateTime[2];
                            if (!string.IsNullOrEmpty(fromDate))
                            {
                                var s = fromDate.Split('/');
                                var dd = s[0];
                                var mm = s[1];
                                var yy = s[2];
                                var resultField = mm + "/" + dd + "/" + yy;

                                fdate = Convert.ToDateTime(resultField);
                                fromto[0] = fdate;
                            }
                            if (ToDate != null)
                            {
                                fromto[1] = ToDate;
                            }
                            criteria.Add("LeaveRequest." + "CreateDate", fromto);
                        }
                        if (!string.IsNullOrEmpty(status))
                        {
                            if (status == "Available")
                            {
                                criteria.Add("Available", true);
                            }
                            else if (status == "Assigned")
                            {
                                criteria.Add("Assigned", true);
                                criteria.Add("Performer", userid);
                            }
                            else if (status == "Sent")
                            {
                                criteria.Add("Completed", true);
                            }
                            else if (status == "Completed")
                            {
                                criteria.Add("Completed", true);
                                criteria.Add("ActivityName", "Complete");
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(sord) && sord == "desc")
                            sord = "Desc";
                        else
                            sord = "Asc";
                        string[] alias = new string[1];
                        alias[0] = "LeaveRequest";
                        criteria.Add("TemplateId", (long)6);
                        Dictionary<long, IList<HRMgmntActivity>> LeaveList = null;
                        if (rolecodeArr[0] == "STAFF")
                        {

                            criteria.Add("LeaveRequest." + "ProcessedBy", userid);
                            LeaveList = pfs.GetHRActivityListWithsearchCriteria(page - 1, rows, sidx, sord, criteria, "AppRole", userRoles, alias);
                        }
                        else
                        {
                            LeaveList = pfs.GetHRActivityListWithsearchCriteria(page - 1, rows, sidx, sord, criteria, "AppRole", userRoles, alias);
                        }

                        if (LeaveList != null && LeaveList.Count > 0)
                        {
                            foreach (HRMgmntActivity pi in LeaveList.First().Value)
                            {
                                pi.LeaveRequest.DifferenceInHours = DateTime.Now - pi.LeaveRequest.CreateDate;
                            }
                            long totalrecords = LeaveList.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            if (status == "Sent" || status == "Completed")
                            {
                                var jsondat = new
                                {
                                    total = totalPages,
                                    page = page,
                                    records = totalrecords,
                                    rows = (from items in LeaveList.First().Value
                                            select new
                                            {
                                                i = 2,
                                                cell = new string[] {
                                        items.LeaveRequest.Id.ToString(),
                                         "<a href='/HRManagement/ShowLeaveRequestDetails?id=" + items.LeaveRequest.Id + "&activityId=" + items.Id + "&activityName=" + items.ActivityName + "&status=" +items.LeaveRequest.Status +"&activityFullName=" + items.LeaveRequest.ActivityFullName + "'>"+ items.LeaveRequest.RequestNo +"</a>",
                                       items.LeaveRequest.CreateDate.ToString(),
                                        items.LeaveRequest.ActivityFullName,
                                       
                                         items.LeaveRequest.StaffName,
                                        items.LeaveRequest.StaffIdNumber,
                                        items.LeaveRequest.BranchCode,
                                        items.LeaveRequest.TypeOfLeave,
                                        items.LeaveRequest.DateApplyingForLeave,
                                        "<img src='/Images/History.png ' id='ImgHistory' onclick=\"ShowComments('" + items.ProcessRefId + "');\" />",
                                        items.LeaveRequest.Status=="Completed"?"Completed":items.LeaveRequest.DifferenceInHours.Value.TotalHours.ToString(),

                            }
                                            })
                                };
                                return Json(jsondat, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                var jsondat = new
                                {
                                    total = totalPages,
                                    page = page,
                                    records = totalrecords,
                                    rows = (from items in LeaveList.First().Value
                                            select new
                                            {
                                                i = 2,
                                                cell = new string[] {
                                        items.Id.ToString(),
                                         "<a href='/HRManagement/ActOnLeaveRequest?Id="+ items.LeaveRequest.Id + "&activityId=" + items.Id + "&activityName=" + items.ActivityName + "&status=" +items.LeaveRequest.Status +"&activityFullName=" + items.LeaveRequest.ActivityFullName + "'>"+ items.LeaveRequest.RequestNo+"</a>",
                                       items.LeaveRequest.CreateDate.ToString(),
                                        items.LeaveRequest.ActivityFullName,
                                        items.LeaveRequest.StaffName,
                                        items.LeaveRequest.StaffIdNumber,
                                        items.LeaveRequest.BranchCode,
                                        items.LeaveRequest.TypeOfLeave,
                                        items.LeaveRequest.DateApplyingForLeave,
                                         "<img src='/Images/History.png' id='ImgHistory' onclick=\"ShowComments('" + items.ProcessRefId + "');\" />",
                                         items.LeaveRequest.Status=="Completed"?"Completed":items.LeaveRequest.DifferenceInHours.Value.TotalHours.ToString(),

                            }
                                            })
                                };
                                return Json(jsondat, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
        public ActionResult LeaveRequest(long? Id, long? activityId, string activityName, string ActivityFullName)
        {
            LeaveRequest lrequest = new LeaveRequest();
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ViewBag.title = "LeaveRequest";
                    UserService us = new UserService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("UserId", userId);
                    criteria.Add("AppCode", "HRM");
                    Dictionary<long, IList<UserAppRole>> UserAppRoleList = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    var UserDetails = (from u in UserAppRoleList.First().Value
                                       select new { u.RoleName }).ToArray();


                    if ((Id != null) && (Id > 0))
                    {
                        string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                        ProcessFlowServices pfs = new ProcessFlowServices();
                        pfs.AssignActivity((Convert.ToInt64(activityId)), userid);

                        LeaveRequest getlrid = Hrms.GetLeaveRequestById(Convert.ToInt64(Id));
                        getlrid.Status = !string.IsNullOrWhiteSpace(activityName) ? activityName : getlrid.Status;
                        getlrid.ActivityFullName = !string.IsNullOrWhiteSpace(ActivityFullName) ? ActivityFullName : getlrid.ActivityFullName;
                        ViewBag.RequestNum = getlrid.RequestNo;
                        ViewBag.Campus = getlrid.BranchCode;
                        string x = getlrid.DateApplyingForLeave;
                        string[] Leave = x.Split(new char[] { '-' });
                        getlrid.DateFrom = Leave[0];
                        getlrid.DateTo = Leave[1];
                        ViewBag.Details = 5;

                        StaffDetailsHRM Staff = Hrms.GetStaffById(getlrid.CreatedBy);
                        string ReportMng = Staff.ReportingManager;
                        Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                        criteria1.Add("UserId", ReportMng);
                        criteria1.Add("AppCode", "HRM");
                        // Dictionary<long, IList<UserAppRole>> UserAppRoleList2 = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 100, "", "", criteria1);
                        Dictionary<long, IList<UserAppRole>> UserAppRoleList2 = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 100, string.Empty, string.Empty, criteria1);
                        var ListCount = (from u in UserAppRoleList2.First().Value
                                         select new { u.AppCode, u.RoleCode }).ToArray();
                        string CheckAppRole = ListCount[0].AppCode + ListCount[0].RoleCode;
                        if (CheckAppRole == "HRMHEAD") { ViewBag.HRApprole = "8"; }

                        return View(getlrid);
                    }
                    else
                    {
                        StaffDetailsHRM Staff = Hrms.GetStaffById(userId);
                        lrequest.ProcessedBy = userId;
                        lrequest.CreateDate = DateTime.Now;
                        lrequest.UserRole = UserDetails[0].RoleName;
                        lrequest.Status = "Opened";
                        lrequest.ActivityFullName = "Opened";
                        lrequest.StaffName = Staff.Name;
                        lrequest.StaffIdNumber = Staff.IdNumber;
                        lrequest.BranchCode = Staff.Campus.ToUpper();
                        lrequest.ReportingManager = Staff.ReportingManager;
                        ViewBag.Reply = 1;
                        return View(lrequest);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult LeaveRequest(LeaveRequest lrequest)
        {
            ProcessFlowServices pfs = new ProcessFlowServices();
            HRManagementService Hrms = new HRManagementService();

            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    string ReportMng = null;
                    StaffDetailsHRM Staff = Hrms.GetStaffById(userId);
                    string ReportMngname = Staff.ReportingManager;
                    if (!string.IsNullOrEmpty(ReportMngname))
                    {
                        StaffDetailsHRM Staffusername = Hrms.GetStaffusername(ReportMngname);
                        ReportMng = Staffusername.StaffUserName;
                    }
                    else
                    {
                        ReportMng = Staff.ReportingManager;
                    }


                    UserService us = new UserService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("UserId", ReportMng);
                    criteria.Add("AppCode", "HRM");
                    Dictionary<long, IList<UserAppRole>> UserAppRoleList2 = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 100, "", "", criteria);
                    var ListCount = (from u in UserAppRoleList2.First().Value
                                     select new { u.AppCode, u.RoleCode }).ToArray();
                    string CheckAppRole = ListCount[0].AppCode + ListCount[0].RoleCode;

                    Dictionary<string, object> Usercriteria = new Dictionary<string, object>();
                    Usercriteria.Add("UserId", userId);
                    Usercriteria.Add("AppCode", "HRM");
                    Dictionary<long, IList<UserAppRole>> UserAppRoleListUserId = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 100, "", "", Usercriteria);
                    var ListCountUserId = (from u in UserAppRoleListUserId.First().Value
                                           select new { u.AppCode, u.RoleCode }).ToArray();
                    string UserIdDetails = ListCountUserId[0].AppCode + ListCountUserId[0].RoleCode;

                    string RejComments = Request["txtRejectionDetails"];

                    long id = 0;

                    if (Request.Form["btnSentApply"] == "Apply")
                    {
                        if (lrequest.Id == 0)
                        {
                            lrequest.CreatedBy = userId;
                            id = pfs.StartHRManagement(lrequest, "HRManagement", userId);
                            lrequest.RequestNo = "LRF-" + id;
                            lrequest.DateApplyingForLeave = lrequest.DateFrom + "-" + lrequest.DateTo;

                            Hrms.CreateOrUpdateLeaveRequest(lrequest);
                            lrequest.Performer = userId;

                            pfs.CompleteActivityHRManagement(lrequest, "HRManagement", userId, ReportMng, CheckAppRole, UserIdDetails, "CreateLeaveRequest", false);
                            TempData["variableName"] = lrequest.RequestNo;
                            return RedirectToAction("LeaveRequestGrid", "HRManagement");
                        }
                    }
                    else if (Request.Form["btnResolveLeave"] == "Accept Leave")
                    {
                        //write the complete activity logic
                        lrequest.Performer = userId;
                        lrequest.DateApplyingForLeave = lrequest.DateFrom + "-" + lrequest.DateTo;
                        // pfs.CompleteActivityHRManagement(lrequest, "HRManagement", userId, lrequest.Status, false);
                        pfs.CompleteActivityHRManagement(lrequest, "HRManagement", userId, ReportMng, CheckAppRole, UserIdDetails, lrequest.Status, false);
                        return RedirectToAction("LeaveRequestGrid", "HRManagement");
                    }
                    else if (Request.Form["btnLeaderRejection"] == "Reject")
                    {
                        CommentsService cmntsSrvc = new CommentsService();
                        Comments cmntsObj = new Comments();
                        cmntsObj.EntityRefId = lrequest.Id;
                        cmntsObj.CommentedBy = userId;
                        cmntsObj.CommentedOn = DateTime.Now;
                        cmntsObj.RejectionComments = Request["txtRejectionDetails"];
                        cmntsObj.AppName = "L-HRM";
                        cmntsSrvc.CreateOrUpdateComments(cmntsObj);
                        lrequest.Performer = userId;
                        //write the complete activity logic
                        lrequest.DateApplyingForLeave = lrequest.DateFrom + "-" + lrequest.DateTo;
                        //  pfs.CompleteActivityHRManagement(lrequest, "HRManagement", userId, lrequest.Status, true);
                        pfs.CompleteActivityHRManagement(lrequest, "HRManagement", userId, ReportMng, CheckAppRole, UserIdDetails, lrequest.Status, true);
                        return RedirectToAction("LeaveRequestGrid", "HRManagement");
                    }
                    else if (Request.Form["btnApproval"] == "Approve")
                    {
                        lrequest.Performer = userId;
                        //write the complete activity logic
                        lrequest.DateApplyingForLeave = lrequest.DateFrom + "-" + lrequest.DateTo;
                        //pfs.CompleteActivityHRManagement(lrequest, "HRManagement", userId, "ApproveLeave", false);
                        pfs.CompleteActivityHRManagement(lrequest, "HRManagement", userId, ReportMng, CheckAppRole, UserIdDetails, "ApproveLeaveRequest", false);
                        //redirect to list page
                        return RedirectToAction("LeaveRequestGrid", "HRManagement");
                    }
                    else if (Request.Form["StaffReply"] == "Reply")
                    {
                        CommentsService cmntsSrvc1 = new CommentsService();
                        Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                        criteria1.Add("EntityRefId", lrequest.Id);
                        Dictionary<long, IList<Comments>> list = cmntsSrvc1.GetCommentsListWithPaging(0, 1000, string.Empty, string.Empty, criteria1);
                        if (list != null && list.Count > 0)
                        {
                            foreach (Comments cm in list.First().Value)
                            {
                                if (string.IsNullOrWhiteSpace(cm.ResolutionComments))
                                {
                                    cm.ResolutionComments = Request["txtReplyDescription"];
                                    cm.AppName = "L-HRM";
                                    cmntsSrvc1.CreateOrUpdateComments(cm);
                                    break;
                                }
                            }
                        }
                        lrequest.Performer = userId;
                        lrequest.DateApplyingForLeave = lrequest.DateFrom + "-" + lrequest.DateTo;
                        //  pfs.CompleteActivityHRManagement(lrequest, "HRManagement", userId, lrequest.Status, false);
                        pfs.CompleteActivityHRManagement(lrequest, "HRManagement", userId, ReportMng, CheckAppRole, UserIdDetails, lrequest.Status, false);
                        return RedirectToAction("LeaveRequestGrid", "HRManagement");
                    }
                    else if (Request.Form["nameComplete"] == "Complete")
                    {
                        lrequest.Performer = userId;
                        lrequest.DateApplyingForLeave = lrequest.DateFrom + "-" + lrequest.DateTo;
                        pfs.CompleteActivityHRManagement(lrequest, "HRManagement", userId, ReportMng, CheckAppRole, UserIdDetails, "Complete", false);
                        return RedirectToAction("LeaveRequestGrid", "HRManagement");
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
        public ActionResult LeaveRejectionGrid(long? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    CommentsService cs = new CommentsService(); // TODO: Initialize to an appropriate value
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    {
                        criteria.Add("EntityRefId", Id);
                        criteria.Add("AppName", "L-HRM");
                    }
                    Dictionary<long, IList<Comments>> CommentsList = cs.GetCommentsListWithPaging(page - 1, rows, sidx, sord, criteria);
                    if (CommentsList != null && CommentsList.Count > 0)
                    {
                        long totalRecords = CommentsList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows = (
                                 from items in CommentsList.First().Value
                                 select new
                                 {
                                     i = items.CommentId,
                                     cell = new string[] { items.CommentedBy, items.CommentedOn.ToString(), items.RejectionComments, items.ResolutionComments }
                                 })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        TempData["NoComments"] = 0;
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
        public ActionResult ActivitiesListJqGrid(long? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ProcessFlowServices pfs = new ProcessFlowServices(); // TODO: Initialize to an appropriate value
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    {
                        criteria.Add("ProcessRefId", Id);
                        criteria.Add("TemplateId", (long)6);
                    }
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<HRMgmntActivity>> ActivitiesList = pfs.GetHRActivityListWithsearchCriteria(page - 1, rows, sidx, sord, criteria, string.Empty, null, null);

                    if (ActivitiesList != null && ActivitiesList.Count > 0)
                    {
                        long totalRecords = ActivitiesList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
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
                                     cell = new string[] { items.Id.ToString(), items.ActivityFullName, items.Available ? "Available" : items.Assigned ? "Assigned" : "Completed", items.Performer != null ? items.Performer.ToString() : "", items.CreatedDate.ToString(), items.AppRole, items.LeaveRequest.Id.ToString() }
                                 })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }

        // BankAccount Details...
        public ActionResult ShowBankAccountDetails(long? Id)
        {
            try
            {
                BankAccount baccount = Hrms.GetBankAccountById(Convert.ToInt64(Id));
                return View(baccount);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
        public ActionResult BankAccountGrid(int? rows, string sidx = "Id", string sord = "Desc", int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TIPS.Service.UserService us = new UserService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("UserId", userId);
                    criteria.Add("AppCode", "HRM");
                    criteria.Add("RoleCode", "HEAD");
                    Dictionary<long, IList<UserAppRole>> UserAppRoleList = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 100, sidx, sord, criteria);
                    var ListCount = (from u in UserAppRoleList.First().Value
                                     where u.UserId == userId
                                     select u).ToList();
                    if (ListCount.Count >= 1)
                    {
                        ViewBag.count = "1";
                    }
                    else
                    {
                        ViewBag.count = "0";
                    }

                    Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                    criteria1.Add("UserId", userId);
                    criteria1.Add("AppCode", "HRM");
                    criteria1.Add("RoleCode", "RM");
                    Dictionary<long, IList<UserAppRole>> UserAppRoleList1 = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 100, sidx, sord, criteria1);
                    var ListCount1 = (from u in UserAppRoleList1.First().Value
                                      where u.UserId == userId
                                      select u).ToList();
                    if (ListCount1.Count >= 1)
                    {
                        ViewBag.count1 = "1";
                    }
                    else
                    {
                        ViewBag.count1 = "0";
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
        public ActionResult BankAccountJqGrid(long? Id, string ddlSearchBy, string txtSearch, string fromDate, string status, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ProcessFlowServices pfs = new ProcessFlowServices(); // TODO: Initialize to an appropriate value
                    //pass userid and get the list of roles from user service GetAppRoleForAnUserListWithPagingAndCriteria
                    string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();

                    UserService us = new UserService();
                    Dictionary<long, IList<UserAppRole>> userAppRole = new Dictionary<long, IList<UserAppRole>>();
                    Dictionary<string, object> criteriaUserAppRole = new Dictionary<string, object>();
                    criteriaUserAppRole.Add("UserId", userid);
                    //criteria.Add("ProcessedBy", userid);
                    userAppRole = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteriaUserAppRole);
                    if (userAppRole != null && userAppRole.Count > 0 && userAppRole.First().Key > 0)
                    {
                        int count = userAppRole.First().Value.Count;
                        //if it has values then for each concatenate APP+ROLE 
                        string[] userRoles = new string[count];
                        string[] deptCodeArr = new string[count];
                        string[] brnCodeArr = new string[count];
                        string[] Rolename = new string[count];
                        int i = 0;
                        Dictionary<string, object> criteria = new Dictionary<string, object>();
                        foreach (UserAppRole uar in userAppRole.First().Value)
                        {
                            string deptCode = uar.DeptCode;
                            string branchCode = uar.BranchCode;
                            string rolename = uar.RoleName;

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
                            if (!string.IsNullOrEmpty(rolename))
                            {
                                Rolename[i] = rolename;
                            }
                            i++;
                        }
                        brnCodeArr = brnCodeArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                        deptCodeArr = deptCodeArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                        criteria.Add("BranchCode", brnCodeArr);
                        if (status != "Assigned")
                            criteria.Add("DeptCode", deptCodeArr);
                        if (!string.IsNullOrEmpty(txtSearch))
                        {
                            txtSearch.Trim();
                            if (ddlSearchBy == "CreateDate")
                            {
                                var issuedate = Convert.ToDateTime(txtSearch);
                                criteria.Add("BankAccount." + ddlSearchBy.Trim(), issuedate);
                            }
                            else
                            {
                                criteria.Add("BankAccount." + ddlSearchBy.Trim(), txtSearch);
                            }
                        }
                        if ((!string.IsNullOrEmpty(fromDate) && !(string.IsNullOrEmpty(fromDate.Trim()))))
                        {
                            fromDate = fromDate.Trim();
                            string To = string.Format("{0:MM/dd/yyyy}", DateTime.Now);
                            DateTime ToDate = Convert.ToDateTime(To + " " + "23:59:59");
                            DateTime fdate = DateTime.Now;
                            DateTime tdate = DateTime.Now;
                            DateTime[] fromto = new DateTime[2];
                            if (!string.IsNullOrEmpty(fromDate))
                            {
                                var s = fromDate.Split('/');
                                var dd = s[0];
                                var mm = s[1];
                                var yy = s[2];
                                var resultField = mm + "/" + dd + "/" + yy;

                                fdate = Convert.ToDateTime(resultField);
                                fromto[0] = fdate;
                            }
                            if (ToDate != null)
                            {
                                fromto[1] = ToDate;
                            }
                            criteria.Add("BankAccount." + "CreateDate", fromto);
                        }
                        if (!string.IsNullOrEmpty(status))
                        {
                            if (status == "Available")
                            {
                                criteria.Add("Available", true);
                            }
                            else if (status == "Assigned")
                            {
                                criteria.Add("Assigned", true);
                                criteria.Add("Performer", userid);
                            }
                            else if (status == "Sent")
                            {
                                criteria.Add("Completed", true);
                            }
                            else if (status == "Completed")
                            {
                                criteria.Add("Completed", true);
                                criteria.Add("ActivityName", "Complete");
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(sord) && sord == "desc")
                            sord = "Desc";
                        else
                            sord = "Asc";
                        string[] alias = new string[1];
                        alias[0] = "BankAccount";
                        criteria.Add("TemplateId", (long)7);
                        Dictionary<long, IList<BankMgmntActivity>> AccountList = null;

                        if (Rolename[0] == "Staff")
                        {
                            criteria.Add("LeaveRequest." + "StaffName", userid);
                            AccountList = pfs.GetBankHRActivityListWithsearchCriteria(page - 1, rows, sidx, sord, criteria, "AppRole", userRoles, alias);
                        }
                        else
                        {
                            AccountList = pfs.GetBankHRActivityListWithsearchCriteria(page - 1, rows, sidx, sord, criteria, "AppRole", userRoles, alias);
                        }

                        if (AccountList != null && AccountList.Count > 0)
                        {
                            foreach (BankMgmntActivity pi in AccountList.First().Value)
                            {
                                pi.BankAccount.DifferenceInHours = DateTime.Now - pi.BankAccount.CreateDate;
                            }
                            long totalrecords = AccountList.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            if (status == "Sent" || status == "Completed")
                            {
                                var jsondat = new
                                {
                                    total = totalPages,
                                    page = page,
                                    records = totalrecords,
                                    rows = (from items in AccountList.First().Value
                                            select new
                                            {
                                                i = 2,
                                                cell = new string[] {
                                      items.BankAccount.Id.ToString(),
                                       "<a href='/HRManagement/ShowBankAccountDetails?id=" + items.BankAccount.Id + "&activityId=" + items.Id + "&activityName=" + items.ActivityName + "&status=" +items.BankAccount.Status +"&activityFullName=" + items.BankAccount.ActivityFullName + "'>"+ items.BankAccount.RequestNo +"</a>",
                                       items.BankAccount.CreateDate.ToString(),
                                       items.BankAccount.ActivityFullName,
                                       items.BankAccount.StaffName,
                                       items.BankAccount.StaffIdNumber,
                                       items.BankAccount.BranchCode,
                                       items.BankAccount.Department,
                                        "<img src='/Images/History.png ' id='ImgHistory' onclick=\"ShowComments('" + items.ProcessRefId + "');\" />",
                                        items.BankAccount.Status=="Completed"?"Completed":items.BankAccount.DifferenceInHours.Value.TotalHours.ToString(),

                            }
                                            })
                                };
                                return Json(jsondat, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                var jsondat = new
                                {
                                    total = totalPages,
                                    page = page,
                                    records = totalrecords,
                                    rows = (from items in AccountList.First().Value
                                            select new
                                            {
                                                i = 2,
                                                cell = new string[] {


                                      items.Id.ToString(),
                                       "<a href='/HRManagement/BankAccount?id=" + items.BankAccount.Id + "&activityId=" + items.Id + "&activityName=" + items.ActivityName + "&status=" +items.BankAccount.Status +"&activityFullName=" + items.BankAccount.ActivityFullName + "'>"+ items.BankAccount.RequestNo +"</a>",
                                       items.BankAccount.CreateDate.ToString(),
                                       items.BankAccount.ActivityFullName, 
                                       items.BankAccount.StaffName,
                                       items.BankAccount.StaffIdNumber,
                                       items.BankAccount.BranchCode,
                                       items.BankAccount.Department,
                                       "<img src='/Images/History.png' id='ImgHistory' onclick=\"ShowComments('" + items.ProcessRefId + "');\" />",
                                       items.BankAccount.Status=="Completed"?"Completed":items.BankAccount.DifferenceInHours.Value.TotalHours.ToString(),
                            }
                                            })
                                };
                                return Json(jsondat, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
        public ActionResult BankAccount(long? Id, long? activityId, string activityName, string ActivityFullName)
        {

            BankAccount bank = new BankAccount();
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    UserService us = new UserService();
                    // string userId = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("UserId", userId);
                    Dictionary<long, IList<UserAppRole>> UserAppRoleList = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    var UserDetails = (from u in UserAppRoleList.First().Value
                                       where u.UserId == userId
                                       select new { u.RoleName, u.BranchCode }).ToArray();

                    if ((Id > 0) && (Id != null))
                    {
                        string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                        ProcessFlowServices pfs = new ProcessFlowServices();
                        pfs.AssignActivity((Convert.ToInt64(activityId)), userid);

                        BankAccount baccount = Hrms.GetBankAccountById(Convert.ToInt64(Id));
                        ViewBag.BankDetails = 5;
                        return View(baccount);

                    }
                    else
                    {
                        StaffDetailsHRM Staff = Hrms.GetStaffById(userId);
                        bank.ProcessedBy = userId;
                        bank.CreateDate = DateTime.Now;
                        bank.UserRole = UserDetails[0].RoleName;
                        bank.Status = "Opened";
                        bank.StaffName = Staff.Name;
                        bank.StaffIdNumber = Staff.IdNumber;
                        bank.Department = Staff.Department;
                        bank.BranchCode = Staff.Campus.ToUpper();
                        bank.DateOfBirth = "10/10/1989";
                        bank.DateOfJoining = Staff.DateOfJoin;
                        ViewBag.BankReply = 1;
                        return View(bank);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult BankAccount(BankAccount account)
        {
            ProcessFlowServices pfs = new ProcessFlowServices();
            HRManagementService Hrms = new HRManagementService();
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (account != null && !string.IsNullOrWhiteSpace(account.Department))
                    {
                        account.DeptCode = account.Department.ToUpper();
                    }


                    string RejComments = Request["txtRejectionDetails"];

                    long id = 0;

                    if (Request.Form["btnSentApply"] == "Apply")
                    {
                        if (account.Id == 0)
                        {
                            id = pfs.StartBankHRManagement(account, "BankAccount", userId);
                            account.RequestNo = "AOF-" + id;
                            Hrms.CreateOrUpdateBankAccount(account);
                            account.Performer = userId;
                            pfs.CompleteBankActivityHRManagement(account, "BankAccount", userId, "CreateAccount", false);
                            TempData["BankAccountId"] = account.RequestNo;
                            return RedirectToAction("BankAccountGrid", "HRManagement");
                        }
                    }
                    else if (Request.Form["btnResolveAccount"] == "Resolve Account")
                    {
                        //write the complete activity logic
                        account.Performer = userId;
                        pfs.CompleteBankActivityHRManagement(account, "BankAccount", userId, account.Status, false);
                        return RedirectToAction("BankAccountGrid", "HRManagement");
                    }
                    else if (Request.Form["btnLeaderRejection"] == "Reject")
                    {
                        CommentsService cmntsSrvc = new CommentsService();
                        Comments cmntsObj = new Comments();
                        cmntsObj.EntityRefId = account.Id;
                        cmntsObj.CommentedBy = userId;
                        cmntsObj.CommentedOn = DateTime.Now;
                        cmntsObj.RejectionComments = Request["txtRejectionDetails"];
                        cmntsObj.AppName = "B-HRM";
                        cmntsSrvc.CreateOrUpdateComments(cmntsObj);
                        account.Performer = userId;
                        //write the complete activity logic
                        pfs.CompleteBankActivityHRManagement(account, "BankAccount", userId, account.Status, true);
                        return RedirectToAction("BankAccountGrid", "HRManagement");
                    }
                    else if (Request.Form["btnApproval"] == "Approve")
                    {
                        account.Performer = userId;
                        //write the complete activity logic
                        pfs.CompleteBankActivityHRManagement(account, "BankAccount", userId, "ApproveAccount", false);
                        //redirect to list page
                        return RedirectToAction("BankAccountGrid", "HRManagement");
                    }
                    else if (Request.Form["StaffReply"] == "Reply")
                    {
                        CommentsService cmntsSrvc1 = new CommentsService();
                        Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                        criteria1.Add("EntityRefId", account.Id);
                        Dictionary<long, IList<Comments>> list = cmntsSrvc1.GetCommentsListWithPaging(0, 1000, string.Empty, string.Empty, criteria1);
                        if (list != null && list.Count > 0)
                        {
                            foreach (Comments cm in list.First().Value)
                            {
                                if (string.IsNullOrWhiteSpace(cm.ResolutionComments))
                                {
                                    cm.ResolutionComments = Request["txtLeaveDescription"];
                                    cm.AppName = "B-HRM";
                                    cmntsSrvc1.CreateOrUpdateComments(cm);
                                    break;
                                }
                            }
                        }
                        account.Performer = userId;
                        pfs.CompleteBankActivityHRManagement(account, "BankAccount", userId, account.Status, false);
                        return RedirectToAction("BankAccountGrid", "HRManagement");
                    }
                    else if (Request.Form["nameComplete"] == "Complete")
                    {
                        account.Performer = userId;
                        //write the complete activity logic
                        //  uploaddisplay(cm1.Id);
                        pfs.CompleteBankActivityHRManagement(account, "BankAccount", userId, "Complete", false);
                        return RedirectToAction("BankAccountGrid", "HRManagement");
                    }
                    return View();

                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }

        }
        public ActionResult BankAccountRejectionGrid(long? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    CommentsService cs = new CommentsService(); // TODO: Initialize to an appropriate value
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    {
                        criteria.Add("EntityRefId", Id);
                        criteria.Add("AppName", "B-HRM");
                    }
                    Dictionary<long, IList<Comments>> CommentsList = cs.GetCommentsListWithPaging(page - 1, rows, sidx, sord, criteria);
                    if (CommentsList != null && CommentsList.Count > 0)
                    {
                        long totalRecords = CommentsList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows = (
                                 from items in CommentsList.First().Value
                                 select new
                                 {
                                     i = items.CommentId,
                                     cell = new string[] { items.CommentedBy, items.CommentedOn.ToString(), items.RejectionComments, items.ResolutionComments }
                                 })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        TempData["NoComments"] = 0;
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
        public ActionResult BankAccountActivitiesListJqGrid(long? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ProcessFlowServices pfs = new ProcessFlowServices(); // TODO: Initialize to an appropriate value
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    {
                        criteria.Add("ProcessRefId", Id);
                        criteria.Add("TemplateId", (long)7);
                    }

                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<BankMgmntActivity>> ActivitiesList = pfs.GetBankHRActivityListWithsearchCriteria(page - 1, rows, sidx, sord, criteria, string.Empty, null, null);

                    if (ActivitiesList != null && ActivitiesList.Count > 0)
                    {
                        long totalRecords = ActivitiesList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
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
                                     cell = new string[] { items.Id.ToString(), items.ActivityFullName, items.Available ? "Available" : items.Assigned ? "Assigned" : "Completed", items.Performer != null ? items.Performer.ToString() : "", items.CreatedDate.ToString(), items.AppRole, items.BankAccount.Id.ToString() }
                                 })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
        public ActionResult BankAccountBulkApprove(long[] ActivityId, string Template, string userId)
        {
            try
            {
                Template = "BankAccount";
                userId = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                ProcessFlowServices pfs = new ProcessFlowServices();
                bool BulkComplete = pfs.BankAccountBulkCompleteActivity(ActivityId, Template, userId);
                return Json(BulkComplete, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
        // CertificateRequest Details...

        public ActionResult ShowCertificateRequestDetails(long? Id)
        {

            try
            {
                CertificateRequest Crequest = Hrms.GetCertificateRequestById(Convert.ToInt64(Id));
                return View(Crequest);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
        public ActionResult CertificateRequestGrid(int? rows, string sidx = "Id", string sord = "Desc", int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TIPS.Service.UserService us = new UserService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("UserId", userId);
                    criteria.Add("AppCode", "HRM");
                    criteria.Add("RoleCode", "HEAD");
                    Dictionary<long, IList<UserAppRole>> UserAppRoleList = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 100, sidx, sord, criteria);
                    var ListCount = (from u in UserAppRoleList.First().Value
                                     where u.UserId == userId
                                     select u).ToList();
                    if (ListCount.Count >= 1)
                    {
                        ViewBag.count = "1";
                    }
                    else
                    {
                        ViewBag.count = "0";
                    }

                    Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                    criteria1.Add("UserId", userId);
                    criteria1.Add("AppCode", "HRM");
                    criteria1.Add("RoleCode", "RM");
                    Dictionary<long, IList<UserAppRole>> UserAppRoleList1 = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 100, sidx, sord, criteria1);
                    var ListCount1 = (from u in UserAppRoleList1.First().Value
                                      where u.UserId == userId
                                      select u).ToList();
                    if (ListCount1.Count >= 1)
                    {
                        ViewBag.count1 = "1";
                    }
                    else
                    {
                        ViewBag.count1 = "0";
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
        public ActionResult CertificateRequestJqGrid(long? Id, string ddlSearchBy, string txtSearch, string fromDate, string status, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ProcessFlowServices pfs = new ProcessFlowServices(); // TODO: Initialize to an appropriate value
                    //pass userid and get the list of roles from user service GetAppRoleForAnUserListWithPagingAndCriteria
                    string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();

                    UserService us = new UserService();
                    Dictionary<long, IList<UserAppRole>> userAppRole = new Dictionary<long, IList<UserAppRole>>();
                    Dictionary<string, object> criteriaUserAppRole = new Dictionary<string, object>();
                    criteriaUserAppRole.Add("UserId", userid);
                    //criteria.Add("ProcessedBy", userid);
                    userAppRole = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteriaUserAppRole);
                    if (userAppRole != null && userAppRole.Count > 0 && userAppRole.First().Key > 0)
                    {
                        int count = userAppRole.First().Value.Count;
                        //if it has values then for each concatenate APP+ROLE 
                        string[] userRoles = new string[count];
                        string[] deptCodeArr = new string[count];
                        string[] brnCodeArr = new string[count];
                        string[] Rolename = new string[count];


                        int i = 0;
                        Dictionary<string, object> criteria = new Dictionary<string, object>();
                        foreach (UserAppRole uar in userAppRole.First().Value)
                        {
                            string deptCode = uar.DeptCode;
                            string branchCode = uar.BranchCode;
                            string rolename = uar.RoleName;

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
                            if (!string.IsNullOrEmpty(rolename))
                            {
                                Rolename[i] = rolename;
                            }
                            i++;
                        }
                        brnCodeArr = brnCodeArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                        deptCodeArr = deptCodeArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                        criteria.Add("BranchCode", brnCodeArr);
                        if (status != "Assigned")
                            criteria.Add("DeptCode", deptCodeArr);
                        if (!string.IsNullOrEmpty(txtSearch))
                        {
                            txtSearch.Trim();
                            if (ddlSearchBy == "CreateDate")
                            {
                                var issuedate = Convert.ToDateTime(txtSearch);
                                criteria.Add("CertificateRequest." + ddlSearchBy.Trim(), issuedate);
                            }
                            else
                            {
                                criteria.Add("CertificateRequest." + ddlSearchBy.Trim(), txtSearch);
                            }
                        }
                        if ((!string.IsNullOrEmpty(fromDate) && !(string.IsNullOrEmpty(fromDate.Trim()))))
                        {
                            fromDate = fromDate.Trim();
                            string To = string.Format("{0:MM/dd/yyyy}", DateTime.Now);
                            DateTime ToDate = Convert.ToDateTime(To + " " + "23:59:59");
                            DateTime fdate = DateTime.Now;
                            DateTime tdate = DateTime.Now;
                            DateTime[] fromto = new DateTime[2];
                            if (!string.IsNullOrEmpty(fromDate))
                            {
                                var s = fromDate.Split('/');
                                var dd = s[0];
                                var mm = s[1];
                                var yy = s[2];
                                var resultField = mm + "/" + dd + "/" + yy;

                                fdate = Convert.ToDateTime(resultField);
                                fromto[0] = fdate;
                            }
                            if (ToDate != null)
                            {
                                fromto[1] = ToDate;
                            }
                            criteria.Add("CertificateRequest." + "CreateDate", fromto);
                        }
                        if (!string.IsNullOrEmpty(status))
                        {
                            if (status == "Available")
                            {
                                criteria.Add("Available", true);
                            }
                            else if (status == "Assigned")
                            {
                                criteria.Add("Assigned", true);
                                criteria.Add("Performer", userid);
                            }
                            else if (status == "Sent")
                            {
                                criteria.Add("Completed", true);
                            }
                            else if (status == "Completed")
                            {
                                criteria.Add("Completed", true);
                                criteria.Add("ActivityName", "Complete");
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(sord) && sord == "desc")
                            sord = "Desc";
                        else
                            sord = "Asc";
                        string[] alias = new string[1];
                        alias[0] = "CertificateRequest";
                        criteria.Add("TemplateId", (long)8);
                        Dictionary<long, IList<CertificateMgmntActivity>> AccountList = null;
                        if (Rolename[0] == "Staff")
                        {
                            criteria.Add("LeaveRequest." + "StaffName", userid);
                            AccountList = pfs.GetCertificateHRActivityListWithsearchCriteria(page - 1, rows, sidx, sord, criteria, "AppRole", userRoles, alias);
                        }
                        else
                        {
                            AccountList = pfs.GetCertificateHRActivityListWithsearchCriteria(page - 1, rows, sidx, sord, criteria, "AppRole", userRoles, alias);
                        }

                        if (AccountList != null && AccountList.Count > 0)
                        {
                            foreach (CertificateMgmntActivity pi in AccountList.First().Value)
                            {
                                pi.CertificateRequest.DifferenceInHours = DateTime.Now - pi.CertificateRequest.CreateDate;
                            }
                            long totalrecords = AccountList.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            if (status == "Sent" || status == "Completed")
                            {
                                var jsondat = new
                                {
                                    total = totalPages,
                                    page = page,
                                    records = totalrecords,
                                    rows = (from items in AccountList.First().Value
                                            select new
                                            {
                                                i = 2,
                                                cell = new string[] {
                                      items.CertificateRequest.Id.ToString(),
                                       "<a href='/HRManagement/ShowCertificateRequestDetails?id=" + items.CertificateRequest.Id + "&activityId=" + items.Id + "&activityName=" + items.ActivityName + "&status=" +items.CertificateRequest.Status +"&activityFullName=" + items.CertificateRequest.ActivityFullName + "'>"+ items.CertificateRequest.RequestNo +"</a>",
                                       items.CertificateRequest.CreateDate.ToString(),
                                       items.CertificateRequest.ActivityFullName,
                                       items.CertificateRequest.StaffName,
                                       items.CertificateRequest.StaffIdNumber,
                                       items.CertificateRequest.BranchCode,
                                       items.CertificateRequest.Department,
                                        "<img src='/Images/History.png ' id='ImgHistory' onclick=\"ShowComments('" + items.ProcessRefId + "');\" />",
                                        items.CertificateRequest.Status=="Completed"?"Completed":items.CertificateRequest.DifferenceInHours.Value.TotalHours.ToString(),

                            }
                                            })
                                };
                                return Json(jsondat, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                var jsondat = new
                                {
                                    total = totalPages,
                                    page = page,
                                    records = totalrecords,
                                    rows = (from items in AccountList.First().Value
                                            select new
                                            {
                                                i = 2,
                                                cell = new string[] {


                                      items.Id.ToString(),
                                       "<a href='/HRManagement/CertificateRequest?id=" + items.CertificateRequest.Id + "&activityId=" + items.Id + "&activityName=" + items.ActivityName + "&status=" +items.CertificateRequest.Status +"&activityFullName=" + items.CertificateRequest.ActivityFullName + "'>"+ items.CertificateRequest.RequestNo +"</a>",
                                       items.CertificateRequest.CreateDate.ToString(),
                                       items.CertificateRequest.ActivityFullName, 
                                       items.CertificateRequest.StaffName,
                                       items.CertificateRequest.StaffIdNumber,
                                       items.CertificateRequest.BranchCode,
                                       items.CertificateRequest.Department,
                                       "<img src='/Images/History.png' id='ImgHistory' onclick=\"ShowComments('" + items.ProcessRefId + "');\" />",
                                       items.CertificateRequest.Status=="Completed"?"Completed":items.CertificateRequest.DifferenceInHours.Value.TotalHours.ToString(),
                            }
                                            })
                                };
                                return Json(jsondat, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
        public ActionResult CertificateRequest(long? Id, long? activityId, string activityName, string ActivityFullName)
        {
            try
            {
                CertificateRequest Crequest = new CertificateRequest();
                UserService us = new UserService();

                string userId = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("UserId", userId);
                Dictionary<long, IList<UserAppRole>> UserAppRoleList = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                var UserDetails = (from u in UserAppRoleList.First().Value
                                   where u.UserId == userId
                                   select new { u.RoleName, u.BranchCode }).ToArray();

                StaffDetailsHRM SDetails = Hrms.GetStaffById(userId);

                if ((Id > 0) && (Id != null))
                {
                    string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                    ProcessFlowServices pfs = new ProcessFlowServices();
                    pfs.AssignActivity((Convert.ToInt64(activityId)), userid);

                    CertificateRequest Crequest1 = Hrms.GetCertificateRequestById(Convert.ToInt64(Id));
                    ViewBag.CertificateDetails = 5;
                    return View(Crequest1);

                }
                else
                {
                    StaffDetailsHRM Staff = Hrms.GetStaffById(userId);
                    Crequest.ProcessedBy = userId;
                    Crequest.CreateDate = DateTime.Now;
                    Crequest.UserRole = UserDetails[0].RoleName;
                    Crequest.Status = "Opened";
                    Crequest.StaffName = Staff.Name;
                    Crequest.StaffIdNumber = Staff.IdNumber;
                    Crequest.BranchCode = Staff.Campus.ToUpper();
                    Crequest.Department = Staff.Department;
                    ViewBag.CertificateReply = 1;
                    return View(Crequest);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult CertificateRequest(CertificateRequest Crequest)
        {
            ProcessFlowServices pfs = new ProcessFlowServices();
            HRManagementService Hrms = new HRManagementService();
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {

                    if (Crequest != null && !string.IsNullOrWhiteSpace(Crequest.Department))
                    {
                        Crequest.DeptCode = Crequest.Department.ToUpper();
                    }

                    string RejComments = Request["txtRejectionDetails"];

                    long id = 0;

                    if (Request.Form["btnSentApply"] == "Apply")
                    {
                        if (Crequest.Id == 0)
                        {
                            id = pfs.StartCertificateHRManagement(Crequest, "CertificateRequest", userId);
                            Crequest.RequestNo = "CRF-" + id;
                            Hrms.CreateOrUpdateCertificateRequest(Crequest);
                            Crequest.Performer = userId;
                            pfs.CompleteCertificateActivityHRManagement(Crequest, "CertificateRequest", userId, "CreateRequest", false);
                            TempData["CertificateId"] = Crequest.RequestNo;
                            return RedirectToAction("CertificateRequestGrid", "HRManagement");
                        }
                    }
                    else if (Request.Form["btnResolveRequest"] == "Resolve Request")
                    {
                        //write the complete activity logic
                        Crequest.Performer = userId;
                        pfs.CompleteCertificateActivityHRManagement(Crequest, "CertificateRequest", userId, Crequest.Status, false);
                        return RedirectToAction("CertificateRequestGrid", "HRManagement");
                    }
                    else if (Request.Form["btnLeaderRejection"] == "Reject")
                    {
                        CommentsService cmntsSrvc = new CommentsService();
                        Comments cmntsObj = new Comments();
                        cmntsObj.EntityRefId = Crequest.Id;
                        cmntsObj.CommentedBy = userId;
                        cmntsObj.CommentedOn = DateTime.Now;
                        cmntsObj.RejectionComments = Request["txtRejectionDetails"];
                        cmntsObj.AppName = "C-HRM";
                        cmntsSrvc.CreateOrUpdateComments(cmntsObj);
                        Crequest.Performer = userId;
                        //write the complete activity logic
                        pfs.CompleteCertificateActivityHRManagement(Crequest, "CertificateRequest", userId, Crequest.Status, true);
                        return RedirectToAction("CertificateRequestGrid", "HRManagement");
                    }
                    else if (Request.Form["btnApproval"] == "Approve")
                    {
                        Crequest.Performer = userId;
                        //write the complete activity logic
                        pfs.CompleteCertificateActivityHRManagement(Crequest, "CertificateRequest", userId, "ApproveRequest", false);
                        //redirect to list page
                        return RedirectToAction("CertificateRequestGrid", "HRManagement");
                    }
                    else if (Request.Form["StaffReply"] == "Reply")
                    {
                        CommentsService cmntsSrvc1 = new CommentsService();
                        Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                        criteria1.Add("EntityRefId", Crequest.Id);
                        Dictionary<long, IList<Comments>> list = cmntsSrvc1.GetCommentsListWithPaging(0, 1000, string.Empty, string.Empty, criteria1);
                        if (list != null && list.Count > 0)
                        {
                            foreach (Comments cm in list.First().Value)
                            {
                                if (string.IsNullOrWhiteSpace(cm.ResolutionComments))
                                {
                                    cm.ResolutionComments = Request["txtLeaveDescription"];
                                    cm.AppName = "C-HRM";
                                    cmntsSrvc1.CreateOrUpdateComments(cm);
                                    break;
                                }
                            }
                        }
                        Crequest.Performer = userId;
                        pfs.CompleteCertificateActivityHRManagement(Crequest, "CertificateRequest", userId, Crequest.Status, false);
                        return RedirectToAction("CertificateRequestGrid", "HRManagement");
                    }
                    else if (Request.Form["nameComplete"] == "Complete")
                    {
                        Crequest.Performer = userId;
                        pfs.CompleteCertificateActivityHRManagement(Crequest, "CertificateRequest", userId, "Complete", false);
                        return RedirectToAction("CertificateRequestGrid", "HRManagement");
                    }

                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }

        }
        public ActionResult CertificateRejectionGrid(long? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    CommentsService cs = new CommentsService(); // TODO: Initialize to an appropriate value
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    {
                        criteria.Add("EntityRefId", Id);
                        criteria.Add("AppName", "C-HRM");
                    }
                    Dictionary<long, IList<Comments>> CommentsList = cs.GetCommentsListWithPaging(page - 1, rows, sidx, sord, criteria);
                    if (CommentsList != null && CommentsList.Count > 0)
                    {
                        long totalRecords = CommentsList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows = (
                                 from items in CommentsList.First().Value
                                 select new
                                 {
                                     i = items.CommentId,
                                     cell = new string[] { items.CommentedBy, items.CommentedOn.ToString(), items.RejectionComments, items.ResolutionComments }
                                 })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        TempData["NoComments"] = 0;
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
        public ActionResult CertificateActivitiesListJqGrid(long? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ProcessFlowServices pfs = new ProcessFlowServices(); // TODO: Initialize to an appropriate value
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    {
                        criteria.Add("ProcessRefId", Id);
                        criteria.Add("TemplateId", (long)8);
                    }
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<CertificateMgmntActivity>> ActivitiesList = pfs.GetCertificateHRActivityListWithsearchCriteria(page - 1, rows, sidx, sord, criteria, string.Empty, null, null);

                    if (ActivitiesList != null && ActivitiesList.Count > 0)
                    {
                        long totalRecords = ActivitiesList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
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
                                     cell = new string[] { items.Id.ToString(), items.ActivityFullName, items.Available ? "Available" : items.Assigned ? "Assigned" : "Completed", items.Performer != null ? items.Performer.ToString() : "", items.CreatedDate.ToString(), items.AppRole, items.CertificateRequest.Id.ToString() }
                                 })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
        public ActionResult CertificateBulkApprove(long[] ActivityId, string Template, string userId)
        {
            try
            {
                Template = "CertificateRequest";
                userId = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                ProcessFlowServices pfs = new ProcessFlowServices();
                bool BulkComplete = pfs.CertificateBulkCompleteActivityHRManagement(ActivityId, Template, userId);
                return Json(BulkComplete, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }

        // SalaryAdvance...

        public ActionResult ShowSalaryAdvanceDetails(long? Id)
        {
            try
            {
                SalaryAdvance Salaryadvance = Hrms.GetSalaryAdvanceRequestById(Convert.ToInt64(Id));
                return View(Salaryadvance);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
        public ActionResult SalaryAdvanceGrid(int? rows, string sidx = "Id", string sord = "Desc", int? page = 1)
        {

            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TIPS.Service.UserService us = new UserService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("UserId", userId);
                    criteria.Add("AppCode", "HRM");
                    criteria.Add("RoleCode", "HEAD");
                    Dictionary<long, IList<UserAppRole>> UserAppRoleList = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 100, sidx, sord, criteria);
                    var ListCount = (from u in UserAppRoleList.First().Value
                                     where u.UserId == userId
                                     select u).ToList();
                    if (ListCount.Count >= 1)
                    {
                        ViewBag.count = "1";
                    }
                    else
                    {
                        ViewBag.count = "0";
                    }

                    Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                    criteria1.Add("UserId", userId);
                    criteria1.Add("AppCode", "HRM");
                    criteria1.Add("RoleCode", "RM");
                    Dictionary<long, IList<UserAppRole>> UserAppRoleList1 = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 100, sidx, sord, criteria1);
                    var ListCount1 = (from u in UserAppRoleList1.First().Value
                                      where u.UserId == userId
                                      select u).ToList();
                    if (ListCount1.Count >= 1)
                    {
                        ViewBag.count1 = "1";
                    }
                    else
                    {
                        ViewBag.count1 = "0";
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
        public ActionResult SalaryAdvanceJqGrid(long? Id, string ddlSearchBy, string txtSearch, string fromDate, string status, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ProcessFlowServices pfs = new ProcessFlowServices(); // TODO: Initialize to an appropriate value
                    //pass userid and get the list of roles from user service GetAppRoleForAnUserListWithPagingAndCriteria
                    string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();

                    UserService us = new UserService();
                    Dictionary<long, IList<UserAppRole>> userAppRole = new Dictionary<long, IList<UserAppRole>>();
                    Dictionary<string, object> criteriaUserAppRole = new Dictionary<string, object>();
                    criteriaUserAppRole.Add("UserId", userid);
                    userAppRole = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteriaUserAppRole);
                    if (userAppRole != null && userAppRole.Count > 0 && userAppRole.First().Key > 0)
                    {
                        int count = userAppRole.First().Value.Count;
                        //if it has values then for each concatenate APP+ROLE 
                        string[] userRoles = new string[count];
                        string[] deptCodeArr = new string[count];
                        string[] brnCodeArr = new string[count];
                        string[] Rolename = new string[count];


                        int i = 0;
                        Dictionary<string, object> criteria = new Dictionary<string, object>();
                        foreach (UserAppRole uar in userAppRole.First().Value)
                        {
                            string deptCode = uar.DeptCode;
                            string branchCode = uar.BranchCode;
                            string rolename = uar.RoleName;

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
                            if (!string.IsNullOrEmpty(rolename))
                            {
                                Rolename[i] = rolename;
                            }
                            i++;
                        }
                        brnCodeArr = brnCodeArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                        deptCodeArr = deptCodeArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                        criteria.Add("BranchCode", brnCodeArr);
                        if (status != "Assigned")
                            criteria.Add("DeptCode", deptCodeArr);
                        if (!string.IsNullOrEmpty(txtSearch))
                        {
                            txtSearch.Trim();
                            if (ddlSearchBy == "CreateDate")
                            {
                                var issuedate = Convert.ToDateTime(txtSearch);
                                criteria.Add("SalaryAdvance." + ddlSearchBy.Trim(), issuedate);
                            }
                            else
                            {
                                criteria.Add("SalaryAdvance." + ddlSearchBy.Trim(), txtSearch);
                            }
                        }
                        if ((!string.IsNullOrEmpty(fromDate) && !(string.IsNullOrEmpty(fromDate.Trim()))))
                        {
                            fromDate = fromDate.Trim();
                            string To = string.Format("{0:MM/dd/yyyy}", DateTime.Now);
                            DateTime ToDate = Convert.ToDateTime(To + " " + "23:59:59");
                            DateTime fdate = DateTime.Now;
                            DateTime tdate = DateTime.Now;
                            DateTime[] fromto = new DateTime[2];
                            if (!string.IsNullOrEmpty(fromDate))
                            {
                                var s = fromDate.Split('/');
                                var dd = s[0];
                                var mm = s[1];
                                var yy = s[2];
                                var resultField = mm + "/" + dd + "/" + yy;

                                fdate = Convert.ToDateTime(resultField);
                                fromto[0] = fdate;
                            }
                            if (ToDate != null)
                            {
                                fromto[1] = ToDate;
                            }
                            criteria.Add("SalaryAdvance." + "CreateDate", fromto);
                        }
                        if (!string.IsNullOrEmpty(status))
                        {
                            if (status == "Available")
                            {
                                criteria.Add("Available", true);
                            }
                            else if (status == "Assigned")
                            {
                                criteria.Add("Assigned", true);
                                criteria.Add("Performer", userid);
                            }
                            else if (status == "Sent")
                            {
                                criteria.Add("Completed", true);
                            }
                            else if (status == "Completed")
                            {
                                criteria.Add("Completed", true);
                                criteria.Add("ActivityName", "Complete");
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(sord) && sord == "desc")
                            sord = "Desc";
                        else
                            sord = "Asc";
                        string[] alias = new string[1];
                        alias[0] = "SalaryAdvance";
                        criteria.Add("TemplateId", (long)9);
                        Dictionary<long, IList<SalaryAdvanceMgmntActivity>> AccountList = null;
                        
                        if (Rolename[0] == "Staff")
                        {
                            criteria.Add("LeaveRequest." + "StaffName", userid);
                            AccountList = pfs.GetSalaryAdvanceHRActivityListWithsearchCriteria(page - 1, rows, sidx, sord, criteria, "AppRole", userRoles, alias);
                        }
                        else
                        {
                            AccountList = pfs.GetSalaryAdvanceHRActivityListWithsearchCriteria(page - 1, rows, sidx, sord, criteria, "AppRole", userRoles, alias);
                        }

                        if (AccountList != null && AccountList.Count > 0)
                        {
                            foreach (SalaryAdvanceMgmntActivity pi in AccountList.First().Value)
                            {
                                pi.SalaryAdvance.DifferenceInHours = DateTime.Now - pi.SalaryAdvance.CreateDate;
                            }
                            long totalrecords = AccountList.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            if (status == "Sent" || status == "Completed")
                            {
                                var jsondat = new
                                {
                                    total = totalPages,
                                    page = page,
                                    records = totalrecords,
                                    rows = (from items in AccountList.First().Value
                                            select new
                                            {
                                                i = 2,
                                                cell = new string[] {
                                      items.SalaryAdvance.Id.ToString(),
                                       "<a href='/HRManagement/ShowSalaryAdvanceDetails?id=" + items.SalaryAdvance.Id + "&activityId=" + items.Id + "&activityName=" + items.ActivityName + "&status=" +items.SalaryAdvance.Status +"&activityFullName=" + items.SalaryAdvance.ActivityFullName + "'>"+ items.SalaryAdvance.RequestNo +"</a>",
                                       items.SalaryAdvance.CreateDate.ToString(),
                                       items.SalaryAdvance.ActivityFullName,
                                       items.SalaryAdvance.StaffName,
                                       items.SalaryAdvance.StaffIdNumber,
                                       items.SalaryAdvance.BranchCode,
                                       items.SalaryAdvance.DateAmountNeeded,
                                       items.SalaryAdvance.Department,
                                        "<img src='/Images/History.png ' id='ImgHistory' onclick=\"ShowComments('" + items.ProcessRefId + "');\" />",
                                        items.SalaryAdvance.Status=="Completed"?"Completed":items.SalaryAdvance.DifferenceInHours.Value.TotalHours.ToString(),

                            }
                                            })
                                };
                                return Json(jsondat, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                var jsondat = new
                                {
                                    total = totalPages,
                                    page = page,
                                    records = totalrecords,
                                    rows = (from items in AccountList.First().Value
                                            select new
                                            {
                                                i = 2,
                                                cell = new string[] {


                                      items.Id.ToString(),
                                       "<a href='/HRManagement/SalaryAdvance?id=" + items.SalaryAdvance.Id + "&activityId=" + items.Id + "&activityName=" + items.ActivityName + "&status=" +items.SalaryAdvance.Status +"&activityFullName=" + items.SalaryAdvance.ActivityFullName + "'>"+ items.SalaryAdvance.RequestNo +"</a>",
                                       items.SalaryAdvance.CreateDate.ToString(),
                                       items.SalaryAdvance.ActivityFullName, 
                                       items.SalaryAdvance.StaffName,
                                       items.SalaryAdvance.StaffIdNumber,
                                       items.SalaryAdvance.BranchCode,
                                       items.SalaryAdvance.DateAmountNeeded,
                                       items.SalaryAdvance.Department,
                                       "<img src='/Images/History.png' id='ImgHistory' onclick=\"ShowComments('" + items.ProcessRefId + "');\" />",
                                       items.SalaryAdvance.Status=="Completed"?"Completed":items.SalaryAdvance.DifferenceInHours.Value.TotalHours.ToString(),
                            }
                                            })
                                };
                                return Json(jsondat, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {

                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
        public ActionResult SalaryAdvance(long? Id, long? activityId, string activityName, string ActivityFullName)
        {
            try
            {
                SalaryAdvance salary = new SalaryAdvance();
                UserService us = new UserService();

                string userId = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("UserId", userId);
                Dictionary<long, IList<UserAppRole>> UserAppRoleList = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                var UserDetails = (from u in UserAppRoleList.First().Value
                                   where u.UserId == userId
                                   select new { u.RoleName, u.BranchCode }).ToArray();

                StaffDetailsHRM SDetails = Hrms.GetStaffById(userId);

                if ((Id > 0) && (Id != null))
                {
                    string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                    ProcessFlowServices pfs = new ProcessFlowServices();
                    pfs.AssignActivity((Convert.ToInt64(activityId)), userid);

                    SalaryAdvance Salary1 = Hrms.GetSalaryAdvanceRequestById(Convert.ToInt64(Id));
                    ViewBag.SalaryAdvanceDetails = 5;
                    return View(Salary1);

                }
                else
                {
                    StaffDetailsHRM Staff = Hrms.GetStaffById(userId);
                    salary.ProcessedBy = userId;
                    salary.CreateDate = DateTime.Now;
                    salary.UserRole = UserDetails[0].RoleName;
                    salary.Status = "Opened";
                    salary.StaffName = Staff.Name;
                    salary.StaffIdNumber = Staff.IdNumber;
                    salary.BranchCode = Staff.Campus.ToUpper();
                    salary.Department = Staff.Department;
                    ViewBag.SalalryAdvanceReply = 1;
                    return View(salary);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult SalaryAdvance(SalaryAdvance salaryRequest)
        {
            ProcessFlowServices pfs = new ProcessFlowServices();
            HRManagementService Hrms = new HRManagementService();
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (salaryRequest != null && !string.IsNullOrWhiteSpace(salaryRequest.Department))
                    {
                        salaryRequest.DeptCode = salaryRequest.Department.ToUpper();
                    }


                    string RejComments = Request["txtRejectionDetails"];

                    long id = 0;

                    if (Request.Form["btnSentApply"] == "Apply")
                    {
                        if (salaryRequest.Id == 0)
                        {
                            id = pfs.StartSalaryAdvanceHRManagement(salaryRequest, "SalaryAdvance", userId);
                            salaryRequest.RequestNo = "SARF-" + id;
                            Hrms.CreateOrUpdateSalaryAdvance(salaryRequest);
                            salaryRequest.Performer = userId;
                            pfs.CompleteSalaryAdvanceActivityHRManagement(salaryRequest, "SalaryAdvance", userId, "CreateRequest", false);
                            TempData["SalaryAdvanceId"] = salaryRequest.RequestNo;
                            return RedirectToAction("SalaryAdvanceGrid", "HRManagement");
                        }
                    }
                    else if (Request.Form["btnResolveRequest"] == "Resolve Request")
                    {
                        //write the complete activity logic
                        salaryRequest.Performer = userId;
                        pfs.CompleteSalaryAdvanceActivityHRManagement(salaryRequest, "SalaryAdvance", userId, salaryRequest.Status, false);
                        return RedirectToAction("SalaryAdvanceGrid", "HRManagement");
                    }
                    else if (Request.Form["btnLeaderRejection"] == "Reject")
                    {
                        CommentsService cmntsSrvc = new CommentsService();
                        Comments cmntsObj = new Comments();
                        cmntsObj.EntityRefId = salaryRequest.Id;
                        cmntsObj.CommentedBy = userId;
                        cmntsObj.CommentedOn = DateTime.Now;
                        cmntsObj.RejectionComments = Request["txtRejectionDetails"];
                        cmntsObj.AppName = "S-HRM";
                        cmntsSrvc.CreateOrUpdateComments(cmntsObj);
                        salaryRequest.Performer = userId;
                        //write the complete activity logic
                        pfs.CompleteSalaryAdvanceActivityHRManagement(salaryRequest, "SalaryAdvance", userId, salaryRequest.Status, true);
                        return RedirectToAction("SalaryAdvanceGrid", "HRManagement");
                    }
                    else if (Request.Form["btnApproval"] == "Approve")
                    {
                        salaryRequest.Performer = userId;
                        //write the complete activity logic
                        pfs.CompleteSalaryAdvanceActivityHRManagement(salaryRequest, "SalaryAdvance", userId, "ApproveRequest", false);
                        //redirect to list page
                        return RedirectToAction("SalaryAdvanceGrid", "HRManagement");
                    }
                    else if (Request.Form["StaffReply"] == "Reply")
                    {
                        CommentsService cmntsSrvc1 = new CommentsService();
                        Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                        criteria1.Add("EntityRefId", salaryRequest.Id);
                        Dictionary<long, IList<Comments>> list = cmntsSrvc1.GetCommentsListWithPaging(0, 1000, string.Empty, string.Empty, criteria1);
                        if (list != null && list.Count > 0)
                        {
                            foreach (Comments cm in list.First().Value)
                            {
                                if (string.IsNullOrWhiteSpace(cm.ResolutionComments))
                                {
                                    cm.ResolutionComments = Request["txtLeaveDescription"];
                                    cm.AppName = "S-HRM";
                                    cmntsSrvc1.CreateOrUpdateComments(cm);
                                    break;
                                }
                            }
                        }
                        salaryRequest.Performer = userId;
                        pfs.CompleteSalaryAdvanceActivityHRManagement(salaryRequest, "SalaryAdvance", userId, salaryRequest.Status, false);
                        return RedirectToAction("SalaryAdvanceGrid", "HRManagement");
                    }
                    else if (Request.Form["nameComplete"] == "Complete")
                    {
                        salaryRequest.Performer = userId;
                        pfs.CompleteSalaryAdvanceActivityHRManagement(salaryRequest, "SalaryAdvance", userId, "Complete", false);
                        return RedirectToAction("SalaryAdvanceGrid", "HRManagement");
                    }

                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
        public ActionResult SalaryAdvanceRejectionGrid(long? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    CommentsService cs = new CommentsService(); // TODO: Initialize to an appropriate value
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    {
                        criteria.Add("EntityRefId", Id);
                        criteria.Add("AppName", "S-HRM");
                    }
                    Dictionary<long, IList<Comments>> CommentsList = cs.GetCommentsListWithPaging(page - 1, rows, sidx, sord, criteria);
                    if (CommentsList != null && CommentsList.Count > 0)
                    {
                        long totalRecords = CommentsList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows = (
                                 from items in CommentsList.First().Value
                                 select new
                                 {
                                     i = items.CommentId,
                                     cell = new string[] { items.CommentedBy, items.CommentedOn.ToString(), items.RejectionComments, items.ResolutionComments }
                                 })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        TempData["NoComments"] = 0;
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
        public ActionResult SalaryAdvanceActivitiesListJqGrid(long? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ProcessFlowServices pfs = new ProcessFlowServices(); // TODO: Initialize to an appropriate value
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    {
                        criteria.Add("ProcessRefId", Id);
                        criteria.Add("TemplateId", (long)9);
                    }
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<SalaryAdvanceMgmntActivity>> ActivitiesList = pfs.GetSalaryAdvanceHRActivityListWithsearchCriteria(page - 1, rows, sidx, sord, criteria, string.Empty, null, null);

                    if (ActivitiesList != null && ActivitiesList.Count > 0)
                    {
                        long totalRecords = ActivitiesList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
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
                                     cell = new string[] { items.Id.ToString(), items.ActivityFullName, items.Available ? "Available" : items.Assigned ? "Assigned" : "Completed", items.Performer != null ? items.Performer.ToString() : "", items.CreatedDate.ToString(), items.AppRole, items.SalaryAdvance.Id.ToString() }
                                 })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
        public ActionResult SalaryAdvanceBulkApprove(long[] ActivityId, string Template, string userId)
        {
            try
            {
                Template = "SalaryAdvance";
                userId = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                ProcessFlowServices pfs = new ProcessFlowServices();
                bool BulkComplete = pfs.SalaryAdvanceBulkCompleteActivityHRManagement(ActivityId, Template, userId);
                return Json(BulkComplete, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "HRManagementPolicy");
                throw ex;
            }
        }
    }
}
