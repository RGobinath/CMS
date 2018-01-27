using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TIPS.Entities;
using TIPS.Entities.Assess;
using TIPS.Entities.Assess.ReportCardClasses;
using TIPS.Service;
using TIPS.ServiceContract;
using TIPS.Entities.StaffManagementEntities;

namespace CMS.Controllers
{
    public class AchievementReportController : BaseController
    {
        string policyName = "ReportCardPolicy";

        #region "Report Crd Inbox"
        /// <summary>
        /// Report Card Inbox
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportCardInbox()
        {
            try
            {
                MastersService ms = new MastersService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj == null)
                {
                    return RedirectToAction("LogOff", "Account");
                }

                ViewBag.lgdUserId = (Userobj.UserId != null) ? Userobj.UserId : "";
                ViewBag.lgdUserName = (Userobj.UserName != null) ? Userobj.UserName : "";
                ViewBag.lgdInUserType = (Userobj.UserType != null) ? Userobj.UserType : "";
                Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                ViewBag.acadddl = AcademicyrMaster.First().Value;

                string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);

                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
            finally
            { }
        }

        /// <summary>
        /// Get Report Card Inbox
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="sidx"></param>
        /// <param name="sord"></param>
        /// <param name="RequestNo"></param>
        /// <param name="Name"></param>
        /// <param name="Campus"></param>
        /// <param name="Section"></param>
        /// <param name="Grade"></param>
        /// <returns></returns>
        public ActionResult GetReportCardInbox(int page, int rows, string sidx, string sord, string RequestNo, string Name, string Campus, string Section, string Grade, string RptStatus, string Semester, string AcademicYear)
        {
            try
            {
                string searchedItems = RequestNo + "," + Campus + "," + Name + "," + Section + "," + Grade + "," + RptStatus + "," + Semester + "," + AcademicYear;
                Session["RptCardMYPSearched"] = searchedItems;
                ReportCardService rptCardSrvc = new ReportCardService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                string colName = string.Empty; string[] values = new string[1];
                if (!string.IsNullOrWhiteSpace(RequestNo)) { criteria.Add("RequestNo", RequestNo); }
                if (!string.IsNullOrWhiteSpace(Name)) { criteria.Add("Name", Name); }
                if (!string.IsNullOrWhiteSpace(Semester))
                {
                    if (Semester == "1") { criteria.Add("Semester", Convert.ToInt64(1)); }
                    if (Semester == "2") { criteria.Add("Semester", Convert.ToInt64(2)); }

                }
                if (RptStatus == "Open") { RptStatus = "WIP"; }
                if (!string.IsNullOrWhiteSpace(RptStatus)) { criteria.Add("RptCardStatus", RptStatus); } else { criteria.Add("RptCardStatus", "WIP"); }
               
                var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                if (usrcmp.Count() != 0)
                {
                    if (usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
                    {
                        if (!string.IsNullOrWhiteSpace(Campus) && Campus != "Select") { criteria.Add("Campus", Campus); }
                        else { criteria.Add("Campus", usrcmp); }
                    }
                }
                if (!string.IsNullOrWhiteSpace(Section) && Section != "Select") { criteria.Add("Section", Section); }
                if (!string.IsNullOrWhiteSpace(Grade) && Grade != "Select") { criteria.Add("Grade", Grade); }
                if (!string.IsNullOrWhiteSpace(AcademicYear)) criteria.Add("AcademicYear", AcademicYear);
                //DateTime DateNow = DateTime.Now;
                //if (Grade == "IX" || Grade == "X")
                //{
                //    criteria.Add("AcademicYear", DateNow.Year.ToString() + "-" + (DateNow.Year + 1).ToString());
                //}
                //else
                //{
                //    if (DateNow.Month >= 5)
                //    {
                //        criteria.Add("AcademicYear", DateNow.Year.ToString() + "-" + (DateNow.Year + 1).ToString());
                //    }
                //    else
                //    {
                //        criteria.Add("AcademicYear", (DateNow.Year - 1).ToString() + "-" + DateNow.Year.ToString());
                //    }
                //}
                Dictionary<long, IList<RptCardInBoxView>> dcnRptCardLst = null;
                if (!string.IsNullOrEmpty(Grade) && Grade != "Select")
                {
                    dcnRptCardLst = rptCardSrvc.GetRepCardInBoxListWithPagingAndCriteriaEQSearch(page - 1, rows, sord, sidx, criteria);
                }
                else
                {
                    dcnRptCardLst = rptCardSrvc.GetRepCardInBoxListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                }
                if (dcnRptCardLst == null || dcnRptCardLst.Count == 0)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    IList<RptCardInBoxView> lstRepCard = dcnRptCardLst.FirstOrDefault().Value;
                    long totalRecords = dcnRptCardLst.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows = (
                        from items in lstRepCard
                        select new
                        {
                            i = items.Id,
                            cell = new string[] 
                       { 
                            items.Id.ToString(),
                            items.RequestNo,                        //inbox secrch Field
                            items.Campus,   //inbox secrch Field
                            items.IdNo,
                            items.Name+" "+items.Initial,          //inbox secrch Field
                            items.Section,       //inbox secrch Field
                            items.Grade,         //inbox secrch Field
                            items.StudentId.ToString(),
                            items.TeacherName,
                            items.Semester==1?"Sem I":"Sem II",
                            items.AcademicYear
                       }
                        })
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, policyName); }
            finally
            { }
        }
        #endregion "Report Crd Inbox"

        #region "PYP"
        /// <summary>
        /// Achievement Report Get Method
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ReportCardPYP(long? Id)
        {
            try
            {
                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj == null || (Userobj != null && Userobj.UserId == null))
                { return RedirectToAction("LogOff", "Account"); }

                string loggedInUserId = Userobj.UserId;//(Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                RptCardPYP objRptCardPYP = new RptCardPYP();

                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (Id > 0)
                {
                    ReportCardService rptSrv = new ReportCardService();
                    objRptCardPYP = rptSrv.GetPYPReportCard(Id ?? 0);
                }

                string loggedInUserName = (Userobj.UserName != null) ? Userobj.UserName : "";
                string loggedInUserType = (Userobj.UserType != null) ? Userobj.UserType : "";

                ViewBag.loggedInUserId = loggedInUserId;
                ViewBag.loggedInUserType = loggedInUserType;
                ViewBag.loggedInUserName = string.IsNullOrWhiteSpace(loggedInUserName) ? loggedInUserId : loggedInUserName;

                return View(objRptCardPYP);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
            finally
            { }
        }

        /// <summary>
        /// Achievement Report Post Method
        /// </summary>
        /// <param name="objRepCardG3"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveOrUpdatePYPReportCard(FormCollection FrmCltn, RptCardPYP objRptCardPYP)
        {
            try
            {
                ReportCardService rptSrv = new ReportCardService();
                if (FrmCltn["Action"] == "Complete")
                {
                    objRptCardPYP.RptCardStatus = "Closed";
                }
                else
                {
                    objRptCardPYP.RptCardStatus = "WIP";
                }

                long Id = rptSrv.SaveOrUpdatePYPReportCard(objRptCardPYP);
                if (FrmCltn["Action"] == "Save")
                {
                    return RedirectToAction("ReportCardPYP", new { Id = Id });
                }
                return RedirectToAction("ReportCardInbox");
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
            finally
            { }
        }
        #endregion "PYP"

        #region "MYP"

        /// <summary>
        /// Achievement Report Get Method
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ReportCardMYP(long? Id)
        {
            try
            {
                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj == null || (Userobj != null && Userobj.UserId == null))
                { return RedirectToAction("LogOff", "Account"); }

                string loggedInUserId = Userobj.UserId;
                ViewBag.loggedInUserRole = string.Empty;
                TIPS.Service.UserService us = new UserService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("UserId", loggedInUserId);
                criteria.Add("AppCode", "A360");
                //criteria.Add("RoleCode", "MRC");
                Dictionary<long, IList<UserAppRole>> UserAppRoleList = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 100, string.Empty, string.Empty, criteria);
                var ListCount = (from u in UserAppRoleList.First().Value
                                 where u.UserId == loggedInUserId
                                 select u.RoleCode).ToList();
                if (ListCount.Contains("A360"))
                { ViewBag.loggedInUserRole = "A360"; }
                else if (ListCount.Contains("AC360"))
                { ViewBag.loggedInUserRole = "AC360"; }

                RptCardMYP objRptCardMYP = new RptCardMYP();
                if (Id > 0)
                {
                    ReportCardService rptSrv = new ReportCardService();
                    objRptCardMYP = rptSrv.GetMYPReportCard(Id ?? 0);
                    RptCardFocus ObjRptCardFocus = rptSrv.GetRptCardFocusByGradeCampusSem(objRptCardMYP.Grade, objRptCardMYP.Campus, objRptCardMYP.Semester, objRptCardMYP.AcademicYear);
                    objRptCardMYP.RptCardFocus = ObjRptCardFocus;
                    objRptCardMYP.RptCardModifiedBy = loggedInUserId;

                    if ((objRptCardMYP.Grade == "IX" || objRptCardMYP.Grade == "X") && objRptCardMYP.Campus != "TIPS ERODE")
                    {
                        ViewBag.Entr = "Hide";

                        switch (objRptCardMYP.Section)
                        {
                            case "A":
                            case "B":
                            case "A1":
                            case "A2":
                                ViewBag.CheckFlag = "AB";
                                break;

                            case "C":
                            case "D":
                            case "S1":
                            case "S2":
                                ViewBag.CheckFlag = "CD";
                                break;
                        }
                    }

                    Assess360Service srvAssess360 = new Assess360Service();
                    Dictionary<string, object> Assess360criteria = new Dictionary<string, object>();
                    Assess360criteria.Add("PreRegNum", objRptCardMYP.PreRegNum);
                    Assess360criteria.Add("AcademicYear", objRptCardMYP.AcademicYear);

                    Dictionary<long, IList<StudentFinalResult_vw>> dcnSubMarks = srvAssess360.GetStudentFinalResultWidthSubjectWiseList(0, 999, string.Empty, string.Empty, Assess360criteria);
                    if (dcnSubMarks != null && dcnSubMarks.Count > 0)
                    {
                        IList<StudentFinalResult_vw> objSubMarksLst = dcnSubMarks.FirstOrDefault().Value;
                        if (objSubMarksLst.Count > 0 && objSubMarksLst[0] != null)
                        {
                            objRptCardMYP.SubjGrd_Eng = GetGradeList(objRptCardMYP.Semester == 1 ? objSubMarksLst[0].EngSemI : objSubMarksLst[0].EngSemII);
                            objRptCardMYP.SubjGrd_Second_Lang = GetGradeList(objRptCardMYP.Semester == 1 ? objSubMarksLst[0].LangSemI : objSubMarksLst[0].LangSemII);
                            objRptCardMYP.SubjGrd_Maths = GetGradeList(objRptCardMYP.Semester == 1 ? objSubMarksLst[0].MathsSemI : objSubMarksLst[0].MathsSemII);
                            objRptCardMYP.SubjGrd_Phys = GetGradeList(objRptCardMYP.Semester == 1 ? objSubMarksLst[0].PhySemI : objSubMarksLst[0].PhySemII);
                            objRptCardMYP.SubjGrd_Chstr = GetGradeList(objRptCardMYP.Semester == 1 ? objSubMarksLst[0].CheSemI : objSubMarksLst[0].CheSemII);
                            objRptCardMYP.SubjGrd_Bio = GetGradeList(objRptCardMYP.Semester == 1 ? objSubMarksLst[0].BioSemI : objSubMarksLst[0].BioSemII);
                            objRptCardMYP.SubjGrd_HisGeo = GetGradeList(objRptCardMYP.Semester == 1 ? objSubMarksLst[0].HcSemI : objSubMarksLst[0].HcSemII);
                            objRptCardMYP.SubjGrd_ICT = GetGradeList(objRptCardMYP.Semester == 1 ? objSubMarksLst[0].IctSemI : objSubMarksLst[0].IctSemII);
                        }
                    }
                }
                else
                { objRptCardMYP.RptCardCreatedBy = loggedInUserId; }

                /* Academic Year getting from student template so, it is not required here */
                //DateTime DateNow = DateTime.Now;
                //string AcademicYear = string.Empty;
                //if (DateNow.Month >= 5)
                //{ AcademicYear = DateNow.Year.ToString() + "-" + (DateNow.Year + 1).ToString(); }
                //else
                //{ AcademicYear = (DateNow.Year - 1).ToString() + "-" + DateNow.Year.ToString(); }
                //objRptCardMYP.AcademicYear = string.IsNullOrWhiteSpace(objRptCardMYP.AcademicYear) ? AcademicYear : objRptCardMYP.AcademicYear;

                string loggedInUserName = (Userobj.UserName != null) ? Userobj.UserName : "";
                string loggedInUserType = (Userobj.UserType != null) ? Userobj.UserType : "";

                ViewBag.loggedInUserId = loggedInUserId;
                ViewBag.loggedInUserType = loggedInUserType;
                ViewBag.loggedInUserName = string.IsNullOrWhiteSpace(loggedInUserName) ? loggedInUserId : loggedInUserName;

                return View(objRptCardMYP);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
            finally
            { }
        }

        /// <summary>
        /// Achievement Report Post Method
        /// </summary>
        /// <param name="objRepCardG3"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveOrUpdateMYPReportCard(FormCollection FrmCltn, RptCardMYP objRptCardMYP)
        {
            try
            {
                ReportCardService rptSrv = new ReportCardService();
                objRptCardMYP.RptCardStatus = FrmCltn["Action"] == "Complete" ? "Closed" : "WIP";

                if (objRptCardMYP.Id == 0)
                { objRptCardMYP.RptCardCreatedDate = DateTime.Now; }
                else { objRptCardMYP.RptCardModifiedDate = DateTime.Now; }

                long Id = rptSrv.SaveOrUpdateMYPReportCard(objRptCardMYP);

                TempData["MYPSaveAlrtMsg"] = "";

                if (FrmCltn["Action"] == "Save")
                { return RedirectToAction("ReportCardMYP", new { Id = Id }); }

                return RedirectToAction("ReportCardInbox");
            }
            catch (Exception ex)
            {
                TempData["MYPSaveAlrtMsg"] = ex.Message;
                ExceptionPolicy.HandleException(ex, policyName);
                return RedirectToAction("ReportCardMYP", "AchievementReport");
            }
            finally
            { }
        }
        #endregion "1 semester 6 Grade Report Card"

        #region "Report Card Focus"
        //public ActionResult ReportCardFocus(long? Id, string Grade, string Campus, long? Semester)
        //{
        //    try
        //    {
        //        TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
        //        if (Userobj == null || (Userobj != null && Userobj.UserId == null))
        //        { return RedirectToAction("LogOff", "Account"); }

        //        string loggedInUserId = Userobj.UserId;
        //        RptCardFocus objRptCardFocus = new RptCardFocus();

        //        DateTime DateNow = DateTime.Now;
        //        string AcademicYear = string.Empty;
        //        if (DateNow.Month >= 5)
        //        { AcademicYear = DateNow.Year.ToString() + "-" + (DateNow.Year + 1).ToString(); }
        //        else
        //        { AcademicYear = (DateNow.Year - 1).ToString() + "-" + DateNow.Year.ToString(); }

        //        ReportCardService rptSrv = new ReportCardService();
        //        if (Id > 0)
        //        {
        //            objRptCardFocus = rptSrv.GetRptCardFocusById(Id ?? 0);
        //            objRptCardFocus.ModifiedBy = loggedInUserId;
        //        }
        //        else if (!string.IsNullOrWhiteSpace(Grade) && !string.IsNullOrWhiteSpace(Campus) && Semester > 0)
        //        {
        //            objRptCardFocus = rptSrv.GetRptCardFocusByGradeCampusSem(Grade, Campus, Semester ?? 0, objRptCardFocus.AcademicYear);
        //            objRptCardFocus.ModifiedBy = loggedInUserId;
        //        }
        //        else
        //        { objRptCardFocus.CreatedBy = loggedInUserId; }
        //       // objRptCardFocus.AcademicYear = string.IsNullOrWhiteSpace(objRptCardFocus.AcademicYear) ? AcademicYear : objRptCardFocus.AcademicYear;
        //        MastersService ms = new MastersService();
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
        //        ViewBag.acadyrddl = AcademicyrMaster.First().Value;
        //        return View(objRptCardFocus);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, policyName);
        //        throw ex;
        //    }
        //    finally
        //    { }
        //}

        //[HttpPost]
        //public ActionResult SaveOrUpdateReportCardFocus(FormCollection FrmCltn, RptCardFocus objRptCardFocus)
        //{
        //    try
        //    {
        //        if (FrmCltn["Action"] == "Save")
        //        {
        //            ReportCardService rptSrv = new ReportCardService();
        //            if (objRptCardFocus.RptCardFocusId == 0)
        //            { objRptCardFocus.CreatedDate = DateTime.Now; }
        //            else { objRptCardFocus.ModifiedDate = DateTime.Now; }

        //            long Id = rptSrv.SaveOrUpdateRptCardFocus(objRptCardFocus);

        //            TempData["FocusSaveAlrtMsg"] = string.Format("Focus add for {0} Semester of {1} Grade in {2} Campus", objRptCardFocus.Semester, objRptCardFocus.Grade, objRptCardFocus.Campus);

        //            return RedirectToAction("ReportCardFocus", "AchievementReport", new { id = 0 });
        //        }
        //        else if (FrmCltn["Action"] == "Reset")
        //        { return RedirectToAction("ReportCardFocus", "AchievementReport", new { id = 0 }); }
        //        else
        //        {
        //            return RedirectToAction("ReportCardFocus", "AchievementReport", new { id = objRptCardFocus.RptCardFocusId, Grade = objRptCardFocus.Grade, Semester = objRptCardFocus.Semester, Campus = objRptCardFocus.Campus });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["FocusSaveAlrtMsg"] = ex.Message;
        //        ExceptionPolicy.HandleException(ex, policyName);
        //        return RedirectToAction("ReportCardFocus", "AchievementReport");
        //    }
        //    finally
        //    { }
        //}


        public ActionResult ReportCardFocus(long? Id, string Grade, string Campus, long? Semester, string AcademicYear)
        {
            try
            {
                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj == null || (Userobj != null && Userobj.UserId == null))
                { return RedirectToAction("LogOff", "Account"); }

                string loggedInUserId = Userobj.UserId;
                RptCardFocus objRptCardFocus = new RptCardFocus();

                DateTime DateNow = DateTime.Now;
                //string AcademicYear = string.Empty;
                //if (DateNow.Month >= 5)
                //{ AcademicYear = DateNow.Year.ToString() + "-" + (DateNow.Year + 1).ToString(); }
                //else
                //{ AcademicYear = (DateNow.Year - 1).ToString() + "-" + DateNow.Year.ToString(); }

                ReportCardService rptSrv = new ReportCardService();
                if (Id > 0)
                {
                    objRptCardFocus = rptSrv.GetRptCardFocusById(Id ?? 0);
                    objRptCardFocus.ModifiedBy = loggedInUserId;
                }
                else if (!string.IsNullOrWhiteSpace(Grade) && !string.IsNullOrWhiteSpace(Campus) && Semester > 0)
                {
                    objRptCardFocus = rptSrv.GetRptCardFocusByGradeCampusSem(Grade, Campus, Semester ?? 0, AcademicYear);
                    // objRptCardFocus = rptSrv.GetRptCardFocusByGradeCampusSem(Grade, Campus, Semester ?? 0, objRptCardFocus.AcademicYear);
                    objRptCardFocus.ModifiedBy = loggedInUserId;
                }
                else
                { objRptCardFocus.CreatedBy = loggedInUserId; }

                MastersService ms = new MastersService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                ViewBag.acadyrddl = AcademicyrMaster.First().Value;
                //objRptCardFocus.AcademicYear = string.IsNullOrWhiteSpace(objRptCardFocus.AcademicYear) ? AcademicYear : objRptCardFocus.AcademicYear;
                return View(objRptCardFocus);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
            finally
            { }
        }

        [HttpPost]
        public ActionResult SaveOrUpdateReportCardFocus(FormCollection FrmCltn, RptCardFocus objRptCardFocus)
        {
            try
            {
                if (FrmCltn["Action"] == "Save")
                {
                    ReportCardService rptSrv = new ReportCardService();
                    if (objRptCardFocus.RptCardFocusId == 0)
                    { objRptCardFocus.CreatedDate = DateTime.Now; }
                    else { objRptCardFocus.ModifiedDate = DateTime.Now; }

                    long Id = rptSrv.SaveOrUpdateRptCardFocus(objRptCardFocus);

                    TempData["FocusSaveAlrtMsg"] = string.Format("Focus add for {0} Semester of {1} Grade in {2} Campus", objRptCardFocus.Semester, objRptCardFocus.Grade, objRptCardFocus.Campus);

                    return RedirectToAction("ReportCardFocus", "AchievementReport", new { id = 0 });
                }
                else if (FrmCltn["Action"] == "Reset")
                { return RedirectToAction("ReportCardFocus", "AchievementReport", new { id = 0 }); }
                else
                {
                    return RedirectToAction("ReportCardFocus", "AchievementReport", new { id = objRptCardFocus.RptCardFocusId, Grade = objRptCardFocus.Grade, Semester = objRptCardFocus.Semester, Campus = objRptCardFocus.Campus, AcademicYear = objRptCardFocus.AcademicYear });
                }
            }
            catch (Exception ex)
            {
                TempData["FocusSaveAlrtMsg"] = ex.Message;
                ExceptionPolicy.HandleException(ex, policyName);
                return RedirectToAction("ReportCardFocus", "AchievementReport");
            }
            finally
            { }
        }
        #endregion

        #region "Helper Methods for Report Cards"
        /// <summary>
        /// Get Student Details
        /// </summary>
        /// <param name="idno"></param>
        /// <param name="name"></param>
        /// <param name="cname"></param>
        /// <param name="grade"></param>
        /// <param name="sect"></param>
        /// <param name="rows"></param>
        /// <param name="sidx"></param>
        /// <param name="sord"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult GetStudentDtls(string IdNo, string Name, string Campus, string Grade, string Section, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                ReportCardService rptCrdSrvc = new ReportCardService();

                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrWhiteSpace(IdNo)) { criteria.Add("IdNo", IdNo); }
                if (!string.IsNullOrWhiteSpace(Name)) { criteria.Add("Name", Name); }
                if (!string.IsNullOrWhiteSpace(Campus)) { criteria.Add("Campus", Campus); }
                if (!string.IsNullOrWhiteSpace(Grade)) { criteria.Add("Grade", Grade); }
                if (!string.IsNullOrWhiteSpace(Section)) { criteria.Add("Section", Section); }

                string acadeYear = string.Empty;
                DateTime DateNow = DateTime.Now;

                if (DateNow.Month >= 5)
                { acadeYear = DateNow.Year.ToString() + "-" + (DateNow.Year + 1).ToString(); }
                else
                { acadeYear = (DateNow.Year - 1).ToString() + "-" + DateNow.Year.ToString(); }

                criteria.Add("AcademicYear", acadeYear);

                string name = "Grade"; string[] values = new string[1] { Grade };

                Dictionary<long, IList<StudentDtlsView>> dcnStdntDtls = rptCrdSrvc.GetStudentDtlsViewListWithPagingAndCriteria(page - 1, rows, sidx, sord, name, values, criteria);
                if (dcnStdntDtls != null && dcnStdntDtls.Count > 0)
                {
                    long totalRecords = dcnStdntDtls.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows = (
                        from items in dcnStdntDtls.FirstOrDefault().Value
                        select new
                        {
                            i = items.Id,
                            cell = new string[] 
                       { 
                           items.Id.ToString(), items.IdNo, items.Name, items.Section, items.Campus, 
                           items.Grade, items.AcademicYear, items.IsHosteller.ToString(), items.PreRegNum
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
            { return ThrowJSONErrorNew(ex, policyName); }
        }

        public ActionResult loadPartialView(string PartialViewName)
        {
            return PartialView(PartialViewName);
        }

        public ActionResult FillCampusCode()
        {
            string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
            UserService us = new UserService();
            //pass userid and get the list of roles from user service GetAppRoleForAnUserListWithPagingAndCriteria
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("UserId", userid);
            Dictionary<long, IList<UserAppRole>> userAppRole = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);
            //assign this list in session
            //before getting it from session check whether this availabel in session or not
            //if available build json from that list and return
            //if not available, get it from db, assign it in session and build json from session
            if (userAppRole != null && userAppRole.First().Value != null && userAppRole.First().Value.Count > 0)
            {
                var BranchCodeList = (
                         from items in userAppRole.First().Value
                         //where items.UserId == userid && 
                         where items.UserId.ToString().ToUpper() == userid.ToUpper() && items.BranchCode != null
                         // orderby items.BranchCode
                         select new
                         {
                             Text = items.BranchCode,
                             Value = items.BranchCode
                         }).Distinct().OrderBy(x => x.Text).ToArray();
                return Json(BranchCodeList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public string GetCampusNameByCampusCode(string Campus)
        {
            switch (Campus)
            {
                case "IB MAIN": Campus = "COIMBATORE"; break;
                case "IB KG": Campus = "COIMBATORE"; break;
                case "KARUR": Campus = "KARUR"; break;
                case "TIRUPUR": Campus = "TIRUPUR"; break;
                case "ERNAKULAM": Campus = "ERNAKULAM"; break;
                case "CHENNAI MAIN": Campus = "CHENNAI"; break;
                case "TIPS SARAN": Campus = "TIPS SARAN"; break;
                case "TIPS ERODE": Campus = "ERODE"; break;
                case "TIPS SALEM": Campus = "SALEM"; break;
                case "CHENNAI CITY": Campus = "CHENNAI"; break;
            }
            return Campus;
        }
        #endregion "Helper Methods for Report Cards"

        #region "Generate PDf Files"
        #region "Common Methods for PDF Generation"
        public PdfPCell formatCell(string CellData, string FontName, float FontSize, bool isBold, bool isUnderline, int HAlign, int vAlign, float pTop, float pBottom, int Colspan, int Rowspan)
        {
            Colspan = Colspan == 0 ? 1 : Colspan;
            Rowspan = Rowspan == 0 ? 1 : Rowspan;
            PdfPCell frmtCell = new PdfPCell(new Phrase(CellData, new iTextSharp.text.Font(FontFactory.GetFont(FontName, FontSize))));

            if (isBold && isUnderline)
            {
                frmtCell = new PdfPCell(new Phrase(CellData, new iTextSharp.text.Font(FontFactory.GetFont(FontName, FontSize, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE))));
            }
            else if (isBold)
            {
                frmtCell = new PdfPCell(new Phrase(CellData, new iTextSharp.text.Font(FontFactory.GetFont(FontName, FontSize, iTextSharp.text.Font.BOLD))));
            }
            else if (isUnderline)
            {
                frmtCell = new PdfPCell(new Phrase(CellData, new iTextSharp.text.Font(FontFactory.GetFont(FontName, FontSize, iTextSharp.text.Font.UNDERLINE))));
            }

            frmtCell.HorizontalAlignment = HAlign;
            frmtCell.VerticalAlignment = vAlign;
            frmtCell.PaddingBottom = pBottom == 0 ? 4 : pBottom;
            frmtCell.PaddingTop = pTop == 0 ? 4 : pTop;
            frmtCell.Colspan = Colspan;
            frmtCell.Rowspan = Rowspan;
            return frmtCell;
        }

        public PdfPTable CreateTable(int NoOfColumns, int[] relativeWidth)
        {
            PdfPTable CrTbl = new PdfPTable(NoOfColumns);
            CrTbl.SetWidths(relativeWidth);
            CrTbl.WidthPercentage = 100;
            CrTbl.DefaultCell.Border = 1;
            CrTbl.DefaultCell.PaddingBottom = 10;
            CrTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            CrTbl.DefaultCell.NoWrap = false;
            return CrTbl;
        }
        #endregion

        #region "Report Card Generation by Grade and Semeter"
        public void ReportCardPYPPDFGen(long id)
        {
            try
            {
                ReportCardService rptCardSrv = new ReportCardService();
                RptCardPYP ObjRptCardPYP = rptCardSrv.GetPYPReportCard(id);
                if (ObjRptCardPYP != null)
                {
                    #region "PDF File Properties - meta information"
                    Document DocRptCrd3G1Sem = new iTextSharp.text.Document();
                    MemoryStream pdfStream = new MemoryStream();
                    PdfWriter pdfWriter = PdfWriter.GetInstance(DocRptCrd3G1Sem, pdfStream);

                    DocRptCrd3G1Sem.AddCreator(Resources.Global.tips);

                    DocRptCrd3G1Sem.AddTitle("Achievement Report");
                    DocRptCrd3G1Sem.AddAuthor(Resources.Global.tips);

                    DocRptCrd3G1Sem.AddSubject("Achievement Report of " + ObjRptCardPYP.Grade + " Grade " + ObjRptCardPYP.Semester + " Semeter");
                    DocRptCrd3G1Sem.AddKeywords(ObjRptCardPYP.Name + " Achievement Report");
                    #endregion "PDF File Properties - meta information"

                    DocRptCrd3G1Sem.Open();

                    #region "PDF Header"
                    PdfPTable table = CreateTable(6, new int[] { 16, 17, 19, 16, 16, 16 });

                    #region "Logo Image"
                    iTextSharp.text.Image LogoImage;
                    PdfPCell imgcel1 = new PdfPCell();
                    imgcel1.Rowspan = 2;
                    imgcel1.PaddingTop = 2;
                    imgcel1.Border = 0;
                    string ImagePath = ConfigurationManager.AppSettings["RptCard"] + "logo.jpg";
                    LogoImage = iTextSharp.text.Image.GetInstance(ImagePath);
                    LogoImage.ScaleAbsolute(50, 50);
                    imgcel1.AddElement(LogoImage);
                    imgcel1.PaddingLeft = 1;
                    table.AddCell(imgcel1);
                    #endregion "Logo Image"

                    PdfPCell cell2 = formatCell("THE INDIAN PUBLIC SCHOOL, " + GetCampusNameByCampusCode(ObjRptCardPYP.Campus) + "          ", "Helvetica", 9.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 10, 5, 0);
                    cell2.Border = 0;
                    table.AddCell(cell2);

                    PdfPCell cell = formatCell("SEMESTER " + (ObjRptCardPYP.Semester == 1 ? "I" : "II") + " – ACHIEVEMENT REPORT                ", "Helvetica", 9.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 15, 5, 0);
                    cell.Border = 0;
                    table.AddCell(cell);

                    table.TotalWidth = 2;// getDefaultCell().setBorder(0);

                    #endregion "PDF Header"

                    #region "Basic Details"
                    table.AddCell(formatCell("Student Name : " + ObjRptCardPYP.Name, "Helvetica", 8.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 5, 5, 3, 0));
                    table.AddCell(formatCell("Teacher Name : " + ObjRptCardPYP.TeacherName, "Helvetica", 8.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 5, 5, 3, 0));
                    table.AddCell(formatCell("Grade : " + ObjRptCardPYP.Grade + " - " + ObjRptCardPYP.Section, "Helvetica", 8.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 5, 5, 3, 0));
                    table.AddCell(formatCell("Date : " + ObjRptCardPYP.RptDate, "Helvetica", 8.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 5, 5, 3, 0));
                    DocRptCrd3G1Sem.Add(table);
                    //DocRptCrd3G1Sem.Add(new Paragraph("\n"));
                    #endregion "Basic Details"

                    #region "Tick Image"
                    PdfPCell cellTick = new PdfPCell();
                    string tickImagePath = ConfigurationManager.AppSettings["RptCard"] + "tickMark.jpg";
                    iTextSharp.text.Image tickImage = iTextSharp.text.Image.GetInstance(tickImagePath);
                    tickImage.ScaleAbsolute(10, 10);
                    tickImage.Alignment = iTextSharp.text.Image.ALIGN_CENTER | iTextSharp.text.Image.ALIGN_MIDDLE;
                    cellTick.AddElement(tickImage);
                    cellTick.HorizontalAlignment = Element.ALIGN_MIDDLE;
                    cellTick.VerticalAlignment = Element.ALIGN_MIDDLE;
                    #endregion

                    #region "Common Cells"
                    PdfPCell strandCell = formatCell("STRAND", "Helvetica", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0);
                    PdfPCell keyElmntCell = formatCell("KEY ELEMENTS", "Helvetica", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0);
                    PdfPCell exptnCell = formatCell("EXPECTATIONS", "Helvetica", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0);
                    PdfPCell AcmpCell = formatCell("ACCOMPLISHED", "Helvetica", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0);
                    PdfPCell procsCell = formatCell("PROGRESSING", "Helvetica", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0);
                    PdfPCell needEnCell = formatCell("NEEDS ENCOURAGEMENT", "Helvetica", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0);
                    PdfPCell emptyCell = formatCell(" ", "Helvetica", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0);
                    #endregion

                    #region "English Table"
                    Paragraph pEngHdr = new Paragraph("ENGLISH", new iTextSharp.text.Font(FontFactory.GetFont("Helvetica", 8.0f, iTextSharp.text.Font.BOLD)));
                    pEngHdr.Alignment = Element.ALIGN_CENTER;
                    pEngHdr.SpacingAfter = 5f;
                    pEngHdr.SpacingBefore = 5f;
                    DocRptCrd3G1Sem.Add(pEngHdr);

                    #region "Reading"
                    PdfPTable tEngRdg = CreateTable(6, new int[] { 10, 13, 37, 13, 13, 14 });

                    tEngRdg.AddCell(strandCell);
                    tEngRdg.AddCell(keyElmntCell);
                    tEngRdg.AddCell(exptnCell);
                    tEngRdg.AddCell(AcmpCell);
                    tEngRdg.AddCell(procsCell);
                    tEngRdg.AddCell(needEnCell);

                    tEngRdg.AddCell(formatCell("READING", "Helvetica", 7.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 3));
                    tEngRdg.AddCell(formatCell("Confidence", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tEngRdg.AddCell(formatCell("To read the given text independently", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                    if (ObjRptCardPYP.Eng_Rdg_Cmpsn_Acmpl == true) { tEngRdg.AddCell(cellTick); } else { tEngRdg.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_Rdg_Cmpsn_Procs == true) { tEngRdg.AddCell(cellTick); } else { tEngRdg.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_Rdg_Cmpsn_NdsEncgmnt == true) { tEngRdg.AddCell(cellTick); } else { tEngRdg.AddCell(emptyCell); };

                    tEngRdg.AddCell(formatCell("Fluency", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tEngRdg.AddCell(formatCell("To read with accurate pronunciation of individual sounds, intonation and rhythm", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                    if (ObjRptCardPYP.Eng_Rdg_Flncy_Acmpl == true) { tEngRdg.AddCell(cellTick); } else { tEngRdg.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_Rdg_Flncy_Procs == true) { tEngRdg.AddCell(cellTick); } else { tEngRdg.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_Rdg_Flncy_NdsEncgmnt == true) { tEngRdg.AddCell(cellTick); } else { tEngRdg.AddCell(emptyCell); };

                    tEngRdg.AddCell(formatCell("Comprehension", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tEngRdg.AddCell(formatCell("To relate and respond to the reading with personal understanding", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                    if (ObjRptCardPYP.Eng_Rdg_Cmpsn_Acmpl == true) { tEngRdg.AddCell(cellTick); } else { tEngRdg.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_Rdg_Cmpsn_Procs == true) { tEngRdg.AddCell(cellTick); } else { tEngRdg.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_Rdg_Cmpsn_NdsEncgmnt == true) { tEngRdg.AddCell(cellTick); } else { tEngRdg.AddCell(emptyCell); };

                    DocRptCrd3G1Sem.Add(tEngRdg);
                    DocRptCrd3G1Sem.Add(new Paragraph("\n"));
                    #endregion

                    #region "Writing"
                    PdfPTable tEngWrt = CreateTable(6, new int[] { 10, 13, 37, 13, 13, 14 });

                    tEngWrt.AddCell(strandCell);
                    tEngWrt.AddCell(keyElmntCell);
                    tEngWrt.AddCell(exptnCell);
                    tEngWrt.AddCell(AcmpCell);
                    tEngWrt.AddCell(procsCell);
                    tEngWrt.AddCell(needEnCell);

                    tEngWrt.AddCell(formatCell("WRITING", "Helvetica", 7.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 4));
                    tEngWrt.AddCell(formatCell("Introduction", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tEngWrt.AddCell(formatCell("To write simple beginning paragraph", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                    if (ObjRptCardPYP.Eng_Wrtng_Intrcn_Acmpl == true) { tEngWrt.AddCell(cellTick); } else { tEngWrt.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_Wrtng_Intrcn_Procs == true) { tEngWrt.AddCell(cellTick); } else { tEngWrt.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_Wrtng_Intrcn_NEngmt == true) { tEngWrt.AddCell(cellTick); } else { tEngWrt.AddCell(emptyCell); };

                    tEngWrt.AddCell(formatCell("Organization of ideas", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tEngWrt.AddCell(formatCell("To use more than one main idea organized into a paragraph", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Eng_Wrtng_OrgIds_Acmpl == true) { tEngWrt.AddCell(cellTick); } else { tEngWrt.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_Wrtng_OrgIds_Procs == true) { tEngWrt.AddCell(cellTick); } else { tEngWrt.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_Wrtng_OrgIds_NEngmt == true) { tEngWrt.AddCell(cellTick); } else { tEngWrt.AddCell(emptyCell); };

                    tEngWrt.AddCell(formatCell("Inclusion of supportive details", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tEngWrt.AddCell(formatCell("To use at least two supportive details about each main idea", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Eng_Wrtng_InSuprtDtls_Acmpl == true) { tEngWrt.AddCell(cellTick); } else { tEngWrt.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_Wrtng_InSuprtDtls_Procs == true) { tEngWrt.AddCell(cellTick); } else { tEngWrt.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_Wrtng_InSuprtDtls_NEngmt == true) { tEngWrt.AddCell(cellTick); } else { tEngWrt.AddCell(emptyCell); };

                    tEngWrt.AddCell(formatCell("Conclusion", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tEngWrt.AddCell(formatCell("Conclusion  To write simple ending paragraph", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Eng_Wrtng_Cnlsn_Acmpl == true) { tEngWrt.AddCell(cellTick); } else { tEngWrt.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_Wrtng_Cnlsn_Procs == true) { tEngWrt.AddCell(cellTick); } else { tEngWrt.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_Wrtng_Cnlsn_NEngmt == true) { tEngWrt.AddCell(cellTick); } else { tEngWrt.AddCell(emptyCell); };

                    DocRptCrd3G1Sem.Add(tEngWrt);
                    DocRptCrd3G1Sem.Add(new Paragraph("\n"));
                    #endregion

                    #region "LISTENING & SPEAKING NON-FICTION"
                    PdfPTable tEngLSNF = CreateTable(6, new int[] { 10, 13, 37, 13, 13, 14 });

                    tEngLSNF.AddCell(strandCell);
                    tEngLSNF.AddCell(keyElmntCell);
                    tEngLSNF.AddCell(exptnCell);
                    tEngLSNF.AddCell(AcmpCell);
                    tEngLSNF.AddCell(procsCell);
                    tEngLSNF.AddCell(needEnCell);

                    PdfPCell cellLSNF = formatCell("LISTENING & SPEAKING", "Helvetica", 7.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 2);
                    cellLSNF.Border = 0;
                    cellLSNF.Border = Rectangle.LEFT_BORDER;
                    tEngLSNF.AddCell(cellLSNF);
                    tEngLSNF.AddCell(formatCell("Topic", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tEngLSNF.AddCell(formatCell("Able to understand the topic of the book", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Eng_LSNFcn_Tpcs_Acmpl == true) { tEngLSNF.AddCell(cellTick); } else { tEngLSNF.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_LSNFcn_Tpcs_Procs == true) { tEngLSNF.AddCell(cellTick); } else { tEngLSNF.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_LSNFcn_Tpcs_NEngmt == true) { tEngLSNF.AddCell(cellTick); } else { tEngLSNF.AddCell(emptyCell); };

                    tEngLSNF.AddCell(formatCell("Main idea(s) & details", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tEngLSNF.AddCell(formatCell("Able to recall all the details linked to the main idea(s)", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Eng_LSNFcn_MIdsDtls_Acmpl == true) { tEngLSNF.AddCell(cellTick); } else { tEngLSNF.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_LSNFcn_MIdsDtls_Procs == true) { tEngLSNF.AddCell(cellTick); } else { tEngLSNF.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_LSNFcn_MIdsDtls_NEngmt == true) { tEngLSNF.AddCell(cellTick); } else { tEngLSNF.AddCell(emptyCell); };

                    PdfPCell cellLSNF1 = formatCell("NON-FICTION", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0, 0, 0, 3);
                    cellLSNF1.Border = 0;
                    cellLSNF1.Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER;
                    tEngLSNF.AddCell(cellLSNF1);

                    tEngLSNF.AddCell(formatCell("Organization", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tEngLSNF.AddCell(formatCell("To know how the book is organized", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Eng_LSNFcn_Orgn_Acmpl == true) { tEngLSNF.AddCell(cellTick); } else { tEngLSNF.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_LSNFcn_Orgn_Procs == true) { tEngLSNF.AddCell(cellTick); } else { tEngLSNF.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_LSNFcn_Orgn_NEngmt == true) { tEngLSNF.AddCell(cellTick); } else { tEngLSNF.AddCell(emptyCell); };

                    tEngLSNF.AddCell(formatCell("Command of vocabulary", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tEngLSNF.AddCell(formatCell("To use key vocabulary from the book", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Eng_LSNFcn_CVocblry_Acmpl == true) { tEngLSNF.AddCell(cellTick); } else { tEngLSNF.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_LSNFcn_CVocblry_Procs == true) { tEngLSNF.AddCell(cellTick); } else { tEngLSNF.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_LSNFcn_CVocblry_NEngmt == true) { tEngLSNF.AddCell(cellTick); } else { tEngLSNF.AddCell(emptyCell); };

                    tEngLSNF.AddCell(formatCell("Accuracy", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tEngLSNF.AddCell(formatCell("To retell the facts accurately", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Eng_LSNFcn_Acrcy_Acmpl == true) { tEngLSNF.AddCell(cellTick); } else { tEngLSNF.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_LSNFcn_Acrcy_Procs == true) { tEngLSNF.AddCell(cellTick); } else { tEngLSNF.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_LSNFcn_Acrcy_NEngmt == true) { tEngLSNF.AddCell(cellTick); } else { tEngLSNF.AddCell(emptyCell); };

                    DocRptCrd3G1Sem.Add(tEngLSNF);
                    DocRptCrd3G1Sem.Add(new Paragraph("\n"));
                    #endregion

                    #region "LISTENING & SPEAKING FICTION"
                    PdfPTable tEngLSF = CreateTable(6, new int[] { 10, 13, 37, 13, 13, 14 });

                    tEngLSF.AddCell(strandCell);
                    tEngLSF.AddCell(keyElmntCell);
                    tEngLSF.AddCell(exptnCell);
                    tEngLSF.AddCell(AcmpCell);
                    tEngLSF.AddCell(procsCell);
                    tEngLSF.AddCell(needEnCell);

                    PdfPCell cellLSF = formatCell("LISTENING & SPEAKING", "Helvetica", 7.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 2);
                    cellLSF.Border = 0;
                    cellLSF.Border = Rectangle.LEFT_BORDER;
                    tEngLSF.AddCell(cellLSF);

                    tEngLSF.AddCell(formatCell("Beginning", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tEngLSF.AddCell(formatCell("To tell how the story begins", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Eng_LSFcn_Bgn_Acmpl == true) { tEngLSF.AddCell(cellTick); } else { tEngLSF.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_LSFcn_Bgn_Procs == true) { tEngLSF.AddCell(cellTick); } else { tEngLSF.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_LSFcn_Bgn_NdsEncgmnt == true) { tEngLSF.AddCell(cellTick); } else { tEngLSF.AddCell(emptyCell); };

                    tEngLSF.AddCell(formatCell("Setting", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tEngLSF.AddCell(formatCell("To identify the setting of the story", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Eng_LSFcn_Stg_Acmpl == true) { tEngLSF.AddCell(cellTick); } else { tEngLSF.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_LSFcn_Stg_Procs == true) { tEngLSF.AddCell(cellTick); } else { tEngLSF.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_LSFcn_Stg_NdsEncgmnt == true) { tEngLSF.AddCell(cellTick); } else { tEngLSF.AddCell(emptyCell); };


                    PdfPCell cellLSF1 = formatCell("FICTION", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0, 0, 0, 4);
                    cellLSF1.Border = 0;
                    cellLSF1.Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER;
                    tEngLSF.AddCell(cellLSF1);

                    tEngLSF.AddCell(formatCell("Character", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tEngLSF.AddCell(formatCell("To identify the main characters of the story and their importance", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Eng_LSFcn_Crtr_Acmpl == true) { tEngLSF.AddCell(cellTick); } else { tEngLSF.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_LSFcn_Crtr_Procs == true) { tEngLSF.AddCell(cellTick); } else { tEngLSF.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_LSFcn_Crtr_NdsEncgmnt == true) { tEngLSF.AddCell(cellTick); } else { tEngLSF.AddCell(emptyCell); };

                    tEngLSF.AddCell(formatCell("Problem", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tEngLSF.AddCell(formatCell("To identify the problem in the story", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Eng_LSFcn_Prblm_Acmpl == true) { tEngLSF.AddCell(cellTick); } else { tEngLSF.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_LSFcn_Prblm_Procs == true) { tEngLSF.AddCell(cellTick); } else { tEngLSF.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_LSFcn_Prblm_NEngmt == true) { tEngLSF.AddCell(cellTick); } else { tEngLSF.AddCell(emptyCell); };

                    tEngLSF.AddCell(formatCell("Sequence", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tEngLSF.AddCell(formatCell("To understand the order of events in the story", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Eng_LSFcn_Seq_Acmpl == true) { tEngLSF.AddCell(cellTick); } else { tEngLSF.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_LSFcn_Seq_Procs == true) { tEngLSF.AddCell(cellTick); } else { tEngLSF.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_LSFcn_Seq_NdsEncgmnt == true) { tEngLSF.AddCell(cellTick); } else { tEngLSF.AddCell(emptyCell); };

                    tEngLSF.AddCell(formatCell("Resolution", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tEngLSF.AddCell(formatCell("To know how the problem is solved / the story ends", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Eng_LSFcn_Rsln_Acmpl == true) { tEngLSF.AddCell(cellTick); } else { tEngLSF.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_LSFcn_Rsln_Procs == true) { tEngLSF.AddCell(cellTick); } else { tEngLSF.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Eng_LSFcn_Rsln_NdsEncgmnt == true) { tEngLSF.AddCell(cellTick); } else { tEngLSF.AddCell(emptyCell); };

                    DocRptCrd3G1Sem.Add(tEngLSF);
                    #endregion
                    #endregion "English Table"

                    DocRptCrd3G1Sem.NewPage();
                    #region "Second Language Table"
                    Paragraph pHindiHdr = new Paragraph("HINDI", new iTextSharp.text.Font(FontFactory.GetFont("Helvetica", 8.0f, iTextSharp.text.Font.BOLD)));
                    pHindiHdr.Alignment = Element.ALIGN_CENTER;
                    pHindiHdr.SpacingAfter = 5f;
                    pHindiHdr.SpacingBefore = 5f;
                    DocRptCrd3G1Sem.Add(pHindiHdr);

                    #region "Reading"
                    PdfPTable tHindiRdg = CreateTable(6, new int[] { 10, 13, 37, 13, 13, 14 });

                    tHindiRdg.AddCell(strandCell);
                    tHindiRdg.AddCell(keyElmntCell);
                    tHindiRdg.AddCell(exptnCell);
                    tHindiRdg.AddCell(AcmpCell);
                    tHindiRdg.AddCell(procsCell);
                    tHindiRdg.AddCell(needEnCell);

                    tHindiRdg.AddCell(formatCell("READING", "Helvetica", 7.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 5));
                    tHindiRdg.AddCell(formatCell("Confidence", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHindiRdg.AddCell(formatCell("To read  confidently", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Hnd_Rdg_Cnfdnc_Acmpl == true) { tHindiRdg.AddCell(cellTick); } else { tHindiRdg.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_Rdg_Cnfdnc_Procs == true) { tHindiRdg.AddCell(cellTick); } else { tHindiRdg.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_Rdg_Cnfdnc_NdsEncgmnt == true) { tHindiRdg.AddCell(cellTick); } else { tHindiRdg.AddCell(emptyCell); };

                    tHindiRdg.AddCell(formatCell("Fluency", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHindiRdg.AddCell(formatCell("To read fluency, intonation and rhythm", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Hnd_Rdg_Flncy_Acmpl == true) { tHindiRdg.AddCell(cellTick); } else { tHindiRdg.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_Rdg_Flncy_Procs == true) { tHindiRdg.AddCell(cellTick); } else { tHindiRdg.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_Rdg_Flncy_NdsEncgmnt == true) { tHindiRdg.AddCell(cellTick); } else { tHindiRdg.AddCell(emptyCell); };

                    tHindiRdg.AddCell(formatCell("Tone and intonation", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHindiRdg.AddCell(formatCell("To convey the emotion  fairly well and  musical", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Hnd_Rdg_TonInTon_Acmpl == true) { tHindiRdg.AddCell(cellTick); } else { tHindiRdg.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_Rdg_TonInTon_Procs == true) { tHindiRdg.AddCell(cellTick); } else { tHindiRdg.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_Rdg_TonInTon_NEngmt == true) { tHindiRdg.AddCell(cellTick); } else { tHindiRdg.AddCell(emptyCell); };

                    tHindiRdg.AddCell(formatCell("Pronunciation", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHindiRdg.AddCell(formatCell("To pronounce clearly", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Hnd_Rdg_Pronctn_Acmpl == true) { tHindiRdg.AddCell(cellTick); } else { tHindiRdg.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_Rdg_Pronctn_Procs == true) { tHindiRdg.AddCell(cellTick); } else { tHindiRdg.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_Rdg_Pronctn_NEngmt == true) { tHindiRdg.AddCell(cellTick); } else { tHindiRdg.AddCell(emptyCell); };

                    tHindiRdg.AddCell(formatCell("Understanding", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHindiRdg.AddCell(formatCell("To understand the concepts well", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Hnd_Rdg_udstnd_Acmpl == true) { tHindiRdg.AddCell(cellTick); } else { tHindiRdg.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_Rdg_udstnd_Procs == true) { tHindiRdg.AddCell(cellTick); } else { tHindiRdg.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_Rdg_udstnd_NdsEncgmnt == true) { tHindiRdg.AddCell(cellTick); } else { tHindiRdg.AddCell(emptyCell); };

                    DocRptCrd3G1Sem.Add(tEngRdg);
                    #endregion

                    DocRptCrd3G1Sem.Add(new Paragraph("\n"));

                    #region "WRITING"
                    PdfPTable tHindiWrt = CreateTable(6, new int[] { 10, 13, 37, 13, 13, 14 });

                    tHindiWrt.AddCell(strandCell);
                    tHindiWrt.AddCell(keyElmntCell);
                    tHindiWrt.AddCell(exptnCell);
                    tHindiWrt.AddCell(AcmpCell);
                    tHindiWrt.AddCell(procsCell);
                    tHindiWrt.AddCell(needEnCell);

                    tHindiWrt.AddCell(formatCell("WRITING", "Helvetica", 7.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 6));
                    tHindiWrt.AddCell(formatCell("Spelling", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHindiWrt.AddCell(formatCell("To write without spelling mistakes", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Hnd_Wrtng_Splng_Acmpl == true) { tHindiWrt.AddCell(cellTick); } else { tHindiWrt.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_Wrtng_Splng_Procs == true) { tHindiWrt.AddCell(cellTick); } else { tHindiWrt.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_Wrtng_Splng_NEngmt == true) { tHindiWrt.AddCell(cellTick); } else { tHindiWrt.AddCell(emptyCell); };

                    tHindiWrt.AddCell(formatCell("Vocabulary", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHindiWrt.AddCell(formatCell("To use appropriate vocabulary while writing", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Hnd_Wrtng_Vocblry_Acmpl == true) { tHindiWrt.AddCell(cellTick); } else { tHindiWrt.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_Wrtng_Vocblry_Procs == true) { tHindiWrt.AddCell(cellTick); } else { tHindiWrt.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_Wrtng_Vocblry_NEngmt == true) { tHindiWrt.AddCell(cellTick); } else { tHindiWrt.AddCell(emptyCell); };

                    tHindiWrt.AddCell(formatCell("Punctuation", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHindiWrt.AddCell(formatCell("To punctuate appropriately", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Hnd_Wrtng_Punctn_Acmpl == true) { tHindiWrt.AddCell(cellTick); } else { tHindiWrt.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_Wrtng_Punctn_Procs == true) { tHindiWrt.AddCell(cellTick); } else { tHindiWrt.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_Wrtng_Punctn_NEngmt == true) { tHindiWrt.AddCell(cellTick); } else { tHindiWrt.AddCell(emptyCell); };

                    tHindiWrt.AddCell(formatCell("Grammar", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHindiWrt.AddCell(formatCell("To use grammar appropriately", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Hnd_Wrtng_Grmr_Acmpl == true) { tHindiWrt.AddCell(cellTick); } else { tHindiWrt.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_Wrtng_Grmr_Procs == true) { tHindiWrt.AddCell(cellTick); } else { tHindiWrt.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_Wrtng_Grmr_NdsEncgmnt == true) { tHindiWrt.AddCell(cellTick); } else { tHindiWrt.AddCell(emptyCell); };

                    tHindiWrt.AddCell(formatCell("Ideas and content", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHindiWrt.AddCell(formatCell("To use the main ideas to convey their understanding", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Hnd_Wrtng_IdsCntnt_Acmpl == true) { tHindiWrt.AddCell(cellTick); } else { tHindiWrt.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_Wrtng_IdsCntnt_Procs == true) { tHindiWrt.AddCell(cellTick); } else { tHindiWrt.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_Wrtng_IdsCntnt_NEngmt == true) { tHindiWrt.AddCell(cellTick); } else { tHindiWrt.AddCell(emptyCell); };

                    tHindiWrt.AddCell(formatCell("Neatness and handwriting", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHindiWrt.AddCell(formatCell("To present the writing neatly", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Hnd_Wrtng_NetHndwrt_Acmpl == true) { tHindiWrt.AddCell(cellTick); } else { tHindiWrt.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_Wrtng_NetHndwrt_Procs == true) { tHindiWrt.AddCell(cellTick); } else { tHindiWrt.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_Wrtng_NetHndwrt_NEngmt == true) { tHindiWrt.AddCell(cellTick); } else { tHindiWrt.AddCell(emptyCell); };

                    DocRptCrd3G1Sem.Add(tHindiWrt);
                    #endregion

                    DocRptCrd3G1Sem.Add(new Paragraph("\n"));

                    #region "LISTENING & SPEAKING"
                    PdfPTable tHindiLS = CreateTable(6, new int[] { 10, 13, 37, 13, 13, 14 });

                    tHindiLS.AddCell(strandCell);
                    tHindiLS.AddCell(keyElmntCell);
                    tHindiLS.AddCell(exptnCell);
                    tHindiLS.AddCell(AcmpCell);
                    tHindiLS.AddCell(procsCell);
                    tHindiLS.AddCell(needEnCell);

                    tHindiLS.AddCell(formatCell("LISTENING & SPEAKING", "Helvetica", 7.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 5));
                    tHindiLS.AddCell(formatCell("Confidence", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHindiLS.AddCell(formatCell("To speak confidently", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Hnd_LS_Confdnc_Acmpl == true) { tHindiLS.AddCell(cellTick); } else { tHindiLS.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_LS_Confdnc_Procs == true) { tHindiLS.AddCell(cellTick); } else { tHindiLS.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_LS_Confdnc_NdsEncgmnt == true) { tHindiLS.AddCell(cellTick); } else { tHindiLS.AddCell(emptyCell); };

                    tHindiLS.AddCell(formatCell("Fluency", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHindiLS.AddCell(formatCell("To speak  with  intonation and rhythm", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Hnd_LS_Flncy_Acmpl == true) { tHindiLS.AddCell(cellTick); } else { tHindiLS.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_LS_Flncy_Procs == true) { tHindiLS.AddCell(cellTick); } else { tHindiLS.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_LS_Flncy_NdsEncgmnt == true) { tHindiLS.AddCell(cellTick); } else { tHindiLS.AddCell(emptyCell); };

                    tHindiLS.AddCell(formatCell("Pronunciation", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHindiLS.AddCell(formatCell("To speak with accurate pronunciation of individual sounds", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Hnd_LS_Pronctn_Acmpl == true) { tHindiLS.AddCell(cellTick); } else { tHindiLS.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_LS_Pronctn_Procs == true) { tHindiLS.AddCell(cellTick); } else { tHindiLS.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_LS_Pronctn_NdsEncgmnt == true) { tHindiLS.AddCell(cellTick); } else { tHindiLS.AddCell(emptyCell); };

                    tHindiLS.AddCell(formatCell("Understanding", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHindiLS.AddCell(formatCell("To convey their understanding clearly", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Hnd_LS_udstnd_Acmpl == true) { tHindiLS.AddCell(cellTick); } else { tHindiLS.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_LS_udstnd_Procs == true) { tHindiLS.AddCell(cellTick); } else { tHindiLS.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_LS_udstnd_NdsEncgmnt == true) { tHindiLS.AddCell(cellTick); } else { tHindiLS.AddCell(emptyCell); };

                    tHindiLS.AddCell(formatCell("Vocabulary", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHindiLS.AddCell(formatCell("To use appropriately vocabulary while speaking", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Hnd_LS_Vocblry_Acmpl == true) { tHindiLS.AddCell(cellTick); } else { tHindiLS.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_LS_Vocblry_Procs == true) { tHindiLS.AddCell(cellTick); } else { tHindiLS.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Hnd_LS_Vocblry_NdsEncgmnt == true) { tHindiLS.AddCell(cellTick); } else { tHindiLS.AddCell(emptyCell); };

                    DocRptCrd3G1Sem.Add(tHindiLS);
                    #endregion
                    #endregion

                    DocRptCrd3G1Sem.NewPage();

                    #region "MATHEMATICS Table"
                    Paragraph pMathsHdr = new Paragraph("MATHEMATICS", new iTextSharp.text.Font(FontFactory.GetFont("Helvetica", 8.0f, iTextSharp.text.Font.BOLD)));
                    pMathsHdr.Alignment = Element.ALIGN_CENTER;
                    pMathsHdr.SpacingAfter = 5f;
                    pMathsHdr.SpacingBefore = 5f;
                    DocRptCrd3G1Sem.Add(pMathsHdr);

                    PdfPTable tMaths = CreateTable(5, new int[] { 15, 40, 15, 15, 15 });

                    tMaths.AddCell(strandCell);
                    tMaths.AddCell(exptnCell);
                    tMaths.AddCell(AcmpCell);
                    tMaths.AddCell(procsCell);
                    tMaths.AddCell(needEnCell);

                    tMaths.AddCell(formatCell("Numbers", "Helvetica", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_TOP, 0, 0, 0, 3));
                    tMaths.AddCell(formatCell("To use base 10 place value system", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Maths_Nmrs_B10VSys_Acmpl == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Maths_Nmrs_B10VSys_Procs == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Maths_Nmrs_B10VSys_NEngmt == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };

                    tMaths.AddCell(formatCell("To solve problems involving addition and subtraction", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Maths_Nmrs_SPIAAS_Acmpl == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Maths_Nmrs_SPIAAS_Procs == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Maths_Nmrs_SPIAAS_NEngmt == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };

                    tMaths.AddCell(formatCell("To use estimation strategies in addition and subtraction", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Maths_Nmrs_UESIAAS_Acmpl == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Maths_Nmrs_UESIAAS_Procs == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Maths_Nmrs_UESIAAS_NdsEncgmnt == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };

                    PdfPCell cellSAS1 = formatCell("Shape and space", "Helvetica", 7.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0, 0, 0, 0);
                    cellSAS1.Border = 0;
                    cellSAS1.Border = PdfPCell.LEFT_BORDER;
                    tMaths.AddCell(cellSAS1);

                    tMaths.AddCell(formatCell("To identify and describe 2D and 3D shapes", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Maths_SAS_IAD2A3S_Acmpl == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Maths_SAS_IAD2A3S_Procs == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Maths_SAS_IAD2A3S_NEngmt == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };

                    PdfPCell cellSAS2 = formatCell("Integrated with ' How we express ourselves '", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0, 0, 0, 2);
                    cellSAS2.Border = 0;
                    cellSAS2.Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER;
                    tMaths.AddCell(cellSAS2);

                    tMaths.AddCell(formatCell("To understand and identify  lines of symmetry , reflective and rotational symmetry", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Maths_SAS_UAILOSRARS_Acmpl == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Maths_SAS_UAILOSRARS_Procs == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Maths_SAS_UAILOSRARS_NEngmt == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };

                    tMaths.AddCell(formatCell("To understand the congruency and similarity in 2D shapes", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Maths_SAS_UTCASI2S_Acmpl == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Maths_SAS_UTCASI2S_Procs == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Maths_SAS_UTCASI2S_NEngmt == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };

                    PdfPCell cellMsrmnt1 = formatCell("Measurement", "Helvetica", 7.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0, 0, 0, 0);
                    cellMsrmnt1.Border = 0;
                    cellMsrmnt1.Border = PdfPCell.LEFT_BORDER;
                    tMaths.AddCell(cellMsrmnt1);

                    tMaths.AddCell(formatCell("To estimate and measure length , perimeter and area", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Maths_Msrmnt_EAMLPAA_Acmpl == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Maths_Msrmnt_EAMLPAA_Procs == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Maths_Msrmnt_EAMLPAA_NEngmt == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };

                    PdfPCell cellMsrmnt2 = formatCell("Integrated with ' How the world works '", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0, 0, 0, 2);
                    cellMsrmnt2.Border = 0;
                    cellMsrmnt2.Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER;
                    tMaths.AddCell(cellMsrmnt2);

                    tMaths.AddCell(formatCell("To convert the units of length, mass and capacity", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Maths_Msrmnt_CTUOLMAC_Acmpl == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Maths_Msrmnt_CTUOLMAC_Procs == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Maths_Msrmnt_CTUOLMAC_NEngmt == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };

                    tMaths.AddCell(formatCell("To read and make a calendar", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.Maths_Msrmnt_RAMAC_Acmpl == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Maths_Msrmnt_RAMAC_Procs == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Maths_Msrmnt_RAMAC_NEngmt == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };

                    tMaths.AddCell(formatCell("Completion of home assignments", "Helvetica", 7.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 2, 0));
                    if (ObjRptCardPYP.Maths_CmpHmAssgn_Acmpl == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Maths_CmpHmAssgn_Procs == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };
                    if (ObjRptCardPYP.Maths_CmpHmAssgn_NEngmt == true) { tMaths.AddCell(cellTick); } else { tMaths.AddCell(emptyCell); };

                    DocRptCrd3G1Sem.Add(tMaths);
                    #endregion

                    //it will create new page
                    DocRptCrd3G1Sem.NewPage();

                    #region "PROGRAMME OF INQUIRY"
                    Paragraph pPOIHdr1 = new Paragraph("PROGRAMME OF INQUIRY", new iTextSharp.text.Font(FontFactory.GetFont("Helvetica", 8.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE)));
                    Paragraph pPOIHdr2 = new Paragraph("Theme – How we express ourselves", new iTextSharp.text.Font(FontFactory.GetFont("Helvetica", 8.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE)));
                    Paragraph pPOIHdr3 = new Paragraph("Central idea – A variety of signs and symbols facilitates local and global communication", new iTextSharp.text.Font(FontFactory.GetFont("Helvetica", 8.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE)));
                    pPOIHdr1.Alignment = Element.ALIGN_CENTER;
                    pPOIHdr1.SpacingBefore = 5f;
                    DocRptCrd3G1Sem.Add(pPOIHdr1);

                    pPOIHdr2.Alignment = Element.ALIGN_CENTER;
                    DocRptCrd3G1Sem.Add(pPOIHdr2);

                    pPOIHdr3.Alignment = Element.ALIGN_CENTER;
                    pPOIHdr3.SpacingAfter = 5f;
                    DocRptCrd3G1Sem.Add(pPOIHdr3);

                    #region "first Table"
                    PdfPTable tPOI = CreateTable(6, new int[] { 20, 40, 10, 10, 10, 10 });

                    tPOI.AddCell(formatCell("LINES OF INQUIRY", "Helvetica", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tPOI.AddCell(formatCell("EXPECTATIONS", "Helvetica", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tPOI.AddCell(formatCell("A", "Helvetica", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tPOI.AddCell(formatCell("B", "Helvetica", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tPOI.AddCell(formatCell("C", "Helvetica", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tPOI.AddCell(formatCell("D", "Helvetica", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                    tPOI.AddCell(formatCell("Language of signs and symbols", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tPOI.AddCell(formatCell("To explore a variety of signs and symbols and interpret their meanings", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.POI_THWEOS_LSAS_A == true) { tPOI.AddCell(cellTick); } else { tPOI.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THWEOS_LSAS_B == true) { tPOI.AddCell(cellTick); } else { tPOI.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THWEOS_LSAS_C == true) { tPOI.AddCell(cellTick); } else { tPOI.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THWEOS_LSAS_D == true) { tPOI.AddCell(cellTick); } else { tPOI.AddCell(emptyCell); };

                    tPOI.AddCell(formatCell("Reasons for the development of communication systems.", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tPOI.AddCell(formatCell("To identify the cultural and historical context in which signs and symbols develop", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.POI_THWEOS_RFTDOCS_A == true) { tPOI.AddCell(cellTick); } else { tPOI.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THWEOS_RFTDOCS_B == true) { tPOI.AddCell(cellTick); } else { tPOI.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THWEOS_RFTDOCS_C == true) { tPOI.AddCell(cellTick); } else { tPOI.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THWEOS_RFTDOCS_D == true) { tPOI.AddCell(cellTick); } else { tPOI.AddCell(emptyCell); };

                    tPOI.AddCell(formatCell("Specialized systems of communication across the world", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tPOI.AddCell(formatCell("To demonstrate how non-verbal communication allows people to transcend barriers such as language", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.POI_THWEOS_SSOCATW_A == true) { tPOI.AddCell(cellTick); } else { tPOI.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THWEOS_SSOCATW_B == true) { tPOI.AddCell(cellTick); } else { tPOI.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THWEOS_SSOCATW_C == true) { tPOI.AddCell(cellTick); } else { tPOI.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THWEOS_SSOCATW_D == true) { tPOI.AddCell(cellTick); } else { tPOI.AddCell(emptyCell); };

                    tPOI.AddCell(formatCell("Comments:", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 10, 10, 0, 0));
                    tPOI.AddCell(formatCell(ObjRptCardPYP.POI_THWEOS_Cmnts, "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 10, 10, 5, 0));

                    DocRptCrd3G1Sem.Add(tPOI);
                    #endregion

                    #region "Development of Skills"
                    Paragraph pDS = new Paragraph("DEVELOPMENT OF SKILLS", new iTextSharp.text.Font(FontFactory.GetFont("Helvetica", 8.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE)));
                    pDS.Alignment = Element.ALIGN_LEFT;
                    pDS.SpacingAfter = 5f;
                    pDS.SpacingBefore = 5f;
                    DocRptCrd3G1Sem.Add(pDS);

                    PdfPTable tDS = CreateTable(5, new int[] { 15, 40, 15, 15, 15 });

                    tDS.AddCell(formatCell("COMMUNICATION SKILL", "Helvetica", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tDS.AddCell(exptnCell);
                    tDS.AddCell(AcmpCell);
                    tDS.AddCell(procsCell);
                    tDS.AddCell(needEnCell);

                    tDS.AddCell(formatCell("Presenting", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tDS.AddCell(formatCell("To communicate information and ideas through a variety of visual media for effective presentation.", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.POI_DOSkls_CSklPrsnt_Acmpl == true) { tDS.AddCell(cellTick); } else { tDS.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_DOSkls_CSklPrsnt_Procs == true) { tDS.AddCell(cellTick); } else { tDS.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_DOSkls_CSklPrsnt_NEngmt == true) { tDS.AddCell(cellTick); } else { tDS.AddCell(emptyCell); };

                    tDS.AddCell(formatCell("Non-verbal communication", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tDS.AddCell(formatCell("To create signs , interpret and utilize symbols", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.POI_DOSkls_CSklNVComm_Acmpl == true) { tDS.AddCell(cellTick); } else { tDS.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_DOSkls_CSklNVComm_Procs == true) { tDS.AddCell(cellTick); } else { tDS.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_DOSkls_CSklNVComm_NEngmt == true) { tDS.AddCell(cellTick); } else { tDS.AddCell(emptyCell); };

                    DocRptCrd3G1Sem.Add(tDS);
                    DocRptCrd3G1Sem.Add(new Paragraph("\n"));

                    PdfPTable tDS1 = CreateTable(5, new int[] { 15, 40, 15, 15, 15 });
                    tDS1.AddCell(formatCell("THINKING SKILLS", "Helvetica", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tDS1.AddCell(exptnCell);
                    tDS1.AddCell(AcmpCell);
                    tDS1.AddCell(procsCell);
                    tDS1.AddCell(needEnCell);

                    tDS1.AddCell(formatCell("Application", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tDS1.AddCell(formatCell("To make use of previously acquired knowledge in practical or new ways", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.POI_DOSkls_TSklAppn_Acmpl == true) { tDS1.AddCell(cellTick); } else { tDS1.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_DOSkls_TSklAppn_Procs == true) { tDS1.AddCell(cellTick); } else { tDS1.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_DOSkls_TSklAppn_NEngmt == true) { tDS1.AddCell(cellTick); } else { tDS1.AddCell(emptyCell); };

                    tDS1.AddCell(formatCell("Synthesis", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tDS1.AddCell(formatCell("To combine parts to create whole, creating, designing, developing, and innovating", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.POI_DOSkls_TSSynth_Acmpl == true) { tDS1.AddCell(cellTick); } else { tDS1.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_DOSkls_TSSynth_Procs == true) { tDS1.AddCell(cellTick); } else { tDS1.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_DOSkls_TSSynth_NEngmt == true) { tDS1.AddCell(cellTick); } else { tDS1.AddCell(emptyCell); };
                    DocRptCrd3G1Sem.Add(tDS1);

                    DocRptCrd3G1Sem.Add(new Paragraph("\n"));
                    PdfPTable tDS2 = CreateTable(5, new int[] { 15, 40, 15, 15, 15 });

                    tDS2.AddCell(formatCell("ICT skill", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tDS2.AddCell(formatCell("To use the information and communication technology  for collecting relevant information and presenting the same throughout the inquiry.", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.POI_DOSkls_ICTSkl_Acmpl == true) { tDS2.AddCell(cellTick); } else { tDS2.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_DOSkls_ICTSkl_Procs == true) { tDS2.AddCell(cellTick); } else { tDS2.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_DOSkls_ICTSkl_NEngmt == true) { tDS2.AddCell(cellTick); } else { tDS2.AddCell(emptyCell); };

                    DocRptCrd3G1Sem.Add(tDS2);
                    DocRptCrd3G1Sem.Add(new Paragraph("\n"));
                    #endregion

                    #region "ATTITUDES"
                    Paragraph pAttud = new Paragraph("ATTITUDES", new iTextSharp.text.Font(FontFactory.GetFont("Helvetica", 8.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE)));
                    pAttud.Alignment = Element.ALIGN_LEFT;
                    pAttud.SpacingAfter = 5f;
                    pAttud.SpacingBefore = 5f;
                    DocRptCrd3G1Sem.Add(pAttud);

                    PdfPTable tAttud = CreateTable(5, new int[] { 15, 40, 15, 15, 15 });

                    tAttud.AddCell(formatCell("ATTITUDES ADDRESSED", "Helvetica", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tAttud.AddCell(exptnCell);
                    tAttud.AddCell(AcmpCell);
                    tAttud.AddCell(procsCell);
                    tAttud.AddCell(needEnCell);

                    tAttud.AddCell(formatCell("Confidence", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tAttud.AddCell(formatCell("To feel confident in their ability as learners, applying what they have learned and making appropriate choices.", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.POI_Attds_Cnfdnc_Acmpl == true) { tAttud.AddCell(cellTick); } else { tAttud.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_Attds_Cnfdnc_Procs == true) { tAttud.AddCell(cellTick); } else { tAttud.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_Attds_Cnfdnc_NEngmt == true) { tAttud.AddCell(cellTick); } else { tAttud.AddCell(emptyCell); };

                    tAttud.AddCell(formatCell("Creativity", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tAttud.AddCell(formatCell("Being creative and imaginative in their thinking and in their approach to problems and dilemmas", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.POI_Attds_Crtvty_Acmpl == true) { tAttud.AddCell(cellTick); } else { tAttud.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_Attds_Crtvty_Procs == true) { tAttud.AddCell(cellTick); } else { tAttud.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_Attds_Crtvty_NEngmt == true) { tAttud.AddCell(cellTick); } else { tAttud.AddCell(emptyCell); };

                    tAttud.AddCell(formatCell("Independence", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tAttud.AddCell(formatCell("To think and act independently, making their own judgments", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.POI_Attds_Indpndc_Acmpl == true) { tAttud.AddCell(cellTick); } else { tAttud.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_Attds_Indpndc_Procs == true) { tAttud.AddCell(cellTick); } else { tAttud.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_Attds_Indpndc_NEngmt == true) { tAttud.AddCell(cellTick); } else { tAttud.AddCell(emptyCell); };

                    DocRptCrd3G1Sem.Add(tAttud);
                    #endregion

                    #region "DEVELOPMENT OF LEARNER PROFILE"
                    Paragraph pDOLP = new Paragraph("DEVELOPMENT OF LEARNER PROFILE", new iTextSharp.text.Font(FontFactory.GetFont("Helvetica", 8.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE)));
                    pDOLP.Alignment = Element.ALIGN_LEFT;
                    pDOLP.SpacingAfter = 5f;
                    pDOLP.SpacingBefore = 5f;
                    DocRptCrd3G1Sem.Add(pDOLP);

                    PdfPTable tDOLP = CreateTable(5, new int[] { 15, 40, 15, 15, 15 });

                    tDOLP.AddCell(formatCell("STRIVES TO BE", "Helvetica", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tDOLP.AddCell(exptnCell);
                    tDOLP.AddCell(AcmpCell);
                    tDOLP.AddCell(procsCell);
                    tDOLP.AddCell(needEnCell);

                    tDOLP.AddCell(formatCell("Communicator", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tDOLP.AddCell(formatCell("To understand and express ideas and information confidently and creatively in more than one language and in a variety of modes of communication", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.POI_DLP_Comm_Acmpl == true) { tDOLP.AddCell(cellTick); } else { tDOLP.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_DLP_Comm_Procs == true) { tDOLP.AddCell(cellTick); } else { tDOLP.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_DLP_Comm_NdsEncgmnt == true) { tDOLP.AddCell(cellTick); } else { tDOLP.AddCell(emptyCell); };

                    tDOLP.AddCell(formatCell("Thinker", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tDOLP.AddCell(formatCell("To exercise initiative in applying thinking skills critically and creatively to recognize and approach complex problems", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.POI_DLP_Thnkr_Acmpl == true) { tDOLP.AddCell(cellTick); } else { tDOLP.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_DLP_Thnkr_Procs == true) { tDOLP.AddCell(cellTick); } else { tDOLP.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_DLP_Thnkr_NdsEncgmnt == true) { tDOLP.AddCell(cellTick); } else { tDOLP.AddCell(emptyCell); };

                    DocRptCrd3G1Sem.Add(tDOLP);
                    #endregion
                    #endregion

                    DocRptCrd3G1Sem.NewPage();

                    #region "Theme : HOW THE WORLD WORKS"
                    Paragraph pHTWWHdr1 = new Paragraph("Theme : HOW THE WORLD WORKS", new iTextSharp.text.Font(FontFactory.GetFont("Helvetica", 8.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE)));
                    Paragraph pHTWWHdr2 = new Paragraph("CENTRAL IDEA: Human survival is connected to understanding the continual changing nature of the Earth", new iTextSharp.text.Font(FontFactory.GetFont("Helvetica", 8.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE)));
                    pHTWWHdr1.Alignment = Element.ALIGN_CENTER;
                    pHTWWHdr1.SpacingBefore = 5f;
                    DocRptCrd3G1Sem.Add(pHTWWHdr1);

                    pHTWWHdr2.Alignment = Element.ALIGN_CENTER;
                    pHTWWHdr2.SpacingAfter = 5f;
                    DocRptCrd3G1Sem.Add(pHTWWHdr2);

                    #region "First Table"
                    PdfPTable tHTWW1 = CreateTable(6, new int[] { 20, 40, 10, 10, 10, 10 });

                    tHTWW1.AddCell(formatCell("LINES OF INQUIRY", "Helvetica", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHTWW1.AddCell(formatCell("EXPECTATIONS", "Helvetica", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHTWW1.AddCell(formatCell("A", "Helvetica", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHTWW1.AddCell(formatCell("B", "Helvetica", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHTWW1.AddCell(formatCell("C", "Helvetica", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHTWW1.AddCell(formatCell("D", "Helvetica", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                    tHTWW1.AddCell(formatCell("Interrelationship of the different components of the Earth", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHTWW1.AddCell(formatCell("To identify the different components of the Earth and their interrelationship", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.POI_THTWW_IOTDCOTE_A == true) { tHTWW1.AddCell(cellTick); } else { tHTWW1.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_IOTDCOTE_B == true) { tHTWW1.AddCell(cellTick); } else { tHTWW1.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_IOTDCOTE_C == true) { tHTWW1.AddCell(cellTick); } else { tHTWW1.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_IOTDCOTE_D == true) { tHTWW1.AddCell(cellTick); } else { tHTWW1.AddCell(emptyCell); };

                    tHTWW1.AddCell(formatCell("How the Earth has changed and is continuing to change", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHTWW1.AddCell(formatCell("To identify the long term and short term changes on the Earth", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.POI_THTWW_HTEHCAICTC_A == true) { tHTWW1.AddCell(cellTick); } else { tHTWW1.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_HTEHCAICTC_B == true) { tHTWW1.AddCell(cellTick); } else { tHTWW1.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_HTEHCAICTC_C == true) { tHTWW1.AddCell(cellTick); } else { tHTWW1.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_HTEHCAICTC_D == true) { tHTWW1.AddCell(cellTick); } else { tHTWW1.AddCell(emptyCell); };

                    tHTWW1.AddCell(formatCell("Reasons for the changes on the Earth", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHTWW1.AddCell(formatCell("To identify the evidence that the Earth has changed", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.POI_THTWW_RFTCOTE_A == true) { tHTWW1.AddCell(cellTick); } else { tHTWW1.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_RFTCOTE_B == true) { tHTWW1.AddCell(cellTick); } else { tHTWW1.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_RFTCOTE_C == true) { tHTWW1.AddCell(cellTick); } else { tHTWW1.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_RFTCOTE_D == true) { tHTWW1.AddCell(cellTick); } else { tHTWW1.AddCell(emptyCell); };

                    tHTWW1.AddCell(formatCell("Human response to the Earth's changes", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHTWW1.AddCell(formatCell("To explore scientific and technological developments that help people understand their response to the changing Earth such as relocation, redesigning and strengthening the defences", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.POI_THTWW_HRTTEC_A == true) { tHTWW1.AddCell(cellTick); } else { tHTWW1.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_HRTTEC_B == true) { tHTWW1.AddCell(cellTick); } else { tHTWW1.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_HRTTEC_C == true) { tHTWW1.AddCell(cellTick); } else { tHTWW1.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_HRTTEC_D == true) { tHTWW1.AddCell(cellTick); } else { tHTWW1.AddCell(emptyCell); };

                    tHTWW1.AddCell(formatCell("Comments:", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 10, 10, 0, 0));
                    tHTWW1.AddCell(formatCell(ObjRptCardPYP.POI_THTWW_Cmnts, "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 10, 10, 5, 0));

                    DocRptCrd3G1Sem.Add(tHTWW1);
                    #endregion

                    #region "DEVELOPMENT OF SKILLS"
                    Paragraph pHTWWDS = new Paragraph("DEVELOPMENT OF SKILLS", new iTextSharp.text.Font(FontFactory.GetFont("Helvetica", 8.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE)));
                    pHTWWDS.Alignment = Element.ALIGN_LEFT;
                    pHTWWDS.SpacingAfter = 5f;
                    pHTWWDS.SpacingBefore = 5f;
                    DocRptCrd3G1Sem.Add(pHTWWDS);

                    PdfPTable tHTWWDS = CreateTable(5, new int[] { 15, 40, 15, 15, 15 });

                    tHTWWDS.AddCell(formatCell("SELF MANAGEMENT SKILL", "Helvetica", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHTWWDS.AddCell(exptnCell);
                    tHTWWDS.AddCell(AcmpCell);
                    tHTWWDS.AddCell(procsCell);
                    tHTWWDS.AddCell(needEnCell);

                    tHTWWDS.AddCell(formatCell("Gross motor skills:", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHTWWDS.AddCell(formatCell("To understand the importance of using their muscle strength to survive during disasters", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.POI_THTWW_SMSklGMSkl_Acmpl == true) { tHTWWDS.AddCell(cellTick); } else { tHTWWDS.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_SMSklGMSkl_Procs == true) { tHTWWDS.AddCell(cellTick); } else { tHTWWDS.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_SMSklGMSkl_NEngmt == true) { tHTWWDS.AddCell(cellTick); } else { tHTWWDS.AddCell(emptyCell); };

                    tHTWWDS.AddCell(formatCell("Informed choices:", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHTWWDS.AddCell(formatCell("To know the appropriate course of action based on the situation in order to protect themselves during the natural changes", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.POI_THTWW_SMSklInfCho_Acmpl == true) { tHTWWDS.AddCell(cellTick); } else { tHTWWDS.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_SMSklInfCho_Procs == true) { tHTWWDS.AddCell(cellTick); } else { tHTWWDS.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_SMSklInfCho_NEngmt == true) { tHTWWDS.AddCell(cellTick); } else { tHTWWDS.AddCell(emptyCell); };

                    DocRptCrd3G1Sem.Add(tHTWWDS);

                    DocRptCrd3G1Sem.Add(new Paragraph("\n"));
                    PdfPTable tHTWWDS1 = CreateTable(5, new int[] { 15, 40, 15, 15, 15 });
                    tHTWWDS1.AddCell(formatCell("RESEARCH SKILL", "Helvetica", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHTWWDS1.AddCell(exptnCell);
                    tHTWWDS1.AddCell(AcmpCell);
                    tHTWWDS1.AddCell(procsCell);
                    tHTWWDS1.AddCell(needEnCell);

                    tHTWWDS1.AddCell(formatCell("Observing", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHTWWDS1.AddCell(formatCell("To observe the changes  using all the senses to notice relevant details", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.POI_THTWW_RSklObsr_Acmpl == true) { tHTWWDS1.AddCell(cellTick); } else { tHTWWDS1.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_RSklObsr_Procs == true) { tHTWWDS1.AddCell(cellTick); } else { tHTWWDS1.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_RSklObsr_NEngmt == true) { tHTWWDS1.AddCell(cellTick); } else { tHTWWDS1.AddCell(emptyCell); };

                    tHTWWDS1.AddCell(formatCell("Recording", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHTWWDS1.AddCell(formatCell("To record their finding and interpret.", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.POI_THTWW_RSklRcd_Acmpl == true) { tHTWWDS1.AddCell(cellTick); } else { tHTWWDS1.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_RSklRcd_Procs == true) { tHTWWDS1.AddCell(cellTick); } else { tHTWWDS1.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_RSklRcd_NEngmt == true) { tHTWWDS1.AddCell(cellTick); } else { tHTWWDS1.AddCell(emptyCell); };

                    tHTWWDS1.AddCell(formatCell("Presenting", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHTWWDS1.AddCell(formatCell("To present their research findings using appropriate", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.POI_THTWW_RSklPrsnt_Acmpl == true) { tHTWWDS1.AddCell(cellTick); } else { tHTWWDS1.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_RSklPrsnt_Procs == true) { tHTWWDS1.AddCell(cellTick); } else { tHTWWDS1.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_RSklPrsnt_NEngmt == true) { tHTWWDS1.AddCell(cellTick); } else { tHTWWDS1.AddCell(emptyCell); };

                    DocRptCrd3G1Sem.Add(tHTWWDS1);

                    DocRptCrd3G1Sem.Add(new Paragraph("\n"));
                    PdfPTable tHTWWDS2 = CreateTable(5, new int[] { 15, 40, 15, 15, 15 });

                    tHTWWDS2.AddCell(formatCell("ICT skill", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHTWWDS2.AddCell(formatCell("communication technology  for collecting relevant information and presenting the same throughout the inquiry.", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.POI_THTWW_ICTSkl_Acmpl == true) { tHTWWDS2.AddCell(cellTick); } else { tHTWWDS2.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_ICTSkl_Procs == true) { tHTWWDS2.AddCell(cellTick); } else { tHTWWDS2.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_ICTSkl_NEngmt == true) { tHTWWDS2.AddCell(cellTick); } else { tHTWWDS2.AddCell(emptyCell); };

                    DocRptCrd3G1Sem.Add(tHTWWDS2);
                    DocRptCrd3G1Sem.Add(new Paragraph("\n"));
                    #endregion

                    #region "ATTITUDES"
                    Paragraph pHTWWAtud = new Paragraph("ATTITUDES", new iTextSharp.text.Font(FontFactory.GetFont("Helvetica", 8.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE)));
                    pHTWWAtud.Alignment = Element.ALIGN_LEFT;
                    pHTWWAtud.SpacingAfter = 5f;
                    pHTWWAtud.SpacingBefore = 5f;
                    DocRptCrd3G1Sem.Add(pHTWWAtud);

                    PdfPTable tHTWWAtud = CreateTable(5, new int[] { 15, 40, 15, 15, 15 });

                    tHTWWAtud.AddCell(formatCell("ATTITUDES ADDRESSED", "Helvetica", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHTWWAtud.AddCell(exptnCell);
                    tHTWWAtud.AddCell(AcmpCell);
                    tHTWWAtud.AddCell(procsCell);
                    tHTWWAtud.AddCell(needEnCell);

                    tHTWWAtud.AddCell(formatCell("Curiosity", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHTWWAtud.AddCell(formatCell("Being curious to know about the nature of learning about the world", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.POI_THTWW_AttCrtsy_Acmpl == true) { tHTWWAtud.AddCell(cellTick); } else { tHTWWAtud.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_AttCrtsy_Procs == true) { tHTWWAtud.AddCell(cellTick); } else { tHTWWAtud.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_AttCrtsy_NEngmt == true) { tHTWWAtud.AddCell(cellTick); } else { tHTWWAtud.AddCell(emptyCell); };

                    tHTWWAtud.AddCell(formatCell("Tolerance", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHTWWAtud.AddCell(formatCell("Being sensitive about the differences and diversity in the world and respond appropriately", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.POI_THTWW_AttTolnc_Acmpl == true) { tHTWWAtud.AddCell(cellTick); } else { tHTWWAtud.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_AttTolnc_Procs == true) { tHTWWAtud.AddCell(cellTick); } else { tHTWWAtud.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_AttTolnc_NEngmt == true) { tHTWWAtud.AddCell(cellTick); } else { tHTWWAtud.AddCell(emptyCell); };

                    DocRptCrd3G1Sem.Add(tHTWWAtud);
                    #endregion

                    #region "DEVELOPMENT OF LEARNER PROFILE"
                    Paragraph pHTWWDOLP = new Paragraph("DEVELOPMENT OF LEARNER PROFILE", new iTextSharp.text.Font(FontFactory.GetFont("Helvetica", 8.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE)));
                    pHTWWDOLP.Alignment = Element.ALIGN_LEFT;
                    pHTWWDOLP.SpacingAfter = 5f;
                    pHTWWDOLP.SpacingBefore = 5f;
                    DocRptCrd3G1Sem.Add(pHTWWDOLP);

                    PdfPTable tHTWWDOLP = CreateTable(5, new int[] { 15, 40, 15, 15, 15 });

                    tHTWWDOLP.AddCell(formatCell("STRIVES TO BE", "Helvetica", 7.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHTWWDOLP.AddCell(exptnCell);
                    tHTWWDOLP.AddCell(AcmpCell);
                    tHTWWDOLP.AddCell(procsCell);
                    tHTWWDOLP.AddCell(needEnCell);

                    tHTWWDOLP.AddCell(formatCell("Risk-taker", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHTWWDOLP.AddCell(formatCell("To approach unfamiliar situations and uncertainty with courage and forethought", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.POI_THTWW_DLPRskTkr_Acmpl == true) { tHTWWDOLP.AddCell(cellTick); } else { tHTWWDOLP.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_DLPRskTkr_Procs == true) { tHTWWDOLP.AddCell(cellTick); } else { tHTWWDOLP.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_DLPRskTkr_NEngmt == true) { tHTWWDOLP.AddCell(cellTick); } else { tHTWWDOLP.AddCell(emptyCell); };

                    tHTWWDOLP.AddCell(formatCell("Knowledgeable", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tHTWWDOLP.AddCell(formatCell("To explore concepts , ideas and issues that have local and global significance as well as acquire indepth knowledge", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.POI_THTWW_DLPKnwdg_Acmpl == true) { tHTWWDOLP.AddCell(cellTick); } else { tHTWWDOLP.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_DLPKnwdg_Procs == true) { tHTWWDOLP.AddCell(cellTick); } else { tHTWWDOLP.AddCell(emptyCell); };
                    if (ObjRptCardPYP.POI_THTWW_DLPKnwdg_NEngmt == true) { tHTWWDOLP.AddCell(cellTick); } else { tHTWWDOLP.AddCell(emptyCell); };

                    DocRptCrd3G1Sem.Add(tHTWWDOLP);
                    #endregion
                    #endregion

                    DocRptCrd3G1Sem.NewPage();

                    #region "PHYSICAL EDUCATION"
                    DocRptCrd3G1Sem.Add(new Paragraph("\n"));
                    Paragraph pPhyEdu = new Paragraph("PHYSICAL EDUCATION", new iTextSharp.text.Font(FontFactory.GetFont("Helvetica", 8.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE)));
                    pPhyEdu.Alignment = Element.ALIGN_CENTER;
                    pPhyEdu.SpacingAfter = 5f;
                    pPhyEdu.SpacingBefore = 5f;
                    DocRptCrd3G1Sem.Add(pPhyEdu);

                    PdfPTable tPhyEdu = CreateTable(5, new int[] { 15, 40, 15, 15, 15 });

                    tPhyEdu.AddCell(keyElmntCell);
                    tPhyEdu.AddCell(exptnCell);
                    tPhyEdu.AddCell(AcmpCell);
                    tPhyEdu.AddCell(procsCell);
                    tPhyEdu.AddCell(needEnCell);

                    tPhyEdu.AddCell(formatCell("Behavior", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tPhyEdu.AddCell(formatCell("Shows empathy towards peers and conducts him/herself in a principled manner", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.PhysEd_Bhvr_Acmpl == true) { tPhyEdu.AddCell(cellTick); } else { tPhyEdu.AddCell(emptyCell); };
                    if (ObjRptCardPYP.PhysEd_Bhvr_Procs == true) { tPhyEdu.AddCell(cellTick); } else { tPhyEdu.AddCell(emptyCell); };
                    if (ObjRptCardPYP.PhysEd_Bhvr_NdsEncgmnt == true) { tPhyEdu.AddCell(cellTick); } else { tPhyEdu.AddCell(emptyCell); };

                    tPhyEdu.AddCell(formatCell("Team work", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tPhyEdu.AddCell(formatCell("Develops social skills while interacting; shows awareness and respect for the views and needs of others", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.PhysEd_TmWrk_Acmpl == true) { tPhyEdu.AddCell(cellTick); } else { tPhyEdu.AddCell(emptyCell); };
                    if (ObjRptCardPYP.PhysEd_TmWrk_Procs == true) { tPhyEdu.AddCell(cellTick); } else { tPhyEdu.AddCell(emptyCell); };
                    if (ObjRptCardPYP.PhysEd_TmWrk_NdsEncgmnt == true) { tPhyEdu.AddCell(cellTick); } else { tPhyEdu.AddCell(emptyCell); };

                    tPhyEdu.AddCell(formatCell("Basic skills", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tPhyEdu.AddCell(formatCell("Demonstrates the basic skills and techniques necessary for active participation in a variety of physical activities", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.PhysEd_BscSkl_Acmpl == true) { tPhyEdu.AddCell(cellTick); } else { tPhyEdu.AddCell(emptyCell); };
                    if (ObjRptCardPYP.PhysEd_BscSkl_Procs == true) { tPhyEdu.AddCell(cellTick); } else { tPhyEdu.AddCell(emptyCell); };
                    if (ObjRptCardPYP.PhysEd_BscSkl_NdsEncgmnt == true) { tPhyEdu.AddCell(cellTick); } else { tPhyEdu.AddCell(emptyCell); };

                    tPhyEdu.AddCell(formatCell("Fitness", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tPhyEdu.AddCell(formatCell("Recognizes the importance of physical activity and maintains a healthy lifestyle", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.PhysEd_Ftns_Acmpl == true) { tPhyEdu.AddCell(cellTick); } else { tPhyEdu.AddCell(emptyCell); };
                    if (ObjRptCardPYP.PhysEd_Ftns_Procs == true) { tPhyEdu.AddCell(cellTick); } else { tPhyEdu.AddCell(emptyCell); };
                    if (ObjRptCardPYP.PhysEd_Ftns_NdsEncgmnt == true) { tPhyEdu.AddCell(cellTick); } else { tPhyEdu.AddCell(emptyCell); };

                    tPhyEdu.AddCell(formatCell("Commitment", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tPhyEdu.AddCell(formatCell("Being committed to their own learning and shows self discipline and responsibility", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.PhysEd_Cmtmnt_Acmpl == true) { tPhyEdu.AddCell(cellTick); } else { tPhyEdu.AddCell(emptyCell); };
                    if (ObjRptCardPYP.PhysEd_Cmtmnt_Procs == true) { tPhyEdu.AddCell(cellTick); } else { tPhyEdu.AddCell(emptyCell); };
                    if (ObjRptCardPYP.PhysEd_Cmtmnt_NdsEncgmnt == true) { tPhyEdu.AddCell(cellTick); } else { tPhyEdu.AddCell(emptyCell); };
                    DocRptCrd3G1Sem.Add(tPhyEdu);
                    #endregion

                    DocRptCrd3G1Sem.NewPage();
                    #region "PERFORMING ARTS"
                    #region "MUSIC"
                    DocRptCrd3G1Sem.Add(new Paragraph("\n"));
                    Paragraph pPA = new Paragraph("PERFORMING ARTS", new iTextSharp.text.Font(FontFactory.GetFont("Helvetica", 8.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE)));
                    pPA.Alignment = Element.ALIGN_CENTER;
                    pPA.SpacingBefore = 5f;
                    DocRptCrd3G1Sem.Add(pPA);

                    Paragraph pPAMus = new Paragraph("MUSIC", new iTextSharp.text.Font(FontFactory.GetFont("Helvetica", 8.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE)));
                    pPAMus.Alignment = Element.ALIGN_CENTER;
                    pPAMus.SpacingAfter = 5f;
                    DocRptCrd3G1Sem.Add(pPAMus);

                    PdfPTable tPAMus = CreateTable(5, new int[] { 15, 40, 15, 15, 15 });

                    tPAMus.AddCell(strandCell);
                    tPAMus.AddCell(exptnCell);
                    tPAMus.AddCell(AcmpCell);
                    tPAMus.AddCell(procsCell);
                    tPAMus.AddCell(needEnCell);

                    tPAMus.AddCell(formatCell("Performing & Singing", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tPAMus.AddCell(formatCell("Sing songs from a variety of times and culture.", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.PrfArt_Musc_PfrmSng_Acmpl == true) { tPAMus.AddCell(cellTick); } else { tPAMus.AddCell(emptyCell); };
                    if (ObjRptCardPYP.PrfArt_Musc_PfrmSng_Procs == true) { tPAMus.AddCell(cellTick); } else { tPAMus.AddCell(emptyCell); };
                    if (ObjRptCardPYP.PrfArt_Musc_PfrmSng_NEngmt == true) { tPAMus.AddCell(cellTick); } else { tPAMus.AddCell(emptyCell); };

                    tPAMus.AddCell(formatCell("Creating and Composing", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tPAMus.AddCell(formatCell("Sing to their own composition", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.PrfArt_Musc_CrtCmp_Acmpl == true) { tPAMus.AddCell(cellTick); } else { tPAMus.AddCell(emptyCell); };
                    if (ObjRptCardPYP.PrfArt_Musc_CrtCmp_Procs == true) { tPAMus.AddCell(cellTick); } else { tPAMus.AddCell(emptyCell); };
                    if (ObjRptCardPYP.PrfArt_Musc_CrtCmp_NEngmt == true) { tPAMus.AddCell(cellTick); } else { tPAMus.AddCell(emptyCell); };

                    tPAMus.AddCell(formatCell("Listening and appreciation", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tPAMus.AddCell(formatCell("Respond to different music, giving reasons for preferences.", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.PrfArt_Musc_LstnAprn_Acmpl == true) { tPAMus.AddCell(cellTick); } else { tPAMus.AddCell(emptyCell); };
                    if (ObjRptCardPYP.PrfArt_Musc_LstnAprn_Procs == true) { tPAMus.AddCell(cellTick); } else { tPAMus.AddCell(emptyCell); };
                    if (ObjRptCardPYP.PrfArt_Musc_LstnAprn_NEngmt == true) { tPAMus.AddCell(cellTick); } else { tPAMus.AddCell(emptyCell); };
                    DocRptCrd3G1Sem.Add(tPAMus);
                    #endregion

                    #region "WESTERN DANCE"
                    DocRptCrd3G1Sem.Add(new Paragraph("\n"));

                    Paragraph pPAWDance = new Paragraph("WESTERN DANCE", new iTextSharp.text.Font(FontFactory.GetFont("Helvetica", 8.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE)));
                    pPAWDance.Alignment = Element.ALIGN_CENTER;
                    pPAWDance.SpacingAfter = 5f;
                    DocRptCrd3G1Sem.Add(pPAWDance);

                    PdfPTable tPAWDance = CreateTable(5, new int[] { 15, 40, 15, 15, 15 });

                    tPAWDance.AddCell(keyElmntCell);
                    tPAWDance.AddCell(exptnCell);
                    tPAWDance.AddCell(AcmpCell);
                    tPAWDance.AddCell(procsCell);
                    tPAWDance.AddCell(needEnCell);

                    tPAWDance.AddCell(formatCell("Skills / Technique", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tPAWDance.AddCell(formatCell("Demonstrates basic loco motor and non-loco motor movement", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.PrfArt_WDnc_SklTech_Acmpl == true) { tPAWDance.AddCell(cellTick); } else { tPAWDance.AddCell(emptyCell); };
                    if (ObjRptCardPYP.PrfArt_WDnc_SklTech_Procs == true) { tPAWDance.AddCell(cellTick); } else { tPAWDance.AddCell(emptyCell); };
                    if (ObjRptCardPYP.PrfArt_WDnc_SklTech_NEngmt == true) { tPAWDance.AddCell(cellTick); } else { tPAWDance.AddCell(emptyCell); };

                    tPAWDance.AddCell(formatCell("Problem-solving", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tPAWDance.AddCell(formatCell("Improvises with focus and concentration", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.PrfArt_WDnc_PrbSlvng_Acmpl == true) { tPAWDance.AddCell(cellTick); } else { tPAWDance.AddCell(emptyCell); };
                    if (ObjRptCardPYP.PrfArt_WDnc_PrbSlvng_Procs == true) { tPAWDance.AddCell(cellTick); } else { tPAWDance.AddCell(emptyCell); };
                    if (ObjRptCardPYP.PrfArt_WDnc_PrbSlvng_NEngmt == true) { tPAWDance.AddCell(cellTick); } else { tPAWDance.AddCell(emptyCell); };

                    tPAWDance.AddCell(formatCell("Collaboration", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tPAWDance.AddCell(formatCell("Performs a movement sequence in small and large groups", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.PrfArt_WDnc_Clbrtn_Acmpl == true) { tPAWDance.AddCell(cellTick); } else { tPAWDance.AddCell(emptyCell); };
                    if (ObjRptCardPYP.PrfArt_WDnc_Clbrtn_Procs == true) { tPAWDance.AddCell(cellTick); } else { tPAWDance.AddCell(emptyCell); };
                    if (ObjRptCardPYP.PrfArt_WDnc_Clbrtn_NEngmt == true) { tPAWDance.AddCell(cellTick); } else { tPAWDance.AddCell(emptyCell); };
                    DocRptCrd3G1Sem.Add(tPAWDance);
                    #endregion

                    #region "CLASSICAL DANCE"
                    DocRptCrd3G1Sem.Add(new Paragraph("\n"));

                    Paragraph pPACDance = new Paragraph("CLASSICAL DANCE", new iTextSharp.text.Font(FontFactory.GetFont("Helvetica", 8.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE)));
                    pPACDance.Alignment = Element.ALIGN_CENTER;
                    pPACDance.SpacingAfter = 5f;
                    DocRptCrd3G1Sem.Add(pPACDance);

                    PdfPTable tPACDance = CreateTable(5, new int[] { 15, 40, 15, 15, 15 });

                    tPACDance.AddCell(keyElmntCell);
                    tPACDance.AddCell(exptnCell);
                    tPACDance.AddCell(AcmpCell);
                    tPACDance.AddCell(procsCell);
                    tPACDance.AddCell(needEnCell);

                    tPACDance.AddCell(formatCell("Skills/Technique", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tPACDance.AddCell(formatCell("Exhibits understanding of single & double hand mudras", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.PrfArt_ClsDnc_SklTech_Acmpl == true) { tPACDance.AddCell(cellTick); } else { tPACDance.AddCell(emptyCell); };
                    if (ObjRptCardPYP.PrfArt_ClsDnc_SklTech_Procs == true) { tPACDance.AddCell(cellTick); } else { tPACDance.AddCell(emptyCell); };
                    if (ObjRptCardPYP.PrfArt_ClsDnc_SklTech_NEngmt == true) { tPACDance.AddCell(cellTick); } else { tPACDance.AddCell(emptyCell); };

                    tPACDance.AddCell(formatCell("Basic steps", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tPACDance.AddCell(formatCell("Performs basic steps and  facial expressions (NAVARASANGAL)", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.PrfArt_ClsDnc_BscStps_Acmpl == true) { tPACDance.AddCell(cellTick); } else { tPACDance.AddCell(emptyCell); };
                    if (ObjRptCardPYP.PrfArt_ClsDnc_BscStps_Procs == true) { tPACDance.AddCell(cellTick); } else { tPACDance.AddCell(emptyCell); };
                    if (ObjRptCardPYP.PrfArt_ClsDnc_BscStps_NEngmt == true) { tPACDance.AddCell(cellTick); } else { tPACDance.AddCell(emptyCell); };

                    tPACDance.AddCell(formatCell("Commitment", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tPACDance.AddCell(formatCell("Shows interest for learning", "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    if (ObjRptCardPYP.PrfArt_ClsDnc_Cmtmnt_Acmpl == true) { tPACDance.AddCell(cellTick); } else { tPACDance.AddCell(emptyCell); };
                    if (ObjRptCardPYP.PrfArt_ClsDnc_Cmtmnt_Procs == true) { tPACDance.AddCell(cellTick); } else { tPACDance.AddCell(emptyCell); };
                    if (ObjRptCardPYP.PrfArt_ClsDnc_Cmtmnt_NEngmt == true) { tPACDance.AddCell(cellTick); } else { tPACDance.AddCell(emptyCell); };
                    DocRptCrd3G1Sem.Add(tPACDance);
                    #endregion
                    #endregion

                    #region "UOI  Grade level scale:"
                    DocRptCrd3G1Sem.Add(new Paragraph("\n"));

                    Paragraph pUOIGLS = new Paragraph("UOI  Grade level scale:", new iTextSharp.text.Font(FontFactory.GetFont("Helvetica", 8.0f, iTextSharp.text.Font.BOLD)));
                    pUOIGLS.Alignment = Element.ALIGN_LEFT;
                    pUOIGLS.SpacingAfter = 5f;
                    DocRptCrd3G1Sem.Add(pUOIGLS);

                    PdfPTable tUOIGLS = CreateTable(2, new int[] { 15, 85 });

                    string aDesc = "Performance level and accomplishments far exceed the expectations. The student demonstrates consistent clarity ";
                    aDesc += "and quality in the thought process and work. Consistently meets all the core criteria and demonstrates complete ";
                    aDesc += "understanding of the concepts learned";

                    string bDesc = "Performance level and accomplishments exceed the overall expectations. The student meets all of the core criteria ";
                    bDesc += "and demonstrates better than average level of understanding";

                    string cDesc = "Performance level and accomplishments  meet the overall expectations. The student meets several core criteria and ";
                    cDesc += "demonstrates fair understanding of the concepts  learned";

                    string dDesc = "Performance level and accomplishments  do not meet the overall expectations. The student needs improvement in ";
                    dDesc += "understanding of several core criteria.  The student  requires  further support in understanding and retention of the ";
                    dDesc += "concepts  learned";

                    tUOIGLS.AddCell(formatCell("A", "Helvetica", 7.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tUOIGLS.AddCell(formatCell(aDesc, "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tUOIGLS.AddCell(formatCell("B", "Helvetica", 7.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tUOIGLS.AddCell(formatCell(bDesc, "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tUOIGLS.AddCell(formatCell("C", "Helvetica", 7.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tUOIGLS.AddCell(formatCell(cDesc, "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tUOIGLS.AddCell(formatCell("D", "Helvetica", 7.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tUOIGLS.AddCell(formatCell(dDesc, "Helvetica", 7.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                    DocRptCrd3G1Sem.Add(tUOIGLS);
                    #endregion

                    #region "Signature"
                    DocRptCrd3G1Sem.Add(new Paragraph("\n"));

                    PdfPTable tSign = CreateTable(4, new int[] { 25, 25, 25, 25 });

                    tSign.AddCell(formatCell("Teacher............................................", "Helvetica", 7.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 10, 10, 0, 0));
                    tSign.AddCell(formatCell("Coordinator........................................", "Helvetica", 7.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 10, 10, 0, 0));
                    tSign.AddCell(formatCell("Head of school.....................................", "Helvetica", 7.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 10, 10, 0, 0));
                    tSign.AddCell(formatCell("Parent.............................................", "Helvetica", 7.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 10, 10, 0, 0));

                    DocRptCrd3G1Sem.Add(tSign);
                    #endregion
                    DocRptCrd3G1Sem.Close();
                    pdfWriter.Close();
                    //return File(ConfigurationManager.AppSettings["RptCard"].ToString() + ObjRptCardPYP.IdNo + "_" + ObjRptCardPYP.Grade + "_" + ObjRptCardPYP.Section + "_ReportCard.pdf", "Application/pdf", "" + ObjRptCardPYP.IdNo + "_" + ObjRptCardPYP.Grade + "_" + ObjRptCardPYP.Section + "_ReportCard.pdf");
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.ContentType = "application/pdf";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + ObjRptCardPYP.IdNo + "_" + ObjRptCardPYP.Grade + "_" + ObjRptCardPYP.Section + "_ReportCard.pdf");
                    Response.BinaryWrite(pdfStream.ToArray());
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
        }

        public void MYPReportCardPDFGen(long id)
        {
            try
            {
                ReportCardService rptCardSrv = new ReportCardService();
                RptCardMYP objRptCardMYP = rptCardSrv.GetMYPReportCard(id);
                if (objRptCardMYP != null)
                {
                    #region "Final Report Card Integration"
                    /* to get the final result for a student */
                    Assess360Service srvAssess360 = new Assess360Service();
                    Dictionary<string, object> Assess360criteria = new Dictionary<string, object>();
                    Assess360criteria.Add("PreRegNum", objRptCardMYP.PreRegNum);
                    Assess360criteria.Add("AcademicYear", objRptCardMYP.AcademicYear);
                    Assess360criteria.Add("Semester", (objRptCardMYP.Semester == 1) ? "Sem I" : "Sem II");

                    // Dictionary<long, IList<StudentFinalResult_vw>> dcnSubMarks = srvAssess360.GetStudentFinalResultWidthSubjectWiseList(0, 999, string.Empty, string.Empty, Assess360criteria);
                    Dictionary<long, IList<SubjectMarksForRptCard_Vw>> dcnSubMarks = rptCardSrv.GetSubjectMarksForRptCard_VwWidthSubjectWiseList(0, 999, string.Empty, string.Empty, Assess360criteria);
                    if (dcnSubMarks != null && dcnSubMarks.Count > 0)
                    {
                        //  IList<StudentFinalResult_vw> objSubMarksLst = dcnSubMarks.FirstOrDefault().Value;
                        IList<SubjectMarksForRptCard_Vw> objSubMarksLst = dcnSubMarks.FirstOrDefault().Value;
                        if (objSubMarksLst.Count > 0 && objSubMarksLst[0] != null)
                        {
                            objRptCardMYP.SubjGrd_Eng = GetGradeList(objRptCardMYP.Semester == 1 ? objSubMarksLst[0].EngSemI : objSubMarksLst[0].EngSemII);
                            objRptCardMYP.SubjGrd_Second_Lang = GetGradeList(objRptCardMYP.Semester == 1 ? objSubMarksLst[0].LangSemI : objSubMarksLst[0].LangSemII);
                            objRptCardMYP.SubjGrd_Maths = GetGradeList(objRptCardMYP.Semester == 1 ? objSubMarksLst[0].MathsSemI : objSubMarksLst[0].MathsSemII);
                            objRptCardMYP.SubjGrd_Phys = GetGradeList(objRptCardMYP.Semester == 1 ? objSubMarksLst[0].PhySemI : objSubMarksLst[0].PhySemII);
                            objRptCardMYP.SubjGrd_Chstr = GetGradeList(objRptCardMYP.Semester == 1 ? objSubMarksLst[0].CheSemI : objSubMarksLst[0].CheSemII);
                            objRptCardMYP.SubjGrd_Bio = GetGradeList(objRptCardMYP.Semester == 1 ? objSubMarksLst[0].BioSemI : objSubMarksLst[0].BioSemII);
                            if ((objRptCardMYP.Grade == "IX" || objRptCardMYP.Grade == "X") && objRptCardMYP.Campus == "TIPS ERODE")
                            {
                                objRptCardMYP.SubjGrd_HisGeo = GetGradeList(objRptCardMYP.Semester == 1 ? objSubMarksLst[0].EcoSemI : objSubMarksLst[0].EcoSemII);
                            }
                            else
                            {
                                objRptCardMYP.SubjGrd_HisGeo = GetGradeList(objRptCardMYP.Semester == 1 ? objSubMarksLst[0].HcSemI : objSubMarksLst[0].HcSemII);
                            }
                            objRptCardMYP.SubjGrd_ICT = GetGradeList(objRptCardMYP.Semester == 1 ? objSubMarksLst[0].IctSemI : objSubMarksLst[0].IctSemII);
                        }
                    }

                    RptCardFocus ObjRptCardFocus = rptCardSrv.GetRptCardFocusByGradeCampusSem(objRptCardMYP.Grade, objRptCardMYP.Campus, objRptCardMYP.Semester, objRptCardMYP.AcademicYear);
                    objRptCardMYP.RptCardFocus = ObjRptCardFocus;
                    #endregion "Final Report Card Integration"

                    #region "PDF File Properties - meta information"
                    var DocRptCrd6G1Sem = new iTextSharp.text.Document();
                    MemoryStream pdfStream = new MemoryStream();
                    PdfWriter pdfWriter = PdfWriter.GetInstance(DocRptCrd6G1Sem, pdfStream);

                    DocRptCrd6G1Sem.AddCreator(Resources.Global.tips);

                    DocRptCrd6G1Sem.AddTitle("Achievement Report");
                    DocRptCrd6G1Sem.AddAuthor(Resources.Global.tips);

                    DocRptCrd6G1Sem.AddSubject("Achievement Report of " + objRptCardMYP.Grade + " Grade " + objRptCardMYP.Semester + " - Semeter");
                    DocRptCrd6G1Sem.AddKeywords(objRptCardMYP.Name + " Achievement Report");
                    #endregion "PDF File Properties - meta information"

                    DocRptCrd6G1Sem.Open();

                    #region "PDF Header"
                    PdfPTable table1 = CreateTable(4, new int[] { 12, 33, 45, 10 });


                    #region "Logo Image"

                    iTextSharp.text.Image LogonaceImage;
                    string LogonaceImagePath = ConfigurationManager.AppSettings["AppLogos"] + "logonace.jpg";
                    LogonaceImage = iTextSharp.text.Image.GetInstance(LogonaceImagePath);
                    LogonaceImage.ScaleAbsolute(50, 50);

                    iTextSharp.text.Image LogoImage;
                    string ImagePath = ConfigurationManager.AppSettings["RptCard"] + "logo.jpg";
                    LogoImage = iTextSharp.text.Image.GetInstance(ImagePath);
                    LogoImage.ScaleAbsolute(50, 50);

                    PdfPCell imgcel1 = new PdfPCell();
                    imgcel1.PaddingTop = 2;
                    imgcel1.Border = 0;
                    imgcel1.AddElement(LogonaceImage);
                    imgcel1.PaddingLeft = 1;
                    table1.AddCell(imgcel1);

                    #endregion "Logo Image"

                    PdfPCell cell2 = formatCell("THE INDIAN PUBLIC SCHOOL, " + GetCampusNameByCampusCode(objRptCardMYP.Campus) + "          ", "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 5, 2, 0);
                    cell2.Border = 0;
                    table1.AddCell(cell2);

                    PdfPCell imgcel2 = new PdfPCell();
                    imgcel2.PaddingTop = 2;
                    imgcel2.Border = 0;
                    imgcel2.AddElement(LogoImage);
                    imgcel2.PaddingLeft = 1;
                    imgcel2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table1.AddCell(imgcel2);

                    PdfPTable table2 = CreateTable(1, new int[] { 100 });

                    PdfPCell cell = formatCell("SEMESTER " + (objRptCardMYP.Semester == 1 ? "I" : "II") + " - SECONDARY ACHIEVEMENT REPORT                ", "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 15, 0, 0);
                    cell.Border = 0;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;

                    table2.AddCell(cell);

                    #endregion "PDF Header"

                    #region "Basic Details"

                    PdfPTable table3 = CreateTable(4, new int[] { 25, 25, 22, 28 });

                    PdfPCell cellStdnt = formatCell("Student : " + objRptCardMYP.Name, "Verdana", 10.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 5, 5, 3, 0);
                    PdfPCell cellTchr = formatCell("Teacher : " + objRptCardMYP.TeacherName, "Verdana", 10.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 5, 5, 0, 0);
                    PdfPCell cellGrd = formatCell("Grade : " + objRptCardMYP.Grade + " " + objRptCardMYP.Section, "Verdana", 10.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 5, 5, 3, 0);
                    PdfPCell cellDt = formatCell("Date : " + String.Format("{0:dd.MM.yyyy}", objRptCardMYP.RptDate), "Verdana", 10.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 5, 5, 0, 0);

                    cellStdnt.Border = PdfPCell.LEFT_BORDER | PdfPCell.TOP_BORDER;
                    cellTchr.Border = PdfPCell.RIGHT_BORDER | PdfPCell.TOP_BORDER;
                    cellGrd.Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER;
                    cellDt.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;

                    cellStdnt.BorderWidth = 1f;
                    cellTchr.BorderWidth = 1f;
                    cellGrd.BorderWidth = 1f;
                    cellDt.BorderWidth = 1f;

                    table3.AddCell(cellStdnt);
                    table3.AddCell(cellTchr);
                    table3.AddCell(cellGrd);
                    table3.AddCell(cellDt);

                    DocRptCrd6G1Sem.Add(table1);
                    DocRptCrd6G1Sem.Add(table2);
                    DocRptCrd6G1Sem.Add(table3);

                    #endregion "Basic Details"
                    float spcAfter = 5f;
                    float spcBefore = 10f;
                    float cellFxdHeight = 50f;

                    #region "Subject Grades Table"
                    Paragraph pSubjHdr = new Paragraph("SUBJECT GRADES", new iTextSharp.text.Font(FontFactory.GetFont("Verdana", 10.0f, iTextSharp.text.Font.BOLD)));
                    pSubjHdr.Alignment = Element.ALIGN_CENTER;
                    pSubjHdr.SpacingAfter = spcAfter;
                    pSubjHdr.SpacingBefore = spcBefore;
                    DocRptCrd6G1Sem.Add(pSubjHdr);

                    PdfPTable tSubjGrd = CreateTable(9, new int[] { 11, 10, 10, 14, 10, 11, 11, 12, 11 });

                    //Table Design 1st row
                    tSubjGrd.AddCell(formatCell("Subjects", "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 5, 5, 0, 0));
                    tSubjGrd.AddCell(formatCell("English", "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 5, 5, 0, 0));
                    tSubjGrd.AddCell(formatCell(objRptCardMYP.Second_Language, "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 5, 5, 0, 0));
                    tSubjGrd.AddCell(formatCell("Mathematics", "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 5, 5, 0, 0));
                    tSubjGrd.AddCell(formatCell("Physics", "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 5, 5, 0, 0));
                    tSubjGrd.AddCell(formatCell("Chemistry", "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 5, 5, 0, 0));
                    tSubjGrd.AddCell(formatCell("Biology", "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 5, 5, 0, 0));
                    //tSubjGrd.AddCell(formatCell("History & Geography", "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 5, 5, 0, 0));
                    if ((objRptCardMYP.Grade == "IX" || objRptCardMYP.Grade == "X") && objRptCardMYP.Campus == "TIPS ERODE")
                    {
                        tSubjGrd.AddCell(formatCell("Economics", "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 5, 5, 0, 0));
                    }
                    else
                    {
                        tSubjGrd.AddCell(formatCell("History & Geography ", "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 5, 5, 0, 0));
                    }
                    tSubjGrd.AddCell(formatCell("ICT", "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 5, 5, 0, 0));

                    //Table Design 2nd row
                    tSubjGrd.AddCell(formatCell("Grades", "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 5, 5, 0, 0));
                    tSubjGrd.AddCell(formatCell(objRptCardMYP.SubjGrd_Eng, "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 5, 5, 0, 0));
                    tSubjGrd.AddCell(formatCell(objRptCardMYP.SubjGrd_Second_Lang, "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 5, 5, 0, 0));
                    tSubjGrd.AddCell(formatCell(objRptCardMYP.SubjGrd_Maths, "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 5, 5, 0, 0));
                    tSubjGrd.AddCell(formatCell(objRptCardMYP.SubjGrd_Phys, "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 5, 5, 0, 0));
                    tSubjGrd.AddCell(formatCell(objRptCardMYP.SubjGrd_Chstr, "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 5, 5, 0, 0));
                    tSubjGrd.AddCell(formatCell(objRptCardMYP.SubjGrd_Bio, "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 5, 5, 0, 0));
                    tSubjGrd.AddCell(formatCell(objRptCardMYP.SubjGrd_HisGeo, "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 5, 5, 0, 0));
                    tSubjGrd.AddCell(formatCell(objRptCardMYP.SubjGrd_ICT, "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 5, 5, 0, 0));

                    // add table to document
                    DocRptCrd6G1Sem.Add(tSubjGrd);
                    #endregion

                    if (objRptCardMYP.Grade == "IX" || objRptCardMYP.Grade == "X")
                    {
                        switch (objRptCardMYP.Section)
                        {
                            case "A":
                            case "B":
                            case "A1":
                            case "A2":
                                ViewBag.CheckFlag = "AB";
                                break;

                            case "C":
                            case "D":
                            case "S1":
                            case "S2":
                                ViewBag.CheckFlag = "CD";
                                break;
                        }


                        #region "Robotics"
                        Paragraph pRoboHdr = new Paragraph("ROBOTICS", new iTextSharp.text.Font(FontFactory.GetFont("Verdana", 10.0f, iTextSharp.text.Font.BOLD)));
                        pRoboHdr.Alignment = Element.ALIGN_CENTER;
                        pRoboHdr.SpacingAfter = spcAfter;
                        pRoboHdr.SpacingBefore = spcBefore;
                        DocRptCrd6G1Sem.Add(pRoboHdr);

                        PdfPTable tRoboSubj = CreateTable(5, new int[] { 15, 21, 20, 22, 22 });

                        tRoboSubj.AddCell(formatCell("Semester Focus", "Verdana", 10.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        //tRoboSubj.AddCell(formatCell(objRptCardMYP.RptCardFocus.Robotics, "Verdana", 10.0f, false, false, Element.ALIGN_JUSTIFIED, Element.ALIGN_MIDDLE, 5, 5, 5, 0));
                        PdfPCell roboFcus = formatCell(objRptCardMYP.RptCardFocus.Robotics, "Verdana", 10.0f, false, false, Element.ALIGN_JUSTIFIED, Element.ALIGN_MIDDLE, 2, 2, 7, 0);
                        roboFcus.FixedHeight = cellFxdHeight;
                        tRoboSubj.AddCell(roboFcus);

                        tRoboSubj.AddCell(formatCell("Evaluation Criteria", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tRoboSubj.AddCell(formatCell("Knowledge & Understanding", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tRoboSubj.AddCell(formatCell("Building & Design", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tRoboSubj.AddCell(formatCell("Programming/Activity Sheet", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tRoboSubj.AddCell(formatCell("Team Participation", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        tRoboSubj.AddCell(formatCell("Achievement", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tRoboSubj.AddCell(formatCell(objRptCardMYP.Rbt_Ach_KnwdUndstnd, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tRoboSubj.AddCell(formatCell(objRptCardMYP.Rbt_Ach_BldngDsgn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tRoboSubj.AddCell(formatCell(objRptCardMYP.Rbt_Ach_Progm, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tRoboSubj.AddCell(formatCell(objRptCardMYP.Rbt_Ach_WrkgInTam, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        tRoboSubj.AddCell(formatCell("Effort", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tRoboSubj.AddCell(formatCell(objRptCardMYP.Rbt_Efrt_KnwdUndstnd, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tRoboSubj.AddCell(formatCell(objRptCardMYP.Rbt_Efrt_BldngDsgn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tRoboSubj.AddCell(formatCell(objRptCardMYP.Rbt_Efrt_Progm, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tRoboSubj.AddCell(formatCell(objRptCardMYP.Rbt_Efrt_WrkgInTam, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        DocRptCrd6G1Sem.Add(tRoboSubj);
                        #endregion

                        if (objRptCardMYP.Campus != "TIPS ERODE")
                        {
                            #region "Spark"
                            Paragraph sparkHdr = new Paragraph("SPARK", new iTextSharp.text.Font(FontFactory.GetFont("Verdana", 10.0f, iTextSharp.text.Font.BOLD)));
                            sparkHdr.Alignment = Element.ALIGN_CENTER;
                            sparkHdr.SpacingAfter = spcAfter;
                            sparkHdr.SpacingBefore = spcBefore;
                            DocRptCrd6G1Sem.Add(sparkHdr);

                            PdfPTable tSparkSubj = CreateTable(5, new int[] { 15, 21, 20, 22, 22 });

                            tSparkSubj.AddCell(formatCell("Semester Focus", "Verdana", 10.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            //tRoboSubj.AddCell(formatCell(objRptCardMYP.RptCardFocus.Robotics, "Verdana", 10.0f, false, false, Element.ALIGN_JUSTIFIED, Element.ALIGN_MIDDLE, 5, 5, 5, 0));
                            PdfPCell sparkFcus = formatCell(objRptCardMYP.RptCardFocus.spark, "Verdana", 10.0f, false, false, Element.ALIGN_JUSTIFIED, Element.ALIGN_MIDDLE, 2, 2, 7, 0);
                            sparkFcus.FixedHeight = cellFxdHeight;
                            tSparkSubj.AddCell(sparkFcus);

                            tSparkSubj.AddCell(formatCell("Evaluation Criteria", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tSparkSubj.AddCell(formatCell("Experimental Skill", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tSparkSubj.AddCell(formatCell("Team Work", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tSparkSubj.AddCell(formatCell("Attitude for Learning", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tSparkSubj.AddCell(formatCell("Written Task", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                            tSparkSubj.AddCell(formatCell("Achievement", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tSparkSubj.AddCell(formatCell(objRptCardMYP.spark_Eval_ExperiSkil, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tSparkSubj.AddCell(formatCell(objRptCardMYP.spark_Eval_TeamWork, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tSparkSubj.AddCell(formatCell(objRptCardMYP.spark_Eval_AttforLearn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tSparkSubj.AddCell(formatCell(objRptCardMYP.spark_Eval_WrittenTask, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                            DocRptCrd6G1Sem.Add(tSparkSubj);
                            #endregion
                        }

                        if (ViewBag.CheckFlag == "CD")
                        {

                            #region "Physical Education"
                            Paragraph pPEHdr = new Paragraph("PHYSICAL EDUCATION", new iTextSharp.text.Font(FontFactory.GetFont("Verdana", 10.0f, iTextSharp.text.Font.BOLD)));
                            pPEHdr.Alignment = Element.ALIGN_CENTER;
                            pPEHdr.SpacingAfter = spcAfter;
                            pPEHdr.SpacingBefore = spcBefore;
                            DocRptCrd6G1Sem.Add(pPEHdr);

                            PdfPTable tPESubj = CreateTable(5, new int[] { 15, 21, 20, 22, 22 });

                            tPESubj.AddCell(formatCell("Evaluation Criteria", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tPESubj.AddCell(formatCell("Behaviour", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tPESubj.AddCell(formatCell("Team work", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tPESubj.AddCell(formatCell("Basic Skill", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tPESubj.AddCell(formatCell("Fitness", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                            tPESubj.AddCell(formatCell("Achievement", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tPESubj.AddCell(formatCell(objRptCardMYP.PhysEd_Ach_Bhvr, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tPESubj.AddCell(formatCell(objRptCardMYP.PhysEd_Ach_TmWrk, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tPESubj.AddCell(formatCell(objRptCardMYP.PhysEd_Ach_BscSkl, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tPESubj.AddCell(formatCell(objRptCardMYP.PhysEd_Ach_Ftns, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                            tPESubj.AddCell(formatCell("Effort", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tPESubj.AddCell(formatCell(objRptCardMYP.PhysEd_Efrt_Bhvr, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tPESubj.AddCell(formatCell(objRptCardMYP.PhysEd_Efrt_TmWrk, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tPESubj.AddCell(formatCell(objRptCardMYP.PhysEd_Efrt_BscSkl, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tPESubj.AddCell(formatCell(objRptCardMYP.PhysEd_Efrt_Ftns, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                            DocRptCrd6G1Sem.Add(tPESubj);
                            #endregion

                            //DocRptCrd6G1Sem.NewPage();
                            #region "Fine Arts"
                            Paragraph pFAHdr = new Paragraph("FINE ARTS", new iTextSharp.text.Font(FontFactory.GetFont("Verdana", 10.0f, iTextSharp.text.Font.BOLD)));
                            pFAHdr.Alignment = Element.ALIGN_CENTER;
                            pFAHdr.SpacingAfter = spcAfter;
                            pFAHdr.SpacingBefore = spcBefore;
                            DocRptCrd6G1Sem.Add(pFAHdr);

                            PdfPTable tFASubj = CreateTable(7, new int[] { 15, 14, 14, 14, 14, 14, 15 });

                            tFASubj.AddCell(formatCell("Evaluation Criteria", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 2));
                            tFASubj.AddCell(formatCell("Music", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 3, 0));
                            tFASubj.AddCell(formatCell("Dance", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 3, 0));

                            tFASubj.AddCell(formatCell("Participation", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tFASubj.AddCell(formatCell("Behaviour", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tFASubj.AddCell(formatCell("Shows knowledge of unit studies", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tFASubj.AddCell(formatCell("Skills/ Technique", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tFASubj.AddCell(formatCell("Problem solving", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tFASubj.AddCell(formatCell("Collaboration", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                            tFASubj.AddCell(formatCell("Achievement", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tFASubj.AddCell(formatCell(objRptCardMYP.FnArt_Musc_Ach_Prtcpn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tFASubj.AddCell(formatCell(objRptCardMYP.FnArt_Musc_Ach_Bhvr, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tFASubj.AddCell(formatCell(objRptCardMYP.FnArt_Musc_Ach_ShwKwdUntStuds, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tFASubj.AddCell(formatCell(objRptCardMYP.FnArt_Dnc_Ach_SklTech, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tFASubj.AddCell(formatCell(objRptCardMYP.FnArt_Dnc_Ach_PrblmSlvng, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tFASubj.AddCell(formatCell(objRptCardMYP.FnArt_Dnc_Ach_Clbrtn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                            tFASubj.AddCell(formatCell("Effort", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tFASubj.AddCell(formatCell(objRptCardMYP.FnArt_Musc_Efrt_Prtcpn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tFASubj.AddCell(formatCell(objRptCardMYP.FnArt_Musc_Efrt_Bhvr, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tFASubj.AddCell(formatCell(objRptCardMYP.FnArt_Musc_Efrt_ShwKwdUntStuds, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tFASubj.AddCell(formatCell(objRptCardMYP.FnArt_Dnc_Efrt_SklTech, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tFASubj.AddCell(formatCell(objRptCardMYP.FnArt_Dnc_Efrt_PrblmSlvng, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tFASubj.AddCell(formatCell(objRptCardMYP.FnArt_Dnc_Efrt_Clbrtn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                            DocRptCrd6G1Sem.Add(tFASubj);
                            #endregion
                        }

                    }
                    else
                    {

                        #region "English Table"
                        Paragraph pEngHdr = new Paragraph("ENGLISH", new iTextSharp.text.Font(FontFactory.GetFont("Verdana", 10.0f, iTextSharp.text.Font.BOLD)));
                        pEngHdr.Alignment = Element.ALIGN_CENTER;
                        pEngHdr.SpacingAfter = spcAfter;
                        pEngHdr.SpacingBefore = spcBefore;
                        DocRptCrd6G1Sem.Add(pEngHdr);

                        PdfPTable tEngSubj = CreateTable(8, new int[] { 10, 13, 13, 13, 13, 13, 12, 13 });

                        tEngSubj.AddCell(formatCell("Semester Focus", "Verdana", 10.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        //tEngSubj.AddCell(formatCell(objRptCardMYP.RptCardFocus.English, "Verdana", 10.0f, false, false, Element.ALIGN_JUSTIFIED, Element.ALIGN_MIDDLE, 10, 10, 7, 0));
                        PdfPCell engFcus = formatCell(objRptCardMYP.RptCardFocus.English, "Verdana", 10.0f, false, false, Element.ALIGN_JUSTIFIED, Element.ALIGN_MIDDLE, 2, 2, 7, 0);
                        //PdfPCell engFcus = formatCell(" English Focus Line 1 \n English Focus Line 2 \n English Focus Line 3 \n English Focus Line 4", "Verdana", 10.0f, false, false, Element.ALIGN_JUSTIFIED, Element.ALIGN_MIDDLE, 2, 2, 7, 0);
                        engFcus.FixedHeight = cellFxdHeight;
                        tEngSubj.AddCell(engFcus);
                        tEngSubj.AddCell(formatCell("Writing", "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 7, 7, 8, 0));

                        tEngSubj.AddCell(formatCell("Evaluation Criteria", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tEngSubj.AddCell(formatCell("Content", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tEngSubj.AddCell(formatCell("Audience", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tEngSubj.AddCell(formatCell("Purpose", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tEngSubj.AddCell(formatCell("Text Structure", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tEngSubj.AddCell(formatCell("Sentence Structure", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tEngSubj.AddCell(formatCell("Vocabulary", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tEngSubj.AddCell(formatCell("Spelling", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        tEngSubj.AddCell(formatCell("Skill", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tEngSubj.AddCell(formatCell(objRptCardMYP.Eng_WrtSkl_Ach_Cntnt, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tEngSubj.AddCell(formatCell(objRptCardMYP.Eng_WrtSkl_Ach_Audn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tEngSubj.AddCell(formatCell(objRptCardMYP.Eng_WrtSkl_Ach_Purps, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tEngSubj.AddCell(formatCell(objRptCardMYP.Eng_WrtSkl_Ach_TxtStr, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tEngSubj.AddCell(formatCell(objRptCardMYP.Eng_WrtSkl_Ach_SenStr, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tEngSubj.AddCell(formatCell(objRptCardMYP.Eng_WrtSkl_Ach_Vocblry, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tEngSubj.AddCell(formatCell(objRptCardMYP.Eng_WrtSkl_Ach_Splng, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        tEngSubj.AddCell(formatCell("Effort", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tEngSubj.AddCell(formatCell(objRptCardMYP.Eng_WrtSkl_Efrt_Cntnt, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tEngSubj.AddCell(formatCell(objRptCardMYP.Eng_WrtSkl_Efrt_Audn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tEngSubj.AddCell(formatCell(objRptCardMYP.Eng_WrtSkl_Efrt_Purps, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tEngSubj.AddCell(formatCell(objRptCardMYP.Eng_WrtSkl_Efrt_TxtStr, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tEngSubj.AddCell(formatCell(objRptCardMYP.Eng_WrtSkl_Efrt_SenStr, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tEngSubj.AddCell(formatCell(objRptCardMYP.Eng_WrtSkl_Efrt_Vocblry, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tEngSubj.AddCell(formatCell(objRptCardMYP.Eng_WrtSkl_Efrt_Splng, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        DocRptCrd6G1Sem.Add(tEngSubj);

                        PdfPTable t2EngSubj = CreateTable(5, new int[] { 10, 23, 23, 22, 22 });

                        t2EngSubj.AddCell(formatCell("Other Areas in Writing", "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 7, 7, 5, 0));
                        t2EngSubj.AddCell(formatCell("Evaluation Criteria", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2EngSubj.AddCell(formatCell("Comprehension", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2EngSubj.AddCell(formatCell("Grammar", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2EngSubj.AddCell(formatCell("Punctuation", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2EngSubj.AddCell(formatCell("Narrative Elements", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        t2EngSubj.AddCell(formatCell("Skill", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2EngSubj.AddCell(formatCell(objRptCardMYP.Eng_OAW_Skl_Cmprsn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2EngSubj.AddCell(formatCell(objRptCardMYP.Eng_OAW_Skl_Grmr, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2EngSubj.AddCell(formatCell(objRptCardMYP.Eng_OAW_Skl_Punctn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2EngSubj.AddCell(formatCell(objRptCardMYP.Eng_OAW_Skl_NartvElmnts, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        t2EngSubj.AddCell(formatCell("Effort", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2EngSubj.AddCell(formatCell(objRptCardMYP.Eng_OAW_Efrt_Cmprsn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2EngSubj.AddCell(formatCell(objRptCardMYP.Eng_OAW_Efrt_Grmr, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2EngSubj.AddCell(formatCell(objRptCardMYP.Eng_OAW_Efrt_Punctn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2EngSubj.AddCell(formatCell(objRptCardMYP.Eng_OAW_Efrt_NartvElmnts, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        DocRptCrd6G1Sem.Add(t2EngSubj);

                        PdfPTable t3EngSubj = CreateTable(6, new int[] { 10, 18, 18, 18, 18, 18 });

                        t3EngSubj.AddCell(formatCell("Listening and Speaking Skills", "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 7, 7, 6, 0));
                        t3EngSubj.AddCell(formatCell("Evaluation Criteria", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t3EngSubj.AddCell(formatCell("Confidence", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t3EngSubj.AddCell(formatCell("Fluency", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t3EngSubj.AddCell(formatCell("Pronunciation", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t3EngSubj.AddCell(formatCell("Understanding", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t3EngSubj.AddCell(formatCell("Vocabulary", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        t3EngSubj.AddCell(formatCell("Skill", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t3EngSubj.AddCell(formatCell(objRptCardMYP.Eng_LSS_Skl_Confdnc, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t3EngSubj.AddCell(formatCell(objRptCardMYP.Eng_LSS_Skl_Flncy, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t3EngSubj.AddCell(formatCell(objRptCardMYP.Eng_LSS_Skl_Pronctn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t3EngSubj.AddCell(formatCell(objRptCardMYP.Eng_LSS_Skl_udstnd, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t3EngSubj.AddCell(formatCell(objRptCardMYP.Eng_LSS_Skl_vcblry, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        t3EngSubj.AddCell(formatCell("Effort", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t3EngSubj.AddCell(formatCell(objRptCardMYP.Eng_LSS_Efrt_Confdnc, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t3EngSubj.AddCell(formatCell(objRptCardMYP.Eng_LSS_Efrt_Flncy, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t3EngSubj.AddCell(formatCell(objRptCardMYP.Eng_LSS_Efrt_Pronctn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t3EngSubj.AddCell(formatCell(objRptCardMYP.Eng_LSS_Efrt_udstnd, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t3EngSubj.AddCell(formatCell(objRptCardMYP.Eng_LSS_Efrt_vcblry, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        DocRptCrd6G1Sem.Add(t3EngSubj);

                        #endregion "English Table"

                        DocRptCrd6G1Sem.NewPage();
                        #region "Second language"
                        Paragraph pFrnchHdr = new Paragraph(objRptCardMYP.Second_Language != null ? objRptCardMYP.Second_Language.ToUpper() : "", new iTextSharp.text.Font(FontFactory.GetFont("Verdana", 10.0f, iTextSharp.text.Font.BOLD)));
                        pFrnchHdr.Alignment = Element.ALIGN_CENTER;
                        pFrnchHdr.SpacingAfter = spcAfter;
                        pFrnchHdr.SpacingBefore = spcBefore;
                        DocRptCrd6G1Sem.Add(pFrnchHdr);

                        PdfPTable tFrnchSubj = CreateTable(7, new int[] { 10, 13, 14, 14, 13, 14, 14 });
                        string SecondLngFocus = "";
                        if (objRptCardMYP.Second_Language == "Hindi")
                        {
                            SecondLngFocus = objRptCardMYP.RptCardFocus.Hindi;
                        }
                        else
                        {
                            SecondLngFocus = objRptCardMYP.RptCardFocus.French;
                        }

                        tFrnchSubj.AddCell(formatCell("Semester Focus", "Verdana", 10.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        //tFrnchSubj.AddCell(formatCell(SecondLngFocus, "Verdana", 10.0f, false, false, Element.ALIGN_JUSTIFIED, Element.ALIGN_MIDDLE, 5, 5, 6, 0));
                        PdfPCell ScndFcus = formatCell(SecondLngFocus, "Verdana", 10.0f, false, false, Element.ALIGN_JUSTIFIED, Element.ALIGN_MIDDLE, 2, 2, 7, 0);
                        ScndFcus.FixedHeight = cellFxdHeight;
                        tFrnchSubj.AddCell(ScndFcus);
                        tFrnchSubj.AddCell(formatCell("Writing Skill", "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 7, 7, 7, 0));

                        tFrnchSubj.AddCell(formatCell("Evaluation Criteria", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFrnchSubj.AddCell(formatCell("Spelling", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFrnchSubj.AddCell(formatCell("Vocabulary", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFrnchSubj.AddCell(formatCell("Punctuation", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFrnchSubj.AddCell(formatCell("Grammar", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFrnchSubj.AddCell(formatCell("Ideas / Content", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFrnchSubj.AddCell(formatCell("Neatness & Handwriting", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        tFrnchSubj.AddCell(formatCell("Skill", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_WrtSkl_Skl_Splng, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_WrtSkl_Skl_Vocblry, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_WrtSkl_Skl_Punctn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_WrtSkl_Skl_Grmr, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_WrtSkl_Skl_IdsCntnt, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_WrtSkl_Skl_NetnsHndwrt, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        tFrnchSubj.AddCell(formatCell("Effort", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_WrtSkl_Efrt_Splng, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_WrtSkl_Efrt_Vocblry, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_WrtSkl_Efrt_Punctn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_WrtSkl_Efrt_Grmr, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_WrtSkl_Efrt_IdsCntnt, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_WrtSkl_Efrt_NetnsHndwrt, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        DocRptCrd6G1Sem.Add(tFrnchSubj);

                        PdfPTable t2FrnchSubj = CreateTable(6, new int[] { 10, 16, 16, 16, 16, 16 });

                        t2FrnchSubj.AddCell(formatCell("Reading Skill", "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 7, 7, 6, 0));

                        t2FrnchSubj.AddCell(formatCell("Evaluation Criteria", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell("Confidence", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell("Fluency", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell("Tone & Intonation", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell("Pronunciation", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell("Understanding", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        t2FrnchSubj.AddCell(formatCell("Skill", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_RdgSkl_Skl_Confdnc, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_RdgSkl_Skl_Flncy, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_RdgSkl_Skl_TonInTon, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_RdgSkl_Skl_Pronctn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_RdgSkl_Skl_udstnd, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        t2FrnchSubj.AddCell(formatCell("Effort", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_RdgSkl_Efrt_Confdnc, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_RdgSkl_Efrt_Flncy, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_RdgSkl_Efrt_TonInTon, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_RdgSkl_Efrt_Pronctn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_RdgSkl_Efrt_udstnd, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        t2FrnchSubj.AddCell(formatCell("Listening and Speaking Skills", "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 7, 7, 6, 0));
                        t2FrnchSubj.AddCell(formatCell("Evaluation Criteria", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell("Confidence", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell("Fluency", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell("Pronunciation", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell("Understanding", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell("Vocabulary", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        t2FrnchSubj.AddCell(formatCell("Skill", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_LSS_Skl_Confdnc, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_LSS_Skl_Flncy, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_LSS_Skl_Pronctn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_LSS_Skl_udstnd, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_LSS_Skl_Vocblry, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        t2FrnchSubj.AddCell(formatCell("Effort", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_LSS_Efrt_Confdnc, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_LSS_Efrt_Flncy, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_LSS_Efrt_Pronctn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_LSS_Efrt_udstnd, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        t2FrnchSubj.AddCell(formatCell(objRptCardMYP.Second_Lng_LSS_Efrt_Vocblry, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        DocRptCrd6G1Sem.Add(t2FrnchSubj);
                        #endregion

                        //DocRptCrd6G1Sem.NewPage();

                        #region "Mathematics"
                        Paragraph pMathsHdr = new Paragraph("MATHEMATICS", new iTextSharp.text.Font(FontFactory.GetFont("Verdana", 10.0f, iTextSharp.text.Font.BOLD)));
                        pMathsHdr.Alignment = Element.ALIGN_CENTER;
                        pMathsHdr.SpacingAfter = spcAfter;
                        pMathsHdr.SpacingBefore = spcBefore;
                        DocRptCrd6G1Sem.Add(pMathsHdr);

                        PdfPTable tMathsSubj = CreateTable(6, new int[] { 15, 18, 18, 17, 16, 16 });

                        tMathsSubj.AddCell(formatCell("Semester Focus", "Verdana", 10.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        //tMathsSubj.AddCell(formatCell(objRptCardMYP.RptCardFocus.Maths, "Verdana", 10.0f, false, false, Element.ALIGN_JUSTIFIED, Element.ALIGN_MIDDLE, 5, 5, 6, 0));
                        PdfPCell mathsFcus = formatCell(objRptCardMYP.RptCardFocus.Maths, "Verdana", 10.0f, false, false, Element.ALIGN_JUSTIFIED, Element.ALIGN_MIDDLE, 2, 2, 7, 0);
                        mathsFcus.FixedHeight = cellFxdHeight;
                        tMathsSubj.AddCell(mathsFcus);

                        tMathsSubj.AddCell(formatCell("Evaluation Criteria", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tMathsSubj.AddCell(formatCell("Knowledge & Understanding", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tMathsSubj.AddCell(formatCell("Usage of appropriate mathematical representation", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tMathsSubj.AddCell(formatCell("Problem Solving", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tMathsSubj.AddCell(formatCell("Logical & Analytical Thinking", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tMathsSubj.AddCell(formatCell("Use of Technology", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        tMathsSubj.AddCell(formatCell("Achievement", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tMathsSubj.AddCell(formatCell(objRptCardMYP.Maths_Ach_KnwdUndstnd, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tMathsSubj.AddCell(formatCell(objRptCardMYP.Maths_Ach_UsgApprMathdsRespn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tMathsSubj.AddCell(formatCell(objRptCardMYP.Maths_Ach_PrblmSlvng, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tMathsSubj.AddCell(formatCell(objRptCardMYP.Maths_Ach_TechSkill, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tMathsSubj.AddCell(formatCell(objRptCardMYP.Maths_Ach_MntlAblty, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        tMathsSubj.AddCell(formatCell("Effort", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tMathsSubj.AddCell(formatCell(objRptCardMYP.Maths_Efrt_KnwdUndstnd, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tMathsSubj.AddCell(formatCell(objRptCardMYP.Maths_Efrt_UsgApprMathdsRespn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tMathsSubj.AddCell(formatCell(objRptCardMYP.Maths_Efrt_PrblmSlvng, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tMathsSubj.AddCell(formatCell(objRptCardMYP.Maths_Efrt_TechSkill, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tMathsSubj.AddCell(formatCell(objRptCardMYP.Maths_Efrt_MntlAblty, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        DocRptCrd6G1Sem.Add(tMathsSubj);
                        #endregion

                        //DocRptCrd6G1Sem.NewPage();
                        #region "Physics"
                        Paragraph pPhysHdr = new Paragraph("PHYSICS", new iTextSharp.text.Font(FontFactory.GetFont("Verdana", 10.0f, iTextSharp.text.Font.BOLD)));
                        pPhysHdr.Alignment = Element.ALIGN_CENTER;
                        pPhysHdr.SpacingAfter = spcAfter;
                        pPhysHdr.SpacingBefore = spcBefore;
                        DocRptCrd6G1Sem.Add(pPhysHdr);

                        PdfPTable tPhysSubj = CreateTable(7, new int[] { 14, 15, 14, 14, 14, 14, 15 });

                        tPhysSubj.AddCell(formatCell("Semester Focus", "Verdana", 10.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        //tPhysSubj.AddCell(formatCell(objRptCardMYP.RptCardFocus.Physics, "Verdana", 10.0f, false, false, Element.ALIGN_JUSTIFIED, Element.ALIGN_MIDDLE, 5, 5, 7, 0));
                        PdfPCell PhysFcus = formatCell(objRptCardMYP.RptCardFocus.Physics, "Verdana", 10.0f, false, false, Element.ALIGN_JUSTIFIED, Element.ALIGN_MIDDLE, 2, 2, 7, 0);
                        PhysFcus.FixedHeight = cellFxdHeight;
                        tPhysSubj.AddCell(PhysFcus);

                        tPhysSubj.AddCell(formatCell("Evaluation Criteria", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPhysSubj.AddCell(formatCell("Recall of Scientific Information", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPhysSubj.AddCell(formatCell("Usage of appropriate scientific languages", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPhysSubj.AddCell(formatCell("Scientific Inquiry", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPhysSubj.AddCell(formatCell("Scientific Application", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPhysSubj.AddCell(formatCell("Interpretation of scientific data", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPhysSubj.AddCell(formatCell("Technical skills", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        tPhysSubj.AddCell(formatCell("Achievement", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPhysSubj.AddCell(formatCell(objRptCardMYP.Phys_Ach_RclScnInfo, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPhysSubj.AddCell(formatCell(objRptCardMYP.Phys_Ach_UsgApprScnficLang, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPhysSubj.AddCell(formatCell(objRptCardMYP.Phys_Ach_ScnficInq, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPhysSubj.AddCell(formatCell(objRptCardMYP.Phys_Ach_ScnficAppn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPhysSubj.AddCell(formatCell(objRptCardMYP.Phys_Ach_IntrptnScnficData, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPhysSubj.AddCell(formatCell(objRptCardMYP.Phys_Ach_TechSkill, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        tPhysSubj.AddCell(formatCell("Effort", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPhysSubj.AddCell(formatCell(objRptCardMYP.Phys_Efrt_RclScnInfo, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPhysSubj.AddCell(formatCell(objRptCardMYP.Phys_Efrt_UsgApprScnficLang, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPhysSubj.AddCell(formatCell(objRptCardMYP.Phys_Efrt_ScnficInq, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPhysSubj.AddCell(formatCell(objRptCardMYP.Phys_Efrt_ScnficAppn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPhysSubj.AddCell(formatCell(objRptCardMYP.Phys_Efrt_IntrptnScnficData, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPhysSubj.AddCell(formatCell(objRptCardMYP.Phys_Efrt_TechSkill, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        DocRptCrd6G1Sem.Add(tPhysSubj);
                        #endregion

                        DocRptCrd6G1Sem.NewPage();
                        #region "Chemistry"
                        Paragraph pCmstrHdr = new Paragraph("CHEMISTRY", new iTextSharp.text.Font(FontFactory.GetFont("Verdana", 10.0f, iTextSharp.text.Font.BOLD)));
                        pCmstrHdr.Alignment = Element.ALIGN_CENTER;
                        pCmstrHdr.SpacingAfter = spcAfter;
                        pCmstrHdr.SpacingBefore = spcBefore;
                        DocRptCrd6G1Sem.Add(pCmstrHdr);

                        PdfPTable tCmstrSubj = CreateTable(7, new int[] { 14, 15, 14, 14, 14, 14, 15 });

                        tCmstrSubj.AddCell(formatCell("Semester Focus", "Verdana", 10.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        //tCmstrSubj.AddCell(formatCell(objRptCardMYP.RptCardFocus.Chemistry, "Verdana", 10.0f, false, false, Element.ALIGN_JUSTIFIED, Element.ALIGN_MIDDLE, 5, 5, 7, 0));
                        PdfPCell CmstrFcus = formatCell(objRptCardMYP.RptCardFocus.Chemistry, "Verdana", 10.0f, false, false, Element.ALIGN_JUSTIFIED, Element.ALIGN_MIDDLE, 2, 2, 7, 0);
                        CmstrFcus.FixedHeight = cellFxdHeight;
                        tCmstrSubj.AddCell(CmstrFcus);

                        tCmstrSubj.AddCell(formatCell("Evaluation Criteria", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tCmstrSubj.AddCell(formatCell("Recall of Scientific Information", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tCmstrSubj.AddCell(formatCell("Usage of appropriate scientific languages", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tCmstrSubj.AddCell(formatCell("Scientific Inquiry", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tCmstrSubj.AddCell(formatCell("Scientific Application", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tCmstrSubj.AddCell(formatCell("Interpretation of scientific data", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tCmstrSubj.AddCell(formatCell("Technical skills", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        tCmstrSubj.AddCell(formatCell("Achievement", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tCmstrSubj.AddCell(formatCell(objRptCardMYP.Chstr_Ach_RclScnInfo, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tCmstrSubj.AddCell(formatCell(objRptCardMYP.Chstr_Ach_UsgApprScnficLang, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tCmstrSubj.AddCell(formatCell(objRptCardMYP.Chstr_Ach_ScnficInq, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tCmstrSubj.AddCell(formatCell(objRptCardMYP.Chstr_Ach_ScnficAppn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tCmstrSubj.AddCell(formatCell(objRptCardMYP.Chstr_Ach_IntrptnScnficData, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tCmstrSubj.AddCell(formatCell(objRptCardMYP.Chstr_Ach_TechSkill, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        tCmstrSubj.AddCell(formatCell("Effort", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tCmstrSubj.AddCell(formatCell(objRptCardMYP.Chstr_Efrt_RclScnInfo, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tCmstrSubj.AddCell(formatCell(objRptCardMYP.Chstr_Efrt_UsgApprScnficLang, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tCmstrSubj.AddCell(formatCell(objRptCardMYP.Chstr_Efrt_ScnficInq, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tCmstrSubj.AddCell(formatCell(objRptCardMYP.Chstr_Efrt_ScnficAppn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tCmstrSubj.AddCell(formatCell(objRptCardMYP.Chstr_Efrt_IntrptnScnficData, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tCmstrSubj.AddCell(formatCell(objRptCardMYP.Chstr_Efrt_TechSkill, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        DocRptCrd6G1Sem.Add(tCmstrSubj);
                        #endregion

                        //DocRptCrd6G1Sem.NewPage();
                        #region "Biology"
                        Paragraph pBioHdr = new Paragraph("BIOLOGY", new iTextSharp.text.Font(FontFactory.GetFont("Verdana", 10.0f, iTextSharp.text.Font.BOLD)));
                        pBioHdr.Alignment = Element.ALIGN_CENTER;
                        pBioHdr.SpacingAfter = spcAfter;
                        pBioHdr.SpacingBefore = spcBefore;
                        DocRptCrd6G1Sem.Add(pBioHdr);

                        PdfPTable tBioSubj = CreateTable(7, new int[] { 14, 15, 14, 14, 14, 14, 15 });

                        tBioSubj.AddCell(formatCell("Semester Focus", "Verdana", 10.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        //tBioSubj.AddCell(formatCell(objRptCardMYP.RptCardFocus.Biology, "Verdana", 10.0f, false, false, Element.ALIGN_JUSTIFIED, Element.ALIGN_MIDDLE, 5, 5, 7, 0));
                        PdfPCell BioFcus = formatCell(objRptCardMYP.RptCardFocus.Biology, "Verdana", 10.0f, false, false, Element.ALIGN_JUSTIFIED, Element.ALIGN_MIDDLE, 2, 2, 7, 0);
                        BioFcus.FixedHeight = cellFxdHeight;
                        tBioSubj.AddCell(BioFcus);

                        tBioSubj.AddCell(formatCell("Evaluation Criteria", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tBioSubj.AddCell(formatCell("Recall of Scientific Information", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tBioSubj.AddCell(formatCell("Usage of appropriate scientific languages", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tBioSubj.AddCell(formatCell("Scientific Inquiry", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tBioSubj.AddCell(formatCell("Scientific Application", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tBioSubj.AddCell(formatCell("Interpretation of scientific data", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tBioSubj.AddCell(formatCell("Technical skills", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        tBioSubj.AddCell(formatCell("Achievement", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tBioSubj.AddCell(formatCell(objRptCardMYP.Bio_Ach_RclScnInfo, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tBioSubj.AddCell(formatCell(objRptCardMYP.Bio_Ach_UsgApprScnficLang, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tBioSubj.AddCell(formatCell(objRptCardMYP.Bio_Ach_ScnficInq, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tBioSubj.AddCell(formatCell(objRptCardMYP.Bio_Ach_ScnficAppn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tBioSubj.AddCell(formatCell(objRptCardMYP.Bio_Ach_IntrptnScnficData, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tBioSubj.AddCell(formatCell(objRptCardMYP.Bio_Ach_TechSkill, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        tBioSubj.AddCell(formatCell("Effort", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tBioSubj.AddCell(formatCell(objRptCardMYP.Bio_Efrt_RclScnInfo, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tBioSubj.AddCell(formatCell(objRptCardMYP.Bio_Efrt_UsgApprScnficLang, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tBioSubj.AddCell(formatCell(objRptCardMYP.Bio_Efrt_ScnficInq, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tBioSubj.AddCell(formatCell(objRptCardMYP.Bio_Efrt_ScnficAppn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tBioSubj.AddCell(formatCell(objRptCardMYP.Bio_Efrt_IntrptnScnficData, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tBioSubj.AddCell(formatCell(objRptCardMYP.Bio_Efrt_TechSkill, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        DocRptCrd6G1Sem.Add(tBioSubj);
                        #endregion

                        //DocRptCrd6G1Sem.NewPage();
                        #region "History & Geography"
                        if (objRptCardMYP.Grade == "IX" && objRptCardMYP.Campus == "TIPS ERODE")
                        {
                            Paragraph pHstrGeoHdr = new Paragraph("ECONOMICS", new iTextSharp.text.Font(FontFactory.GetFont("Verdana", 10.0f, iTextSharp.text.Font.BOLD)));
                            pHstrGeoHdr.Alignment = Element.ALIGN_CENTER;
                            pHstrGeoHdr.SpacingAfter = spcAfter;
                            pHstrGeoHdr.SpacingBefore = spcBefore;
                            DocRptCrd6G1Sem.Add(pHstrGeoHdr);
                        }
                        else
                        {
                            Paragraph pHstrGeoHdr = new Paragraph("HISTORY & GEOGRAPHY", new iTextSharp.text.Font(FontFactory.GetFont("Verdana", 10.0f, iTextSharp.text.Font.BOLD)));
                            pHstrGeoHdr.Alignment = Element.ALIGN_CENTER;
                            pHstrGeoHdr.SpacingAfter = spcAfter;
                            pHstrGeoHdr.SpacingBefore = spcBefore;
                            DocRptCrd6G1Sem.Add(pHstrGeoHdr);
                        }

                        PdfPTable tHstrGeoSubj = CreateTable(5, new int[] { 15, 21, 20, 22, 22 });

                        tHstrGeoSubj.AddCell(formatCell("Semester Focus", "Verdana", 10.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        //tHstrGeoSubj.AddCell(formatCell(objRptCardMYP.RptCardFocus.HistoryGeography, "Verdana", 10.0f, false, false, Element.ALIGN_JUSTIFIED, Element.ALIGN_MIDDLE, 5, 5, 5, 0));
                        PdfPCell HstrGeoFcus = formatCell(objRptCardMYP.RptCardFocus.HistoryGeography, "Verdana", 10.0f, false, false, Element.ALIGN_JUSTIFIED, Element.ALIGN_MIDDLE, 2, 2, 7, 0);
                        HstrGeoFcus.FixedHeight = cellFxdHeight;
                        tHstrGeoSubj.AddCell(HstrGeoFcus);

                        tHstrGeoSubj.AddCell(formatCell("Evaluation Criteria", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tHstrGeoSubj.AddCell(formatCell("Understanding of the Concept", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tHstrGeoSubj.AddCell(formatCell("Research Skills", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tHstrGeoSubj.AddCell(formatCell("Organization and Presentation", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tHstrGeoSubj.AddCell(formatCell("Recall of information", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        tHstrGeoSubj.AddCell(formatCell("Achievement", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tHstrGeoSubj.AddCell(formatCell(objRptCardMYP.HisGeo_Ach_UndrsndConcpt, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tHstrGeoSubj.AddCell(formatCell(objRptCardMYP.HisGeo_Ach_RsrchSkl, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tHstrGeoSubj.AddCell(formatCell(objRptCardMYP.HisGeo_Ach_OrgnPrsntn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tHstrGeoSubj.AddCell(formatCell(objRptCardMYP.HisGeo_Ach_RclInfo, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        tHstrGeoSubj.AddCell(formatCell("Effort", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tHstrGeoSubj.AddCell(formatCell(objRptCardMYP.HisGeo_Efrt_UndrsndConcpt, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tHstrGeoSubj.AddCell(formatCell(objRptCardMYP.HisGeo_Efrt_RsrchSkl, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tHstrGeoSubj.AddCell(formatCell(objRptCardMYP.HisGeo_Efrt_OrgnPrsntn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tHstrGeoSubj.AddCell(formatCell(objRptCardMYP.HisGeo_Efrt_RclInfo, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        DocRptCrd6G1Sem.Add(tHstrGeoSubj);
                        #endregion

                        //DocRptCrd6G1Sem.NewPage();
                        #region "Computer Studies and Information technology (ICT)"
                        Paragraph pICTHdr = new Paragraph("Information Communication Technology (ICT)", new iTextSharp.text.Font(FontFactory.GetFont("Verdana", 10.0f, iTextSharp.text.Font.BOLD)));
                        pICTHdr.Alignment = Element.ALIGN_CENTER;
                        pICTHdr.SpacingAfter = spcAfter;
                        pICTHdr.SpacingBefore = spcBefore;
                        DocRptCrd6G1Sem.Add(pICTHdr);

                        PdfPTable tICTSubj = CreateTable(5, new int[] { 15, 21, 20, 22, 22 });

                        tICTSubj.AddCell(formatCell("Semester Focus", "Verdana", 10.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        //tICTSubj.AddCell(formatCell(objRptCardMYP.RptCardFocus.ICT, "Verdana", 10.0f, false, false, Element.ALIGN_JUSTIFIED, Element.ALIGN_MIDDLE, 5, 5, 5, 0));
                        PdfPCell ICTFcus = formatCell(objRptCardMYP.RptCardFocus.ICT, "Verdana", 10.0f, false, false, Element.ALIGN_JUSTIFIED, Element.ALIGN_MIDDLE, 2, 2, 7, 0);
                        ICTFcus.FixedHeight = cellFxdHeight;
                        tICTSubj.AddCell(ICTFcus);

                        tICTSubj.AddCell(formatCell("Evaluation Criteria", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tICTSubj.AddCell(formatCell("Recall of information", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tICTSubj.AddCell(formatCell("Investigate", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tICTSubj.AddCell(formatCell("Planning", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tICTSubj.AddCell(formatCell("Application of Technology", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        tICTSubj.AddCell(formatCell("Achievement", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tICTSubj.AddCell(formatCell(objRptCardMYP.ICT_Ach_RclInfo, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tICTSubj.AddCell(formatCell(objRptCardMYP.ICT_Ach_Invtgt, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tICTSubj.AddCell(formatCell(objRptCardMYP.ICT_Ach_Plnng, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tICTSubj.AddCell(formatCell(objRptCardMYP.ICT_Ach_AppnOfTech, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        tICTSubj.AddCell(formatCell("Effort", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tICTSubj.AddCell(formatCell(objRptCardMYP.ICT_Efrt_RclInfo, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tICTSubj.AddCell(formatCell(objRptCardMYP.ICT_Efrt_Invtgt, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tICTSubj.AddCell(formatCell(objRptCardMYP.ICT_Efrt_Plnng, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tICTSubj.AddCell(formatCell(objRptCardMYP.ICT_Efrt_AppnOfTech, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        DocRptCrd6G1Sem.Add(tICTSubj);
                        #endregion

                        //DocRptCrd6G1Sem.NewPage();
                        #region "Robotics"
                        Paragraph pRoboHdr = new Paragraph("ROBOTICS", new iTextSharp.text.Font(FontFactory.GetFont("Verdana", 10.0f, iTextSharp.text.Font.BOLD)));
                        pRoboHdr.Alignment = Element.ALIGN_CENTER;
                        pRoboHdr.SpacingAfter = spcAfter;
                        pRoboHdr.SpacingBefore = spcBefore;
                        DocRptCrd6G1Sem.Add(pRoboHdr);

                        PdfPTable tRoboSubj = CreateTable(5, new int[] { 15, 21, 20, 22, 22 });

                        tRoboSubj.AddCell(formatCell("Semester Focus", "Verdana", 10.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        //tRoboSubj.AddCell(formatCell(objRptCardMYP.RptCardFocus.Robotics, "Verdana", 10.0f, false, false, Element.ALIGN_JUSTIFIED, Element.ALIGN_MIDDLE, 5, 5, 5, 0));
                        PdfPCell roboFcus = formatCell(objRptCardMYP.RptCardFocus.Robotics, "Verdana", 10.0f, false, false, Element.ALIGN_JUSTIFIED, Element.ALIGN_MIDDLE, 2, 2, 7, 0);
                        roboFcus.FixedHeight = cellFxdHeight;
                        tRoboSubj.AddCell(roboFcus);

                        tRoboSubj.AddCell(formatCell("Evaluation Criteria", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tRoboSubj.AddCell(formatCell("Knowledge & Understanding", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tRoboSubj.AddCell(formatCell("Building & Design", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tRoboSubj.AddCell(formatCell("Programming/Activity Sheet", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tRoboSubj.AddCell(formatCell("Team Participation", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        tRoboSubj.AddCell(formatCell("Achievement", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tRoboSubj.AddCell(formatCell(objRptCardMYP.Rbt_Ach_KnwdUndstnd, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tRoboSubj.AddCell(formatCell(objRptCardMYP.Rbt_Ach_BldngDsgn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tRoboSubj.AddCell(formatCell(objRptCardMYP.Rbt_Ach_Progm, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tRoboSubj.AddCell(formatCell(objRptCardMYP.Rbt_Ach_WrkgInTam, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        tRoboSubj.AddCell(formatCell("Effort", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tRoboSubj.AddCell(formatCell(objRptCardMYP.Rbt_Efrt_KnwdUndstnd, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tRoboSubj.AddCell(formatCell(objRptCardMYP.Rbt_Efrt_BldngDsgn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tRoboSubj.AddCell(formatCell(objRptCardMYP.Rbt_Efrt_Progm, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tRoboSubj.AddCell(formatCell(objRptCardMYP.Rbt_Efrt_WrkgInTam, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        DocRptCrd6G1Sem.Add(tRoboSubj);
                        #endregion

                        DocRptCrd6G1Sem.NewPage();
                        if (objRptCardMYP.Campus != "TIPS ERODE")
                        {
                            #region "Spark"
                            Paragraph sparkHdr = new Paragraph("SPARK", new iTextSharp.text.Font(FontFactory.GetFont("Verdana", 10.0f, iTextSharp.text.Font.BOLD)));
                            sparkHdr.Alignment = Element.ALIGN_CENTER;
                            sparkHdr.SpacingAfter = spcAfter;
                            sparkHdr.SpacingBefore = spcBefore;
                            DocRptCrd6G1Sem.Add(sparkHdr);

                            PdfPTable tSparkSubj = CreateTable(5, new int[] { 15, 21, 20, 22, 22 });

                            tSparkSubj.AddCell(formatCell("Semester Focus", "Verdana", 10.0f, true, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            //tRoboSubj.AddCell(formatCell(objRptCardMYP.RptCardFocus.Robotics, "Verdana", 10.0f, false, false, Element.ALIGN_JUSTIFIED, Element.ALIGN_MIDDLE, 5, 5, 5, 0));
                            PdfPCell sparkFcus = formatCell(objRptCardMYP.RptCardFocus.spark, "Verdana", 10.0f, false, false, Element.ALIGN_JUSTIFIED, Element.ALIGN_MIDDLE, 2, 2, 7, 0);
                            sparkFcus.FixedHeight = cellFxdHeight;
                            tSparkSubj.AddCell(sparkFcus);

                            tSparkSubj.AddCell(formatCell("Evaluation Criteria", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tSparkSubj.AddCell(formatCell("Experimental Skill", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tSparkSubj.AddCell(formatCell("Team Work", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tSparkSubj.AddCell(formatCell("Attitude for Learning", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tSparkSubj.AddCell(formatCell("Written Task", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                            tSparkSubj.AddCell(formatCell("Achievement", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tSparkSubj.AddCell(formatCell(objRptCardMYP.spark_Eval_ExperiSkil, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tSparkSubj.AddCell(formatCell(objRptCardMYP.spark_Eval_TeamWork, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tSparkSubj.AddCell(formatCell(objRptCardMYP.spark_Eval_AttforLearn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                            tSparkSubj.AddCell(formatCell(objRptCardMYP.spark_Eval_WrittenTask, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                            DocRptCrd6G1Sem.Add(tSparkSubj);
                            #endregion
                        }


                        #region "Physical Education"
                        Paragraph pPEHdr = new Paragraph("PHYSICAL EDUCATION", new iTextSharp.text.Font(FontFactory.GetFont("Verdana", 10.0f, iTextSharp.text.Font.BOLD)));
                        pPEHdr.Alignment = Element.ALIGN_CENTER;
                        pPEHdr.SpacingAfter = spcAfter;
                        pPEHdr.SpacingBefore = spcBefore;
                        DocRptCrd6G1Sem.Add(pPEHdr);

                        PdfPTable tPESubj = CreateTable(5, new int[] { 15, 21, 20, 22, 22 });

                        tPESubj.AddCell(formatCell("Evaluation Criteria", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPESubj.AddCell(formatCell("Behaviour", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPESubj.AddCell(formatCell("Team work", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPESubj.AddCell(formatCell("Basic Skill", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPESubj.AddCell(formatCell("Fitness", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        tPESubj.AddCell(formatCell("Achievement", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPESubj.AddCell(formatCell(objRptCardMYP.PhysEd_Ach_Bhvr, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPESubj.AddCell(formatCell(objRptCardMYP.PhysEd_Ach_TmWrk, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPESubj.AddCell(formatCell(objRptCardMYP.PhysEd_Ach_BscSkl, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPESubj.AddCell(formatCell(objRptCardMYP.PhysEd_Ach_Ftns, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        tPESubj.AddCell(formatCell("Effort", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPESubj.AddCell(formatCell(objRptCardMYP.PhysEd_Efrt_Bhvr, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPESubj.AddCell(formatCell(objRptCardMYP.PhysEd_Efrt_TmWrk, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPESubj.AddCell(formatCell(objRptCardMYP.PhysEd_Efrt_BscSkl, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tPESubj.AddCell(formatCell(objRptCardMYP.PhysEd_Efrt_Ftns, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        DocRptCrd6G1Sem.Add(tPESubj);
                        #endregion

                        //DocRptCrd6G1Sem.NewPage();
                        #region "Fine Arts"
                        Paragraph pFAHdr = new Paragraph("FINE ARTS", new iTextSharp.text.Font(FontFactory.GetFont("Verdana", 10.0f, iTextSharp.text.Font.BOLD)));
                        pFAHdr.Alignment = Element.ALIGN_CENTER;
                        pFAHdr.SpacingAfter = spcAfter;
                        pFAHdr.SpacingBefore = spcBefore;
                        DocRptCrd6G1Sem.Add(pFAHdr);

                        PdfPTable tFASubj = CreateTable(7, new int[] { 15, 14, 14, 14, 14, 14, 15 });

                        tFASubj.AddCell(formatCell("Evaluation Criteria", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 2));
                        tFASubj.AddCell(formatCell("Music", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 3, 0));
                        tFASubj.AddCell(formatCell("Dance", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 3, 0));

                        tFASubj.AddCell(formatCell("Participation", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFASubj.AddCell(formatCell("Behaviour", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFASubj.AddCell(formatCell("Shows knowledge of unit studies", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFASubj.AddCell(formatCell("Skills/ Technique", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFASubj.AddCell(formatCell("Problem solving", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFASubj.AddCell(formatCell("Collaboration", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        tFASubj.AddCell(formatCell("Achievement", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFASubj.AddCell(formatCell(objRptCardMYP.FnArt_Musc_Ach_Prtcpn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFASubj.AddCell(formatCell(objRptCardMYP.FnArt_Musc_Ach_Bhvr, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFASubj.AddCell(formatCell(objRptCardMYP.FnArt_Musc_Ach_ShwKwdUntStuds, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFASubj.AddCell(formatCell(objRptCardMYP.FnArt_Dnc_Ach_SklTech, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFASubj.AddCell(formatCell(objRptCardMYP.FnArt_Dnc_Ach_PrblmSlvng, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFASubj.AddCell(formatCell(objRptCardMYP.FnArt_Dnc_Ach_Clbrtn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        tFASubj.AddCell(formatCell("Effort", "Verdana", 10.0f, false, false, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFASubj.AddCell(formatCell(objRptCardMYP.FnArt_Musc_Efrt_Prtcpn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFASubj.AddCell(formatCell(objRptCardMYP.FnArt_Musc_Efrt_Bhvr, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFASubj.AddCell(formatCell(objRptCardMYP.FnArt_Musc_Efrt_ShwKwdUntStuds, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFASubj.AddCell(formatCell(objRptCardMYP.FnArt_Dnc_Efrt_SklTech, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFASubj.AddCell(formatCell(objRptCardMYP.FnArt_Dnc_Efrt_PrblmSlvng, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                        tFASubj.AddCell(formatCell(objRptCardMYP.FnArt_Dnc_Efrt_Clbrtn, "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                        DocRptCrd6G1Sem.Add(tFASubj);
                        #endregion
                    }

                    DocRptCrd6G1Sem.Add(new Paragraph("\n"));
                    DocRptCrd6G1Sem.Add(new Paragraph("\n"));
                    //DocRptCrd6G1Sem.Add(new Paragraph("\n"));

                    #region "signature"
                    Paragraph pSgntrHdr = new Paragraph(" ", new iTextSharp.text.Font(FontFactory.GetFont("Verdana", 10.0f, iTextSharp.text.Font.BOLD)));
                    pSgntrHdr.Alignment = Element.ALIGN_CENTER;
                    pSgntrHdr.SpacingAfter = spcAfter;
                    pSgntrHdr.SpacingBefore = spcBefore;
                    DocRptCrd6G1Sem.Add(pSgntrHdr);

                    PdfPTable tSgntrSubj = CreateTable(4, new int[] { 25, 25, 25, 25 });

                    tSgntrSubj.AddCell(formatCell("Class Teacher", "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, 80, 0, 0, 0));
                    tSgntrSubj.AddCell(formatCell("Coordinator", "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, 80, 0, 0, 0));
                    tSgntrSubj.AddCell(formatCell("Head of the School", "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, 80, 0, 0, 0));
                    tSgntrSubj.AddCell(formatCell("Parent", "Verdana", 10.0f, true, false, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, 80, 0, 0, 0));

                    DocRptCrd6G1Sem.Add(tSgntrSubj);
                    #endregion

                    #region "Criteria of Assessment"
                    Paragraph pCOAHdr = new Paragraph("CRITERIA OF ASSESSMENT", new iTextSharp.text.Font(FontFactory.GetFont("Verdana", 10.0f, iTextSharp.text.Font.BOLD)));
                    pCOAHdr.Alignment = Element.ALIGN_CENTER;
                    pCOAHdr.SpacingAfter = 5f;
                    pCOAHdr.SpacingBefore = 15f;
                    DocRptCrd6G1Sem.Add(pCOAHdr);

                    PdfPTable tCOASubj = CreateTable(10, new int[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 });
                    tCOASubj.AddCell(formatCell("Grade", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tCOASubj.AddCell(formatCell("A*", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tCOASubj.AddCell(formatCell("A", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tCOASubj.AddCell(formatCell("B", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tCOASubj.AddCell(formatCell("C", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tCOASubj.AddCell(formatCell("D", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tCOASubj.AddCell(formatCell("E", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tCOASubj.AddCell(formatCell("F", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tCOASubj.AddCell(formatCell("G", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tCOASubj.AddCell(formatCell("U", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                    tCOASubj.AddCell(formatCell("%of Marks", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tCOASubj.AddCell(formatCell("90-100", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tCOASubj.AddCell(formatCell("80-89", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tCOASubj.AddCell(formatCell("70-79", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tCOASubj.AddCell(formatCell("60-69", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tCOASubj.AddCell(formatCell("50-59", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tCOASubj.AddCell(formatCell("40-49", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tCOASubj.AddCell(formatCell("30-39", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tCOASubj.AddCell(formatCell("20-29", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tCOASubj.AddCell(formatCell("0-19", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    DocRptCrd6G1Sem.Add(tCOASubj);

                    //Achievement
                    Paragraph pAchmntHdr = new Paragraph("Achievement:", new iTextSharp.text.Font(FontFactory.GetFont("Verdana", 10.0f, iTextSharp.text.Font.BOLD)));
                    pAchmntHdr.Alignment = Element.ALIGN_LEFT;
                    pAchmntHdr.SpacingAfter = 5f;
                    pAchmntHdr.SpacingBefore = 5f;
                    DocRptCrd6G1Sem.Add(pAchmntHdr);

                    PdfPTable tAchmntSubj = CreateTable(4, new int[] { 25, 25, 25, 25 });
                    tAchmntSubj.AddCell(formatCell("Surpassing", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tAchmntSubj.AddCell(formatCell("Meeting Expectation", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tAchmntSubj.AddCell(formatCell("Satisfactory", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tAchmntSubj.AddCell(formatCell("Experiencing Difficulty", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                    tAchmntSubj.AddCell(formatCell("A", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tAchmntSubj.AddCell(formatCell("B", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tAchmntSubj.AddCell(formatCell("C", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tAchmntSubj.AddCell(formatCell("D", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                    DocRptCrd6G1Sem.Add(tAchmntSubj);

                    //Skill
                    Paragraph pSkillHdr = new Paragraph("Skill:", new iTextSharp.text.Font(FontFactory.GetFont("Verdana", 10.0f, iTextSharp.text.Font.BOLD)));
                    pSkillHdr.Alignment = Element.ALIGN_LEFT;
                    pSkillHdr.SpacingAfter = 5f;
                    pSkillHdr.SpacingBefore = 5f;
                    DocRptCrd6G1Sem.Add(pSkillHdr);

                    PdfPTable tSkillSubj = CreateTable(4, new int[] { 25, 25, 25, 25 });
                    tSkillSubj.AddCell(formatCell("Surpassing", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tSkillSubj.AddCell(formatCell("Meeting Expectation", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tSkillSubj.AddCell(formatCell("Improving", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tSkillSubj.AddCell(formatCell("Experiencing Difficulty", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                    tSkillSubj.AddCell(formatCell("A", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tSkillSubj.AddCell(formatCell("B", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tSkillSubj.AddCell(formatCell("C", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tSkillSubj.AddCell(formatCell("D", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                    DocRptCrd6G1Sem.Add(tSkillSubj);

                    //Effort
                    Paragraph pEffortHdr = new Paragraph("Effort:", new iTextSharp.text.Font(FontFactory.GetFont("Verdana", 10.0f, iTextSharp.text.Font.BOLD)));
                    pEffortHdr.Alignment = Element.ALIGN_LEFT;
                    pEffortHdr.SpacingAfter = 5f;
                    pEffortHdr.SpacingBefore = 5f;
                    DocRptCrd6G1Sem.Add(pEffortHdr);

                    PdfPTable tEffortSubj = CreateTable(5, new int[] { 20, 20, 20, 20, 20 });
                    tEffortSubj.AddCell(formatCell("High Level", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tEffortSubj.AddCell(formatCell("Satisfactory", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tEffortSubj.AddCell(formatCell("Consistent", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tEffortSubj.AddCell(formatCell("Showing Development", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tEffortSubj.AddCell(formatCell("Inconsistent", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                    tEffortSubj.AddCell(formatCell("A", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tEffortSubj.AddCell(formatCell("B*", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tEffortSubj.AddCell(formatCell("B", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tEffortSubj.AddCell(formatCell("C", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));
                    tEffortSubj.AddCell(formatCell("D", "Verdana", 10.0f, false, false, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0, 0, 0, 0));

                    DocRptCrd6G1Sem.Add(tEffortSubj);
                    #endregion

                    DocRptCrd6G1Sem.Close();
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.ContentType = "application/pdf";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + objRptCardMYP.IdNo + "_" + objRptCardMYP.Grade + "_" + objRptCardMYP.Section + "_ReportCard.pdf");
                    Response.BinaryWrite(pdfStream.ToArray());
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
        }

        #endregion

        #endregion

        #region start ReportCard for 1-5 Grades

        public ActionResult ReportCardMainAction()
        {
            try
            {
                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj == null)
                {
                    return RedirectToAction("LogOff", "Account");
                }

                ViewBag.lgdUserId = (Userobj.UserId != null) ? Userobj.UserId : "";
                ViewBag.lgdUserName = (Userobj.UserName != null) ? Userobj.UserName : "";
                ViewBag.lgdInUserType = (Userobj.UserType != null) ? Userobj.UserType : "";

                string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);

                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
            finally
            { }
        }
        public ActionResult JqGridRptCardMainAction(int page, int rows, string sidx, string sord, string RequestNo, string Name, string Campus, string Section, string Grade, string RptStatus)
        {
            try
            {
                var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                string[] str = new string[usrcmp.ToList().Count];
                int i = 0;
                foreach (var var in usrcmp)
                {
                    str[i] = var;
                    i++;
                }

                ReportCardService rptCardSrvc = new ReportCardService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                string colName = string.Empty; string[] values = new string[1];
                if (!string.IsNullOrWhiteSpace(RequestNo)) { criteria.Add("RequestNo", RequestNo); }
                if (!string.IsNullOrWhiteSpace(Name)) { criteria.Add("Name", Name); }
                if (RptStatus == "Open") { RptStatus = "WIP"; }
                if (!string.IsNullOrWhiteSpace(RptStatus)) { criteria.Add("RptCardStatus", RptStatus); } else { criteria.Add("RptCardStatus", "WIP"); }

                if (usrcmp.Count() != 0)
                {
                    if (usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
                    {
                        if (!string.IsNullOrWhiteSpace(Campus) && Campus != "Select") { criteria.Add("Campus", Campus); }
                        else { criteria.Add("Campus", str); }
                    }
                }

                if (!string.IsNullOrWhiteSpace(Section) && Section != "Select") { criteria.Add("Section", Section); }
                if (!string.IsNullOrWhiteSpace(Grade) && Grade != "Select") { criteria.Add("Grade", Grade); }

                Dictionary<long, IList<RptCardPYPFirstGrade>> dcnRptCardLst = rptCardSrvc.GetRepCardFirstGradeListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (dcnRptCardLst == null || dcnRptCardLst.Count == 0)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    IList<RptCardPYPFirstGrade> lstRepCard = dcnRptCardLst.FirstOrDefault().Value;
                    long totalRecords = dcnRptCardLst.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows = (
                        from items in lstRepCard
                        select new
                        {
                            i = items.Id,
                            cell = new string[] 
                       { 
                            items.Id.ToString(),
                            items.RequestNo,                        //inbox secrch Field
                            items.Campus,   //inbox secrch Field
                            items.IdNo,
                            items.Name,          //inbox secrch Field
                            items.Section,       //inbox secrch Field
                            items.Grade,         //inbox secrch Field
                            items.PreRegNum.ToString(),
                            items.TeacherName,
                            items.Semester.ToString()
                       }
                        })
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, policyName); }
            finally
            { }
        }


        public ActionResult PYPFirstGrade(long? Id)
        {
            try
            {
                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj == null || (Userobj != null && Userobj.UserId == null))
                { return RedirectToAction("LogOff", "Account"); }
                string loggedInUserId = Userobj.UserId;//(Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                RptCardPYPFirstGrade objfstgrade = new RptCardPYPFirstGrade();
                if (Id > 0)
                {
                    ReportCardService rptSrv = new ReportCardService();
                    objfstgrade = rptSrv.GetPYPFirstGrade(Id ?? 0);
                    objfstgrade.RptCardModifiedBy = loggedInUserId;
                    ViewBag.status = objfstgrade.RptCardStatus;
                }
                else
                {
                    string loggedInUserName = (Userobj.UserName != null) ? Userobj.UserName : "";
                    string loggedInUserType = (Userobj.UserType != null) ? Userobj.UserType : "";

                    ViewBag.loggedInUserId = loggedInUserId;
                    ViewBag.loggedInUserType = loggedInUserType;
                    ViewBag.loggedInUserName = string.IsNullOrWhiteSpace(loggedInUserName) ? loggedInUserId : loggedInUserName;
                    objfstgrade.RptCardCreatedBy = loggedInUserId;
                }
                return View(objfstgrade);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
            finally
            { }
        }
        [HttpPost]
        public ActionResult PYPFirstGrade(FormCollection FrmCltn, RptCardPYPFirstGrade rpt)
        {
            try
            {
                ReportCardService objrpt = new ReportCardService();
                rpt.RptCardStatus = FrmCltn["Action"] == "Complete" ? "Closed" : "WIP";

                if (rpt.Id == 0)
                { rpt.RptCardCreatedDate = DateTime.Now; }
                else { rpt.RptCardModifiedDate = DateTime.Now; }

                long Id = objrpt.SaveOrUpdatePYPFirstGrade(rpt);
                TempData["PYPSaveAlrtMsg"] = "";
                if (FrmCltn["Action"] == "Save")
                {
                    return RedirectToAction("PYPFirstGrade", new { Id = Id });
                }
                return RedirectToAction("ReportCardMainAction");
            }
            catch (Exception ex)
            {
                TempData["PYPSaveAlrtMsg"] = ex.Message;
                ExceptionPolicy.HandleException(ex, policyName);
                return RedirectToAction("PYPFirstGrade", "AchievementReport");
            }
        }



        public ActionResult PYPSecondGrade()
        {
            return View();
        }

        #endregion end ReportCard for 1-5 Grads

        #region "Bulk Report Card Creation for MYP"

        public ActionResult BulkReportCardCreationMYP()
        {
            try
            {
                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj == null)
                {
                    return RedirectToAction("LogOff", "Account");
                }
                DateTime DateNow = DateTime.Now;
                string[] Academicyear = new string[2];
                Academicyear[0] = "Select";
                Academicyear[1] = (DateNow.Year).ToString() + "-" + (DateNow.Year + 1).ToString();
                ViewBag.acadddl = Academicyear;
                string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
        }

        public ActionResult JqgridBulkRptCreation(string campus, string grade, string semester, string academicyear, string section, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(campus) && string.IsNullOrWhiteSpace(grade) && string.IsNullOrWhiteSpace(academicyear))
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ReportCardService rptCrdSrvc = new ReportCardService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrWhiteSpace(campus)) { criteria.Add("Campus", campus); }
                    if (!string.IsNullOrWhiteSpace(grade)) { criteria.Add("Grade", grade); }
                    if (!string.IsNullOrWhiteSpace(academicyear)) { criteria.Add("AcademicYear", academicyear); }
                    if (!string.IsNullOrWhiteSpace(section)) { criteria.Add("Section", section); }
                    criteria.Add("AdmissionStatus", "Registered");
                    Dictionary<long, IList<StudentDtlsView>> dcnStdntDtls = rptCrdSrvc.GetStudentDtlsViewListforBulkRptCardWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (dcnStdntDtls != null && dcnStdntDtls.FirstOrDefault().Value.Count > 0 && dcnStdntDtls.FirstOrDefault().Key > 0)
                    {
                        long totalRecords = dcnStdntDtls.FirstOrDefault().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows = (
                            from items in dcnStdntDtls.FirstOrDefault().Value
                            select new
                            {
                                i = items.Id,
                                cell = new string[] 
                       { 
                           items.Id.ToString(), items.PreRegNum, items.IdNo, items.Name,items.Campus,items.Grade,items.Section, 
                            items.AcademicYear
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
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
            finally { }
        }


        public ActionResult SaveBulkReportCardCreation(string campus, string grade, string semester, string academicyear, string section, string teachName, string rptDate)
        {
            try
            {


                ReportCardService rptCrdSrvc = new ReportCardService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrWhiteSpace(campus)) { criteria.Add("Campus", campus); }
                if (!string.IsNullOrWhiteSpace(grade)) { criteria.Add("Grade", grade); }
                if (!string.IsNullOrWhiteSpace(academicyear)) { criteria.Add("AcademicYear", academicyear); }
                if (!string.IsNullOrWhiteSpace(section)) { criteria.Add("Section", section); }
                criteria.Add("AdmissionStatus", "Registered");
                Dictionary<long, IList<StudentDtlsView>> dcnStdntDtls = rptCrdSrvc.GetStudentDtlsViewListforBulkRptCardWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                if (dcnStdntDtls != null && dcnStdntDtls.FirstOrDefault().Value.Count > 0 && dcnStdntDtls.FirstOrDefault().Key > 0)
                {
                    List<StudentDtlsView> dcnStdntDtlsforrptCardMYP = dcnStdntDtls.FirstOrDefault().Value.ToList();
                    long count = rptCrdSrvc.SaveOrUpdateBulkMYPReportCard(dcnStdntDtlsforrptCardMYP, semester, teachName, rptDate);
                    return Json(count, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
            finally { }

        }
        public ActionResult GetTeacherName(string techName)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Name", techName);
                Dictionary<long, IList<StaffDetails>> staffdetails = sms.GetStaffDetailsListWithPaging(0, 10, string.Empty, string.Empty, criteria);
                var nameslst = (from u in staffdetails.First().Value
                                where u.Name != null
                                select u.Name).Distinct().ToList();
                return Json(nameslst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
                throw ex;
            }
        }
        #endregion

    }
}