using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersistenceFactory;
using TIPS.Entities.Attendance;
using TIPS.Entities;
using TIPS.Entities.AdmissionEntities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Web;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Configuration;

namespace TIPS.Component
{
    public class AttendanceBC
    {
        PersistenceServiceFactory PSF = null;
        public AttendanceBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }
        public Dictionary<long, IList<UserAppRole>> GetAppRoleForAnUserListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<UserAppRole>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StudentAttendanceView>> GetStudentListListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<StudentAttendanceView>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<Attendance>> GetStudentTemplateForAnAttendanceListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<Attendance>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<Attendance>> GetAbsentListForAnAttendanceListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<Attendance>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<Holidays>> GetHolidaysListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<Holidays>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateAttendanceList(Attendance att)
        {
            try
            {
                if (att != null)
                    PSF.SaveOrUpdate<Attendance>(att);
                else { throw new Exception("Value is required and it cannot be null.."); }
                return att.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Attendance GetAttentanceById(long Id)
        {
            try
            {
                Attendance attdel = null;
                if (Id > 0)
                    attdel = PSF.Get<Attendance>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return attdel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long DeleteAttendancevalue(Attendance att)
        {
            try
            {
                if (att != null)
                    PSF.Delete<Attendance>(att);
                else { throw new Exception("Value is required and it cannot be null.."); }
                return 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long CreateOrUpdateHolyDays(Holidays holy)
        {
            try
            {
                if (holy != null)
                    PSF.Save<Holidays>(holy);
                else { throw new Exception("Value is required and it cannot be null.."); }
                return holy.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateEmailLog(EmailLog el)
        {
            try
            {
                if (el != null)
                    PSF.SaveOrUpdate<EmailLog>(el);
                else { throw new Exception("Attendance is required and it cannot be null.."); }
                return el.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<FamilyDetails>> GetFamilyDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<FamilyDetails>> familydet = new Dictionary<long, IList<FamilyDetails>>();
                return PSF.GetListWithEQSearchCriteriaCount<FamilyDetails>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateSMSLog(SMS sms)
        {
            try
            {
                if (sms != null)
                    PSF.SaveOrUpdate<SMS>(sms);
                else { throw new Exception("Attendance is required and it cannot be null.."); }
                return sms.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Holidays GetHolidaysById(long Id)
        {
            try
            {
                Holidays attdel = null;
                if (Id > 0)
                    attdel = PSF.Get<Holidays>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return attdel;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long DeleteHolidaysList(Holidays hd)
        {
            try
            {
                if (hd != null)
                    PSF.Delete<Holidays>(hd);
                else { throw new Exception("Value is required and it cannot be null.."); }
                return 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<MonthMasterForAttendance>> GetMonthMasterForAttendanceListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<MonthMasterForAttendance>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region "Attendance Report Details"
        public long CreateOrUpdateAttendanceMonitor(AttendanceMonitor attMonObj)
        {
            try
            {
                if (attMonObj != null)
                    PSF.SaveOrUpdate<AttendanceMonitor>(attMonObj);
                else { throw new Exception("Value is required and it cannot be null.."); }
                return attMonObj.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<AttendanceMonitor>> GetAttendanceMonitorListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<AttendanceMonitor>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<EmailConfiguration>> GetEmailConfigurationListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<EmailConfiguration>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long SaveOrUpdateEmailConfiguration(EmailConfiguration mailObj)
        {
            try
            {
                if (mailObj != null)
                    PSF.SaveOrUpdate<EmailConfiguration>(mailObj);
                else { throw new Exception("Value is required and it cannot be null.."); }
                return mailObj.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public long DeleteEmailConfigurationList(string[] ids)
        {
            try
            {
                string[] arrayId = ids[0].Split(',');
                for (int i = 0; i < arrayId.Length; i++)
                {
                    var singleId = arrayId[i];
                    string query = "DELETE FROM EmailConfiguration WHERE Id='" + Convert.ToInt64(singleId) + "'";
                    PSF.ExecuteSql(query);
                }
                return 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void AttendanceFinishAndNotFinishServiceFunc()
        {
            try
            {
                string recipientInfo = string.Empty; string subject = string.Empty;
                string msg = string.Empty; string mailBody = string.Empty;
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                DateTime DateNow = DateTime.Now;
                criteria.Add("AcademicYear", (DateNow.Year.ToString() + "-" + (DateNow.Year + 1).ToString()));
                Dictionary<long, IList<EmailConfiguration>> mailConfig = PSF.GetListWithEQSearchCriteriaCount<EmailConfiguration>(0, 99999, string.Empty, string.Empty, criteria);
                if (mailConfig != null && mailConfig.FirstOrDefault().Value.Count > 0 && mailConfig.FirstOrDefault().Key > 0)
                {
                    List<EmailConfiguration> ovrallList = mailConfig.FirstOrDefault().Value.ToList();
                    var email_List = (from u in mailConfig.FirstOrDefault().Value
                                      select new { u.Email, u.Category }).Distinct().ToList();

                    if (mailConfig != null && mailConfig.FirstOrDefault().Value.Count > 0 && mailConfig.FirstOrDefault().Key > 0)
                    {

                        foreach (var item in email_List)
                        {
                            string cmps = string.Empty; string grd = string.Empty;
                            var cgs_List = (from u in mailConfig.FirstOrDefault().Value
                                            where u.Email == item.Email
                                            select new { u.Campus, u.Grade }).Distinct().ToList();

                            var cgsWithSec_List = (from u in mailConfig.FirstOrDefault().Value
                                                   where u.Email == item.Email
                                                   select new { u.Campus, u.Grade, u.Section }).Distinct().ToList();

                            recipientInfo = "Dear Team,<br/> Today Attendance Reports Details";
                            subject = "Daily Attendance Reports";
                            msg = "<table border=1px width=100%>";
                            msg = msg + "<tr><td style='font-weight: bold;'>S No</td><td style='font-weight: bold;'>Campus</td><td style='font-weight: bold;'>Grade</td><td style='font-weight: bold;'>Finished</td><td style='font-weight: bold;'>Not Finished</td></tr>";
                            int rolNum = 0;
                            foreach (var cgs in cgs_List)
                            {
                                string fnsh = string.Empty; string notFnsh = string.Empty;
                                var sec_arry = (from u in cgsWithSec_List
                                                where u.Campus == cgs.Campus && u.Grade == cgs.Grade
                                                select u.Section).ToArray();

                                foreach (var sec_Item in sec_arry)
                                {
                                    criteria.Clear();
                                    criteria.Add("Campus", cgs.Campus);
                                    criteria.Add("Grade", cgs.Grade);
                                    criteria.Add("Section", sec_Item);
                                    criteria.Add("Status", "Completed");
                                    Dictionary<long, IList<AttendanceFinishAndNotFinish_Vw>> atFinAndNF = PSF.GetListWithEQSearchCriteriaCount<AttendanceFinishAndNotFinish_Vw>(0, 9999, string.Empty, string.Empty, criteria);
                                    if (atFinAndNF != null && atFinAndNF.FirstOrDefault().Value.Count > 0 && atFinAndNF.FirstOrDefault().Key > 0)
                                    {
                                        fnsh += sec_Item + ",";
                                    }
                                    else
                                    {
                                        notFnsh += sec_Item + ",";
                                    }
                                    cmps = cgs.Campus; grd = cgs.Grade;

                                }
                                rolNum = rolNum + 1;
                                msg = msg + "<tr><td>" + rolNum + "</td><td>" + cmps + "</td><td>" + grd + "</td><td>" + fnsh + "</td><td>" + notFnsh + "</td></tr>";
                            }
                            msg = msg + "</table>";
                            mailBody = GetMailTemplateBody();
                            SendDialyMailFunc(cmps, grd, msg, mailBody, recipientInfo, subject, item.Email);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AttendancePresentAndAbsentServiceFunc()
        {
            try
            {
                string recipientInfo = string.Empty; string subject = string.Empty;
                string msg = string.Empty; string mailBody = string.Empty;
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                DateTime DateNow = DateTime.Now;
                criteria.Add("AcademicYear", (DateNow.Year.ToString() + "-" + (DateNow.Year + 1).ToString()));
                Dictionary<long, IList<EmailConfiguration>> mailConfig = PSF.GetListWithEQSearchCriteriaCount<EmailConfiguration>(0, 99999, string.Empty, string.Empty, criteria);
                if (mailConfig != null && mailConfig.FirstOrDefault().Value.Count > 0 && mailConfig.FirstOrDefault().Key > 0)
                {

                    List<EmailConfiguration> ovrallList = mailConfig.FirstOrDefault().Value.ToList();
                    var email_List = (from u in mailConfig.FirstOrDefault().Value
                                      select new { u.Email, u.Category }).Distinct().ToList();
                    if (mailConfig != null && mailConfig.FirstOrDefault().Value.Count > 0 && mailConfig.FirstOrDefault().Key > 0)
                    {

                        foreach (var item in email_List)
                        {
                            string cmps = string.Empty; string grd = string.Empty; string sec = string.Empty;
                            var cgs_List = (from u in mailConfig.FirstOrDefault().Value
                                            where u.Email == item.Email
                                            select new { u.Campus, u.Grade }).Distinct().ToList();

                            var cgsWithSec_List = (from u in mailConfig.FirstOrDefault().Value
                                                   where u.Email == item.Email
                                                   select new { u.Campus, u.Grade, u.Section }).Distinct().ToList();

                            recipientInfo = "Dear Team,<br/> Today Attendance Reports Details";
                            subject = "Daily Attendance Reports";
                            msg = "<table border=1px width=100%>";
                            msg = msg + "<tr><td style='font-weight: bold;'>S No</td><td style='font-weight: bold;'>Campus</td><td style='font-weight: bold;'>Grade</td><td style='font-weight: bold;'>Section</td><td style='font-weight: bold;'>Present</td><td style='font-weight: bold;'>Absent</td></tr>";
                            int rolNum = 0;
                            foreach (var cgs in cgs_List)
                            {
                                int studCount = 0; int prcCount = 0; int absCount = 0;
                                var sec_arry = (from u in cgsWithSec_List
                                                where u.Campus == cgs.Campus && u.Grade == cgs.Grade
                                                select u.Section).ToArray();

                                foreach (var sec_Item in sec_arry)
                                {

                                    criteria.Clear();
                                    criteria.Add("Campus", cgs.Campus);
                                    criteria.Add("Grade", cgs.Grade);
                                    criteria.Add("Section", sec_Item);
                                    criteria.Add("AdmissionStatus", "Registered");
                                    criteria.Add("AcademicYear", (DateNow.Year.ToString() + "-" + (DateNow.Year + 1).ToString()));
                                    Dictionary<long, IList<StudentTemplateView>> studList = PSF.GetListWithEQSearchCriteriaCount<StudentTemplateView>(0, 9999, string.Empty, string.Empty, criteria);
                                    if (studList != null && studList.FirstOrDefault().Value.Count > 0 && studList.FirstOrDefault().Key > 0)
                                    {
                                        studCount = studList.FirstOrDefault().Value.Count;
                                    }

                                    criteria.Clear();
                                    criteria.Add("Campus", cgs.Campus);
                                    criteria.Add("Grade", cgs.Grade);
                                    criteria.Add("Section", sec_Item);
                                    criteria.Add("Status_Flag", "Absent");
                                    Dictionary<long, IList<AttendanceDialyRpt_Vw>> overallList = PSF.GetListWithEQSearchCriteriaCount<AttendanceDialyRpt_Vw>(0, 9999, string.Empty, string.Empty, criteria);
                                    if (overallList != null && overallList.FirstOrDefault().Value.Count > 0 && overallList.FirstOrDefault().Key > 0)
                                    {
                                        absCount = overallList.FirstOrDefault().Value.Count;
                                    }
                                    prcCount = studCount - absCount;
                                    cmps = cgs.Campus; grd = cgs.Grade; sec = sec_Item;
                                    rolNum = rolNum + 1;
                                    msg = msg + "<tr><td>" + rolNum + "</td><td>" + cmps + "</td><td>" + grd + "</td><td>" + sec + "</td><td>" + prcCount + "</td><td>" + absCount + "</td></tr>";
                                }
                            }
                            msg = msg + "</table>";
                            mailBody = GetMailTemplateBody();
                            SendDialyMailFunc(cmps, grd, msg, mailBody, recipientInfo, subject, item.Email);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool SendDialyMailFunc(string campus, string grade, string msg, string mailBody, string recipientInfo, string subject, string toMail)
        {
            try
            {
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                string server = ConfigurationManager.AppSettings["CampusEmailType"].ToString();
                criteria.Add("Campus", campus);
                criteria.Add("Server", server);
                IList<CampusEmailId> campusemaildet = PSF.GetListWithSearchCriteria<CampusEmailId>(0, 99999, string.Empty, string.Empty, criteria);
                mail.To.Add(toMail);
                mail.Subject = subject; // st.Subject;
                mailBody = mailBody.Replace("{{CampusmailId}}", campusemaildet.First().EmailId);
                mailBody = mailBody.Replace("{{Ph.No}}", campusemaildet.First().PhoneNumber);
                mailBody = mailBody.Replace("{{DateTime}}", DateTime.Now.ToString());
                mailBody = mailBody.Replace("{{Recipients}}", recipientInfo);
                mailBody = mailBody.Replace("{{Content}}", msg);
                mail.Body = mailBody;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("localhost", 25);
                smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address  
                //Or your Smtp Email ID and Password  
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = true;
                mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                smtp.Credentials = new System.Net.NetworkCredential
               (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                {
                    try
                    {
                        new Task(() => { smtp.Send(mail); }).Start();
                        //smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("quota"))
                        {
                            mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                            smtp.Credentials = new System.Net.NetworkCredential(campusemaildet.First().AlternateEmailId.ToString(), campusemaildet.First().AlternateEmailIdPassword.ToString());
                            new Task(() => { smtp.Send(mail); }).Start();
                            //smtp.Send(mail);
                            return true;
                        }
                        else
                        {
                            mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                            smtp.Credentials = new System.Net.NetworkCredential(campusemaildet.First().AlternateEmailId, campusemaildet.First().AlternateEmailIdPassword);
                            new Task(() => { smtp.Send(mail); }).Start();
                            //smtp.Send(mail);
                            return true;
                        }
                    }
                }
                else { return false; }
            }
            catch (Exception ey)
            {
                ExceptionPolicy.HandleException(ey, "AdmissionPolicy");
                throw ey;
            }
        }
        private string GetMailTemplateBody()
        {
            string MessageBody = System.IO.File.ReadAllText(HttpContext.Current.Request.MapPath("~/Views/Shared/AdmissionEmailBody.html"));
            return MessageBody;
        }

        public long CreateAttendanceList(Attendance att)
        {
            try
            {
                if (att != null)
                    PSF.Save<Attendance>(att);
                else { throw new Exception("Value is required and it cannot be null.."); }
                return att.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion "End"

        public long SaveOrUpdateHolyDays(Holidays holy)
        {
            try
            {
                if (holy != null)
                    PSF.SaveOrUpdate<Holidays>(holy);
                else { throw new Exception("Value is required and it cannot be null.."); }
                return holy.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
