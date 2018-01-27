using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TIPS.Entities;
using TIPS.Entities.AdmissionEntities;
using TIPS.Entities.StaffManagementEntities;
using TIPS.Entities.StaffEntities;
using TIPS.Service;
using TIPS.ServiceContract;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TIPS.Entities.Assess;
using TIPS.Entities.CommunictionEntities;
using TIPS.Entities.StoreEntities;
using TIPS.Entities.TransportEntities;
using TIPS.Component;
using TIPS.Entities.AdminTemplateEntities;
using System.Data;
using TIPS.Entities.StudentsReportEntities;
namespace CMS.Controllers
{
    public class AdminTemplateController : BaseController
    {
        //
        // GET: /AdminTemplate/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult getCampusList()
        {
            try
            {
                MastersService ms = new MastersService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                //use the below method without the count value, since this method is not using the value "REVISIT"
                //anbu need to change this logic whenver is possible
                Dictionary<long, IList<CampusMaster>> CampusMasterList = ms.GetCampusMasterListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);
                if (CampusMasterList != null && CampusMasterList.First().Value != null && CampusMasterList.First().Value.Count > 0)
                {
                    var CampusList = (
                             from items in CampusMasterList.First().Value

                             select new
                             {
                                 Text = items.Name,
                                 Value = items.Name
                             }).Distinct().ToList();

                    return Json(CampusList, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(null, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult AdminTemplate()
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
                    #region Dashboard
                    #region IndexPage Counts
                    AdminTemplateService ATS = new AdminTemplateService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    long Dash_TotalUsersCount = 0;
                    long Dash_TotalCampusCount = 0;
                    long Dash_TotalStudentsCount = 0;
                    long Dash_TotalStaffCount = 0;
                    long Dash_TotalLoginCount = 0;
                    criteria.Clear();
                    Dictionary<long, IList<AdminTemplateDashboardIndex_vw>> AdminTemplateDashboardIndex_vwDetails = ATS.GetAdminTemplateDashboardIndex_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    if (AdminTemplateDashboardIndex_vwDetails != null && AdminTemplateDashboardIndex_vwDetails.Count > 0 && AdminTemplateDashboardIndex_vwDetails.FirstOrDefault().Value.Count > 0)
                    {
                        Dash_TotalUsersCount = Convert.ToInt64(AdminTemplateDashboardIndex_vwDetails.FirstOrDefault().Value[0].UserCount);
                        Dash_TotalCampusCount = Convert.ToInt64(AdminTemplateDashboardIndex_vwDetails.FirstOrDefault().Value[0].CampusCount);
                        Dash_TotalStudentsCount = Convert.ToInt64(AdminTemplateDashboardIndex_vwDetails.FirstOrDefault().Value[0].StudentsCount);
                        Dash_TotalStaffCount = Convert.ToInt64(AdminTemplateDashboardIndex_vwDetails.FirstOrDefault().Value[0].StaffCount);
                        Dash_TotalLoginCount = Convert.ToInt64(AdminTemplateDashboardIndex_vwDetails.FirstOrDefault().Value[0].ViewersCount);
                    }
                    @ViewBag.Dash_TotalUsersCount = Dash_TotalUsersCount;
                    @ViewBag.Dash_TotalCampusCount = Dash_TotalCampusCount;
                    @ViewBag.Dash_TotalStudentsCount = Dash_TotalStudentsCount;
                    @ViewBag.Dash_TotalStaffCount = Dash_TotalStaffCount;
                    @ViewBag.Dash_TotalLoginCount = Dash_TotalLoginCount;
                    #endregion
                    #region Call Management Charts

                    CallManagementService cms = new CallManagementService();
                    Dictionary<string, object> Criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    Criteria.Add("Performer", userId);
                    Dictionary<long, IList<CallManagementChart>> GetIssueCountByStatus = cms.GetCallManagementListWithPagingChart(null, 9999, string.Empty, string.Empty, Criteria);
                    var IssueCount = (from u in GetIssueCountByStatus.First().Value
                                      where u.IsInformation == false
                                      select u).ToList();
                    long LogIssCnt = 0;
                    long ResRejIssCnt = 0;
                    long log_ResRejIssCnt = 0;
                    long ResIssCnt = 0;
                    long AppRejCnt = 0;
                    long res_apprejcnt = 0;
                    long AppIssCnt = 0;
                    long AppIssTotCnt = 0;
                    long TobeCmpltdcnt = 0;
                    long CmpltdCnt = 0;
                    long totalIssue = 0;
                    foreach (var itemdata in IssueCount)
                    {
                        if (itemdata.Status == "LogIssue")
                        {
                            LogIssCnt++;
                        }
                        else if (itemdata.Status == "ResolveIssueRejection")
                        {
                            ResRejIssCnt++;
                        }
                        else if (itemdata.Status == "ResolveIssue")
                        {
                            ResIssCnt++;
                        }
                        else if (itemdata.Status == "ApproveIssueRejection")
                        {
                            AppRejCnt++;
                        }
                        else if (itemdata.Status == "ApproveIssue")
                        {
                            AppIssCnt++;
                        }
                        else if (itemdata.Status == "Complete")
                        {
                            TobeCmpltdcnt++;
                        }

                        else if (itemdata.Status == "Completed")
                        {
                            CmpltdCnt++;
                        }
                        else
                        { }
                    }
                    totalIssue = LogIssCnt + ResRejIssCnt + ResIssCnt + AppRejCnt + AppIssCnt + TobeCmpltdcnt + CmpltdCnt;
                    log_ResRejIssCnt = LogIssCnt + ResRejIssCnt;
                    res_apprejcnt = ResIssCnt + AppRejCnt;
                    AppIssTotCnt = AppIssCnt + TobeCmpltdcnt;


                    @ViewBag.log_ResRejIssCnt = log_ResRejIssCnt;
                    @ViewBag.res_apprejcnt = res_apprejcnt;
                    @ViewBag.AppIssTotCnt = AppIssTotCnt;
                    @ViewBag.CmpltdCnt = CmpltdCnt;
                    #endregion
                    #endregion
                    #region Staff
                    #region Department Wise Counts
                    criteria.Clear();
                    long Staff_AcademicsDeptCnt = 0;
                    long Staff_AdmissionDeptCnt = 0;
                    long Staff_AdministrativeDeptCnt = 0;
                    long Staff_AccountsDeptCnt = 0;
                    long Staff_TransportDeptCnt = 0;
                    long Staff_HRDeptCnt = 0;
                    long Staff_StoreDeptCnt = 0;
                    long Staff_ITDeptCnt = 0;
                    long Staff_HostelCnt = 0;
                    long Staff_EDPCnt = 0;
                    criteria.Clear();
                    Dictionary<long, IList<AdminTemplateStaffGroupByDept_vw>> AdminTemplateStaffGroupByDept_vwDetails = null;
                    AdminTemplateStaffGroupByDept_vwDetails = ATS.GetAdminTemplateStaffGroupByDept_vwListWithPagingAndCriteria(null, 999999, string.Empty, string.Empty, criteria);
                    if (AdminTemplateStaffGroupByDept_vwDetails != null && AdminTemplateStaffGroupByDept_vwDetails.Count > 0 && AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value.Count > 0)
                    {
                        var Staff_AcademicsList = (from u in AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value
                                                   where u.Department == "Academics"
                                                   select u).ToList();
                        if (Staff_AcademicsList.Count > 0)
                            Staff_AcademicsDeptCnt = Staff_AcademicsList.Count;


                        var AdmissionList = (from u in AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value
                                             where u.Department == "Admission"
                                             select u).ToList();
                        if (Staff_AcademicsList.Count > 0)
                            Staff_AdmissionDeptCnt = AdmissionList.Count;



                        var AdministrativeList = (from u in AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value
                                                  where u.Department == "Administrative"
                                                  select u).ToList();
                        if (Staff_AcademicsList.Count > 0)
                            Staff_AdministrativeDeptCnt = AdministrativeList.Count;


                        var AccountsList = (from u in AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value
                                            where u.Department == "Accounts"
                                            select u).ToList();
                        if (Staff_AcademicsList.Count > 0)
                            Staff_AccountsDeptCnt = AccountsList.Count;




                        var TransportList = (from u in AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value
                                             where u.Department == "Transport"
                                             select u).ToList();
                        if (Staff_AcademicsList.Count > 0)
                            Staff_TransportDeptCnt = TransportList.Count;


                        var HRList = (from u in AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value
                                      where u.Department == "HR"
                                      select u).ToList();
                        if (Staff_AcademicsList.Count > 0)
                            Staff_HRDeptCnt = HRList.Count;



                        var StoreList = (from u in AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value
                                         where u.Department == "Store"
                                         select u).ToList();
                        if (Staff_AcademicsList.Count > 0)
                            Staff_StoreDeptCnt = StoreList.Count;


                        var ITList = (from u in AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value
                                      where u.Department == "IT"
                                      select u).ToList();
                        if (Staff_AcademicsList.Count > 0)
                            Staff_ITDeptCnt = ITList.Count;


                        var HostelList = (from u in AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value
                                          where u.Department == "Hostel"
                                          select u).ToList();
                        if (HostelList.Count > 0)
                            Staff_HostelCnt = HostelList.Count;


                        var EDPList = (from u in AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value
                                       where u.Department == "EDP"
                                       select u).ToList();
                        if (EDPList.Count > 0)
                            Staff_EDPCnt = EDPList.Count;
                    }
                    @ViewBag.Staff_AcademicsDeptCnt = Staff_AcademicsDeptCnt;
                    @ViewBag.Staff_AdmissionDeptCnt = Staff_AdmissionDeptCnt;
                    @ViewBag.Staff_AdministrativeDeptCnt = Staff_AdministrativeDeptCnt;
                    @ViewBag.Staff_AccountsDeptCnt = Staff_AccountsDeptCnt;
                    @ViewBag.Staff_TransportDeptCnt = Staff_TransportDeptCnt;
                    @ViewBag.Staff_HRDeptCnt = Staff_HRDeptCnt;
                    @ViewBag.Staff_StoreDeptCnt = Staff_StoreDeptCnt;
                    @ViewBag.Staff_ITDeptCnt = Staff_ITDeptCnt;
                    @ViewBag.Staff_HostelCnt = Staff_HostelCnt;
                    @ViewBag.Staff_EDPCnt = Staff_EDPCnt;
                    #endregion
                    #region Campus Wise Counts
                    criteria.Clear();
                    long Staff_IBKGCount = 0;
                    long Staff_IBMAINCount = 0;
                    long Staff_KARURKGCount = 0;
                    long Staff_KARURCount = 0;
                    long Staff_TIRUPURKGCount = 0;
                    long Staff_TIRUPURCount = 0;
                    long Staff_ERNAKULAMKGCount = 0;
                    long Staff_ERNAKULAMCount = 0;
                    long Staff_CHENNAICITYCount = 0;
                    long Staff_CHENNAIMAINCount = 0;
                    long Staff_TIPSERODECount = 0;
                    long Staff_TIPSSALEMCount = 0;
                    long Staff_TIPSSARANCount = 0;
                    Dictionary<long, IList<AdminTemplateStaffDetails_vw>> AdminTemplateStaffDetails_vwDetails = null;
                    AdminTemplateStaffDetails_vwDetails = ATS.GetAdminTemplateStaffDetails_vwListWithPagingAndCriteria(null, 999999, string.Empty, string.Empty, criteria);
                    if (AdminTemplateStaffDetails_vwDetails != null && AdminTemplateStaffDetails_vwDetails.Count > 0 && AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value.Count > 0)
                    {

                        var Staff_IBKGList = (from u in AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value
                                              where u.Campus == "IB KG"
                                              select u).ToList();
                        if (Staff_IBKGList.Count > 0)
                            Staff_IBKGCount = Staff_IBKGList.FirstOrDefault().Count;

                        var Staff_IBMAINList = (from u in AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value
                                                where u.Campus == "IB MAIN"
                                                select u).ToList();
                        if (Staff_IBMAINList.Count > 0)
                            Staff_IBMAINCount = Staff_IBMAINList.FirstOrDefault().Count;


                        var Staff_KARURKGList = (from u in AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value
                                                 where u.Campus == "KARUR KG"
                                                 select u).ToList();
                        if (Staff_KARURKGList.Count > 0)
                            Staff_KARURKGCount = Staff_KARURKGList.FirstOrDefault().Count;

                        var Staff_KARURList = (from u in AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value
                                               where u.Campus == "KARUR"
                                               select u).ToList();
                        if (Staff_KARURList.Count > 0)
                            Staff_KARURCount = Staff_KARURList.FirstOrDefault().Count;

                        var Staff_TIRUPURKGList = (from u in AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value
                                                   where u.Campus == "TIRUPUR KG"
                                                   select u).ToList();
                        if (Staff_TIRUPURKGList.Count > 0)
                            Staff_TIRUPURKGCount = Staff_TIRUPURKGList.FirstOrDefault().Count;

                        var Staff_TIRUPURList = (from u in AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value
                                                 where u.Campus == "TIRUPUR"
                                                 select u).ToList();
                        if (Staff_TIRUPURList.Count > 0)
                            Staff_TIRUPURCount = Staff_TIRUPURList.FirstOrDefault().Count;

                        var Staff_ERNAKULAMKGList = (from u in AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value
                                                     where u.Campus == "ERNAKULAM KG"
                                                     select u).ToList();
                        if (Staff_ERNAKULAMKGList.Count > 0)
                            Staff_ERNAKULAMKGCount = Staff_ERNAKULAMKGList.FirstOrDefault().Count;

                        var Staff_ERNAKULAMList = (from u in AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value
                                                   where u.Campus == "ERNAKULAM"
                                                   select u).ToList();
                        if (Staff_ERNAKULAMList.Count > 0)
                            Staff_ERNAKULAMCount = Staff_ERNAKULAMList.FirstOrDefault().Count;

                        var Staff_CHENNAICITYList = (from u in AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value
                                                     where u.Campus == "CHENNAI CITY"
                                                     select u).ToList();
                        if (Staff_CHENNAICITYList.Count > 0)
                            Staff_CHENNAICITYCount = Staff_CHENNAICITYList.FirstOrDefault().Count;

                        var Staff_CHENNAIMAINList = (from u in AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value
                                                     where u.Campus == "CHENNAI MAIN"
                                                     select u).ToList();
                        if (Staff_CHENNAIMAINList.Count > 0)
                            Staff_CHENNAIMAINCount = Staff_CHENNAIMAINList.FirstOrDefault().Count;

                        var Staff_TIPSERODEList = (from u in AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value
                                                   where u.Campus == "TIPS ERODE"
                                                   select u).ToList();
                        if (Staff_TIPSERODEList.Count > 0)
                            Staff_TIPSERODECount = Staff_TIPSERODEList.FirstOrDefault().Count;

                        var Staff_TIPSSALEMList = (from u in AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value
                                                   where u.Campus == "TIPS SALEM"
                                                   select u).ToList();
                        if (Staff_TIPSSALEMList.Count > 0)
                            Staff_TIPSSALEMCount = Staff_TIPSSALEMList.FirstOrDefault().Count;

                        var Staff_TIPSSARANList = (from u in AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value
                                                   where u.Campus == "TIPS SARAN"
                                                   select u).ToList();
                        if (Staff_TIPSSARANList.Count > 0)
                            Staff_TIPSSARANCount = Staff_TIPSSARANList.FirstOrDefault().Count;
                    }
                    @ViewBag.Staff_IBKGCount = Staff_IBKGCount;
                    @ViewBag.Staff_IBMAINCount = Staff_IBMAINCount;
                    @ViewBag.Staff_KARURKGCount = Staff_KARURKGCount;
                    @ViewBag.Staff_KARURCount = Staff_KARURCount;
                    @ViewBag.Staff_TIRUPURKGCount = Staff_TIRUPURKGCount;
                    @ViewBag.Staff_TIRUPURCount = Staff_TIRUPURCount;
                    @ViewBag.Staff_ERNAKULAMKGCount = Staff_ERNAKULAMKGCount;
                    @ViewBag.Staff_ERNAKULAMCount = Staff_ERNAKULAMCount;
                    @ViewBag.Staff_CHENNAICITYCount = Staff_CHENNAICITYCount;
                    @ViewBag.Staff_CHENNAIMAINCount = Staff_CHENNAIMAINCount;
                    @ViewBag.Staff_TIPSERODECount = Staff_TIPSERODECount;
                    @ViewBag.Staff_TIPSSALEMCount = Staff_TIPSSALEMCount;
                    @ViewBag.Staff_TIPSSARANCount = Staff_TIPSSARANCount;
                    #endregion
                    #endregion
                    #region Admission Management
                    criteria.Clear();
                    string Campus = string.Empty;
                    DateTime Currnt_Date = DateTime.Now;
                    int Currnt_Year = Currnt_Date.Year;
                    string CurrentAcademicYear = string.Empty;
                    if (Currnt_Date.Month >= 6 && Currnt_Date.Month <= 12)
                    {
                        CurrentAcademicYear = Currnt_Year + "-" + (Currnt_Year + 1);
                    }
                    if (Currnt_Date.Month >= 1 && Currnt_Date.Month <= 4)
                    {
                        CurrentAcademicYear = (Currnt_Year - 1) + "-" + Currnt_Year;
                    }
                    long ResidentialCount = 0;
                    long DayScholorCount = 0;
                    long WeekBoarderCount = 0;
                    Dictionary<long, IList<AdminTemplateStudBoardingReport_vw>> BoardingTypeDetails = null;
                    string[] ColorCodes = { "0000FF", "8A2BE2", "A52A2A", "DEB887", "5F9EA0", "BDB76B", "8B008B", "556B2F", "FF8C00", "FF00FF", "191970", "CD853F", "FF0000", "708090", "008080", "9ACD32" };
                    IList<AdminTemplateStudBoardingReport_vw> BoardingTypeList = new List<AdminTemplateStudBoardingReport_vw>();
                    criteria.Add("AcademicYear", CurrentAcademicYear);
                    if (Campus == "" && Campus == null) { Campus = "All"; }
                    if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                    BoardingTypeDetails = ATS.GetAdminTemplateStudBoardingReport_vwListWithPagingAndCriteria(null, 999999, string.Empty, string.Empty, criteria);
                    if (BoardingTypeDetails != null && BoardingTypeDetails.Count > 0 && BoardingTypeDetails.FirstOrDefault().Value.Count > 0)
                    {
                        int TotalCount = Convert.ToInt32(BoardingTypeDetails.FirstOrDefault().Key);
                        Campus = string.Empty;
                        for (int i = 0; i < TotalCount; i++)
                        {
                            AdminTemplateStudBoardingReport_vw Obj = new AdminTemplateStudBoardingReport_vw();
                            if (!string.IsNullOrEmpty(BoardingTypeDetails.FirstOrDefault().Value[i].BoardingType)) { Obj.BoardingType = BoardingTypeDetails.FirstOrDefault().Value[i].BoardingType; }
                            if (BoardingTypeDetails.FirstOrDefault().Value[i].Count >= 0) { Obj.Count = BoardingTypeDetails.FirstOrDefault().Value[i].Count; }
                            if (Obj != null) { BoardingTypeList.Add(Obj); }
                        }
                    }
                    var ResidentialList = (from u in BoardingTypeList
                                           where u.BoardingType == "Residential"
                                           select u).ToList();
                    if (ResidentialList.Count > 0)
                    {
                        ResidentialCount = ResidentialList.FirstOrDefault().Count;
                    }
                    var DayScholorList = (from u in BoardingTypeList
                                          where u.BoardingType == "Day Scholar"
                                          select u).ToList();
                    if (DayScholorList.Count > 0)
                    {
                        DayScholorCount = DayScholorList.FirstOrDefault().Count;
                    }
                    var WeekBoarderList = (from u in BoardingTypeList
                                           where u.BoardingType == "Week Boarder"
                                           select u).ToList();
                    if (WeekBoarderList.Count > 0)
                    {
                        WeekBoarderCount = WeekBoarderList.FirstOrDefault().Count;
                    }

                    @ViewBag.ResidentialCount = ResidentialCount;
                    @ViewBag.DayScholorCount = DayScholorCount;
                    @ViewBag.WeekBoarderCount = WeekBoarderCount;
                    #endregion
                    #region Assessment
                    criteria.Clear();
                    MastersService ms = new MastersService();
                    Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    ViewBag.gradeddl1 = GradeMaster.First().Value;
                    #endregion
                    #region Email
                    long Email_IBKGCount = 0;
                    long Email_IBMAINCount = 0;
                    long Email_KARURKGCount = 0;
                    long Email_KARURCount = 0;
                    long Email_TIRUPURKGCount = 0;
                    long Email_TIRUPURCount = 0;
                    long Email_ERNAKULAMKGCount = 0;
                    long Email_ERNAKULAMCount = 0;
                    long Email_CHENNAICITYCount = 0;
                    long Email_CHENNAIMAINCount = 0;
                    long Email_TIPSERODECount = 0;
                    long Email_TIPSSALEMCount = 0;
                    long Email_TIPSSARANCount = 0;
                    criteria.Clear();
                    string AcademicYear = string.Empty;
                    //DateTime Currnt_Date = DateTime.Now;
                    //int Currnt_Year = Currnt_Date.Year;
                    if (Currnt_Date.Month >= 6 && Currnt_Date.Month <= 12)
                    {
                        AcademicYear = Currnt_Year + "-" + (Currnt_Year + 1);
                    }
                    if (Currnt_Date.Month >= 1 && Currnt_Date.Month <= 4)
                    {
                        AcademicYear = (Currnt_Year - 1) + "-" + Currnt_Year;
                    }
                    criteria.Add("AcademicYear", AcademicYear);
                    Dictionary<long, IList<AdminTemplateEmailReport_vw>> EmailDetails = null;
                    IList<AdminTemplateEmailReport_vw> EmailDetailsList = new List<AdminTemplateEmailReport_vw>();
                    EmailDetails = ATS.GetAdminTemplateEmailReport_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    if (EmailDetails != null && EmailDetails.Count > 0 && EmailDetails.FirstOrDefault().Value.Count > 0)
                    {
                        var Email_IBKGList = (from u in EmailDetails.FirstOrDefault().Value
                                              where u.Campus == "IB KG"
                                              select u).ToList();
                        if (Email_IBKGList.Count > 0)
                            Email_IBKGCount = Email_IBKGList.FirstOrDefault().Count;

                        var Email_IBMAINList = (from u in EmailDetails.FirstOrDefault().Value
                                                where u.Campus == "IB MAIN"
                                                select u).ToList();
                        if (Email_IBMAINList.Count > 0)
                            Email_IBMAINCount = Email_IBMAINList.FirstOrDefault().Count;

                        var Email_KARURKGList = (from u in EmailDetails.FirstOrDefault().Value
                                                 where u.Campus == "KARUR KG"
                                                 select u).ToList();
                        if (Email_KARURKGList.Count > 0)
                            Email_KARURKGCount = Email_KARURKGList.FirstOrDefault().Count;

                        var Email_KARURList = (from u in EmailDetails.FirstOrDefault().Value
                                               where u.Campus == "KARUR"
                                               select u).ToList();
                        if (Email_KARURList.Count > 0)
                            Email_KARURCount = Email_KARURList.FirstOrDefault().Count;

                        var Email_TIRUPURKGList = (from u in EmailDetails.FirstOrDefault().Value
                                                   where u.Campus == "TIRUPUR KG"
                                                   select u).ToList();
                        if (Email_TIRUPURKGList.Count > 0)
                            Email_TIRUPURKGCount = Email_TIRUPURKGList.FirstOrDefault().Count;

                        var Email_TIRUPURList = (from u in EmailDetails.FirstOrDefault().Value
                                                 where u.Campus == "TIRUPUR"
                                                 select u).ToList();
                        if (Email_TIRUPURList.Count > 0)
                            Email_TIRUPURCount = Email_TIRUPURList.FirstOrDefault().Count;

                        var Email_ERNAKULAMKGList = (from u in EmailDetails.FirstOrDefault().Value
                                                     where u.Campus == "ERNAKULAM KG"
                                                     select u).ToList();
                        if (Email_ERNAKULAMKGList.Count > 0)
                            Email_ERNAKULAMKGCount = Email_ERNAKULAMKGList.FirstOrDefault().Count;

                        var Email_ERNAKULAMList = (from u in EmailDetails.FirstOrDefault().Value
                                                   where u.Campus == "ERNAKULAM"
                                                   select u).ToList();
                        if (Email_ERNAKULAMList.Count > 0)
                            Email_ERNAKULAMCount = Email_ERNAKULAMList.FirstOrDefault().Count;

                        var Email_CHENNAICITYList = (from u in EmailDetails.FirstOrDefault().Value
                                                     where u.Campus == "CHENNAI CITY"
                                                     select u).ToList();
                        if (Email_CHENNAICITYList.Count > 0)
                            Email_CHENNAICITYCount = Email_CHENNAICITYList.FirstOrDefault().Count;

                        var Email_CHENNAIMAINList = (from u in EmailDetails.FirstOrDefault().Value
                                                     where u.Campus == "CHENNAI MAIN"
                                                     select u).ToList();
                        if (Email_CHENNAIMAINList.Count > 0)
                            Email_CHENNAIMAINCount = Email_CHENNAIMAINList.FirstOrDefault().Count;

                        var Email_TIPSERODEList = (from u in EmailDetails.FirstOrDefault().Value
                                                   where u.Campus == "TIPS ERODE"
                                                   select u).ToList();
                        if (Email_TIPSERODEList.Count > 0)
                            Email_TIPSERODECount = Email_TIPSERODEList.FirstOrDefault().Count;

                        var Email_TIPSSALEMList = (from u in EmailDetails.FirstOrDefault().Value
                                                   where u.Campus == "TIPS SALEM"
                                                   select u).ToList();
                        if (Email_TIPSSALEMList.Count > 0)
                            Email_TIPSSALEMCount = Email_TIPSSALEMList.FirstOrDefault().Count;

                        var Email_TIPSSARANList = (from u in EmailDetails.FirstOrDefault().Value
                                                   where u.Campus == "TIPS SARAN"
                                                   select u).ToList();
                        if (Email_TIPSSARANList.Count > 0)
                            Email_TIPSSARANCount = Email_TIPSSARANList.FirstOrDefault().Count;
                    }
                    @ViewBag.Email_IBKGCount = Email_IBKGCount;
                    @ViewBag.Email_IBMAINCount = Email_IBMAINCount;
                    @ViewBag.Email_KARURKGCount = Email_KARURKGCount;
                    @ViewBag.Email_KARURCount = Email_KARURCount;
                    @ViewBag.Email_TIRUPURKGCount = Email_TIRUPURKGCount;
                    @ViewBag.Email_TIRUPURCount = Email_TIRUPURCount;
                    @ViewBag.Email_ERNAKULAMKGCount = Email_ERNAKULAMKGCount;
                    @ViewBag.Email_ERNAKULAMCount = Email_ERNAKULAMCount;
                    @ViewBag.Email_CHENNAICITYCount = Email_CHENNAICITYCount;
                    @ViewBag.Email_CHENNAIMAINCount = Email_CHENNAIMAINCount;
                    @ViewBag.Email_TIPSERODECount = Email_TIPSERODECount;
                    @ViewBag.Email_TIPSSALEMCount = Email_TIPSSALEMCount;
                    @ViewBag.Email_TIPSSARANCount = Email_TIPSSARANCount;
                    @ViewBag.AcademicYear = AcademicYear;
                    #endregion
                    #region SMS
                    long SMS_IBKGCount = 0;
                    long SMS_IBMAINCount = 0;
                    long SMS_KARURKGCount = 0;
                    long SMS_KARURCount = 0;
                    long SMS_TIRUPURKGCount = 0;
                    long SMS_TIRUPURCount = 0;
                    long SMS_ERNAKULAMKGCount = 0;
                    long SMS_ERNAKULAMCount = 0;
                    long SMS_CHENNAICITYCount = 0;
                    long SMS_CHENNAIMAINCount = 0;
                    long SMS_TIPSERODECount = 0;
                    long SMS_TIPSSALEMCount = 0;
                    long SMS_TIPSSARANCount = 0;
                    criteria.Clear();
                    if (Currnt_Date.Month >= 6 && Currnt_Date.Month <= 12)
                    {
                        AcademicYear = Currnt_Year + "-" + (Currnt_Year + 1);
                    }
                    if (Currnt_Date.Month >= 1 && Currnt_Date.Month <= 4)
                    {
                        AcademicYear = (Currnt_Year - 1) + "-" + Currnt_Year;
                    }
                    criteria.Add("AcademicYear", AcademicYear);
                    Dictionary<long, IList<AdminTemplateSMSReport_vw>> SMSDeatails = null;
                    IList<AdminTemplateSMSReport_vw> SMSDetailsList = new List<AdminTemplateSMSReport_vw>();
                    SMSDeatails = ATS.GetAdminTemplateSMSReport_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    if (SMSDeatails != null && SMSDeatails.Count > 0 && SMSDeatails.FirstOrDefault().Value.Count > 0)
                    {
                        var SMS_IBKGList = (from u in SMSDeatails.FirstOrDefault().Value
                                            where u.Campus == "IB KG"
                                            select u).ToList();
                        if (SMS_IBKGList.Count > 0)
                            SMS_IBKGCount = SMS_IBKGList.FirstOrDefault().Count;

                        var SMS_IBMAINList = (from u in SMSDeatails.FirstOrDefault().Value
                                              where u.Campus == "IB MAIN"
                                              select u).ToList();
                        if (SMS_IBMAINList.Count > 0)
                            SMS_IBMAINCount = SMS_IBMAINList.FirstOrDefault().Count;

                        var SMS_KARURKGList = (from u in SMSDeatails.FirstOrDefault().Value
                                               where u.Campus == "KARUR KG"
                                               select u).ToList();
                        if (SMS_KARURKGList.Count > 0)
                            SMS_KARURKGCount = SMS_KARURKGList.FirstOrDefault().Count;

                        var SMS_KARURList = (from u in SMSDeatails.FirstOrDefault().Value
                                             where u.Campus == "KARUR"
                                             select u).ToList();
                        if (SMS_KARURList.Count > 0)
                            SMS_KARURCount = SMS_KARURList.FirstOrDefault().Count;

                        var SMS_TIRUPURKGList = (from u in SMSDeatails.FirstOrDefault().Value
                                                 where u.Campus == "TIRUPUR KG"
                                                 select u).ToList();
                        if (SMS_TIRUPURKGList.Count > 0)
                            SMS_TIRUPURKGCount = SMS_TIRUPURKGList.FirstOrDefault().Count;

                        var SMS_TIRUPURList = (from u in SMSDeatails.FirstOrDefault().Value
                                               where u.Campus == "TIRUPUR"
                                               select u).ToList();
                        if (SMS_TIRUPURList.Count > 0)
                            SMS_TIRUPURCount = SMS_TIRUPURList.FirstOrDefault().Count;

                        var SMS_ERNAKULAMKGList = (from u in SMSDeatails.FirstOrDefault().Value
                                                   where u.Campus == "ERNAKULAM KG"
                                                   select u).ToList();
                        if (SMS_ERNAKULAMKGList.Count > 0)
                            SMS_ERNAKULAMKGCount = SMS_ERNAKULAMKGList.FirstOrDefault().Count;

                        var SMS_ERNAKULAMList = (from u in SMSDeatails.FirstOrDefault().Value
                                                 where u.Campus == "ERNAKULAM"
                                                 select u).ToList();
                        if (SMS_ERNAKULAMList.Count > 0)
                            SMS_ERNAKULAMCount = SMS_ERNAKULAMList.FirstOrDefault().Count;

                        var SMS_CHENNAICITYList = (from u in SMSDeatails.FirstOrDefault().Value
                                                   where u.Campus == "CHENNAI CITY"
                                                   select u).ToList();
                        if (SMS_CHENNAICITYList.Count > 0)
                            SMS_CHENNAICITYCount = SMS_CHENNAICITYList.FirstOrDefault().Count;

                        var SMS_CHENNAIMAINList = (from u in SMSDeatails.FirstOrDefault().Value
                                                   where u.Campus == "CHENNAI MAIN"
                                                   select u).ToList();
                        if (SMS_CHENNAIMAINList.Count > 0)
                            SMS_CHENNAIMAINCount = SMS_CHENNAIMAINList.FirstOrDefault().Count;

                        var SMS_TIPSERODEList = (from u in SMSDeatails.FirstOrDefault().Value
                                                 where u.Campus == "TIPS ERODE"
                                                 select u).ToList();
                        if (SMS_TIPSERODEList.Count > 0)
                            SMS_TIPSERODECount = SMS_TIPSERODEList.FirstOrDefault().Count;

                        var SMS_TIPSSALEMList = (from u in SMSDeatails.FirstOrDefault().Value
                                                 where u.Campus == "TIPS SALEM"
                                                 select u).ToList();
                        if (SMS_TIPSSALEMList.Count > 0)
                            SMS_TIPSSALEMCount = SMS_TIPSSALEMList.FirstOrDefault().Count;

                        var SMS_TIPSSARANList = (from u in SMSDeatails.FirstOrDefault().Value
                                                 where u.Campus == "TIPS SARAN"
                                                 select u).ToList();
                        if (SMS_TIPSSARANList.Count > 0)
                            SMS_TIPSSARANCount = SMS_TIPSSARANList.FirstOrDefault().Count;
                    }
                    @ViewBag.SMS_IBKGCount = SMS_IBKGCount;
                    @ViewBag.SMS_IBMAINCount = SMS_IBMAINCount;
                    @ViewBag.SMS_KARURKGCount = SMS_KARURKGCount;
                    @ViewBag.SMS_KARURCount = SMS_KARURCount;
                    @ViewBag.SMS_TIRUPURKGCount = SMS_TIRUPURKGCount;
                    @ViewBag.SMS_TIRUPURCount = SMS_TIRUPURCount;
                    @ViewBag.SMS_ERNAKULAMKGCount = SMS_ERNAKULAMKGCount;
                    @ViewBag.SMS_ERNAKULAMCount = SMS_ERNAKULAMCount;
                    @ViewBag.SMS_CHENNAICITYCount = SMS_CHENNAICITYCount;
                    @ViewBag.SMS_CHENNAIMAINCount = SMS_CHENNAIMAINCount;
                    @ViewBag.SMS_TIPSERODECount = SMS_TIPSERODECount;
                    @ViewBag.SMS_TIPSSALEMCount = SMS_TIPSSALEMCount;
                    @ViewBag.SMS_TIPSSARANCount = SMS_TIPSSARANCount;
                    @ViewBag.AcademicYear = AcademicYear;
                    #endregion
                    #region Store
                    criteria.Clear();
                    StoreService Store = new StoreService();
                    long MaterialGroupMasterCount = 0;
                    long MaterialSubGroupMasterCount = 0;
                    long MaterialMasterCount = 0;
                    string[] ColorCodes1 = { "8B008B", "0e6aad", "8A2BE2", "FF4500", "f8bd19", "e44a00", "008ee4", "6baa01", "f8bd19", "BDB76B" };
                    //MaterialGroupMaster
                    Dictionary<long, IList<MaterialGroupMaster>> MaterialGroupMasterDetails = null;
                    MaterialGroupMasterDetails = Store.GetMaterialGroupMasterListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);

                    if (MaterialGroupMasterDetails != null && MaterialGroupMasterDetails.Count > 0 && MaterialGroupMasterDetails.FirstOrDefault().Value.Count > 0)
                    {
                        MaterialGroupMasterCount = MaterialGroupMasterDetails.FirstOrDefault().Key;
                    }

                    //MaterialSubGroupMasterCount
                    Dictionary<long, IList<MaterialSubGroupMaster>> MaterialSubGroupMasterDetails = null;
                    MaterialSubGroupMasterDetails = Store.GetMaterialSubGroupMasterListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    if (MaterialSubGroupMasterDetails != null && MaterialSubGroupMasterDetails.Count > 0 && MaterialSubGroupMasterDetails.FirstOrDefault().Value.Count > 0)
                    {
                        MaterialSubGroupMasterCount = MaterialSubGroupMasterDetails.FirstOrDefault().Key;
                    }

                    //MaterialMasterCount
                    Dictionary<long, IList<MaterialsMaster>> MaterialsMasterDetails = null;
                    MaterialsMasterDetails = Store.GetMaterialsMasterlistWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    if (MaterialsMasterDetails != null && MaterialsMasterDetails.Count > 0 && MaterialsMasterDetails.FirstOrDefault().Value.Count > 0)
                    {
                        MaterialMasterCount = MaterialsMasterDetails.FirstOrDefault().Key;
                    }
                    @ViewBag.MaterialGroupMasterCount = MaterialGroupMasterCount;
                    @ViewBag.MaterialSubGroupMasterCount = MaterialSubGroupMasterCount;
                    @ViewBag.MaterialMasterCount = MaterialMasterCount;
                    #endregion
                    #region Users
                    long Users_CMSUsersCount = 0;
                    long Users_ParentCount = 0;
                    long Users_StaffCount = 0;
                    long Users_StudentCount = 0;
                    long Users_OthersCount = 0;
                    criteria.Clear();
                    Dictionary<long, IList<AdminTemplateUsersTypeReport_vw>> UserTypeDetails = null;
                    string[] ColorCodes2 = { "0000FF", "8A2BE2", "A52A2A", "DEB887", "5F9EA0", "BDB76B", "8B008B", "556B2F", "FF8C00", "FF00FF", "191970", "CD853F", "FF0000", "708090", "008080", "9ACD32" };
                    IList<AdminTemplateUsersTypeReport_vw> UserTypeDetailsList = new List<AdminTemplateUsersTypeReport_vw>();
                    UserTypeDetails = ATS.GetAdminTemplateUsersTypeReport_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    if (UserTypeDetails != null && UserTypeDetails.Count > 0 && UserTypeDetails.FirstOrDefault().Value.Count > 0)
                    {
                        int TotalCount = Convert.ToInt32(UserTypeDetails.FirstOrDefault().Key);
                        Campus = string.Empty;
                        for (int i = 0; i < TotalCount; i++)
                        {
                            AdminTemplateUsersTypeReport_vw Obj = new AdminTemplateUsersTypeReport_vw();
                            if (!string.IsNullOrEmpty(UserTypeDetails.FirstOrDefault().Value[i].UserType)) { Obj.UserType = UserTypeDetails.FirstOrDefault().Value[i].UserType; }
                            if (UserTypeDetails.FirstOrDefault().Value[i].Count >= 0) { Obj.Count = UserTypeDetails.FirstOrDefault().Value[i].Count; }
                            if (Obj != null) { UserTypeDetailsList.Add(Obj); }
                        }
                    }
                    if (UserTypeDetailsList != null && UserTypeDetailsList.Count > 0)
                    {
                        var Users_CMSUsersList = (from u in UserTypeDetailsList
                                                  where u.UserType == "CMSUsers"
                                                  select u).ToList();
                        if (Users_CMSUsersList != null && Users_CMSUsersList.Count > 0) { Users_CMSUsersCount = Users_CMSUsersList.FirstOrDefault().Count; }


                        var Users_ParentList = (from u in UserTypeDetailsList
                                                where u.UserType == "Parent"
                                                select u).ToList();
                        if (Users_ParentList != null && Users_ParentList.Count > 0) { Users_ParentCount = Users_ParentList.FirstOrDefault().Count; }

                        var Users_StaffList = (from u in UserTypeDetailsList
                                               where u.UserType == "Staff"
                                               select u).ToList();
                        if (Users_StaffList != null && Users_StaffList.Count > 0) { Users_StaffCount = Users_StaffList.FirstOrDefault().Count; }

                        var Users_StudentList = (from u in UserTypeDetailsList
                                                 where u.UserType == "Student"
                                                 select u).ToList();
                        if (Users_StudentList != null && Users_StudentList.Count > 0) { Users_StudentCount = Users_StudentList.FirstOrDefault().Count; }

                        var Users_OthersList = (from u in UserTypeDetailsList
                                                where u.UserType == "Others"
                                                select u).ToList();
                        if (Users_OthersList != null && Users_OthersList.Count > 0) { Users_OthersCount = Users_OthersList.FirstOrDefault().Count; }
                    }
                    @ViewBag.Users_CMSUsersCount = Users_CMSUsersCount;
                    @ViewBag.Users_ParentCount = Users_ParentCount;
                    @ViewBag.Users_StaffCount = Users_StaffCount;
                    @ViewBag.Users_StudentCount = Users_StudentCount;
                    @ViewBag.Users_OthersCount = Users_OthersCount;
                    #endregion
                    #region Login History
                    long Login_CMSUsersCount = 0;
                    long Login_ParentCount = 0;
                    long Login_StaffCount = 0;
                    long Login_StudentCount = 0;
                    long Login_OthersCount = 0;
                    criteria.Clear();
                    Dictionary<long, IList<AdminTemplateLoginHistory_vw>> LoginHistoryDetails = null;
                    LoginHistoryDetails = ATS.GetAdminTemplateLoginHistory_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    if (LoginHistoryDetails != null && LoginHistoryDetails.Count > 0 && LoginHistoryDetails.FirstOrDefault().Value.Count > 0)
                    {
                        var Login_CMSUsersList = (from u in LoginHistoryDetails.FirstOrDefault().Value
                                                  where u.UserType == "CMSUsers"
                                                  select u).ToList();
                        if (Login_CMSUsersList.Count > 0) { Login_CMSUsersCount = Login_CMSUsersList.FirstOrDefault().Count; }

                        var Login_ParentList = (from u in LoginHistoryDetails.FirstOrDefault().Value
                                                where u.UserType == "Parent"
                                                select u).ToList();
                        if (Login_ParentList.Count > 0) { Login_ParentCount = Login_ParentList.FirstOrDefault().Count; }

                        var Login_StaffList = (from u in LoginHistoryDetails.FirstOrDefault().Value
                                               where u.UserType == "Staff"
                                               select u).ToList();
                        if (Login_StaffList.Count > 0) { Login_StaffCount = Login_StaffList.FirstOrDefault().Count; }

                        var Login_StudentList = (from u in LoginHistoryDetails.FirstOrDefault().Value
                                                 where u.UserType == "Student"
                                                 select u).ToList();
                        if (Login_StudentList.Count > 0) { Login_StudentCount = Login_StudentList.FirstOrDefault().Count; }

                        var Login_OthersList = (from u in LoginHistoryDetails.FirstOrDefault().Value
                                                where u.UserType == "Others"
                                                select u).ToList();
                        if (Login_OthersList.Count > 0) { Login_OthersCount = Login_OthersList.FirstOrDefault().Count; }

                    }
                    @ViewBag.Login_CMSUsersCount = Login_CMSUsersCount;
                    @ViewBag.Login_ParentCount = Login_ParentCount;
                    @ViewBag.Login_StaffCount = Login_StaffCount;
                    @ViewBag.Login_StudentCount = Login_StudentCount;
                    @ViewBag.Login_OthersCount = Login_OthersCount;
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
                        CampusChart = CampusChart + " <dataset seriesname='Students' color='F6BD0F'>";
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
                        CampusChart = CampusChart + " <dataset seriesname='Staff' color='8BBA00'>";
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
                        CampusChart = CampusChart + " <dataset seriesname='Users' color='08E8E' parentyaxis='S' renderas='Line'>";
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
        //#endregion
        #region Staff
        public ActionResult StaffManagement()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region Department Wise Counts
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    long AcademicsDeptCnt = 0;
                    long AdmissionDeptCnt = 0;
                    long AdministrativeDeptCnt = 0;
                    long AccountsDeptCnt = 0;
                    long TransportDeptCnt = 0;
                    long HRDeptCnt = 0;
                    long StoreDeptCnt = 0;
                    long ITDeptCnt = 0;
                    long HostelCnt = 0;
                    long EDPCnt = 0;
                    criteria.Clear();
                    AdminTemplateService ATS = new AdminTemplateService();
                    Dictionary<long, IList<AdminTemplateStaffGroupByDept_vw>> AdminTemplateStaffGroupByDept_vwDetails = null;
                    AdminTemplateStaffGroupByDept_vwDetails = ATS.GetAdminTemplateStaffGroupByDept_vwListWithPagingAndCriteria(null, 999999, string.Empty, string.Empty, criteria);
                    if (AdminTemplateStaffGroupByDept_vwDetails != null && AdminTemplateStaffGroupByDept_vwDetails.Count > 0 && AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value.Count > 0)
                    {
                        var AcademicsList = (from u in AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value
                                             where u.Department == "Academics"
                                             select u).ToList();
                        if (AcademicsList.Count > 0)
                            AcademicsDeptCnt = AcademicsList.Count;


                        var AdmissionList = (from u in AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value
                                             where u.Department == "Admission"
                                             select u).ToList();
                        if (AcademicsList.Count > 0)
                            AdmissionDeptCnt = AdmissionList.Count;



                        var AdministrativeList = (from u in AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value
                                                  where u.Department == "Administrative"
                                                  select u).ToList();
                        if (AcademicsList.Count > 0)
                            AdministrativeDeptCnt = AdministrativeList.Count;


                        var AccountsList = (from u in AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value
                                            where u.Department == "Accounts"
                                            select u).ToList();
                        if (AcademicsList.Count > 0)
                            AccountsDeptCnt = AccountsList.Count;




                        var TransportList = (from u in AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value
                                             where u.Department == "Transport"
                                             select u).ToList();
                        if (AcademicsList.Count > 0)
                            TransportDeptCnt = TransportList.Count;


                        var HRList = (from u in AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value
                                      where u.Department == "HR"
                                      select u).ToList();
                        if (AcademicsList.Count > 0)
                            HRDeptCnt = HRList.Count;



                        var StoreList = (from u in AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value
                                         where u.Department == "Store"
                                         select u).ToList();
                        if (AcademicsList.Count > 0)
                            StoreDeptCnt = StoreList.Count;


                        var ITList = (from u in AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value
                                      where u.Department == "IT"
                                      select u).ToList();
                        if (AcademicsList.Count > 0)
                            ITDeptCnt = ITList.Count;


                        var HostelList = (from u in AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value
                                          where u.Department == "Hostel"
                                          select u).ToList();
                        if (HostelList.Count > 0)
                            HostelCnt = HostelList.Count;


                        var EDPList = (from u in AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value
                                       where u.Department == "EDP"
                                       select u).ToList();
                        if (EDPList.Count > 0)
                            EDPCnt = EDPList.Count;
                    }
                    @ViewBag.AcademicsDeptCnt = AcademicsDeptCnt;
                    @ViewBag.AdmissionDeptCnt = AdmissionDeptCnt;
                    @ViewBag.AdministrativeDeptCnt = AdministrativeDeptCnt;
                    @ViewBag.AccountsDeptCnt = AccountsDeptCnt;
                    @ViewBag.TransportDeptCnt = TransportDeptCnt;
                    @ViewBag.HRDeptCnt = HRDeptCnt;
                    @ViewBag.StoreDeptCnt = StoreDeptCnt;
                    @ViewBag.ITDeptCnt = ITDeptCnt;
                    @ViewBag.HostelCnt = HostelCnt;
                    @ViewBag.EDPCnt = EDPCnt;
                    #endregion

                    #region Campus Wise Counts

                    criteria.Clear();
                    long IBKGCount = 0;
                    long IBMAINCount = 0;
                    long KARURKGCount = 0;
                    long KARURCount = 0;
                    long TIRUPURKGCount = 0;
                    long TIRUPURCount = 0;
                    long ERNAKULAMKGCount = 0;
                    long ERNAKULAMCount = 0;
                    long CHENNAICITYCount = 0;
                    long CHENNAIMAINCount = 0;
                    long TIPSERODECount = 0;
                    long TIPSSALEMCount = 0;
                    long TIPSSARANCount = 0;
                    Dictionary<long, IList<AdminTemplateStaffDetails_vw>> AdminTemplateStaffDetails_vwDetails = null;
                    AdminTemplateStaffDetails_vwDetails = ATS.GetAdminTemplateStaffDetails_vwListWithPagingAndCriteria(null, 999999, string.Empty, string.Empty, criteria);
                    if (AdminTemplateStaffDetails_vwDetails != null && AdminTemplateStaffDetails_vwDetails.Count > 0 && AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value.Count > 0)
                    {

                        var IBKGList = (from u in AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value
                                        where u.Campus == "IB KG"
                                        select u).ToList();
                        if (IBKGList.Count > 0)
                            IBKGCount = IBKGList.FirstOrDefault().Count;

                        var IBMAINList = (from u in AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value
                                          where u.Campus == "IB MAIN"
                                          select u).ToList();
                        if (IBMAINList.Count > 0)
                            IBMAINCount = IBMAINList.FirstOrDefault().Count;


                        var KARURKGList = (from u in AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value
                                           where u.Campus == "KARUR KG"
                                           select u).ToList();
                        if (KARURKGList.Count > 0)
                            KARURKGCount = KARURKGList.FirstOrDefault().Count;

                        var KARURList = (from u in AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value
                                         where u.Campus == "KARUR"
                                         select u).ToList();
                        if (KARURList.Count > 0)
                            KARURCount = KARURList.FirstOrDefault().Count;

                        var TIRUPURKGList = (from u in AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value
                                             where u.Campus == "TIRUPUR KG"
                                             select u).ToList();
                        if (TIRUPURKGList.Count > 0)
                            TIRUPURKGCount = TIRUPURKGList.FirstOrDefault().Count;

                        var TIRUPURList = (from u in AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value
                                           where u.Campus == "TIRUPUR"
                                           select u).ToList();
                        if (TIRUPURList.Count > 0)
                            TIRUPURCount = TIRUPURList.FirstOrDefault().Count;

                        var ERNAKULAMKGList = (from u in AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value
                                               where u.Campus == "ERNAKULAM KG"
                                               select u).ToList();
                        if (ERNAKULAMKGList.Count > 0)
                            ERNAKULAMKGCount = ERNAKULAMKGList.FirstOrDefault().Count;

                        var ERNAKULAMList = (from u in AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value
                                             where u.Campus == "ERNAKULAM"
                                             select u).ToList();
                        if (ERNAKULAMList.Count > 0)
                            ERNAKULAMCount = ERNAKULAMList.FirstOrDefault().Count;

                        var CHENNAICITYList = (from u in AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value
                                               where u.Campus == "CHENNAI CITY"
                                               select u).ToList();
                        if (CHENNAICITYList.Count > 0)
                            CHENNAICITYCount = CHENNAICITYList.FirstOrDefault().Count;

                        var CHENNAIMAINList = (from u in AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value
                                               where u.Campus == "CHENNAI MAIN"
                                               select u).ToList();
                        if (CHENNAIMAINList.Count > 0)
                            CHENNAIMAINCount = CHENNAIMAINList.FirstOrDefault().Count;

                        var TIPSERODEList = (from u in AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value
                                             where u.Campus == "TIPS ERODE"
                                             select u).ToList();
                        if (TIPSERODEList.Count > 0)
                            TIPSERODECount = TIPSERODEList.FirstOrDefault().Count;

                        var TIPSSALEMList = (from u in AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value
                                             where u.Campus == "TIPS SALEM"
                                             select u).ToList();
                        if (TIPSSALEMList.Count > 0)
                            TIPSSALEMCount = TIPSSALEMList.FirstOrDefault().Count;

                        var TIPSSARANList = (from u in AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value
                                             where u.Campus == "TIPS SARAN"
                                             select u).ToList();
                        if (TIPSSARANList.Count > 0)
                            TIPSSARANCount = TIPSSARANList.FirstOrDefault().Count;
                    }
                    @ViewBag.IBKGCount = IBKGCount;
                    @ViewBag.IBMAINCount = IBMAINCount;
                    @ViewBag.KARURKGCount = KARURKGCount;
                    @ViewBag.KARURCount = KARURCount;
                    @ViewBag.TIRUPURKGCount = TIRUPURKGCount;
                    @ViewBag.TIRUPURCount = TIRUPURCount;
                    @ViewBag.ERNAKULAMKGCount = ERNAKULAMKGCount;
                    @ViewBag.ERNAKULAMCount = ERNAKULAMCount;
                    @ViewBag.CHENNAICITYCount = CHENNAICITYCount;
                    @ViewBag.CHENNAIMAINCount = CHENNAIMAINCount;
                    @ViewBag.TIPSERODECount = TIPSERODECount;
                    @ViewBag.TIPSSALEMCount = TIPSSALEMCount;
                    @ViewBag.TIPSSARANCount = TIPSSARANCount;
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
        public ActionResult StaffManagementCampusWiseChart()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region Code

                    AdminTemplateService ATS = new AdminTemplateService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    string[] ColorCodes = { "0000FF", "8A2BE2", "A52A2A", "DEB887", "5F9EA0", "BDB76B", "8B008B", "556B2F", "FF8C00", "FF00FF", "191970", "CD853F", "FF0000", "708090", "008080", "9ACD32" };
                    criteria.Clear();
                    Dictionary<long, IList<AdminTemplateStaffDetails_vw>> AdminTemplateStaffDetails_vwDetails = null;
                    IList<AdminTemplateStaffDetails_vw> CampusWiseStaffDetails = new List<AdminTemplateStaffDetails_vw>();
                    AdminTemplateStaffDetails_vwDetails = ATS.GetAdminTemplateStaffDetails_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    if (AdminTemplateStaffDetails_vwDetails != null && AdminTemplateStaffDetails_vwDetails.Count > 0 && AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value.Count > 0)
                    {
                        int TotalCount = Convert.ToInt32(AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Key);
                        string Campus = string.Empty;
                        for (int i = 0; i < TotalCount; i++)
                        {
                            AdminTemplateStaffDetails_vw Obj = new AdminTemplateStaffDetails_vw();
                            if (!string.IsNullOrEmpty(AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value[i].Campus)) { Obj.Campus = AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value[i].Campus; }
                            if (AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value[i].Count >= 0) { Obj.Count = AdminTemplateStaffDetails_vwDetails.FirstOrDefault().Value[i].Count; }
                            if (Obj != null) { CampusWiseStaffDetails.Add(Obj); }
                        }
                    }
                    var CampusChart = "<graph caption='Campus wise Registered Staff Report' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";
                    CampusChart = CampusChart + " <set name='' value='0' color='008ee4' />";
                    int j = 0;
                    foreach (var StaffItem in CampusWiseStaffDetails)
                    {
                        CampusChart = CampusChart + " <set name='" + StaffItem.Campus + "' value='" + StaffItem.Count + "' color='" + ColorCodes[j] + "' />";
                        j++;
                    }
                    CampusChart = CampusChart + " <set name='' value='0' color='008ee4' />";
                    CampusChart = CampusChart + "</graph>";
                    Response.Write(CampusChart);
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
        public ActionResult StaffManagementStatusWiseChart()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region Code
                    AdminTemplateService ATS = new AdminTemplateService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    string[] ColorCodes = { "0000FF", "8A2BE2", "A52A2A", "DEB887", "5F9EA0", "BDB76B", "8B008B", "556B2F", "FF8C00", "FF00FF", "191970", "CD853F", "FF0000", "708090", "008080", "9ACD32" };
                    criteria.Clear();
                    Dictionary<long, IList<AdminTemplateStaffStatus_vw>> AdminTemplateStaffStatus_vwDetails = null;
                    IList<AdminTemplateStaffStatus_vw> StaffStatusDetails = new List<AdminTemplateStaffStatus_vw>();
                    AdminTemplateStaffStatus_vwDetails = ATS.GetAdminTemplateStaffStatus_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    if (AdminTemplateStaffStatus_vwDetails != null && AdminTemplateStaffStatus_vwDetails.Count > 0 && AdminTemplateStaffStatus_vwDetails.FirstOrDefault().Value.Count > 0)
                    {
                        int TotalCount = Convert.ToInt32(AdminTemplateStaffStatus_vwDetails.FirstOrDefault().Key);
                        //string Status = string.Empty;
                        for (int i = 0; i < TotalCount; i++)
                        {
                            AdminTemplateStaffStatus_vw Obj = new AdminTemplateStaffStatus_vw();
                            if (!string.IsNullOrEmpty(AdminTemplateStaffStatus_vwDetails.FirstOrDefault().Value[i].Status)) { Obj.Status = AdminTemplateStaffStatus_vwDetails.FirstOrDefault().Value[i].Status; }
                            if (AdminTemplateStaffStatus_vwDetails.FirstOrDefault().Value[i].Count >= 0) { Obj.Count = AdminTemplateStaffStatus_vwDetails.FirstOrDefault().Value[i].Count; }
                            if (Obj != null) { StaffStatusDetails.Add(Obj); }
                        }
                    }
                    var CampusChart = "<graph caption='Status wise Staff Report' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";
                    CampusChart = CampusChart + " <set name='' value='0' color='008ee4' />";
                    int j = 0;
                    foreach (var StaffItem in StaffStatusDetails)
                    {
                        CampusChart = CampusChart + " <set name='" + StaffItem.Status + "' value='" + StaffItem.Count + "' color='" + ColorCodes[j] + "' />";
                        j++;
                    }
                    CampusChart = CampusChart + " <set name='' value='0' color='008ee4' />";
                    CampusChart = CampusChart + "</graph>";
                    Response.Write(CampusChart);
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
        public ActionResult StaffDeptWiseChart(string Campus)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    AdminTemplateService ATS = new AdminTemplateService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    string[] ColorCodes = { "0000FF", "8A2BE2", "A52A2A", "DEB887", "5F9EA0", "BDB76B", "8B008B", "556B2F", "FF8C00", "FF00FF", "191970", "CD853F", "FF0000", "708090", "008080", "9ACD32" };
                    Dictionary<long, IList<AdminTemplateStaffsGrpByDeptAndCampus_vw>> AdminTemplateStaffsGrpByDeptAndCampus_vwDetails = null;
                    IList<AdminTemplateStaffsGrpByDeptAndCampus_vw> StaffDeptDetails = new List<AdminTemplateStaffsGrpByDeptAndCampus_vw>();
                    criteria.Clear();
                    if (Campus == "" || Campus == "All")
                    {
                        Dictionary<long, IList<AdminTemplateStaffGroupByDept_vw>> AdminTemplateStaffGroupByDept_vwDetails = null;
                        IList<AdminTemplateStaffGroupByDept_vw> AllCampusStaffDetails = new List<AdminTemplateStaffGroupByDept_vw>();
                        AdminTemplateStaffGroupByDept_vwDetails = ATS.GetAdminTemplateStaffGroupByDept_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                        if (AdminTemplateStaffGroupByDept_vwDetails != null && AdminTemplateStaffGroupByDept_vwDetails.Count > 0 && AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value.Count > 0)
                        {
                            int TotalCount = Convert.ToInt32(AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Key);
                            for (int i = 0; i < TotalCount; i++)
                            {
                                AdminTemplateStaffGroupByDept_vw Obj = new AdminTemplateStaffGroupByDept_vw();
                                if (!string.IsNullOrEmpty(AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value[i].Department) && (AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value[i].Count >= 0))
                                {
                                    Obj.Department = AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value[i].Department;
                                    Obj.Count = AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value[i].Count;
                                }
                                if (AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value[i].Department == null && (AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value[i].Count >= 0))
                                {
                                    Obj.Department = "Others";
                                    Obj.Count = AdminTemplateStaffGroupByDept_vwDetails.FirstOrDefault().Value[i].Count;
                                }
                                if (Obj != null) { AllCampusStaffDetails.Add(Obj); }
                            }
                        }
                        var CampusChart = "<graph caption='Department wise Staff Report for " + Campus + "' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";
                        CampusChart = CampusChart + " <set name='' value='0' color='08E8E' />";
                        int j = 0;
                        foreach (var StaffItem in AllCampusStaffDetails)
                        {
                            bool Val = StaffItem.Department.Contains("&");
                            if (Val == true) { StaffItem.Department = StaffItem.Department.Replace("&", "or"); }
                            CampusChart = CampusChart + " <set name='" + StaffItem.Department + "' value='" + StaffItem.Count + "' color='08E8E'/>";
                            j++;
                        }
                        CampusChart = CampusChart + " <set name='' value='0' color='08E8E' />";
                        CampusChart = CampusChart + "</graph>";
                        Response.Write(CampusChart);
                        return null;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                        AdminTemplateStaffsGrpByDeptAndCampus_vwDetails = ATS.GetAdminTemplateStaffsGrpByDeptAndCampus_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                        if (AdminTemplateStaffsGrpByDeptAndCampus_vwDetails != null && AdminTemplateStaffsGrpByDeptAndCampus_vwDetails.Count > 0 && AdminTemplateStaffsGrpByDeptAndCampus_vwDetails.FirstOrDefault().Value.Count > 0)
                        {
                            int TotalCount = Convert.ToInt32(AdminTemplateStaffsGrpByDeptAndCampus_vwDetails.FirstOrDefault().Key);
                            for (int i = 0; i < TotalCount; i++)
                            {
                                AdminTemplateStaffsGrpByDeptAndCampus_vw Obj = new AdminTemplateStaffsGrpByDeptAndCampus_vw();
                                if (!string.IsNullOrEmpty(AdminTemplateStaffsGrpByDeptAndCampus_vwDetails.FirstOrDefault().Value[i].Department) && (AdminTemplateStaffsGrpByDeptAndCampus_vwDetails.FirstOrDefault().Value[i].Count >= 0))
                                {
                                    Obj.Department = AdminTemplateStaffsGrpByDeptAndCampus_vwDetails.FirstOrDefault().Value[i].Department;
                                    Obj.Count = AdminTemplateStaffsGrpByDeptAndCampus_vwDetails.FirstOrDefault().Value[i].Count;
                                }
                                if (AdminTemplateStaffsGrpByDeptAndCampus_vwDetails.FirstOrDefault().Value[i].Department == null && (AdminTemplateStaffsGrpByDeptAndCampus_vwDetails.FirstOrDefault().Value[i].Count >= 0))
                                {
                                    Obj.Department = "Others";
                                    Obj.Count = AdminTemplateStaffsGrpByDeptAndCampus_vwDetails.FirstOrDefault().Value[i].Count;
                                }
                                if (Obj != null) { StaffDeptDetails.Add(Obj); }
                            }
                        }
                        var CampusChart = "<graph caption='Department wise Staff Report for " + Campus + "' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";
                        CampusChart = CampusChart + " <set name='' value='0' color='08E8E' />";
                        int j = 0;
                        foreach (var StaffItem in StaffDeptDetails)
                        {
                            bool Val = StaffItem.Department.Contains("&");
                            if (Val == true) { StaffItem.Department = StaffItem.Department.Replace("&", "or"); }
                            CampusChart = CampusChart + " <set name='" + StaffItem.Department + "' value='" + StaffItem.Count + "' color='08E8E'/>";
                            j++;
                        }
                        CampusChart = CampusChart + " <set name='' value='0' color='08E8E' />";
                        CampusChart = CampusChart + "</graph>";
                        Response.Write(CampusChart);
                        return null;
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        #endregion
        #region AdmissionManagement
        public ActionResult AdmissionManagement(string Campus)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region Code
                    AdminTemplateService ATS = new AdminTemplateService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    DateTime Currnt_Date = DateTime.Now;
                    int Currnt_Year = Currnt_Date.Year;
                    string CurrentAcademicYear = string.Empty;
                    if (Currnt_Date.Month >= 6 && Currnt_Date.Month <= 12)
                    {
                        CurrentAcademicYear = Currnt_Year + "-" + (Currnt_Year + 1);
                    }
                    if (Currnt_Date.Month >= 1 && Currnt_Date.Month <= 4)
                    {
                        CurrentAcademicYear = (Currnt_Year - 1) + "-" + Currnt_Year;
                    }
                    long ResidentialCount = 0;
                    long DayScholorCount = 0;
                    long WeekBoarderCount = 0;
                    Dictionary<long, IList<AdminTemplateStudBoardingReport_vw>> BoardingTypeDetails = null;
                    string[] ColorCodes = { "0000FF", "8A2BE2", "A52A2A", "DEB887", "5F9EA0", "BDB76B", "8B008B", "556B2F", "FF8C00", "FF00FF", "191970", "CD853F", "FF0000", "708090", "008080", "9ACD32" };
                    IList<AdminTemplateStudBoardingReport_vw> BoardingTypeList = new List<AdminTemplateStudBoardingReport_vw>();
                    criteria.Add("AcademicYear", CurrentAcademicYear);
                    if (Campus == "") { Campus = "All"; }
                    if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                    BoardingTypeDetails = ATS.GetAdminTemplateStudBoardingReport_vwListWithPagingAndCriteria(null, 999999, string.Empty, string.Empty, criteria);
                    if (BoardingTypeDetails != null && BoardingTypeDetails.Count > 0 && BoardingTypeDetails.FirstOrDefault().Value.Count > 0)
                    {
                        int TotalCount = Convert.ToInt32(BoardingTypeDetails.FirstOrDefault().Key);
                        Campus = string.Empty;
                        for (int i = 0; i < TotalCount; i++)
                        {
                            AdminTemplateStudBoardingReport_vw Obj = new AdminTemplateStudBoardingReport_vw();
                            if (!string.IsNullOrEmpty(BoardingTypeDetails.FirstOrDefault().Value[i].BoardingType)) { Obj.BoardingType = BoardingTypeDetails.FirstOrDefault().Value[i].BoardingType; }
                            if (BoardingTypeDetails.FirstOrDefault().Value[i].Count >= 0) { Obj.Count = BoardingTypeDetails.FirstOrDefault().Value[i].Count; }
                            if (Obj != null) { BoardingTypeList.Add(Obj); }
                        }
                    }
                    var ResidentialList = (from u in BoardingTypeList
                                           where u.BoardingType == "Residential"
                                           select u).ToList();
                    if (ResidentialList.Count > 0)
                    {
                        ResidentialCount = ResidentialList.FirstOrDefault().Count;
                    }
                    var DayScholorList = (from u in BoardingTypeList
                                          where u.BoardingType == "Day Scholar"
                                          select u).ToList();
                    if (DayScholorList.Count > 0)
                    {
                        DayScholorCount = DayScholorList.FirstOrDefault().Count;
                    }
                    var WeekBoarderList = (from u in BoardingTypeList
                                           where u.BoardingType == "Week Boarder"
                                           select u).ToList();
                    if (WeekBoarderList.Count > 0)
                    {
                        WeekBoarderCount = WeekBoarderList.FirstOrDefault().Count;
                    }

                    @ViewBag.ResidentialCount = ResidentialCount;
                    @ViewBag.DayScholorCount = DayScholorCount;
                    @ViewBag.WeekBoarderCount = WeekBoarderCount;
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
        public ActionResult GetBoardingTypeCount(string Campus)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region Code
                    AdminTemplateService ATS = new AdminTemplateService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    DateTime Currnt_Date = DateTime.Now;
                    int Currnt_Year = Currnt_Date.Year;
                    string CurrentAcademicYear = string.Empty;
                    if (Currnt_Date.Month >= 6 && Currnt_Date.Month <= 12)
                    {
                        CurrentAcademicYear = Currnt_Year + "-" + (Currnt_Year + 1);
                    }
                    if (Currnt_Date.Month >= 1 && Currnt_Date.Month <= 4)
                    {
                        CurrentAcademicYear = (Currnt_Year - 1) + "-" + Currnt_Year;
                    }
                    long ResidentialCount = 0;
                    long DayScholorCount = 0;
                    long WeekBoarderCount = 0;
                    Dictionary<long, IList<AdminTemplateStudBoardingReport_vw>> BoardingTypeDetails = null;
                    string[] ColorCodes = { "0000FF", "8A2BE2", "A52A2A", "DEB887", "5F9EA0", "BDB76B", "8B008B", "556B2F", "FF8C00", "FF00FF", "191970", "CD853F", "FF0000", "708090", "008080", "9ACD32" };
                    IList<AdminTemplateStudBoardingReport_vw> BoardingTypeList = new List<AdminTemplateStudBoardingReport_vw>();
                    criteria.Add("AcademicYear", CurrentAcademicYear);
                    if (Campus == "") { Campus = "All"; }
                    if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                    BoardingTypeDetails = ATS.GetAdminTemplateStudBoardingReport_vwListWithPagingAndCriteria(null, 999999, string.Empty, string.Empty, criteria);
                    if (BoardingTypeDetails != null && BoardingTypeDetails.Count > 0 && BoardingTypeDetails.FirstOrDefault().Value.Count > 0)
                    {
                        int TotalCount = Convert.ToInt32(BoardingTypeDetails.FirstOrDefault().Key);
                        Campus = string.Empty;
                        for (int i = 0; i < TotalCount; i++)
                        {
                            AdminTemplateStudBoardingReport_vw Obj = new AdminTemplateStudBoardingReport_vw();
                            if (!string.IsNullOrEmpty(BoardingTypeDetails.FirstOrDefault().Value[i].BoardingType)) { Obj.BoardingType = BoardingTypeDetails.FirstOrDefault().Value[i].BoardingType; }
                            if (BoardingTypeDetails.FirstOrDefault().Value[i].Count >= 0) { Obj.Count = BoardingTypeDetails.FirstOrDefault().Value[i].Count; }
                            if (Obj != null) { BoardingTypeList.Add(Obj); }
                        }
                    }
                    var ResidentialList = (from u in BoardingTypeList
                                           where u.BoardingType == "Residential"
                                           select u).ToList();
                    if (ResidentialList.Count > 0)
                    {
                        ResidentialCount = ResidentialList.FirstOrDefault().Count;
                    }
                    var DayScholorList = (from u in BoardingTypeList
                                          where u.BoardingType == "Day Scholar"
                                          select u).ToList();
                    if (DayScholorList.Count > 0)
                    {
                        DayScholorCount = DayScholorList.FirstOrDefault().Count;
                    }
                    var WeekBoarderList = (from u in BoardingTypeList
                                           where u.BoardingType == "Week Boarder"
                                           select u).ToList();
                    if (WeekBoarderList.Count > 0)
                    {
                        WeekBoarderCount = WeekBoarderList.FirstOrDefault().Count;
                    }
                    return Json(new { ResidentialCount, DayScholorCount, WeekBoarderCount }, JsonRequestBehavior.AllowGet);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult GetAcademicYearWiseReportChart(string Campus)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region Code
                    Dictionary<string, object> criteria = new Dictionary<string, object>();

                    DateTime Currnt_Date = DateTime.Now;
                    int Currnt_Year = Currnt_Date.Year;
                    string CurrentAcademicYear = string.Empty;
                    if (Currnt_Date.Month >= 6 && Currnt_Date.Month <= 12)
                    {
                        CurrentAcademicYear = Currnt_Year + "-" + (Currnt_Year + 1);
                    }
                    if (Currnt_Date.Month >= 1 && Currnt_Date.Month <= 4)
                    {
                        CurrentAcademicYear = (Currnt_Year - 1) + "-" + Currnt_Year;
                    }

                    string FutureAcademicYear = string.Empty;
                    long CurrentAcademicYear1 = 0;
                    long CurrentAcademicYear2 = 0;
                    bool Val = CurrentAcademicYear.Contains("-");
                    if (Val == true)
                    {
                        string[] strArray = CurrentAcademicYear.Split('-');
                        CurrentAcademicYear1 = Convert.ToInt64(strArray[0]);
                        CurrentAcademicYear2 = Convert.ToInt64(strArray[1]);
                    }
                    FutureAcademicYear = (CurrentAcademicYear1 + 1).ToString() + "-" + (CurrentAcademicYear2 + 1).ToString();

                    string[] AcademicYears = { FutureAcademicYear, CurrentAcademicYear };

                    criteria.Clear();

                    AdminTemplateService ATS = new AdminTemplateService();
                    if (Campus == "") { Campus = "All"; }
                    if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                    if (AcademicYears.Length > 0) { criteria.Add("AcademicYear", AcademicYears); }
                    Dictionary<long, IList<AdminTemplateStudentsAcademicYearWiseCount_vw>> AcademicYearWiseDetails = null;
                    AcademicYearWiseDetails = ATS.GetAdminTemplateStudentsAcademicYearWiseCount_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    //List<AdminTemplateStudentsAcademicYearWiseCount_vw> AcademicYearWiseList = new List<AdminTemplateStudentsAcademicYearWiseCount_vw>();
                    //if (AcademicYearWiseDetails != null && AcademicYearWiseDetails.Count > 0 && AcademicYearWiseDetails.FirstOrDefault().Value.Count > 0)
                    //{
                    //    int count = Convert.ToInt32(AcademicYearWiseDetails.FirstOrDefault().Key);
                    //    for (int i = 0; i < count; i++)
                    //    {
                    //        AdminTemplateStudentsAcademicYearWiseCount_vw Obj = new AdminTemplateStudentsAcademicYearWiseCount_vw();
                    //        Obj.AcademicYear = AcademicYearWiseDetails.FirstOrDefault().Value[i].AcademicYear;
                    //        Obj.Total = AcademicYearWiseDetails.FirstOrDefault().Value[i].Total;
                    //        Obj.Boys = AcademicYearWiseDetails.FirstOrDefault().Value[i].Boys;
                    //        Obj.Girls = AcademicYearWiseDetails.FirstOrDefault().Value[i].Girls;
                    //        AcademicYearWiseList.Add(Obj);
                    //    }
                    //}
                    var MutiSeriesGraph = "<graph caption='Registered students count compare with academic years' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='0'>";
                    MutiSeriesGraph = MutiSeriesGraph + "<categories>";
                    MutiSeriesGraph = MutiSeriesGraph + "<category name='Total Students' />";
                    MutiSeriesGraph = MutiSeriesGraph + "<category name='Boys' />";
                    MutiSeriesGraph = MutiSeriesGraph + "<category name='Girls' />";
                    MutiSeriesGraph = MutiSeriesGraph + "</categories>";

                    if (AcademicYearWiseDetails != null && AcademicYearWiseDetails.FirstOrDefault().Key > 0 && (AcademicYearWiseDetails.FirstOrDefault().Value[0].AcademicYear == CurrentAcademicYear || AcademicYearWiseDetails.FirstOrDefault().Value[0].AcademicYear == FutureAcademicYear))
                    {
                        MutiSeriesGraph = MutiSeriesGraph + " <dataset seriesname='" + AcademicYearWiseDetails.FirstOrDefault().Value[0].AcademicYear + "' color='008ee4'>";
                        MutiSeriesGraph = MutiSeriesGraph + "<set value='" + AcademicYearWiseDetails.FirstOrDefault().Value[0].Total + "' />";
                        MutiSeriesGraph = MutiSeriesGraph + "<set value='" + AcademicYearWiseDetails.FirstOrDefault().Value[0].Boys + "' />";
                        MutiSeriesGraph = MutiSeriesGraph + "<set value='" + AcademicYearWiseDetails.FirstOrDefault().Value[0].Girls + "' />";
                        MutiSeriesGraph = MutiSeriesGraph + "</dataset>";
                    }
                    if (AcademicYearWiseDetails != null && AcademicYearWiseDetails.FirstOrDefault().Key > 1 && (AcademicYearWiseDetails.FirstOrDefault().Value[1].AcademicYear == FutureAcademicYear || AcademicYearWiseDetails.FirstOrDefault().Value[1].AcademicYear == CurrentAcademicYear))
                    {
                        MutiSeriesGraph = MutiSeriesGraph + " <dataset seriesname='" + AcademicYearWiseDetails.FirstOrDefault().Value[1].AcademicYear + "' color='808000'>";
                        MutiSeriesGraph = MutiSeriesGraph + "<set value='" + AcademicYearWiseDetails.FirstOrDefault().Value[1].Total + "' />";
                        MutiSeriesGraph = MutiSeriesGraph + "<set value='" + AcademicYearWiseDetails.FirstOrDefault().Value[1].Boys + "' />";
                        MutiSeriesGraph = MutiSeriesGraph + "<set value='" + AcademicYearWiseDetails.FirstOrDefault().Value[1].Girls + "' />";
                        MutiSeriesGraph = MutiSeriesGraph + "</dataset>";
                    }
                    MutiSeriesGraph = MutiSeriesGraph + "</graph>";
                    Response.Write(MutiSeriesGraph);
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
        public ActionResult GetAdmissionStatusWiseReport(string Campus)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region
                    AdminTemplateService ATS = new AdminTemplateService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    DateTime Currnt_Date = DateTime.Now;
                    int Currnt_Year = Currnt_Date.Year;
                    string CurrentAcademicYear = string.Empty;
                    if (Currnt_Date.Month >= 6 && Currnt_Date.Month <= 12)
                    {
                        CurrentAcademicYear = Currnt_Year + "-" + (Currnt_Year + 1);
                    }
                    if (Currnt_Date.Month >= 1 && Currnt_Date.Month <= 4)
                    {
                        CurrentAcademicYear = (Currnt_Year - 1) + "-" + Currnt_Year;
                    }
                    Dictionary<long, IList<AdminTemplateAdmissionStatusReport_vw>> StatusWiseDetails = null;
                    string[] ColorCodes = { "0000FF", "8A2BE2", "A52A2A", "DEB887", "5F9EA0", "BDB76B", "8B008B", "556B2F", "FF8C00", "FF00FF", "191970", "CD853F", "FF0000", "708090", "008080", "9ACD32" };
                    IList<AdminTemplateAdmissionStatusReport_vw> StatusWiseList = new List<AdminTemplateAdmissionStatusReport_vw>();
                    criteria.Add("AcademicYear", CurrentAcademicYear);
                    if (Campus == "") { Campus = "All"; }
                    if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                    StatusWiseDetails = ATS.GetAdminTemplateAdmissionStatusReport_vwListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    if (StatusWiseDetails != null && StatusWiseDetails.Count > 0 && StatusWiseDetails.FirstOrDefault().Value.Count > 0)
                    {
                        int TotalCount = Convert.ToInt32(StatusWiseDetails.FirstOrDefault().Key);
                        Campus = string.Empty;
                        for (int i = 0; i < TotalCount; i++)
                        {
                            AdminTemplateAdmissionStatusReport_vw Obj = new AdminTemplateAdmissionStatusReport_vw();
                            if (!string.IsNullOrEmpty(StatusWiseDetails.FirstOrDefault().Value[i].AdmissionStatus)) { Obj.AdmissionStatus = StatusWiseDetails.FirstOrDefault().Value[i].AdmissionStatus; }
                            if (StatusWiseDetails.FirstOrDefault().Value[i].Count >= 0) { Obj.Count = StatusWiseDetails.FirstOrDefault().Value[i].Count; }
                            if (Obj != null) { StatusWiseList.Add(Obj); }
                        }
                    }
                    var CampusChart = "<graph caption='Admission Status Report for " + CurrentAcademicYear + "' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";
                    CampusChart = CampusChart + " <set name='' value='0' color='008ee4' />";
                    int j = 0;
                    foreach (var StatusItem in StatusWiseList)
                    {
                        CampusChart = CampusChart + " <set name='" + StatusItem.AdmissionStatus + "' value='" + StatusItem.Count + "' color='" + ColorCodes[j] + "' />";
                        j++;
                    }
                    CampusChart = CampusChart + " <set name='' value='0' color='008ee4' />";
                    CampusChart = CampusChart + "</graph>";
                    Response.Write(CampusChart);
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
        public ActionResult GetAdmissionBoardingTypeReport(string Campus)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region Code
                    AdminTemplateService ATS = new AdminTemplateService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    DateTime Currnt_Date = DateTime.Now;
                    int Currnt_Year = Currnt_Date.Year;
                    string CurrentAcademicYear = string.Empty;
                    if (Currnt_Date.Month >= 6 && Currnt_Date.Month <= 12)
                    {
                        CurrentAcademicYear = Currnt_Year + "-" + (Currnt_Year + 1);
                    }
                    if (Currnt_Date.Month >= 1 && Currnt_Date.Month <= 4)
                    {
                        CurrentAcademicYear = (Currnt_Year - 1) + "-" + Currnt_Year;
                    }
                    Dictionary<long, IList<AdminTemplateStudBoardingReport_vw>> BoardingTypeDetails = null;
                    string[] ColorCodes = { "0000FF", "8A2BE2", "A52A2A", "DEB887", "5F9EA0", "BDB76B", "8B008B", "556B2F", "FF8C00", "FF00FF", "191970", "CD853F", "FF0000", "708090", "008080", "9ACD32" };
                    IList<AdminTemplateStudBoardingReport_vw> BoardingTypeList = new List<AdminTemplateStudBoardingReport_vw>();
                    criteria.Add("AcademicYear", CurrentAcademicYear);
                    if (Campus == "") { Campus = "All"; }
                    if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                    BoardingTypeDetails = ATS.GetAdminTemplateStudBoardingReport_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    if (BoardingTypeDetails != null && BoardingTypeDetails.Count > 0 && BoardingTypeDetails.FirstOrDefault().Value.Count > 0)
                    {
                        int TotalCount = Convert.ToInt32(BoardingTypeDetails.FirstOrDefault().Key);
                        Campus = string.Empty;
                        for (int i = 0; i < TotalCount; i++)
                        {
                            AdminTemplateStudBoardingReport_vw Obj = new AdminTemplateStudBoardingReport_vw();
                            if (!string.IsNullOrEmpty(BoardingTypeDetails.FirstOrDefault().Value[i].BoardingType)) { Obj.BoardingType = BoardingTypeDetails.FirstOrDefault().Value[i].BoardingType; }
                            if (BoardingTypeDetails.FirstOrDefault().Value[i].Count >= 0) { Obj.Count = BoardingTypeDetails.FirstOrDefault().Value[i].Count; }
                            if (Obj != null) { BoardingTypeList.Add(Obj); }
                        }
                    }
                    var CampusChart = "<graph caption='Boarding Type Report For " + CurrentAcademicYear + " Students' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";
                    CampusChart = CampusChart + " <set name='' value='0' color='008ee4' />";
                    int j = 0;
                    foreach (var BoardingItem in BoardingTypeList)
                    {
                        CampusChart = CampusChart + " <set name='" + BoardingItem.BoardingType + "' value='" + BoardingItem.Count + "' color='" + ColorCodes[j] + "' />";
                        j++;
                    }
                    CampusChart = CampusChart + " <set name='' value='0' color='008ee4' />";
                    CampusChart = CampusChart + "</graph>";
                    Response.Write(CampusChart);
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

        #region Students
        public ActionResult Students()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region code
                    AdmissionManagementService AMS = new AdmissionManagementService();
                    AdminTemplateService ATS = new AdminTemplateService();
                    long IBKGCount = 0;
                    long IBMAINCount = 0;
                    long KARURKGCount = 0;
                    long KARURCount = 0;
                    long TIRUPURKGCount = 0;
                    long TIRUPURCount = 0;
                    long ERNAKULAMKGCount = 0;
                    long ERNAKULAMCount = 0;
                    long CHENNAICITYCount = 0;
                    long CHENNAIMAINCount = 0;
                    long TIPSERODECount = 0;
                    long TIPSSALEMCount = 0;
                    long TIPSSARANCount = 0;
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
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
                    IList<AdminTemplateStudents_vw> CampusWiseStudentDetails = new List<AdminTemplateStudents_vw>();
                    AdminTemplateStudents_vwDetails = ATS.GetAdminTemplateStudents_vwListWithPagingAndCriteria(null, 999999, string.Empty, string.Empty, criteria);
                    if (AdminTemplateStudents_vwDetails != null && AdminTemplateStudents_vwDetails.Count > 0 && AdminTemplateStudents_vwDetails.FirstOrDefault().Value.Count > 0)
                    {
                        var IBKGList = (from u in AdminTemplateStudents_vwDetails.FirstOrDefault().Value
                                        where u.Campus == "IB KG"
                                        select u).ToList();
                        if (IBKGList.Count > 0)
                            IBKGCount = IBKGList.FirstOrDefault().Count;

                        var IBMAINList = (from u in AdminTemplateStudents_vwDetails.FirstOrDefault().Value
                                          where u.Campus == "IB MAIN"
                                          select u).ToList();
                        if (IBMAINList.Count > 0)
                            IBMAINCount = IBMAINList.FirstOrDefault().Count;


                        var KARURKGList = (from u in AdminTemplateStudents_vwDetails.FirstOrDefault().Value
                                           where u.Campus == "KARUR KG"
                                           select u).ToList();
                        if (KARURKGList.Count > 0)
                            KARURKGCount = KARURKGList.FirstOrDefault().Count;

                        var KARURList = (from u in AdminTemplateStudents_vwDetails.FirstOrDefault().Value
                                         where u.Campus == "KARUR"
                                         select u).ToList();
                        if (KARURList.Count > 0)
                            KARURCount = KARURList.FirstOrDefault().Count;

                        var TIRUPURKGList = (from u in AdminTemplateStudents_vwDetails.FirstOrDefault().Value
                                             where u.Campus == "TIRUPUR KG"
                                             select u).ToList();
                        if (TIRUPURKGList.Count > 0)
                            TIRUPURKGCount = TIRUPURKGList.FirstOrDefault().Count;

                        var TIRUPURList = (from u in AdminTemplateStudents_vwDetails.FirstOrDefault().Value
                                           where u.Campus == "TIRUPUR"
                                           select u).ToList();
                        if (TIRUPURList.Count > 0)
                            TIRUPURCount = TIRUPURList.FirstOrDefault().Count;

                        var ERNAKULAMKGList = (from u in AdminTemplateStudents_vwDetails.FirstOrDefault().Value
                                               where u.Campus == "ERNAKULAM KG"
                                               select u).ToList();
                        if (ERNAKULAMKGList.Count > 0)
                            ERNAKULAMKGCount = ERNAKULAMKGList.FirstOrDefault().Count;

                        var ERNAKULAMList = (from u in AdminTemplateStudents_vwDetails.FirstOrDefault().Value
                                             where u.Campus == "ERNAKULAM"
                                             select u).ToList();
                        if (ERNAKULAMList.Count > 0)
                            ERNAKULAMCount = ERNAKULAMList.FirstOrDefault().Count;

                        var CHENNAICITYList = (from u in AdminTemplateStudents_vwDetails.FirstOrDefault().Value
                                               where u.Campus == "CHENNAI CITY"
                                               select u).ToList();
                        if (CHENNAICITYList.Count > 0)
                            CHENNAICITYCount = CHENNAICITYList.FirstOrDefault().Count;

                        var CHENNAIMAINList = (from u in AdminTemplateStudents_vwDetails.FirstOrDefault().Value
                                               where u.Campus == "CHENNAI MAIN"
                                               select u).ToList();
                        if (CHENNAIMAINList.Count > 0)
                            CHENNAIMAINCount = CHENNAIMAINList.FirstOrDefault().Count;

                        var TIPSERODEList = (from u in AdminTemplateStudents_vwDetails.FirstOrDefault().Value
                                             where u.Campus == "TIPS ERODE"
                                             select u).ToList();
                        if (TIPSERODEList.Count > 0)
                            TIPSERODECount = TIPSERODEList.FirstOrDefault().Count;

                        var TIPSSALEMList = (from u in AdminTemplateStudents_vwDetails.FirstOrDefault().Value
                                             where u.Campus == "TIPS SALEM"
                                             select u).ToList();
                        if (TIPSSALEMList.Count > 0)
                            TIPSSALEMCount = TIPSSALEMList.FirstOrDefault().Count;

                        var TIPSSARANList = (from u in AdminTemplateStudents_vwDetails.FirstOrDefault().Value
                                             where u.Campus == "TIPS SARAN"
                                             select u).ToList();
                        if (TIPSSARANList.Count > 0)
                            TIPSSARANCount = TIPSSARANList.FirstOrDefault().Count;
                    }
                    @ViewBag.IBKGCount = IBKGCount;
                    @ViewBag.IBMAINCount = IBMAINCount;
                    @ViewBag.KARURKGCount = KARURKGCount;
                    @ViewBag.KARURCount = KARURCount;
                    @ViewBag.TIRUPURKGCount = TIRUPURKGCount;
                    @ViewBag.TIRUPURCount = TIRUPURCount;
                    @ViewBag.ERNAKULAMKGCount = ERNAKULAMKGCount;
                    @ViewBag.ERNAKULAMCount = ERNAKULAMCount;
                    @ViewBag.CHENNAICITYCount = CHENNAICITYCount;
                    @ViewBag.CHENNAIMAINCount = CHENNAIMAINCount;
                    @ViewBag.TIPSERODECount = TIPSERODECount;
                    @ViewBag.TIPSSALEMCount = TIPSSALEMCount;
                    @ViewBag.TIPSSARANCount = TIPSSARANCount;
                    //@ViewBag.CampusWiseStudentDetails = CampusWiseStudentDetails;
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
        public ActionResult GetAllCampusSudentsChart()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region Code
                    AdminTemplateService ATS = new AdminTemplateService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
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
                    Dictionary<long, IList<AdminTemplateStudentTemplate_vw>> AdminTemplateStudents_vwDetails = null;
                    string[] ColorCodes = { "0000FF", "8A2BE2", "A52A2A", "DEB887", "5F9EA0", "BDB76B", "8B008B", "556B2F", "FF8C00", "FF00FF", "191970", "CD853F", "FF0000", "708090", "008080", "9ACD32" };
                    IList<AdminTemplateStudentTemplate_vw> CampusWiseStudentDetails = new List<AdminTemplateStudentTemplate_vw>();
                    AdminTemplateStudents_vwDetails = ATS.GetAdminTemplateStudentTemplate_vwListWithPagingAndCriteria(0, 999999, string.Empty, string.Empty, criteria);
                    if (AdminTemplateStudents_vwDetails != null && AdminTemplateStudents_vwDetails.Count > 0 && AdminTemplateStudents_vwDetails.FirstOrDefault().Value.Count > 0)
                    {
                        int TotalCount = Convert.ToInt32(AdminTemplateStudents_vwDetails.FirstOrDefault().Key);
                        string Campus = string.Empty;
                        for (int i = 0; i < TotalCount; i++)
                        {
                            AdminTemplateStudentTemplate_vw Obj = new AdminTemplateStudentTemplate_vw();
                            if (!string.IsNullOrEmpty(AdminTemplateStudents_vwDetails.FirstOrDefault().Value[i].Campus)) { Obj.Campus = AdminTemplateStudents_vwDetails.FirstOrDefault().Value[i].Campus; }
                            if (AdminTemplateStudents_vwDetails.FirstOrDefault().Value[i].Count >= 0) { Obj.Count = AdminTemplateStudents_vwDetails.FirstOrDefault().Value[i].Count; }
                            if (Obj != null) { CampusWiseStudentDetails.Add(Obj); }
                        }
                    }
                    var CampusChart = "<graph caption='Campus wise Students Count for " + AcademicYear + "' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";
                    CampusChart = CampusChart + " <set name='' value='0' color='008ee4' />";
                    int j = 0;
                    foreach (var StaffItem in CampusWiseStudentDetails)
                    {
                        CampusChart = CampusChart + " <set name='" + StaffItem.Campus + "' value='" + StaffItem.Count + "' color='" + ColorCodes[j] + "' />";
                        j++;
                    }
                    CampusChart = CampusChart + " <set name='' value='0' color='008ee4' />";
                    CampusChart = CampusChart + "</graph>";
                    Response.Write(CampusChart);
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
        public ActionResult GetAllGradeSudentsChart()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region Code
                    AdminTemplateService ATS = new AdminTemplateService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
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
                    Dictionary<long, IList<AdminTemplateStudentsCountGroupByGrade_vw>> AdminTemplateStudentsCountGroupByGrade_vwDetails = null;
                    string[] ColorCodes = { "0000FF", "8A2BE2", "A52A2A", "DEB887", "5F9EA0", "BDB76B", "8B008B", "556B2F", "FF8C00", "FF00FF", "191970", "CD853F", "FF0000", "708090", "008080", "9ACD32" };
                    IList<AdminTemplateStudentsCountGroupByGrade_vw> CampusWiseStudentDetails = new List<AdminTemplateStudentsCountGroupByGrade_vw>();
                    AdminTemplateStudentsCountGroupByGrade_vwDetails = ATS.GetAdminTemplateStudentsCountGroupByGrade_vwListWithPagingAndCriteria(0, 999999, string.Empty, string.Empty, criteria);
                    if (AdminTemplateStudentsCountGroupByGrade_vwDetails != null && AdminTemplateStudentsCountGroupByGrade_vwDetails.Count > 0 && AdminTemplateStudentsCountGroupByGrade_vwDetails.FirstOrDefault().Value.Count > 0)
                    {
                        int TotalCount = Convert.ToInt32(AdminTemplateStudentsCountGroupByGrade_vwDetails.FirstOrDefault().Key);
                        string Grade = string.Empty;
                        for (int i = 0; i < TotalCount; i++)
                        {
                            AdminTemplateStudentsCountGroupByGrade_vw Obj = new AdminTemplateStudentsCountGroupByGrade_vw();
                            if (!string.IsNullOrEmpty(AdminTemplateStudentsCountGroupByGrade_vwDetails.FirstOrDefault().Value[i].Grade)) { Obj.Grade = AdminTemplateStudentsCountGroupByGrade_vwDetails.FirstOrDefault().Value[i].Grade; }
                            if (AdminTemplateStudentsCountGroupByGrade_vwDetails.FirstOrDefault().Value[i].Total >= 0) { Obj.Total = AdminTemplateStudentsCountGroupByGrade_vwDetails.FirstOrDefault().Value[i].Total; }
                            if (Obj != null) { CampusWiseStudentDetails.Add(Obj); }
                        }
                    }
                    var CampusChart = "<graph caption='Campus Wise Count Reports' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='0' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";
                    CampusChart = CampusChart + " <set name='' value='0' color='008ee4' />";
                    int j = 0;
                    foreach (var StudItem in CampusWiseStudentDetails)
                    {
                        CampusChart = CampusChart + " <set name='" + StudItem.Grade + "' value='" + StudItem.Total + "' color='" + ColorCodes[j] + "' />";
                        j++;
                    }
                    CampusChart = CampusChart + " <set name='' value='0' color='008ee4' />";
                    CampusChart = CampusChart + "</graph>";
                    Response.Write(CampusChart);
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
        public ActionResult GetAcademicYearWiseStudentsCount(string Campus)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region code
                    Dictionary<string, object> criteria = new Dictionary<string, object>();

                    DateTime Currnt_Date = DateTime.Now;
                    int Currnt_Year = Currnt_Date.Year;
                    string CurrentAcademicYear = string.Empty;
                    if (Currnt_Date.Month >= 6 && Currnt_Date.Month <= 12)
                    {
                        CurrentAcademicYear = Currnt_Year + "-" + (Currnt_Year + 1);
                    }
                    if (Currnt_Date.Month >= 1 && Currnt_Date.Month <= 4)
                    {
                        CurrentAcademicYear = (Currnt_Year - 1) + "-" + Currnt_Year;
                    }

                    //string PastAcademicYear = string.Empty;
                    string FutureAcademicYear = string.Empty;
                    long CurrentAcademicYear1 = 0;
                    long CurrentAcademicYear2 = 0;
                    bool Val = CurrentAcademicYear.Contains("-");
                    if (Val == true)
                    {
                        string[] strArray = CurrentAcademicYear.Split('-');
                        CurrentAcademicYear1 = Convert.ToInt64(strArray[0]);
                        CurrentAcademicYear2 = Convert.ToInt64(strArray[1]);
                    }
                    //PastAcademicYear = (CurrentAcademicYear1 - 1).ToString() + "-" + (CurrentAcademicYear2 - 1).ToString();
                    FutureAcademicYear = (CurrentAcademicYear1 + 1).ToString() + "-" + (CurrentAcademicYear2 + 1).ToString();

                    string[] AcademicYears = { CurrentAcademicYear, FutureAcademicYear };



                    AdminTemplateService ATS = new AdminTemplateService();
                    criteria.Clear();
                    criteria.Add("AcademicYear", AcademicYears);
                    if (Campus == "") { Campus = "All"; }
                    if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                    Dictionary<long, IList<AdminTemplateStudentsAcademicYearWiseCount_vw>> AcademicYearWiseDetails = null;
                    AcademicYearWiseDetails = ATS.GetAdminTemplateStudentsAcademicYearWiseCount_vwListWithPagingAndCriteria(null, 999999, string.Empty, string.Empty, criteria);
                    var AcademicYear = "<graph caption='Academic Year Wise Students Count for " + Campus + " Campus' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='0'>";
                    if (AcademicYearWiseDetails != null && AcademicYearWiseDetails.FirstOrDefault().Key > 0 && (AcademicYearWiseDetails.FirstOrDefault().Value[0].AcademicYear == CurrentAcademicYear || AcademicYearWiseDetails.FirstOrDefault().Value[0].AcademicYear == FutureAcademicYear))
                    {
                        AcademicYear = AcademicYear + " <set name='" + AcademicYearWiseDetails.FirstOrDefault().Value[0].AcademicYear + "' value='" + AcademicYearWiseDetails.FirstOrDefault().Value[0].Total + "' color='BDB76B' />";
                    }
                    if (AcademicYearWiseDetails != null && AcademicYearWiseDetails.FirstOrDefault().Key > 1 && (AcademicYearWiseDetails.FirstOrDefault().Value[1].AcademicYear == FutureAcademicYear || AcademicYearWiseDetails.FirstOrDefault().Value[1].AcademicYear == CurrentAcademicYear))
                    {
                        AcademicYear = AcademicYear + " <set name='" + AcademicYearWiseDetails.FirstOrDefault().Value[1].AcademicYear + "' value='" + AcademicYearWiseDetails.FirstOrDefault().Value[1].Total + "' color='DEB887' />";
                    }
                    AcademicYear = AcademicYear + "</graph>";
                    Response.Write(AcademicYear);
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
        public ActionResult GetStudentsChartByCampusForAllGrade(string Campus)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region Code
                    AdminTemplateService ATS = new AdminTemplateService();
                    StudentsReportService SRS = new StudentsReportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
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
                    if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                    Dictionary<long, IList<MISReport_vw>> MISReport_vwDetails = null;
                    string[] ColorCodes = { "0000FF", "8A2BE2", "A52A2A", "DEB887", "5F9EA0", "BDB76B", "8B008B", "556B2F", "FF8C00", "FF00FF", "191970", "CD853F", "FF0000", "708090", "008080", "9ACD32" };
                    IList<MISReport_vw> CampusWiseStudentDetails = new List<MISReport_vw>();
                    MISReport_vwDetails = SRS.GetMISReport_vwListWithPaging(0, 999999, string.Empty, string.Empty, criteria);
                    if (MISReport_vwDetails != null && MISReport_vwDetails.Count > 0 && MISReport_vwDetails.FirstOrDefault().Value.Count > 0)
                    {
                        int TotalCount = Convert.ToInt32(MISReport_vwDetails.FirstOrDefault().Key);
                        string Grade = string.Empty;
                        for (int i = 0; i < TotalCount; i++)
                        {
                            MISReport_vw Obj = new MISReport_vw();
                            if (!string.IsNullOrEmpty(MISReport_vwDetails.FirstOrDefault().Value[i].Grade)) { Obj.Grade = MISReport_vwDetails.FirstOrDefault().Value[i].Grade; }
                            if (MISReport_vwDetails.FirstOrDefault().Value[i].Boys >= 0) { Obj.Boys = MISReport_vwDetails.FirstOrDefault().Value[i].Boys; }
                            if (MISReport_vwDetails.FirstOrDefault().Value[i].Girls >= 0) { Obj.Girls = MISReport_vwDetails.FirstOrDefault().Value[i].Girls; }
                            if (MISReport_vwDetails.FirstOrDefault().Value[i].Total >= 0) { Obj.Total = MISReport_vwDetails.FirstOrDefault().Value[i].Total; }
                            if (Obj != null) { CampusWiseStudentDetails.Add(Obj); }
                        }
                    }
                    var CampusChart = "<graph caption=' " + Campus + "-Students Counts Report in " + AcademicYear + " Academic Year' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='0' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";

                    CampusChart = CampusChart + "<categories>";
                    foreach (var GradeItem in CampusWiseStudentDetails)
                    {
                        CampusChart = CampusChart + "<category name='" + GradeItem.Grade + "' />";
                    }
                    CampusChart = CampusChart + "</categories>";

                    CampusChart = CampusChart + " <dataset seriesname='Boys' color='050BB8'>";
                    foreach (var BoysItem in CampusWiseStudentDetails)
                    {
                        CampusChart = CampusChart + "<set value='" + BoysItem.Boys + "' />";
                    }
                    CampusChart = CampusChart + "</dataset>";


                    CampusChart = CampusChart + " <dataset seriesname='Girls' color='8BBA00'>";
                    foreach (var GirlsItem in CampusWiseStudentDetails)
                    {
                        CampusChart = CampusChart + "<set value='" + GirlsItem.Girls + "' />";
                    }
                    CampusChart = CampusChart + "</dataset>";


                    CampusChart = CampusChart + " <dataset seriesname='Total' color='B51111' parentyaxis='S' renderas='Line'>";
                    foreach (var TotalItem in CampusWiseStudentDetails)
                    {
                        CampusChart = CampusChart + "<set value='" + TotalItem.Total + "' />";
                    }
                    CampusChart = CampusChart + "</dataset>";
                    CampusChart = CampusChart + "</graph>";
                    Response.Write(CampusChart);
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

        public ActionResult GetStudentsChartByCampusAndGrade(string Campus, string Grade)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region Code
                    AdminTemplateService ATS = new AdminTemplateService();
                    StudentsReportService SRS = new StudentsReportService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
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
                    if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                    if (!string.IsNullOrEmpty(Grade)) { criteria.Add("Grade", Grade); }
                    Dictionary<long, IList<AdminTemplateStudCountGrpByCampusGradeSection_vw>> AdminTemplateStudCountGrpByCampusGradeSection_vwDetails = null;
                    string[] ColorCodes = { "0000FF", "8A2BE2", "A52A2A", "DEB887", "5F9EA0", "BDB76B", "8B008B", "556B2F", "FF8C00", "FF00FF", "191970", "CD853F", "FF0000", "708090", "008080", "9ACD32" };
                    IList<AdminTemplateStudCountGrpByCampusGradeSection_vw> CampusAndGradeWiseStudentDetails = new List<AdminTemplateStudCountGrpByCampusGradeSection_vw>();
                    AdminTemplateStudCountGrpByCampusGradeSection_vwDetails = ATS.GetAdminTemplateStudCountGrpByCampusGradeSection_vwListWithPagingAndCriteria(0, 999999, string.Empty, string.Empty, criteria);
                    if (AdminTemplateStudCountGrpByCampusGradeSection_vwDetails != null && AdminTemplateStudCountGrpByCampusGradeSection_vwDetails.Count > 0 && AdminTemplateStudCountGrpByCampusGradeSection_vwDetails.FirstOrDefault().Value.Count > 0)
                    {
                        int TotalCount = Convert.ToInt32(AdminTemplateStudCountGrpByCampusGradeSection_vwDetails.FirstOrDefault().Key);
                        string Section = string.Empty;
                        for (int i = 0; i < TotalCount; i++)
                        {
                            AdminTemplateStudCountGrpByCampusGradeSection_vw Obj = new AdminTemplateStudCountGrpByCampusGradeSection_vw();
                            if (!string.IsNullOrEmpty(AdminTemplateStudCountGrpByCampusGradeSection_vwDetails.FirstOrDefault().Value[i].Section)) { Obj.Section = AdminTemplateStudCountGrpByCampusGradeSection_vwDetails.FirstOrDefault().Value[i].Section; }
                            if (AdminTemplateStudCountGrpByCampusGradeSection_vwDetails.FirstOrDefault().Value[i].Boys >= 0) { Obj.Boys = AdminTemplateStudCountGrpByCampusGradeSection_vwDetails.FirstOrDefault().Value[i].Boys; }
                            if (AdminTemplateStudCountGrpByCampusGradeSection_vwDetails.FirstOrDefault().Value[i].Girls >= 0) { Obj.Girls = AdminTemplateStudCountGrpByCampusGradeSection_vwDetails.FirstOrDefault().Value[i].Girls; }
                            if (AdminTemplateStudCountGrpByCampusGradeSection_vwDetails.FirstOrDefault().Value[i].Total >= 0) { Obj.Total = AdminTemplateStudCountGrpByCampusGradeSection_vwDetails.FirstOrDefault().Value[i].Total; }
                            if (Obj != null) { CampusAndGradeWiseStudentDetails.Add(Obj); }
                        }
                    }
                    var CampusChart = "<graph caption=' " + Campus + " -" + Grade + " Students Counts Report in " + AcademicYear + " Academic Year' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='0' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";

                    CampusChart = CampusChart + "<categories>";
                    foreach (var SectionItem in CampusAndGradeWiseStudentDetails)
                    {
                        CampusChart = CampusChart + "<category name='" + SectionItem.Section + "' />";
                    }
                    CampusChart = CampusChart + "</categories>";

                    CampusChart = CampusChart + " <dataset seriesname='Boys' color='4E3F63'>";
                    foreach (var BoysItem in CampusAndGradeWiseStudentDetails)
                    {
                        CampusChart = CampusChart + "<set value='" + BoysItem.Boys + "' />";
                    }
                    CampusChart = CampusChart + "</dataset>";


                    CampusChart = CampusChart + " <dataset seriesname='Girls' color='007B7D'>";
                    foreach (var GirlsItem in CampusAndGradeWiseStudentDetails)
                    {
                        CampusChart = CampusChart + "<set value='" + GirlsItem.Girls + "' />";
                    }
                    CampusChart = CampusChart + "</dataset>";


                    CampusChart = CampusChart + " <dataset seriesname='Total' color='7D6A00' parentyaxis='S' renderas='Line'>";
                    foreach (var TotalItem in CampusAndGradeWiseStudentDetails)
                    {
                        CampusChart = CampusChart + "<set value='" + TotalItem.Total + "' />";
                    }
                    CampusChart = CampusChart + "</dataset>";
                    CampusChart = CampusChart + "</graph>";
                    Response.Write(CampusChart);
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

        #region Call Management
        public ActionResult CallManagement()
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
                    var DepartmentChart = "<graph caption='' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";
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
        public ActionResult CallmanagementIssueCountbyIssueStatusChart()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region Code
                    CallManagementService cms = new CallManagementService();
                    Dictionary<string, object> Criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<CallManagementChart>> GetIssueCountByStatus = cms.GetCallManagementListWithPagingChart(null, 9999, string.Empty, string.Empty, Criteria);
                    var IssueCount = (from u in GetIssueCountByStatus.First().Value
                                      where u.IsInformation == false
                                      select u).ToList();
                    long LogIssCnt = 0;
                    long ResRejIssCnt = 0;
                    long log_ResRejIssCnt = 0;
                    long ResIssCnt = 0;
                    long AppRejCnt = 0;
                    long res_apprejcnt = 0;
                    long AppRejIssCnt = 0;
                    long AppIssCnt = 0;
                    long AppIssTotCnt = 0;
                    long TobeCmpltdcnt = 0;
                    long CmpltdCnt = 0;
                    long totalIssueCnt = 0;
                    long totalIssue = 0;
                    foreach (var itemdata in IssueCount)
                    {
                        if (itemdata.Status == "LogIssue")
                        {
                            LogIssCnt++;
                        }
                        else if (itemdata.Status == "ResolveIssueRejection")
                        {
                            ResRejIssCnt++;
                        }
                        else if (itemdata.Status == "ResolveIssue")
                        {
                            ResIssCnt++;
                        }
                        else if (itemdata.Status == "ApproveIssueRejection")
                        {
                            AppRejCnt++;
                        }
                        else if (itemdata.Status == "ApproveIssue")
                        {
                            AppIssCnt++;
                        }
                        else if (itemdata.Status == "Complete")
                        {
                            TobeCmpltdcnt++;
                        }

                        else if (itemdata.Status == "Completed")
                        {
                            CmpltdCnt++;
                        }
                        else
                        { }
                    }
                    totalIssue = LogIssCnt + ResRejIssCnt + ResIssCnt + AppRejCnt + AppIssCnt + TobeCmpltdcnt + CmpltdCnt;
                    log_ResRejIssCnt = LogIssCnt + ResRejIssCnt;
                    res_apprejcnt = ResIssCnt + AppRejCnt;
                    AppIssTotCnt = AppIssCnt + TobeCmpltdcnt;

                    var PieChart = "<graph caption='' showpercentvalues='0' showNames='0' showValues='0' decimalPrecision='0' decimals='1' basefontsize='11' issliced='1' slicingDistance='10' funnelBaseWidth='25' fillAlpha='50' showborder='1' showhovercap='1' hoverCapBgColor='ffffff' useSameSlantAngle='1' isHollow='0' nameDistance='50'>";
                    PieChart = PieChart + " <set name='Log Issue' value='" + log_ResRejIssCnt + "' color='008ee4' />";
                    PieChart = PieChart + " <set name='Resolve Issue' value='" + res_apprejcnt + "' color='f8bd19' />";
                    PieChart = PieChart + " <set name='Approve Issue' value='" + AppIssTotCnt + "' color='6baa01' />";
                    PieChart = PieChart + " <set name='Completed' value='" + CmpltdCnt + "' color='e44a00' />";
                    PieChart = PieChart + "</graph>";
                    Response.Write(PieChart);

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
        #region Assessment
        public ActionResult Assessment()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region code
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    MastersService ms = new MastersService();
                    Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    ViewBag.gradeddl1 = GradeMaster.First().Value;
                    return View();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }

        }
        public ActionResult GetAssessmentRankChart(string Campus, string Grade)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region Code
                    Assess360Service AS = new Assess360Service();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    string sidx = "GetMarks";
                    string sord = "Asc";

                    long FirstRankCount = 0;
                    long SecondRankCount = 0;
                    long ThirdRankCount = 0;

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
                    if (Campus == "") { Campus = "All"; }
                    if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                    if (Grade == "") { Grade = "All"; }
                    if (!string.IsNullOrEmpty(Grade)) { criteria.Add("Grade", Grade); }
                    string[] Ranks = { "1", "2", "3" };
                    criteria.Add("Rank", Ranks);
                    AdminTemplateService ATS = new AdminTemplateService();
                    Dictionary<long, IList<AdminTemplateAssess360GrpByCampusAndGrade_vw>> AdminTemplateAssess360GrpByCampusAndGrade_vwDetails = null;
                    IList<AdminTemplateAssess360GrpByCampusAndGrade_vw> AdminTemplateAssess360GrpByCampusAndGrade_vwList = new List<AdminTemplateAssess360GrpByCampusAndGrade_vw>();
                    AdminTemplateAssess360GrpByCampusAndGrade_vwDetails = ATS.GetAdminTemplateAssess360GrpByCampusAndGrade_vwListWithPagingAndCriteria(0, 9999, sidx, sord, criteria);
                    if (AdminTemplateAssess360GrpByCampusAndGrade_vwDetails != null && AdminTemplateAssess360GrpByCampusAndGrade_vwDetails.Count > 0 && AdminTemplateAssess360GrpByCampusAndGrade_vwDetails.FirstOrDefault().Value.Count > 0)
                    {
                        AdminTemplateAssess360GrpByCampusAndGrade_vw AssessObj = new AdminTemplateAssess360GrpByCampusAndGrade_vw();
                        var FirstRankList = (from u in AdminTemplateAssess360GrpByCampusAndGrade_vwDetails.FirstOrDefault().Value
                                             where u.Rank == 1
                                             select u).ToList();
                        if (FirstRankList.Count > 0) { FirstRankCount = FirstRankList.Count; }
                        var SecondRankList = (from u in AdminTemplateAssess360GrpByCampusAndGrade_vwDetails.FirstOrDefault().Value
                                              where u.Rank == 2
                                              select u).ToList();
                        if (SecondRankList.Count > 0) { SecondRankCount = SecondRankList.Count; }
                        var ThirdRankList = (from u in AdminTemplateAssess360GrpByCampusAndGrade_vwDetails.FirstOrDefault().Value
                                             where u.Rank == 3
                                             select u).ToList();
                        if (ThirdRankList.Count > 0) { ThirdRankCount = ThirdRankList.Count; }
                    }
                    long Empty = 0;
                    var CampusChart = "<graph caption='Rank wise students count Chart' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='0'>";
                    if (FirstRankCount == 0 && SecondRankCount == 0 && ThirdRankCount == 0)
                    {
                        CampusChart = CampusChart + " <set name='' value='' color='008ee4' />";
                        CampusChart = CampusChart + " <set name='First Rank' value='' color='8A2BE2' />";
                        CampusChart = CampusChart + " <set name='Second Rank' value='' color='6baa01' />";
                        CampusChart = CampusChart + " <set name='Third Rank' value='' color='f8bd19' />";
                        CampusChart = CampusChart + " <set name='' value='' color='008ee4' />";
                    }
                    else
                    {
                        CampusChart = CampusChart + " <set name='' value='" + Empty + "' color='008ee4' />";
                        CampusChart = CampusChart + " <set name='First Rank' value='" + FirstRankCount + "' color='8A2BE2' />";
                        CampusChart = CampusChart + " <set name='Second Rank' value='" + SecondRankCount + "' color='6baa01' />";
                        CampusChart = CampusChart + " <set name='Third Rank' value='" + ThirdRankCount + "' color='f8bd19' />";
                        CampusChart = CampusChart + " <set name='' value='" + Empty + "' color='008ee4' />";
                    }
                    CampusChart = CampusChart + "</graph>";
                    Response.Write(CampusChart);
                    #endregion
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult GetAssessmentRankList(string Campus, string Grade)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region code
                    Assess360Service AS = new Assess360Service();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    string sidx = "GetMarks";
                    string sord = "Asc";

                    //long FirstRankCount = 0;
                    //long SecondRankCount = 0;
                    //long ThirdRankCount = 0;
                    //long Below75Count = 0;
                    //long Below75And90Count = 0;
                    //long Above90Count = 0;
                    //decimal firstMark = 0;
                    //decimal secondMark = 0;
                    //decimal thirdMark = 0;
                    string FirstRankMark = string.Empty;
                    string SecondRankMark = string.Empty;
                    string ThirdRankMark = string.Empty;
                    string RankName = string.Empty;

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
                    if (Campus == "") { Campus = "All"; }
                    if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                    if (Grade == "") { Grade = "All"; }
                    if (!string.IsNullOrEmpty(Grade)) { criteria.Add("Grade", Grade); }
                    string[] Ranks = { "1", "2", "3" };
                    criteria.Add("Rank", Ranks);


                    //Dictionary<long, IList<Assess360AdminTemplate_vw>> Assess360AdminTemplate_vwDetails = null;
                    //IList<Assess360AdminTemplate_vw> Assess360AdminTemplate_vwList = new List<Assess360AdminTemplate_vw>();
                    //Assess360AdminTemplate_vwDetails = AS.GetAssess360AdminTemplate_vwListWithPagingAndCriteria(null, 9999, sidx, sord, criteria);
                    //if (Assess360AdminTemplate_vwDetails != null && Assess360AdminTemplate_vwDetails.Count > 0 && Assess360AdminTemplate_vwDetails.FirstOrDefault().Value.Count > 0)
                    //{
                    //    var Asses360GetMarkList = (from u in Assess360AdminTemplate_vwDetails.FirstOrDefault().Value
                    //                               select u.GetMarks).ToList().Distinct().ToArray();
                    //    int count = Asses360GetMarkList.Length;

                    //    if (Asses360GetMarkList != null)
                    //    {
                    //        for (int i = 1; i <= 3; i++)
                    //        {
                    //            Assess360AdminTemplate_vw AssessObj = new Assess360AdminTemplate_vw();
                    //            var RankList = (from u in Assess360AdminTemplate_vwDetails.FirstOrDefault().Value
                    //                            where u.GetMarks == Convert.ToDecimal(Asses360GetMarkList[count - i])
                    //                            select u).ToList();
                    //            if (i == 1) { FirstRankCount = RankList.Count; firstMark = RankList.FirstOrDefault().GetMarks; AssessObj.Mark = firstMark; AssessObj.RankName = "First Rank"; }
                    //            if (i == 2) { SecondRankCount = RankList.Count; secondMark = RankList.FirstOrDefault().GetMarks; AssessObj.Mark = secondMark; AssessObj.RankName = "Second Rank"; }
                    //            if (i == 3) { ThirdRankCount = RankList.Count; thirdMark = RankList.FirstOrDefault().GetMarks; AssessObj.Mark = thirdMark; AssessObj.RankName = "Third Rank"; }
                    //            Assess360AdminTemplate_vwList.Add(AssessObj);
                    //        }
                    //    }
                    //    return Json(Assess360AdminTemplate_vwList, JsonRequestBehavior.AllowGet);
                    //}
                    AdminTemplateService ATS = new AdminTemplateService();
                    Dictionary<long, IList<AdminTemplateAssess360GrpByCampusAndGrade_vw>> AdminTemplateAssess360GrpByCampusAndGrade_vwDetails = null;
                    IList<AdminTemplateAssess360GrpByCampusAndGrade_vw> AdminTemplateAssess360GrpByCampusAndGrade_vwList = new List<AdminTemplateAssess360GrpByCampusAndGrade_vw>();
                    AdminTemplateAssess360GrpByCampusAndGrade_vwDetails = ATS.GetAdminTemplateAssess360GrpByCampusAndGrade_vwListWithPagingAndCriteria(0, 9999, sidx, sord, criteria);
                    if (AdminTemplateAssess360GrpByCampusAndGrade_vwDetails != null && AdminTemplateAssess360GrpByCampusAndGrade_vwDetails.Count > 0 && AdminTemplateAssess360GrpByCampusAndGrade_vwDetails.FirstOrDefault().Value.Count > 0)
                    {
                        decimal Empty = 0;
                        AdminTemplateAssess360GrpByCampusAndGrade_vw AssessObj = new AdminTemplateAssess360GrpByCampusAndGrade_vw();
                        var FirstRankList = (from u in AdminTemplateAssess360GrpByCampusAndGrade_vwDetails.FirstOrDefault().Value
                                             where u.Rank == 1
                                             select u).ToList();
                        if (FirstRankList.Count > 0) { AssessObj.Mark = FirstRankList[0].GetMarks.ToString(); AssessObj.RankName = "First Rank"; AdminTemplateAssess360GrpByCampusAndGrade_vwList.Add(AssessObj); }
                        //else { AssessObj.Mark = "0"; AssessObj.RankName = "First Rank"; AdminTemplateAssess360GrpByCampusAndGrade_vwList.Add(AssessObj); }
                        var SecondRankList = (from u in AdminTemplateAssess360GrpByCampusAndGrade_vwDetails.FirstOrDefault().Value
                                              where u.Rank == 2
                                              select u).ToList();
                        if (SecondRankList.Count > 0) { AssessObj.Mark = SecondRankList[0].GetMarks.ToString(); AssessObj.RankName = "Second Rank"; AdminTemplateAssess360GrpByCampusAndGrade_vwList.Add(AssessObj); }
                        //else { AssessObj = null; AssessObj.Mark = Empty.ToString(); AssessObj.RankName = "Second Rank"; AdminTemplateAssess360GrpByCampusAndGrade_vwList.Add(AssessObj); }
                        var ThirdRankList = (from u in AdminTemplateAssess360GrpByCampusAndGrade_vwDetails.FirstOrDefault().Value
                                             where u.Rank == 3
                                             select u).ToList();
                        if (ThirdRankList.Count > 0) { AssessObj.Mark = ThirdRankList[0].GetMarks.ToString(); AssessObj.RankName = "Third Rank"; AdminTemplateAssess360GrpByCampusAndGrade_vwList.Add(AssessObj); }
                        //else { AssessObj = null; AssessObj.Mark = Empty.ToString(); AssessObj.RankName = "Third Rank"; AdminTemplateAssess360GrpByCampusAndGrade_vwList.Add(AssessObj); }
                        return Json(AdminTemplateAssess360GrpByCampusAndGrade_vwList, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult GetAssessmentCountChart()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region Code
                    Assess360Service AS = new Assess360Service();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    string sidx = "GetMarks";
                    string sord = "Asc";
                    Dictionary<long, IList<Assess360AdminTemplate_vw>> Assess360AdminTemplate_vwDetails = null;
                    IList<Assess360AdminTemplate_vw> Assess360AdminTemplate_vwList = new List<Assess360AdminTemplate_vw>();
                    long Below75Count = 0;
                    long Below75And90Count = 0;
                    long Above90Count = 0;
                    Assess360AdminTemplate_vwDetails = AS.GetAssess360AdminTemplate_vwListWithPagingAndCriteria(null, 9999, sidx, sord, criteria);
                    if (Assess360AdminTemplate_vwDetails != null && Assess360AdminTemplate_vwDetails.Count > 0 && Assess360AdminTemplate_vwDetails.FirstOrDefault().Value.Count > 0)
                    {
                        var Below75List = (from u in Assess360AdminTemplate_vwDetails.FirstOrDefault().Value
                                           where u.GetMarks < 75
                                           select u).ToList();
                        Below75Count = Below75List.Count;

                        var Below75And90List = (from u in Assess360AdminTemplate_vwDetails.FirstOrDefault().Value
                                                where u.GetMarks < 75 && u.GetMarks > 90
                                                select u).ToList();
                        Below75And90Count = Below75And90List.Count;

                        var Above90List = (from u in Assess360AdminTemplate_vwDetails.FirstOrDefault().Value
                                           where u.GetMarks > 90
                                           select u).ToList();
                        Above90Count = Above90List.Count;
                    }
                    long Empty = 0;
                    var CountChart = "<graph caption='Total Mark wise students count chart' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='0'>";
                    CountChart = CountChart + " <set name='' value='" + Empty + "' color='008ee4' />";
                    CountChart = CountChart + " <set name='Below 75' value='" + Below75Count + "' color='8A2BE2' />";
                    CountChart = CountChart + " <set name='Above 75And Below 90' value='" + Below75And90Count + "' color='6baa01' />";
                    CountChart = CountChart + " <set name='Above 90' value='" + Above90Count + "' color='f8bd19' />";
                    CountChart = CountChart + " <set name='' value='" + Empty + "' color='008ee4' />";
                    CountChart = CountChart + "</graph>";
                    Response.Write(CountChart);
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
        public ActionResult GetAssessmentRanksCount(string Campus, string Grade)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    DateTime Currnt_Date = DateTime.Now;
                    int Currnt_Year = Currnt_Date.Year;
                    string CurrentAcademicYear = string.Empty;
                    if (Currnt_Date.Month >= 6 && Currnt_Date.Month <= 12)
                    {
                        CurrentAcademicYear = Currnt_Year + "-" + (Currnt_Year + 1);
                    }
                    if (Currnt_Date.Month >= 1 && Currnt_Date.Month <= 4)
                    {
                        CurrentAcademicYear = (Currnt_Year - 1) + "-" + Currnt_Year;
                    }
                    long Below75Count = 0;
                    long MeritListCount = 0;
                    long HightAcieversClubCount = 0;
                    long ChairmanAwardCount = 0;
                    if (Campus == "") { Campus = "All"; }
                    if (Grade == "") { Grade = "All"; }
                    criteria.Clear();
                    if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                    if (!string.IsNullOrEmpty(Grade)) { criteria.Add("Grade", Grade); }
                    criteria.Add("AcademicYear", CurrentAcademicYear);
                    AdminTemplateService ATS = new AdminTemplateService();
                    Dictionary<long, IList<AdminTemplateAssess360MarkCount_vw>> AdminTemplateAssess360MarkCount_vwDetails = null;
                    AdminTemplateAssess360MarkCount_vwDetails = ATS.GetAdminTemplateAssess360MarkCount_vwListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    if (AdminTemplateAssess360MarkCount_vwDetails != null && AdminTemplateAssess360MarkCount_vwDetails.Count > 0 && AdminTemplateAssess360MarkCount_vwDetails.FirstOrDefault().Value.Count > 0)
                    {
                        Below75Count = Convert.ToInt64(AdminTemplateAssess360MarkCount_vwDetails.FirstOrDefault().Value[0].Below75);
                        MeritListCount = Convert.ToInt64(AdminTemplateAssess360MarkCount_vwDetails.FirstOrDefault().Value[0].MeritList);
                        HightAcieversClubCount = Convert.ToInt64(AdminTemplateAssess360MarkCount_vwDetails.FirstOrDefault().Value[0].HighAcheiversClub);
                        ChairmanAwardCount = Convert.ToInt64(AdminTemplateAssess360MarkCount_vwDetails.FirstOrDefault().Value[0].ChairmanAward);
                    }
                    return Json(new { Below75Count, MeritListCount, HightAcieversClubCount, ChairmanAwardCount }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        #endregion
        #region Email
        public ActionResult Email()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region Code
                    AdminTemplateService ATS = new AdminTemplateService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    long IBKGCount = 0;
                    long IBMAINCount = 0;
                    long KARURKGCount = 0;
                    long KARURCount = 0;
                    long TIRUPURKGCount = 0;
                    long TIRUPURCount = 0;
                    long ERNAKULAMKGCount = 0;
                    long ERNAKULAMCount = 0;
                    long CHENNAICITYCount = 0;
                    long CHENNAIMAINCount = 0;
                    long TIPSERODECount = 0;
                    long TIPSSALEMCount = 0;
                    long TIPSSARANCount = 0;
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
                    Dictionary<long, IList<AdminTemplateEmailReport_vw>> EmailDetails = null;
                    IList<AdminTemplateEmailReport_vw> EmailDetailsList = new List<AdminTemplateEmailReport_vw>();
                    EmailDetails = ATS.GetAdminTemplateEmailReport_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    if (EmailDetails != null && EmailDetails.Count > 0 && EmailDetails.FirstOrDefault().Value.Count > 0)
                    {
                        var IBKGList = (from u in EmailDetails.FirstOrDefault().Value
                                        where u.Campus == "IB KG"
                                        select u).ToList();
                        if (IBKGList.Count > 0)
                            IBKGCount = IBKGList.FirstOrDefault().Count;

                        var IBMAINList = (from u in EmailDetails.FirstOrDefault().Value
                                          where u.Campus == "IB MAIN"
                                          select u).ToList();
                        if (IBMAINList.Count > 0)
                            IBMAINCount = IBMAINList.FirstOrDefault().Count;

                        var KARURKGList = (from u in EmailDetails.FirstOrDefault().Value
                                           where u.Campus == "KARUR KG"
                                           select u).ToList();
                        if (KARURKGList.Count > 0)
                            KARURKGCount = KARURKGList.FirstOrDefault().Count;

                        var KARURList = (from u in EmailDetails.FirstOrDefault().Value
                                         where u.Campus == "KARUR"
                                         select u).ToList();
                        if (KARURList.Count > 0)
                            KARURCount = KARURList.FirstOrDefault().Count;

                        var TIRUPURKGList = (from u in EmailDetails.FirstOrDefault().Value
                                             where u.Campus == "TIRUPUR KG"
                                             select u).ToList();
                        if (TIRUPURKGList.Count > 0)
                            TIRUPURKGCount = TIRUPURKGList.FirstOrDefault().Count;

                        var TIRUPURList = (from u in EmailDetails.FirstOrDefault().Value
                                           where u.Campus == "TIRUPUR"
                                           select u).ToList();
                        if (TIRUPURList.Count > 0)
                            TIRUPURCount = TIRUPURList.FirstOrDefault().Count;

                        var ERNAKULAMKGList = (from u in EmailDetails.FirstOrDefault().Value
                                               where u.Campus == "ERNAKULAM KG"
                                               select u).ToList();
                        if (ERNAKULAMKGList.Count > 0)
                            ERNAKULAMKGCount = ERNAKULAMKGList.FirstOrDefault().Count;

                        var ERNAKULAMList = (from u in EmailDetails.FirstOrDefault().Value
                                             where u.Campus == "ERNAKULAM"
                                             select u).ToList();
                        if (ERNAKULAMList.Count > 0)
                            ERNAKULAMCount = ERNAKULAMList.FirstOrDefault().Count;

                        var CHENNAICITYList = (from u in EmailDetails.FirstOrDefault().Value
                                               where u.Campus == "CHENNAI CITY"
                                               select u).ToList();
                        if (CHENNAICITYList.Count > 0)
                            CHENNAICITYCount = CHENNAICITYList.FirstOrDefault().Count;

                        var CHENNAIMAINList = (from u in EmailDetails.FirstOrDefault().Value
                                               where u.Campus == "CHENNAI MAIN"
                                               select u).ToList();
                        if (CHENNAIMAINList.Count > 0)
                            CHENNAIMAINCount = CHENNAIMAINList.FirstOrDefault().Count;

                        var TIPSERODEList = (from u in EmailDetails.FirstOrDefault().Value
                                             where u.Campus == "TIPS ERODE"
                                             select u).ToList();
                        if (TIPSERODEList.Count > 0)
                            TIPSERODECount = TIPSERODEList.FirstOrDefault().Count;

                        var TIPSSALEMList = (from u in EmailDetails.FirstOrDefault().Value
                                             where u.Campus == "TIPS SALEM"
                                             select u).ToList();
                        if (TIPSSALEMList.Count > 0)
                            TIPSSALEMCount = TIPSSALEMList.FirstOrDefault().Count;

                        var TIPSSARANList = (from u in EmailDetails.FirstOrDefault().Value
                                             where u.Campus == "TIPS SARAN"
                                             select u).ToList();
                        if (TIPSSARANList.Count > 0)
                            TIPSSARANCount = TIPSSARANList.FirstOrDefault().Count;
                    }
                    @ViewBag.IBKGCount = IBKGCount;
                    @ViewBag.IBMAINCount = IBMAINCount;
                    @ViewBag.KARURKGCount = KARURKGCount;
                    @ViewBag.KARURCount = KARURCount;
                    @ViewBag.TIRUPURKGCount = TIRUPURKGCount;
                    @ViewBag.TIRUPURCount = TIRUPURCount;
                    @ViewBag.ERNAKULAMKGCount = ERNAKULAMKGCount;
                    @ViewBag.ERNAKULAMCount = ERNAKULAMCount;
                    @ViewBag.CHENNAICITYCount = CHENNAICITYCount;
                    @ViewBag.CHENNAIMAINCount = CHENNAIMAINCount;
                    @ViewBag.TIPSERODECount = TIPSERODECount;
                    @ViewBag.TIPSSALEMCount = TIPSSALEMCount;
                    @ViewBag.TIPSSARANCount = TIPSSARANCount;
                    @ViewBag.AcademicYear = AcademicYear;
                    return View();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }

        }
        public ActionResult CampusWiseEmailCountChart()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region code
                    AdminTemplateService ATS = new AdminTemplateService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    string[] ColorCodes = { "0000FF", "8A2BE2", "A52A2A", "DEB887", "5F9EA0", "BDB76B", "8B008B", "556B2F", "FF8C00", "FF00FF", "191970", "CD853F", "FF0000", "708090", "008080", "9ACD32" };
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
                    Dictionary<long, IList<AdminTemplateEmailReport_vw>> EmailDetails = null;
                    IList<AdminTemplateEmailReport_vw> EmailDetailsList = new List<AdminTemplateEmailReport_vw>();
                    EmailDetails = ATS.GetAdminTemplateEmailReport_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    if (EmailDetails != null && EmailDetails.Count > 0 && EmailDetails.FirstOrDefault().Value.Count > 0)
                    {
                        int TotalCount = Convert.ToInt32(EmailDetails.FirstOrDefault().Key);
                        string Campus = string.Empty;
                        for (int i = 0; i < TotalCount; i++)
                        {
                            AdminTemplateEmailReport_vw Obj = new AdminTemplateEmailReport_vw();
                            if (!string.IsNullOrEmpty(EmailDetails.FirstOrDefault().Value[i].Campus)) { Obj.Campus = EmailDetails.FirstOrDefault().Value[i].Campus; }
                            if (EmailDetails.FirstOrDefault().Value[i].Count >= 0) { Obj.Count = EmailDetails.FirstOrDefault().Value[i].Count; }
                            if (Obj != null) { EmailDetailsList.Add(Obj); }
                        }
                    }
                    var CampusChart = "<graph caption='' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";
                    CampusChart = CampusChart + " <set name='' value='0' color='008ee4' />";
                    int j = 0;
                    foreach (var EmailDetailsItem in EmailDetailsList)
                    {
                        CampusChart = CampusChart + " <set name='" + EmailDetailsItem.Campus + "' value='" + EmailDetailsItem.Count + "' color='" + ColorCodes[j] + "' />";
                        j++;
                    }
                    CampusChart = CampusChart + " <set name='' value='0' color='008ee4' />";
                    CampusChart = CampusChart + "</graph>";
                    Response.Write(CampusChart);
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
        #region SMS
        public ActionResult SMS()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region code
                    AdminTemplateService ATS = new AdminTemplateService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    long IBKGCount = 0;
                    long IBMAINCount = 0;
                    long KARURKGCount = 0;
                    long KARURCount = 0;
                    long TIRUPURKGCount = 0;
                    long TIRUPURCount = 0;
                    long ERNAKULAMKGCount = 0;
                    long ERNAKULAMCount = 0;
                    long CHENNAICITYCount = 0;
                    long CHENNAIMAINCount = 0;
                    long TIPSERODECount = 0;
                    long TIPSSALEMCount = 0;
                    long TIPSSARANCount = 0;
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
                    Dictionary<long, IList<AdminTemplateSMSReport_vw>> SMSDeatails = null;
                    IList<AdminTemplateSMSReport_vw> SMSDetailsList = new List<AdminTemplateSMSReport_vw>();
                    SMSDeatails = ATS.GetAdminTemplateSMSReport_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    if (SMSDeatails != null && SMSDeatails.Count > 0 && SMSDeatails.FirstOrDefault().Value.Count > 0)
                    {
                        var IBKGList = (from u in SMSDeatails.FirstOrDefault().Value
                                        where u.Campus == "IB KG"
                                        select u).ToList();
                        if (IBKGList.Count > 0)
                            IBKGCount = IBKGList.FirstOrDefault().Count;

                        var IBMAINList = (from u in SMSDeatails.FirstOrDefault().Value
                                          where u.Campus == "IB MAIN"
                                          select u).ToList();
                        if (IBMAINList.Count > 0)
                            IBMAINCount = IBMAINList.FirstOrDefault().Count;

                        var KARURKGList = (from u in SMSDeatails.FirstOrDefault().Value
                                           where u.Campus == "KARUR KG"
                                           select u).ToList();
                        if (KARURKGList.Count > 0)
                            KARURKGCount = KARURKGList.FirstOrDefault().Count;

                        var KARURList = (from u in SMSDeatails.FirstOrDefault().Value
                                         where u.Campus == "KARUR"
                                         select u).ToList();
                        if (KARURList.Count > 0)
                            KARURCount = KARURList.FirstOrDefault().Count;

                        var TIRUPURKGList = (from u in SMSDeatails.FirstOrDefault().Value
                                             where u.Campus == "TIRUPUR KG"
                                             select u).ToList();
                        if (TIRUPURKGList.Count > 0)
                            TIRUPURKGCount = TIRUPURKGList.FirstOrDefault().Count;

                        var TIRUPURList = (from u in SMSDeatails.FirstOrDefault().Value
                                           where u.Campus == "TIRUPUR"
                                           select u).ToList();
                        if (TIRUPURList.Count > 0)
                            TIRUPURCount = TIRUPURList.FirstOrDefault().Count;

                        var ERNAKULAMKGList = (from u in SMSDeatails.FirstOrDefault().Value
                                               where u.Campus == "ERNAKULAM KG"
                                               select u).ToList();
                        if (ERNAKULAMKGList.Count > 0)
                            ERNAKULAMKGCount = ERNAKULAMKGList.FirstOrDefault().Count;

                        var ERNAKULAMList = (from u in SMSDeatails.FirstOrDefault().Value
                                             where u.Campus == "ERNAKULAM"
                                             select u).ToList();
                        if (ERNAKULAMList.Count > 0)
                            ERNAKULAMCount = ERNAKULAMList.FirstOrDefault().Count;

                        var CHENNAICITYList = (from u in SMSDeatails.FirstOrDefault().Value
                                               where u.Campus == "CHENNAI CITY"
                                               select u).ToList();
                        if (CHENNAICITYList.Count > 0)
                            CHENNAICITYCount = CHENNAICITYList.FirstOrDefault().Count;

                        var CHENNAIMAINList = (from u in SMSDeatails.FirstOrDefault().Value
                                               where u.Campus == "CHENNAI MAIN"
                                               select u).ToList();
                        if (CHENNAIMAINList.Count > 0)
                            CHENNAIMAINCount = CHENNAIMAINList.FirstOrDefault().Count;

                        var TIPSERODEList = (from u in SMSDeatails.FirstOrDefault().Value
                                             where u.Campus == "TIPS ERODE"
                                             select u).ToList();
                        if (TIPSERODEList.Count > 0)
                            TIPSERODECount = TIPSERODEList.FirstOrDefault().Count;

                        var TIPSSALEMList = (from u in SMSDeatails.FirstOrDefault().Value
                                             where u.Campus == "TIPS SALEM"
                                             select u).ToList();
                        if (TIPSSALEMList.Count > 0)
                            TIPSSALEMCount = TIPSSALEMList.FirstOrDefault().Count;

                        var TIPSSARANList = (from u in SMSDeatails.FirstOrDefault().Value
                                             where u.Campus == "TIPS SARAN"
                                             select u).ToList();
                        if (TIPSSARANList.Count > 0)
                            TIPSSARANCount = TIPSSARANList.FirstOrDefault().Count;
                    }
                    @ViewBag.IBKGCount = IBKGCount;
                    @ViewBag.IBMAINCount = IBMAINCount;
                    @ViewBag.KARURKGCount = KARURKGCount;
                    @ViewBag.KARURCount = KARURCount;
                    @ViewBag.TIRUPURKGCount = TIRUPURKGCount;
                    @ViewBag.TIRUPURCount = TIRUPURCount;
                    @ViewBag.ERNAKULAMKGCount = ERNAKULAMKGCount;
                    @ViewBag.ERNAKULAMCount = ERNAKULAMCount;
                    @ViewBag.CHENNAICITYCount = CHENNAICITYCount;
                    @ViewBag.CHENNAIMAINCount = CHENNAIMAINCount;
                    @ViewBag.TIPSERODECount = TIPSERODECount;
                    @ViewBag.TIPSSALEMCount = TIPSSALEMCount;
                    @ViewBag.TIPSSARANCount = TIPSSARANCount;
                    @ViewBag.AcademicYear = AcademicYear;
                    return View();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }

        }
        public ActionResult CampusWiseSMSCountChart()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region code
                    AdminTemplateService ATS = new AdminTemplateService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    string[] ColorCodes = { "0000FF", "8A2BE2", "A52A2A", "DEB887", "5F9EA0", "BDB76B", "8B008B", "556B2F", "FF8C00", "FF00FF", "191970", "CD853F", "FF0000", "708090", "008080", "9ACD32" };
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
                    Dictionary<long, IList<AdminTemplateSMSReport_vw>> SMSDetails = null;
                    IList<AdminTemplateSMSReport_vw> SMSDetailsList = new List<AdminTemplateSMSReport_vw>();
                    SMSDetails = ATS.GetAdminTemplateSMSReport_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    if (SMSDetails != null && SMSDetails.Count > 0 && SMSDetails.FirstOrDefault().Value.Count > 0)
                    {
                        int TotalCount = Convert.ToInt32(SMSDetails.FirstOrDefault().Key);
                        string Campus = string.Empty;
                        for (int i = 0; i < TotalCount; i++)
                        {
                            AdminTemplateSMSReport_vw Obj = new AdminTemplateSMSReport_vw();
                            if (!string.IsNullOrEmpty(SMSDetails.FirstOrDefault().Value[i].Campus)) { Obj.Campus = SMSDetails.FirstOrDefault().Value[i].Campus; }
                            if (SMSDetails.FirstOrDefault().Value[i].Count >= 0) { Obj.Count = SMSDetails.FirstOrDefault().Value[i].Count; }
                            if (Obj != null) { SMSDetailsList.Add(Obj); }
                        }
                    }
                    var CampusChart = "<graph caption='' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";
                    CampusChart = CampusChart + " <set name='' value='0' color='008ee4' />";
                    int j = 0;
                    foreach (var SMSDetailsItem in SMSDetailsList)
                    {
                        CampusChart = CampusChart + " <set name='" + SMSDetailsItem.Campus + "' value='" + SMSDetailsItem.Count + "' color='" + ColorCodes[j] + "' />";
                        j++;
                    }
                    CampusChart = CampusChart + " <set name='' value='0' color='008ee4' />";
                    CampusChart = CampusChart + "</graph>";
                    Response.Write(CampusChart);
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
        #region Store
        public ActionResult Store()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region code
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    StoreService Store = new StoreService();
                    long MaterialGroupMasterCount = 0;
                    long MaterialSubGroupMasterCount = 0;
                    long MaterialMasterCount = 0;
                    string[] ColorCodes = { "8B008B", "0e6aad", "8A2BE2", "FF4500", "f8bd19", "e44a00", "008ee4", "6baa01", "f8bd19", "BDB76B" };
                    //MaterialGroupMaster
                    Dictionary<long, IList<MaterialGroupMaster>> MaterialGroupMasterDetails = null;
                    MaterialGroupMasterDetails = Store.GetMaterialGroupMasterListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);

                    if (MaterialGroupMasterDetails != null && MaterialGroupMasterDetails.Count > 0 && MaterialGroupMasterDetails.FirstOrDefault().Value.Count > 0)
                    {
                        MaterialGroupMasterCount = MaterialGroupMasterDetails.FirstOrDefault().Key;
                    }

                    //MaterialSubGroupMasterCount
                    Dictionary<long, IList<MaterialSubGroupMaster>> MaterialSubGroupMasterDetails = null;
                    MaterialSubGroupMasterDetails = Store.GetMaterialSubGroupMasterListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    if (MaterialSubGroupMasterDetails != null && MaterialSubGroupMasterDetails.Count > 0 && MaterialSubGroupMasterDetails.FirstOrDefault().Value.Count > 0)
                    {
                        MaterialSubGroupMasterCount = MaterialSubGroupMasterDetails.FirstOrDefault().Key;
                    }

                    //MaterialMasterCount
                    Dictionary<long, IList<MaterialsMaster>> MaterialsMasterDetails = null;
                    MaterialsMasterDetails = Store.GetMaterialsMasterlistWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    if (MaterialsMasterDetails != null && MaterialsMasterDetails.Count > 0 && MaterialsMasterDetails.FirstOrDefault().Value.Count > 0)
                    {
                        MaterialMasterCount = MaterialsMasterDetails.FirstOrDefault().Key;
                    }
                    @ViewBag.MaterialGroupMasterCount = MaterialGroupMasterCount;
                    @ViewBag.MaterialSubGroupMasterCount = MaterialSubGroupMasterCount;
                    @ViewBag.MaterialMasterCount = MaterialMasterCount;
                    return View();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }

        }
        public ActionResult StoreCampusWiseRequestChart()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region code
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    StoreService Store = new StoreService();
                    Dictionary<long, IList<StoreReportForAdminTemplate_vw>> StoreReportForAdminTemplate_vwDetails = null;
                    StoreReportForAdminTemplate_vwDetails = Store.GetStoreReportForAdminTemplate_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    List<StoreReportForAdminTemplate_vw> StoreReportForAdminTemplate_vwList = new List<StoreReportForAdminTemplate_vw>();
                    List<CampusMaster> CampusList = new List<CampusMaster>();
                    string sord = "";
                    sord = sord == "desc" ? "Desc" : "Asc";
                    string sidx = "Flag";
                    StudentsReportService SRS = new StudentsReportService();
                    Dictionary<long, IList<CampusMaster>> CampusMasterList = SRS.GetCampusMasterListWithPaging(0, 9999, sidx, sord, criteria);
                    CampusList = CampusMasterList.FirstOrDefault().Value.ToList();

                    string[] ColorCodes = { "8B008B", "0e6aad", "8A2BE2", "FF4500", "f8bd19", "e44a00", "008ee4", "6baa01", "f8bd19", "BDB76B" };
                    var CampusArray = (from u in CampusMasterList.FirstOrDefault().Value
                                       select u.Name).ToArray();

                    var InwardCampus = (from u in StoreReportForAdminTemplate_vwDetails.FirstOrDefault().Value
                                        select u.RequiredForCampus).ToArray();

                    if (StoreReportForAdminTemplate_vwDetails != null && StoreReportForAdminTemplate_vwDetails.Count > 0 && StoreReportForAdminTemplate_vwDetails.FirstOrDefault().Value.Count > 0)
                    {
                        var CampusChart = "<graph caption='Campus Wise Store Request Count' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";
                        int i = 0;

                        foreach (var item in CampusList)
                        {
                            long TempCount = 0;
                            if (CampusArray.Contains(item.Name))
                            {

                                var TempList = (from u in StoreReportForAdminTemplate_vwDetails.FirstOrDefault().Value
                                                where u.RequiredForCampus == item.Name
                                                select u).ToList();
                                if (TempList.Count > 0)
                                {
                                    TempCount = TempList[0].Count;

                                    CampusChart = CampusChart + " <set name='" + TempList[0].RequiredForCampus + "' value='" + TempList[0].Count + "' color='" + ColorCodes[i] + "' />";
                                    i = i + 1;
                                }
                            }
                        }
                        CampusChart = CampusChart + "</graph>";
                        Response.Write(CampusChart);
                    }
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
        public ActionResult StoreInwardChart()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region code
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    StoreService Store = new StoreService();
                    Dictionary<long, IList<StoreInwardReportForAdminTemplate_vw>> StoreInwardReportForAdminTemplate_vwDetails = null;
                    StoreInwardReportForAdminTemplate_vwDetails = Store.GetStoreInwardReportForAdminTemplate_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    string[] ColorCodes = { "8B008B", "0e6aad", "8A2BE2", "FF4500", "f8bd19", "e44a00", "008ee4", "6baa01", "f8bd19", "BDB76B" };
                    if (StoreInwardReportForAdminTemplate_vwDetails != null && StoreInwardReportForAdminTemplate_vwDetails.Count > 0 && StoreInwardReportForAdminTemplate_vwDetails.FirstOrDefault().Value.Count > 0)
                    {
                        var CampusChart = "<graph caption='Campus Wise Store Inward Count' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";
                        int i = 0;
                        foreach (var item in StoreInwardReportForAdminTemplate_vwDetails.FirstOrDefault().Value)
                        {
                            CampusChart = CampusChart + " <set name='" + item.Store + "' value='" + item.Count + "' color='" + ColorCodes[i] + "' />";
                            i = i + 1;
                        }
                        CampusChart = CampusChart + "</graph>";
                        Response.Write(CampusChart);
                    }
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
        public ActionResult StoreMaterialsMasterChart()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region code
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    StoreService Store = new StoreService();
                    long MaterialGroupMasterCount = 0;
                    long MaterialSubGroupMasterCount = 0;
                    long MaterialMasterCount = 0;
                    string[] ColorCodes = { "8B008B", "0e6aad", "8A2BE2", "FF4500", "f8bd19", "e44a00", "008ee4", "6baa01", "f8bd19", "BDB76B" };
                    //MaterialGroupMaster
                    Dictionary<long, IList<MaterialGroupMaster>> MaterialGroupMasterDetails = null;
                    MaterialGroupMasterDetails = Store.GetMaterialGroupMasterListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);

                    if (MaterialGroupMasterDetails != null && MaterialGroupMasterDetails.Count > 0 && MaterialGroupMasterDetails.FirstOrDefault().Value.Count > 0)
                    {
                        MaterialGroupMasterCount = MaterialGroupMasterDetails.FirstOrDefault().Key;
                    }

                    //MaterialSubGroupMasterCount
                    Dictionary<long, IList<MaterialSubGroupMaster>> MaterialSubGroupMasterDetails = null;
                    MaterialSubGroupMasterDetails = Store.GetMaterialSubGroupMasterListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    if (MaterialSubGroupMasterDetails != null && MaterialSubGroupMasterDetails.Count > 0 && MaterialSubGroupMasterDetails.FirstOrDefault().Value.Count > 0)
                    {
                        MaterialSubGroupMasterCount = MaterialSubGroupMasterDetails.FirstOrDefault().Key;
                    }

                    //MaterialMasterCount
                    Dictionary<long, IList<MaterialsMaster>> MaterialsMasterDetails = null;
                    MaterialsMasterDetails = Store.GetMaterialsMasterlistWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    if (MaterialsMasterDetails != null && MaterialsMasterDetails.Count > 0 && MaterialsMasterDetails.FirstOrDefault().Value.Count > 0)
                    {
                        MaterialMasterCount = MaterialsMasterDetails.FirstOrDefault().Key;
                    }
                    var MasterChart = "<graph caption='Material Master Count' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";
                    MasterChart = MasterChart + " <set name='Group' value='" + MaterialGroupMasterCount + "' color='008ee4' issliced='1'/>";
                    MasterChart = MasterChart + " <set name='Sub Group' value='" + MaterialSubGroupMasterCount + "' color='f8bd19' issliced='1'/>";
                    MasterChart = MasterChart + " <set name='Material' value='" + MaterialMasterCount + "' color='6baa01' issliced='1'/>";
                    MasterChart = MasterChart + "</graph>";
                    Response.Write(MasterChart);
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

        #region Transport

        public ActionResult Transport()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region code
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    MastersService ms = new MastersService();
                    Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    ViewBag.gradeddl1 = GradeMaster.First().Value;
                    return View();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }

        }
        public ActionResult TransportCampusWiseCountChart()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    TransportBC samp = new TransportBC();

                    List<CampusMaster> CampusList = new List<CampusMaster>();
                    string sord = "";
                    sord = sord == "desc" ? "Desc" : "Asc";
                    string sidx = "Flag";
                    StudentsReportService SRS = new StudentsReportService();
                    Dictionary<long, IList<CampusMaster>> CampusMasterList = SRS.GetCampusMasterListWithPaging(0, 9999, sidx, sord, criteria);
                    CampusList = CampusMasterList.FirstOrDefault().Value.ToList();
                    criteria.Clear();
                    Dictionary<long, IList<DriverMaster>> DriverMasterDetails = null;
                    DriverMasterDetails = samp.GetDriverMasterDetails(null, 9999, string.Empty, string.Empty, criteria);
                    List<DriverMaster> DriverMasterList = new List<DriverMaster>();

                    var CampusArray = (from u in CampusMasterList.FirstOrDefault().Value
                                       select u.Name).ToArray();

                    var DriversCampus = (from u in DriverMasterDetails.FirstOrDefault().Value
                                         where u.Name != null && u.Campus != null
                                         select u.Campus).ToArray().Distinct();
                    var CampusChart = "<graph caption='Campus Wise Transport Report' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='0' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";
                    CampusChart = CampusChart + "<categories>";
                    foreach (var item in CampusList)
                    {
                        CampusChart = CampusChart + "<category name='" + item.Name + "' />";
                    }
                    CampusChart = CampusChart + "</categories>";

                    if (DriverMasterDetails != null && DriverMasterDetails.Count > 0 && DriverMasterDetails.FirstOrDefault().Value.Count > 0)
                    {
                        List<DriverMaster> DriverMasterData = new List<DriverMaster>();
                        foreach (var item in CampusList)
                        {
                            DriverMaster DM = new DriverMaster();
                            long TempCount = 0;
                            string Campus = string.Empty;
                            var TempList = (from u in DriverMasterDetails.FirstOrDefault().Value
                                            where u.Campus == item.Name
                                            select u).ToList();
                            if (TempList.Count == 0) { TempCount = 0; Campus = item.Name; }
                            else { TempCount = TempList.Count; Campus = TempList.FirstOrDefault().Campus; }
                            DM.Campus = Campus;
                            DM.ReportCount = TempCount;
                            DriverMasterData.Add(DM);
                        }

                        CampusChart = CampusChart + " <dataset seriesname='Driver' color='008ee4'>";
                        foreach (var Driver in DriverMasterData)
                        {
                            CampusChart = CampusChart + "<set value='" + Driver.ReportCount + "' />";
                        }
                        CampusChart = CampusChart + "</dataset>";
                    }
                    criteria.Clear();
                    Dictionary<long, IList<RouteMaster>> RouteMasterDetails = null;
                    RouteMasterDetails = samp.GetRouteMasterDetails(null, 9999, string.Empty, string.Empty, criteria);
                    List<RouteMaster> RouteMasterList = new List<RouteMaster>();
                    if (RouteMasterDetails != null && RouteMasterDetails.Count > 0 && RouteMasterDetails.FirstOrDefault().Value.Count > 0)
                    {
                        List<RouteMaster> RouteMasterData = new List<RouteMaster>();
                        foreach (var item in CampusList)
                        {
                            RouteMaster RM = new RouteMaster();
                            long TempCount = 0;
                            string Campus = string.Empty;
                            var TempList = (from u in RouteMasterDetails.FirstOrDefault().Value
                                            where u.Campus == item.Name
                                            select u).ToList();
                            if (TempList.Count == 0) { TempCount = 0; Campus = item.Name; }
                            else { TempCount = TempList.Count; Campus = TempList.FirstOrDefault().Campus; }
                            RM.Campus = Campus;
                            RM.ReportCount = TempCount;
                            RouteMasterData.Add(RM);
                        }

                        CampusChart = CampusChart + " <dataset seriesname='Route' color='8B008B'>";
                        foreach (var Route in RouteMasterData)
                        {
                            CampusChart = CampusChart + "<set value='" + Route.ReportCount + "' />";
                        }
                        CampusChart = CampusChart + "</dataset>";
                    }
                    criteria.Clear();
                    Dictionary<long, IList<LocationMaster>> LocationMasterDetails = null;
                    LocationMasterDetails = samp.GetLocationMasterDetails(null, 9999, string.Empty, string.Empty, criteria);
                    List<LocationMaster> LocationMasterList = new List<LocationMaster>();
                    if (LocationMasterDetails != null && LocationMasterDetails.Count > 0 && LocationMasterDetails.FirstOrDefault().Value.Count > 0)
                    {
                        List<LocationMaster> LocationMasterData = new List<LocationMaster>();
                        foreach (var item in CampusList)
                        {
                            LocationMaster LM = new LocationMaster();
                            long TempCount = 0;
                            string Campus = string.Empty;
                            var TempList = (from u in LocationMasterDetails.FirstOrDefault().Value
                                            where u.Campus == item.Name
                                            select u).ToList();
                            if (TempList.Count == 0) { TempCount = 0; Campus = item.Name; }
                            else { TempCount = TempList.Count; Campus = TempList.FirstOrDefault().Campus; }
                            LM.Campus = Campus;
                            LM.ReportCount = TempCount;
                            LocationMasterData.Add(LM);
                        }

                        CampusChart = CampusChart + " <dataset seriesname='Location' color='e44a00'>";
                        foreach (var Location in LocationMasterData)
                        {
                            CampusChart = CampusChart + "<set value='" + Location.ReportCount + "' />";
                        }
                        CampusChart = CampusChart + "</dataset>";
                    }

                    criteria.Clear();
                    Dictionary<long, IList<VehicleSubTypeMaster>> VehicleSubTypeMasterDetails = null;
                    VehicleSubTypeMasterDetails = samp.GetVehicleSubTypeMasterListWithPagingAndCriteriaLikeSearch(null, 9999, string.Empty, string.Empty, criteria);
                    List<VehicleSubTypeMaster> VehicleSubTypeMasterList = new List<VehicleSubTypeMaster>();
                    if (VehicleSubTypeMasterDetails != null && VehicleSubTypeMasterDetails.Count > 0 && VehicleSubTypeMasterDetails.FirstOrDefault().Value.Count > 0)
                    {
                        List<VehicleSubTypeMaster> VehicleSubTypeMasterData = new List<VehicleSubTypeMaster>();
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
                        }

                        CampusChart = CampusChart + " <dataset seriesname='Vehicles' color='f8bd19'>";
                        foreach (var VechcleSubType in VehicleSubTypeMasterData)
                        {
                            CampusChart = CampusChart + "<set value='" + VechcleSubType.ReportCount + "' />";
                        }
                        CampusChart = CampusChart + "</dataset>";
                    }


                    CampusChart = CampusChart + "</graph>";
                    Response.Write(CampusChart);
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
        public ActionResult TransportVehicleTypeCountChart()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    TransportBC samp = new TransportBC();

                    List<CampusMaster> CampusList = new List<CampusMaster>();
                    string sord = "";
                    sord = sord == "desc" ? "Desc" : "Asc";
                    string sidx = "Flag";
                    StudentsReportService SRS = new StudentsReportService();
                    Dictionary<long, IList<CampusMaster>> CampusMasterList = SRS.GetCampusMasterListWithPaging(0, 9999, sidx, sord, criteria);
                    CampusList = CampusMasterList.FirstOrDefault().Value.ToList();

                    criteria.Clear();
                    Dictionary<long, IList<VehicleTypeMaster>> VehicleTypeMasterDetails = null;
                    VehicleTypeMasterDetails = samp.GetVehicleTypeMasterListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    List<VehicleTypeMaster> VehicleTypeMasterList = new List<VehicleTypeMaster>();

                    var CampusArray = (from u in CampusMasterList.FirstOrDefault().Value
                                       select u.Name).ToArray();


                    var VehicleTypeMasterArray = (from u in VehicleTypeMasterDetails.FirstOrDefault().Value
                                                  where u.VehicleType != null
                                                  select u).ToArray().Distinct();

                    var CampusChart = "<graph caption='Campus Wise Vehicle Type Count' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='0' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";
                    CampusChart = CampusChart + "<categories>";
                    foreach (var item in CampusList)
                    {
                        CampusChart = CampusChart + "<category name='" + item.Name + "' />";
                    }
                    CampusChart = CampusChart + "</categories>";

                    criteria.Clear();
                    Dictionary<long, IList<VehicleSubTypeMaster>> VehicleSubTypeMasterDetails = null;
                    VehicleSubTypeMasterDetails = samp.GetVehicleSubTypeMasterListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    List<VehicleSubTypeMaster> VehicleSubTypeMasterList = new List<VehicleSubTypeMaster>();
                    string[] ColorCodes = { "8B008B", "0e6aad", "8A2BE2", "FF4500", "f8bd19", "e44a00", "008ee4", "6baa01", "f8bd19", "BDB76B" };
                    int ColorCodeId = 0;
                    foreach (var item in VehicleTypeMasterArray)
                    {
                        var TempList = (from u in VehicleSubTypeMasterDetails.FirstOrDefault().Value
                                        where u.VehicleTypeId == Convert.ToInt32(item.Id)
                                        select u).ToList();

                        List<VehicleSubTypeMaster> VehicleSubTypeMasterData = new List<VehicleSubTypeMaster>();
                        foreach (var CampusItem in CampusList)
                        {
                            VehicleSubTypeMaster VSM = new VehicleSubTypeMaster();
                            long TempCount = 0;
                            string Campus = string.Empty;
                            var TempVehicleTypeList = (from u in TempList
                                                       where u.Campus == CampusItem.Name
                                                       select u).ToList();

                            if (TempList.Count == 0) { TempCount = 0; Campus = CampusItem.Name; }
                            else { TempCount = TempVehicleTypeList.Count; Campus = TempList.FirstOrDefault().Campus; }
                            VSM.Campus = Campus;
                            VSM.ReportCount = TempCount;
                            VehicleSubTypeMasterData.Add(VSM);
                        }

                        CampusChart = CampusChart + " <dataset seriesname='" + item.VehicleType + "' color='" + ColorCodes[ColorCodeId] + "'>";
                        foreach (var VehicleSubType in VehicleSubTypeMasterData)
                        {
                            CampusChart = CampusChart + "<set value='" + VehicleSubType.ReportCount + "' />";
                        }
                        CampusChart = CampusChart + "</dataset>";
                        ColorCodeId = ColorCodeId + 1;
                    }

                    CampusChart = CampusChart + "</graph>";
                    Response.Write(CampusChart);

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
        public ActionResult TransportReportChartForSingleCampus(string Campus)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region Code
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                    AdminTemplateService ATS = new AdminTemplateService();
                    Dictionary<long, IList<AdminTemplateTransportReportByCampus_vw>> AdminTemplateTransportReportByCampus_vwDetails = null;
                    IList<AdminTemplateTransportReportByCampus_vw> AdminTemplateTransportReportByCampus_vwList = new List<AdminTemplateTransportReportByCampus_vw>();
                    AdminTemplateTransportReportByCampus_vwDetails = ATS.GetAdminTemplateTransportReportByCampus_vwListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    if (AdminTemplateTransportReportByCampus_vwDetails != null && AdminTemplateTransportReportByCampus_vwDetails.Count > 0 && AdminTemplateTransportReportByCampus_vwDetails.FirstOrDefault().Value.Count > 0)
                    {
                        var CampusChart = "<graph caption='Transport Reports for " + Campus + " Campus' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='0'>";
                        CampusChart = CampusChart + " <set name='Driver Count' value='" + AdminTemplateTransportReportByCampus_vwDetails.FirstOrDefault().Value[0].DriverCount + "' color='8A2BE2' issliced='1'/>";
                        CampusChart = CampusChart + " <set name='Route Count' value='" + AdminTemplateTransportReportByCampus_vwDetails.FirstOrDefault().Value[0].RouteCount + "' color='6baa01' issliced='1'/>";
                        CampusChart = CampusChart + " <set name='Location Count' value='" + AdminTemplateTransportReportByCampus_vwDetails.FirstOrDefault().Value[0].LocationCount + "' color='f8bd19' issliced='1'/>";
                        CampusChart = CampusChart + " <set name='Vehicles Count' value='" + AdminTemplateTransportReportByCampus_vwDetails.FirstOrDefault().Value[0].VehiclesCount + "' color='008ee4' issliced='1'/>";
                        CampusChart = CampusChart + "</graph>";
                        Response.Write(CampusChart);
                    }

                    #endregion
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult TransportVehicleTypeCountChartForSingleCampus(string Campus)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region Code
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    TransportBC samp = new TransportBC();
                    Dictionary<long, IList<VehicleTypeMaster>> VehicleTypeMasterDetails = null;
                    VehicleTypeMasterDetails = samp.GetVehicleTypeMasterListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    List<VehicleTypeMaster> VehicleTypeMasterList = new List<VehicleTypeMaster>();

                    var VehicleTypeMasterArray = (from u in VehicleTypeMasterDetails.FirstOrDefault().Value
                                                  where u.VehicleType != null
                                                  select u).ToArray().Distinct();



                    criteria.Clear();
                    Dictionary<long, IList<VehicleSubTypeMaster>> VehicleSubTypeMasterDetails = null;
                    VehicleSubTypeMasterDetails = samp.GetVehicleSubTypeMasterListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    List<VehicleSubTypeMaster> VehicleSubTypeMasterList = new List<VehicleSubTypeMaster>();
                    string[] ColorCodes = { "8B008B", "0e6aad", "8A2BE2", "FF4500", "f8bd19", "e44a00", "008ee4", "6baa01", "f8bd19", "BDB76B" };
                    //int ColorCodeId = 0;
                    List<VehicleSubTypeMaster> VehicleSubTypeMasterData = new List<VehicleSubTypeMaster>();
                    foreach (var item in VehicleTypeMasterArray)
                    {
                        var TempVehicleSubTypeList = (from u in VehicleSubTypeMasterDetails.FirstOrDefault().Value
                                                      where u.VehicleTypeId == Convert.ToInt32(item.Id) && u.Campus == Campus
                                                      select u).ToList();

                        VehicleSubTypeMaster VSM = new VehicleSubTypeMaster();
                        string TempCampus = string.Empty;

                        if (TempVehicleSubTypeList.Count > 0)
                        {
                            VSM.Model = item.VehicleType;
                            VSM.ReportCount = TempVehicleSubTypeList.Count;
                        }
                        else
                        {
                            VSM.Model = item.VehicleType;
                            VSM.ReportCount = 0;
                        }
                        VehicleSubTypeMasterData.Add(VSM);
                    }
                    var CampusChart = "<graph caption='Vehicles Type Reports for " + Campus + " Campus' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='0'>";
                    int j = 0;
                    foreach (var VehicleSubTypeItem in VehicleSubTypeMasterData)
                    {
                        CampusChart = CampusChart + " <set name='" + VehicleSubTypeItem.Model + "' value='" + VehicleSubTypeItem.ReportCount + "' color='" + ColorCodes[j] + "' issliced='1'/>";
                        j++;
                    }
                    CampusChart = CampusChart + "</graph>";
                    Response.Write(CampusChart);

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
        #region Users
        public ActionResult Users()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region code
                    AdminTemplateService ATS = new AdminTemplateService();
                    long CMSUsersCount = 0;
                    long ParentCount = 0;
                    long StaffCount = 0;
                    long StudentCount = 0;
                    long OthersCount = 0;
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    Dictionary<long, IList<AdminTemplateUsersTypeReport_vw>> UserTypeDetails = null;
                    string[] ColorCodes = { "0000FF", "8A2BE2", "A52A2A", "DEB887", "5F9EA0", "BDB76B", "8B008B", "556B2F", "FF8C00", "FF00FF", "191970", "CD853F", "FF0000", "708090", "008080", "9ACD32" };
                    IList<AdminTemplateUsersTypeReport_vw> UserTypeDetailsList = new List<AdminTemplateUsersTypeReport_vw>();
                    UserTypeDetails = ATS.GetAdminTemplateUsersTypeReport_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    if (UserTypeDetails != null && UserTypeDetails.Count > 0 && UserTypeDetails.FirstOrDefault().Value.Count > 0)
                    {
                        int TotalCount = Convert.ToInt32(UserTypeDetails.FirstOrDefault().Key);
                        string Campus = string.Empty;
                        for (int i = 0; i < TotalCount; i++)
                        {
                            AdminTemplateUsersTypeReport_vw Obj = new AdminTemplateUsersTypeReport_vw();
                            if (!string.IsNullOrEmpty(UserTypeDetails.FirstOrDefault().Value[i].UserType)) { Obj.UserType = UserTypeDetails.FirstOrDefault().Value[i].UserType; }
                            if (UserTypeDetails.FirstOrDefault().Value[i].Count >= 0) { Obj.Count = UserTypeDetails.FirstOrDefault().Value[i].Count; }
                            if (Obj != null) { UserTypeDetailsList.Add(Obj); }
                        }
                    }
                    if (UserTypeDetailsList != null && UserTypeDetailsList.Count > 0)
                    {
                        var CMSUsersList = (from u in UserTypeDetailsList
                                            where u.UserType == "CMSUsers"
                                            select u).ToList();
                        if (CMSUsersList != null && CMSUsersList.Count > 0) { CMSUsersCount = CMSUsersList.FirstOrDefault().Count; }


                        var ParentList = (from u in UserTypeDetailsList
                                          where u.UserType == "Parent"
                                          select u).ToList();
                        if (ParentList != null && ParentList.Count > 0) { ParentCount = ParentList.FirstOrDefault().Count; }

                        var StaffList = (from u in UserTypeDetailsList
                                         where u.UserType == "Staff"
                                         select u).ToList();
                        if (StaffList != null && StaffList.Count > 0) { StaffCount = StaffList.FirstOrDefault().Count; }

                        var StudentList = (from u in UserTypeDetailsList
                                           where u.UserType == "Student"
                                           select u).ToList();
                        if (StudentList != null && StudentList.Count > 0) { StudentCount = StudentList.FirstOrDefault().Count; }

                        var OthersList = (from u in UserTypeDetailsList
                                          where u.UserType == "Others"
                                          select u).ToList();
                        if (OthersList != null && OthersList.Count > 0) { OthersCount = OthersList.FirstOrDefault().Count; }
                    }
                    @ViewBag.CMSUsersCount = CMSUsersCount;
                    @ViewBag.ParentCount = ParentCount;
                    @ViewBag.StaffCount = StaffCount;
                    @ViewBag.StudentCount = StudentCount;
                    @ViewBag.OthersCount = OthersCount;
                    return View();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult GetUserTypeChart()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region code
                    AdminTemplateService ATS = new AdminTemplateService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    Dictionary<long, IList<AdminTemplateUsersTypeReport_vw>> UserTypeDetails = null;
                    string[] ColorCodes = { "FF00FF", "191970", "CD853F", "FF0000", "708090", "008080", "9ACD32", "0000FF", "8A2BE2", "A52A2A", "DEB887", "5F9EA0", "BDB76B", "8B008B", "556B2F", "FF8C00" };
                    IList<AdminTemplateUsersTypeReport_vw> UserTypeDetailsList = new List<AdminTemplateUsersTypeReport_vw>();
                    UserTypeDetails = ATS.GetAdminTemplateUsersTypeReport_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    if (UserTypeDetails != null && UserTypeDetails.Count > 0 && UserTypeDetails.FirstOrDefault().Value.Count > 0)
                    {
                        int TotalCount = Convert.ToInt32(UserTypeDetails.FirstOrDefault().Key);
                        string Campus = string.Empty;
                        for (int i = 0; i < TotalCount; i++)
                        {
                            AdminTemplateUsersTypeReport_vw Obj = new AdminTemplateUsersTypeReport_vw();
                            if (!string.IsNullOrEmpty(UserTypeDetails.FirstOrDefault().Value[i].UserType)) { Obj.UserType = UserTypeDetails.FirstOrDefault().Value[i].UserType; }
                            if (UserTypeDetails.FirstOrDefault().Value[i].Count >= 0) { Obj.Count = UserTypeDetails.FirstOrDefault().Value[i].Count; }
                            if (Obj != null) { UserTypeDetailsList.Add(Obj); }
                        }
                    }
                    var CampusChart = "<graph caption='User Type wise Report' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";
                    int j = 0;
                    foreach (var UsersTypeItem in UserTypeDetailsList)
                    {
                        CampusChart = CampusChart + " <set name='" + UsersTypeItem.UserType + "' value='" + UsersTypeItem.Count + "' color='" + ColorCodes[j] + "' />";
                        j++;
                    }
                    CampusChart = CampusChart + "</graph>";
                    Response.Write(CampusChart);
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
        public ActionResult GetUsersCampusWiseChart()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region code
                    AdminTemplateService ATS = new AdminTemplateService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    Dictionary<long, IList<AdminTemplateUsersReport_vw>> UserDetails = null;
                    string[] ColorCodes = { "0000FF", "8A2BE2", "A52A2A", "DEB887", "5F9EA0", "BDB76B", "8B008B", "556B2F", "FF8C00", "FF00FF", "191970", "CD853F", "FF0000", "708090", "008080", "9ACD32" };

                    IList<AdminTemplateUsersReport_vw> UserDetailsList = new List<AdminTemplateUsersReport_vw>();
                    UserDetails = ATS.GetAdminTemplateUsersReport_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    if (UserDetails != null && UserDetails.Count > 0 && UserDetails.FirstOrDefault().Value.Count > 0)
                    {
                        int TotalCount = Convert.ToInt32(UserDetails.FirstOrDefault().Key);
                        string Campus = string.Empty;
                        for (int i = 0; i < TotalCount; i++)
                        {
                            AdminTemplateUsersReport_vw Obj = new AdminTemplateUsersReport_vw();
                            if (!string.IsNullOrEmpty(UserDetails.FirstOrDefault().Value[i].Campus)) { Obj.Campus = UserDetails.FirstOrDefault().Value[i].Campus; }
                            if (UserDetails.FirstOrDefault().Value[i].Count >= 0) { Obj.Count = UserDetails.FirstOrDefault().Value[i].Count; }
                            if (Obj != null) { UserDetailsList.Add(Obj); }
                        }
                    }
                    var CampusChart = "<graph caption='Campus wise Users Count Report' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='1'>";
                    CampusChart = CampusChart + " <set name='' value='0' color='008ee4' />";
                    int j = 0;
                    foreach (var UsersItem in UserDetailsList)
                    {
                        CampusChart = CampusChart + " <set name='" + UsersItem.Campus + "' value='" + UsersItem.Count + "' color='" + ColorCodes[j] + "' />";
                        j++;
                    }
                    CampusChart = CampusChart + " <set name='' value='0' color='008ee4' />";
                    CampusChart = CampusChart + "</graph>";
                    Response.Write(CampusChart);
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
        #region Login History
        public ActionResult LoginHistory()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region code
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    AdminTemplateService ATS = new AdminTemplateService();
                    long CMSUsersCount = 0;
                    long ParentCount = 0;
                    long StaffCount = 0;
                    long StudentCount = 0;
                    long OthersCount = 0;
                    criteria.Clear();
                    Dictionary<long, IList<AdminTemplateLoginHistory_vw>> LoginHistoryDetails = null;
                    LoginHistoryDetails = ATS.GetAdminTemplateLoginHistory_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    if (LoginHistoryDetails != null && LoginHistoryDetails.Count > 0 && LoginHistoryDetails.FirstOrDefault().Value.Count > 0)
                    {
                        var CMSUsersList = (from u in LoginHistoryDetails.FirstOrDefault().Value
                                            where u.UserType == "CMSUsers"
                                            select u).ToList();
                        if (CMSUsersList.Count > 0) { CMSUsersCount = CMSUsersList.FirstOrDefault().Count; }

                        var ParentList = (from u in LoginHistoryDetails.FirstOrDefault().Value
                                          where u.UserType == "Parent"
                                          select u).ToList();
                        if (ParentList.Count > 0) { ParentCount = ParentList.FirstOrDefault().Count; }

                        var StaffList = (from u in LoginHistoryDetails.FirstOrDefault().Value
                                         where u.UserType == "Staff"
                                         select u).ToList();
                        if (StaffList.Count > 0) { StaffCount = StaffList.FirstOrDefault().Count; }

                        var StudentList = (from u in LoginHistoryDetails.FirstOrDefault().Value
                                           where u.UserType == "Student"
                                           select u).ToList();
                        if (StudentList.Count > 0) { StudentCount = StudentList.FirstOrDefault().Count; }

                        var OthersList = (from u in LoginHistoryDetails.FirstOrDefault().Value
                                          where u.UserType == "Others"
                                          select u).ToList();
                        if (OthersList.Count > 0) { OthersCount = OthersList.FirstOrDefault().Count; }

                    }
                    @ViewBag.CMSUsersCount = CMSUsersCount;
                    @ViewBag.ParentCount = ParentCount;
                    @ViewBag.StaffCount = StaffCount;
                    @ViewBag.StudentCount = StudentCount;
                    @ViewBag.OthersCount = OthersCount;
                    return View();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }

        }
        public ActionResult GetBrowserWiseChart()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region Code
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    UserService US = new UserService();
                    string[] ColorCodes = { "0e6aad", "e44a00", "008ee4", "FF4500", "f8bd19", "6baa01", "f8bd19", "BDB76B", "8B008B", "8A2BE2" };
                    criteria.Clear();

                    AdminTemplateService ATS = new AdminTemplateService();
                    Dictionary<long, IList<AdminTemplateBrowserWiseReport_vw>> AdminTemplateBrowserWiseReport_vwDetails = null;
                    AdminTemplateBrowserWiseReport_vwDetails = ATS.GetAdminTemplateBrowserWiseReport_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    var BrowserWiseChart = "<graph caption='Browser name wise login report' showpercentvalues='0' showNames='1' showValues='1' decimalPrecision='0' decimals='1' basefontsize='11' issliced='1' slicingDistance='10' funnelBaseWidth='25' fillAlpha='50' showborder='1' showhovercap='1' hoverCapBgColor='ffffff' useSameSlantAngle='1' isHollow='0' nameDistance='50' rotateNames='1'>";
                    if (AdminTemplateBrowserWiseReport_vwDetails != null && AdminTemplateBrowserWiseReport_vwDetails.Count > 0 && AdminTemplateBrowserWiseReport_vwDetails.FirstOrDefault().Value.Count > 0)
                    {
                        int i = 0;
                        foreach (var item in AdminTemplateBrowserWiseReport_vwDetails.FirstOrDefault().Value)
                        {
                            if (item.BrowserName != null)
                            {
                                BrowserWiseChart = BrowserWiseChart + " <set name='" + item.BrowserName + "' value='" + item.Count + "' color='" + ColorCodes[i] + "' />";
                                i = i + 1;
                            }
                        }
                    }
                    BrowserWiseChart = BrowserWiseChart + "</graph>";
                    Response.Write(BrowserWiseChart);
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
        public ActionResult GetUserTypeWiseChart()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region code
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    UserService US = new UserService();
                    string[] ColorCodes = { "8B008B", "0e6aad", "8A2BE2", "FF4500", "f8bd19", "e44a00", "008ee4", "6baa01", "f8bd19", "BDB76B" };
                    criteria.Clear();
                    AdminTemplateService ATS = new AdminTemplateService();
                    Dictionary<long, IList<AdminTemplateLoginHistory_vw>> LoginHistoryDetails = null;
                    LoginHistoryDetails = ATS.GetAdminTemplateLoginHistory_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                    var UserTypeWiseChart = "<graph caption='User Type Wise Report' xAxisName='' decimalPrecision='0' forceDecimals='0' formatNumberScale='0' yAxisName='' animation='1' showNames='1' showValues='1' divlinecolor='d3d3d3' distance='5' angle='45' rotateNames='0'>";
                    if (LoginHistoryDetails != null && LoginHistoryDetails.Count > 0 && LoginHistoryDetails.FirstOrDefault().Value.Count > 0)
                    {
                        int i = 0;
                        foreach (var item in LoginHistoryDetails.FirstOrDefault().Value)
                        {
                            if (item.UserType != null)
                            {
                                UserTypeWiseChart = UserTypeWiseChart + " <set name='" + item.UserType + "' value='" + item.Count + "' color='" + ColorCodes[i] + "' />";
                                i = i + 1;
                            }
                        }

                    }
                    UserTypeWiseChart = UserTypeWiseChart + "</graph>";
                    Response.Write(UserTypeWiseChart);
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
        #region Test
        public ActionResult Test()
        {
            return View();
        }
        public ActionResult TestChart()
        {
            long T1 = 20;
            long T2 = 10;
            long T3 = 30;

            var PieChart = "<graph caption='Test Chart' xAxisName='' yAxisName='' decimalPrecision='0' showNames='1' rotateNames='1' showValues='1' bgColor='FFFFFF' divlinecolor='F79708' distance='6' angle='45'>";
            PieChart = PieChart + " <set name='Log Issue' value='" + T1 + "' color='2ecc71' />";
            PieChart = PieChart + " <set name='Resolve Issue' value='" + T2 + "' color='3498db' />";
            PieChart = PieChart + " <set name='Approve Issue' value='" + T3 + "' color='e74c3c' />";
            PieChart = PieChart + "</graph>";
            Response.Write(PieChart);
            return null;
        }
        #endregion
    }
}
