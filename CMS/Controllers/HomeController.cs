using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TIPS.Entities;
using TIPS.Entities.AdmissionEntities;
using TIPS.Entities.Attendance;
using TIPS.Service;
using TIPS.ServiceContract;
using System.IO;
using TIPS.Component;
using TIPS.Entities.InboxEntities;
using OfficeOpenXml;
using System.Drawing;
using ThoughtWorks.QRCode.Codec.Util;
using OfficeOpenXml.Style;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using TIPS.Entities.AdminTemplateEntities;
using TIPS.Entities.TransportEntities;
using TIPS.Entities.StaffManagementEntities;
using CMS.Helpers;
using TIPS.Entities.StaffEntities;

namespace CMS.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Home()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    //these session variables are created and assigned empty to initialize session object
                    //but need to see how to avoid and change it accordingly "REVIST" comments by JP and Anbu
                    Session["emailsent"] = "";
                    // Session["campusname"] = "";
                    Session["cmpnm"] = "";
                    Session["grd"] = "";
                    Session["sect"] = "";
                    Session["acdyr"] = "";
                    Session["apnam"] = "";
                    Session["stats"] = "";
                    Session["appno"] = "";
                    Session["regno"] = "";
                    Session["feestructyr"] = "";
                    Session["ishosteller"] = "";
                    Session["hostlr"] = "";
                    Session["Promotioncamp"] = "";
                    Session["idnum"] = "";
                    Session["transfered"] = "";
                    Session["transferedId"] = "";
                    Session["transferedName"] = "";
                    Session["pagename"] = "";
                    Session["transferpdf"] = "";
                    Session["discontinue"] = "";
                    Session["discontinueName"] = "";
                    Session["bonafidepdf"] = "";
                    Session["promotion"] = "";
                    Session["promotionId"] = "";
                    Session["notpromotedpreregno"] = "";
                    Session["ptcam"] = "";
                    Session["ptgrd"] = "";
                    Session["ptsect"] = "";
                    Session["attachment"] = "";
                    Session["attachmentnames"] = "";
                    if (Session["UserId"].ToString() == "CHE-GRL")
                    {
                        Session["Promotioncamp"] = "CHENNAI";
                    }
                    if (Session["UserId"].ToString() == "TIR-GRL")
                    {
                        Session["Promotioncamp"] = "TIRUPUR";
                    }
                    if (Session["UserId"].ToString() == "ERN-GRL")
                    {
                        Session["Promotioncamp"] = "ERNAKULAM";
                    }
                    if (Session["UserId"].ToString() == "KAR-GRL")
                    {
                        Session["Promotioncamp"] = "KARUR";
                    }
                    if (Session["UserId"].ToString() == "GRL-ADMS")
                    {
                        Session["Promotioncamp"] = "IB MAIN";
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }

        }

        #region New Home Page Design By Gobi
        public ActionResult HomePage()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    CallManagementService cms = new CallManagementService();
                    Dictionary<string, object> Criteria = new Dictionary<string, object>();
                    Criteria.Add("Performer", userId);
                    Dictionary<long, IList<CallManagementChart>> GetIssueCountByStatus = cms.GetCallManagementListWithPagingChart(null, 9999, string.Empty, string.Empty, Criteria);
                    var IssueCount = (from u in GetIssueCountByStatus.First().Value
                                      where u.IsInformation == false
                                      select u).ToList();
                    int LogIssCnt = 0;
                    int ResRejIssCnt = 0;
                    int log_ResRejIssCnt = 0;
                    int ResIssCnt = 0;
                    int AppRejCnt = 0;
                    int res_apprejcnt = 0;
                    //int AppRejIssCnt = 0;
                    int AppIssCnt = 0;
                    int AppIssTotCnt = 0;
                    int TobeCmpltdcnt = 0;
                    int CmpltdCnt = 0;
                    int TotalIssueCnt = 0;

                    foreach (var item in IssueCount)
                    {
                        if (item.Status == "LogIssue")
                        {
                            LogIssCnt++;
                        }
                        if (item.Status == "ResolveIssueRejection")
                        {
                            ResRejIssCnt++;
                        }
                        if (item.Status == "ResolveIssue")
                        {
                            ResIssCnt++;
                        }
                        if (item.Status == "ApproveIssueRejection")
                        {
                            AppRejCnt++;
                        }
                        if (item.Status == "ApproveIssue")
                        {
                            AppIssCnt++;
                        }
                        if (item.Status == "Complete")
                        {
                            TobeCmpltdcnt++;
                        }
                        if (item.Status == "Completed")
                        {
                            CmpltdCnt++;
                        }
                    }
                    TotalIssueCnt = LogIssCnt + ResRejIssCnt + ResIssCnt + AppRejCnt + AppIssCnt + TobeCmpltdcnt + CmpltdCnt;
                    log_ResRejIssCnt = LogIssCnt + ResRejIssCnt;
                    res_apprejcnt = ResIssCnt + AppRejCnt;
                    AppIssTotCnt = AppIssCnt + TobeCmpltdcnt;

                    ViewBag.LogIssCnt = LogIssCnt;
                    ViewBag.ResRejIssCnt = ResRejIssCnt;
                    ViewBag.ResIssCnt = ResIssCnt;
                    ViewBag.AppRejCnt = AppRejCnt;
                    ViewBag.AppIssCnt = AppIssCnt;
                    ViewBag.TobeCmpltdcnt = TobeCmpltdcnt;
                    ViewBag.CmpltdCnt = CmpltdCnt;

                    ViewBag.TotalIssueCnt = TotalIssueCnt;
                    ViewBag.log_ResRejIssCnt = log_ResRejIssCnt;
                    ViewBag.res_apprejcnt = res_apprejcnt;
                    ViewBag.AppIssTotCnt = AppIssTotCnt;


                    //                    Criteria.Add("Performer", userId);
                    Criteria.Clear();
                    string AcademicYear = "";
                    DateTime TodayDate = DateTime.Now;

                    int Curr_Month = TodayDate.Month;
                    int Curr_Year = TodayDate.Year;
                    if (Curr_Month >= 05)
                    {
                        AcademicYear = Curr_Year + "-" + (Curr_Year + 1);
                    }
                    if (Curr_Month <= 04)
                    {
                        AcademicYear = (Curr_Year - 1) + "-" + Curr_Year;
                    }
                    Criteria.Add("AcademicYear", AcademicYear);
                    AdmissionManagementService AMS = new AdmissionManagementService();
                    Dictionary<long, IList<StudentTemplate>> StudentDetails = AMS.GetStudentDetailsListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, Criteria);
                    //Dictionary<long, IList<StudentTemplate>> GetStudentTemplate = cms.GetCallManagementListWithPagingChart(null, 9999, string.Empty, string.Empty, Criteria);
                    var Stud = (from u in StudentDetails.First().Value
                                select u).ToList();
                    int NewEnquiry = 0;
                    int Discontinued = 0;
                    int NewReg = 0;
                    int Declined = 0;
                    int SentApproval = 0;
                    int Registered = 0;

                    foreach (var item in Stud)
                    {
                        if (item.AdmissionStatus == "New Enquiry")
                        {
                            NewEnquiry++;
                        }
                        if (item.AdmissionStatus == "Discontinued")
                        {
                            Discontinued++;
                        }
                        if (item.AdmissionStatus == "New Registration")
                        {
                            NewReg++;
                        }
                        if (item.AdmissionStatus == "Declined")
                        {
                            Declined++;
                        }
                        if (item.AdmissionStatus == "Sent For Approval")
                        {
                            SentApproval++;
                        }
                        if (item.AdmissionStatus == "Registered")
                        {
                            Registered++;
                        }
                    }
                    ViewBag.NewEnquiry = NewEnquiry;
                    ViewBag.Discontinued = Discontinued;
                    ViewBag.NewReg = NewReg;
                    ViewBag.Declined = Declined;
                    ViewBag.SentApproval = SentApproval;
                    ViewBag.Registered = Registered;
                    return View();
                    //return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public ActionResult HomePages()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    CallManagementService cms = new CallManagementService();
                    Dictionary<string, object> Criteria = new Dictionary<string, object>();
                    Criteria.Add("Performer", userId);
                    Dictionary<long, IList<CallManagementChart>> GetIssueCountByStatus = cms.GetCallManagementListWithPagingChart(null, 9999, string.Empty, string.Empty, Criteria);
                    var IssueCount = (from u in GetIssueCountByStatus.First().Value
                                      where u.IsInformation == false
                                      select u).ToList();
                    int LogIssCnt = 0;
                    int ResRejIssCnt = 0;
                    int log_ResRejIssCnt = 0;
                    int ResIssCnt = 0;
                    int AppRejCnt = 0;
                    int res_apprejcnt = 0;
                    //int AppRejIssCnt = 0;
                    int AppIssCnt = 0;
                    int AppIssTotCnt = 0;
                    int TobeCmpltdcnt = 0;
                    int CmpltdCnt = 0;
                    int TotalIssueCnt = 0;

                    foreach (var item in IssueCount)
                    {
                        if (item.Status == "LogIssue")
                        {
                            LogIssCnt++;
                        }
                        if (item.Status == "ResolveIssueRejection")
                        {
                            ResRejIssCnt++;
                        }
                        if (item.Status == "ResolveIssue")
                        {
                            ResIssCnt++;
                        }
                        if (item.Status == "ApproveIssueRejection")
                        {
                            AppRejCnt++;
                        }
                        if (item.Status == "ApproveIssue")
                        {
                            AppIssCnt++;
                        }
                        if (item.Status == "Complete")
                        {
                            TobeCmpltdcnt++;
                        }
                        if (item.Status == "Completed")
                        {
                            CmpltdCnt++;
                        }
                    }
                    TotalIssueCnt = LogIssCnt + ResRejIssCnt + ResIssCnt + AppRejCnt + AppIssCnt + TobeCmpltdcnt + CmpltdCnt;
                    log_ResRejIssCnt = LogIssCnt + ResRejIssCnt;
                    res_apprejcnt = ResIssCnt + AppRejCnt;
                    AppIssTotCnt = AppIssCnt + TobeCmpltdcnt;

                    ViewBag.LogIssCnt = LogIssCnt;
                    ViewBag.ResRejIssCnt = ResRejIssCnt;
                    ViewBag.ResIssCnt = ResIssCnt;
                    ViewBag.AppRejCnt = AppRejCnt;
                    ViewBag.AppIssCnt = AppIssCnt;
                    ViewBag.TobeCmpltdcnt = TobeCmpltdcnt;
                    ViewBag.CmpltdCnt = CmpltdCnt;

                    ViewBag.TotalIssueCnt = TotalIssueCnt;
                    ViewBag.log_ResRejIssCnt = log_ResRejIssCnt;
                    ViewBag.res_apprejcnt = res_apprejcnt;
                    ViewBag.AppIssTotCnt = AppIssTotCnt;


                    //                    Criteria.Add("Performer", userId);
                    Criteria.Clear();
                    string AcademicYear = "";
                    DateTime TodayDate = DateTime.Now;

                    int Curr_Month = TodayDate.Month;
                    int Curr_Year = TodayDate.Year;
                    if (Curr_Month >= 05)
                    {
                        AcademicYear = Curr_Year + "-" + (Curr_Year + 1);
                    }
                    if (Curr_Month <= 04)
                    {
                        AcademicYear = (Curr_Year - 1) + "-" + Curr_Year;
                    }
                    Criteria.Add("AcademicYear", AcademicYear);
                    AdmissionManagementService AMS = new AdmissionManagementService();
                    Dictionary<long, IList<StudentTemplate>> StudentDetails = AMS.GetStudentDetailsListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, Criteria);
                    //Dictionary<long, IList<StudentTemplate>> GetStudentTemplate = cms.GetCallManagementListWithPagingChart(null, 9999, string.Empty, string.Empty, Criteria);
                    var Stud = (from u in StudentDetails.First().Value
                                select u).ToList();
                    int NewEnquiry = 0;
                    int Discontinued = 0;
                    int NewReg = 0;
                    int Declined = 0;
                    int SentApproval = 0;
                    int Registered = 0;

                    foreach (var item in Stud)
                    {
                        if (item.AdmissionStatus == "New Enquiry")
                        {
                            NewEnquiry++;
                        }
                        if (item.AdmissionStatus == "Discontinued")
                        {
                            Discontinued++;
                        }
                        if (item.AdmissionStatus == "New Registration")
                        {
                            NewReg++;
                        }
                        if (item.AdmissionStatus == "Declined")
                        {
                            Declined++;
                        }
                        if (item.AdmissionStatus == "Sent For Approval")
                        {
                            SentApproval++;
                        }
                        if (item.AdmissionStatus == "Registered")
                        {
                            Registered++;
                        }
                    }
                    ViewBag.NewEnquiry = NewEnquiry;
                    ViewBag.Discontinued = Discontinued;
                    ViewBag.NewReg = NewReg;
                    ViewBag.Declined = Declined;
                    ViewBag.SentApproval = SentApproval;
                    ViewBag.Registered = Registered;
                    return View();
                    //return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        #endregion

        //Change this method like below "REVISIT"
        //pass both CSE and GRL as string array into one criteria and get the results
        //dont use the dictionary count method instead use direct list method
        //create variable as CSERoleExists, GRLRoleExists so that it will give meaning
        public ActionResult CallManagement(string reset, int? rows, string sidx = "Id", string sord = "Desc", int? page = 1)
        {
            try
            {
                if (reset == "True")
                {
                    ViewBag.SearchKey = "";
                    ViewBag.SearchValue = "";
                    ViewBag.CMGTStatus = "";
                }
                else
                {
                    Dictionary<String, Object> dict = Session["CallMgmtPage"] as Dictionary<String, Object>;
                    if (dict != null && dict.FirstOrDefault().Key.Count() > 0)
                    {

                        ViewBag.SearchKey = dict.FirstOrDefault().Key;
                        ViewBag.SearchValue = dict.FirstOrDefault().Value;
                    }
                    ViewBag.CMGTStatus = Session["CMGTStatus"];
                    ViewBag.CMGTSearched = Session["CMGTSearched"];
                }
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    TIPS.Service.UserService us = new UserService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("UserId", userId);
                    criteria.Add("AppCode", "ISM");
                    criteria.Add("RoleCode", "CSE");
                    Dictionary<long, IList<UserAppRole>> UserAppRoleList = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 100, sidx, sord, criteria);
                    var ListCount = (from u in UserAppRoleList.First().Value
                                     where u.UserId == userId
                                     select u).ToList();
                    ViewBag.count = "0";
                    if (ListCount.Count >= 1)
                    {
                        ViewBag.count = "1";
                    }
                    Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                    criteria1.Add("UserId", userId);
                    criteria1.Add("AppCode", "ISM");
                    criteria1.Add("RoleCode", "GRL");
                    Dictionary<long, IList<UserAppRole>> UserAppRoleList1 = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 100, sidx, sord, criteria1);
                    var ListCount1 = (from u in UserAppRoleList1.First().Value
                                      where u.UserId == userId
                                      select u).ToList();
                    ViewBag.count1 = "0";
                    if (ListCount1.Count >= 1)
                    {
                        ViewBag.count1 = "1";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
            return View();
        }
        public bool StoreSessionVariable(string key, object value)
        {
            bool retValue = false;
            Dictionary<String, Object> dict = new Dictionary<String, Object>();
            dict.Add(key, value);
            Session["CallMgmtPage"] = dict;
            retValue = true;
            return retValue;
        }
        //fromIssueNum,toIssueNum removed since it is not used
        //The grid list need to be modified with tool bar search ans search panel in left side as part of new ui look with bootstrap theme

        //public ActionResult CallManagementListJqGrid(long? Id, string ddlSearchBy, string txtSearch, string fromDate, string status, string Grades,
        //    string IssueNumber, string BranchCode, string StudentName, string Grade, string InformationFor, string LeaveType, string IssueType, string IssueGroup, string Status,
        //    int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        StoreSessionVariable(ddlSearchBy, txtSearch);
        //        if (!string.IsNullOrWhiteSpace(status))
        //            Session["CMGTStatus"] = status;
        //        else
        //            status = (string)Session["CMGTStatus"];

        //        Session["CMGTSearched"] = fromDate;

        //        string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
        //        UserService us = new UserService();
        //        Dictionary<string, object> criteriaUserAppRole = new Dictionary<string, object>();
        //        criteriaUserAppRole.Add("UserId", userid);
        //        criteriaUserAppRole.Add("AppCode", "ISM");
        //        Dictionary<long, IList<UserAppRole>> userAppRole = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteriaUserAppRole);
        //        if (userAppRole != null && userAppRole.Count > 0 && userAppRole.First().Key > 0)
        //        {
        //            int count = userAppRole.First().Value.Count;
        //            //if it has values then for each concatenate APP+ROLE 
        //            string[] userRoles = new string[count];
        //            string[] deptCodeArr = new string[count];
        //            string[] brnCodeArr = new string[count];
        //            string[] GradeArr = new string[count];
        //            int i = 0;
        //            foreach (UserAppRole uar in userAppRole.First().Value)
        //            {
        //                string deptCode = uar.DeptCode;
        //                string branchCode = uar.BranchCode;
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
        //                if (!string.IsNullOrEmpty(uar.Grade))
        //                    GradeArr[i] = uar.Grade;
        //                i++;
        //            }
        //            brnCodeArr = brnCodeArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        //            deptCodeArr = deptCodeArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        //            GradeArr = GradeArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            criteria.Add("BranchCode", brnCodeArr);
        //            if (status != "Assigned")
        //                criteria.Add("DeptCode", deptCodeArr);
        //            if (!string.IsNullOrEmpty(txtSearch))
        //            {
        //                txtSearch.Trim();
        //                if (ddlSearchBy == "IssueDate")
        //                {
        //                    var issuedate = Convert.ToDateTime(txtSearch);
        //                    criteria.Add("CallManagementView." + ddlSearchBy.Trim(), issuedate);
        //                }
        //                else
        //                {
        //                    criteria.Add("CallManagementView." + ddlSearchBy.Trim(), txtSearch);
        //                }
        //            }
        //            if ((!string.IsNullOrEmpty(fromDate) && !(string.IsNullOrEmpty(fromDate.Trim()))))
        //            {
        //                fromDate = fromDate.Trim();
        //                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
        //                string To = string.Format("{0:MM/dd/yyyy}", DateTime.Now);
        //                DateTime[] fromto = new DateTime[2];
        //                fromto[0] = DateTime.Parse(fromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
        //                fromto[1] = Convert.ToDateTime(To + " " + "23:59:59");
        //                criteria.Add("CallManagementView." + "IssueDate", fromto);
        //            }
        //            if (!string.IsNullOrEmpty(status))
        //            {
        //                if (status == "Available")
        //                    criteria.Add("Available", true);
        //                else if (status == "Assigned")
        //                {
        //                    criteria.Add("Assigned", true);
        //                    criteria.Add("Performer", userid);
        //                }
        //                else if (status == "Sent") criteria.Add("Completed", true);
        //                else if (status == "Completed")
        //                {
        //                    criteria.Add("Completed", true);
        //                    criteria.Add("ActivityName", "Complete");
        //                }
        //            }

        //            if (!string.IsNullOrEmpty(IssueNumber)) { criteria.Add("CallManagementView." + "IssueNumber", IssueNumber); }
        //            if (!string.IsNullOrEmpty(BranchCode)) { criteria.Add("CallManagementView." + "BranchCode", BranchCode); }
        //            if (!string.IsNullOrEmpty(StudentName)) { criteria.Add("CallManagementView." + "StudentName", StudentName); }
        //            if (!string.IsNullOrEmpty(Grade)) { criteria.Add("CallManagementView." + "Grade", Grade); }
        //            else if (GradeArr.Count() > 0) { criteria.Add("CallManagementView." + "Grade", GradeArr); }
        //            else { }
        //            if (!string.IsNullOrEmpty(InformationFor)) { criteria.Add("CallManagementView." + "InformationFor", InformationFor); }
        //            if (!string.IsNullOrEmpty(LeaveType)) { criteria.Add("CallManagementView." + "LeaveType", LeaveType); }
        //            if (!string.IsNullOrEmpty(IssueGroup)) { criteria.Add("CallManagementView." + "IssueGroup", IssueGroup); }
        //            if (!string.IsNullOrEmpty(IssueType)) { criteria.Add("CallManagementView." + "IssueType", IssueType); }
        //            // if (!string.IsNullOrEmpty(Status)) { criteria.Add("CallManagementView." + "Status", Status); }

        //            string[] alias = new string[1];
        //            alias[0] = "CallManagementView";
        //            sidx = "CallManagementView." + sidx;
        //            sord = sord == "desc" ? "Desc" : "Asc";

        //            Dictionary<long, IList<CallMgmntActivity>> ActivitiesList = null;
        //            ProcessFlowServices pfs = new ProcessFlowServices(); // TODO: Initialize to an appropriate value
        //            if (!string.IsNullOrEmpty(txtSearch) || !string.IsNullOrEmpty(IssueNumber) || !string.IsNullOrEmpty(BranchCode) || !string.IsNullOrEmpty(StudentName)
        //                || !string.IsNullOrEmpty(Grade) || !string.IsNullOrEmpty(InformationFor) || !string.IsNullOrEmpty(LeaveType) || !string.IsNullOrEmpty(IssueGroup)
        //                || !string.IsNullOrEmpty(IssueType) || !string.IsNullOrEmpty(Status))
        //            {
        //                ActivitiesList = pfs.GetActivityListWithsearchCriteriaLikeSearch(page - 1, rows, sidx, sord, criteria, "AppRole", userRoles, alias);
        //            }
        //            else
        //            {
        //                ActivitiesList = pfs.GetActivityListWithsearchCriteria(page - 1, rows, sidx, sord, criteria, "AppRole", userRoles, alias);
        //            }

        //            if (ActivitiesList != null && ActivitiesList.Count > 0)
        //            {
        //                //
        //                #region " Written for Salem main campus with add the section "


        //                var query = (from quer in userAppRole.First().Value
        //                             where quer.BranchCode == "SALEM MAIN" && quer.DeptCode == "ACADEMICS-MYP" && quer.Section != null
        //                             select quer.Section).Distinct().ToList();

        //                if (query.Count != 0)
        //                {
        //                    foreach (var item in ActivitiesList.First().Value.ToList())
        //                    {
        //                        if (item.CallManagementView.BranchCode == "SALEM MAIN" && item.CallManagementView.Section == "A" && query[0] == "B")
        //                        {

        //                            ActivitiesList.First().Value.Remove(ActivitiesList.First().Value.Single(s => s.Id == item.Id));

        //                        }
        //                        else if (item.CallManagementView.BranchCode == "SALEM MAIN" && item.CallManagementView.Section == "B" && query[0] == "A")
        //                        {
        //                            ActivitiesList.First().Value.Remove(ActivitiesList.First().Value.Single(s => s.Id == item.Id));

        //                        }
        //                        else { }

        //                    }
        //                }

        //                #endregion " END "
        //                //
        //                #region List
        //                foreach (CallMgmntActivity pi in ActivitiesList.First().Value)
        //                {
        //                    pi.ProcessInstance.DifferenceInHours = DateTime.Now - pi.ProcessInstance.DateCreated;
        //                }
        //                long totalRecords = ActivitiesList.First().Key;
        //                int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
        //                if (status == "Sent" || status == "Completed")
        //                {
        //                    var jsonData = new
        //                    {
        //                        total = totalPages,
        //                        page = page,
        //                        records = totalRecords,
        //                        rows = (
        //                             from items in ActivitiesList.First().Value
        //                             select new
        //                             {
        //                                 i = items.Id,
        //                                 cell = new string[] 
        //                             {  
        //                               items.Id.ToString(),
        //                               "<a href='/Home/CallForward?id=" + items.CallManagementView.Id + "&activityId=" + items.Id + "&activityName=" + items.ActivityName + "&status=" + status +"&activityFullName=" + items.ActivityFullName + "'>" + items.CallManagementView.IssueNumber + "</a>",
        //                               items.CallManagementView.BranchCode,
        //                               items.CallManagementView.StudentName,
        //                               items.CallManagementView.Grade,
        //                               items.CallManagementView.IssueDate!=null?items.CallManagementView.IssueDate.Value.Date.ToString("dd/MM/yyyy"):null,
        //                               items.CallManagementView.InformationFor,
        //                               items.CallManagementView.LeaveType,
        //                               items.CallManagementView.IssueGroup,
        //                               items.CallManagementView.IssueType,
        //                               items.ActivityFullName, 
        //                               items.CallManagementView.ActionDate!=null? items.CallManagementView.ActionDate.Value.Date.ToString("dd/MM/yyyy"):null,
        //                               items.CallManagementView.UserType,
        //                               "<img src='/Images/History.png ' id='ImgHistory' onclick=\"ShowComments('" + items.ProcessRefId +"' , '"+items.CallManagementView.UserType+"');\" />",
        //                               items.ProcessInstance.Status=="Completed"?"Completed":items.ProcessInstance.DifferenceInHours.Value.TotalHours.ToString(),
        //                               items.CallManagementView.Email,
        //                               items.CallManagementView.Resolution,
        //                               items.CallManagementView.Id.ToString(),
        //                               items.CallManagementView.Description
        //                             }
        //                             })
        //                    };
        //                    return Json(jsonData, JsonRequestBehavior.AllowGet);
        //                }
        //                else
        //                {
        //                    var jsonData = new
        //                    {
        //                        total = totalPages,
        //                        page = page,
        //                        records = totalRecords,
        //                        rows = (
        //                             from items in ActivitiesList.First().Value
        //                             select new
        //                             {
        //                                 i = items.Id,
        //                                 cell = new string[] 
        //                           {   
        //                               items.Id.ToString(), 
        //                               "<a href='/Home/CallRegister?id=" + items.CallManagementView.Id + "&activityId=" + items.Id + "&activityName=" + items.ActivityName + "&activityFullName=" + items.ActivityFullName + "'>" + items.CallManagementView.IssueNumber + "</a>",
        //                              items.CallManagementView.BranchCode,
        //                               items.CallManagementView.StudentName,
        //                               items.CallManagementView.Grade,
        //                               items.CallManagementView.IssueDate!=null?items.CallManagementView.IssueDate.Value.ToString("dd/MM/yyyy"):null,
        //                               items.CallManagementView.InformationFor,
        //                               items.CallManagementView.LeaveType,
        //                               items.CallManagementView.IssueGroup,
        //                               items.CallManagementView.IssueType,
        //                               items.ActivityFullName, 
        //                               items.CallManagementView.ActionDate!=null? items.CallManagementView.ActionDate.Value.Date.ToString("dd/MM/yyyy"):null,
        //                               items.CallManagementView.UserType,
        //                              "<img src='/Images/History.png ' id='ImgHistory' onclick=\"ShowComments('" + items.ProcessRefId +"' , '"+items.CallManagementView.UserType+"');\" />",
        //                               items.ProcessInstance.Status=="Completed"?"Completed":items.ProcessInstance.DifferenceInHours.Value.TotalHours.ToString(),
        //                               items.CallManagementView.Email,
        //                               items.CallManagementView.Resolution,
        //                               items.CallManagementView.Id.ToString(),
        //                               items.CallManagementView.Description

        //                           }
        //                             })
        //                    };
        //                    return Json(jsonData, JsonRequestBehavior.AllowGet);
        //                }
        //                #endregion
        //            }
        //        }
        //        return Json(null, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
        //        throw ex;
        //    }
        //}

        public ActionResult CallManagementListJqGrid(long? Id, string ddlSearchBy, string txtSearch, string fromDate, string status, string Grades,
            string IssueNumber, string BranchCode, string StudentName, string Grade, string InformationFor, string LeaveType, string IssueType, string IssueGroup, string Status,
            int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                StoreSessionVariable(ddlSearchBy, txtSearch);
                if (!string.IsNullOrWhiteSpace(status))
                    Session["CMGTStatus"] = status;
                else
                    status = (string)Session["CMGTStatus"];

                Session["CMGTSearched"] = fromDate;

                string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                UserService us = new UserService();
                Dictionary<string, object> criteriaUserAppRole = new Dictionary<string, object>();
                criteriaUserAppRole.Add("UserId", userid);
                criteriaUserAppRole.Add("AppCode", "ISM");
                Dictionary<long, IList<UserAppRole_Vw>> userAppRole = us.GetAppRoleOnlyActiveUsersPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteriaUserAppRole);
                if (userAppRole != null && userAppRole.Count > 0 && userAppRole.First().Key > 0)
                {
                    int count = userAppRole.First().Value.Count;
                    //if it has values then for each concatenate APP+ROLE 
                    string[] userRoles = new string[count];
                    string[] deptCodeArr = new string[count];
                    string[] brnCodeArr = new string[count];
                    string[] GradeArr = new string[count];
                    int i = 0;
                    foreach (UserAppRole_Vw uar in userAppRole.First().Value)
                    {
                        string deptCode = uar.DeptCode;
                        string branchCode = uar.BranchCode;
                        if (!string.IsNullOrWhiteSpace(uar.AppCode.Trim()) && !string.IsNullOrWhiteSpace(uar.RoleCode.Trim()))
                            userRoles[i] = uar.AppCode.Trim() + uar.RoleCode.Trim();
                        if (!string.IsNullOrEmpty(deptCode))
                            deptCodeArr[i] = deptCode;
                        if (!string.IsNullOrEmpty(branchCode))
                            brnCodeArr[i] = branchCode;
                        if (!string.IsNullOrEmpty(uar.Grade))
                            GradeArr[i] = uar.Grade;
                        i++;
                    }
                    brnCodeArr = brnCodeArr.Where(x => !string.IsNullOrEmpty(x)).Distinct().ToArray();
                    deptCodeArr = deptCodeArr.Where(x => !string.IsNullOrEmpty(x)).Distinct().ToArray();
                    GradeArr = GradeArr.Where(x => !string.IsNullOrEmpty(x)).Distinct().ToArray();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("BranchCode", brnCodeArr);
                    if (status != "Assigned")
                        criteria.Add("DeptCode", deptCodeArr);
                    if (!string.IsNullOrEmpty(txtSearch))
                    {
                        txtSearch.Trim();
                        if (ddlSearchBy == "IssueDate")
                        {
                            var issuedate = Convert.ToDateTime(txtSearch);
                            criteria.Add("CallManagementView." + ddlSearchBy.Trim(), issuedate);
                        }
                        else
                        {
                            criteria.Add("CallManagementView." + ddlSearchBy.Trim(), txtSearch);
                        }
                    }
                    if ((!string.IsNullOrEmpty(fromDate) && !(string.IsNullOrEmpty(fromDate.Trim()))))
                    {
                        fromDate = fromDate.Trim();
                        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                        string To = string.Format("{0:MM/dd/yyyy}", DateTime.Now);
                        DateTime[] fromto = new DateTime[2];
                        fromto[0] = DateTime.Parse(fromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        fromto[1] = Convert.ToDateTime(To + " " + "23:59:59");
                        criteria.Add("CallManagementView." + "IssueDate", fromto);
                    }
                    if (!string.IsNullOrEmpty(status))
                    {
                        if (status == "Available")
                            criteria.Add("Available", true);
                        else if (status == "Assigned")
                        {
                            criteria.Add("Assigned", true);
                            criteria.Add("Performer", userid);
                        }
                        else if (status == "Sent") criteria.Add("Completed", true);
                        else if (status == "Completed")
                        {
                            criteria.Add("Completed", true);
                            criteria.Add("ActivityName", "Complete");
                        }
                    }

                    if (!string.IsNullOrEmpty(IssueNumber)) { criteria.Add("CallManagementView." + "IssueNumber", IssueNumber); }
                    if (!string.IsNullOrEmpty(BranchCode)) { criteria.Add("CallManagementView." + "BranchCode", BranchCode); }
                    if (!string.IsNullOrEmpty(StudentName)) { criteria.Add("CallManagementView." + "StudentName", StudentName); }
                    if (!string.IsNullOrEmpty(Grade)) { criteria.Add("CallManagementView." + "Grade", Grade); }
                    else if (GradeArr.Count() > 0) { criteria.Add("CallManagementView." + "Grade", GradeArr); }
                    else { }
                    if (!string.IsNullOrEmpty(InformationFor)) { criteria.Add("CallManagementView." + "InformationFor", InformationFor); }
                    if (!string.IsNullOrEmpty(LeaveType)) { criteria.Add("CallManagementView." + "LeaveType", LeaveType); }
                    if (!string.IsNullOrEmpty(IssueGroup)) { criteria.Add("CallManagementView." + "IssueGroup", IssueGroup); }
                    if (!string.IsNullOrEmpty(IssueType)) { criteria.Add("CallManagementView." + "IssueType", IssueType); }
                    // if (!string.IsNullOrEmpty(Status)) { criteria.Add("CallManagementView." + "Status", Status); }

                    string[] alias = new string[1];
                    alias[0] = "CallManagementView";
                    sidx = "CallManagementView." + sidx;
                    sord = sord == "desc" ? "Desc" : "Asc";

                    Dictionary<long, IList<CallMgmntActivity>> ActivitiesList = null;
                    ProcessFlowServices pfs = new ProcessFlowServices(); // TODO: Initialize to an appropriate value
                    if (!string.IsNullOrEmpty(txtSearch) || !string.IsNullOrEmpty(IssueNumber) || !string.IsNullOrEmpty(BranchCode) || !string.IsNullOrEmpty(StudentName)
                        || !string.IsNullOrEmpty(Grade) || !string.IsNullOrEmpty(InformationFor) || !string.IsNullOrEmpty(LeaveType) || !string.IsNullOrEmpty(IssueGroup)
                        || !string.IsNullOrEmpty(IssueType) || !string.IsNullOrEmpty(Status))
                    {
                        ActivitiesList = pfs.GetActivityListWithsearchCriteriaLikeSearch(page - 1, rows, sidx, sord, criteria, "AppRole", userRoles, alias);
                    }
                    else
                    {
                        ActivitiesList = pfs.GetActivityListWithsearchCriteria(page - 1, rows, sidx, sord, criteria, "AppRole", userRoles, alias);
                    }
                    if (ActivitiesList != null && ActivitiesList.Count > 0)
                    {
                        foreach (CallMgmntActivity pi in ActivitiesList.First().Value)
                        {
                            pi.ProcessInstance.DifferenceInHours = DateTime.Now - pi.ProcessInstance.DateCreated;
                        }
                        long totalRecords = ActivitiesList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        if (status == "Sent" || status == "Completed")
                        {
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
                                       "<a href='/Home/CallForward?id=" + items.CallManagementView.Id + "&activityId=" + items.Id + "&activityName=" + items.ActivityName + "&status=" + status +"&activityFullName=" + items.ActivityFullName + "'>" + items.CallManagementView.IssueNumber + "</a>",
                                       items.CallManagementView.BranchCode,
                                       items.CallManagementView.StudentName,
                                       items.CallManagementView.Grade,
                                       items.CallManagementView.IssueDate!=null?items.CallManagementView.IssueDate.Value.Date.ToString("dd/MM/yyyy"):null,
                                       items.CallManagementView.InformationFor,
                                       items.CallManagementView.LeaveType,
                                       items.CallManagementView.IssueGroup,
                                       items.CallManagementView.IssueType,
                                       items.ActivityFullName, 
                                       items.CallManagementView.ActionDate!=null? items.CallManagementView.ActionDate.Value.Date.ToString("dd/MM/yyyy"):null,
                                       items.CallManagementView.UserType,
                                       "<img src='/Images/History.png ' id='ImgHistory' onclick=\"ShowComments('" + items.ProcessRefId +"' , '"+items.CallManagementView.UserType+"');\" />",
                                       items.ProcessInstance.Status=="Completed"?"Completed":items.ProcessInstance.DifferenceInHours.Value.TotalHours.ToString(),
                                       items.CallManagementView.Email,
                                       items.CallManagementView.Resolution,
                                       items.CallManagementView.Id.ToString(),
                                       items.CallManagementView.Description
                                       
                                     }
                                     })
                            };
                            return Json(jsonData, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
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
                                      items.CallManagementView.BranchCode,
                                       items.CallManagementView.StudentName,
                                       items.CallManagementView.Grade,
                                       items.CallManagementView.IssueDate!=null?items.CallManagementView.IssueDate.Value.ToString("dd/MM/yyyy"):null,
                                       items.CallManagementView.InformationFor,
                                       items.CallManagementView.LeaveType,
                                       items.CallManagementView.IssueGroup,
                                       items.CallManagementView.IssueType,
                                       items.ActivityFullName, 
                                       items.CallManagementView.ActionDate!=null? items.CallManagementView.ActionDate.Value.Date.ToString("dd/MM/yyyy"):null,
                                       items.CallManagementView.UserType,
                                      "<img src='/Images/History.png ' id='ImgHistory' onclick=\"ShowComments('" + items.ProcessRefId +"' , '"+items.CallManagementView.UserType+"');\" />",
                                       items.ProcessInstance.Status=="Completed"?"Completed":items.ProcessInstance.DifferenceInHours.Value.TotalHours.ToString(),
                                       items.CallManagementView.Email,
                                       items.CallManagementView.Resolution,
                                       items.CallManagementView.Id.ToString(),
                                       items.CallManagementView.Description
                                   }
                                     })
                            };
                            return Json(jsonData, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public ActionResult Activities(long? Id, string UserType)
        {
            ViewBag.Id = Id;
            ViewBag.UserType = UserType;
            return View();
        }

        public ActionResult ActivitiesListJqGrid(long? Id, string UserType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                ProcessFlowServices pfs = new ProcessFlowServices(); // TODO: Initialize to an appropriate value
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("ProcessRefId", Id);
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<CallMgmntActivity>> ActivitiesList = pfs.GetActivityListWithsearchCriteria(page - 1, rows, sidx, sord, criteria, string.Empty, null, null);
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
                                         items.Performer != null ? us.GetUserNameByUserId(items.Performer.ToString()) : "",
                                         items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"),
                                         items.AppRole,
                                         items.CallManagementView.Id.ToString()
                                     }
                             })
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

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
                            //StaffActivities AssignActivity = pfs.GetStaffActivitiesById(ActivityId);
                            //StaffIssuesService sis = new StaffIssuesService();
                            //UserService us = new UserService();
                            //StaffIssues staffissues = sis.GetStaffIssuesById(AssignActivity.ProcessRefId);
                            //Dictionary<string, object> Criteria = new Dictionary<string, object>();
                            //Criteria.Add("AppCode", "SIM");
                            //Dictionary<long, IList<UserAppRole_Vw>> UserAppRoleList = us.GetAppRoleOnlyActiveUsersPagingAndCriteria(0, 9999, string.Empty, string.Empty, Criteria);
                            //string AssignedBy = us.GetUserNameByUserId(userId);
                            //string[] ResolverEmail = (from u in UserAppRoleList.First().Value
                            //                          where u.AppCode == "SIM" && u.RoleCode == "GRL" && u.DeptCode == staffissues.DeptCode && u.BranchCode == staffissues.BranchCode && u.UserId == AssignActivity.Performer
                            //                          select u.Email).Distinct().ToArray();
                            //StaffIssuesController sic = new StaffIssuesController();
                            //sic.ActorsEmail(staffissues, "", ResolverEmail, staffissues.IssueNumber, AssignedBy, "", staffissues.IssueGroup, "", "", "Assigned", "", "", "");                            
                        }
                        else
                            isAssigned = pfs.AssignActivity(ActivityId, UserId);
                    }
                    return Json(isAssigned, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
            finally
            {
                pfs = null;
            }
        }

        public JsonResult ReceiverGroup(string Campus, string AppCode, string RoleCode, string IssGroup)
        {
            UserService us = new UserService();
            try
            {
                string userId = base.ValidateUser();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("AppCode", AppCode);
                criteria.Add("RoleCode", RoleCode);
                if (!string.IsNullOrEmpty(Campus))
                    criteria.Add("BranchCode", Campus);
                if (!string.IsNullOrEmpty(IssGroup))
                    criteria.Add("DeptCode", IssGroup);
                Dictionary<long, IList<UserAppRole_Vw>> RcvrGrp = us.GetAppRoleOnlyActiveUsersPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                if (RcvrGrp != null && RcvrGrp.Count > 0 && RcvrGrp.FirstOrDefault().Key > 0)
                {
                    var jsonData = new
                    {
                        rows = (
                        from items in RcvrGrp.FirstOrDefault().Value
                        where items.UserId != userId
                        select new { UserId = items.UserId, UserName = items.UserName }).Distinct().ToArray()
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                return Json(new EmptyResult());
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                return ThrowJSONError(ex);
            }
        }

        //"REVISIT" the below list is not correct. it is in the selection of list of roles but assigining always 0th index value.
        //anbu need to debug and confirm
        public ActionResult CallRegister(long? id, long? activityId, string activityName, string ActivityFullName)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ViewBag.ActivityId = activityId ?? 0;
                    if (id > 0)
                    {
                        CallManagementService cms = new CallManagementService(); // TODO: Initialize to an appropriate value
                        ProcessFlowServices pfs = new ProcessFlowServices();
                        ViewBag.flag = 1;
                        //pass activity id and userid to assign activity to user
                        if (activityName == "TransportPickup" || activityName == "LeaveNotification-Hostel" || activityName == "LeaveNotification-Dayscholar" || activityName == "ParentVisit" || activityName == "ParentPickup")
                        {
                            //do nothing
                        }
                        else
                        {
                            string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                            pfs.AssignActivity((Convert.ToInt64(activityId)), userid);
                        }
                        //pass callmgmnt id to get call mgmnt object
                        CallManagement cm = cms.GetCallManagementById(Convert.ToInt64(id));
                        cm.Status = !string.IsNullOrWhiteSpace(activityName) ? activityName : cm.Status;
                        cm.ActivityFullName = !string.IsNullOrWhiteSpace(ActivityFullName) ? ActivityFullName : cm.ActivityFullName;
                        Session["Time"] = String.Format("{0:h:MM tt}", cm.IssueDate);
                        UserService us = new UserService();
                        cm.CreatedUserName = us.GetUserNameByUserId(cm.UserInbox);
                        return View(cm);
                    }
                    else
                    {
                        Dictionary<string, object> criteria = new Dictionary<string, object>();
                        criteria.Add("UserId", userId);
                        UserService us = new UserService();
                        //"REVISIT" the below list is not correct. it is in the selection of list of roles but assigining always 0th index value.
                        Dictionary<long, IList<UserAppRole>> UserAppRoleList = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                        var UserDetails = (from u in UserAppRoleList.First().Value
                                           where u.UserId == userId
                                           select new { u.RoleName, u.BranchCode }).ToArray();
                        CallManagement cm = new CallManagement();
                        cm.IssueDate = DateTime.Now;
                        cm.UserInbox = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                        cm.Status = "LogIssue";
                        cm.ActivityFullName = "Log Issue";
                        cm.UserRoleName = UserDetails[0].RoleName;
                        User user = (User)Session["objUser"];
                        if (user != null)
                            cm.CreatedUserName = user.UserName;
                        //  cm.ActionDate = DateTime.Now;
                        return View(cm);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        //Code review paused here date 24 Mar 2014
        //JP Anbu//errors on save button press, sick or ordinary check box value is missing, Action date mandatory is not working on save button press
        //review not completed since many scenarios need to be checked.
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CallRegister(CallManagement cm1, FormCollection fc, HttpPostedFileBase file1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (cm1 != null && !string.IsNullOrWhiteSpace(cm1.IssueGroup))
                    {
                        cm1.DeptCode = cm1.IssueGroup.ToUpper();
                    }
                    //cm1.LeaveType = LeaveNotification;
                    cm1.LeaveType = (Request.Form["hdnSick"] == "Sick") ? "Sick" : (Request.Form["hdnOrdinary"] == "Ordinary") ? "Ordinary" : "";
                    if (!string.IsNullOrWhiteSpace(cm1.InformationFor))
                    {
                        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                        cm1.ActionDate = (Request["txtActionDate"] != null) ? (DateTime.Parse(Request["txtActionDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault)) : DateTime.Now;
                        if (cm1.InformationFor != "Leave Notification")
                        {
                            cm1.LeaveType = "";
                        }
                    }
                    if (cm1.IssueGroup != null)
                        cm1.LeaveType = "";
                    UserService us = new UserService();
                    criteria.Add("AppCode", "ISM");
                    Dictionary<long, IList<UserAppRole_Vw>> UserAppRoleList = us.GetAppRoleOnlyActiveUsersPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    string[] Email = (from u in UserAppRoleList.First().Value
                                      where u.RoleCode == "CSE" && u.UserId == cm1.UserInbox
                                      select u.Email).ToArray();
                    string dept = string.Empty;
                    string info = string.Empty;
                    if (cm1.IssueGroup != null)
                        dept = cm1.IssueGroup.ToUpper();
                    else
                    {
                        if (cm1.InformationFor != null && cm1.InformationFor.Contains("Transport"))
                            dept = "TRANSPORT";
                        else if (cm1.InformationFor == "General Info")
                            dept = "GENERALINFO";
                        else dept = cm1.IsHosteller ? "HOSTEL" : "DAYSCHOLAR";
                    }

                    string[] ResolverEmail = (from u in UserAppRoleList.First().Value
                                              where u.DeptCode == dept && u.RoleCode == "GRL" && u.BranchCode == cm1.BranchCode && u.Email != null && u.AppCode == "ISM"
                                              select u.Email).ToArray();

                    string[] ApproverEmail = (from u in UserAppRoleList.First().Value
                                              where u.DeptCode == dept && u.RoleCode == "APP" && u.BranchCode == cm1.BranchCode && u.Email != null && u.AppCode == "ISM"
                                              select u.Email).ToArray();

                    string LogIssuerMailId = Email != null && Email.Length > 0 ? Email[0].ToString() : string.Empty;

                    string ResolverMailId = ResolverEmail != null && ResolverEmail.Length > 0 ? ResolverEmail[0].ToString() : string.Empty;
                    string ApproverMailId = ApproverEmail != null && ApproverEmail.Length > 0 ? ApproverEmail[0].ToString() : string.Empty;

                    string recepient = cm1.Email;
                    string RejComments = Request["txtRejCommentsArea"];
                    string ResolutionComments = cm1.Resolution;
                    //REVISIT Check this modelstate.isvalid can be made avail in the starting itself
                    if (ModelState.IsValid)
                    {
                        CallManagementService cms1 = new CallManagementService();
                        ProcessFlowServices pfs = new ProcessFlowServices(); // TODO: Initialize to an appropriate value
                        long id = 0;
                        if (Request.Form["BtnSave"] == "Save")
                        {
                            #region "Save only"
                            if (cm1.Id == 0)
                            {
                                id = pfs.StartCallManagement(cm1, "CallManagement", Request["UserInbox"]);
                                ViewBag.flag = 1;
                                string cur = (DateTime.Now.Year).ToString().Substring(2);
                                string nxt = (DateTime.Now.Year + 1).ToString().Substring(2);
                                if (DateTime.Now.Month > 5)
                                {
                                    cm1.IssueNumber = "CMGT-" + cur + "-" + nxt + "-" + id.ToString();
                                }
                                else
                                {
                                    cm1.IssueNumber = "CMGT-" + (Convert.ToInt64(cur) - 1).ToString() + "-" + cur + "-" + id.ToString();
                                }
                                cm1.IssueDate = Convert.ToDateTime(Request["IssueDate"]);
                                cm1.UserInbox = Request["UserInbox"];
                                cms1.CreateOrUpdateCallManagement(cm1);
                                TempData["SuccessIssueCreation"] = cm1.IssueNumber;
                                return RedirectToAction("CallRegister", new { id = cm1.Id });
                            }
                            else
                            {
                                ViewBag.flag = 0;
                                cm1.IssueDate = Convert.ToDateTime(Request["IssueDate"]);
                                cm1.UserInbox = Request["UserInbox"];
                                cm1.Status = Request["Status"];
                                if (string.IsNullOrEmpty(cm1.Description))
                                { cm1.Description = Request["txtDescription"]; }
                                cms1.CreateOrUpdateCallManagement(cm1);
                                TempData["SuccessIssueCreation"] = string.Empty;
                                return RedirectToAction("CallRegister", new { id = cm1.Id });
                            }
                            #endregion
                            #region "commented save"
                            //if (cm1.Id == 0)
                            //{
                            //    id = pfs.StartCallManagement(cm1, "CallManagement", Request["UserInbox"]);
                            //    ViewBag.flag = 1;
                            //    cm1.IssueNumber = "CMGT-" + id.ToString();
                            //    cm1.IssueDate = Convert.ToDateTime(Request["IssueDate"]);
                            //    cm1.UserInbox = Request["UserInbox"];
                            //    if (cm1.IssueType != null)
                            //    {
                            //        info = " You have saved an Issue Group on  " + cm1.IssueGroup.ToUpper();
                            //        UpdateInbox(cm1.School, info, userId, cm1.Id, cm1.Status);
                            //    }
                            //    else
                            //    {
                            //        info = " You have saved an Information on " + cm1.InformationFor.ToUpper();
                            //        UpdateInbox(cm1.School, info, userId, cm1.Id, cm1.Status);
                            //    }
                            //    cms1.CreateOrUpdateCallManagement(cm1);
                            //    TempData["SuccessIssueCreation"] = cm1.IssueNumber;
                            //    return RedirectToAction("CallRegister", new { id = cm1.Id });
                            //}
                            //else
                            //{
                            //    ViewBag.flag = 0;
                            //    cm1.IssueDate = Convert.ToDateTime(Request["IssueDate"]);
                            //    cm1.UserInbox = Request["UserInbox"];
                            //    cm1.Status = Request["Status"];
                            //    if (string.IsNullOrEmpty(cm1.Description))
                            //    { cm1.Description = Request["txtDescription"]; }
                            //    if (cm1.IssueType != null)
                            //    {
                            //        info = " You have saved an Issue Type on  " + cm1.IssueGroup.ToUpper();
                            //        UpdateInbox(cm1.School, info, userId, cm1.Id, cm1.Status);
                            //    }
                            //    else
                            //    {
                            //        info = " You have saved an Information on " + cm1.InformationFor.ToUpper();
                            //        UpdateInbox(cm1.School, info, userId, cm1.Id, cm1.Status);
                            //    }
                            //    cms1.CreateOrUpdateCallManagement(cm1);
                            //    TempData["SuccessIssueCreation"] = string.Empty;
                            //    return RedirectToAction("CallRegister", new { id = cm1.Id });
                            //}
                            #endregion

                        }

                        if (cm1 != null)
                        {
                            if (Request.Form["btnCompleteLogIssue"] == "CompleteLogIssue" || Request.Form["btnCompleteLogIssue"] == "Save and Submit")
                            {
                                //Complete button clicked but id not yet created
                                //so create and complete



                                #region "Save and Submit"
                                //Complete button clicked but id not yet created
                                //so create and complete
                                if (cm1.Id == 0)
                                {
                                    cm1.UserType = "CMSUsers";
                                    id = pfs.StartCallManagement(cm1, "CallManagement", Request["UserInbox"]);
                                    ViewBag.flag = 1;
                                    string cur = (DateTime.Now.Year).ToString().Substring(2);
                                    string nxt = (DateTime.Now.Year + 1).ToString().Substring(2);
                                    //string cur = (DateTime.Now.Year - 1).ToString().Substring(2);
                                    //string nxt = (DateTime.Now.Year).ToString().Substring(2);
                                    //cm1.IssueNumber = "CMGT-" + id.ToString();
                                    //cm1.IssueNumber = "CMGT-" + cur + "-" + nxt + "-" + id.ToString();                                   
                                    if (DateTime.Now.Month > 5)
                                    {
                                        cm1.IssueNumber = "CMGT-" + cur + "-" + nxt + "-" + id.ToString();
                                    }
                                    else
                                    {
                                        cm1.IssueNumber = "CMGT-" + (Convert.ToInt64(cur) - 1).ToString() + "-" + cur + "-" + id.ToString();
                                    }
                                    cm1.IssueDate = Convert.ToDateTime(Request["IssueDate"]);
                                    // cm1.ActionDate = Convert.ToDateTime(Request["txtActionDate"]);
                                    cm1.UserInbox = Request["UserInbox"];
                                    cms1.CreateOrUpdateCallManagement(cm1);
                                }
                                if (cm1.IsInformation == true)
                                {
                                    cm1.UserType = "CMSUsers";
                                    //if it is information then issue group and issue type is not required and empty value need to be assigned
                                    cm1.IssueGroup = string.Empty;
                                    cm1.IssueType = string.Empty;
                                    if (cm1.InformationFor == "Transport Pickup")//TransportPickup
                                    {
                                        cm1.Status = "TransportPickup";
                                        //if (cm1.IsHosteller) cm1.Status = "TransportPickup-Hostel"; else cm1.Status = "TransportPickup-Dayscholar";
                                        pfs.CreateInformationActivity(cm1, "CallManagement", userId, "LogIssue", "TransportPickup", cm1.IsHosteller);
                                    }

                                    else if (cm1.InformationFor == "Leave Notification")
                                    {
                                        SaveAttendance(cm1);
                                        //LeaveNotification-Hostel    LeaveNotification-Dayscholar
                                        if (cm1.IsHosteller) cm1.Status = "LeaveNotification-Hostel"; else cm1.Status = "LeaveNotification-Dayscholar";
                                        pfs.CreateInformationActivity(cm1, "CallManagement", userId, "LogIssue", "LeaveNotification", cm1.IsHosteller);
                                    }

                                    else if (cm1.InformationFor == "Parent Pickup")
                                    {
                                        cm1.Status = "ParentPickup";

                                        //  if (cm1.IsHosteller) cm1.Status = "ParentPickup-Hostel"; else cm1.Status = "ParentPickup-Dayscholar";
                                        pfs.CreateInformationActivity(cm1, "CallManagement", userId, "LogIssue", "ParentPickup", cm1.IsHosteller);
                                    }
                                    else if (cm1.InformationFor == "General Info")
                                    {
                                        cm1.Status = "GeneralInfo";
                                        pfs.CreateInformationActivity(cm1, "CallManagement", userId, "LogIssue", "GeneralInfo", cm1.IsHosteller);
                                    }

                                    // Function For Sending mail

                                    SendEmail(cm1, LogIssuerMailId, recepient, cm1.IssueNumber, cm1.InformationFor, cm1.StudentName, cm1.IssueGroup, cm1.CallPhoneNumber, RejComments, cm1.Status, ResolutionComments, cm1.LeaveType);
                                    ActorsEmail(cm1, LogIssuerMailId, ResolverEmail, recepient, cm1.IssueNumber, cm1.InformationFor, cm1.StudentName, cm1.IssueGroup, cm1.CallPhoneNumber, RejComments, cm1.Status, ApproverEmail, ResolutionComments, cm1.LeaveType);
                                    TempData["SubmitSuccessMsg"] = cm1.IssueNumber;
                                    return RedirectToAction("CallManagement");
                                }
                                else
                                {
                                    cm1.IsInformation = false;// = string.Empty;
                                    cm1.InformationFor = string.Empty;
                                    cm1.LeaveType = string.Empty;
                                }
                                //write the complete activity logic
                                cm1.UserType = "CMSUsers";

                                pfs.CompleteActivityCallManagement(cm1, "CallManagement", userId, "LogIssue", false);
                                //    if (cm1.IssueType == "Extended Leave Request")
                                //    SaveAttendance(cm1);
                                if (cm1.IssueGroup == "Tipstech")
                                {
                                    // SendEmail(cm1, LogIssuerMailId, recepient, cm1.IssueNumber, cm1.InformationFor, cm1.StudentName, cm1.IssueGroup, cm1.CallPhoneNumber, RejComments, cm1.Status, ResolutionComments, LeaveNotification);
                                    pfs.CompleteActivityCallManagement(cm1, "CallManagement", userId, cm1.Status, false);
                                    pfs.CompleteActivityCallManagement(cm1, "CallManagement", userId, cm1.Status, false);
                                    pfs.CompleteActivityCallManagement(cm1, "CallManagement", userId, "Complete", false);
                                    SendTipstechMail(cm1);
                                }
                                // Function For Sending mail
                                else
                                {
                                    SendEmail(cm1, LogIssuerMailId, recepient, cm1.IssueNumber, cm1.InformationFor, cm1.StudentName, cm1.IssueGroup, cm1.CallPhoneNumber, RejComments, cm1.Status, ResolutionComments, cm1.LeaveType);
                                    ActorsEmail(cm1, LogIssuerMailId, ResolverEmail, recepient, cm1.IssueNumber, cm1.InformationFor, cm1.StudentName, cm1.IssueGroup, cm1.CallPhoneNumber, RejComments, cm1.Status, ApproverEmail, ResolutionComments, cm1.LeaveType);
                                }
                                //redirect to list page
                                TempData["SubmitSuccessMsg"] = cm1.IssueNumber;
                                return RedirectToAction("CallManagement");
                                #endregion


                                //if (cm1.Id == 0)
                                //{
                                //    cm1.UserType = "CMSUsers";
                                //    id = pfs.StartCallManagement(cm1, "CallManagement", Request["UserInbox"]);
                                //    ViewBag.flag = 1;
                                //    cm1.IssueNumber = "CMGT-" + id.ToString();
                                //    cm1.IssueDate = Convert.ToDateTime(Request["IssueDate"]);
                                //    // cm1.ActionDate = Convert.ToDateTime(Request["txtActionDate"]);
                                //    cm1.UserInbox = Request["UserInbox"];
                                //    if (cm1.IssueType != null)
                                //    {
                                //        info = " You have saved and submitted an Issue Type on  " + cm1.IssueGroup.ToUpper();
                                //        UpdateInbox(cm1.School, info, userId, cm1.Id, cm1.Status);
                                //    }
                                //    else
                                //    {
                                //        info = " You have saved and submitted an Information on " + cm1.InformationFor.ToUpper();
                                //        UpdateInbox(cm1.School, info, userId, cm1.Id, cm1.Status);
                                //    }
                                //    cms1.CreateOrUpdateCallManagement(cm1);
                                //}
                                //if (cm1.IsInformation == true)
                                //{
                                //    cm1.UserType = "CMSUsers";
                                //    //if it is information then issue group and issue type is not required and empty value need to be assigned
                                //    cm1.IssueGroup = string.Empty;
                                //    cm1.IssueType = string.Empty;
                                //    if (cm1.InformationFor == "Transport Pickup")//TransportPickup
                                //    {
                                //        cm1.Status = "TransportPickup";
                                //        //if (cm1.IsHosteller) cm1.Status = "TransportPickup-Hostel"; else cm1.Status = "TransportPickup-Dayscholar";
                                //        pfs.CreateInformationActivity(cm1, "CallManagement", userId, "LogIssue", "TransportPickup", cm1.IsHosteller);
                                //    }

                                //    else if (cm1.InformationFor == "Leave Notification")
                                //    {
                                //        SaveAttendance(cm1);
                                //        //LeaveNotification-Hostel    LeaveNotification-Dayscholar
                                //        if (cm1.IsHosteller) cm1.Status = "LeaveNotification-Hostel"; else cm1.Status = "LeaveNotification-Dayscholar";
                                //        pfs.CreateInformationActivity(cm1, "CallManagement", userId, "LogIssue", "LeaveNotification", cm1.IsHosteller);
                                //    }

                                //    else if (cm1.InformationFor == "Parent Pickup")
                                //    {
                                //        cm1.Status = "ParentPickup";

                                //        pfs.CreateInformationActivity(cm1, "CallManagement", userId, "LogIssue", "ParentPickup", cm1.IsHosteller);
                                //    }
                                //    else if (cm1.InformationFor == "General Info")
                                //    {
                                //        cm1.Status = "GeneralInfo";

                                //        pfs.CreateInformationActivity(cm1, "CallManagement", userId, "LogIssue", "GeneralInfo", cm1.IsHosteller);
                                //    }
                                //    // Function For Sending mail

                                //    SendEmail(cm1, LogIssuerMailId, recepient, cm1.IssueNumber, cm1.InformationFor, cm1.StudentName, cm1.IssueGroup, cm1.CallPhoneNumber, RejComments, cm1.Status, ResolutionComments, cm1.LeaveType);
                                //    ActorsEmail(cm1, LogIssuerMailId, ResolverEmail, recepient, cm1.IssueNumber, cm1.InformationFor, cm1.StudentName, cm1.IssueGroup, cm1.CallPhoneNumber, RejComments, cm1.Status, ApproverEmail, ResolutionComments, cm1.LeaveType);
                                //    TempData["SubmitSuccessMsg"] = cm1.IssueNumber;
                                //    return RedirectToAction("CallManagement");
                                //}
                                //else
                                //{
                                //    cm1.IsInformation = false;// = string.Empty;
                                //    cm1.InformationFor = string.Empty;
                                //    cm1.LeaveType = string.Empty;
                                //}
                                ////write the complete activity logic
                                //cm1.UserType = "CMSUsers";

                                //pfs.CompleteActivityCallManagement(cm1, "CallManagement", userId, "LogIssue", false);
                                ////    if (cm1.IssueType == "Extended Leave Request")
                                ////    SaveAttendance(cm1);
                                //if (cm1.IssueGroup == "Tipstech")
                                //{
                                //    // SendEmail(cm1, LogIssuerMailId, recepient, cm1.IssueNumber, cm1.InformationFor, cm1.StudentName, cm1.IssueGroup, cm1.CallPhoneNumber, RejComments, cm1.Status, ResolutionComments, LeaveNotification);
                                //    pfs.CompleteActivityCallManagement(cm1, "CallManagement", userId, cm1.Status, false);
                                //    pfs.CompleteActivityCallManagement(cm1, "CallManagement", userId, cm1.Status, false);
                                //    pfs.CompleteActivityCallManagement(cm1, "CallManagement", userId, "Complete", false);
                                //    SendTipstechMail(cm1);
                                //}
                                //// Function For Sending mail
                                //else
                                //{
                                //    SendEmail(cm1, LogIssuerMailId, recepient, cm1.IssueNumber, cm1.InformationFor, cm1.StudentName, cm1.IssueGroup, cm1.CallPhoneNumber, RejComments, cm1.Status, ResolutionComments, cm1.LeaveType);
                                //    ActorsEmail(cm1, LogIssuerMailId, ResolverEmail, recepient, cm1.IssueNumber, cm1.InformationFor, cm1.StudentName, cm1.IssueGroup, cm1.CallPhoneNumber, RejComments, cm1.Status, ApproverEmail, ResolutionComments, cm1.LeaveType);
                                //}
                                ////redirect to list page
                                //TempData["SubmitSuccessMsg"] = cm1.IssueNumber;
                                //return RedirectToAction("CallManagement");
                            }
                            if (Request.Form["DocUpload"] == "Upload")
                            {
                                string path = file1.InputStream.ToString();
                                byte[] imageSize = new byte[file1.ContentLength];

                                file1.InputStream.Read(imageSize, 0, (int)file1.ContentLength);
                                Documents d = new Documents();
                                d.EntityRefId = cm1.Id;
                                d.FileName = file1.FileName;
                                d.UploadedOn = DateTime.Now;
                                d.UploadedBy = Session["UserId"].ToString();
                                d.DocumentData = imageSize;
                                d.DocumentSize = file1.ContentLength.ToString();
                                d.AppName = "CMS";
                                d.DocumentType = Request["ddlDocumentType"];
                                DocumentsService ds = new DocumentsService();
                                ds.CreateOrUpdateDocuments(d);
                                ViewBag.flag = 1;
                                ViewData["imageid"] = d.Upload_Id;
                                return View(cm1);
                            }
                            if (Request.Form["btnCompleteInfo"] == "Complete Info")
                            {
                                pfs.CompleteInformationActivity(cm1, "CallManagement", cm1.UserInbox, cm1.Status);
                                SendEmail(cm1, LogIssuerMailId, recepient, cm1.IssueNumber, cm1.InformationFor, cm1.StudentName, cm1.IssueGroup, cm1.CallPhoneNumber, RejComments, cm1.Status, ResolutionComments, cm1.LeaveType);
                                ActorsEmail(cm1, LogIssuerMailId, ResolverEmail, recepient, cm1.IssueNumber, cm1.InformationFor, cm1.StudentName, cm1.IssueGroup, cm1.CallPhoneNumber, RejComments, cm1.Status, ApproverEmail, ResolutionComments, cm1.LeaveType);
                                return RedirectToAction("CallManagement");
                            }
                            //reject issue logic
                            if (Request.Form["btnRejectIssue"] == "Reject Issue" || Request.Form["btnRejectIssue"] == "More Info Required")
                            {
                                string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                                CommentsService cmntsSrvc = new CommentsService();
                                Comments cmntsObj = new Comments();
                                cmntsObj.EntityRefId = cm1.Id;

                                cmntsObj.CommentedBy = userid;
                                cmntsObj.CommentedOn = DateTime.Now;
                                cmntsObj.RejectionComments = Request["txtRejCommentsArea"];
                                cmntsSrvc.CreateOrUpdateComments(cmntsObj);

                                if (!string.IsNullOrEmpty(cm1.UserType))
                                {
                                    if (cm1.UserType == "CMSUsers")
                                    {
                                        info = " You have Rejected an Issue Type on  " + cm1.IssueGroup.ToUpper() + "With issue number " + cm1.IssueNumber;
                                        UpdateInbox(cm1.School, info, userId, cm1.Id, cm1.Status);
                                        pfs.CompleteActivityCallManagement(cm1, "CallManagement", userId, cm1.Status, true);
                                    }
                                    else if (cm1.UserType == "Parent")
                                    {
                                        info = " You have Rejected an Issue Type on  " + cm1.IssueGroup.ToUpper() + "With issue number " + cm1.IssueNumber;
                                        UpdateInbox(cm1.School, info, userId, cm1.Id, cm1.Status);
                                        pfs.CompleteActivityCallManagement(cm1, "ParentPortal", userId, cm1.Status, true);
                                        SendMailToParent(cm1, cm1.IssueNumber, cm1.Status, cm1.Email, RejComments);
                                    }
                                    else if (cm1.UserType == "Student")
                                    {
                                        info = " You have Rejected an Issue Type on  " + cm1.IssueGroup.ToUpper() + "With issue number " + cm1.IssueNumber;
                                        UpdateInbox(cm1.School, info, userId, cm1.Id, cm1.Status);
                                        pfs.CompleteActivityCallManagement(cm1, "StudentPortal", userId, cm1.Status, true);
                                    }
                                }

                                if (cm1.Status == "ApproveIssueRejection")
                                    ActorsEmail(cm1, LogIssuerMailId, ResolverEmail, recepient, cm1.IssueNumber, cm1.InformationFor, cm1.StudentName, cm1.IssueGroup, cm1.CallPhoneNumber, RejComments, cm1.Status, ApproverEmail, ResolutionComments, cm1.LeaveType);
                                else
                                {
                                    // Function For Sending mail
                                    if (cm1.UserType == "CMSUsers" || cm1.UserType == "Parent")
                                        ActorsEmail(cm1, LogIssuerMailId, ResolverEmail, recepient, cm1.IssueNumber, cm1.InformationFor, cm1.StudentName, cm1.IssueGroup, cm1.CallPhoneNumber, RejComments, cm1.Status, ApproverEmail, ResolutionComments, cm1.LeaveType);
                                    else if (cm1.UserType == "Student")
                                        SendEmailToStudent(cm1, RejComments);
                                }
                                return RedirectToAction("CallManagement");
                            }

                            if (Request.Form["btnSendMailToParent"] == "Send Mail")
                            {
                                CommentsService cmntsSrvc = new CommentsService();
                                criteria.Clear();
                                criteria.Add("EntityRefId", cm1.Id);
                                Dictionary<long, IList<Comments>> cmntsList = cmntsSrvc.GetCommentsListWithPaging(0, 100, "CommentId", "Desc", criteria);
                                if (cmntsList != null && cmntsList.First().Value != null && cmntsList.First().Value.Count > 0)
                                {
                                    IList<Comments> commenstList = cmntsList.First().Value;
                                    RejComments = commenstList[0].RejectionComments;
                                }
                                SendEmail(cm1, LogIssuerMailId, recepient, cm1.IssueNumber, cm1.InformationFor, cm1.StudentName, cm1.IssueGroup, cm1.CallPhoneNumber, RejComments, cm1.Status, ResolutionComments, cm1.LeaveType);
                                return RedirectToAction("CallManagement");
                            }

                            if (Request.Form["btnReplyIssue"] == "Reply Issue" || Request.Form["btnReplyIssue"] == "Reply")
                            {
                                CommentsService cmntsSrvc1 = new CommentsService();
                                Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                                criteria1.Add("EntityRefId", cm1.Id);
                                Dictionary<long, IList<Comments>> list = cmntsSrvc1.GetCommentsListWithPaging(0, 1000, string.Empty, string.Empty, criteria1);
                                info = " You have Replied an Issue Group on  " + cm1.IssueGroup.ToUpper() + "With issue number " + cm1.IssueNumber;
                                UpdateInbox(cm1.School, info, userId, cm1.Id, cm1.Status);
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
                                if (!string.IsNullOrEmpty(cm1.UserType))
                                {
                                    if (cm1.UserType == "CMSUsers")
                                        pfs.CompleteActivityCallManagement(cm1, "CallManagement", userId, cm1.Status, false);
                                    else if (cm1.UserType == "Parent")
                                        pfs.CompleteActivityCallManagement(cm1, "ParentPortal", userId, cm1.Status, false);
                                    else
                                        pfs.CompleteActivityCallManagement(cm1, "CallManagement", userId, cm1.Status, false);
                                }
                                ActorsEmail(cm1, LogIssuerMailId, ResolverEmail, recepient, cm1.IssueNumber, cm1.InformationFor, cm1.StudentName, cm1.IssueGroup, cm1.CallPhoneNumber, RejComments, cm1.Status, ApproverEmail, ResolutionComments, cm1.LeaveType);
                                // SendEmail(cm1.Email, ResolverMailId, recepient, cm1.IssueNumber, cm1.InformationFor, cm1.StudentName, cm1.IssueGroup, cm1.CallPhoneNumber, RejComments, cm1.Status, ApproverMailId);
                                //redirect to list page
                                return RedirectToAction("CallManagement");
                            }
                            //Resolve issue logic
                            if (Request.Form["btnResolveIssue"] == "Resolve Issue")
                            {
                                //if (!string.IsNullOrEmpty(cm1.UserType))
                                //{
                                //    if (cm1.UserType == "CMSUsers")
                                //    {
                                //        info = " You have Resolved an Issue Group on  " + cm1.IssueGroup.ToUpper() + "With issue number " + cm1.IssueNumber;
                                //        UpdateInbox(cm1.School, info, userId, cm1.Id, cm1.Status);
                                //        pfs.CompleteActivityCallManagement(cm1, "CallManagement", userId, cm1.Status, false);
                                //    }
                                //    else if (cm1.UserType == "Parent")
                                //    {
                                //        info = " You have Resolved an Issue Group on  " + cm1.IssueGroup.ToUpper() + "With issue number " + cm1.IssueNumber;
                                //        UpdateInbox(cm1.School, info, userId, cm1.Id, cm1.Status);
                                //        pfs.CompleteActivityCallManagement(cm1, "ParentPortal", userId, cm1.Status, false);
                                //    }
                                //    else if (cm1.UserType == "Student")
                                //    {
                                //        info = " You have Resolved an Issue Group on  " + cm1.IssueGroup.ToUpper() + "With issue number " + cm1.IssueNumber;
                                //        UpdateInbox(cm1.School, info, userId, cm1.Id, cm1.Status);
                                //        pfs.CompleteActivityCallManagement(cm1, "StudentPortal", userId, cm1.Status, false);
                                //        pfs.CompleteActivityCallManagement(cm1, "StudentPortal", userId, cm1.Status, false);
                                //    }
                                //}
                                //if (cm1.UserType == "CMSUsers" || cm1.UserType == "Parent")
                                //{
                                //    info = " You have Resolved an Issue Group on  " + cm1.IssueGroup.ToUpper() + "With issue number " + cm1.IssueNumber;
                                //    UpdateInbox(cm1.School, info, userId, cm1.Id, cm1.Status);
                                //    if (ApproverMailId != "nirmala@tipsc.info" && !string.IsNullOrEmpty(ApproverMailId))
                                //        ActorsEmail(cm1, LogIssuerMailId, ResolverEmail, recepient, cm1.IssueNumber, cm1.InformationFor, cm1.StudentName, cm1.IssueGroup, cm1.CallPhoneNumber, RejComments, cm1.Status, ApproverEmail, ResolutionComments, cm1.LeaveType);
                                //}
                                //else if (cm1.UserType == "Student")
                                //    SendEmailToStudent(cm1, "");
                                //return RedirectToAction("CallManagement");
                                #region "Resolve Issue"
                                var exceptionIssueGroupList = new List<string> { "Fees / Finance", "Hostel", "Administrative", "HR" };
                                //string IsApproveRequired = ConfigurationManager.AppSettings["IsApproveRequired"];
                                if (!string.IsNullOrEmpty(cm1.UserType))
                                {
                                    if (cm1.UserType == "CMSUsers")
                                    {
                                        pfs.CompleteActivityCallManagement(cm1, "CallManagement", userId, cm1.Status, false);
                                        if (cm1.BranchCode == "IB MAIN" && !exceptionIssueGroupList.Contains(cm1.IssueGroup))
                                        {
                                            pfs.CompleteActivityCallManagement(cm1, "CallManagement", userId, cm1.Status, false);
                                            ActorsEmail(cm1, LogIssuerMailId, ResolverEmail, recepient, cm1.IssueNumber, cm1.InformationFor, cm1.StudentName, cm1.IssueGroup, cm1.CallPhoneNumber, RejComments, cm1.Status, ApproverEmail, ResolutionComments, cm1.LeaveType);
                                        }
                                    }
                                    else if (cm1.UserType == "Parent")
                                    {
                                        pfs.CompleteActivityCallManagement(cm1, "ParentPortal", userId, cm1.Status, false);
                                        if (cm1.BranchCode == "IB MAIN" && !exceptionIssueGroupList.Contains(cm1.IssueGroup))
                                        {
                                            pfs.CompleteActivityCallManagement(cm1, "ParentPortal", userId, cm1.Status, false);
                                            SendEmail(cm1, LogIssuerMailId, recepient, cm1.IssueNumber, cm1.InformationFor, cm1.StudentName, cm1.IssueGroup, cm1.CallPhoneNumber, RejComments, cm1.Status, ResolutionComments, cm1.LeaveType);
                                        }
                                    }
                                    else if (cm1.UserType == "Student")
                                    {
                                        pfs.CompleteActivityCallManagement(cm1, "StudentPortal", userId, cm1.Status, false);
                                        pfs.CompleteActivityCallManagement(cm1, "StudentPortal", userId, cm1.Status, false);
                                        SendEmailToStudent(cm1, "");
                                    }
                                }
                                if (cm1.BranchCode == "IB MAIN" && !exceptionIssueGroupList.Contains(cm1.IssueGroup))
                                { }
                                else
                                {
                                    if (cm1.UserType == "CMSUsers" || cm1.UserType == "Parent")
                                    {
                                        if (ApproverMailId != "nirmala@tipsc.info" && !string.IsNullOrEmpty(ApproverMailId))
                                            ActorsEmail(cm1, LogIssuerMailId, ResolverEmail, recepient, cm1.IssueNumber, cm1.InformationFor, cm1.StudentName, cm1.IssueGroup, cm1.CallPhoneNumber, RejComments, cm1.Status, ApproverEmail, ResolutionComments, cm1.LeaveType);
                                    }
                                }
                                return RedirectToAction("CallManagement");
                                #endregion
                            }
                            //Approve issue logic
                            if (Request.Form["btnApproveIssue"] == "Approve Issue")
                            {
                                if (!string.IsNullOrEmpty(cm1.UserType))
                                {
                                    if (cm1.UserType == "CMSUsers")
                                    {
                                        info = " You have Approved an Issue Group on  " + cm1.IssueGroup.ToUpper() + "With issue number " + cm1.IssueNumber;
                                        UpdateInbox(cm1.School, info, userId, cm1.Id, cm1.Status);
                                        pfs.CompleteActivityCallManagement(cm1, "CallManagement", userId, cm1.Status, false);
                                        ActorsEmail(cm1, LogIssuerMailId, ResolverEmail, recepient, cm1.IssueNumber, cm1.InformationFor, cm1.StudentName, cm1.IssueGroup, cm1.CallPhoneNumber, RejComments, cm1.Status, ApproverEmail, ResolutionComments, cm1.LeaveType);
                                    }
                                    else if (cm1.UserType == "Parent")
                                    {
                                        info = " You have Approved an Issue Group on  " + cm1.IssueGroup.ToUpper() + "With issue number " + cm1.IssueNumber;
                                        UpdateInbox(cm1.School, info, userId, cm1.Id, cm1.Status);
                                        pfs.CompleteActivityCallManagement(cm1, "ParentPortal", userId, cm1.Status, false);
                                        pfs.CompleteActivityCallManagement(cm1, "ParentPortal", userId, "Complete", false);
                                        SendEmail(cm1, LogIssuerMailId, recepient, cm1.IssueNumber, cm1.InformationFor, cm1.StudentName, cm1.IssueGroup, cm1.CallPhoneNumber, RejComments, cm1.Status, ResolutionComments, cm1.LeaveType);
                                    }
                                    else
                                    {
                                        info = " You have Resolved an Issue Group on  " + cm1.IssueGroup.ToUpper() + "With issue number " + cm1.IssueNumber;
                                        UpdateInbox(cm1.School, info, userId, cm1.Id, cm1.Status);
                                        pfs.CompleteActivityCallManagement(cm1, "CallManagement", userId, cm1.Status, false);
                                    }
                                }
                                //SendEmail(cm1.Email, ResolverMailId, recepient, cm1.IssueNumber, cm1.InformationFor, cm1.StudentName, cm1.IssueGroup, cm1.CallPhoneNumber, RejComments, cm1.Status, ApproverMailId);

                                return RedirectToAction("CallManagement");
                            }
                            //Complete logic
                            if (Request.Form["btnIdComplete"] == "Complete")
                            {
                                //write the complete activity logic
                                pfs.CompleteActivityCallManagement(cm1, "CallManagement", userId, "Complete", false);
                                info = " You have Completed an Issue Group on  " + cm1.IssueGroup.ToUpper() + "With issue number " + cm1.IssueNumber;
                                UpdateInbox(cm1.School, info, userId, cm1.Id, cm1.Status);
                                SendEmail(cm1, LogIssuerMailId, recepient, cm1.IssueNumber, cm1.InformationFor, cm1.StudentName, cm1.IssueGroup, cm1.CallPhoneNumber, RejComments, cm1.Status, ResolutionComments, cm1.LeaveType);
                                SendMailToActors(cm1);
                                //ActorsEmail(cm1.Email, ResolverMailId, recepient, cm1.IssueNumber, cm1.InformationFor, cm1.StudentName, cm1.IssueGroup, cm1.CallPhoneNumber, RejComments, cm1.Status, ApproverMailId, ResolutionComments);
                                //redirect to list page
                                return RedirectToAction("CallManagement");
                            }
                            return View(cm1);
                        }
                        else return View(cm1);
                    }
                    ViewBag.flag = 1;
                    return View(cm1);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public void SendMailToActors(CallManagement cm)
        {
            BaseController bc = new BaseController();
            IList<CampusEmailId> campusemaildet = bc.GetEmailIdByCampus(cm.BranchCode, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
            string SendEmail = ConfigurationManager.AppSettings["SendEmail1"];
            string From = ConfigurationManager.AppSettings["From"];
            if (SendEmail == "false")
            {
                return;
            }
            else
            {
                try
                {
                    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                    mail.Subject = "Tips support request #" + cm.IssueNumber + " is completed";
                    mail.Body = "Dear Sir/Madam, <br/><br/>" +
                                         mail.Subject + "<br/><br/>" +
                                         "<b>Issue Description</b><br/><br/>" +
                                         cm.Description + "<br/><br/>" +
                                         "<b>Resolution Comments</b><br/><br/>" +
                                         cm.Resolution + "<br/><br/>" +
                                         " Regards,<br/>" +
                                         " TIPS Support desk";

                    if (cm.BranchCode == "ERNAKULAM")
                    {
                        switch (cm.IssueGroup)
                        {
                            case "Administrative":
                                {
                                    mail.To.Add("sita@tipsc.info");
                                    mail.To.Add("nirmala@tipsc.info");
                                    //mail.To.Add("m.anbarasan@xcdsys.com");
                                    //mail.To.Add("creatoranbu@gmail.com");
                                    break;
                                }
                            case "Fees / Finance":
                                {
                                    mail.To.Add("sita@tipsc.info");
                                    mail.To.Add("kripa@tipsglobal.org");
                                    break;
                                }
                            case "HR":
                                {
                                    mail.To.Add("sita@tipsc.info");
                                    mail.To.Add("rajkumar@tipsglobal.org");
                                    break;
                                }
                        }
                    }
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient("localhost", 25);
                    smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = true;
                    if (From == "live")
                    {
                        try
                        {
                            mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                            smtp.Credentials = new System.Net.NetworkCredential
                            (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                            if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                            {
                                smtp.Send(mail);
                            }

                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.Contains("quota"))
                            {
                                mail.From = new MailAddress(campusemaildet.FirstOrDefault().AlternateEmailId);
                                smtp.Credentials = new System.Net.NetworkCredential
                                (campusemaildet.FirstOrDefault().AlternateEmailId, campusemaildet.FirstOrDefault().AlternateEmailIdPassword);
                                if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                {
                                    smtp.Send(mail);
                                }
                            }
                            else
                            {
                                mail.From = new MailAddress(campusemaildet.FirstOrDefault().AlternateEmailId);
                                smtp.Credentials = new System.Net.NetworkCredential
                                (campusemaildet.FirstOrDefault().AlternateEmailId, campusemaildet.FirstOrDefault().AlternateEmailIdPassword);
                                if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                {
                                    smtp.Send(mail);
                                }
                            }
                        }
                    }
                    else if (From == "test")
                    {
                        mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                        smtp.Credentials = new System.Net.NetworkCredential
                        (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                        if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                        {
                            smtp.Send(mail);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                    throw ex;
                }
            }
        }

        public ActionResult CallForward(long id, long activityId, string activityName, string status, string ActivityFullName)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    CallManagementService cms = new CallManagementService(); // TODO: Initialize to an appropriate value
                    ProcessFlowServices pfs = new ProcessFlowServices();
                    CallManagement cm = cms.GetCallManagementById(Convert.ToInt64(id));
                    Activity act = new Activity();
                    cm.Status = activityName;
                    cm.ActivityFullName = ActivityFullName;
                    Session["Time"] = String.Format("{0:h:MM tt}", cm.IssueDate);
                    ViewBag.status = status;
                    UserService us = new UserService();
                    cm.CreatedUserName = us.GetUserNameByUserId(cm.UserInbox);
                    return View(cm);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult StudentInfo()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ms = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    ViewBag.gradeddl = GradeMaster.First().Value;
                    ViewBag.acadddl = AcademicyrMaster.First().Value;
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult StudentInfoListJqGrid(string idno, string name, string Gender, string section, string campname, string grade, string bType, string vanno, string astatus, string acayear, string frmDate, string toDate, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                    else
                    {
                        MasterDataService mds = new MasterDataService();
                        AdmissionManagementService ams = new AdmissionManagementService();
                        Dictionary<string, object> criteria = new Dictionary<string, object>();
                        if ((string.IsNullOrEmpty(idno))
                            && (string.IsNullOrEmpty(name))
                            && (string.IsNullOrEmpty(Gender))
                            && (string.IsNullOrEmpty(section))
                            && (string.IsNullOrEmpty(campname))
                            && (string.IsNullOrEmpty(grade))
                            && (string.IsNullOrEmpty(bType))
                            && (string.IsNullOrEmpty(astatus))
                            && (string.IsNullOrEmpty(acayear))
                            && (string.IsNullOrEmpty(frmDate))
                            && (string.IsNullOrEmpty(toDate))
                            && (string.IsNullOrEmpty(vanno)))
                        {
                            return Json(null, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(idno))
                            {
                                idno = idno.Trim();
                                criteria.Add("NewId", idno);
                            }
                            if (!string.IsNullOrWhiteSpace(name))
                            {
                                name = name.Trim();
                                criteria.Add("Name", name);
                            }
                            if (!string.IsNullOrWhiteSpace(campname))
                            {
                                if (campname.Contains("Select"))
                                {
                                }
                                else
                                    criteria.Add("Campus", campname);
                            }
                            else
                            {
                                UserService us = new UserService();
                                Dictionary<string, object> criteriaUserAppRole = new Dictionary<string, object>();
                                criteriaUserAppRole.Add("UserId", userId);
                                Dictionary<long, IList<UserAppRole>> userAppRole = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteriaUserAppRole);
                                int count = 0;
                                int i = 0;
                                if (userAppRole != null && userAppRole.Count > 0 && userAppRole.First().Key > 0)
                                {
                                    count = userAppRole.First().Value.Count;
                                    string[] brnCodeArr = new string[count];
                                    foreach (UserAppRole uar in userAppRole.First().Value)
                                    {
                                        string branchCode = uar.BranchCode;

                                        if (!string.IsNullOrEmpty(branchCode))
                                        {
                                            brnCodeArr[i] = branchCode;
                                        }
                                        i++;
                                    }
                                    brnCodeArr = brnCodeArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                                    criteria.Add("Campus", brnCodeArr);
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(section))
                            {
                                if (section.Contains("Select"))
                                {
                                }
                                else
                                    criteria.Add("Section", section);
                            }

                            if (!string.IsNullOrWhiteSpace(grade))
                            {
                                if (grade.Contains("Select"))
                                {
                                    grade = "";
                                }
                                else
                                    criteria.Add("Grade", grade);
                            }
                            if (!string.IsNullOrWhiteSpace(bType))
                            {
                                if (bType.Contains("Select"))
                                {
                                }
                                else
                                    criteria.Add("BoardingType", bType);
                            }
                            if (!string.IsNullOrWhiteSpace(astatus))
                            {
                                criteria.Add("AdmissionStatus", astatus);
                            }
                            if (!string.IsNullOrWhiteSpace(acayear))
                            {
                                if (acayear.Contains("Select"))
                                {
                                }
                                else
                                    criteria.Add("AcademicYear", acayear);
                            }
                            if (!string.IsNullOrWhiteSpace(vanno)) { criteria.Add("VanNo", vanno); }

                            if ((!string.IsNullOrEmpty(frmDate) && !(string.IsNullOrEmpty(toDate.Trim()))))
                            {
                                frmDate = frmDate.Trim();
                                frmDate = string.Format("{0:MM/dd/yyyy}", frmDate);
                                string To = string.Format("{0:MM/dd/yyyy}", toDate);
                                DateTime ToDate = Convert.ToDateTime(To + " " + "23:59:59");
                                DateTime fdate = DateTime.Now;
                                DateTime tdate = DateTime.Now;
                                DateTime[] fromto = new DateTime[2];
                                if (!string.IsNullOrEmpty(frmDate))
                                {
                                    fromto[0] = Convert.ToDateTime(frmDate);
                                }
                                if (ToDate != null)
                                {
                                    fromto[1] = ToDate;
                                }
                                criteria.Add("CreatedDate", fromto);
                            }

                            sord = sord == "desc" ? "Desc" : "Asc";
                            Dictionary<long, IList<StudentDetailsExport>> studentdetailslist;
                            if (!string.IsNullOrWhiteSpace(Gender))
                            {
                                criteria.Add("Gender", Gender);
                            }

                            if (!string.IsNullOrWhiteSpace(grade) || !string.IsNullOrWhiteSpace(Gender))
                            {

                                studentdetailslist = ams.GetStudentExportListWithEQsearchCriteria(page - 1, rows, sidx, sord, criteria);
                            }
                            else
                            {
                                studentdetailslist = ams.GetStudentExportListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);

                            }
                            if (studentdetailslist != null && studentdetailslist.Count > 0)
                            {
                                if (ExptXl == 1)
                                {
                                    var List = studentdetailslist.First().Value.ToList();
                                    base.ExptToXL(List, "StudentList", (items => new
                                    {
                                        items.Campus,
                                        items.CreatedDate,
                                        items.NewId,
                                        items.Name,
                                        items.Gender,
                                        items.DOB,
                                        items.Grade,
                                        items.Section,
                                        items.AcademicYear,
                                        items.BoardingType,
                                        items.BGRP,
                                        items.FoodType,
                                        items.TransportRequired,
                                        items.VanNo,
                                        items.FatherName,
                                        items.MotherName,
                                        items.MobileNo,
                                        items.FatherMobileNumber,
                                        items.MotherMobileNumber,
                                        items.EmailId,
                                        items.FatherEmail,
                                        items.MotherEmail,
                                        items.Address,
                                        items.FatherOccupation,
                                        items.MotherOccupation
                                    }));
                                    return new EmptyResult();
                                }
                                else
                                {
                                    long totalrecords = studentdetailslist.First().Key;
                                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                                    var jsondat1 = new
                                    {
                                        total = totalPages,
                                        page = page,
                                        records = totalrecords,

                                        rows = (from items in studentdetailslist.First().Value
                                                select new
                                                {
                                                    i = items.Id,
                                                    cell = new string[] {
                               items.Campus,
                               items.CreatedDate,
                               items.NewId,
                               items.Name,
                               items.Gender,
                               items.DOB,
                               items.Grade,
                               items.Section,
                               items.AcademicYear,
                               items.BoardingType,
                               items.BGRP,
                               items.FoodType,
                               items.TransportRequired,
                               items.VanNo,
                               items.FatherName,
                               items.MotherName,
                               items.MobileNo,
                               items.FatherMobileNumber,
                               items.MotherMobileNumber,
                               items.EmailId,
                               items.FatherEmail,
                               items.MotherEmail,
                               items.Address,
                               items.FatherOccupation,
                               items.MotherOccupation
                            }
                                                })
                                    };
                                    return Json(jsondat1, JsonRequestBehavior.AllowGet);
                                }

                            }
                            else
                            {
                                return Json(null, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CompleteActivity(CallManagement cm, string template, string userid, string activityName, bool rejection)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                    ProcessFlowServices pfs = new ProcessFlowServices();
                    bool isInformation = true;//get the value from checkbox
                    bool CompleteInformation = true;//get the value from button click
                    if (isInformation == true)
                    {
                        pfs.CreateInformationActivity(cm, "CallManagement", userid, activityName, "LeaveNotification", true);
                    }
                    else if (CompleteInformation) pfs.CompleteInformationActivity(cm, "CallManagement", userid, activityName);
                    else pfs.CompleteActivityCallManagement(cm, "CallManagement", userid, activityName, false);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CompleteInformationActivity(string userid)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult CompleteActivity(string template, string userid, string activityName, bool rejection)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    CallManagement cm = new CallManagement();
                    userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                    ProcessFlowServices pfs = new ProcessFlowServices();
                    bool isInformation = true;//get the value from checkbox
                    bool CompleteInformation = true;//get the value from button click
                    if (isInformation == true)
                    {
                        pfs.CreateInformationActivity(cm, "CallManagement", userid, activityName, "LeaveNotification", true);
                    }
                    else if (CompleteInformation) pfs.CompleteInformationActivity(cm, "CallManagement", userid, activityName);
                    else pfs.CompleteActivityCallManagement(cm, "CallManagement", userid, activityName, false);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
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
                    { criteria.Add("EntityRefId", Id); }
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult ExportExcel(string idno, string name, string section, string campname, string grade, string bType, string astatus, string acayear, int rows)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                    else
                    {
                        MasterDataService mds = new MasterDataService();
                        AdmissionManagementService ams = new AdmissionManagementService();
                        Dictionary<string, object> criteria = new Dictionary<string, object>();
                        if (!string.IsNullOrWhiteSpace(idno))
                        {
                            idno = idno.Trim();
                            criteria.Add("NewId", idno);
                        }
                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            name = name.Trim();
                            criteria.Add("Name", name);
                        }
                        if (!string.IsNullOrWhiteSpace(campname))
                        {
                            if (campname.Contains("Select"))
                            {
                            }
                            else
                                criteria.Add("Campus", campname);
                        }

                        if (!string.IsNullOrWhiteSpace(section))
                        {
                            if (section.Contains("Select"))
                            {
                            }
                            else
                                criteria.Add("Section", section);
                        }

                        if (!string.IsNullOrWhiteSpace(grade))
                        {
                            if (grade.Contains("Select"))
                            {
                                grade = "";
                            }
                            else
                                criteria.Add("Grade", grade);
                        }
                        if (!string.IsNullOrWhiteSpace(bType))
                        {
                            if (bType.Contains("Select"))
                            {
                            }
                            else
                                criteria.Add("BoardingType", bType);
                        }
                        if (!string.IsNullOrWhiteSpace(astatus))
                        {
                            criteria.Add("AdmissionStatus", astatus);
                        }

                        if (!string.IsNullOrWhiteSpace(acayear))
                        {
                            if (acayear.Contains("Select"))
                            {
                            }
                            else
                                criteria.Add("AcademicYear", acayear);
                        }
                        Dictionary<long, IList<StudentDetailsExport>> studentdetailslist;
                        if (!string.IsNullOrWhiteSpace(grade))
                        {

                            studentdetailslist = ams.GetStudentExportListWithEQsearchCriteria(0, rows, string.Empty, string.Empty, criteria);
                        }
                        else
                        {
                            studentdetailslist = ams.GetStudentExportListWithPagingAndCriteria(0, rows, string.Empty, string.Empty, criteria);

                        }
                        if (studentdetailslist != null && studentdetailslist.First().Value != null && studentdetailslist.First().Value.Count > 0)
                        {
                            var stuList = studentdetailslist.First().Value.ToList();
                            base.ExptToXL(stuList, "StudentList", (items => new
                            {
                                items.Campus,
                                items.CreatedDate,
                                items.NewId,
                                items.Name,
                                items.Gender,
                                items.Grade,
                                items.Section,
                                items.AcademicYear,
                                items.BoardingType,
                                items.FoodType,
                                items.TransportRequired,
                                items.FatherName,
                                items.MotherName,
                                items.FatherMobileNumber,
                                items.MotherMobileNumber,
                                items.EmailId,
                                items.FatherEmail,
                                items.MotherEmail,
                                items.Address
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult SystemManagement()
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult Masters(string master)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ms = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    MasterDetails md = new MasterDetails();
                    if (master == "assignment")
                    {
                        md.Masters = "AssignmentName";
                        criteria.Add("Masters", "AssignmentName");
                        ViewBag.master = "Assignmentname";
                    }
                    Dictionary<long, IList<MasterDetails>> masterdetailslist = ms.GetMasterDetailListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    criteria.Clear();
                    ViewBag.ddllist = masterdetailslist.First().Value.OrderBy(x => x.Masters);
                    return View(md);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddRegion(RegionMaster rm, string test)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (test == "edit")
                    {
                        MastersService rs = new MastersService();
                        ViewBag.flag = 1;
                        rm.UpdatedBy = "Henry";
                        rm.UpdatedDate = DateTime.Now.ToString("dd/mm/yyyy");

                        rs.CreateOrUpdateRegionMaster(rm);
                    }
                    else
                    {
                        if (rm.RegionName == null)
                        {
                            rm.RegionName = "*";
                        }
                        rm.CreatedBy = "Henry";
                        rm.CreatedDate = DateTime.Now.ToString();//.ToString("dd/mm/yyyy");
                        rm.UpdatedBy = "nil";
                        rm.UpdatedDate = DateTime.Now.ToString(); //"01/01/1999";

                        rm.FormId = 0;
                        rm.FormCode = "nil";
                        MastersService bs = new MastersService();
                        ViewBag.flag = 1;
                        long id = bs.CreateOrUpdateRegionMaster(rm);
                        rm.FormId = id;
                        rm.FormCode = "REGN-" + id.ToString();

                        bs.CreateOrUpdateRegionMaster(rm);
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteRegion(string id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService rs = new MastersService();
                    var test = id.Split(',');
                    long[] idtodelete = new long[test.Length];
                    int i = 0;
                    foreach (string val in test)
                    {
                        idtodelete[i] = Convert.ToInt64(val);
                        i++;
                    }
                    ViewBag.flag = 1;
                    rs.DeleteRegionMaster(idtodelete);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddGrade(GradeMaster gm, string test)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (test == "edit")
                    {
                        MastersService gs = new MastersService();
                        ViewBag.flag = 1;
                        gm.UpdatedBy = "Henry";
                        gm.UpdatedDate = DateTime.Now.ToString("dd/mm/yyyy");

                        gs.CreateOrUpdateGradeMaster(gm);
                    }
                    else
                    {
                        gm.CreatedBy = "Henry";
                        gm.CreatedDate = DateTime.Now.ToString();//.ToString("dd/mm/yyyy");
                        gm.UpdatedBy = "nil";
                        gm.UpdatedDate = DateTime.Now.ToString(); //"01/01/1999";

                        gm.FormId = 0;
                        gm.FormCode = "nil";

                        MastersService gs = new MastersService();
                        ViewBag.flag = 1;
                        long id = gs.CreateOrUpdateGradeMaster(gm);
                        gm.FormId = id;
                        gm.FormCode = "GRAD-" + id.ToString();

                        gs.CreateOrUpdateGradeMaster(gm);
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteGrade(string id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService gs = new MastersService();
                    var test = id.Split(',');
                    long[] idtodelete = new long[test.Length];
                    int i = 0;
                    foreach (string val in test)
                    {
                        idtodelete[i] = Convert.ToInt64(val);
                        i++;
                    }
                    ViewBag.flag = 1;
                    gs.DeleteGradeMaster(idtodelete);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddSection(SectionMaster sm, string test)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (test == "edit")
                    {
                        MastersService ss = new MastersService();
                        ViewBag.flag = 1;
                        ss.CreateOrUpdateSectionMaster(sm);
                    }
                    else
                    {
                        sm.FormId = 0;
                        sm.FormCode = "nil";

                        MastersService ss = new MastersService();
                        ViewBag.flag = 1;
                        long id = ss.CreateOrUpdateSectionMaster(sm);
                        sm.FormId = id;
                        sm.FormCode = "SECT-" + id.ToString();

                        ss.CreateOrUpdateSectionMaster(sm);
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteSection(string id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ss = new MastersService();
                    var test = id.Split(',');
                    long[] idtodelete = new long[test.Length];
                    int i = 0;
                    foreach (string val in test)
                    {
                        idtodelete[i] = Convert.ToInt64(val);
                        i++;
                    }
                    ViewBag.flag = 1;
                    ss.DeleteSectionMaster(idtodelete);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddFeeStructureYear(FeeStructureYearMaster fm, string test)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (test == "edit")
                    {
                        MastersService fs = new MastersService();
                        ViewBag.flag = 1;

                        fs.CreateOrUpdateFeeStructureYearMaster(fm);
                    }
                    else
                    {
                        fm.FormId = 0;
                        fm.FormCode = "nil";
                        MastersService fs = new MastersService();
                        ViewBag.flag = 1;
                        long id = fs.CreateOrUpdateFeeStructureYearMaster(fm);
                        fm.FormId = id;
                        fm.FormCode = "FSTR-" + id.ToString();

                        fs.CreateOrUpdateFeeStructureYearMaster(fm);
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteFeeStructureYear(string id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService fs = new MastersService();
                    var test = id.Split(',');
                    long[] idtodelete = new long[test.Length];
                    int i = 0;
                    foreach (string val in test)
                    {
                        idtodelete[i] = Convert.ToInt64(val);
                        i++;
                    }
                    ViewBag.flag = 1;
                    fs.DeleteFeeStructureYearMaster(idtodelete);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddFeeType(FeeTypeMaster fm, string test)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (test == "edit")
                    {
                        MastersService fs = new MastersService();
                        ViewBag.flag = 1;
                        fs.CreateOrUpdateFeeTypeMaster(fm);
                    }
                    else
                    {
                        fm.FormId = 0;
                        fm.FormCode = "nil";
                        MastersService fs = new MastersService();
                        ViewBag.flag = 1;
                        long id = fs.CreateOrUpdateFeeTypeMaster(fm);
                        fm.FormId = id;
                        fm.FormCode = "FTYP-" + id.ToString();
                        fs.CreateOrUpdateFeeTypeMaster(fm);
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteFeeType(string id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService fs = new MastersService();
                    var test = id.Split(',');
                    long[] idtodelete = new long[test.Length];
                    int i = 0;
                    foreach (string val in test)
                    {
                        idtodelete[i] = Convert.ToInt64(val);
                        i++;
                    }
                    ViewBag.flag = 1;
                    fs.DeleteFeeTypeMaster(idtodelete);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddCampus(CampusMaster cm, string test)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (test == "edit")
                    {
                        MastersService cs = new MastersService();
                        ViewBag.flag = 1;
                        cs.CreateOrUpdateCampusMaster(cm);
                    }
                    else
                    {
                        cm.FormId = 0;
                        cm.FormCode = "nil";
                        MastersService fs = new MastersService();
                        ViewBag.flag = 1;
                        long id = fs.CreateOrUpdateCampusMaster(cm);
                        cm.FormId = id;
                        cm.FormCode = "CAMP-" + id.ToString();
                        fs.CreateOrUpdateCampusMaster(cm);
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteCampus(string id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService fs = new MastersService();
                    var test = id.Split(',');
                    long[] idtodelete = new long[test.Length];
                    int i = 0;
                    foreach (string val in test)
                    {
                        idtodelete[i] = Convert.ToInt64(val);
                        i++;
                    }
                    ViewBag.flag = 1;
                    fs.DeleteCampusMaster(idtodelete);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddRelationships(RelationshipMaster rm, string test)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (test == "edit")
                    {
                        MastersService rs = new MastersService();
                        ViewBag.flag = 1;

                        rs.CreateOrUpdateRelationshipMaster(rm);
                    }
                    else
                    {
                        rm.FormId = 0;
                        rm.FormCode = "nil";
                        MastersService rs = new MastersService();
                        ViewBag.flag = 1;
                        long id = rs.CreateOrUpdateRelationshipMaster(rm);
                        rm.FormId = id;
                        rm.FormCode = "RLTN-" + id.ToString();

                        rs.CreateOrUpdateRelationshipMaster(rm);
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteRelationships(string id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService rs = new MastersService();

                    var test = id.Split(',');

                    long[] idtodelete = new long[test.Length];
                    int i = 0;
                    foreach (string val in test)
                    {
                        idtodelete[i] = Convert.ToInt64(val);
                        i++;
                    }
                    ViewBag.flag = 1;
                    rs.DeleteRelationshipMaster(idtodelete);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddApplication(Application app, string test)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ms = new MastersService();
                    if (test == "edit")
                    {
                        ViewBag.flag = 1;

                        ms.CreateOrUpdateApplicationMaster(app);
                    }
                    else
                    {
                        ViewBag.flag = 1;
                        ms.CreateOrUpdateApplicationMaster(app);
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddAcademicYear(AcademicyrMaster am, string test)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ays = new MastersService();
                    if (test == "edit")
                    {
                        ViewBag.flag = 1;

                        ays.CreateOrUpdateAcademicYearMaster(am);
                    }
                    else
                    {
                        am.FormId = 0;
                        am.FormCode = "nil";

                        ViewBag.flag = 1;
                        long id = ays.CreateOrUpdateAcademicYearMaster(am);
                        am.FormId = id;
                        am.FormCode = "ACAD-" + id.ToString();

                        ays.CreateOrUpdateAcademicYearMaster(am);
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteAcademicYear(string id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ays = new MastersService();
                    var test = id.Split(',');
                    long[] idtodelete = new long[test.Length];
                    int i = 0;
                    foreach (string val in test)
                    {
                        idtodelete[i] = Convert.ToInt64(val);
                        i++;
                    }
                    ViewBag.flag = 1;
                    ays.DeleteAcademicyrMaster(idtodelete);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddModeOfPayment(ModeOfPaymentMaster mm, string test)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (test == "edit")
                    {
                        MastersService ms = new MastersService();
                        ViewBag.flag = 1;

                        ms.CreateOrUpdateModeOfPaymentMaster(mm);
                    }
                    else
                    {
                        mm.FormId = 0;
                        mm.FormCode = "nil";
                        MastersService ays = new MastersService();
                        ViewBag.flag = 1;
                        long id = ays.CreateOrUpdateModeOfPaymentMaster(mm);
                        mm.FormId = id;
                        mm.FormCode = "MDPT-" + id.ToString();

                        ays.CreateOrUpdateModeOfPaymentMaster(mm);
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteModeOfPayment(string id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ms = new MastersService();
                    var test = id.Split(',');
                    long[] idtodelete = new long[test.Length];
                    int i = 0;
                    foreach (string val in test)
                    {
                        idtodelete[i] = Convert.ToInt64(val);
                        i++;
                    }
                    ViewBag.flag = 1;
                    ms.DeleteModeOfPaymentMaster(idtodelete);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddBloodGroup(BloodGroupMaster bm, string test)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService bs = new MastersService();
                    if (test == "edit")
                    {
                        ViewBag.flag = 1;
                        bs.CreateOrUpdateBloodGroupMaster(bm);
                    }
                    else
                    {
                        bm.FormId = 0;
                        bm.FormCode = "nil";
                        ViewBag.flag = 1;
                        long id = bs.CreateOrUpdateBloodGroupMaster(bm);
                        bm.FormId = id;
                        bm.FormCode = "BGRP-" + id.ToString();

                        bs.CreateOrUpdateBloodGroupMaster(bm);
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteBloodGroup(string id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService bs = new MastersService();
                    var test = id.Split(',');
                    long[] idtodelete = new long[test.Length];
                    int i = 0;
                    foreach (string val in test)
                    {
                        idtodelete[i] = Convert.ToInt64(val);
                        i++;
                    }
                    ViewBag.flag = 1;
                    bs.DeleteBloodGroupMaster(idtodelete);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddDocumentType(DocumentTypeMaster dm, string test)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (test == "edit")
                    {
                        MastersService ds = new MastersService();
                        ds.CreateOrUpdateDocumentTypeMaster(dm);
                    }
                    else
                    {
                        dm.FormId = 0;
                        dm.FormCode = "nil";
                        MastersService ds = new MastersService();
                        long id = ds.CreateOrUpdateDocumentTypeMaster(dm);
                        dm.FormId = id;
                        dm.FormCode = "DOTP-" + id.ToString();

                        ds.CreateOrUpdateDocumentTypeMaster(dm);
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteDocumentType(string id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ds = new MastersService();
                    var test = id.Split(',');
                    long[] idtodelete = new long[test.Length];
                    int i = 0;
                    foreach (string val in test)
                    {
                        idtodelete[i] = Convert.ToInt64(val);
                        i++;
                    }
                    ViewBag.flag = 1;
                    ds.DeleteDocumentTypeMaster(idtodelete);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddIssueGroup(IssueGroupMaster im, string test)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService igs = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("IssueGroup", im.IssueGroup);
                    Dictionary<long, IList<IssueGroupMaster>> issuegroup = igs.GetIssueGroupMasterListWithPagingAndCriteria(null, null, null, null, criteria);
                    if (test == "edit")
                    {
                        if (issuegroup != null && issuegroup.First().Value != null && issuegroup.First().Value.Count > 1)
                        {
                            return null; //return Json("false");  //throw new Exception("The given region is already exists!");
                        }
                        else
                        {
                            ViewBag.flag = 1;
                            igs.CreateOrUpdateIssueGroupMaster(im);
                            return null;
                        }
                    }
                    else
                    {
                        if (issuegroup != null && issuegroup.First().Value != null && issuegroup.First().Value.Count > 0)
                        {
                            return null; //return Json("false");  //throw new Exception("The given region is already exists!");
                        }
                        else
                        {
                            im.Id = 0;
                            im.FormCode = "nil";
                            MastersService bs = new MastersService();
                            ViewBag.flag = 1;
                            long id = bs.CreateOrUpdateIssueGroupMaster(im);
                            im.Id = id;
                            im.FormCode = "IGRP-" + id.ToString();
                            bs.CreateOrUpdateIssueGroupMaster(im);
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteIssueGroup(string id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService bs = new MastersService();
                    var test = id.Split(',');
                    long[] idtodelete = new long[test.Length];
                    int i = 0;
                    foreach (string val in test)
                    {
                        idtodelete[i] = Convert.ToInt64(val);
                        i++;
                    }
                    ViewBag.flag = 1;
                    bs.DeleteIssueGroupMaster(idtodelete);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddIssueType(IssueTypeMaster itm, string test)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService its = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("IssueGroup", itm.IssueGroup);
                    criteria.Add("IssueType", itm.IssueType);

                    Dictionary<long, IList<IssueTypeMaster>> issuetype = its.GetIssueTypeMasterListWithPagingAndCriteria(null, null, null, null, criteria);
                    if (test == "edit")
                    {
                        if (issuetype != null && issuetype.First().Value != null && issuetype.First().Value.Count > 1)
                        {
                            return null;
                        }
                        else
                        {
                            ViewBag.flag = 1;
                            its.CreateOrUpdateIssueTypeMaster(itm);
                            return null;
                        }
                    }
                    else
                    {
                        if (issuetype != null && issuetype.First().Value != null && issuetype.First().Value.Count > 0)
                        {
                            return null;
                        }
                        else
                        {
                            itm.FormId = 0;
                            itm.FormCode = "nil";
                            MastersService bs = new MastersService();
                            ViewBag.flag = 1;
                            long id = bs.CreateOrUpdateIssueTypeMaster(itm);
                            itm.FormId = id;
                            itm.FormCode = "ITYP-" + id.ToString();
                            bs.CreateOrUpdateIssueTypeMaster(itm);
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteIssueType(string id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService bs = new MastersService();
                    var test = id.Split(',');
                    long[] idtodelete = new long[test.Length];
                    int i = 0;
                    foreach (string val in test)
                    {
                        idtodelete[i] = Convert.ToInt64(val);
                        i++;
                    }
                    ViewBag.flag = 1;
                    bs.DeleteIssueTypeMaster(idtodelete);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult Regionddl()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService rs = new MastersService();
                    Dictionary<long, string> regi = new Dictionary<long, string>();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();

                    Dictionary<long, IList<RegionMaster>> regionmasterlist = rs.GetRegionMasterListWithPagingAndCriteria(null, null, null, null, criteria);
                    foreach (RegionMaster region in regionmasterlist.First().Value)
                    {
                        regi.Add(region.FormId, region.RegionCod);
                    }
                    return PartialView("Select", regi);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult Issuetypeddl()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService rs = new MastersService();
                    Dictionary<long, string> issuegrp = new Dictionary<long, string>();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<IssueGroupMaster>> issuetypemasterlist = rs.GetIssueGroupMasterListWithPagingAndCriteria(null, null, null, null, criteria);
                    foreach (IssueGroupMaster issuegroup in issuetypemasterlist.First().Value)
                    {
                        issuegrp.Add(issuegroup.Id, issuegroup.IssueGroup);
                    }
                    return PartialView("Select", issuegrp);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddStudentDetails(StudentDetails sdm, string test)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MasterDataService sds = new MasterDataService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("id_no", sdm.id_no);
                    Dictionary<long, IList<StudentDetails>> student = sds.GetStudentDetailsListWithPagingAndCriteria(null, null, null, null, criteria);
                    if (test == "edit")
                    {
                        if (student != null && student.First().Value != null && student.First().Value.Count > 1)
                        {
                            return null; //return Json("false");  //throw new Exception("The given region is already exists!");
                        }
                        else
                        {
                            if (sdm.IsHosteller.ToString() == "Yes")
                            {
                                sdm.IsHosteller = true;
                            }
                            else
                            {
                                sdm.IsHosteller = false;
                            }

                            ViewBag.flag = 1;
                            sds.CreateOrUpdateStudentDetailsMasterList(sdm);
                            return null;
                        }
                    }
                    else
                    {
                        if (student != null && student.First().Value != null && student.First().Value.Count > 0)
                        {
                            return null; //return Json("false");  //throw new Exception("The given region is already exists!");
                        }
                        else
                        {
                            if (sdm.IsHosteller.ToString() == "Yes")
                            {
                                sdm.IsHosteller = true;
                            }
                            else
                            {
                                sdm.IsHosteller = false;
                            }

                            ViewBag.flag = 1;
                            long id = sds.CreateOrUpdateStudentDetailsMasterList(sdm);
                            sds.CreateOrUpdateStudentDetailsMasterList(sdm);
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public JsonResult Matersjqgrid(string id, string txtSearch, string idno, string name, string sect, string cname, string grad, string btype, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                if (id == "Application")
                {
                    MastersService gs = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<Application>> Applicationmasterlist = gs.GetApplicationMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (Applicationmasterlist != null && Applicationmasterlist.Count > 0)
                    {
                        long totalrecords = Applicationmasterlist.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in Applicationmasterlist.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.Id.ToString(),items.AppCode,items.AppName
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

                if (id == "Grade")
                {
                    MastersService gs = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<GradeMaster>> grademasterlist = gs.GetGradeMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (grademasterlist != null && grademasterlist.Count > 0)
                    {
                        long totalrecords = grademasterlist.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in grademasterlist.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.FormId.ToString(),items.FormCode,items.FormCode,items.grad.ToString(),items.gradcod,items.graddesc
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
                else if (id == "Region")
                {
                    MastersService rs = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<RegionMaster>> regionmasterlist = rs.GetRegionMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (regionmasterlist != null && regionmasterlist.Count > 0)
                    {
                        long totalrecords = regionmasterlist.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat1 = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in regionmasterlist.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.FormId.ToString(),items.FormCode,items.FormCode,items.RegionCod,items.RegionName,items.CreatedBy,items.CreatedDate,items.UpdatedBy,items.UpdatedDate
                            }
                                    })
                        };
                        return Json(jsondat1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (id == "Country")
                {
                    MastersService cs = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<CountryMaster>> countrymasterlist = cs.GetCountryMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (countrymasterlist != null && countrymasterlist.Count > 0)
                    {
                        long totalrecords = countrymasterlist.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat1 = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in countrymasterlist.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.FormId.ToString(),items.FormCode,items.FormCode,items.CountryCode,items.CountryName,items.RegionCode                            }
                                    })
                        };
                        return Json(jsondat1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (id == "Section")
                {
                    MastersService ss = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<SectionMaster>> sectionmasterlist = ss.GetSectionMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (sectionmasterlist != null && sectionmasterlist.Count > 0)
                    {
                        long totalrecords = sectionmasterlist.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat1 = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in sectionmasterlist.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.FormId.ToString(),items.FormCode,items.FormCode,items.Section,items.Sectiondesc                                                                          
                            }
                                    })
                        };
                        return Json(jsondat1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (id == "FeeStructureYear")
                {
                    MastersService fs = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<FeeStructureYearMaster>> feestructureyearmasterlist = fs.GetFeeStructureYearMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (feestructureyearmasterlist != null && feestructureyearmasterlist.Count > 0)
                    {
                        long totalrecords = feestructureyearmasterlist.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat1 = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in feestructureyearmasterlist.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.FormId.ToString(),items.FormCode,items.FormCode,items.FeeStructureYear,items.FeeStructureYeardesc                                                                          
                            }
                                    })
                        };
                        return Json(jsondat1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (id == "FeeType")
                {
                    MastersService fs = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<FeeTypeMaster>> FeeTypeMaster = fs.GetFeeTypeMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (FeeTypeMaster != null && FeeTypeMaster.Count > 0)
                    {
                        long totalrecords = FeeTypeMaster.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);

                        var jsondat1 = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in FeeTypeMaster.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.FormId.ToString(),items.FormCode,items.FormCode,items.FeeType,items.FeeTypedesc                                                                          
                            }
                                    })
                        };
                        return Json(jsondat1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (id == "Campus")
                {
                    MastersService fs = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<CampusMaster>> CampusMaster = fs.GetCampusMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (CampusMaster != null && CampusMaster.Count > 0)
                    {
                        long totalrecords = CampusMaster.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat1 = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in CampusMaster.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.FormId.ToString(),items.FormCode,items.FormCode,items.Name,items.Code,items.Location,items.Country
                            }
                                    })
                        };
                        return Json(jsondat1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (id == "BloodGroup")
                {
                    MastersService bs = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<BloodGroupMaster>> bloodgroupmasterlist = bs.GetBloodGroupMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (bloodgroupmasterlist != null && bloodgroupmasterlist.Count > 0)
                    {
                        long totalrecords = bloodgroupmasterlist.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat1 = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in bloodgroupmasterlist.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.FormId.ToString(),items.FormCode,items.FormCode,items.BloodGroup,items.BloodGroupdesc                                                                          
                            }
                                    })
                        };
                        return Json(jsondat1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (id == "Relationships")
                {
                    MastersService rs = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<RelationshipMaster>> bloodgroupmasterlist = rs.GetRelationshipMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (bloodgroupmasterlist != null && bloodgroupmasterlist.Count > 0)
                    {
                        long totalrecords = bloodgroupmasterlist.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat1 = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in bloodgroupmasterlist.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.FormId.ToString(),items.FormCode,items.FormCode,items.Relationships                                     
                            }
                                    })
                        };
                        return Json(jsondat1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (id == "AcademicYear")
                {
                    MastersService ayr = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<AcademicyrMaster>> academicyrmasterlist = ayr.GetAcademicyrMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (academicyrmasterlist != null && academicyrmasterlist.Count > 0)
                    {
                        long totalrecords = academicyrmasterlist.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat1 = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in academicyrmasterlist.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.FormId.ToString(),items.FormCode,items.FormCode,items.AcademicYear,items.AcademicYeardesc                                     
                            }
                                    })
                        };
                        return Json(jsondat1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (id == "ModeOfPayment")
                {
                    MastersService ms = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<ModeOfPaymentMaster>> modeofpmtmasterlist = ms.GetModeOfPaymentMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (modeofpmtmasterlist != null && modeofpmtmasterlist.Count > 0)
                    {
                        long totalrecords = modeofpmtmasterlist.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat1 = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in modeofpmtmasterlist.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.FormId.ToString(),items.FormCode,items.FormCode,items.ModeOfPayment,items.ModeOfPaymentdesc
                            }
                                    })
                        };
                        return Json(jsondat1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (id == "DocumentType")
                {
                    MastersService ds = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<DocumentTypeMaster>> doctypemasterlist = ds.GetDocumentTypeMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (doctypemasterlist != null && doctypemasterlist.Count > 0)
                    {
                        long totalrecords = doctypemasterlist.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat1 = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in doctypemasterlist.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.FormId.ToString(),items.FormCode,items.FormCode,items.DocumentType,items.DocumentTypedesc
                            }
                                    })
                        };
                        return Json(jsondat1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (id == "IssueGroup")
                {
                    MastersService ds = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<IssueGroupMaster>> issuegroupmasterlist = ds.GetIssueGroupMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (issuegroupmasterlist != null && issuegroupmasterlist.Count > 0)
                    {
                        long totalrecords = issuegroupmasterlist.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat1 = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in issuegroupmasterlist.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.Id.ToString(),items.FormCode,items.FormCode,items.IssueGroup
                            }
                                    })
                        };
                        return Json(jsondat1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (id == "IssueType")
                {
                    MastersService ds = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<IssueTypeMaster>> issuetypemasterlist = ds.GetIssueTypeMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (issuetypemasterlist != null && issuetypemasterlist.Count > 0)
                    {
                        long totalrecords = issuetypemasterlist.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat1 = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in issuetypemasterlist.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.FormId.ToString(),items.FormCode,items.FormCode,items.IssueGroup,items.IssueType
                            }
                                    })
                        };
                        return Json(jsondat1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (id == "StudentDetails")
                {
                    MasterDataService sds = new MasterDataService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();

                    if (!string.IsNullOrWhiteSpace(idno))
                    {
                        criteria.Add("id_no", idno);
                    }
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        criteria.Add("name", name);
                    }
                    if (!string.IsNullOrWhiteSpace(cname))
                    {
                        criteria.Add("campus_name", cname);
                    }
                    if (!string.IsNullOrWhiteSpace(sect))
                    {
                        criteria.Add("section", sect);
                    }
                    if (!string.IsNullOrWhiteSpace(grad))
                    {
                        criteria.Add("grade", grad);
                    }
                    if (!string.IsNullOrWhiteSpace(btype))
                    {
                        criteria.Add("BoardingType", btype);
                    }
                    Dictionary<long, IList<StudentDetails>> studentdetailslist = sds.GetStudentDetailsListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (studentdetailslist != null && studentdetailslist.Count > 0)
                    {
                        long totalrecords = studentdetailslist.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat1 = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in studentdetailslist.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.id_no.ToString(),items.name,items.campus_name,items.section,items.grade,Convert.ToBoolean(items.IsHosteller).ToString(),items.BoardingType,items.Id.ToString()
                            }
                                    })
                        };
                        return Json(jsondat1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (id == "Role")
                {
                    MastersService ds = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<Role>> role = ds.GetRoleMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (role != null && role.Count > 0)
                    {
                        long totalrecords = role.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat1 = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in role.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.Id.ToString(),items.RoleCode,items.RoleName,items.Description
                            }
                                    })
                        };
                        return Json(jsondat1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (id == "AssignmentName")
                {
                    MastersService ds = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<AssignmentNameMaster>> am = ds.GetAssignmentNameMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (am != null && am.Count > 0)
                    {
                        long totalrecords = am.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat1 = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in am.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.Id.ToString(),items.AssignmentName,items.AssignmentCode,items.Description
                            }
                                    })
                        };
                        return Json(jsondat1, JsonRequestBehavior.AllowGet);
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddRole(Role rl, string test)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (test == "edit")
                    {
                        MastersService rs = new MastersService();
                        ViewBag.flag = 1;
                        rl.ModifiedBy = Session["UserId"].ToString();
                        rl.ModifiedDate = DateTime.Now;
                        rs.CreateOrUpdateRoleTypeMaster(rl);
                    }
                    else
                    {
                        if (rl.RoleCode == null)
                        {
                            rl.RoleCode = "*";
                        }
                        rl.CreatedBy = Session["UserId"].ToString();
                        rl.CreatedDate = DateTime.Now;//.ToString("dd/mm/yyyy");
                        rl.ModifiedBy = Session["UserId"].ToString();
                        rl.ModifiedDate = DateTime.Now; //"01/01/1999";

                        rl.Id = 0;

                        MastersService bs = new MastersService();
                        ViewBag.flag = 1;
                        long id = bs.CreateOrUpdateRoleTypeMaster(rl);
                        rl.Id = id;

                        bs.CreateOrUpdateRoleTypeMaster(rl);
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddAssignmentName(AssignmentNameMaster am, string test)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(base.ValidateUser()))
                {
                    return RedirectToAction("LogOff", "Account");
                }
                else
                {
                    MastersService mstrSrv = new MastersService();
                    if (test == "edit")
                    {
                        am.UpdatedBy = Session["UserId"].ToString();
                        am.UpdatedDate = DateTime.Now.ToString("dd/mm/yyyy");
                    }
                    else
                    {
                        am.Id = 0;
                        am.CreatedBy = Session["UserId"].ToString();
                        am.CreatedDate = DateTime.Now.ToString();
                    }
                    ViewBag.flag = 1;
                    long amId = mstrSrv.CreateOrUpdateAssignmentNameMaster(am);
                    // adding new assignment name to session of Assignment Name master 
                    // code modifies by : Lee on 22-Jun-2013
                    // if the user working on bulk component entry, 
                    // if user added any new value then we are updating the new value to session.
                    if (amId > 0)
                    {
                        IList<AssignmentNameMaster> AsgnmtMasterLst = new List<AssignmentNameMaster>();
                        if (Session["AssignmentMasterLst"] != null)
                        {
                            AsgnmtMasterLst = (IList<AssignmentNameMaster>)Session["AssignmentMasterLst"];
                        }
                        AsgnmtMasterLst.Add(new AssignmentNameMaster() { AssignmentName = am.AssignmentName });
                        Session["AssignmentMasterLst"] = AsgnmtMasterLst;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult StudentDetails()
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult StudentDetailsListJqGrid(string AdmStatus, string idno, string name, string cname, string grade, string sect, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MasterDataService sds = new MasterDataService();
                    AdmissionManagementService ams = new AdmissionManagementService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrWhiteSpace(idno))
                    {
                        criteria.Add("NewId", idno);
                    }
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        criteria.Add("Name", name);
                    }
                    if (!string.IsNullOrWhiteSpace(cname))
                    {
                        criteria.Add("Campus", cname);
                    }
                    if (!string.IsNullOrWhiteSpace(grade))
                    {
                        criteria.Add("Grade", grade);
                    }
                    if (!string.IsNullOrWhiteSpace(sect))
                    {
                        criteria.Add("Section", sect);
                    }
                    if (!string.IsNullOrEmpty(AdmStatus) && AdmStatus == "Discontinue")
                        criteria.Add("AdmissionStatus", "Discontinue");
                    else
                        criteria.Add("AdmissionStatus", "Registered");
                    switch (sidx)
                    {
                        case "id_no":
                            {
                                sidx = "NewId";
                                break;
                            }
                        case "name":
                            {
                                sidx = "Name";
                                break;
                            }
                        case "section":
                            {
                                sidx = "Section";
                                break;
                            }
                        case "campus_name":
                            {
                                sidx = "Campus";
                                break;
                            }
                        case "grade":
                            {
                                sidx = "Grade";
                                break;
                            }
                        case "IsHosteller":
                            {
                                sidx = "IsHosteller";
                                break;
                            }
                        default: sidx = sidx.ToString();
                            break;
                    }
                    if (!string.IsNullOrWhiteSpace(sord) && sord == "desc")
                        sord = "Desc";
                    else
                        sord = "Asc";

                    Dictionary<long, IList<StudentTemplate>> studentdetailslist = ams.GetStudentDetailsListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    long totalRecords = studentdetailslist.First().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows = (
                        from items in studentdetailslist.First().Value
                        select new
                        {
                            i = items.Id,
                            cell = new string[] 
                       { 
                           items.Id.ToString(),
                           items.NewId,
                           items.Name,
                           items.Section,
                           items.Campus,
                           items.Grade,
                           items.IsHosteller.ToString(),
                           items.BoardingType,
                           items.EmailId
                    }
                        })
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public PartialViewResult CommentsGrid()
        {
            return PartialView();
        }

        public ActionResult SystemSettings()
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult UserManagement()
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public JsonResult FillIssueGroup()
        {
            try
            {
                MasterDataService mds = new MasterDataService();
                Dictionary<long, IList<IssueGroupMaster>> IssueGroupList = mds.GetIssueGroupListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, null);
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public ActionResult FillIssueType(string IssueGroup)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MasterDataService mds = new MasterDataService();
                    IList<IssueTypeMaster> IssueTypeList = mds.GetIssueTypeById(IssueGroup);
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public void SendEmail(CallManagement cm1, string LogIssuerMailId, string recepient, string IssueNumber, string InformationFor, string StudentName, string IssueGroup, string CallNumber, string RejComments, string Status, string ResolutionComments, string LeaveNotification)
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
                        IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(cm1.BranchCode, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                        mail.To.Add(recepient);
                        if (InformationFor != null && LeaveNotification == "Sick")
                        {
                            mail.Subject = "Tips support request #" + IssueNumber + " - For Sick Leave Information ";

                            string Body = "Dear Sir/Madam, <br/><br/>" +
                                        "Tips support request #" + IssueNumber + " - For Sick Leave Information <br/><br/>" +
                                        " Thanks for contacting our support desk. We are sorry to hear about " + StudentName + "’s health problem and the request for leave is sent to the concerned group and approved.<br/> " +
                                        " Wishing a speedy recovery. <br/><br/>" +
                                        " Regards, <br/>" +
                                        " TIPS Support desk";
                            mail.Body = Body;
                        }
                        else if (InformationFor != null && LeaveNotification == "Ordinary")
                        {
                            mail.Subject = "Tips support request #" + IssueNumber + " - For Leave Information ";

                            string Body = "Dear Sir/Madam, <br/><br/>" +
                                        "Tips support request #" + IssueNumber + " - For Leave Information <br/><br/>" +
                                        " Thanks for contacting our support desk. Your leave request for " + StudentName + " is sent to the concerned group and approved.<br/><br/> " +

                                        " Regards, <br/>" +
                                        " TIPS Support desk";
                            mail.Body = Body;
                        }
                        else if (InformationFor == "Parent Pickup")
                        {
                            mail.Subject = "Tips support request #" + IssueNumber + " -  For Pickup from school";

                            string Body = "Dear Sir/Madam,<br/><br/>" +
                             "Tips support request #" + IssueNumber + " For Pickup from school<br/><br/>" +
                             "Thanks for contacting our support desk. We have sent your request of your intention to pickup " + StudentName + " by yourself and you may approach the reception once you reach our school.<br/>" +
                            "The school vehicle will not drop them in the usual drop location." +
                             " Please show your pickup card at the reception and quote this request number.<br/><br/>" +
                             " Regards, <br/>" +
                             " TIPS Support desk";
                            mail.Body = Body;
                        }

                        else if (InformationFor == "Transport Pickup")
                        {
                            mail.Subject = "Tips support request #" + IssueNumber + " - For Drop/Pickup to home";

                            string Body = "Dear Sir/Madam,<br/><br/>" +
                             "Tips support request #" + IssueNumber + " For Drop or Pickup to home<br/><br/>" +
                             "Thanks for contacting our support desk. We have sent your request to drop " + StudentName + " at the requested destination. Please inform  " + StudentName + " of <br/>" +
                             "this change so can be co-ordinated easily.<br/><br/>" +
                             " Regards, <br/>" +
                             " TIPS Support desk";
                            mail.Body = Body;
                        }
                        else if (InformationFor == "General Info")
                        {
                            mail.Subject = "Tips support request #" + IssueNumber + " is raised";
                            string Body = "Dear Sir/Madam,<br/><br/>" +
                            "Tips support request #" + IssueNumber + " is raised<br/><br/>" +
                            "The mentioned request is:<br/><br/>" +
                            "<b>" + cm1.Description + "</b><br/><br/>" +
                            "Thanks for contacting our support desk. We have made note of your request and informed to the concerned department.<br/><br/>" +
                                //"this change so can be co-ordinated easily.<br/><br/>" +
                            " Regards, <br/>" +
                            " TIPS Support desk";
                            mail.Body = Body;
                        }
                        else if (Status == "Completed")
                        {
                            // mail.To.Add(LogIssuerMailId);
                            mail.Subject = "Tips support request #" + IssueNumber + " is completed";

                            string Body = "Dear Parent,<br/><br/>" +
                            "Tips support request #" + IssueNumber + " is completed<br/><br/>" +
                            "<b>Issue Description</b><br/><br/>" +
                                         cm1.Description + ".<br/><br/>" +
                            " Thank you for contacting our support desk and your issue is resolved with the below comments.<br/><br/>" +
                            "<b>Resolution Description</b><br/><br/>" +
                            ResolutionComments + ". " + "<br/><br/>" +
                            " Regards,<br/>" +
                            " TIPS Support desk";
                            mail.Body = Body;
                            if (cm1.BranchCode == "CHENNAI MAIN" || cm1.BranchCode == "CHENNAI CITY")
                                mail.Bcc.Add("b.micheal@xcdsys.com");
                            DocumentsService ds = new DocumentsService();
                            Dictionary<string, object> criteria = new Dictionary<string, object>();
                            criteria.Add("EntityRefId", cm1.Id);
                            criteria.Add("AppName", "CMS");
                            criteria.Add("DocumentType", "For Parent");
                            Dictionary<long, IList<Documents>> UploadedFiles = ds.GetDocumentsListWithPaging(0, 50, "Upload_Id", "Asc", criteria);
                            if (UploadedFiles.Values != null && UploadedFiles.FirstOrDefault().Value.Count > 0 && UploadedFiles.FirstOrDefault().Key > 0)
                            {
                                foreach (var item in UploadedFiles.FirstOrDefault().Value)
                                {
                                    MemoryStream ms = new MemoryStream(item.DocumentData);
                                    Attachment mailAttach = new Attachment(ms, item.FileName);  //Data posted from form
                                    mail.Attachments.Add(mailAttach);
                                }
                            }
                        }
                        else if (Status == "ResolveIssueRejection")
                        {
                            mail.Subject = "Tips support request #" + IssueNumber + " - Need more information";

                            string Body = "Dear Parent,<br/><br/>" +
                                "Tips support request #" + IssueNumber + " - Need more information<br/><br/>" +
                             " Thank you for contacting our support desk, please find below the information needed by the concerned staff for the issue you had raised. <br/>"
                             + RejComments + ". " + "<br/>" +
                            " In-case you may need further information please mail our support desk.<br/><br/>" +
                            " Regards <br/>" +
                            " TIPS Support desk";

                            mail.Body = Body;
                        }
                        else if (IssueGroup != "")
                        {
                            string Body = "";
                            mail.Subject = "Tips support request #" + IssueNumber + " - Logged";
                            if (cm1.IssueType == "Job related")
                            {
                                Body = "Dear Candidate,<br/><br/>";
                            }
                            else
                            {
                                Body = "Dear Parent,<br/><br/>";
                            }
                            Body = Body + "Tips support request #" + IssueNumber + " - Logged<br/><br/>" +
                                 "<b>Issue Description</b><br/><br/>" +
                                         cm1.Description + ".<br/><br/>" +
                                " Thank you for contacting our support desk, your call is registered and find the mentioned call number and please quote this for any further correspondences.<br/>" +
                                " You will be updated shortly on the action items on the same by the concerned persons. <br/>" +
                               " If you don’t receive a mail from us within 48 hours, please mail us at " + campusemaildet.First().EmailId.ToString() + " to know the status of your request.<br/><br/>" +
                               " Regards<br/>" +
                               " TIPS Support desk";
                            mail.Body = Body;

                            if (cm1.BranchCode == "CHENNAI MAIN" || cm1.BranchCode == "CHENNAI CITY")
                                mail.Bcc.Add("m.anbarasan@xcdsys.com");
                        }
                        mail.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient("localhost", 25);
                        smtp.Host = "smtp.gmail.com";
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.EnableSsl = true;
                        EmailLog el = new EmailLog();
                        el.NewId = cm1.StudentNumber;
                        el.StudName = cm1.StudentName;
                        //  el.EmailFrom = mail.From.ToString();
                        el.EmailTo = mail.To.ToString();
                        if (From == "live")
                        {
                            try
                            {
                                mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                                el.EmailFrom = mail.From.ToString();
                                smtp.Credentials = new System.Net.NetworkCredential
                              (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                                if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                {
                                    smtp.Send(mail);
                                    el.IsSent = true;
                                    emaillogloop(mail.From.ToString(), mail.To.ToString(), mail, el);
                                }
                                else
                                {
                                    el.IsSent = false;
                                    emaillogloop(mail.From.ToString(), mail.To.ToString(), mail, el);
                                }
                            }

                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("quota"))
                                {
                                    mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                                    el.EmailFrom = campusemaildet.First().AlternateEmailId.ToString();
                                    smtp.Credentials = new System.Net.NetworkCredential
                                    (campusemaildet.First().AlternateEmailId.ToString(), campusemaildet.First().AlternateEmailIdPassword.ToString());
                                    if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                    {
                                        smtp.Send(mail);
                                        el.IsSent = true;
                                        emaillogloop(mail.From.ToString(), mail.To.ToString(), mail, el);
                                    }
                                    else
                                    {
                                        el.IsSent = false;
                                        emaillogloop(mail.From.ToString(), mail.To.ToString(), mail, el);
                                    }
                                }
                                else
                                {
                                    mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                                    el.EmailFrom = campusemaildet.First().AlternateEmailId.ToString();
                                    smtp.Credentials = new System.Net.NetworkCredential
                                    (campusemaildet.First().AlternateEmailId.ToString(), campusemaildet.First().AlternateEmailIdPassword.ToString());
                                    if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                    {
                                        smtp.Send(mail);
                                        el.IsSent = true;
                                        emaillogloop(mail.From.ToString(), mail.To.ToString(), mail, el);
                                    }
                                    else
                                    {
                                        el.IsSent = false;
                                        emaillogloop(mail.From.ToString(), mail.To.ToString(), mail, el);
                                    }
                                }
                            }
                        }
                        else if (From == "test")
                        {
                            mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                            smtp.Credentials = new System.Net.NetworkCredential
                           (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                            if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                            {
                                smtp.Send(mail);
                                mail.To.Clear();
                            }
                        }

                    }
                    catch (Exception)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public void ActorsEmail(CallManagement cm1, string LogIssuerMailId, string[] ResolverEmail, string recepient, string IssueNumber, string InformationFor, string StudentName, string IssueGroup, string CallNumber, string RejComments, string Status, string[] ApproverEmail, string ResolutionComments, string LeaveNotification)
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
                        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();

                        if (Status == "ResolveIssue" || Status == "LeaveNotification-Dayscholar" || Status == "LeaveNotification-Hostel" || Status == "TransportPickup" || Status == "ParentPickup" || Status == "GeneralInfo")
                        {
                            for (int i = 0; i < ResolverEmail.Length; i++)
                            {
                                if (ResolverEmail[i] != "")
                                    mail.To.Add(ResolverEmail[i]);
                            }
                            mail.Subject = "Tips support request #" + IssueNumber + " needs your resolve ";
                            string Body = "Dear Sir/Madam, <br/><br/>" +
                                        "Tips support request #" + IssueNumber + " needs your resolve <br/><br/>" +
                                        "<b>Issue Description</b><br/><br/>" +
                                         cm1.Description + ".<br/><br/>" +
                                        "The issue " + IssueNumber + "  is assigned for your closing and please resolve this issue asap.<br/><br/>" +
                                        "Regards<br/>" +
                                        "TIPS Support desk";
                            mail.Body = Body;
                        }
                        if (Status == "ResolveIssueRejection")
                        {
                            for (int i = 0; i < ResolverEmail.Length; i++)
                            {
                                if (ResolverEmail[i] != "")
                                    mail.To.Add(ResolverEmail[i]);
                            }
                            mail.Subject = "Tips support request #" + IssueNumber + " needs your resolve ";
                            string Body = "Dear Sir/Madam, <br/><br/>" +
                                        "Tips support request #" + IssueNumber + " needs your resolve <br/><br/>" +
                                        "<b>Issue Description</b><br/><br/>" +
                                         cm1.Description + ".<br/><br/>" +
                                        "The issue " + IssueNumber + "  is reassigned for your closing and please resolve this issue asap.<br/><br/>" +
                                        "Regards<br/>" +
                                        "TIPS Support desk";
                            mail.Body = Body;
                        }

                        if (Status == "ApproveIssue")
                        {
                            for (int i = 0; i < ApproverEmail.Length; i++)
                            {
                                if (ApproverEmail[i] != "")
                                    mail.To.Add(ApproverEmail[i]);
                            }
                            mail.Subject = "Tips support request #" + IssueNumber + " needs your approve ";
                            string Body = "Dear Sir/Madam, <br/><br/>" +
                                          "Tips support request #" + IssueNumber + " needs your approve <br/><br/>" +
                                          "<b>Issue Description</b><br/><br/>" +
                                         cm1.Description + ".<br/><br/>" +
                                          "<b>Resolution Comments</b><br/><br/>" +
                                         cm1.Resolution + "<br/><br/>" +
                                        "The issue " + IssueNumber + " is sent for your approval, please approve this issue.<br/><br/>" +
                                        "Regards<br/>" +
                                        "TIPS Support desk";
                            mail.Body = Body;
                            if (cm1.BranchCode == "CHENNAI MAIN" || cm1.BranchCode == "CHENNAI CITY")
                                mail.Bcc.Add("b.micheal@xcdsys.com");
                        }
                        if (Status == "ApproveIssueRejection")
                        {
                            for (int i = 0; i < ResolverEmail.Length; i++)
                            {
                                if (ResolverEmail[i] != "")
                                    mail.To.Add(ResolverEmail[i]);
                            }
                            mail.Subject = "Tips support request #" + IssueNumber + " needs your action ";
                            string Body = "Dear Sir/Madam, <br/><br/>" +
                                          "Tips support request #" + IssueNumber + " needs your action <br/><br/>" +
                                            "The issue " + IssueNumber + " is reassigned to you for more information, please provide the needed information.<br/><br/>" +
                                            "Regards<br/>" +
                                            "TIPS Support desk";

                            mail.Body = Body;
                        }
                        if (Status == "Complete")
                        {
                            mail.To.Add(LogIssuerMailId);
                            mail.Subject = "Tips support request #" + IssueNumber + " is approved ";
                            string Body = "Dear Sir/Madam, <br/><br/>" +
                                          "Tips support request #" + IssueNumber + " is approved. <br/><br/>" +
                                          "<b>Issue Description</b><br/><br/>" +
                                         cm1.Description + ".<br/><br/>" +
                                          "<b>Resolution Comments</b><br/><br/>" +
                                         cm1.Resolution + "<br/><br/>" +
                                           "Tips support request #" + IssueNumber + " needs your action <br/>" +
                                           "Please complete the issue asap. <br/> <br/>" +
                                            "Regards<br/>" +
                                            "TIPS Support desk";
                            mail.Body = Body;
                            if (cm1.BranchCode == "CHENNAI MAIN" || cm1.BranchCode == "CHENNAI CITY")
                                mail.Bcc.Add("m.anbarasan@xcdsys.com");
                        }
                        if (Status == "ChangeofIssueGroup")
                        {
                            for (int i = 0; i < ResolverEmail.Length; i++)
                            {
                                if (ResolverEmail[i] != "")
                                    mail.To.Add(ResolverEmail[i]);
                            }
                            mail.Subject = "Tips support request #" + IssueNumber + " needs your resolve ";
                            string Body = "Dear Sir/Madam, <br/><br/>" +
                                        "Tips support request #" + IssueNumber + " is forwarded to you from " + IssueGroup + ".Please Look into this issue asap" + "<br/><br/>" +
                                        "<b>Issue Description</b><br/><br/>" +
                                         cm1.Description + ".<br/><br/>" +
                                        "The issue " + IssueNumber + "  is assigned for your closing and please resolve this issue asap.<br/><br/>" +
                                        "Regards<br/>" +
                                        "TIPS Support desk";
                            mail.Body = Body;
                        }
                        mail.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient("localhost", 25);
                        smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address 
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.EnableSsl = true;
                        BaseController bc = new BaseController();
                        IList<CampusEmailId> campusemaildet = bc.GetEmailIdByCampus(cm1.BranchCode, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                        if (From == "live")
                        {
                            try
                            {
                                mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                                smtp.Credentials = new System.Net.NetworkCredential
                               (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                                if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                {
                                    smtp.Send(mail);
                                }
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("quota"))
                                {
                                    mail.From = new MailAddress(campusemaildet.First().AlternateEmailId);

                                    smtp.Credentials = new System.Net.NetworkCredential
                                    (campusemaildet.First().AlternateEmailId, campusemaildet.First().AlternateEmailIdPassword);
                                    if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                    {
                                        smtp.Send(mail);
                                    }
                                }
                                else
                                {
                                    mail.From = new MailAddress(campusemaildet.First().AlternateEmailId);

                                    smtp.Credentials = new System.Net.NetworkCredential
                                    (campusemaildet.First().AlternateEmailId, campusemaildet.First().AlternateEmailIdPassword);
                                    if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                    {
                                        smtp.Send(mail);
                                    }
                                }
                            }
                        }
                        else if (From == "test")
                        {
                            mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                            smtp.Credentials = new System.Net.NetworkCredential
                           (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                            if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                            {
                                smtp.Send(mail);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public PartialViewResult AssignSection()
        {
            return PartialView();
        }

        public PartialViewResult CancelSectionAllocation()
        {
            return PartialView();
        }

        public PartialViewResult Promote()
        {
            return PartialView();
        }

        public PartialViewResult Transfer()
        {
            return PartialView();
        }

        public PartialViewResult PrintIdCard()
        {
            return PartialView();
        }

        public PartialViewResult PrintPickupCard()
        {
            return PartialView();
        }

        public ActionResult ViewWithoutMasterPage()
        {
            return View();
        }

        public PartialViewResult FileUpload()
        {
            return PartialView();
        }

        public ActionResult StatusReport()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;

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
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("TemplateId", (long)1);
                    Dictionary<long, IList<WorkFlowStatus>> workflowstatus = ps.GetWorkFlowStatusListWithsearchCriteria(0, 1000, string.Empty, string.Empty, criteria);
                    var workflowsta = (
                             from items in workflowstatus.First().Value
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
                ExceptionPolicy.HandleException(ex, "StaffMgmntPolicy");
                throw ex;
            }
        }


        //public JsonResult Statusjqgrid(string id, string txtSearch, string ddlcampus, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        ProcessFlowServices pfs = new ProcessFlowServices();
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        if ((string.IsNullOrEmpty(txtSearch))
        //           && (string.IsNullOrEmpty(ddlcampus)))
        //        {
        //            return Json(null, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            if (!string.IsNullOrWhiteSpace(txtSearch))
        //            {
        //                if (txtSearch.Contains("Select"))
        //                {
        //                }
        //                else
        //                {
        //                    criteria.Add("Status", txtSearch == "Complete" ? "Completed" : txtSearch);
        //                }
        //            }
        //            if (!string.IsNullOrWhiteSpace(ddlcampus))
        //            {
        //                criteria.Add("School", ddlcampus);
        //            }
        //            else
        //            {
        //                string userId = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
        //                UserService us = new UserService();
        //                Dictionary<string, object> criteriaUserAppRole = new Dictionary<string, object>();
        //                criteriaUserAppRole.Add("UserId", userId);
        //                Dictionary<long, IList<UserAppRole>> userAppRole = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteriaUserAppRole);
        //                int count = 0;int i = 0;
        //                if (userAppRole != null && userAppRole.Count > 0 && userAppRole.First().Key > 0)
        //                {
        //                    count = userAppRole.First().Value.Count;
        //                    string[] brnCodeArr = new string[count];
        //                    foreach (UserAppRole uar in userAppRole.First().Value)
        //                    {
        //                        if (!string.IsNullOrEmpty(uar.BranchCode))
        //                            brnCodeArr[i] = uar.BranchCode;
        //                        i++;
        //                    }
        //                    brnCodeArr = brnCodeArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        //                    criteria.Add("School", brnCodeArr);
        //                }
        //            }
        //            sord = sord == "desc" ? "Desc" : "Asc";
        //            Dictionary<long, IList<CallMgmntPIView>> processinstance = pfs.GetProcessInstanceViewListWithsearchCriteria(page - 1, rows, sidx, sord, criteria);
        //            if (processinstance != null && processinstance.First().Value != null)
        //            {
        //                foreach (CallMgmntPIView pi in processinstance.First().Value)
        //                {
        //                    pi.ProcessInstance.DifferenceInHours = DateTime.Now - pi.ProcessInstance.DateCreated;
        //                }
        //            }
        //            string[] datecreated = new string[100];
        //            var date = (from items in processinstance.First().Value select items.ProcessInstance.DateCreated);
        //            if (processinstance != null && processinstance.Count > 0)
        //            {
        //                long totalrecords = processinstance.First().Key;
        //                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
        //                var jsondat = new
        //                {
        //                    total = totalPages,
        //                    page = page,
        //                    records = totalrecords,

        //                    rows = (from items in processinstance.First().Value
        //                            select new
        //                            {
        //                                i = 2,
        //                                cell = new string[] {
        //                       items.Id.ToString(),
        //                       "<a href='/Home/ShowIssueDetails?id=" + items.Id+"'>" + items.IssueNumber + "</a>",
        //                       items.School,
        //                       items.StudentName,
        //                       items.Grade,
        //                       items.ProcessInstance.DateCreated.Value.ToString("dd/MM/yyyy hh:mm:ss tt"),
        //                       items.InformationFor,
        //                       items.LeaveType,
        //                       items.IssueGroup,
        //                       items.IssueType,
        //                       items.ProcessInstance.CreatedBy,
        //                       items.Status,
        //                        "<img src='/Images/History.png ' id='ImgHistory' onclick=\"ShowComments('" + items.Id + "');\" />",
        //                       items.Status=="Completed"? "Completed": items.ProcessInstance.DifferenceInHours.Value.TotalHours.ToString(),
        //                    }
        //                            })
        //                };
        //                return Json(jsondat, JsonRequestBehavior.AllowGet);
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
        //        throw ex;
        //    }
        //}

        public JsonResult Statusjqgrid(string id, string txtSearch, string ddlcampus, string Name, string ddlgrade, string SupNum, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                ProcessFlowServices pfs = new ProcessFlowServices();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likecriteria = new Dictionary<string, object>();
                if ((string.IsNullOrEmpty(txtSearch)) && (string.IsNullOrEmpty(ddlcampus))) { return Json(null, JsonRequestBehavior.AllowGet); }
                else
                {
                    if (!string.IsNullOrWhiteSpace(txtSearch))
                    {
                        if (txtSearch.Contains("Select")) { }
                        else { criteria.Add("Status", txtSearch == "Complete" ? "Completed" : txtSearch); }
                    }
                    if (!string.IsNullOrWhiteSpace(ddlcampus)) { criteria.Add("School", ddlcampus); }
                    if (!string.IsNullOrWhiteSpace(Name)) { likecriteria.Add("StudentName", Name); }
                    if (!string.IsNullOrWhiteSpace(ddlgrade)) { criteria.Add("Grade", ddlgrade); }
                    if (!string.IsNullOrWhiteSpace(SupNum)) { criteria.Add("IssueNumber", SupNum); }
                    else if (ddlcampus == null || ddlcampus == "")
                    {
                        string userId = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                        UserService us = new UserService();
                        Dictionary<string, object> criteriaUserAppRole = new Dictionary<string, object>();
                        criteriaUserAppRole.Add("UserId", userId);
                        Dictionary<long, IList<UserAppRole>> userAppRole = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteriaUserAppRole);
                        int count = 0;
                        int i = 0;
                        if (userAppRole != null && userAppRole.Count > 0 && userAppRole.First().Key > 0)
                        {
                            count = userAppRole.First().Value.Count;
                            string[] brnCodeArr = new string[count];
                            foreach (UserAppRole uar in userAppRole.First().Value)
                            {
                                string branchCode = uar.BranchCode;

                                if (!string.IsNullOrEmpty(branchCode))
                                {
                                    brnCodeArr[i] = branchCode;
                                }
                                i++;
                            }
                            brnCodeArr = brnCodeArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                            criteria.Add("School", brnCodeArr);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(sord) && sord == "desc")
                        sord = "Desc";
                    else
                        sord = "Asc";

                    Dictionary<long, IList<CallMgmntPIView>> processinstance = pfs.GetProcessInstanceViewListWithExactandLikesearchCriteria(page - 1, rows, sidx, sord, criteria,likecriteria);
                    if (processinstance != null && processinstance.First().Value != null)
                    {
                        foreach (CallMgmntPIView pi in processinstance.First().Value)
                        {
                            pi.ProcessInstance.DifferenceInHours = DateTime.Now - pi.ProcessInstance.DateCreated;
                        }
                    }
                    string[] datecreated = new string[100];
                    var date = (from items in processinstance.First().Value select items.ProcessInstance.DateCreated);

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
                               "<a href='/Home/ShowIssueDetails?id=" + items.Id+"'>" + items.IssueNumber + "</a>",
                               items.School,
                               items.StudentName,
                               items.Grade,
                               items.ProcessInstance.DateCreated.Value.ToString("dd/MM/yyyy hh:mm:ss tt"),
                               items.InformationFor,
                               items.LeaveType,
                               items.IssueGroup,
                               items.IssueType,
                               items.ProcessInstance.CreatedBy,
                               items.Status,
                                "<img src='/Images/History.png ' id='ImgHistory' onclick=\"ShowComments('" + items.Id + "');\" />",
                               items.Status=="Completed"? "Completed": items.ProcessInstance.DifferenceInHours.Value.TotalHours.ToString(),
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
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult UserAppRole()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    UserService us = new UserService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<Application>> appcode = us.GetApplicationListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    Dictionary<long, IList<Role>> rolecode = us.GetRoleListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    Dictionary<long, IList<Department>> deptcode = us.GetDepartmentListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                    if (usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
                    {
                        criteria.Add("Name", usrcmp);
                    }
                    MastersService ms = new MastersService();
                    criteria.Clear();

                    ViewBag.appcodeddl = appcode.First().Value;
                    ViewBag.rolecodeddl = rolecode.First().Value;
                    ViewBag.deptcodeddl = deptcode.First().Value;

                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddUserAppRole(UserAppRole apm, string test)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    UserService aps = new UserService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("UserId", apm.UserId);
                    criteria.Add("AppCode", apm.AppCode);
                    criteria.Add("RoleCode", apm.RoleCode);
                    criteria.Add("DeptCode", apm.DeptCode);
                    criteria.Add("BranchCode", apm.BranchCode);
                    Dictionary<long, IList<UserAppRole>> userapprole = aps.GetAppRoleForAnUserListWithPagingAndCriteria(null, null, null, null, criteria);
                    if (test == "edit")
                    {
                        if (userapprole != null && userapprole.First().Value != null && userapprole.First().Value.Count > 1)
                        {
                            return null;
                        }
                        else
                        {
                            if (apm.RoleCode == "CSE")
                            {
                                apm.DeptCode = null;
                            }
                            ViewBag.flag = 1;
                            aps.CreateOrUpdateUserAppRole(apm);
                            return null;
                        }
                    }
                    else
                    {
                        if (userapprole != null && userapprole.First().Value != null && userapprole.First().Value.Count > 0)
                        {
                            var script1 = @"ErrMsg(""This Combination already exists"");";
                            return JavaScript(script1);
                        }
                        else
                        {
                            if (apm.RoleCode == "CSE")
                            {
                                apm.DeptCode = null;
                            }
                            aps.CreateOrUpdateUserAppRole(apm);
                            var script = @"SucessMsg(""Role mapped Sucessfully"");";
                            return JavaScript(script);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }


        public ActionResult AppCodeddl()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    UserService aps = new UserService();
                    Dictionary<long, string> appcd = new Dictionary<long, string>();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<Application>> application = aps.GetApplicationListWithPagingAndCriteria(0, 9999, null, null, criteria);
                    foreach (Application app in application.First().Value)
                    {
                        appcd.Add(app.Id, app.AppCode);
                    }
                    return PartialView("Select", appcd);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult RoleCodeddl()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    UserService rcs = new UserService();
                    Dictionary<long, string> rlcd = new Dictionary<long, string>();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<Role>> role = rcs.GetRoleListWithPagingAndCriteria(0, 9999, null, null, criteria);
                    foreach (Role rol in role.First().Value)
                    {
                        rlcd.Add(rol.Id, rol.RoleCode);
                    }
                    return PartialView("Select", rlcd);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult DeptCodeddl()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    UserService dcs = new UserService();
                    Dictionary<long, string> dptcd = new Dictionary<long, string>();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<Department>> department = dcs.GetDepartmentListWithPagingAndCriteria(0, 9999, null, null, criteria);
                    foreach (Department dept in department.First().Value)
                    {
                        dptcd.Add(dept.Id, dept.DeptCode);
                    }
                    return PartialView("Select", dptcd);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult BranchCodeddl()
        {
            try
            {
                //UserService bcs = new UserService();
                MastersService ms = new MastersService();
                Dictionary<long, string> brncd = new Dictionary<long, string>();
                Dictionary<string, object> criteria = new Dictionary<string, object>();

                // Dictionary<long, IList<Branch>> branch = bcs.GetBranchListWithPagingAndCriteria(null, null, null, null, criteria);
                Dictionary<long, IList<CampusMaster>> branch = ms.GetCampusMasterListWithPagingAndCriteria(0, 9999, null, null, criteria);
                criteria.Clear();
                foreach (CampusMaster brnch in branch.First().Value)
                {
                    brncd.Add(brnch.FormId, brnch.Name);
                }
                return PartialView("Select", brncd);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult Campusddl()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<long, string> campus = new Dictionary<long, string>();
                    campus.Add(0, "IB MAIN");
                    campus.Add(1, "IB KG");
                    return PartialView("Select", campus);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult Sectionddl()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<long, string> section = new Dictionary<long, string>();
                    section.Add(0, "A");
                    section.Add(1, "B");
                    section.Add(2, "C");
                    section.Add(3, "D");
                    section.Add(4, "E");
                    section.Add(5, "F");
                    return PartialView("Select", section);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult Gradeddl()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<long, string> grade = new Dictionary<long, string>();

                    grade.Add(0, "I");
                    grade.Add(1, "II");
                    grade.Add(2, "III");
                    grade.Add(3, "IV");
                    grade.Add(4, "V");
                    grade.Add(5, "VI");
                    grade.Add(6, "VII");
                    grade.Add(7, "VIII");
                    grade.Add(8, "IX");

                    return PartialView("Select", grade);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult Hostellerddl()
        {
            Dictionary<long, string> hosteller = new Dictionary<long, string>();
            hosteller.Add(0, "No");
            hosteller.Add(1, "Yes");
            return PartialView("Select", hosteller);
        }

        public ActionResult BoardingTypeddl()
        {
            Dictionary<long, string> btype = new Dictionary<long, string>();
            btype.Add(0, "Day Scholar");
            btype.Add(1, "Week Boarder");
            btype.Add(2, "Residential");

            return PartialView("Select", btype);
        }
        public ActionResult FusionChartsTest()
        {
            return View();
        }

        public ActionResult GetIssueCountByStatus(string cam)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    CallManagementService cms = new CallManagementService();
                    Dictionary<string, object> IssCount = new Dictionary<string, object>();
                    IssCount.Add("BranchCode", cam);
                    Dictionary<long, IList<CallManagementDashboard>> GetIssueCountByStatus = cms.GetCallManagementListWithPagingDashboard(null, 9999, string.Empty, string.Empty, IssCount);
                    var IssueCount = (from u in GetIssueCountByStatus.First().Value
                                      where u.IsInformation == false
                                      select u).ToList();
                    return Json(IssueCount, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
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
                    CallManagementService cms = new CallManagementService();
                    Dictionary<string, object> IssCount = new Dictionary<string, object>();
                    IssCount.Add("BranchCode", cam);
                    Dictionary<long, IList<CallManagementDashboard>> GetIssueCountByIssueGroup = cms.GetCallManagementListWithPagingDashboard(null, 9999, string.Empty, string.Empty, IssCount);
                    var IssueCount = (from u in GetIssueCountByIssueGroup.First().Value
                                      select u).ToList();
                    return Json(IssueCount, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult GetIssueCountByIssueGroupNew(string cam)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    CallManagementService cms = new CallManagementService();
                    Dictionary<string, object> IssCount = new Dictionary<string, object>();
                    IssCount.Add("BranchCode", cam);
                    Dictionary<long, IList<CallManagementDashboard>> GetIssueCountByIssueGroup = cms.GetCallManagementListWithPagingDashboard(null, 9999, string.Empty, string.Empty, IssCount);
                    var IssueCount = (from u in GetIssueCountByIssueGroup.First().Value
                                      select u).ToList();
                    var aca = 0;
                    var admn = 0;
                    var adms = 0;
                    var fees = 0;
                    var hstl = 0;
                    var hr = 0;
                    var stre = 0;
                    var trns = 0;
                    var it = 0;
                    var rcpt = 0;
                    foreach (var itemdata in IssueCount)
                    {
                        if (itemdata.IssueGroup == "Academics")
                        {
                            aca++;
                        }
                        else if (itemdata.IssueGroup == "Administrative")
                        {
                            admn++;
                        }
                        else if (itemdata.IssueGroup == "Admission")
                        {
                            adms++;
                        }
                        else if (itemdata.IssueGroup == "Fees / Finance")
                        {
                            fees++;
                        }
                        else if (itemdata.IssueGroup == "Hostel")
                        {
                            hstl++;
                        }
                        else if (itemdata.IssueGroup == "HR")
                        {
                            hr++;
                        }
                        else if (itemdata.IssueGroup == "Store")
                        {
                            stre++;
                        }
                        else if (itemdata.IssueGroup == "Transport")
                        {
                            trns++;
                        }
                        else if (itemdata.IssueGroup == "IT")
                        {
                            it++;
                        }
                        else if (itemdata.IssueGroup == "Reception")
                        {
                            rcpt++;
                        }
                        else
                        { }
                    }
                    var FunnelChart = "<graph caption='' xAxisName='Issue Group' yAxisName='Issue Count' decimalPrecision='0' formatNumberScale='0' showNames='1' rotateNames='1'>";
                    FunnelChart = FunnelChart + " <set name='Academics' value='" + aca + "' color='AFD8F8' link='/Home/IssueList?IssueGroup=Academics&amp;cam=" + cam + "' />";
                    FunnelChart = FunnelChart + " <set name='Administrative' value='" + admn + "' color='F6BD0F' link='/Home/IssueList?IssueGroup=Administrative&amp;cam=" + cam + "'/>";
                    FunnelChart = FunnelChart + " <set name='Admission' value='" + adms + "' color='8BBA00' link='/Home/IssueList?IssueGroup=Admission&amp;cam=" + cam + "'/>";
                    FunnelChart = FunnelChart + " <set name='Fees / Finance' value='" + fees + "' color='FF8E46' link='/Home/IssueList?IssueGroup=Fees / Finance&amp;cam=" + cam + "'/>";
                    FunnelChart = FunnelChart + " <set name='Hostel' value='" + hstl + "' color='08E8E' link='/Home/IssueList?IssueGroup=Hostel&amp;cam=" + cam + "' />";
                    FunnelChart = FunnelChart + " <set name='HR' value='" + hr + "' color='D64646' link='/Home/IssueList?IssueGroup=HR&amp;cam=" + cam + "' />";
                    FunnelChart = FunnelChart + " <set name='Store' value='" + stre + "' color='8BBA00' link='/Home/IssueList?IssueGroup=Store&amp;cam=" + cam + "'/>";
                    FunnelChart = FunnelChart + " <set name='Transport' value='" + trns + "' color='FF8E46' link='/Home/IssueList?IssueGroup=Transport&amp;cam=" + cam + "'/>";
                    FunnelChart = FunnelChart + " <set name='IT' value='" + it + "' color='08E8E' link='/Home/IssueList?IssueGroup=IT&amp;cam=" + cam + "'/>";
                    FunnelChart = FunnelChart + " <set name='Reception' value='" + rcpt + "' color='D64646' link='/Home/IssueList?IssueGroup=Reception&amp;cam=" + cam + "' /></graph>";
                    Response.Write(FunnelChart);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult ShowIssueDetails(long id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    CallManagementService cms = new CallManagementService(); // TODO: Initialize to an appropriate value
                    ProcessFlowServices pfs = new ProcessFlowServices();
                    CallManagement cm = cms.GetCallManagementById(Convert.ToInt64(id));

                    return View(cm);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }


        public ActionResult IssueCreation()
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult BulkInfoCompleteNew()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    string Template = "CallManagement";
                    CallManagementService cms = new CallManagementService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    // criteria.Add("Performer", userId);
                    ProcessFlowServices pfs = new ProcessFlowServices();
                    Dictionary<long, IList<BulkCompleteInfo>> BulkInfoList = cms.GetInformationListWithsearchCriteria(null, 9999, string.Empty, string.Empty, criteria);

                    long[] ActivityIds = (from b in BulkInfoList.First().Value
                                          select b.Id).ToArray();

                    bool BulkInfoComplete = pfs.BulkInfoCompleteActivityCallManagement(ActivityIds, Template, userId);
                    return Json(BulkInfoComplete, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }

        }
        public ActionResult BulkComplete(long[] ActivityId, string Template, string userId, string[] recepient, long[] IssueId, string[] ResolutionComments, string[] BranchCode)
        {
            try
            {
                userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Template = "CallManagement";
                    ProcessFlowServices pfs = new ProcessFlowServices();
                    bool BulkComplete = pfs.BulkCompleteActivityCallManagement(ActivityId, Template, userId);

                    string SendEmail = ConfigurationManager.AppSettings["SendEmail1"];
                    string From = ConfigurationManager.AppSettings["From"];

                    if (SendEmail == "false")
                    {
                        return null;
                    }
                    else
                    {
                        try
                        {
                            for (int i = 0; i < recepient.Length; i++)
                            {
                                IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(BranchCode[i], ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                                mail.To.Add(recepient[i]);
                                string IssueNumber = string.Empty;
                                string cur = (DateTime.Now.Year).ToString().Substring(2);
                                string nxt = (DateTime.Now.Year + 1).ToString().Substring(2);
                                if (DateTime.Now.Month > 5)
                                {
                                    IssueNumber = "CMGT-" + cur + "-" + nxt + "-" + IssueId[i].ToString();
                                }
                                else
                                {
                                    IssueNumber = "CMGT-" + (Convert.ToInt64(cur) - 1).ToString() + "-" + cur + "-" + IssueId[i].ToString();
                                }
                                mail.Subject = "Tips support request " + IssueNumber + " is completed";
                                string Body = "Dear Parent,<br/><br/>" +
                                "Tips support request " + IssueNumber + " is completed<br/><br/>" +
                                " Thank you for contacting our support desk and your issue is resolved with the below comments.<br/><br/>" +
                                ResolutionComments[i] + ". " + "<br/><br/>" +
                                " Regards,<br/>" +
                                " TIPS Support desk";
                                mail.Body = Body;
                                if (BranchCode[i] == "CHENNAI MAIN" || BranchCode[i] == "CHENNAI CITY")
                                    mail.Bcc.Add("askchennaicity@tipsc.info");
                                DocumentsService ds = new DocumentsService();
                                Dictionary<string, object> criteria = new Dictionary<string, object>();
                                criteria.Add("EntityRefId", IssueId[i]);
                                criteria.Add("AppName", "CMS");
                                criteria.Add("DocumentType", "For Parent");
                                Dictionary<long, IList<Documents>> UploadedFiles = ds.GetDocumentsListWithPaging(0, 50, "Upload_Id", "Asc", criteria);
                                if (UploadedFiles.Values != null && UploadedFiles.FirstOrDefault().Value.Count > 0 && UploadedFiles.FirstOrDefault().Key > 0)
                                {
                                    foreach (var item in UploadedFiles.FirstOrDefault().Value)
                                    {
                                        MemoryStream ms = new MemoryStream(item.DocumentData);
                                        Attachment mailAttach = new Attachment(ms, item.FileName);  //Data posted from form
                                        mail.Attachments.Add(mailAttach);
                                    }
                                }
                                mail.IsBodyHtml = true;
                                SmtpClient smtp = new SmtpClient("localhost", 25);
                                smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address  
                                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                                smtp.EnableSsl = true;
                                if (From == "live")
                                {
                                    try
                                    {
                                        mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                                        smtp.Credentials = new System.Net.NetworkCredential
                                      (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                                        if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                        {
                                            smtp.Send(mail);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        if (ex.Message.Contains("quota"))
                                        {
                                            mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                                            smtp.Credentials = new System.Net.NetworkCredential
                                            (campusemaildet.First().AlternateEmailId.ToString(), campusemaildet.First().AlternateEmailIdPassword.ToString());
                                            if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                            {
                                                smtp.Send(mail);
                                            }
                                        }
                                        else
                                        {
                                            mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                                            smtp.Credentials = new System.Net.NetworkCredential
                                            (campusemaildet.First().AlternateEmailId.ToString(), campusemaildet.First().AlternateEmailIdPassword.ToString());
                                            if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                            {
                                                smtp.Send(mail);
                                            }
                                        }
                                    }
                                }
                                else if (From == "test")
                                {
                                    mail.From = new MailAddress("tipscmsupp0rthyderabad247@gmail.com");
                                    smtp.Credentials = new System.Net.NetworkCredential
                                    ("tipscmsupp0rthyderabad247@gmail.com", "Supp0rt24by7");
                                    if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                    {
                                        smtp.Send(mail);
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                    return Json(BulkComplete, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult ApproverSendEmail(string To, string Sub, string IssDesc, string ResComments, string Campus)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    char[] delimiterChars = { ',', ';', ' ' };
                    string[] ToArr = To.Split(delimiterChars);
                    bool mailsent;
                    //string[] CCArr = CC.Split(delimiterChars);
                    try
                    {
                        BaseController bc = new BaseController();
                        IList<CampusEmailId> campusemaildet = bc.GetEmailIdByCampus(Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                        string SendEmail = ConfigurationManager.AppSettings["SendEmail1"];
                        string From = ConfigurationManager.AppSettings["From"];
                        if (SendEmail == "false")
                        {
                            return null;
                        }
                        else
                        {
                            try
                            {
                                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                                for (int i = 0; i < ToArr.Length; i++)
                                {
                                    if (!string.IsNullOrEmpty(ToArr[i]))
                                        mail.To.Add(ToArr[i]);
                                }
                                mail.Subject = Sub;

                                string Body = "Dear Sir/Madam, <br/><br/>" +
                                Sub + "<br/><br/>" +
                                "<b>Issue Description</b><br/><br/>" +
                                IssDesc + "<br/><br/>" +
                                "<b>Resolution Comments</b><br/><br/>" +
                                ResComments + "<br/><br/>" +

                                " Regards,<br/>" +
                                " TIPS Support desk";

                                mail.Body = Body;
                                mail.IsBodyHtml = true;
                                SmtpClient smtp = new SmtpClient("localhost", 25);
                                smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
                                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                                smtp.EnableSsl = true;
                                if (From == "live")
                                {
                                    try
                                    {
                                        mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                                        smtp.Credentials = new System.Net.NetworkCredential
                                        (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                                        if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                        {
                                            smtp.Send(mail);
                                        }
                                        mailsent = true;
                                        return Json(mailsent, JsonRequestBehavior.AllowGet);
                                    }
                                    catch (Exception ex)
                                    {
                                        if (ex.Message.Contains("quota"))
                                        {
                                            mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                                            smtp.Credentials = new System.Net.NetworkCredential
                                            (campusemaildet.First().AlternateEmailId.ToString(), campusemaildet.First().AlternateEmailIdPassword.ToString());
                                            if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                            {
                                                smtp.Send(mail);
                                            }
                                        }
                                        else
                                        {
                                            mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                                            smtp.Credentials = new System.Net.NetworkCredential
                                            (campusemaildet.First().AlternateEmailId.ToString(), campusemaildet.First().AlternateEmailIdPassword.ToString());
                                            if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                            {
                                                smtp.Send(mail);
                                            }
                                        }
                                    }
                                }
                                else if (From == "test")
                                {
                                    mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                                    smtp.Credentials = new System.Net.NetworkCredential
                                    (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                                    if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                    {
                                        smtp.Send(mail);
                                    }
                                    mailsent = true;
                                    return Json(mailsent, JsonRequestBehavior.AllowGet);
                                }
                            }

                            catch (Exception)
                            {
                                return null;
                            }
                        }
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                        throw ex;
                    }
                }
            }

            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
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
                    CallManagementService cms = new CallManagementService();
                    Dictionary<string, object> IssCount = new Dictionary<string, object>();
                    IssCount.Add("BranchCode", cam);
                    Dictionary<long, IList<CallManagementDashboard>> CallManagementList = cms.GetCallManagementListWithPagingDashboard(null, 9999, string.Empty, string.Empty, IssCount);

                    if (CallManagementList != null && CallManagementList.Count > 0)
                    {
                        var IssueList = (from u in CallManagementList.First().Value
                                         where u.Status != "Completed" && u.IssueGroup != null && u.IssueGroup != ""
                                         select u).ToList();
                        IList<CallManagementDashboard> diffinhours1 = new List<CallManagementDashboard>();
                        foreach (CallManagementDashboard cm in IssueList)
                        {
                            cm.DifferenceInHours = DateTime.Now - cm.IssueDate;
                            cm.Hours = Convert.ToInt32(cm.DifferenceInHours.Value.TotalHours);
                            diffinhours1.Add(cm);
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
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
                    CallManagementService cms = new CallManagementService();
                    Dictionary<string, object> IssCount = new Dictionary<string, object>();
                    IssCount.Add("BranchCode", cam);
                    Dictionary<long, IList<CallManagementDashboard>> CallManagementList = cms.GetCallManagementListWithPagingDashboard(null, 9999, string.Empty, string.Empty, IssCount);

                    if (CallManagementList != null && CallManagementList.Count > 0)
                    {
                        var IssueList = (from u in CallManagementList.First().Value
                                         where u.Status == "Completed" && u.IssueGroup != null && u.IssueGroup != ""
                                         select u).ToList();
                        IList<CallManagementDashboard> diffinhours1 = new List<CallManagementDashboard>();
                        foreach (CallManagementDashboard cm in IssueList)
                        {
                            cm.DifferenceInHours = DateTime.Now - cm.IssueDate;
                            cm.Hours = Convert.ToInt16(cm.DifferenceInHours.Value.TotalHours);
                            diffinhours1.Add(cm);
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult InformationChart(string cam)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    CallManagementService cms = new CallManagementService();
                    Dictionary<string, object> IssCount = new Dictionary<string, object>();
                    IssCount.Add("BranchCode", cam);
                    Dictionary<long, IList<CallManagementDashboard>> CallManagementList = cms.GetCallManagementListWithPagingDashboard(null, 9999, string.Empty, string.Empty, IssCount);

                    if (CallManagementList != null && CallManagementList.Count > 0)
                    {
                        var IssueList = (from u in CallManagementList.First().Value
                                         where u.InformationFor != null && u.InformationFor != ""
                                         select u).ToList();

                        return Json(IssueList, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult WeeklyIssueStatus(string cam)
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

                    TotalCountPerDay1 = TotalCountPerDay1 + " select count(issuedate)as count, CONVERT(varchar , IssueDate ,105) as IssueDate from CallManagement ";
                    TotalCountPerDay1 = TotalCountPerDay1 + " where IssueDate between '" + FromDate + "' and '" + ToDate + "' ";
                    if (!string.IsNullOrEmpty(cam))
                    {
                        TotalCountPerDay1 = TotalCountPerDay1 + "and BranchCode='" + cam + "' ";
                    }
                    TotalCountPerDay1 = TotalCountPerDay1 + "and Status in ('Approveissue','Completed','Logissue','Resolveissue','ResolveIssueRejection','ApproveIssueRejection') ";
                    TotalCountPerDay1 = TotalCountPerDay1 + " group by  CONVERT(varchar , IssueDate ,105) ";
                    TotalCountPerDay1 = TotalCountPerDay1 + " order by IssueDate ";

                    string strQry1 = "";

                    strQry1 = strQry1 + " select issuedate ,SUM(completed) as completed, SUM(approveissue+logissue+ResolveIssue+ResolveIssueRejection+ApproveIssueRejection) as NonCompleted ";
                    strQry1 = strQry1 + "from ( ";
                    strQry1 = strQry1 + "select convert(varchar,issuedate,105) as issuedate, ";
                    strQry1 = strQry1 + "case when Status='ApproveIssue'then count(Status) else 0 end as approveissue, ";
                    strQry1 = strQry1 + "case when Status='Completed'then count(Status) else 0 end as Completed, ";
                    strQry1 = strQry1 + "case when Status='LogIssue'then count(Status) else 0 end as LogIssue, ";
                    strQry1 = strQry1 + "case when Status='ResolveIssue'then count(Status) else 0 end as ResolveIssue, ";
                    strQry1 = strQry1 + "case when Status='ResolveIssueRejection'then count(Status) else 0 end as ResolveIssueRejection, ";
                    strQry1 = strQry1 + "case when Status='ApproveIssueRejection'then count(Status) else 0 end as ApproveIssueRejection ";
                    strQry1 = strQry1 + "from CallManagement where  ";
                    strQry1 = strQry1 + "IssueDate between '" + FromDate + "' and '" + ToDate + "' ";
                    if (!string.IsNullOrEmpty(cam))
                    {
                        strQry1 = strQry1 + "and BranchCode='" + cam + "' ";
                    }
                    strQry1 = strQry1 + "and Status in ('Approveissue','Completed','Logissue','Resolveissue','ResolveIssueRejection','ApproveIssueRejection') ";
                    strQry1 = strQry1 + "group by Status,convert(varchar,issuedate,105)) a ";
                    strQry1 = strQry1 + "group by issuedate ";
                    strQry1 = strQry1 + "order by issuedate ";

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

                    if (TotIssueCntPerDayList.Count == 0 && IssueCntPerDayList.Count == 0)
                    {
                        string str = "";
                        str = str + "<graph  caption='' ";
                        str = str + "  xAxisName='Date' ";
                        str = str + " yAxisName='Issue Count' ";
                        str = str + " lineThickness='1' ";
                        str = str + " showValues='1' ";
                        str = str + " formatNumberScale='0' ";
                        str = str + " anchorRadius='2' ";
                        str = str + " divLineAlpha='20' ";
                        str = str + " divLineColor='CC3300' ";
                        str = str + " showAlternateHGridColor='1' ";
                        str = str + " alternateHGridColor='CC3300' ";
                        str = str + " shadowAlpha='40' ";
                        str = str + "  numvdivlines='7' ";
                        str = str + " chartRightMargin='35' ";
                        str = str + " bgColor='ffffff' ";
                        str = str + " alternateHGridAlpha='7' ";
                        str = str + " limitsDecimalPrecision='0' ";
                        str = str + " divLineDecimalPrecision='0' ";
                        str = str + " rotateNames='1' ";
                        str = str + " decimalPrecision='0'> ";

                        str = str + " <categories> ";
                        str = str + " <category name = ''/> ";
                        str = str + " </categories> ";
                        str = str + " <dataset seriesName ='Total Count' color = 'AA0078' anchorBorderColor = 'AA0078' anchorBgColor = 'AA0078'> ";
                        str = str + "<set value = '0'/> ";
                        str = str + "</dataset> ";
                        str = str + " <dataset seriesName ='Non Completed' color = '1D8BD1' anchorBorderColor = '1D8BD1' anchorBgColor = '1D8BD1'> ";
                        str = str + "<set value = '0'/> ";
                        str = str + "</dataset> ";
                        str = str + " <dataset seriesName ='Completed' color = 'F1683C' anchorBorderColor = 'F1683C' anchorBgColor = 'F1683C'> ";
                        str = str + "<set value = '0'/> ";
                        str = str + "</dataset> ";
                        str = str + "</graph>";
                        Response.Write(str);
                        return null;
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public PartialViewResult SendEmail()
        {
            return PartialView();
        }

        public ActionResult Dashboard()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult IssueList(string status, string IssueGroup, string InformationFor, string IsHosteller, string NonCompletedSLA, string CompletedSLA, string cam)
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult IssueListJqGrid(string status, string IssueGroup, string InformationFor, string IsHosteller, string NonCompletedSLA, string CompletedSLA, string Campus, int expxl, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    CallManagementService cms = new CallManagementService();
                    Dictionary<string, object> Criteria = new Dictionary<string, object>();
                    Criteria.Add("BranchCode", Campus);
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<CallManagementDashboard>> GetIssueCount = cms.GetCallManagementListWithPagingDashboard(page - 1, 9999, sidx, sord, Criteria);
                    var IssueCount = new List<CallManagementDashboard>();
                    if (!string.IsNullOrEmpty(status))
                    {
                        if (status == "LogIssue")
                        {
                            IssueCount = (from u in GetIssueCount.First().Value
                                          where (u.Status == status || u.Status == "ResolveIssueRejection") && u.IsInformation == false
                                          select u).ToList();
                        }
                        else if (status == "ResolveIssue")
                        {
                            IssueCount = (from u in GetIssueCount.First().Value
                                          where (u.Status == status || u.Status == "ApproveIssueRejection") && u.IsInformation == false
                                          select u).ToList();
                        }
                        else if (status == "ApproveIssue")
                        {
                            IssueCount = (from u in GetIssueCount.First().Value
                                          where (u.Status == "Complete") && u.IsInformation == false
                                          select u).ToList();
                        }
                        else
                        {
                            IssueCount = (from u in GetIssueCount.First().Value
                                          where u.Status == status && u.IsInformation == false
                                          select u).ToList();
                        }
                    }

                    else if (!string.IsNullOrEmpty(IssueGroup))
                    {
                        IssueCount = (from u in GetIssueCount.First().Value
                                      where u.IssueGroup == IssueGroup && u.IsInformation == false
                                      select u).ToList();
                    }

                    else if (!string.IsNullOrEmpty(InformationFor) && !string.IsNullOrEmpty(IsHosteller))
                    {
                        bool IsHostel = Convert.ToBoolean(IsHosteller);
                        IssueCount = (from u in GetIssueCount.First().Value
                                      where u.InformationFor == InformationFor && u.IsHosteller == IsHostel
                                      select u).ToList();
                    }

                    else if (!string.IsNullOrEmpty(InformationFor))
                    {
                        IssueCount = (from u in GetIssueCount.First().Value
                                      where u.InformationFor == InformationFor
                                      select u).ToList();
                    }
                    else if (!string.IsNullOrEmpty(NonCompletedSLA))
                    {
                        if (NonCompletedSLA == "below24")
                        {
                            IssueCount = (from u in GetIssueCount.First().Value
                                          where u.IsInformation == false && u.Status != "Completed" && DateTime.Now.Subtract(u.IssueDate).TotalHours < 24
                                          select u).ToList();
                        }
                        else if (NonCompletedSLA == "24-48")
                        {
                            IssueCount = (from u in GetIssueCount.First().Value
                                          where u.IsInformation == false && u.Status != "Completed" && (DateTime.Now.Subtract(u.IssueDate).TotalHours > 24 && DateTime.Now.Subtract(u.IssueDate).TotalHours < 48)
                                          select u).ToList();
                        }
                        else
                        {
                            IssueCount = (from u in GetIssueCount.First().Value
                                          where u.IsInformation == false && u.Status != "Completed" && DateTime.Now.Subtract(u.IssueDate).TotalHours > 48
                                          select u).ToList();
                        }
                    }
                    else if (!string.IsNullOrEmpty(CompletedSLA))
                    {
                        if (CompletedSLA == "below24")
                        {
                            IssueCount = (from u in GetIssueCount.First().Value
                                          where u.IsInformation == false && u.Status == "Completed" && DateTime.Now.Subtract(u.IssueDate).TotalHours < 24
                                          select u).ToList();
                        }
                        else if (CompletedSLA == "24-48")
                        {
                            IssueCount = (from u in GetIssueCount.First().Value
                                          where u.IsInformation == false && u.Status == "Completed" && (DateTime.Now.Subtract(u.IssueDate).TotalHours > 24 && DateTime.Now.Subtract(u.IssueDate).TotalHours < 48)
                                          select u).ToList();
                        }
                        else
                        {
                            IssueCount = (from u in GetIssueCount.First().Value
                                          where u.IsInformation == false && u.Status == "Completed" && DateTime.Now.Subtract(u.IssueDate).TotalHours > 48
                                          select u).ToList();
                        }
                    }

                    if (expxl == 1)
                    {
                        var IssueList = IssueCount.ToList();
                        base.ExptToXL(IssueList, "IssueList", (item => new
                        {
                            item.IssueNumber,
                            item.StudentName,
                            item.Grade,
                            item.IssueDate,
                            item.InformationFor,
                            item.LeaveType,
                            item.IssueGroup,
                            item.IssueType,
                            item.Status,
                            item.ActionDate,
                        }));
                        return new EmptyResult();
                    }

                    if (IssueCount != null && IssueCount.Count > 0)
                    {
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
                                       "<a href='/Home/CallForward?id=" + items.Id + "&activityId=" + items.Id + "&activityName=" + items.ActivityFullName + "&status=" + status +"&activityFullName=" + items.ActivityFullName + "'>" + items.IssueNumber + "</a>",
                                       items.StudentName,
                                       items.Grade,
                                       items.IssueDate!=null?items.IssueDate.ToString():null,
                                       items.InformationFor,
                                       items.LeaveType,
                                       items.IssueGroup,
                                       items.IssueType,
                                       items.Status, 
                                       items.ActionDate!=null? items.ActionDate.Value.Date.ToString("dd/MM/yyyy"):null,
                                       items.Email,
                                       items.Resolution,
                                       items.Id.ToString()
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public JsonResult Documentsjqgrid(long Id, string AppName, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                DocumentsService ds = new DocumentsService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("EntityRefId", Id);
                criteria.Add("AppName", AppName);
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
                               items.Upload_Id.ToString(),
                               items.UploadedBy,
                               items.UploadedOn.ToString(),
                               String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '#' onclick = 'uploaddat("+"\"" + items.Upload_Id + "\"" + ")' >{0}</a>",items.FileName),
                               items.DocumentType
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult uploaddisplay(long Id, string AppName)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                    else
                    {
                        DocumentsService ds = new DocumentsService();
                        Dictionary<string, object> criteria = new Dictionary<string, object>();
                        criteria.Add("Upload_Id", Id);
                        criteria.Add("AppName", AppName);
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
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public ActionResult NonCompletedSlaStatusChart1(string cam)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    CallManagementService cms = new CallManagementService();
                    Dictionary<string, object> Criteria = new Dictionary<string, object>();
                    Criteria.Add("Performer", userId);
                    Dictionary<long, IList<CallManagementChart>> CallManagementList = cms.GetCallManagementListWithPagingChart(null, 9999, string.Empty, string.Empty, Criteria);

                    if (CallManagementList != null && CallManagementList.Count > 0)
                    {
                        var IssueList = (from u in CallManagementList.First().Value
                                         where u.Status != "Completed" && u.IssueGroup != null && u.IssueGroup != ""
                                         select u).ToList();
                        IList<CallManagementChart> diffinhours1 = new List<CallManagementChart>();
                        foreach (CallManagementChart cm in IssueList)
                        {
                            cm.DifferenceInHours = DateTime.Now - cm.IssueDate;
                            cm.Hours = Convert.ToInt16(cm.DifferenceInHours.Value.TotalHours);
                            diffinhours1.Add(cm);
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public ActionResult GetIssueCountByStatus1(string cam)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    CallManagementService cms = new CallManagementService();
                    Dictionary<string, object> Criteria = new Dictionary<string, object>();
                    Criteria.Add("Performer", userId);
                    Dictionary<long, IList<CallManagementChart>> GetIssueCountByStatus = cms.GetCallManagementListWithPagingChart(null, 9999, string.Empty, string.Empty, Criteria);
                    var IssueCount = (from u in GetIssueCountByStatus.First().Value
                                      where u.IsInformation == false
                                      select u).ToList();
                    return Json(IssueCount, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult GetIssueCountByIssueGroup1(string cam)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    CallManagementService cms = new CallManagementService();
                    Dictionary<string, object> Criteria = new Dictionary<string, object>();
                    Criteria.Add("Performer", userId);
                    Dictionary<long, IList<CallManagementChart>> GetIssueCountByIssueGroup = cms.GetCallManagementListWithPagingChart(null, 9999, string.Empty, string.Empty, Criteria);
                    var IssueCount = (from u in GetIssueCountByIssueGroup.First().Value
                                      select u).ToList();
                    return Json(IssueCount, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public void SendMailToParent(CallManagement cm1, string IssueNumber, string Status, string recepient, string RejComments)
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
                        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                        mail.To.Add(recepient);
                        if (Status == "ResolveIssueRejection")
                        {
                            mail.Subject = "Tips support request #" + IssueNumber + " - Need more information";

                            string Body = "Dear Parent,<br/><br/>" +
                                "Tips support request #" + IssueNumber + " - Need more information<br/><br/>" +
                             " Thank you for registering issue, please find below the information needed by the concerned staff for the issue you had raised. <br/>"
                             + RejComments + ". " + "<br/>" +
                            " In-case you may need further information please mail our support desk.<br/><br/>" +
                            " Regards <br/>" +
                            " TIPS Support desk";

                            mail.Body = Body;
                        }

                        mail.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient("localhost", 25);
                        smtp.Host = "smtp.gmail.com";
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.EnableSsl = true;
                        BaseController bc = new BaseController();
                        IList<CampusEmailId> campusemaildet = bc.GetEmailIdByCampus(cm1.BranchCode, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                        if (From == "live")
                        {
                            try
                            {
                                mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                                smtp.Credentials = new System.Net.NetworkCredential
                               (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                                if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                {
                                    smtp.Send(mail);
                                }
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("quota"))
                                {
                                    mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                                    smtp.Credentials = new System.Net.NetworkCredential
                                    (campusemaildet.First().AlternateEmailId.ToString(), campusemaildet.First().AlternateEmailIdPassword.ToString());
                                    if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                    {
                                        smtp.Send(mail);
                                    }
                                }
                                else
                                {
                                    mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                                    smtp.Credentials = new System.Net.NetworkCredential
                                    (campusemaildet.First().AlternateEmailId.ToString(), campusemaildet.First().AlternateEmailIdPassword.ToString());
                                    if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                    {
                                        smtp.Send(mail);
                                    }
                                }
                            }
                        }
                        else if (From == "test")
                        {
                            mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                            smtp.Credentials = new System.Net.NetworkCredential
                             (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                            if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                            {
                                smtp.Send(mail);
                            }
                        }

                    }
                    catch (Exception)
                    {

                    }
                }
            }

            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }

        }

        public JsonResult GetEmailIds()
        {
            try
            {
                UserService us = new UserService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<UserAppRole>> UserAppRoleList = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                var Email = (from u in UserAppRoleList.First().Value
                             where u.Email != null
                             select u.Email).Distinct().ToArray();
                return Json(Email, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public JsonResult MoveBackToAvailable(long ActivityId)
        {
            ProcessFlowServices pfs = new ProcessFlowServices();
            try
            {
                bool backToAvailable = false;
                if (ActivityId > 0)
                {
                    backToAvailable = pfs.MoveBackToAvailable(ActivityId);
                    return Json(backToAvailable, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }

        }

        public ActionResult AdmissionCountReport()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ms = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    ViewBag.acadddl = AcademicyrMaster.First().Value;

                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }

        }

        public ActionResult AdmissionCountListJqGrid(string acayear, string campname, string grade, string section, string ExptType, int rows, string sidx, string sord, int? page)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    AdmissionManagementService ams = new AdmissionManagementService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrWhiteSpace(acayear)) { criteria.Add("AcademicYear", acayear); }
                    if (!string.IsNullOrWhiteSpace(campname))
                    {
                        if (campname.Contains("Select"))
                        {
                        }
                        else
                            criteria.Add("Campus", campname);
                    }
                    else
                    {
                        UserService us = new UserService();
                        Dictionary<string, object> criteriaUserAppRole = new Dictionary<string, object>();
                        criteriaUserAppRole.Add("UserId", userId);
                        Dictionary<long, IList<UserAppRole>> userAppRole = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteriaUserAppRole);
                        int count = 0;
                        int i = 0;
                        if (userAppRole != null && userAppRole.Count > 0 && userAppRole.First().Key > 0)
                        {
                            count = userAppRole.First().Value.Count;
                            string[] brnCodeArr = new string[count];
                            foreach (UserAppRole uar in userAppRole.First().Value)
                            {
                                string branchCode = uar.BranchCode;

                                if (!string.IsNullOrEmpty(branchCode))
                                {
                                    brnCodeArr[i] = branchCode;
                                }
                                i++;
                            }
                            brnCodeArr = brnCodeArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                            criteria.Add("Campus", brnCodeArr);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(section))
                    {
                        if (section.Contains("Select"))
                        {
                        }
                        else
                            criteria.Add("Section", section);
                    }

                    if (!string.IsNullOrWhiteSpace(grade))
                    {
                        if (grade.Contains("Select"))
                        {
                            grade = "";
                        }
                        else
                            criteria.Add("Grade", grade);
                    }
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<AdmissionCountReport_vw>> studentdetailslist = ams.GetAdmissionCountReport_vwWithEQsearchCriteria(page - 1, rows, sidx, sord, criteria);
                    if (studentdetailslist != null && studentdetailslist.First().Key > 0 && studentdetailslist.Count > 0)
                    {
                        if (ExptType == "Excel")
                        {
                            int Max = 24;
                            var List = studentdetailslist.First().Value.ToList();
                            base.ExptToXL(List, "AdmissionCountReport", (items => new
                            {
                                items.AcademicYear,
                                items.Campus,
                                items.Grade,
                                items.Section,
                                items.TotalCount,
                                items.Vacancy,
                                MaxAllowedStrength = Max
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalrecords = studentdetailslist.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            int Max = 24;
                            var jsondat1 = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,

                                rows = (from items in studentdetailslist.First().Value
                                        select new
                                        {
                                            cell = new string[] {
                               items.Id.ToString(),
                               items.AcademicYear,
                               items.Campus,
                               items.Grade,
                               items.Section,
                               items.TotalCount.ToString(),
                               items.Vacancy.ToString(),
                               Max.ToString(),
                            }
                                        })
                            };
                            return Json(jsondat1, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                        return Json(null);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public void SendTipstechMail(CallManagement cm)
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
                        IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(cm.BranchCode, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                        mail.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient("localhost", 25);
                        smtp.Host = "smtp.gmail.com";
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.EnableSsl = true;
                        mail.Subject = "Tips support request #" + cm.IssueNumber + " - Logged with Issue Group " + cm.IssueGroup + "";

                        string Body = "Dear Sir/Madam, <br/><br/>";
                        Body = Body + "Tips support request #" + cm.IssueNumber + " - Logged with Issue Group " + cm.IssueGroup + "<br/><br/>";
                        Body = Body + "Plz find below the details of the request<br/><br/><br/>";
                        if (!string.IsNullOrEmpty(cm.StudentNumber))
                        {
                            Body = Body + "<b>Student Name : </b>" + cm.StudentName + "<br/>";
                            Body = Body + "<b>Student ID : </b>" + cm.StudentNumber + "<br/>";
                            Body = Body + "<b>Grade : </b>" + cm.Grade + "<br/>";
                            Body = Body + "<b>Section : </b>" + cm.Section + "<br/><br/>";
                        }
                        Body = Body + "<b>Campus : </b>" + cm.BranchCode + "<br/>";
                        Body = Body + "<b>Issue Group : </b>" + cm.IssueGroup + "<br/>";
                        Body = Body + "<b>Issue Type : </b>" + cm.IssueType + "<br/>";
                        Body = Body + "<b>Caller Name : </b>" + cm.CallerName + "<br/>";
                        Body = Body + "<b>Caller Phone Number : </b>" + cm.CallPhoneNumber + "<br/>";
                        Body = Body + "<b>Caller Email Id : </b>" + cm.Email + "<br/><br/>";
                        Body = Body + "<b>Description : </b><br/><br/>";
                        Body = Body + " " + cm.Description + ".<br/><br/>";
                        Body = Body + " Regards<br/>";
                        Body = Body + " TIPS Support desk";
                        mail.Body = Body;
                        if (From == "live")
                        {
                            try
                            {
                                mail.To.Add("narayanan@tipstech.org");
                                mail.To.Add("chocks@tipstech.org");
                                mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                                smtp.Credentials = new System.Net.NetworkCredential
                              (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                                if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                {
                                    smtp.Send(mail);
                                }
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("quota"))
                                {
                                    mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                                    smtp.Credentials = new System.Net.NetworkCredential
                                    (campusemaildet.First().AlternateEmailId.ToString(), campusemaildet.First().AlternateEmailIdPassword.ToString());
                                    if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                    {
                                        smtp.Send(mail);
                                    }
                                }
                                else
                                {
                                    mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                                    smtp.Credentials = new System.Net.NetworkCredential
                                    (campusemaildet.First().AlternateEmailId.ToString(), campusemaildet.First().AlternateEmailIdPassword.ToString());
                                    if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                    {
                                        smtp.Send(mail);
                                    }
                                }
                            }
                        }
                        else if (From == "test")
                        {
                            mail.To.Add("m.anbarasan@xcdsys.com");
                            mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                            smtp.Credentials = new System.Net.NetworkCredential
                           (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                            if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                            {
                                smtp.Send(mail);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        #region TIPS Master
        public ActionResult TIPSMaster()
        {
            return View();
        }

        public JsonResult JqGridTIPSMaster(int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                UserService aps = new UserService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<TIPSMaster>> Master = aps.GetTIPSMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);

                if (Master != null && Master.FirstOrDefault().Key > 0)
                {
                    long totalrecords = Master.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in Master.First().Value
                                select new
                                {
                                    cell = new string[] {
                             items.Id.ToString(),
                             items.Campus,
                             items.Grade,
                             items.Section,
                             items.AcademicYear,
                             items.NumStud.ToString(),
                             items.CreatedBy,
                             items.ModifiedBy,
                             items.CreatedDate.ToString(),
                             items.ModifiedDate.ToString(),
                            }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);

                }
                else { return null; }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }


        }

        public ActionResult TIPSCrudFunction(TIPSMaster tips, string Edit, string Add, string Delete)
        {
            try
            {
                UserService us = new UserService();
                string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                if (Edit == "true")
                {
                    // Working for Edit option
                    us.CreateOrUpdateTIPSMaster(tips, userid);
                }
                else if (Add == "true")
                {
                    // Working for Add option
                    us.CreateOrUpdateTIPSMaster(tips, userid);
                }
                else
                {
                    // Working for Delete option

                }
                return null;
            }
            catch (Exception ex)
            {
                return ThrowJSONErrorNew(ex, "CallMgmntPolicy");
            }
            finally
            { }
        }

        #endregion End TIPS Master

        public void SaveAttendance(CallManagement cm)
        {
            if (cm.IssueType == "Extended Leave Request")
            {
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                var StartingDate = DateTime.Parse(Request["FromDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);

                // todate list
                IFormatProvider provider1 = new System.Globalization.CultureInfo("en-CA", true);
                var EndingDate = DateTime.Parse(Request["ToDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);

                foreach (DateTime absentDate in GetDateRange(StartingDate, EndingDate))
                {
                    DateTime DateNow = DateTime.Now;
                    Attendance att = new Attendance();
                    AttendanceService attSer = new AttendanceService();
                    AdmissionManagementService ams = new AdmissionManagementService();
                    StudentTemplate st = ams.GetStudentDetailsByNewId(cm.StudentNumber.Trim());
                    if (st.PreRegNum > 0)
                        att.PreRegNum = st.PreRegNum;
                    att.Name = cm.StudentName;
                    att.AbsentDate = absentDate;
                    att.IsAbsent = true;
                    att.AttendanceType = "FullDay";
                    att.CreatedBy = cm.UserInbox;
                    att.ModifiedBy = cm.UserInbox;
                    att.UserRole = cm.UserRoleName;
                    if (DateNow.Month >= 5)
                    {
                        att.AcademicYear = DateNow.Year.ToString() + "-" + (DateNow.Year + 1).ToString();
                    }
                    else { att.AcademicYear = (DateNow.Year - 1).ToString() + "-" + DateNow.Year.ToString(); }
                    att.EntryFrom = "CMS";
                    attSer.CreateOrUpdateAttendanceList(att);
                }
            }
            else
            {
                DateTime DateNow = DateTime.Now;
                Attendance att = new Attendance();
                AttendanceService attSer = new AttendanceService();
                AdmissionManagementService ams = new AdmissionManagementService();
                StudentTemplate st = ams.GetStudentDetailsByNewId(cm.StudentNumber.Trim());
                if (st.PreRegNum > 0)
                    att.PreRegNum = st.PreRegNum;
                att.Name = cm.StudentName;
                att.AbsentDate = cm.ActionDate;
                att.IsAbsent = true;
                att.CreatedBy = cm.UserInbox;
                att.ModifiedBy = cm.UserInbox;
                att.UserRole = cm.UserRoleName;
                if (DateNow.Month >= 5)
                {
                    att.AcademicYear = DateNow.Year.ToString() + "-" + (DateNow.Year + 1).ToString();
                }
                else { att.AcademicYear = (DateNow.Year - 1).ToString() + "-" + DateNow.Year.ToString(); }
                att.EntryFrom = "CMS";
                attSer.CreateOrUpdateAttendanceList(att);
            }
        }

        // find date list between two days
        private List<DateTime> GetDateRange(DateTime StartingDate, DateTime EndingDate)
        {
            if (StartingDate > EndingDate)
            {
                return null;
            }
            List<DateTime> rv = new List<DateTime>();
            DateTime tmpDate = StartingDate;
            do
            {
                rv.Add(tmpDate);
                tmpDate = tmpDate.AddDays(1);
            } while (tmpDate <= EndingDate);
            return rv;
        }


        #region SavedSearch Added By Micheal
        public ActionResult SaveorUpdateCMSSearchTemplate(SavedSearchTemplate ObjSvSrchTmplt)
        {
            try
            {
                UserService UsrSrv = new UserService();
                ObjSvSrchTmplt.CreatedBy = Session["userId"].ToString();
                ObjSvSrchTmplt.UserId = Session["userId"].ToString();
                if (ObjSvSrchTmplt.Id <= 0) { ObjSvSrchTmplt.DateCreated = DateTime.Now; }
                else { ObjSvSrchTmplt.DateModified = DateTime.Now; }

                long id = UsrSrv.CreateOrUpdateSavedSearchTemplate(ObjSvSrchTmplt);
                return Json(id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, "CallMgmntPolicy"); }
        }

        public ActionResult GetCMSSavedSearchTemplate()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    UserService UsrSrv = new UserService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("UserId", userId);
                    criteria.Add("Application", "CMS");
                    Dictionary<long, IList<SavedSearchTemplate>> dcnAssCompMaster = UsrSrv.GetSavedSearchTemplateListWithEQPagingAndCriteria(0, 50, "", "", criteria);
                    if (dcnAssCompMaster != null && dcnAssCompMaster.Count > 0)
                    {
                        IList<SavedSearchTemplate> AssCompMaster = dcnAssCompMaster.FirstOrDefault().Value;
                        var AssCompMasterLst = new
                        {
                            rows = (
                                 from items in AssCompMaster
                                 select new
                                 {
                                     Text = items.SearchName,
                                     Value = items.Id,
                                     //DateCreated = items.DateCreated,
                                     IsDefault = items.IsDefault,
                                     SavedSearch = items.SavedSearch
                                 }).ToArray()
                        };
                        return Json(AssCompMasterLst, JsonRequestBehavior.AllowGet);
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, "CallMgmntPolicy"); }
        }

        #endregion

        #region Monthly wise Issue and login count report
        public ActionResult LogInCountReport()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    int[] years = new int[15];
                    DateTime daytime = DateTime.Now;
                    int CurYear = daytime.Year;
                    ViewBag.CurYear = CurYear;
                    CurYear = CurYear - 5;
                    for (int i = 0; i < 15; i++)
                    {
                        years[i] = CurYear + i;
                    }
                    ViewBag.years = years;
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public ActionResult loginCountReportJqGrid(string id, string UserType, int? CountYear, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                UserService ussvc = new UserService();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(UserType))
                    criteria.Add("UserType", UserType.Trim());
                if (CountYear >= 0)
                    criteria.Add("Year", CountYear);
                Dictionary<long, IList<LogInCountReportView>> LoginCountList = ussvc.GetLogInCountListWithPagingAndCriteria_vw(page - 1, rows, sidx, sord, criteria);
                if (LoginCountList != null && LoginCountList.Count > 0 && LoginCountList.FirstOrDefault().Key > 0 && LoginCountList.FirstOrDefault().Value.Count > 0)
                {
                    if (ExptXl == 1)
                    {
                        var List = LoginCountList.First().Value.ToList();
                        base.ExptToXL(List, "LoginCountReportList", (items => new
                        {
                            items.Id,
                            items.UserType,
                            items.Year,
                            items.Jan,
                            items.Feb,
                            items.Mar,
                            items.Apr,
                            items.May,
                            items.Jun,
                            items.Jul,
                            items.Aug,
                            items.Sep,
                            items.Oct,
                            items.Nov,
                            items.Dec,

                        }));
                        return new EmptyResult();
                    }
                    else
                    {
                        long totalRecords = LoginCountList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows = (
                                 from items in LoginCountList.First().Value
                                 select new
                                 {
                                     i = items.Id,
                                     cell = new string[] { 
                                     items.Id.ToString(), 
                                     items.UserType,
                                     items.Year.ToString(), 
                                     items.Jan.ToString(), 
                                     items.Feb.ToString(), 
                                     items.Mar.ToString(), 
                                     items.Apr.ToString(), 
                                     items.May.ToString(), 
                                     items.Jun.ToString(), 
                                     items.Jul.ToString(), 
                                     items.Aug.ToString(), 
                                     items.Sep.ToString(),
                                     items.Oct.ToString(),
                                     items.Nov.ToString(),
                                     items.Dec.ToString()
                                     }
                                 })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public ActionResult IssueCountReport()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    int[] years = new int[15];
                    DateTime daytime = DateTime.Now;
                    int CurYear = daytime.Year;
                    ViewBag.CurYear = CurYear;
                    CurYear = CurYear - 5;
                    for (int i = 0; i < 15; i++)
                    {
                        years[i] = CurYear + i;
                    }
                    ViewBag.years = years;
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public ActionResult IssueCountReportJqGrid(string id, string Campus, int? CountYear, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                CallManagementService Cmsvc = new CallManagementService();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(Campus))
                    criteria.Add("Campus", Campus.Trim());
                if (CountYear >= 0)
                    criteria.Add("Year", CountYear);
                Dictionary<long, IList<IssueCountReportView>> IssueCountList = Cmsvc.GetIssueCountListWithPagingAndCriteria_vw(page - 1, rows, sidx, sord, criteria);
                if (IssueCountList != null && IssueCountList.Count > 0 && IssueCountList.FirstOrDefault().Key > 0 && IssueCountList.FirstOrDefault().Value.Count > 0)
                {
                    if (ExptXl == 1)
                    {
                        var List = IssueCountList.First().Value.ToList();
                        base.ExptToXL(List, "IssueCountReportList", (items => new
                        {
                            items.Id,
                            items.Campus,
                            items.Year,
                            items.Jan,
                            items.Feb,
                            items.Mar,
                            items.Apr,
                            items.May,
                            items.Jun,
                            items.Jul,
                            items.Aug,
                            items.Sep,
                            items.Oct,
                            items.Nov,
                            items.Dec,

                        }));
                        return new EmptyResult();
                    }
                    else
                    {
                        long totalRecords = IssueCountList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows = (
                                 from items in IssueCountList.First().Value
                                 select new
                                 {
                                     i = items.Id,
                                     cell = new string[] { 
                                     items.Id.ToString(), 
                                     items.Campus,
                                     items.Year.ToString(), 
                                     items.Jan.ToString(), 
                                     items.Feb.ToString(), 
                                     items.Mar.ToString(), 
                                     items.Apr.ToString(), 
                                     items.May.ToString(), 
                                     items.Jun.ToString(), 
                                     items.Jul.ToString(), 
                                     items.Aug.ToString(), 
                                     items.Sep.ToString(),
                                     items.Oct.ToString(),
                                     items.Nov.ToString(),
                                     items.Dec.ToString()
                                     }
                                 })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult GetloginCountReportChart(string UserType, int? CountYear)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    UserService Usr = new UserService();
                    Dictionary<string, object> Criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(UserType))
                        Criteria.Add("UserType", UserType.Trim());
                    if (CountYear >= 0)
                        Criteria.Add("Year", CountYear);
                    Dictionary<long, IList<LogInCountReportView>> GetLoginCountByUser = Usr.GetLogInCountListWithPagingAndCriteria_vw(null, 9999, string.Empty, string.Empty, Criteria);
                    var LogInCount = (from u in GetLoginCountByUser.First().Value
                                      select u).ToList();
                    var Jan = 0; var Feb = 0; var Mar = 0; var Apr = 0; var May = 0; var Jun = 0; var Jul = 0; var Aug = 0; var Sep = 0; var Oct = 0; var Nov = 0; var Dec = 0;
                    foreach (var itemdata in LogInCount)
                    {
                        Jan = Jan + itemdata.Jan;
                        Feb = Feb + itemdata.Feb;
                        Mar = Mar + itemdata.Mar;
                        Apr = Apr + itemdata.Apr;
                        May = May + itemdata.May;
                        Jun = Jun + itemdata.Jun;
                        Jul = Jul + itemdata.Jul;
                        Aug = Aug + itemdata.Aug;
                        Sep = Sep + itemdata.Sep;
                        Oct = Oct + itemdata.Oct;
                        Nov = Nov + itemdata.Nov;
                        Dec = Dec + itemdata.Dec;
                    }
                    var LogInChart = "<graph caption='' xAxisName='Month' yAxisName='LogIn Count' decimalPrecision='0' formatNumberScale='0' showNames='1' rotateNames='1'>";
                    LogInChart = LogInChart + " <set name='January' value='" + Jan + "' color='AFD8F8' />";
                    LogInChart = LogInChart + " <set name='February' value='" + Feb + "' color='F6BD0F' />";
                    LogInChart = LogInChart + " <set name='March' value='" + Mar + "' color='8BBA00' />";
                    LogInChart = LogInChart + " <set name='April' value='" + Apr + "' color='FF8E46' />";
                    LogInChart = LogInChart + " <set name='May' value='" + May + "' color='08E8E' />";
                    LogInChart = LogInChart + " <set name='June' value='" + Jun + "' color='D64646' />";
                    LogInChart = LogInChart + " <set name='July' value='" + Jul + "' color='8BBA00' />";
                    LogInChart = LogInChart + " <set name='August' value='" + Aug + "' color='FF8E46' />";
                    LogInChart = LogInChart + " <set name='September' value='" + Sep + "' color='08E8EA' />";
                    LogInChart = LogInChart + " <set name='October' value='" + Oct + "' color='D64646' />";
                    LogInChart = LogInChart + " <set name='November' value='" + Nov + "' color='08A8EA' />";
                    LogInChart = LogInChart + " <set name='December' value='" + Dec + "' color='08E0E5' /></graph>";
                    Response.Write(LogInChart);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult GetIssueCountReportChart(string Campus, int? CountYear)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    CallManagementService cmsr = new CallManagementService();
                    Dictionary<string, object> Criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(Campus))
                        Criteria.Add("Campus", Campus.Trim());
                    if (CountYear >= 0)
                        Criteria.Add("Year", CountYear);
                    Dictionary<long, IList<IssueCountReportView>> GetIssueCountByCampus = cmsr.GetIssueCountListWithPagingAndCriteria_vw(null, 9999, string.Empty, string.Empty, Criteria);
                    var IssueCount = (from u in GetIssueCountByCampus.First().Value
                                      select u).ToList();

                    var Jan = 0; var Feb = 0; var Mar = 0; var Apr = 0; var May = 0; var Jun = 0; var Jul = 0; var Aug = 0; var Sep = 0; var Oct = 0; var Nov = 0; var Dec = 0;
                    foreach (var itemdata in IssueCount)
                    {
                        Jan = Jan + itemdata.Jan;
                        Feb = Feb + itemdata.Feb;
                        Mar = Mar + itemdata.Mar;
                        Apr = Apr + itemdata.Apr;
                        May = May + itemdata.May;
                        Jun = Jun + itemdata.Jun;
                        Jul = Jul + itemdata.Jul;
                        Aug = Aug + itemdata.Aug;
                        Sep = Sep + itemdata.Sep;
                        Oct = Oct + itemdata.Oct;
                        Nov = Nov + itemdata.Nov;
                        Dec = Dec + itemdata.Dec;

                    }
                    var IssueCountChart = "<graph caption='' xAxisName='Month' yAxisName='Issue Count' decimalPrecision='0' formatNumberScale='0' showNames='1' rotateNames='1'>";
                    IssueCountChart = IssueCountChart + " <set name='January' value='" + Jan + "' color='AFD8F8' />";
                    IssueCountChart = IssueCountChart + " <set name='February' value='" + Feb + "' color='F6BD0F' />";
                    IssueCountChart = IssueCountChart + " <set name='March' value='" + Mar + "' color='8BBA00' />";
                    IssueCountChart = IssueCountChart + " <set name='April' value='" + Apr + "' color='FF8E46' />";
                    IssueCountChart = IssueCountChart + " <set name='May' value='" + May + "' color='08E8E' />";
                    IssueCountChart = IssueCountChart + " <set name='June' value='" + Jun + "' color='D64646' />";
                    IssueCountChart = IssueCountChart + " <set name='July' value='" + Jul + "' color='8BBA00' />";
                    IssueCountChart = IssueCountChart + " <set name='August' value='" + Aug + "' color='FF8E46' />";
                    IssueCountChart = IssueCountChart + " <set name='September' value='" + Sep + "' color='08E8EA' />";
                    IssueCountChart = IssueCountChart + " <set name='October' value='" + Oct + "' color='D64646' />";
                    IssueCountChart = IssueCountChart + " <set name='November' value='" + Nov + "' color='08E8EA' />";
                    IssueCountChart = IssueCountChart + " <set name='December' value='" + Dec + "' color='08E8E5' /></graph>";
                    Response.Write(IssueCountChart);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        #endregion

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

        #region Admin added by Micheal
        public ActionResult Admin()
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        #endregion

        public ActionResult KeepAlive()
        {
            return View();
        }

        public ActionResult DeleteUploadedFileById(long id)
        {
            try
            {
                Documents doc = new Documents();
                doc.Upload_Id = id;
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    DocumentsBC dbc = new DocumentsBC();
                    if (doc.Upload_Id > 0)
                    {
                        dbc.DeleteUploadedFileById(doc);
                    }
                    var script = @"InfoMsg(""Deleted  Successfully"");";
                    return JavaScript(script);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        #region General Home page design
        public ActionResult GeneralHomePage()
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        #endregion

        public ActionResult LinkToLMS()
        {
            string userId = base.ValidateUser();
            return Redirect("http://117.239.246.85:8083/index.php/login/icms_auth/" + userId);
        }

        public ActionResult IssueCountReportDurationWise()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult IssueCountReportDurationWiseJqGrid(string Campus, string FromDate, string ToDate, int rows, string sidx, string sord, int? page = 1, int? ExptXl = 0)
        {
            try
            {
                List<DataRow> IssueCountList = GetIssueCountList(Campus, FromDate, ToDate);
                List<DataRow> IssueCountListIssueGroupWise = GetIssueCountListIssueGroupWise(Campus, FromDate, ToDate);
                List<DataRow> AllIssuesWithDuration = GetAllIssueWithDuration(Campus, FromDate, ToDate);
                if (IssueCountList != null && IssueCountList.Count > 0)
                {
                    if (ExptXl == 1)
                    {
                        // Export to Excel with Multiple Sheets

                        DataSet Workbookset = new DataSet("WorkBook");
                        DataTable table1 = new DataTable();
                        table1.TableName = "Status wise";
                        Workbookset.Tables.Add(table1);

                        DataTable table2 = new DataTable();
                        table2.TableName = "Issue Group wise";
                        Workbookset.Tables.Add(table2);

                        DataTable table3 = new DataTable();
                        table3.TableName = "Show Issues";
                        Workbookset.Tables.Add(table3);
                        //
                        ExportToExcelIssueStatusCount(Workbookset, IssueCountList, IssueCountListIssueGroupWise, AllIssuesWithDuration);

                        //var List = IssueCountList;
                        //base.ExptToXL(List, "IssueCountReportList", (items => new
                        //{
                        //    Id = items.ItemArray[0].ToString(),
                        //    Campus = items.ItemArray[1].ToString(),
                        //    Logged = items.ItemArray[2].ToString(),
                        //    Completed = items.ItemArray[3].ToString(),
                        //    NonCompleted = items.ItemArray[4].ToString()
                        //}));
                        //  return new EmptyResult();
                    }
                    else
                    {
                        long totalRecords = IssueCountList.Count;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows = (
                                 from items in IssueCountList
                                 select new
                                 {
                                     cell = new string[] { 
                                     items.ItemArray[0].ToString(), 
                                     items.ItemArray[1].ToString(),
                                     items.ItemArray[2].ToString(), 
                                     items.ItemArray[3].ToString(), 
                                     items.ItemArray[4].ToString(),
                                     }
                                 })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public void ExportToExcelIssueStatusCount(DataSet Workbookset, List<DataRow> IssueCountList, List<DataRow> IssueCountListIssueGroupWise, List<DataRow> AllIssuesWithDuration)
        {
            try
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    int TableCount = Workbookset.Tables.Count;
                    for (int i = 0; i < TableCount; i++)
                    {
                        if (Workbookset.Tables[i].TableName.ToString() == "Status wise")
                        {
                            ExcelWorksheet ws = pck.Workbook.Worksheets.Add(Workbookset.Tables[i].TableName.ToString());
                            //ws.View.ShowGridLines = false;
                            //ws.Column(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Row(1).Height = 28.50;
                            //Color Selection
                            System.Drawing.Color WhiteHex = System.Drawing.ColorTranslator.FromHtml("#F2F2F2");
                            System.Drawing.Color GreenHex = System.Drawing.ColorTranslator.FromHtml("#70AD47");
                            List<string> lstHeader = new List<string> { "Id", "Campus", "Logged", "Completed", "NonCompleted" };
                            //Header Section
                            for (int k = 0; k < lstHeader.Count; k++)
                            {
                                ws.Cells[1, k + 1].Value = lstHeader[k];
                                ws.Cells[1, k + 1].Style.Font.Bold = true;
                                ws.Cells[1, k + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                ws.Cells[1, k + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                ws.Cells[1, k + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells[1, k + 1].Style.Fill.BackgroundColor.SetColor(GreenHex);
                                ws.Cells[1, k + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                ws.Cells[1, k + 1].AutoFitColumns(15);
                            }
                            int j = 2;
                            for (int P = 0; P < IssueCountList.Count(); P++)
                            {
                                ws.Cells["A" + j + ""].Value = Convert.ToInt32(IssueCountList[P].ItemArray[0]);
                                ws.Cells["B" + j + ""].Value = IssueCountList[P].ItemArray[1];
                                ws.Cells["C" + j + ""].Value = Convert.ToInt32(IssueCountList[P].ItemArray[2]);
                                ws.Cells["D" + j + ""].Value = Convert.ToInt32(IssueCountList[P].ItemArray[3]);
                                ws.Cells["E" + j + ""].Value = Convert.ToInt32(IssueCountList[P].ItemArray[4]);
                                j = j + 1;
                            }
                            int Rowcount = IssueCountList.Count() + 2;
                            int columnCount = lstHeader.Count() + 1;

                            //Borders Matrix Logic
                            for (int l = 1; l < Rowcount; l++)
                            {
                                for (int m = 1; m < columnCount; m++)
                                {
                                    ws.Cells[l, m].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    ws.Cells[l, m].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    ws.Cells[l, m].Style.Border.Bottom.Color.SetColor(GreenHex);
                                    ws.Cells[l, m].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    ws.Cells[l, m].Style.Border.Left.Color.SetColor(GreenHex);
                                    ws.Cells[l, m].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                    ws.Cells[l, m].Style.Border.Right.Color.SetColor(GreenHex);
                                }
                            }
                            ws.Cells["A" + 2 + ":E" + IssueCountList.Count() + 1 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        }
                        if (Workbookset.Tables[i].TableName.ToString() == "Issue Group wise")
                        {
                            ExcelWorksheet ws = pck.Workbook.Worksheets.Add(Workbookset.Tables[i].TableName.ToString());
                            // ws.View.ShowGridLines = false;
                            ws.Row(1).Height = 28.50;
                            //Color Selection
                            System.Drawing.Color WhiteHex = System.Drawing.ColorTranslator.FromHtml("#F2F2F2");
                            System.Drawing.Color GreenHex = System.Drawing.ColorTranslator.FromHtml("#70AD47");
                            List<string> lstHeader = new List<string> { "Id", "Campus", "Issue Group", "Logged", "Completed", "NonCompleted" };
                            //Header Section
                            for (int k = 0; k < lstHeader.Count; k++)
                            {
                                ws.Cells[1, k + 1].Value = lstHeader[k];
                                ws.Cells[1, k + 1].Style.Font.Bold = true;
                                ws.Cells[1, k + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                ws.Cells[1, k + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                ws.Cells[1, k + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells[1, k + 1].Style.Fill.BackgroundColor.SetColor(GreenHex);
                                ws.Cells[1, k + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                ws.Cells[1, k + 1].AutoFitColumns(15);
                            }
                            int j = 2;
                            for (int P = 0; P < IssueCountListIssueGroupWise.Count(); P++)
                            {
                                ws.Cells["A" + j + ""].Value = Convert.ToInt32(IssueCountListIssueGroupWise[P].ItemArray[0]);
                                ws.Cells["B" + j + ""].Value = IssueCountListIssueGroupWise[P].ItemArray[1];
                                ws.Cells["C" + j + ""].Value = IssueCountListIssueGroupWise[P].ItemArray[2];
                                ws.Cells["D" + j + ""].Value = Convert.ToInt32(IssueCountListIssueGroupWise[P].ItemArray[3]);
                                ws.Cells["E" + j + ""].Value = Convert.ToInt32(IssueCountListIssueGroupWise[P].ItemArray[4]);
                                ws.Cells["F" + j + ""].Value = Convert.ToInt32(IssueCountListIssueGroupWise[P].ItemArray[5]);
                                j = j + 1;
                            }
                            int Rowcount = IssueCountListIssueGroupWise.Count() + 2;
                            int columnCount = lstHeader.Count() + 1;

                            //Borders Matrix Logic
                            for (int l = 1; l < Rowcount; l++)
                            {
                                for (int m = 1; m < columnCount; m++)
                                {
                                    ws.Cells[l, m].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    ws.Cells[l, m].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    ws.Cells[l, m].Style.Border.Bottom.Color.SetColor(GreenHex);
                                    ws.Cells[l, m].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    ws.Cells[l, m].Style.Border.Left.Color.SetColor(GreenHex);
                                    ws.Cells[l, m].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                    ws.Cells[l, m].Style.Border.Right.Color.SetColor(GreenHex);
                                }
                            }
                            ws.Cells["A" + 2 + ":F" + IssueCountListIssueGroupWise.Count() + 1 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        }

                        if (Workbookset.Tables[i].TableName.ToString() == "Show Issues")
                        {
                            ExcelWorksheet ws = pck.Workbook.Worksheets.Add(Workbookset.Tables[i].TableName.ToString());
                            // ws.View.ShowGridLines = false;
                            ws.Row(1).Height = 28.50;
                            //Color Selection
                            System.Drawing.Color WhiteHex = System.Drawing.ColorTranslator.FromHtml("#F2F2F2");
                            System.Drawing.Color GreenHex = System.Drawing.ColorTranslator.FromHtml("#70AD47");
                            List<string> lstHeader = new List<string> { "Id", "Campus", "Grade", "Section", "Issue Group", "Issue Type", "Description", "Created Date", "Created By", "Status" };
                            //Header Section
                            for (int k = 0; k < lstHeader.Count; k++)
                            {
                                ws.Cells[1, k + 1].Value = lstHeader[k];
                                ws.Cells[1, k + 1].Style.Font.Bold = true;
                                ws.Cells[1, k + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                ws.Cells[1, k + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                ws.Cells[1, k + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells[1, k + 1].Style.Fill.BackgroundColor.SetColor(GreenHex);
                                ws.Cells[1, k + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                ws.Cells[1, k + 1].AutoFitColumns(15);
                            }
                            int j = 2;
                            for (int P = 0; P < AllIssuesWithDuration.Count(); P++)
                            {
                                ws.Cells["A" + j + ""].Value = Convert.ToInt32(AllIssuesWithDuration[P].ItemArray[0]);
                                ws.Cells["B" + j + ""].Value = AllIssuesWithDuration[P].ItemArray[1];
                                ws.Cells["C" + j + ""].Value = AllIssuesWithDuration[P].ItemArray[2];
                                ws.Cells["D" + j + ""].Value = AllIssuesWithDuration[P].ItemArray[3];
                                ws.Cells["E" + j + ""].Value = AllIssuesWithDuration[P].ItemArray[4];
                                ws.Cells["F" + j + ""].Value = AllIssuesWithDuration[P].ItemArray[5];
                                ws.Cells["G" + j + ""].Value = AllIssuesWithDuration[P].ItemArray[6];
                                ws.Cells["H" + j + ""].Value = AllIssuesWithDuration[P].ItemArray[7];
                                ws.Cells["I" + j + ""].Value = AllIssuesWithDuration[P].ItemArray[8];
                                ws.Cells["J" + j + ""].Value = AllIssuesWithDuration[P].ItemArray[9];
                                j = j + 1;
                            }
                            int Rowcount = AllIssuesWithDuration.Count() + 2;
                            int columnCount = lstHeader.Count() + 1;

                            //Borders Matrix Logic
                            for (int l = 1; l < Rowcount; l++)
                            {
                                for (int m = 1; m < columnCount; m++)
                                {
                                    ws.Cells[l, m].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    ws.Cells[l, m].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    ws.Cells[l, m].Style.Border.Bottom.Color.SetColor(GreenHex);
                                    ws.Cells[l, m].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    ws.Cells[l, m].Style.Border.Left.Color.SetColor(GreenHex);
                                    ws.Cells[l, m].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                    ws.Cells[l, m].Style.Border.Right.Color.SetColor(GreenHex);
                                }
                            }
                            ws.Cells["A" + 2 + ":J" + AllIssuesWithDuration.Count() + 1 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        }
                    }
                    byte[] data = pck.GetAsByteArray();
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=" + "Reports" + ".xlsx");
                    Response.BinaryWrite(data);
                    Response.End();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public JsonResult IssueCountReportChartDurationWise(string Campus, string FromDate, string ToDate)
        {
            string strxml = "";
            strxml = strxml + "<graph  caption='' ";
            strxml = strxml + "  xAxisName='Campus' ";
            strxml = strxml + " yAxisName='Issue Count' ";
            strxml = strxml + " lineThickness='1' ";
            strxml = strxml + " showValues='0' ";
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

            List<DataRow> IssueCountList = GetIssueCountList(Campus, FromDate, ToDate);
            if (IssueCountList.Count == 0)
            {
                string str = "";
                str = str + "<graph  caption='' ";
                str = str + "  xAxisName='Date' ";
                str = str + " yAxisName='Issue Count' ";
                str = str + " lineThickness='1' ";
                str = str + " showValues='1' ";
                str = str + " formatNumberScale='0' ";
                str = str + " anchorRadius='2' ";
                str = str + " divLineAlpha='20' ";
                str = str + " divLineColor='CC3300' ";
                str = str + " showAlternateHGridColor='1' ";
                str = str + " alternateHGridColor='CC3300' ";
                str = str + " shadowAlpha='40' ";
                str = str + "  numvdivlines='7' ";
                str = str + " chartRightMargin='35' ";
                str = str + " bgColor='ffffff' ";
                str = str + " alternateHGridAlpha='7' ";
                str = str + " limitsDecimalPrecision='0' ";
                str = str + " divLineDecimalPrecision='0' ";
                str = str + " rotateNames='1' ";
                str = str + " decimalPrecision='0'> ";

                str = str + " <categories> ";
                str = str + " <category name = ''/> ";
                str = str + " </categories> ";
                str = str + " <dataset seriesName ='Total Count' color = 'AA0078' anchorBorderColor = 'AA0078' anchorBgColor = 'AA0078'> ";
                str = str + "<set value = '0'/> ";
                str = str + "</dataset> ";
                str = str + " <dataset seriesName ='Non Completed' color = '1D8BD1' anchorBorderColor = '1D8BD1' anchorBgColor = '1D8BD1'> ";
                str = str + "<set value = '0'/> ";
                str = str + "</dataset> ";
                str = str + " <dataset seriesName ='Completed' color = 'F1683C' anchorBorderColor = 'F1683C' anchorBgColor = 'F1683C'> ";
                str = str + "<set value = '0'/> ";
                str = str + "</dataset> ";
                str = str + "</graph>";
                Response.Write(str);
                return null;
            }
            if (IssueCountList != null && IssueCountList.Count > 0)
            {
                for (int d = 0; d < IssueCountList.Count; d++)
                {
                    strxml = strxml + " <category name = '" + IssueCountList[d].ItemArray[1] + "'/> ";
                }
                strxml = strxml + " </categories> ";
                strxml = strxml + " <dataset seriesName ='Total Count' color = 'AA0078' anchorBorderColor = 'AA0078' anchorBgColor = 'AA0078'> ";
                for (int e = 0; e < IssueCountList.Count; e++)
                {
                    strxml = strxml + "<set value = '" + IssueCountList[e].ItemArray[2] + "'/> ";
                }
                strxml = strxml + "</dataset> ";
                strxml = strxml + " <dataset seriesName ='Completed' color = '1D8BD1' anchorBorderColor = '1D8BD1' anchorBgColor = '1D8BD1'> ";
                for (int f = 0; f < IssueCountList.Count; f++)
                {
                    strxml = strxml + "<set value = '" + IssueCountList[f].ItemArray[3] + "'/> ";
                }
                strxml = strxml + "</dataset> ";
                strxml = strxml + " <dataset seriesName ='NonCompleted' color = 'F1683C' anchorBorderColor = 'F1683C' anchorBgColor = 'F1683C'>  ";
                for (int g = 0; g < IssueCountList.Count; g++)
                {
                    strxml = strxml + "<set value = '" + IssueCountList[g].ItemArray[4] + "'/> ";
                }
                strxml = strxml + "</dataset> ";
            }
            strxml = strxml + "</graph>";
            Response.Write(strxml);
            return null;
        }

        public List<DataRow> GetIssueCountList(string Campus, string FromDate, string ToDate)
        {
            IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
            DateTime? fdate = null;
            DateTime? tdate = null;
            if (!string.IsNullOrWhiteSpace(FromDate))
            {
                fdate = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
            }
            if (!string.IsNullOrWhiteSpace(ToDate))
            {

                tdate = DateTime.Parse(ToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                string To = string.Format("{0:MM/dd/yyyy}", tdate);
                tdate = Convert.ToDateTime(To + " " + "23:59:59");
            }
            else
                tdate = DateTime.Now;
            string strQry1 = "";
            strQry1 = strQry1 + " select ROW_NUMBER() OVER (ORDER BY b.BranchCode) AS Id,  b.BranchCode, (b.Completed+b.NonCompleted) Logged, b.Completed, b.NonCompleted  from ";
            strQry1 = strQry1 + " ( ";
            strQry1 = strQry1 + " select a.BranchCode ,SUM(completed) as Completed, SUM(ResolveIssue+ResolveIssueRejection+approveissue+ApproveIssueRejection+Complete) as NonCompleted ";
            strQry1 = strQry1 + " from ( ";
            strQry1 = strQry1 + " select BranchCode, ";
            strQry1 = strQry1 + " case when Status='ResolveIssue'then count(Status) else 0 end as ResolveIssue, ";
            strQry1 = strQry1 + " case when Status='ResolveIssueRejection'then count(Status) else 0 end as ResolveIssueRejection, ";
            strQry1 = strQry1 + " case when Status='ApproveIssue'then count(Status) else 0 end as approveissue, ";
            strQry1 = strQry1 + " case when Status='ApproveIssueRejection'then count(Status) else 0 end as ApproveIssueRejection, ";
            strQry1 = strQry1 + " case when Status='Complete'then count(Status) else 0 end as Complete, ";
            strQry1 = strQry1 + " case when Status='Completed'then count(Status) else 0 end as Completed ";
            strQry1 = strQry1 + " from CallManagement where  BranchCode is not null and IsInformation=0 and Status in ('Approveissue','Completed','Logissue','Resolveissue','ResolveIssueRejection','ApproveIssueRejection') ";

            if (!string.IsNullOrWhiteSpace(Campus) && fdate != null && tdate != null)
            {
                strQry1 = strQry1 + " and BranchCode = '" + Campus + "' ";
                if (fdate != null && tdate != null)
                    strQry1 = strQry1 + " and IssueDate between '" + fdate + "' and '" + tdate + "' ";
            }
            if (!string.IsNullOrWhiteSpace(Campus) && fdate == null && tdate == null)
            {
                strQry1 = strQry1 + " and BranchCode = '" + Campus + "' ";
            }
            if (!string.IsNullOrWhiteSpace(Campus) && fdate == null && tdate != null)
            {
                strQry1 = strQry1 + " and BranchCode = '" + Campus + "' ";
                strQry1 = strQry1 + " and IssueDate <='" + tdate + "' ";
            }
            if (fdate != null && tdate != null)
                strQry1 = strQry1 + " and IssueDate between '" + fdate + "' and '" + tdate + "' ";

            strQry1 = strQry1 + " group by Status,BranchCode) a ";
            strQry1 = strQry1 + " group by a.BranchCode ";
            strQry1 = strQry1 + " )b ";
            CallManagementService cms = new CallManagementService();
            DataTable IssueCount = cms.GetStatusWiseIssueCount(strQry1);
            List<DataRow> IssueCountList = null;
            if (IssueCount != null)
            {
                IssueCountList = IssueCount.AsEnumerable().ToList();
            }
            return IssueCountList;
        }

        public List<DataRow> GetIssueCountListIssueGroupWise(string Campus, string FromDate, string ToDate)
        {
            IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
            DateTime? fdate = null;
            DateTime? tdate = null;
            if (!string.IsNullOrWhiteSpace(FromDate))
            {
                fdate = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
            }
            if (!string.IsNullOrWhiteSpace(ToDate))
            {

                tdate = DateTime.Parse(ToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                string To = string.Format("{0:MM/dd/yyyy}", tdate);
                tdate = Convert.ToDateTime(To + " " + "23:59:59");
            }
            else
                tdate = DateTime.Now;
            string strQry1 = "";
            strQry1 = strQry1 + " select ROW_NUMBER() OVER (ORDER BY b.BranchCode) AS Id,  b.BranchCode, b.IssueGroup, (b.Completed+b.NonCompleted) Logged, b.Completed, b.NonCompleted  from ";
            strQry1 = strQry1 + " ( ";
            strQry1 = strQry1 + " select a.BranchCode,a.IssueGroup ,SUM(completed) as Completed, SUM(ResolveIssue+ResolveIssueRejection+approveissue+ApproveIssueRejection+Complete) as NonCompleted ";
            strQry1 = strQry1 + " from ( ";
            strQry1 = strQry1 + " select BranchCode, IssueGroup, ";
            strQry1 = strQry1 + " case when Status='ResolveIssue'then count(Status) else 0 end as ResolveIssue, ";
            strQry1 = strQry1 + " case when Status='ResolveIssueRejection'then count(Status) else 0 end as ResolveIssueRejection, ";
            strQry1 = strQry1 + " case when Status='ApproveIssue'then count(Status) else 0 end as approveissue, ";
            strQry1 = strQry1 + " case when Status='ApproveIssueRejection'then count(Status) else 0 end as ApproveIssueRejection, ";
            strQry1 = strQry1 + " case when Status='Complete'then count(Status) else 0 end as Complete, ";
            strQry1 = strQry1 + " case when Status='Completed'then count(Status) else 0 end as Completed ";
            strQry1 = strQry1 + " from CallManagement where BranchCode is not null and IsInformation=0 and IssueGroup is not null and  Status in ('Approveissue','Completed','Logissue','Resolveissue','ResolveIssueRejection','ApproveIssueRejection') ";

            if (!string.IsNullOrWhiteSpace(Campus) && fdate != null && tdate != null)
            {
                strQry1 = strQry1 + " and BranchCode = '" + Campus + "' ";
                if (fdate != null && tdate != null)
                    strQry1 = strQry1 + " and IssueDate between '" + fdate + "' and '" + tdate + "' ";
            }
            if (!string.IsNullOrWhiteSpace(Campus) && fdate == null && tdate == null)
            {
                strQry1 = strQry1 + " and BranchCode = '" + Campus + "' ";
            }
            if (!string.IsNullOrWhiteSpace(Campus) && fdate == null && tdate != null)
            {
                strQry1 = strQry1 + " and BranchCode = '" + Campus + "' ";
                strQry1 = strQry1 + " and IssueDate <='" + tdate + "' ";
            }
            if (fdate != null && tdate != null)
                strQry1 = strQry1 + " and IssueDate between '" + fdate + "' and '" + tdate + "' ";

            strQry1 = strQry1 + " group by Status,BranchCode,IssueGroup) a ";
            strQry1 = strQry1 + " group by a.BranchCode,a.IssueGroup  ";
            strQry1 = strQry1 + " )b ";
            CallManagementService cms = new CallManagementService();
            DataTable IssueCount = cms.GetStatusWiseIssueCount(strQry1);
            List<DataRow> IssueCountList = null;
            if (IssueCount != null)
            {
                IssueCountList = IssueCount.AsEnumerable().ToList();
            }
            return IssueCountList;
        }

        public List<DataRow> GetAllIssueWithDuration(string Campus, string FromDate, string ToDate)
        {
            IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
            DateTime? fdate = null;
            DateTime? tdate = null;
            if (!string.IsNullOrWhiteSpace(FromDate))
            {
                fdate = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
            }
            if (!string.IsNullOrWhiteSpace(ToDate))
            {

                tdate = DateTime.Parse(ToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                string To = string.Format("{0:MM/dd/yyyy}", tdate);
                tdate = Convert.ToDateTime(To + " " + "23:59:59");
            }
            else
                tdate = DateTime.Now;
            string strQry1 = "";

            strQry1 = strQry1 + " select ROW_NUMBER() OVER (ORDER BY BranchCode) AS Id, BranchCode,Grade, Section, IssueGroup, IssueType, Description, IssueDate,UserInbox,Status ";
            strQry1 = strQry1 + " from CallManagement ";
            strQry1 = strQry1 + " where BranchCode is not null and IsInformation=0 and IssueGroup is not null ";

            if (!string.IsNullOrWhiteSpace(Campus) && fdate != null && tdate != null)
            {
                strQry1 = strQry1 + " and BranchCode = '" + Campus + "' ";
                if (fdate != null && tdate != null)
                    strQry1 = strQry1 + " and IssueDate between '" + fdate + "' and '" + tdate + "' ";
            }
            if (!string.IsNullOrWhiteSpace(Campus) && fdate == null && tdate == null)
            {
                strQry1 = strQry1 + " and BranchCode = '" + Campus + "' ";
            }
            if (!string.IsNullOrWhiteSpace(Campus) && fdate == null && tdate != null)
            {
                strQry1 = strQry1 + " and BranchCode = '" + Campus + "' ";
                strQry1 = strQry1 + " and IssueDate <='" + tdate + "' ";
            }
            if (fdate != null && tdate != null)
                strQry1 = strQry1 + " and IssueDate between '" + fdate + "' and '" + tdate + "' ";

            CallManagementService cms = new CallManagementService();
            DataTable IssueCount = cms.GetStatusWiseIssueCount(strQry1);
            List<DataRow> IssueCountList = null;
            if (IssueCount != null)
            {
                IssueCountList = IssueCount.AsEnumerable().ToList();
            }
            return IssueCountList;
        }

        public ActionResult CoOrdinatorsContactInfo()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    //Modified by Thamizhmani
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult CoOrdinatorsContactInfoJqGrid(CoOrdinatorsContactInfo cco, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    UserService us = new UserService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrWhiteSpace(cco.Campus)) criteria.Add("Campus", cco.Campus);
                    if (!string.IsNullOrWhiteSpace(cco.Server)) criteria.Add("Server", cco.Server);
                    if (!string.IsNullOrWhiteSpace(cco.EmailId)) criteria.Add("EmailId", cco.EmailId);
                    if (!string.IsNullOrWhiteSpace(cco.MobileNo)) criteria.Add("MobileNo", cco.MobileNo);
                    IList<CoOrdinatorsContactInfo> CoOrdinatorsContactInfo = us.GetCoOrdinatorsContactInfoListWithPaging(0, 1000, string.Empty, string.Empty, criteria);

                    if (CoOrdinatorsContactInfo != null && CoOrdinatorsContactInfo.Count() > 0)
                    {
                        if (ExportType == "Excel")
                        {
                            var List = CoOrdinatorsContactInfo.ToList();
                            ExptToXL(List, "VehicleTyreMaintenanceList", (items => new
                            {
                                items.Id,
                                items.Campus,
                                items.Server,
                                items.Category,
                                items.EmailId,
                                items.MobileNo
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalrecords = CoOrdinatorsContactInfo.Count;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var CoOrdinatorsContactInfoList = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,
                                rows = (from items in CoOrdinatorsContactInfo
                                        select new
                                        {
                                            cell = new string[] {
                               items.Id.ToString(),items.Campus, items.Server, items.Category, items.EmailId, items.MobileNo
                            }
                                        })
                            };
                            return Json(CoOrdinatorsContactInfoList, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddCoOrdinatorsContactInfo(CoOrdinatorsContactInfo cci, string test)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    UserService us = new UserService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrWhiteSpace(cci.Campus)) criteria.Add("Campus", cci.Campus);
                    if (!string.IsNullOrWhiteSpace(cci.Server)) criteria.Add("Server", cci.Server);
                    if (!string.IsNullOrWhiteSpace(cci.Category)) criteria.Add("Category", cci.Category);
                    if (!string.IsNullOrWhiteSpace(cci.EmailId)) criteria.Add("EmailId", cci.EmailId);
                    if (!string.IsNullOrWhiteSpace(cci.MobileNo)) criteria.Add("MobileNo", cci.MobileNo);
                    IList<CoOrdinatorsContactInfo> CoOrdinatorsContactInfo = us.GetCoOrdinatorsContactInfoListWithPaging(0, 1000, string.Empty, string.Empty, criteria);
                    var script = "";
                    if (CoOrdinatorsContactInfo != null && CoOrdinatorsContactInfo.Count() > 0)
                    {
                        if (test == "edit")
                        {
                            script = @"ErrMsg(""This Combination already exists"");";
                        }
                    }
                    else
                    {
                        us.CreateOrUpdateCoOrdinatorsContactInfo(cci);
                        script = @"InfoMsg(""Created Successfully"");";
                    }
                    return JavaScript(script);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public void SendEmailToStudent(CallManagement cm, string RejComments)
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
                        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                        mail.To.Add(cm.Email);
                        IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(cm.BranchCode, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                        string Body = "";
                        if (cm.Status == "ResolveIssueRejection")
                        {
                            mail.Subject = "Tips support request #" + cm.IssueNumber + " - is rejected";
                            Body = "Dear Student,<br/><br/>" +
                               "Tips support request #" + cm.IssueNumber + " - is rejected<br/><br/>" +
                               "<b>Issue Description</b><br/><br/>" +
                                        cm.Description + ".<br/><br/>" +
                            " Thank you for contacting our support desk, please find below the rejection comments given by the concerned staff for the issue you had raised. <br/>" +
                            "<b>Rejection Description</b><br/><br/>" +
                             RejComments + ". " + "<br/>" +
                           " Regards <br/>" +
                           " TIPS Support desk";
                        }
                        if (cm.Status == "Completed")
                        {
                            mail.Subject = "Tips support request #" + cm.IssueNumber + " is completed";
                            Body = "Dear Student,<br/><br/>" +
                           "Tips support request #" + cm.IssueNumber + " is completed<br/><br/>" +
                           "<b>Issue Description</b><br/><br/>" +
                                        cm.Description + ".<br/><br/>" +
                           " Thank you for contacting our support desk and your issue is resolved with the below comments.<br/><br/>" +
                           "<b>Resolution Description</b><br/><br/>" +
                           cm.Resolution + ". " + "<br/><br/>" +
                           " Regards,<br/>" +
                           " TIPS Support desk";
                        }
                        mail.Body = Body;
                        mail.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient("localhost", 25);
                        smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address  
                        //Or your Smtp Email ID and Password  
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.EnableSsl = true;
                        smtp.Credentials = new System.Net.NetworkCredential
                        (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                        mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                        if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                        {
                            try
                            {
                                new Task(() => { smtp.Send(mail); }).Start();
                            }
                            catch (Exception)
                            {
                                mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                                smtp.Credentials = new System.Net.NetworkCredential(campusemaildet.First().AlternateEmailId, campusemaildet.First().AlternateEmailIdPassword);
                                new Task(() => { smtp.Send(mail); }).Start();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }

        }
        #region UserAppRole Modified by Thamizh
        public JsonResult UserAppRolejqgrid(UserAppRole role, string id, string txtSearch, string userid, string appcd, string rlcd, string depcd, string brncd, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {

                UserService us = new UserService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();

                if (!string.IsNullOrWhiteSpace(role.AppCode))
                {
                    criteria.Add("AppCode", role.AppCode);
                }
                if (!string.IsNullOrWhiteSpace(role.RoleCode))
                {
                    criteria.Add("RoleCode", role.RoleCode);
                }
                if (!string.IsNullOrWhiteSpace(role.DeptCode))
                {
                    criteria.Add("DeptCode", role.DeptCode);
                }
                if (!string.IsNullOrWhiteSpace(role.BranchCode))
                {
                    criteria.Add("BranchCode", role.BranchCode);
                }
                if (!string.IsNullOrWhiteSpace(userid))
                {
                    criteria.Add("UserId", userid);
                }
                if ((!string.IsNullOrWhiteSpace(appcd)))
                {
                    if (appcd.Contains("Select"))
                    {

                    }
                    else
                    {
                        criteria.Add("AppCode", appcd);
                    }
                }
                if (!string.IsNullOrWhiteSpace(rlcd))
                {
                    if (rlcd.Contains("Select"))
                    {

                    }
                    else
                    {
                        criteria.Add("RoleCode", rlcd);
                    }
                }
                if (!string.IsNullOrWhiteSpace(depcd))
                {
                    if (depcd.Contains("Select"))
                    {

                    }
                    else
                    {
                        criteria.Add("DeptCode", depcd);
                    }
                }
                if (!string.IsNullOrWhiteSpace(brncd))
                {
                    if (brncd.Contains("Select"))
                    {

                    }
                    else
                    {
                        criteria.Add("BranchCode", brncd);
                    }
                }
                if (!string.IsNullOrWhiteSpace(sord) && sord == "desc")
                    sord = "Desc";
                else
                    sord = "Asc";
                Dictionary<long, IList<UserAppRole>> userapprole = us.GetAppRoleForAnUserListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (userapprole != null && userapprole.Count > 0)
                {
                    long totalrecords = userapprole.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in userapprole.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                               items.UserId,
                               items.AppCode,
                               items.RoleCode,
                               items.DeptCode,
                               items.BranchCode,
                               items.Email,
                               items.Id.ToString() 
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        #endregion

        #region New Ace Home
        public ActionResult NewHome()
        {
            Session["emailsent"] = "";
            //Session["campusname"] = "";
            Session["cmpnm"] = "";
            Session["grd"] = "";
            Session["sect"] = "";
            Session["acdyr"] = "";
            Session["apnam"] = "";
            Session["stats"] = "";
            Session["appno"] = "";
            Session["regno"] = "";
            Session["feestructyr"] = "";
            Session["ishosteller"] = "";
            Session["hostlr"] = "";
            Session["Promotioncamp"] = "";
            Session["idnum"] = "";
            Session["transfered"] = "";
            Session["transferedId"] = "";
            Session["transferedName"] = "";
            Session["pagename"] = "";
            Session["transferpdf"] = "";
            Session["discontinue"] = "";
            Session["discontinueName"] = "";
            Session["bonafidepdf"] = "";
            Session["promotion"] = "";
            Session["promotionId"] = "";
            Session["notpromotedpreregno"] = "";
            Session["ptcam"] = "";
            Session["ptgrd"] = "";
            Session["ptsect"] = "";
            Session["attachment"] = "";
            Session["attachmentnames"] = "";
            if (Session["UserId"].ToString() == "CHE-GRL")
            {
                Session["Promotioncamp"] = "CHENNAI";
            }
            if (Session["UserId"].ToString() == "TIR-GRL")
            {
                Session["Promotioncamp"] = "TIRUPUR";
            }
            if (Session["UserId"].ToString() == "ERN-GRL")
            {
                Session["Promotioncamp"] = "ERNAKULAM";
            }
            if (Session["UserId"].ToString() == "KAR-GRL")
            {
                Session["Promotioncamp"] = "KARUR";
            }
            if (Session["UserId"].ToString() == "GRL-ADMS")
            {
                Session["Promotioncamp"] = "IB MAIN";
            }
            AdminTemplateService ATS = new AdminTemplateService();
            StudentsReportService SRS = new StudentsReportService();

            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Clear();

            string sord = "";
            sord = sord == "desc" ? "Desc" : "Asc";
            string sidx = "Flag";
            List<CampusMaster> CampusList = new List<CampusMaster>();
            Dictionary<long, IList<CampusMaster>> CampusMasterList = SRS.GetCampusMasterListWithPaging(0, 9999, sidx, sord, criteria);
            CampusList = CampusMasterList.FirstOrDefault().Value.ToList();

            #region Student Count Code
            criteria.Clear();

            string AcademicYear = string.Empty;
            DateTime Currnt_Date = DateTime.Now;
            int Currnt_Year = Currnt_Date.Year;
            if (Currnt_Date.Month >= 5 && Currnt_Date.Month <= 12)
            {
                AcademicYear = Currnt_Year + "-" + (Currnt_Year + 1);
            }
            if (Currnt_Date.Month >= 1 && Currnt_Date.Month <= 5)
            {
                AcademicYear = (Currnt_Year - 1) + "-" + Currnt_Year;
            }
            criteria.Add("AcademicYear", AcademicYear);
            Dictionary<long, IList<AdminTemplateStudentTemplate_vw>> AdminTemplateStudents_vwDetails = null;
            string[] ColorCodes = { "0000FF", "8A2BE2", "A52A2A", "DEB887", "5F9EA0", "BDB76B", "8B008B", "556B2F", "FF8C00", "FF00FF", "191970", "CD853F", "FF0000", "708090", "008080", "9ACD32" };
            IList<AdminTemplateStudentTemplate_vw> CampusWiseStudentDetails = new List<AdminTemplateStudentTemplate_vw>();
            List<AdminTemplateStudentTemplate_vw> CampusStudDetailsList = new List<AdminTemplateStudentTemplate_vw>();
            AdminTemplateStudents_vwDetails = ATS.GetAdminTemplateStudentTemplate_vwListWithPagingAndCriteria(0, 999999, string.Empty, string.Empty, criteria);
            CampusStudDetailsList = AdminTemplateStudents_vwDetails.FirstOrDefault().Value.ToList();
            long StudentsCount = 0;
            if (CampusStudDetailsList != null && CampusStudDetailsList.Count > 0)
            {
                foreach (var CampusItem in CampusList)
                {
                    var CampusCount = (from u in CampusStudDetailsList
                                       where u.Campus == CampusItem.Name
                                       select u).ToList();
                    AdminTemplateStudentTemplate_vw Obj = new AdminTemplateStudentTemplate_vw();
                    Obj.Campus = CampusItem.Name;
                    if (CampusCount != null && CampusCount.Count > 0)
                    {
                        Obj.Count = Convert.ToInt64(CampusCount[0].Count);
                        StudentsCount = StudentsCount + Convert.ToInt64(CampusCount[0].Count);
                    }
                    else
                    {
                        Obj.Count = 0;
                    }
                    CampusWiseStudentDetails.Add(Obj);
                }
            }
            @ViewBag.StudentsCount = StudentsCount;
            @ViewBag.CampusList = CampusList;
            @ViewBag.CampusWiseStudentDetails = CampusWiseStudentDetails.ToList();
            //return Json(CampusWiseStudentDetails.ToList(), JsonRequestBehavior.AllowGet);
            #endregion

            #region Staff Count Code
            criteria.Clear();
            long StaffCount = 0;
            Dictionary<long, IList<AdminTemplateStaffDetails_vw>> AdminTemplateStaffDetails_vwDetails = null;
            List<AdminTemplateStaffDetails_vw> CampusStaffDetailsList = new List<AdminTemplateStaffDetails_vw>();
            IList<AdminTemplateStaffDetails_vw> CampusWiseStaffDetails = new List<AdminTemplateStaffDetails_vw>();
            AdminTemplateStaffDetails_vwDetails = ATS.GetAdminTemplateStaffDetails_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
            CampusStaffDetailsList = AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value.ToList();
            if (CampusStaffDetailsList != null && CampusStaffDetailsList.Count > 0)
            {
                foreach (var CampusItem in CampusList)
                {
                    var CampusCount = (from u in CampusStaffDetailsList
                                       where u.Campus == CampusItem.Name
                                       select u).ToList();
                    AdminTemplateStaffDetails_vw Obj = new AdminTemplateStaffDetails_vw();
                    Obj.Campus = CampusItem.Name;
                    if (CampusCount != null && CampusCount.Count > 0)
                    {
                        Obj.Count = Convert.ToInt64(CampusCount[0].Count);
                        StaffCount = StaffCount + Convert.ToInt64(CampusCount[0].Count);
                    }
                    else
                    {
                        Obj.Count = 0;
                    }
                    CampusWiseStaffDetails.Add(Obj);
                }
            }
            @ViewBag.StaffCount = StaffCount;
            @ViewBag.CampusWiseStaffDetails = CampusWiseStaffDetails.ToList();
            //return Json(CampusWiseStudentDetails.ToList(), JsonRequestBehavior.AllowGet);
            #endregion

            #region Users Count
            criteria.Clear();
            long UsersCount = 0;
            Dictionary<long, IList<AdminTemplateUsersReport_vw>> UserDetails = null;
            UserDetails = ATS.GetAdminTemplateUsersReport_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
            List<AdminTemplateUsersReport_vw> CampusUsersDetailsList = new List<AdminTemplateUsersReport_vw>();
            IList<AdminTemplateUsersReport_vw> CampusWiseUserDetails = new List<AdminTemplateUsersReport_vw>();
            CampusUsersDetailsList = UserDetails.FirstOrDefault().Value.ToList();
            if (CampusUsersDetailsList != null && CampusUsersDetailsList.Count > 0)
            {
                foreach (var CampusItem in CampusList)
                {
                    var CampusCount = (from u in CampusUsersDetailsList
                                       where u.Campus == CampusItem.Name
                                       select u).ToList();
                    AdminTemplateUsersReport_vw Obj = new AdminTemplateUsersReport_vw();
                    Obj.Campus = CampusItem.Name;
                    if (CampusCount != null && CampusCount.Count > 0)
                    {
                        Obj.Count = Convert.ToInt64(CampusCount[0].Count);
                        UsersCount = UsersCount + Convert.ToInt64(CampusCount[0].Count);
                    }
                    else
                    {
                        Obj.Count = 0;
                    }
                    CampusWiseUserDetails.Add(Obj);
                }
            }
            @ViewBag.UsersCount = UsersCount;
            @ViewBag.CampusWiseUserDetails = CampusWiseUserDetails.ToList();
            #endregion

            #region Transport Counts
            criteria.Clear();
            TransportBC TranBC = new TransportBC();
            long TransportCount = 0;
            Dictionary<long, IList<VehicleSubTypeMaster>> VehicleSubTypeMasterDetails = null;
            List<VehicleSubTypeMaster> VehicleSubTypeMasterData = new List<VehicleSubTypeMaster>();
            VehicleSubTypeMasterDetails = TranBC.GetVehicleSubTypeMasterListWithPagingAndCriteriaLikeSearch(null, 9999, string.Empty, string.Empty, criteria);
            List<VehicleSubTypeMaster> VehicleSubTypeMasterList = new List<VehicleSubTypeMaster>();
            if (VehicleSubTypeMasterDetails != null && VehicleSubTypeMasterDetails.Count > 0 && VehicleSubTypeMasterDetails.FirstOrDefault().Value.Count > 0)
            {
                foreach (var item in CampusList)
                {
                    VehicleSubTypeMaster VSM = new VehicleSubTypeMaster();
                    long TempCount = 0;
                    string Campus = string.Empty;
                    var TempList = (from u in VehicleSubTypeMasterDetails.FirstOrDefault().Value
                                    where u.Campus == item.Name
                                    select u).ToList();
                    if (TempList.Count == 0) { TempCount = 0; Campus = item.Name; }
                    else { TempCount = TempList.Count; Campus = TempList.FirstOrDefault().Campus; }
                    VSM.Campus = Campus;
                    VSM.ReportCount = TempCount;
                    VehicleSubTypeMasterData.Add(VSM);
                    TransportCount = TransportCount + TempCount;
                }
            }
            @ViewBag.TransportCount = TransportCount;
            @ViewBag.CampusWiseTransportDetails = VehicleSubTypeMasterData.ToList();
            #endregion

            #region Issue Code
            criteria.Clear();
            long IssuesTotal = 0;
            Dictionary<long, IList<AdminTemplateCampusWiseIssueCount_vw>> AdminTemplateCampusWiseIssueCount_vwDetails = null;
            List<AdminTemplateCampusWiseIssueCount_vw> CampusIssueDetailsList = new List<AdminTemplateCampusWiseIssueCount_vw>();
            IList<AdminTemplateCampusWiseIssueCount_vw> CampusWiseIssueDetails = new List<AdminTemplateCampusWiseIssueCount_vw>();
            AdminTemplateCampusWiseIssueCount_vwDetails = ATS.GetAdminTemplateCampusWiseIssueCount_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
            CampusIssueDetailsList = AdminTemplateCampusWiseIssueCount_vwDetails.FirstOrDefault().Value.ToList();
            if (CampusStaffDetailsList != null && CampusStaffDetailsList.Count > 0)
            {
                foreach (var CampusItem in CampusList)
                {
                    var CampusCount = (from u in CampusIssueDetailsList
                                       where u.Campus == CampusItem.Name
                                       select u).ToList();
                    AdminTemplateCampusWiseIssueCount_vw Obj = new AdminTemplateCampusWiseIssueCount_vw();
                    Obj.Campus = CampusItem.Name;
                    if (CampusCount != null && CampusCount.Count > 0)
                    {
                        Obj.Count = Convert.ToInt64(CampusCount[0].Count);
                        IssuesTotal = IssuesTotal + Convert.ToInt64(CampusCount[0].Count);
                    }
                    else
                    {
                        Obj.Count = 0;
                    }
                    CampusWiseIssueDetails.Add(Obj);
                }
            }
            @ViewBag.IssuesTotal = IssuesTotal;
            @ViewBag.CampusWiseIssueDetails = CampusWiseIssueDetails.ToList();
            //return Json(CampusWiseStudentDetails.ToList(), JsonRequestBehavior.AllowGet);
            #endregion

            return View();
        }
        public ActionResult CampusWiseCountsChart()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    List<CampusMaster> CampusList = new List<CampusMaster>();
                    string sord = "";
                    sord = sord == "desc" ? "Desc" : "Asc";
                    string sidx = "Flag";
                    StudentsReportService SRS = new StudentsReportService();
                    Dictionary<long, IList<CampusMaster>> CampusMasterList = SRS.GetCampusMasterListWithPaging(0, 9999, sidx, sord, criteria);
                    CampusList = CampusMasterList.FirstOrDefault().Value.ToList();

                    var CampusChart = "<graph caption='Campus Wise Students,Staffs and Users Counts Reports' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='0' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";
                    CampusChart = CampusChart + "<categories>";
                    foreach (var CampusNameItem in CampusList)
                    {
                        CampusChart = CampusChart + "<category name='" + CampusNameItem.Name + "' />";
                    }
                    CampusChart = CampusChart + "</categories>";

                    AdminTemplateService ATS = new AdminTemplateService();
                    criteria.Clear();
                    string AcademicYear = string.Empty;
                    DateTime Currnt_Date = DateTime.Now;
                    int Currnt_Year = Currnt_Date.Year;
                    if (Currnt_Date.Month >= 6 && Currnt_Date.Month <= 12)
                    {
                        AcademicYear = Currnt_Year + "-" + (Currnt_Year + 1);
                    }
                    if (Currnt_Date.Month >= 1 && Currnt_Date.Month <= 4)
                    {
                        AcademicYear = (Currnt_Year - 1) + "-" + Currnt_Year;
                    }
                    criteria.Add("AcademicYear", AcademicYear);
                    Dictionary<long, IList<AdminTemplateStudents_vw>> AdminTemplateStudents_vwDetails = null;
                    //string[] ColorCodes = { "0000FF", "8A2BE2", "A52A2A", "DEB887", "5F9EA0", "BDB76B", "8B008B", "556B2F", "FF8C00", "FF00FF", "191970", "CD853F", "FF0000", "708090", "008080", "9ACD32" };
                    IList<AdminTemplateStudents_vw> CampusWiseStudentDetails = new List<AdminTemplateStudents_vw>();
                    AdminTemplateStudents_vwDetails = ATS.GetAdminTemplateStudents_vwListWithPagingAndCriteria(null, 999999, string.Empty, string.Empty, criteria);
                    if (AdminTemplateStudents_vwDetails != null && AdminTemplateStudents_vwDetails.Count > 0 && AdminTemplateStudents_vwDetails.FirstOrDefault().Value.Count > 0)
                    {
                        List<AdminTemplateStudents_vw> StudentsData = new List<AdminTemplateStudents_vw>();
                        foreach (var CampusItem in CampusList)
                        {
                            AdminTemplateStudents_vw ST = new AdminTemplateStudents_vw();
                            long TempCount = 0;
                            string Campus = string.Empty;
                            var TempList = (from u in AdminTemplateStudents_vwDetails.FirstOrDefault().Value
                                            where u.Campus == CampusItem.Name
                                            select u).ToList();
                            if (TempList.Count == 0) { TempCount = 0; Campus = CampusItem.Name; }
                            else { TempCount = TempList.FirstOrDefault().Count; Campus = TempList.FirstOrDefault().Campus; }
                            ST.Campus = Campus;
                            ST.ReportCount = TempCount;
                            StudentsData.Add(ST);
                        }
                        CampusChart = CampusChart + " <dataset seriesname='Students' color='8A939D'>";
                        foreach (var Student in StudentsData)
                        {
                            CampusChart = CampusChart + "<set value='" + Student.ReportCount + "' />";
                        }
                        CampusChart = CampusChart + "</dataset>";
                    }

                    //For Staff Count
                    criteria.Clear();
                    Dictionary<long, IList<AdminTemplateStaffDetails_vw>> AdminTemplateStaffDetails_vwDetails = null;
                    //string[] ColorCodes = { "0000FF", "8A2BE2", "A52A2A", "DEB887", "5F9EA0", "BDB76B", "8B008B", "556B2F", "FF8C00", "FF00FF", "191970", "CD853F", "FF0000", "708090", "008080", "9ACD32" };
                    IList<AdminTemplateStaffDetails_vw> CampusWiseStaffDetails = new List<AdminTemplateStaffDetails_vw>();
                    AdminTemplateStaffDetails_vwDetails = ATS.GetAdminTemplateStaffDetails_vwListWithPagingAndCriteria(null, 999999, string.Empty, string.Empty, criteria);
                    if (AdminTemplateStaffDetails_vwDetails != null && AdminTemplateStaffDetails_vwDetails.Count > 0 && AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value.Count > 0)
                    {
                        List<AdminTemplateStaffDetails_vw> StaffData = new List<AdminTemplateStaffDetails_vw>();
                        foreach (var CampusItem in CampusList)
                        {
                            AdminTemplateStaffDetails_vw Staff = new AdminTemplateStaffDetails_vw();
                            long TempCount = 0;
                            string Campus = string.Empty;
                            var TempList = (from u in AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value
                                            where u.Campus == CampusItem.Name
                                            select u).ToList();
                            if (TempList.Count == 0) { TempCount = 0; Campus = CampusItem.Name; }
                            else { TempCount = TempList.FirstOrDefault().Count; Campus = TempList.FirstOrDefault().Campus; }
                            Staff.Campus = Campus;
                            Staff.ReportCount = TempCount;
                            StaffData.Add(Staff);
                        }
                        CampusChart = CampusChart + " <dataset seriesname='Staff' color='E5E8ED'>";
                        foreach (var StaffItems in StaffData)
                        {
                            CampusChart = CampusChart + "<set value='" + StaffItems.ReportCount + "' />";
                        }
                        CampusChart = CampusChart + "</dataset>";
                    }


                    //For Users Count
                    criteria.Clear();
                    Dictionary<long, IList<AdminTemplateUsersReport_vw>> AdminTemplateUsersReport_vwDetails = null;
                    //string[] ColorCodes = { "0000FF", "8A2BE2", "A52A2A", "DEB887", "5F9EA0", "BDB76B", "8B008B", "556B2F", "FF8C00", "FF00FF", "191970", "CD853F", "FF0000", "708090", "008080", "9ACD32" };
                    IList<AdminTemplateUsersReport_vw> CampusWiseUserDetails = new List<AdminTemplateUsersReport_vw>();
                    AdminTemplateUsersReport_vwDetails = ATS.GetAdminTemplateUsersReport_vwListWithPagingAndCriteria(null, 999999, string.Empty, string.Empty, criteria);
                    if (AdminTemplateUsersReport_vwDetails != null && AdminTemplateUsersReport_vwDetails.Count > 0 && AdminTemplateUsersReport_vwDetails.FirstOrDefault().Value.Count > 0)
                    {
                        List<AdminTemplateUsersReport_vw> UserData = new List<AdminTemplateUsersReport_vw>();
                        foreach (var CampusItem in CampusList)
                        {
                            AdminTemplateUsersReport_vw User = new AdminTemplateUsersReport_vw();
                            long TempCount = 0;
                            string Campus = string.Empty;
                            var TempList = (from u in AdminTemplateUsersReport_vwDetails.FirstOrDefault().Value
                                            where u.Campus == CampusItem.Name
                                            select u).ToList();
                            if (TempList.Count == 0) { TempCount = 0; Campus = CampusItem.Name; }
                            else { TempCount = TempList.FirstOrDefault().Count; Campus = TempList.FirstOrDefault().Campus; }
                            User.Campus = Campus;
                            User.ReportCount = TempCount;
                            UserData.Add(User);
                        }
                        CampusChart = CampusChart + " <dataset seriesname='Users' color='53004B' parentyaxis='S' renderas='Line'>";
                        foreach (var UsersItem in UserData)
                        {
                            CampusChart = CampusChart + "<set value='" + UsersItem.ReportCount + "' />";
                        }
                        CampusChart = CampusChart + "</dataset>";
                    }



                    CampusChart = CampusChart + "</graph>";
                    Response.Write(CampusChart);
                    return null;

                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult CallmanagementIssueCountbyIssueGroupChart()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region code
                    CallManagementService cms = new CallManagementService();
                    Dictionary<string, object> Criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<CallManagementChart>> GetIssueCountByIssueGroup = cms.GetCallManagementListWithPagingChart(null, 9999, string.Empty, string.Empty, Criteria);
                    var IssueCount = (from u in GetIssueCountByIssueGroup.First().Value
                                      select u).ToList();
                    long aca = 0;
                    long admn = 0;
                    long adms = 0;
                    long fees = 0;
                    long hstl = 0;
                    long hr = 0;
                    long stre = 0;
                    long trns = 0;
                    long it = 0;
                    long rcpt = 0;
                    foreach (var itemdata in IssueCount)
                    {
                        if (itemdata.IssueGroup == "Academics")
                        {
                            aca++;
                        }
                        else if (itemdata.IssueGroup == "Administrative")
                        {
                            admn++;
                        }
                        else if (itemdata.IssueGroup == "Admission")
                        {
                            adms++;
                        }
                        else if (itemdata.IssueGroup == "Fees / Finance")
                        {
                            fees++;
                        }
                        else if (itemdata.IssueGroup == "Hostel")
                        {
                            hstl++;
                        }
                        else if (itemdata.IssueGroup == "HR")
                        {
                            hr++;
                        }
                        else if (itemdata.IssueGroup == "Store")
                        {
                            stre++;
                        }
                        else if (itemdata.IssueGroup == "Transport")
                        {
                            trns++;
                        }
                        else if (itemdata.IssueGroup == "IT")
                        {
                            it++;
                        }
                        else if (itemdata.IssueGroup == "Reception")
                        {
                            rcpt++;
                        }
                        else
                        { }
                    }
                    var DepartmentChart = "<graph caption='Callmanagement Issue Count by Issue Group Chart' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";
                    DepartmentChart = DepartmentChart + " <set name='Academics' value='" + aca + "' color='AFD8F8' />";
                    DepartmentChart = DepartmentChart + " <set name='Administrative' value='" + admn + "' color='F6BD0F' />";
                    DepartmentChart = DepartmentChart + " <set name='Admission' value='" + adms + "' color='8BBA00' />";
                    DepartmentChart = DepartmentChart + " <set name='Fees / Finance' value='" + fees + "' color='FF8E46' />";
                    DepartmentChart = DepartmentChart + " <set name='Hostel' value='" + hstl + "' color='08E8E' />";
                    DepartmentChart = DepartmentChart + " <set name='HR' value='" + hr + "' color='D64646' />";
                    DepartmentChart = DepartmentChart + " <set name='Store' value='" + stre + "' color='8BBA00' />";
                    DepartmentChart = DepartmentChart + " <set name='Transport' value='" + trns + "' color='FF8E46' />";
                    DepartmentChart = DepartmentChart + " <set name='IT' value='" + it + "' color='08E8E' />";
                    DepartmentChart = DepartmentChart + " <set name='Reception' value='" + rcpt + "' color='D64646' />";
                    DepartmentChart = DepartmentChart + "</graph>";
                    Response.Write(DepartmentChart);
                    return null;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }

        }
        #endregion

        public void UpdateInbox(string Campus, string issue, string userId, long Id, string Status)
        {
            InboxService IBS = new InboxService();
            Inbox In = new Inbox();
            In.Campus = Campus;
            In.UserId = userId;
            In.InformationFor = issue;
            In.CreatedDate = DateTime.Now;
            In.Module = "Call Management";
            In.Status = "Inbox";
            In.Campus = Campus;
            In.PreRegNum = Id;
            In.RefNumber = Id;
            IBS.CreateOrUpdateInbox(In);
        }

        public ActionResult ChangeofIssueGroup(long ActivityId, string Id, string IssueGroup, string IssueType)
        {
            ProcessFlowServices pfs = new ProcessFlowServices();
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    string oldIssueGroup = string.Empty;
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    CallManagementService cms = new CallManagementService();
                    CallManagement cm = cms.GetCallManagementById(Convert.ToInt64(Id));
                    oldIssueGroup = cm.IssueGroup;
                    cm.IssueGroup = IssueGroup;
                    cm.IssueType = IssueType;
                    cm.DeptCode = IssueGroup.ToUpper();
                    criteria.Add("ProcessRefId", cm.Id);
                    criteria.Add("InstanceId", cm.InstanceId);
                    Dictionary<long, IList<Activity>> Activitylist = cms.GetCallManagementActivityList(0, 50, string.Empty, string.Empty, criteria);
                    if (Activitylist != null && Activitylist.Count > 0 && Activitylist.FirstOrDefault().Key > 0)
                    {
                        IList<Activity> actlist = Activitylist.FirstOrDefault().Value;
                        foreach (var item in actlist)
                        {
                            item.DeptCode = IssueGroup.ToUpper();
                            pfs.CreateActivity(item);
                        }
                    }
                    cms.CreateOrUpdateCallManagement(cm);
                    Activity Ac = pfs.GetActivityById(ActivityId);
                    Ac.Available = true;
                    Ac.Assigned = false;
                    Ac.Performer = null;
                    pfs.CreateActivity(Ac);
                    //sending mail part

                    UserService us = new UserService();
                    criteria.Clear();
                    criteria.Add("AppCode", "ISM");
                    string recepient = cm.Email;
                    Dictionary<long, IList<UserAppRole_Vw>> UserAppRoleList = us.GetAppRoleOnlyActiveUsersPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    string[] ResolverEmail = (from u in UserAppRoleList.First().Value
                                              where u.DeptCode == IssueGroup.ToUpper() && u.RoleCode == "GRL" && u.BranchCode == cm.BranchCode && u.Email != null && u.AppCode == "ISM"
                                              select u.Email).Distinct().ToArray();
                    string ResolverMailId = ResolverEmail != null && ResolverEmail.Length > 0 ? ResolverEmail[0].ToString() : string.Empty;
                    string[] Rejcmt = null;
                    ActorsEmail(cm, "", ResolverEmail, recepient, cm.IssueNumber, cm.InformationFor, cm.StudentName, oldIssueGroup, cm.CallPhoneNumber, "", "ChangeofIssueGroup", Rejcmt, "", cm.LeaveType);
                    //return RedirectToAction("CallManagement");
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
            finally
            {
                pfs = null;
            }
        }

        public ActionResult SampleImgUploadCorp()
        {
            return View();
        }

        public ActionResult CallManagementInStudentManagmentJqgrid(string NewId, int? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    // if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                    // criteria.Add("StudentName", Name);
                    criteria.Add("StudentNumber", NewId);
                    sord = sord == "desc" ? "Desc" : "Asc";
                    WidgetService pfs = new WidgetService();
                    //Assess360Controller As = new Assess360Controller();
                    Dictionary<long, IList<CallManagementView>> CallmanagementDetails = pfs.GetIssueDetailsListWithPaging(page - 1, rows, sord, sidx, criteria);
                    if (CallmanagementDetails != null && CallmanagementDetails.Count > 0)
                    {
                        long totalrecords = CallmanagementDetails.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var CallmanagementList = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in CallmanagementDetails.First().Value
                                    select new
                                    {
                                        cell = new string[] {
                                              items.Id.ToString(),
                                items.CallFrom,
                                items.CallerName,
                                items.IssueNumber,
                                items.IssueType,
                                items.IssueGroup,
                                items.IssueDate.Value.ToString("dd/MM/yyyy"),
                                items.Description,
                                items.Resolution,
                                items.Status,
                                items.Approver
                                            }
                                    })
                        };
                        return Json(CallmanagementList, JsonRequestBehavior.AllowGet);
                    }

                    else return Json(null);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;

            }
        }
        #region Added By Prabakaran for Call Management
        public ActionResult CallManagementHistory()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public ActionResult CallManagementHistoryJqGrid(string Campus, string Grade, string Section, string IssueNumber, string IssueGroup, string IssueType, string StudentName, string StudentNumber, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    CallManagementService cms = new CallManagementService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<string, object> likecriteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(Campus))
                    {
                        criteria.Add("School", Campus);
                    }
                    criteria.Add("ActionName", "Deleted");
                    if (!string.IsNullOrEmpty(Grade))
                    { criteria.Add("Grade", Grade); }
                    if (!string.IsNullOrEmpty(Section))
                    { criteria.Add("Section", Section); }
                    if (!string.IsNullOrEmpty(IssueNumber))
                    { criteria.Add("IssueNumber", IssueNumber); }
                    if (!string.IsNullOrEmpty(IssueGroup))
                    { criteria.Add("IssueGroup", IssueGroup); }
                    if (!string.IsNullOrEmpty(IssueType))
                    { criteria.Add("IssueType", IssueType); }
                    if (!string.IsNullOrEmpty(StudentName))
                    { criteria.Add("StudentName", StudentName); }
                    if (!string.IsNullOrEmpty(StudentNumber))
                    { criteria.Add("StudentNumber", StudentNumber); }
                    //if (!string.IsNullOrEmpty(IssueDate))
                    //{ likecriteria.Add("IssueDate", DateTime.ParseExact(IssueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)); }
                    //if (!string.IsNullOrEmpty(DeletedDate))
                    //{ likecriteria.Add("TriggerDate", DateTime.ParseExact(DeletedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)); }
                    Dictionary<long, IList<CallManagementHistory>> CallManagementHistoryList = cms.GetCallManagementHistoryListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria, likecriteria);
                    if (CallManagementHistoryList != null && CallManagementHistoryList.Count > 0)
                    {
                        long totalrecords = CallManagementHistoryList.First().Key;
                        int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat = new
                        {
                            total = totalpages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in CallManagementHistoryList.FirstOrDefault().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[]{
                                            items.CallManagement_TriggerId.ToString(),             
                                            items.Id.ToString(),
                                            items.IssueNumber,
                                            items.Description,
                                            items.UserInbox,
                                            items.InformationFor,
                                            items.IssueGroup,
                                            items.IssueType,
                                            items.Status,
                                            items.ActivityFullName,
                                            items.IssueDate.ToString("dd/MM/yyyy"),
                                            items.School,
                                            items.StudentName,
                                            items.Grade,
                                            items.Section,
                                            items.IsHosteller.ToString(),
                                            items.Email,
                                            items.IsIssueCompleted.ToString(),
                                            items.CallPhoneNumber,
                                            items.CallFrom,
                                            items.CallerName,
                                            items.StudentNumber,
                                            items.StudentType,
                                            items.Receiver,
                                            items.ReceiverGroup,
                                            items.Approver,
                                            items.InstanceId.ToString(),
                                            items.UserRoleName,
                                            items.Resolution,
                                            items.IsInformation.ToString(),
                                            items.BranchCode,
                                            items.DeptCode,
                                            items.WaitingForParentCnfm.ToString(),
                                            items.LeaveType,
                                            items.ActionDate.ToString(),
                                            items.Performer,
                                            items.UserType,
                                            items.BoardingType,
                                            items.ActionName,
                                            items.TriggerDate!=null?items.TriggerDate.Value.ToString("dd/MM/yyyy"):"",
                                            items.DeleteComments,
                                            items.RevertDate!=null?items.RevertDate.Value.ToString("dd/MM/yyyy"):"",
                                            items.RevertComments,
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

            }
            catch (Exception ex)
            {

                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public ActionResult DeleteCallManagement()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {

                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public ActionResult DeleteStudentCallManagement(string IssueNumber, string DeleteComments)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    CallManagementService cms = new CallManagementService();
                    CallManagement callmgmnt = cms.GetCallManagementByIssueNumber(IssueNumber);

                    if (callmgmnt != null)
                    {
                        if (callmgmnt.Id > 0)
                        {
                            cms.DeleteCallManagement(callmgmnt.Id);
                            CallManagement callmgment = cms.GetCallManagementById(callmgmnt.Id);
                            if (callmgment == null)
                            {
                                CallManagementHistory callmgmnthstry = cms.GetCallManagementHistoryByCallManagementId(callmgmnt.Id);
                                callmgmnthstry.DeleteComments = DeleteComments;
                                cms.CreateOrUpdateCallManagementHistory(callmgmnthstry);
                                return Json("success", JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json("failed", JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json("failed", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json("IssueNofailed", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public JsonResult IssueNumberAutoComplete(string term)
        {
            try
            {

                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return null;
                else
                {
                    CallManagementService cms = new CallManagementService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    //if (!string.IsNullOrEmpty(Campus))
                    //{
                    //    criteria.Add("School", Campus);
                    //}
                    if (!string.IsNullOrEmpty(term))
                    {
                        criteria.Add("IssueNumber", term);
                    }
                    Dictionary<long, IList<CallManagement>> callmanagement = null;
                    callmanagement = cms.GetCallManagementListWithPaging(0, 999999, null, null, criteria);
                    if (callmanagement != null && callmanagement.FirstOrDefault().Key >= 1)
                    {
                        var IssueNumber = (from items in callmanagement.FirstOrDefault().Value
                                           select new
                                           {

                                               IssueNumber = items.IssueNumber

                                           }).Distinct().ToList();
                        return Json(IssueNumber, JsonRequestBehavior.AllowGet);
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public JsonResult CallMgmntHistoryIssueNumberAutoComplete(string term, string Campus)
        {
            try
            {

                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return null;
                else
                {
                    CallManagementService cms = new CallManagementService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<string, object> likecriteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(Campus))
                    {
                        criteria.Add("School", Campus);
                    }
                    if (!string.IsNullOrEmpty(term))
                    {
                        likecriteria.Add("IssueNumber", term);
                    }
                    criteria.Add("ActionName", "Deleted");
                    Dictionary<long, IList<CallManagementHistory>> callmanagement = null;

                    callmanagement = cms.GetCallManagementHistoryListWithPagingAndCriteria(0, 999999, null, null, criteria, likecriteria);
                    if (callmanagement != null && callmanagement.FirstOrDefault().Key >= 1)
                    {
                        var IssueNumber = (from items in callmanagement.FirstOrDefault().Value
                                           select new
                                           {

                                               IssueNumber = items.IssueNumber

                                           }).Distinct().ToList();
                        return Json(IssueNumber, JsonRequestBehavior.AllowGet);
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public ActionResult EditCallManagementHistory(CallManagementHistory callMgmntHistory)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return null;
                else
                {
                    var JsonMsg = string.Empty;
                    CallManagementService cms = new CallManagementService();
                    if (callMgmntHistory != null)
                    {
                        if (callMgmntHistory.CallManagement_TriggerId > 0)
                        {
                            CallManagementHistory callmgmnthstry = cms.GetCallManagementHistoryById(callMgmntHistory.CallManagement_TriggerId);
                            if (callmgmnthstry.CallManagement_TriggerId > 0)
                            {
                                callmgmnthstry.DeleteComments = callMgmntHistory.DeleteComments;
                                JsonMsg = "success";
                                if (callMgmntHistory.RevertComments != null)
                                {
                                    callmgmnthstry.RevertComments = callMgmntHistory.RevertComments;
                                    callmgmnthstry.ActionName = "Reverted";
                                    callmgmnthstry.RevertDate = DateTime.Now;
                                    CallManagement callmgmnt = new CallManagement();
                                    callmgmnt.IssueNumber = callmgmnthstry.IssueNumber;
                                    callmgmnt.Description = callmgmnthstry.Description;
                                    callmgmnt.UserInbox = callmgmnthstry.UserInbox;
                                    callmgmnt.IssueType = callmgmnthstry.IssueType;
                                    callmgmnt.Status = callmgmnthstry.Status;
                                    callmgmnt.StudentName = callmgmnthstry.StudentName;
                                    callmgmnt.IssueDate = callmgmnthstry.IssueDate;
                                    callmgmnt.StudentNumber = callmgmnthstry.StudentNumber;
                                    callmgmnt.StudentType = callmgmnthstry.StudentType;
                                    callmgmnt.School = callmgmnthstry.School;
                                    callmgmnt.InstanceId = callmgmnthstry.InstanceId;
                                    callmgmnt.Email = callmgmnthstry.Email;
                                    callmgmnt.UserRoleName = callmgmnthstry.UserRoleName;
                                    callmgmnt.Resolution = callmgmnthstry.Resolution;
                                    callmgmnt.IsInformation = callmgmnthstry.IsInformation;
                                    callmgmnt.Grade = callmgmnthstry.Grade;
                                    callmgmnt.InformationFor = callmgmnthstry.InformationFor;
                                    callmgmnt.IsHosteller = callmgmnthstry.IsHosteller;
                                    callmgmnt.BranchCode = callmgmnthstry.BranchCode;
                                    callmgmnt.DeptCode = callmgmnthstry.DeptCode;
                                    callmgmnt.WaitingForParentCnfm = callmgmnthstry.WaitingForParentCnfm;
                                    callmgmnt.IsIssueCompleted = callmgmnthstry.IsIssueCompleted;
                                    callmgmnt.ActivityFullName = callmgmnthstry.ActivityFullName;
                                    callmgmnt.LeaveType = callmgmnthstry.LeaveType;
                                    callmgmnt.ActionDate = callmgmnthstry.ActionDate;
                                    callmgmnt.Performer = callmgmnthstry.Performer;
                                    callmgmnt.UserType = callmgmnthstry.UserType;
                                    callmgmnt.BoardingType = callmgmnthstry.BoardingType;
                                    callmgmnt.CallPhoneNumber = callmgmnthstry.CallPhoneNumber;
                                    callmgmnt.CallFrom = callmgmnthstry.CallFrom;
                                    callmgmnt.CallerName = callmgmnthstry.CallerName;
                                    callmgmnt.Section = callmgmnthstry.Section;
                                    callmgmnt.IssueGroup = callmgmnthstry.IssueGroup;
                                    callmgmnt.Receiver = callmgmnthstry.Receiver;
                                    callmgmnt.ReceiverGroup = callmgmnthstry.ReceiverGroup;
                                    callmgmnt.Approver = callmgmnthstry.Approver;
                                    cms.CreateOrUpdateCallManagement(callmgmnt);
                                    if (callmgmnt.Id > 0)
                                    {
                                        IList<Activity> activity = cms.GetActivityByProcessRefId(callmgmnthstry.InstanceId);
                                        if (activity != null)
                                        {
                                            foreach (var items in activity)
                                            {
                                                items.ProcessRefId = callmgmnt.Id;
                                                cms.CreateOrUpdateActivity(items);
                                            }
                                        }
                                        DocumentsService ds = new DocumentsService();
                                        IList<Documents> d = ds.GetDocumentsByEntityRefId(callMgmntHistory.Id);
                                        if (d != null)
                                        {
                                            foreach (var items in d)
                                            {
                                                if (items.AppName == "CMS")
                                                {
                                                    items.EntityRefId = callmgmnt.Id;
                                                    ds.CreateOrUpdateDocuments(items);
                                                }

                                            }
                                        }
                                        CommentsService cs = new CommentsService();
                                        Comments comments = cs.GetCommentsById(callMgmntHistory.Id);
                                        if (comments != null)
                                        {
                                            if (comments.AppName == "CMS")
                                            {
                                                comments.EntityRefId = callmgmnt.Id;
                                                cs.CreateOrUpdateComments(comments);
                                            }
                                        }
                                        JsonMsg = "successrevert";
                                    }

                                }
                                cms.CreateOrUpdateCallManagementHistory(callmgmnthstry);
                                return Json(JsonMsg, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                JsonMsg = "failed";
                                return Json(JsonMsg, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            JsonMsg = "failed";
                            return Json(JsonMsg, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        JsonMsg = "failed";
                        return Json(JsonMsg, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {

                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        #endregion

        //#region Added By Prabakaran
        //public void ActorsEmail(StaffIssues si, string[] ResolverEmail, string IssueNumber, string InformationFor, string IssueGroup, string Status, string AssignedBy)
        //{
        //    try
        //    {
        //        string SendEmail = ConfigurationManager.AppSettings["SendEmail1"];
        //        string From = ConfigurationManager.AppSettings["From"];
        //        if (SendEmail == "false")
        //            return;
        //        else
        //        {
        //            try
        //            {
        //                IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(si.BranchCode, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
        //                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
        //                string Body = string.Empty;
        //                if (Status == "ResolveIssue")
        //                {
        //                    StaffIssuesService sis = new StaffIssuesService();
        //                    StaffDetails SD = sis.GetStaffDetailsByUserId(si.CreatedBy);
        //                    string name = "";
        //                    if (SD == null) name = SD.Name;
        //                    else name = si.CreatedBy;
        //                    for (int i = 0; i < ResolverEmail.Length; i++)
        //                    {
        //                        if (ResolverEmail[i] != "")
        //                            mail.To.Add(ResolverEmail[i]);
        //                    }
        //                    mail.Subject = "Tips support request #" + IssueNumber + " needs your action ";
        //                    Body = "Dear Sir/Madam, <br/><br/>" +
        //                               "Tips support request #" + IssueNumber + " needs your action <br/><br/>" +
        //                                "<b>Issue Description:</b><br/>" +
        //                               "" + si.Description + "<br/><br/>" +
        //                               "<b>Issue Raised By:</b><br/>" +
        //                               "   " + name + "<br/><br/><br/>" +
        //                               "The issue " + IssueNumber + "  is assigned for your closing and please resolve this issue asap.<br/><br/>" + "<b>Issue Assigend By:</b><br/>" + " " + AssignedBy + "<br/>";
        //                }
        //                mail.Body = Body;
        //                EmailHelper emh = new EmailHelper();
        //                emh.SendEmailWithEmailTemplate(mail, si.BranchCode, GetGeneralBodyofMail());
        //            }
        //            catch (Exception)
        //            {

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
        //        throw ex;

        //    }
        //}
        //#endregion
        #region IssueCountReportDetails
        public ActionResult IssueCountReportDetails()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
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
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult GetIssueCountReportByCampusJqGrid(string Campus, string FromDate, string ToDate, string ExprtToExcel, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                sord = sord == "desc" ? "Desc" : "Asc";
                CallManagementService cms = new CallManagementService();
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                DateTime? FromDateNew = new DateTime();
                DateTime? ToDatenew = new DateTime();
                if (!string.IsNullOrEmpty(FromDate))
                {
                    FromDate = FromDate + " " + "00:00:00";
                    FromDateNew = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    if (string.IsNullOrEmpty(ToDate))
                    {
                        ToDatenew = DateTime.Now;
                    }
                    if (!string.IsNullOrEmpty(ToDate))
                    {
                        ToDate = ToDate + " " + "23:59:59";
                        ToDatenew = DateTime.Parse(ToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    }
                }
                else
                {
                    FromDateNew = null;
                    ToDatenew = null;
                }
                Dictionary<long, IList<IssueCountReportByCampus_SP>> IssueCountReportByCampusList = cms.GetIssueCountReportByCampus_SPList(Campus, FromDateNew, ToDatenew);
                if (IssueCountReportByCampusList == null || IssueCountReportByCampusList.FirstOrDefault().Key == 0)
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    IList<IssueCountReportByCampus_SP> IssueCountRepList = new List<IssueCountReportByCampus_SP>();
                    if (sord == "Desc")
                    {
                        IssueCountRepList = (from u in IssueCountReportByCampusList.FirstOrDefault().Value select u).OrderByDescending(x => x.GetType().GetProperty(sidx).GetValue(x, null)).ToList();
                    }
                    else
                    {
                        IssueCountRepList = (from u in IssueCountReportByCampusList.FirstOrDefault().Value select u).OrderBy(x => x.GetType().GetProperty(sidx).GetValue(x, null)).ToList();
                    }
                    if (ExprtToExcel == "Excel")
                    {
                        base.ExptToXL(IssueCountRepList, "IssueCountReportByCampus" + "-On-" + DateTime.Today.ToString("dd/MM/yyyy"), (item => new
                        {
                            Campus = item.BranchCode,
                            item.Logged,
                            item.Completed,
                            item.NonCompleted,
                            item.ResolveIssue,
                            item.ApproveIssue,
                            item.Complete,
                        }));
                        return new EmptyResult();
                    }
                    else
                    {
                        //IList<TcRequestReasonByCampus_Vw> TcReportByReasonList = tcRequestReportByReasonList.FirstOrDefault().Value;
                        long totalRecords = IssueCountRepList.Count;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows =
                            (from items in IssueCountRepList
                             select new
                             {
                                 i = items.Id,
                                 cell = new string[]
                           {
                                  items.Id.ToString(),
                                  items.BranchCode,
                                  items.Logged.ToString(),
                                  items.Completed.ToString(),
                                  items.NonCompleted.ToString(),
                                  items.ResolveIssue.ToString(),
                                  items.ApproveIssue.ToString(),
                                  items.Complete.ToString(),
                           }
                             })
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult GetIssueCountReportByIssueGroupJqGrid(string Campus, string IssueGroup, string FromDate, string ToDate, string ExprtToExcel, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                sord = sord == "desc" ? "Desc" : "Asc";
                CallManagementService cms = new CallManagementService();
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                DateTime? FromDateNew = new DateTime();
                DateTime? ToDatenew = new DateTime();
                if (!string.IsNullOrEmpty(FromDate))
                {
                    FromDate = FromDate + " " + "00:00:00";
                    FromDateNew = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    if (string.IsNullOrEmpty(ToDate))
                    {
                        ToDatenew = DateTime.Now;
                    }
                    if (!string.IsNullOrEmpty(ToDate))
                    {
                        ToDate = ToDate + " " + "23:59:59";
                        ToDatenew = DateTime.Parse(ToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    }
                }
                else
                {
                    FromDateNew = null;
                    ToDatenew = null;
                }
                Dictionary<long, IList<IssueCountReportByIssueGroup_SP>> IssueCountReportByIssueGroupList = cms.GetIssueCountReportByIssueGroup_SPList(Campus, IssueGroup, FromDateNew, ToDatenew);
                IList<IssueCountReportByIssueGroup_SP> IssueCountRepList = new List<IssueCountReportByIssueGroup_SP>();
                if (sord == "Desc")
                {
                    IssueCountRepList = (from u in IssueCountReportByIssueGroupList.FirstOrDefault().Value select u).OrderByDescending(x => x.GetType().GetProperty(sidx).GetValue(x, null)).ToList();
                }
                else
                {
                    IssueCountRepList = (from u in IssueCountReportByIssueGroupList.FirstOrDefault().Value select u).OrderBy(x => x.GetType().GetProperty(sidx).GetValue(x, null)).ToList();
                }
                if (ExprtToExcel == "Excel")
                {
                    base.ExptToXL(IssueCountRepList, "IssueCountReportByIssueGroup" + "-On-" + DateTime.Today.ToString("dd/MM/yyyy"), (item => new
                    {
                        Campus = item.BranchCode,
                        item.IssueGroup,
                        item.Logged,
                        item.Completed,
                        item.NonCompleted,
                        item.ResolveIssue,
                        item.ApproveIssue,
                        item.Complete,
                    }));
                    return new EmptyResult();
                }
                else if (IssueCountReportByIssueGroupList == null || IssueCountReportByIssueGroupList.FirstOrDefault().Key == 0)
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //IList<TcRequestReasonByCampus_Vw> TcReportByReasonList = tcRequestReportByReasonList.FirstOrDefault().Value;
                    long totalRecords = IssueCountRepList.Count;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows =
                        (from items in IssueCountRepList
                         select new
                         {
                             i = items.Id,
                             cell = new string[]
                           {
                                  items.Id.ToString(),
                                  items.BranchCode,
                                  items.IssueGroup,
                                  items.Logged.ToString(),
                                  items.Completed.ToString(),
                                  items.NonCompleted.ToString(),
                                  items.ResolveIssue.ToString(),
                                  items.ApproveIssue.ToString(),
                                  items.Complete.ToString(),
                           }
                         })
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        #endregion
        #region PerformerWiseCountReport
        public ActionResult PerformerWiseIssueCountReport()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
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
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult GetPerformerWiseIssueCountReportJqGrid(string BranchCode, string Performer, string FromDate, string ToDate, string ExprtToExcel, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                sord = sord == "desc" ? "Desc" : "Asc";
                CallManagementService cms = new CallManagementService();
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                DateTime? FromDateNew = new DateTime();
                DateTime? ToDatenew = new DateTime();
                if (!string.IsNullOrEmpty(FromDate))
                {
                    FromDate = FromDate + " " + "00:00:00";
                    FromDateNew = DateTime.Parse(FromDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    if (string.IsNullOrEmpty(ToDate))
                    {
                        ToDatenew = DateTime.Now;
                    }
                    if (!string.IsNullOrEmpty(ToDate))
                    {
                        ToDate = ToDate + " " + "23:59:59";
                        ToDatenew = DateTime.Parse(ToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    }
                }
                else
                {
                    FromDateNew = null;
                    ToDatenew = null;
                }
                Dictionary<long, IList<PerformerWiseIssueCountReport_SP>> PerformerWiseIssueCountReportList = cms.GetPerformerWiseIssueCountReport_SPList(BranchCode, Performer, FromDateNew, ToDatenew);
                IList<PerformerWiseIssueCountReport_SP> PerformerWiseIssueCountList = new List<PerformerWiseIssueCountReport_SP>();
                if (sord == "Desc" && PerformerWiseIssueCountReportList != null || PerformerWiseIssueCountReportList.FirstOrDefault().Key > 0)
                {
                    PerformerWiseIssueCountList = (from u in PerformerWiseIssueCountReportList.FirstOrDefault().Value select u).OrderByDescending(x => x.GetType().GetProperty(sidx).GetValue(x, null)).ToList();
                }
                else
                {
                    if (PerformerWiseIssueCountReportList != null || PerformerWiseIssueCountReportList.FirstOrDefault().Key > 0)
                    {
                        PerformerWiseIssueCountList = (from u in PerformerWiseIssueCountReportList.FirstOrDefault().Value select u).OrderBy(x => x.GetType().GetProperty(sidx).GetValue(x, null)).ToList();
                    }
                }
                if (ExprtToExcel == "Excel")
                {
                    base.ExptToXL(PerformerWiseIssueCountList, "PerformerWiseIssueCountReport" + "-On-" + DateTime.Today.ToString("dd/MM/yyyy"), (item => new
                    {
                        item.BranchCode,
                        item.Performer,
                        item.Assigned,
                        item.Resolved,
                        item.Completed,
                        item.Rejected,
                    }));
                    return new EmptyResult();
                }
                else if (PerformerWiseIssueCountReportList == null || PerformerWiseIssueCountReportList.FirstOrDefault().Key == 0)
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //IList<TcRequestReasonByCampus_Vw> TcReportByReasonList = tcRequestReportByReasonList.FirstOrDefault().Value;
                    long totalRecords = PerformerWiseIssueCountList.Count;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows =
                        (from items in PerformerWiseIssueCountList
                         select new
                         {
                             i = items.Id,
                             cell = new string[]
                           {
                                  items.Id.ToString(),
                                  items.BranchCode,
                                  items.Performer,
                                  items.Assigned.ToString(),
                                  items.Resolved.ToString(),                            
                                  items.Completed.ToString(),
                                  items.Rejected.ToString(),  
                           }
                         })
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        #endregion
        public ActionResult getTktPerformerList()
        {
            try
            {
                UserService uS = new UserService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("AppCode", "TKT");
                Dictionary<long, IList<UserAppRole>> PerformerList = uS.GetPerformerListWithCriteria(0, 1000, string.Empty, string.Empty, criteria);
                if (PerformerList != null && PerformerList.First().Value != null && PerformerList.First().Value.Count > 0)
                {
                    var PerformerName = (
                             from items in PerformerList.First().Value

                             select new
                             {
                                 Text = items.UserId,
                                 Value = items.UserId
                             }).Distinct().ToList();

                    return Json(PerformerName, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(null, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult getCallManagementPerformerList(string Campus)
        {
            try
            {
                UserService uS = new UserService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("BranchCode", Campus);
                criteria.Add("AppCode", "ISM");
                Dictionary<long, IList<UserAppRole_Vw>> PerformerList = uS.GetAppRoleOnlyActiveUsersPagingAndCriteria(0, 3000, string.Empty, string.Empty, criteria);
                if (PerformerList != null && PerformerList.First().Value != null && PerformerList.First().Value.Count > 0)
                {
                    var PerformerName = (
                             from items in PerformerList.First().Value

                             select new
                             {
                                 Text = items.UserId,
                                 Value = items.UserId
                             }).Distinct().ToList();

                    return Json(PerformerName, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(null, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

