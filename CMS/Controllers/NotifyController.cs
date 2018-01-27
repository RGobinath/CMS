using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TIPS.Entities;
using TIPS.Entities.AdmissionEntities;
using TIPS.Entities.ParentPortalEntities;
using TIPS.Service;
using TIPS.ServiceContract;

namespace CMS.Controllers
{
    public class NotifyController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult FillCampusList()
        {
            string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
            UserService us = new UserService();
            //pass userid and get the list of roles from user service GetAppRoleForAnUserListWithPagingAndCriteria
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("UserId", userid);
            Dictionary<long, IList<UserAppRole>> userAppRole = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);
            if (userAppRole != null && userAppRole.First().Value != null && userAppRole.First().Value.Count > 0)
            {
                var BranchCodeList = (
                         from items in userAppRole.First().Value
                         where items.UserId == userid && items.BranchCode != null
                         select new
                         {
                             Text = items.BranchCode,
                             Value = items.BranchCode
                         }).Distinct().ToList();
                return Json(BranchCodeList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        /* Start of Notification */

        public ActionResult notify()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    int rows = 9999;
                    string sidx = "Id";
                    string sord = "desc";
                    int? page = 1;
                    ParentPortalService pps = new ParentPortalService();
                    Notification note = new Notification();
                    Session["Time"] = String.Format("{0:h:MM tt}", note.PublishDate);
                    note.PublishDate = "";
                    Session["attachment"] = "";
                    Session["attachmentnames"] = "";
                    var maxnoteid = pps.GetMaxNoteId();
                    note.NotePreId = maxnoteid + 1;
                    pps.UpdateNoteCount(note.NotePreId);
                    note.NoteType = "General";
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Notification n = new Notification();
                    string[] pubToarray = { "General", "Parent", "Student" };
                    criteria.Add("Status", "1");
                    Dictionary<long, IList<Notification>> nList = pps.GetNoteTypesearchCriteriaAlias(page - 1, rows, sidx, sord, "PublishTo", pubToarray, criteria, null);
                    string cD = DateTime.Now.ToString("dd-M-yyyy");
                    long len = nList.Count();
                    if (len > 0)
                    {
                        string[] expDate = (from items in nList.First().Value select items.ExpireDate).ToArray();

                        long[] expID = (from items in nList.First().Value select items.Id).ToArray();

                        long[] notepreId = (from items in nList.First().Value select items.NotePreId).ToArray();

                        // checking for the valid notification, setting the valid notification by expiredate

                        for (var j = 0; j < expDate.Count(); j++)
                        {
                            DateTime edate = DateTime.ParseExact(expDate[j], "dd-M-yyyy", CultureInfo.InvariantCulture);

                            long eID = expID[j];

                            long npID = notepreId[j];

                            int vDate = DateTime.Compare(DateTime.Now, edate);

                            if (vDate > 0)
                            {
                                pps.SetValidNotificaionByExpireDate(eID);
                                pps.SetValidShowNotificationByExpireDate(npID);
                            }
                            else if (vDate == 0)
                            {
                                pps.SetValidNotificationByPublishDate(eID);
                                pps.SetValidShowNotificationByPublishDate(npID);
                            }

                        }
                    }
                    MastersService ms = new MastersService();
                    criteria.Clear();
                    Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    ViewBag.acadddl = AcademicyrMaster.First().Value;
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    return View(note);
                }
            }

            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult notify(Notification n1, FormCollection fc, HttpPostedFileBase file2)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    userId = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                    long id = 0;
                    ParentPortalService pps = new ParentPortalService();
                    ShowNotification showNote = new ShowNotification();
                    if (ModelState.IsValid)
                    {
                        if (Request.Form["btnSubmit"] == "Submit")
                        {
                            if (id == 0)
                            {
                                n1.Performer = userId;
                                n1.Status = "1"; // 1 for unread - 0 for read
                                DateTime pubdate = DateTime.ParseExact(n1.PublishDate, "dd-M-yyyy", CultureInfo.InvariantCulture);
                                DateTime curDate = DateTime.Today;
                                int setValid = DateTime.Compare(curDate, pubdate);
                                if (setValid == 0)
                                {
                                    n1.Valid = "1";
                                }
                                else
                                {
                                    n1.Valid = "0";
                                }
                                n1.NewIds = "";
                                n1.PublishedOn = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
                                ViewBag.SuccessNotifyCreation = 1;
                                n1.Grade = Request.Form["Grade"];
                                n1.Section = Request.Form["Section"];
                                n1.Type = "Notification";
                                pps.CreateOrUpdateNotification(n1);
                                TempData["SuccessNotifyCreation"] = n1.Id;
                                return RedirectToAction("Notify", new { id = n1.Id });
                            }
                        }
                    }
                    return View(n1);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        public ActionResult MailAttachments2(long NotePreId, HttpPostedFileBase file2)
        {
            // System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            string[] strAttachname = file2.FileName.Split('\\');
            Attachment noteAttach = new Attachment(file2.InputStream, strAttachname[strAttachname.Length - 1]);  //Data posted from form

            string path = file2.InputStream.ToString();
            byte[] imageSize = new byte[file2.ContentLength];
            file2.InputStream.Read(imageSize, 0, (int)file2.ContentLength);
            NoteAttachment na = new NoteAttachment();
            na.Attachment = imageSize;
            na.AttachmentName = strAttachname.First().ToString();
            na.NotePreId = NotePreId;
            ParentPortalService pps = new ParentPortalService();
            pps.CreateOrUpdateNoteAttachment(na);

            Dictionary<string, object> criteria3 = new Dictionary<string, object>();
            var maxattachemntid = pps.GetMaxAttachmentId();

            if ((Session["attachment"].ToString() != ""))
            {
                Session["attachment"] = Session["attachment"] + "," + maxattachemntid;
            }
            else
            {
                Session["attachment"] = maxattachemntid;
            }

            Session["attachmentnames"] = strAttachname.First().ToString();
            return Json(new { success = true, result = strAttachname.First().ToString() }, "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult MailAttachments3(long NotePreId, HttpPostedFileBase file3)
        {
            // System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            string[] strAttachname = file3.FileName.Split('\\');
            Attachment noteAttach = new Attachment(file3.InputStream, strAttachname[strAttachname.Length - 1]);  //Data posted from form

            string path = file3.InputStream.ToString();
            byte[] imageSize = new byte[file3.ContentLength];
            file3.InputStream.Read(imageSize, 0, (int)file3.ContentLength);
            NoteAttachment na = new NoteAttachment();
            na.Attachment = imageSize;
            na.AttachmentName = strAttachname.First().ToString();
            na.NotePreId = NotePreId;
            ParentPortalService pps = new ParentPortalService();
            pps.CreateOrUpdateNoteAttachment(na);

            Dictionary<string, object> criteria3 = new Dictionary<string, object>();
            var maxattachemntid = pps.GetMaxAttachmentId();

            if ((Session["attachment"].ToString() != ""))
            {
                Session["attachment"] = Session["attachment"] + "," + maxattachemntid;
            }
            else
            {
                Session["attachment"] = maxattachemntid;
            }

            Session["attachmentnames"] = strAttachname.First().ToString();
            return Json(new { success = true, result = strAttachname.First().ToString() }, "text/html", JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteAttachment()
        {
            try
            {
                ParentPortalService pps = new ParentPortalService();
                if (Session["attachment"].ToString() != "")
                {
                    var test = Session["attachment"].ToString().Split(',');

                    long[] idtodelete = new long[test.Length];
                    int i = 0;
                    foreach (string val in test)
                    {
                        idtodelete[i] = Convert.ToInt64(val);
                        pps.DeleteAttachment(idtodelete[i]);
                        i++;
                    }

                    Session["attachment"] = "";
                    Session["attachmentnames"] = "";
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteAtt(long delid)
        {
            try
            {
                ParentPortalService pps = new ParentPortalService();

                long did = Convert.ToInt64(delid);

                if (did != 0 && did > 0)
                {
                    pps.DeleteAttachment(did);
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        public JsonResult Documentsjqgrid(long Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                DocumentsService ds = new DocumentsService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("EntityRefId", Id);
                criteria.Add("AppName", "NOTE");
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
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        public ActionResult displayAtt(long Id)
        {
            try
            {
                int rows = 5;
                string sidx = "";
                string sord = "desc";
                int? page = 1;

                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ParentPortalService pps = new ParentPortalService();

                    Dictionary<string, object> Doccriteria = new Dictionary<string, object>();

                    Doccriteria.Add("Id", Id);

                    Dictionary<long, IList<NoteAttachment>> docList = pps.GetDocumentsListWithPaging(page - 1, rows, sidx, sord, Doccriteria);

                    long nCount = docList.Count;

                    long len = docList.First().Value.Count();


                    if (docList != null && docList.FirstOrDefault().Value != null)
                    {

                        IList<NoteAttachment> list = docList.FirstOrDefault().Value;
                        NoteAttachment doc = list.FirstOrDefault();
                        if (doc.Attachment != null)
                        {
                            int startIndx = Convert.ToInt32(doc.AttachmentName.LastIndexOf(".").ToString());
                            int FileLength = Convert.ToInt32(doc.AttachmentName.Length);
                            string fileExtn = doc.AttachmentName.Substring(startIndx, (FileLength - startIndx));
                            return File(doc.Attachment, GetContentTypeByFileExtension(fileExtn), doc.AttachmentName);
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
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
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
                    criteria.Add("AppName", "NOTE");
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
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        public ActionResult StudentMgt(string pagename)
        {
            try
            {
                if (pagename != null)
                {
                    Session["pagename"] = pagename.ToString();
                }

                Session["campus"] = "";

                StudentTemplate st = new StudentTemplate();

                MastersService ms = new MastersService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();

                Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                Dictionary<long, IList<SectionMaster>> SectionMaster = ms.GetSectionMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);

                ViewBag.gradeddl1 = GradeMaster.First().Value;
                ViewBag.acadddl = AcademicyrMaster.First().Value;
                ViewBag.sectionddl = SectionMaster.First().Value;

                if (Session["registered"] != null && Session["registered"].ToString() == "yes")
                {
                    ViewBag.Registered = "yes";
                    ViewBag.RegId = Session["regid"];  // regid;
                    Session["registered"] = "";
                    Session["regid"] = "";
                }
                Session["editid"] = 0;
                Session["idnum"] = "";
                string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                //  editid = 0;
                return View(st);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        //public ActionResult SpecificNotification(string publishto, string PreRegNo, string NewId, string campus, string AcademicYear)
        //{
        //    try
        //    {
        //        ParentPortalService pps = new ParentPortalService();

        //        Notification note = new Notification();

        //        Session["Time"] = String.Format("{0:h:MM tt}", note.PublishDate);

        //        note.PublishDate = "";


        //        note.PublishTo = publishto;

        //        Session["attachment"] = "";
        //        Session["attachmentnames"] = "";

        //        note.NewIds = NewId;
        //        note.PreRegNos = PreRegNo;
        //        note.Campus = campus;
        //        note.AcademicYear = AcademicYear;

        //        var maxnoteid = pps.GetMaxNoteId();

        //        note.NotePreId = maxnoteid + 1;

        //        pps.UpdateNoteCount(note.NotePreId);

        //        return View(note);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
        //        throw ex;
        //    }
        //}
        public ActionResult SpecificNotification(string PreRegNo, string NewId, string campus, string AcademicYear)
        {
            try
            {
                ParentPortalService pps = new ParentPortalService();

                Notification note = new Notification();

                Session["Time"] = String.Format("{0:h:MM tt}", note.PublishDate);
                note.PublishDate = "";
                Session["attachment"] = "";
                Session["attachmentnames"] = "";

                note.NewIds = NewId;
                note.PreRegNos = PreRegNo;
                note.Campus = campus;
                note.AcademicYear = AcademicYear;

                var maxnoteid = pps.GetMaxNoteId();

                note.NotePreId = maxnoteid + 1;

                pps.UpdateNoteCount(note.NotePreId);

                return View(note);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }
        public ActionResult SpecificNotification1(string publishto, string PreRegNo, string NewId, string campus)
        {
            try
            {
                ParentPortalService pps = new ParentPortalService();

                Notification note = new Notification();

                Session["Time"] = String.Format("{0:h:MM tt}", note.PublishDate);

                note.PublishDate = "";


                note.PublishTo = publishto;

                Session["attachment"] = "";
                Session["attachmentnames"] = "";

                note.NewIds = NewId;
                note.PreRegNos = PreRegNo;
                note.Campus = campus;

                var maxnoteid = pps.GetMaxNoteId();

                note.NotePreId = maxnoteid + 1;

                pps.UpdateNoteCount(note.NotePreId);

                return View(note);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SpecificNotification(Notification n1, FormCollection fc, HttpPostedFileBase file2)
        {
            try
            {
                string userId = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                long id = 0;
                ParentPortalService pps = new ParentPortalService();
                ShowNotification showNote = new ShowNotification();
                if (ModelState.IsValid)
                {
                    if (Request.Form["btnSubmit"] == "Submit")
                    {
                        if (id == 0)
                        {
                            if (n1.PublishTo == "Parent")
                            {
                                n1.Performer = userId;
                                n1.Status = "1"; // 1 for unread - 0 for read
                                n1.PublishedOn = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
                                DateTime pubdate = DateTime.ParseExact(n1.PublishDate, "dd-M-yyyy", CultureInfo.InvariantCulture);
                                DateTime curDate = DateTime.Today;
                                int setValid = DateTime.Compare(curDate, pubdate);
                                if (setValid == 0) { n1.Valid = "1"; }
                                else { n1.Valid = "0"; }
                                string[] newIdsList = n1.NewIds.Split(',');
                                long newIdCount = newIdsList.Count();
                                if (newIdCount > 0)
                                {
                                    for (int i = 0; i < newIdCount; i++)
                                    {
                                        showNote.NotePreId = n1.NotePreId;
                                        showNote.NoteValid = n1.Valid;
                                        showNote.ViewStatus = "0"; // 0 - unread, 1- read
                                        showNote.NewId = newIdsList[i];
                                        if (showNote.NewId != "") { pps.CreateShowNotification(showNote); }
                                    }
                                }
                                ViewBag.SuccessNotifyCreation = 1;
                                pps.CreateOrUpdateNotification(n1);
                                string noteTopic = n1.Topic;
                                string noteExpDate = n1.ExpireDate;
                                string noteCampus = n1.Campus;
                                string selPreRegNums = n1.PreRegNos;
                                SendNotificationEmail(noteTopic, selPreRegNums, noteCampus);
                                TempData["SuccessNotifyCreation"] = n1.Id;
                                return RedirectToAction("Notify", new { id = n1.Id });
                            }
                            if (n1.PublishTo == "Student")
                            {
                                n1.Performer = userId;
                                n1.Status = "1"; // 1 for unread - 0 for read
                                n1.PublishedOn = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
                                DateTime pubdate = DateTime.ParseExact(n1.PublishDate, "dd-M-yyyy", CultureInfo.InvariantCulture);
                                DateTime curDate = DateTime.Today;
                                int setValid = DateTime.Compare(curDate, pubdate);
                                if (setValid == 0) { n1.Valid = "1"; }
                                else { n1.Valid = "0"; }
                                string[] newIdsList = n1.NewIds.Split(',');
                                long newIdCount = newIdsList.Count();
                                if (newIdCount > 0)
                                {
                                    for (int i = 0; i < newIdCount; i++)
                                    {
                                        showNote.NotePreId = n1.NotePreId;
                                        showNote.NoteValid = n1.Valid;
                                        showNote.ViewStatus = "0"; // 0 - unread, 1- read
                                        showNote.NewId = newIdsList[i];
                                        if (showNote.NewId != "") { pps.CreateShowNotification(showNote); }
                                    }
                                }
                                ViewBag.SuccessNotifyCreation = 1;

                                pps.CreateOrUpdateNotification(n1);
                                string noteTopic = n1.Topic;
                                string noteExpDate = n1.ExpireDate;
                                string noteCampus = n1.Campus;
                                string selPreRegNums = n1.PreRegNos;
                                TempData["SuccessNotifyCreation"] = n1.Id;
                                return RedirectToAction("Notify", new { id = n1.Id });
                            }
                        }
                    }
                }
                return View(n1);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }
        //public ActionResult SpecificNotification(Notification n1, FormCollection fc, HttpPostedFileBase file2)
        //{
        //    try
        //    {
        //        string userId = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
        //        long id = 0;
        //        ParentPortalService pps = new ParentPortalService();
        //        ShowNotification showNote = new ShowNotification();
        //        if (ModelState.IsValid)
        //        {
        //            if (Request.Form["btnSubmit"] == "Submit")
        //            {
        //                if (id == 0)
        //                {
        //                    if (n1.PublishTo == "Parent")
        //                    {
        //                        n1.Performer = userId;
        //                        n1.Status = "1"; // 1 for unread - 0 for read
        //                        n1.PublishedOn = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
        //                        DateTime pubdate = DateTime.ParseExact(n1.PublishDate, "dd-M-yyyy", CultureInfo.InvariantCulture);
        //                        DateTime curDate = DateTime.Today;
        //                        int setValid = DateTime.Compare(curDate, pubdate);
        //                        if (setValid == 0) { n1.Valid = "1"; }
        //                        else { n1.Valid = "0"; }
        //                        string[] newIdsList = n1.NewIds.Split(',');
        //                        long newIdCount = newIdsList.Count();
        //                        if (newIdCount > 0)
        //                        {
        //                            for (int i = 0; i < newIdCount; i++)
        //                            {
        //                                showNote.NotePreId = n1.NotePreId;
        //                                showNote.NoteValid = n1.Valid;
        //                                showNote.ViewStatus = "0"; // 0 - unread, 1- read
        //                                showNote.NewId = newIdsList[i];
        //                                if (showNote.NewId != "") { pps.CreateShowNotification(showNote); }
        //                            }
        //                        }
        //                        ViewBag.SuccessNotifyCreation = 1;
        //                        pps.CreateOrUpdateNotification(n1);
        //                        string noteTopic = n1.Topic;
        //                        string noteExpDate = n1.ExpireDate;
        //                        string noteCampus = n1.Campus;
        //                        string selPreRegNums = n1.PreRegNos;
        //                        SendNotificationEmail(noteTopic, selPreRegNums, noteCampus);
        //                        TempData["SuccessNotifyCreation"] = n1.Id;
        //                        return RedirectToAction("Notify", new { id = n1.Id });
        //                    }
        //                    if (n1.PublishTo == "Student")
        //                    {
        //                        n1.Performer = userId;
        //                        n1.Status = "1"; // 1 for unread - 0 for read
        //                        n1.PublishedOn = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
        //                        DateTime pubdate = DateTime.ParseExact(n1.PublishDate, "dd-M-yyyy", CultureInfo.InvariantCulture);
        //                        DateTime curDate = DateTime.Today;
        //                        int setValid = DateTime.Compare(curDate, pubdate);
        //                        if (setValid == 0) { n1.Valid = "1"; }
        //                        else { n1.Valid = "0"; }
        //                        string[] newIdsList = n1.NewIds.Split(',');
        //                        long newIdCount = newIdsList.Count();
        //                        if (newIdCount > 0)
        //                        {
        //                            for (int i = 0; i < newIdCount; i++)
        //                            {
        //                                showNote.NotePreId = n1.NotePreId;
        //                                showNote.NoteValid = n1.Valid;
        //                                showNote.ViewStatus = "0"; // 0 - unread, 1- read
        //                                showNote.NewId = newIdsList[i];
        //                                if (showNote.NewId != "") { pps.CreateShowNotification(showNote); }
        //                            }
        //                        }
        //                        ViewBag.SuccessNotifyCreation = 1;

        //                        pps.CreateOrUpdateNotification(n1);

        //                        TempData["SuccessNotifyCreation"] = n1.Id;
        //                        return RedirectToAction("Notify", new { id = n1.Id });
        //                    }
        //                }

        //            }

        //        }
        //        return View(n1);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
        //        throw ex;
        //    }
        //}

        public void SendNotificationEmail(string noteTopic, string selPreRegNums, string noteCampus)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                BaseController bc = new BaseController();
                string cmp = Session["campus"].ToString();
                if (Session["campus"].ToString() == "")
                {
                    cmp = "All";
                }
                ParentPortalService pps = new ParentPortalService();
                string SendEmail = ConfigurationManager.AppSettings["SendEmail1"];
                if (SendEmail == "false")
                    return;
                else
                {
                    try
                    {
                        var prereg = selPreRegNums.ToString().Split(',');
                        long[] prnid = new long[prereg.Length];
                        long prnLen = prereg.Length;
                        int mailcnt = 0;
                        IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(noteCampus.ToString(), ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                        string From = ConfigurationManager.AppSettings["From"];
                        string To = ConfigurationManager.AppSettings["To"];
                        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                        if (To == "live")
                        {
                            mail.To.Add(campusemaildet.First().EmailId.ToString());
                        }
                        else if (To == "test")
                        {
                            mail.To.Add(campusemaildet.First().EmailId.ToString());
                        }

                        mail.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient("localhost", 25);
                        smtp.Host = "smtp.gmail.com";
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.EnableSsl = true;
                        mail.Subject = noteTopic + " - Notification for you in TIPS Parent Portal";
                        string Body = "Dear Parent,<br><br> Please check your parent portal for a new notification. <br><br> Regards <br> TIPS Management Team";
                        mail.Body = Body;
                        if (prnLen > 0)
                        {
                            for (int i = 0; i < prnLen; i++)
                            {
                                if (prereg[i] != "")
                                {
                                    Dictionary<string, object> criteria3 = new Dictionary<string, object>();
                                    criteria3.Add("PreRegNum", Convert.ToInt64(prereg[i]));
                                    Dictionary<long, IList<StudentTemplate>> StudentTemplate = ads.GetStudentDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria3);
                                    long stlen = StudentTemplate.First().Value.Count();
                                    if (stlen > 0)
                                    {
                                        if ((StudentTemplate.First().Value[0].EmailId != null) && (StudentTemplate.First().Value[0].EmailId.Contains("@")))
                                        {
                                            if (ValidEmailOrNot(StudentTemplate.First().Value[0].EmailId))
                                            {
                                                mail.Bcc.Add(StudentTemplate.First().Value[0].EmailId);
                                                mailcnt = mailcnt + 1;
                                            }

                                        }
                                        else
                                        {
                                            Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                                            criteria1.Add("PreRegNum", Convert.ToInt64(prereg[i]));
                                            criteria1.Add("FamilyDetailType", "Father");
                                            Dictionary<long, IList<FamilyDetails>> FamilyDetails = ads.GetFamilyDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria1);
                                            if (FamilyDetails.First().Value.Count() != 0)
                                            {
                                                if ((FamilyDetails.First().Value[0].Email != null) && (FamilyDetails.First().Value[0].Email.Contains("@")))
                                                {
                                                    if (ValidEmailOrNot(FamilyDetails.First().Value[0].Email))
                                                    {
                                                        mail.Bcc.Add(FamilyDetails.First().Value[0].Email);
                                                        mailcnt = mailcnt + 1;
                                                    }
                                                }
                                                else
                                                {
                                                    Dictionary<string, object> criteria2 = new Dictionary<string, object>();
                                                    criteria2.Add("PreRegNum", Convert.ToInt64(prereg[i]));
                                                    criteria2.Add("FamilyDetailType", "Mother");
                                                    Dictionary<long, IList<FamilyDetails>> FamilyDetails1 = ads.GetFamilyDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria2);
                                                    if (FamilyDetails1.First().Value.Count() != 0)
                                                    {
                                                        if ((FamilyDetails1.First().Value[0].Email != null) && (FamilyDetails1.First().Value[0].Email.Contains("@")))
                                                        {
                                                            if (ValidEmailOrNot(FamilyDetails1.First().Value[0].Email))
                                                            {
                                                                mail.Bcc.Add(FamilyDetails1.First().Value[0].Email);
                                                                mailcnt = mailcnt + 1;
                                                            }
                                                        }
                                                    }

                                                }
                                            }

                                        }

                                    }
                                    int quotient = mail.Bcc.Count / (Convert.ToInt32(ConfigurationManager.AppSettings["EmailCnt"]));
                                    int remainder = mail.Bcc.Count % (Convert.ToInt32(ConfigurationManager.AppSettings["EmailCnt"]));
                                    if (mail.Bcc.Count >= (Convert.ToInt32(ConfigurationManager.AppSettings["EmailCnt"])))
                                    {
                                        if (From == "live")
                                        {
                                            mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                                            smtp.Credentials = new System.Net.NetworkCredential
                                           (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                                            if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.Bcc.ToString()))
                                            {
                                                try
                                                {
                                                    smtp.Send(mail);
                                                }
                                                catch (Exception ex)
                                                {
                                                    if (ex.Message.Contains("quota"))
                                                    {
                                                        mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                                                        smtp.Credentials = new System.Net.NetworkCredential(campusemaildet.First().AlternateEmailId.ToString(), campusemaildet.First().AlternateEmailIdPassword.ToString());
                                                        smtp.Send(mail);
                                                    }
                                                    else
                                                    {
                                                        mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                                                        smtp.Credentials = new System.Net.NetworkCredential(campusemaildet.First().AlternateEmailId, campusemaildet.First().AlternateEmailIdPassword);
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
                                            if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.Bcc.ToString()))
                                            {
                                                smtp.Send(mail);
                                            }
                                        }
                                        try
                                        {
                                            NoteEmailLog el = new NoteEmailLog();
                                            el.Id = 0;
                                            if (To == "live")
                                            {
                                                el.EmailTo = campusemaildet.First().EmailId.ToString();
                                            }
                                            else if (To == "test")
                                            {
                                                el.EmailTo = campusemaildet.First().EmailId.ToString();
                                            }
                                            el.EmailCC = mail.CC.ToString();
                                            if (mail.Bcc.ToString().Length < 3990)
                                            {
                                                el.EmailBCC = mail.Bcc.ToString();
                                            }
                                            el.Subject = mail.Subject.ToString();

                                            if (mail.Body.ToString().Length < 3990)
                                            {
                                                el.Message = Body;
                                            }
                                            el.EmailDateTime = DateTime.Now.ToString();
                                            el.BCC_Count = mail.Bcc.Count;
                                            el.Module = "Notification";
                                            pps.CreateOrUpdateNoteEmailLog(el);
                                            mail.Bcc.Clear();
                                            mailcnt = 0;
                                        }
                                        catch (Exception ex)
                                        {
                                            throw ex;
                                        }

                                    }

                                }
                            }

                            if (mail.Bcc.Count < (Convert.ToInt32(ConfigurationManager.AppSettings["EmailCnt"])))
                            {
                                if (mail.Bcc.Count != 0)
                                {
                                    if (From == "live")
                                    {
                                        mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                                        smtp.Credentials = new System.Net.NetworkCredential
                                       (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                                        if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.Bcc.ToString()))
                                        {
                                            try
                                            {
                                                smtp.Send(mail);
                                            }
                                            catch (Exception ex)
                                            {
                                                if (ex.Message.Contains("quota"))
                                                {
                                                    mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                                                    smtp.Credentials = new System.Net.NetworkCredential(campusemaildet.First().AlternateEmailId.ToString(), campusemaildet.First().AlternateEmailIdPassword.ToString());
                                                    smtp.Send(mail);
                                                }
                                                else
                                                {
                                                    mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                                                    smtp.Credentials = new System.Net.NetworkCredential(campusemaildet.First().AlternateEmailId, campusemaildet.First().AlternateEmailIdPassword);
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
                                        if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.Bcc.ToString()))
                                        {
                                            smtp.Send(mail);
                                        }
                                    }

                                    NoteEmailLog el = new NoteEmailLog();

                                    el.Id = 0;
                                    if (To == "live")
                                    {
                                        el.EmailTo = campusemaildet.First().EmailId.ToString();
                                    }
                                    else if (To == "test")
                                    {
                                        el.EmailTo = campusemaildet.First().EmailId.ToString();
                                    }
                                    try
                                    {
                                        el.Subject = mail.Subject;
                                        el.EmailCC = mail.CC.ToString();
                                        if (mail.Bcc.ToString().Length < 3990)
                                        {
                                            el.EmailBCC = mail.Bcc.ToString();
                                        }

                                        el.Subject = mail.Subject.ToString();

                                        if (mail.Body.ToString().Length < 3990)
                                        {
                                            el.Message = Body;
                                        }
                                        el.EmailDateTime = DateTime.Now.ToString();
                                        el.BCC_Count = mail.Bcc.Count;
                                        el.Module = "Notification";
                                        pps.CreateOrUpdateNoteEmailLog(el);

                                    }
                                    catch (Exception ex)
                                    {
                                        throw ex;
                                    }
                                }
                            }

                        }

                    }
                    catch (System.Net.WebException ex)
                    {
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }


        public ActionResult ParentNotification(string NewId, string campus)
        {
            Session["noteparent"] = NewId;
            Session["campus"] = campus;
            return View();
        }

        public ActionResult StudentNotification(string NewId, string campus)
        {
            Session["notestudent"] = NewId;
            Session["campus"] = campus;
            return View();
        }

        public JsonResult NotificationListJqGrid(long? NoteId, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                ParentPortalService pps = new ParentPortalService();

                Dictionary<string, object> criteria = new Dictionary<string, object>();

                string UserId = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();

                if (!string.IsNullOrWhiteSpace(sord) && sord == "desc")
                    sord = "Desc";
                else
                    sord = "Asc";

                if (UserId != "Ashok")
                {
                    criteria.Add("Performer", UserId);
                }
                var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                if (usrcmp!=null&& usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
                {
                    criteria.Add("Campus", usrcmp);
                }
                Dictionary<long, IList<Notification>> nDet = pps.GetValuesFromNotification(page - 1, rows, sidx, sord, criteria);
                long totalRecords = nDet.First().Key;
                int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);

                var notedet = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = (from items in nDet.First().Value
                            select new
                            {
                                i = items.Id,
                                cell = new string[] 
                                { 
                                   items.Id.ToString(),
                                   items.NoteType == "General"?"<a href='/Notify/ViewNotifications?npreid=" + items.NotePreId + "' style='color:blue;'>"+items.Topic+"</a>":"<a href='/Notify/ViewIndividualNotifications?npreid=" + items.NotePreId + "' style='color:blue;'>"+items.Topic+"</a>",
                                   items.Message,
                                   items.PublishDate,
                                   items.ExpireDate,
                                   items.PublishTo,
                                   items.NoteType,
                                   items.Campus,
                                   items.NewIds,
                                   items.Status,
                                   items.Valid,
                                   items.NotePreId.ToString(),
                                   items.PublishedOn
                                }
                            })
                };
                return Json(notedet, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        public ActionResult ViewNotifications(long? npreid)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ParentPortalService pps = new ParentPortalService();

                    long vid = Convert.ToInt64(npreid);
                    MastersService ms = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    ViewBag.acadddl = AcademicyrMaster.First().Value;
                    Notification n = new Notification();
                    if (vid > 0)
                    {
                        n = pps.getNoteDetailsfromPreId(vid);

                        return View(n);
                    }
                    else
                    {
                        return View(n);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }
        public ActionResult ViewIndividualNotifications(long? npreid)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ParentPortalService pps = new ParentPortalService();

                    long vid = Convert.ToInt64(npreid);
                    MastersService ms = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Clear();
                    Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    ViewBag.acadddl = AcademicyrMaster.First().Value;
                    Notification n = new Notification();
                    if (vid > 0)
                    {
                        n = pps.getNoteDetailsfromPreId(vid);

                        return View(n);
                    }
                    else
                    {
                        return View(n);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        } 
        public JsonResult ViewNotificationAttachments(long notepreid)
        {
            ParentPortalService pps = new ParentPortalService();

            IList<NoteAttachment> dList = pps.GetDocListById(notepreid);

            string[] attId = (from items in dList select items.Id.ToString()).ToArray();

            string[] attName = (from items in dList select items.AttachmentName.ToString()).ToArray();

            long atlen = attId.Count();

            string[] noteAtt = new string[dList.Count];

            string totNoteAtt = "<br><div>";

            for (var j = 0; j < atlen; j++)
            {
                noteAtt[j] += "<a href='#' onclick = 'uploaddat(";
                noteAtt[j] += attId[j];
                noteAtt[j] += ")' style='color:black;text-decoration:underline;' >" + attName[j] + "</a>&nbsp;&nbsp; <a href='#' onclick='delatt(" + attId[j] + ")' style='color:red; text-decoration:underline;' >Delete this Attachment</a><br>";

                totNoteAtt += noteAtt[j];
            }

            totNoteAtt += "</div>";

            return Json(totNoteAtt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AdmissionManagementListJqGrid(string Id, string grade, string section, string acadyr, string appname, string idnum, string admstat, string appno, string preregnumber, string ishosteller, string flag, string flag1, string reset, string stdntmgmt, int rows, string sidx, string sord, int? page = 1)
        {
            string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
            try
            {
                Session["emailsent"] = "";
                sord = sord == "desc" ? "Desc" : "Asc";
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                string colName = string.Empty; string[] values = new string[1];
                if (!string.IsNullOrWhiteSpace(Id))
                {
                    Session["cmpnm"] = Id;
                    criteria.Add("Campus", Session["cmpnm"].ToString());
                }
                else
                {
                    if (Session["UserCampus"] != null)
                    {
                        criteria.Add("Campus", Session["UserCampus"]);
                    }
                }
                if (reset != "yes")
                {

                    if (grade != "null" && !string.IsNullOrWhiteSpace(grade))
                    {
                        string[] Gradarr = grade.Split(',');
                        criteria.Add("Grade", Gradarr);
                    }
                    if (section != "null" && !string.IsNullOrWhiteSpace(section))
                    {
                        string[] sectionarr = section.Split(',');
                        criteria.Add("Section", sectionarr);
                    }

                    //if (!string.IsNullOrWhiteSpace(grade) || (Session["grd"].ToString() != ""))
                    //{
                    //    if (grade != "")
                    //    {
                    //        Session["grd"] = grade;
                    //    }
                    //    colName = "Grade";
                    //    values[0] = Session["grd"].ToString();
                    //}

                    //if (!string.IsNullOrWhiteSpace(section) || (Session["sect"].ToString() != ""))
                    //{
                    //    if (section != "")
                    //    {
                    //        Session["sect"] = section;
                    //    }
                    //    criteria.Add("Section", Session["sect"]);
                    //}

                    if (!string.IsNullOrWhiteSpace(acadyr) || (Session["acdyr"].ToString() != ""))
                    {
                        if (acadyr != "")
                        {
                            Session["acdyr"] = acadyr;
                            criteria.Add("AcademicYear", Session["acdyr"]);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(appname) || (Session["apnam"].ToString() != ""))
                    {
                        if (appname != "")
                        {
                            Session["apnam"] = appname;
                            criteria.Add("Name", Session["apnam"]);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(preregnumber) || (Session["regno"].ToString() != ""))
                    {
                        if (preregnumber != "")
                        {
                            Session["regno"] = preregnumber;
                            criteria.Add("PreRegNum", Convert.ToInt64(Session["regno"]));
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(idnum) || (Session["idnum"].ToString() != ""))
                    {
                        if (idnum != "")
                        {
                            Session["idnum"] = idnum;
                            criteria.Add("NewId", Session["idnum"]);
                        }
                    }

                    if (stdntmgmt == "yes")
                    {
                        if (!string.IsNullOrWhiteSpace(ishosteller) || (Session["ishosteller"].ToString() != ""))
                        {
                            if (ishosteller != "")
                            {
                                if (ishosteller == "Yes")
                                {
                                    Session["ishosteller"] = true;
                                    Session["hostlr"] = "Yes";
                                }
                                else
                                {
                                    Session["ishosteller"] = false;
                                    Session["hostlr"] = "No";
                                }
                                criteria.Add("IsHosteller", Session["ishosteller"]);
                            }

                        }
                    }
                }
                else
                {
                    Session["cmpnm"] = "";
                    Session["grd"] = "";
                    Session["sect"] = "";
                    Session["acdyr"] = "";
                    Session["apnam"] = "";
                    Session["stats"] = "";
                    Session["appno"] = "";
                    Session["regno"] = "";
                    Session["ishosteller"] = "";
                    Session["feestructyr"] = "";
                    Session["idnum"] = "";
                }

                if (flag1 == "Register")   // To check whether this call is from StudentManagement page
                {
                    if (!string.IsNullOrWhiteSpace(admstat) || (Session["stats"].ToString() != ""))
                    {
                        if (admstat != "")
                        {
                            Session["stats"] = admstat;
                        }
                        criteria.Add("AdmissionStatus", Session["stats"]);
                    }
                    else
                    {
                        criteria.Add("AdmissionStatus", "Registered");
                    }
                    ViewBag.Studentmgmt = "yes";
                    Session["studentmgmt"] = "yes";
                }
                else
                {
                    Session["studentmgmt"] = "";

                    if (userid == "Ashok")
                    {
                        if (flag != "Ashok")
                        {
                            criteria.Add("Status", "1");
                        }
                    }
                    if ((userid == "GRL-ADMS") || (userid == "CHE-GRL") || (userid == "TIR-GRL") || (userid == "ERN-GRL") || (userid == "KAR-GRL") || (userid == "APP-ADMN"))
                    {
                        if ((flag != "GRL-ADMS") || (flag != "CHE-GRL") || (userid == "TIR-GRL") || (userid == "ERN-GRL") || (userid == "KAR-GRL") || (userid == "APP-ADMN"))
                        {
                            criteria.Add("Status1", "2");
                        }
                    }
                }
                Dictionary<long, IList<TIPS.Entities.AdmissionEntities.StudentTemplateView>> StudentTemplate;

                StudentTemplate = ads.GetStudentDetailsListWithPagingAndCriteriaWithAlias(page - 1, rows, sidx, sord, colName, values, criteria, null);

                if (StudentTemplate.Count > 0)
                {
                    long totalrecords = StudentTemplate.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in StudentTemplate.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                                    items.ApplicationNo,
                              items.PreRegNum.ToString(),
                            String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '#' onclick = 'getdata("+"\"" + items.Id + "\"" + ")' >{0}</a>",items.Name),
                            items.Grade,
                            items.Section,
                            items.Campus,
                            items.FeeStructYear,
                            items.AdmissionStatus,
                            items.NewId,
                            items.AcademicYear,
                            items.CreatedDate                            
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
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult EditNotification(Notification editnote)
        {
            try
            {
                ParentPortalService pps = new ParentPortalService();

                ShowNotification showNote = new ShowNotification();

                long notepreid = editnote.NotePreId;

                string UserId = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();

                editnote.Performer = UserId;

                DateTime expdate = DateTime.ParseExact(editnote.ExpireDate, "dd-M-yyyy", CultureInfo.InvariantCulture);

                DateTime curDate = DateTime.Today;

                int setValid = DateTime.Compare(curDate, expdate);

                if (setValid == 0 || setValid == -1)
                {
                    editnote.Valid = "1";

                    pps.SetShowNotificationByValidNote(notepreid, 1);

                }
                else if (setValid == 1)
                {
                    editnote.Valid = "0";
                    pps.SetShowNotificationByValidNote(notepreid, 0);
                }

                if (editnote.NoteType.Equals("GradeLevel") || editnote.NoteType.Equals("Individual"))
                {
                    if (editnote.NewIds != null || editnote.NewIds != "")
                    {
                        string[] newIdsList = editnote.NewIds.Split(',');

                        long newIdCount = newIdsList.Count();

                        if (newIdCount > 0)
                        {
                            pps.DeleteExistingNewIdFromShowNotification(notepreid);

                            for (int i = 0; i < newIdCount; i++)
                            {
                                if (newIdsList[i] != "")
                                {
                                    showNote.NotePreId = editnote.NotePreId;
                                    showNote.NoteValid = editnote.Valid;
                                    showNote.ViewStatus = "0"; // 0 - unread, 1- read
                                    showNote.NewId = newIdsList[i];

                                    if (showNote.NewId != "")
                                    {
                                        pps.CreateShowNotification(showNote);
                                    }

                                }
                            }
                        }
                    }
                }
                else
                {
                    editnote.Grade = Request.Form["Grade"];
                    editnote.Section = Request.Form["Section"];
                }
                pps.CreateOrUpdateNotification(editnote);

                TempData["SuccessNotifyUpdate"] = editnote.Id;

                return RedirectToAction("Notify", new { id = editnote.Id });
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        public JsonResult IndividualNotificationForSelectedIdJqGrid(long? npreId, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                ParentPortalService pps = new ParentPortalService();

                long npid = Convert.ToInt64(npreId);

                IList<ShowNotification> nidList = pps.GetShowNotificationListbyNotePreId(npid);


                string[] values = (from items in nidList select items.NewId).ToArray();

                Dictionary<string, object> criteria = new Dictionary<string, object>();
                {
                    criteria.Add("NotePreId", npid);
                }

                Dictionary<long, IList<ShowNotification>> snList = pps.GetShowNotificationSearchCriteriaAlias(page - 1, rows, sidx, sord, "NewId", values, criteria, null);

                if (snList != null && snList.Count > 0)
                {
                    long totalRecords = snList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);

                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows = (
                             from items in snList.First().Value
                             select new
                             {
                                 i = items.Id,
                                 cell = new string[] {
                                    
                                     items.Id.ToString(),items.NewId.ToString()
                                    
                                 }
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
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteNotification(string id)
        {
            try
            {
                ParentPortalService pps = new ParentPortalService();

                var test = id.Split(',');

                long[] idtodelete = new long[test.Length];
                int i = 0;
                foreach (string val in test)
                {
                    idtodelete[i] = Convert.ToInt64(val);

                    IList<Notification> delList = pps.GetNotificationListById(idtodelete[i]);

                    long delNotePreId = delList.First().NotePreId;

                    pps.DeleteNoteAttachmentByNotePreId(delNotePreId);

                    pps.DeleteShowNotificationByNotePreId(delNotePreId);

                    i++;
                }

                pps.DeleteNotification(idtodelete);

                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        public ActionResult notetypeddl()
        {
            try
            {
                Dictionary<string, string> notetype = new Dictionary<string, string>();

                string[] notelist = { "GradeLevel", "Individual", "General" };

                var notedet = notelist.ToList();

                return Json(notedet, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        public ActionResult publishtoddl()
        {
            try
            {
                Dictionary<string, string> publishto = new Dictionary<string, string>();

                string[] publists = { "Parent", "Student", "Staff" };

                var pubdet = publists.ToList();

                return Json(pubdet, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        /* End of Notification */

        /* Start of PhotoGallery */
       
        #region start PhotoGallery methods
        public ActionResult PhotoGallery()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ParentPortalService pps = new ParentPortalService();

                    PhotoGallery pg = new PhotoGallery();

                    Session["uploadedfiles"] = "";
                    Session["uploadedfilenames"] = "";

                    var maxpgid = pps.GetMaxPGId();

                    pg.PGPreId = maxpgid + 1;

                    pps.UpdatePGCount(pg.PGPreId);
                    #region BreadCrumb
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    #endregion
                    return View(pg);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PhotoGallery(PhotoGallery pg1, FormCollection fc, HttpPostedFileBase file2)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    userId = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();

                    long id = 0;

                    ParentPortalService pps = new ParentPortalService();

                    if (ModelState.IsValid)
                    {
                        if (Request.Form["btnSubmit"] == "Submit")
                        {
                            if (id == 0)
                            {
                                pg1.Performer = userId;
                                pg1.Status = "1";
                                pg1.IsActive = true;
                                pg1.CreatedOn = DateTime.Now.ToString();
                                ViewBag.SuccessNotifyCreation = 1;
                                pps.CreateOrUpdatePhotoGallery(pg1);
                                pps.UpdatePhotoGalleryNotificationStatus(pg1.Campus);
                                TempData["SuccessPGCreation"] = pg1.Id;
                                return RedirectToAction("PhotoGallery", new { id = pg1.Id });
                            }

                        }
                    }
                    return View(pg1);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        public ActionResult PhotoGalleryFilesUpload(long PGPreId, HttpPostedFileBase[] File, long? Folder_Id)
        {
            long fCount = File.Count();
            string[] strAttachname = { };
            string uploadedFiles = "";

            for (int i = 0; i < fCount; i++)
            {
                // System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                strAttachname = File[i].FileName.Split('\\');
                Attachment pgAttach = new Attachment(File[i].InputStream, strAttachname[strAttachname.Length - 1]);  //Data posted from form

                string path = File[i].InputStream.ToString();
                byte[] imageSize = new byte[File[0].ContentLength];
                File[i].InputStream.Read(imageSize, 0, (int)File[0].ContentLength);
                PhotoGalleryPhotos pgp = new PhotoGalleryPhotos();
                pgp.Photo = imageSize;
                pgp.PhotoName = strAttachname.First().ToString();
                pgp.PGPreId = PGPreId;
                if (Folder_Id > 0)
                {
                    pgp.Folder_Id = Convert.ToInt64(Folder_Id);
                }
                ParentPortalService pps = new ParentPortalService();
                pps.CreateOrUpdatePhotoGalleryPhoto(pgp);

                Dictionary<string, object> criteria3 = new Dictionary<string, object>();

                var maxattachemntid = pps.GetMaxPGPId();

                if ((Session["uploadedfiles"].ToString() != ""))
                {
                    Session["uploadedfiles"] = Session["uploadedfiles"] + "," + maxattachemntid;
                }
                else
                {
                    Session["uploadedfiles"] = maxattachemntid;
                }
                Session["uploadedfilenames"] = strAttachname.First().ToString();

                uploadedFiles += strAttachname.First().ToString();

                uploadedFiles += ",";

            }
            return Json(new { success = true, result = uploadedFiles.ToString() }, "text/html", JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PhotoGalleryDeleteUploadedFiles()
        {
            try
            {
                ParentPortalService pps = new ParentPortalService();
                if (Session["uploadedfiles"].ToString() != "")
                {
                    var test = Session["uploadedfiles"].ToString().Split(',');

                    long[] idtodelete = new long[test.Length];
                    int i = 0;
                    foreach (string val in test)
                    {
                        idtodelete[i] = Convert.ToInt64(val);
                        pps.DeleteUploadedFiles(idtodelete[i]);
                        i++;
                    }

                    Session["uploadedfiles"] = "";
                    Session["uploadedfilenames"] = "";
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        public JsonResult PhotoGalleryListJqGrid(long? studno, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                ParentPortalService pps = new ParentPortalService();

                Dictionary<string, object> criteria = new Dictionary<string, object>();

                string userId = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();

                if (!string.IsNullOrWhiteSpace(sord) && sord == "desc")
                    sord = "Desc";
                else
                    sord = "Asc";
                long ParentRefId = 0;
                criteria.Add("Performer", userId);
                criteria.Add("ParentRefId", ParentRefId);
                Dictionary<long, IList<PhotoGallery>> piDet = pps.GetValuesFromPhotoGallery(page - 1, rows, sidx, sord, criteria);
                long totalRecords = piDet.First().Key;
                int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);

                var pginfo = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = (from items in piDet.First().Value
                            select new
                            {
                                i = items.Id,
                                cell = new string[] 
                                { 
                                   items.Id.ToString(),
                                   items.Campus,
                                   items.Grade,
            "<a href='/Notify/ViewAlbumDetails?pgpreid=" + items.PGPreId + "' style='color:blue;'>"+items.AlbumName+"</a>",
                                   items.Description,
                                   items.PublishedTo,
                                   items.Performer,
                                   items.CreatedOn,
                                   items.Status,
                                   items.IsActive==true?"Yes":"No"

                                }
                            })
                };


                return Json(pginfo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }
        public ActionResult ViewAlbumDetails(long? pgPreId)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ParentPortalService pps = new ParentPortalService();

                    long vid = Convert.ToInt64(pgPreId);

                    PhotoGallery pg = new PhotoGallery();

                    Session["ids"] = "";

                    if (vid > 0)
                    {
                        pg = pps.getPGDetailsfromId(vid);

                        return View(pg);
                    }
                    else
                    {
                        return View(pg);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }

        }

        public ActionResult uploaddisplay1(long Id)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum", Id);
                criteria.Add("DocumentType", "Student Photo");

                Dictionary<long, IList<UploadedFiles>> UploadedFiles = ads.GetUploadedFilesListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);

                //                IList<UploadedFiles> list1 = ads.GetUploadedFilesByPreRegNum(Id,"no");
                if (UploadedFiles != null && UploadedFiles.FirstOrDefault().Value != null && UploadedFiles.FirstOrDefault().Value.Count != 0)
                {
                    if (UploadedFiles.First().Value[0].OldFiles == 1)
                    {
                        //         var filePaths = Directory.GetFiles(@"C:\Users\Admin\Desktop\TIPSImages\3\01c585ec-349f-4212-a59c-b35ba287f7da\", "*.*", SearchOption.AllDirectories);

                        string ImagePath = UploadedFiles.First().Value[0].FileDirectory + "\\" + UploadedFiles.First().Value[0].DocumentName;// "green.jpg"; ConfigurationManager.AppSettings["ImageFilePath1"] 

                        if (!System.IO.File.Exists(ImagePath))
                        {
                            ImagePath = ConfigurationManager.AppSettings["ImageFilePath1"] + UploadedFiles.First().Value[0].DocumentName;
                        }

                        return File(ImagePath, "image/jpg");
                    }
                    else
                    {
                        IList<UploadedFiles> list = UploadedFiles.FirstOrDefault().Value;
                        //    IList<UploadedFiles> list = UploadedFiles;
                        UploadedFiles doc = list.FirstOrDefault();
                        if (doc.DocumentData != null)
                        {
                            MemoryStream ms = new MemoryStream(doc.DocumentData);
                            var cd = new System.Net.Mime.ContentDisposition
                            {
                                // for example foo.bak
                                FileName = doc.DocumentName,

                                // always prompt the user for downloading, set to true if you want 
                                // the browser to try to show the file inline
                                Inline = false,
                            };
                            Response.AppendHeader("Content-Disposition", cd.ToString());
                            return File(doc.DocumentData, "image/jpg");
                        }
                        else
                        {
                            string ImagePath = ConfigurationManager.AppSettings["ImageFilePath"];
                            return File(ImagePath, "image/jpg");
                        }
                    }
                }
                else
                {
                    var dir = Server.MapPath("/Images");
                    string ImagePath = ConfigurationManager.AppSettings["ImageFilePath"];
                    return File(ImagePath, "image/jpg");
                }
            }
            catch (Exception ex)
            {

                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public ActionResult ViewAlbumPhotos(string id, string PGPreId)
        {
            try
            {
                int rows = 9999;
                string sidx = "Id";
                string sord = "desc";
                int? page = 1;
                ParentPortalService pps = new ParentPortalService();
                long vid = Convert.ToInt64(id);
                long vPGPid = Convert.ToInt64(PGPreId);
                long Folder_Id = 0;
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Id", vid);
                criteria.Add("PGPreId", vPGPid);
                criteria.Add("Folder_Id", Folder_Id);
                Dictionary<long, IList<PhotoGalleryPhotos>> UploadedFiles = pps.GetValuesFromPhotoGalleryPhotos(page - 1, rows, sidx, sord, criteria);
                long nCount = UploadedFiles.Count;
                long len = UploadedFiles.First().Value.Count();
                IList<PhotoGalleryPhotos> list = UploadedFiles.FirstOrDefault().Value;
                PhotoGalleryPhotos doc = list.FirstOrDefault();
                if (doc.Photo != null)
                {
                    MemoryStream ms = new MemoryStream(doc.Photo);

                    var cd = new System.Net.Mime.ContentDisposition
                    {
                        // for example foo.bak
                        FileName = doc.PhotoName,

                        // always prompt the user for downloading, set to true if you want 
                        // the browser to try to show the file inline
                        Inline = false,
                    };
                    Response.AppendHeader("Content-Disposition", cd.ToString());

                    return File(doc.Photo, "image/jpg");
                }
                return View();

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeletePhotoGallery(string id)
        {
            try
            {
                ParentPortalService pps = new ParentPortalService();

                var test = id.Split(',');

                long[] idtodelete = { };
                long[] preidtodel = { };

                long did;

                int i = 0;
                foreach (string val in test)
                {
                    //idtodelete[i] = Convert.ToInt64(val);

                    did = Convert.ToInt64(val);

                    PhotoGallery pg = pps.getPGDetailsfromDelId(did);

                    IList<PhotoGalleryPhotos> pgpList = pps.getPGPreIdList(pg.PGPreId);
                    IList<PhotoGallery> pglist = pps.getPGalleryPreIdList(pg.PGPreId);

                    idtodelete = (from items in pglist select items.Id).ToArray();
                    preidtodel = (from items in pgpList select items.Id).ToArray();

                    pps.DeletePhotoGalleryPhotosbyPGPreId(preidtodel);
                    pps.DeletePhotoGallerybyId(idtodelete);
                }


                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }
        public ActionResult EditPhotoGallery(PhotoGallery pg1)
        {
            try
            {
                if (pg1 != null)
                {
                    if (pg1.Id > 0)
                    {
                        ParentPortalService pps = new ParentPortalService();
                        PhotoGallery pgdetails = pps.GetPhotoGalleryById(pg1.Id);
                        pps.UpdatePhotoGalleryDetails(pg1.IsActive, pgdetails.PGPreId);
                        //pgdetails.IsActive = pg1.IsActive;

                        // pps.CreateOrUpdatePhotoGallery(pgdetails);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {

                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult DeletePhotosFromAlbum(string id)
        {
            try
            {
                ParentPortalService pps = new ParentPortalService();

                long Del = Convert.ToInt64(id);

                pps.DeletePhotosById(Del);

                return Json("1", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        #region Added By Prabakaran
        //public ActionResult CreateNewFolder(string PGPreId, string FolderName)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(PGPreId))
        //        {
        //            ParentPortalService pps = new ParentPortalService();
        //            long vPGPid = Convert.ToInt64(PGPreId);
        //            PhotoGalleryFolder pgf = new PhotoGalleryFolder();
        //            PhotoGalleryFolder pgfdetails = pps.getPGFolderByFolderNamewithPgPreId(vPGPid, FolderName);
        //            if (pgfdetails != null)
        //            {
        //                return Json("Exist", JsonRequestBehavior.AllowGet);
        //            }
        //            pgf.FolderName = FolderName;
        //            pgf.PGPreId = vPGPid;
        //            pps.CreateOrUpdatePhotoGalleryFolder(pgf);
        //        }
        //        return Json("success", JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
        //        throw ex;
        //    }
        //}
        public ActionResult DeleteFolder(string PGPreId, string Id)
        {
            try
            {
                if (!string.IsNullOrEmpty(PGPreId))
                {
                    ParentPortalService pps = new ParentPortalService();
                    long vPGPid = Convert.ToInt64(PGPreId);
                    long vId = Convert.ToInt64(Id);
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("PGPreId", vPGPid);
                    IList<PhotoGallery> pglist = new List<PhotoGallery>();
                    PhotoGallery pg = pps.GetPhotoGalleryById(vId);
                    if (pg != null)
                    {
                        criteria.Add("ParentRefId", vId);
                        pps.DeletePhotoGallery(pg);
                        pps.DeletePhotoGalleryPhotosByFolder_IdandId(pg.Id, pg.PGPreId);
                        Dictionary<long, IList<PhotoGallery>> PGList = pps.GetValuesFromPhotoGallery(0, 9999, string.Empty, string.Empty, criteria);
                        if (PGList.FirstOrDefault().Key > 0)
                        {
                            foreach (var items in PGList.FirstOrDefault().Value)
                            {
                                pps.DeletePhotoGalleryPhotosByFolder_IdandId(items.Id, items.PGPreId);
                                pps.DeletePhotoGallery(items);
                                IList<PhotoGallery> pgreclist = RecPhotoGallery(items.Id);
                            }
                        }
                    }

                }
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }
        public ActionResult ViewFolderPhotos(string id, string PGPreId, string Folder_Id)
        {
            try
            {
                int rows = 9999;
                string sidx = "Id";
                string sord = "desc";
                int? page = 1;

                ParentPortalService pps = new ParentPortalService();


                long vid = Convert.ToInt64(id);
                long vPGPid = Convert.ToInt64(PGPreId);
                long vFolder_Id = Convert.ToInt64(Folder_Id);

                Dictionary<string, object> criteria = new Dictionary<string, object>();

                criteria.Add("Id", vid);
                criteria.Add("PGPreId", vPGPid);
                criteria.Add("Folder_Id", vFolder_Id);

                Dictionary<long, IList<PhotoGalleryPhotos>> UploadedFiles = pps.GetValuesFromPhotoGalleryPhotos(page - 1, rows, sidx, sord, criteria);
                long nCount = UploadedFiles.Count;
                long len = UploadedFiles.First().Value.Count();
                IList<PhotoGalleryPhotos> list = UploadedFiles.FirstOrDefault().Value;
                PhotoGalleryPhotos doc = list.FirstOrDefault();
                if (doc.Photo != null)
                {
                    MemoryStream ms = new MemoryStream(doc.Photo);

                    var cd = new System.Net.Mime.ContentDisposition
                    {
                        // for example foo.bak
                        FileName = doc.PhotoName,

                        // always prompt the user for downloading, set to true if you want 
                        // the browser to try to show the file inline
                        Inline = false,
                    };
                    Response.AppendHeader("Content-Disposition", cd.ToString());

                    return File(doc.Photo, "image/jpg");
                }

                return View();

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }
        //public ActionResult FolderNameAutoComplete(long Id, string term)
        //{
        //    try
        //    {
        //        ParentPortalService pps = new ParentPortalService();
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        criteria.Add("PGPreId", Id);
        //        if (!string.IsNullOrEmpty(term))
        //        { criteria.Add("FolderName", term); }
        //        Dictionary<long, IList<PhotoGalleryFolder>> FoldersList = pps.GetValuesFromPhotoGalleryFolder(0, 9999, "FolderName", "Asc", criteria);
        //        if (FoldersList.Count > 0 && FoldersList.FirstOrDefault().Key > 0)
        //        {
        //            var FolderNames = (from u in FoldersList.FirstOrDefault().Value where u.FolderName != null select new { FolderName = u.FolderName }).ToList();
        //            return Json(FolderNames, JsonRequestBehavior.AllowGet);
        //        }
        //        return Json(null, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}                
        public ActionResult CreateNewFolderPG(string PGPreId, string FolderName, long? ParentPKId)
        {
            try
            {
                string userId = base.ValidateUser();
                if (!string.IsNullOrEmpty(PGPreId))
                {
                    ParentPortalService pps = new ParentPortalService();
                    long vPGPid = Convert.ToInt64(PGPreId);
                    long vParentPKId = Convert.ToInt64(ParentPKId);
                    //PhotoGalleryFolder pgf = new PhotoGalleryFolder();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (vParentPKId > 0)
                    {
                        criteria.Add("ParentRefId", vParentPKId);
                    }
                    else
                    {
                        PhotoGallery parentpg = pps.getPGDetailsByParentRefIdandPGPreId(vPGPid);
                        if (parentpg != null)
                        {
                            criteria.Add("ParentRefId", parentpg.Id);
                        }
                    }
                    criteria.Add("PGPreId", vPGPid);
                    criteria.Add("FolderName", FolderName);
                    Dictionary<long, IList<PhotoGallery>> pgallery = pps.GetValuesFromPhotoGallery(0, 9999, string.Empty, string.Empty, criteria);
                    if (pgallery == null || pgallery.FirstOrDefault().Key == 0 || pgallery.Count == 0)
                    {
                        PhotoGallery pgfdetails = pps.getPGById(vPGPid);
                        if (pgfdetails != null)
                        {
                            PhotoGallery pg = new PhotoGallery();
                            pg.AlbumName = pgfdetails.AlbumName;
                            pg.Description = pgfdetails.Description;
                            pg.Campus = pgfdetails.Campus;
                            pg.Grade = pgfdetails.Grade;
                            pg.Performer = userId;
                            pg.CreatedOn = DateTime.Now.ToString();
                            pg.FolderName = FolderName;
                            pg.IsActive = pgfdetails.IsActive;
                            pg.PGPreId = pgfdetails.PGPreId;
                            pg.PublishedTo = pgfdetails.PublishedTo;
                            pg.Status = "1";
                            if (vParentPKId > 0)
                            {
                                pg.ParentRefId = vParentPKId;
                            }
                            else
                            {
                                pg.ParentRefId = pgfdetails.Id;
                            }
                            pps.CreateOrUpdatePhotoGallery(pg);
                        }
                        return Json("success", JsonRequestBehavior.AllowGet);
                    }
                }
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }
        public ActionResult ViewFolderDetails(long? pgPreId, long Id)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    int rows = 9999;
                    string sidx = "Id";
                    string sord = "desc";
                    int? page = 1;
                    ParentPortalService pps = new ParentPortalService();
                    long vid = Convert.ToInt64(pgPreId);
                    PhotoGallery pg = new PhotoGallery();
                    pg = pps.getPGDetailsfromId(vid);
                    Session["ids"] = "";
                    PhotoGallery oldpg = pps.getPGDetailsByPKId(Id);
                    PhotoGallery parentoldpg = pps.getPGDetailsByParentRefIdandPGPreId(vid);
                    if (oldpg != null)
                    {
                        ViewBag.PrePgId = oldpg.ParentRefId;
                        if (parentoldpg != null)
                        {
                            if (oldpg.ParentRefId == parentoldpg.Id)
                            {
                                ViewBag.ParentPg = "1";
                            }
                            else
                            {
                                ViewBag.ParentPg = "0";
                            }
                        }
                    }
                    if (Id > 0 && vid > 0)
                    {
                        Dictionary<string, object> criteria = new Dictionary<string, object>();

                        criteria.Add("ParentRefId", Id);
                        Dictionary<long, IList<PhotoGallery>> PhotoGallery = pps.GetValuesFromPhotoGallery(page - 1, rows, sidx, sord, criteria);
                        ViewBag.ParentPKId = Id;
                        if (PhotoGallery.FirstOrDefault().Key > 0)
                        {
                            long len = PhotoGallery.First().Value.Count();
                            string[] ImgIds = new string[len];                            
                            int i = 0;
                            foreach (var var in PhotoGallery.FirstOrDefault().Value)
                            {
                                if (var.ParentRefId == Id)
                                {
                                    ImgIds[i] = var.Id.ToString() + "-" + var.FolderName;
                                    i++;
                                }
                            }                            
                        }
                        return View(pg);
                    }
                    else
                    {
                        return View(pg);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }
        public JsonResult TotalViewAlbumPhotos(long pgPreId)
        {
            try
            {
                int rows = 9999;
                string sidx = "Id";
                string sord = "desc";
                int? page = 1;
                ParentPortalService pps = new ParentPortalService();

                long vPGPid = Convert.ToInt64(pgPreId);
                long Folder_Id = 0;
                Dictionary<string, object> criteria = new Dictionary<string, object>();

                criteria.Add("PGPreId", vPGPid);
                PhotoGallery pg = pps.getPGById(pgPreId);
                if (pg != null)
                {
                    criteria.Add("ParentRefId", pg.Id);
                }
                Dictionary<long, IList<PhotoGallery>> PhotoGallery = pps.GetValuesFromPhotoGallery(page - 1, rows, sidx, sord, criteria);
                criteria.Remove("ParentRefId");
                criteria.Add("Folder_Id", Folder_Id);
                Dictionary<long, IList<PhotoGalleryPhotos>> PhotoGalleryPhotos = pps.GetValuesFromPhotoGalleryPhotos(page - 1, rows, sidx, sord, criteria);
                long len = 0;
                long phlen = 0;
                if (PhotoGallery.FirstOrDefault().Key > 0)
                {
                    len = PhotoGallery.First().Value.Count();
                }
                if (PhotoGalleryPhotos.FirstOrDefault().Key > 0)
                {
                    phlen = PhotoGalleryPhotos.First().Value.Count();
                }
                long totallen = len + phlen;
                string[] ImgIds = new string[totallen];
                int j = 0;
                for (int i = 0; i < totallen; i++)
                {
                    if (i < len)
                    {

                        string str = PhotoGallery.First().Value[i].FolderName.ToString();
                        if (str.Length >= 30)
                        {

                            str = str.Substring(0, 30) + "...";
                        }
                        ImgIds[i] = PhotoGallery.First().Value[i].Id.ToString() + "*" + PhotoGallery.First().Value[i].FolderName.ToString() + "*" + str;
                    }
                    else
                    {
                        if (phlen > 0)
                        {
                            ImgIds[i] = PhotoGalleryPhotos.First().Value[j].Id.ToString();
                            j++;
                        }
                    }
                }
                return Json(ImgIds, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }
        public JsonResult TotalViewFolderPhotos(long pgPreId, long Id)
        {
            try
            {
                int rows = 9999;
                string sidx = "Id";
                string sord = "desc";
                int? page = 1;
                ParentPortalService pps = new ParentPortalService();
                long vPGPid = Convert.ToInt64(pgPreId);
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PGPreId", vPGPid);
                criteria.Add("ParentRefId", Id);
                PhotoGallery pg = pps.getPGById(pgPreId);
                Dictionary<long, IList<PhotoGallery>> PhotoGallery = pps.GetValuesFromPhotoGallery(page - 1, rows, sidx, sord, criteria);
                criteria.Remove("ParentRefId");
                criteria.Add("Folder_Id", Id);
                Dictionary<long, IList<PhotoGalleryPhotos>> PhotoGalleryPhotos = pps.GetValuesFromPhotoGalleryPhotos(page - 1, rows, sidx, sord, criteria);
                long len = 0;
                long phlen = 0;
                if (PhotoGallery.FirstOrDefault().Key > 0)
                {
                    len = PhotoGallery.First().Value.Count();
                }
                if (PhotoGalleryPhotos.FirstOrDefault().Key > 0)
                {
                    phlen = PhotoGalleryPhotos.First().Value.Count();
                }
                long totallen = len + phlen;
                string[] ImgIds = new string[totallen];
                int j = 0;
                for (int i = 0; i < totallen; i++)
                {
                    if (i < len)
                    {
                        string str = PhotoGallery.First().Value[i].FolderName.ToString();
                        if (str.Length >= 30)
                        {

                            str = str.Substring(0, 30) + "...";
                        }
                        ImgIds[i] = PhotoGallery.First().Value[i].Id.ToString() + "*" + PhotoGallery.First().Value[i].FolderName.ToString() + "*" + str;
                    }
                    else
                    {
                        if (phlen > 0)
                        {
                            ImgIds[i] = PhotoGalleryPhotos.First().Value[j].Id.ToString();
                            j++;
                        }

                    }
                }
                return Json(ImgIds, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }
        public IList<PhotoGallery> RecPhotoGallery(long Id)
        {
            ParentPortalService pps = new ParentPortalService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("ParentRefId", Id);
            Dictionary<long, IList<PhotoGallery>> PGList = pps.GetValuesFromPhotoGallery(0, 9999, string.Empty, string.Empty, criteria);
            IList<PhotoGallery> list = new List<PhotoGallery>();
            if (PGList.FirstOrDefault().Key > 0)
            {
                foreach (var items in PGList.FirstOrDefault().Value)
                {
                    pps.DeletePhotoGalleryPhotosByFolder_IdandId(items.Id, items.PGPreId);
                    pps.DeletePhotoGallery(items);
                    RecPhotoGallery(items.Id);
                }
            }
            return list;
        }
        #endregion

        #endregion end of photogallery methods
        /* End of PhotoGallery */
        #region start of food menu


        public string[] AcademicYear()
        {
            DateTime DateNow = DateTime.Now;
            string[] Academicyear = new string[2];
            Academicyear[0] = DateNow.Year.ToString() + "-" + (DateNow.Year + 1).ToString();
            Academicyear[1] = (DateNow.Year + 1).ToString() + "-" + (DateNow.Year + 2).ToString();
            return Academicyear;
        }

        public ActionResult FoodMenu()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ParentPortalService pps = new ParentPortalService();

                    FoodMenuView fm = new FoodMenuView();

                    Dictionary<string, object> bfcriteria = new Dictionary<string, object>();
                    Dictionary<string, object> lnvcriteria = new Dictionary<string, object>();
                    Dictionary<string, object> lvcriteria = new Dictionary<string, object>();

                    bfcriteria.Add("FoodType", "BreakFast");
                    lnvcriteria.Add("FoodType", "LunchNonVeg");
                    lvcriteria.Add("FoodType", "LunchVeg");

                    Dictionary<long, IList<FoodNameMaster>> cmpBreakFast = pps.GetCampusBreakFastListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, bfcriteria);
                    ViewBag.BreakFast = cmpBreakFast.First().Value;

                    Dictionary<long, IList<FoodNameMaster>> cmpLunchNV = pps.GetCampusBreakFastListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, lnvcriteria);
                    ViewBag.LunchNonVeg = cmpLunchNV.First().Value;

                    Dictionary<long, IList<FoodNameMaster>> cmpLunchV = pps.GetCampusBreakFastListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, lvcriteria);
                    ViewBag.LunchVeg = cmpLunchV.First().Value;

                    return View(fm);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        public ActionResult SaveFoodMenu(string campus, string months, string week, string day, string breakfast, string lunchnv, string lunchv, string notes)
        {
            ParentPortalService pps = new ParentPortalService();

            string userId = base.ValidateUser();

            FoodMenuView fm = new FoodMenuView();

            Dictionary<string, object> criteria = new Dictionary<string, object>();

            criteria.Add("Campus", campus);
            criteria.Add("Months", months);
            criteria.Add("Week", week);
            criteria.Add("Day", day);
            Dictionary<long, IList<FoodMenuView>> AssignmentName = pps.GetFoodMenuListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);

            if (AssignmentName != null && AssignmentName.First().Value != null && AssignmentName.First().Value.Count > 0)
            {
                var script = @"ErrMsg(""The given Details already Exists!"");";
                return JavaScript(script);
            }
            else
            {
                fm.Performer = userId;
                fm.CreatedOn = DateTime.Now.ToString();

                fm.Campus = campus;
                fm.Months = months;
                fm.Week = week;
                fm.Day = day;
                fm.BreakFast = breakfast;
                fm.LunchNonVeg = lunchnv;
                fm.LunchVeg = lunchv;
                fm.Notes = notes;

                pps.CreateOrUpdateFoodMenu(fm);
                var script = @"InfoMsg(""The given Food Menu added successfully."");";
                return JavaScript(script);
            }
        }

        public ActionResult SaveFoodMenubyWeek()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteFoodMenuList(string id)
        {
            try
            {
                ParentPortalService pps = new ParentPortalService();

                var test = id.Split(',');

                long[] idtodelete = new long[test.Length];
                int i = 0;
                foreach (string val in test)
                {
                    idtodelete[i] = Convert.ToInt64(val);

                    i++;
                }

                pps.DeleteFoodMenuDetails(idtodelete);

                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        public ActionResult FMWeek()
        {
            FMWeeks fmw = new FMWeeks();
            ViewBag.acadddl = AcademicYear();
            fmw.CreatedOn = DateTime.Now;
            return View(fmw);
        }

        public JsonResult FillCampusFoodMenu(string campus)
        {
            ParentPortalService pps = new ParentPortalService();

            Dictionary<string, object> bfcriteria = new Dictionary<string, object>();
            Dictionary<string, object> lnvcriteria = new Dictionary<string, object>();
            Dictionary<string, object> lvcriteria = new Dictionary<string, object>();
            Dictionary<string, object> scriteria = new Dictionary<string, object>();

            bfcriteria.Add("FoodType", "BreakFast");
            bfcriteria.Add("Campus", campus);

            lnvcriteria.Add("FoodType", "LunchNonVeg");
            lnvcriteria.Add("Campus", campus);

            lvcriteria.Add("FoodType", "LunchVeg");
            lvcriteria.Add("Campus", campus);

            scriteria.Add("FoodType", "Snacks");
            scriteria.Add("Campus", campus);

            Dictionary<long, IList<FoodNameMaster>> cmpBreakFast = pps.GetCampusBreakFastListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, bfcriteria);
            ViewBag.W1BreakFast = cmpBreakFast.First().Value;
            ViewBag.W2BreakFast = cmpBreakFast.First().Value;
            ViewBag.W3BreakFast = cmpBreakFast.First().Value;
            ViewBag.W4BreakFast = cmpBreakFast.First().Value;

            Dictionary<long, IList<FoodNameMaster>> cmpLunchNV = pps.GetCampusBreakFastListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, lnvcriteria);
            ViewBag.W1LunchNonVeg = cmpLunchNV.First().Value;
            ViewBag.W2LunchNonVeg = cmpLunchNV.First().Value;
            ViewBag.W3LunchNonVeg = cmpLunchNV.First().Value;
            ViewBag.W4LunchNonVeg = cmpLunchNV.First().Value;

            Dictionary<long, IList<FoodNameMaster>> cmpLunchV = pps.GetCampusBreakFastListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, lvcriteria);
            ViewBag.W1LunchVeg = cmpLunchV.First().Value;
            ViewBag.W2LunchVeg = cmpLunchV.First().Value;
            ViewBag.W3LunchVeg = cmpLunchV.First().Value;
            ViewBag.W4LunchVeg = cmpLunchV.First().Value;

            Dictionary<long, IList<FoodNameMaster>> cmpSnacks = pps.GetCampusBreakFastListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, scriteria);
            ViewBag.W1Snacks = cmpSnacks.First().Value;
            ViewBag.W2Snacks = cmpSnacks.First().Value;
            ViewBag.W3Snacks = cmpSnacks.First().Value;
            ViewBag.W4Snacks = cmpSnacks.First().Value;

            return Json(ViewBag.W1BreakFast, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddNewFoodMenu()
        {
            try
            {
                FMWeeks fmw = new FMWeeks();
                ParentPortalService pps = new ParentPortalService();

                Dictionary<string, object> bfcriteria = new Dictionary<string, object>();
                Dictionary<string, object> lnvcriteria = new Dictionary<string, object>();
                Dictionary<string, object> lvcriteria = new Dictionary<string, object>();
                Dictionary<string, object> scriteria = new Dictionary<string, object>();

                bfcriteria.Add("FoodType", "BreakFast");

                lnvcriteria.Add("FoodType", "LunchNonVeg");
                lvcriteria.Add("FoodType", "LunchVeg");

                scriteria.Add("FoodType", "Snacks");

                Dictionary<long, IList<FoodNameMaster>> cmpBreakFast = pps.GetCampusBreakFastListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, bfcriteria);
                ViewBag.W1BreakFast = cmpBreakFast.First().Value;
                ViewBag.W2BreakFast = cmpBreakFast.First().Value;
                ViewBag.W3BreakFast = cmpBreakFast.First().Value;
                ViewBag.W4BreakFast = cmpBreakFast.First().Value;

                Dictionary<long, IList<FoodNameMaster>> cmpLunchNV = pps.GetCampusBreakFastListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, lnvcriteria);
                ViewBag.W1LunchNonVeg = cmpLunchNV.First().Value;
                ViewBag.W2LunchNonVeg = cmpLunchNV.First().Value;
                ViewBag.W3LunchNonVeg = cmpLunchNV.First().Value;
                ViewBag.W4LunchNonVeg = cmpLunchNV.First().Value;

                Dictionary<long, IList<FoodNameMaster>> cmpLunchV = pps.GetCampusBreakFastListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, lvcriteria);
                ViewBag.W1LunchVeg = cmpLunchV.First().Value;
                ViewBag.W2LunchVeg = cmpLunchV.First().Value;
                ViewBag.W3LunchVeg = cmpLunchV.First().Value;
                ViewBag.W4LunchVeg = cmpLunchV.First().Value;

                Dictionary<long, IList<FoodNameMaster>> cmpSnacks = pps.GetCampusBreakFastListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, scriteria);
                ViewBag.W1Snacks = cmpSnacks.First().Value;
                ViewBag.W2Snacks = cmpSnacks.First().Value;
                ViewBag.W3Snacks = cmpSnacks.First().Value;
                ViewBag.W4Snacks = cmpSnacks.First().Value;

                ViewBag.acadddl = AcademicYear();
                fmw.Campus = "";
                return View(fmw);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult AddNewFoodMenu(FMWeeks fmw1, FormCollection fc1)
        {
            try
            {
                ParentPortalService pps = new ParentPortalService();
                long id = 0;
                string userId = base.ValidateUser();

                if (ModelState.IsValid)
                {
                    if (Request.Form["btnSubmit"] == "Submit")
                    {
                        if (id == 0)
                        {
                            fmw1.Performer = userId;
                            fmw1.CreatedOn = DateTime.Now;

                            // Array 0-4 for week 1

                            // for Monday

                            fmw1.FMDaysList[0].Week = "Week1";
                            fmw1.FMDaysList[0].Day = "MONDAY";

                            string fmw1MonBreakFastList = "";
                            string fmw1MonLunchNVList = "";
                            string fmw1MonLunchVList = "";
                            string fmw1MonSnacksList = "";

                            foreach (var item in fmw1.FMDaysList[0].BreakFast1)
                            {
                                fmw1MonBreakFastList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[0].LunchNonVeg1)
                            {
                                fmw1MonLunchNVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[0].LunchVeg1)
                            {
                                fmw1MonLunchVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[0].Snacks1)
                            {
                                fmw1MonSnacksList += item + ", ";
                            }

                            fmw1.FMDaysList[0].BreakFast = fmw1MonBreakFastList;
                            fmw1.FMDaysList[0].LunchNonVeg = fmw1MonLunchNVList;
                            fmw1.FMDaysList[0].LunchVeg = fmw1MonLunchVList;
                            fmw1.FMDaysList[0].Snacks = fmw1MonSnacksList;

                            // for Tuesday

                            fmw1.FMDaysList[1].Week = "Week1";
                            fmw1.FMDaysList[1].Day = "TUESDAY";

                            string fmw1TueBreakFastList = "";
                            string fmw1TueLunchNVList = "";
                            string fmw1TueLunchVList = "";
                            string fmw1TueSnacksList = "";

                            foreach (var item in fmw1.FMDaysList[1].BreakFast1)
                            {
                                fmw1TueBreakFastList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[1].LunchNonVeg1)
                            {
                                fmw1TueLunchNVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[1].LunchVeg1)
                            {
                                fmw1TueLunchVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[1].Snacks1)
                            {
                                fmw1TueSnacksList += item + ", ";
                            }

                            fmw1.FMDaysList[1].BreakFast = fmw1TueBreakFastList;
                            fmw1.FMDaysList[1].LunchNonVeg = fmw1TueLunchNVList;
                            fmw1.FMDaysList[1].LunchVeg = fmw1TueLunchVList;
                            fmw1.FMDaysList[1].Snacks = fmw1TueSnacksList;

                            // for wednesday

                            fmw1.FMDaysList[2].Week = "Week1";
                            fmw1.FMDaysList[2].Day = "WEDNESDAY";

                            string fmw1WedBreakFastList = "";
                            string fmw1WedLunchNVList = "";
                            string fmw1WedLunchVList = "";
                            string fmw1WedSnacksList = "";

                            foreach (var item in fmw1.FMDaysList[2].BreakFast1)
                            {
                                fmw1WedBreakFastList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[2].LunchNonVeg1)
                            {
                                fmw1WedLunchNVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[2].LunchVeg1)
                            {
                                fmw1WedLunchVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[2].Snacks1)
                            {
                                fmw1WedSnacksList += item + ", ";
                            }

                            fmw1.FMDaysList[2].BreakFast = fmw1WedBreakFastList;
                            fmw1.FMDaysList[2].LunchNonVeg = fmw1WedLunchNVList;
                            fmw1.FMDaysList[2].LunchVeg = fmw1WedLunchVList;
                            fmw1.FMDaysList[2].Snacks = fmw1WedSnacksList;

                            // for Thursday

                            fmw1.FMDaysList[3].Week = "Week1";
                            fmw1.FMDaysList[3].Day = "THURSDAY";

                            string fmw1ThuBreakFastList = "";
                            string fmw1ThuLunchNVList = "";
                            string fmw1ThuLunchVList = "";
                            string fmw1ThuSnacksList = "";

                            foreach (var item in fmw1.FMDaysList[3].BreakFast1)
                            {
                                fmw1ThuBreakFastList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[3].LunchNonVeg1)
                            {
                                fmw1ThuLunchNVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[3].LunchVeg1)
                            {
                                fmw1ThuLunchVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[3].Snacks1)
                            {
                                fmw1ThuSnacksList += item + ", ";
                            }

                            fmw1.FMDaysList[3].BreakFast = fmw1ThuBreakFastList;
                            fmw1.FMDaysList[3].LunchNonVeg = fmw1ThuLunchNVList;
                            fmw1.FMDaysList[3].LunchVeg = fmw1ThuLunchVList;
                            fmw1.FMDaysList[3].Snacks = fmw1ThuSnacksList;


                            // for Friday

                            fmw1.FMDaysList[4].Week = "Week1";
                            fmw1.FMDaysList[4].Day = "FRIDAY";

                            string fmw1FriBreakFastList = "";
                            string fmw1FriLunchNVList = "";
                            string fmw1FriLunchVList = "";
                            string fmw1FriSnacksList = "";

                            foreach (var item in fmw1.FMDaysList[4].BreakFast1)
                            {
                                fmw1FriBreakFastList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[4].LunchNonVeg1)
                            {
                                fmw1FriLunchNVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[4].LunchVeg1)
                            {
                                fmw1FriLunchVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[4].Snacks1)
                            {
                                fmw1FriSnacksList += item + ", ";
                            }

                            fmw1.FMDaysList[4].BreakFast = fmw1FriBreakFastList;
                            fmw1.FMDaysList[4].LunchNonVeg = fmw1FriLunchNVList;
                            fmw1.FMDaysList[4].LunchVeg = fmw1FriLunchVList;
                            fmw1.FMDaysList[4].Snacks = fmw1FriSnacksList;


                            // Array 5-9 for week 2

                            // for Monday

                            fmw1.FMDaysList[5].Week = "Week2";
                            fmw1.FMDaysList[5].Day = "MONDAY";

                            string fmw2MonBreakFastList = "";
                            string fmw2MonLunchNVList = "";
                            string fmw2MonLunchVList = "";
                            string fmw2MonSnacksList = "";

                            foreach (var item in fmw1.FMDaysList[5].BreakFast1)
                            {
                                fmw2MonBreakFastList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[5].LunchNonVeg1)
                            {
                                fmw2MonLunchNVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[5].LunchVeg1)
                            {
                                fmw2MonLunchVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[5].Snacks1)
                            {
                                fmw2MonSnacksList += item + ", ";
                            }

                            fmw1.FMDaysList[5].BreakFast = fmw2MonBreakFastList;
                            fmw1.FMDaysList[5].LunchNonVeg = fmw2MonLunchNVList;
                            fmw1.FMDaysList[5].LunchVeg = fmw2MonLunchVList;
                            fmw1.FMDaysList[5].Snacks = fmw2MonSnacksList;

                            // for Tuesday

                            fmw1.FMDaysList[6].Week = "Week2";
                            fmw1.FMDaysList[6].Day = "TUESDAY";

                            string fmw2TueBreakFastList = "";
                            string fmw2TueLunchNVList = "";
                            string fmw2TueLunchVList = "";
                            string fmw2TueSnacksList = "";

                            foreach (var item in fmw1.FMDaysList[6].BreakFast1)
                            {
                                fmw2TueBreakFastList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[6].LunchNonVeg1)
                            {
                                fmw2TueLunchNVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[6].LunchVeg1)
                            {
                                fmw2TueLunchVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[6].Snacks1)
                            {
                                fmw2TueSnacksList += item + ", ";
                            }

                            fmw1.FMDaysList[6].BreakFast = fmw2TueBreakFastList;
                            fmw1.FMDaysList[6].LunchNonVeg = fmw2TueLunchNVList;
                            fmw1.FMDaysList[6].LunchVeg = fmw2TueLunchVList;
                            fmw1.FMDaysList[6].Snacks = fmw2TueSnacksList;

                            // for wednesday

                            fmw1.FMDaysList[7].Week = "Week2";
                            fmw1.FMDaysList[7].Day = "WEDNESDAY";

                            string fmw2WedBreakFastList = "";
                            string fmw2WedLunchNVList = "";
                            string fmw2WedLunchVList = "";
                            string fmw2WedSnacksList = "";

                            foreach (var item in fmw1.FMDaysList[7].BreakFast1)
                            {
                                fmw2WedBreakFastList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[7].LunchNonVeg1)
                            {
                                fmw2WedLunchNVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[7].LunchVeg1)
                            {
                                fmw2WedLunchVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[7].Snacks1)
                            {
                                fmw2WedSnacksList += item + ", ";
                            }

                            fmw1.FMDaysList[7].BreakFast = fmw2WedBreakFastList;
                            fmw1.FMDaysList[7].LunchNonVeg = fmw2WedLunchNVList;
                            fmw1.FMDaysList[7].LunchVeg = fmw2WedLunchVList;
                            fmw1.FMDaysList[7].Snacks = fmw2WedSnacksList;

                            // for Thursday

                            fmw1.FMDaysList[8].Week = "Week2";
                            fmw1.FMDaysList[8].Day = "THURSDAY";

                            string fmw2ThuBreakFastList = "";
                            string fmw2ThuLunchNVList = "";
                            string fmw2ThuLunchVList = "";
                            string fmw2ThuSnacksList = "";

                            foreach (var item in fmw1.FMDaysList[8].BreakFast1)
                            {
                                fmw2ThuBreakFastList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[8].LunchNonVeg1)
                            {
                                fmw2ThuLunchNVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[8].LunchVeg1)
                            {
                                fmw2ThuLunchVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[8].Snacks1)
                            {
                                fmw2ThuSnacksList += item + ", ";
                            }

                            fmw1.FMDaysList[8].BreakFast = fmw2ThuBreakFastList;
                            fmw1.FMDaysList[8].LunchNonVeg = fmw2ThuLunchNVList;
                            fmw1.FMDaysList[8].LunchVeg = fmw2ThuLunchVList;
                            fmw1.FMDaysList[8].Snacks = fmw2ThuSnacksList;


                            // for Friday

                            fmw1.FMDaysList[9].Week = "Week2";
                            fmw1.FMDaysList[9].Day = "FRIDAY";

                            string fmw2FriBreakFastList = "";
                            string fmw2FriLunchNVList = "";
                            string fmw2FriLunchVList = "";
                            string fmw2FriSnacksList = "";

                            foreach (var item in fmw1.FMDaysList[9].BreakFast1)
                            {
                                fmw2FriBreakFastList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[9].LunchNonVeg1)
                            {
                                fmw2FriLunchNVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[9].LunchVeg1)
                            {
                                fmw2FriLunchVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[9].Snacks1)
                            {
                                fmw2FriSnacksList += item + ", ";
                            }

                            fmw1.FMDaysList[9].BreakFast = fmw2FriBreakFastList;
                            fmw1.FMDaysList[9].LunchNonVeg = fmw2FriLunchNVList;
                            fmw1.FMDaysList[9].LunchVeg = fmw2FriLunchVList;
                            fmw1.FMDaysList[9].Snacks = fmw2FriSnacksList;

                            // Array 10-14 for week 3


                            // for Monday

                            fmw1.FMDaysList[10].Week = "Week3";
                            fmw1.FMDaysList[10].Day = "MONDAY";

                            string fmw3MonBreakFastList = "";
                            string fmw3MonLunchNVList = "";
                            string fmw3MonLunchVList = "";
                            string fmw3MonSnacksList = "";

                            foreach (var item in fmw1.FMDaysList[10].BreakFast1)
                            {
                                fmw3MonBreakFastList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[10].LunchNonVeg1)
                            {
                                fmw3MonLunchNVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[10].LunchVeg1)
                            {
                                fmw3MonLunchVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[10].Snacks1)
                            {
                                fmw3MonSnacksList += item + ", ";
                            }

                            fmw1.FMDaysList[10].BreakFast = fmw3MonBreakFastList;
                            fmw1.FMDaysList[10].LunchNonVeg = fmw3MonLunchNVList;
                            fmw1.FMDaysList[10].LunchVeg = fmw3MonLunchVList;
                            fmw1.FMDaysList[10].Snacks = fmw3MonSnacksList;

                            // for Tuesday

                            fmw1.FMDaysList[11].Week = "Week3";
                            fmw1.FMDaysList[11].Day = "TUESDAY";

                            string fmw3TueBreakFastList = "";
                            string fmw3TueLunchNVList = "";
                            string fmw3TueLunchVList = "";
                            string fmw3TueSnacksList = "";

                            foreach (var item in fmw1.FMDaysList[11].BreakFast1)
                            {
                                fmw3TueBreakFastList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[11].LunchNonVeg1)
                            {
                                fmw3TueLunchNVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[11].LunchVeg1)
                            {
                                fmw3TueLunchVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[11].Snacks1)
                            {
                                fmw3TueSnacksList += item + ", ";
                            }

                            fmw1.FMDaysList[11].BreakFast = fmw3TueBreakFastList;
                            fmw1.FMDaysList[11].LunchNonVeg = fmw3TueLunchNVList;
                            fmw1.FMDaysList[11].LunchVeg = fmw3TueLunchVList;
                            fmw1.FMDaysList[11].Snacks = fmw3TueSnacksList;

                            // for wednesday

                            fmw1.FMDaysList[12].Week = "Week3";
                            fmw1.FMDaysList[12].Day = "WEDNESDAY";

                            string fmw3WedBreakFastList = "";
                            string fmw3WedLunchNVList = "";
                            string fmw3WedLunchVList = "";
                            string fmw3WedSnacksList = "";

                            foreach (var item in fmw1.FMDaysList[12].BreakFast1)
                            {
                                fmw3WedBreakFastList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[12].LunchNonVeg1)
                            {
                                fmw3WedLunchNVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[12].LunchVeg1)
                            {
                                fmw3WedLunchVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[12].Snacks1)
                            {
                                fmw3WedSnacksList += item + ", ";
                            }

                            fmw1.FMDaysList[12].BreakFast = fmw3WedBreakFastList;
                            fmw1.FMDaysList[12].LunchNonVeg = fmw3WedLunchNVList;
                            fmw1.FMDaysList[12].LunchVeg = fmw3WedLunchVList;
                            fmw1.FMDaysList[12].Snacks = fmw3WedSnacksList;

                            // for Thursday

                            fmw1.FMDaysList[13].Week = "Week3";
                            fmw1.FMDaysList[13].Day = "THURSDAY";

                            string fmw3ThuBreakFastList = "";
                            string fmw3ThuLunchNVList = "";
                            string fmw3ThuLunchVList = "";
                            string fmw3ThuSnacksList = "";

                            foreach (var item in fmw1.FMDaysList[13].BreakFast1)
                            {
                                fmw3ThuBreakFastList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[13].LunchNonVeg1)
                            {
                                fmw3ThuLunchNVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[13].LunchVeg1)
                            {
                                fmw3ThuLunchVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[13].Snacks1)
                            {
                                fmw3ThuSnacksList += item + ", ";
                            }

                            fmw1.FMDaysList[13].BreakFast = fmw3ThuBreakFastList;
                            fmw1.FMDaysList[13].LunchNonVeg = fmw3ThuLunchNVList;
                            fmw1.FMDaysList[13].LunchVeg = fmw3ThuLunchVList;
                            fmw1.FMDaysList[13].Snacks = fmw3ThuSnacksList;


                            // for Friday

                            fmw1.FMDaysList[14].Week = "Week3";
                            fmw1.FMDaysList[14].Day = "FRIDAY";

                            string fmw3FriBreakFastList = "";
                            string fmw3FriLunchNVList = "";
                            string fmw3FriLunchVList = "";
                            string fmw3FriSnacksList = "";

                            foreach (var item in fmw1.FMDaysList[14].BreakFast1)
                            {
                                fmw3FriBreakFastList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[14].LunchNonVeg1)
                            {
                                fmw3FriLunchNVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[14].LunchVeg1)
                            {
                                fmw3FriLunchVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[14].Snacks1)
                            {
                                fmw3FriSnacksList += item + ", ";
                            }

                            fmw1.FMDaysList[14].BreakFast = fmw3FriBreakFastList;
                            fmw1.FMDaysList[14].LunchNonVeg = fmw3FriLunchNVList;
                            fmw1.FMDaysList[14].LunchVeg = fmw3FriLunchVList;
                            fmw1.FMDaysList[14].Snacks = fmw3FriSnacksList;

                            // Array 15-19 for week 4

                            // for Monday

                            fmw1.FMDaysList[15].Week = "Week4";
                            fmw1.FMDaysList[15].Day = "MONDAY";

                            string fmw4MonBreakFastList = "";
                            string fmw4MonLunchNVList = "";
                            string fmw4MonLunchVList = "";
                            string fmw4MonSnacksList = "";

                            foreach (var item in fmw1.FMDaysList[15].BreakFast1)
                            {
                                fmw4MonBreakFastList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[15].LunchNonVeg1)
                            {
                                fmw4MonLunchNVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[15].LunchVeg1)
                            {
                                fmw4MonLunchVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[15].Snacks1)
                            {
                                fmw4MonSnacksList += item + ", ";
                            }

                            fmw1.FMDaysList[15].BreakFast = fmw4MonBreakFastList;
                            fmw1.FMDaysList[15].LunchNonVeg = fmw4MonLunchNVList;
                            fmw1.FMDaysList[15].LunchVeg = fmw4MonLunchVList;
                            fmw1.FMDaysList[15].Snacks = fmw4MonSnacksList;

                            // for Tuesday

                            fmw1.FMDaysList[16].Week = "Week4";
                            fmw1.FMDaysList[16].Day = "TUESDAY";

                            string fmw4TueBreakFastList = "";
                            string fmw4TueLunchNVList = "";
                            string fmw4TueLunchVList = "";
                            string fmw4TueSnacksList = "";

                            foreach (var item in fmw1.FMDaysList[16].BreakFast1)
                            {
                                fmw4TueBreakFastList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[16].LunchNonVeg1)
                            {
                                fmw4TueLunchNVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[16].LunchVeg1)
                            {
                                fmw4TueLunchVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[16].Snacks1)
                            {
                                fmw4TueSnacksList += item + ", ";
                            }

                            fmw1.FMDaysList[16].BreakFast = fmw4TueBreakFastList;
                            fmw1.FMDaysList[16].LunchNonVeg = fmw4TueLunchNVList;
                            fmw1.FMDaysList[16].LunchVeg = fmw4TueLunchVList;
                            fmw1.FMDaysList[16].Snacks = fmw4TueSnacksList;

                            // for wednesday

                            fmw1.FMDaysList[17].Week = "Week4";
                            fmw1.FMDaysList[17].Day = "WEDNESDAY";

                            string fmw4WedBreakFastList = "";
                            string fmw4WedLunchNVList = "";
                            string fmw4WedLunchVList = "";
                            string fmw4WedSnacksList = "";

                            foreach (var item in fmw1.FMDaysList[17].BreakFast1)
                            {
                                fmw4WedBreakFastList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[17].LunchNonVeg1)
                            {
                                fmw4WedLunchNVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[17].LunchVeg1)
                            {
                                fmw4WedLunchVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[17].Snacks1)
                            {
                                fmw4WedSnacksList += item + ", ";
                            }

                            fmw1.FMDaysList[17].BreakFast = fmw4WedBreakFastList;
                            fmw1.FMDaysList[17].LunchNonVeg = fmw4WedLunchNVList;
                            fmw1.FMDaysList[17].LunchVeg = fmw4WedLunchVList;
                            fmw1.FMDaysList[17].Snacks = fmw4WedSnacksList;

                            // for Thursday

                            fmw1.FMDaysList[18].Week = "Week4";
                            fmw1.FMDaysList[18].Day = "THURSDAY";

                            string fmw4ThuBreakFastList = "";
                            string fmw4ThuLunchNVList = "";
                            string fmw4ThuLunchVList = "";
                            string fmw4ThuSnacksList = "";

                            foreach (var item in fmw1.FMDaysList[18].BreakFast1)
                            {
                                fmw4ThuBreakFastList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[18].LunchNonVeg1)
                            {
                                fmw4ThuLunchNVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[18].LunchVeg1)
                            {
                                fmw4ThuLunchVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[18].Snacks1)
                            {
                                fmw4ThuSnacksList += item + ", ";
                            }

                            fmw1.FMDaysList[18].BreakFast = fmw4ThuBreakFastList;
                            fmw1.FMDaysList[18].LunchNonVeg = fmw4ThuLunchNVList;
                            fmw1.FMDaysList[18].LunchVeg = fmw4ThuLunchVList;
                            fmw1.FMDaysList[18].Snacks = fmw4ThuSnacksList;


                            // for Friday

                            fmw1.FMDaysList[19].Week = "Week4";
                            fmw1.FMDaysList[19].Day = "FRIDAY";

                            string fmw4FriBreakFastList = "";
                            string fmw4FriLunchNVList = "";
                            string fmw4FriLunchVList = "";
                            string fmw4FriSnacksList = "";

                            foreach (var item in fmw1.FMDaysList[19].BreakFast1)
                            {
                                fmw4FriBreakFastList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[19].LunchNonVeg1)
                            {
                                fmw4FriLunchNVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[19].LunchVeg1)
                            {
                                fmw4FriLunchVList += item + ", ";
                            }

                            foreach (var item in fmw1.FMDaysList[19].Snacks1)
                            {
                                fmw4FriSnacksList += item + ", ";
                            }

                            fmw1.FMDaysList[19].BreakFast = fmw4FriBreakFastList;
                            fmw1.FMDaysList[19].LunchNonVeg = fmw4FriLunchNVList;
                            fmw1.FMDaysList[19].LunchVeg = fmw4FriLunchVList;
                            fmw1.FMDaysList[19].Snacks = fmw4FriSnacksList;


                            pps.CreateOrUpdateFoodMenuByWeeks(fmw1);

                            //pps.CreateOrUpdateFoodMenu(fm1);
                            //pps.CreateOrUpdateFoodNameMater(fm1);

                            TempData["SuccessFNMCreation"] = fmw1.Id;

                            return RedirectToAction("FMWeek", new { id = fmw1.Id });
                        }
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        public JsonResult FoodMenuListJqGrid(string campus, string month, string week, string academicyear, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                ParentPortalService pps = new ParentPortalService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();

                if (!string.IsNullOrWhiteSpace(campus)) { criteria.Add("Campus", campus); }
                if (!string.IsNullOrWhiteSpace(month)) { criteria.Add("Months", month); }
                if (!string.IsNullOrWhiteSpace(week)) { criteria.Add("Week", week); }
                if (!string.IsNullOrWhiteSpace(academicyear)) { criteria.Add("AcademicYear", academicyear); }

                string UserId = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();

                if (!string.IsNullOrWhiteSpace(sord) && sord == "desc")
                    sord = "Desc";
                else
                    sord = "Asc";

                criteria.Add("Performer", UserId);

                Dictionary<long, IList<FoodMenuView>> nDet = pps.GetValuesFromFoodMenu(page - 1, rows, sidx, sord, criteria);
                string title = "Foodmenu";
                if (nDet != null && nDet.Count > 0)
                {

                    if (ExportType == "PDF")
                    {
                        string[] TblHeaders = new string[] { "  Day   ", " ", "  Non-Veg   ", "   Veg    ", "   Snacks  " };
                        float[] widths = new float[] { 20f, 20f, 20f, 20f, 20f };
                        var Foodmenu = (from items in nDet.First().Value
                                        select new
                                        {
                                            items.Day,
                                            items.BreakFast,
                                            items.LunchNonVeg,
                                            items.LunchVeg,
                                            items.Snacks,
                                        }).ToList();
                        DataTable dt = ListToDataTable(Foodmenu);
                        ExportToPDF_FoodMenu(title, TblHeaders, widths, dt, campus, month, week);
                    }
                    else
                    {
                        long totalRecords = nDet.First().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);

                        var notedet = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows = (from items in nDet.First().Value
                                    select new
                                    {
                                        i = items.Id,
                                        cell = new string[] 
                                { 
                                   items.Id.ToString(),
                                   items.Campus,
                                   items.Months,
                                   items.Week,
                                   items.Day,
                                   items.BreakFast,
                                   items.LunchNonVeg,
                                   items.LunchVeg,
                                   items.Snacks,
                                   items.Notes,
                                   items.CreatedOn
                                   
                                }
                                    })
                        };
                        return Json(notedet, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        public ActionResult FoodNameListMaster()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ParentPortalService pps = new ParentPortalService();

                    FoodNameMaster fnm = new FoodNameMaster();


                    return View(fnm);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult FoodNameListMaster(FoodNameMaster fnm1, FormCollection fc)
        {
            try
            {
                ParentPortalService pps = new ParentPortalService();

                string userId = base.ValidateUser();
                long id = 0;
                if (ModelState.IsValid)
                {
                    if (Request.Form["btnSubmit"] == "Submit")
                    {
                        if (id == 0)
                        {
                            fnm1.Performer = userId;
                            fnm1.CreatedOn = DateTime.Now.ToString();

                            pps.CreateOrUpdateFoodNameMater(fnm1);

                            TempData["SuccessFNMCreation"] = fnm1.Id;

                            return RedirectToAction("FoodNameListMaster", new { id = fnm1.Id });
                        }
                    }
                }
                return View(fnm1);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        public JsonResult FoodNameListMasterJqGrid(long? NoteId, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                ParentPortalService pps = new ParentPortalService();

                Dictionary<string, object> criteria = new Dictionary<string, object>();

                string UserId = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();

                if (!string.IsNullOrWhiteSpace(sord) && sord == "desc")
                    sord = "Desc";
                else
                    sord = "Asc";

                criteria.Add("Performer", UserId);

                Dictionary<long, IList<FoodNameMaster>> nDet = pps.GetValuesFromFoodNameMaster(page - 1, rows, sidx, sord, criteria);
                long totalRecords = nDet.First().Key;
                int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);

                var notedet = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = (from items in nDet.First().Value
                            select new
                            {
                                i = items.Id,
                                cell = new string[] 
                                { 
                                   items.Id.ToString(),
                                   items.Campus,
                                   items.FoodType,
                                   items.FoodName,
                                   items.Performer,
                                   items.CreatedOn
                                }
                            })
                };
                return Json(notedet, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteFoodNameMater(string id)
        {
            try
            {
                ParentPortalService pps = new ParentPortalService();

                var test = id.Split(',');

                long[] idtodelete = new long[test.Length];
                int i = 0;
                foreach (string val in test)
                {
                    idtodelete[i] = Convert.ToInt64(val);
                    i++;
                }

                pps.DeleteFoodNameMaterDetails(idtodelete);

                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                throw ex;
            }
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult FillCampusBreakFast(string campus)
        {
            try
            {
                ParentPortalService pps = new ParentPortalService();

                Dictionary<string, object> criteria = new Dictionary<string, object>();

                criteria.Add("Campus", campus);
                criteria.Add("FoodType", "BreakFast");

                Dictionary<long, IList<FoodNameMaster>> cmpBreakFast = pps.GetCampusBreakFastListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);

                ViewBag.BreakFast = cmpBreakFast.First().Value;
                return Json(ViewBag.BreakFast, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }


        public void ExportToPDF_FoodMenu(string Title, string[] TblHeaders, float[] widths, DataTable dt, string campus, string month, string week)
        {
            Document document = new Document(PageSize.A4, 10.25f, 10.25f, 10.5f, 10.5f);

            // For PDF export we are using the free open-source iTextSharp library.

            Document pdfDoc = new Document();
            pdfDoc.AddTitle(Title);
            MemoryStream pdfStream = new MemoryStream();
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, pdfStream);

            pdfDoc.Open();//Open Document to write
            pdfDoc.NewPage();

            // Font Heading = FontFactory.GetFont("ARIAL", 6, BaseColor.BLACK);
            Font Content = FontFactory.GetFont("ARIAL", 8);

            PdfPTable Pdfheader = new PdfPTable(dt.Columns.Count);
            Pdfheader.TotalWidth = 500f;
            Pdfheader.LockedWidth = true;
            float[] width = new float[] { 60f, 90f, 100f, 100f, 50f };
            Pdfheader.SetWidths(width);


            #region "Logo Image"
            iTextSharp.text.Image LogoImage;
            PdfPCell imgcel = new PdfPCell();
            imgcel.Colspan = 2;
            //imgcel.Rowspan = 2;
            imgcel.PaddingTop = 1;
            imgcel.PaddingBottom = 5;
            imgcel.Border = 0;
            string ImagePath = ConfigurationManager.AppSettings["RptCard"] + "logo.jpg";
            LogoImage = iTextSharp.text.Image.GetInstance(ImagePath);
            LogoImage.ScaleAbsolute(50, 50);
            imgcel.AddElement(LogoImage);
            imgcel.PaddingLeft = 1;
            Pdfheader.AddCell(imgcel);
            #endregion "Logo Image"

            PdfPCell headcel = new PdfPCell(new Phrase("THE INDIAN PUBLIC SCHOOL, COIMBATORE", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 8.0f, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0)))));
            headcel.Colspan = 3;
            headcel.Border = 0;
            headcel.HorizontalAlignment = Element.ALIGN_LEFT;
            headcel.Padding = 3;
            Pdfheader.AddCell(headcel);


            PdfPCell cel = new PdfPCell(new Phrase("Campus  :   " + campus + "", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 6.0f, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0)))));
            cel.Colspan = 5;
            cel.Border = 0;
            cel.Padding = 5;
            cel.HorizontalAlignment = Element.ALIGN_RIGHT;
            Pdfheader.AddCell(cel);


            PdfPCell cellhead = new PdfPCell(new Phrase(week, new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 8.0f, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0)))));
            cellhead.Colspan = 5;
            cellhead.Padding = 5;
            cellhead.HorizontalAlignment = Element.ALIGN_CENTER;
            Pdfheader.AddCell(cellhead);


            PdfPCell cell1 = new PdfPCell(new Phrase("8 AM BREAK FAST", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 8.0f, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0)))));
            cell1.Colspan = 2;
            cellhead.Padding = 5;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            Pdfheader.AddCell(cell1);

            PdfPCell cell2 = new PdfPCell(new Phrase("12 PM LUNCH", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 8.0f, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0)))));
            cell2.Colspan = 3;
            cell2.Padding = 5;
            cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            Pdfheader.AddCell(cell2);

            PdfPCell cell3 = new PdfPCell(new Phrase("  DAY ", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 8.0f, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0)))));
            cell3.Colspan = 1;
            cell3.Padding = 5;
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            Pdfheader.AddCell(cell3);

            PdfPCell cell4 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 8.0f, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0)))));
            cell4.Colspan = 1;
            cell4.Padding = 5;
            cell4.HorizontalAlignment = Element.ALIGN_CENTER;
            Pdfheader.AddCell(cell4);

            PdfPCell cell5 = new PdfPCell(new Phrase("  Non-Veg   ", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 8.0f, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0)))));
            cell5.Colspan = 1;
            cell5.Padding = 5;
            cell5.HorizontalAlignment = Element.ALIGN_CENTER;
            Pdfheader.AddCell(cell5);

            PdfPCell cell6 = new PdfPCell(new Phrase("  Veg   ", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 8.0f, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0)))));
            cell6.Colspan = 2;
            cell6.Padding = 5;
            cell6.HorizontalAlignment = Element.ALIGN_CENTER;
            Pdfheader.AddCell(cell6);

            PdfPCell PdfPCell = null;


            //How add the data from datatable to pdf table
            for (int rows = 0; rows < dt.Rows.Count; rows++)
            {
                for (int column = 0; column < dt.Columns.Count; column++)
                {
                    PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows][column].ToString(), Content)));
                    PdfPCell.Padding = 6;
                    Pdfheader.AddCell(PdfPCell);
                }
            }

            PdfPCell cell7 = new PdfPCell(new Phrase("Note :Eggs for daily breakfast for Non-veg students", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 8.0f, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0)))));
            cell7.HorizontalAlignment = Element.ALIGN_CENTER;
            cell7.Colspan = 5;
            cell7.Padding = 5;
            Pdfheader.AddCell(cell7);


            Pdfheader.SpacingBefore = 7f; // Give some space after the text or it may overlap the table 
            pdfDoc.Add(Pdfheader);
            pdfDoc.Close();
            pdfWriter.Close();

            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Title + ".pdf");
            Response.BinaryWrite(pdfStream.ToArray());
            Response.End();
        }

        #endregion end of food menu
    }
}
