using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TIPS.Entities.AdmissionEntities;
using TIPS.Entities.Assess;
using TIPS.Entities.HRManagementEntities;
using TIPS.Entities.QAEntities;
using TIPS.Entities.StaffManagementEntities;
using TIPS.Service;
using TIPS.ServiceContract;

namespace CMS.Controllers
{
    public class QAController : BaseController
    {
        string loggedInUserId = string.Empty;
        QAService QAS = new QAService();
        HRManagementService Hrms = new HRManagementService();
        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
        CallManagementService cms = new CallManagementService();
        UserService userservice = new UserService();

        public ActionResult QAIndex()
        {
            try
            {
                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj == null || (Userobj != null && Userobj.UserId == null))
                { return RedirectToAction("LogOn", "Account"); }
                loggedInUserId = Userobj.UserId;
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likeSearchCriteria = new Dictionary<string, object>();
                //CampusBasedStaffDetails staff = new CampusBasedStaffDetails();
                //staff = QAS.GetCampusBasedStaffDetailsbyUserId(loggedInUserId);
                //if (!string.IsNullOrWhiteSpace(staff.Campus)) { }
                if (Userobj.Campus != null)
                    @ViewBag.Campus = Userobj.Campus;
                else
                    @ViewBag.Campus = "Campus";

                criteria.Add("ProcessedDate", DateTime.Today);
                Dictionary<long, IList<QAProcessLog>> QADetails = QAS.GetQAProcessLogListWithPagingAndCriteria(0, 1, string.Empty, string.Empty, criteria, likeSearchCriteria);
                if (QADetails.First().Key == 0)
                {
                    QAQuestionEscalation();
                }

                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }

        }

        public ActionResult FillSubjectByUserId()
        {
            try
            {
                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj == null || (Userobj != null && Userobj.UserId == null))
                { return RedirectToAction("LogOn", "Account"); }
                loggedInUserId = Userobj.UserId;
                AdmissionManagementService ams = new AdmissionManagementService();
                Assess360Service assesservice = new Assess360Service();

                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likeSearchCriteria = new Dictionary<string, object>();
                //criteria.Add("Campus", st.Campus);
                //criteria.Add("Grade", st.Grade);
                criteria.Add("UserId", loggedInUserId);
                Dictionary<long, IList<CampusBasedStaffDetails>> staffDetailslist = QAS.GetCampusBasedStaffDetailsListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria, likeSearchCriteria);
                var subjectlist = (from u in staffDetailslist select u).ToList();

                //Dictionary<long, IList<SubjectMaster>> subjectmaster = assesservice.GetSubjectMasterListWithPagingAndCriteria(0, 999, string.Empty, string.Empty, criteria);
                if (subjectlist != null && subjectlist.First().Value != null && subjectlist.First().Value.Count > 0)
                {
                    var LocationMasterList = (
                                             from items in subjectlist.First().Value
                                             select new
                                             {
                                                 Text = items.Subject,
                                                 Value = items.Subject
                                             }).Distinct().ToList().OrderBy(x => x.Text);
                    return Json(LocationMasterList, JsonRequestBehavior.AllowGet);

                }



                return null;


            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }

        public ActionResult AnsweredQuestionJQGrid(string Status, string Campus, string Grade, string Subject, string Question, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                long[] QuestiondIdArray = null;
                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                CampusBasedStaffDetails campusstaffDet = new CampusBasedStaffDetails();
                if (Userobj == null || (Userobj != null && Userobj.UserId == null))
                { return RedirectToAction("LogOn", "Account"); }
                loggedInUserId = Userobj.UserId;
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likeSearchCriteria = new Dictionary<string, object>();

                
                    ProcessFlowServices pfs = new ProcessFlowServices(); // TODO: Initialize to an appropriate value

                    sord = sord == "desc" ? "Desc" : "Asc";
                    criteria.Add("UserId", loggedInUserId);
                    Dictionary<long, IList<CampusBasedStaffDetails>> staffDetailslist = QAS.GetCampusBasedStaffDetailsListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria, likeSearchCriteria);
                    criteria.Clear();
                    IList<CampusBasedStaffDetails> staffdetails = staffDetailslist.FirstOrDefault().Value;
                    var UserSubject = (from u in staffdetails select u.Subject).Distinct().ToArray();
                    var UserSection = (from u in staffdetails select u.Section).Distinct().ToArray();
                    var UserGrade = (from u in staffdetails select u.Grade).Distinct().ToArray();
                    criteria.Add("Subject", UserSubject);
                    criteria.Add("Grade", UserGrade);
                    criteria.Add("Section", UserSection);



                    //if (!string.IsNullOrWhiteSpace(Campus)) { criteria.Add("Campus", Campus); }
                    //if (!string.IsNullOrWhiteSpace(Grade)) { criteria.Add("Grade", Grade); }
                    //if (!string.IsNullOrWhiteSpace(Subject)) { criteria.Add("Subject", Subject); }
                    //if (!string.IsNullOrWhiteSpace(Question)) { likeSearchCriteria.Add("Question", Question); }

                    if (Status == "Inbox")
                    {
                        criteria.Clear();
                        if (!string.IsNullOrWhiteSpace(Campus)) { criteria.Add("Campus", Campus); }
                        if (!string.IsNullOrWhiteSpace(Grade)) { criteria.Add("Grade", Grade); }
                        if (!string.IsNullOrWhiteSpace(Subject)) { criteria.Add("Subject", Subject); }
                        if (!string.IsNullOrWhiteSpace(Question)) { likeSearchCriteria.Add("QuestionTitle", Question); }
                        criteria.Add("AssignedTo", loggedInUserId);
                        criteria.Add("IsAnswered", 'N');
                        criteria.Add("Status", "INBOX");

                    }
                    if (Status == "Campus")
                    {
                        criteria.Clear();
                        criteria.Add("Subject", UserSubject);
                        if (!string.IsNullOrWhiteSpace(Question)) { likeSearchCriteria.Add("QuestionTitle", Question); }
                        criteria.Add("Campus", staffDetailslist.FirstOrDefault().Value[0].Campus);
                        if (!string.IsNullOrWhiteSpace(Grade)) { criteria.Add("Grade", Grade); }
                        criteria.Add("IsAnswered", 'N');
                        criteria.Add("Replies", (long)0);
                        criteria.Add("Status", "CAMPUS");



                    }
                    if (Status == "AllCampus")
                    {
                        criteria.Clear();
                        criteria.Add("Subject", UserSubject);
                        if (!string.IsNullOrWhiteSpace(Campus)) { criteria.Add("Campus", Campus); }
                        if (!string.IsNullOrWhiteSpace(Question)) { likeSearchCriteria.Add("QuestionTitle", Question); }
                        if (!string.IsNullOrWhiteSpace(Grade)) { criteria.Add("Grade", Grade); }
                        criteria.Add("IsAnswered", 'N');
                        criteria.Add("Replies", (long)0);
                        criteria.Add("Status", "ALLCAMPUS");


                    }
                    if (Status == "Answered")
                    {
                        criteria.Clear();
                        if (!string.IsNullOrWhiteSpace(Campus)) { criteria.Add("Campus", Campus); }
                        if (!string.IsNullOrWhiteSpace(Grade)) { criteria.Add("Grade", Grade); }
                        if (!string.IsNullOrWhiteSpace(Subject)) { criteria.Add("Subject", Subject); }
                        if (!string.IsNullOrWhiteSpace(Question)) { likeSearchCriteria.Add("QuestionTitle", Question); }
                        if (!string.IsNullOrWhiteSpace(Status)) { criteria.Add("AnsweredBy", loggedInUserId); }
                        Dictionary<long, IList<QADetails>> QADetails = QAS.GetQADetailsListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria, likeSearchCriteria);
                        QuestiondIdArray = (from u in QADetails.FirstOrDefault().Value select u.QuestionId).Distinct().ToArray();
                        criteria.Clear();
                    }
                    if (Status == "Answered")
                    {
                        if (QuestiondIdArray.Length > 0)
                            criteria.Add("Id", QuestiondIdArray);
                        else return null;
                    }
                    if (Status == "CommonFAQ")
                    {
                        criteria.Clear();
                        criteria.Add("Subject", UserSubject);
                        if (!string.IsNullOrWhiteSpace(Question)) { likeSearchCriteria.Add("QuestionTitle", Question); }
                        if (!string.IsNullOrWhiteSpace(Campus)) { criteria.Add("Campus", Campus); }


                        Dictionary<long, IList<QAQuestions>> QAQuestionslistDashboard = QAS.GetQAQuestionListWithPagingAndCriteria(0, 99999, "ModifiedDate", "Desc", criteria, likeSearchCriteria);
                        //  IList<QAQuestions> commonfaq=QAQuestionslistDashboard.FirstOrDefault().Value;
                        var commonfaq = (from u in QAQuestionslistDashboard.FirstOrDefault().Value where u.Replies > 0 select u).ToList();
                        if (commonfaq != null && commonfaq.Count > 0)
                        {
                            long totalRecords = commonfaq.Count;
                            int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                            var jsonData = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalRecords,
                                rows = (
                                     from items in commonfaq
                                     select new
                                     {
                                         i = items.Id,
                                         cell = new string[] 
                                 { 
                                     items.Id.ToString(), 
                                     items.QuestionRefId,
                                     items.Question,
                                     items.QuestionTitle,
                                     items.Campus,
                                     items.Grade,
                                     items.Subject,
                                     "<br/>R"+ items.StudentName+"<br/>"+items.RaisedDate.ToString("dd/MM/yyyy")+"<br/>",
                                     "<br/>Replies :"+items.Replies+"<br/>Viewers :"+items.Views+"<br/>"
                                 }
                                     })
                            };
                            return Json(jsonData, JsonRequestBehavior.AllowGet);


                        }
                        else
                            return null;
                    }
                    else if (Status != "CommonFAQ")
                    {
                        
                        Dictionary<long, IList<QAQuestions>> QAQuestionslist = QAS.GetQAQuestionListWithPagingAndCriteria(0, 99999, "ModifiedDate", "Desc", criteria, likeSearchCriteria);
                        if (QAQuestionslist != null && QAQuestionslist.Count > 0)
                        {
                            long totalRecords = QAQuestionslist.First().Key;
                            int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                            var jsonData = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalRecords,
                                rows = (
                                     from items in QAQuestionslist.First().Value
                                     select new
                                     {
                                         i = items.Id,
                                         cell = new string[] 
                                 { 
                                     items.Id.ToString(), 
                                     items.QuestionRefId,
                                     items.Question,
                                     items.QuestionTitle,
                                     items.Campus,
                                     items.Grade,
                                     items.Subject,
                                     "<br/>R"+ items.StudentName+"<br/>"+items.RaisedDate.ToString("dd/MM/yyyy")+"<br/>",
                                     "<br/>Replies :"+items.Replies+"<br/>Viewers :"+items.Views+"<br/>"
                                 }
                                     })
                            };
                            return Json(jsonData, JsonRequestBehavior.AllowGet);
                        }
                        else
                            return null;
                    }
                    else
                        return null;
                   
              
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }
        }

        public ActionResult QAAnswersPopup(long QuestionId)
        {
           
           

            QAViewers viewers = new QAViewers();
            viewers.QuestionId = QuestionId;
            viewers.ViewedBy = Session["UserId"].ToString();
            viewers.ViewedDate = DateTime.Now;
            QAQuestions question = QAS.GetQAQuetionById(QuestionId);
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<string, object> likeSearchCriteria = new Dictionary<string, object>();
            criteria.Add("QuestionId", QuestionId);
            criteria.Add("ViewedBy", viewers.ViewedBy);
            Dictionary<long, IList<QAViewers>> QAViewersList = QAS.GetQAViewsListWithPagingAndCriteria(0, 9999, "Id", "desc", criteria, likeSearchCriteria);
            if (QAViewersList.First().Key == 0)
            {
                QAS.CreateorUpdateViewers(viewers);
                question.Views = question.Views == null ? 0 + 1 : question.Views + 1;

                QAS.CreateOrUpdateQuestion(question);
            }
            ViewBag.QuestionId = QuestionId;

            return View();



        }

        public ActionResult AnswerDivCreation(long QuestionId)
        {
            try
            {
                loggedInUserId = Session["UserId"].ToString();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likeSearchCriteria = new Dictionary<string, object>();
                string table = string.Empty;


                if (!string.IsNullOrEmpty(loggedInUserId))
                {
                    QAQuestions question = QAS.GetQAQuetionById(QuestionId);

                    // 

                    if (question != null)
                    {
                        criteria.Add("QuestionId", question.Id);
                        Dictionary<long, IList<QAAnswers>> Answerlist = QAS.GetQAAnswersListWithPagingAndCriteria(0, 999, "Id", "desc", criteria, likeSearchCriteria);
                        IList<QAAnswers> Answers = Answerlist.FirstOrDefault().Value.ToList();


                        table = table + "<div class='itemdiv dialogdiv'>";
                        table = table + "<div class='user'>";
                        //table = table + "<img alt='Alexa's Avatar' src='../assets/avatars/avatar1.png'/>";
                        //table = table + "<img  class='QAImages'  src='../../Images/QAStudent.png' />";
                        table = table + "<img alt='" + question.StudentName + "'s Avatar' src='ViewPhotos?UserId=" + question.RaisedBy + "&UserType=Student' />";
                        table = table + "</div>";
                        table = table + "<div class='body'>";
                        table = table + "<div class='time'>";
                        table = table + "<i class='ace-icon fa fa-clock-o'></i>";

                        TimeSpan span = (DateTime.Now - question.RaisedDate);
                        string time = "";
                        if (span.Days > 0)
                            time = String.Format("{0} days", span.Days);
                        else if (span.Hours > 0)
                            time = String.Format("{0} hours", span.Hours);
                        else if (span.Minutes > 0)
                            time = String.Format("{0} minutes", span.Minutes);
                        else
                            time = String.Format("{0} seconds", span.Seconds);
                        table = table + "<span class='green'>" + time + "</span>";
                        table = table + "</div>";
                        table = table + "<div class='name'>";
                        table = table + "<a href='#'>" + question.StudentName + "," + question.Grade + " ( " + question.Campus + ")</a>";
                        table = table + "</div>";
                        table = table + "<div class='text'><p class='alert alert-success'>" + question.Question + "</p></div>";
                        //table = table + "<div class='tools'>";
                        //table = table + "<a href='#' class='btn btn-minier btn-info'>";
                        //table = table + "<i class='icon-only ace-icon fa fa-share'></i>";
                        //table = table + "</a>";
                        //table = table + "</div>";
                        table = table + "</div>";
                        table = table + "</div>";

                        if (Answers != null && Answers.Count > 0)
                        {

                            foreach (var qadet in Answers)
                            {
                                //table = table + "<div class='divQuestion'>";
                                //table = table + "<table  width='100%'><tr><td style='width:10%'>";
                                //table = table + "<img  class='QAImages' src='../../Images/QATeacher.png' />";
                                //table = table + "</td><td    style='width:90%; font-size:14px;'> Replied  on " + qadet.AnsweredDate + "  by " + qadet.AnsweredBy + "  , " + qadet.Campus + "</td></tr>";
                                //table = table + "<tr><td colspan='2' style='color:#005C00;font-size:14px; font-family:Verdana;' >" + qadet.Answer + "</td></tr>";
                                //table = table + "</table>";
                                //table = table + "</div>";

                                table = table + "<div class='itemdiv dialogdiv'>";
                                table = table + "<div class='user'>";
                                //table = table + "<img  class='QAImages' src='../../Images/QATeacher.png' />";
                                table = table + "<img alt='" + qadet.AnsweredBy + "'s Avatar' src='ViewPhotos?UserId=" + qadet.AnsweredBy + "&UserType=Staff' />";
                                table = table + "</div>";
                                table = table + "<div class='body'>";
                                table = table + "<div class='time'>";
                                table = table + "<i class='ace-icon fa fa-clock-o'></i>";
                                TimeSpan span1 = (DateTime.Now - qadet.AnsweredDate);

                                string time1 = "";
                                if (span1.Days > 0)
                                    time1 = String.Format("{0} days", span1.Days);
                                else if (span1.Hours > 0)
                                    time1 = String.Format("{0} hours", span1.Hours);
                                else if (span1.Minutes > 0)
                                    time1 = String.Format("{0} minuts", span1.Minutes);
                                else
                                    time1 = String.Format("{0} sec", span1.Seconds);

                                table = table + "<span class='blue' >" + time1 + "</span>";
                                table = table + "</div>";
                                table = table + "<div class='name'>";
                                table = table + "<a href='#'>" + qadet.AnsweredBy + " ( " + qadet.Campus + ")</a>";
                                table = table + "</div>";

                                table = table + "<div class='text' id='AnsDiv_" + qadet.Id + "'><p class='alert alert-info'>" + qadet.Answer;
                                table = table + "<br /><span style='float:right'> <button onclick='CalculateLike(" + qadet.Id + ");' class='btn btn-sm btn-primary' style='height: 16px;width:47px;'><div style='margin-top: -4px;'><i class='fa fa-thumbs-o-up '></i><strong><span class='label-sm'>Like</span></strong> </div></button>&nbsp;&nbsp;&nbsp;<span id='lblLikes_" + qadet.Id+ "'> " + qadet.Likes + "</Span></span><br />";
                                //table = table + "<div class='tools action-buttons'>";
                                //table = table + "<button onclick='CalculateLike(" + qadet.Id + ");' class='btn btn-sm btn-primary' style='height: 1px;width:28px;'><div style='margin-top: -7px;'><i class='fa fa-thumbs-o-up '></i><strong><span class='label-sm'>Like</span></strong> </div></button>&nbsp;&nbsp;&nbsp;<span id='lblLikes_" + qadet.Id+ "'> " + qadet.Likes + "</Span>";
                                ////table = table + "<a href='#' class='blue'>";
                                ////table = table + "<i class='ace-icon fa fa-pencil bigger-125' id='" + qadet.Id + "' onclick='Edit(" + qadet.Id + ");' ></i>";
                                ////table = table + "</a>";
                                //table = table + "</div>";
                                
                                

                                    table=table+"</p></div>";
                                    
                                //table = table + "<div class='tools'>";
                                //table = table + "<a href='#' class='btn btn-minier btn-info'>";
                                //table = table + "<i class='icon-only ace-icon fa fa-share'></i>";
                                //table = table + "</a>";
                                //table = table + "</div>";
                                table = table + "</div>";
                                table = table + "</div>";
                            }

                        }
                        //table = table + "</td>";
                        //table = table + "</tr>";
                        //table = table + "</table>";
                        Response.Write(table);


                    }
                }
                else
                    return null;




                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;

            }
        }

        [ValidateInput(false)]
        public JsonResult PostAnswer(long QuestionId, string Answer)
        {
            try
            {
                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj != null && Userobj.UserId != null)
                {

                    loggedInUserId = Userobj.UserId;
                    //loggedInUserId = "SANGEETHA";
                    StaffDetailsHRM Staff = Hrms.GetStaffById(loggedInUserId);

                    if (Staff != null)
                    {

                        QAQuestions questions = QAS.GetQAQuetionById(QuestionId);
                        if (loggedInUserId == questions.AssignedTo && DateTime.Today < questions.ExpiryDate && questions.ExpiryStatus == "AVAILABLE" && questions.Status == "INBOX")
                        {
                            questions.IsAnswered = 'Y';
                            questions.ExpiryStatus = "CLOSED";


                        }

                        questions.Replies = questions.Replies + 1;
                        questions.ModifiedBy = loggedInUserId;
                        questions.ModifiedDate = DateTime.Now;
                        QAS.SaveorUpdateQAQuestions(questions);
                        QAAnswers answers = new QAAnswers();
                        answers.QuestionId = QuestionId;
                        answers.Answer = Answer;
                        answers.AnsweredBy = loggedInUserId;
                        answers.AnsweredDate = DateTime.Now;
                        answers.LastModifiedDate = DateTime.Now;
                        answers.StaffName = Staff.Name;
                        answers.Campus = Staff.Campus;
                        long Id = QAS.SaveorUpdateQAAnswers(answers);
                    }



                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }

        }

        public ActionResult ViewPhotos(string UserId, string UserType)
        {

            //if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOn", "Account");
            // else
            {
                try
                {
                    AdmissionManagementService admissionservice = new AdmissionManagementService();
                    Dictionary<long, IList<UploadedFiles>> UploadedFileslist = null;
                    IList<UploadedFiles> list = null;
                    UploadedFiles upfiles = null;

                    long PreRegNum = 0;
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (UserType == "Student")
                    {

                        StudentTemplate st = admissionservice.GetStudentDetailsByNewId(UserId);
                        if (st != null)
                        {
                            PreRegNum = st.PreRegNum;
                            criteria.Add("DocumentType", "Student Photo");
                            criteria.Add("PreRegNum", PreRegNum);
                        }

                    }
                    else if (UserType == "Staff")
                    {
                        StaffDetails staff = new StaffDetails();
                        staff = QAS.GetStaffDetailsByUserId(UserId);
                        if (staff != null)
                        {
                            PreRegNum = staff.PreRegNum;

                            criteria.Add("DocumentType", "Staff Photo");
                            criteria.Add("PreRegNum", PreRegNum);
                        }
                        //StaffDetails staff = QAS.GetStaffDetailsByUserId(UserId);
                    }

                    UploadedFileslist = QAS.GetUploadedFilesListWithPagingAndCriteria(0, 11, string.Empty, string.Empty, criteria);

                    if (UploadedFileslist.First().Key > 0)
                    {
                        list = UploadedFileslist.FirstOrDefault().Value;
                        upfiles = list.FirstOrDefault();

                        if (upfiles.DocumentData != null)
                        {
                            //criteria.Clear();
                            //criteria.Add("DocumentType", "QA");
                            //criteria.Add("DocumentName", "QA");
                            //criteria.Add("PreRegNum", );

                            UploadedFileslist = QAS.GetUploadedFilesListWithPagingAndCriteria(0, 11, string.Empty, string.Empty, criteria);
                            list = UploadedFileslist.FirstOrDefault().Value;
                            upfiles = list.FirstOrDefault();

                        }
                        else
                        {
                            criteria.Clear();
                            criteria.Add("DocumentType", "QA");
                            criteria.Add("DocumentName", "QA");
                            UploadedFileslist = QAS.GetUploadedFilesListWithPagingAndCriteria(0, 11, string.Empty, string.Empty, criteria);
                            list = UploadedFileslist.FirstOrDefault().Value;
                            upfiles = list.FirstOrDefault();

                        }
                    }
                    else
                    {
                        criteria.Clear();
                        criteria.Add("DocumentType", "QA");
                        criteria.Add("DocumentName", "QA");
                        UploadedFileslist = QAS.GetUploadedFilesListWithPagingAndCriteria(0, 11, string.Empty, string.Empty, criteria);
                        list = UploadedFileslist.FirstOrDefault().Value;
                        upfiles = list.FirstOrDefault();

                    }

                    using (MemoryStream ms = new MemoryStream())
                    using (System.Drawing.Image thumbnail = System.Drawing.Image.FromStream(new MemoryStream(upfiles.DocumentData)).GetThumbnailImage(50, 50, null, new IntPtr()))
                    {
                        thumbnail.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        upfiles.DocumentData = ms.ToArray();
                    }
                    return File(upfiles.DocumentData, "image/jpg");

                }
                catch (Exception ex)
                {
                    ExceptionPolicy.HandleException(ex, "ParentPortalIssuesPolicy");
                    throw ex;
                }
            }

        }

        public ActionResult QAQuestionEscalation()
        {
            try
            {

                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj == null || (Userobj != null && Userobj.UserId == null))
                { return RedirectToAction("LogOn", "Account"); }
                loggedInUserId = Userobj.UserId;
                QAQuestions questionsObj = new QAQuestions();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likeSearchCriteria = new Dictionary<string, object>();
                Dictionary<long, IList<QAQuestions>> Questionlist = QAS.GetQAQuestionListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria, likeSearchCriteria);
                IList<QAQuestions> QuestionIList = Questionlist.FirstOrDefault().Value;

                var InboxList = (from u in QuestionIList where u.Status == "INBOX" select u).ToList();
                var CampusList = (from u in QuestionIList where u.Status == "CAMPUS" select u).ToList();
                var AllCampusList = (from u in QuestionIList where u.Status == "ALLCAMPUS" select u).ToList();
                foreach (var item in InboxList)
                {
                    if (item.IsAnswered == 'N' && DateTime.Today > item.ExpiryDate && item.Replies == 0)
                    {
                        questionsObj = QAS.GetQAQuetionById(item.Id);
                        questionsObj.Status = "CAMPUS";
                        questionsObj.ExpiryStatus = "EXPIRED";

                        questionsObj.ExpiryDate = questionsObj.ExpiryDate.AddDays(Convert.ToInt64(ConfigurationManager.AppSettings["QAEscalationPeriod"]));
                        QAS.SaveorUpdateQAQuestions(questionsObj);
                        // questionsObj.ExpiryDate= questionsObj.ExpiryDate.

                    }
                }

                foreach (var item in CampusList)
                {
                    if (DateTime.Today > item.ExpiryDate && item.Replies == 0)
                    {
                        questionsObj = QAS.GetQAQuetionById(item.Id);
                        questionsObj.Status = "ALLCAMPUS";
                        questionsObj.ExpiryDate = questionsObj.ExpiryDate.AddDays(Convert.ToInt64(ConfigurationManager.AppSettings["QAEscalationPeriod"]));
                        QAS.SaveorUpdateQAQuestions(questionsObj);
                        // questionsObj.ExpiryDate= questionsObj.ExpiryDate.

                    }
                }

                criteria.Clear();

                criteria.Add("ProcessedDate", DateTime.Today);
                Dictionary<long, IList<QAProcessLog>> QADetails = QAS.GetQAProcessLogListWithPagingAndCriteria(0, 10, string.Empty, string.Empty, criteria, likeSearchCriteria);
                if (QADetails.First().Key == 0)
                {
                    QAProcessLog qaProcesslogObj = new QAProcessLog();
                    qaProcesslogObj.IsProcessed = "Y";
                    qaProcesslogObj.ProcessedBy = loggedInUserId;
                    qaProcesslogObj.ProcessedDate = DateTime.Now;
                    QAS.SaveOrUpdateQAProcessLog(qaProcesslogObj);
                }




                return null;

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }

        }
        public ActionResult GoogleChart()
        {

            return View();
        }

        public ActionResult InboxChart()
        {
            try
            {
                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj == null || (Userobj != null && Userobj.UserId == null))
                { return RedirectToAction("LogOn", "Account"); }
                loggedInUserId = Userobj.UserId;
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likeSearchCriteria = new Dictionary<string, object>();
                //criteria.Add("AssignedTo", loggedInUserId);
                //criteria.Add("IsAnswered", 'N');
                //criteria.Add("Status", "INBOX");

                IList<QADashboard> QAReport = new List<QADashboard>();


                QAReport = QAS.GetGetQAReportbyFlag("INBOX", loggedInUserId);
                var InboxGraph = " <graph caption='Grade Based Inbox Questions' xAxisName='Period Year' forceDecimals='0'  decimalPrecision='0'  rotateLabels='1' formatNumberScale='0'   yAxisName='' animation='1' showNames='1' showValues='1'   distance='6'   rotateNames='0'>";
                string[] color = new string[12] { "#993300", "#003366", "#93A8A9", "#D1E6E7", "FFA62F", "E66C2C", "C25A7C", "59E817", "00FF00", "FFA62F", "FF7F50", "FF00FF" };

                int i = 0;
                foreach (var item in QAReport)
                {

                    // PeriodYearGraph = PeriodYearGraph + " <set name='2013-2014' value='" + Period13to14 + "' color='8B008B' link=\"JavaScript: MCClickEvent('"+a+"');"+"\" />";
                    InboxGraph = InboxGraph + " <set name='" + item.Grade + "' value='" + item.NoOfQuestions + "' color='" + color[i] + "' />";
                    i = i + 1;
                }
                InboxGraph = InboxGraph + "</graph>";
                Response.Write(InboxGraph);

                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }

        }
        //QADashboard All campus chart
        public ActionResult QAAllCampusChart()
        {
            try
            {
                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj == null || (Userobj != null && Userobj.UserId == null))
                { return RedirectToAction("LogOn", "Account"); }
                loggedInUserId = Userobj.UserId;
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likeSearchCriteria = new Dictionary<string, object>();
                //criteria.Add("AssignedTo", loggedInUserId);
                //criteria.Add("IsAnswered", 'N');
                //criteria.Add("Status", "INBOX");

                IList<QADashboard> QAReport = new List<QADashboard>();


                QAReport = QAS.GetGetQAReportbyFlag("ALLCAMPUS", loggedInUserId);
                var AllCampusChart = " <graph caption='UnAnswered Questions-All Campus' xAxisName='Period Year' forceDecimals='0' decimalPrecision='0'   rotateLabels='1' formatNumberScale='0'   yAxisName='' animation='1' showNames='1' showValues='1'  divlinecolor='d3d3d3' distance='6'   rotateNames='0'>";
                string[] color = new string[12] { "#6fb3e0", "2091CF", "AF4E96", "DA5430", "FEE074", "E66C2C", "C25A7C", "59E817", "00FF00", "FFA62F", "FF7F50", "FF00FF" };

                int i = 0;
                foreach (var item in QAReport)
                {

                    // PeriodYearGraph = PeriodYearGraph + " <set name='2013-2014' value='" + Period13to14 + "' color='8B008B' link=\"JavaScript: MCClickEvent('"+a+"');"+"\" />";
                    AllCampusChart = AllCampusChart + " <set name='" + item.Campus + "' value='" + item.NoOfQuestions + "' color='" + color[i] + "' />";
                    i = i + 1;
                }
                AllCampusChart = AllCampusChart + "</graph>";
                Response.Write(AllCampusChart);

                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }

        }
        //QAQuestion count

        public ActionResult QAQuestionCount()
        {
            try
            {
                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj == null || (Userobj != null && Userobj.UserId == null))
                { return RedirectToAction("LogOn", "Account"); }
                loggedInUserId = Userobj.UserId;
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likeSearchCriteria = new Dictionary<string, object>();
                //criteria.Add("AssignedTo", loggedInUserId);
                //criteria.Add("IsAnswered", 'N');
                //criteria.Add("Status", "INBOX");

                // IList<QADashboard> QAReport = new List<QADashboard>();


                IList<QADashboard> QAReport = QAS.GetGetQAReportbyFlag("QUESTIONCOUNT", loggedInUserId);
                string CountString = QAReport[0].InboxCount.ToString() + "," + QAReport[0].CampusCount.ToString() + "," + QAReport[0].AllCampusCount.ToString();

                long[] longArr = new long[] { QAReport[0].InboxCount, QAReport[0].CampusCount, QAReport[0].AllCampusCount };
                //long InboxCount = QAReport[0].InboxCount;
                //long CampusCount = QAReport[0].CampusCount;
                //long AllCampusCount = QAReport[0].AllCampusCount;

                return Json(longArr, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }

        }
        //Success Ratio chart based on the Answered Question

        public ActionResult QASuccessRatio()
        {
            try
            {
                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj == null || (Userobj != null && Userobj.UserId == null))
                { return RedirectToAction("LogOn", "Account"); }
                loggedInUserId = Userobj.UserId;
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likeSearchCriteria = new Dictionary<string, object>();
                //criteria.Add("AssignedTo", loggedInUserId);
                //criteria.Add("IsAnswered", 'N');
                //criteria.Add("Status", "INBOX");

                IList<QADashboard> QAReport = new List<QADashboard>();
                string AllCampusChart = "";

                QAReport = QAS.GetGetQAReportbyFlag("SUCCESSRATIO", loggedInUserId);
                if (QAReport.Count > 0)
                {
                     AllCampusChart = " <graph caption='Answered and UnAnswered Questions  ' xAxisName='Period Year' forceDecimals='0' decimalPrecision='0'   rotateLabels='1' formatNumberScale='0'   yAxisName='' animation='1' showNames='1' showValues='1'  divlinecolor='d3d3d3' distance='6'   rotateNames='0'>";
                    string[] color = new string[12] { "#2091CF", "AF4E96", "DA5430", "FEE074", "68BC31", "E66C2C", "C25A7C", "59E817", "00FF00", "FFA62F", "FF7F50", "FF00FF" };

                    int i = 0;
                    AllCampusChart = AllCampusChart + " <set name='Answered' value='" + QAReport[0].Success + "' color='#8181F7' />";
                    AllCampusChart = AllCampusChart + " <set name='UnAnswered' value='" + QAReport[0].Failiure + "' color='#81F7F3' />";
                    //foreach (var item in QAReport)
                    //{

                    //    // PeriodYearGraph = PeriodYearGraph + " <set name='2013-2014' value='" + Period13to14 + "' color='8B008B' link=\"JavaScript: MCClickEvent('"+a+"');"+"\" />";
                    //    AllCampusChart = AllCampusChart + " <set name='" + item.Campus + "' value='" + item.NoOfQuestions + "' color='" + color[i] + "' />";
                    //    i = i + 1;
                    //}
                    AllCampusChart = AllCampusChart + "</graph>";
                    Response.Write(AllCampusChart);
                }

                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StaffIssuesPolicy");
                throw ex;
            }

        }


        public ActionResult DOJOSample() {
            return View();
        }

        public long LikeCount(long AnswerId)
        {
            try
            {

                QALikes likes = new QALikes();
                likes.LikedBy = Session["UserId"].ToString();
                likes.LikeDate = DateTime.Now;
                likes.AnswerId = AnswerId;
                QAAnswers answer = QAS.GetQAAnswerById(AnswerId);
                

                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likeSearchCriteria = new Dictionary<string, object>();
                criteria.Add("AnswerId", AnswerId);
                criteria.Add("LikedBy", likes.LikedBy);
                Dictionary<long, IList<QALikes>> QALikeslist = QAS.GetQALikesListWithPagingAndCriteria(0, 9999, "Id", "desc", criteria, likeSearchCriteria);
                if (QALikeslist.First().Key == 0)
                {

                    QAS.CreateOrUpdateLikes(likes);
                    answer.Likes = answer.Likes + 1;
                    QAS.SaveorUpdateQAAnswers(answer);


                }





                return answer.Likes;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StudentPortalPolicy");
                throw ex;
            }


        }
    }
}
