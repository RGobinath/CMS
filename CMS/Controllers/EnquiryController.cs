using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Net.Mail;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using TIPS.Entities;
using CMS.Controllers;
using TIPS.Entities.AdmissionEntities;
using TIPS.Service;
using TIPS.Entities.EnquiryEntities;
using TIPS.ServiceContract;
using CMS.Helpers;
using TIPS.Entities.Assess;

namespace CMS.Controllers
{
    public class EnquiryController : BaseController
    {
        string policyName = "EnquiryPolicy";
        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
        EnquiryService EnqSvc = new EnquiryService();
        public ActionResult Enquiry()
        {
            User Userobj = (User)Session["objUser"];
            if (Userobj == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            return View();
        }
        public ActionResult GetFollowUpDetailsList(int page, int rows, string sidx, string sord, long? EnquiryDetailsId)
        {
            try
            {
                if (EnquiryDetailsId > 0)
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("EnquiryDetailsId", EnquiryDetailsId);
                    Dictionary<long, IList<FollowUpDetails>> dcnFollowUpDetails = EnqSvc.GetFollowUpDetailsWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    if (dcnFollowUpDetails == null || dcnFollowUpDetails.Count == 0 || dcnFollowUpDetails.FirstOrDefault().Key == 0)
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        IList<FollowUpDetails> lstFollowUpDetails = dcnFollowUpDetails.FirstOrDefault().Value;
                        long totalRecords = dcnFollowUpDetails.FirstOrDefault().Key;
                        int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                        var jsonData = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalRecords,
                            rows =
                            (
                                from items in lstFollowUpDetails
                                select new
                                {
                                    i = items.FollowUpDetailsId,
                                    cell = new string[] 
                                        { 
                                            items.FollowUpDetailsId.ToString(),
                                            items.EnquiryDetailsId.ToString(),
                                            items.FollowUpRemarks,
                                            items.StaffFollowUpDate!=null?items.StaffFollowUpDate.Value.ToString("dd/MM/yyyy"):"",
                                            items.NextFollowUpDate!=null?items.NextFollowUpDate.Value.ToString("dd/MM/yyyy"):"",                                    
                                            items.StaffName
                                        }
                                }
                            )
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                } return null;
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, policyName); }
            finally
            { }
        }

        public void SaveButtonPress(StudentTemplate st, AdmissionManagementService ams1)
        {
            string RecipientInfo = "", Subject = "", Body = "", MailBody = ""; bool retValue;
            EmailHelper em = new EmailHelper();
            MailBody = GetBodyofMail();
            Session["tabfreeze"] = "no";
            st.FamilyDetailsList = null;
            st.PastSchoolDetailsList = null;
            st.ApproveAssignList = null;
            st.PaymentDetailsList = null;
            st.DocumentDetailsList = null;
            ViewBag.admissionstatus = "New Registration";
            Dictionary<string, object> criteria4 = new Dictionary<string, object>();
            criteria4.Add("StudentId", Convert.ToInt64(Session["preregid"]));
            Dictionary<long, IList<AddressDetails>> add = ams1.GetAddressDetailsListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria4);
            if (add.FirstOrDefault().Value.Count != 0)
            {
                if (add.FirstOrDefault().Value.Count == 1) { st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[0].Id = add.FirstOrDefault().Value[0].Id; }
                else if (add.FirstOrDefault().Value.Count == 2)  // (add.Count == 2)
                {
                    st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[0].Id = add.FirstOrDefault().Value[0].Id;
                    st.AddressDetailsList[1].AddressType = "Secondary Address"; st.AddressDetailsList[1].Id = add.FirstOrDefault().Value[1].Id;
                }
                else { }
            }
            else
            {
                if (add.FirstOrDefault().Value.Count == 1) { st.AddressDetailsList[0].AddressType = "Primary Address"; }
                else if (add.FirstOrDefault().Value.Count == 2) { st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[1].AddressType = "Secondary Address"; }
                else { }
            }
            st.AdmissionStatus = "New Registration";
            st.Status = "1";
            st.Status1 = "2";         // key for newregistration,sentforapproval,onhold,callforinterview
            ViewBag.Date = Session["date"];  // date;
            ViewBag.time = Session["time"]; // time;

            if (st.Id == 0)    // to send email only once
            {
                IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(st.Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                if (ConfigurationManager.AppSettings["SendEmail1"].ToString() == "true")
                {
                    RecipientInfo = "Dear Parent,";
                    Subject = "Welcome to TIPS - Student Registration In Progress"; // st.Subject;
                    Body = "Thanks for registering with TIPS school and your application is sent for processing. Your application number is " + st.ApplicationNo + " and please refer this number for all further communication. For any queries, mail us at " + campusemaildet.First().EmailId.ToString() + ".";
                    retValue = em.SendStudentRegistrationMail(st, st.EmailId, st.Campus, Subject, Body, MailBody, RecipientInfo, "Parent",null);
                    //Send Mail to Managment
                    RecipientInfo = "Dear Team,";
                    Subject = "A new application is available for processing"; // st.Subject;
                    string Body1 = "A new application is available for processing and you are requested to process the same.<br><br>Campus: " + st.Campus + "<br>";
                    Body1 += "Application number: " + st.ApplicationNo + "<br>Pre-Reg number: " + st.PreRegNum + "<br>Applicant name: " + st.Name + "<br>Application grade: " + st.Grade + "<br>Applied Academic year: " + st.AcademicYear + "<br>";
                    retValue = em.SendStudentRegistrationMail(st, null, st.Campus, Subject, Body1, MailBody, RecipientInfo, "Management",null);
                }
                //commented by micheal
                //string recepientnos = "";
                // commented by me
                //if ((st.Phone != "") && (!string.IsNullOrEmpty(st.Phone)) && (st.Phone != null))
                //{
                //    if (Regex.IsMatch(st.Phone, @"(?<!\d)\d{10}(?!\d)", RegexOptions.IgnoreCase))
                //    { recepientnos = st.Phone; }
                //    else { recepientnos = ""; }
                //}
                //else { recepientnos = ""; }

                //string smsurl = "";
                //string dataString;
                //WebRequest request;
                //WebResponse response;
                //Stream s;
                //StreamReader readStream;
                //string message = "";
                //message = "Thanks for your admission with TIPS for " + st.Grade + "Grade and we will be sending you the joining details soon. ";
                //smsurl = ConfigurationManager.AppSettings["SMSService"].ToString() + "&senderID=THEJOM&receipientno=" + recepientnos + "&dcs=0&msgtxt=" + message + "&state=1";
                //try
                //{
                //    request = WebRequest.Create(smsurl);
                //    response = request.GetResponse();
                //    s = response.GetResponseStream();
                //    readStream = new StreamReader(s);
                //    dataString = readStream.ReadToEnd();
                //    response.Close();
                //    s.Close();
                //    readStream.Close();

                //}
                //catch (Exception)
                //{

                //}
                ams1.CreateOrUpdateStudentDetails(st);
            }
        }
        public ActionResult NewEnquiry(long? StudentId)
        {
            User Userobj = (User)Session["objUser"];
            if (Userobj == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                MastersService ms = new MastersService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                //Dictionary<long, IList<BloodGroupMaster>> BloodGroupMaster = ms.GetBloodGroupMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                //criteria.Clear();
                var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                if (usrcmp != null && usrcmp.FirstOrDefault() != null)
                {
                    criteria.Add("Name", usrcmp);// to check if the usrcmp obj is null or with data
                }
                Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                criteria.Clear();
                Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                //Dictionary<long, IList<RelationshipMaster>> RelationshipMaster = ms.GetRelationshipMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                criteria.Clear();
                criteria.Add("DocumentFor", "Student");
                Dictionary<long, IList<DocumentTypeMaster>> DocumentTypeMaster = ms.GetDocumentTypeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                criteria.Clear();
                ViewBag.campusddl = CampusMaster.FirstOrDefault().Value;
                ViewBag.gradeddl = GradeMaster.First().Value;
                //ViewBag.familyddl = RelationshipMaster.FirstOrDefault().Value;
                ViewBag.documentddl = DocumentTypeMaster.FirstOrDefault().Value;
                //ViewBag.bloodgrpddl = BloodGroupMaster.FirstOrDefault().Value;
                ViewBag.Studentmgmt = "no";
                EnquiryDetails EnquiryDetails = new EnquiryDetails();
                if (StudentId != null)
                {
                    EnquiryDetails = EnqSvc.GetEnquiryDetailsById(Convert.ToInt64(StudentId));
                    ViewBag.Followup = "yes";
                    ViewBag.enquiryid = EnquiryDetails.EnquiryDetailsId;
                    if (EnquiryDetails.EnquiryStatus == "Completed") { ViewBag.completed = "hide"; }
                    return View(EnquiryDetails);
                }
                else
                {
                    ViewBag.hidevalue = "hide";
                    return View(EnquiryDetails);
                }
            }
        }
        [HttpPost]
        public ActionResult NewEnquiry(EnquiryDetails ed)
        {
            try
            {
                string RecipientInfo = "", Subject = "", Body = "", MailBody = ""; bool retValue;
                EmailHelper em = new EmailHelper();
                MailBody = GetBodyofMail();
                string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                if (!string.IsNullOrEmpty(Request["DOB"]))
                {
                    ed.DOB = DateTime.Parse(Request["DOB"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                }
                if (!string.IsNullOrEmpty(Request["EnquiredDate"]))
                {
                    ed.EnquiredDate = DateTime.Parse(Request["EnquiredDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                }
                if (!string.IsNullOrEmpty(Request["FollowupDate"]))
                {
                    ed.FollowupDate = DateTime.Parse(Request["FollowupDate"], provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                }
                if (ed.Subjects != null)
                {
                    string singleitem = "";
                    foreach (var item in ed.Subjects)
                    {
                        singleitem += item + ", ";
                    }
                    ed.Subjects = singleitem;
                }
                if (ed.EnquiryDetailsId == 0)
                {
                    ed.CreatedBy = Session["UserId"].ToString();
                    ed.CreatedDate = DateTime.Now;
                }
                else
                {
                    ed.ModifiedBy = Session["UserId"].ToString();
                    ed.ModifiedDate = DateTime.Now;
                }

                /// Enquir Status is completed then data will store into the StudentTemplate
                if (ed.EnquiryStatus == "Completed")
                {
                    AdmissionManagementService ams = new AdmissionManagementService();
                    ////commented by micheal
                    //var Ids = ed.CourseEntryId.Split(',');
                    //long[] EnqCourseId = new long[Ids.Length];
                    //int j = 0;
                    ////int cnt = 0;
                    //foreach (string val in Ids)
                    //{
                    //    EnqCourseId[j] = Convert.ToInt64(val);
                    //    j++;
                    //}
                    Session["preregno"] = 0;
                    Session["preregid"] = 0;
                    Dictionary<long, IList<PreRegDetails>> prd = ams.GetPreRegDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, null);
                    var id1 = prd.First().Value[0].PreRegNum + 1;
                    PreRegDetails srn = new PreRegDetails();
                    srn.PreRegNum = id1;
                    srn.Id = prd.First().Value[0].Id;
                    srn.Date = DateTime.Now;
                    ams.CreateOrUpdatePreRegDetails(srn);
                    Session["preregno"] = id1;
                    StudentTemplate st = new StudentTemplate();
                    st.PreRegNum = id1;
                    st.ApplicationNo = ed.EnquiryDetailsCode;
                    st.Name = ed.StudentName;
                    st.Gender = ed.Gender;
                    st.Grade = ed.Grade;
                    st.EmailId = ed.EnquirerEmailId;
                    st.LanguagesKnown = ed.LanguagesKnown;
                    st.Campus = ed.Campus;
                    st.CreatedDate = ed.CreatedDate.ToString();
                    st.DOB = ed.DOB.ToString();
                    st.EmailId = ed.EnquirerEmailId;
                    SaveButtonPress(st, ams);
                    TempData["NewRegistrationMsg"] = ed.EnquiryDetailsCode;
                    return RedirectToAction("EnquiryManagement");
                }
                Int64 enqid = ed.EnquiryDetailsId;
                string EnquiryDetailsId = EnqSvc.SaveOrUpdateEnquiryDetails(ed);
                if (enqid != 0)
                {
                    return RedirectToAction("NewEnquiry", new { StudentId = enqid });
                }
                else    // to send email only once
                {
                    TempData["NewEnquiryMsg"] = ed.EnquiryDetailsId;
                    IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(ed.Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                    Subject = "Welcome to TIPS - Enquiry In Progress"; // mail Subject;
                    RecipientInfo = "Dear Parent,";
                    Body = "Thank you for enquiring us. You have raised an enquiry for board " + ed.Board + " in " + ed.Campus + ".Your Enquiry No is " + EnquiryDetailsId + ", please refer this number for all further communication. For any queries, mail us at " + campusemaildet.First().EmailId.ToString() + ".";
                    retValue = em.SendStudentEnquiryMail(null,ed.EnquirerEmailId, ed.Campus, Subject, Body, MailBody, RecipientInfo, "Parent");
                    //campus admin email logic can be setup here
                    /// if create an enquiry for other campus then send a mail option...
                    /// get user campus value form the user table...
                    //UserService us = new UserService();
                    //User User = us.GetUserByUserId(Session["UserId"].ToString());
                    //if (ed.Campus != User.Campus & !string.IsNullOrEmpty(User.Campus))
                    //{
                    //    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    //    criteria.Add("Campus", ed.Campus);
                    //    criteria.Add("AppRole", "ENQ");
                    //    Dictionary<long, IList<UserAppRole>> UserApprolelist = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    //    var UserEmailId = from usr in UserApprolelist.FirstOrDefault().Value
                    //                      where usr.RoleCode == "ENQ"
                    //                      select usr.Email;

                    //    string[] usremailarray = UserEmailId.Distinct().ToArray();
                    //    if (UserEmailId != null)
                    //    {
                    //        // send a mail option...
                    //        Sendmailforothercampusenquiry(User.Campus, ed.Campus, ed.Board, usremailarray);
                    //    }
                    //}
                    if (ed.SendMessage == true)
                    {
                        string recepientnos = "";
                        if ((ed.EnquirerMobileNo != "") && (!string.IsNullOrEmpty(ed.EnquirerMobileNo)) && (ed.EnquirerMobileNo != null))
                        {
                            if (Regex.IsMatch(ed.EnquirerMobileNo, @"(?<!\d)\d{10}(?!\d)", RegexOptions.IgnoreCase))
                            { recepientnos = ed.EnquirerMobileNo; }
                            else { recepientnos = ""; }
                        }
                        else { recepientnos = ""; }
                        string smsurl = "";
                        string dataString;
                        WebRequest request; WebResponse response; Stream s; StreamReader readStream;
                        string Message = "Dear Parent, thank you for enquiring with us.You have raised an enquiry about " + ed.Board +" in "+ed.Campus+",  For any queries contact through mail .TIPS Team.";
                        smsurl = ConfigurationManager.AppSettings["SMSService"].ToString() + "&senderID=TIPSGB&receipientno=" + recepientnos + "&dcs=0&msgtxt=" + Message + "&state=1";
                        if (!string.IsNullOrEmpty(recepientnos))
                        {
                            try
                            {
                                request = WebRequest.Create(smsurl);
                                response = request.GetResponse();
                                s = response.GetResponseStream();
                                readStream = new StreamReader(s);
                                dataString = readStream.ReadToEnd();
                                response.Close();
                                s.Close();
                                readStream.Close();
                            }
                            catch (Exception ex)
                            {
                                ExceptionPolicy.HandleException(ex, policyName);
                                throw ex;
                            }
                        }
                    }
                }
                return RedirectToAction("EnquiryManagement");
                //  return RedirectToAction("NewEnquiry", new { StudentId = Convert.ToInt64(EnquiryDetailsId) });
            }
            catch (Exception ex)
            {
                return ThrowJSONErrorNew(ex, policyName);
            }
            finally
            { }
        }
        public void sendEmail(MailMessage mail, SmtpClient smtp)
        {
            smtp.Send(mail);
        }

        public ActionResult AddFollowupDetails(string EnquiryDetailsId, string FollowUpRemarks, string StaffName, string StaffFollowUpDate, string NextFollowUpDate)
        {
            try
            {
                FollowUpDetails fud = new FollowUpDetails();

                fud.EnquiryDetailsId = Convert.ToInt64(EnquiryDetailsId);
                fud.FollowUpRemarks = FollowUpRemarks;
                fud.StaffName = StaffName;
                if (StaffFollowUpDate != null)
                {
                    fud.StaffFollowUpDate = DateTime.Parse(StaffFollowUpDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                }
                if (NextFollowUpDate != null)
                {
                    fud.NextFollowUpDate = DateTime.Parse(NextFollowUpDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                }
                EnqSvc.SaveOrUpdateFollowUpDetails(fud);
                long followupcount = EnqSvc.FollowUpIdCount(fud.EnquiryDetailsId);
                if (followupcount > 4)
                {
                    AdmissionManagementService tas = new AdmissionManagementService();
                    EnquiryDetails EnqDetails = EnqSvc.GetEnquiryDetailsById(fud.EnquiryDetailsId);
                    EnqDetails.EnquiryStatus = "Pending";
                    EnqSvc.SaveOrUpdateEnquiryDetails(EnqDetails);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "ThejomayaPolicy");
                throw ex;
            }
        }

        public ActionResult EnquiryManagement(string pagename)
        {
            if (pagename != null)
            {
                Session["pagename"] = pagename.ToString();
            }
            MastersService ms = new MastersService();
            Dictionary<string, Object> criteria = new Dictionary<string, object>();
            var usrcmp = Session["UserCampus"] as IEnumerable<string>;
            if (usrcmp != null && usrcmp.Count() > 0)
            {
                if (usrcmp.FirstOrDefault().Distinct() != null)            // to check if the usrcmp obj is null or with data
                {
                    criteria.Add("Name", usrcmp);
                }
            }
            Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.campusddl = CampusMaster.FirstOrDefault().Value;
            return View(new EnquiryDetails());
        }
        public ActionResult GetEnquiryDetailsList(long? EnquiryDetailsId, string StudentName, string ParentName, string ContactMobile, string ddlcampus, string RefNo, string EnquiryStatus, string FollowupDate, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                User Userobj = (User)Session["objUser"];
                if (Userobj == null)
                {
                    return RedirectToAction("LogOff", "Account");
                }
                ReportCardService rptCardSrvc = new ReportCardService();
                #region CriteriaBuilding
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (EnquiryDetailsId != null && EnquiryDetailsId > 0)
                {
                    criteria.Add("EnquiryDetailsId", EnquiryDetailsId);
                }
                if (!string.IsNullOrEmpty(StudentName))
                {
                    criteria.Add("StudentName", StudentName);
                }
                if (!string.IsNullOrEmpty(ParentName))
                {
                    criteria.Add("ParentName", ParentName);
                }
                if (!string.IsNullOrEmpty(ContactMobile))
                {
                    criteria.Add("EnquirerMobileNo", ContactMobile);
                }
                if (!string.IsNullOrEmpty(ddlcampus)) { criteria.Add("Campus", ddlcampus); }
                else
                {
                    var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                    criteria.Add("Campus", usrcmp.ToArray());
                }
                if (!string.IsNullOrEmpty(RefNo))
                {
                    criteria.Add("EnquiryDetailsCode", RefNo);
                }
                if (!string.IsNullOrEmpty(EnquiryStatus))
                {
                    criteria.Add("EnquiryStatus", EnquiryStatus);
                }
                if (!string.IsNullOrEmpty(FollowupDate))
                {
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    DateTime FromDate = DateTime.Parse(FollowupDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    criteria.Add("FollowupDate", FromDate);
                }
                #endregion

                Dictionary<long, IList<EnquiryDetailGridView>> dcnEnquiryDetails = null;

                if (!string.IsNullOrEmpty(RefNo) || !string.IsNullOrEmpty(ParentName) || !string.IsNullOrEmpty(StudentName))
                {
                    dcnEnquiryDetails = EnqSvc.GetEnquiryDetailsGridViewListWithPagingAndCriteriaEQSearch(page - 1, rows, sord, sidx, criteria);
                }
                else
                {
                    dcnEnquiryDetails = EnqSvc.GetEnquiryDetailsGridViewListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                }

                if (dcnEnquiryDetails == null || dcnEnquiryDetails.Count == 0 || dcnEnquiryDetails.FirstOrDefault().Key == 0)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                else if (ExportType == "excel")
                {
                    string headerTable = @"<Table><tr><td><b>Month:</b></td><td> </td><td colspan='8' align='center' style='font-size: large;'>The Indian Public School</td></tr><tr><td colspan='8'></td></tr><tr><td colspan='4'><b>Grade</b> : </td><td colspan='3'> <b>Assess360 Point Chart</b><td><b>Class Teacher</b> :</td></tr>";
                    headerTable = headerTable + "</b></Table>";
                    string[] TblHeaders = new string[] { "Id", "Student Name", "Character", "Attendence/Punctuality", "Homework Completion", "Homework Accuracy(15)", "Weekly Chapter Tests(20)", "SLC Parent Assessment(5)", "Term Assessment(25)", "Total" };

                    if (dcnEnquiryDetails != null && dcnEnquiryDetails.FirstOrDefault().Value != null && dcnEnquiryDetails.FirstOrDefault().Value.Count > 0)
                    {
                        var EnqList = dcnEnquiryDetails.FirstOrDefault().Value.ToList();
                        base.ExptToXL(EnqList, "Enquiry_List", (items => new
                        {
                            Id = items.EnquiryDetailsId,
                            Student_Name = items.StudentName,
                            Course_Program = items.CourseProgram,
                            DateOfEnquiry = items.EnquiredDate != null ? items.EnquiredDate.Value.ToString("dd/MM/yyyy") : items.EnquiredDate.ToString(),
                            FollowUpDate = items.FollowupDate != null ? items.FollowupDate.Value.ToString("dd/MM/yyyy") : items.FollowupDate.ToString(),
                            Status = items.EnquiryStatus
                        }));
                    }
                    return new EmptyResult();
                }
                else
                {
                    IList<EnquiryDetailGridView> lstEnquiryDetails = dcnEnquiryDetails.FirstOrDefault().Value;
                    long totalRecords = dcnEnquiryDetails.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows =
                        (
                        from items in lstEnquiryDetails
                        select new
                        {
                            i = items.EnquiryDetailsId,
                            cell = new string[]
                           {
                                 items.EnquiryDetailsId.ToString(),
                                 String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '/Enquiry/NewEnquiry?studentid="+items.EnquiryDetailsId+"' >{0}</a>",items.EnquiryDetailsCode),
                                 items.ParentName,
                                 items.StudentName,
                                 items.Campus,
                                 items.CourseProgram,
                                 items.EnquirerMobileNo,
                                 items.EnquirerEmailId,
                                 items.EnquiryStatus,
                                 items.FollowupDate!=null?items.FollowupDate.Value.ToString("dd/MM/yyyy"):"",
                                 items.EnquiredDate!=null?items.EnquiredDate.Value.ToString("dd/MM/yyyy"):"",
                                 items.CreatedBy,
                                 items.ModifiedBy,
                           }
                        }
                        )
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, policyName); }
            finally
            { }
        }
        //commented by me
        //public JsonResult FillNewEnquirySubjects() 
        //{
        //    MastersService ms = new MastersService();
        //    Dictionary<string, object> criteria = new Dictionary<string, object>();
        //    Dictionary<long, IList<EnquirySubjectMaster>> EnquirySubMaster = ms.GetEnquirySubMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
        //    var Subjects = (
        //    from items in EnquirySubMaster.FirstOrDefault().Value
        //    select new
        //    {
        //        Text = items.SubjectName,
        //        Value = items.SubjectName
        //    }).Distinct().ToList();
        //    return Json(Subjects, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult SaveCoursedetails(long EnqId, string campus, string board, string grade, string subject)
        {
            try
            {
                if (EnqId > 0)
                {
                    User Userobj = (User)Session["objUser"];
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (EnqId > 0) { criteria.Add("EnquiryId", EnqId); }
                    if (!string.IsNullOrEmpty(campus)) { criteria.Add("Campus", campus); }
                    if (!string.IsNullOrEmpty(board)) { criteria.Add("Board", board); }
                    if (!string.IsNullOrEmpty(grade)) { criteria.Add("Grade", grade); }
                    if (!string.IsNullOrEmpty(subject)) { criteria.Add("Subjects", subject); }
                    Dictionary<long, IList<EnquiryCourse>> EnqCourseList = null;
                    EnqCourseList = EnqSvc.GetEnquiryCourseDetailsListWithPagingAndCriteria(0, 10, string.Empty, string.Empty, criteria);
                    if (EnqCourseList != null && EnqCourseList.Count > 0 && EnqCourseList.FirstOrDefault().Value.Count > 0)
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        EnquiryCourse EnCrs = new EnquiryCourse();
                        //long count=EnqSvc.GetCourseCount(EnqId);
                        criteria.Clear();
                        criteria.Add("EnquiryId", EnqId);
                        EnqCourseList = EnqSvc.GetEnquiryCourseDetailsListWithPagingAndCriteria(0, 10, string.Empty, string.Empty, criteria);
                        EnCrs.CourseCode = EnqId + "-Course-" + EnqCourseList.FirstOrDefault().Key;
                        EnCrs.Campus = campus;
                        EnCrs.Grade = grade;
                        EnCrs.Board = board;
                        EnCrs.Subjects = subject;
                        EnCrs.EnquiryId = Convert.ToInt64(EnqId);
                        EnqSvc.CreateOrUpdateEnquiryCourse(EnCrs, Userobj.UserId);
                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }
                }
                return Json("ValueIsNull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
        }

        public void Sendmailforothercampusenquiry(string CurrentCampus, string RecievedCampus, string Board, string[] EmailId)
        {
            string RecipientInfo = "", Subject = "", Body = "", MailBody = ""; bool retValue;
            EmailHelper em = new EmailHelper();
            MailBody = GetBodyofMail();
            IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(CurrentCampus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
            Subject = "Welcome to TIPS - Enquiry In Progress"; // mail Subject;
            RecipientInfo = "Dear Parent,";
            Body = "An enquiry is raised from " + CurrentCampus + " for " + Board + " to " + RecievedCampus + ", kindly check your enquiry inbox for further details.";
            foreach (var singleEmail in EmailId)
            {
                retValue = em.SendStudentEnquiryMail(null, singleEmail, CurrentCampus, Subject, Body, MailBody, RecipientInfo, "Parent");
            }
        }

        public ActionResult EnquiryEmail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EnquiryEmail(EnquiryDetails ed)
        {
            User Userobj = (User)Session["objUser"];
            if (Userobj == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            ReportCardService rptCardSrvc = new ReportCardService();
            AdmissionManagementService ads = new AdmissionManagementService();
            #region CriteriaBuilding
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            if (ed.EnquiryDetailsId != null && ed.EnquiryDetailsId > 0)
            {
                criteria.Add("EnquiryDetailsId", ed.EnquiryDetailsId);
            }
            if (!string.IsNullOrEmpty(ed.StudentName))
            {
                criteria.Add("StudentName", ed.StudentName);
            }
            if (!string.IsNullOrEmpty(ed.ParentName))
            {
                criteria.Add("ParentName", ed.ParentName);
            }
            if (!string.IsNullOrEmpty(ed.EnquirerMobileNo))
            {
                criteria.Add("EnquirerMobileNo", ed.EnquirerMobileNo);
            }
            if (!string.IsNullOrEmpty(ed.Campus)) { criteria.Add("Campus", ed.Campus); } else { criteria.Add("Campus", Userobj.Campus); }
            if (!string.IsNullOrEmpty(ed.Timing))
            {
                criteria.Add("Timing", ed.Timing);
            }
            if (!string.IsNullOrEmpty(ed.Batch))
            {
                criteria.Add("Batch", ed.Batch);
            }
            if (!string.IsNullOrEmpty(ed.CourseType))
            {
                criteria.Add("CourseType", ed.CourseType);
            }
            if (!string.IsNullOrEmpty(ed.Course))
            {
                criteria.Add("Course", ed.Course);
            }
            if (!string.IsNullOrEmpty(ed.Program))
            {
                criteria.Add("Program", ed.Program);
            }
            if (!string.IsNullOrEmpty(ed.AdmittedRefNo))
            {
                criteria.Add("EnquiryDetailsCode", ed.AdmittedRefNo);
            }
            if (!string.IsNullOrEmpty(ed.EnquiryStatus))
            {
                criteria.Add("EnquiryStatus", ed.EnquiryStatus);
            }
            if (!string.IsNullOrEmpty(ed.FollowupDate1))
            {
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                DateTime FromDate = DateTime.Parse(ed.FollowupDate1, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                criteria.Add("FollowupDate", FromDate);
            }
            #endregion

            Dictionary<long, IList<EnquiryDetails>> dcnEnquiryDetails = null;

            if (!string.IsNullOrEmpty(ed.Course))
            {
                dcnEnquiryDetails = EnqSvc.GetEnquiryDetailsListWithPagingAndCriteriaEQSearch(null, 10000, null, null, criteria);
            }
            else
            {
                dcnEnquiryDetails = EnqSvc.GetEnquiryDetailsListWithPagingAndCriteria(null, 10000, null, null, criteria);
            }

            // preregnum = ads.GetStudentDetailsListWithPagingAndCriteriaWithAlias(0, 10000, null, null, colName, values, criteria, null);

            IEnumerable<EnquiryDetails> abc = dcnEnquiryDetails.FirstOrDefault().Value.ToList();
            var distcmp = abc.Select(x => x.Campus).Distinct().ToList();      // get distinct campus of the students from the grid list

            BaseController bc = new BaseController();
            string cmp = Session["cmpnm"].ToString();
            if (Session["cmpnm"].ToString() == "")
            {
                //  preregnum=
                cmp = "All";
            }
            for (int l = 0; l < distcmp.Count(); l++)
            {
                IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(distcmp[l].ToString(), ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                string From = ConfigurationManager.AppSettings["From"];
                string To = ConfigurationManager.AppSettings["To"];
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();

                if (To == "live")
                {
                    mail.To.Add(campusemaildet.FirstOrDefault().EmailId.ToString());
                }
                else if (To == "test")
                {
                    mail.To.Add(campusemaildet.FirstOrDefault().EmailId.ToString());
                }

                if (ed.Subject != null)
                {
                    mail.Subject = ed.Subject;
                }
                if (Session["attachment"].ToString() != "")
                {
                    var ar = Session["attachment"].ToString().Split(',');
                    foreach (var var in ar)
                    {
                        Dictionary<string, object> criteria3 = new Dictionary<string, object>();
                        criteria3.Add("Id", Convert.ToInt64(var));
                        Dictionary<long, IList<EmailAttachment>> emailattachment = ads.GetEmailAttachmentListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria3);
                        MemoryStream ms = new MemoryStream(emailattachment.FirstOrDefault().Value[0].Attachment);
                        Attachment mailAttach = new Attachment(ms, emailattachment.FirstOrDefault().Value[0].AttachmentName.ToString());  //Data posted from form
                        mail.Attachments.Add(mailAttach);
                    }
                }
                string Body = ed.EmailContent;
                Body = Body.Replace("\r\n", "<br/>");

                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("localhost", 25);
                smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address  
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = true;

                int mailcnt = 0;
                int cnt = 1;

                for (int j = 0; j < dcnEnquiryDetails.FirstOrDefault().Key; j++)
                {
                    if (distcmp[l].ToString() == dcnEnquiryDetails.FirstOrDefault().Value[j].Campus.ToString())   // to send email campus wise according to the campus email id
                    {
                        mailcnt = mailcnt + 1;
                        //  if (st.Father == true)
                        {
                            Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                            //     criteria1.Add("EnquiryDetailsId", dcnEnquiryDetails.FirstOrDefault().Value[j].EnquiryDetailsId);                         
                            //     Dictionary<long, IList<FamilyDetails>> FamilyDetails = ads.GetFamilyDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria1);
                            if (dcnEnquiryDetails.FirstOrDefault().Value.Count() != 0)
                            {
                                if ((dcnEnquiryDetails.FirstOrDefault().Value[j].EnquirerEmailId != null) && (dcnEnquiryDetails.FirstOrDefault().Value[j].EnquirerEmailId.Contains("@")))
                                {
                                    if (Regex.IsMatch(dcnEnquiryDetails.FirstOrDefault().Value[0].EnquirerEmailId,
            @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
            RegexOptions.IgnoreCase))
                                    {
                                        mail.Bcc.Add(dcnEnquiryDetails.FirstOrDefault().Value[j].EnquirerEmailId);
                                    }
                                }
                            }
                        }

                        if (mail.Bcc.Count >= (Convert.ToInt32(ConfigurationManager.AppSettings["EmailCnt"])))
                        {
                            if (From == "live")
                            {
                                mail.From = new MailAddress(campusemaildet.FirstOrDefault().EmailId.ToString());
                                smtp.Credentials = new System.Net.NetworkCredential
                               (campusemaildet.FirstOrDefault().EmailId.ToString(), campusemaildet.FirstOrDefault().Password.ToString());
                                smtp.Send(mail);
                            }
                            else if (From == "test")
                            {
                                mail.From = new MailAddress(campusemaildet.FirstOrDefault().EmailId.ToString());
                                smtp.Credentials = new System.Net.NetworkCredential
                               (campusemaildet.FirstOrDefault().EmailId.ToString(), campusemaildet.FirstOrDefault().Password.ToString());
                                smtp.Send(mail);
                            }
                            EmailLog el = new EmailLog();
                            el.Id = 0;
                            if (To == "live")
                            {
                                el.EmailTo = campusemaildet.FirstOrDefault().EmailId.ToString();
                            }
                            else if (To == "test")
                            {
                                el.EmailTo = campusemaildet.FirstOrDefault().EmailId.ToString();
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
                            ads.CreateOrUpdateEmailLog(el);
                            mail.Bcc.Clear();
                        }
                    }
                }

                if (mail.Bcc.Count < (Convert.ToInt32(ConfigurationManager.AppSettings["EmailCnt"])))
                {
                    if (From == "live")
                    {
                        mail.From = new MailAddress(campusemaildet.FirstOrDefault().EmailId.ToString());
                        smtp.Credentials = new System.Net.NetworkCredential
                       (campusemaildet.FirstOrDefault().EmailId.ToString(), campusemaildet.FirstOrDefault().Password.ToString());
                        smtp.Send(mail);
                    }
                    else if (From == "test")
                    {
                        mail.From = new MailAddress(campusemaildet.FirstOrDefault().EmailId.ToString());
                        smtp.Credentials = new System.Net.NetworkCredential
                       (campusemaildet.FirstOrDefault().EmailId.ToString(), campusemaildet.FirstOrDefault().Password.ToString());
                        smtp.Send(mail);
                    }
                    EmailLog el = new EmailLog();
                    el.Id = 0;
                    if (To == "live")
                    {
                        el.EmailTo = campusemaildet.FirstOrDefault().EmailId.ToString();
                    }
                    else if (To == "test")
                    {
                        el.EmailTo = campusemaildet.FirstOrDefault().EmailId.ToString();
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
                    ads.CreateOrUpdateEmailLog(el);
                    mail.Bcc.Clear();
                }
            }
            Session["emailsent"] = "yes";
            Session["attachment"] = "";
            Session["attachmentnames"] = "";

            return RedirectToAction("EnquiryManagement", new { pagename = "enquirymail" });
        }

        public ActionResult EnquiryCourseGrid(long? EnquiryDetailsId, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                ReportCardService RepS = new ReportCardService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (EnquiryDetailsId != null)
                    criteria.Add("EnquiryId", EnquiryDetailsId);
                Dictionary<long, IList<EnquiryCourse>> EnqcourseDetails = EnqSvc.GetEnquiryCourseDetailsListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                long totalrecords = EnqcourseDetails.FirstOrDefault().Key;
                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                var jsondat = new
                {
                    total = totalPages,
                    page = page,
                    records = totalrecords,

                    rows = (from items in EnqcourseDetails.FirstOrDefault().Value
                            select new
                            {
                                i = 2,
                                cell = new string[] {
                            items.Id.ToString(),
                            items.CourseCode,
                            items.Campus,
                            items.Board,
                            items.Grade,
                            items.Subjects,
                            items.EnquiryId.ToString(),
                            items.CreatedBy,
                            items.CreatedDate.ToString(),
                            items.ModifiedBy,
                            items.ModifiedDate.ToString()
                            }
                            })
                };
                return Json(jsondat, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteMultipleListDetails(string id)
        {
            try
            {
                AdmissionManagementService AmdSrv = new AdmissionManagementService();
                var values = id.Split(',');

                long[] idtodelete = new long[values.Length];
                int i = 0;
                foreach (string val in values)
                {
                    idtodelete[i] = Convert.ToInt64(val);
                    i++;
                }

                //EnqSvc.DeleteEnquiryCourseDetails(idtodelete);

                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
        }

        public string GetBodyofMail()
        {
            string MessageBody = System.IO.File.ReadAllText(HttpContext.Server.MapPath("~/Views/Shared/AdmissionEmailBody.html"));
            return MessageBody;
        }

        public ActionResult GetSubjectsByGradeForEnquiry(string Grade)
        {
            try
            {
                MastersService ms = new MastersService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Grade", Grade);
                Dictionary<long, IList<SubjectMaster>> subMaster = ms.GetSubjectMasterListWithPagingAndCriteria(0, 9999, "SubjectName", "Asc", criteria);
                if (subMaster != null && subMaster.Count > 0 && subMaster.FirstOrDefault().Key > 0)
                {
                    var subjectMstrLst = new
                    {
                        rows = (
                             from items in subMaster.FirstOrDefault().Value
                             select new
                             {
                                 Text = items.SubjectName,
                                 Value = items.SubjectName
                             }).Distinct().ToList()
                    };

                    return Json(subjectMstrLst, JsonRequestBehavior.AllowGet);
                }
                else
                { return Json(null, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "EnquiryPolicy");
                throw ex;
            }
        }
        public ActionResult KioskEnquiry()
        {
            User Userobj = (User)Session["objUser"];
            if (Userobj == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            return View();
        }

      
        
    }
}











