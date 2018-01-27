using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TIPS.Entities;
using TIPS.Entities.CommunictionEntities;
using TIPS.Entities.AdmissionEntities;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Net;
using Google.GData.Client;
using Google.Contacts;
using Google.GData.Extensions;
using TIPS.Service;
using TIPS.ServiceContract;
using TIPS.Entities.StaffManagementEntities;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using TIPS.Entities.ParentPortalEntities;
using System.Globalization;

namespace CMS.Controllers
{
    public class CommunicationController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        CommunicationService comSer = new CommunicationService();
        UserService UsSvc = new UserService();
        #region BulkEmailLog by felix kinoniya and changed by micheal

        public ActionResult RecipientsEmailRequest()
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

        //public ActionResult JqGridRecipientsEmailRequest(string BulkReqId, string Subject, string Attachment, string Message, string Status, string CreatedBy, int rows, string sidx, string sord, int? page = 1)
        //{
        //    try
        //    {
        //        //string userId = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
        //        CommunicationService comSer = new CommunicationService();
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        if (!string.IsNullOrEmpty(BulkReqId)) { criteria.Add("BulkReqId", BulkReqId); }
        //        if (!string.IsNullOrEmpty(Subject)) { criteria.Add("Subject", Subject); }
        //        if (!string.IsNullOrEmpty(Attachment))
        //        {
        //            if (Attachment == "1") { criteria.Add("Attachment", true); }
        //            if (Attachment == "0") { criteria.Add("Attachment", false); }
        //        }
        //        if (!string.IsNullOrEmpty(Status))
        //        {
        //            if (Status == "1") { criteria.Add("Status", "Email Composed"); }
        //            if (Status == "2") { criteria.Add("Status", "Recipients Added"); }
        //            if (Status == "3") { criteria.Add("Status", "CompletedWithErrors"); }
        //            if (Status == "4") { criteria.Add("Status", "SuccessfullyCompleted"); }
        //            if (Status == "5") { criteria.Add("Status", "Suspend"); }
        //        }
        //        //var usrcmp = Session["UserCampus"] as IEnumerable<string>;
        //        //if (usrcmp != null && usrcmp.First()!=null)           // to check if the usrcmp obj is null or with data
        //        //{
        //        //    criteria.Add("Campus", usrcmp);
        //        //}
        //        if (Session["UserId"] != null)
        //            criteria.Add("CreatedBy", Session["UserId"]);
        //        if (!string.IsNullOrEmpty(Message)) { criteria.Add("Message", Message); }

        //        Dictionary<long, IList<ComposeEmailInfo>> BulkComposeEmailRegList = comSer.GetComposeEmailInfoListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);

        //        long totalrecords = BulkComposeEmailRegList.First().Key;
        //        int totalPages = (int)Math.Ceiling(totalrecords / (float)5);
        //        var AssLst = new
        //        {
        //            total = totalPages,
        //            page = page,
        //            records = totalrecords,
        //            rows = (
        //                 from items in BulkComposeEmailRegList.First().Value

        //                 select new
        //                 {
        //                     cell = new string[] 
        //                                 {
        //                                     items.Id.ToString(),
        //                                     items.IdKeyValue.ToString(),
        //                                     "<a href='/Communication/BulkEmailRequest?Id=" + items.Id+"'>" + items.BulkReqId + "</a>",
        //                                   //  items.BulkReqId,
        //                                     items.UserId,
        //                                     items.Campus,
        //                                     items.AcademicYear,
        //                                     items.Father.ToString(),
        //                                     items.Mother.ToString(),
        //                                     items.General.ToString(),
        //                                     items.Subject,
        //                                     items.Attachment.ToString(),
        //                                     "<a  onclick=\"ShowComments('" + items.Id +"' , '"+items.BulkReqId+"');\">" +"..."+ "</a>",
        //                                    // items.Message,
        //                                     items.Status,
        //                                     items.CreatedBy,
        //                                     items.ModifiedBy,
        //                                     items.CreatedDate.ToString("dd/MM/yyyy"),
        //                                     items.ModifiedDate.ToString("dd/MM/yyyy")
        //                                 }
        //                 }).ToList()
        //        };
        //        return Json(AssLst, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}

        public ActionResult JqGridRecipientsEmailRequest(string BulkReqId, string Subject, string Attachment, string Message, string Status, string CreatedBy, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                //string userId = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                sord = sord == "desc" ? "Desc" : "Asc";
                CommunicationService comSer = new CommunicationService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(BulkReqId)) { criteria.Add("BulkReqId", BulkReqId); }
                if (!string.IsNullOrEmpty(Subject)) { criteria.Add("Subject", Subject); }
                if (!string.IsNullOrEmpty(Attachment))
                {
                    if (Attachment == "1") { criteria.Add("Attachment", true); }
                    if (Attachment == "0") { criteria.Add("Attachment", false); }
                }
                if (!string.IsNullOrEmpty(Status))
                {
                    if (Status == "1") { criteria.Add("Status", "Email Composed"); }
                    if (Status == "2") { criteria.Add("Status", "Recipients Added"); }
                    if (Status == "3") { criteria.Add("Status", "CompletedWithErrors"); }
                    if (Status == "4") { criteria.Add("Status", "SuccessfullyCompleted"); }
                    if (Status == "5") { criteria.Add("Status", "Suspend"); }
                }
                if (Session["UserId"].ToString() != null)
                    criteria.Add("CreatedBy", Session["UserId"]);
                if (!string.IsNullOrEmpty(Message)) { criteria.Add("Message", Message); }
                Dictionary<long, IList<BulkEmailRequestWithCount_vw>> BulkComposeEmailRegList = comSer.GetBulkEmailRequestWithCount_vwListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                UserService us = new UserService();
                long totalrecords = BulkComposeEmailRegList.First().Key;
                int totalPages = (int)Math.Ceiling(totalrecords / (float)5);
                var AssLst = new
                {
                    total = totalPages,
                    page = page,
                    records = totalrecords,
                    rows = (
                         from items in BulkComposeEmailRegList.First().Value

                         select new
                         {
                             cell = new string[] 
                                         {
                                             items.Id.ToString(),
                                             items.IdKeyValue.ToString(),
                                             "<a href='/Communication/BulkEmailRequest?Id=" + items.Id+"'>" + items.BulkReqId + "</a>",                                           
                                             items.UserId,
                                             items.Campus,
                                             items.Grade,
                                             items.AcademicYear,
                                             items.Father.ToString(),
                                             items.Mother.ToString(),
                                             items.General.ToString(),
                                             items.Subject,
                                             items.Attachment.ToString(),
                                             "<a  onclick=\"ShowComments('" + items.Id +"' , '"+items.BulkReqId+"');\">" +"..."+ "</a>",                                            
                                             items.Status,
                                             items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
                                             items.ModifiedBy,
                                             items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd'/'MM'/'yyyy"):"",
                                             items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd'/'MM'/'yyyy"):"",    
                                             items.Sent.ToString(),
                                             items.NotSent.ToString(),
                                             items.InProgress.ToString(),
                                             items.InvalidMail.ToString(),  
                                             items.Total.ToString()
                                         }
                         }).ToList()
                };
                return Json(AssLst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult ShowMessage(long Id)
        {
            string data = "";
            CommunicationService comServ = new CommunicationService();
            ComposeEmailInfo cei = comServ.GetComposeMailInfoById(Id);
            data = cei.Message;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveSuspend(long Id, string Reason)
        {
            CommunicationService comServ = new CommunicationService();
            ComposeEmailInfo cei = comServ.GetComposeMailInfoById(Id);
            cei.Suspend = "Suspend";
            cei.Status = "Suspend";
            cei.Reason = Reason;
            comServ.CreateOrUpdateComposeEmailInfo(cei);
            return null;
        }

        public ActionResult BulkEmailRequest(long? Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ComposeEmailInfo cei = new ComposeEmailInfo();
                    CommunicationService comSer = new CommunicationService();
                    MastersService ms = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    Dictionary<long, IList<FeeStructureYearMaster>> FeeStructyrMaster = ms.GetFeeStructureYearMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    ViewBag.acadddl = AcademicyrMaster.First().Value;
                    ViewBag.feestructddl = FeeStructyrMaster.First().Value;
                    if (Id > 0) { cei = comSer.GetComposeEmailInfoById(Id ?? 0); }
                    else { }
                    return View(cei);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult BulkEmailRequest(ComposeEmailInfo compEInfo)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOn", "Account");
                else
                {
                    CommunicationService comSer = new CommunicationService();
                    compEInfo.ModifiedDate = DateTime.Now;
                    compEInfo.ModifiedBy = userId;
                    if (compEInfo.Id == 0)
                    {
                        compEInfo.CreatedDate = DateTime.Now;
                        compEInfo.CreatedBy = userId;
                        long Id = comSer.CreateOrUpdateComposeEmailInfo(compEInfo);
                        compEInfo.BulkReqId = "BER-" + Id;
                        compEInfo.Status = "Email Composed";
                    }
                    comSer.CreateOrUpdateComposeEmailInfo(compEInfo);
                    return RedirectToAction("BulkEmailRequest", new { Id = compEInfo.Id });
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        //public ActionResult BulkMailAttachments(HttpPostedFileBase[] file2, string PreRegNum)
        //{
        //    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
        //    CommunicationService comSer = new CommunicationService();
        //    ComposeEmailInfo cei = new ComposeEmailInfo();
        //    string attName = "";

        //    for (int i = 0; i < file2.Length; i++)
        //    {
        //        string[] strAttachname = file2[i].FileName.Split('\\');
        //        Attachment mailAttach = new Attachment(file2[i].InputStream, strAttachname[strAttachname.Length - 1]);  //Data posted from form

        //        //string path = file1.InputStream.ToString();
        //        byte[] imageSize = new byte[file2[i].ContentLength];
        //        file2[i].InputStream.Read(imageSize, 0, (int)file2[i].ContentLength);
        //        EmailAttachment ea = new EmailAttachment();
        //        ea.Attachment = imageSize;
        //        ea.AttachmentName = strAttachname.First().ToString();
        //        ea.PreRegNum = Convert.ToInt32(PreRegNum);

        //        AdmissionManagementService ams = new AdmissionManagementService();
        //        ams.CreateOrUpdateEmailAttachment(ea);
        //        attName = "success fully Attached";
        //    }
        //    // SaveOrUpdate the Attachment value in ComposeEmailInfo table
        //    // PreRegNum == ComposeEmailInfo Table Id
        //    cei = comSer.GetComposeEmailInfoById(Convert.ToInt64(PreRegNum));
        //    cei.Attachment = true;
        //    comSer.CreateOrUpdateComposeEmailInfo(cei);
        //    return Json(new { success = true, result = attName }, "text/html", JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult BulkMailAttachments(HttpPostedFileBase[] file2, string PreRegNum)
        //{
        //    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
        //    CommunicationService comSer = new CommunicationService();
        //    ComposeEmailInfo cei = new ComposeEmailInfo();
        //    string attName = "";
        //    string att = "";
        //    for (int i = 0; i < file2.Length; i++)
        //    {
        //        string[] strAttachname = file2[i].FileName.Split('\\');
        //        Attachment mailAttach = new Attachment(file2[i].InputStream, strAttachname[strAttachname.Length - 1]);  //Data posted from form

        //        //string path = file1.InputStream.ToString();
        //        byte[] imageSize = new byte[file2[i].ContentLength];
        //        file2[i].InputStream.Read(imageSize, 0, (int)file2[i].ContentLength);
        //        EmailAttachment ea = new EmailAttachment();
        //        ea.Attachment = imageSize;
        //        ea.AttachmentName = strAttachname.First().ToString();
        //        ea.PreRegNum = Convert.ToInt32(PreRegNum);
        //        att = att + ea.AttachmentName + "<br/>";
        //        AdmissionManagementService ams = new AdmissionManagementService();
        //        ams.CreateOrUpdateEmailAttachment(ea);

        //    }
        //    attName = att + " successfully Attached";
        //    // SaveOrUpdate the Attachment value in ComposeEmailInfo table
        //    // PreRegNum == ComposeEmailInfo Table Id
        //    cei = comSer.GetComposeEmailInfoById(Convert.ToInt64(PreRegNum));
        //    cei.Attachment = true;
        //    comSer.CreateOrUpdateComposeEmailInfo(cei);
        //    return Json(new { success = true, result = attName }, "text/html", JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult BulkMailAttachments(HttpPostedFileBase[] file2, string PreRegNum)
        //{
        //    ComposeEmailInfo cei = new ComposeEmailInfo();
        //    Int64 AttmemSize = 0;
        //    string attName = "";
        //    string att = "";
        //    List<EmailAttachment> Attachmentlist = new List<EmailAttachment>();
        //    for (int i = 0; i < file2.Length; i++)
        //    {
        //        string[] strAttachname = file2[i].FileName.Split('\\');
        //        byte[] imageSize = new byte[file2[i].ContentLength];
        //        file2[i].InputStream.Read(imageSize, 0, (int)file2[i].ContentLength);
        //        AttmemSize = AttmemSize + file2[i].ContentLength;
        //        EmailAttachment ea = new EmailAttachment();
        //        ea.Attachment = imageSize;
        //        ea.AttachmentName = strAttachname.First().ToString();
        //        ea.PreRegNum = Convert.ToInt32(PreRegNum);
        //        att = att + ea.AttachmentName + "<br/>";
        //        Attachmentlist.Add(ea);
        //    }
        //    if ((AttmemSize / 1048576) < 2)
        //    {
        //        comSer.CreateOrUpdateEmailAttachmentsList(Attachmentlist);
        //        attName = att + " successfully Attached";
        //        // SaveOrUpdate the Attachment value in ComposeEmailInfo table
        //        // PreRegNum == ComposeEmailInfo Table Id
        //        cei = comSer.GetComposeEmailInfoById(Convert.ToInt64(PreRegNum));
        //        cei.Attachment = true;
        //        comSer.CreateOrUpdateComposeEmailInfo(cei);
        //        return Json(new { success = true, Message = attName }, "text/html", JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    { return Json(new { success = false, Message = "NotUploaded" }, "text/html", JsonRequestBehavior.AllowGet); }

        //}
        public ActionResult BulkMailAttachments(HttpPostedFileBase[] file2, string PreRegNum, string AppName)
        {
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();

            Int64 AttmemSize = 0;
            string attName = "";
            string att = "";
            List<EmailAttachment> Attachmentlist = new List<EmailAttachment>();
            for (int i = 0; i < file2.Length; i++)
            {
                string[] strAttachname = file2[i].FileName.Split('\\');
                Attachment mailAttach = new Attachment(file2[i].InputStream, strAttachname[strAttachname.Length - 1]);  //Data posted from form
                //string path = file1.InputStream.ToString();
                byte[] imageSize = new byte[file2[i].ContentLength];
                file2[i].InputStream.Read(imageSize, 0, (int)file2[i].ContentLength);
                AttmemSize = AttmemSize + file2[i].ContentLength;
                EmailAttachment ea = new EmailAttachment();
                ea.Attachment = imageSize;
                ea.AttachmentName = strAttachname.First().ToString();
                ea.PreRegNum = Convert.ToInt32(PreRegNum);
                ea.AppName = AppName;
                att = att + ea.AttachmentName + "<br/>";
                Attachmentlist.Add(ea);
            }
            if ((AttmemSize / 1048576) < 2)
            {
                comSer.CreateOrUpdateEmailAttachmentsList(Attachmentlist);
                attName = att + " successfully Attached";
                // SaveOrUpdate the Attachment value in ComposeEmailInfo table
                // PreRegNum == ComposeEmailInfo Table Id
                if (AppName == "Student")
                {
                    ComposeEmailInfo cei = new ComposeEmailInfo();
                    cei = comSer.GetComposeEmailInfoById(Convert.ToInt64(PreRegNum));
                    cei.Attachment = true;
                    comSer.CreateOrUpdateComposeEmailInfo(cei);
                }
                if (AppName == "Staff")
                {
                    StaffComposeMailInfo scei = new StaffComposeMailInfo();
                    scei = comSer.GetStaffComposeEmailInfoById(Convert.ToInt64(PreRegNum));
                    scei.Attachment = true;
                    comSer.CreateOrUpdateStaffComposeEmailInfo(scei);
                }
                return Json(new { success = true, Message = attName }, "text/html", JsonRequestBehavior.AllowGet);
            }
            else
            { return Json(new { success = false, Message = "NotUploaded" }, "text/html", JsonRequestBehavior.AllowGet); }

        }
        public ActionResult BulkEmailDeleteAttachment(string PreRegNum)
        {
            try
            {
                if (PreRegNum != "0")
                {
                    CommunicationService comSer = new CommunicationService();
                    AdmissionManagementService ams = new AdmissionManagementService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("PreRegNum", Convert.ToInt32(PreRegNum));
                    Dictionary<long, IList<EmailAttachment>> emailattachment = ams.GetEmailAttachmentListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                    IList<EmailAttachment> listcount = emailattachment.FirstOrDefault().Value.ToArray();
                    long[] idtodelete = new long[listcount.Count];
                    int i = 0;
                    foreach (var val in listcount)
                    {
                        idtodelete[i] = Convert.ToInt64(val.Id);
                        i++;
                    }
                    ams.DeleteAttachment(idtodelete);

                    // SaveOrUpdate the Attachment value in ComposeEmailInfo table
                    ComposeEmailInfo cei = new ComposeEmailInfo();
                    cei = comSer.GetComposeEmailInfoById(Convert.ToInt64(PreRegNum));
                    cei.Attachment = false;
                    comSer.CreateOrUpdateComposeEmailInfo(cei);
                }

                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult JqgridBulkEmailRequest(string Campus, string Grade, string AdStatus, string FeeStYear, string StName, string StId, string StIshostel, string AcYear, string VanNo, string Section, string saveOrClear, long ComposeId, string AppliedFrmDate, string AppliedToDate, int rows, string sord, string sidx, int? page = 1)
        {
            try
            {
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                DateTime DateNow = DateTime.Now;
                CommunicationService comSer = new CommunicationService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                ComposeEmailInfo cei = new ComposeEmailInfo();
                Dictionary<long, IList<BulkEmailRequest>> BulkEmailRegList = null;
                Dictionary<long, IList<RecipientsEmailInfo>> BulkEmailRecList = null;
                if (ComposeId > 0) { cei = comSer.GetComposeEmailInfoById(ComposeId); }
                if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                if (!string.IsNullOrEmpty(AdStatus)) { criteria.Add("AdmissionStatus", AdStatus); }
                else criteria.Add("AdmissionStatus", "Registered");
                if (!string.IsNullOrEmpty(FeeStYear)) { criteria.Add("FeeStructYear", FeeStYear); }
                if (!string.IsNullOrEmpty(StName)) { likeCriteria.Add("Name", StName); }
                if (!string.IsNullOrEmpty(StId)) { likeCriteria.Add("NewId", StId); }
                if (!string.IsNullOrEmpty(VanNo)) { criteria.Add("VanNo", VanNo); }
                if (!string.IsNullOrEmpty(StIshostel))
                {
                    criteria.Add("IsHosteller", Convert.ToBoolean(StIshostel));
                }
                if (!string.IsNullOrEmpty(AcYear)) { criteria.Add("AcademicYear", AcYear); }
                if (Grade != "null" && Grade != null)
                {
                    string[] Gradarr = Grade.Split(',');
                    criteria.Add("Grade", Gradarr);
                }
                if (Section != "null" && Section != null)
                {
                    string[] sectionarr = Section.Split(',');
                    criteria.Add("Section", sectionarr);
                }
                sord = sord == "desc" ? "Desc" : "Asc";
                if (ComposeId > 0 && cei.IsSaveList)
                {
                    criteria.Clear();
                    criteria.Add("ComposeId", ComposeId);
                    BulkEmailRecList = comSer.GetRecipientsEmailListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                }
                else
                {
                    if (!string.IsNullOrEmpty(Campus))
                    {
                        if ((!string.IsNullOrEmpty(AppliedFrmDate)) && (!string.IsNullOrEmpty(AppliedToDate)))
                        {
                            if (!string.IsNullOrEmpty(AppliedFrmDate) && !string.IsNullOrEmpty(AppliedToDate))
                            {
                                AppliedFrmDate = AppliedFrmDate.Trim();
                                AppliedToDate = AppliedToDate.Trim();
                                DateTime[] fromto = new DateTime[2];
                                fromto[0] = DateTime.Parse(AppliedFrmDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                                fromto[1] = DateTime.Parse(AppliedToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);// Convert.ToDateTime(To + " " + "23:59:59");
                                criteria.Add("CreatedDateNew", fromto);
                            }
                        }
                        List<string> FamilyType = new List<string>();
                        if (cei.General == true)
                            FamilyType.Add("General");
                        if (cei.Father == true)
                            FamilyType.Add("Father");
                        if (cei.Mother == true)
                            FamilyType.Add("Mother");
                        if (FamilyType.Count > 0) { criteria.Add("FamilyDetailType", FamilyType.ToArray()); }
                        BulkEmailRegList = comSer.GetBulkEmailRegListWithLikeandExactPagingAndCriteria(page - 1, 9999, sord, sidx, criteria, likeCriteria);
                    }
                }

                if (saveOrClear == "Save")
                {
                    List<RecipientsEmailInfo> recipientslist = new List<RecipientsEmailInfo>();
                    if (BulkEmailRegList != null)
                    {
                        foreach (var item in BulkEmailRegList.FirstOrDefault().Value)
                        {
                            RecipientsEmailInfo rei = new RecipientsEmailInfo();
                            rei.ComposeId = Convert.ToInt64(ComposeId);
                            rei.PreRegNum = item.PreRegNum;
                            rei.Campus = item.Campus;
                            rei.Name = item.Name;
                            rei.NewId = item.NewId;
                            rei.IsHosteller = item.IsHosteller.ToString();
                            rei.FeeStructYear = item.FeeStructYear;
                            rei.AcademicYear = item.AcademicYear;
                            rei.AdmissionStatus = item.AdmissionStatus;
                            rei.VanNo = item.VanNo;
                            rei.RecipientsCreatedDate = DateTime.Now;
                            rei.RecipientsModifiedDate = DateTime.Now;
                            rei.IdKeyValue = item.IdKeyValue;
                            rei.FamilyDetailType = item.FamilyDetailType;
                            rei.U_EmailId = item.U_EmailId;
                            rei.AppliedDate = item.CreatedDateNew;
                            //if (cei.Father == true) { rei.FatherEmailId = item.FatherEmailId; }
                            //if (cei.Mother) { rei.MotherEmailId = item.MotherEmailId; }
                            //if (cei.General == true) { rei.GeneralEmailId = item.GeneralEmailId; }
                            rei.Grade = item.Grade;
                            rei.Section = item.Section;
                            rei.Status = "InProgress";
                            recipientslist.Add(rei);
                        }
                        comSer.CreateOrUpdateRecipientsEmailList(recipientslist);
                        foreach (BulkEmailRequest item in BulkEmailRegList.FirstOrDefault().Value)
                        {
                            item.Status = "InProgress";
                            item.RecipientsCreatedDate = DateTime.Now;
                            item.RecipientsModifiedDate = DateTime.Now;
                        }

                        /// Save to the ComposeEmailInfo table
                        cei.IsSaveList = true;
                        cei.Status = "Recipients Added";
                        if (recipientslist[0] != null)
                        {
                            if (AcYear != "" & !string.IsNullOrEmpty(AcYear))
                                cei.AcademicYear = AcYear;
                            cei.AdmissionStatus = AdStatus;
                            if (FeeStYear != "" & !string.IsNullOrEmpty(FeeStYear))
                                cei.FeeStructYear = FeeStYear;
                            if (StName != "" & !string.IsNullOrEmpty(StName))
                                cei.StudentName = StName;
                            if (StId != "" & !string.IsNullOrEmpty(StId))
                                cei.NewId = StId;
                            cei.IsHosteller = StIshostel;
                            //if (StIshostel == "True")
                            // cei.IsHosteller = "True";
                            //else
                            // cei.IsHosteller = "False"; // Convert.ToBoolean(StIshostel);
                            //cei.AcademicYear = AcYear;
                            cei.VanNo = VanNo;
                            cei.Campus = Campus;
                            cei.Grade = Grade;
                            cei.Section = Section;
                            if (AppliedFrmDate != "" & !string.IsNullOrEmpty(AppliedFrmDate))
                                cei.AppliedFromDate = DateTime.Parse(AppliedFrmDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            if (AppliedToDate != "" & !string.IsNullOrEmpty(AppliedToDate))
                                cei.AppliedToDate = DateTime.Parse(AppliedToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        }
                        comSer.CreateOrUpdateComposeEmailInfo(cei);
                    }

                }
                else
                {
                    if (saveOrClear == "Clear")
                    {
                        comSer.ClearSavedListInRecipientsEmailInfo(Convert.ToInt64(ComposeId));
                        cei.IsSaveList = false;
                        cei.Status = "Email Composed";
                        cei.Campus = null;
                        cei.Grade = null;
                        cei.Section = null;
                        cei.StudentName = null;
                        cei.AcademicYear = null;
                        cei.AdmissionStatus = null;
                        cei.FeeStructYear = null;
                        cei.VanNo = null;
                        cei.AppliedFromDate = null;
                        cei.AppliedToDate = null;
                        comSer.CreateOrUpdateComposeEmailInfo(cei);
                        var jsondat = new { rows = (new { cell = new string[] { } }) };
                        return Json(jsondat, JsonRequestBehavior.AllowGet);
                    }
                }
                if (BulkEmailRegList != null && BulkEmailRegList.FirstOrDefault().Value.Count > 0 && BulkEmailRegList.FirstOrDefault().Key > 0)
                {
                    long totalrecords = BulkEmailRegList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in BulkEmailRegList.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                                             items.Id.ToString(),
                                             items.IdKeyValue.ToString(),
                                             items.PreRegNum.ToString(),
                                             items.NewId,
                                             items.Name,
                                             items.Campus,
                                             items.Grade,
                                             items.Section,
                                             items.FeeStructYear,
                                             items.AdmissionStatus,
                                             items.AcademicYear,
                                             items.IsHosteller==true?"Yes":"No",
                                             items.VanNo,
                                             items.Status,
                                             items.FamilyDetailType,
                                             items.U_EmailId,
                                             items.CreatedDateNew.ToString("dd'/'MM'/'yyyy"),
                                             //(cei.General)?items.GeneralEmailId:"",
                                             //(cei.Father)?items.FatherEmailId:"",
                                             //(cei.Mother)?items.MotherEmailId:"",
                                             // items.RecipientsCreatedDate!=null? ConvertDateTimeToDate(DateTime.Now.ToString("dd/MM/yyyy"),"en-GB"):"",
                                             //items.RecipientsModifiedDate!=null? ConvertDateTimeToDate(DateTime.Now.ToString("dd/MM/yyyy"),"en-GB"):"",
                                             items.RecipientsCreatedDate != null?items.RecipientsCreatedDate.ToString():"",
                                             items.RecipientsModifiedDate != null?items.RecipientsModifiedDate.ToString():"",
                                             }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (BulkEmailRecList != null && BulkEmailRecList.FirstOrDefault().Value.Count > 0 && BulkEmailRecList.FirstOrDefault().Key > 0)
                    {
                        long totalrecords = BulkEmailRecList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in BulkEmailRecList.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                                                  items.Id.ToString(),
                                                  items.IdKeyValue.ToString(),
                                                  items.PreRegNum.ToString(),
                                                  items.NewId,
                                                  items.Name,
                                                  items.Campus,
                                                  items.Grade,
                                                  items.Section,
                                                  items.FeeStructYear,
                                                  items.AdmissionStatus,
                                                  items.AcademicYear,
                                                  items.IsHosteller=="True"?"Yes":"No",
                                                   items.VanNo,
                                                  items.Status,
                                                  items.FamilyDetailType,
                                                  items.U_EmailId,
                                                  items.AppliedDate.ToString("dd'/'MM'/'yyyy"),
                                                  //items.GeneralEmailId,
                                                  //items.FatherEmailId,
                                                  //items.MotherEmailId,
                                                  //items.RecipientsCreatedDate!=null? ConvertDateTimeToDate(items.RecipientsCreatedDate.ToString("dd/MM/yyyy"),"en-GB"):"",
                                                  //items.RecipientsModifiedDate!=null? ConvertDateTimeToDate(items.RecipientsModifiedDate.ToString("dd/MM/yyyy"),"en-GB"):"",
                                                  items.RecipientsCreatedDate != null?items.RecipientsCreatedDate.ToString():"",
                                                  items.RecipientsModifiedDate != null?items.RecipientsModifiedDate.ToString():"",
                                                  }
                                    })
                        };
                        return Json(jsondat, JsonRequestBehavior.AllowGet);
                    }
                }

                var jsonda = new { rows = (new { cell = new string[] { } }) };
                return Json(jsonda, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public bool SendEmailAndUpdateRecipientsEmailInfo(System.Net.Mail.MailMessage mail, IList<RecipientsEmailInfo> listToUpdate, CommunicationService comServ, SmtpClient smtp, IList<CampusEmailId> CampusEmailId)
        {
            bool retValue = false;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
                //send email and store it in db
                smtp.Send(mail);
                comServ.CreateOrUpdateRecipientsEmailListWithStatus(listToUpdate, "Sent");
                //psf.save the list
                mail.Bcc.Clear();
                retValue = true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("quota"))
                {
                    try
                    {
                        smtp.Host = CampusEmailId.First().AlternateBulkEmailIdHost; //Or Your SMTP Server Address 
                        smtp.Port = CampusEmailId.First().AlternateBulkEmailIdPort;
                        mail.From = new MailAddress(CampusEmailId.First().EmailId.ToString());
                        smtp.Credentials = new System.Net.NetworkCredential
                        (CampusEmailId.First().AlternateBulkEmailId.ToString(), CampusEmailId.First().AlternateBulkEmailIdPassword.ToString());
                        smtp.Send(mail);
                        mail.Bcc.Clear();
                        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                        retValue = true;
                    }
                    catch (Exception)
                    {
                        comServ.CreateOrUpdateRecipientsEmailListWithStatus(listToUpdate, "Not Sent");
                        mail.Bcc.Clear();
                        retValue = false;
                    }
                }
                else
                {
                    try
                    {
                        smtp.Host = CampusEmailId.First().AlternateBulkEmailIdHost; //Or Your SMTP Server Address 
                        smtp.Port = CampusEmailId.First().AlternateBulkEmailIdPort;
                        mail.From = new MailAddress(CampusEmailId.First().EmailId.ToString());
                        smtp.Credentials = new System.Net.NetworkCredential
                        (CampusEmailId.First().AlternateBulkEmailId.ToString(), CampusEmailId.First().AlternateBulkEmailIdPassword.ToString());
                        smtp.Send(mail);
                        mail.Bcc.Clear();
                        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                        retValue = true;
                    }
                    catch (Exception)
                    {
                        comServ.CreateOrUpdateRecipientsEmailListWithStatus(listToUpdate, "Not Sent");
                        mail.Bcc.Clear();
                        retValue = false;
                    }
                }
            }
            return retValue;
        }

        public bool SendEmailAndUpdateRecipientsEmailInfoForHotmail(System.Net.Mail.MailMessage mail, IList<RecipientsEmailInfo> listToUpdate, CommunicationService comServ, SmtpClient smtp, IList<CampusEmailId> CampusEmailId)
        {
            bool retValue = false;
            try
            {
                smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address 
                smtp.Port = 25;
                smtp.Credentials = new System.Net.NetworkCredential
               (CampusEmailId.First().EmailId.ToString(), CampusEmailId.First().Password.ToString());
                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
                //send email and store it in db
                smtp.Send(mail);
                comServ.CreateOrUpdateRecipientsEmailListWithStatus(listToUpdate, "Sent");
                //psf.save the list
                mail.Bcc.Clear();
                retValue = true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("quota"))
                {
                    try
                    {
                        smtp.Credentials = new System.Net.NetworkCredential
                        (CampusEmailId.First().AlternateEmailId.ToString(), CampusEmailId.First().AlternateEmailIdPassword.ToString());
                        smtp.Send(mail);
                        mail.Bcc.Clear();
                        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                        retValue = true;
                    }
                    catch (Exception)
                    {
                        comServ.CreateOrUpdateRecipientsEmailListWithStatus(listToUpdate, "Not Sent");
                        mail.Bcc.Clear();
                        retValue = false;
                    }
                }
                else
                {
                    try
                    {
                        smtp.Credentials = new System.Net.NetworkCredential
                       (CampusEmailId.First().AlternateEmailId.ToString(), CampusEmailId.First().AlternateEmailIdPassword.ToString());
                        smtp.Send(mail);
                        mail.Bcc.Clear();
                        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                        retValue = true;
                    }
                    catch (Exception)
                    {
                        comServ.CreateOrUpdateRecipientsEmailListWithStatus(listToUpdate, "Not Sent");
                        mail.Bcc.Clear();
                        retValue = false;
                    }
                }
            }
            return retValue;
        }

        public ActionResult SendBulkEmailRequest(long ComposeId, string Campus, bool IsAlterNativeMail, bool StudentPortal, bool ParentPortal, string ExpiryDate)
        {

            /// Bulk email Added option is true
            CommunicationService comServ = new CommunicationService();
            ComposeEmailInfo cei = comServ.GetComposeMailInfoById(ComposeId);
            IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
            cei.BulkEmailAdded = true;
            cei.StudentPortal = StudentPortal;
            cei.ParentPortal = ParentPortal;
            cei.MailDate = DateTime.Today.Date.ToString("dd-MM-yyyy");
            cei.ExpiryDate = ExpiryDate;
            comServ.CreateOrUpdateComposeEmailInfo(cei);
            #region notification
            if (cei.BulkEmailAdded == true && (cei.StudentPortal == true || cei.ParentPortal == true) && !string.IsNullOrEmpty(cei.ExpiryDate))
            {
                ParentPortalService pps = new ParentPortalService();
                Notification notify = new Notification();
                notify.Campus = cei.Campus;
                notify.Grade = cei.Grade;
                notify.Section = cei.Section;
                notify.AcademicYear = cei.AcademicYear;
                notify.ExpireDate = cei.ExpiryDate;
                notify.PublishedOn = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
                notify.Topic = cei.Subject;
                notify.Message = cei.Message;
                notify.Performer = cei.CreatedBy;
                notify.NoteType = "General";
                notify.NewIds = "";
                notify.Type = "BulkMail";
                notify.PublishDate = cei.MailDate;
                notify.Status = "1"; // 1 for unread - 0 for read
                DateTime pubdate = DateTime.ParseExact(cei.MailDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                DateTime curDate = DateTime.Today;
                int setValid = DateTime.Compare(curDate, pubdate);
                if (setValid == 0)
                {
                    notify.Valid = "1";
                }
                else
                {
                    notify.Valid = "0";
                }
                if (cei.StudentPortal == true && cei.ParentPortal == true)
                {
                    notify.PublishTo = "General";
                }
                else if (cei.StudentPortal == true && cei.ParentPortal == false)
                {
                    notify.PublishTo = "Student";
                }
                else if (cei.ParentPortal == true && cei.StudentPortal == false)
                {
                    notify.PublishTo = "Parent";
                }
                else
                {
                    notify.PublishTo = "";
                }
                var maxnoteid = pps.GetMaxNoteId();
                notify.NotePreId = maxnoteid + 1;
                pps.UpdateNoteCount(notify.NotePreId);
                pps.CreateOrUpdateNotification(notify);
                if (notify.Id > 0)
                {
                    if (cei.Attachment == true)
                    {
                        AdmissionManagementService ads = new AdmissionManagementService();
                        Dictionary<string, object> criteria = new Dictionary<string, object>();
                        criteria.Add("PreRegNum", Convert.ToInt32(cei.Id));
                        criteria.Add("AppName", "Student");
                        Dictionary<long, IList<EmailAttachment>> emailattachment = ads.GetEmailAttachmentListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                        List<NoteAttachment> nlist = new List<NoteAttachment>();
                        foreach (var item in emailattachment.FirstOrDefault().Value)
                        {
                            NoteAttachment noteattach = new NoteAttachment();
                            noteattach.Attachment = item.Attachment;
                            noteattach.AttachmentName = item.AttachmentName;
                            noteattach.NotePreId = notify.NotePreId;
                            nlist.Add(noteattach);
                        }
                        if (nlist!=null && nlist.Count > 0)
                        {
                            pps.CreateOrUpdateNoteAttachmentsList(nlist);
                        }
                    }
                }
            }
            #endregion
            if (!string.IsNullOrEmpty(cei.Campus))
                SendMailToAcaDir(cei);
            new Task(() => { SendBulkEmailRequestWithAsync(ComposeId, (!string.IsNullOrEmpty(cei.Campus)) ? cei.Campus : Campus, IsAlterNativeMail); }).Start();
            //bulk send email has been initiated and in progress
            //status will be updated once it is complete
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        //public ActionResult SendBulkEmailRequestWithAsync(long ComposeId, string Campus, bool IsAlterNativeMail)
        //{
        //    try
        //    {
        //        bool bulkSendCompleteWithError = false;
        //        AdmissionManagementService ams = new AdmissionManagementService();
        //        CommunicationService comServ = new CommunicationService();
        //        IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
        //        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
        //        ComposeEmailInfo cei = comServ.GetComposeMailInfoById(ComposeId);
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        criteria.Add("ComposeId", ComposeId);
        //        IList<RecipientsEmailInfo> listToUpdate = new List<RecipientsEmailInfo>();
        //        Dictionary<long, IList<RecipientsEmailInfo>> BulkEmailRegList = comServ.GetRecipientsEmailListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
        //        AdmissionManagementService admserv = new AdmissionManagementService();
        //        if (cei.Attachment == true)
        //        {
        //            criteria.Clear();
        //            criteria.Add("PreRegNum", Convert.ToInt32(cei.Id));
        //            criteria.Add("AppName", "Student");
        //            Dictionary<long, IList<EmailAttachment>> emailattachment = ams.GetEmailAttachmentListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
        //            Attachment mailAttach = null;
        //            foreach (var item in emailattachment.FirstOrDefault().Value)
        //            {
        //                MemoryStream ms = new MemoryStream(item.Attachment);
        //                mailAttach = new Attachment(ms, item.AttachmentName.ToString());  //Data posted from form
        //                mail.Attachments.Add(mailAttach);
        //            }

        //        }

        //        if (cei != null)
        //        {
        //            mail.Body = cei.Message;
        //            mail.Subject = cei.Subject;
        //            mail.IsBodyHtml = true;
        //            SmtpClient smtp = new SmtpClient();
        //            smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address 
        //            smtp.Port = 25;
        //            //Or your Smtp Email ID and Password  
        //            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //            smtp.EnableSsl = true;
        //            if (!string.IsNullOrEmpty(cei.AlternativeEmailId)&& !string.IsNullOrEmpty(cei.AlternatPassword))
        //            {
        //                mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
        //                mail.To.Add(campusemaildet.First().EmailId.ToString());
        //                smtp.Credentials = new System.Net.NetworkCredential
        //               (cei.AlternativeEmailId, cei.AlternatPassword);
        //            }
        //            else
        //            {
        //                smtp.Host = campusemaildet.First().BulkEmailIdHost; //Or Your SMTP Server Address 
        //                smtp.Port = campusemaildet.First().BulkEmailIdPort;
        //                mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
        //                mail.To.Add(campusemaildet.First().EmailId.ToString());
        //                smtp.Credentials = new System.Net.NetworkCredential
        //                (campusemaildet.First().BulkEmailId.ToString(), campusemaildet.First().BulkEmailIdPassword.ToString());
        //            }
        //            IList<RecipientsEmailInfo> BulkEmailListExceptHotmail = new List<RecipientsEmailInfo>();
        //            IList<RecipientsEmailInfo> BulkEmailListHotmail = new List<RecipientsEmailInfo>();

        //            BulkEmailListHotmail = (from c in BulkEmailRegList.FirstOrDefault().Value
        //                                    where (c.U_EmailId != null && (c.U_EmailId.Contains("outlook.com") || c.U_EmailId.Contains("hotmail.com")))//c.U_EmailId.EndsWith("hotmail.com")
        //                                    select c).ToList();

        //            BulkEmailListExceptHotmail = BulkEmailRegList.FirstOrDefault().Value.Except(BulkEmailListHotmail).ToList();

        //            string Count = ConfigurationManager.AppSettings["RecipientsCountPerMail"];
        //            long totalcount = BulkEmailListExceptHotmail.Count;
        //            int i = 0; int noOfRec = Convert.ToInt32(Count);
        //            int inc = 0;
        //            foreach (var item in BulkEmailListExceptHotmail)
        //            {
        //                inc++;
        //                //logic for the number of recipients
        //                //read it from web config
        //                //if recipient is more than 1 then add in BCC
        //                item.Status = "True";
        //                //send email logic
        //                if (ValidEmailOrNot(item.U_EmailId))
        //                {
        //                    listToUpdate.Add(item);
        //                    mail.Bcc.Add(item.U_EmailId);
        //                    i++;
        //                    if (i == noOfRec)
        //                    {
        //                        i = 0;
        //                        bool result = SendEmailAndUpdateRecipientsEmailInfo(mail, listToUpdate, comServ, smtp, campusemaildet);
        //                        if (result == false)
        //                            bulkSendCompleteWithError = true;
        //                        mail.Bcc.Clear();
        //                        listToUpdate.Clear();
        //                    }
        //                    else if (inc == totalcount)
        //                    {
        //                        bool result = SendEmailAndUpdateRecipientsEmailInfo(mail, listToUpdate, comServ, smtp, campusemaildet);
        //                        if (result == false)
        //                            bulkSendCompleteWithError = true;
        //                        mail.Bcc.Clear();
        //                        listToUpdate.Clear();
        //                    }
        //                    else { }
        //                }
        //                else
        //                {
        //                    IList<RecipientsEmailInfo> WrongMailUpdate = new List<RecipientsEmailInfo>();
        //                    WrongMailUpdate.Add(item);
        //                    comServ.CreateOrUpdateRecipientsEmailListWithStatus(WrongMailUpdate, "InValid MailId");
        //                }
        //            }
        //            totalcount = BulkEmailListHotmail.Count;
        //            i = 0; inc = 0;
        //            foreach (var item in BulkEmailListHotmail)
        //            {
        //                inc++;
        //                //logic for the number of recipients
        //                //read it from web config
        //                //if recipient is more than 1 then add in BCC
        //                item.Status = "True";
        //                //send email logic
        //                if (ValidEmailOrNot(item.U_EmailId))
        //                {
        //                    listToUpdate.Add(item);
        //                    mail.Bcc.Add(item.U_EmailId);
        //                    i++;
        //                    if (i == noOfRec)
        //                    {
        //                        i = 0;
        //                        bool result = SendEmailAndUpdateRecipientsEmailInfoForHotmail(mail, listToUpdate, comServ, smtp, campusemaildet);
        //                        if (result == false)
        //                            bulkSendCompleteWithError = true;
        //                        mail.Bcc.Clear();
        //                        listToUpdate.Clear();
        //                    }
        //                    else if (inc == totalcount)
        //                    {
        //                        bool result = SendEmailAndUpdateRecipientsEmailInfoForHotmail(mail, listToUpdate, comServ, smtp, campusemaildet);
        //                        if (result == false)
        //                            bulkSendCompleteWithError = true;
        //                        mail.Bcc.Clear();
        //                        listToUpdate.Clear();
        //                    }
        //                    else { }
        //                }
        //                else
        //                {
        //                    IList<RecipientsEmailInfo> WrongMailUpdate = new List<RecipientsEmailInfo>();
        //                    WrongMailUpdate.Add(item);
        //                    comServ.CreateOrUpdateRecipientsEmailListWithStatus(WrongMailUpdate, "InValid MailId");
        //                }
        //            }

        //            //if the number of recipient is not dividable with the total recipients,
        //            //save the list here if listToUpdate is not null means
        //            if (listToUpdate != null && listToUpdate.Count > 0)
        //            {
        //                bool result = SendEmailAndUpdateRecipientsEmailInfo(mail, listToUpdate, comServ, smtp, campusemaildet);
        //                if (result == false)
        //                    bulkSendCompleteWithError = true;
        //                mail.Bcc.Clear();
        //                listToUpdate.Clear();
        //            }
        //            if (bulkSendCompleteWithError)
        //                cei.Status = "CompletedWithErrors";
        //            else cei.Status = "SuccessfullyCompleted";
        //            //save this
        //            comServ.CreateOrUpdateComposeEmailInfo(cei);
        //        }
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
        //        throw ex;
        //    }
        //}

        public ActionResult SendBulkEmailRequestWithAsync(long ComposeId, string Campus, bool IsAlterNativeMail)
        {
            try
            {
                //bool bulkSendCompleteWithError = false;
                AdmissionManagementService ams = new AdmissionManagementService();
                CommunicationService comServ = new CommunicationService();
                IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                ComposeEmailInfo cei = comServ.GetComposeMailInfoById(ComposeId);
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("ComposeId", ComposeId);
                IList<RecipientsEmailInfo> SentlistToUpdate = new List<RecipientsEmailInfo>();
                IList<RecipientsEmailInfo> NotSentlistToUpdate = new List<RecipientsEmailInfo>();
                Dictionary<long, IList<RecipientsEmailInfo>> BulkEmailRegList = comServ.GetRecipientsEmailListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                AdmissionManagementService admserv = new AdmissionManagementService();
                if (cei != null)
                {
                    foreach (var item in BulkEmailRegList.FirstOrDefault().Value)
                    {
                        //send email logic
                        if (ValidEmailOrNot(item.U_EmailId))
                        {
                            try
                            {
                                using (var client = new System.Net.Mail.SmtpClient(campusemaildet.FirstOrDefault().BulkEmailIdHost, campusemaildet.FirstOrDefault().BulkEmailIdPort))
                                {
                                    client.Credentials = new System.Net.NetworkCredential(campusemaildet.FirstOrDefault().BulkEmailId, campusemaildet.FirstOrDefault().BulkEmailIdPassword);
                                    client.EnableSsl = true;
                                    mail = new MailMessage(campusemaildet.FirstOrDefault().EmailId, item.U_EmailId, cei.Subject, cei.Message);
                                    if (cei.Attachment == true)
                                    {
                                        criteria.Clear();
                                        criteria.Add("PreRegNum", Convert.ToInt32(cei.Id));
                                        criteria.Add("AppName", "Student");
                                        Dictionary<long, IList<EmailAttachment>> emailattachment = ams.GetEmailAttachmentListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                                        Attachment mailAttach = null;
                                        foreach (var attItem in emailattachment.FirstOrDefault().Value)
                                        {
                                            MemoryStream ms = new MemoryStream(attItem.Attachment);
                                            mailAttach = new Attachment(ms, attItem.AttachmentName.ToString());  //Data posted from form
                                            mail.Attachments.Add(mailAttach);
                                        }
                                    }
                                    client.Send(mail);
                                }
                                SentlistToUpdate.Add(item);
                                comServ.CreateOrUpdateRecipientsEmailListWithStatus(SentlistToUpdate, "Sent");

                            }
                            catch (Exception ex)
                            {
                                NotSentlistToUpdate.Add(item);
                                comServ.CreateOrUpdateRecipientsEmailListWithStatus(NotSentlistToUpdate, "NotSent");
                            }
                        }
                    }

                    //if the number of recipient is not dividable with the total recipients,
                    //save the list here if listToUpdate is not null means
                    //if (listToUpdate != null && listToUpdate.Count > 0)
                    //{
                    //    bool result = SendEmailAndUpdateRecipientsEmailInfo(mail, listToUpdate, comServ, smtp, campusemaildet);
                    //    if (result == false)
                    //        bulkSendCompleteWithError = true;
                    //    mail.Bcc.Clear();
                    //    listToUpdate.Clear();
                    //}
                    if (NotSentlistToUpdate.Count > 0)
                        cei.Status = "CompletedWithErrors";
                    else cei.Status = "SuccessfullyCompleted";
                    //save this
                    comServ.CreateOrUpdateComposeEmailInfo(cei);
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }



        public ActionResult SendSelectEmailFunction(string Ids, long ComposeId, bool IsAlterNativeMail)
        {
            try
            {
                AdmissionManagementService ams = new AdmissionManagementService();
                CommunicationService comServ = new CommunicationService();
                SMS sms = new SMS(); bool bulkSendCompleteWithError = false;
                ComposeEmailInfo cei = comServ.GetComposeMailInfoById(ComposeId);
                IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(cei.Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                IList<RecipientsEmailInfo> listToUpdate = new List<RecipientsEmailInfo>();
                if (cei.Attachment == true)
                {
                    criteria.Clear();
                    criteria.Add("PreRegNum", Convert.ToInt32(cei.Id));
                    criteria.Add("AppName", "Student");
                    Dictionary<long, IList<EmailAttachment>> emailattachment = ams.GetEmailAttachmentListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                    Attachment mailAttach = null;
                    foreach (var item in emailattachment.FirstOrDefault().Value)
                    {
                        MemoryStream ms = new MemoryStream(item.Attachment);
                        mailAttach = new Attachment(ms, item.AttachmentName.ToString());  //Data posted from form
                        mail.Attachments.Add(mailAttach);
                    }

                }
                List<long> bulklongPReg = new List<long>();
                var bulkPreg = Ids.Split(',');
                foreach (var item in bulkPreg)
                {
                    bulklongPReg.Add(Convert.ToInt64(item));
                }
                long[] bulkarr = bulklongPReg.ToArray();
                criteria.Clear();
                criteria.Add("IdKeyValue", bulkarr);
                criteria.Add("ComposeId", ComposeId);
                Dictionary<long, IList<RecipientsEmailInfo>> BulkEmailRecipientsList = comServ.GetRecipientsEmailListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                if (cei != null)
                {
                    mail.Body = cei.Message;
                    mail.Subject = cei.Subject;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address 
                    smtp.Port = 25;
                    //Or your Smtp Email ID and Password  
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = true;
                    if (!string.IsNullOrEmpty(cei.AlternativeEmailId) && !string.IsNullOrEmpty(cei.AlternatPassword))
                    {
                        //smtp.Host = "172.16.17.253";
                        //smtp.Port = 587;
                        mail.From = new MailAddress(cei.AlternativeEmailId);
                        mail.To.Add(campusemaildet.First().EmailId.ToString());
                        smtp.Credentials = new System.Net.NetworkCredential
                       (cei.AlternativeEmailId, cei.AlternatPassword);
                    }
                    else
                    {
                        smtp.Host = campusemaildet.First().BulkEmailIdHost; //Or Your SMTP Server Address 
                        smtp.Port = campusemaildet.First().BulkEmailIdPort;
                        mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                        mail.To.Add(campusemaildet.First().EmailId.ToString());
                        smtp.Credentials = new System.Net.NetworkCredential
                       (campusemaildet.First().BulkEmailId.ToString(), campusemaildet.First().BulkEmailIdPassword.ToString());
                    }
                    IList<RecipientsEmailInfo> BulkEmailListExceptHotmail = new List<RecipientsEmailInfo>();
                    IList<RecipientsEmailInfo> BulkEmailListHotmail = new List<RecipientsEmailInfo>();

                    BulkEmailListHotmail = (from c in BulkEmailRecipientsList.FirstOrDefault().Value
                                            where (c.U_EmailId != null && (c.U_EmailId.Contains("outlook.com") || c.U_EmailId.Contains("hotmail.com")))//c.U_EmailId.EndsWith("hotmail.com")
                                            select c).ToList();
                    BulkEmailListExceptHotmail = BulkEmailRecipientsList.FirstOrDefault().Value.Except(BulkEmailListHotmail).ToList();
                    string Count = ConfigurationManager.AppSettings["RecipientsCountPerMail"].ToString();
                    long totalcount = BulkEmailListExceptHotmail.Count;
                    int i = 0; int noOfRec = Convert.ToInt32(Count);
                    int inc = 0;
                    foreach (var item in BulkEmailListExceptHotmail)
                    {
                        inc++;
                        //logic for the number of recipients
                        //read it from web config
                        item.Status = "True";
                        //send email logic
                        if (ValidEmailOrNot(item.U_EmailId))
                        {
                            listToUpdate.Add(item);
                            mail.Bcc.Add(item.U_EmailId);
                            i++;
                            if (i == noOfRec || totalcount < 10)
                            {
                                i = 0;
                                bool result = SendEmailAndUpdateRecipientsEmailInfo(mail, listToUpdate, comServ, smtp, campusemaildet);
                                if (result == false)
                                    bulkSendCompleteWithError = true;
                                mail.Bcc.Clear();
                                listToUpdate.Clear();
                            }
                            else if (inc == totalcount)
                            {
                                bool result = SendEmailAndUpdateRecipientsEmailInfo(mail, listToUpdate, comServ, smtp, campusemaildet);
                                if (result == false)
                                    bulkSendCompleteWithError = true;
                                mail.Bcc.Clear();
                                listToUpdate.Clear();
                            }
                            else { }
                        }
                        else
                        {
                            IList<RecipientsEmailInfo> WrongMailUpdate = new List<RecipientsEmailInfo>();
                            WrongMailUpdate.Add(item);
                            comServ.CreateOrUpdateRecipientsEmailListWithStatus(WrongMailUpdate, "InValid MailId");
                        }
                    }
                    totalcount = BulkEmailListHotmail.Count;
                    i = 0; inc = 0;
                    foreach (var item in BulkEmailListHotmail)
                    {
                        inc++;
                        //logic for the number of recipients
                        //read it from web config
                        item.Status = "True";
                        //send email logic
                        if (ValidEmailOrNot(item.U_EmailId))
                        {
                            listToUpdate.Add(item);
                            mail.Bcc.Add(item.U_EmailId);
                            i++;
                            if (i == noOfRec || totalcount < 10)
                            {
                                i = 0;
                                bool result = SendEmailAndUpdateRecipientsEmailInfoForHotmail(mail, listToUpdate, comServ, smtp, campusemaildet);
                                if (result == false)
                                    bulkSendCompleteWithError = true;
                                mail.Bcc.Clear();
                                listToUpdate.Clear();
                            }
                            else if (inc == totalcount)
                            {
                                bool result = SendEmailAndUpdateRecipientsEmailInfoForHotmail(mail, listToUpdate, comServ, smtp, campusemaildet);
                                if (result == false)
                                    bulkSendCompleteWithError = true;
                                mail.Bcc.Clear();
                                listToUpdate.Clear();
                            }
                            else { }
                        }
                        else
                        {
                            IList<RecipientsEmailInfo> WrongMailUpdate = new List<RecipientsEmailInfo>();
                            WrongMailUpdate.Add(item);
                            comServ.CreateOrUpdateRecipientsEmailListWithStatus(WrongMailUpdate, "InValid MailId");
                        }
                    }
                    /// Send mail to who are all having Academic Role
                    // SentMailToAcaDir(cei);
                    /// End
                    if (bulkSendCompleteWithError)
                        cei.Status = "CompletedWithErrors";
                    else cei.Status = "SuccessfullyCompleted";
                    criteria.Clear();
                    criteria.Add("ComposeId", ComposeId);
                    Dictionary<long, IList<RecipientsEmailInfo>> checkSentMails = comServ.GetRecipientsEmailListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                    if (checkSentMails != null && checkSentMails.FirstOrDefault().Value.Count > 0 && checkSentMails.FirstOrDefault().Key > 0)
                    {
                        foreach (var item in checkSentMails.FirstOrDefault().Value)
                        {
                            if (item.Status != "Sent")
                            {
                                if (item.Status != "InValid MailId") { cei.Status = "PartiallyCompleted"; break; }
                            }
                        }
                    }
                    //save this
                    cei.StudentPortal = false;
                    cei.ParentPortal = false;
                    cei.ExpiryDate = null;
                    comServ.CreateOrUpdateComposeEmailInfo(cei);
                }
                return null;
            }

            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult SendFalseEmailFunction(long ComposeId, bool IsAlterNativeMail)
        {
            try
            {
                AdmissionManagementService ams = new AdmissionManagementService();
                CommunicationService comServ = new CommunicationService();
                SMS sms = new SMS(); bool bulkSendCompleteWithError = false;
                ComposeEmailInfo cei = comServ.GetComposeMailInfoById(ComposeId);
                IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(cei.Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                IList<RecipientsEmailInfo> listToUpdate = new List<RecipientsEmailInfo>();
                if (cei.Attachment == true)
                {
                    criteria.Clear();
                    criteria.Add("PreRegNum", Convert.ToInt32(cei.Id));
                    criteria.Add("AppName", "Student");
                    Dictionary<long, IList<EmailAttachment>> emailattachment = ams.GetEmailAttachmentListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                    Attachment mailAttach = null;
                    foreach (var item in emailattachment.FirstOrDefault().Value)
                    {
                        MemoryStream ms = new MemoryStream(item.Attachment);
                        mailAttach = new Attachment(ms, item.AttachmentName.ToString());  //Data posted from form
                        mail.Attachments.Add(mailAttach);
                    }

                }
                criteria.Clear();
                criteria.Add("ComposeId", ComposeId);
                criteria.Add("Status", "Not Sent");
                Dictionary<long, IList<RecipientsEmailInfo>> BulkEmailRecipientsList = comServ.GetRecipientsEmailListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                if (cei != null)
                {
                    mail.Body = cei.Message;
                    mail.Subject = cei.Subject;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address 
                    smtp.Port = 25;
                    //Or your Smtp Email ID and Password  
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = true;
                    if (!string.IsNullOrEmpty(cei.AlternativeEmailId) && !string.IsNullOrEmpty(cei.AlternatPassword))
                    {
                        //smtp.Host = "172.16.17.253";
                        //smtp.Port = 587;
                        mail.From = new MailAddress(cei.AlternativeEmailId);
                        mail.To.Add(campusemaildet.First().EmailId.ToString());
                        smtp.Credentials = new System.Net.NetworkCredential
                       (cei.AlternativeEmailId, cei.AlternatPassword);
                    }
                    else
                    {
                        smtp.Host = campusemaildet.First().BulkEmailIdHost; //Or Your SMTP Server Address 
                        smtp.Port = campusemaildet.First().BulkEmailIdPort;
                        mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                        mail.To.Add(campusemaildet.First().EmailId.ToString());
                        smtp.Credentials = new System.Net.NetworkCredential
                       (campusemaildet.First().BulkEmailId.ToString(), campusemaildet.First().BulkEmailIdPassword.ToString());
                    }

                    IList<RecipientsEmailInfo> BulkEmailListExceptHotmail = new List<RecipientsEmailInfo>();
                    IList<RecipientsEmailInfo> BulkEmailListHotmail = new List<RecipientsEmailInfo>();

                    BulkEmailListHotmail = (from c in BulkEmailRecipientsList.FirstOrDefault().Value
                                            where (c.U_EmailId != null && (c.U_EmailId.Contains("outlook.com") || c.U_EmailId.Contains("hotmail.com")))//c.U_EmailId.EndsWith("hotmail.com")
                                            select c).ToList();
                    BulkEmailListExceptHotmail = BulkEmailRecipientsList.FirstOrDefault().Value.Except(BulkEmailListHotmail).ToList();

                    string Count = ConfigurationManager.AppSettings["RecipientsCountPerMail"].ToString();
                    long totalcount = BulkEmailListExceptHotmail.Count;
                    int i = 0; int noOfRec = Convert.ToInt32(Count);
                    int inc = 0;
                    foreach (var item in BulkEmailListExceptHotmail)
                    {
                        //logic for the number of recipients
                        //read it from web config
                        item.Status = "True";
                        inc++;
                        //send email logic
                        if (ValidEmailOrNot(item.U_EmailId))
                        {
                            mail.Bcc.Add(item.U_EmailId);
                            listToUpdate.Add(item);
                            if (i == noOfRec || totalcount < 10)
                            {
                                i = 0;
                                bool result = SendEmailAndUpdateRecipientsEmailInfo(mail, listToUpdate, comServ, smtp, campusemaildet);
                                if (result == false)
                                    bulkSendCompleteWithError = true;
                                mail.Bcc.Clear();
                                listToUpdate.Clear();
                            }
                            else if (inc == totalcount)
                            {
                                bool result = SendEmailAndUpdateRecipientsEmailInfo(mail, listToUpdate, comServ, smtp, campusemaildet);
                                if (result == false)
                                    bulkSendCompleteWithError = true;
                                mail.Bcc.Clear();
                                listToUpdate.Clear();
                            }
                            else { }
                        }
                        else
                        {
                            IList<RecipientsEmailInfo> WrongMailUpdate = new List<RecipientsEmailInfo>();
                            WrongMailUpdate.Add(item);
                            comServ.CreateOrUpdateRecipientsEmailListWithStatus(WrongMailUpdate, "InValid MailId");
                        }
                    }
                    totalcount = BulkEmailListHotmail.Count;
                    i = 0; inc = 0;
                    foreach (var item in BulkEmailListExceptHotmail)
                    {
                        //logic for the number of recipients
                        //read it from web config
                        item.Status = "True";
                        inc++;
                        //send email logic
                        if (ValidEmailOrNot(item.U_EmailId))
                        {
                            mail.Bcc.Add(item.U_EmailId);
                            listToUpdate.Add(item);
                            if (i == noOfRec || totalcount < 10)
                            {
                                i = 0;
                                bool result = SendEmailAndUpdateRecipientsEmailInfoForHotmail(mail, listToUpdate, comServ, smtp, campusemaildet);
                                if (result == false)
                                    bulkSendCompleteWithError = true;
                                mail.Bcc.Clear();
                                listToUpdate.Clear();
                            }
                            else if (inc == totalcount)
                            {
                                bool result = SendEmailAndUpdateRecipientsEmailInfoForHotmail(mail, listToUpdate, comServ, smtp, campusemaildet);
                                if (result == false)
                                    bulkSendCompleteWithError = true;
                                mail.Bcc.Clear();
                                listToUpdate.Clear();
                            }
                            else { }
                        }
                        else
                        {
                            IList<RecipientsEmailInfo> WrongMailUpdate = new List<RecipientsEmailInfo>();
                            WrongMailUpdate.Add(item);
                            comServ.CreateOrUpdateRecipientsEmailListWithStatus(WrongMailUpdate, "InValid MailId");
                        }
                    }
                    /// Send mail to who are all having Academic Role
                    // SentMailToAcaDir(cei);
                    /// End
                    if (bulkSendCompleteWithError)
                        cei.Status = "CompletedWithErrors";
                    else cei.Status = "SuccessfullyCompleted";
                    criteria.Clear();
                    criteria.Add("ComposeId", ComposeId);
                    Dictionary<long, IList<RecipientsEmailInfo>> checkSentMails = comServ.GetRecipientsEmailListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                    if (checkSentMails != null && checkSentMails.FirstOrDefault().Value.Count > 0 && checkSentMails.FirstOrDefault().Key > 0)
                    {
                        foreach (var item in checkSentMails.FirstOrDefault().Value)
                        {
                            if (item.Status == "Not Sent" || item.Status == "InProgress") { cei.Status = "PartiallyCompleted"; break; }
                        }
                    }
                    //save this
                    comServ.CreateOrUpdateComposeEmailInfo(cei);
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult ReciptAddingFunction(RecipientsEmailInfo rei, long ComposeId)
        {
            CommunicationService comServ = new CommunicationService();

            RecipientsEmailInfo getlist = comServ.GetRecipientlistById(ComposeId, rei.IdKeyValue);
            getlist.ComposeId = ComposeId;
            getlist.U_EmailId = rei.U_EmailId;
            //getlist.FatherEmailId = rei.FatherEmailId;
            //getlist.MotherEmailId = rei.MotherEmailId;
            //getlist.GeneralEmailId = rei.GeneralEmailId;
            getlist.RecipientsModifiedDate = DateTime.Now;
            comServ.CreateOrUpdateRecipients(getlist);

            return null;
        }

        public ActionResult ReciptDelete(string[] Id, long ComposeId)
        {
            CommunicationService comServ = new CommunicationService();
            var bulkPreregnum = Id[0].Split(',');
            List<long> bulkIdKeyValue = new List<long>();
            foreach (var item in bulkPreregnum) { bulkIdKeyValue.Add(Convert.ToInt64(item)); }

            comServ.DeleteRecipientsList(bulkIdKeyValue, ComposeId);
            return null;
        }

        public JsonResult ViewBulkMailAttachments(long AttRefId, string AppName)
        {
            CommunicationService cmsvc = new CommunicationService();
            IList<EmailAttachment> dList = cmsvc.GetDocumentListByIdandApp(AttRefId, AppName);
            if (dList.Count > 0)
            {
                string[] attId = (from items in dList select items.Id.ToString()).ToArray();
                string[] attName = (from items in dList select items.AttachmentName.ToString()).ToArray();
                long atlen = attId.Count();
                string[] mailAtt = new string[dList.Count];
                string totNoteAtt = "<br><div>";
                for (var j = 0; j < atlen; j++)
                {
                    mailAtt[j] += "<a href='#' onclick = 'uploaddat(" + attId[j] + ")' style='color:black;text-decoration:underline;' >" + attName[j] + "</a>&nbsp;&nbsp; <a href='#' onclick='delatt(" + attId[j] + ")' style='color:red; text-decoration:underline;' ><i class='fa fa-times'></i></a><br>";
                    totNoteAtt += mailAtt[j];
                }
                totNoteAtt += "</div>";
                return Json(totNoteAtt, JsonRequestBehavior.AllowGet);
            }
            else
            {
                CommunicationService comSer = new CommunicationService();
                ComposeEmailInfo cei = new ComposeEmailInfo();
                cei = comSer.GetComposeEmailInfoById(Convert.ToInt64(AttRefId));
                cei.Attachment = false;
                comSer.CreateOrUpdateComposeEmailInfo(cei);
                return Json("None", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DeleteEmailAtt(long AttId)
        {
            try
            {
                CommunicationService cmsvc = new CommunicationService();
                long deleteid = Convert.ToInt64(AttId);
                if (deleteid != 0 && deleteid > 0)
                {
                    cmsvc.DeleteEmailAttachment(deleteid);
                }


                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult SendTestMail(string MailId, long MailComposeId)
        {
            if (ValidEmailOrNot(MailId) && !string.IsNullOrEmpty(MailId) && MailComposeId > 0)
            {
                try
                {
                    CommunicationService comSer = new CommunicationService();
                    ComposeEmailInfo comEmail = comSer.GetComposeMailInfoById(MailComposeId);
                    if (comEmail != null)
                    {
                        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                        Dictionary<string, object> criteria = new Dictionary<string, object>();
                        if (comEmail.Attachment == true)
                        {
                            AdmissionManagementService ams = new AdmissionManagementService();
                            criteria.Clear();
                            criteria.Add("PreRegNum", Convert.ToInt32(comEmail.Id));
                            Dictionary<long, IList<EmailAttachment>> emailattachment = ams.GetEmailAttachmentListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);
                            Attachment mailAttach = null;
                            foreach (var item in emailattachment.FirstOrDefault().Value)
                            {
                                MemoryStream ms = new MemoryStream(item.Attachment);
                                mailAttach = new Attachment(ms, item.AttachmentName.ToString());  //Data posted from form
                                mail.Attachments.Add(mailAttach);
                            }
                        }
                        UserService us = new UserService();
                        User uscls = us.GetUserByUserId(comEmail.UserId);
                        IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(uscls.Campus == null ? "" : uscls.Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                        mail.Body = comEmail.Message;
                        mail.Subject = comEmail.Subject;
                        mail.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient("localhost", 25);
                        //Or your Smtp Email ID and Password  
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.EnableSsl = true;
                        smtp.Host = campusemaildet.First().BulkEmailIdHost; //Or Your SMTP Server Address 
                        smtp.Port = campusemaildet.First().BulkEmailIdPort;
                        mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                        mail.To.Add(MailId);
                        smtp.Credentials = new System.Net.NetworkCredential
                       (campusemaildet.First().BulkEmailId.ToString(), campusemaildet.First().BulkEmailIdPassword.ToString());
                        ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                        {
                            return true;
                        };
                        smtp.Send(mail);
                        return Json("Successfully Test Mail Sent", JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    return Json(ex.Message, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Invalid Mail Address", JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        #region Google

        public JsonResult GoogleAuthEmailValidation(string EmailId, string Password)
        {

            try
            {
                if (EmailId != "" || Password != "")
                {
                    RequestSettings rsLoginInfo = new RequestSettings("", EmailId, Password);
                    rsLoginInfo.AutoPaging = true;
                    ContactsRequest cr = new ContactsRequest(rsLoginInfo);
                    Feed<Contact> f = cr.GetContacts();
                    if (f.Entries != null)
                    {
                        foreach (Contact e in f.Entries)
                        {
                            foreach (EMail email in e.Emails)
                            {
                                var emailadd = email.Address;
                            }
                            break;
                        }
                    }
                }
            }
            catch (InvalidCredentialsException)
            {
                return Json("Invalid", JsonRequestBehavior.AllowGet);
            }
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion end

        #region BulkSmslog Added By Micheal

        public ActionResult SMSSendingLog()
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
        public ActionResult JqGridSMSRequest(string BulkReqId, string SMSTemplate, string Status, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                sord = sord == "desc" ? "Desc" : "Asc";
                CommunicationService comSer = new CommunicationService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(BulkReqId)) { criteria.Add("SMSReqId", BulkReqId); }
                if (!string.IsNullOrEmpty(SMSTemplate)) { criteria.Add("SMSTemplate", SMSTemplate); }
                if (!string.IsNullOrEmpty(Status)) { criteria.Add("Status", Status); }
                //var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                //if (usrcmp != null && usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
                //{
                //    criteria.Add("Campus", usrcmp);
                //}
                if (Session["UserId"].ToString() != null)
                    criteria.Add("CreatedBy", Session["UserId"]);
                Dictionary<long, IList<ComposeSMSInfo>> BulkSMSRegList = comSer.GetSMSInfoListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (BulkSMSRegList != null && BulkSMSRegList.Count > 0 && BulkSMSRegList.FirstOrDefault().Value != null)
                {
                    long totalrecords = BulkSMSRegList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)5);
                    var AssLst = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (
                             from items in BulkSMSRegList.First().Value

                             select new
                             {
                                 cell = new string[] 
                                         {
                                             items.Id.ToString(),
                                             "<a href='/Communication/BulkSMSRequest?Id=" + items.Id+"'>" + items.SMSReqId + "</a>",
                                             items.Campus,
                                             items.SMSTemplate,
                                             items.SMSTemplateValue,
                                             items.Message,
                                             items.Father.ToString(),
                                             items.Mother.ToString(),
                                             items.Status,
                                             items.CreatedBy,
                                             items.ModifiedBy,
                                             items.CreatedDate.ToString("dd'/'MM'/'yyyy"),
                                             items.ModifiedDate.ToString("dd'/'MM'/'yyyy")
                                         }
                             }).ToList()
                    };
                    return Json(AssLst, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult BulkSMSRequest(long? Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    CommunicationService comSer = new CommunicationService();
                    MastersService ms = new MastersService();
                    AdmissionManagementService ads = new AdmissionManagementService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<SMSTemplate>> smstemplate = ads.GetSMSTemplateListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                    ViewBag.smstemplate = smstemplate.First().Value;
                    criteria.Clear();
                    Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    ViewBag.acadddl = AcademicyrMaster.First().Value;
                    ComposeSMSInfo smsInfo = new ComposeSMSInfo();

                    if (Id > 0)
                    {
                        smsInfo = comSer.GetComposeSMSInfoById(Id ?? 0);
                    }
                    else { }
                    ViewBag.Message = smsInfo.Message;
                    return View(smsInfo);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult BulkSMSRequest(ComposeSMSInfo sms)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    CommunicationService comSer = new CommunicationService();
                    if (sms.Id == 0)
                    {
                        sms.CreatedDate = DateTime.Now;
                    }
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    if (Request.Form["FromDate"] != "" && Request.Form["ToDate"] != "")
                    {
                        sms.FromDate = DateTime.Parse(Request.Form["FromDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        sms.ToDate = DateTime.Parse(Request.Form["ToDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    }
                    else if (Request.Form["TemplateDate"] != "")
                    {
                        sms.TemplateDate = DateTime.Parse(Request.Form["TemplateDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    }
                    else
                    {
                    }
                    sms.ModifiedDate = DateTime.Now;
                    sms.CreatedBy = userId;
                    long Id = comSer.CreateOrUpdateComposeSMSInfo(sms);
                    sms.SMSReqId = "SMSR-" + Id;
                    sms.Status = "SMS Composed";
                    comSer.CreateOrUpdateComposeSMSInfo(sms);
                    return RedirectToAction("BulkSMSRequest", new { Id = sms.Id });
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult BulkSMSRequestJqGrid(string AdStatus, string NewId, string StIshostel, string AcYear, long SMSComposeId, string Campus, string Name, string Status, string Grade, string VanNo, string saveOrClear, string Section, string AppliedFrmDate, string AppliedToDate, int rows, string sord, string sidx, int? page = 1)
        {
            try
            {
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                CommunicationService comSer = new CommunicationService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<BulkSMSRequest_vw>> BulkSMSReqList = null;
                Dictionary<long, IList<SMSRecipientsInfo>> BulkSMSRecList = null;
                ComposeSMSInfo smsInfo = new ComposeSMSInfo();
                if (SMSComposeId > 0) { smsInfo = comSer.GetComposeSMSInfoById(SMSComposeId); }
                if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                if (!string.IsNullOrEmpty(AdStatus)) { criteria.Add("AdmissionStatus", AdStatus); }
                else criteria.Add("AdmissionStatus", "Registered");
                if (!string.IsNullOrEmpty(Status)) { criteria.Add("Status", Status); }
                if (!string.IsNullOrEmpty(NewId)) { criteria.Add("NewId", NewId); }
                if (!string.IsNullOrEmpty(StIshostel)) { criteria.Add("IsHosteller", Convert.ToBoolean(StIshostel)); }
                if (!string.IsNullOrEmpty(AcYear)) { criteria.Add("AcademicYear", AcYear); }
                if (!string.IsNullOrEmpty(Name)) { criteria.Add("Name", Name); }
                if (!string.IsNullOrEmpty(VanNo)) { criteria.Add("VanNo", VanNo); }
                if (Grade != "null" && Grade != null)
                {
                    string[] Gradarr = Grade.Split(',');
                    criteria.Add("Grade", Gradarr);
                }
                if (Section != null && Section != "null")
                {
                    string[] sectionarr = Section.Split(',');
                    criteria.Add("Section", sectionarr);
                }
                if ((!string.IsNullOrEmpty(AppliedFrmDate)) && (!string.IsNullOrEmpty(AppliedToDate)))
                {
                    if (!string.IsNullOrEmpty(AppliedFrmDate) && !string.IsNullOrEmpty(AppliedToDate))
                    {
                        AppliedFrmDate = AppliedFrmDate.Trim();
                        AppliedToDate = AppliedToDate.Trim();
                        //string To = string.Format("{0:MM/dd/yyyy}", DateTime.Now);
                        DateTime[] fromto = new DateTime[2];
                        fromto[0] = DateTime.Parse(AppliedFrmDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        fromto[1] = DateTime.Parse(AppliedToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);// Convert.ToDateTime(To + " " + "23:59:59");
                        criteria.Add("CreatedDateNew", fromto);
                    }
                }
                sord = sord == "desc" ? "Desc" : "Asc";
                if (SMSComposeId > 0 && smsInfo.IsSaveList)
                {
                    criteria.Clear();
                    criteria.Add("SMSComposeId", SMSComposeId);
                    rows = saveOrClear == "Clear" ? 99999 : rows;
                    if (!string.IsNullOrEmpty(Name))
                        BulkSMSRecList = comSer.GetBulkSMSRecipLikeSearchCriteria(page - 1, rows, sidx, sord, criteria);
                    else
                        BulkSMSRecList = comSer.GetBulkSMSRecipListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                }
                else
                {
                    if (!string.IsNullOrEmpty(Campus))
                    {
                        List<string> FamilyType = new List<string>();
                        if (smsInfo.Father == true)
                            FamilyType.Add("Father");
                        if (smsInfo.Mother == true)
                            FamilyType.Add("Mother");
                        if (FamilyType[0] != "") { criteria.Add("FamilyDetailType", FamilyType.ToArray()); }
                        BulkSMSReqList = comSer.GetBulkSMSReqListWithPagingAndCriteria(page - 1, 99999, sidx, sord, criteria);
                    }
                }
                if (saveOrClear == "Save")
                {
                    List<SMSRecipientsInfo> SMSReciplist = new List<SMSRecipientsInfo>();
                    if (BulkSMSReqList != null)
                    {
                        foreach (var item in BulkSMSReqList.FirstOrDefault().Value)
                        {
                            SMSRecipientsInfo SMSRei = new SMSRecipientsInfo();
                            SMSRei.SMSComposeId = Convert.ToInt64(SMSComposeId);
                            SMSRei.IdKeyValue = item.IdKeyValue;
                            SMSRei.PreRegNum = item.PreRegNum;
                            SMSRei.Campus = item.Campus;
                            SMSRei.Name = item.Name;
                            SMSRei.VanNo = item.VanNo;
                            SMSRei.NewId = item.NewId;
                            SMSRei.AcademicYear = item.AcademicYear;
                            SMSRei.IsHosteller = item.IsHosteller;
                            SMSRei.FamilyDetailType = item.FamilyDetailType;
                            SMSRei.AdmissionStatus = item.AdmissionStatus;
                            SMSRei.RecipientsCreatedDate = DateTime.Now;
                            SMSRei.RecipientsModifiedDate = DateTime.Now;
                            SMSRei.Grade = item.Grade;
                            SMSRei.Section = item.Section;
                            SMSRei.AppliedDate = item.CreatedDateNew;
                            if (!string.IsNullOrEmpty(item.MobileNumber))
                            {
                                item.MobileNumber.Replace(" ", string.Empty);
                                if (item.MobileNumber.Contains(","))
                                {
                                    var MNumber = item.MobileNumber.Split(',');
                                    for (int i = 0; i < MNumber.Length; i++)
                                    {
                                        SMSRecipientsInfo SMSReiSepComma = new SMSRecipientsInfo();
                                        SMSReiSepComma.SMSComposeId = Convert.ToInt64(SMSComposeId);
                                        SMSReiSepComma.IdKeyValue = item.IdKeyValue;
                                        SMSReiSepComma.PreRegNum = item.PreRegNum;
                                        SMSReiSepComma.Campus = item.Campus;
                                        SMSReiSepComma.Name = item.Name;
                                        SMSReiSepComma.NewId = item.NewId;
                                        SMSReiSepComma.AcademicYear = item.AcademicYear;
                                        SMSReiSepComma.VanNo = item.VanNo;
                                        SMSReiSepComma.IsHosteller = item.IsHosteller;
                                        SMSReiSepComma.FamilyDetailType = item.FamilyDetailType;
                                        SMSReiSepComma.AdmissionStatus = item.AdmissionStatus;
                                        SMSReiSepComma.RecipientsCreatedDate = DateTime.Now;
                                        SMSReiSepComma.RecipientsModifiedDate = DateTime.Now;
                                        SMSReiSepComma.Grade = item.Grade;
                                        SMSReiSepComma.Section = item.Section;
                                        SMSReiSepComma.MobileNumber = MNumber[i].Trim();
                                        SMSReiSepComma.Status = "InProgress";
                                        SMSReciplist.Add(SMSReiSepComma);
                                    }
                                }
                                else
                                {
                                    SMSRei.MobileNumber = item.MobileNumber.Trim();
                                    SMSRei.Status = "InProgress";
                                    SMSReciplist.Add(SMSRei);
                                }
                            }
                            else
                            {
                                SMSRei.MobileNumber = item.MobileNumber;
                                SMSRei.Status = "No Number";
                                SMSReciplist.Add(SMSRei);
                            }

                        }
                        comSer.CreateOrUpdateSMSRecipientsList(SMSReciplist);
                        foreach (BulkSMSRequest_vw item in BulkSMSReqList.FirstOrDefault().Value)
                        {
                            item.Status = !string.IsNullOrEmpty(item.MobileNumber) ? "InProgress" : "No Number";
                            item.SMSRecipientCreatedDate = DateTime.Now;
                            item.SMSRecipientCreatedDate = DateTime.Now;
                        }

                        /// Save to the ComposeEmailInfo table
                        smsInfo.IsSaveList = true;
                        //smsInfo.Status = "Recipients Added";

                        if (SMSReciplist[0] != null)
                        {
                            if (AcYear != "" && !string.IsNullOrEmpty(AcYear))
                                smsInfo.AcademicYear = AcYear;
                            smsInfo.AdmissionStatus = AdStatus;
                            if (Name != "" && !string.IsNullOrEmpty(Name))
                                smsInfo.StudentName = Name;
                            if (NewId != "" && !string.IsNullOrEmpty(NewId))
                                smsInfo.NewId = NewId;
                            smsInfo.IsHosteller = StIshostel;
                            smsInfo.Campus = Campus;
                            smsInfo.Status = "Recipients Added";
                            smsInfo.Grade = Grade;
                            smsInfo.Section = Section;
                            if (AppliedFrmDate != "" && !string.IsNullOrEmpty(AppliedFrmDate))
                                smsInfo.AppliedFromDate = DateTime.Parse(AppliedFrmDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            if (AppliedToDate != "" && !string.IsNullOrEmpty(AppliedToDate))
                                smsInfo.AppliedToDate = DateTime.Parse(AppliedToDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        }
                        comSer.CreateOrUpdateComposeSMSInfo(smsInfo);
                    }
                    if (SMSComposeId > 0 && smsInfo.IsSaveList)
                    {
                        criteria.Clear();
                        criteria.Add("SMSComposeId", SMSComposeId);
                        rows = saveOrClear == "Clear" ? 99999 : rows;
                        if (!string.IsNullOrEmpty(Name))
                            BulkSMSRecList = comSer.GetBulkSMSRecipLikeSearchCriteria(page - 1, rows, sidx, sord, criteria);
                        else
                            BulkSMSRecList = comSer.GetBulkSMSRecipListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    }
                }
                else if (saveOrClear == "Suspend")
                {
                    smsInfo.Suspended = true;
                    smsInfo.ModifiedDate = DateTime.Now;
                    smsInfo.Status = "Suspended";
                    comSer.CreateOrUpdateComposeSMSInfo(smsInfo);
                }
                else
                {
                    if (saveOrClear == "Clear")
                    {
                        comSer.ClearSavedListInSMSRecipientsInfo(Convert.ToInt64(SMSComposeId));
                        smsInfo.IsSaveList = false;
                        smsInfo.Campus = null;
                        smsInfo.Section = null;
                        smsInfo.Grade = null;
                        smsInfo.StudentName = null;
                        smsInfo.AcademicYear = null;
                        smsInfo.AdmissionStatus = null;
                        smsInfo.VanNo = null;
                        smsInfo.Status = "SMS Composed";
                        smsInfo.AppliedFromDate = null;
                        smsInfo.AppliedToDate = null;
                        comSer.CreateOrUpdateComposeSMSInfo(smsInfo);
                        var jsondat = new { rows = (new { cell = new string[] { } }) };
                        return Json(jsondat, JsonRequestBehavior.AllowGet);
                    }
                }
                if (BulkSMSReqList != null && BulkSMSReqList.FirstOrDefault().Value.Count > 0 && BulkSMSReqList.FirstOrDefault().Key > 0)
                {
                    long totalrecords = BulkSMSReqList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in BulkSMSReqList.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                                        items.Id.ToString(),
                                        items.IdKeyValue.ToString(),
                                        items.PreRegNum.ToString(),
                                        items.NewId,
                                        items.Name,
                                        items.Campus,
                                        items.Grade,
                                        items.Section,
                                        items.IsHosteller.ToString(),
                                        items.AcademicYear,
                                        items.AdmissionStatus,
                                        items.VanNo,
                                        items.Status=="InProgress"? String.Format(@"<p style='color:#0000FF'>{0}</p>",items.Status)
                                        :items.Status=="InValid Number"? String.Format(@"<p style='color:#FF0000'>{0}</p>",items.Status):
                                        items.Status=="Success"? String.Format(@"<p style='color:#00FF00'>{0}</p>",items.Status):items.Status,
                                        items.FamilyDetailType,
                                        items.MobileNumber,
                                        items.CreatedDateNew.ToString("dd'/'MM'/'yyyy"),
                                        items.SMSRecipientCreatedDate!=null? ConvertDateTimeToDate(items.SMSRecipientCreatedDate.ToString("dd'/'MM'/'yyyy"),"en-GB"):"",
                                        items.SMSRecipientModifiedDate!=null? ConvertDateTimeToDate(items.SMSRecipientModifiedDate.ToString("dd'/'MM'/'yyyy"),"en-GB"):"",
                                        items.SentSMSStatusWithTid,
                                        items.SentSMSReportsWithStatus
                                        //items.SMSRecipientCreatedDate != null?items.SMSRecipientCreatedDate.ToString():"",
                                        //items.SMSRecipientModifiedDate != null?items.SMSRecipientModifiedDate.ToString():"",
                            }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (BulkSMSRecList != null && BulkSMSRecList.FirstOrDefault().Value.Count > 0 && BulkSMSRecList.FirstOrDefault().Key > 0)
                    {
                        long totalrecords = BulkSMSRecList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in BulkSMSRecList.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                                        items.Id.ToString(),
                                        items.IdKeyValue.ToString(),
                                        items.PreRegNum.ToString(),
                                        items.NewId,
                                        items.Name,
                                        items.Campus,
                                        items.Grade,
                                        items.Section,
                                        items.IsHosteller.ToString(),
                                        items.AcademicYear,
                                        items.AdmissionStatus,
                                        items.VanNo,
                                        items.Status=="InProgress"? String.Format(@"<p style='color:#0000FF'>{0}</p>",items.Status)
                                        :items.Status=="Number wrong"? String.Format(@"<p style='color:#FF0000'>{0}</p>",items.Status):
                                        items.Status=="Success"? String.Format(@"<p style='color:#00FF00'>{0}</p>",items.Status):items.Status,
                                        items.FamilyDetailType,
                                        items.MobileNumber,
                                        items.AppliedDate.ToString("dd'/'MM'/'yyyy"),
                                        //items.FatherMobile_NO,
                                        //items.MotherMobile_NO,
                                        items.RecipientsCreatedDate != null?items.RecipientsCreatedDate.ToString():"",
                                        items.RecipientsModifiedDate != null?items.RecipientsModifiedDate.ToString():"",
                                        items.SentSMSStatusWithTid,
                                        items.SentSMSReportsWithStatus
                                    }
                                    })
                        };
                        return Json(jsondat, JsonRequestBehavior.AllowGet);
                    }
                }
                var jsonda = new { rows = (new { cell = new string[] { } }) };
                return Json(jsonda, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult SendBulkSMS(long SMSComposeId)
        {
            try
            {
                if (SMSComposeId > 0)
                {
                    ComposeSMSInfo smsInfo = new ComposeSMSInfo();
                    CommunicationService comSer = new CommunicationService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("SMSComposeId", SMSComposeId);
                    Dictionary<long, IList<SMSRecipientsInfo>> BulkSMSRecipientList = comSer.GetBulkSMSRecipListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                    List<SMSRecipientsInfo> BulkSMSRecipientInfo = BulkSMSRecipientList.FirstOrDefault().Value.ToList();
                    smsInfo = comSer.GetComposeSMSInfoById(SMSComposeId);
                    // for sending single sms.....................
                    SendBulkSMSInSingleWay(BulkSMSRecipientInfo, smsInfo);
                    //getting SMS Status Report from Portal 
                    /// Send SMS to who are all having Acadmic Director role
                    if (!string.IsNullOrEmpty(smsInfo.Campus))
                        SendSMSToAcaDir(smsInfo);
                    ///End
                    new Task(() => { GetSentSMSReportsInformation(SMSComposeId); }).Start();
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else { throw new Exception("sms object is required"); }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        private void SendBulkSMSInSingleWay(List<SMSRecipientsInfo> bulkSMSRecipientInfo, ComposeSMSInfo smsInfo)
        {
            string strUrl; WebRequest request; WebResponse response; Stream s; StreamReader readStream;
            CommunicationService comSer = new CommunicationService();
            if (bulkSMSRecipientInfo != null & bulkSMSRecipientInfo.Count > 0)
            {
                foreach (var smsList in bulkSMSRecipientInfo)
                {
                    string dataString = string.Empty;
                    if (!string.IsNullOrEmpty(smsList.MobileNumber) && smsList.MobileNumber.Length < 13)
                    {
                        if (smsList.MobileNumber.Length == 12)
                        {
                            if (smsList.MobileNumber[0] == '9' && smsList.MobileNumber[1] == '1')
                            {
                                smsList.MobileNumber = smsList.MobileNumber.Substring(2, 10);
                            }
                        }
                        if (smsList.MobileNumber.Length == 11)
                        {
                            if (smsList.MobileNumber[0] == '0')
                            {
                                smsList.MobileNumber = smsList.MobileNumber.Substring(1, 10);
                            }
                        }
                        if (smsList.MobileNumber.Length < 11)
                        {
                            if (Regex.IsMatch(smsList.MobileNumber, @"(?<!\d)\d{10}(?!\d)", RegexOptions.IgnoreCase))
                            {
                                //Sending SMS
                                //strUrl = ConfigurationManager.AppSettings["SMSService"].ToString() + "&senderID=TIPSGB&receipientno=" + smsList.MobileNumber + "&dcs=0&msgtxt=" + smsInfo.Message + "&state=1";
                                strUrl = ConfigurationManager.AppSettings["SMSService"].ToString() + "&senderID=" + GetSenderIdByCampus(smsInfo.Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString()) + "&receipientno=" + smsList.MobileNumber + "&dcs=0&msgtxt=" + smsInfo.Message + "&state=1";

                                try
                                {
                                    request = WebRequest.Create(strUrl);
                                    response = request.GetResponse();
                                    s = response.GetResponseStream();
                                    readStream = new StreamReader(s);
                                    dataString = readStream.ReadToEnd();
                                    UpdateSMSRecipientsStatus(smsList.Id, "Success", dataString, smsList.SentSMSReportsWithStatus != null ? smsList.SentSMSReportsWithStatus : string.Empty);
                                    response.Close();
                                    s.Close();
                                    readStream.Close();
                                }
                                catch (Exception ex)
                                {
                                    UpdateSMSRecipientsStatus(smsList.Id, "Failed", dataString, smsList.SentSMSReportsWithStatus != null ? smsList.SentSMSReportsWithStatus : string.Empty);
                                    smsInfo.Status = "Sending Failure";
                                    smsInfo.BulkSMSSent = false;
                                    comSer.CreateOrUpdateComposeSMSInfo(smsInfo);
                                    ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                                    throw ex;
                                }

                            }
                            else
                            {
                                UpdateSMSRecipientsStatus(smsList.Id, "InValid Number", dataString, smsList.SentSMSReportsWithStatus != null ? smsList.SentSMSReportsWithStatus : string.Empty);
                            }
                        }

                    }
                    else
                        UpdateSMSRecipientsStatus(smsList.Id, "InValid Number", dataString, smsList.SentSMSReportsWithStatus != null ? smsList.SentSMSReportsWithStatus : string.Empty);
                }
                smsInfo.ModifiedDate = DateTime.Now;
                smsInfo.BulkSMSSent = true;
                smsInfo.Status = "Message Sent";
                comSer.CreateOrUpdateComposeSMSInfo(smsInfo);
            }
        }
        private void GetSentSMSReportsInformation(long SMSComposeId)
        {
            string strUrl; WebRequest request; WebResponse response; Stream s; StreamReader readStream;
            strUrl = ConfigurationManager.AppSettings["SMSStatusService"].ToString();
            CommunicationService comSer = new CommunicationService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("SMSComposeId", SMSComposeId);
            Dictionary<long, IList<SMSRecipientsInfo>> BulkSMSRecipientList = comSer.GetBulkSMSRecipListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
            List<SMSRecipientsInfo> bulkSMSRecipientInfo = BulkSMSRecipientList.FirstOrDefault().Value.ToList();
            if (bulkSMSRecipientInfo != null & bulkSMSRecipientInfo.Count > 0)
            {
                foreach (SMSRecipientsInfo smsRecp in bulkSMSRecipientInfo)
                {
                    string dataString = string.Empty;
                    if (!string.IsNullOrEmpty(smsRecp.SentSMSStatusWithTid))
                    {
                        try
                        {
                            string tId = smsRecp.SentSMSStatusWithTid;
                            var idVal = tId.Split(',');
                            request = WebRequest.Create(strUrl + "&tid=" + idVal[1]);
                            response = request.GetResponse();
                            s = response.GetResponseStream();
                            readStream = new StreamReader(s);
                            dataString = readStream.ReadToEnd();
                            UpdateSMSRecipientsStatus(smsRecp.Id, smsRecp.Status, smsRecp.SentSMSStatusWithTid, dataString);
                            response.Close();
                            s.Close();
                            readStream.Close();
                        }
                        catch (Exception)
                        {
                            UpdateSMSRecipientsStatus(smsRecp.Id, smsRecp.Status, smsRecp.SentSMSStatusWithTid, string.Empty);
                        }
                    }
                    else
                    {
                        UpdateSMSRecipientsStatus(smsRecp.Id, smsRecp.Status, smsRecp.SentSMSStatusWithTid, string.Empty);
                    }
                }
            }
        }
        public ActionResult SendSelectedSMSFunction(string IdKeyValues, long SMSComposeId)
        {
            try
            {
                if ((SMSComposeId > 0) && (!string.IsNullOrEmpty(IdKeyValues)))
                {
                    CommunicationService comSer = new CommunicationService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    ComposeSMSInfo smsInfo = comSer.GetComposeSMSInfoById(SMSComposeId);
                    /// Send SMS to who are all having Acadmic Director role
                    SendSMSToAcaDir(smsInfo);
                    ///End
                    var RecpIdKeyValue = IdKeyValues.Split(',').ToList();
                    foreach (var item in RecpIdKeyValue)
                    {
                        criteria.Add("IdKeyValue", Convert.ToInt64(item));
                        criteria.Add("SMSComposeId", SMSComposeId);
                        Dictionary<long, IList<SMSRecipientsInfo>> BulkSMSRecipientList = comSer.GetBulkSMSRecipListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                        List<SMSRecipientsInfo> bulkSMSRecipientList = BulkSMSRecipientList.FirstOrDefault().Value.ToList();
                        SendBulkSMSInSingleWay(bulkSMSRecipientList, smsInfo);
                        criteria.Clear();
                    }

                }
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (System.Net.WebException ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public bool UpdateSMSRecipientsStatus(long Id, string SMSStatus, string smsStatusWithTid, string SentSMSReportsWithStatus)
        {
            try
            {
                CommunicationService comSer = new CommunicationService();
                SMSRecipientsInfo smsRecip = comSer.GetSMSREcipientDetailsById(Id);
                if (Id > 0 && smsRecip != null)
                {
                    smsRecip.Status = SMSStatus;
                    smsRecip.SentSMSStatusWithTid = smsStatusWithTid;
                    smsRecip.SentSMSReportsWithStatus = SentSMSReportsWithStatus;
                    smsRecip.RecipientsModifiedDate = DateTime.Now;
                    comSer.UpdateSMSRecipient(smsRecip);
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
            finally { }
        }
        public ActionResult DeleteSMSRecip(string Id, long SMSComposeId)
        {
            CommunicationService comSer = new CommunicationService();
            var IdKeyValues = Id.Split(',');
            List<long> PreNo = new List<long>();
            foreach (var item in IdKeyValues) { PreNo.Add(Convert.ToInt64(item)); }
            comSer.DeleteSMSRecipientsList(PreNo, SMSComposeId);
            return null;
        }
        public ActionResult EditSMSRecipient(SMSRecipientsInfo RecipList, long SMSComposeId)
        {
            CommunicationService comSer = new CommunicationService();
            SMSRecipientsInfo getinfo = comSer.GetSMSREcipientDetailsById(RecipList.Id);
            if (getinfo != null)
            {
                getinfo.SMSComposeId = SMSComposeId;
                getinfo.RecipientsModifiedDate = DateTime.Now;
                getinfo.MobileNumber = RecipList.MobileNumber;
                comSer.UpdateSMSRecipient(getinfo);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        public ActionResult SMSSuspend(long SMSComposeId)
        {
            try
            {
                ViewBag.ComposeId = SMSComposeId;
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }

        }
        public ActionResult BulkSMSRequestSuspend(long SMSComposeId, string SReason, string smsSusp)
        {
            try
            {
                CommunicationService Comsc = new CommunicationService();
                ComposeSMSInfo smsInfo = new ComposeSMSInfo();
                if (SMSComposeId > 0) { smsInfo = Comsc.GetComposeSMSInfoById(SMSComposeId); }
                if (smsInfo != null)
                {
                    if (smsSusp == "Suspend")
                    {
                        smsInfo.Suspended = true;
                        smsInfo.ReasonForSuspend = SReason;
                        smsInfo.ModifiedDate = DateTime.Now;
                        smsInfo.Status = "Suspended";
                        Comsc.CreateOrUpdateComposeSMSInfo(smsInfo);
                    }
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                return Json("0", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }

        }

        public ActionResult BulkSMSRequestReport(long ComposeId)
        {
            string userId = base.ValidateUser();
            if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
            else
            {
                CommunicationService comSer = new CommunicationService();
                ComposeSMSInfo smsInfo = comSer.GetComposeSMSInfoById(ComposeId);
                BulkSMSRequestReport_vw SMSRep = comSer.GetSMSReportInfoBySMSComposeId(smsInfo.Id);
                //smsInfo.CreatedDate = smsInfo.CreatedDate.ToShortDateString();
                smsInfo.Sent = SMSRep.Sent;
                smsInfo.NotValid = SMSRep.NotValid;
                smsInfo.Failed = SMSRep.Failed;
                smsInfo.UnDelivered = SMSRep.NotDelivered;
                smsInfo.Total = SMSRep.Total;
                return View(smsInfo);
            }
        }
        public ActionResult GetSMSStatusReportChart(long ComposeId)
        {
            if (ComposeId > 0)
            {
                CommunicationService comSer = new CommunicationService();
                BulkSMSRequestReport_vw SMSRep = comSer.GetSMSReportInfoBySMSComposeId(ComposeId);
                var SMSRepPieChart = "<graph caption='' showValues='0'>";
                SMSRepPieChart = SMSRepPieChart + " <set name='Sent' value='" + SMSRep.Sent + "' color='007A00' />";
                SMSRepPieChart = SMSRepPieChart + " <set name='Not Valid Number' value='" + SMSRep.NotValid + "' color='4A0093' />";
                SMSRepPieChart = SMSRepPieChart + " <set name='Failed' value='" + SMSRep.Failed + "' color='FF0000' />";
                SMSRepPieChart = SMSRepPieChart + " <set name='Not Delivered' value='" + SMSRep.NotDelivered + "' color='0066FF' /></graph>";
                Response.Write(SMSRepPieChart);
                return null;
            }
            return null;
        }
        public ActionResult SendTestSMSFunction(string TestNumber, long SMSComposeId)
        {
            CommunicationService comSer = new CommunicationService();
            ComposeSMSInfo smsInfo = comSer.GetComposeSMSInfoById(SMSComposeId);
            if (smsInfo != null)
            {
                //string strUrl = ConfigurationManager.AppSettings["SMSService"].ToString() + "&senderID=TIPSGB&receipientno=" + TestNumber + "&dcs=0&msgtxt=" + smsInfo.Message + "&state=1";
                string strUrl = ConfigurationManager.AppSettings["SMSService"].ToString() + "&senderID=" + GetSenderIdByCampus(smsInfo.Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString()) + "&receipientno=" + TestNumber + "&dcs=0&msgtxt=" + smsInfo.Message + "&state=1";
                WebRequest request = WebRequest.Create(strUrl);
                WebResponse response = request.GetResponse();
                Stream s = response.GetResponseStream();
                StreamReader readStream = new StreamReader(s);
                string dataString = readStream.ReadToEnd();
                response.Close();
                s.Close();
                readStream.Close();
                if (dataString.Contains("Template not matched"))
                    return Json("The Given Content is wrong", JsonRequestBehavior.AllowGet);
                else
                    return Json("Successfully Test Message Sent", JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TestSMSCreditFunction()
        {
            try
            {
                string strUrl = ConfigurationManager.AppSettings["SMSCreditService"].ToString() + "&type=0";
                WebRequest request = WebRequest.Create(strUrl);
                WebResponse response = request.GetResponse();
                Stream s = response.GetResponseStream();
                StreamReader readStream = new StreamReader(s);
                string dataString = readStream.ReadToEnd();
                response.Close();
                s.Close();
                readStream.Close();
                if (dataString != null && dataString != "")
                {
                    var strcredit = dataString.Split(',');
                    return Json(strcredit[1], JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult CheckSMSCredit(long SMSComposeId)
        {
            try
            {
                string StrCredit = string.Empty;
                string strUrl = ConfigurationManager.AppSettings["SMSCreditService"].ToString() + "&type=0";
                WebRequest request = WebRequest.Create(strUrl);
                WebResponse response = request.GetResponse();
                Stream s = response.GetResponseStream();
                StreamReader readStream = new StreamReader(s);
                string dataString = readStream.ReadToEnd();
                response.Close(); s.Close(); readStream.Close();
                var tempCredit = dataString.Split(',');
                StrCredit = Regex.Replace(tempCredit[1], "[^0-9]+", string.Empty);
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("SMSComposeId", SMSComposeId);
                Dictionary<long, IList<SMSRecipientsInfo>> BulkSMSRecipientList = comSer.GetBulkSMSRecipListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                if (BulkSMSRecipientList != null && BulkSMSRecipientList.Count > 0 && BulkSMSRecipientList.FirstOrDefault().Value != null)
                {
                    var result = from r in BulkSMSRecipientList.FirstOrDefault().Value
                                 where r.MobileNumber != null
                                 select r;
                    if (Convert.ToInt32(StrCredit) >= result.Count()) { return Json("Success", JsonRequestBehavior.AllowGet); }
                    else { return Json("", JsonRequestBehavior.AllowGet); }
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        #endregion

        #region Added By Micheal
        public ActionResult FillGrades(string campus)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ms = new MastersService();
                    Dictionary<string, Object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(campus))
                    {
                        criteria.Add("Campus", campus);
                        Dictionary<long, IList<CampusGradeMaster>> TIPSGrade = ms.GetCampusGradeMasterListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                        var Gradname = (
                              from items in TIPSGrade.FirstOrDefault().Value
                              select new
                              {
                                  Text = items.gradcod,
                                  Value = items.gradcod
                              }).Distinct().ToList();
                        return Json(Gradname, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        #endregion

        private void SendSMSToAcaDir(ComposeSMSInfo smsInfo)
        {
            string strUrl; WebRequest request; WebResponse response; Stream s; StreamReader readStream;
            // string mobileNum = ConfigurationManager.AppSettings["AcaDirMobileNum"];
            CommunicationService comSer = new CommunicationService();
            //string mobileNum = "";
            IList<CoOrdinatorsContactInfo> CoOrdinatorsContactInfo = GetCoOrdinatorsContactInfo(smsInfo.Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString(), "Bulk Sms");
            if (CoOrdinatorsContactInfo != null && CoOrdinatorsContactInfo.Count > 0)
            {
                foreach (var item in CoOrdinatorsContactInfo)
                {
                    if (!string.IsNullOrWhiteSpace(item.MobileNo) && Regex.IsMatch(item.MobileNo, @"(?<!\d)\d{10}(?!\d)", RegexOptions.IgnoreCase))
                    {
                        //Sending SMS
                        // strUrl = ConfigurationManager.AppSettings["SMSService"].ToString() + "&senderID=TIPSGB&receipientno=" + item.MobileNo + "&dcs=0&msgtxt=" + smsInfo.Message + "&state=1";
                        strUrl = ConfigurationManager.AppSettings["SMSService"].ToString() + "&senderID=" + GetSenderIdByCampus(smsInfo.Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString()) + "&receipientno=" + item.MobileNo + "&dcs=0&msgtxt=" + smsInfo.Message + "&state=1";
                        try
                        {
                            request = WebRequest.Create(strUrl);
                            response = request.GetResponse();
                            s = response.GetResponseStream();
                            readStream = new StreamReader(s);
                            response.Close();
                            s.Close();
                            readStream.Close();
                        }
                        catch (Exception ex)
                        {
                            ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                        }

                    }
                }
            }
        }
        //////
        private void SendMailToAcaDir(ComposeEmailInfo cei)
        {
            System.Net.Mail.MailMessage AcaDirMailmsg = new System.Net.Mail.MailMessage();
            AdmissionManagementService ams = new AdmissionManagementService();
            /// Sent mail who are all having Academic Director Role
            if (cei != null && !string.IsNullOrWhiteSpace(cei.Campus))
            {
                if (cei.Attachment == true)
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("PreRegNum", Convert.ToInt32(cei.Id));
                    criteria.Add("AppName", "Student");
                    Dictionary<long, IList<EmailAttachment>> emailattachment = ams.GetEmailAttachmentListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                    Attachment mailAttach = null;
                    foreach (var item in emailattachment.FirstOrDefault().Value)
                    {
                        MemoryStream ms = new MemoryStream(item.Attachment);
                        mailAttach = new Attachment(ms, item.AttachmentName.ToString());  //Data posted from form
                        AcaDirMailmsg.Attachments.Add(mailAttach);
                    }

                }
                IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(cei.Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                AcaDirMailmsg.Body = cei.Message;
                AcaDirMailmsg.Subject = cei.Subject;
                AcaDirMailmsg.IsBodyHtml = true;
                SmtpClient sentAcaDirsmtp = new SmtpClient();
                sentAcaDirsmtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address 
                sentAcaDirsmtp.Port = 25;
                //Or your Smtp Email ID and Password  
                sentAcaDirsmtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                sentAcaDirsmtp.EnableSsl = true;
                if (!string.IsNullOrEmpty(cei.AlternativeEmailId) && !string.IsNullOrEmpty(cei.AlternatPassword))
                {
                    AcaDirMailmsg.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                    AcaDirMailmsg.To.Add(campusemaildet.First().EmailId.ToString());
                    sentAcaDirsmtp.Credentials = new System.Net.NetworkCredential
                   (cei.AlternativeEmailId, cei.AlternatPassword);
                }
                else
                {
                    sentAcaDirsmtp.Host = campusemaildet.First().BulkEmailIdHost; //Or Your SMTP Server Address 
                    sentAcaDirsmtp.Port = campusemaildet.First().BulkEmailIdPort;
                    AcaDirMailmsg.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                    AcaDirMailmsg.To.Add(campusemaildet.First().EmailId.ToString());
                    sentAcaDirsmtp.Credentials = new System.Net.NetworkCredential
                   (campusemaildet.First().BulkEmailId.ToString(), campusemaildet.First().BulkEmailIdPassword.ToString());

                }
                IList<CoOrdinatorsContactInfo> CoOrdinatorsContactInfo = GetCoOrdinatorsContactInfo(cei.Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString(), "Bulk Email");
                if (CoOrdinatorsContactInfo != null && CoOrdinatorsContactInfo.Count > 0)
                {
                    foreach (var item in CoOrdinatorsContactInfo)
                    {
                        if (!string.IsNullOrWhiteSpace(item.EmailId))
                            AcaDirMailmsg.Bcc.Add(item.EmailId);
                    }
                }
                try
                {
                    ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    {
                        return true;
                    };
                    new Task(() => { sentAcaDirsmtp.Send(AcaDirMailmsg); }).Start();
                }
                catch (Exception ex) { ExceptionPolicy.HandleException(ex, "AdmissionPolicy"); }
            }
        }
        #region Campus EmailId Master
        public ActionResult CampusMailConfig()
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
        public ActionResult JqGridCampusMailId(string Campus, string Server, string EmailId, string Password, string AlternateEmailId, string AlternateEmailIdPassword, string FBLink, string PhoneNumber, string SenderID, string SchoolName, string WebSiteName, string PinCode, string Address, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                //string userId = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrWhiteSpace(Campus)) criteria.Add("Campus", Campus);
                if (!string.IsNullOrWhiteSpace(Server)) criteria.Add("Server", Server);
                if (!string.IsNullOrWhiteSpace(EmailId)) criteria.Add("EmailId", EmailId);
                if (!string.IsNullOrWhiteSpace(Password)) criteria.Add("Password", Password);
                if (!string.IsNullOrWhiteSpace(AlternateEmailId)) criteria.Add("AlternateEmailId", AlternateEmailId);
                if (!string.IsNullOrWhiteSpace(AlternateEmailIdPassword)) criteria.Add("AlternateEmailIdPassword", AlternateEmailIdPassword);
                if (!string.IsNullOrWhiteSpace(FBLink)) criteria.Add("FBLink", FBLink);
                if (!string.IsNullOrWhiteSpace(PhoneNumber)) criteria.Add("PhoneNumber", PhoneNumber);

                if (!string.IsNullOrWhiteSpace(SenderID)) criteria.Add("WebSiteName", WebSiteName);
                if (!string.IsNullOrWhiteSpace(SenderID)) criteria.Add("SchoolName", SchoolName);
                if (!string.IsNullOrWhiteSpace(SenderID)) criteria.Add("Address", Address);
                if (!string.IsNullOrWhiteSpace(SenderID)) criteria.Add("PinCode", PinCode);

                if (!string.IsNullOrWhiteSpace(SenderID)) criteria.Add("SenderID", SenderID);
                Dictionary<long, IList<CampusEmailId>> CampusMailIdList = comSer.GetCampusMailIdListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (CampusMailIdList != null && CampusMailIdList.Count > 0 && CampusMailIdList.FirstOrDefault().Key > 0 && CampusMailIdList.FirstOrDefault().Value != null)
                {
                    long totalrecords = CampusMailIdList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)5);
                    var camMailLst = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (
                             from items in CampusMailIdList.First().Value

                             select new
                             {
                                 cell = new string[] 
                                         {
                                             items.Id.ToString(),
                                             items.Campus,
                                             items.Server,
                                             items.EmailId,
                                             items.Password,
                                             items.AlternateEmailId,
                                             items.AlternateEmailIdPassword,
                                             items.FBLink,
                                             items.PhoneNumber,
                                             items.Address,
                                             items.WebSiteName,
                                             items.SchoolName,
                                             items.PinCode,
                                             items.SenderID,                                             
                                             items.CreatedBy,
                                             items.ModifiedBy,
                                             items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd'/'MM'/'yyyy"):"",
                                             items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd'/'MM'/'yyyy"):""
                                         }
                             }).ToList()
                    };
                    return Json(camMailLst, JsonRequestBehavior.AllowGet);
                }
                else { return Json(null, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult AddorUpdateCampusMailId(CampusEmailId cE, string edit)
        {
            try
            {
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    if (cE.Id > 0 && edit == "edit")
                    {
                        CampusEmailId CamMail = UsSvc.GetCampusEmailIdById(cE.Id);
                        CamMail.EmailId = cE.EmailId;
                        CamMail.Password = cE.Password;
                        CamMail.AlternateEmailId = cE.AlternateEmailId;
                        CamMail.AlternateEmailIdPassword = cE.AlternateEmailIdPassword;
                        CamMail.FBLink = cE.FBLink;
                        CamMail.PhoneNumber = cE.PhoneNumber;
                        CamMail.Address = cE.Address;
                        CamMail.WebSiteName = cE.WebSiteName;
                        CamMail.SchoolName = cE.SchoolName;
                        CamMail.PinCode = cE.PinCode;
                        CamMail.SenderID = cE.SenderID;
                        CamMail.ModifiedBy = userId;
                        CamMail.ModifiedDate = DateTime.Now;
                        UsSvc.SaveOrUpdateCampusEmailId(CamMail);
                    }
                    else
                    {
                        cE.CreatedDate = DateTime.Now;
                        cE.CreatedBy = userId;
                        cE.ModifiedBy = userId;
                        cE.ModifiedDate = DateTime.Now;
                        UsSvc.SaveOrUpdateCampusEmailId(cE);
                    }

                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "SystemMgntPolicy");
                throw ex;
            }
        }
        #endregion
        #region StaffNotification
        #region StaffBulkMail
        public ActionResult StaffBulkEmail(long? Id)
        {

            StaffComposeMailInfo cei = new StaffComposeMailInfo();
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<DesignationMaster>> DesignationMaster = ms.GetDesignationMasterListWithPagingAndCriteria(0, 200, string.Empty, string.Empty, criteria);
            ViewBag.designationddl = DesignationMaster.First().Value;
            Dictionary<long, IList<StaffDepartmentMaster>> DepartmentMaster = ms.GetStaffDepartmentMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.departmentddl = DepartmentMaster.First().Value;
            ViewBag.EmptyValue = "Yes";
            if (Id > 0) { cei = comSer.GetStaffComposeEmailInfoById(Id ?? 0); ViewBag.EmptyValue = "False"; }
            else { }

            return View(cei);
        }
        public ActionResult StaffBulkEmailRequest(string Campus, string department, string AdStatus, string designation, string StName, string StId, string StIshostel, string AcYear, string VanNo, string Section, string saveOrClear, long ComposeId, string AppliedFrmDate, string AppliedToDate, int rows, string sord, string sidx, int? page = 1)
        {
            try
            {
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                DateTime DateNow = DateTime.Now;
                CommunicationService comSer = new CommunicationService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likeCriteria = new Dictionary<string, object>();
                StaffComposeMailInfo cei = new StaffComposeMailInfo();
                Dictionary<long, IList<StaffDetailsView>> BulkEmailRegList = null;
                Dictionary<long, IList<StaffRecipientsEmailInfo>> BulkEmailRecList = null;
                if (ComposeId > 0) { cei = comSer.GetStaffComposeEmailInfoById(ComposeId); }
                if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                if (!string.IsNullOrEmpty(department)) { criteria.Add("Department", department); }
                if (!string.IsNullOrEmpty(designation)) { criteria.Add("Designation", designation); }
                if (!string.IsNullOrEmpty(StName)) { criteria.Add("Name", StName); }
                if (!string.IsNullOrEmpty(StId)) { criteria.Add("IdNumber", StId); }
                criteria.Add("Status", "Registered");
                criteria.Add("WorkingType", "Staff");
                sord = sord == "desc" ? "Desc" : "Asc";
                if (ComposeId > 0 && cei.IsSaveList)
                {
                    criteria.Clear();
                    criteria.Add("ComposeId", ComposeId);
                    BulkEmailRecList = comSer.GetStaffEmailListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                }
                else
                {
                    if (!string.IsNullOrEmpty(Campus))
                    {
                        StaffManagementService sms = new StaffManagementService();
                        BulkEmailRegList = sms.GetStaffDetailsViewListWithPaging(page - 1, 9999, sord, sidx, criteria);
                    }
                }

                if (saveOrClear == "Save")
                {
                    List<StaffRecipientsEmailInfo> recipientslist = new List<StaffRecipientsEmailInfo>();
                    if (BulkEmailRegList != null)
                    {
                        foreach (var item in BulkEmailRegList.FirstOrDefault().Value)
                        {
                            StaffRecipientsEmailInfo rei = new StaffRecipientsEmailInfo();
                            rei.ComposeId = Convert.ToInt64(ComposeId);
                            rei.PreRegNum = item.PreRegNum;
                            rei.Campus = item.Campus;
                            rei.Name = item.Name;
                            rei.IdNumber = item.IdNumber;
                            rei.Status = item.Status;
                            rei.RecipientsCreatedDate = DateTime.Now;
                            rei.RecipientsModifiedDate = DateTime.Now;
                            rei.Department = item.Department;
                            rei.Designation = item.Designation;
                            rei.EmailId = item.EmailId;
                            rei.IdKeyValue = item.Id;
                            // rei.FamilyDetailType = item.FamilyDetailType;
                            rei.EmailId = item.EmailId;
                            rei.Status = "InProgress";
                            recipientslist.Add(rei);
                        }
                        comSer.CreateOrUpdateStaffEmailList(recipientslist);
                        foreach (StaffDetailsView item in BulkEmailRegList.FirstOrDefault().Value)
                        {
                            item.Status = "InProgress";
                        }

                        /// Save to the ComposeEmailInfo table
                        cei.IsSaveList = true;
                        cei.Status = "Recipients Added";
                        if (recipientslist[0] != null)
                        {

                            cei.Status = AdStatus;

                            if (StName != "" & !string.IsNullOrEmpty(StName))
                                cei.Name = StName;
                            if (StId != "" & !string.IsNullOrEmpty(StId))
                                cei.IdNumber = StId;
                            cei.Campus = Campus;

                        }
                        comSer.CreateOrUpdateStaffComposeEmailInfo(cei);
                    }

                }
                else
                {
                    if (saveOrClear == "Clear")
                    {
                        comSer.DeleteSavedListInStaffmailInfo(Convert.ToInt64(ComposeId));
                        cei.IsSaveList = false;
                        //  cei.Status = "Email Composed";
                        cei.Campus = null;
                        cei.Department = null;
                        cei.Designation = null;
                        cei.Name = null;
                        cei.Status = null;
                        comSer.CreateOrUpdateStaffComposeEmailInfo(cei);
                        var jsondat = new { rows = (new { cell = new string[] { } }) };
                        return Json(jsondat, JsonRequestBehavior.AllowGet);
                    }
                }
                if (BulkEmailRegList != null && BulkEmailRegList.FirstOrDefault().Value.Count > 0 && BulkEmailRegList.FirstOrDefault().Key > 0)
                {
                    long totalrecords = BulkEmailRegList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in BulkEmailRegList.First().Value
                                select new
                                {
                                    i = 2,

                                    cell = new string[] {
                                             items.Id.ToString(),
                                             items.Id.ToString(),
                                             items.PreRegNum.ToString(),
                                             items.IdNumber,
                                             items.Name,
                                             items.Campus,
                                             items.Department,
                                             items.Designation,
                                             items.EmailId,
                                             items.Status,
                                             }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (BulkEmailRecList != null && BulkEmailRecList.FirstOrDefault().Value.Count > 0 && BulkEmailRecList.FirstOrDefault().Key > 0)
                    {
                        long totalrecords = BulkEmailRecList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in BulkEmailRecList.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                                                  items.Id.ToString(),
                                                  items.IdKeyValue.ToString(),
                                                  items.PreRegNum.ToString(),
                                                  items.IdNumber,
                                                  items.Name,
                                                  items.Campus,
                                                  items.Department,
                                                  items.Designation,
                                                  items.EmailId,
                                                  items.Status,
                                                  //items.RecipientsCreatedDate != null?items.RecipientsCreatedDate.ToString():"",
                                                  //items.RecipientsModifiedDate != null?items.RecipientsModifiedDate.ToString():"",
                                                  }
                                    })
                        };
                        return Json(jsondat, JsonRequestBehavior.AllowGet);
                    }
                }

                var jsonda = new { rows = (new { cell = new string[] { } }) };
                return Json(jsonda, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult StaffBulkEmail(StaffComposeMailInfo compEInfo)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    CommunicationService comSer = new CommunicationService();
                    compEInfo.ModifiedDate = DateTime.Now;
                    compEInfo.ModifiedBy = userId;
                    if (compEInfo.Id == 0)
                    {
                        compEInfo.CreatedDate = DateTime.Now;
                        compEInfo.CreatedBy = userId;
                        long Id = comSer.CreateOrUpdateStaffComposeEmailInfo(compEInfo);
                        compEInfo.BulkReqId = "BER-" + Id;
                        compEInfo.Status = "Email Composed";
                    }
                    comSer.CreateOrUpdateStaffComposeEmailInfo(compEInfo);
                    return RedirectToAction("StaffBulkEmail", new { Id = compEInfo.Id });
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult ShowStaffMailMessage(long Id)
        {
            string data = "";
            CommunicationService comServ = new CommunicationService();
            StaffComposeMailInfo cei = comServ.GetStaffComposeEmailInfoById(Id);
            data = cei.Message;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendMailToStaff(long ComposeId, string Campus, bool IsAlterNativeMail)
        {

            /// Bulk email Added option is true
            CommunicationService comServ = new CommunicationService();
            StaffComposeMailInfo cei = comServ.GetStaffComposeEmailInfoById(ComposeId);

            cei.BulkEmailAdded = true;
            comServ.CreateOrUpdateStaffComposeEmailInfo(cei);
            // SendMailToAcaDir(cei);
            new Task(() => { SendMailToStaffBulkRequest(ComposeId, (!string.IsNullOrEmpty(cei.Campus)) ? cei.Campus : Campus, IsAlterNativeMail); }).Start();
            //bulk send email has been initiated and in progress
            //status will be updated once it is complete
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendMailToStaffBulkRequest(long ComposeId, string Campus, bool IsAlterNativeMail)
        {
            try
            {
                bool bulkSendCompleteWithError = false;
                AdmissionManagementService ams = new AdmissionManagementService();
                CommunicationService comServ = new CommunicationService();
                IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                string From = ConfigurationManager.AppSettings["From"];
                string To = ConfigurationManager.AppSettings["To"];

                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                StaffComposeMailInfo cei = comServ.GetStaffComposeEmailInfoById(ComposeId);
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("ComposeId", ComposeId);
                IList<StaffRecipientsEmailInfo> listToUpdate = new List<StaffRecipientsEmailInfo>();
                Dictionary<long, IList<StaffRecipientsEmailInfo>> BulkEmailRegList = comServ.GetStaffEmailListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                AdmissionManagementService admserv = new AdmissionManagementService();
                if (cei.Attachment == true)
                {
                    criteria.Clear();
                    criteria.Add("PreRegNum", Convert.ToInt32(cei.Id));
                    criteria.Add("AppName", "Staff");
                    Dictionary<long, IList<EmailAttachment>> emailattachment = ams.GetEmailAttachmentListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                    Attachment mailAttach = null;
                    foreach (var item in emailattachment.FirstOrDefault().Value)
                    {
                        MemoryStream ms = new MemoryStream(item.Attachment);
                        mailAttach = new Attachment(ms, item.AttachmentName.ToString());  //Data posted from form
                        mail.Attachments.Add(mailAttach);
                    }

                }

                if (cei != null)
                {
                    mail.Body = cei.Message;
                    mail.Subject = cei.Subject;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient("localhost", 25);
                    smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address  
                    //Or your Smtp Email ID and Password  
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = true;
                    if (IsAlterNativeMail == true)
                    {
                        mail.From = new MailAddress(cei.AlternativeEmailId);
                        mail.To.Add(campusemaildet.First().EmailId.ToString());
                        smtp.Credentials = new System.Net.NetworkCredential(cei.AlternativeEmailId, cei.AlternatPassword);
                    }
                    else
                    {
                        mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                        mail.To.Add(campusemaildet.First().EmailId.ToString());
                        smtp.Credentials = new System.Net.NetworkCredential(campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                    }
                    string Count = ConfigurationManager.AppSettings["RecipientsCountPerMail"];
                    long totalcount = BulkEmailRegList.First().Key;
                    int i = 0; int noOfRec = Convert.ToInt32(Count);
                    int inc = 1;
                    foreach (var item in BulkEmailRegList.FirstOrDefault().Value)
                    {
                        inc++;
                        //logic for the number of recipients
                        //read it from web config
                        //if recipient is more than 1 then add in BCC
                        item.Status = "True";
                        //send email logic
                        if (ValidEmailOrNot(item.EmailId))
                        {
                            listToUpdate.Add(item);
                            mail.Bcc.Add(item.EmailId);
                            i++;
                            if (i == noOfRec)
                            {
                                i = 0;
                                bool result = SendEmailAndUpdateStaffEmailInfo(mail, listToUpdate, comServ, smtp, campusemaildet);
                                if (result == false)
                                    bulkSendCompleteWithError = true;
                                mail.Bcc.Clear();
                                listToUpdate.Clear();
                            }
                            else if (inc == totalcount)
                            {
                                bool result = SendEmailAndUpdateStaffEmailInfo(mail, listToUpdate, comServ, smtp, campusemaildet);
                                if (result == false)
                                    bulkSendCompleteWithError = true;
                                mail.Bcc.Clear();
                                listToUpdate.Clear();
                            }
                            else { }
                        }
                        else
                        {
                            IList<StaffRecipientsEmailInfo> WrongMailUpdate = new List<StaffRecipientsEmailInfo>();
                            WrongMailUpdate.Add(item);
                            comServ.CreateOrUpdateStaffEmailListWithStatus(WrongMailUpdate, "InValid MailId");
                        }
                    }
                    //if the number of recipient is not dividable with the total recipients,
                    //save the list here if listToUpdate is not null means
                    //if (listToUpdate != null && listToUpdate.Count > 0)
                    //{
                    //    bool result = SendEmailAndUpdateRecipientsEmailInfo(mail, listToUpdate, comServ, smtp, campusemaildet);
                    //    if (result == false)
                    //        bulkSendCompleteWithError = true;
                    //    mail.Bcc.Clear();
                    //    listToUpdate.Clear();
                    //}
                    if (bulkSendCompleteWithError)
                        cei.Status = "CompletedWithErrors";
                    else cei.Status = "SuccessfullyCompleted";
                    //save this
                    comServ.CreateOrUpdateStaffComposeEmailInfo(cei);
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public bool SendEmailAndUpdateStaffEmailInfo(System.Net.Mail.MailMessage mail, IList<StaffRecipientsEmailInfo> listToUpdate, CommunicationService comServ, SmtpClient smtp, IList<CampusEmailId> CampusEmailId)
        {
            bool retValue = false;
            try
            {
                //send email and store it in db
                mail.From = new MailAddress(CampusEmailId.First().EmailId.ToString());
                smtp.Credentials = new System.Net.NetworkCredential
                (CampusEmailId.First().EmailId.ToString(), CampusEmailId.First().Password.ToString());
                smtp.Send(mail);
                comServ.CreateOrUpdateStaffEmailListWithStatus(listToUpdate, "Sent");
                //psf.save the list
                mail.Bcc.Clear();
                retValue = true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("quota"))
                {
                    try
                    {
                        mail.From = new MailAddress(CampusEmailId.First().AlternateEmailId.ToString());
                        smtp.Credentials = new System.Net.NetworkCredential
                        (CampusEmailId.First().AlternateEmailId.ToString(), CampusEmailId.First().AlternateEmailIdPassword.ToString());
                        smtp.Send(mail);
                        mail.Bcc.Clear();
                        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                        retValue = true;
                    }
                    catch (Exception)
                    {
                        comServ.CreateOrUpdateStaffEmailListWithStatus(listToUpdate, "Not Sent");
                        mail.Bcc.Clear();
                        retValue = false;
                    }
                }
                else
                {
                    try
                    {
                        mail.From = new MailAddress(CampusEmailId.First().AlternateEmailId.ToString());
                        smtp.Credentials = new System.Net.NetworkCredential
                        (CampusEmailId.First().AlternateEmailId.ToString(), CampusEmailId.First().AlternateEmailIdPassword.ToString());
                        smtp.Send(mail);
                        mail.Bcc.Clear();
                        ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                        retValue = true;
                    }
                    catch (Exception)
                    {
                        comServ.CreateOrUpdateStaffEmailListWithStatus(listToUpdate, "Not Sent");
                        mail.Bcc.Clear();
                        retValue = false;
                    }
                }
            }
            return retValue;
        }

        public ActionResult SendSelectStaffEmailFunction(string Ids, long ComposeId, bool IsAlterNativeMail)
        {
            try
            {
                AdmissionManagementService ams = new AdmissionManagementService();
                CommunicationService comServ = new CommunicationService();
                SMS sms = new SMS(); bool bulkSendCompleteWithError = false;
                //ComposeEmailInfo cei = comServ.GetComposeMailInfoById(ComposeId);//Commented By Prabakaran
                StaffComposeMailInfo cei = comServ.GetStaffComposeEmailInfoById(ComposeId);
                //  Send mail to who are all having Academic Role
                //SendMailToAcaDir(cei);
                //  End
                IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(cei.Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                string From = ConfigurationManager.AppSettings["From"];
                string To = ConfigurationManager.AppSettings["To"];
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                IList<StaffRecipientsEmailInfo> listToUpdate = new List<StaffRecipientsEmailInfo>();
                if (cei.Attachment == true)
                {
                    criteria.Clear();
                    criteria.Add("PreRegNum", Convert.ToInt32(cei.Id));
                    criteria.Add("AppName", "Staff");
                    Dictionary<long, IList<EmailAttachment>> emailattachment = ams.GetEmailAttachmentListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                    Attachment mailAttach = null;
                    foreach (var item in emailattachment.FirstOrDefault().Value)
                    {
                        MemoryStream ms = new MemoryStream(item.Attachment);
                        mailAttach = new Attachment(ms, item.AttachmentName.ToString());  //Data posted from form
                        mail.Attachments.Add(mailAttach);
                    }

                }
                List<long> bulklongPReg = new List<long>();
                var bulkPreg = Ids.Split(',');
                foreach (var item in bulkPreg)
                {
                    bulklongPReg.Add(Convert.ToInt64(item));
                }

                long[] bulkarr = bulklongPReg.ToArray();
                criteria.Clear();
                criteria.Add("IdKeyValue", bulkarr);
                criteria.Add("ComposeId", ComposeId);
                Dictionary<long, IList<StaffRecipientsEmailInfo>> BulkEmailRecipientsList = comServ.GetStaffEmailListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);

                if (cei != null)
                {
                    mail.Body = cei.Message;
                    mail.Subject = cei.Subject;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient("localhost", 25);
                    smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address  
                    //Or your Smtp Email ID and Password  
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = true;
                    if (IsAlterNativeMail == true)
                    {
                        mail.From = new MailAddress(cei.AlternativeEmailId);
                        mail.To.Add(campusemaildet.First().EmailId.ToString());
                        smtp.Credentials = new System.Net.NetworkCredential
                       (cei.AlternativeEmailId, cei.AlternatPassword);
                    }
                    else
                    {
                        mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                        mail.To.Add(campusemaildet.First().EmailId.ToString());
                        smtp.Credentials = new System.Net.NetworkCredential
                       (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());

                    }

                    string Count = ConfigurationManager.AppSettings["RecipientsCountPerMail"].ToString();
                    long totalcount = BulkEmailRecipientsList.First().Key;
                    int i = 0; int noOfRec = Convert.ToInt32(Count);
                    int inc = 1;
                    foreach (var item in BulkEmailRecipientsList.FirstOrDefault().Value)
                    {
                        inc++;
                        //logic for the number of recipients
                        //read it from web config
                        item.Status = "True";
                        //send email logic
                        if (ValidEmailOrNot(item.EmailId))
                        {
                            listToUpdate.Add(item);
                            mail.Bcc.Add(item.EmailId);
                            i++;
                            if (i == noOfRec || totalcount < 10)
                            {
                                i = 0;
                                bool result = SendEmailAndUpdateStaffEmailInfo(mail, listToUpdate, comServ, smtp, campusemaildet);
                                if (result == false)
                                    bulkSendCompleteWithError = true;
                                mail.Bcc.Clear();
                                listToUpdate.Clear();
                            }
                            else if (inc == totalcount)
                            {
                                bool result = SendEmailAndUpdateStaffEmailInfo(mail, listToUpdate, comServ, smtp, campusemaildet);
                                if (result == false)
                                    bulkSendCompleteWithError = true;
                                mail.Bcc.Clear();
                                listToUpdate.Clear();
                            }
                            else { }
                        }
                        else
                        {
                            IList<StaffRecipientsEmailInfo> WrongMailUpdate = new List<StaffRecipientsEmailInfo>();
                            WrongMailUpdate.Add(item);
                            comServ.CreateOrUpdateStaffEmailListWithStatus(WrongMailUpdate, "InValid MailId");
                        }
                    }
                    if (bulkSendCompleteWithError)
                        cei.Status = "CompletedWithErrors";
                    else cei.Status = "SuccessfullyCompleted";
                    criteria.Clear();
                    criteria.Add("ComposeId", ComposeId);
                    Dictionary<long, IList<StaffRecipientsEmailInfo>> checkSentMails = comServ.GetStaffEmailListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                    if (checkSentMails != null && checkSentMails.FirstOrDefault().Value.Count > 0 && checkSentMails.FirstOrDefault().Key > 0)
                    {
                        foreach (var item in checkSentMails.FirstOrDefault().Value)
                        {
                            if (item.Status != "Sent" && item.Status != "InValid MailId") { cei.Status = "PartiallyCompleted"; break; }
                        }
                    }
                    //save this
                    comServ.CreateOrUpdateStaffComposeEmailInfo(cei);
                }
                return null;
            }

            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult SendStaffTestMail(string MailId, long MailComposeId)
        {
            if (ValidEmailOrNot(MailId) && !string.IsNullOrEmpty(MailId) && MailComposeId > 0)
            {
                try
                {
                    CommunicationService comSer = new CommunicationService();
                    StaffComposeMailInfo comEmail = comSer.GetStaffComposeEmailInfoById(MailComposeId);
                    if (comEmail != null)
                    {
                        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                        Dictionary<string, object> criteria = new Dictionary<string, object>();
                        if (comEmail.Attachment == true)
                        {
                            AdmissionManagementService ams = new AdmissionManagementService();
                            criteria.Clear();
                            criteria.Add("PreRegNum", Convert.ToInt32(comEmail.Id));
                            criteria.Add("AppName", "Staff");
                            Dictionary<long, IList<EmailAttachment>> emailattachment = ams.GetEmailAttachmentListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);
                            Attachment mailAttach = null;
                            foreach (var item in emailattachment.FirstOrDefault().Value)
                            {
                                MemoryStream ms = new MemoryStream(item.Attachment);
                                mailAttach = new Attachment(ms, item.AttachmentName.ToString());  //Data posted from form
                                mail.Attachments.Add(mailAttach);
                            }
                        }
                        UserService us = new UserService();
                        User uscls = us.GetUserByUserId(comEmail.UserId);
                        IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(uscls.Campus == null ? "" : uscls.Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                        //if (campusemaildet != null && campusemaildet.Count > 0)
                        mail.Body = comEmail.Message;
                        mail.Subject = comEmail.Subject;
                        mail.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient("localhost", 25);
                        smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address  
                        //Or your Smtp Email ID and Password  
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.EnableSsl = true;
                        mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                        mail.To.Add(campusemaildet.First().EmailId.ToString());
                        smtp.Credentials = new System.Net.NetworkCredential(campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                        mail.Bcc.Add(MailId);
                        smtp.Send(mail);
                        return Json("Successfully Test Mail Sent", JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    return Json(ex.Message, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Invalid Mail Address", JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendFalseEmailFunctionForStaff(long ComposeId, bool IsAlterNativeMail)
        {
            try
            {
                AdmissionManagementService ams = new AdmissionManagementService();
                CommunicationService comServ = new CommunicationService();
                SMS sms = new SMS(); bool bulkSendCompleteWithError = false;
                ComposeEmailInfo cei = comServ.GetComposeMailInfoById(ComposeId);
                IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(cei.Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                string From = ConfigurationManager.AppSettings["From"];
                string To = ConfigurationManager.AppSettings["To"];
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                IList<StaffRecipientsEmailInfo> listToUpdate = new List<StaffRecipientsEmailInfo>();
                if (cei.Attachment == true)
                {
                    criteria.Clear();
                    criteria.Add("PreRegNum", Convert.ToInt32(cei.Id));
                    criteria.Add("AppName", "Staff");
                    Dictionary<long, IList<EmailAttachment>> emailattachment = ams.GetEmailAttachmentListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                    Attachment mailAttach = null;
                    foreach (var item in emailattachment.FirstOrDefault().Value)
                    {
                        MemoryStream ms = new MemoryStream(item.Attachment);
                        mailAttach = new Attachment(ms, item.AttachmentName.ToString());  //Data posted from form
                        mail.Attachments.Add(mailAttach);
                    }

                }
                criteria.Clear();
                criteria.Add("ComposeId", ComposeId);
                criteria.Add("Status", "Not Sent");
                Dictionary<long, IList<StaffRecipientsEmailInfo>> BulkEmailRecipientsList = comServ.GetStaffEmailListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                if (cei != null)
                {
                    mail.Body = cei.Message;
                    mail.Subject = cei.Subject;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient("localhost", 25);
                    smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address  
                    //Or your Smtp Email ID and Password  
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = true;
                    if (From == "live")
                    {
                        mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                        smtp.Credentials = new System.Net.NetworkCredential
                       (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());

                    }
                    if (From == "test")
                    {
                        mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                        smtp.Credentials = new System.Net.NetworkCredential
                       (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());

                    }
                    string Count = ConfigurationManager.AppSettings["RecipientsCountPerMail"].ToString();
                    int i = 0; int noOfRec = Convert.ToInt32(Count);
                    int inc = 1;
                    foreach (var item in BulkEmailRecipientsList.FirstOrDefault().Value)
                    {
                        //logic for the number of recipients
                        //read it from web config
                        item.Status = "True";
                        listToUpdate.Add(item);
                        //send email logic
                        if (ValidEmailOrNot(item.EmailId))
                        {
                            mail.Bcc.Add(item.EmailId);
                            if (i == noOfRec || BulkEmailRecipientsList.FirstOrDefault().Value.Count < 10)
                            {
                                i = 0;
                                bool result = SendEmailAndUpdateStaffEmailInfo(mail, listToUpdate, comServ, smtp, campusemaildet);
                                if (result == false)
                                    bulkSendCompleteWithError = true;
                                mail.Bcc.Clear();
                                listToUpdate.Clear();
                            }
                            else if (inc == BulkEmailRecipientsList.FirstOrDefault().Value.Count)
                            {
                                bool result = SendEmailAndUpdateStaffEmailInfo(mail, listToUpdate, comServ, smtp, campusemaildet);
                                if (result == false)
                                    bulkSendCompleteWithError = true;
                                mail.Bcc.Clear();
                                listToUpdate.Clear();
                            }
                            else { }
                        }
                        else
                        {
                            IList<StaffRecipientsEmailInfo> WrongMailUpdate = new List<StaffRecipientsEmailInfo>();
                            WrongMailUpdate.Add(item);
                            comServ.CreateOrUpdateStaffEmailListWithStatus(WrongMailUpdate, "InValid MailId");
                        }

                    }
                    /// Send mail to who are all having Academic Role
                    //  SentMailToAcaDir(cei);
                    /// End
                    if (bulkSendCompleteWithError)
                        cei.Status = "CompletedWithErrors";
                    else cei.Status = "SuccessfullyCompleted";
                    criteria.Clear();
                    criteria.Add("ComposeId", ComposeId);
                    Dictionary<long, IList<RecipientsEmailInfo>> checkSentMails = comServ.GetRecipientsEmailListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                    if (checkSentMails != null && checkSentMails.FirstOrDefault().Value.Count > 0 && checkSentMails.FirstOrDefault().Key > 0)
                    {
                        foreach (var item in checkSentMails.FirstOrDefault().Value)
                        {
                            if (item.Status == "Not Sent" || item.Status == "InProgress") { cei.Status = "PartiallyCompleted"; break; }
                        }
                    }
                    //save this
                    comServ.CreateOrUpdateComposeEmailInfo(cei);
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }


        public ActionResult SuspendEmailInStaffComposeMailInfo(long Id, string Reason)
        {
            CommunicationService comServ = new CommunicationService();
            StaffComposeMailInfo cei = comServ.GetStaffComposeEmailInfoById(Id);
            cei.Suspend = "Suspend";
            cei.Status = "Suspend";
            cei.Reason = Reason;
            comServ.CreateOrUpdateStaffComposeEmailInfo(cei);
            return null;
        }

        public ActionResult BulkEmailStatus()
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
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult JqGridBulkEmailStatus(string BulkReqId, string Subject, string Attachment, string Message, string Status, string CreatedBy, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                //string userId = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                CommunicationService comSer = new CommunicationService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(BulkReqId)) { criteria.Add("BulkReqId", BulkReqId); }
                if (!string.IsNullOrEmpty(Subject)) { criteria.Add("Subject", Subject); }
                if (!string.IsNullOrEmpty(Attachment))
                {
                    if (Attachment == "1") { criteria.Add("Attachment", true); }
                    if (Attachment == "0") { criteria.Add("Attachment", false); }
                }
                if (!string.IsNullOrEmpty(Status))
                {
                    if (Status == "1") { criteria.Add("Status", "Email Composed"); }
                    if (Status == "2") { criteria.Add("Status", "Recipients Added"); }
                    if (Status == "3") { criteria.Add("Status", "CompletedWithErrors"); }
                    if (Status == "4") { criteria.Add("Status", "SuccessfullyCompleted"); }
                    if (Status == "5") { criteria.Add("Status", "Suspend"); }
                }
                if (Session["UserId"].ToString() != null)
                    criteria.Add("CreatedBy", Session["UserId"]);
                if (!string.IsNullOrEmpty(Message)) { criteria.Add("Message", Message); }
                Dictionary<long, IList<StaffComposeMailInfo>> BulkComposeEmailRegList = comSer.GetStaffComposeMailInfoListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                UserService us = new UserService();
                long totalrecords = BulkComposeEmailRegList.First().Key;
                int totalPages = (int)Math.Ceiling(totalrecords / (float)5);
                var AssLst = new
                {
                    total = totalPages,
                    page = page,
                    records = totalrecords,
                    rows = (
                         from items in BulkComposeEmailRegList.First().Value

                         select new
                         {
                             cell = new string[] 
                                         {
                                             items.Id.ToString(),
                                            // items.IdKeyValue.ToString(),
                                             "<a href='/Communication/StaffBulkEmail?Id=" + items.Id+"'>" + items.BulkReqId + "</a>",
                                           //  items.BulkReqId,
                                             items.UserId,
                                             items.Campus,
                                           //  items.AcademicYear,
                                             //items.Father.ToString(),
                                             //items.Mother.ToString(),
                                             items.General.ToString(),
                                             items.Subject,
                                             items.Attachment.ToString(),
                                             "<a  onclick=\"ShowComments('" + items.Id +"' , '"+items.BulkReqId+"');\">" +"..."+ "</a>",
                                            // items.Message,
                                             items.Status,
                                             items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
                                             items.ModifiedBy,
                                             items.CreatedDate.ToString("dd/MM/yyyy"),
                                             items.ModifiedDate.ToString("dd/MM/yyyy"),
                                             items.CreatedBy
                                         }
                         }).ToList()
                };
                return Json(AssLst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        #region Added By Prabakaran for Edit ReceiptsList
        public ActionResult StaffReciptAddingFunction(long IdKeyValue, long ComposeId, string EMail)
        {
            CommunicationService comServ = new CommunicationService();

            StaffRecipientsEmailInfo getlist = comServ.GetStaffRecipientlistById(ComposeId, IdKeyValue);
            getlist.ComposeId = ComposeId;
            getlist.EmailId = EMail;
            //getlist.FatherEmailId = rei.FatherEmailId;
            //getlist.MotherEmailId = rei.MotherEmailId;
            //getlist.GeneralEmailId = rei.GeneralEmailId;
            getlist.RecipientsModifiedDate = DateTime.Now;
            comServ.CreateOrUpdateStaffRecipients(getlist);

            return null;
        }
        public ActionResult StaffReciptDelete(string[] Id, long ComposeId)
        {
            CommunicationService comServ = new CommunicationService();
            var bulkPreregnum = Id[0].Split(',');
            List<long> bulkIdKeyValue = new List<long>();
            foreach (var item in bulkPreregnum) { bulkIdKeyValue.Add(Convert.ToInt64(item)); }

            comServ.DeleteStaffRecipientsList(bulkIdKeyValue, ComposeId);
            return null;
        }
        #endregion
        #endregion
        public ActionResult StaffBulkSMSRequest(long? Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    CommunicationService comSer = new CommunicationService();
                    MastersService ms = new MastersService();
                    AdmissionManagementService ads = new AdmissionManagementService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<SMSTemplate>> smstemplate = ads.GetSMSTemplateListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                    ViewBag.smstemplate = smstemplate.First().Value;
                    criteria.Clear();
                    Dictionary<long, IList<DesignationMaster>> DesignationMaster = ms.GetDesignationMasterListWithPagingAndCriteria(0, 200, string.Empty, string.Empty, criteria);
                    ViewBag.designationddl = DesignationMaster.First().Value;
                    Dictionary<long, IList<StaffDepartmentMaster>> DepartmentMaster = ms.GetStaffDepartmentMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    ViewBag.departmentddl = DepartmentMaster.First().Value;
                    Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    ViewBag.acadddl = AcademicyrMaster.First().Value;
                    StaffComposeSMSInfo smsInfo = new StaffComposeSMSInfo();

                    if (Id > 0)
                    {
                        smsInfo = comSer.GetStaffComposeSMSInfoById(Id ?? 0);
                    }
                    else { }
                    ViewBag.Message = smsInfo.Message;
                    return View(smsInfo);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult StaffBulkSMSRequest(StaffComposeSMSInfo sms)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    CommunicationService comSer = new CommunicationService();
                    if (sms.Id == 0)
                    {
                        sms.CreatedDate = DateTime.Now;
                    }
                    sms.ModifiedDate = DateTime.Now;
                    sms.CreatedBy = userId;
                    long Id = comSer.CreateOrUpdateStaffComposeSMSInfo(sms);
                    sms.SMSReqId = "SMSR-" + Id;
                    sms.Status = "SMS Composed";
                    comSer.CreateOrUpdateStaffComposeSMSInfo(sms);
                    return RedirectToAction("StaffBulkSMSRequest", new { Id = sms.Id });
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult StaffBulkSMSRequestJqGrid(string AdStatus, string NewId, long SMSComposeId, string Campus, string Name, string Status, string Department, string Designation, string saveOrClear, int rows, string sord, string sidx, int? page = 1)
        {
            try
            {
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                CommunicationService comSer = new CommunicationService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<StaffDetailsView>> BulkSMSReqList = null;
                Dictionary<long, IList<StaffSMSRecipientsInfo>> BulkSMSRecList = null;
                StaffComposeSMSInfo smsInfo = new StaffComposeSMSInfo();
                if (SMSComposeId > 0) { smsInfo = comSer.GetStaffComposeSMSInfoById(SMSComposeId); }
                if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                if (!string.IsNullOrEmpty(Status)) { criteria.Add("Status", Status); }
                //if (!string.IsNullOrEmpty(NewId)) { criteria.Add("NewId", NewId); }
                if (!string.IsNullOrEmpty(NewId)) { criteria.Add("IdNumber", NewId); }
                if (!string.IsNullOrEmpty(Name)) { criteria.Add("Name", Name); }
                if (!string.IsNullOrEmpty(Department)) { criteria.Add("Department", Department); }
                if (!string.IsNullOrEmpty(Designation)) { criteria.Add("Designation", Designation); }
                criteria.Add("Status", "Registered");
                criteria.Add("WorkingType", "Staff");
                StaffManagementService sms = new StaffManagementService();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (SMSComposeId > 0 && smsInfo.IsSaveList)
                {
                    criteria.Clear();
                    criteria.Add("SMSComposeId", SMSComposeId);
                    rows = saveOrClear == "Clear" ? 99999 : rows;
                    if (!string.IsNullOrEmpty(Name))
                        BulkSMSRecList = comSer.GetStaffBulkSMSRecipLikeSearchCriteria(page - 1, rows, sidx, sord, criteria);
                    else
                        BulkSMSRecList = comSer.GetStaffSMSListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                }
                else
                {
                    if (!string.IsNullOrEmpty(Campus))
                    {
                        BulkSMSReqList = sms.GetStaffDetailsViewListWithPaging(page - 1, 9999, sord, sidx, criteria);
                    }
                }
                if (saveOrClear == "Save")
                {
                    List<StaffSMSRecipientsInfo> SMSReciplist = new List<StaffSMSRecipientsInfo>();
                    if (BulkSMSReqList != null)
                    {
                        foreach (var item in BulkSMSReqList.FirstOrDefault().Value)
                        {
                            StaffSMSRecipientsInfo SMSRei = new StaffSMSRecipientsInfo();
                            SMSRei.SMSComposeId = Convert.ToInt64(SMSComposeId);
                            //SMSRei.Id = item.Id;
                            SMSRei.PreRegNum = item.PreRegNum;
                            SMSRei.Campus = item.Campus;
                            SMSRei.Name = item.Name;
                            SMSRei.IdNumber = item.IdNumber;
                            SMSRei.Department = item.Department;
                            SMSRei.Designation = item.Designation;
                            SMSRei.RecipientsCreatedDate = DateTime.Now;
                            SMSRei.RecipientsModifiedDate = DateTime.Now;

                            if (!string.IsNullOrEmpty(item.PhoneNo))
                            {
                                item.PhoneNo.Replace(" ", string.Empty);
                                if (item.PhoneNo.Contains(","))
                                {
                                    var MNumber = item.PhoneNo.Split(',');
                                    for (int i = 0; i < MNumber.Length; i++)
                                    {
                                        StaffSMSRecipientsInfo SMSReiSepComma = new StaffSMSRecipientsInfo();
                                        SMSReiSepComma.SMSComposeId = Convert.ToInt64(SMSComposeId);
                                        //SMSReiSepComma.Id = item.Id;
                                        SMSReiSepComma.PreRegNum = item.PreRegNum;
                                        SMSReiSepComma.Campus = item.Campus;
                                        SMSReiSepComma.Name = item.Name;
                                        SMSReiSepComma.IdNumber = item.IdNumber;
                                        SMSReiSepComma.RecipientsCreatedDate = DateTime.Now;
                                        SMSReiSepComma.RecipientsModifiedDate = DateTime.Now;
                                        SMSReiSepComma.Department = item.Department;
                                        SMSReiSepComma.Designation = item.Designation;
                                        SMSReiSepComma.MobileNumber = MNumber[i].Trim();
                                        SMSReiSepComma.Status = "InProgress";
                                        SMSReciplist.Add(SMSReiSepComma);
                                    }
                                }
                                else
                                {
                                    SMSRei.MobileNumber = item.PhoneNo.Trim();
                                    SMSRei.Status = "InProgress";
                                    SMSReciplist.Add(SMSRei);
                                }
                            }
                            else
                            {
                                SMSRei.MobileNumber = item.PhoneNo;
                                SMSRei.Status = "No Number";
                                SMSReciplist.Add(SMSRei);
                            }

                        }
                        comSer.CreateOrUpdateSMSStaffList(SMSReciplist);
                        foreach (StaffDetailsView item in BulkSMSReqList.FirstOrDefault().Value)
                        {
                            item.Status = !string.IsNullOrEmpty(item.PhoneNo) ? "InProgress" : "No Number";
                            //  item.SMSRecipientCreatedDate = DateTime.Now;
                            //item.SMSRecipientCreatedDate = DateTime.Now;
                        }

                        /// Save to the ComposeEmailInfo table
                        smsInfo.IsSaveList = true;
                        //smsInfo.Status = "Recipients Added";

                        if (SMSReciplist[0] != null)
                        {

                            if (Name != "" & !string.IsNullOrEmpty(Name))
                                smsInfo.Name = Name;
                            if (NewId != "" & !string.IsNullOrEmpty(NewId))
                                smsInfo.IdNumber = NewId;
                            smsInfo.Campus = Campus;
                            smsInfo.Status = "Recipients Added";
                        }
                        comSer.CreateOrUpdateStaffComposeSMSInfo(smsInfo);
                    }
                    //if (SMSComposeId > 0 && smsInfo.IsSaveList)
                    //{
                    //    criteria.Clear();
                    //    criteria.Add("SMSComposeId", SMSComposeId);
                    //    rows = saveOrClear == "Clear" ? 99999 : rows;
                    //    if (!string.IsNullOrEmpty(Name))
                    //        BulkSMSRecList = comSer.GetStaffBulkSMSRecipLikeSearchCriteria(page - 1, rows, sidx, sord, criteria);
                    //    else
                    //        BulkSMSRecList = comSer.GetStaffSMSListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    //}
                }
                else if (saveOrClear == "Suspend")
                {
                    smsInfo.Suspended = true;
                    smsInfo.ModifiedDate = DateTime.Now;
                    smsInfo.Status = "Suspended";
                    comSer.CreateOrUpdateStaffComposeSMSInfo(smsInfo);
                }
                else
                {
                    if (saveOrClear == "Clear")
                    {
                        comSer.ClearSavedListInStaffSMSInfo(Convert.ToInt64(SMSComposeId));
                        smsInfo.IsSaveList = false;
                        smsInfo.Campus = null;
                        smsInfo.Department = null;
                        smsInfo.Designation = null;
                        smsInfo.Name = null;
                        smsInfo.Status = "SMS Composed";
                        comSer.CreateOrUpdateStaffComposeSMSInfo(smsInfo);
                        var jsondat = new { rows = (new { cell = new string[] { } }) };
                        return Json(jsondat, JsonRequestBehavior.AllowGet);
                    }
                }
                if (BulkSMSReqList != null && BulkSMSReqList.FirstOrDefault().Value.Count > 0 && BulkSMSReqList.FirstOrDefault().Key > 0)
                {
                    long totalrecords = BulkSMSReqList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in BulkSMSReqList.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                                        items.Id.ToString(),
                                     //   items.IdKeyValue.ToString(),
                                        items.PreRegNum.ToString(),
                                        items.IdNumber,
                                        items.Name,
                                        items.Campus,
                                        items.Department,
                                        items.Designation,
                                        items.Status=="InProgress"? String.Format(@"<p style='color:#0000FF'>{0}</p>",items.Status)
                                        :items.Status=="InValid Number"? String.Format(@"<p style='color:#FF0000'>{0}</p>",items.Status):
                                        items.Status=="Success"? String.Format(@"<p style='color:#00FF00'>{0}</p>",items.Status):items.Status,
                                        items.PhoneNo,
                            }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (BulkSMSRecList != null && BulkSMSRecList.FirstOrDefault().Value.Count > 0 && BulkSMSRecList.FirstOrDefault().Key > 0)
                    {
                        long totalrecords = BulkSMSRecList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in BulkSMSRecList.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                                        items.Id.ToString(),
                                        items.PreRegNum.ToString(),
                                        items.IdNumber,
                                        items.Name,
                                        items.Campus,
                                        items.Department,
                                        items.Designation,
                                        items.Status=="InProgress"? String.Format(@"<p style='color:#0000FF'>{0}</p>",items.Status)
                                        :items.Status=="Number wrong"? String.Format(@"<p style='color:#FF0000'>{0}</p>",items.Status):
                                        items.Status=="Success"? String.Format(@"<p style='color:#00FF00'>{0}</p>",items.Status):items.Status,
                                        items.MobileNumber,
                                        items.RecipientsCreatedDate != null?items.RecipientsCreatedDate.ToString():"",
                                        items.RecipientsModifiedDate != null?items.RecipientsModifiedDate.ToString():"",
                                        items.SentSMSStatusWithTid,
                                        items.SentSMSReportsWithStatus
                                    }
                                    })
                        };
                        return Json(jsondat, JsonRequestBehavior.AllowGet);
                    }
                }
                var jsonda = new { rows = (new { cell = new string[] { } }) };
                return Json(jsonda, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult SendTestSMSFunctionToStaff(string TestNumber, long SMSComposeId)
        {
            string strUrl; WebRequest request; WebResponse response; Stream s; StreamReader readStream;
            CommunicationService comSer = new CommunicationService();
            StaffComposeSMSInfo smsInfo = comSer.GetStaffComposeSMSInfoById(SMSComposeId);
            if (TestNumber.Length < 11)
            {
                if (Regex.IsMatch(TestNumber, @"(?<!\d)\d{10}(?!\d)", RegexOptions.IgnoreCase))
                {
                    if (smsInfo != null)
                    {
                        string dataString = string.Empty;
                        //strUrl = ConfigurationManager.AppSettings["SMSService"].ToString() + "&senderID=TIPSGL&receipientno=" + TestNumber + "&dcs=0&msgtxt=" + smsInfo.Message + "&state=1";
                        strUrl = ConfigurationManager.AppSettings["SMSService"].ToString() + "&senderID=" + GetSenderIdByCampus(smsInfo.Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString()) + "&receipientno=" + TestNumber + "&dcs=0&msgtxt=" + smsInfo.Message + "&state=1";
                        request = WebRequest.Create(strUrl);
                        response = request.GetResponse();
                        s = response.GetResponseStream();
                        readStream = new StreamReader(s);
                        dataString = readStream.ReadToEnd();
                        //  UpdateSMSRecipientsStatus(smsList.Id, "Success", dataString, smsList.SentSMSReportsWithStatus != null ? smsList.SentSMSReportsWithStatus : string.Empty);
                        response.Close();
                        s.Close();
                        readStream.Close();
                        return Json("Successfully Test Message Sent", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StaffCheckSMSCredit(long SMSComposeId)
        {
            try
            {
                string StrCredit = string.Empty;
                string strUrl = ConfigurationManager.AppSettings["SMSCreditService"].ToString() + "&type=0";
                WebRequest request = WebRequest.Create(strUrl);
                WebResponse response = request.GetResponse();
                Stream s = response.GetResponseStream();
                StreamReader readStream = new StreamReader(s);
                string dataString = readStream.ReadToEnd();
                response.Close(); s.Close(); readStream.Close();
                var tempCredit = dataString.Split(',');
                StrCredit = Regex.Replace(tempCredit[1], "[^0-9]+", string.Empty);
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("SMSComposeId", SMSComposeId);
                Dictionary<long, IList<SMSRecipientsInfo>> BulkSMSRecipientList = comSer.GetBulkSMSRecipListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                if (BulkSMSRecipientList != null && BulkSMSRecipientList.Count > 0 && BulkSMSRecipientList.FirstOrDefault().Value != null)
                {
                    var result = from r in BulkSMSRecipientList.FirstOrDefault().Value
                                 where r.MobileNumber != null
                                 select r;
                    if (Convert.ToInt32(StrCredit) >= result.Count()) { return Json("Success", JsonRequestBehavior.AllowGet); }
                    else { return Json("", JsonRequestBehavior.AllowGet); }
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult SendSMSToSelectedStaff(string IdKeyValues, long SMSComposeId)
        {
            try
            {
                if ((SMSComposeId > 0) && (!string.IsNullOrEmpty(IdKeyValues)))
                {
                    CommunicationService comSer = new CommunicationService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    StaffComposeSMSInfo smsInfo = comSer.GetStaffComposeSMSInfoById(SMSComposeId);
                    /// Send SMS to who are all having Acadmic Director role
                    // SendSMSToAcaDir(smsInfo);
                    ///End
                    var RecpIdKeyValue = IdKeyValues.Split(',').ToList();
                    foreach (var item in RecpIdKeyValue)
                    {
                        criteria.Add("Id", Convert.ToInt64(item));
                        criteria.Add("SMSComposeId", SMSComposeId);
                        Dictionary<long, IList<StaffSMSRecipientsInfo>> BulkSMSRecipientList = comSer.GetStaffBulkSMSRecipLikeSearchCriteria(0, 99999, string.Empty, string.Empty, criteria);
                        List<StaffSMSRecipientsInfo> bulkSMSRecipientList = BulkSMSRecipientList.FirstOrDefault().Value.ToList();
                        SendBulkSMSToStaff(bulkSMSRecipientList, smsInfo);
                        criteria.Clear();
                    }
                }
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (System.Net.WebException ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        private void SendBulkSMSToStaff(List<StaffSMSRecipientsInfo> bulkSMSRecipientInfo, StaffComposeSMSInfo smsInfo)
        {
            string strUrl; WebRequest request; WebResponse response; Stream s; StreamReader readStream;
            CommunicationService comSer = new CommunicationService();
            if (bulkSMSRecipientInfo != null & bulkSMSRecipientInfo.Count > 0)
            {
                foreach (var smsList in bulkSMSRecipientInfo)
                {
                    string dataString = string.Empty;
                    if (!string.IsNullOrEmpty(smsList.MobileNumber) && smsList.MobileNumber.Length < 13)
                    {
                        if (smsList.MobileNumber.Length == 12)
                        {
                            if (smsList.MobileNumber[0] == '9' && smsList.MobileNumber[1] == '1')
                            {
                                smsList.MobileNumber = smsList.MobileNumber.Substring(2, 10);
                            }
                        }
                        if (smsList.MobileNumber.Length == 11)
                        {
                            if (smsList.MobileNumber[0] == '0')
                            {
                                smsList.MobileNumber = smsList.MobileNumber.Substring(1, 10);
                            }
                        }
                        if (smsList.MobileNumber.Length < 11)
                        {
                            if (Regex.IsMatch(smsList.MobileNumber, @"(?<!\d)\d{10}(?!\d)", RegexOptions.IgnoreCase))
                            {
                                //Sending SMS
                                //strUrl = ConfigurationManager.AppSettings["SMSService"].ToString() + "&senderID=TIPSGB&receipientno=" + smsList.MobileNumber + "&dcs=0&msgtxt=" + smsInfo.Message + "&state=1";
                                strUrl = ConfigurationManager.AppSettings["SMSService"].ToString() + "&senderID=" + GetSenderIdByCampus(smsInfo.Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString()) + "&receipientno=" + smsList.MobileNumber + "&dcs=0&msgtxt=" + smsInfo.Message + "&state=1";
                                try
                                {
                                    request = WebRequest.Create(strUrl);
                                    response = request.GetResponse();
                                    s = response.GetResponseStream();
                                    readStream = new StreamReader(s);
                                    dataString = readStream.ReadToEnd();
                                    UpdateStaffBulkSMSInfoStatus(smsList.Id, "Success", dataString, smsList.SentSMSReportsWithStatus != null ? smsList.SentSMSReportsWithStatus : string.Empty);
                                    response.Close();
                                    s.Close();
                                    readStream.Close();
                                }
                                catch (Exception ex)
                                {
                                    UpdateStaffBulkSMSInfoStatus(smsList.Id, "Failed", dataString, smsList.SentSMSReportsWithStatus != null ? smsList.SentSMSReportsWithStatus : string.Empty);
                                    smsInfo.Status = "Sending Failure";
                                    smsInfo.BulkSMSSent = false;
                                    comSer.CreateOrUpdateStaffComposeSMSInfo(smsInfo);
                                    ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                                    throw ex;
                                }

                            }
                            else
                            {
                                UpdateStaffBulkSMSInfoStatus(smsList.Id, "InValid Number", dataString, smsList.SentSMSReportsWithStatus != null ? smsList.SentSMSReportsWithStatus : string.Empty);
                            }
                        }

                    }
                    else
                        UpdateStaffBulkSMSInfoStatus(smsList.Id, "InValid Number", dataString, smsList.SentSMSReportsWithStatus != null ? smsList.SentSMSReportsWithStatus : string.Empty);
                }
                smsInfo.ModifiedDate = DateTime.Now;
                smsInfo.BulkSMSSent = true;
                smsInfo.Status = "Message Sent";
                comSer.CreateOrUpdateStaffComposeSMSInfo(smsInfo);
            }
        }

        public bool UpdateStaffBulkSMSInfoStatus(long Id, string SMSStatus, string smsStatusWithTid, string SentSMSReportsWithStatus)
        {
            try
            {
                CommunicationService comSer = new CommunicationService();
                StaffSMSRecipientsInfo smsRecip = comSer.GetStaffSMSDetailsById(Id);
                if (Id > 0 && smsRecip != null)
                {
                    smsRecip.Status = SMSStatus;
                    smsRecip.SentSMSStatusWithTid = smsStatusWithTid;
                    smsRecip.SentSMSReportsWithStatus = SentSMSReportsWithStatus;
                    smsRecip.RecipientsModifiedDate = DateTime.Now;
                    comSer.UpdateStaffSMSStatus(smsRecip);
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
            finally { }
        }

        public ActionResult SendBulkSMSToStaff(long SMSComposeId)
        {
            try
            {
                if (SMSComposeId > 0)
                {
                    StaffComposeSMSInfo smsInfo = new StaffComposeSMSInfo();
                    CommunicationService comSer = new CommunicationService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("SMSComposeId", SMSComposeId);
                    Dictionary<long, IList<StaffSMSRecipientsInfo>> BulkSMSRecipientList = comSer.GetStaffBulkSMSRecipLikeSearchCriteria(0, 99999, string.Empty, string.Empty, criteria);
                    List<StaffSMSRecipientsInfo> BulkSMSRecipientInfo = BulkSMSRecipientList.FirstOrDefault().Value.ToList();
                    smsInfo = comSer.GetStaffComposeSMSInfoById(SMSComposeId);
                    // for sending single sms.....................
                    SendBulkSMSToStaff(BulkSMSRecipientInfo, smsInfo);
                    //getting SMS Status Report from Portal 
                    /// Send SMS to who are all having Acadmic Director role
                    // SendSMSToAcaDir(smsInfo);
                    ///End
                    new Task(() => { GetSentSMSToStaffReportInformation(SMSComposeId); }).Start();
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else { throw new Exception("sms object is required"); }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        private void GetSentSMSToStaffReportInformation(long SMSComposeId)
        {
            string strUrl; WebRequest request; WebResponse response; Stream s; StreamReader readStream;
            strUrl = ConfigurationManager.AppSettings["SMSStatusService"].ToString();
            CommunicationService comSer = new CommunicationService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("SMSComposeId", SMSComposeId);
            Dictionary<long, IList<StaffSMSRecipientsInfo>> BulkSMSRecipientList = comSer.GetStaffBulkSMSRecipLikeSearchCriteria(0, 99999, string.Empty, string.Empty, criteria);
            List<StaffSMSRecipientsInfo> bulkSMSRecipientInfo = BulkSMSRecipientList.FirstOrDefault().Value.ToList();
            if (bulkSMSRecipientInfo != null & bulkSMSRecipientInfo.Count > 0)
            {
                foreach (StaffSMSRecipientsInfo smsRecp in bulkSMSRecipientInfo)
                {
                    string dataString = string.Empty;
                    if (!string.IsNullOrEmpty(smsRecp.SentSMSStatusWithTid))
                    {
                        try
                        {
                            string tId = smsRecp.SentSMSStatusWithTid;
                            var idVal = tId.Split(',');
                            request = WebRequest.Create(strUrl + "&tid=" + idVal[1]);
                            response = request.GetResponse();
                            s = response.GetResponseStream();
                            readStream = new StreamReader(s);
                            dataString = readStream.ReadToEnd();
                            UpdateStaffBulkSMSInfoStatus(smsRecp.Id, smsRecp.Status, smsRecp.SentSMSStatusWithTid, dataString);
                            response.Close();
                            s.Close();
                            readStream.Close();
                        }
                        catch (Exception)
                        {
                            UpdateStaffBulkSMSInfoStatus(smsRecp.Id, smsRecp.Status, smsRecp.SentSMSStatusWithTid, string.Empty);
                        }
                    }
                    else
                    {
                        UpdateStaffBulkSMSInfoStatus(smsRecp.Id, smsRecp.Status, smsRecp.SentSMSStatusWithTid, string.Empty);
                    }
                }
            }
        }

        public ActionResult StaffSMSSuspend(long SMSComposeId)
        {
            try
            {
                ViewBag.ComposeId = SMSComposeId;
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }

        }

        public ActionResult StaffBulkSMSRequestSuspend(long SMSComposeId, string SReason, string smsSusp)
        {
            try
            {
                CommunicationService Comsc = new CommunicationService();
                StaffComposeSMSInfo smsInfo = new StaffComposeSMSInfo();
                if (SMSComposeId > 0) { smsInfo = Comsc.GetStaffComposeSMSInfoById(SMSComposeId); }
                if (smsInfo != null)
                {
                    if (smsSusp == "Suspend")
                    {
                        smsInfo.Suspended = true;
                        smsInfo.ReasonForSuspend = SReason;
                        smsInfo.ModifiedDate = DateTime.Now;
                        smsInfo.Status = "Suspended";
                        Comsc.CreateOrUpdateStaffComposeSMSInfo(smsInfo);
                    }
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                return Json("0", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }

        }

        public ActionResult StaffSMSSendingLog()
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
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }

        }

        public ActionResult SMSRequestLogForStaff(string BulkReqId, string SMSReqId, string Campus, string SMSTemplate, string Status, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                CommunicationService comSer = new CommunicationService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();

                if (!string.IsNullOrEmpty(BulkReqId)) { criteria.Add("SMSReqId", BulkReqId); }
                if (!string.IsNullOrEmpty(SMSTemplate)) { criteria.Add("SMSTemplate", SMSTemplate); }
                if (!string.IsNullOrEmpty(Status)) { criteria.Add("Status", Status); }
                if (!string.IsNullOrEmpty(SMSReqId)) { criteria.Add("SMSReqId", SMSReqId); }
                if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }

                if (Session["UserId"].ToString() != null)
                    criteria.Add("CreatedBy", Session["UserId"]);
                Dictionary<long, IList<StaffComposeSMSInfo>> BulkSMSRegList = comSer.GetStaffComposeSMSListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (BulkSMSRegList != null && BulkSMSRegList.Count > 0 && BulkSMSRegList.FirstOrDefault().Value != null)
                {
                    long totalrecords = BulkSMSRegList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)5);
                    var AssLst = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (
                             from items in BulkSMSRegList.First().Value

                             select new
                             {
                                 cell = new string[] 
                                         {
                                             items.Id.ToString(),
                                             "<a href='/Communication/StaffBulkSMSRequest?Id=" + items.Id+"'>" + items.SMSReqId + "</a>",
                                             items.Campus,
                                             items.SMSTemplate,
                                             items.SMSTemplateValue,
                                             items.Message,
                                             items.Status,
                                             items.CreatedBy,
                                             items.ModifiedBy,
                                             items.CreatedDate.ToString("dd/MM/yyyy"),
                                             items.ModifiedDate.ToString("dd/MM/yyyy")
                                         }
                             }).ToList()
                    };
                    return Json(AssLst, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult StaffBulkSMSRequestReport(long ComposeId)
        {
            string userId = base.ValidateUser();
            if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
            else
            {
                CommunicationService comSer = new CommunicationService();
                StaffComposeSMSInfo smsInfo = comSer.GetStaffComposeSMSInfoById(ComposeId);
                StaffBulkSMSRequestReport_vw SMSRep = comSer.GetStaffSMSReportInfoBySMSComposeId(smsInfo.Id);
                //smsInfo.CreatedDate = smsInfo.CreatedDate.ToShortDateString();
                smsInfo.Sent = SMSRep.Sent;
                smsInfo.NotValid = SMSRep.NotValid;
                smsInfo.Failed = SMSRep.Failed;
                smsInfo.UnDelivered = SMSRep.NotDelivered;
                smsInfo.Total = SMSRep.Total;
                return View(smsInfo);
            }
        }

        public ActionResult GetStaffSMSStatusReportChart(long ComposeId)
        {
            if (ComposeId > 0)
            {
                CommunicationService comSer = new CommunicationService();
                StaffBulkSMSRequestReport_vw SMSRep = comSer.GetStaffSMSReportInfoBySMSComposeId(ComposeId);
                var SMSRepPieChart = "<graph caption='' showValues='0'>";
                SMSRepPieChart = SMSRepPieChart + " <set name='Sent' value='" + SMSRep.Sent + "' color='007A00' />";
                SMSRepPieChart = SMSRepPieChart + " <set name='Not Valid Number' value='" + SMSRep.NotValid + "' color='4A0093' />";
                SMSRepPieChart = SMSRepPieChart + " <set name='Failed' value='" + SMSRep.Failed + "' color='FF0000' />";
                SMSRepPieChart = SMSRepPieChart + " <set name='Not Delivered' value='" + SMSRep.NotDelivered + "' color='0066FF' /></graph>";
                Response.Write(SMSRepPieChart);
                return null;
            }
            return null;
        }

        public ActionResult EditStaffSMSRecipient(StaffSMSRecipientsInfo RecipList, long SMSComposeId)
        {
            CommunicationService comSer = new CommunicationService();
            StaffSMSRecipientsInfo getinfo = comSer.GetStaffSMSDetailsById(RecipList.Id);
            if (getinfo != null)
            {
                getinfo.SMSComposeId = SMSComposeId;
                getinfo.RecipientsModifiedDate = DateTime.Now;
                getinfo.MobileNumber = RecipList.MobileNumber;
                comSer.UpdateStaffSMSStatus(getinfo);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public ActionResult DeleteStaffSMSRecip(string Id, long SMSComposeId)
        {
            CommunicationService comSer = new CommunicationService();
            var IdKeyValues = Id.Split(',');
            List<long> PreNo = new List<long>();
            foreach (var item in IdKeyValues) { PreNo.Add(Convert.ToInt64(item)); }
            comSer.DeleteStaffSMSRecipientsList(PreNo, SMSComposeId);
            return null;
        }
        #endregion
        #region SMSCountReport
        public ActionResult SMSCountReport()
        {
            try
            {
                #region BreadCrumb
                string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                #endregion
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffMgmntPolicy");
                throw ex;
            }
        }
        public ActionResult GetSMSCount_SPListJqGrid(string Campus, string FromDate, string ToDate, int rows, string sidx, string sord, int? page = 1, long? ExptXl = 0)
        {
            try
            {
                CommunicationService comSer = new CommunicationService();
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                Dictionary<long, IList<SMSCount_SP>> SMSCount_SP = null;
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
                SMSCount_SP = comSer.GetSMSCountDetailsListbySP(Campus, FromDateNew, ToDatenew);
                if (ExptXl == 1)
                {
                    base.ExptToXL(SMSCount_SP.FirstOrDefault().Value, "SMSCountReport" + "-On-" + DateTime.Today.ToString("dd/MM/yyyy"), (item => new
                    {
                        item.Campus,
                        item.Sent,
                        item.Failed,
                        Not_Failed = item.NotValid,
                        Not_Delivered = item.NotDelivered,
                        DND_Applied = item.DNDApplied,
                        item.Total
                    }));
                    return new EmptyResult();
                }
                else if (SMSCount_SP == null || SMSCount_SP.FirstOrDefault().Key == 0)
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    IList<SMSCount_SP> SMSCountlist = SMSCount_SP.FirstOrDefault().Value;
                    long totalRecords = SMSCount_SP.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows =
                        (
                        from items in SMSCountlist
                        select new
                        {
                            i = items.Id,
                            cell = new string[]
                           {                                
                                items.Id.ToString(),
                                items.Campus.ToString(),
                                items.Sent.ToString(),
                                items.Failed.ToString(),
                                items.NotValid.ToString(),
                                items.NotDelivered.ToString(),
                                items.DNDApplied.ToString(),
                                items.Total.ToString()
                           }
                        }
                        )
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        #endregion
        #region Added By Prabakaran
        public ActionResult ShowEmailRecipientsDetails(string ComposeId, string Status)
        {
            try
            {
                ViewBag.ComposeId = ComposeId;
                ViewBag.Status = Status;
                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult ShowEmailRecipientsDetailsJqgrid(string ComposeId, string Status, RecipientsEmailInfo rei, int rows, string sidx, string sord, int? page = 1, long? ExptXl = 0)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<string, object> likecriteria = new Dictionary<string, object>();
                    criteria.Clear();
                    if (!string.IsNullOrEmpty(Status))
                    {
                        if (Status == "Total")
                        {
                        }
                        else
                        {
                            criteria.Add("Status", Status);
                        }
                    }
                    if (!string.IsNullOrEmpty(ComposeId))
                    {
                        criteria.Add("ComposeId", Convert.ToInt64(ComposeId));
                    }
                    if (rei != null)
                    {
                        if (!string.IsNullOrEmpty(rei.NewId))
                        {
                            criteria.Add("NewId", rei.NewId);
                        }
                        if (!string.IsNullOrEmpty(rei.Name))
                        {
                            likecriteria.Add("Name", rei.Name);
                        }
                        if (!string.IsNullOrEmpty(rei.Grade))
                        {
                            criteria.Add("Grade", rei.Grade);
                        }
                        if (!string.IsNullOrEmpty(rei.Section))
                        {
                            criteria.Add("Section", rei.Section);
                        }
                        //if (!string.IsNullOrEmpty(rei.IsHosteller))
                        //{
                        //    criteria.Add("IsHosteller",rei.IsHosteller);
                        //}
                    }
                    Dictionary<long, IList<RecipientsEmailInfo>> RecipientsEmailList = comSer.GetRecipientsEmailListWithExactAndLikeSearchCriteria(page - 1, rows, sidx, sord, criteria, likecriteria);
                    if (ExptXl == 1)
                    {
                        base.ExptToXL(RecipientsEmailList.FirstOrDefault().Value, "EmailRecipientsList-" + DateTime.Today.ToShortDateString(), (items => new
                        {
                            Compose_Request_Id = "BER-" + items.ComposeId.ToString(),
                            items.NewId,
                            items.Name,
                            items.Campus,
                            items.Grade,
                            items.Section,
                            items.FeeStructYear,
                            items.AdmissionStatus,
                            items.AcademicYear,
                            Is_Hosteller = items.IsHosteller == "True" ? "Yes" : "No",
                            items.VanNo,
                            items.Status,
                            items.FamilyDetailType,
                            items.U_EmailId,
                            Applied_Date = items.AppliedDate.ToString("dd'/'MM'/'yyyy"),
                            Created_Date = items.RecipientsCreatedDate != null ? items.RecipientsCreatedDate.Value.ToString("dd'/'MM'/'yyyy") : "",
                            Modified_Date = items.RecipientsModifiedDate != null ? items.RecipientsModifiedDate.Value.ToString("dd'/'MM'/'yyyy") : "",
                        }));
                        return new EmptyResult();
                    }
                    else if (RecipientsEmailList != null && RecipientsEmailList.Count > 0)
                    {
                        long totalrecords = RecipientsEmailList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in RecipientsEmailList.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[]{
                                       "BER-"+items.ComposeId.ToString(),                                       
                                        items.NewId,
                                        items.Name,
                                        items.Campus,
                                        items.Grade,
                                        items.Section,
                                        items.FeeStructYear,
                                        items.AdmissionStatus,
                                        items.AcademicYear,
                                        items.IsHosteller=="True"?"Yes":"No",
                                        items.VanNo,
                                        items.Status,
                                        items.FamilyDetailType,
                                        items.U_EmailId,
                                        items.AppliedDate.ToString("dd'/'MM'/'yyyy"),                                        
                                        items.RecipientsCreatedDate != null?items.RecipientsCreatedDate.Value.ToString("dd'/'MM'/'yyyy"):"",
                                        items.RecipientsModifiedDate != null?items.RecipientsModifiedDate.Value.ToString("dd'/'MM'/'yyyy"):"",
                                       }
                                    })

                        };
                        return Json(jsondat, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var Empty = new { rows = (new { cell = new string[] { } }) };
                        return Json(Empty, JsonRequestBehavior.AllowGet);
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        #endregion

        //public Boolean SendRawEmail(String from, String to, String cc, String Subject, String text, String html, String replyTo, string attachPath)
        //{
        //    AlternateView plainView = AlternateView.CreateAlternateViewFromString(text, Encoding.UTF8, "text/plain");
        //    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(html, Encoding.UTF8, "text/html");

        //    MailMessage mailMessage = new MailMessage();

        //    mailMessage.From = new MailAddress(from);

        //    List<String> toAddresses = to.Replace(", ", ",").Split(',').ToList();

        //    foreach (String toAddress in toAddresses)
        //    {
        //        mailMessage.To.Add(new MailAddress(toAddress));
        //    }

        //    List<String> ccAddresses = cc.Replace(", ", ",").Split(',').Where(y => y != "").ToList();

        //    foreach (String ccAddress in ccAddresses)
        //    {
        //        mailMessage.CC.Add(new MailAddress(ccAddress));
        //    }

        //    mailMessage.Subject = Subject;
        //    mailMessage.SubjectEncoding = Encoding.UTF8;

        //    if (replyTo != null)
        //    {
        //        mailMessage.ReplyToList.Add(new MailAddress(replyTo));
        //    }

        //    if (text != null)
        //    {
        //        mailMessage.AlternateViews.Add(plainView);
        //    }

        //    if (html != null)
        //    {
        //        mailMessage.AlternateViews.Add(htmlView);
        //    }

        //    if (attachPath.Trim() != "")
        //    {
        //        if (System.IO.File.Exists(attachPath))
        //        {
        //            System.Net.Mail.Attachment objAttach = new System.Net.Mail.Attachment(attachPath);
        //            objAttach.ContentType = new ContentType("application/octet-stream");
        //            System.Net.Mime.ContentDisposition disposition = objAttach.ContentDisposition;
        //            disposition.DispositionType = "attachment";
        //            disposition.CreationDate = System.IO.File.GetCreationTime(attachPath);
        //            disposition.ModificationDate = System.IO.File.GetLastWriteTime(attachPath);
        //            disposition.ReadDate = System.IO.File.GetLastAccessTime(attachPath);
        //            mailMessage.Attachments.Add(objAttach);
        //        }
        //    }

        //    RawMessage rawMessage = new RawMessage();

        //    using (MemoryStream memoryStream = ConvertMailMessageToMemoryStream(mailMessage))
        //    {
        //        rawMessage.WithData(memoryStream);
        //    }

        //    SendRawEmailRequest request = new SendRawEmailRequest();
        //    request.WithRawMessage(rawMessage);

        //    request.WithDestinations(toAddresses);
        //    request.WithSource(from);

        //    AmazonSimpleEmailService ses = AWSClientFactory.CreateAmazonSimpleEmailServiceClient(ConfigurationManager.AppSettings.Get("AccessKeyId"), ConfigurationManager.AppSettings.Get("SecretKeyId"));

        //    try
        //    {
        //        SendRawEmailResponse response = ses.SendRawEmail(request);
        //        SendRawEmailResult result = response.SendRawEmailResult;
        //        return true;
        //    }
        //    catch (AmazonSimpleEmailServiceException ex)
        //    {
        //        return false;
        //    }
        //}
        //public static MemoryStream ConvertMailMessageToMemoryStream(MailMessage message)
        //{
        //    Assembly assembly = typeof(SmtpClient).Assembly;
        //    Type mailWriterType = assembly.GetType("System.Net.Mail.MailWriter");
        //    MemoryStream fileStream = new MemoryStream();
        //    ConstructorInfo mailWriterContructor = mailWriterType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(Stream) }, null);
        //    object mailWriter = mailWriterContructor.Invoke(new object[] { fileStream });
        //    MethodInfo sendMethod = typeof(MailMessage).GetMethod("Send", BindingFlags.Instance | BindingFlags.NonPublic);
        //    sendMethod.Invoke(message, BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { mailWriter, true }, null);
        //    MethodInfo closeMethod = mailWriter.GetType().GetMethod("Close", BindingFlags.Instance | BindingFlags.NonPublic);
        //    closeMethod.Invoke(mailWriter, BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] { }, null);
        //    return fileStream;
        //}

    }
}
