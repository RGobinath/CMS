using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TIPS.Entities;
using TIPS.Entities.StaffEntities;
using TIPS.Service;
using TIPS.ServiceContract;
using TIPS.Entities.AdmissionEntities;
using CMS.Helpers;
using TIPS.Entities.StaffManagementEntities;
using TIPS.Entities.InboxEntities;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;
namespace CMS.Controllers
{
    public class StaffIssuesController : EmailHelper
    {
        string info = string.Empty;
        UserService us = new UserService();
        public ActionResult StaffIssueManagement()
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
                    criteria.Add("AppCode", "SIM");
                    //criteria.Add("RoleCode", "STAFF");
                    Dictionary<long, IList<UserAppRole>> UserAppRoleList = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 100, string.Empty, string.Empty, criteria);
                    var ListCount = (from u in UserAppRoleList.First().Value
                                     where u.UserId == userId && u.RoleCode == "STAFF"
                                     select u).ToList();
                    if (ListCount.Count >= 1)
                    {
                        ViewBag.count = "1";
                    }
                    else
                    {
                        ViewBag.count = "0";
                    }
                    ViewBag.SIMGTStatus = Session["SIMGTStatus"];
                    var ListCount1 = (from u in UserAppRoleList.First().Value
                                      where u.UserId == userId && u.RoleCode == "GRL"
                                      select u).ToList();
                    if (ListCount1.Count >= 1)
                    {
                        foreach (var items in ListCount1)
                        {
                            if (items.RoleCode == "GRL" && items.AppCode == "SIM")
                            {
                                ViewBag.ShowDueDate = "1";
                            }
                            else
                            {
                                ViewBag.ShowDueDate = "0";
                            }
                        }

                    }
                    else
                    {
                        ViewBag.ShowDueDate = "0";
                    }
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
            return View();
        }

        //public ActionResult StaffIssueManagementListJqGrid(long? Id, string ExptType, string ddlSearchBy, string txtSearch, string fromDate, long? fromIssueNum, long? toIssueNum, string status, int rows, string sidx, string sord, int? page = 1)
        //{
        //    string userId = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
        //    try
        //    {
        //        UserService us = new UserService();
        //        StaffIssuesService sis = new StaffIssuesService();
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        Dictionary<long, IList<UserAppRole>> userAppRole = new Dictionary<long, IList<UserAppRole>>();
        //        Dictionary<string, object> criteriaUserAppRole = new Dictionary<string, object>();
        //        criteriaUserAppRole.Add("UserId", userId);
        //        criteriaUserAppRole.Add("AppCode", "SIM");
        //        userAppRole = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteriaUserAppRole);

        //        if (userAppRole != null && userAppRole.Count > 0 && userAppRole.First().Key > 0)
        //        {
        //            int count = userAppRole.First().Value.Count;
        //            //if it has values then for each concatenate APP+ROLE 
        //            string[] userRoles = new string[count];
        //            string[] deptCodeArr = new string[count];
        //            string[] brnCodeArr = new string[count];
        //            string[] roleCodeArr = new string[count];
        //            int i = 0;
        //            foreach (UserAppRole uar in userAppRole.First().Value)
        //            {
        //                string deptCode = uar.DeptCode;
        //                string branchCode = uar.BranchCode;
        //                //if (uar.RoleCode == "STAFF")
        //                //{
        //                //    criteria.Add("StaffIssues." + "CreatedBy", userId);
        //                //}
        //                if (!string.IsNullOrWhiteSpace(uar.AppCode.Trim()) && !string.IsNullOrWhiteSpace(uar.RoleCode.Trim()))
        //                {
        //                    userRoles[i] = uar.AppCode.Trim() + uar.RoleCode.Trim();
        //                }
        //                if (!string.IsNullOrEmpty(deptCode))
        //                {
        //                    deptCodeArr[i] = deptCode;
        //                }
        //                if (!string.IsNullOrEmpty(branchCode))
        //                {
        //                    brnCodeArr[i] = branchCode;
        //                }
        //                if (!string.IsNullOrEmpty(uar.RoleCode))
        //                {
        //                    roleCodeArr[i] = uar.RoleCode;
        //                }
        //                i++;
        //            }
        //            if (userRoles.Contains("SIMSTAFF"))
        //                criteria.Add("StaffIssues." + "CreatedBy", userId);
        //            brnCodeArr = brnCodeArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        //            deptCodeArr = deptCodeArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        //            criteria.Add("BranchCode", brnCodeArr);
        //            if (status != "Assigned")
        //                criteria.Add("DeptCode", deptCodeArr);
        //            IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
        //            if (!string.IsNullOrEmpty(txtSearch))
        //            {
        //                txtSearch.Trim();
        //                if (ddlSearchBy == "CreatedDate")
        //                {
        //                    DateTime[] fromto = new DateTime[2];
        //                    fromto[0] = DateTime.Parse(txtSearch, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
        //                    string To = string.Format("{0:MM/dd/yyyy}", fromto[0]);
        //                    fromto[1] = Convert.ToDateTime(To + " " + "23:59:59");
        //                    criteria.Add("StaffIssues." + "CreatedDate", fromto);
        //                }
        //                else
        //                    criteria.Add("StaffIssues." + ddlSearchBy.Trim(), txtSearch);
        //            }
        //            if ((!string.IsNullOrEmpty(fromDate) && !(string.IsNullOrEmpty(fromDate.Trim()))))
        //            {
        //                fromDate = fromDate.Trim();
        //                string To = string.Format("{0:MM/dd/yyyy}", DateTime.Now);
        //                DateTime[] fromto = new DateTime[2];
        //                fromto[0] = DateTime.Parse(fromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
        //                fromto[1] = Convert.ToDateTime(To + " " + "23:59:59");
        //                criteria.Add("StaffIssues." + "CreatedDate", fromto);
        //            }

        //            //if ((!string.IsNullOrEmpty(fromDate) && !(string.IsNullOrEmpty(fromDate.Trim()))))
        //            //{
        //            //    fromDate = fromDate.Trim();
        //            //    string To = string.Format("{0:MM/dd/yyyy}", DateTime.Now);
        //            //    DateTime ToDate = Convert.ToDateTime(To + " " + "23:59:59");
        //            //    DateTime fdate = DateTime.Now;
        //            //    DateTime tdate = DateTime.Now;
        //            //    DateTime[] fromto = new DateTime[2];
        //            //    if (!string.IsNullOrEmpty(fromDate))
        //            //    {
        //            //        fdate = Convert.ToDateTime(fromDate);
        //            //        fromto[0] = fdate;
        //            //    }
        //            //    if (ToDate != null)
        //            //    {
        //            //        fromto[1] = ToDate;
        //            //    }
        //            //    criteria.Add("StaffIssues." + "CreatedDate", fromto);
        //            //}
        //            if (!string.IsNullOrEmpty(status))
        //            {
        //                if (status == "Available")
        //                    criteria.Add("Available", true);
        //                else if (status == "Assigned")
        //                {
        //                    criteria.Add("Assigned", true);
        //                    criteria.Add("Performer", userId);
        //                }
        //                else if (status == "Sent")
        //                {
        //                    criteria.Add("Completed", true);

        //                }
        //                else if (status == "Completed")
        //                {
        //                    criteria.Add("Completed", true);
        //                    criteria.Add("ActivityName", "Complete");
        //                }
        //            }
        //            string[] alias = new string[1];
        //            alias[0] = "StaffIssues";
        //            ProcessFlowServices pfs = new ProcessFlowServices();
        //            Dictionary<string, object> Criteria = new Dictionary<string, object>();
        //            Dictionary<long, IList<StaffMgmntActivity>> StaffIssueList = new Dictionary<long, IList<StaffMgmntActivity>>();
        //            //if (status == "Sent")
        //            //{
        //            //    Criteria.Add("Completed", true);
        //            //    Criteria.Add("Performer", userId);
        //            //    StaffIssueList = pfs.GetStaffIssueActivityListWithsearchCriteriaOnly(page - 1, rows, sord, sidx, Criteria);
        //            //}
        //            //else
        //            StaffIssueList = pfs.GetStaffIssueActivityListWithsearchCriteria(page - 1, rows, sidx, sord, criteria, "AppRole", userRoles, alias);
        //            if (StaffIssueList.Values != null && StaffIssueList.Count > 0)
        //            {
        //                foreach (StaffMgmntActivity pi in StaffIssueList.First().Value)
        //                {
        //                    pi.ProcessInstance.DifferenceInHours = DateTime.Now - pi.ProcessInstance.DateCreated;
        //                }
        //                long totalrecords = StaffIssueList.First().Key;
        //                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                if (ExptType == "Excel")
        //                {
        //                    var List = StaffIssueList.First().Value.ToList();
        //                    base.ExptToXL(List, "StaffIssueList", (items => new
        //                    {
        //                        items.Id,
        //                        items.StaffIssues.BranchCode,
        //                        items.StaffIssues.IssueNumber,
        //                        items.StaffIssues.IssueGroup,
        //                        items.StaffIssues.IssueType,
        //                        items.StaffIssues.Description,
        //                        items.StaffIssues.CreatedBy,
        //                        CreatedDate = items.StaffIssues.CreatedDate.ToString("dd/MM/yyyy"),
        //                        ActivityFullName = items.ActivityFullName,
        //                    }));
        //                    return new EmptyResult();
        //                }
        //                else
        //                {
        //                    if (status == "Sent")
        //                    {
        //                        var jsondat = new
        //                        {
        //                            total = totalPages,
        //                            page = page,
        //                            records = totalrecords,
        //                            rows = (from items in StaffIssueList.First().Value
        //                                    select new
        //                                    {
        //                                        i = 2,
        //                                        cell = new string[] {
        //                           items.Id.ToString(),
        //                          "<a href='/StaffIssues/ShowStaffIssue?id=" + items.StaffIssues.Id + "'>" + items.StaffIssues.IssueNumber + "</a>",
        //                          items.StaffIssues.BranchCode,
        //                           items.StaffIssues.IssueGroup,
        //                           items.StaffIssues.IssueType,
        //                           items.StaffIssues.CreatedBy,
        //                           items.StaffIssues.CreatedDate.ToString("dd/MM/yyyy"),
        //                           items.ActivityFullName,
        //                           "<img src='/Images/History.png ' id='ImgHistory' onclick=\"ShowComments('" + items.ProcessRefId + "');\" />",
        //                           items.ProcessInstance.Status=="Completed"?"Completed":items.ProcessInstance.DifferenceInHours.Value.TotalHours.ToString(),
        //                           items.StaffIssues.CreatedBy
        //                    }
        //                                    })
        //                        };
        //                        return Json(jsondat, JsonRequestBehavior.AllowGet);
        //                    }
        //                    else
        //                    {
        //                        var jsondat = new
        //                        {
        //                            total = totalPages,
        //                            page = page,
        //                            records = totalrecords,
        //                            rows = (from items in StaffIssueList.First().Value
        //                                    select new
        //                                    {
        //                                        i = 2,
        //                                        cell = new string[] {
        //                           items.Id.ToString(),
        //                          "<a href='/StaffIssues/StaffIssueCreation?id=" + items.StaffIssues.Id +"&activityId=" + items.Id + "&activityName=" + items.ActivityName + "&activityFullName=" + items.ActivityFullName +"'>" + items.StaffIssues.IssueNumber + "</a>",
        //                          items.StaffIssues.BranchCode, 
        //                          items.StaffIssues.IssueGroup,
        //                           items.StaffIssues.IssueType,
        //                           items.StaffIssues.CreatedBy,
        //                           items.StaffIssues.CreatedDate.ToString("dd/MM/yyyy"),
        //                           items.ActivityFullName,
        //                           "<img src='/Images/History.png ' id='ImgHistory' onclick=\"ShowComments('" + items.ProcessRefId + "');\" />",
        //                           items.ProcessInstance.Status=="Completed"?"Completed":items.ProcessInstance.DifferenceInHours.Value.TotalHours.ToString(),
        //                           items.StaffIssues.CreatedBy
        //                    }
        //                                    })
        //                        };
        //                        return Json(jsondat, JsonRequestBehavior.AllowGet);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
        //        throw ex;
        //    }
        //}
        public ActionResult StaffIssueManagementListJqGrid(long? Id, string ExptType, string ddlSearchBy, string txtSearch, string fromDate, long? fromIssueNum, long? toIssueNum, string status, int rows, string sidx, string sord, int? page = 1)
        {
            string userId = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
            try
            {
                if (!string.IsNullOrWhiteSpace(status))
                    Session["SIMGTStatus"] = status;
                else
                    status = (string)Session["SIMGTStatus"];
                UserService us = new UserService();
                StaffIssuesService sis = new StaffIssuesService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<UserAppRole>> userAppRole = new Dictionary<long, IList<UserAppRole>>();
                Dictionary<string, object> criteriaUserAppRole = new Dictionary<string, object>();
                criteriaUserAppRole.Add("UserId", userId);
                criteriaUserAppRole.Add("AppCode", "SIM");
                userAppRole = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteriaUserAppRole);                
                if (userAppRole != null && userAppRole.Count > 0 && userAppRole.First().Key > 0)
                {
                    int count = userAppRole.First().Value.Count;
                    //if it has values then for each concatenate APP+ROLE 
                    string[] userRoles = new string[count];
                    string[] deptCodeArr = new string[count];
                    string[] brnCodeArr = new string[count];
                    string[] roleCodeArr = new string[count];
                    int i = 0;
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

                    //if (roleCodeArr.Contains("STAFF"))

                    if (userRoles.Contains("SIMSTAFF"))
                        criteria.Add("StaffIssues." + "CreatedBy", userId);
                    brnCodeArr = brnCodeArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    deptCodeArr = deptCodeArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    criteria.Add("BranchCode", brnCodeArr);
                    if (status != "Assigned")
                        criteria.Add("DeptCode", deptCodeArr);
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    if (!string.IsNullOrEmpty(txtSearch))
                    {
                        txtSearch.Trim();
                        if (ddlSearchBy == "CreatedDate")
                        {
                            DateTime[] fromto = new DateTime[2];
                            fromto[0] = DateTime.Parse(txtSearch, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            string To = string.Format("{0:dd/MM/yyyy}", fromto[0]);
                            fromto[1] = Convert.ToDateTime(To + " " + "23:59:59");
                            criteria.Add("StaffIssues." + "CreatedDate", fromto);
                        }
                        else
                            criteria.Add("StaffIssues." + ddlSearchBy.Trim(), txtSearch);
                    }
                    if ((!string.IsNullOrEmpty(fromDate) && !(string.IsNullOrEmpty(fromDate.Trim()))))
                    {
                        fromDate = fromDate.Trim();
                        string To = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
                        DateTime[] fromto = new DateTime[2];
                        fromto[0] = DateTime.Parse(fromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        fromto[1] = Convert.ToDateTime(To + " " + "23:59:59");
                        criteria.Add("StaffIssues." + "CreatedDate", fromto);
                    }
                    if (!string.IsNullOrEmpty(status))
                    {
                        if (status == "Available")
                            criteria.Add("Available", true);
                        else if (status == "Assigned")
                        {
                            criteria.Add("Assigned", true);
                            criteria.Add("Performer", userId);
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
                    string[] alias = new string[1];
                    alias[0] = "StaffIssues";
                    ProcessFlowServices pfs = new ProcessFlowServices();
                    Dictionary<string, object> Criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<StaffMgmntActivity>> StaffIssueList = new Dictionary<long, IList<StaffMgmntActivity>>();
                    //if (status == "Sent")
                    //{
                    //    Criteria.Add("Completed", true);
                    //    Criteria.Add("Performer", userId);
                    //    StaffIssueList = pfs.GetStaffIssueActivityListWithsearchCriteriaOnly(page - 1, rows, sord, sidx, Criteria);
                    //}
                    //else
                    StaffIssueList = pfs.GetStaffIssueActivityListWithsearchCriteria(page - 1, rows, sidx, sord, criteria, "AppRole", userRoles, alias);
                    if (StaffIssueList.Values != null && StaffIssueList.Count > 0)
                    {
                        //foreach (StaffMgmntActivity pi in StaffIssueList.First().Value)
                        //{
                        //    pi.ProcessInstance.DifferenceInHours = DateTime.Now - pi.ProcessInstance.DateCreated;
                        //}
                        foreach (StaffMgmntActivity pi in StaffIssueList.First().Value)
                        {
                            if (pi.StaffIssues.DueDate != null)
                            {
                                pi.StaffIssues.DifferenceInHours = DateTime.Now - pi.StaffIssues.DueDate;
                            }
                            else
                            {
                                pi.StaffIssues.DifferenceInHours = TimeSpan.Zero;
                            }
                            pi.ProcessInstance.DifferenceInHours = DateTime.Now - pi.ProcessInstance.DateCreated;
                        }
                        long totalrecords = StaffIssueList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        if (ExptType == "Excel")
                        {
                            var List = StaffIssueList.First().Value.ToList();
                            base.ExptToXL(List, "StaffIssueList", (items => new
                            {
                                items.Id,
                                items.StaffIssues.BranchCode,
                                items.StaffIssues.IssueNumber,
                                items.StaffIssues.IssueGroup,
                                items.StaffIssues.IssueType,
                                items.StaffIssues.Description,
                                CreatedBy = items.StaffIssues.CreatedBy != null ? us.GetUserNameByUserId(items.StaffIssues.CreatedBy) : "",
                                //items.StaffIssues.CreatedBy,
                                CreatedDate = items.StaffIssues.CreatedDate.ToString("dd/MM/yyyy"),
                                ActivityFullName = items.ActivityFullName,
                                items.StaffIssues.Resolution,
                                Resolved_Date = items.ModifiedDate != null ? items.ModifiedDate.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                                DueDate = items.StaffIssues.DueDate != null ? items.StaffIssues.DueDate.Value.ToString("dd/MM/yyyy HH:mm:ss") : ""
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            if (status == "Sent")
                            {
                                var jsondat = new
                                {
                                    total = totalPages,
                                    page = page,
                                    records = totalrecords,
                                    rows = (from items in StaffIssueList.First().Value
                                            select new
                                            {
                                                i = 2,
                                                cell = new string[] {
                                                  items.Id.ToString(),
                                                  items.ProcessRefId.ToString(),
                                                  "<a href='/StaffIssues/ShowStaffIssue?id=" + items.StaffIssues.Id + "'>" + items.StaffIssues.IssueNumber + "</a>",
                                                  items.StaffIssues.BranchCode,
                                                  items.StaffIssues.IssueGroup,
                                                  items.StaffIssues.IssueType,
                                                  ddlSearchBy=="Description"?txtSearch:items.StaffIssues.Description.Substring(0,Math.Min(items.StaffIssues.Description.Length, 20)),
                                                  items.StaffIssues.CreatedBy != null ? us.GetUserNameByUserId(items.StaffIssues.CreatedBy) : "",
                                                  //items.StaffIssues.CreatedBy,
                                                  items.StaffIssues.CreatedDate.ToString("dd/MM/yyyy"),
                                                  items.ActivityFullName,
                                                  "<img src='/Images/History.png ' id='ImgHistory' onclick=\"ShowComments('" + items.ProcessRefId + "');\" />",
                                                  items.StaffIssues.DueDate!=null? items.StaffIssues.DueDate.Value.ToString("dd/MM/yyyy HH:mm:ss"):"",
                                                  items.ProcessInstance.Status=="Completed"?"Completed":items.ProcessInstance.DifferenceInHours.Value.TotalHours.ToString(),
                                                  items.ProcessInstance.Status=="Completed"?"Completed":items.StaffIssues.DifferenceInHours.Value.TotalHours.ToString()
                                                  
                                             }
                                            })
                                };
                                return Json(jsondat, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                string temp = string.Empty;
                                var jsondat = new
                                {
                                    total = totalPages,
                                    page = page,
                                    records = totalrecords,
                                    rows = (from items in StaffIssueList.First().Value
                                            select new
                                            {
                                                i = 2,
                                                cell = new string[] {
                                                items.Id.ToString(),
                                                items.ProcessRefId.ToString(),                                                
                                                //items.StaffIssues.DueDate!=null?"<a href='/StaffIssues/StaffIssueCreation?id=" + items.StaffIssues.Id +"&activityId=" + items.Id + "&activityName=" + items.ActivityName + "&activityFullName=" + items.ActivityFullName +"'>" + items.StaffIssues.IssueNumber + "</a>":"<a href='#' onclick=\"ShowDueDate('" + items.StaffIssues.Id + "');\" >"+items.StaffIssues.IssueNumber+"</a>",
                                                userRoles.Contains("SIMSTAFF")?"<a href='/StaffIssues/StaffIssueCreation?id=" + items.StaffIssues.Id +"&activityId=" + items.Id + "&activityName=" + items.ActivityName + "&activityFullName=" + items.ActivityFullName +"'>" + items.StaffIssues.IssueNumber + "</a>":items.StaffIssues.DueDate!=null && userRoles.Contains("SIMGRL")?"<a href='/StaffIssues/StaffIssueCreation?id=" + items.StaffIssues.Id +"&activityId=" + items.Id + "&activityName=" + items.ActivityName + "&activityFullName=" + items.ActivityFullName +"'>" + items.StaffIssues.IssueNumber + "</a>":"<a href='#' onclick=\"ShowDueDate('" + items.StaffIssues.Id + "');\" >"+items.StaffIssues.IssueNumber+"</a>",
                                                items.StaffIssues.BranchCode,
                                                items.StaffIssues.IssueGroup,
                                                items.StaffIssues.IssueType,
                                                ddlSearchBy=="Description"?txtSearch:items.StaffIssues.Description.Substring(0,Math.Min(items.StaffIssues.Description.Length, 20)),
                                                items.StaffIssues.CreatedBy != null ? us.GetUserNameByUserId(items.StaffIssues.CreatedBy) : "",
                                                //items.StaffIssues.CreatedBy,
                                                items.StaffIssues.CreatedDate.ToString("dd/MM/yyyy"),
                                                items.ActivityFullName,
                                                "<img src='/Images/History.png ' id='ImgHistory' onclick=\"ShowComments('" + items.ProcessRefId + "');\" />",
                                                items.StaffIssues.DueDate!=null? items.StaffIssues.DueDate.Value.ToString("dd/MM/yyyy HH:mm:ss"):"",
                                                items.ProcessInstance.Status=="Completed"?"Completed":items.ProcessInstance.DifferenceInHours.Value.TotalHours.ToString(),
                                                items.ProcessInstance.Status=="Completed"?"Completed":items.StaffIssues.DifferenceInHours.Value.TotalHours.ToString()
                            }
                                            })
                                };
                                return Json(jsondat, JsonRequestBehavior.AllowGet);
                            }
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
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }
        public ActionResult ShowDescription(long Id)
        {
            string data = "";
            StaffIssuesService sis = new StaffIssuesService();
            StaffIssues Si = sis.GetStaffIssuesById(Id);
            data = Si.Description;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult Activities(long Id)
        {
            ViewBag.Id = Id;
            return PartialView();
        }

        public ActionResult LoadUserControl(string id, long? ActivityId)
        {
            string userId = base.ValidateUser();
            if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
            else
            {
                ViewBag.Id = ActivityId;
                return PartialView(id);
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
                    { criteria.Add("ProcessRefId", Id); }
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<StaffMgmntActivity>> ActivitiesList = pfs.GetStaffIssueActivityListWithsearchCriteria(page - 1, rows, sidx, sord, criteria, string.Empty, null, null);
                    if (ActivitiesList != null && ActivitiesList.Count > 0)
                    {
                        UserService us = new UserService();
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
                                     cell = new string[] 
                                 { 
                                     items.Id.ToString(), 
                                     items.ActivityFullName,
                                     items.Available ? "Available" : items.Assigned ? "Assigned" : "Completed",
                                     items.Performer != null ? us.GetUserNameByUserId( items.Performer.ToString()) : "",
                                     items.CreatedDate!=null? items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):null,
                                     items.AppRole,
                                     items.StaffIssues.Id.ToString() }
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
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }
        public ActionResult StaffIssueCreation(long? id, long? activityId, string activityName, string ActivityFullName)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ProcessFlowServices pfs = new ProcessFlowServices();
                    StaffIssuesService sis = new StaffIssuesService();
                    StaffIssues sm = new StaffIssues();
                    if (id > 0)
                    {
                        string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                        pfs.AssignStaffActivity((Convert.ToInt64(activityId)), userid);
                        sm.ActivityId = Convert.ToInt64(activityId);
                        sm = sis.GetStaffIssuesById(Convert.ToInt64(id));
                        sm.CreatedUserName = us.GetUserNameByUserId(sm.CreatedBy);
                    }
                    else
                    {
                        Dictionary<string, object> criteria = new Dictionary<string, object>();
                        criteria.Add("UserId", userId);
                        criteria.Add("AppCode", "SIM");
                        Dictionary<long, IList<UserAppRole>> UserAppRoleList = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                        var UserDetails = (from u in UserAppRoleList.First().Value
                                           where u.UserId == userId && u.BranchCode != null
                                           select new { u.RoleName, u.BranchCode,u.RoleCode }).ToArray();
                        sm.CreatedDate = DateTime.Now;
                        sm.CreatedBy = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                        sm.Status = "LogIssue";
                        sm.ActivityFullName = "Log Issue";
                        if (UserDetails != null)
                            sm.UserRoleName = UserDetails[0].RoleName != null ? UserDetails[0].RoleName : "";
                        // sm.BranchCode = UserDetails[0].BranchCode;
                        User user = (User)Session["objUser"];
                        if (user != null)
                        {
                            sm.BranchCode = user.Campus;
                            for (int i = 0; i < UserDetails.Length; i++)
                            {
                                if (UserDetails[i].RoleCode == "SICTR")
                                {
                                    sm.BranchCode = null;
                                }
                            }
                            sm.CreatedUserName = user.UserName;
                        }
                        return View(sm);
                    }
                    return View(sm);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult StaffIssueCreation(StaffIssues sm, HttpPostedFileBase file1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (ModelState.IsValid)
                    {
                        ProcessFlowServices pfs = new ProcessFlowServices();
                        StaffIssuesService sis = new StaffIssuesService();
                        long id = 0;
                        if (sm != null && !string.IsNullOrWhiteSpace(sm.IssueGroup))
                        {
                            sm.DeptCode = sm.IssueGroup.ToUpper();
                        }

                        UserService us = new UserService();
                        Dictionary<string, object> Criteria = new Dictionary<string, object>();
                        Criteria.Add("AppCode", "SIM");
                        Dictionary<long, IList<UserAppRole_Vw>> UserAppRoleList = us.GetAppRoleOnlyActiveUsersPagingAndCriteria(0, 9999, string.Empty, string.Empty, Criteria);
                        //Dictionary<long, IList<UserAppRole>> UserAppRoleList = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, Criteria);
                        //string[] Email = (from u in UserAppRoleList.First().Value
                        //                  where u.AppCode == "SIM" && u.RoleCode == "STAFF" && u.UserId == sm.CreatedBy
                        //                  select u.Email).Distinct().ToArray();
                        string[] IssuerUserId = (from u in UserAppRoleList.First().Value
                                                 where u.AppCode == "SIM" && u.RoleCode == "STAFF" && u.UserId == sm.CreatedBy
                                                 select u.UserId).Distinct().ToArray();
                        Criteria.Clear();
                        StaffManagementService sms = new StaffManagementService();
                        Dictionary<long, IList<StaffDetails>> IssuerStaffdetails;
                        if (IssuerUserId.Length > 0) { Criteria.Add("StaffUserName", IssuerUserId); }
                        IssuerStaffdetails = sms.GetStaffDetailsListWithPaging(0, 10, string.Empty, string.Empty, Criteria);

                        string[] Email = (from u in IssuerStaffdetails.FirstOrDefault().Value
                                          select u.EmailId).Distinct().ToArray();
                        sm.CreatedUserName = us.GetUserNameByUserId(sm.CreatedBy);
                        //Modified By Gobi for Ticket-261
                        //Commented By Prabakaran
                        //string[] ResolverEmail = (from u in UserAppRoleList.First().Value
                        //                          where u.AppCode == "SIM" && u.RoleCode == "GRL" && u.DeptCode == sm.DeptCode && u.BranchCode == sm.BranchCode
                        //                          select u.Email).Distinct().ToArray();
                        string[] ResolverEmail = new string[] { };
                        if (sm.DeptCode == "IT")
                        {
                            ResolverEmail = (from u in UserAppRoleList.First().Value
                                             where u.AppCode == "SIM" && u.RoleCode == "GRL-HEAD" && u.DeptCode == sm.DeptCode
                                             //&& u.BranchCode == sm.BranchCode
                                             select u.Email).Distinct().ToArray();
                        }

                        else
                        {
                            ResolverEmail = (from u in UserAppRoleList.First().Value
                                             where u.AppCode == "SIM" && u.RoleCode == "GRL" && u.DeptCode == sm.DeptCode && u.BranchCode == sm.BranchCode
                                             select u.Email).Distinct().ToArray();
                        }

                        //string[] ResolverUserId = (from u in UserAppRoleList.First().Value
                        //                           where u.AppCode == "SIM" && u.RoleCode == "GRL" && u.DeptCode == sm.DeptCode
                        //                           select u.UserId).Distinct().ToArray();
                        //Criteria.Clear();
                        //Dictionary<long, IList<StaffDetails>> ResolverStaffdetails;
                        //if (ResolverUserId.Length > 0) { Criteria.Add("StaffUserName", ResolverUserId); }
                        //ResolverStaffdetails = sms.GetStaffDetailsListWithPaging(0, 10, string.Empty, string.Empty, Criteria);

                        //string[] ResolverEmail = (from u in ResolverStaffdetails.FirstOrDefault().Value
                        //                          select u.EmailId).Distinct().ToArray();

                        string LogIssuerMailId = Email != null && Email.Length > 0 ? Email[0].ToString() : string.Empty;
                        string ResolverMailId = ResolverEmail != null && ResolverEmail.Length > 0 ? ResolverEmail[0].ToString() : string.Empty;
                        string RejComments = Request["txtRejCommentsArea"];
                        string ResolutionComments = sm.Resolution;
                        //
                        if (Request.Form["btnSave"] == "Save")
                        {
                            if (sm.Id == 0)
                            {
                                id = pfs.StartStaffIssueManagement(sm, "StaffIssues", userId);
                                ViewBag.flag = 1;
                                sm.IssueNumber = "SIMGMT-" + id.ToString();
                                sm.CreatedDate = sm.CreatedDate;
                                sm.CreatedBy = userId;
                                sis.CreateOrUpdateStaffIssues(sm);
                                return RedirectToAction("StaffIssueCreation", new { id = sm.Id, activityId = sm.ActivityId });
                                //TempData["SuccessIssueCreation"] = sm.IssueNumber;
                                //return View(sm);
                            }
                            else
                            {
                            }
                        }
                        if (sm != null)
                        {
                            if (Request.Form["btnCompleteLogIssue"] == "CompleteLogIssue" || Request.Form["btnCompleteLogIssue"] == "Save and Submit")
                            {
                                if (sm.Id == 0)
                                {
                                    id = pfs.StartStaffIssueManagement(sm, "StaffIssues", userId);
                                    ViewBag.flag = 1;
                                    sm.IssueNumber = "SIMGMT-" + id.ToString();
                                    sm.CreatedDate = sm.CreatedDate;
                                    sm.CreatedBy = userId;
                                    sis.CreateOrUpdateStaffIssues(sm);
                                    //write the complete activity logic
                                    // sm.Performer = userId;
                                    pfs.CompleteActivityStaffIssueManagement(sm, "StaffIssues", userId, "LogIssue", false);
                                    TempData["SuccessIssueCreation"] = sm.IssueNumber;
                                    SendEmail(sm, LogIssuerMailId, ResolverEmail, sm.IssueNumber, "", sm.IssueGroup, "", RejComments, sm.Status, "", ResolutionComments, "");
                                    ActorsEmail(sm, LogIssuerMailId, ResolverEmail, sm.IssueNumber, "", "", sm.IssueGroup, "", RejComments, sm.Status, "", ResolutionComments, "");
                                    return RedirectToAction("StaffIssueManagement");
                                }
                                else
                                {
                                    //sis.CreateOrUpdateStaffIssues(sm);
                                    sm.CreatedDate = DateTime.Now;
                                    sis.CreateOrUpdateStaffIssues(sm);
                                    if (sm.InstanceId > 0 && sm.Id > 0)//Added By Prabakaran
                                    {
                                        StaffActivities staffactivitiesdetails = pfs.GetStaffActivities(sm.InstanceId, sm.Id);
                                        staffactivitiesdetails.CreatedDate = DateTime.Now;
                                        if (staffactivitiesdetails != null)
                                        {
                                            pfs.CreateOrUpdateStaffActivities(staffactivitiesdetails);
                                        }
                                    } 
                                    pfs.CompleteActivityStaffIssueManagement(sm, "StaffIssues", userId, "LogIssue", false);
                                    TempData["SuccessIssueCreation"] = sm.IssueNumber;
                                    SendEmail(sm, LogIssuerMailId, ResolverEmail, sm.IssueNumber, "", "", sm.IssueGroup, "", RejComments, sm.Status, "", ResolutionComments);
                                    ActorsEmail(sm, LogIssuerMailId, ResolverEmail, sm.IssueNumber, "", "", sm.IssueGroup, "", RejComments, sm.Status, "", ResolutionComments, "");
                                    return RedirectToAction("StaffIssueManagement");
                                }
                            }
                        }
                        if (Request.Form["DocUpload"] == "Upload")
                        {
                            DocumentsService ds = new DocumentsService();
                            string path = file1.InputStream.ToString();
                            byte[] imageSize = new byte[file1.ContentLength];
                            file1.InputStream.Read(imageSize, 0, (int)file1.ContentLength);
                            Documents d = new Documents();
                            d.EntityRefId = sm.Id;
                            d.FileName = file1.FileName;
                            d.UploadedOn = DateTime.Now;
                            d.UploadedBy = Session["UserId"].ToString();
                            d.DocumentData = imageSize;
                            d.DocumentSize = file1.ContentLength.ToString();
                            d.AppName = "SIM";
                            ds.CreateOrUpdateDocuments(d);
                            ViewBag.flag = 1;
                            ViewBag.FileUploaded = 1;
                            return View(sm);
                        }

                        //reject issue logic
                        if (Request.Form["btnRejectIssue"] == "Reject Issue" || Request.Form["btnRejectIssue"] == "More Info Required")
                        {
                            CommentsService cmntsSrvc = new CommentsService();
                            Comments cmntsObj = new Comments();
                            cmntsObj.EntityRefId = sm.Id;

                            cmntsObj.CommentedBy = userId;
                            cmntsObj.CommentedOn = DateTime.Now;
                            cmntsObj.RejectionComments = Request["txtRejCommentsArea"];
                            cmntsObj.AppName = "SIM";
                            cmntsSrvc.CreateOrUpdateComments(cmntsObj);
                            // sm.Performer = userId;
                            //write the complete activity logic
                            StaffIssues staffissues = sis.GetStaffIssuesById(sm.Id);
                            sm.DueDate = staffissues.DueDate;
                            sm.AssignedDate = staffissues.AssignedDate;
                            pfs.CompleteActivityStaffIssueManagement(sm, "StaffIssues", userId, sm.Status, true);
                            ActorsEmail(sm, LogIssuerMailId, ResolverEmail, sm.IssueNumber, "", "", sm.IssueGroup, "", RejComments, sm.Status, "", ResolutionComments, "");
                            return RedirectToAction("StaffIssueManagement");
                        }

                        if (Request.Form["btnReplyIssue"] == "Reply Issue" || Request.Form["btnReplyIssue"] == "Reply")
                        {
                            CommentsService cmntsSrvc1 = new CommentsService();
                            Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                            criteria1.Add("EntityRefId", sm.Id);
                            criteria1.Add("AppName", "SIM");

                            Dictionary<long, IList<Comments>> list = cmntsSrvc1.GetCommentsListWithPaging(0, 1000, string.Empty, string.Empty, criteria1);
                            if (list != null && list.Count > 0)
                            {
                                foreach (Comments cm in list.First().Value)
                                {
                                    if (string.IsNullOrWhiteSpace(cm.ResolutionComments))
                                    {
                                        cm.ResolutionComments = Request["txtRejCommentsArea"];
                                        cmntsSrvc1.CreateOrUpdateComments(cm);
                                        break;
                                    }
                                }
                            }
                            //write the complete activity logic
                            //cm1.Performer = userId;
                            StaffIssues staffissues = sis.GetStaffIssuesById(sm.Id);
                            sm.DueDate = staffissues.DueDate;
                            sm.AssignedDate = staffissues.AssignedDate;
                            pfs.CompleteActivityStaffIssueManagement(sm, "StaffIssues", userId, sm.Status, false);
                            ActorsEmail(sm, LogIssuerMailId, ResolverEmail, sm.IssueNumber, "", "", sm.IssueGroup, "", RejComments, sm.Status, "", ResolutionComments, "");
                            return RedirectToAction("StaffIssueManagement");
                        }
                        //Resolve issue logic
                        if (Request.Form["btnResolveIssue"] == "Resolve Issue")
                        {
                            //write the complete activity logic
                            //cm1.Performer = userId;
                            StaffIssues staffissues = sis.GetStaffIssuesById(sm.Id);
                            sm.DueDate = staffissues.DueDate;
                            sm.AssignedDate = staffissues.AssignedDate;
                            pfs.CompleteActivityStaffIssueManagement(sm, "StaffIssues", userId, sm.Status, false);
                            ActorsEmail(sm, LogIssuerMailId, ResolverEmail, sm.IssueNumber, "", "", sm.IssueGroup, "", RejComments, sm.Status, "", ResolutionComments, "");
                            pfs.CompleteActivityStaffIssueManagement(sm, "StaffIssues", sm.CreatedBy, "Complete", false);
                            return RedirectToAction("StaffIssueManagement");
                        }
                        //Complete logic
                        //if (Request.Form["btnIdComplete"] == "Complete")
                        //{
                        //    StaffIssues staffissues = sis.GetStaffIssuesById(sm.Id);
                        //    sm.DueDate = staffissues.DueDate;
                        //    pfs.CompleteActivityStaffIssueManagement(sm, "StaffIssues", userId, "Complete", false);
                        //    SendEmail(sm, LogIssuerMailId, ResolverEmail, sm.IssueNumber, "", sm.IssueGroup, "", RejComments, sm.Status, "", ResolutionComments, "");
                        //    // ActorsEmail(sm, LogIssuerMailId, ResolverEmail, sm.IssueNumber, "", "", sm.IssueGroup, "", RejComments, sm.Status, "", ResolutionComments, "");
                        //    return RedirectToAction("StaffIssueManagement");
                        //}
                    }
                    else
                    {
                        return View(sm);
                    }
                    return View(sm);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }
        public ActionResult ShowStaffIssue(long id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StaffIssuesService sis = new StaffIssuesService(); // TODO: Initialize to an appropriate value
                    ProcessFlowServices pfs = new ProcessFlowServices();
                    StaffIssues sm = sis.GetStaffIssuesById(Convert.ToInt64(id));
                    UserService us = new UserService();
                    sm.CreatedUserName = us.GetUserNameByUserId(sm.CreatedBy);
                    //Added by Thamizhmani
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    return View(sm);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }

        public ActionResult DescriptionForSelectedIdJqGrid(long? Id, int rows, string sidx, string sord, int? page = 1)
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
                        criteria.Add("AppName", "SIM");
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
                                     cell = new string[] { items.CommentedBy, items.CommentedOn.ToString("dd'/'MM'/'yyyy"), items.RejectionComments, items.ResolutionComments }
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
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }

        public JsonResult StaffDocumentsjqgrid(long Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                DocumentsService ds = new DocumentsService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("EntityRefId", Id);
                criteria.Add("AppName", "SIM");

                Dictionary<long, IList<Documents>> UploadedFiles = ds.GetDocumentsListWithPaging(page - 1, rows, sidx, sord, criteria);
                if (UploadedFiles.Values != null && UploadedFiles.First().Value.Count > 0)
                {
                    long totalrecords = UploadedFiles.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in UploadedFiles.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                               items.UploadedBy,
                               items.UploadedOn.ToString(),
                               String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '#' onclick = 'uploaddat("+"\"" + items.Upload_Id + "\"" + ")' >{0}</a>",items.FileName),
                            }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }

        public ActionResult uploaddisplay(long Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    DocumentsService ds = new DocumentsService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("Upload_Id", Id);
                    criteria.Add("AppName", "SIM");

                    Dictionary<long, IList<Documents>> UploadedFiles = ds.GetDocumentsListWithPaging(0, 1000, string.Empty, string.Empty, criteria);

                    if (UploadedFiles != null && UploadedFiles.FirstOrDefault().Value != null)
                    {
                        IList<Documents> list = UploadedFiles.FirstOrDefault().Value;
                        Documents doc = list.FirstOrDefault();
                        if (doc.DocumentData != null)
                        {
                            int startIndx = Convert.ToInt32(doc.FileName.LastIndexOf(".").ToString());
                            int FileLength = Convert.ToInt32(doc.FileName.Length);
                            string fileExtn = doc.FileName.Substring(startIndx, (FileLength - startIndx));
                            return File(doc.DocumentData, GetContentTypeByFileExtension(fileExtn), doc.FileName);
                        }
                        else
                        {
                            var dir = Server.MapPath("/Images");
                            string ImagePath = ConfigurationManager.AppSettings["ImageFilePath"];
                            return File(ImagePath, "image/jpg");
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
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }

        public JsonResult FillStaffIssueGroup()
        {
            try
            {
                MasterDataService mds = new MasterDataService();
                Dictionary<long, IList<StaffIssueGroupMaster>> IssueGroupList = mds.GetStaffIssueGroupListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, null);
                var IssueGroup1 = (
                         from items in IssueGroupList.First().Value
                         select new
                         {
                             Text = items.IssueGroup,
                             Value = items.IssueGroup
                         }).ToList();
                return Json(IssueGroup1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }
        public ActionResult FillStaffIssueType(string IssueGroup)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MasterDataService mds = new MasterDataService();
                    IList<StaffIssueTypeMaster> IssueTypeList = mds.GetStaffIssueTypeById(IssueGroup);
                    var IssueType = new
                    {
                        rows = (
                             from items in IssueTypeList

                             select new
                             {
                                 Text = items.IssueType,
                                 Value = items.IssueType
                             }).ToArray(),
                    };

                    return Json(IssueType, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }

        public void SendEmail(StaffIssues si, string LogIssuerMailId, string[] ResolverEmail, string IssueNumber, string StudentName, string IssueGroup, string CallNumber, string RejComments, string Status, string ApproverMailId, string ResolutionComments, string LeaveNotification)
        {
            try
            {
                string SendEmail = ConfigurationManager.AppSettings["SendEmail1"];
                string From = ConfigurationManager.AppSettings["From"];
                if (SendEmail == "false")
                    return;
                else
                {
                    try
                    {
                        IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(si.BranchCode, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                        string Body = string.Empty;
                        if (si.Status == "ResolveIssue")
                        {
                            mail.To.Add(LogIssuerMailId);
                            mail.Subject = "Tips support request #" + IssueNumber + " - Logged";
                            Body = "Dear Sir/Madam, <br/><br/>" +
                                "Tips support request #" + IssueNumber + " - Logged<br/><br/>" +
                                 "<b>Issue Description:</b><br/>" +
                                       "" + si.Description + "<br/><br/>" +
                        " Thank you for contacting the support desk, your call is registered and find the mentioned call number and please quote this for any further correspondences.<br/>" +
                        " You will be updated shortly on the action items on the same by the concerned persons. <br/><br/>" +
                        " If you don’t receive a mail from us within 48 hours, please mail us at " + campusemaildet.First().EmailId.ToString() + " to know the status of your request.<br/><br/>";
                        }

                        if (si.Status == "Completed")
                        {
                            mail.To.Add(LogIssuerMailId);
                            for (int i = 0; i < ResolverEmail.Length; i++)
                            {
                                if (ResolverEmail[i] != "")
                                    mail.To.Add(ResolverEmail[i]);
                            }
                            mail.Subject = "Tips support request #" + IssueNumber + " - is completed";
                            Body = "Dear Sir/Madam,<br/><br/>" +
                           "Tips support request #" + IssueNumber + " is completed<br/><br/>" +
                           "<b>Issue Description</b><br/><br/>" +
                                        si.Description + ".<br/><br/>" +
                                // " Thank you for contacting our support desk and your issue is resolved with the below comments.<br/><br/>" +
                           "<b>Resolution Description</b><br/><br/>" +
                           si.Resolution + ". " + "<br/><br/>";

                        }
                        mail.Body = Body;
                        SendEmailWithEmailTemplate(mail, si.BranchCode, GetGeneralBodyofMail());
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }

        public void ActorsEmail(StaffIssues si, string LogIssuerMailId, string[] ResolverEmail, string IssueNumber, string InformationFor, string StudentName, string IssueGroup, string CallNumber, string RejComments, string Status, string ApproverMailId, string ResolutionComments, string LeaveNotification)
        {
            try
            {
                string SendEmail = ConfigurationManager.AppSettings["SendEmail1"];
                string From = ConfigurationManager.AppSettings["From"];
                if (SendEmail == "false")
                    return;
                else
                {
                    try
                    {
                        IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(si.BranchCode, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                        string Body = string.Empty;
                        if (Status == "Assigned")
                        {
                            for (int i = 0; i < ResolverEmail.Length; i++)
                            {
                                if (ResolverEmail[i] != "")
                                    mail.To.Add(ResolverEmail[i]);
                            }
                            mail.Subject = "Tips support request #" + IssueNumber + " needs your action ";
                            Body = "Dear Sir/Madam, <br/><br/>" +
                                       "Tips support request #" + IssueNumber + " needs your action <br/><br/>" +
                                        "<b>Issue Description:</b><br/>" +
                                       "" + si.Description + "<br/><br/>" +
                                       "The issue " + IssueNumber + "  is assigned for your closing and please resolve this issue asap.<br/><br/>" + "<b>Issue Assigend By:</b><br/>" + " " + InformationFor + "<br/>";
                        }
                        if (Status == "ResolveIssue")
                        {
                            for (int i = 0; i < ResolverEmail.Length; i++)
                            {
                                if (ResolverEmail[i] != "")
                                    mail.To.Add(ResolverEmail[i]);
                            }
                            mail.Subject = "Tips support request #" + IssueNumber + " needs your action ";
                            Body = "Dear Sir/Madam, <br/><br/>" +
                                       "Tips support request #" + IssueNumber + " needs your action <br/><br/>" +
                                        "<b>Issue Description:</b><br/>" +
                                       "" + si.Description + "<br/><br/>" +
                                       "The issue " + IssueNumber + "  is assigned for your closing and please resolve this issue asap.<br/><br/>";
                        }
                        else if (Status == "ResolveIssueRejection")
                        {
                            mail.To.Add(LogIssuerMailId);
                            mail.Subject = "Tips support request #" + IssueNumber + " - Need more information";

                            Body = "Dear Sir/Madam, <br/><br/>" +
                               "Tips support request #" + IssueNumber + " - Need more information<br/><br/>" +
                            "Please find below the information needed by the concerned staff for the issue you had raised. <br/>" +
                            "<b>Issue Description:</b><br/>" +
                            "" + si.Description + "<br/><br/>" +
                            "<b>Rejection Comments:</b><br/>"
                            + RejComments + ". " + "<br/><br/>" +
                           " In-case if you need further information please mail to support desk.<br/><br/>";
                        }

                        if (Status == "Complete")
                        {
                            mail.To.Add(LogIssuerMailId);
                            mail.Subject = "Tips support request #" + IssueNumber + " is resolved ";
                            //Body = "Dear Sir/Madam, <br/><br/>" +
                            //               "The issue number #" + IssueNumber + " is resolved .<br/><br/>" +
                            //                "<b>Issue Description:</b><br/>" +
                            //                "" + si.Description + "<br/><br/>" +
                            //                 "<b>Resolution Description:</b><br/>" +
                            //                "" + si.Resolution + "<br/><br/>";
                            Body = "Dear Sir/Madam, <br/><br/>" +
                                           "The issue number #" + IssueNumber + " is resolved and also completed.If any inconvenience, please reraise the request.<br/><br/>" +
                                            "<b>Issue Description:</b><br/>" +
                                            "" + si.Description + "<br/><br/>" +
                                             "<b>Resolution Description:</b><br/>" +
                                            "" + si.Resolution + "<br/><br/>";
                        }
                        if (Status == "ReOpenIssue")
                        {
                            for (int i = 0; i < ResolverEmail.Length; i++)
                            {
                                if (ResolverEmail[i] != "")
                                    mail.To.Add(ResolverEmail[i]);
                            }
                            mail.Subject = "Tips support request #" + IssueNumber + " reopen and needs your action ";
                            Body = "Dear Sir/Madam, <br/><br/>" +
                                       "Tips support request #" + IssueNumber + " reopen and needs your action <br/><br/>" +
                                        "<b>Issue Description:</b><br/>" + si.Description + "<br/><br/>" +
                                        "<b>ReOpen Description:</b><br/>" + RejComments + "<br/><br/>" +
                                       "The issue " + IssueNumber + "  is reassigned for your closing and please resolve this issue asap.<br/><br/>";
                        }
                        mail.Body = Body;
                        SendEmailWithEmailTemplate(mail, si.BranchCode, GetGeneralBodyofMail());
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;

            }
        }

        public void emaillogloop(string From, string To, MailMessage mail, EmailLog el)
        {
            AdmissionManagementService ads = new AdmissionManagementService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            el.EmailCC = mail.CC.ToString();
            if (mail.Bcc.ToString().Length < 3990)
            {
                el.EmailBCC = mail.Bcc.ToString();
            }
            el.Subject = mail.Subject.ToString();
            if (mail.Body.ToString().Length < 3990)
            {
                el.Message = mail.Body;
            }
            el.EmailDateTime = DateTime.Now.ToString();
            el.BCC_Count = mail.Bcc.Count;
            ads.CreateOrUpdateEmailLog(el);
            mail.To.Clear();
        }

        public ActionResult BulkIssueComplete(long[] ProcessRefId)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    bool BulkComplete = false;
                    StaffIssuesService sis = new StaffIssuesService();
                    ProcessFlowServices pfs = new ProcessFlowServices();
                    for (int i = 0; i < ProcessRefId.Length; i++)
                    {
                        StaffIssues Si = sis.GetStaffIssuesById(ProcessRefId[i]);
                        pfs.CompleteActivityStaffIssueManagement(Si, "StaffIssues", userId, Si.Status, false);
                        BulkComplete = true;
                    }
                    return Json(BulkComplete, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;

            }
        }

        public ActionResult StaffIssueStatusReport()
        {
            string userId = base.ValidateUser();
            if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
            else
            {
                return View();
            }
        }

        public ActionResult FillWorkFlowStatus()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ProcessFlowServices ps = new ProcessFlowServices();
                    Dictionary<long, IList<WorkFlowStatus>> workflowstatus = ps.GetWorkFlowStatusListWithsearchCriteria(0, 50, string.Empty, string.Empty, null);
                    var workflowsta = (
                             from items in workflowstatus.First().Value
                             where items.TemplateId == 2
                             select new
                             {
                                 Text = items.WFStatus,
                                 Value = items.WFStatus
                             }).Distinct().ToList();
                    return Json(workflowsta, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }

        public JsonResult StaffIssueStatusjqgrid(string id, string txtSearch, string ddlcampus, int rows, string sidx, string sord, int? page = 1)
        {
            string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
            try
            {
                ProcessFlowServices pfs = new ProcessFlowServices();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrWhiteSpace(txtSearch))
                {
                    if (txtSearch.Contains("Select"))
                    {
                    }
                    else
                    {
                        criteria.Add("Status", txtSearch == "Complete" ? "Completed" : txtSearch);
                    }
                }
                if (!string.IsNullOrWhiteSpace(ddlcampus))
                {
                    criteria.Add("BranchCode", ddlcampus);
                }

                sord = sord == "desc" ? "Desc" : "Asc";
                UserService us = new UserService();
                Dictionary<long, IList<UserAppRole>> userAppRole = new Dictionary<long, IList<UserAppRole>>();
                Dictionary<string, object> criteriaUserAppRole = new Dictionary<string, object>();
                criteriaUserAppRole.Add("UserId", userid);

                userAppRole = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteriaUserAppRole);

                if (string.IsNullOrWhiteSpace(ddlcampus))
                {
                    string[] brcode = (from u in userAppRole.First().Value
                                       where u.UserId == userid && u.BranchCode != null
                                       select u.BranchCode).Distinct().ToArray();
                    criteria.Add("BranchCode", brcode);
                }

                //criteria.Add("CreatedBy", userid);

                Dictionary<long, IList<StaffIssues>> processinstance = pfs.GetStaffViewListWithsearchCriteria(page - 1, rows, sord, sidx, criteria);
                if (processinstance != null && processinstance.First().Value != null)
                {
                    foreach (StaffIssues pi in processinstance.First().Value)
                    {
                        pi.DifferenceInHours = DateTime.Now - pi.CreatedDate;
                    }
                }
                string[] datecreated = new string[100];
                var date = (from items in processinstance.First().Value select items.CreatedDate);

                if (processinstance != null && processinstance.Count > 0)
                {
                    long totalrecords = processinstance.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in processinstance.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                               items.Id.ToString(),
                               "<a href='/StaffIssues/ShowStaffIssue?id=" + items.Id+"'>" + items.IssueNumber + "</a>",
                               items.BranchCode,
                               items.CreatedDate.ToString("dd/MM/yyyy"),
                               items.IssueGroup,
                               items.IssueType,
                               items.CreatedBy != null ? us.GetUserNameByUserId(items.CreatedBy) : "",
                               //items.CreatedBy,
                               items.Status,
                                "<img src='/Images/History.png ' id='ImgHistory' onclick=\"ShowComments('" + items.Id + "');\" />",
                               items.Status=="Completed"? "Completed": items.DifferenceInHours.Value.TotalHours.ToString(),
                            }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }

        public ActionResult StaffIssueDashboard()
        {
            string userId = base.ValidateUser();
            if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
            else
            {
                return View();
            }
        }

        public ActionResult NonCompletedSlaStatusChart(string cam)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StaffIssuesService sis = new StaffIssuesService();
                    Dictionary<string, object> IssCount = new Dictionary<string, object>();
                    IssCount.Add("BranchCode", cam);
                    Dictionary<long, IList<StaffIssues>> StaffManagementList = sis.GetStaffIssueManagementListWithPaging(null, 9999, string.Empty, string.Empty, IssCount);

                    if (StaffManagementList != null && StaffManagementList.Count > 0)
                    {
                        var IssueList = (from u in StaffManagementList.First().Value
                                         where u.Status != "Completed" && u.IssueGroup != null && u.IssueGroup != ""
                                         select u).ToList();
                        IList<StaffIssues> diffinhours1 = new List<StaffIssues>();
                        foreach (StaffIssues sm in IssueList)
                        {
                            sm.DifferenceInHours = DateTime.Now - sm.CreatedDate;
                            sm.Hours = Convert.ToInt16(sm.DifferenceInHours.Value.TotalHours);
                            diffinhours1.Add(sm);
                        }
                        return Json(diffinhours1.ToList(), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }

        public ActionResult CompletedSlaStatusChart(string cam)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StaffIssuesService sis = new StaffIssuesService();
                    Dictionary<string, object> IssCount = new Dictionary<string, object>();
                    IssCount.Add("BranchCode", cam);
                    Dictionary<long, IList<StaffIssues>> StaffManagementList = sis.GetStaffIssueManagementListWithPaging(null, 9999, string.Empty, string.Empty, IssCount);

                    if (StaffManagementList != null && StaffManagementList.Count > 0)
                    {
                        var IssueList = (from u in StaffManagementList.First().Value
                                         where u.Status == "Completed" && u.IssueGroup != null && u.IssueGroup != ""
                                         select u).ToList();
                        IList<StaffIssues> diffinhours1 = new List<StaffIssues>();
                        foreach (StaffIssues sm in IssueList)
                        {
                            sm.DifferenceInHours = DateTime.Now - sm.CreatedDate;
                            sm.Hours = Convert.ToInt16(sm.DifferenceInHours.Value.TotalHours);
                            diffinhours1.Add(sm);
                        }
                        return Json(diffinhours1.ToList(), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }

        public ActionResult GetIssueCountByIssueGroup(string cam)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StaffIssuesService sis = new StaffIssuesService();
                    Dictionary<string, object> IssCount = new Dictionary<string, object>();
                    IssCount.Add("BranchCode", cam);
                    Dictionary<long, IList<StaffIssues>> GetIssueCountByIssueGroup = sis.GetStaffIssueManagementListWithPaging(null, 9999, string.Empty, string.Empty, IssCount);
                    var IssueCount = (from u in GetIssueCountByIssueGroup.First().Value
                                      select u).ToList();
                    return Json(IssueCount, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }

        public ActionResult GetIssueCountByStatus(string cam)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StaffIssuesService sis = new StaffIssuesService();
                    Dictionary<string, object> IssCount = new Dictionary<string, object>();
                    IssCount.Add("BranchCode", cam);
                    Dictionary<long, IList<StaffIssues>> GetIssueCountByStatus = sis.GetStaffIssueManagementListWithPaging(null, 9999, string.Empty, string.Empty, IssCount);
                    var IssueCount = (from u in GetIssueCountByStatus.First().Value
                                      select u).ToList();
                    return Json(IssueCount, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }

        public ActionResult StaffIssueList(string status, string IssueGroup, string InformationFor, string IsHosteller, string NonCompletedSLA, string CompletedSLA, string cam)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ViewBag.status = status;
                    ViewBag.IssueGroup = IssueGroup;
                    ViewBag.InformationFor = InformationFor;
                    ViewBag.IsHosteller = IsHosteller;
                    ViewBag.NonCompletedSLA = NonCompletedSLA;
                    ViewBag.CompletedSLA = CompletedSLA;
                    ViewBag.Campus = cam;
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }
        public ActionResult StaffIssueListJqGrid(string status, string IssueGroup, string InformationFor, string IsHosteller, string NonCompletedSLA, string CompletedSLA, string Campus, int expxl, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StaffIssuesService sis = new StaffIssuesService();
                    Dictionary<string, object> Criteria = new Dictionary<string, object>();
                    Criteria.Add("BranchCode", Campus);

                    Dictionary<long, IList<StaffIssues>> GetIssueCount = sis.GetStaffIssueManagementListWithPaging(page - 1, 9999, sidx, sord, Criteria);
                    var IssueCount = new List<StaffIssues>();
                    if (!string.IsNullOrEmpty(status))
                    {
                        if (status == "LogIssue")
                        {
                            IssueCount = (from u in GetIssueCount.First().Value
                                          where (u.Status == status || u.Status == "ResolveIssueRejection")
                                          select u).ToList();
                        }
                        else if (status == "ResolveIssue")
                        {
                            IssueCount = (from u in GetIssueCount.First().Value
                                          where (u.Status == status || u.Status == "ApproveIssueRejection")
                                          select u).ToList();
                        }
                        else if (status == "ApproveIssue")
                        {
                            IssueCount = (from u in GetIssueCount.First().Value
                                          where (u.Status == "Complete")
                                          select u).ToList();
                        }
                        else
                        {
                            IssueCount = (from u in GetIssueCount.First().Value
                                          where u.Status == status
                                          select u).ToList();
                        }
                    }

                    else if (!string.IsNullOrEmpty(IssueGroup))
                    {
                        IssueCount = (from u in GetIssueCount.First().Value
                                      where u.IssueGroup == IssueGroup
                                      select u).ToList();
                    }


                    else if (!string.IsNullOrEmpty(NonCompletedSLA))
                    {
                        if (NonCompletedSLA == "below24")
                        {
                            IssueCount = (from u in GetIssueCount.First().Value
                                          where u.Status != "Completed" && DateTime.Now.Subtract(u.CreatedDate).TotalHours < 24
                                          select u).ToList();
                        }
                        else if (NonCompletedSLA == "24-48")
                        {
                            IssueCount = (from u in GetIssueCount.First().Value
                                          where u.Status != "Completed" && (DateTime.Now.Subtract(u.CreatedDate).TotalHours > 24 && DateTime.Now.Subtract(u.CreatedDate).TotalHours < 48)
                                          select u).ToList();
                        }
                        else
                        {
                            IssueCount = (from u in GetIssueCount.First().Value
                                          where u.Status != "Completed" && DateTime.Now.Subtract(u.CreatedDate).TotalHours > 48
                                          select u).ToList();
                        }
                    }
                    else if (!string.IsNullOrEmpty(CompletedSLA))
                    {
                        if (CompletedSLA == "below24")
                        {
                            IssueCount = (from u in GetIssueCount.First().Value
                                          where u.Status == "Completed" && DateTime.Now.Subtract(u.CreatedDate).TotalHours < 24
                                          select u).ToList();
                        }
                        else if (CompletedSLA == "24-48")
                        {
                            IssueCount = (from u in GetIssueCount.First().Value
                                          where u.Status == "Completed" && (DateTime.Now.Subtract(u.CreatedDate).TotalHours > 24 && DateTime.Now.Subtract(u.CreatedDate).TotalHours < 48)
                                          select u).ToList();
                        }
                        else
                        {
                            IssueCount = (from u in GetIssueCount.First().Value
                                          where u.Status == "Completed" && DateTime.Now.Subtract(u.CreatedDate).TotalHours > 48
                                          select u).ToList();
                        }
                    }

                    if (expxl == 1)
                    {
                        var IssueList = IssueCount.ToList();
                        base.ExptToXL(IssueList, "StaffIssueList", (item => new
                        {
                            item.IssueNumber,
                            item.IssueGroup,
                            item.IssueType,
                            item.CreatedBy,
                            item.CreatedDate,
                            item.ActivityFullName,
                        }));
                        return new EmptyResult();
                    }

                    if (IssueCount != null && IssueCount.Count > 0)
                    {
                        UserService us = new UserService();
                        long totalRecords = IssueCount.Count;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows = (
                                 from items in IssueCount
                                 select new
                                 {
                                     i = items.Id,
                                     cell = new string[] 
                                     {   items.Id.ToString(),
                                       "<a href='/Staff/ShowStaffIssue?id=" + items.Id + "&activityId=" + items.Id + "&activityName=" + items.ActivityFullName + "&status=" + status +"&activityFullName=" + items.ActivityFullName + "'>" + items.IssueNumber + "</a>",
                                       items.IssueGroup,
                                       items.IssueType,
                                       items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
                                       items.CreatedDate!=null?items.CreatedDate.ToString("dd/MM/yyyy"):null,
                                       items.ActivityFullName, 
                                       items.Resolution,
                                       items.Id.ToString(),
                                       items.CreatedBy
                                     }
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
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }

        public ActionResult StaffWeeklyIssueStatus(string cam)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    DateTime FromDate;
                    DateTime ToDate;
                    DayOfWeek day = DateTime.Now.DayOfWeek;
                    int dayofWeek = Convert.ToInt16(DateTime.Now.DayOfWeek);
                    int dayindex = day - DayOfWeek.Monday;
                    FromDate = DateTime.Today.AddDays(-7);
                    ToDate = FromDate.AddDays(6);
                    string TDate = string.Format("{0:MM/dd/yyyy}", ToDate);
                    string tdate = TDate.Trim();
                    ToDate = Convert.ToDateTime(tdate + " " + "23:59:59");

                    string TotalCountPerDay1 = "";

                    TotalCountPerDay1 = TotalCountPerDay1 + " select count(CreatedDate)as count, CONVERT(varchar , CreatedDate ,105) as CreatedDate from StaffIssues ";
                    TotalCountPerDay1 = TotalCountPerDay1 + " where CreatedDate between '" + FromDate + "' and '" + ToDate + "' ";
                    if (!string.IsNullOrEmpty(cam))
                    {
                        TotalCountPerDay1 = TotalCountPerDay1 + "and BranchCode='" + cam + "' ";
                    }
                    TotalCountPerDay1 = TotalCountPerDay1 + "and Status in ('Completed','Logissue','Resolveissue','ResolveIssueRejection') ";
                    TotalCountPerDay1 = TotalCountPerDay1 + " group by  CONVERT(varchar , CreatedDate ,105) ";
                    TotalCountPerDay1 = TotalCountPerDay1 + " order by CreatedDate ";

                    string strQry1 = "";

                    strQry1 = strQry1 + " select createddate ,SUM(completed) as completed, SUM(logissue+ResolveIssue+ResolveIssueRejection) as NonCompleted ";
                    strQry1 = strQry1 + "from ( ";
                    strQry1 = strQry1 + "select convert(varchar,createddate,105) as createddate, ";
                    strQry1 = strQry1 + "case when Status='Completed'then count(Status) else 0 end as Completed, ";
                    strQry1 = strQry1 + "case when Status='LogIssue'then count(Status) else 0 end as LogIssue, ";
                    strQry1 = strQry1 + "case when Status='ResolveIssue'then count(Status) else 0 end as ResolveIssue, ";
                    strQry1 = strQry1 + "case when Status='ResolveIssueRejection'then count(Status) else 0 end as ResolveIssueRejection ";
                    strQry1 = strQry1 + "from StaffIssues where  ";
                    strQry1 = strQry1 + "CreatedDate between '" + FromDate + "' and '" + ToDate + "' ";
                    if (!string.IsNullOrEmpty(cam))
                    {
                        strQry1 = strQry1 + "and BranchCode='" + cam + "' ";
                    }
                    strQry1 = strQry1 + "and Status in ('Completed','Logissue','Resolveissue','ResolveIssueRejection') ";
                    strQry1 = strQry1 + "group by Status,convert(varchar,createddate,105)) a ";
                    strQry1 = strQry1 + "group by createddate ";
                    strQry1 = strQry1 + "order by createddate ";

                    List<DataRow> TotIssueCntPerDayList = null;
                    List<DataRow> IssueCntPerDayList = null;

                    MasterDataService mds = new MasterDataService();

                    DataTable TotIssueCntPerDayDT = mds.GetTotalIssueCountPerDay(TotalCountPerDay1);
                    DataTable IssueCntPerDayListDT = mds.GetTotalIssueCountPerDay(strQry1);

                    if (TotIssueCntPerDayDT != null && IssueCntPerDayListDT != null)
                    {
                        TotIssueCntPerDayList = TotIssueCntPerDayDT.AsEnumerable().ToList();
                        IssueCntPerDayList = IssueCntPerDayListDT.AsEnumerable().ToList();
                    }

                    string[] StatusArray = new string[4];

                    StatusArray[0] = "Completed";
                    StatusArray[1] = "NonCompleted";

                    string[] colorArray = new string[4];
                    colorArray[0] = "1D8BD1";
                    colorArray[1] = "F1683C";

                    string strxml = "";
                    int j = 0;
                    int k = j;

                    DateTime[] StatusDateArray = new DateTime[7];

                    string[] StatusDate = new string[7];

                    for (int a = 0; a <= 6; a++)
                    {
                        StatusDateArray[a] = FromDate.AddDays(a);
                        StatusDate[a] = string.Format("{0:dd/MM/yyyy}", StatusDateArray[a]);
                    }
                    string fd = string.Format("{0:dd/MM/yyyy}", FromDate);
                    string td = string.Format("{0:dd/MM/yyyy}", ToDate);

                    strxml = strxml + "<graph  caption='' ";
                    strxml = strxml + "  xAxisName='Date' ";
                    strxml = strxml + " yAxisName='Issue Count' ";
                    strxml = strxml + " lineThickness='1' ";
                    strxml = strxml + " showValues='1' ";
                    strxml = strxml + " formatNumberScale='0' ";
                    strxml = strxml + " anchorRadius='2' ";
                    strxml = strxml + " divLineAlpha='20' ";
                    strxml = strxml + " divLineColor='CC3300' ";
                    strxml = strxml + " showAlternateHGridColor='1' ";
                    strxml = strxml + " alternateHGridColor='CC3300' ";
                    strxml = strxml + " shadowAlpha='40' ";
                    strxml = strxml + "  numvdivlines='7' ";
                    strxml = strxml + " chartRightMargin='35' ";
                    strxml = strxml + " bgColor='ffffff' ";
                    strxml = strxml + " alternateHGridAlpha='7' ";
                    strxml = strxml + " limitsDecimalPrecision='0' ";
                    strxml = strxml + " divLineDecimalPrecision='0' ";
                    strxml = strxml + " rotateNames='1' ";
                    strxml = strxml + " decimalPrecision='0'> ";

                    strxml = strxml + " <categories> ";

                    for (int d = 0; d <= 6; d++)
                    {
                        strxml = strxml + " <category name = '" + StatusDate[d] + "'/> ";
                    }
                    strxml = strxml + " </categories> ";

                    strxml = strxml + " <dataset seriesName ='Total Count' color = 'AA0078' anchorBorderColor = 'AA0078' anchorBgColor = 'AA0078'> ";
                    //  For Total Issue count per day
                    DateTime fromdate = FromDate;
                    int totdaycnt = 0;
                    int dflag = 0;
                    int e = 0;

                    foreach (var items in TotIssueCntPerDayList)
                    {
                        if (dflag == 0)
                        {
                            totdaycnt = 0;
                        }
                        else
                        {
                            totdaycnt = e;
                        }

                        for (e = totdaycnt; e <= 6; e++)
                        {
                            int issdat = Convert.ToInt32(items.ItemArray[1].ToString().Substring(0, 2));
                            if (issdat == fromdate.Day)
                            {
                                strxml = strxml + "<set value = '" + items[0] + "'/> ";
                                fromdate = fromdate.AddDays(1);
                                dflag = 1;
                                e++;
                                break;
                            }
                            else
                            {
                                strxml = strxml + "<set value = '0'/> ";
                                fromdate = fromdate.AddDays(1);
                                dflag = 1;
                            }
                        }
                    }
                    for (int rday = 1; rday <= 7 - e; rday++)
                    {
                        strxml = strxml + "<set value = '0'/> ";
                    }
                    strxml = strxml + "</dataset> ";

                    int daycount = 0;
                    for (int arrcount = 0; arrcount <= 1; arrcount++)
                    {
                        string strstatus = StatusArray[arrcount];
                        strxml = strxml + " <dataset seriesName ='" + strstatus + "' color = '" + colorArray[arrcount] + "' anchorBorderColor = '" + colorArray[arrcount] + "' anchorBgColor = '" + colorArray[arrcount] + "'> ";
                        foreach (var items in IssueCntPerDayList)
                        {
                            if (daycount >= 7)
                            {
                                daycount = 0;
                                k = 0;
                            }
                            else
                            {
                                if (k > 7)
                                {
                                    k = 0;
                                }
                            }
                            for (daycount = k; daycount <= 6; daycount++)
                            {
                                int b = Convert.ToInt32(items.ItemArray[0].ToString().Substring(0, 2));
                                if (b == FromDate.Day)
                                {
                                    if (arrcount == 0)
                                    {
                                        strxml = strxml + "<set value = '" + items[1] + "'/> ";
                                        FromDate = FromDate.AddDays(1);
                                        k++;
                                        break;
                                    }
                                    if (arrcount == 1)
                                    {
                                        strxml = strxml + "<set value = '" + items[2] + "'/> ";
                                        FromDate = FromDate.AddDays(1);
                                        k++;
                                        break;
                                    }
                                }
                                else
                                {
                                    strxml = strxml + "<set value = '0'/> ";
                                    FromDate = FromDate.AddDays(1);
                                    k++;
                                }
                            }
                            if (daycount == 6)
                            {
                                strxml = strxml + "</dataset> ";
                                daycount++;
                                FromDate = FromDate.AddDays(-7);
                                break;
                            }
                        }
                        int z = daycount;
                        for (int r1day = 1; r1day <= 6 - z; r1day++)
                        {
                            strxml = strxml + "<set value = '0'/> ";
                            daycount++;
                            FromDate = FromDate.AddDays(1);
                            if (r1day == 6 - z)
                            {
                                FromDate = FromDate.AddDays(-7);
                                strxml = strxml + "</dataset> ";
                                daycount++;
                            }
                        }
                    }
                    strxml = strxml + "</graph>";
                    Response.Write(strxml);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }
        public void UpdateInbox(string Campus, string issue, string userId, long Id)
        {
            InboxService IBS = new InboxService();
            Inbox In = new Inbox();
            In.Campus = Campus;
            In.UserId = userId;
            In.InformationFor = issue;
            In.CreatedDate = DateTime.Now;
            In.Module = "Staff Issues Management";
            In.Status = "Inbox";
            In.PreRegNum = Id;
            In.RefNumber = Id;
            IBS.CreateOrUpdateInbox(In);
        }
        #region Added By Prabakaran
        public ActionResult ShowDueDate(string Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("logOut", "Account");
                else
                {
                    StaffIssues objStaffIssues = new StaffIssues();
                    StaffIssuesService sis = new StaffIssuesService();
                    if (!string.IsNullOrEmpty(Id))
                    {
                        if (Convert.ToInt64(Id) > 0)
                        {
                            objStaffIssues = sis.GetStaffIssuesById(Convert.ToInt64(Id));
                            ViewBag.Description = objStaffIssues.Description;
                            ViewBag.Id = objStaffIssues.Id;
                        }
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }
        public ActionResult UpdateStaffIssues(long Id, string DueDate)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    StaffIssues objStaffIssues = new StaffIssues();
                    StaffIssuesService sis = new StaffIssuesService();
                    if (Id > 0)
                    {
                        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                        objStaffIssues = sis.GetStaffIssuesById(Convert.ToInt64(Id));
                        objStaffIssues.DueDate = DateTime.Parse(DueDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        sis.CreateOrUpdateStaffIssues(objStaffIssues);
                    }
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }
        #endregion
        #region StaffIssueManagement Report By Prabakaran
        public ActionResult StaffIssueManagementReport()
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
                    criteria.Add("AppCode", "SIM");
                    //criteria.Add("RoleCode", "STAFF");
                    Dictionary<long, IList<UserAppRole>> UserAppRoleList = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 100, string.Empty, string.Empty, criteria);
                    if (UserAppRoleList.FirstOrDefault().Key > 0)
                    {
                        var ListCount = (from u in UserAppRoleList.First().Value
                                         where u.UserId == userId && u.RoleCode == "GRL"
                                         select u).ToList();
                        if (ListCount.Count > 0)
                        {
                            ViewBag.DeptCode = ListCount.FirstOrDefault().DeptCode;
                            ViewBag.UserId = ListCount.FirstOrDefault().UserId;
                            Session["DeptCode"] = ListCount.FirstOrDefault().DeptCode;
                            Session["Performer"] = ListCount.FirstOrDefault().UserId;
                        }
                        else
                        {
                            ViewBag.DeptCode = "";
                            ViewBag.UserId = "";
                            Session["DeptCode"] = "";
                            Session["Performer"] = "";
                        }
                    }
                    else
                    {
                        ViewBag.DeptCode = "";
                        ViewBag.UserId = "";
                        Session["DeptCode"] = "";
                        Session["Performer"] = "";
                    }
                    //ViewBag.UserId = userId;
                    return View();
                }
            }
            catch (Exception ex)
            {

                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }
        public ActionResult StaffIssueManagementReportListJqGrid(string Campus, string IssueGroup, string IssueType, string Performer, string FromDate, string ToDate, string DateType, string DueDateType, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        {
            try
            {
                StaffIssuesService sis = new StaffIssuesService();
                Dictionary<string, object> Criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(Campus))
                { Criteria.Add("BranchCode", Campus); }
                if (!string.IsNullOrEmpty(IssueGroup))
                { Criteria.Add("IssueGroup", IssueGroup); }
                if (!string.IsNullOrEmpty(IssueType))
                { Criteria.Add("IssueType", IssueType); }
                if (!string.IsNullOrEmpty(Performer))
                { Criteria.Add("Performer", Performer); }
                //if (string.IsNullOrEmpty(Performer))
                //{ Criteria.Add("Performer", userId); }
                //if (string.IsNullOrEmpty(IssueGroup))
                //{
                //    var DeptCode = Session["DeptCode"] != null ? Session["DeptCode"].ToString() : "";
                //    if (!string.IsNullOrEmpty(DeptCode))
                //    { Criteria.Add("IssueGroup", DeptCode); }
                //}
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                if (!string.IsNullOrEmpty(FromDate))
                {

                    if (!string.IsNullOrEmpty(ToDate))
                    {
                        DateTime[] FromToDate = new DateTime[2];
                        FromToDate[0] = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        FromToDate[1] = DateTime.Parse(ToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        string ToDate1 = string.Format("{0:dd/MM/yyyy}", FromToDate[1]);
                        ToDate1 = ToDate1 + " 23:59:59";
                        FromToDate[1] = DateTime.Parse(ToDate1, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);

                        Criteria.Add(DateType, FromToDate);
                    }
                    else
                    {
                        DateTime[] fromto = new DateTime[2];
                        fromto[0] = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        fromto[1] = DateTime.Now;
                        Criteria.Add(DateType, fromto);
                    }
                }
                Dictionary<long, IList<StaffIssueManagementReport_vw>> GetStaffIssueManagementReportList = sis.GetStaffIssueManagementReport_vwListWithPaging(page - 1, 9999999, sidx, sord, Criteria);
                if (GetStaffIssueManagementReportList != null && GetStaffIssueManagementReportList.FirstOrDefault().Key > 0)
                {

                    if (!string.IsNullOrEmpty(DueDateType))
                    {
                        foreach (var item in GetStaffIssueManagementReportList.FirstOrDefault().Value)
                        {
                            if (item.ModifiedDate != null && item.DueDate != null)
                            {
                                item.DifferenceInHours = item.ModifiedDate - item.DueDate;
                            }
                        }
                        var DueDateList = new List<StaffIssueManagementReport_vw>();
                        if (DueDateType == "1")
                        {
                            DueDateList = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.DueDate != null && u.ModifiedDate != null && u.DifferenceInHours.Value.TotalHours <= 0 select u).ToList();
                        }
                        if (DueDateType == "2")
                        {
                            DueDateList = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.DueDate != null && u.ModifiedDate != null && u.DifferenceInHours.Value.TotalHours > 0 && u.DifferenceInHours.Value.TotalHours <= 12 select u).ToList();
                        }
                        if (DueDateType == "3")
                        {
                            DueDateList = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.DueDate != null && u.ModifiedDate != null && u.DifferenceInHours.Value.TotalHours > 0 && u.DifferenceInHours.Value.TotalHours > 12 select u).ToList();
                        }
                        var DueDateWiseList = (from fpl in DueDateList
                                               group fpl by fpl.Performer into fplt
                                               //where fplt.FirstOrDefault().IssueNumber == ""
                                               select new
                                               {
                                                   cell = new string[]
                                                       {                                                                                                                        
                                                            fplt.FirstOrDefault().Id.ToString(),
                                                            fplt.FirstOrDefault().InstanceId.ToString(),  
                                                            fplt.FirstOrDefault().ProcessRefId.ToString(),  
                                                            fplt.FirstOrDefault().Performer!=null?fplt.FirstOrDefault().Performer.ToString():"UnAssigned",  
                                                            fplt.Sum(x => x.Available).ToString(),                                                                                                                    
                                                            fplt.Sum(x => x.Assigned).ToString(),                                                                                                                         
                                                            fplt.Sum(x => x.Resolved).ToString(),
                                                            fplt.Sum(x => x.TotalCount).ToString()
                                                               
                                                       }
                                               }).ToList();
                        if (ExptXl == 1)
                        {
                            var List = DueDateWiseList;
                            int i = 1;
                            foreach (var item1 in DueDateWiseList)
                            {
                                item1.cell[0] = i.ToString();
                                item1.cell[7] = (Convert.ToInt64(item1.cell[7]) + Convert.ToInt64(item1.cell[6])).ToString();
                                i++;
                            }
                            base.ExptToXL(List, "StaffIssueManagementReportCount" + "-On-" + DateTime.Today.ToString("dd/MM/yyyy"), (items => new
                            {
                                SNo = items.cell[0],
                                Performer = items.cell[3] != null ? us.GetUserNameByUserId(items.cell[3]) : "UnAssigned",
                                Available = items.cell[4],
                                Assigned = items.cell[5],
                                Resolved = items.cell[6],
                                Total = items.cell[7]

                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            foreach (var item1 in DueDateWiseList)
                            {
                                item1.cell[7] = (Convert.ToInt64(item1.cell[7]) + Convert.ToInt64(item1.cell[6])).ToString();
                            }
                            long totalRecords = DueDateWiseList.Count;
                            int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                            var jsonData = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalRecords,
                                rows = (
                                    from items in DueDateWiseList
                                    select new
                                    {
                                        i = items.cell[1],
                                        cell = new string[] 
                                     { 
                                         //items.StaffIssues.DueDate!=null?:"<a href='#' onclick=\"ShowDueDate('" + items.StaffIssues.Id + "');\" >"+items.StaffIssues.IssueNumber+"</a>",
                                         items.cell[0],
                                         items.cell[1],
                                         items.cell[2],
                                         items.cell[3]!=null?us.GetUserNameByUserId(items.cell[3]):"UnAssigned",
                                         items.cell[4],
                                         "<a href='#' onclick=\"ShowCallDetails('" + items.cell[5] + "','Assigned','"+items.cell[3]+"');\" >"+items.cell[5]+"</a>",
                                         "<a href='#' onclick=\"ShowCallDetails('" + items.cell[6] + "','Resolved','"+items.cell[3]+"');\" >"+items.cell[6]+"</a>",
                                         "<a href='#' onclick=\"ShowCallDetails('" + items.cell[7] + "','TotalCount','"+items.cell[3]+"');\" >"+items.cell[7]+"</a>",
                                     }
                                    })

                            };
                            return Json(jsonData, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        var List1 = (from fpl in GetStaffIssueManagementReportList.FirstOrDefault().Value
                                     group fpl by fpl.Performer into fplt
                                     //where fplt.FirstOrDefault().IssueNumber == ""
                                     select new
                                     {
                                         cell = new string[]
                                                       {                                                                                                                        
                                                            fplt.FirstOrDefault().Id.ToString(),
                                                            fplt.FirstOrDefault().InstanceId.ToString(),  
                                                            fplt.FirstOrDefault().ProcessRefId.ToString(),  
                                                            fplt.FirstOrDefault().Performer!=null?fplt.FirstOrDefault().Performer.ToString():"",  
                                                            fplt.Sum(x => x.Available).ToString(),                                                                                                                    
                                                            fplt.Sum(x => x.Assigned).ToString(),                                                                                                                         
                                                            fplt.Sum(x => x.Resolved).ToString(),
                                                            fplt.Sum(x => x.TotalCount).ToString()
                                                               
                                                       }

                                     }).ToList();
                        if (ExptXl == 1)
                        {
                            var List = List1;
                            int i = 1;
                            foreach (var item1 in List1)
                            {
                                item1.cell[0] = i.ToString();
                                item1.cell[7] = (Convert.ToInt64(item1.cell[7]) + Convert.ToInt64(item1.cell[6])).ToString();
                                i++;
                            }
                            base.ExptToXL(List, "StaffIssueManagementReportCount" + "-On-" + DateTime.Today.ToString("dd/MM/yyyy"), (items => new
                            {
                                SNo = items.cell[0],
                                Performer = items.cell[3] != "" ? us.GetUserNameByUserId(items.cell[3]) : "UnAssigned",
                                Available = items.cell[4],
                                Assigned = items.cell[5],
                                Resolved = items.cell[6],
                                Total = items.cell[7]

                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            foreach (var item1 in List1)
                            {
                                item1.cell[7] = (Convert.ToInt64(item1.cell[7]) + Convert.ToInt64(item1.cell[6])).ToString();
                            }
                            long totalRecords = List1.Count;
                            int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                            var jsonData = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalRecords,
                                rows = (
                                    from items in List1
                                    select new
                                    {
                                        i = items.cell[1],
                                        cell = new string[] 
                                     { 
                                         items.cell[0],
                                         items.cell[1],
                                         items.cell[2],
                                         items.cell[3]!=""?us.GetUserNameByUserId(items.cell[3]):"UnAssigned",
                                         items.cell[4],
                                         "<a href='#' onclick=\"ShowCallDetails('" + items.cell[5] + "','Assigned','"+items.cell[3]+"');\" >"+items.cell[5]+"</a>",
                                         "<a href='#' onclick=\"ShowCallDetails('" + items.cell[6] + "','Resolved','"+items.cell[3]+"');\" >"+items.cell[6]+"</a>",
                                         "<a href='#' onclick=\"ShowCallDetails('" + items.cell[7] + "','TotalCount','"+items.cell[3]+"');\" >"+items.cell[7]+"</a>",
                                     }
                                    })

                            };
                            return Json(jsonData, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult getCallPerformerList(string IssueGroup, string Performer)
        {
            try
            {
                UserService uS = new UserService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("AppCode", "SIM");
                if (!string.IsNullOrEmpty(IssueGroup))
                {
                    criteria.Add("DeptCode", IssueGroup);
                }
                string[] RoleCodeArray = new string[2];
                RoleCodeArray[0] = "GRL";
                RoleCodeArray[1] = "GRL-HEAD";
                criteria.Add("RoleCode", RoleCodeArray);
                if (!string.IsNullOrEmpty(Performer))
                {
                    criteria.Add("UserId", Performer);
                }
                Dictionary<long, IList<UserAppRole_Vw>> PerformerList = us.GetAppRoleOnlyActiveUsersPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                if (PerformerList != null && PerformerList.Count > 0 && PerformerList.FirstOrDefault().Key > 0)
                {
                    var PerformerName = (
                             from items in PerformerList.First().Value
                             select new
                             {
                                 Text = items.UserName,
                                 Value = items.UserId
                             }).Distinct().ToList();

                    return Json(PerformerName, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json("", JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult ShowCallDetails(long Totalcount, string CountName, string Performer)
        {
            try
            {
                ViewBag.Result = 1;
                ViewBag.CountName = CountName;
                ViewBag.Performer = Performer;
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult ShowCallDetailsListJqGrid(string Campus, string IssueGroup, string IssueType, string DueDateType, string DateType, string FromDate, string ToDate, string CountName, string Result, string Performer, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        {
            try
            {
                {
                    StaffIssuesService sis = new StaffIssuesService();
                    Dictionary<string, object> Criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(Campus))
                    { Criteria.Add("BranchCode", Campus); }
                    if (!string.IsNullOrEmpty(IssueGroup))
                    { Criteria.Add("IssueGroup", IssueGroup); }
                    if (!string.IsNullOrEmpty(IssueType))
                    { Criteria.Add("IssueType", IssueType); }
                    if (!string.IsNullOrEmpty(Performer))
                    { Criteria.Add("Performer", Performer); }
                    //if (!string.IsNullOrEmpty(CountName) && !string.IsNullOrEmpty(Result))
                    //{ Criteria.Add(CountName, Convert.ToInt64(Result)); }
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    if (!string.IsNullOrEmpty(FromDate))
                    {

                        if (!string.IsNullOrEmpty(ToDate))
                        {
                            DateTime[] FromToDate = new DateTime[2];
                            FromToDate[0] = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            FromToDate[1] = DateTime.Parse(ToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            string ToDate1 = string.Format("{0:dd/MM/yyyy}", FromToDate[1]);
                            ToDate1 = ToDate1 + " 23:59:59";
                            FromToDate[1] = DateTime.Parse(ToDate1, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);

                            Criteria.Add(DateType, FromToDate);
                        }
                        else
                        {
                            DateTime[] fromto = new DateTime[2];
                            fromto[0] = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            fromto[1] = DateTime.Now;
                            //string To = string.Format("{0:dd/MM/yyyy}", fromto[0]);
                            //fromto[1] = Convert.ToDateTime(To + " " + "23:59:59");
                            Criteria.Add(DateType, fromto);
                        }
                    }


                    Dictionary<long, IList<StaffIssueManagementReport_vw>> GetStaffIssueManagementReportList = sis.GetStaffIssueManagementReport_vwListWithPaging(page - 1, 9999999, sidx, sord, Criteria);
                    if (GetStaffIssueManagementReportList != null && GetStaffIssueManagementReportList.FirstOrDefault().Key > 0)
                    {
                        var GetStaffIssueMangementCountReportList1 = new List<StaffIssueManagementReport_vw>();
                        if (CountName == "Assigned")
                        {
                            if (!string.IsNullOrEmpty(Performer))
                            {
                                GetStaffIssueMangementCountReportList1 = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Assigned > 0 select u).ToList();
                            }
                            else
                            {
                                GetStaffIssueMangementCountReportList1 = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Assigned > 0 && Performer != "" && Performer != null select u).ToList();
                            }
                        }
                        if (CountName == "Resolved")
                        {
                            if (!string.IsNullOrEmpty(Performer))
                            {
                                GetStaffIssueMangementCountReportList1 = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Resolved > 0 select u).ToList();
                            }
                            else
                            {
                                GetStaffIssueMangementCountReportList1 = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Resolved > 0 && Performer != "" && Performer != null select u).ToList();
                            }

                        }
                        if (CountName == "TotalCount")
                        {
                            if (!string.IsNullOrEmpty(Performer))
                            {
                                var AssignedList = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Assigned > 0 select u).ToList();
                                var ResolvedList = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Resolved > 0 && u.Performer != null && u.Performer != "" select u).ToList();
                                GetStaffIssueMangementCountReportList1 = AssignedList.Union(ResolvedList).ToList();
                            }
                            else
                            {
                                var AssignedList = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Assigned > 0 && u.Performer != null && u.Performer != "" select u).ToList();
                                var ResolvedList = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Resolved > 0 && u.Performer != null && u.Performer != "" select u).ToList();
                                GetStaffIssueMangementCountReportList1 = AssignedList.Union(ResolvedList).ToList();
                            }
                        }
                        if (!string.IsNullOrEmpty(DueDateType))
                        {
                            foreach (var item in GetStaffIssueMangementCountReportList1)
                            {
                                //if (item.ModifiedDate != null && item.DueDate != null)
                                //{                                    
                                //    item.DifferenceInHours = item.ModifiedDate - item.DueDate;
                                //}
                                if (item.DueDate != null)
                                {
                                    item.DifferenceInHours = DateTime.Now - item.DueDate;
                                    if (item.ModifiedDate != null)
                                    {
                                        item.DifferenceInHours1 = item.ModifiedDate - item.DueDate;
                                        if (item.AssignedDate != null)
                                        {
                                            item.DifferenceInHours2 = item.ModifiedDate - item.AssignedDate;
                                        }
                                        else
                                        {
                                            item.DifferenceInHours2 = TimeSpan.Zero;
                                        }
                                    }
                                    else
                                    {
                                        if (item.AssignedDate != null)
                                        {
                                            item.DifferenceInHours2 = DateTime.Now - item.AssignedDate;
                                        }
                                        else
                                        {
                                            item.DifferenceInHours2 = TimeSpan.Zero;
                                        }
                                    }
                                }
                                else
                                {
                                    item.DifferenceInHours = TimeSpan.Zero;
                                    item.DifferenceInHours2 = TimeSpan.Zero;
                                }
                            }
                            var DueDateList = new List<StaffIssueManagementReport_vw>();
                            if (DueDateType == "1")
                            {
                                DueDateList = (from u in GetStaffIssueMangementCountReportList1 where u.DueDate != null && u.ModifiedDate != null && u.DifferenceInHours1.Value.TotalHours <= 0 select u).ToList();
                            }
                            if (DueDateType == "2")
                            {
                                DueDateList = (from u in GetStaffIssueMangementCountReportList1 where u.DueDate != null && u.ModifiedDate != null && u.DifferenceInHours1.Value.TotalHours > 0 && u.DifferenceInHours1.Value.TotalHours <= 12 select u).ToList();
                            }
                            if (DueDateType == "3")
                            {
                                DueDateList = (from u in GetStaffIssueMangementCountReportList1 where u.DueDate != null && u.ModifiedDate != null && u.DifferenceInHours1.Value.TotalHours > 0 && u.DifferenceInHours1.Value.TotalHours > 12 select u).ToList();
                            }
                            //var DueDateWiseList = (from fpl in DueDateList
                            //                       group fpl by fpl.Performer into fplt
                            //                       //where fplt.FirstOrDefault().IssueNumber == ""
                            //                       select new
                            //                       {
                            //                           cell = new string[]
                            //                           {                                                                                                                        
                            //                                fplt.FirstOrDefault().Id.ToString(),
                            //                                fplt.FirstOrDefault().InstanceId.ToString(),  
                            //                                fplt.FirstOrDefault().ProcessRefId.ToString(),  
                            //                                fplt.FirstOrDefault().Performer!=null?fplt.FirstOrDefault().Performer.ToString():"",  
                            //                                fplt.Sum(x => x.Available).ToString(),                                                                                                                    
                            //                                fplt.Sum(x => x.Assigned).ToString(),                                                                                                                         
                            //                                fplt.Sum(x => x.Resolved).ToString()

                            //                           }
                            //                       }).ToList();
                            if (ExptXl == 1)
                            {
                                var List = DueDateList;
                                int i = 1;
                                foreach (var item1 in DueDateList)
                                {
                                    item1.Id = i;
                                    i++;
                                }
                                base.ExptToXL(List, "StaffIssueManagementReport", (items => new
                                {
                                    SlNo = items.Id,
                                    Performer = items.Performer != null ? us.GetUserNameByUserId(items.Performer) : "",
                                    Issue_Number = items.IssueNumber,
                                    Campus = items.BranchCode,
                                    Issue_Group = items.IssueGroup,
                                    Issue_Type = items.IssueType,
                                    Description = items.Description,
                                    Created_By = items.CreatedBy,
                                    Created_Date = items.CreatedDate.Value.ToString("dd/MM/yyyy"),
                                    Status = items.Status,
                                    Resolution = items.Resolution,
                                    Resolved_Date = items.ModifiedDate != null ? items.ModifiedDate.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                                    Due_Date = items.DueDate != null ? items.DueDate.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                                    Issue_Hours = items.DueDate != null ? items.DifferenceInHours2.Value.Days + "d " + items.DifferenceInHours2.Value.Hours + "h " + items.DifferenceInHours2.Value.Minutes + "m " : "0d 0h 0m"

                                }));
                                return new EmptyResult();
                            }
                            else
                            {

                                long totalRecords = DueDateList.Count;
                                int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                                var jsonData = new
                                {
                                    total = totalPages,
                                    page = page,
                                    records = totalRecords,
                                    rows = (
                                        from items in DueDateList
                                        select new
                                        {
                                            i = items.Id,
                                            cell = new string[] 
                                     { 
                                         items.Performer!=null?us.GetUserNameByUserId(items.Performer):"",
                                         "<a href='/StaffIssues/ShowStaffIssue?id=" + items.ProcessRefId + "'>" + items.IssueNumber + "</a>",
                                         items.BranchCode,
                                         items.IssueGroup,
                                         items.IssueType,
                                         items.Description,
                                         items.CreatedBy,
                                         items.CreatedDate.Value.ToString("dd/MM/yyyy"),
                                         items.Status,
                                         items.Resolution,
                                         items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd/MM/yyyy HH:mm:ss"):"",
                                         items.DueDate!=null?items.DueDate.Value.ToString("dd/MM/yyyy HH:mm:ss"):"",
                                         items.Status == "Completed"?"Completed":items.DifferenceInHours.Value.TotalHours.ToString(),                                         
                                         items.ModifiedDate == null?"NotCompleted":items.DifferenceInHours1.Value.TotalHours.ToString(),
                                         items.DueDate!=null?items.DifferenceInHours2.Value.Days+"d "+ items.DifferenceInHours2.Value.Hours +"h " + items.DifferenceInHours2.Value.Minutes + "m ":"0d 0h 0m"
                                     }
                                        })

                                };
                                return Json(jsonData, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            foreach (var item in GetStaffIssueMangementCountReportList1)
                            {
                                //if (item.ModifiedDate != null && item.DueDate != null)
                                //{                                    
                                //    item.DifferenceInHours = item.ModifiedDate - item.DueDate;
                                //}
                                if (item.DueDate != null)
                                {
                                    item.DifferenceInHours = DateTime.Now - item.DueDate;
                                    if (item.ModifiedDate != null)
                                    {
                                        item.DifferenceInHours1 = item.ModifiedDate - item.DueDate;
                                        if (item.AssignedDate != null)
                                        {
                                            item.DifferenceInHours2 = item.ModifiedDate - item.AssignedDate;
                                        }
                                        else
                                        {
                                            item.DifferenceInHours2 = TimeSpan.Zero;
                                        }
                                    }
                                    else
                                    {
                                        if (item.AssignedDate != null)
                                        {
                                            item.DifferenceInHours2 = DateTime.Now - item.AssignedDate;
                                        }
                                        else
                                        {
                                            item.DifferenceInHours2 = TimeSpan.Zero;
                                        }
                                    }
                                }
                                else
                                {
                                    item.DifferenceInHours = TimeSpan.Zero;
                                    item.DifferenceInHours2 = TimeSpan.Zero;
                                }
                            }
                            if (ExptXl == 1)
                            {
                                var List = GetStaffIssueMangementCountReportList1;
                                int i = 1;
                                foreach (var item1 in GetStaffIssueMangementCountReportList1)
                                {
                                    item1.Id = i;
                                    i++;
                                }
                                base.ExptToXL(List, "StaffIssueList", (items => new
                                {
                                    SlNo = items.Id,
                                    Performer = items.Performer != null ? us.GetUserNameByUserId(items.Performer) : "",
                                    Issue_Number = items.IssueNumber,
                                    Campus = items.BranchCode,
                                    Issue_Group = items.IssueGroup,
                                    Issue_Type = items.IssueType,
                                    Description = items.Description,
                                    Created_By = items.CreatedBy,
                                    Created_Date = items.CreatedDate.Value.ToString("dd/MM/yyyy"),
                                    Status = items.Status,
                                    Resolution = items.Resolution,
                                    Resolved_Date = items.ModifiedDate != null ? items.ModifiedDate.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                                    Due_Date = items.DueDate != null ? items.DueDate.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                                    Issue_Hours = items.DueDate != null ? items.DifferenceInHours2.Value.Days + "d " + items.DifferenceInHours2.Value.Hours + "h " + items.DifferenceInHours2.Value.Minutes + "m " : "0d 0h 0m"

                                }));
                                return new EmptyResult();
                            }
                            else
                            {

                                long totalRecords = GetStaffIssueMangementCountReportList1.Count;
                                int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                                var jsonData = new
                                {
                                    total = totalPages,
                                    page = page,
                                    records = totalRecords,
                                    rows = (
                                        from items in GetStaffIssueMangementCountReportList1
                                        select new
                                        {
                                            i = items.Id,
                                            cell = new string[] 
                                     { 
                                         
                                         items.Performer!=null?us.GetUserNameByUserId(items.Performer):"",
                                         "<a href='/StaffIssues/ShowStaffIssue?id=" + items.ProcessRefId + "'>" + items.IssueNumber + "</a>",
                                         items.BranchCode,
                                         items.IssueGroup,
                                         items.IssueType,
                                         items.Description,
                                         items.CreatedBy,
                                         items.CreatedDate.Value.ToString("dd/MM/yyyy"),
                                         items.Status,
                                         items.Resolution,
                                         items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd/MM/yyyy HH:mm:ss"):"",
                                         items.DueDate!=null?items.DueDate.Value.ToString("dd/MM/yyyy HH:mm:ss"):"",
                                         items.Status == "Completed"?"Completed":items.DifferenceInHours.Value.TotalHours.ToString(),                                         
                                         items.ModifiedDate == null?"NotCompleted":items.DifferenceInHours1.Value.TotalHours.ToString(),
                                         items.DueDate!=null?items.DifferenceInHours2.Value.Days+"d "+ items.DifferenceInHours2.Value.Hours +"h " + items.DifferenceInHours2.Value.Minutes + "m ":"0d 0h 0m"
                                     }
                                        })

                                };
                                return Json(jsonData, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        var Empty = new { rows = (new { cell = new string[] { } }) };
                        return Json(Empty, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult StaffIssueManagementDateWiseReportListJqGrid(string Campus, string IssueGroup, string IssueType, string Performer, string FromDate, string ToDate, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        {
            try
            {
                StaffIssuesService sis = new StaffIssuesService();
                Dictionary<string, object> Criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(Campus))
                { Criteria.Add("BranchCode", Campus); }
                if (!string.IsNullOrEmpty(IssueGroup))
                { Criteria.Add("IssueGroup", IssueGroup); }
                if (!string.IsNullOrEmpty(IssueType))
                { Criteria.Add("IssueType", IssueType); }
                if (!string.IsNullOrEmpty(Performer))
                { Criteria.Add("Performer", Performer); }
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                DateTime[] FromToDateTime = new DateTime[2];
                if (!string.IsNullOrEmpty(FromDate))
                {
                    if (!string.IsNullOrEmpty(ToDate))
                    {
                        FromToDateTime[0] = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        FromToDateTime[1] = DateTime.Parse(ToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        string ToDate1 = string.Format("{0:dd/MM/yyyy}", FromToDateTime[1]);
                        ToDate1 = ToDate1 + " 23:59:59";
                        FromToDateTime[1] = DateTime.Parse(ToDate1, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    }
                    else
                    {
                        FromToDateTime[0] = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        FromToDateTime[1] = DateTime.Now;
                        //string To = string.Format("{0:dd/MM/yyyy}", fromto[0]);
                        //fromto[1] = Convert.ToDateTime(To + " " + "23:59:59");                        
                    }
                }
                if (!string.IsNullOrEmpty(FromDate))
                {
                    Dictionary<long, IList<StaffIssueManagementReport_vw>> GetStaffIssueManagementReportList = sis.GetStaffIssueManagementReport_vwListWithPaging(page - 1, 9999999, sidx, sord, Criteria);
                    if (GetStaffIssueManagementReportList != null && GetStaffIssueManagementReportList.FirstOrDefault().Key > 0)
                    {

                        var List1 = (from fpl in GetStaffIssueManagementReportList.FirstOrDefault().Value
                                     group fpl by fpl.Performer into fplt
                                     //where fplt.FirstOrDefault().Performer != null && fplt.FirstOrDefault().Performer != ""
                                     select new
                                     {
                                         cell = new string[] {
                                                 fplt.FirstOrDefault().Id.ToString(),
                                                 fplt.FirstOrDefault().InstanceId.ToString(),  
                                                 fplt.FirstOrDefault().ProcessRefId.ToString(),
                                                 fplt.FirstOrDefault().Performer!=null?fplt.FirstOrDefault().Performer.ToString():"",  
                                                 fplt.Sum(x => x.Available).ToString(),                                                                                                                    
                                                 fplt.Sum(x => x.Assigned).ToString(),                                                                                                                         
                                                 fplt.Sum(x => x.Resolved).ToString(),
                                                 fplt.Sum(x => x.TotalCount).ToString()
                                             }
                                     }).ToList();
                        var DateWiseList = (from items in GetStaffIssueManagementReportList.FirstOrDefault().Value
                                            where items.ModifiedDate >= FromToDateTime[0] && items.ModifiedDate <= FromToDateTime[1] && items.Performer != null && items.Performer != ""
                                            select items).ToList();
                        var ResolvedCount = (from fpl in DateWiseList
                                             group fpl by fpl.Performer into fplt
                                             //where fplt.FirstOrDefault().Performer != "" && fplt.FirstOrDefault().Performer!=null
                                             select new
                                             {
                                                 cell = new string[]
                                                       {                                                                                                                        
                                                            fplt.FirstOrDefault().Id.ToString(),
                                                            fplt.FirstOrDefault().InstanceId.ToString(),  
                                                            fplt.FirstOrDefault().ProcessRefId.ToString(),  
                                                            fplt.FirstOrDefault().Performer!=null?fplt.FirstOrDefault().Performer.ToString():"",  
                                                            fplt.Sum(x => x.Available).ToString(),                                                                                                                    
                                                            fplt.Sum(x => x.Assigned).ToString(),                                                                                                                         
                                                            fplt.Sum(x => x.Resolved).ToString(),
                                                            fplt.Sum(x => x.TotalCount).ToString()
                                                               
                                                       }

                                             }).ToList();
                        foreach (var items in List1)
                        {
                            long Count = 0;
                            items.cell[6] = Convert.ToInt64(Count).ToString();
                            foreach (var item1 in ResolvedCount)
                            {
                                if (items.cell[3] == item1.cell[3])
                                {
                                    items.cell[6] = item1.cell[6];
                                    items.cell[7] = (Convert.ToInt64(items.cell[7]) + Convert.ToInt64(item1.cell[6])).ToString();
                                    break;
                                }
                            }
                        }
                        if (ExptXl == 1)
                        {
                            var List = List1;
                            int i = 1;
                            foreach (var item1 in List1)
                            {
                                item1.cell[0] = i.ToString();
                                i++;
                            }
                            base.ExptToXL(List, "StaffIssueManagementDateWiseReportCount" + "-On-" + DateTime.Today.ToString("dd/MM/yyyy"), (items => new
                            {
                                SNo = items.cell[0],
                                Performer = items.cell[3] != "" ? us.GetUserNameByUserId(items.cell[3]) : "UnAssigned",
                                Available = items.cell[4],
                                Assigned = items.cell[5],
                                Resolved = items.cell[6],
                                TotalCount = items.cell[7],

                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalRecords = List1.Count;
                            int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                            var jsonData = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalRecords,
                                rows = (
                                    from items in List1
                                    select new
                                    {
                                        i = items.cell[1],
                                        cell = new string[] 
                                     { 
                                         items.cell[0],
                                         items.cell[1],
                                         items.cell[2],
                                         items.cell[3]!=""?us.GetUserNameByUserId(items.cell[3]):"UnAssigned",
                                         items.cell[4],
                                         "<a href='#' onclick=\"ShowCallDetails1('Assigned','"+items.cell[3]+"');\" >"+items.cell[5]+"</a>",
                                         "<a href='#' onclick=\"ShowCallDetails1('Resolved','"+items.cell[3]+"');\" >"+items.cell[6]+"</a>",
                                         "<a href='#' onclick=\"ShowCallDetails1('TotalCount','"+items.cell[3]+"');\" >"+items.cell[7]+"</a>",
                                       
                                     }
                                    })

                            };
                            return Json(jsonData, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        var Empty = new { rows = (new { cell = new string[] { } }) };
                        return Json(Empty, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult StaffIssueManagementDateWiseReport()
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
                    criteria.Add("AppCode", "SIM");
                    //criteria.Add("RoleCode", "STAFF");
                    Dictionary<long, IList<UserAppRole>> UserAppRoleList = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 100, string.Empty, string.Empty, criteria);
                    if (UserAppRoleList.FirstOrDefault().Key > 0)
                    {
                        var ListCount = (from u in UserAppRoleList.First().Value
                                         where u.UserId == userId && u.RoleCode == "GRL"
                                         select u).ToList();
                        if (ListCount.Count > 0)
                        {
                            ViewBag.DeptCode = ListCount.FirstOrDefault().DeptCode;
                            ViewBag.UserId = ListCount.FirstOrDefault().UserId;
                            Session["DeptmntCode"] = ListCount.FirstOrDefault().DeptCode;
                            Session["PerformerId"] = ListCount.FirstOrDefault().UserId;
                        }
                        else
                        {
                            ViewBag.DeptCode = "";
                            ViewBag.UserId = "";
                            Session["DeptmntCode"] = "";
                            Session["PerformerId"] = "";
                        }
                    }
                    else
                    {
                        ViewBag.DeptCode = "";
                        ViewBag.UserId = "";
                        Session["DeptmntCode"] = "";
                        Session["PerformerId"] = "";
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {

                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }
        public ActionResult ShowCallDetails1(string HeaderName, string Performer)
        {
            try
            {
                ViewBag.Result = 1;
                ViewBag.HeaderName = HeaderName;
                ViewBag.Performer = Performer;
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult ShowCallDetailsListJqGrid1(string Campus, string IssueGroup, string IssueType, string FromDate, string ToDate, string HeaderName, string Result, string Performer, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        {
            try
            {
                {
                    StaffIssuesService sis = new StaffIssuesService();
                    Dictionary<string, object> Criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(Campus))
                    { Criteria.Add("BranchCode", Campus); }
                    if (!string.IsNullOrEmpty(IssueGroup))
                    { Criteria.Add("IssueGroup", IssueGroup); }
                    if (!string.IsNullOrEmpty(IssueType))
                    { Criteria.Add("IssueType", IssueType); }
                    if (!string.IsNullOrEmpty(Performer))
                    { Criteria.Add("Performer", Performer); }
                    //if (!string.IsNullOrEmpty(HeaderName) && !string.IsNullOrEmpty(Result))
                    //{ Criteria.Add(HeaderName, Convert.ToInt64(Result)); }
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    DateTime[] FromToDateTime = new DateTime[2];
                    if (HeaderName == "Resolved" || HeaderName == "TotalCount")
                    {
                        if (!string.IsNullOrEmpty(FromDate))
                        {
                            if (!string.IsNullOrEmpty(ToDate))
                            {
                                FromToDateTime[0] = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                                FromToDateTime[1] = DateTime.Parse(ToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                                string ToDate1 = string.Format("{0:dd/MM/yyyy}", FromToDateTime[1]);
                                ToDate1 = ToDate1 + " 23:59:59";
                                FromToDateTime[1] = DateTime.Parse(ToDate1, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                                if (HeaderName == "Resolved") { Criteria.Add("ModifiedDate", FromToDateTime); }
                            }
                            else
                            {
                                FromToDateTime[0] = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                                FromToDateTime[1] = DateTime.Now;
                                if (HeaderName == "Resolved") { Criteria.Add("ModifiedDate", FromToDateTime); }
                            }
                        }
                    }
                    Dictionary<long, IList<StaffIssueManagementReport_vw>> GetStaffIssueManagementReportList = sis.GetStaffIssueManagementReport_vwListWithPaging(page - 1, 9999999, sidx, sord, Criteria);
                    if (GetStaffIssueManagementReportList != null && GetStaffIssueManagementReportList.Count > 0)
                    {
                        var GetStaffIssueManagementReportList1 = new List<StaffIssueManagementReport_vw>();
                        if (HeaderName == "Assigned")
                        {
                            if (!string.IsNullOrEmpty(Performer))
                            {
                                GetStaffIssueManagementReportList1 = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Assigned > 0 select u).ToList();
                            }
                            else
                            {
                                GetStaffIssueManagementReportList1 = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Assigned > 0 && Performer != "" && Performer != null select u).ToList();
                            }
                        }
                        if (HeaderName == "Resolved")
                        {
                            if (!string.IsNullOrEmpty(Performer))
                            {
                                GetStaffIssueManagementReportList1 = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Resolved > 0 select u).ToList();
                            }
                            else
                            {
                                GetStaffIssueManagementReportList1 = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Resolved > 0 && Performer != "" && Performer != null select u).ToList();
                            }
                        }
                        if (HeaderName == "TotalCount")
                        {
                            if (!string.IsNullOrEmpty(Performer))
                            {
                                var AssignedList = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Assigned > 0 select u).ToList();
                                var ResolvedList = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Resolved > 0 && u.ModifiedDate >= FromToDateTime[0] && u.ModifiedDate <= FromToDateTime[1] && u.Performer != null && u.Performer != "" select u).ToList();
                                GetStaffIssueManagementReportList1 = AssignedList.Union(ResolvedList).ToList();
                            }
                            else
                            {
                                var AssignedList = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Assigned > 0 && u.Performer != null && u.Performer != "" select u).ToList();
                                var ResolvedList = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Resolved > 0 && u.ModifiedDate >= FromToDateTime[0] && u.ModifiedDate <= FromToDateTime[1] && u.Performer != null && u.Performer != "" select u).ToList();
                                GetStaffIssueManagementReportList1 = AssignedList.Union(ResolvedList).ToList();
                            }
                        }
                        //if (ExptXl == 1)
                        //{
                        //    var List = GetStaffIssueManagementReportList1;
                        //    int i = 1;
                        //    foreach (var item1 in GetStaffIssueManagementReportList1)
                        //    {
                        //        if (item1.DueDate != null)
                        //        {
                        //            item1.DifferenceInHours = DateTime.Now - item1.DueDate;
                        //            if (item1.ModifiedDate != null && item1.Status == "Completed")
                        //            {
                        //                item1.DifferenceInHours1 = item1.ModifiedDate - item1.DueDate;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            item1.DifferenceInHours = TimeSpan.Zero;
                        //        }
                        //        item1.Id = i;
                        //        i++;
                        //    }
                        //    string SearchCriteria = "";
                        //    if (!string.IsNullOrEmpty(Campus))
                        //    {
                        //        SearchCriteria += "<b>Campus</b> :" + Campus;
                        //    }
                        //    if (!string.IsNullOrEmpty(IssueGroup))
                        //    {
                        //        SearchCriteria += "&nbsp&nbsp<b>IssueGroup</b> :" + IssueGroup;
                        //    }
                        //    if (!string.IsNullOrEmpty(IssueType))
                        //    {
                        //        SearchCriteria += "&nbsp&nbsp<b>Issue Type</b> :" + IssueType;
                        //    }
                        //    if (!string.IsNullOrEmpty(Performer))
                        //    {

                        //        SearchCriteria += "&nbsp&nbsp<b>Performer</b> :" + us.GetUserNameByUserId(Performer);
                        //    }
                        //    if (!string.IsNullOrEmpty(FromDate))
                        //    {
                        //        SearchCriteria += "&nbsp&nbsp<b>From</b> :" + FromDate;
                        //    }
                        //    if (!string.IsNullOrEmpty(FromDate))
                        //    {
                        //        SearchCriteria += "&nbsp&nbsp<b>To</b> :" + ToDate;
                        //    }
                        //    string title = "StaffIssueManagementReport" + DateTime.Now.ToString("0:dd/MM/YY") + "";
                        //    string logopath = ConfigurationManager.AppSettings["SLAStatus"].ToString() + "green.jpg";
                        //    //string[] filesindirectory = Directory.GetFiles(Server.MapPath("~/Images/green.jpg"));
                        //    string files = Server.MapPath("~/Images/green.jpg");
                        //    string headerTable = @"<Table><tr><td colspan='8' align='center' style='font-size: large;<img src=" + files + ">'><b> The Indian Public School,Coimbatore</b></td></tr><br>" +
                        //        @"<Table><tr><td colspan='13' align='center' style='font-size: large;'><b>Staff Issue Management Report</b></td></tr><br>" +
                        //        //"<tr><td colspan='13' align='center' style='font-size: medium;'><b>Campus</b> : " + Campus + " " + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp <b>Grade : </b> " + IssueGroup + "</td></tr>";
                        //    "<tr><td colspan='13' align='center' style='font-size: medium;'>" + SearchCriteria + "</td></tr>";
                        //    headerTable = headerTable + "</b></Table>";
                        //    ExptToXL_StaffIssueManagement(List, title, (items => new
                        //    {
                        //        SlNo = items.Id,
                        //        Performer = items.Performer != null ? us.GetUserNameByUserId(items.Performer) : "",
                        //        Issue_Number = items.IssueNumber,
                        //        Campus = items.BranchCode,
                        //        Issue_Group = items.IssueGroup,
                        //        Issue_Type = items.IssueType,
                        //        Description = items.Description,
                        //        Created_By = items.CreatedBy,
                        //        Created_Date = items.CreatedDate.Value.ToString("dd/MM/yyyy"),
                        //        Status = items.Status,
                        //        Resolution = items.Resolution,
                        //        Resolved_Date = items.ModifiedDate != null ? items.ModifiedDate.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                        //        Due_Date = items.DueDate != null ? items.DueDate.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                        //        SLA_Status = items.Status == "Completed" ? "<img src=" + logopath + "/>" : "",
                        //        Due_Date_Type = items.ModifiedDate == null ? "Not Completed" : GetDueDateType(items.DifferenceInHours1)

                        //    }), headerTable);
                        //    return new EmptyResult();
                        //}                        
                        foreach (var item1 in GetStaffIssueManagementReportList1)
                        {
                            if (item1.DueDate != null)
                            {
                                item1.DifferenceInHours = DateTime.Now - item1.DueDate;
                                if (item1.ModifiedDate != null && (item1.Status == "Completed" || item1.Status == "ResolveIssueRejection"))
                                {
                                    item1.DifferenceInHours1 = item1.ModifiedDate - item1.DueDate;
                                    if (item1.AssignedDate != null)
                                    {
                                        item1.DifferenceInHours2 = item1.ModifiedDate - item1.AssignedDate;
                                    }
                                    else
                                    {
                                        item1.DifferenceInHours2 = TimeSpan.Zero;
                                    }
                                }
                                else
                                {
                                    if (item1.AssignedDate != null)
                                    {
                                        item1.DifferenceInHours2 = DateTime.Now - item1.AssignedDate;
                                    }
                                    else
                                    {
                                        item1.DifferenceInHours2 = TimeSpan.Zero;
                                    }
                                }
                            }
                            else
                            {
                                item1.DifferenceInHours = TimeSpan.Zero;
                                item1.DifferenceInHours2 = TimeSpan.Zero;
                            }
                        }
                        long totalRecords = GetStaffIssueManagementReportList1.Count;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows = (
                                from items in GetStaffIssueManagementReportList1
                                select new
                                {
                                    i = items.Id,
                                    cell = new string[] 
                                     { 
                                         
                                         items.Performer!=null?us.GetUserNameByUserId(items.Performer):"",
                                         "<a href='/StaffIssues/ShowStaffIssue?id=" + items.ProcessRefId + "'>" + items.IssueNumber + "</a>",
                                         items.BranchCode,
                                         items.IssueGroup,
                                         items.IssueType,
                                         items.Description,
                                         items.CreatedBy,
                                         items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
                                         items.Status,
                                         items.Resolution!=null?items.Resolution:"",
                                         items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd/MM/yyyy HH:mm:ss"):"",                                         
                                         items.DueDate!=null?items.DueDate.Value.ToString("dd/MM/yyyy HH:mm:ss"):"",
                                         items.Status == "Completed"?"Completed":items.DifferenceInHours.Value.TotalHours.ToString(),                                         
                                         items.ModifiedDate == null?"NotCompleted":items.DifferenceInHours1.Value.TotalHours.ToString(),
                                         items.DueDate!=null?items.DifferenceInHours2.Value.Days+"d "+ items.DifferenceInHours2.Value.Hours +"h " + items.DifferenceInHours2.Value.Minutes + "m ":"0d 0h 0m"
                                     }
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
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult GetStaffIssueManagementReportChart(string Campus, string IssueGroup, string IssueType, string Performer, string FromDate, string ToDate, string DateType, string DueDateType)
        {
            try
            {
                var DeptCode = Session["DeptCode"] != null ? Session["DeptCode"].ToString() : "";
                var Performer1 = Session["Performer"] != null ? Session["Performer"].ToString() : "";
                if (!string.IsNullOrEmpty(Performer) || !string.IsNullOrEmpty(Performer1))
                {
                    StaffIssuesService sis = new StaffIssuesService();
                    Dictionary<string, object> Criteria = new Dictionary<string, object>();
                    string sord = "Asc";
                    string sidx = "Id";
                    if (!string.IsNullOrEmpty(Campus))
                    { Criteria.Add("BranchCode", Campus); }
                    if (!string.IsNullOrEmpty(IssueGroup))
                    { Criteria.Add("IssueGroup", IssueGroup); }
                    if (!string.IsNullOrEmpty(IssueType))
                    { Criteria.Add("IssueType", IssueType); }
                    if (!string.IsNullOrEmpty(Performer))
                    { Criteria.Add("Performer", Performer); }
                    if (string.IsNullOrEmpty(Performer))
                    { Criteria.Add("Performer", Performer1); }
                    if (string.IsNullOrEmpty(IssueGroup))
                    {
                        if (!string.IsNullOrEmpty(DeptCode))
                        { Criteria.Add("IssueGroup", DeptCode); }
                    }
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    if (!string.IsNullOrEmpty(FromDate))
                    {

                        if (!string.IsNullOrEmpty(ToDate))
                        {
                            DateTime[] FromToDate = new DateTime[2];
                            FromToDate[0] = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            FromToDate[1] = DateTime.Parse(ToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            string ToDate1 = string.Format("{0:dd/MM/yyyy}", FromToDate[1]);
                            ToDate1 = ToDate1 + " 23:59:59";
                            FromToDate[1] = DateTime.Parse(ToDate1, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);

                            Criteria.Add(DateType, FromToDate);
                        }
                        else
                        {
                            DateTime[] fromto = new DateTime[2];
                            fromto[0] = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            fromto[1] = DateTime.Now;
                            //string To = string.Format("{0:dd/MM/yyyy}", fromto[0]);
                            //fromto[1] = Convert.ToDateTime(To + " " + "23:59:59");
                            Criteria.Add(DateType, fromto);
                        }
                    }


                    Dictionary<long, IList<StaffIssueManagementReport_vw>> GetStaffIssueManagementReportList = sis.GetStaffIssueManagementReport_vwListWithPaging(0, 9999999, sidx, sord, Criteria);
                    if (GetStaffIssueManagementReportList != null && GetStaffIssueManagementReportList.FirstOrDefault().Key > 0)
                    {

                        if (!string.IsNullOrEmpty(DueDateType))
                        {
                            foreach (var item in GetStaffIssueManagementReportList.FirstOrDefault().Value)
                            {
                                if (item.ModifiedDate != null && item.DueDate != null)
                                {
                                    item.DifferenceInHours = item.ModifiedDate - item.DueDate;
                                }
                            }
                            var DueDateList = new List<StaffIssueManagementReport_vw>();
                            if (DueDateType == "1")
                            {
                                DueDateList = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.DueDate != null && u.ModifiedDate != null && u.DifferenceInHours.Value.TotalHours <= 0 select u).ToList();
                            }
                            if (DueDateType == "2")
                            {
                                DueDateList = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.DueDate != null && u.ModifiedDate != null && u.DifferenceInHours.Value.TotalHours > 0 && u.DifferenceInHours.Value.TotalHours <= 12 select u).ToList();
                            }
                            if (DueDateType == "3")
                            {
                                DueDateList = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.DueDate != null && u.ModifiedDate != null && u.DifferenceInHours.Value.TotalHours > 0 && u.DifferenceInHours.Value.TotalHours > 12 select u).ToList();
                            }
                            var DueDateWiseList = (from fpl in DueDateList
                                                   group fpl by fpl.Performer into fplt
                                                   //where fplt.FirstOrDefault().IssueNumber == ""
                                                   select new
                                                   {
                                                       cell = new string[]
                                                       {                                                                                                                        
                                                            fplt.FirstOrDefault().Id.ToString(),
                                                            fplt.FirstOrDefault().InstanceId.ToString(),  
                                                            fplt.FirstOrDefault().ProcessRefId.ToString(),  
                                                            fplt.FirstOrDefault().Performer!=null?fplt.FirstOrDefault().Performer.ToString():"",  
                                                            fplt.Sum(x => x.Available).ToString(),                                                                                                                    
                                                            fplt.Sum(x => x.Assigned).ToString(),                                                                                                                         
                                                            fplt.Sum(x => x.Resolved).ToString()
                                                               
                                                       }
                                                   }).ToList();
                            return Json(DueDateWiseList, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            var List1 = (from fpl in GetStaffIssueManagementReportList.FirstOrDefault().Value
                                         group fpl by fpl.Performer into fplt
                                         //where fplt.FirstOrDefault().IssueNumber == ""
                                         select new
                                         {
                                             cell = new string[]
                                                       {                                                                                                                        
                                                            fplt.FirstOrDefault().Id.ToString(),
                                                            fplt.FirstOrDefault().InstanceId.ToString(),  
                                                            fplt.FirstOrDefault().ProcessRefId.ToString(),  
                                                            fplt.FirstOrDefault().Performer!=null?fplt.FirstOrDefault().Performer.ToString():"",  
                                                            fplt.Sum(x => x.Available).ToString(),                                                                                                                    
                                                            fplt.Sum(x => x.Assigned).ToString(),                                                                                                                         
                                                            fplt.Sum(x => x.Resolved).ToString()
                                                               
                                                       }

                                         }).ToList();
                            return Json(List1, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json("", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult GetStaffIssueManagementDateWiseReportChart(string Campus, string IssueGroup, string IssueType, string Performer, string FromDate, string ToDate)
        {
            try
            {
                var DeptCode = Session["DeptmntCode"] != null ? Session["DeptmntCode"].ToString() : "";
                var Performer1 = Session["PerformerId"] != null ? Session["PerformerId"].ToString() : "";
                if (!string.IsNullOrEmpty(Performer) || !string.IsNullOrEmpty(Performer1))
                {
                    StaffIssuesService sis = new StaffIssuesService();
                    Dictionary<string, object> Criteria = new Dictionary<string, object>();
                    string sord = "Asc";
                    string sidx = "Id";
                    if (!string.IsNullOrEmpty(Campus))
                    { Criteria.Add("BranchCode", Campus); }
                    if (!string.IsNullOrEmpty(IssueGroup))
                    { Criteria.Add("IssueGroup", IssueGroup); }
                    if (!string.IsNullOrEmpty(IssueType))
                    { Criteria.Add("IssueType", IssueType); }
                    if (!string.IsNullOrEmpty(Performer))
                    { Criteria.Add("Performer", Performer); }
                    if (string.IsNullOrEmpty(IssueGroup))
                    { Criteria.Add("IssueGroup", DeptCode); }
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    DateTime[] FromToDateTime = new DateTime[2];
                    if (!string.IsNullOrEmpty(FromDate))
                    {
                        if (!string.IsNullOrEmpty(ToDate))
                        {
                            FromToDateTime[0] = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            FromToDateTime[1] = DateTime.Parse(ToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            string ToDate1 = string.Format("{0:dd/MM/yyyy}", FromToDateTime[1]);
                            ToDate1 = ToDate1 + " 23:59:59";
                            FromToDateTime[1] = DateTime.Parse(ToDate1, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        }
                        else
                        {
                            FromToDateTime[0] = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            FromToDateTime[1] = DateTime.Now;
                            //string To = string.Format("{0:dd/MM/yyyy}", fromto[0]);
                            //fromto[1] = Convert.ToDateTime(To + " " + "23:59:59");                        
                        }
                    }
                    if (!string.IsNullOrEmpty(FromDate))
                    {
                        Dictionary<long, IList<StaffIssueManagementReport_vw>> GetStaffIssueManagementReportList = sis.GetStaffIssueManagementReport_vwListWithPaging(0, 9999999, sidx, sord, Criteria);
                        if (GetStaffIssueManagementReportList != null && GetStaffIssueManagementReportList.FirstOrDefault().Key > 0)
                        {

                            //var AssignedCount = (from fpl in GetStaffIssueManagementReportList.FirstOrDefault().Value
                            //                     group fpl by fpl.Performer into fplt
                            //                     // where fplt.FirstOrDefault().Assigned == 1 && fplt.FirstOrDefault().Performer != null && fplt.FirstOrDefault().Performer != ""
                            //                     select new
                            //                     {
                            //                         cell = new string[] {
                            //                     fplt.FirstOrDefault().Performer!=null?fplt.FirstOrDefault().Performer.ToString():"",  
                            //                     fplt.Sum(x => x.Assigned).ToString() 
                            //                 }
                            //                     }).ToList();
                            var List1 = (from fpl in GetStaffIssueManagementReportList.FirstOrDefault().Value
                                         group fpl by fpl.Performer into fplt
                                         //where fplt.FirstOrDefault().Performer != null && fplt.FirstOrDefault().Performer != ""
                                         select new
                                         {
                                             cell = new string[] {
                                                 fplt.FirstOrDefault().Id.ToString(),
                                                 fplt.FirstOrDefault().InstanceId.ToString(),  
                                                 fplt.FirstOrDefault().ProcessRefId.ToString(),
                                                 fplt.FirstOrDefault().Performer!=null?fplt.FirstOrDefault().Performer.ToString():"UnAssigned",  
                                                 fplt.Sum(x => x.Available).ToString(),                                                                                                                    
                                                 fplt.Sum(x => x.Assigned).ToString(),                                                                                                                         
                                                 fplt.Sum(x => x.Resolved).ToString(),
                                                 fplt.Sum(x => x.TotalCount).ToString()
                                             }
                                         }).ToList();
                            var DateWiseList = (from items in GetStaffIssueManagementReportList.FirstOrDefault().Value
                                                where items.ModifiedDate >= FromToDateTime[0] && items.ModifiedDate <= FromToDateTime[1] && items.Performer != null && items.Performer != ""
                                                select items).ToList();
                            var ResolvedCount = (from fpl in DateWiseList
                                                 group fpl by fpl.Performer into fplt
                                                 //where fplt.FirstOrDefault().Performer != "" && fplt.FirstOrDefault().Performer!=null
                                                 select new
                                                 {
                                                     cell = new string[]
                                                       {                                                                                                                        
                                                            fplt.FirstOrDefault().Id.ToString(),
                                                            fplt.FirstOrDefault().InstanceId.ToString(),  
                                                            fplt.FirstOrDefault().ProcessRefId.ToString(),  
                                                            fplt.FirstOrDefault().Performer!=null?fplt.FirstOrDefault().Performer.ToString():"",  
                                                            fplt.Sum(x => x.Available).ToString(),                                                                                                                    
                                                            fplt.Sum(x => x.Assigned).ToString(),                                                                                                                         
                                                            fplt.Sum(x => x.Resolved).ToString()
                                                               
                                                       }

                                                 }).ToList();
                            foreach (var items in List1)
                            {
                                long Count = 0;
                                items.cell[6] = Convert.ToInt64(Count).ToString();
                                foreach (var item1 in ResolvedCount)
                                {
                                    if (items.cell[3] == item1.cell[3])
                                    {
                                        items.cell[6] = item1.cell[6];
                                        items.cell[7] = (Convert.ToInt64(items.cell[7]) + Convert.ToInt64(item1.cell[6])).ToString();
                                        break;
                                    }
                                }
                            }
                            return Json(List1, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json("", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json("", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        //public void ExptToXL_StaffIssueManagement<T, TResult>(IList<T> stuList, string filename, Func<T, TResult> selector, string headerTable)
        //{
        //    Response.ClearContent();
        //    Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".xls");
        //    Response.ContentType = "application/vnd.ms-excel";
        //    System.IO.StringWriter stw = new System.IO.StringWriter();
        //    HtmlTextWriter htextw = new HtmlTextWriter(stw);
        //    DataGrid dg = new DataGrid();
        //    dg.HeaderStyle.BackColor = System.Drawing.Color.FromName("#206FAE");
        //    dg.HeaderStyle.Font.Bold = true;
        //    dg.HeaderStyle.ForeColor = System.Drawing.Color.White;
        //    dg.DataSource = stuList.Select(selector);
        //    dg.DataBind();
        //    dg.RenderControl(htextw);
        //    Response.Write(headerTable);
        //    Response.Write(stw.ToString());            
        //    Response.End();
        //    Response.Clear();
        //}
        public string GetDueDateType(TimeSpan? DueDateValue)
        {
            try
            {
                if (DueDateValue.Value.TotalHours <= 0)
                {
                    return "Completed Before Due Date";
                }
                else if (DueDateValue.Value.TotalHours > 0 && DueDateValue.Value.TotalHours <= 24)
                {
                    return "Completed Above Due Date within 24 hours";
                }
                else if (DueDateValue.Value.TotalHours > 24)
                {
                    return "Completed Above Due Date after 24 hours";
                }
                else
                    return "";
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string GetSLAStatus(TimeSpan? DueDateValue, DateTime? DueDate)
        {
            try
            {
                if (DueDate != null)
                {
                    if (DueDateValue.Value.TotalHours <= 0)
                    {
                        return "yellow";
                    }
                    else if (DueDateValue.Value.TotalHours > 0 && DueDateValue.Value.TotalHours <= 24)
                    {
                        return "orange";
                    }
                    else if (DueDateValue.Value.TotalHours > 24)
                    {
                        return "red";
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "blue";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void ExportToExcelSIMDR(string Campus, string IssueGroup, string IssueType, string FromDate, string ToDate, string HeaderName, string Result, string Performer, int rows, string sidx, string sord, int? page = 1)
        {
            //for export
            ExcelPackage objExcelPackage = new ExcelPackage();   //create new workbook

            //string[] filesindirectory = Directory.GetFiles(Server.MapPath("~/Images"));
            string[] files = new string[5];
            files[0] = Server.MapPath("~/Images/green.jpg");
            files[1] = Server.MapPath("~/Images/orange.jpg");
            files[2] = Server.MapPath("~/Images/redblink3.gif");
            files[3] = Server.MapPath("~/Images/blue.jpg");
            files[4] = Server.MapPath("~/Images/yellow.jpg");
            int count = 0;
            ExcelWorksheet ews = objExcelPackage.Workbook.Worksheets.Add("Worksheet" + count); //create new worksheet
            ews.View.ZoomScale = 100;
            ews.View.ShowGridLines = true;
            ews.Cells["A3"].Value = "Sl.No";
            ews.Cells["B3"].Value = "Performer";
            ews.Cells["C3"].Value = "Issue_Number";
            ews.Cells["D3"].Value = "Campus";
            ews.Cells["E3"].Value = "Issue_Group";
            ews.Cells["F3"].Value = "Issue_Type";
            ews.Cells["G3"].Value = "Description";
            ews.Cells["H3"].Value = "Created_By";
            ews.Cells["I3"].Value = "Created_Date";
            ews.Cells["J3"].Value = "Status";
            ews.Cells["K3"].Value = "Resolution";
            ews.Cells["L3"].Value = "Resolved_Date";
            ews.Cells["M3"].Value = "Due_Date";
            ews.Cells["N3"].Value = "SLA_Status";
            ews.Cells["O3"].Value = "Due_Date_Type";
            ews.Cells["P3"].Value = "Issue_Hours";
            Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#B7DEE8");
            ews.Cells["A3:P3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ews.Cells["A3:P3"].Style.Fill.BackgroundColor.SetColor(colFromHex);
            //For Title
            string SearchCriteria = "";
            if (!string.IsNullOrEmpty(Campus))
            {
                SearchCriteria += "Campus : " + Campus;
            }
            if (!string.IsNullOrEmpty(IssueGroup))
            {
                SearchCriteria += " IssueGroup : " + IssueGroup;
            }
            if (!string.IsNullOrEmpty(IssueType))
            {
                SearchCriteria += " Issue Type : " + IssueType;
            }
            if (!string.IsNullOrEmpty(Performer))
            {

                SearchCriteria += " Performer : " + us.GetUserNameByUserId(Performer);
            }
            if (!string.IsNullOrEmpty(FromDate))
            {
                SearchCriteria += " From : " + FromDate;
            }
            if (!string.IsNullOrEmpty(FromDate))
            {
                SearchCriteria += " To : " + ToDate;
            }
            ews.Cells["A1:P1"].Merge = true;
            ews.Cells["A2:P2"].Merge = true;
            ews.Cells["A2:P2"].Value = SearchCriteria;
            ews.Cells["A1:P1"].Value = "STAFF ISSUE MANAGEMENT DATEWISE REPORT";
            ews.Cells["A1:P1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["A2:P2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["A1:P1"].Style.Font.Bold = true;
            ews.Cells["A2:P2"].Style.Font.Bold = true;
            ews.Cells["A3:P3"].Style.Font.Bold = true;
            int i = 4;
            int rowIndex = 3;
            int columnIndex = 13;
            //int j = 13;
            IList<StaffIssueManagementReport_vw> StaffIssueManagementReportDetails = new List<StaffIssueManagementReport_vw>();
            StaffIssueManagementReportDetails = StaffIssueManagementDateWiseReportWithCriteria(Campus, IssueGroup, IssueType, FromDate, ToDate, HeaderName, Result, Performer, rows, sidx, sord, page);
            if (StaffIssueManagementReportDetails.Count > 0)
            {
                for (int k = 0; k < StaffIssueManagementReportDetails.Count; k++)
                {
                    ews.Cells["A" + i].Value = StaffIssueManagementReportDetails[k].Id;
                    ews.Cells["B" + i].Value = StaffIssueManagementReportDetails[k].Performer;
                    ews.Cells["C" + i].Value = StaffIssueManagementReportDetails[k].IssueNumber;
                    ews.Cells["D" + i].Value = StaffIssueManagementReportDetails[k].BranchCode;
                    ews.Cells["E" + i].Value = StaffIssueManagementReportDetails[k].IssueGroup;
                    ews.Cells["F" + i].Value = StaffIssueManagementReportDetails[k].IssueType;
                    ews.Cells["G" + i].Value = StaffIssueManagementReportDetails[k].Description;
                    ews.Cells["H" + i].Value = StaffIssueManagementReportDetails[k].CreatedBy;
                    ews.Cells["I" + i].Value = StaffIssueManagementReportDetails[k].CreatedDate != null ? StaffIssueManagementReportDetails[k].CreatedDate.Value.ToString("dd/MM/yyyy") : "";
                    ews.Cells["J" + i].Value = StaffIssueManagementReportDetails[k].Status;
                    ews.Cells["K" + i].Value = StaffIssueManagementReportDetails[k].Resolution;
                    ews.Cells["L" + i].Value = StaffIssueManagementReportDetails[k].ModifiedDate != null ? StaffIssueManagementReportDetails[k].ModifiedDate.Value.ToString("dd/MM/yyyy HH:mm:ss") : "";
                    ews.Cells["M" + i].Value = StaffIssueManagementReportDetails[k].DueDate != null ? StaffIssueManagementReportDetails[k].DueDate.Value.ToString("dd/MM/yyyy HH:mm:ss") : "";
                    ews.Cells["N" + i].Value = "";
                    ews.Cells["O" + i].Value = StaffIssueManagementReportDetails[k].ModifiedDate == null ? "Not Completed" : GetDueDateType(StaffIssueManagementReportDetails[k].DifferenceInHours1);
                    ews.Cells["P" + i].Value = StaffIssueManagementReportDetails[k].DueDate != null ? StaffIssueManagementReportDetails[k].DifferenceInHours2.Value.Days + "d " + StaffIssueManagementReportDetails[k].DifferenceInHours2.Value.Hours + "h " + StaffIssueManagementReportDetails[k].DifferenceInHours2.Value.Minutes + "m " : "0d 0h 0m";
                    ews.Cells["G" + i].Style.WrapText = true;
                    ews.Cells["K" + i].Style.WrapText = true;
                    var SLA_Status = StaffIssueManagementReportDetails[k].Status == "Completed" ? "Completed" : GetSLAStatus(StaffIssueManagementReportDetails[k].DifferenceInHours, StaffIssueManagementReportDetails[k].DueDate);
                    if (SLA_Status == "Completed")
                    {
                        System.Web.UI.WebControls.Image TEST_IMAGE = new System.Web.UI.WebControls.Image();
                        System.Drawing.Image myImage = System.Drawing.Image.FromFile(files[0]);
                        var pic = ews.Drawings.AddPicture(count.ToString(), myImage);
                        // Row, RowoffsetPixel, Column, ColumnOffSetPixel
                        pic.SetSize(10, 10);
                        pic.SetPosition(rowIndex, 5, columnIndex, 35);
                    }
                    if (SLA_Status == "orange")
                    {
                        System.Web.UI.WebControls.Image TEST_IMAGE = new System.Web.UI.WebControls.Image();
                        System.Drawing.Image myImage = System.Drawing.Image.FromFile(files[1]);
                        var pic = ews.Drawings.AddPicture(count.ToString(), myImage);
                        // Row, RowoffsetPixel, Column, ColumnOffSetPixel
                        pic.SetSize(10, 10);
                        pic.SetPosition(rowIndex, 5, columnIndex, 35);
                    }
                    if (SLA_Status == "red")
                    {
                        System.Web.UI.WebControls.Image TEST_IMAGE = new System.Web.UI.WebControls.Image();
                        System.Drawing.Image myImage = System.Drawing.Image.FromFile(files[2]);
                        var pic = ews.Drawings.AddPicture(count.ToString(), myImage);
                        // Row, RowoffsetPixel, Column, ColumnOffSetPixel
                        pic.SetSize(10, 10);
                        pic.SetPosition(rowIndex, 5, columnIndex, 35);
                    }
                    if (SLA_Status == "blue")
                    {
                        System.Web.UI.WebControls.Image TEST_IMAGE = new System.Web.UI.WebControls.Image();
                        System.Drawing.Image myImage = System.Drawing.Image.FromFile(files[3]);
                        var pic = ews.Drawings.AddPicture(count.ToString(), myImage);
                        // Row, RowoffsetPixel, Column, ColumnOffSetPixel
                        pic.SetSize(10, 10);
                        pic.SetPosition(rowIndex, 5, columnIndex, 35);
                    }
                    if (SLA_Status == "yellow")
                    {
                        System.Web.UI.WebControls.Image TEST_IMAGE = new System.Web.UI.WebControls.Image();
                        System.Drawing.Image myImage = System.Drawing.Image.FromFile(files[4]);
                        var pic = ews.Drawings.AddPicture(count.ToString(), myImage);
                        // Row, RowoffsetPixel, Column, ColumnOffSetPixel
                        pic.SetSize(10, 10);
                        pic.SetPosition(rowIndex, 5, columnIndex, 35);
                    }
                    count++;
                    rowIndex++;
                    i = i + 1;
                }
            }
            ews.Cells[ews.Dimension.Address].AutoFitColumns();
            ews.Column(7).Width = 60;
            ews.Column(11).Width = 60;
            string Todaydate = DateTime.Now.ToString("dd/MM/yyyy");
            string FileName = "StaffIssueManagementDateWiseReport" + "-On-" + Todaydate; ;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;  filename=" + FileName + ".xlsx");
            byte[] File = objExcelPackage.GetAsByteArray();
            Response.BinaryWrite(File);
            Response.End();
        }
        public IList<StaffIssueManagementReport_vw> StaffIssueManagementDateWiseReportWithCriteria(string Campus, string IssueGroup, string IssueType, string FromDate, string ToDate, string HeaderName, string Result, string Performer, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StaffIssuesService sis = new StaffIssuesService();
                List<StaffIssueManagementReport_vw> StaffIssueManagementReportList = new List<StaffIssueManagementReport_vw>();
                Dictionary<string, object> Criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(Campus))
                { Criteria.Add("BranchCode", Campus); }
                if (!string.IsNullOrEmpty(IssueGroup))
                { Criteria.Add("IssueGroup", IssueGroup); }
                if (!string.IsNullOrEmpty(IssueType))
                { Criteria.Add("IssueType", IssueType); }
                if (!string.IsNullOrEmpty(Performer))
                { Criteria.Add("Performer", Performer); }
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                DateTime[] FromToDateTime = new DateTime[2];
                if (HeaderName == "Resolved" || HeaderName == "TotalCount")
                {
                    if (!string.IsNullOrEmpty(FromDate))
                    {
                        if (!string.IsNullOrEmpty(ToDate))
                        {
                            FromToDateTime[0] = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            FromToDateTime[1] = DateTime.Parse(ToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            string ToDate1 = string.Format("{0:dd/MM/yyyy}", FromToDateTime[1]);
                            ToDate1 = ToDate1 + " 23:59:59";
                            FromToDateTime[1] = DateTime.Parse(ToDate1, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            if (HeaderName == "Resolved") { Criteria.Add("ModifiedDate", FromToDateTime); }
                        }
                        else
                        {
                            FromToDateTime[0] = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            FromToDateTime[1] = DateTime.Now;
                            if (HeaderName == "Resolved") { Criteria.Add("ModifiedDate", FromToDateTime); }
                            //string To = string.Format("{0:dd/MM/yyyy}", fromto[0]);
                            //fromto[1] = Convert.ToDateTime(To + " " + "23:59:59");                        
                        }
                    }
                }
                Dictionary<long, IList<StaffIssueManagementReport_vw>> GetStaffIssueManagementReportList = sis.GetStaffIssueManagementReport_vwListWithPaging(page - 1, 9999999, sidx, sord, Criteria);
                if (GetStaffIssueManagementReportList != null && GetStaffIssueManagementReportList.Count > 0)
                {
                    var GetStaffIssueManagementReportList1 = new List<StaffIssueManagementReport_vw>();
                    if (HeaderName == "Assigned")
                    {
                        if (!string.IsNullOrEmpty(Performer))
                        {
                            GetStaffIssueManagementReportList1 = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Assigned > 0 select u).ToList();
                        }
                        else
                        {
                            GetStaffIssueManagementReportList1 = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Assigned > 0 && Performer != "" && Performer != null select u).ToList();
                        }
                    }
                    if (HeaderName == "Resolved")
                    {
                        if (!string.IsNullOrEmpty(Performer))
                        {
                            GetStaffIssueManagementReportList1 = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Resolved > 0 select u).ToList();
                        }
                        else
                        {
                            GetStaffIssueManagementReportList1 = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Resolved > 0 && Performer != "" && Performer != null select u).ToList();
                        }
                    }
                    if (HeaderName == "TotalCount")
                    {
                        if (!string.IsNullOrEmpty(Performer))
                        {
                            var AssignedList = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Assigned > 0 select u).ToList();
                            var ResolvedList = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Resolved > 0 && u.ModifiedDate >= FromToDateTime[0] && u.ModifiedDate <= FromToDateTime[1] && u.Performer != null && u.Performer != "" select u).ToList();
                            GetStaffIssueManagementReportList1 = AssignedList.Union(ResolvedList).ToList();
                        }
                        else
                        {
                            var AssignedList = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Assigned > 0 select u).ToList();
                            var ResolvedList = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Resolved > 0 && u.ModifiedDate >= FromToDateTime[0] && u.ModifiedDate <= FromToDateTime[1] && u.Performer != null && u.Performer != "" select u).ToList();
                            GetStaffIssueManagementReportList1 = AssignedList.Union(ResolvedList).ToList();
                        }
                    }
                    //StaffIssueManagementReportList = GetStaffIssueManagementReportList1;
                    var List = GetStaffIssueManagementReportList1;
                    int i = 1;
                    foreach (var item1 in GetStaffIssueManagementReportList1)
                    {
                        if (item1.DueDate != null)
                        {
                            item1.DifferenceInHours = DateTime.Now - item1.DueDate;
                            if (item1.ModifiedDate != null && (item1.Status == "Completed" || item1.Status == "ResolveIssueRejection"))
                            {
                                item1.DifferenceInHours1 = item1.ModifiedDate - item1.DueDate;
                                if (item1.AssignedDate != null)
                                {
                                    item1.DifferenceInHours2 = item1.ModifiedDate - item1.AssignedDate;
                                }
                                else
                                {
                                    item1.DifferenceInHours2 = TimeSpan.Zero;
                                }
                            }
                            else
                            {
                                if (item1.AssignedDate != null)
                                {
                                    item1.DifferenceInHours2 = DateTime.Now - item1.AssignedDate;
                                }
                                else
                                {
                                    item1.DifferenceInHours2 = TimeSpan.Zero;
                                }
                            }
                        }
                        else
                        {
                            item1.DifferenceInHours = TimeSpan.Zero;
                            item1.DifferenceInHours2 = TimeSpan.Zero;
                        }
                        item1.Id = i;
                        i++;
                    }
                    StaffIssueManagementReportList = List;
                    return StaffIssueManagementReportList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult StaffIssueManagementDateWiseReportPDF(string Campus, string IssueGroup, string IssueType, string FromDate, string ToDate, string HeaderName, string Result, string Performer, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                IList<StaffIssueManagementReport_vw> StaffIssueManagementReport = new List<StaffIssueManagementReport_vw>();
                StaffIssueManagementReport = StaffIssueManagementDateWiseReportWithCriteria(Campus, IssueGroup, IssueType, FromDate, ToDate, HeaderName, Result, Performer, rows, sidx, sord, page);
                if (StaffIssueManagementReport.Count > 0)
                {
                    StaffIssueManagementReportPDF staffissuemanagementreportpdf = new StaffIssueManagementReportPDF();
                    staffissuemanagementreportpdf.StaffIssueManagementReportList = StaffIssueManagementReport;
                    ViewBag.Campus = Campus;
                    ViewBag.IssueGroup = IssueGroup;
                    ViewBag.IssueType = IssueType;
                    ViewBag.FromDate = FromDate;
                    ViewBag.ToDate = ToDate;
                    if (!string.IsNullOrEmpty(Performer))
                    {
                        ViewBag.Performer = us.GetUserNameByUserId(Performer);
                    }
                    return new Rotativa.ViewAsPdf("StaffIssueManagementDateWiseReportPDF", staffissuemanagementreportpdf)
                    {
                        FileName = "Staff Issue Management DateWise Report On-" + DateTime.Today.ToString("dd/MM/yyyy") + ".pdf",
                        PageOrientation = Rotativa.Options.Orientation.Landscape,
                        PageMargins = new Rotativa.Options.Margins(0, 0, 0, 0)
                    };
                }
                else
                {
                    StaffIssueManagementReportPDF staffissuemanagementreportpdf = new StaffIssueManagementReportPDF();
                    return new Rotativa.ViewAsPdf("StaffIssueManagementDateWiseReportPDF", staffissuemanagementreportpdf)
                    {
                        FileName = "Staff Issue Management DateWise Report On-" + DateTime.Today.ToString("dd/MM/yyyy") + ".pdf",
                        PageOrientation = Rotativa.Options.Orientation.Landscape,
                        PageMargins = new Rotativa.Options.Margins(0, 0, 0, 0)
                    };
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void ExportToExcelSIMR(string Campus, string IssueGroup, string IssueType, string DueDateType, string DateType, string FromDate, string ToDate, string CountName, string Result, string Performer, int rows, string sidx, string sord, int? page = 1)
        {
            //for export
            ExcelPackage objExcelPackage = new ExcelPackage();   //create new workbook

            //string[] filesindirectory = Directory.GetFiles(Server.MapPath("~/Images"));
            string[] files = new string[5];
            files[0] = Server.MapPath("~/Images/green.jpg");
            files[1] = Server.MapPath("~/Images/orange.jpg");
            files[2] = Server.MapPath("~/Images/redblink3.gif");
            files[3] = Server.MapPath("~/Images/blue.jpg");
            files[4] = Server.MapPath("~/Images/yellow.jpg");
            int count = 0;
            ExcelWorksheet ews = objExcelPackage.Workbook.Worksheets.Add("Worksheet" + count); //create new worksheet
            ews.View.ZoomScale = 100;
            ews.View.ShowGridLines = true;
            ews.Cells["A3"].Value = "Sl.No";
            ews.Cells["B3"].Value = "Performer";
            ews.Cells["C3"].Value = "Issue_Number";
            ews.Cells["D3"].Value = "Campus";
            ews.Cells["E3"].Value = "Issue_Group";
            ews.Cells["F3"].Value = "Issue_Type";
            ews.Cells["G3"].Value = "Description";
            ews.Cells["H3"].Value = "Created_By";
            ews.Cells["I3"].Value = "Created_Date";
            ews.Cells["J3"].Value = "Status";
            ews.Cells["K3"].Value = "Resolution";
            ews.Cells["L3"].Value = "Resolved_Date";
            ews.Cells["M3"].Value = "Due_Date";
            ews.Cells["N3"].Value = "SLA_Status";
            ews.Cells["O3"].Value = "Due_Date_Type";
            ews.Cells["P3"].Value = "Issue_Hours";
            Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#B7DEE8");
            ews.Cells["A3:P3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ews.Cells["A3:P3"].Style.Fill.BackgroundColor.SetColor(colFromHex);
            //For Title
            string SearchCriteria = "";
            if (!string.IsNullOrEmpty(Campus))
            {
                SearchCriteria += "Campus : " + Campus;
            }
            if (!string.IsNullOrEmpty(IssueGroup))
            {
                SearchCriteria += " IssueGroup : " + IssueGroup;
            }
            if (!string.IsNullOrEmpty(IssueType))
            {
                SearchCriteria += " Issue Type : " + IssueType;
            }
            if (!string.IsNullOrEmpty(DueDateType))
            {
                if (DueDateType == "1")
                {
                    SearchCriteria += " Due Date Type : Below Due Date";
                }
                if (DueDateType == "2")
                {
                    SearchCriteria += " Due Date Type : After Due Date Below 24 hours";
                }
                if (DueDateType == "3")
                {
                    SearchCriteria += " Due Date Type : After Due Date Above 24 hours";
                }
            }
            if (!string.IsNullOrEmpty(DateType))
            {
                if (DateType == "ModifiedDate")
                {
                    SearchCriteria += " Date Type : Resolved Date";
                }
                else
                {
                    SearchCriteria += " Date Type : " + DateType;
                }
            }
            if (!string.IsNullOrEmpty(Performer))
            {

                SearchCriteria += " Performer : " + us.GetUserNameByUserId(Performer);
            }
            if (!string.IsNullOrEmpty(FromDate))
            {
                SearchCriteria += " From : " + FromDate;
            }
            if (!string.IsNullOrEmpty(FromDate))
            {
                SearchCriteria += " To : " + ToDate;
            }
            ews.Cells["A1:P1"].Merge = true;
            ews.Cells["A2:P2"].Merge = true;
            ews.Cells["A2:P2"].Value = SearchCriteria;
            ews.Cells["A1:P1"].Value = "STAFF ISSUE MANAGEMENT REPORT";
            ews.Cells["A1:P1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["A2:P2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ews.Cells["A1:P1"].Style.Font.Bold = true;
            ews.Cells["A2:P2"].Style.Font.Bold = true;
            ews.Cells["A3:P3"].Style.Font.Bold = true;
            int i = 4;
            int rowIndex = 3;
            int columnIndex = 13;
            //int j = 13;
            IList<StaffIssueManagementReport_vw> StaffIssueManagementReportDetails = new List<StaffIssueManagementReport_vw>();
            StaffIssueManagementReportDetails = StaffIssueManagementReportWithCriteria(Campus, IssueGroup, IssueType, DueDateType, DateType, FromDate, ToDate, CountName, Result, Performer, rows, sidx, sord, page);
            if (StaffIssueManagementReportDetails.Count > 0)
            {
                for (int k = 0; k < StaffIssueManagementReportDetails.Count; k++)
                {
                    ews.Cells["A" + i].Value = StaffIssueManagementReportDetails[k].Id;
                    ews.Cells["B" + i].Value = StaffIssueManagementReportDetails[k].Performer;
                    ews.Cells["C" + i].Value = StaffIssueManagementReportDetails[k].IssueNumber;
                    ews.Cells["D" + i].Value = StaffIssueManagementReportDetails[k].BranchCode;
                    ews.Cells["E" + i].Value = StaffIssueManagementReportDetails[k].IssueGroup;
                    ews.Cells["F" + i].Value = StaffIssueManagementReportDetails[k].IssueType;
                    ews.Cells["G" + i].Value = StaffIssueManagementReportDetails[k].Description;
                    ews.Cells["H" + i].Value = StaffIssueManagementReportDetails[k].CreatedBy;
                    ews.Cells["I" + i].Value = StaffIssueManagementReportDetails[k].CreatedDate != null ? StaffIssueManagementReportDetails[k].CreatedDate.Value.ToString("dd/MM/yyyy") : "";
                    ews.Cells["J" + i].Value = StaffIssueManagementReportDetails[k].Status;
                    ews.Cells["K" + i].Value = StaffIssueManagementReportDetails[k].Resolution;
                    ews.Cells["L" + i].Value = StaffIssueManagementReportDetails[k].ModifiedDate != null ? StaffIssueManagementReportDetails[k].ModifiedDate.Value.ToString("dd/MM/yyyy HH:mm:ss") : "";
                    ews.Cells["M" + i].Value = StaffIssueManagementReportDetails[k].DueDate != null ? StaffIssueManagementReportDetails[k].DueDate.Value.ToString("dd/MM/yyyy HH:mm:ss") : "";
                    ews.Cells["N" + i].Value = "";
                    ews.Cells["O" + i].Value = StaffIssueManagementReportDetails[k].ModifiedDate == null ? "Not Completed" : GetDueDateType(StaffIssueManagementReportDetails[k].DifferenceInHours1);
                    ews.Cells["P" + i].Value = StaffIssueManagementReportDetails[k].DueDate != null ? StaffIssueManagementReportDetails[k].DifferenceInHours2.Value.Days + "d " + StaffIssueManagementReportDetails[k].DifferenceInHours2.Value.Hours + "h " + StaffIssueManagementReportDetails[k].DifferenceInHours2.Value.Minutes + "m " : "0d 0h 0m";
                    ews.Cells["G" + i].Style.WrapText = true;
                    ews.Cells["K" + i].Style.WrapText = true;
                    var SLA_Status = StaffIssueManagementReportDetails[k].Status == "Completed" ? "Completed" : GetSLAStatus(StaffIssueManagementReportDetails[k].DifferenceInHours, StaffIssueManagementReportDetails[k].DueDate);
                    if (SLA_Status == "Completed")
                    {
                        System.Web.UI.WebControls.Image TEST_IMAGE = new System.Web.UI.WebControls.Image();
                        System.Drawing.Image myImage = System.Drawing.Image.FromFile(files[0]);
                        var pic = ews.Drawings.AddPicture(count.ToString(), myImage);
                        // Row, RowoffsetPixel, Column, ColumnOffSetPixel
                        pic.SetSize(10, 10);
                        pic.SetPosition(rowIndex, 5, columnIndex, 35);
                    }
                    if (SLA_Status == "orange")
                    {
                        System.Web.UI.WebControls.Image TEST_IMAGE = new System.Web.UI.WebControls.Image();
                        System.Drawing.Image myImage = System.Drawing.Image.FromFile(files[1]);
                        var pic = ews.Drawings.AddPicture(count.ToString(), myImage);
                        // Row, RowoffsetPixel, Column, ColumnOffSetPixel
                        pic.SetSize(10, 10);
                        pic.SetPosition(rowIndex, 5, columnIndex, 35);
                    }
                    if (SLA_Status == "red")
                    {
                        System.Web.UI.WebControls.Image TEST_IMAGE = new System.Web.UI.WebControls.Image();
                        System.Drawing.Image myImage = System.Drawing.Image.FromFile(files[2]);
                        var pic = ews.Drawings.AddPicture(count.ToString(), myImage);
                        // Row, RowoffsetPixel, Column, ColumnOffSetPixel
                        pic.SetSize(10, 10);
                        pic.SetPosition(rowIndex, 5, columnIndex, 35);
                    }
                    if (SLA_Status == "blue")
                    {
                        System.Web.UI.WebControls.Image TEST_IMAGE = new System.Web.UI.WebControls.Image();
                        System.Drawing.Image myImage = System.Drawing.Image.FromFile(files[3]);
                        var pic = ews.Drawings.AddPicture(count.ToString(), myImage);
                        // Row, RowoffsetPixel, Column, ColumnOffSetPixel
                        pic.SetSize(10, 10);
                        pic.SetPosition(rowIndex, 5, columnIndex, 35);
                    }
                    if (SLA_Status == "yellow")
                    {
                        System.Web.UI.WebControls.Image TEST_IMAGE = new System.Web.UI.WebControls.Image();
                        System.Drawing.Image myImage = System.Drawing.Image.FromFile(files[4]);
                        var pic = ews.Drawings.AddPicture(count.ToString(), myImage);
                        // Row, RowoffsetPixel, Column, ColumnOffSetPixel
                        pic.SetSize(10, 10);
                        pic.SetPosition(rowIndex, 5, columnIndex, 35);
                    }
                    count++;
                    rowIndex++;
                    i = i + 1;
                }
            }
            ews.Cells[ews.Dimension.Address].AutoFitColumns();
            ews.Column(7).Width = 60;
            ews.Column(11).Width = 60;
            string Todaydate = DateTime.Now.ToString("dd/MM/yyyy");
            string FileName = "StaffIssueManagementReport" + "-On-" + Todaydate; ;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;  filename=" + FileName + ".xlsx");
            byte[] File = objExcelPackage.GetAsByteArray();
            Response.BinaryWrite(File);
            Response.End();
        }
        public IList<StaffIssueManagementReport_vw> StaffIssueManagementReportWithCriteria(string Campus, string IssueGroup, string IssueType, string DueDateType, string DateType, string FromDate, string ToDate, string CountName, string Result, string Performer, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                {
                    List<StaffIssueManagementReport_vw> StaffIssueManagementReportList = new List<StaffIssueManagementReport_vw>();
                    StaffIssuesService sis = new StaffIssuesService();
                    Dictionary<string, object> Criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(Campus))
                    { Criteria.Add("BranchCode", Campus); }
                    if (!string.IsNullOrEmpty(IssueGroup))
                    { Criteria.Add("IssueGroup", IssueGroup); }
                    if (!string.IsNullOrEmpty(IssueType))
                    { Criteria.Add("IssueType", IssueType); }
                    if (!string.IsNullOrEmpty(Performer))
                    { Criteria.Add("Performer", Performer); }
                    //if (!string.IsNullOrEmpty(CountName) && !string.IsNullOrEmpty(Result))
                    //{ Criteria.Add(CountName, Convert.ToInt64(Result)); }
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    if (!string.IsNullOrEmpty(FromDate))
                    {

                        if (!string.IsNullOrEmpty(ToDate))
                        {
                            DateTime[] FromToDate = new DateTime[2];
                            FromToDate[0] = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            FromToDate[1] = DateTime.Parse(ToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            string ToDate1 = string.Format("{0:dd/MM/yyyy}", FromToDate[1]);
                            ToDate1 = ToDate1 + " 23:59:59";
                            FromToDate[1] = DateTime.Parse(ToDate1, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);

                            Criteria.Add(DateType, FromToDate);
                        }
                        else
                        {
                            DateTime[] fromto = new DateTime[2];
                            fromto[0] = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            fromto[1] = DateTime.Now;
                            Criteria.Add(DateType, fromto);
                        }
                    }
                    Dictionary<long, IList<StaffIssueManagementReport_vw>> GetStaffIssueManagementReportList = sis.GetStaffIssueManagementReport_vwListWithPaging(page - 1, 9999999, sidx, sord, Criteria);
                    if (GetStaffIssueManagementReportList != null && GetStaffIssueManagementReportList.FirstOrDefault().Key > 0)
                    {
                        var GetStaffIssueMangementCountReportList1 = new List<StaffIssueManagementReport_vw>();
                        if (CountName == "Assigned")
                        {
                            if (!string.IsNullOrEmpty(Performer))
                            {
                                GetStaffIssueMangementCountReportList1 = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Assigned > 0 select u).ToList();
                            }
                            else
                            {
                                GetStaffIssueMangementCountReportList1 = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Assigned > 0 && Performer != "" && Performer != null select u).ToList();
                            }
                        }
                        if (CountName == "Resolved")
                        {
                            if (!string.IsNullOrEmpty(Performer))
                            {
                                GetStaffIssueMangementCountReportList1 = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Resolved > 0 select u).ToList();
                            }
                            else
                            {
                                GetStaffIssueMangementCountReportList1 = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Resolved > 0 && Performer != "" && Performer != null select u).ToList();
                            }

                        }
                        if (CountName == "TotalCount")
                        {
                            if (!string.IsNullOrEmpty(Performer))
                            {
                                var AssignedList = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Assigned > 0 select u).ToList();
                                var ResolvedList = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Resolved > 0 && u.Performer != null && u.Performer != "" select u).ToList();
                                GetStaffIssueMangementCountReportList1 = AssignedList.Union(ResolvedList).ToList();
                            }
                            else
                            {
                                var AssignedList = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Assigned > 0 && u.Performer != null && u.Performer != "" select u).ToList();
                                var ResolvedList = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.Resolved > 0 && u.Performer != null && u.Performer != "" select u).ToList();
                                GetStaffIssueMangementCountReportList1 = AssignedList.Union(ResolvedList).ToList();
                            }
                        }
                        if (!string.IsNullOrEmpty(DueDateType))
                        {
                            int i = 1;
                            foreach (var item in GetStaffIssueMangementCountReportList1)
                            {
                                //if (item.ModifiedDate != null && item.DueDate != null)
                                //{
                                //    item.DifferenceInHours = item.ModifiedDate - item.DueDate;
                                //}
                                if (item.DueDate != null)
                                {
                                    item.DifferenceInHours = DateTime.Now - item.DueDate;
                                    if (item.ModifiedDate != null)
                                    {
                                        item.DifferenceInHours1 = item.ModifiedDate - item.DueDate;
                                        if (item.AssignedDate != null)
                                        {
                                            item.DifferenceInHours2 = item.ModifiedDate - item.AssignedDate;
                                        }
                                        else
                                        {
                                            item.DifferenceInHours2 = TimeSpan.Zero;
                                        }
                                    }
                                    else
                                    {
                                        if (item.AssignedDate != null)
                                        {
                                            item.DifferenceInHours2 = DateTime.Now - item.AssignedDate;
                                        }
                                        else
                                        {
                                            item.DifferenceInHours2 = TimeSpan.Zero;
                                        }
                                    }
                                }
                                else
                                {
                                    item.DifferenceInHours = TimeSpan.Zero;
                                    item.DifferenceInHours2 = TimeSpan.Zero;
                                }
                                item.Id = i;
                                i++;
                            }
                            var DueDateList = new List<StaffIssueManagementReport_vw>();
                            if (DueDateType == "1")
                            {
                                DueDateList = (from u in GetStaffIssueMangementCountReportList1 where u.DueDate != null && u.ModifiedDate != null && u.DifferenceInHours1.Value.TotalHours <= 0 select u).ToList();
                            }
                            if (DueDateType == "2")
                            {
                                DueDateList = (from u in GetStaffIssueMangementCountReportList1 where u.DueDate != null && u.ModifiedDate != null && u.DifferenceInHours1.Value.TotalHours > 0 && u.DifferenceInHours1.Value.TotalHours <= 12 select u).ToList();
                            }
                            if (DueDateType == "3")
                            {
                                DueDateList = (from u in GetStaffIssueMangementCountReportList1 where u.DueDate != null && u.ModifiedDate != null && u.DifferenceInHours1.Value.TotalHours > 0 && u.DifferenceInHours1.Value.TotalHours > 12 select u).ToList();
                            }

                            return StaffIssueManagementReportList = DueDateList;
                        }
                        else
                        {
                            int i = 1;
                            foreach (var item in GetStaffIssueMangementCountReportList1)
                            {
                                //if (item.ModifiedDate != null && item.DueDate != null)
                                //{
                                //    item.DifferenceInHours = item.ModifiedDate - item.DueDate;
                                //}
                                if (item.DueDate != null)
                                {
                                    item.DifferenceInHours = DateTime.Now - item.DueDate;
                                    if (item.ModifiedDate != null)
                                    {
                                        item.DifferenceInHours1 = item.ModifiedDate - item.DueDate;
                                        if (item.AssignedDate != null)
                                        {
                                            item.DifferenceInHours2 = item.ModifiedDate - item.AssignedDate;
                                        }
                                        else
                                        {
                                            item.DifferenceInHours2 = TimeSpan.Zero;
                                        }
                                    }
                                    if (item.AssignedDate != null)
                                    {
                                        item.DifferenceInHours2 = DateTime.Now - item.AssignedDate;
                                    }
                                    else
                                    {
                                        item.DifferenceInHours2 = TimeSpan.Zero;
                                    }
                                }
                                else
                                {
                                    item.DifferenceInHours = TimeSpan.Zero;
                                    item.DifferenceInHours2 = TimeSpan.Zero;
                                }
                                item.Id = i;
                                i++;
                            }
                            return StaffIssueManagementReportList = GetStaffIssueMangementCountReportList1;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult StaffIssueManagementReportPDF(string Campus, string IssueGroup, string IssueType, string DueDateType, string DateType, string FromDate, string ToDate, string CountName, string Result, string Performer, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                IList<StaffIssueManagementReport_vw> StaffIssueManagementReport = new List<StaffIssueManagementReport_vw>();
                StaffIssueManagementReport = StaffIssueManagementReportWithCriteria(Campus, IssueGroup, IssueType, DueDateType, DateType, FromDate, ToDate, CountName, Result, Performer, rows, sidx, sord, page);
                if (StaffIssueManagementReport.Count > 0)
                {
                    StaffIssueManagementReportPDF staffissuemanagementreportpdf = new StaffIssueManagementReportPDF();
                    staffissuemanagementreportpdf.StaffIssueManagementReportList = StaffIssueManagementReport;
                    ViewBag.Campus = Campus;
                    ViewBag.IssueGroup = IssueGroup;
                    ViewBag.IssueType = IssueType;
                    if (DueDateType == "1")
                    {
                        ViewBag.DueDateType = "Before Due Date";
                    }
                    if (DueDateType == "2")
                    {
                        ViewBag.DueDateType = "After Due Date Below 24 hours";
                    }
                    if (DueDateType == "3")
                    {
                        ViewBag.DueDateType = "After Due Date Above 24 hours";
                    }
                    ViewBag.DateType = DateType;
                    ViewBag.FromDate = FromDate;
                    ViewBag.ToDate = ToDate;
                    if (!string.IsNullOrEmpty(Performer))
                    {
                        ViewBag.Performer = us.GetUserNameByUserId(Performer);
                    }
                    return new Rotativa.ViewAsPdf("StaffIssueManagementReportPDF", staffissuemanagementreportpdf)
                    {
                        FileName = "Staff Issue Management Report On-" + DateTime.Today.ToString("dd/MM/yyyy") + ".pdf",
                        PageOrientation = Rotativa.Options.Orientation.Landscape,
                        PageMargins = new Rotativa.Options.Margins(0, 0, 0, 0)
                    };
                }
                else
                {
                    StaffIssueManagementReportPDF staffissuemanagementreportpdf = new StaffIssueManagementReportPDF();
                    return new Rotativa.ViewAsPdf("StaffIssueManagementReportPDF", staffissuemanagementreportpdf)
                    {
                        FileName = "Staff Issue Management Report On-" + DateTime.Today.ToString("dd/MM/yyyy") + ".pdf",
                        PageOrientation = Rotativa.Options.Orientation.Landscape,
                        PageMargins = new Rotativa.Options.Margins(0, 0, 0, 0)
                    };
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult StaffIssueManagementCountReportPDF(string Campus, string IssueGroup, string IssueType, string DueDateType, string DateType, string FromDate, string ToDate, string CountName, string Result, string Performer, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                IList<StaffIssueManagementReport_vw> StaffIssueManagementReport = new List<StaffIssueManagementReport_vw>();
                //StaffIssueManagementReport = StaffIssueManagementReportWithCriteria(Campus, IssueGroup, IssueType, DueDateType, DateType, FromDate, ToDate, CountName, Result, Performer, rows, sidx, sord, page);                
                StaffIssuesService sis = new StaffIssuesService();
                Dictionary<string, object> Criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(Campus))
                { Criteria.Add("BranchCode", Campus); }
                if (!string.IsNullOrEmpty(IssueGroup))
                { Criteria.Add("IssueGroup", IssueGroup); }
                if (!string.IsNullOrEmpty(IssueType))
                { Criteria.Add("IssueType", IssueType); }
                if (!string.IsNullOrEmpty(Performer))
                { Criteria.Add("Performer", Performer); }
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                if (!string.IsNullOrEmpty(FromDate))
                {

                    if (!string.IsNullOrEmpty(ToDate))
                    {
                        DateTime[] FromToDate = new DateTime[2];
                        FromToDate[0] = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        FromToDate[1] = DateTime.Parse(ToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        string ToDate1 = string.Format("{0:dd/MM/yyyy}", FromToDate[1]);
                        ToDate1 = ToDate1 + " 23:59:59";
                        FromToDate[1] = DateTime.Parse(ToDate1, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);

                        Criteria.Add(DateType, FromToDate);
                    }
                    else
                    {
                        DateTime[] fromto = new DateTime[2];
                        fromto[0] = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        fromto[1] = DateTime.Now;
                        Criteria.Add(DateType, fromto);
                    }
                }
                Dictionary<long, IList<StaffIssueManagementReport_vw>> GetStaffIssueManagementReportList = sis.GetStaffIssueManagementReport_vwListWithPaging(page - 1, 9999999, sidx, sord, Criteria);
                if (GetStaffIssueManagementReportList != null && GetStaffIssueManagementReportList.FirstOrDefault().Key > 0)
                {

                    if (!string.IsNullOrEmpty(DueDateType))
                    {
                        foreach (var item in GetStaffIssueManagementReportList.FirstOrDefault().Value)
                        {
                            if (item.ModifiedDate != null && item.DueDate != null)
                            {
                                item.DifferenceInHours = item.ModifiedDate - item.DueDate;
                            }
                        }
                        var DueDateList = new List<StaffIssueManagementReport_vw>();
                        if (DueDateType == "1")
                        {
                            DueDateList = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.DueDate != null && u.ModifiedDate != null && u.DifferenceInHours.Value.TotalHours <= 0 select u).ToList();
                        }
                        if (DueDateType == "2")
                        {
                            DueDateList = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.DueDate != null && u.ModifiedDate != null && u.DifferenceInHours.Value.TotalHours > 0 && u.DifferenceInHours.Value.TotalHours <= 12 select u).ToList();
                        }
                        if (DueDateType == "3")
                        {
                            DueDateList = (from u in GetStaffIssueManagementReportList.FirstOrDefault().Value where u.DueDate != null && u.ModifiedDate != null && u.DifferenceInHours.Value.TotalHours > 0 && u.DifferenceInHours.Value.TotalHours > 12 select u).ToList();
                        }
                        var DueDateWiseList = (from fpl in DueDateList
                                               group fpl by fpl.Performer into fplt
                                               //where fplt.FirstOrDefault().IssueNumber == ""
                                               select new
                                               {
                                                   cell = new string[]
                                                       {                                                                                                                        
                                                            fplt.FirstOrDefault().Id.ToString(),
                                                            fplt.FirstOrDefault().InstanceId.ToString(),  
                                                            fplt.FirstOrDefault().ProcessRefId.ToString(),  
                                                            fplt.FirstOrDefault().Performer!=null?fplt.FirstOrDefault().Performer.ToString():"UnAssigned",  
                                                            fplt.Sum(x => x.Available).ToString(),                                                                                                                    
                                                            fplt.Sum(x => x.Assigned).ToString(),                                                                                                                         
                                                            fplt.Sum(x => x.Resolved).ToString(),
                                                            fplt.Sum(x => x.TotalCount).ToString()
                                                               
                                                       }
                                               }).ToList();

                        var List = DueDateWiseList;
                        int i = 1;
                        foreach (var item1 in DueDateWiseList)
                        {
                            StaffIssueManagementReport_vw Obj = new StaffIssueManagementReport_vw();
                            item1.cell[0] = i.ToString();
                            item1.cell[7] = (Convert.ToInt64(item1.cell[7]) + Convert.ToInt64(item1.cell[6])).ToString();
                            Obj.Id = Convert.ToInt64(item1.cell[0]);
                            Obj.Performer = item1.cell[3] != "" ? us.GetUserNameByUserId(item1.cell[3]) : "UnAssigned";
                            Obj.Available = Convert.ToInt64(item1.cell[4]);
                            Obj.Resolved = Convert.ToInt64(item1.cell[5]);
                            Obj.Assigned = Convert.ToInt64(item1.cell[6]);
                            Obj.TotalCount = Convert.ToInt64(item1.cell[7]);
                            StaffIssueManagementReport.Add(Obj);
                            i++;
                        }
                        if (DueDateWiseList.Count > 0)
                        {
                            StaffIssueManagementReportPDF staffissuemanagementreportpdf = new StaffIssueManagementReportPDF();
                            staffissuemanagementreportpdf.StaffIssueManagementReportList = StaffIssueManagementReport;
                            ViewBag.Campus = Campus;
                            ViewBag.IssueGroup = IssueGroup;
                            ViewBag.IssueType = IssueType;
                            if (DueDateType == "1")
                            {
                                ViewBag.DueDateType = "Before Due Date";
                            }
                            if (DueDateType == "2")
                            {
                                ViewBag.DueDateType = "After Due Date Below 24 hours";
                            }
                            if (DueDateType == "3")
                            {
                                ViewBag.DueDateType = "After Due Date Above 24 hours";
                            }
                            ViewBag.DateType = DateType;
                            ViewBag.FromDate = FromDate;
                            ViewBag.ToDate = ToDate;
                            if (!string.IsNullOrEmpty(Performer))
                            {
                                ViewBag.Performer = us.GetUserNameByUserId(Performer);
                            }
                            return new Rotativa.ViewAsPdf("StaffIssueManagementCountReportPDF", staffissuemanagementreportpdf)
                            {
                                FileName = "Staff Issue Management Count Report On-" + DateTime.Today.ToString("dd/MM/yyyy") + ".pdf",
                                PageOrientation = Rotativa.Options.Orientation.Landscape,
                                PageMargins = new Rotativa.Options.Margins(0, 0, 0, 0)
                            };
                        }
                    }
                    else
                    {
                        var List1 = (from fpl in GetStaffIssueManagementReportList.FirstOrDefault().Value
                                     group fpl by fpl.Performer into fplt
                                     //where fplt.FirstOrDefault().IssueNumber == ""
                                     select new
                                     {
                                         cell = new string[]
                                                       {                                                                                                                        
                                                            fplt.FirstOrDefault().Id.ToString(),
                                                            fplt.FirstOrDefault().InstanceId.ToString(),  
                                                            fplt.FirstOrDefault().ProcessRefId.ToString(),  
                                                            fplt.FirstOrDefault().Performer!=null?fplt.FirstOrDefault().Performer.ToString():"",  
                                                            fplt.Sum(x => x.Available).ToString(),                                                                                                                    
                                                            fplt.Sum(x => x.Assigned).ToString(),                                                                                                                         
                                                            fplt.Sum(x => x.Resolved).ToString(),
                                                            fplt.Sum(x => x.TotalCount).ToString()
                                                               
                                                       }

                                     }).ToList();

                        var List = List1;
                        int i = 1;
                        if (List1.Count > 0)
                        {
                            StaffIssueManagementReportPDF staffissuemanagementreportpdf = new StaffIssueManagementReportPDF();
                            foreach (var item1 in List1)
                            {
                                StaffIssueManagementReport_vw Obj = new StaffIssueManagementReport_vw();
                                item1.cell[0] = i.ToString();
                                item1.cell[7] = (Convert.ToInt64(item1.cell[7]) + Convert.ToInt64(item1.cell[6])).ToString();
                                Obj.Id = Convert.ToInt64(item1.cell[0]);
                                Obj.Performer = item1.cell[3] != "" ? us.GetUserNameByUserId(item1.cell[3]) : "UnAssigned";
                                Obj.Available = Convert.ToInt64(item1.cell[4]);
                                Obj.Resolved = Convert.ToInt64(item1.cell[5]);
                                Obj.Assigned = Convert.ToInt64(item1.cell[6]);
                                Obj.TotalCount = Convert.ToInt64(item1.cell[7]);
                                StaffIssueManagementReport.Add(Obj);
                                i++;
                            }
                            staffissuemanagementreportpdf.StaffIssueManagementReportList = StaffIssueManagementReport;
                            ViewBag.Campus = Campus;
                            ViewBag.IssueGroup = IssueGroup;
                            ViewBag.IssueType = IssueType;
                            if (DueDateType == "1")
                            {
                                ViewBag.DueDateType = "Before Due Date";
                            }
                            if (DueDateType == "2")
                            {
                                ViewBag.DueDateType = "After Due Date Below 24 hours";
                            }
                            if (DueDateType == "3")
                            {
                                ViewBag.DueDateType = "After Due Date Above 24 hours";
                            }
                            ViewBag.DateType = DateType;
                            ViewBag.FromDate = FromDate;
                            ViewBag.ToDate = ToDate;
                            if (!string.IsNullOrEmpty(Performer))
                            {
                                ViewBag.Performer = us.GetUserNameByUserId(Performer);
                            }
                            return new Rotativa.ViewAsPdf("StaffIssueManagementCountReportPDF", staffissuemanagementreportpdf)
                            {
                                FileName = "Staff Issue Management Count Report On-" + DateTime.Today.ToString("dd/MM/yyyy") + ".pdf",
                                PageOrientation = Rotativa.Options.Orientation.Landscape,
                                PageMargins = new Rotativa.Options.Margins(0, 0, 0, 0)
                            };
                        }
                    }
                }
                StaffIssueManagementReportPDF staffissuemanagementcountreportpdf = new StaffIssueManagementReportPDF();
                return new Rotativa.ViewAsPdf("StaffIssueManagementCountReportPDF", staffissuemanagementcountreportpdf)
                {
                    FileName = "Staff Issue Management Report Count On-" + DateTime.Today.ToString("dd/MM/yyyy") + ".pdf",
                    PageOrientation = Rotativa.Options.Orientation.Landscape,
                    PageMargins = new Rotativa.Options.Margins(0, 0, 0, 0)
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult StaffIssueManagementDateWiseCountReportPDF(string Campus, string IssueGroup, string IssueType, string FromDate, string ToDate, string CountName, string Result, string Performer, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                IList<StaffIssueManagementReport_vw> StaffIssueManagementReport = new List<StaffIssueManagementReport_vw>();
                StaffIssuesService sis = new StaffIssuesService();
                Dictionary<string, object> Criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(Campus))
                { Criteria.Add("BranchCode", Campus); }
                if (!string.IsNullOrEmpty(IssueGroup))
                { Criteria.Add("IssueGroup", IssueGroup); }
                if (!string.IsNullOrEmpty(IssueType))
                { Criteria.Add("IssueType", IssueType); }
                if (!string.IsNullOrEmpty(Performer))
                { Criteria.Add("Performer", Performer); }
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                DateTime[] FromToDateTime = new DateTime[2];
                if (!string.IsNullOrEmpty(FromDate))
                {
                    if (!string.IsNullOrEmpty(ToDate))
                    {
                        FromToDateTime[0] = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        FromToDateTime[1] = DateTime.Parse(ToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        string ToDate1 = string.Format("{0:dd/MM/yyyy}", FromToDateTime[1]);
                        ToDate1 = ToDate1 + " 23:59:59";
                        FromToDateTime[1] = DateTime.Parse(ToDate1, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    }
                    else
                    {
                        FromToDateTime[0] = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        FromToDateTime[1] = DateTime.Now;
                        //string To = string.Format("{0:dd/MM/yyyy}", fromto[0]);
                        //fromto[1] = Convert.ToDateTime(To + " " + "23:59:59");                        
                    }
                }
                if (!string.IsNullOrEmpty(FromDate))
                {
                    Dictionary<long, IList<StaffIssueManagementReport_vw>> GetStaffIssueManagementReportList = sis.GetStaffIssueManagementReport_vwListWithPaging(page - 1, 9999999, sidx, sord, Criteria);
                    if (GetStaffIssueManagementReportList != null && GetStaffIssueManagementReportList.FirstOrDefault().Key > 0)
                    {

                        var List1 = (from fpl in GetStaffIssueManagementReportList.FirstOrDefault().Value
                                     group fpl by fpl.Performer into fplt
                                     //where fplt.FirstOrDefault().Performer != null && fplt.FirstOrDefault().Performer != ""
                                     select new
                                     {
                                         cell = new string[] {
                                                 fplt.FirstOrDefault().Id.ToString(),
                                                 fplt.FirstOrDefault().InstanceId.ToString(),  
                                                 fplt.FirstOrDefault().ProcessRefId.ToString(),
                                                 fplt.FirstOrDefault().Performer!=null?fplt.FirstOrDefault().Performer.ToString():"",  
                                                 fplt.Sum(x => x.Available).ToString(),                                                                                                                    
                                                 fplt.Sum(x => x.Assigned).ToString(),                                                                                                                         
                                                 fplt.Sum(x => x.Resolved).ToString(),
                                                 fplt.Sum(x => x.TotalCount).ToString()
                                             }
                                     }).ToList();
                        var DateWiseList = (from items in GetStaffIssueManagementReportList.FirstOrDefault().Value
                                            where items.ModifiedDate >= FromToDateTime[0] && items.ModifiedDate <= FromToDateTime[1] && items.Performer != null && items.Performer != ""
                                            select items).ToList();
                        var ResolvedCount = (from fpl in DateWiseList
                                             group fpl by fpl.Performer into fplt
                                             //where fplt.FirstOrDefault().Performer != "" && fplt.FirstOrDefault().Performer!=null
                                             select new
                                             {
                                                 cell = new string[]
                                                       {                                                                                                                        
                                                            fplt.FirstOrDefault().Id.ToString(),
                                                            fplt.FirstOrDefault().InstanceId.ToString(),  
                                                            fplt.FirstOrDefault().ProcessRefId.ToString(),  
                                                            fplt.FirstOrDefault().Performer!=null?fplt.FirstOrDefault().Performer.ToString():"",  
                                                            fplt.Sum(x => x.Available).ToString(),                                                                                                                    
                                                            fplt.Sum(x => x.Assigned).ToString(),                                                                                                                         
                                                            fplt.Sum(x => x.Resolved).ToString(),
                                                            fplt.Sum(x => x.TotalCount).ToString()
                                                               
                                                       }

                                             }).ToList();
                        foreach (var items in List1)
                        {
                            long Count = 0;
                            items.cell[6] = Convert.ToInt64(Count).ToString();
                            foreach (var item1 in ResolvedCount)
                            {
                                if (items.cell[3] == item1.cell[3])
                                {
                                    items.cell[6] = item1.cell[6];
                                    items.cell[7] = (Convert.ToInt64(items.cell[7]) + Convert.ToInt64(item1.cell[6])).ToString();
                                    break;
                                }
                            }
                        }
                        int i = 1;
                        foreach (var item1 in List1)
                        {
                            StaffIssueManagementReport_vw Obj = new StaffIssueManagementReport_vw();
                            item1.cell[0] = i.ToString();
                            item1.cell[7] = (Convert.ToInt64(item1.cell[7]) + Convert.ToInt64(item1.cell[6])).ToString();
                            Obj.Id = Convert.ToInt64(item1.cell[0]);
                            Obj.Performer = item1.cell[3] != "" ? us.GetUserNameByUserId(item1.cell[3]) : "UnAssigned";
                            Obj.Available = Convert.ToInt64(item1.cell[4]);
                            Obj.Resolved = Convert.ToInt64(item1.cell[5]);
                            Obj.Assigned = Convert.ToInt64(item1.cell[6]);
                            Obj.TotalCount = Convert.ToInt64(item1.cell[7]);
                            StaffIssueManagementReport.Add(Obj);
                            i++;
                        }
                        if (List1.Count > 0)
                        {
                            StaffIssueManagementReportPDF staffissuemanagementreportpdf = new StaffIssueManagementReportPDF();
                            staffissuemanagementreportpdf.StaffIssueManagementReportList = StaffIssueManagementReport;
                            ViewBag.Campus = Campus;
                            ViewBag.IssueGroup = IssueGroup;
                            ViewBag.IssueType = IssueType;
                            ViewBag.FromDate = FromDate;
                            ViewBag.ToDate = ToDate;
                            if (!string.IsNullOrEmpty(Performer))
                            {
                                ViewBag.Performer = us.GetUserNameByUserId(Performer);
                            }
                            return new Rotativa.ViewAsPdf("StaffIssueManagementDateWiseCountReportPDF", staffissuemanagementreportpdf)
                            {
                                FileName = "Staff Issue Management DateWise Count Report On-" + DateTime.Today.ToString("dd/MM/yyyy") + ".pdf",
                                PageOrientation = Rotativa.Options.Orientation.Landscape,
                                PageMargins = new Rotativa.Options.Margins(0, 0, 0, 0)
                            };
                        }
                    }
                }
                StaffIssueManagementReportPDF staffissuemanagementcountreportpdf = new StaffIssueManagementReportPDF();
                return new Rotativa.ViewAsPdf("StaffIssueManagementDateWiseCountReportPDF", staffissuemanagementcountreportpdf)
                {
                    FileName = "Staff Issue Management DateWise Count Report On-" + DateTime.Today.ToString("dd/MM/yyyy") + ".pdf",
                    PageOrientation = Rotativa.Options.Orientation.Landscape,
                    PageMargins = new Rotativa.Options.Margins(0, 0, 0, 0)
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
        //public void SendEmailForResolver(StaffIssues si, string UserId,HttpContext httpcontext)
        //{
        //    try
        //    {
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        criteria.Add("AppCode", "SIM");
        //        criteria.Add("UserId", UserId);
        //        Dictionary<long, IList<UserAppRole_Vw>> UserAppRoleList = us.GetAppRoleOnlyActiveUsersPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
        //        string[] ResolverMail = (from u in UserAppRoleList.FirstOrDefault().Value where u.Email != null select u.Email).Distinct().ToArray();
        //        ActorsEmail(si, "", ResolverMail, si.IssueNumber, "", "", si.IssueGroup, "", "", si.Status, "", "", "");
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}        
        #endregion
        //Added By Prabakaran for Assign a Call to Resolver
        public ActionResult AssignActivityjson(long ActivityId, string UserId, string IsType)
        {
            ProcessFlowServices pfs = new ProcessFlowServices();
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    bool isAssigned = false;
                    if (ActivityId > 0 && !string.IsNullOrWhiteSpace(UserId))
                    {
                        if (IsType == "Staff")
                        {
                            isAssigned = pfs.AssignStaffActivity(ActivityId, UserId);
                            StaffActivities staffactivities = pfs.GetStaffActivitiesByActivityId(ActivityId);
                            if (staffactivities != null)
                            {
                                StaffIssues staffissues = pfs.GetStaffIssuesById(staffactivities.ProcessRefId);

                                if (staffissues != null)
                                {
                                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                                    criteria.Add("AppCode", "SIM");
                                    criteria.Add("UserId", UserId);
                                    Dictionary<long, IList<UserAppRole_Vw>> UserAppRoleList = us.GetAppRoleOnlyActiveUsersPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                                    string[] ResolverMail = (from u in UserAppRoleList.FirstOrDefault().Value where u.Email != null select u.Email).Distinct().ToArray();
                                    StaffIssuesService sis = new StaffIssuesService();
                                    staffissues.AssignedDate = DateTime.Now;
                                    sis.CreateOrUpdateStaffIssues(staffissues);
                                    ActorsEmail(staffissues, "", ResolverMail, staffissues.IssueNumber, "", "", staffissues.IssueGroup, "", "", staffissues.Status, "", "", "");
                                }
                            }
                        }
                        else
                            isAssigned = pfs.AssignActivity(ActivityId, UserId);
                    }
                    return Json(isAssigned, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }
        public JsonResult FillStaffIssueGroupByDeptCode(string DeptCode)
        {
            try
            {
                MasterDataService mds = new MasterDataService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(DeptCode))
                {
                    criteria.Add("IssueGroup", DeptCode);
                }
                Dictionary<long, IList<StaffIssueGroupMaster>> IssueGroupList = mds.GetStaffIssueGroupListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                if (IssueGroupList.FirstOrDefault().Key > 0)
                {
                    var IssueGroup1 = (
                             from items in IssueGroupList.First().Value
                             select new
                             {
                                 Text = items.IssueGroup,
                                 Value = items.IssueGroup
                             }).ToList();
                    return Json(IssueGroup1, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }
    }
}
