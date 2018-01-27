using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.IO;
using TIPS.Entities.TicketingSystem;
using TIPS.Service;
using TIPS.Entities;
using System.Configuration;
using TIPS.Entities.AdmissionEntities;
using TIPS.ServiceContract;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TIPS.Service.TicketingSystem;
using System.Threading.Tasks;
using CMS.Controllers;

namespace CMS.Helpers
{
    public class EmailHelper : BaseController
    {

        public void SendEmailNotification(TicketSystem objTcktSys, string userid, string activityName, bool? isrejction, string MailBody)
        {
            try
            {
                // IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(objTcktSys.BranchCode, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                bool rejection = isrejction ?? true;// false;
                UserService us = new UserService();
                Dictionary<long, IList<UserAppRole>> userAppRole = new Dictionary<long, IList<UserAppRole>>();
                Dictionary<string, object> criteriaUserAppRole = new Dictionary<string, object>();

                //Based on activity need to assign the role ta get the users
                //logic need to be written
                criteriaUserAppRole.Add("AppCode", "TKT");
                if (activityName == "LogETicket" || activityName == "ResolveETicketRejection" || activityName == "CloseETicket" || (activityName == "CompleteETicket" && isrejction == true))
                {
                    criteriaUserAppRole.Add("RoleCode", "ETR");
                }
                else if (activityName == "ResolveETicket" || activityName == "CloseETicketRejection" || activityName == "ReopenETicket")
                { criteriaUserAppRole.Add("RoleCode", "ETC"); }
                else return;
                userAppRole = us.GetRoleUsersListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteriaUserAppRole);
                //if list userAppRole is null then empty grid
                if (userAppRole != null && userAppRole.Count > 0 && userAppRole.First().Key > 0)
                {
                    int count = userAppRole.First().Value.Count;
                    //if it has values then for each concatenate APP+ROLE 
                    string[] userEmails = new string[count];

                    int i = 0;
                    Dictionary<string, object> criteria = new Dictionary<string, object>();

                    foreach (UserAppRole uar in userAppRole.First().Value)
                    {
                        if (!string.IsNullOrWhiteSpace(uar.Email))
                        {
                            userEmails[i] = uar.Email.Trim();
                        }
                        i++;
                    }
                    if (userEmails == null || (userEmails[0] == null && userEmails.Length == 0)) { return; }
                    if (userEmails != null && userEmails.Length > 0)
                    {
                        string SendEmail = ConfigurationManager.AppSettings["SendEmail1"];
                        string From = ConfigurationManager.AppSettings["From"];
                        if (SendEmail == "false")
                        { return; }
                        else
                        {
                            try
                            {
                                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                                mail.Subject = "e-Ticket Notification" + objTcktSys.TicketNo + " "; string msg = "";

                                foreach (string s in userEmails)
                                {
                                    if (!string.IsNullOrWhiteSpace(s))
                                        mail.To.Add(s);
                                }
                                if (activityName == "CompleteETicket" && isrejction == true) { activityName = "ReopenETicket"; }
                                switch (activityName)
                                {
                                    case "LogETicket":
                                        {
                                            msg += "An e-Ticket " + objTcktSys.TicketNo + " has been Logged for the module " + objTcktSys.Module + " with " + objTcktSys.Priority + " priority. The e-Ticket is Raised by " + userid + ". Please try resolving the ticket based on Priority. The summary of the ticket is <b><i>\"" + objTcktSys.Summary + "\"</i></b> ";
                                            break;
                                        }
                                    case "ResolveETicket":
                                        {
                                            if (rejection)
                                            {
                                                msg += "An e-Ticket " + objTcktSys.TicketNo + " has been Rejected for the module " + objTcktSys.Module + ". The e-Ticket is Rejected for additional information by " + userid + ". Please try replying the same based on the comments.<b><i>\"" + ShowComments(objTcktSys.Id, userid, "ResolveETicket", "Rejection", objTcktSys.TicketNo) + "\"</i></b>";
                                                msg = msg + CommentSummary(objTcktSys.Id, userid, "ResolveETicket", "Rejection", objTcktSys.TicketNo);
                                            }
                                            else
                                            {
                                                msg += "An e-Ticket " + objTcktSys.TicketNo + " has been Resolved for the module " + objTcktSys.Module + ". The e-Ticket is Resolved by " + userid + " with comments <b><i>\"" + ShowComments(objTcktSys.Id, userid, "ResolveETicket", "Resolution", objTcktSys.TicketNo) + "\"</i></b>. Please verify the same and complete/Reject based on the results. ";
                                                msg = msg + CommentSummary(objTcktSys.Id, userid, "ResolveETicket", "Rejection", objTcktSys.TicketNo);
                                            }
                                            break;
                                        }
                                    case "ResolveETicketRejection":
                                        {
                                            msg += "An e-Ticket " + objTcktSys.TicketNo + " has been Replied for the module " + objTcktSys.Module + ".by " + userid + " with Comments. <b><i>\"" + ShowComments(objTcktSys.Id, userid, "ResolveETicketRejection", "Resolution", objTcktSys.TicketNo) + "\"</i></b>. Please try resolving the same based on the reply comments. ";
                                            msg = msg + CommentSummary(objTcktSys.Id, userid, "ResolveETicket", "Rejection", objTcktSys.TicketNo);
                                            break;
                                        }
                                    case "ReopenETicket":
                                        {
                                            if (rejection)
                                            {
                                                msg += "An e-Ticket " + objTcktSys.TicketNo + " has been Reopen for the module " + objTcktSys.Module + ". The e-Ticket is Reopened for additional information by " + userid + ". Please try replying the same based on the comments.<b><i>\"" + ShowComments(objTcktSys.Id, userid, "CompleteETicket", "Rejection", objTcktSys.TicketNo) + "\"</i></b>";
                                                msg = msg + CommentSummary(objTcktSys.Id, userid, "CompleteETicket", "Rejection", objTcktSys.TicketNo);
                                            }
                                            else
                                            {
                                                msg += "An e-Ticket " + objTcktSys.TicketNo + " has been Resolved for the module " + objTcktSys.Module + ". The e-Ticket is Resolved by " + userid + " with comments <b><i>\"" + ShowComments(objTcktSys.Id, userid, "ReopenETicket", "Rejection", objTcktSys.TicketNo) + "\"</i></b>. Please verify the same and complete/Reject based on the results. ";
                                                msg = msg + CommentSummary(objTcktSys.Id, userid, "ReopenETicket", "Rejection", objTcktSys.TicketNo);
                                            }
                                            break;
                                        }
                                    case "CloseETicketRejection":
                                        {
                                            if (rejection)
                                            {
                                                msg += "";
                                            }
                                            else
                                            {
                                                msg += "An e-Ticket " + objTcktSys.TicketNo + " has been Rejected for the module " + objTcktSys.Module + ". The e-Ticket is Rejected by " + userid + " <b><i>\"" + ShowComments(objTcktSys.Id, userid, "CloseETicketRejection", "Resolution", objTcktSys.TicketNo) + "\"</i></b>. Please try resolving the same based on the comments. ";
                                                msg = msg + CommentSummary(objTcktSys.Id, userid, "ResolveETicket", "Rejection", objTcktSys.TicketNo);
                                            }
                                            break;
                                        }
                                    case "CloseETicket":
                                        {
                                            if (rejection)
                                            {
                                                msg += "An e-Ticket " + objTcktSys.TicketNo + " has been Rejected for the module " + objTcktSys.Module + ". The e-Ticket is Rejected for additionnal information by " + userid + " <b><i>\"" + ShowComments(objTcktSys.Id, userid, "CloseETicket", "Rejection", objTcktSys.TicketNo) + "\"</i></b>. Please try replying the same based on the comments. ";
                                                msg = msg + CommentSummary(objTcktSys.Id, userid, "ResolveETicket", "Rejection", objTcktSys.TicketNo);
                                            }
                                            //else msg += "An e-Ticket " + objTcktSys.TicketNo + " has been Completed for the module " + objTcktSys.Module + ". The e-Ticket is Completed by " + userid + " with comments <b><i>\"" + objTcktSys.Comments + "\"</i></b> ";
                                            else
                                            {
                                                msg += "An e-Ticket " + objTcktSys.TicketNo + " has been Completed for the module " + objTcktSys.Module + ". The e-Ticket is Completed by " + userid + " with comments <b><i>\"" + ShowComments(objTcktSys.Id, userid, "CloseETicket", "Resolution", objTcktSys.TicketNo) + "\"</i></b> ";
                                                msg = msg + CommentSummary(objTcktSys.Id, userid, "ResolveETicket", "Rejection", objTcktSys.TicketNo);
                                            }
                                            break;
                                        }
                                    default:
                                        return;
                                }
                                MailBody = MailBody.Replace("{{DateTime}}", DateTime.Now.ToString());
                                MailBody = MailBody.Replace("{{Content}}", msg);
                                //Body = Body.Replace("{{footer}}", footer);
                                mail.Body = MailBody;
                                mail.IsBodyHtml = true;
                                SmtpClient smtp = new SmtpClient("localhost", 25);
                                smtp.Host = "smtp.gmail.com";
                                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                                smtp.EnableSsl = true;
                                if (From == "live")
                                {
                                    try
                                    {
                                        mail.From = new MailAddress("tipscmsupp0rthyderabad247@gmail.com");
                                        smtp.Credentials = new System.Net.NetworkCredential
                                       ("tipscmsupp0rthyderabad247@gmail.com", "Supp0rt24by7");
                                        smtp.Send(mail);
                                    }
                                    catch (Exception ex)
                                    {
                                        if (ex.Message.Contains("quota"))
                                        {
                                            mail.From = new MailAddress("tipscmsupp0rthyderabad247@gmail.com");
                                            smtp.Credentials = new System.Net.NetworkCredential
                                            ("tipscmsupp0rthyderabad247@gmail.com", "Supp0rt24by7");
                                            smtp.Send(mail);
                                        }
                                        else
                                        {
                                            mail.From = new MailAddress("tipscmsupp0rthyderabad247@gmail.com");
                                            smtp.Credentials = new System.Net.NetworkCredential
                                            ("tipscmsupp0rthyderabad247@gmail.com", "Supp0rt24by7");
                                            smtp.Send(mail);
                                        }
                                    }
                                }
                                else if (From == "test")
                                {
                                    mail.From = new MailAddress("tipscmsupp0rthyderabad247@gmail.com");
                                    smtp.Credentials = new System.Net.NetworkCredential
                                    ("tipscmsupp0rthyderabad247@gmail.com", "Supp0rt24by7");
                                    //this is to send email to test mail only to avoid mis communication to the parent
                                    //  mail.To.Add("tipscmsupp0rthyderabad247@gmail.com");
                                    smtp.Send(mail);
                                }
                                EmailLog el = new EmailLog();
                                el.Id = 0;

                                el.EmailTo = mail.To.ToString();

                                el.EmailCC = mail.CC.ToString();
                                if (mail.Bcc.ToString().Length < 3990)
                                {
                                    el.EmailBCC = mail.Bcc.ToString();
                                }

                                el.Subject = mail.Subject.ToString();

                                if (mail.Body.ToString().Length < 3990)
                                {
                                    el.Message = msg;
                                }
                                el.EmailDateTime = DateTime.Now.ToString();
                                el.BCC_Count = mail.Bcc.Count;
                                el.Module = "ETicket";
                                //create the admission management object
                                AdmissionManagementService ads = new AdmissionManagementService();
                                //log the email to the database
                                ads.CreateOrUpdateEmailLog(el);
                            }
                            catch (Exception)
                            { }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "Assess360Policy");
                throw ex;
            }
        }

        public string ShowComments(long ticketId, string userId, string activityName, string comments, string TktNo)
        {
            string retVal = "";
            string RejectionComments = "";
            string TicketSummary = "";
            TicketSystemService tcktSrvs = new TicketSystemService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Clear();
            criteria.Add("TicketId", ticketId);
            //if (!string.IsNullOrEmpty(comments)) { criteria.Add("IsRejectionOrResolution", comments); } 
            if (comments == "Resolution") { if (!string.IsNullOrEmpty(comments)) { criteria.Add("IsRejectionOrResolution", comments); } }
            if (!string.IsNullOrEmpty(userId)) { criteria.Add("CommentedBy", userId); }
            if (!string.IsNullOrEmpty(activityName)) { criteria.Add("ActivityName", activityName); }
            string sord = "Desc";
            sord = sord == "desc" ? "Desc" : "Asc";
            Dictionary<long, IList<TicketComments>> objTcktCmnts = tcktSrvs.GetTicketCommentsListWithPaging(0, 10, string.Empty, sord, criteria);
            int count = objTcktCmnts.FirstOrDefault().Value.Count();
            RejectionComments = RejectionComments + TicketSummary;
            if (objTcktCmnts != null && objTcktCmnts.FirstOrDefault().Value.Count > 0 && objTcktCmnts.FirstOrDefault().Key > 0)
            {
                retVal = objTcktCmnts.FirstOrDefault().Value[count - 1].RejectionComments;
            }
            return retVal;
        }
        //public string CommentSummary(long ticketId, string userId, string activityName, string comments, string TktNo)
        public string CommentSummary(long ticketId, string userId, string activityName, string comments, string TktNo)
        {
            string retVal = "";
            string RejectionComments = "";
            string TicketSummary = "";
            TicketSystemService tcktSrvs = new TicketSystemService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Clear();
            criteria.Add("TicketNo", TktNo);
            Dictionary<long, IList<TicketSystem>> TktSystmList = tcktSrvs.GetTicketSystemBCListWithPagingAndCriteria(0, 10, string.Empty, string.Empty, criteria);
            if (TktSystmList != null && TktSystmList.FirstOrDefault().Value.Count > 0 && TktSystmList.FirstOrDefault().Key > 0)
            {
                TicketSummary = TktSystmList.FirstOrDefault().Value[0].Summary;
            }
            //TicketSummary = "<br/>Ticket Summary: <br/><b><i>" + TicketSummary + "</i></b>";
            criteria.Clear();
            criteria.Add("TicketId", ticketId);
            Dictionary<long, IList<TicketComments>> objTcktCmnts = tcktSrvs.GetTicketCommentsListWithPaging(0, 10, string.Empty, string.Empty, criteria);
            int count = objTcktCmnts.FirstOrDefault().Value.Count();
            if (count > 1)
            {
                TicketSummary = "<br/><br/>Ticket Summary: <br/><b><i>" + TicketSummary + "</i></b><br/><br/> Ticket History:";
            }
            else
            {
                TicketSummary = "<br/><br/>Ticket Summary: <br/><b><i>" + TicketSummary + "</i></b>";
            }
            //RejectionComments = RejectionComments + TicketSummary;
            if (objTcktCmnts != null && objTcktCmnts.FirstOrDefault().Value.Count > 0 && objTcktCmnts.FirstOrDefault().Key > 0)
            {
                //if (count == 1)
                //{
                //    for (int i = 0; i < count; i++)
                //    {
                //        RejectionComments = RejectionComments + objTcktCmnts.FirstOrDefault().Value[i].CommentedBy + " Gives " + objTcktCmnts.FirstOrDefault().Value[i].IsRejectionOrResolution + " Comment. Comment is <b><i>\"" + objTcktCmnts.FirstOrDefault().Value[i].RejectionComments + "\"</i></b><br/>";
                //    }
                //}
                if (count > 1)
                {
                    for (int i = 0; i < count - 1; i++)
                    {
                        //RejectionComments = RejectionComments + objTcktCmnts.FirstOrDefault().Value[i].CommentedBy + " Gives " + objTcktCmnts.FirstOrDefault().Value[i].IsRejectionOrResolution + " Comment. Comment is <b><i>\"" + objTcktCmnts.FirstOrDefault().Value[i].RejectionComments + "\"</i></b><br/>";

                        // RejectionComments = RejectionComments + 
                        //objTcktCmnts.FirstOrDefault().Value[i].CommentedBy + " Gives " + 
                        //objTcktCmnts.FirstOrDefault().Value[i].IsRejectionOrResolution + " Comment. Comment is <b><i>\"" + 
                        //objTcktCmnts.FirstOrDefault().Value[i].RejectionComments + "\"</i></b><br/>";
                        RejectionComments = RejectionComments +
                            objTcktCmnts.FirstOrDefault().Value[i].IsRejectionOrResolution + " Comment : <br/> &nbsp;&nbsp;&nbsp;&nbsp;"
                            + objTcktCmnts.FirstOrDefault().Value[i].RejectionComments + " - By " + objTcktCmnts.FirstOrDefault().Value[i].CommentedBy + "<br/>";
                    }
                }
            }
            retVal = TicketSummary + "<br/>" + RejectionComments;
            return retVal;
        }

        public bool SendEmailWithEmailTemplate(MailMessage mail, string Campus, string GeneralBody)
        {
            IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
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

        public bool SendStudentRegistrationMail(StudentTemplate st, string RecipientMail, string Campus, string Subject, string Body, string MailBody, string RecipientInfo, string SendMailType, Attachment MailAttachment)
        {
            try
            {
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                BaseController bse = new BaseController();
                IList<CampusEmailId> campusemaildet = bse.GetEmailIdByCampus(Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                if (SendMailType == "Management")
                {
                    string HeadADMNMail = ConfigurationManager.AppSettings["HeadAddmissionMail"].ToString();
                    IList<CampusAdminEmailId> campusadmindet = bse.GetEmailIdByCampusAdmin(Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                    mail.To.Add(campusadmindet.First().EmailId);
                    mail.CC.Add(HeadADMNMail);
                }
                else
                {
                    if (SendMailType == "Head-Admission")
                        mail.To.Add(RecipientMail);
                    else
                        mail.To.Add(campusemaildet.First().EmailId);
                }
                mail.Subject = Subject; // st.Subject;
                MailBody = MailBody.Replace("{{CampusmailId}}", campusemaildet.First().EmailId);
                MailBody = MailBody.Replace("{{Ph.No}}", campusemaildet.First().PhoneNumber);
                MailBody = MailBody.Replace("{{DateTime}}", DateTime.Now.ToString());
                MailBody = MailBody.Replace("{{Recipients}}", RecipientInfo);
                MailBody = MailBody.Replace("{{Content}}", Body);
                MailBody = MailBody.Replace("{{FbLink}}", campusemaildet.First().FBLink);
                mail.Body = MailBody;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("localhost", 25);
                smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address  
                //Or your Smtp Email ID and Password  
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = true;
                if (SendMailType == "Parent")
                {
                    if (!string.IsNullOrEmpty(RecipientMail))
                    {
                        mail.Bcc.Add(RecipientMail);
                        if (MailAttachment != null)
                        {
                            mail.Attachments.Add(MailAttachment);
                        }
                    }
                }
                mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                smtp.Credentials = new System.Net.NetworkCredential
               (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                {
                    try
                    {
                        new Task(() => { smtp.Send(mail); }).Start();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("quota"))
                        {
                            mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                            smtp.Credentials = new System.Net.NetworkCredential(campusemaildet.First().AlternateEmailId.ToString(), campusemaildet.First().AlternateEmailIdPassword.ToString());
                            new Task(() => { smtp.Send(mail); }).Start();
                            return true;
                        }
                        else
                        {
                            mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                            smtp.Credentials = new System.Net.NetworkCredential(campusemaildet.First().AlternateEmailId, campusemaildet.First().AlternateEmailIdPassword);
                            new Task(() => { smtp.Send(mail); }).Start();
                            return true;
                        }
                    }
                }
                else { return false; }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public bool SendStudentEnquiryMail(string Grade, string RecipientMail, string Campus, string Subject, string Body, string MailBody, string RecipientInfo, string SendMailType)
        {
            try
            {
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                BaseController bse = new BaseController();
                IList<CampusEmailId> campusemaildet = bse.GetEmailIdByCampus(Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                if (SendMailType == "Management")
                {
                    IList<CampusAdminEmailId> campusadmindet = bse.GetEmailIdByCampusAdmin(Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                    mail.To.Add(campusadmindet.First().EmailId);
                }
                else
                {
                    mail.To.Add(campusemaildet.First().EmailId);
                }
                //mail.To.Add(campusemaildet.First().EmailId);
                mail.Subject = Subject; // st.Subject;
                MailBody = MailBody.Replace("{{CampusmailId}}", campusemaildet.First().EmailId);
                MailBody = MailBody.Replace("{{Ph.No}}", campusemaildet.First().PhoneNumber);
                MailBody = MailBody.Replace("{{DateTime}}", DateTime.Now.ToString());
                MailBody = MailBody.Replace("{{Recipients}}", RecipientInfo);
                MailBody = MailBody.Replace("{{Content}}", Body);
                MailBody = MailBody.Replace("{{FbLink}}", campusemaildet.First().FBLink == null ? "#" : campusemaildet.First().FBLink);
                mail.Body = MailBody;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("localhost", 25);
                smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address  
                //Or your Smtp Email ID and Password  
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = true;
                if (SendMailType == "Parent")
                {
                    if (!string.IsNullOrEmpty(RecipientMail))
                    {
                        if (IsValidEmailAddress(RecipientMail))
                        {
                            mail.Bcc.Add(RecipientMail);
                        }
                    }
                }
                mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                smtp.Credentials = new System.Net.NetworkCredential
               (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                {
                    try
                    {
                        new Task(() => { smtp.Send(mail); }).Start();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("quota"))
                        {
                            mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                            smtp.Credentials = new System.Net.NetworkCredential(campusemaildet.First().AlternateEmailId.ToString(), campusemaildet.First().AlternateEmailIdPassword.ToString());
                            new Task(() => { smtp.Send(mail); }).Start();
                            return true;
                        }
                        else
                        {
                            mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                            smtp.Credentials = new System.Net.NetworkCredential(campusemaildet.First().AlternateEmailId, campusemaildet.First().AlternateEmailIdPassword);
                            new Task(() => { smtp.Send(mail); }).Start();
                            return true;
                        }
                    }
                }
                else { return false; }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        /// <summary>
        /// Determines whether an email address is valid.
        /// </summary>
        /// <param name="emailAddress">The email address to validate.</param>
        /// <returns>
        /// <c>true</c> if the email address is valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidEmailAddress(string emailAddress)
        {
            // An empty or null string is not valid
            if (String.IsNullOrEmpty(emailAddress))
            {
                return (false);
            }

            // Regular expression to match valid email address
            string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            // Match the email address using a regular expression
            Regex re = new Regex(emailRegex);
            if (re.IsMatch(emailAddress))
                return (true);
            else
                return (false);
        }

        public bool SendMailToRecipient(MailMessage mail, string Campus, string MailBody, string RecipientInfo, Attachment MailAttachment)
        {
            try
            {
                //System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                BaseController bse = new BaseController();
                IList<CampusEmailId> campusemaildet = bse.GetEmailIdByCampus(Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                //mail.Subject = Subject; // st.Subject;
                ///Complesury add these line 
                MailBody = MailBody.Replace("{{CampusmailId}}", campusemaildet.First().EmailId);
                MailBody = MailBody.Replace("{{Ph.No}}", campusemaildet.First().PhoneNumber);
                MailBody = MailBody.Replace("{{DateTime}}", DateTime.Now.ToString());
                MailBody = MailBody.Replace("{{Recipients}}", RecipientInfo);
                MailBody = MailBody.Replace("{{Content}}", mail.Body);
                MailBody = MailBody.Replace("{{FbLink}}", campusemaildet.First().FBLink);
                mail.Body = MailBody;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("localhost", 25);
                smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address  
                //Or your Smtp Email ID and Password  
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = true;

                if (MailAttachment != null)
                    mail.Attachments.Add(MailAttachment);

                mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                smtp.Credentials = new System.Net.NetworkCredential
               (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                {
                    try
                    {
                        new Task(() => { smtp.Send(mail); }).Start();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("quota"))
                        {
                            mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                            smtp.Credentials = new System.Net.NetworkCredential(campusemaildet.First().AlternateEmailId.ToString(), campusemaildet.First().AlternateEmailIdPassword.ToString());
                            new Task(() => { smtp.Send(mail); }).Start();
                            return true;
                        }
                        else
                        {
                            mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                            smtp.Credentials = new System.Net.NetworkCredential(campusemaildet.First().AlternateEmailId, campusemaildet.First().AlternateEmailIdPassword);
                            new Task(() => { smtp.Send(mail); }).Start();
                            return true;
                        }
                    }
                }
                else { return false; }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        //public bool SendCredentialToMail(User user)
        //{
        //    criteria.Clear();
        //    //if (!string.IsNullOrWhiteSpace(user.OrganizationCode)) { criteria.Add("OrganizationCode", user.OrganizationCode); }
        //    Dictionary<long, IList<SupportEmailId>> EmailIdDetails = Msvc.GetSupportEmailIdListWithPaging(0, 1000, string.Empty, string.Empty, criteria);
        //    string MailId = EmailIdDetails.FirstOrDefault().Value[0].EmailId;
        //    string MailIdPassword = EmailIdDetails.FirstOrDefault().Value[0].Password;
        //    MailMessage mail = new MailMessage();
        //    mail.From = new MailAddress(MailId);
        //    mail.To.Add(user.EmailId);
        //    mail.Subject = "HDMS Password";
        //    string RandomText = user.RandomNumber.ToString();
        //    mail.Body = "Dear " + user.UserName + ",\n \n" +
        //        "\tCongratulations on registering in Support Dest Management. Now you can access your Support Desk account online at anytime and any where \n" +
        //        "Check your account details, This your User login crenditial.\n\nUsername: " + user.UserId + "\nPassword: " + user.Password + "\nPIN :" + RandomText +
        //        "\n\nFor your security, we recommend that you change your password after you first log in.\n" +
        //         "Please feel free to contact us for any further clarifications or assistance via email at xcdsys.com or call us 0000-000-0000 from your mobile phone.\n\n" +
        //         "Assuring you of our best services at all times.\n\n" +
        //         "Warm Regards,\nTeam SUPPORT DESK TEAM." +
        //         "\n\nThis is an automated message. Please do not reply.";
        //    SmtpClient smtp = new SmtpClient("localhost", 25);
        //    smtp.Host = "smtp.gmail.com";
        //    smtp.EnableSsl = true;
        //    smtp.Credentials = new System.Net.NetworkCredential(MailId, MailIdPassword);
        //    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    smtp.Send(mail);
        //    return true;
        //}
        //public string GetBodyOfMailNotification(string Subject, string message)
        //{
        //    string messageBody = "";
        //    messageBody = messageBody + "<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /><style type='text/css'>";
        //    messageBody = messageBody + "body{margin: 0 0 0 0;padding: 0 0 0 0 !important;background-color: #ffffff !important;font-size: 10pt;font-family: 'Lucida Grande' ,Verdana,Arial,sans-serif;line-height: 14px;color: #303030;}";
        //    messageBody = messageBody + "table td{border-collapse: collapse;}td{margin: 0;}td img{display: block;}a{color: #865827;text-decoration: underline;}a:hover{text-decoration: underline;color: #865827;}a img{text-decoration: none;}";
        //    messageBody = messageBody + "a:visited{color: #865827;}a:active{color: #865827;}h1{font-size: 18pt;color: #865827;line-height: 20px;}h3{font-size: 14pt;color: #865827;line-height: 20px;}h4{font-size: 10pt;color: #58585a;}";
        //    messageBody = messageBody + "p{font-size: 10pt;}</style></head><body><table width='600' border='0' cellpadding='5' cellspacing='0' style='margin: auto;'><tr><td style='padding-bottom: 15px;' align='left'><a href='http://xcdsys.com'>";
        //    messageBody = messageBody + "<img src='../../Images/xcd-logo.png' />' alt='Exceed Systems' border='0'></a></td><td style='padding-bottom: 15px; font-size: 10px; color: #777;' align='right'>Technical Support is available from <strong>8am - 8pm (HDMS) Mon-Fri</strong><br />";
        //    messageBody = messageBody + "Toll free: <strong>0000000</strong> email: <strong>helpdeskmanagementsystems@gmail.com</strong></td></tr><tr><td colspan='2' align='center' style='border-top: solid 3px #eee; padding-top: 15px;'></td></tr></table>";
        //    messageBody = messageBody + "<table width='600' border='0' cellpadding='5' cellspacing='0' style='margin: auto;'><tr><td style='padding-bottom: 25px;'>" + message + "</td></tr><tr><td>Warm Regards,<br />Team SUPPORT DESK TEAM.</td></tr></table><table width='600' border='0' cellpadding='5' cellspacing='0' style='margin: auto;'><tr>";
        //    messageBody = messageBody + "<td colspan='2' align='center' style='padding-top: 7px; padding-bottom: 7px; border-top: 3px solid #777;border-bottom: 1px dotted #777;'><p style='font-size: 12px; line-height: 14px; color: #777;'>This is an automated message. Please do not reply.</p>";
        //    messageBody = messageBody + "</td></tr></table></body></html>";
        //    return messageBody;
        //}

    }
}