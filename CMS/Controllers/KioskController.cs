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
using TIPS.Entities.CommunictionEntities;
using CMS.Controllers;
using TIPS.Entities.AdmissionEntities;
using TIPS.Service;
using TIPS.Entities.EnquiryEntities;
using TIPS.ServiceContract;
using CMS.Helpers;
using TIPS.Entities.Assess;
using System.Web.UI.WebControls;

namespace CMS.Controllers
{
    public class KioskController : BaseController
    {
        MastersService ms = new MastersService();
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult NewParent()
        {

            Dictionary<string, object> criteria = new Dictionary<string, object>();
            //Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            //ViewBag.acadddl = AcademicyrMaster.First().Value;
            Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.campusddl = CampusMaster.FirstOrDefault().Value;
            Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.gradeddl = GradeMaster.First().Value;
            DateTime DateNow = DateTime.Now;
            string acayear1 = "";
            string acayear2 = "";
            string acayear3 = "";
            acayear1 = DateNow.Year.ToString() + "-" + (DateNow.Year + 1).ToString();
            acayear2 = (DateNow.Year + 1).ToString() + "-" + (DateNow.Year + 2).ToString();
            acayear3 = (DateNow.Year + 2).ToString() + "-" + (DateNow.Year + 3).ToString();
            string[] acayr = new string[3];
            acayr[0] = acayear1;
            acayr[1] = acayear2;
            acayr[2] = acayear3;

            ViewBag.acadyrddl = acayr;
            return View();
        }

        public ActionResult ExistingParent()
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.acadddl = AcademicyrMaster.First().Value;
            Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.campusddl = CampusMaster.FirstOrDefault().Value;
            Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.gradeddl = GradeMaster.First().Value;
            MasterDataService mds = new MasterDataService();
            //criteria.Add("ParentCode", "1");
            Dictionary<long, IList<IssueGroupMaster>> IssueGroupList = mds.GetIssueGroupListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            var IssueGroupddl = (
                     from items in IssueGroupList.First().Value
                     select new
                     {
                         Text = items.IssueGroup,
                         Value = items.IssueGroup
                     }).ToList();
            criteria.Clear();
            ViewBag.IssueGroupddl = IssueGroupddl;
            return View();
        }




        [HttpPost]
        public ActionResult EnquiryDetails(StudentTemplate st, FamilyDetails fd, HttpPostedFileBase file1)
        {
            try
            {
                //st.AddressDetailsList = null;
                //        st.FamilyDetailsList = null;

                AdmissionManagementService ads = new AdmissionManagementService();
                FamilyDetails fdet = new FamilyDetails();
                AddressDetails Adet = new AddressDetails();
                AdmissionManagementService ams = new AdmissionManagementService();
                Dictionary<long, IList<PreRegDetails>> prd = ams.GetPreRegDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, null);
                var id1 = prd.First().Value[0].PreRegNum + 1;
                PreRegDetails srn = new PreRegDetails();
                srn.PreRegNum = id1;
                srn.Id = prd.First().Value[0].Id;
                srn.Date = DateTime.Now;
                ams.CreateOrUpdatePreRegDetails(srn);
                Session["preregno"] = id1;
                st.ApplicationNo = "TIPS-" + id1;
                st.PreRegNum = id1;
                //st.Status = "1";
                st.AcademicYear = st.JoiningAcademicYear;
                st.Status1 = "2";
                st.AdmissionStatus = "New Enquiry";
                st.EntryFrom = "Kiosk";
                st.CreatedDate = DateTime.Now.Date.ToString();
                st.CreatedTime = DateTime.Now.TimeOfDay.ToString();
                st.FamilyDetailsList[0].FamilyDetailType = "Father";
                st.FamilyDetailsList[0].StudentId = st.Id;
                st.FamilyDetailsList[0].PreRegNum = st.PreRegNum;
                fdet.FamilyDetailType = st.FamilyDetailsList[0].FamilyDetailType;
                fdet.Name = st.FamilyDetailsList[0].Name;
                fdet.StudentId = st.FamilyDetailsList[0].StudentId;
                fdet.PreRegNum = st.FamilyDetailsList[0].PreRegNum;
                fdet.Mobile = st.FamilyDetailsList[0].Mobile;
                ads.CreateOrUpdateFamilyDetails(fdet);
                TempData["Father"] = st.FamilyDetailsList[0].Name;
                TempData["altmobile"] = st.FamilyDetailsList[0].Mobile;
                st.FamilyDetailsList[1].FamilyDetailType = "Mother";
                st.FamilyDetailsList[1].StudentId = st.Id;
                st.FamilyDetailsList[1].PreRegNum = st.PreRegNum;
                fdet.FamilyDetailType = st.FamilyDetailsList[1].FamilyDetailType;
                fdet.Name = st.FamilyDetailsList[1].Name;
                fdet.StudentId = st.FamilyDetailsList[1].StudentId;
                fdet.PreRegNum = st.FamilyDetailsList[1].PreRegNum;
                fdet.Id = 0;
                TempData["Mother"] = st.FamilyDetailsList[1].Name;
                ads.CreateOrUpdateFamilyDetails(fdet);
                TempData["DeviceType"] = st.DeviceType;

                if (st.DeviceType != "Desktop")
                {
                    string strUrl; WebRequest request; WebResponse response; Stream s; StreamReader readStream;
                    CommunicationService comSer = new CommunicationService();
                    StaffComposeSMSInfo smsInfo = new StaffComposeSMSInfo();
                    string TestNumber = st.FamilyDetailsList[0].Mobile;
                    if (TestNumber.Length < 11)
                    {
                        if (Regex.IsMatch(TestNumber, @"(?<!\d)\d{10}(?!\d)", RegexOptions.IgnoreCase))
                        {
                            if (smsInfo != null)
                            {
                                string dataString = string.Empty;
                                smsInfo.Message = " Dear Parent, thank you for registering with TIPS school and Your online application number is " + st.ApplicationNo + "  .TIPS Team.";
                                smsInfo.Campus = st.Campus;
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
                            }
                        }
                    }
                }
                st.FamilyDetailsList = null;
                AdmissionManagementService ams1 = new AdmissionManagementService();
                Dictionary<string, object> criteria4 = new Dictionary<string, object>();
                criteria4.Add("StudentId", st.Id);
                Dictionary<long, IList<AddressDetails>> add = ams1.GetAddressDetailsListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria4);
                if (add.First().Value.Count != 0)
                {
                    if (add.First().Value.Count == 1) { st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[0].Id = add.First().Value[0].Id; }
                    else if (add.First().Value.Count == 2)  // (add.Count == 2)
                    {
                        st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[0].Id = add.First().Value[0].Id;
                        st.AddressDetailsList[1].AddressType = "Secondary Address"; st.AddressDetailsList[1].Id = add.First().Value[1].Id;
                    }
                    else { }
                }
                else
                {
                    st.AddressDetailsList[0].AddressType = "Primary Address"; st.AddressDetailsList[1].AddressType = "Secondary Address";
                }
                st.AddressDetailsList = null;
                //info = " You have saved an New Student Registration with Application Number " + st.ApplicationNo;
                //UpdateInbox(st.Campus, info, userid, st.PreRegNum);
                ads.CreateOrUpdateStudentDetails(st);
                UserService us = new UserService();
                CampusEmailId campusemailiddtls = us.GetCampusEmailIdByCampusWithServer(st.Campus, "Live");
                if (campusemailiddtls != null)
                {
                    TempData["contact"] = campusemailiddtls.BulkEmailId;
                }
                else
                {
                    TempData["contact"] = "askcbemain@tipsglobal.net";
                }


                TempData["Id"] = st.Id;
                string RecipientInfo = "", Subject = "", Body = "", MailBody = ""; bool retValue;
                EmailHelper em = new EmailHelper();
                MailBody = GetBodyofMail();
                //StudentTemplate st = new StudentTemplate();
                IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(st.EnquiryLocationFrom, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                if (st.DeviceType == "Desktop")
                {
                    if (ConfigurationManager.AppSettings["SendEmail1"].ToString() == "true")
                    {
                        RecipientInfo = "Dear Parent,";
                        Subject = "Welcome to TIPS - Student Registration In Progress"; // st.Subject;
                        Body = "Thanks for registering with TIPS school and your application is sent for processing. Your online application number is " + st.ApplicationNo + " and please refer this number for all further communication. For any queries, mail us at " + campusemaildet.First().EmailId.ToString() + ".";
                        //MailBody = Body;
                        if (!string.IsNullOrEmpty(st.EmailId))
                            retValue = em.SendStudentRegistrationMail(st, st.EmailId, st.Campus, Subject, Body, MailBody, RecipientInfo, "Parent", null);
                        //Send Mail to Managment
                        RecipientInfo = "Dear Team,";
                        Subject = "A new Enquiry is available for processing"; // st.Subject;
                        string Body1 = "A new Enquiry is available for Clarification and you are requested to Meet them.<br><br>Campus: " + st.Campus + "<br>";
                        Body1 += "Application number: " + st.ApplicationNo + "<br>Applicant name: " + st.Name + "<br>Application grade: " + st.Grade + "<br>Enquired Academic year: " + st.JoiningAcademicYear + "<br>";
                        //MailBody = Body1;
                        retValue = em.SendStudentRegistrationMail(st, null, st.EnquiryLocationFrom, Subject, Body1, MailBody, RecipientInfo, "Management", null);
                    }
                }



                //var succmsg = "Thanks For your Enquiry in TIPS.Your EnquiryNumber is " + st.ApplicationNo +"Our Consultant Will Meet you asap. Please Wait!!";

                return RedirectToAction("PrintTicket");


            }

            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }




        //public ActionResult KioskBasicDetails(long? StudentId, string StudentName, string Grade, string Campus, string AcademicYear, string Section, string Phone, string Email, string Visit)
        //{

        //    if (Visit != "" && Visit != null)
        //    {
        //        EnquiryService EnqSvc = new EnquiryService();
        //        KioskBasicDetails kbd = new KioskBasicDetails();
        //        kbd.StudentName = StudentName;
        //        kbd.Grade = Grade;
        //        kbd.Campus = Campus;
        //        kbd.AcademicYear = AcademicYear;
        //        kbd.Section = Section;
        //        kbd.Phone = Phone;
        //        kbd.EmailId = Email;
        //        kbd.VisitPurpose = Visit;
        //        EnqSvc.SaveOrUpdateKioskBasicDetails(kbd);

        //        if (Visit == "Issue Request")
        //        {
        //            return Json("Issuerequest", JsonRequestBehavior.AllowGet);

        //        }

        //        return Json("Feedback", JsonRequestBehavior.AllowGet);


        //    }
        //    else
        //    {
        //        return Json("Basic Details Invalid", JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public ActionResult Feedback()
        //{
        //    return View();
        //}


        public ActionResult PrintTicket()
        {
            StudentTemplate St = new StudentTemplate();
            AdmissionManagementService ams = new AdmissionManagementService();
            var Id = TempData["Id"];
            if (Id != null)
            {
                St = ams.GetStudentDetailsById(Convert.ToInt64(Id));
                if (St.Campus == "IB MAIN")
                {
                    ViewBag.Institution = "CBE International Board";
                }
                if (St.Campus == "IB KG")
                {
                    ViewBag.Institution = " CBE Kinder Garten";
                }
                if (St.Campus == "CBSE MAIN")
                {
                    ViewBag.Institution = "CBE CBSE";
                }
                if (St.Campus == "KARUR")
                {
                    ViewBag.Institution = "KARUR International Board";
                }
                if (St.Campus == "KARUR KG")
                {
                    ViewBag.Institution = " KARUR Kinder Garten";
                }
                if (St.Campus == "KARUR CBSE")
                {
                    ViewBag.Institution = "KARUR CBSE";
                }
                if (St.Campus == "CHENNAI CITY")
                {
                    ViewBag.Campus = "CHENNAI International Board";
                }
                if (St.Campus == "CHENNAI MAIN")
                {
                    ViewBag.Institution = "CHENNAI Kinder Garten";
                }
                if (St.Campus == "TIRUPUR")
                {
                    ViewBag.Institution = "TIRUPUR International Board";
                }
                if (St.Campus == "TIRUPUR KG")
                {
                    ViewBag.Institution = " TIRUPUR Kinder Garten";
                }
                if (St.Campus == "ERNAKULAM")
                {
                    ViewBag.Institution = " ERNAKULAM ";
                }

                else
                {
                    ViewBag.Campus = "IB MAIN";
                }

                ViewBag.Email = St.EmailId;
                ViewBag.Phone = St.MobileNo;
                ViewBag.FatherName = TempData["Father"];
                ViewBag.MotherName = TempData["Mother"];
                ViewBag.AltMobile = TempData["altmobile"];
                ViewBag.contact = TempData["contact"];
                ViewBag.devicetype = TempData["DeviceType"];

                return View(St);
            }
            else
            {
                return View();
            }

        }
        public ActionResult FillIssueType(string IssueGroup)
        {
            try
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
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "KioskPolicy");
                throw ex;
            }
        }



        public ActionResult RegisterIssues(string Campus, string StudentName, string Grade, string Section, string IssueType, string IssueGroup, string Description, string StudentId,string devicetype)
        {
            try
            {
                CallManagement cm = new CallManagement();
                StudentTemplate st = new StudentTemplate();
                ProcessFlowServices pfs = new ProcessFlowServices();
                CallManagementService cms1 = new CallManagementService();
                AdmissionManagementService ams = new AdmissionManagementService();
                st = ams.GetStudentDetailsByNewId(StudentId);
                if (st != null)
                {
                    cm.School = st.Campus;
                    cm.IssueGroup = IssueGroup;
                    cm.IssueType = IssueType;
                    cm.Description = Description;
                    cm.Email = st.EmailId;
                    cm.StudentNumber = StudentId;
                    cm.Grade = st.Grade;
                    cm.Status = "ResolveIssue";
                    cm.StudentName = st.Name;
                    cm.Section = st.Section;
                    cm.UserType = "Parent";
                    cm.BranchCode = cm.School;
                    cm.DeptCode = IssueGroup.ToUpper();
                    cm.UserInbox = 'P' + StudentId;
                    cm.IssueDate = DateTime.Now;
                    string UserId = StudentId;
                    long id = 0;
                    id = pfs.StartCallManagement(cm, "parentportal", UserId);
                    pfs.CompleteActivityCallManagement(cm, "parentportal", UserId, "LogIssue", false);
                    var Id = cms1.CreateOrUpdateCallManagement(cm);
                    string cur = (DateTime.Now.Year).ToString().Substring(2);
                    string nxt = (DateTime.Now.Year + 1).ToString().Substring(2);
                    cm.IssueNumber = "PCMGT-" + cur + "-" + nxt + "-" + Id;
                    cms1.CreateOrUpdateCallManagement(cm);
                    if (devicetype != "Desktop")
                    {
                        string strUrl; WebRequest request; WebResponse response; Stream s; StreamReader readStream;
                        CommunicationService comSer = new CommunicationService();
                        StaffComposeSMSInfo smsInfo = new StaffComposeSMSInfo();
                        string TestNumber = st.MobileNo;
                        if (TestNumber.Length < 11)
                        {
                            if (Regex.IsMatch(TestNumber, @"(?<!\d)\d{10}(?!\d)", RegexOptions.IgnoreCase))
                            {
                                if (smsInfo != null)
                                {
                                    string dataString = string.Empty;
                                    smsInfo.Message = " Dear Parent, thank you for registering registering Issue and Your Issue Ticket Number is " + cm.IssueNumber + "  .TIPS Team.";
                                    //smsInfo.Campus = st.Campus;
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
                                }
                            }
                        }
                    }
                    TempData["device"] = devicetype;
                    TempData["IssueNumber"] = cm.IssueNumber;
                    TempData["Issuegroup"] = cm.IssueGroup;
                    UserService us = new UserService();
                    CampusEmailId campusemailiddtls = us.GetCampusEmailIdByCampusWithServer(cm.School, "Live");
                    TempData["contact"] = campusemailiddtls.BulkEmailId;
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("AppCode", "ISM");
                    Dictionary<long, IList<UserAppRole>> UserAppRoleList = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    string issue = string.Empty;
                    if (cm.IssueGroup != null)
                        issue = cm.IssueGroup.ToUpper();
                    string[] ResolverEmail = (from u in UserAppRoleList.First().Value
                                              where u.DeptCode == issue && u.RoleCode == "GRL" && u.BranchCode == cm.BranchCode && u.Email != null
                                              select u.Email).Distinct().ToArray();
                    ActorsEmail(cm, ResolverEmail);
                    string RecipientInfo = "", Subject = "", Body = "", MailBody = ""; bool retValue;
                    EmailHelper em = new EmailHelper();
                    MailBody = GetBodyofMail();
                    //StudentTemplate st = new StudentTemplate();
                    IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(st.Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                    if (devicetype == "Desktop")
                    {
                        if (ConfigurationManager.AppSettings["SendEmail1"].ToString() == "true")
                        {
                            RecipientInfo = "Dear Parent,";
                            Subject = "Welcome to TIPS - Student Issue Registration "; // st.Subject;
                            Body = "Your Issue has been Registered Successfully and your Issue ticket number is " + cm.IssueNumber + " and please refer this number for all further communication. For any queries, mail us at " + campusemaildet.First().EmailId.ToString() + ".";
                            //MailBody = Body;
                            if (!string.IsNullOrEmpty(st.EmailId))
                                retValue = em.SendStudentRegistrationMail(st, st.EmailId, st.Campus, Subject, Body, MailBody, RecipientInfo, "Parent", null);

                        }
                    }
                    var succmsg = "The Issue has been Successfully Registered.Your EnquiryNumber is " + cm.IssueNumber;

                    return Json(succmsg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Errmsg = "The Student Id Number / Parent Id Number is Incorrect or does not exist";

                    return Json(Errmsg, JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        public string GetBodyofMail()
        {
            string MessageBody = System.IO.File.ReadAllText(HttpContext.Server.MapPath("~/Views/Shared/AdmissionEmailBody.html"));
            return MessageBody;
        }




        public ActionResult GetStudentDetailsByIdNumber(string StudentId)
        {
            try
            {
                TempData["StudentId"] = StudentId;
                AdmissionManagementService ams = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("NewId", StudentId);
                //criteria.Add("IsActive", true);
                //Dictionary<long, IList<StudentTemplate>> StudentList = ams.GetStudentDetailsbyNameandCampus(0, 9999, "", "", criteria);
                StudentTemplate st = ams.GetStudentDetailsByNewId(StudentId);
                //if (StudentList != null && StudentList.FirstOrDefault().Value != null && StudentList.FirstOrDefault().Key > 0)
                //{
                //    return Json(StudentList.FirstOrDefault().Value[0], JsonRequestBehavior.AllowGet);
                //}
                if (st != null)
                {
                    return Json(st, JsonRequestBehavior.AllowGet);
                }

                //var Studid = "Student Id is Incorrect!";

                return Json(null, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult GetFamilyDetailsByPreRegNum(string PreRegNum)
        {
            try
            {
                AdmissionManagementService ams = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum", Convert.ToInt64(PreRegNum));
                Dictionary<long, IList<FamilyDetails>> FamilyDetails = ams.GetFamilyDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                if (FamilyDetails != null && FamilyDetails.FirstOrDefault().Value != null && FamilyDetails.FirstOrDefault().Key > 0)
                {
                    return Json(FamilyDetails.FirstOrDefault().Value[0], JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }

        }


        public void ActorsEmail(CallManagement cm, string[] ResolverEmail)
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

                        if (cm.Status == "ResolveIssue")
                        {
                            for (int i = 0; i < ResolverEmail.Length; i++)
                            {
                                if (ResolverEmail[i] != "")
                                    mail.To.Add(ResolverEmail[i]);
                            }
                            mail.Subject = "Tips support request #" + cm.IssueNumber + " needs your resolve ";
                            string Body = "Dear Sir/Madam, <br/><br/>" +
                                        "Tips support request #" + cm.IssueNumber + " needs your resolve <br/><br/>" +
                                        "<b>Issue Description</b><br/><br/>" +
                                         cm.Description + ".<br/><br/>" +
                                        "The issue " + cm.IssueNumber + "  is assigned for your closing and please resolve this issue asap.<br/><br/>";
                            mail.Body = Body;
                        }

                        mail.IsBodyHtml = true;
                        BaseController bc = new BaseController();
                        IList<CampusEmailId> campusemaildet = bc.GetEmailIdByCampus(cm.BranchCode, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                        SendEmailWithEmailTemplate(mail, cm.BranchCode);
                    }
                    catch (Exception ex)
                    {
                        ExceptionPolicy.HandleException(ex, "StudentPortalPolicy");
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "StudentPortalPolicy");
                throw ex;
            }
        }

        public bool SendEmailWithEmailTemplate(MailMessage mail, string Campus)
        {
            IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
            string GeneralBody = GetGeneralBodyofMail();
            GeneralBody = GeneralBody.Replace("{{CampusmailId}}", campusemaildet.First().EmailId);
            GeneralBody = GeneralBody.Replace("{{Ph.No}}", campusemaildet.First().PhoneNumber);
            GeneralBody = GeneralBody.Replace("{{DateTime}}", DateTime.Now.ToString());
            GeneralBody = GeneralBody.Replace("{{Recipients}}", "");
            GeneralBody = GeneralBody.Replace("{{Content}}", mail.Body);
            GeneralBody = GeneralBody.Replace("{{FbLink}}", campusemaildet.First().FBLink);
            mail.Body = GeneralBody;
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
                    return true;
                }
                catch (Exception)
                {
                    mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                    smtp.Credentials = new System.Net.NetworkCredential(campusemaildet.First().AlternateEmailId, campusemaildet.First().AlternateEmailIdPassword);
                    new Task(() => { smtp.Send(mail); }).Start();
                    return true;
                }
            }
            return true;
        }


        public ActionResult ConfirmDetails(string mobileno, string campus)
        {
            try
            {
                string sOTP = String.Empty;
                string strUrl; WebRequest request; WebResponse response; Stream s; StreamReader readStream;
                if (!string.IsNullOrWhiteSpace(mobileno) && Regex.IsMatch(mobileno, @"(?<!\d)\d{10}(?!\d)", RegexOptions.IgnoreCase))
                {
                    int iOTPLength = 5;
                    string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };

                    Random rand = new Random();
                    string sTempChars = String.Empty;
                    for (int i = 0; i < iOTPLength; i++)
                    {

                        int p = rand.Next(0, saAllowedCharacters.Length);

                        sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];

                        sOTP += sTempChars;

                    }
                    var Message = "Your OTP for Issue Registration in TIPS is " + sOTP;
                    ViewData["OTP"] = sOTP;
                    //Sending SMS
                    // strUrl = ConfigurationManager.AppSettings["SMSService"].ToString() + "&senderID=TIPSGB&receipientno=" + item.MobileNo + "&dcs=0&msgtxt=" + smsInfo.Message + "&state=1";
                    strUrl = ConfigurationManager.AppSettings["SMSService"].ToString() + "&senderID=" + GetSenderIdByCampus(campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString()) + "&receipientno=" + mobileno + "&dcs=0&msgtxt=" + Message + "&state=1";

                    request = WebRequest.Create(strUrl);
                    response = request.GetResponse();
                    s = response.GetResponseStream();
                    readStream = new StreamReader(s);
                    response.Close();
                    s.Close();
                    readStream.Close();


                }
                return Json(sOTP, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult IssueRegistration()
        {
            try
            {
                if (TempData["IssueNumber"] != null && TempData["Issuegroup"] != null && TempData["contact"] != null)
                {
                    ViewBag.IssueNumber = TempData["IssueNumber"].ToString();
                    ViewBag.Issuegroup = TempData["Issuegroup"];
                    ViewBag.contact = TempData["contact"];
                    ViewBag.Device = TempData["device"];
                    return View();
                }
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }



        public ActionResult Index1()
        {
            try
            {

                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }



    }
}
