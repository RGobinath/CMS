using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TIPS.ServiceContract;
using TIPS.Service;
using TIPS.Entities;
using TIPS.Component;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using TIPS.Entities.InboxEntities;
using TIPS.Entities.AdmissionEntities;
using TIPS.Entities.StoreEntities;
using TIPS.Entities.StaffEntities;
using TIPS.Entities.StaffManagementEntities;

namespace CMS.Controllers
{
    public class InboxController : BaseController
    {
        MastersService ms = new MastersService();
        InboxService IS = new InboxService();
        public ActionResult Notification()
        {
            return View();
        }
        public ActionResult Inbox()
        {
            return View();
        }
        public ActionResult InboxJqgrid(int? Id,string ddlSearchby, string txtSearch, int rows, string sord, string sidx, int? page = 1)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            string userId = base.ValidateUser();
            criteria.Add("UserId", userId);
            criteria.Add("Status","Inbox");
            Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();
            if (ddlSearchby == "PreRegNum" )
            {
                var PreRegNum = Convert.ToInt64(txtSearch);
                LikeCriteria.Add(ddlSearchby, PreRegNum);
            }
            else if (ddlSearchby == "RefNumber")
            {
                var RefNum = Convert.ToInt64(txtSearch);
                LikeCriteria.Add(ddlSearchby, RefNum);
            }
            else if (!string.IsNullOrEmpty(ddlSearchby)&&!string.IsNullOrEmpty(txtSearch)) 
            {
                LikeCriteria.Add(ddlSearchby.Trim(), txtSearch);
            }
         //   if (!string.IsNullOrEmpty(Term)) { LikeCriteria.Add("PreRegNum", Term); }
            //if (RefNum>0) { LikeCriteria.Add("RefNum", RefNum); }
            Dictionary<long, IList<Inbox>> InboxDetails = IS.InboxListWithLikeAndExcactSerachCriteria(page - 1, rows, sord, sidx, criteria, LikeCriteria);
          // Dictionary<long, IList<Inbox>> InboxDetails = IS.GetInboxDetailsWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);
            if (InboxDetails != null && InboxDetails.Count > 0 && InboxDetails.FirstOrDefault().Key > 0 && InboxDetails.FirstOrDefault().Value.Count > 0)
            {
                long totalrecords = InboxDetails.First().Key;
                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                var InboxList = new
                {
                    total = totalPages,
                    page = page,
                    records = totalrecords,
                    rows = (from items in InboxDetails.First().Value
                            select new
                            {
                                i = 2,
                                cell = new string[] {
                               items.Id.ToString(),
                               items.IsRead.ToString(),
                               items.Module,
                               String.Format("<b>"+ items.InformationFor +"</b>"+"    "+ @"<b><a style='color:#034af3;text-decoration:underline' href= '#' onclick = 'getdata("+"\"" + items.PreRegNum + "\"" + ")' >{0}</a></b>" ,items.Module ) + "  " + TimeAgo(items.CreatedDate)+".",
                                items.Module == "Call Management"? String.Format("<img src='/Images/History.png ' id='ImgHistory' onclick=\"ShowComments('" + GetCallmanagementById(items.RefNumber) +"', '"+ items.UserId +"' );\" />"):
                                items.Module == "Store"? String.Format("<img src='/Images/History.png ' id='ImgHistory' onclick=\"ShowComments('" + GetStoreById(items.RefNumber) +"', '"+ items.UserId +"' );\" />"):
                               items.Module == "Admission"? String.Format("<img src='/Images/History.png ' id='ImgHistory' onclick=\"AdmissionStatus('" + GetAdmissionByPreRegNum(items.PreRegNum) +"', '"+ items.UserId +"' );\" />"):
                                items.Module == "Staff Issues Management"? String.Format("<img src='/Images/History.png ' id='ImgHistory' onclick=\"StaffActivities('" + GetStaffIssueById(items.PreRegNum) +"', '"+ items.UserId +"' );\" />"):null,
                             items.CreatedDate.ToString(),
                                }
                            })
                };
                return Json(InboxList, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Inboxdata(long id)
        {
            Inbox In = IS.GetInboxDetailsById(id);
            if (In.Module == "Admission")
            {
                if (In.IsRead == false)
                {
                    In.IsRead = true;
                    IS.CreateOrUpdateInbox(In);
                }
                GetAdmissionInbox(id);
                return RedirectToAction("NewRegistration", "Admission", new { id = id });
            }
            else if (In.Module == "Call Management")
            {
                if (In.IsRead == false)
                {
                    In.IsRead = true;
                    IS.CreateOrUpdateInbox(In);
                }
                //GetCallManagementInbox(id);
                ProcessFlowServices ads = new ProcessFlowServices();
                CallManagement CM = ads.GetCallManagementById(Convert.ToInt64(id));
                return RedirectToAction("CallForward", "Home", new { id = id, activityId = 0, activityName="", status = CM.Status });
             
            }
            else if (In.Module == "Store")
            {
                if (In.IsRead == false)
                {
                    In.IsRead = true;
                    IS.CreateOrUpdateInbox(In);
                }
                GetStoreInbox(id);
                return RedirectToAction("MaterialRequest", "Store", new { id = id});
            }
            else if (In.Module == "Staff Issues Management")
            {
                if (In.IsRead == false)
                {
                    In.IsRead = true;
                    IS.CreateOrUpdateInbox(In);
                }
         
                return RedirectToAction("ShowStaffIssue", "StaffIssues", new { id = id });
            }
            else if (In.Module == "Staff Management")
            {
                if (In.IsRead == false)
                {
                    In.IsRead = true;
                    IS.CreateOrUpdateInbox(In);
                }
              //  GetStaffByPreRegNum(id);
                return RedirectToAction("GetStaffByPreRegNum", new { PreRegNum = id });
            }
            return null;
        }
        public ActionResult GetAdmissionInbox(long id)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                StudentTemplate StudentTemplate = ads.GetStudentDetailsByPreRegNo(id);
                ViewBag.approvalstatus = StudentTemplate.Status;

                FamilyDetails familyDetails = new FamilyDetails();
                if (StudentTemplate.FamilyDetailsList == null && StudentTemplate.FamilyDetailsList.Count == 0)
                {
                    IList<FamilyDetails> familydetailsList = new List<FamilyDetails>();
                    familydetailsList.Add(familyDetails);
                    StudentTemplate.FamilyDetailsList = familydetailsList;
                }

                PastSchoolDetails PastSchoolDetails = new PastSchoolDetails();

                if (StudentTemplate.PastSchoolDetailsList == null && StudentTemplate.PastSchoolDetailsList.Count == 0)
                {
                    IList<PastSchoolDetails> PastSchoolDetailsList = new List<PastSchoolDetails>();
                    PastSchoolDetailsList.Add(PastSchoolDetails);
                    StudentTemplate.PastSchoolDetailsList = PastSchoolDetailsList;
                }

                AddressDetails AddressDetails = new AddressDetails();
                if (StudentTemplate.AddressDetailsList == null && StudentTemplate.AddressDetailsList.Count == 0)
                {
                    IList<AddressDetails> AddressDetailsList = new List<AddressDetails>();
                    AddressDetailsList.Add(AddressDetails);
                    StudentTemplate.AddressDetailsList = AddressDetailsList;
                }

                Session["editid"] = StudentTemplate.Id;

                Session["preregno"] = StudentTemplate.PreRegNum;

                return RedirectToAction("NewRegistration", "Admission");
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public JsonResult GetInboxCount()
        {
            try
            {
                InboxService Is = new InboxService();
                string userid = base.ValidateUser();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("UserId", userid);
                Dictionary<long, IList<InboxCount_Vw>> InboxDetails = IS.GetInboxCountDetails(0, 9999, "Id", "Desc", criteria);
                var InboxCount = (from u in InboxDetails.First().Value
                                  where u.UnreadCount != null && u.UnreadCount != ""
                                  select u.UnreadCount).Distinct().ToList();
                return Json(InboxCount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public long GetCallmanagementById(long Id)
        {
            ProcessFlowServices ps = new ProcessFlowServices();
            CallManagement cm = ps.GetCallManagementById(Convert.ToInt64(Id));
            if (cm != null)
                return cm.InstanceId;
            else
                return 0;
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
                criteria.Add("InstanceId", Id);
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<Activity>> ActivitiesList = pfs.GetActivityWithsearchCriteria(page - 1, rows, sidx, sord, criteria, string.Empty, null, null);
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
                                         //items.CallManagementView.Id.ToString()
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
        //public string TimeAgo(DateTime? CreatedTime)
        //{
        //    DateTime startTime = DateTime.Now;
        //    DateTime endTime = DateTime.Now.AddDays(1).AddSeconds(123);
        //    TimeSpan span = endTime.Subtract(startTime);
        //    Debug.WriteLine("Time Difference (seconds): " + span.Seconds);
        //    Debug.WriteLine("Time Difference (minutes): " + span.Minutes);
        //    Debug.WriteLine("Time Difference (hours): " + span.Hours);
        //    Debug.WriteLine("Time Difference (days): " + span.Days);
        //    string daytimestring = (span.Days + span.Hours + span.Minutes + span.Seconds).ToString();
        //    return daytimestring;

        //}
        public static string TimeAgo(DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                return String.Format("about {0} {1} ago",
                years, years == 1 ? "year" : "years");
            }
            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0)
                    months += 1;
                return String.Format("about {0} {1} ago",
                months, months == 1 ? "month" : "months");
            }
            if (span.Days > 0)
                return String.Format("about {0} {1} ago",
                span.Days, span.Days == 1 ? "day" : "days");
            if (span.Hours > 0)
                return String.Format("about {0} {1} ago",
                span.Hours, span.Hours == 1 ? "hour" : "hours");
            if (span.Minutes > 0)
                return String.Format("about {0} {1} ago",
                span.Minutes, span.Minutes == 1 ? "minute" : "minutes");
            if (span.Seconds > 5)
                return String.Format("about {0} seconds ago", span.Seconds);
            if (span.Seconds <= 5)
                return "just now";
            return string.Empty;
        }
        public long GetStoreById(long Id)
        {
            StoreService ps = new StoreService();
            MaterialRequest cm = ps.GetMaterialRequestById(Convert.ToInt64(Id));
            if (cm != null)
                return cm.InstanceId;
            else
                return 0;
        }
        public ActionResult GetStoreInbox(long id)
        {
            try
            {
                StoreService ads = new StoreService();
                MaterialRequest store = ads.GetMaterialRequestById(Convert.ToInt64(id));
                Session["Id"] = store.Id;
                Session["InstanceId"] = store.InstanceId;
                return RedirectToAction("MaterialRequest", "Store");

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public long GetStaffIssueById(long Id)
        {
            StaffIssuesService ps = new StaffIssuesService();
            StaffIssues cm = ps.GetStaffIssuesById(Convert.ToInt64(Id));
            if (cm != null)
                return cm.InstanceId;
            else
                return 0;
        }
        public long GetInboxById(long Id)
        {
            InboxService ps = new InboxService();
            Inbox cm = ps.GetInboxById(Convert.ToInt64(Id));
            Id=cm.Id;
            return (Id);
        }
        public ActionResult DeleteInboxById(int? Id)
        {
            try
            {
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    InboxService ts = new InboxService();
                    Inbox In = new Inbox();
                    
                    if (Id > 0)
                       In = IS.GetInboxById(Convert.ToInt64(Id));
                        if(In.Status=="Inbox")
                        {
                            In.Status = "Deleted";
                            IS.CreateOrUpdateInbox(In);
                            return Json(Id, JsonRequestBehavior.AllowGet);
                        }
                    if (In != null)
                        if (In.Status == "Inbox")
                        {
                            In.Status = "Deleted";
                            IS.CreateOrUpdateInbox(In);
                            return Json(Id, JsonRequestBehavior.AllowGet);
                           // ts.DeleteInboxbyId(bc);
                        }
                    return RedirectToAction("Inbox");
                }
              
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult TrashJqgrid(int? Id, int rows, string sord, string sidx, int? page = 1)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            string userId = base.ValidateUser();
            criteria.Add("UserId", userId);
            criteria.Add("Status", "Deleted");
            Dictionary<long, IList<Inbox>> TrashDetails = IS.GetInboxDetailsWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);
            if (TrashDetails != null && TrashDetails.Count > 0 && TrashDetails.FirstOrDefault().Key > 0 && TrashDetails.FirstOrDefault().Value.Count > 0)
            {
                long totalrecords = TrashDetails.First().Key;
                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                var InboxList = new
                {
                    total = totalPages,
                    page = page,
                    records = totalrecords,
                    rows = (from items in TrashDetails.First().Value
                            select new
                            {
                                i = 2,
                                cell = new string[] {
                               items.Id.ToString(),
                              items.Module,
                               String.Format("<b>"+ items.InformationFor +"</b>"+"    "+ @"<b><a style='color:#034af3;text-decoration:underline' href= '#' onclick = 'getdata("+"\"" + items.PreRegNum + "\"" + ")' >{0}</a></b>" ,items.Module ) + "  " + TimeAgo(items.CreatedDate)+".",
                                items.Module == "Call Management"? String.Format("<img src='/Images/History.png ' id='ImgHistory' onclick=\"ShowComments('" + GetCallmanagementById(items.RefNumber) +"', '"+ items.UserId +"' );\" />"):
                                items.Module == "Store"? String.Format("<img src='/Images/History.png ' id='ImgHistory' onclick=\"ShowComments('" + GetStoreById(items.RefNumber) +"', '"+ items.UserId +"' );\" />"):
                                items.Module == "Admission"? String.Format("<img src='/Images/History.png ' id='ImgHistory' onclick=\"AdmissionStatus('" + GetAdmissionByPreRegNum(items.PreRegNum) +"', '"+ items.UserId +"' );\" />"):
                              items.Module == "Staff Issues Management"? String.Format("<img src='/Images/History.png ' id='ImgHistory' onclick=\"StaffActivities('" + GetStaffIssueById(items.RefNumber) +"', '"+ items.UserId +"' );\" />"):null,
                             items.CreatedDate.ToString(),
                             items.Status=="Deleted"?String.Format("<img src='/Images/undo.png ' id='ImgHistory' onclick=\"UndoInbox('" + GetInboxById(items.Id) +"' );\" />"):null,
                                }
                            })
                };
                return Json(InboxList, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public ActionResult StaffActivities(long? Id, string UserType)
        {
            ViewBag.Id = Id;
            ViewBag.UserType = UserType;
            return View();
        }
        public ActionResult StaffActivitiesListJqGrid(long? Id, string UserType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                ProcessFlowServices pfs = new ProcessFlowServices(); // TODO: Initialize to an appropriate value
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("InstanceId", Id);
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<StaffActivities>> StaffActivitiesList = pfs.GetStaffActivityWithsearchCriteria(page - 1, rows, sidx, sord, criteria, string.Empty, null, null);
                if (StaffActivitiesList != null && StaffActivitiesList.Count > 0)
                {
                    UserService us = new UserService();
                    long totalRecords = StaffActivitiesList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows = (
                             from items in StaffActivitiesList.First().Value
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
                                         //items.CallManagementView.Id.ToString()
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
        public ActionResult Undo(long id)
        {
            Inbox In = new Inbox();
            In = IS.GetInboxById(Convert.ToInt64(id));
            if (In.Status == "Deleted")
            {
                In.Status = "Inbox";
                IS.CreateOrUpdateInbox(In);
                // ts.DeleteInboxbyId(bc);
            }
            return RedirectToAction("Inbox");
        }
        public ActionResult AdmissionStatus(long? Id, string UserType)
        {
            ViewBag.Id = Id;
            ViewBag.UserType = UserType;
            return View();
        }
        public ActionResult AdmissionStatusJqgrid(long? Id, string UserType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                InboxService Is = new InboxService(); // TODO: Initialize to an appropriate value
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum",Id);
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<AdmissionStatus_Vw>> StaffActivitiesList = Is.GetAdmissionsWithPagingAndCriteria(page - 1, rows, sord,sidx, criteria);
                if (StaffActivitiesList != null && StaffActivitiesList.Count > 0)
                {
                    UserService us = new UserService();
                    long totalRecords = StaffActivitiesList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows = (
                             from items in StaffActivitiesList.First().Value
                             select new
                             {
                                 i = items.Id,
                                 cell = new string[]
                                     {
                                         items.Id.ToString(),
                                         items.ApplicationNo,
                                         items.Campus, 
                                       items.PreRegNum.ToString(),
                                       items.Name,
                                       items.AdmissionStatus,
                                         items.CreatedDate.ToString("dd/MM/yyyy hh:mm:ss tt"),
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
        public long GetAdmissionByPreRegNum(long PreRegNum)
        {
            InboxService ps = new InboxService();
            AdmissionStatus_Vw cm = ps.GetAdmissionDetailsByPreRegNo(PreRegNum);
            if (cm != null)
                return cm.PreRegNum;
            else
                return 0;
        }
        public ActionResult GetStaffByPreRegNum(long PreRegNum)
        {
            InboxService ps = new InboxService();
            StaffDetailsView cm = ps.GetStaffDetailsByPreRegNo(PreRegNum);
            long id = cm.Id;
            return RedirectToAction("ApplicationForm","StaffManagement", new { id = id });
        }



    }

}
