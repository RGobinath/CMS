using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TIPS.Entities;
using TIPS.Entities.AdmissionEntities;
using TIPS.Entities.Attendance;
using TIPS.Service;
using TIPS.ServiceContract;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Helpers;

namespace CMS.Controllers
{
    public class AttendanceController : BaseController
    {
        #region "Object Declaration"
        AttendanceService attServObj = new AttendanceService();
        MastersService msObj = new MastersService();
        IFormatProvider providerObj = new System.Globalization.CultureInfo("en-CA", true);
        #endregion "End"

        #region "Holidays"

        public ActionResult Holidays()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    DateTime dttime = DateTime.Now;
                    ViewBag.acadyrddl = GetAcademicYear();
                    ViewBag.dattime = dttime.ToString("dd/MM/yyyy");
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
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw ex;
            }
        }
        public ActionResult Delete(string[] Id)
        {
            try
            {
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    int i;
                    string[] arrayId = Id[0].Split(',');
                    for (i = 0; i < arrayId.Length; i++)
                    {
                        var singleId = arrayId[i];
                        Holidays holiday = attServObj.GetHolidaysById(Convert.ToInt64(singleId));
                        attServObj.DeleteHolidaysList(holiday);
                    }
                    var script = @"SucessMsg(""Deleted  Sucessfully"");";
                    return JavaScript(script);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "");
                throw ex;
            }
        }
        public ActionResult AddandListHolidaysJqGrid(string Date, string Holiday, string Comments, string Campus, string Grade, string Academicyear, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(Date))
                {
                    var date = convertDateMnthToMonthDate(Date);
                    criteria.Add("Holiday", date);
                }
                if (!string.IsNullOrEmpty(Holiday))
                {
                    var holiyday = convertDateMnthToMonthDate(Holiday);
                    criteria.Add("Holiday", holiyday);
                }
                if (!string.IsNullOrWhiteSpace(Comments)) { criteria.Add("Comments", Comments); }
                if (!string.IsNullOrEmpty(Campus))
                {
                    var campusArr = Campus.Split(',');

                    if (!string.IsNullOrWhiteSpace(Campus)) { criteria.Add("Campus", campusArr); }
                }
                if (!string.IsNullOrEmpty(Grade))
                {
                    var gradeArr = Grade.Split(',');
                    if (!string.IsNullOrWhiteSpace(Grade)) { criteria.Add("Grade", gradeArr); }
                }
                //if (!string.IsNullOrWhiteSpace(Campus)) { criteria.Add("Campus", Campus); }
                //if (!string.IsNullOrWhiteSpace(Grade)) { criteria.Add("Grade", Grade); }
                if (!string.IsNullOrWhiteSpace(Academicyear)) { criteria.Add("AcademicYear", Academicyear); }
                Dictionary<long, IList<Holidays>> HolidaysList = attServObj.GetHolidaysListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (HolidaysList != null && HolidaysList.Count > 0 && HolidaysList.First().Key > 0)
                {
                    long totalrecords = HolidaysList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in HolidaysList.First().Value
                                select new
                                {
                                    cell = new string[] {
                                    items.Id.ToString(),
                                    items.Holiday.ToString("dd/MM/yyyy"),
                                    items.Comments,
                                    items.Campus,
                                    items.Grade,
                                    items.AcademicYear
                                 }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);

                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult Holidays(string fromdate, string todate, string Comments, string Campus, string Grade, string Academicyear)
        {
            try
            {
                string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Holidays hd = new Holidays();
                if (!string.IsNullOrEmpty(todate) && !string.IsNullOrEmpty(fromdate))
                {
                    var StartingDate = convertDateMnthToMonthDate(fromdate);
                    var EndingDate = convertDateMnthToMonthDate(todate);
                    var campusArr = Campus.Split(',');
                    campusArr = campusArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    var gradeArr = Grade.Split(',');
                    gradeArr = gradeArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    foreach (var campusItem in campusArr)
                    {
                        foreach (var gradeItem in gradeArr)
                        {
                            foreach (DateTime date in GetDateRange(StartingDate, EndingDate))
                            {
                                if (!string.IsNullOrEmpty(campusItem) || !string.IsNullOrEmpty(gradeItem))
                                {
                                    hd.Holiday = date;
                                    hd.Name = userid;
                                    hd.Comments = Comments;
                                    hd.Campus = campusItem;
                                    hd.Grade = gradeItem;
                                    hd.AcademicYear = Academicyear;
                                    if (date.DayOfWeek.ToString() != "Sunday")
                                    {
                                        attServObj.CreateOrUpdateHolyDays(hd);
                                    }

                                }
                            }
                        }
                    }
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw ex;
            }
        }
        public ActionResult FindAbsList(string PreRegNum, string date)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(date))
            {
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                var datevalue = DateTime.Parse(date, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                criteria.Add("PreRegNum", Convert.ToInt64(PreRegNum));
                criteria.Add("AbsentDate", datevalue);
            }
            Dictionary<long, IList<Attendance>> AttendanceList = attServObj.GetStudentTemplateForAnAttendanceListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
            if (AttendanceList != null && AttendanceList.Count > 0 && AttendanceList.First().Key > 0)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            return Json(date, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveHodlidaysEditRecords(Holidays daysObj)
        {
            daysObj.Name = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
            attServObj.SaveOrUpdateHolyDays(daysObj);
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        #endregion "End"

        #region "Attendance"

        public ActionResult Attendance()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    DateTime dttime = DateTime.Now;
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = msObj.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    ViewBag.acadddl = AcademicyrMaster.First().Value;
                    ViewBag.dattime = dttime.ToString("dd/MM/yyyy");
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
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw ex;
            }
        }        
        public ActionResult DisableDateinDatepicker(string Campus, string grade)
        {
            DateTime DateNow = DateTime.Now;
            string Academicyear = string.Empty;
            if (grade == "IX")
                Academicyear = DateNow.Year.ToString() + "-" + (DateNow.Year + 1).ToString();
            else
            {
                //Academicyear = (DateNow.Year - 1).ToString() + "-" + DateNow.Year.ToString();
                if (DateNow.Month >= 5)
                {
                    Academicyear = DateNow.Year.ToString() + "-" + (DateNow.Year + 1).ToString();
                }
                else
                {
                    Academicyear = (DateNow.Year - 1).ToString() + "-" + DateNow.Year.ToString();
                }
            }
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                string[] camarr = new string[2];
                camarr[0] = Campus;
                camarr[1] = "ALL";
                criteria.Add("Campus", camarr);
                criteria.Add("Grade", grade);
                criteria.Add("AcademicYear", Academicyear);
                Dictionary<long, IList<Holidays>> HolidaysList = attServObj.GetHolidaysListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                if (HolidaysList != null && HolidaysList.Count > 0 && HolidaysList.First().Key > 0)
                {
                    List<Holidays> HDays = HolidaysList.FirstOrDefault().Value.ToList();
                    IEnumerable<string> hday = (from p in HDays
                                                select p.Holiday.ToString("d/M/yyyy"));
                    string[] Hdate = hday.ToArray();
                    return Json(Hdate, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw ex;
            }
        }
        public ActionResult GetAttendanceViewJqGrid(string campus, string grade, string section, string date, string ExportToXL, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                if (string.IsNullOrEmpty(campus) && string.IsNullOrEmpty(grade) && (string.IsNullOrEmpty(section) || (section == "Select")))
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    DateTime DateNow = DateTime.Now;
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    var Datevalue = DateTime.Parse(date, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    if (!string.IsNullOrWhiteSpace(campus)) { criteria.Add("Campus", campus); }
                    if (!string.IsNullOrWhiteSpace(grade)) { criteria.Add("Grade", grade); }
                    if (!string.IsNullOrWhiteSpace(section)) { criteria.Add("Section", section); }
                    criteria.Add("AdmissionStatus", "Registered");
                    if (grade == "IX" || grade == "X" || grade == "XI" || grade == "XII")
                    {
                        criteria.Add("AcademicYear", DateNow.Year.ToString() + "-" + (DateNow.Year + 1).ToString());
                    }
                    else
                    {
                        if (DateNow.Month >= 5)
                        {
                            criteria.Add("AcademicYear", DateNow.Year.ToString() + "-" + (DateNow.Year + 1).ToString());
                        }
                        else
                        {
                            criteria.Add("AcademicYear", (DateNow.Year - 1).ToString() + "-" + DateNow.Year.ToString());
                        }
                    }

                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<StudentAttendanceView>> StudentList = attServObj.GetStudentListListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    criteria.Clear();
                    criteria.Add("AbsentDate", Datevalue);
                    ///get the list from table student template for the search criteria--campus,grade,section --50
                    ///get the list from attendence for the same search criteria -- date --10
                    Dictionary<long, IList<Attendance>> AttendanceList = attServObj.GetStudentTemplateForAnAttendanceListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);

                    List<Attendance> alreadyExists = AttendanceList.FirstOrDefault().Value.ToList();
                    IEnumerable<long> blkLong = from p in alreadyExists
                                                orderby p.PreRegNum ascending
                                                select p.PreRegNum;
                    long[] attids = blkLong.ToArray();
                    foreach (StudentAttendanceView a in StudentList.FirstOrDefault().Value)
                    {
                        if (attids.Contains((a.PreRegNum)))
                        {
                            Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                            criteria1.Add("PreRegNum", a.PreRegNum);
                            criteria1.Add("AbsentDate", Datevalue);
                            Dictionary<long, IList<Attendance>> AbsentList = attServObj.GetAbsentListForAnAttendanceListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria1);
                            var AbsList = (from u in AbsentList.First().Value
                                           select new { u.Id, u.AbsentDate }).ToArray();
                            a.IsAbsent = true;
                            a.AbsentDate = AbsList[0].AbsentDate.Value.ToString("dd/MM/yyyy");
                            a.AttId = AbsList[0].Id.ToString();
                        }
                        else
                        {
                            a.IsAbsent = false;
                            a.AbsentDate = date;
                        }
                    }
                    if (StudentList != null && StudentList.Count > 0)
                    {
                        if (ExportToXL == "true" || ExportToXL == "True")
                        {

                            base.ExptToXL(StudentList.First().Value.ToList(), "AttendanceDetails", (items => new
                            {
                                items.PreRegNum,
                                items.NewId,
                                items.Name,
                                items.Campus,
                                items.Grade,
                                items.Section,
                                items.AbsentDate,
                                items.IsAbsent,
                            }));
                            return new EmptyResult();

                        }
                        else
                        {
                            long totalrecords = StudentList.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var jsondat = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,

                                rows = (from items in StudentList.First().Value
                                        select new
                                        {
                                            cell = new string[] {
                               items.PreRegNum.ToString(),
                               items.NewId,
                               items.Name,
                               items.Campus,
                               items.Grade,
                               items.Section,
                               items.AbsentDate,
                               items.IsAbsent!= true ?"<button type=\"button\" id=\"btnAbsent\" class=\"btn btn-danger\" name=\"" + items.Id + "\" >Mark Absent</button>":"<button type=\"button\" id=\"btnPresent\" class=\"btn btn-success\" name=\"" + items.Id + "\" >Mark present</button>",
                               items.AttId
                            }
                                        })
                            };
                            return Json(jsondat, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw ex;
            }
        }
        public ActionResult SaveAttendance(string PreRegNum, string campus, string Name, string Attid, string AbsentDate, string SelectedDate)
        {
            try
            {
                string ablist = "";
                DateTime DateNow = DateTime.Now;
                Attendance att = new Attendance();
                string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("UserId", userid);
                criteria.Add("AppCode", "ATT");
                Dictionary<long, IList<UserAppRole>> UserAppRoleList = attServObj.GetAppRoleForAnUserListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                var UserDetails = (from u in UserAppRoleList.First().Value
                                   select new { u.RoleName }).ToList();

                if (PreRegNum != null && Attid == "")
                {
                    att.PreRegNum = Convert.ToInt64(PreRegNum);
                    att.Name = Name;
                    if (DateNow.Month >= 5)
                    {
                        att.AcademicYear = DateNow.Year.ToString() + "-" + (DateNow.Year + 1).ToString();
                    }
                    else { att.AcademicYear = (DateNow.Year - 1).ToString() + "-" + DateNow.Year.ToString(); }

                    att.IsAbsent = true;
                    if (AbsentDate == null || AbsentDate == "")
                    {
                        ablist = SelectedDate;
                        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                        var selectdate = DateTime.Parse(SelectedDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        att.AbsentDate = selectdate;
                    }
                    else
                    {
                        ablist = AbsentDate;
                        IFormatProvider absent = new System.Globalization.CultureInfo("en-CA", true);
                        var absentdate = DateTime.Parse(AbsentDate, absent, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        att.AbsentDate = Convert.ToDateTime(absentdate.ToString());
                    }
                    att.CreatedBy = userid;
                    att.ModifiedBy = userid;
                    att.UserRole = UserDetails.Count == 0 ? "" : UserDetails[0].RoleName;
                    att.EntryFrom = "ATT";
                    att.AttendanceType = "FullDay";
                    /////////////////////////
                    // When button is pressed more than one time
                    criteria.Clear();
                    criteria.Add("AcademicYear", att.AcademicYear);
                    criteria.Add("PreRegNum", att.PreRegNum);
                    criteria.Add("AbsentDate", att.AbsentDate);
                    Dictionary<long, IList<Attendance>> AttendanceList = attServObj.GetStudentTemplateForAnAttendanceListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                    if (AttendanceList.FirstOrDefault().Key == 0) { attServObj.CreateOrUpdateAttendanceList(att); }
                    else { }
                    ////////////////////////
                }
                if (Attid != "" && PreRegNum != null)
                {
                    ablist = SelectedDate;
                    Attendance attdel = attServObj.GetAttentanceById(Convert.ToInt64(Attid));
                    attServObj.DeleteAttendancevalue(attdel);
                }

                // the student absent information send to their parents email address
                string CheckTestOrlive = ConfigurationManager.AppSettings["SendEmail1"];

                // Here the check it's a live or test like true or false
                if (CheckTestOrlive != "false")
                {
                    string todaydate = DateTime.Now.ToString("dd/MM/yyyy");
                    try
                    {
                        SMS sms = new SMS();
                        AdmissionManagementService ads = new AdmissionManagementService();
                        IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                        string From = ConfigurationManager.AppSettings["From"];
                        string To = ConfigurationManager.AppSettings["To"];

                        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();

                        mail.Subject = "Attendance Information";
                        string Body = "";
                        if (PreRegNum != null && Attid == "")
                        {
                            if (todaydate == ablist)
                            {
                                Body = "Dear Parent,<br><br>This is to inform you that your child " + Name + " is absent on " + ablist + ".";
                            }
                            else
                            {
                                Body = "Dear Parent,<br><br>This is to inform you that your child " + Name + " is absent on " + ablist + ".";
                            }
                        }
                        else if (Attid != "" && PreRegNum != null)
                        {
                            if (todaydate == ablist)
                            {
                                Body = "Dear Parent,<br><br>This is to inform you that your child " + Name + "is Present on " + ablist + ". Sorry for the inconvenience.";
                            }
                            else
                            {
                                Body = "Dear Parent,<br><br>This is to inform you that your child " + Name + "is Present on " + ablist + ". Sorry for the inconvenience.";
                            }
                        }
                        string Str = "<html>";
                        Str += "<head>";
                        Str += "<title></title>";
                        Str += "</head>";
                        Str += "<body>";
                        Str += "<table border=0 width=95% cellpadding=0 cellspacing=0>";
                        Str += "<tr>";
                        Str += "<td>" + Body + "</td>";
                        Str += "</tr>";
                        Str += "</table>";
                        Str += "</body>";
                        Str += "</html>";
                        mail.Body = Str;
                        mail.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient("localhost", 25);
                        smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address  
                        //Or your Smtp Email ID and Password  
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.EnableSsl = true;
                        criteria.Clear();
                        criteria.Add("PreRegNum", Convert.ToInt64(PreRegNum));
                        Dictionary<long, IList<StudentAttendanceView>> StudentList = attServObj.GetStudentListListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                        if (StudentList != null && StudentList.Count > 0 && StudentList.First().Key > 0)
                        {
                            EmailLog el = new EmailLog();
                            string sentmail = "sucess";

                            if (!string.IsNullOrEmpty(StudentList.First().Value[0].EmailId))
                            {
                                if (Regex.IsMatch(StudentList.First().Value[0].EmailId,
                           @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                           @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                           RegexOptions.IgnoreCase))
                                {
                                    mail.To.Add(StudentList.First().Value[0].EmailId);
                                }
                                if (From == "live")
                                {
                                    try
                                    {
                                        mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                                        smtp.Credentials = new System.Net.NetworkCredential
                                       (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                                        if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                        {
                                            new Task(() => { smtp.Send(mail); }).Start();
                                            el.IsSent = true;
                                        }
                                        else
                                            el.IsSent = false;
                                    }
                                    catch (Exception ex)
                                    {
                                        if (ex.Message.Contains("quota"))
                                        {
                                            mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                                            smtp.Credentials = new System.Net.NetworkCredential
                                            (campusemaildet.First().AlternateEmailId.ToString(), campusemaildet.First().AlternateEmailIdPassword.ToString());
                                            if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                            {
                                                new Task(() => { smtp.Send(mail); }).Start();
                                                el.IsSent = true;
                                            }
                                            else
                                                el.IsSent = false;
                                        }
                                        else
                                        {
                                            mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                                            smtp.Credentials = new System.Net.NetworkCredential
                                            (campusemaildet.First().AlternateEmailId.ToString(), campusemaildet.First().AlternateEmailIdPassword.ToString());
                                            if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                            {
                                                new Task(() => { smtp.Send(mail); }).Start();
                                                el.IsSent = true;
                                            }
                                            else
                                                el.IsSent = false;
                                        }
                                    }
                                }
                                if (From == "test")
                                {
                                    mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                                    smtp.Credentials = new System.Net.NetworkCredential
                                   (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                                    if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                                    {
                                        new Task(() => { smtp.Send(mail); }).Start();
                                        el.IsSent = true;
                                    }
                                    else
                                        el.IsSent = false;
                                }

                                el.Id = 0;
                                el.EmailFrom = campusemaildet.First().EmailId.ToString();
                                el.NewId = StudentList.First().Value[0].NewId;
                                el.StudName = StudentList.First().Value[0].Name;
                                el.EmailTo = StudentList.First().Value[0].EmailId;
                                try
                                {
                                    if (mail.Bcc.ToString().Length < 3990)
                                    {
                                        el.EmailBCC = campusemaildet.First().EmailId.ToString();
                                    }
                                    el.Subject = mail.Subject.ToString();
                                    if (mail.Body.ToString().Length < 3990)
                                    {
                                        el.Message = Body;
                                    }
                                    el.EmailDateTime = DateTime.Now.ToString();
                                    el.BCC_Count = mail.Bcc.Count;
                                    attServObj.CreateOrUpdateEmailLog(el);
                                }
                                catch (Exception ex)
                                {
                                    ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                                    throw ex;
                                }
                            }

                            // The Student absent information send their parents mobile Numbers
                            try
                            {
                                string recepientnos = "";
                                string failedrecepientnos = "";
                                string Message = "";
                                if (PreRegNum != null && Attid == "")
                                {
                                    Message = "Dear Parent this is to inform you that your child " + Name + " is absent on " + ablist + " .";
                                }
                                else if (Attid != "" && PreRegNum != null)
                                {
                                    Message = "Dear Parent this is to inform you that your child " + Name + " is present on " + ablist + " .Sorry for the inconvenience.";
                                }
                                criteria.Clear();
                                criteria.Add("PreRegNum", Convert.ToInt64(PreRegNum));
                                Dictionary<long, IList<FamilyDetails>> FamilyDetails = attServObj.GetFamilyDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);

                                if (FamilyDetails != null && FamilyDetails.Count > 0 && FamilyDetails.First().Key > 0)
                                {
                                    if (FamilyDetails.First().Value[0].Mobile != null)
                                    {
                                        StudentTemplate stud = ads.GetStudentDetailsByPreRegNum(FamilyDetails.First().Value[0].PreRegNum);
                                        if (stud != null) { sms.StudName = stud.Name; sms.NewId = stud.NewId; }
                                        if (Regex.IsMatch(FamilyDetails.First().Value[0].Mobile, @"(?<!\d)\d{10}(?!\d)", RegexOptions.IgnoreCase))
                                        {
                                            if (Regex.IsMatch(FamilyDetails.First().Value[0].Mobile, @"(?<!\d)\d{10}(?!\d)", RegexOptions.IgnoreCase))
                                            {
                                                if (recepientnos == "") { recepientnos = FamilyDetails.First().Value[0].Mobile.ToString(); }
                                                else
                                                { recepientnos = recepientnos + "," + FamilyDetails.First().Value[0].Mobile.ToString(); }
                                            }
                                            else
                                            {
                                                if (failedrecepientnos == "") { failedrecepientnos = FamilyDetails.First().Value[0].Mobile.ToString(); }
                                                else
                                                { failedrecepientnos = failedrecepientnos + "," + FamilyDetails.First().Value[0].Mobile.ToString(); }
                                            }
                                        }
                                    }
                                    string strUrl;
                                    string dataString = "";
                                    WebRequest request;
                                    WebResponse response;
                                    Stream s;
                                    StreamReader readStream;

                                    strUrl = ConfigurationManager.AppSettings["SMSService"].ToString() + "&senderID=TIPSGB&receipientno=" + recepientnos + "&dcs=0&msgtxt=" + Message + "&state=1";
                                    try
                                    {
                                        request = WebRequest.Create(strUrl);
                                        response = request.GetResponse();
                                        s = response.GetResponseStream();
                                        readStream = new StreamReader(s);
                                        dataString = readStream.ReadToEnd();
                                        response.Close();
                                        s.Close();
                                        readStream.Close();
                                        sms.SuccessSMSNos = recepientnos;
                                        sms.FailedSMSNos = failedrecepientnos;
                                        sms.CreatedDate = DateTime.Now.ToString();
                                        sms.Message = Message;
                                        sms.Flag = "success";
                                        sms.Status = dataString;
                                        attServObj.CreateOrUpdateSMSLog(sms);
                                    }
                                    catch (Exception)
                                    {
                                        sms.SuccessSMSNos = recepientnos;
                                        sms.FailedSMSNos = failedrecepientnos;
                                        sms.CreatedDate = DateTime.Now.ToString();
                                        sms.Message = Message;
                                        sms.Flag = "failed";
                                        sms.url = strUrl;
                                        sms.Status = dataString;
                                        attServObj.CreateOrUpdateSMSLog(sms);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                                throw ex;
                            }
                            return Json(sentmail, JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                        throw ex;
                    }
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw ex;
            }
        }

        #endregion "End"

        #region "Attendance Reports"

        public ActionResult AttendanceReports()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    DateTime DateNow = DateTime.Now;
                    string[] Academicyear = new string[3];
                    Academicyear[0] = "Select";
                    Academicyear[1] = (DateNow.Year - 1).ToString() + "-" + DateNow.Year.ToString();
                    Academicyear[2] = (DateNow.Year).ToString() + "-" + (DateNow.Year + 1).ToString();
                    ViewBag.acadddl = Academicyear;
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
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw ex;
            }
        }

        public ActionResult GetAttendanceReportsJqGrid(string campus, string grade, string section, string searchmonth, string AcademicYear, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                DateTime DateNow = DateTime.Now;
                int reduceCount = 0;
                int acaYear = 0;

                if (string.IsNullOrEmpty(campus) && string.IsNullOrEmpty(grade) && (string.IsNullOrEmpty(section) || (section == "Select")))
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                if ((grade == "IX" || grade == "X" || grade == "XI" || grade == "XII") && AcademicYear == (DateNow.Year - 1).ToString() + "-" + (DateNow.Year).ToString())
                {
                    var emptyJsonVal = new { rows = (new { cell = new string[] { } }) };
                    return Json(emptyJsonVal, JsonRequestBehavior.AllowGet);
                }
                if (Convert.ToInt32(searchmonth) == 5 || Convert.ToInt32(searchmonth) == 05)
                {
                    var emptyJsonVal = new { rows = (new { cell = new string[] { } }) };
                    return Json(emptyJsonVal, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // Swith case for getting academicyear based on month wise.
                    string[] year = AcademicYear.Split('-');
                    switch (Convert.ToInt32(searchmonth))
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            //acaYear = (DateNow.Year);
                            if (grade == "I" || grade == "II" || grade == "III" || grade == "IV" || grade == "V" || grade == "VI" || grade == "VII" || grade == "VIII" || grade.ToUpper() == "ALL")
                                acaYear = Convert.ToInt32(year[1]);
                            else
                                acaYear = Convert.ToInt32(year[0]);
                            break;
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                            acaYear = Convert.ToInt32(year[0]);
                            break;
                    }
                    // End

                    DateTime startDate = new DateTime(acaYear, Convert.ToInt32(searchmonth), 1);
                    DateTime endDate = new DateTime(acaYear, Convert.ToInt32(searchmonth), DateNow.Day);
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrWhiteSpace(campus)) { criteria.Add("Campus", campus); }

                    if (grade != null && grade != "All")
                    {
                        if (!string.IsNullOrWhiteSpace(grade)) { criteria.Add("Grade", grade); }
                    }

                    if (section != null && section != "All")
                    {
                        if (!string.IsNullOrWhiteSpace(section)) { criteria.Add("Section", section); }
                    }
                    criteria.Add("AdmissionStatus", "Registered");
                    sord = sord == "desc" ? "Desc" : "Asc";
                    Dictionary<long, IList<StudentAttendanceView>> StudentList = attServObj.GetStudentListListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    criteria.Clear();
                    criteria.Add("AcademicYear", AcademicYear);
                    Dictionary<long, IList<Attendance>> AttendanceList = attServObj.GetStudentTemplateForAnAttendanceListWithPagingAndCriteria(0, 999999, string.Empty, string.Empty, criteria);
                    List<Attendance> alreadyExists = AttendanceList.FirstOrDefault().Value.ToList();
                    IEnumerable<long> blkLong = from p in alreadyExists
                                                orderby p.PreRegNum ascending
                                                select p.PreRegNum;
                    long[] attids = blkLong.ToArray();
                    foreach (StudentAttendanceView a in StudentList.FirstOrDefault().Value)
                    {
                        // brought forward attendance function
                        var boughtforWardCount = broughtForwardAttendance(a.PreRegNum, Convert.ToInt32(searchmonth), campus, grade, AcademicYear);
                        a.BroughtForward = boughtforWardCount[0];
                        if (attids.Contains((a.PreRegNum)))
                        {
                            Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                            criteria1.Add("PreRegNum", a.PreRegNum);
                            if (!string.IsNullOrEmpty(searchmonth))
                            {
                                DateTime[] fromto = new DateTime[2];
                                fromto = GetLastAndFirstDateTimeinMonth(searchmonth, acaYear);
                                criteria1.Add("AbsentDate", fromto);
                            }
                            //criteria.Add("AttendanceType", "FullDay");
                            criteria1.Add("AcademicYear", AcademicYear);
                            Dictionary<long, IList<Attendance>> AbsentList = attServObj.GetAbsentListForAnAttendanceListWithPagingAndCriteria(0, 999999, string.Empty, string.Empty, criteria1);
                            a.AbsentCountList = AbsentList.First().Value.Count;
                            List<Attendance> Absentdate = AbsentList.FirstOrDefault().Value.ToList();
                            var AbsentLong = (from p in Absentdate
                                              where p.AttendanceType == "FullDay"
                                              orderby p.PreRegNum ascending
                                              select p).ToArray();
                            var halfAbsentLong = (from p in Absentdate
                                                  where p.AttendanceType == "HalfDay"
                                                  orderby p.PreRegNum ascending
                                                  select p).ToArray();
                            // Default Prest List
                            a.Date1 = "<b style='color:Green'>P</b>"; a.Date2 = "<b style='color:Green'>P</b>"; a.Date3 = "<b style='color:Green'>P</b>"; a.Date4 = "<b style='color:Green'>P</b>"; a.Date5 = "<b style='color:Green'>P</b>"; a.Date6 = "<b style='color:Green'>P</b>"; a.Date7 = "<b style='color:Green'>P</b>"; a.Date8 = "<b style='color:Green'>P</b>"; a.Date9 = "<b style='color:Green'>P</b>"; a.Date10 = "<b style='color:Green'>P</b>"; a.Date11 = "<b style='color:Green'>P</b>"; a.Date12 = "<b style='color:Green'>P</b>"; a.Date13 = "<b style='color:Green'>P</b>"; a.Date14 = "<b style='color:Green'>P</b>"; a.Date15 = "<b style='color:Green'>P</b>";
                            a.Date16 = "<b style='color:Green'>P</b>"; a.Date17 = "<b style='color:Green'>P</b>"; a.Date18 = "<b style='color:Green'>P</b>"; a.Date19 = "<b style='color:Green'>P</b>"; a.Date20 = "<b style='color:Green'>P</b>"; a.Date21 = "<b style='color:Green'>P</b>"; a.Date22 = "<b style='color:Green'>P</b>"; a.Date23 = "<b style='color:Green'>P</b>"; a.Date24 = "<b style='color:Green'>P</b>"; a.Date25 = "<b style='color:Green'>P</b>"; a.Date26 = "<b style='color:Green'>P</b>"; a.Date27 = "<b style='color:Green'>P</b>"; a.Date28 = "<b style='color:Green'>P</b>"; a.Date29 = "<b style='color:Green'>P</b>"; a.Date30 = "<b style='color:Green'>P</b>";
                            a.Date31 = "<b style='color:Green'>P</b>";
                            // hide default present using this condition only for current month

                            if (grade == "IX" && AcademicYear == (DateNow.Year - 1).ToString() + "-" + DateNow.Year.ToString())
                            { }
                            else
                            {
                                if (DateNow.Month == Convert.ToInt32(searchmonth))
                                {
                                    if (1 <= DateNow.Day) { a.Date1 = "<b style='color:Green'>P</b>"; } else { a.Date1 = ""; } if (2 <= DateNow.Day) { a.Date2 = "<b style='color:Green'>P</b>"; } else { a.Date2 = ""; } if (3 <= DateNow.Day) { a.Date3 = "<b style='color:Green'>P</b>"; } else { a.Date3 = ""; }
                                    if (4 <= DateNow.Day) { a.Date4 = "<b style='color:Green'>P</b>"; } else { a.Date4 = ""; } if (5 <= DateNow.Day) { a.Date5 = "<b style='color:Green'>P</b>"; } else { a.Date5 = ""; } if (6 <= DateNow.Day) { a.Date6 = "<b style='color:Green'>P</b>"; } else { a.Date6 = ""; }
                                    if (7 <= DateNow.Day) { a.Date7 = "<b style='color:Green'>P</b>"; } else { a.Date7 = ""; } if (8 <= DateNow.Day) { a.Date8 = "<b style='color:Green'>P</b>"; } else { a.Date8 = ""; } if (9 <= DateNow.Day) { a.Date9 = "<b style='color:Green'>P</b>"; } else { a.Date9 = ""; }
                                    if (10 <= DateNow.Day) { a.Date10 = "<b style='color:Green'>P</b>"; } else { a.Date10 = ""; } if (11 <= DateNow.Day) { a.Date11 = "<b style='color:Green'>P</b>"; } else { a.Date11 = ""; } if (12 <= DateNow.Day) { a.Date12 = "<b style='color:Green'>P</b>"; } else { a.Date12 = ""; }
                                    if (13 <= DateNow.Day) { a.Date13 = "<b style='color:Green'>P</b>"; } else { a.Date13 = ""; } if (14 <= DateNow.Day) { a.Date14 = "<b style='color:Green'>P</b>"; } else { a.Date14 = ""; } if (15 <= DateNow.Day) { a.Date15 = "<b style='color:Green'>P</b>"; } else { a.Date15 = ""; }
                                    if (16 <= DateNow.Day) { a.Date16 = "<b style='color:Green'>P</b>"; } else { a.Date16 = ""; } if (17 <= DateNow.Day) { a.Date17 = "<b style='color:Green'>P</b>"; } else { a.Date17 = ""; } if (18 <= DateNow.Day) { a.Date18 = "<b style='color:Green'>P</b>"; } else { a.Date18 = ""; }
                                    if (19 <= DateNow.Day) { a.Date19 = "<b style='color:Green'>P</b>"; } else { a.Date19 = ""; } if (20 <= DateNow.Day) { a.Date20 = "<b style='color:Green'>P</b>"; } else { a.Date20 = ""; } if (21 <= DateNow.Day) { a.Date21 = "<b style='color:Green'>P</b>"; } else { a.Date21 = ""; }
                                    if (22 <= DateNow.Day) { a.Date22 = "<b style='color:Green'>P</b>"; } else { a.Date22 = ""; } if (23 <= DateNow.Day) { a.Date23 = "<b style='color:Green'>P</b>"; } else { a.Date23 = ""; } if (24 <= DateNow.Day) { a.Date24 = "<b style='color:Green'>P</b>"; } else { a.Date24 = ""; }
                                    if (25 <= DateNow.Day) { a.Date25 = "<b style='color:Green'>P</b>"; } else { a.Date25 = ""; } if (26 <= DateNow.Day) { a.Date26 = "<b style='color:Green'>P</b>"; } else { a.Date26 = ""; } if (27 <= DateNow.Day) { a.Date27 = "<b style='color:Green'>P</b>"; } else { a.Date27 = ""; }
                                    if (28 <= DateNow.Day) { a.Date28 = "<b style='color:Green'>P</b>"; } else { a.Date28 = ""; } if (29 <= DateNow.Day) { a.Date29 = "<b style='color:Green'>P</b>"; } else { a.Date29 = ""; } if (30 <= DateNow.Day) { a.Date30 = "<b style='color:Green'>P</b>"; } else { a.Date30 = ""; }
                                    if (31 <= DateNow.Day) { a.Date31 = "<b style='color:Green'>P</b>"; } else { a.Date31 = ""; }
                                }
                            }
                            // Only for Absent List 
                            for (var i = 0; i < AbsentLong.Length; i++)
                            {
                                string Abvalue = AbsentLong[i].AbsentDate.Value.ToString("dd");
                                switch (Abvalue)
                                {
                                    case "1":
                                    case "01": { if (ExportType == "Excel")a.Date1 = "<b style='color:Red'>A</b>"; else { a.Date1 = "<b style='color:Red'>A</b>"; } } break;
                                    case "2":
                                    case "02": { if (ExportType == "Excel")a.Date2 = "<b style='color:Red'>A</b>"; else { a.Date2 = "<b style='color:Red'>A</b>"; } } break;
                                    case "3":
                                    case "03": { if (ExportType == "Excel")a.Date3 = "<b style='color:Red'>A</b>"; else { a.Date3 = "<b style='color:Red'>A</b>"; } } break;
                                    case "4":
                                    case "04": { if (ExportType == "Excel")a.Date4 = "<b style='color:Red'>A</b>"; else { a.Date4 = "<b style='color:Red'>A</b>"; } } break;
                                    case "5":
                                    case "05": { if (ExportType == "Excel")a.Date5 = "<b style='color:Red'>A</b>"; else { a.Date5 = "<b style='color:Red'>A</b>"; } } break;
                                    case "6":
                                    case "06": { if (ExportType == "Excel")a.Date6 = "<b style='color:Red'>A</b>"; else { a.Date6 = "<b style='color:Red'>A</b>"; } } break;
                                    case "7":
                                    case "07": { if (ExportType == "Excel")a.Date7 = "<b style='color:Red'>A</b>"; else { a.Date7 = "<b style='color:Red'>A</b>"; } } break;
                                    case "8":
                                    case "08": { if (ExportType == "Excel")a.Date8 = "<b style='color:Red'>A</b>"; else { a.Date8 = "<b style='color:Red'>A</b>"; } } break;
                                    case "9":
                                    case "09": { if (ExportType == "Excel")a.Date9 = "<b style='color:Red'>A</b>"; else { a.Date9 = "<b style='color:Red'>A</b>"; } } break;
                                    case "10": { if (ExportType == "Excel")a.Date10 = "<b style='color:Red'>A</b>"; else { a.Date10 = "<b style='color:Red'>A</b>"; } } break;
                                    case "11": { if (ExportType == "Excel")a.Date11 = "<b style='color:Red'>A</b>"; else { a.Date11 = "<b style='color:Red'>A</b>"; } } break;
                                    case "12": { if (ExportType == "Excel")a.Date12 = "<b style='color:Red'>A</b>"; else { a.Date12 = "<b style='color:Red'>A</b>"; } } break;
                                    case "13": { if (ExportType == "Excel")a.Date13 = "<b style='color:Red'>A</b>"; else { a.Date13 = "<b style='color:Red'>A</b>"; } } break;
                                    case "14": { if (ExportType == "Excel")a.Date14 = "<b style='color:Red'>A</b>"; else { a.Date14 = "<b style='color:Red'>A</b>"; } } break;
                                    case "15": { if (ExportType == "Excel")a.Date15 = "<b style='color:Red'>A</b>"; else { a.Date15 = "<b style='color:Red'>A</b>"; } } break;
                                    case "16": { if (ExportType == "Excel")a.Date16 = "<b style='color:Red'>A</b>"; else { a.Date16 = "<b style='color:Red'>A</b>"; } } break;
                                    case "17": { if (ExportType == "Excel")a.Date17 = "<b style='color:Red'>A</b>"; else { a.Date17 = "<b style='color:Red'>A</b>"; } } break;
                                    case "18": { if (ExportType == "Excel")a.Date18 = "<b style='color:Red'>A</b>"; else { a.Date18 = "<b style='color:Red'>A</b>"; } } break;
                                    case "19": { if (ExportType == "Excel")a.Date19 = "<b style='color:Red'>A</b>"; else { a.Date19 = "<b style='color:Red'>A</b>"; } } break;
                                    case "20": { if (ExportType == "Excel")a.Date20 = "<b style='color:Red'>A</b>"; else { a.Date20 = "<b style='color:Red'>A</b>"; } } break;
                                    case "21": { if (ExportType == "Excel")a.Date21 = "<b style='color:Red'>A</b>"; else { a.Date21 = "<b style='color:Red'>A</b>"; } } break;
                                    case "22": { if (ExportType == "Excel")a.Date22 = "<b style='color:Red'>A</b>"; else { a.Date22 = "<b style='color:Red'>A</b>"; } } break;
                                    case "23": { if (ExportType == "Excel")a.Date23 = "<b style='color:Red'>A</b>"; else { a.Date23 = "<b style='color:Red'>A</b>"; } } break;
                                    case "24": { if (ExportType == "Excel")a.Date24 = "<b style='color:Red'>A</b>"; else { a.Date24 = "<b style='color:Red'>A</b>"; } } break;
                                    case "25": { if (ExportType == "Excel")a.Date25 = "<b style='color:Red'>A</b>"; else { a.Date25 = "<b style='color:Red'>A</b>"; } } break;
                                    case "26": { if (ExportType == "Excel")a.Date26 = "<b style='color:Red'>A</b>"; else { a.Date26 = "<b style='color:Red'>A</b>"; } } break;
                                    case "27": { if (ExportType == "Excel")a.Date27 = "<b style='color:Red'>A</b>"; else { a.Date27 = "<b style='color:Red'>A</b>"; } } break;
                                    case "28": { if (ExportType == "Excel")a.Date28 = "<b style='color:Red'>A</b>"; else { a.Date28 = "<b style='color:Red'>A</b>"; } } break;
                                    case "29": { if (ExportType == "Excel")a.Date29 = "<b style='color:Red'>A</b>"; else { a.Date29 = "<b style='color:Red'>A</b>"; } } break;
                                    case "30": { if (ExportType == "Excel")a.Date30 = "<b style='color:Red'>A</b>"; else { a.Date30 = "<b style='color:Red'>A</b>"; } } break;
                                    case "31": { if (ExportType == "Excel")a.Date31 = "<b style='color:Red'>A</b>"; else { a.Date31 = "<b style='color:Red'>A</b>"; } } break;
                                    default: break;
                                }
                            }
                            /// Only for Half day absent
                            for (var j = 0; j < halfAbsentLong.Length; j++)
                            {
                                string Abvalue = halfAbsentLong[j].AbsentDate.Value.ToString("dd");
                                switch (Abvalue)
                                {
                                    case "1":
                                    case "01": { if (ExportType == "Excel")a.Date1 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date1 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "2":
                                    case "02": { if (ExportType == "Excel")a.Date2 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date2 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "3":
                                    case "03": { if (ExportType == "Excel")a.Date3 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date3 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "4":
                                    case "04": { if (ExportType == "Excel")a.Date4 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date4 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "5":
                                    case "05": { if (ExportType == "Excel")a.Date5 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date5 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "6":
                                    case "06": { if (ExportType == "Excel")a.Date6 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date6 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "7":
                                    case "07": { if (ExportType == "Excel")a.Date7 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date7 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "8":
                                    case "08": { if (ExportType == "Excel")a.Date8 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date8 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "9":
                                    case "09": { if (ExportType == "Excel")a.Date9 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date9 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "10": { if (ExportType == "Excel")a.Date10 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date10 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "11": { if (ExportType == "Excel")a.Date11 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date11 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "12": { if (ExportType == "Excel")a.Date12 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date12 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "13": { if (ExportType == "Excel")a.Date13 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date13 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "14": { if (ExportType == "Excel")a.Date14 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date14 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "15": { if (ExportType == "Excel")a.Date15 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date15 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "16": { if (ExportType == "Excel")a.Date16 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date16 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "17": { if (ExportType == "Excel")a.Date17 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date17 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "18": { if (ExportType == "Excel")a.Date18 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date18 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "19": { if (ExportType == "Excel")a.Date19 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date19 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "20": { if (ExportType == "Excel")a.Date20 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date20 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "21": { if (ExportType == "Excel")a.Date21 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date21 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "22": { if (ExportType == "Excel")a.Date22 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date22 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "23": { if (ExportType == "Excel")a.Date23 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date23 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "24": { if (ExportType == "Excel")a.Date24 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date24 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "25": { if (ExportType == "Excel")a.Date25 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date25 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "26": { if (ExportType == "Excel")a.Date26 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date26 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "27": { if (ExportType == "Excel")a.Date27 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date27 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "28": { if (ExportType == "Excel")a.Date28 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date28 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "29": { if (ExportType == "Excel")a.Date29 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date29 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "30": { if (ExportType == "Excel")a.Date30 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date30 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    case "31": { if (ExportType == "Excel")a.Date31 = "<b style='color:OrangeRed'>" + halfAbsentLong[j].AbsentCategory + "</b>"; else { a.Date31 = "<b style='color:OrangeRed '>" + halfAbsentLong[j].AbsentCategory + "</b>"; } } break;
                                    default: break;
                                }
                            }
                        }
                        else
                        {
                            a.Date1 = "<b style='color:Green'>P</b>"; a.Date2 = "<b style='color:Green'>P</b>"; a.Date3 = "<b style='color:Green'>P</b>"; a.Date4 = "<b style='color:Green'>P</b>"; a.Date5 = "<b style='color:Green'>P</b>"; a.Date6 = "<b style='color:Green'>P</b>"; a.Date7 = "<b style='color:Green'>P</b>"; a.Date8 = "<b style='color:Green'>P</b>"; a.Date9 = "<b style='color:Green'>P</b>"; a.Date10 = "<b style='color:Green'>P</b>"; a.Date11 = "<b style='color:Green'>P</b>"; a.Date12 = "<b style='color:Green'>P</b>"; a.Date13 = "<b style='color:Green'>P</b>"; a.Date14 = "<b style='color:Green'>P</b>"; a.Date15 = "<b style='color:Green'>P</b>";
                            a.Date16 = "<b style='color:Green'>P</b>"; a.Date17 = "<b style='color:Green'>P</b>"; a.Date18 = "<b style='color:Green'>P</b>"; a.Date19 = "<b style='color:Green'>P</b>"; a.Date20 = "<b style='color:Green'>P</b>"; a.Date21 = "<b style='color:Green'>P</b>"; a.Date22 = "<b style='color:Green'>P</b>"; a.Date23 = "<b style='color:Green'>P</b>"; a.Date24 = "<b style='color:Green'>P</b>"; a.Date25 = "<b style='color:Green'>P</b>"; a.Date26 = "<b style='color:Green'>P</b>"; a.Date27 = "<b style='color:Green'>P</b>"; a.Date28 = "<b style='color:Green'>P</b>"; a.Date29 = "<b style='color:Green'>P</b>"; a.Date30 = "<b style='color:Green'>P</b>"; a.Date31 = "<b style='color:Green'>P</b>";
                            if (grade == "IX" && AcademicYear == (DateNow.Year - 1).ToString() + "-" + DateNow.Year.ToString())
                            { }
                            else
                            {
                                if (DateNow.Month == Convert.ToInt32(searchmonth))
                                {
                                    // This month only
                                    if (1 <= DateNow.Day) { a.Date1 = "<b style='color:Green'>P</b>"; } else { a.Date1 = ""; } if (2 <= DateNow.Day) { a.Date2 = "<b style='color:Green'>P</b>"; } else { a.Date2 = ""; } if (3 <= DateNow.Day) { a.Date3 = "<b style='color:Green'>P</b>"; } else { a.Date3 = ""; }
                                    if (4 <= DateNow.Day) { a.Date4 = "<b style='color:Green'>P</b>"; } else { a.Date4 = ""; } if (5 <= DateNow.Day) { a.Date5 = "<b style='color:Green'>P</b>"; } else { a.Date5 = ""; } if (6 <= DateNow.Day) { a.Date6 = "<b style='color:Green'>P</b>"; } else { a.Date6 = ""; }
                                    if (7 <= DateNow.Day) { a.Date7 = "<b style='color:Green'>P</b>"; } else { a.Date7 = ""; } if (8 <= DateNow.Day) { a.Date8 = "<b style='color:Green'>P</b>"; } else { a.Date8 = ""; } if (9 <= DateNow.Day) { a.Date9 = "<b style='color:Green'>P</b>"; } else { a.Date9 = ""; }
                                    if (10 <= DateNow.Day) { a.Date10 = "<b style='color:Green'>P</b>"; } else { a.Date10 = ""; } if (11 <= DateNow.Day) { a.Date11 = "<b style='color:Green'>P</b>"; } else { a.Date11 = ""; } if (12 <= DateNow.Day) { a.Date12 = "<b style='color:Green'>P</b>"; } else { a.Date12 = ""; }
                                    if (13 <= DateNow.Day) { a.Date13 = "<b style='color:Green'>P</b>"; } else { a.Date13 = ""; } if (14 <= DateNow.Day) { a.Date14 = "<b style='color:Green'>P</b>"; } else { a.Date14 = ""; } if (15 <= DateNow.Day) { a.Date15 = "<b style='color:Green'>P</b>"; } else { a.Date15 = ""; }
                                    if (16 <= DateNow.Day) { a.Date16 = "<b style='color:Green'>P</b>"; } else { a.Date16 = ""; } if (17 <= DateNow.Day) { a.Date17 = "<b style='color:Green'>P</b>"; } else { a.Date17 = ""; } if (18 <= DateNow.Day) { a.Date18 = "<b style='color:Green'>P</b>"; } else { a.Date18 = ""; }
                                    if (19 <= DateNow.Day) { a.Date19 = "<b style='color:Green'>P</b>"; } else { a.Date19 = ""; } if (20 <= DateNow.Day) { a.Date20 = "<b style='color:Green'>P</b>"; } else { a.Date20 = ""; } if (21 <= DateNow.Day) { a.Date21 = "<b style='color:Green'>P</b>"; } else { a.Date21 = ""; }
                                    if (22 <= DateNow.Day) { a.Date22 = "<b style='color:Green'>P</b>"; } else { a.Date22 = ""; } if (23 <= DateNow.Day) { a.Date23 = "<b style='color:Green'>P</b>"; } else { a.Date23 = ""; } if (24 <= DateNow.Day) { a.Date24 = "<b style='color:Green'>P</b>"; } else { a.Date24 = ""; }
                                    if (25 <= DateNow.Day) { a.Date25 = "<b style='color:Green'>P</b>"; } else { a.Date25 = ""; } if (26 <= DateNow.Day) { a.Date26 = "<b style='color:Green'>P</b>"; } else { a.Date26 = ""; } if (27 <= DateNow.Day) { a.Date27 = "<b style='color:Green'>P</b>"; } else { a.Date27 = ""; }
                                    if (28 <= DateNow.Day) { a.Date28 = "<b style='color:Green'>P</b>"; } else { a.Date28 = ""; } if (29 <= DateNow.Day) { a.Date29 = "<b style='color:Green'>P</b>"; } else { a.Date29 = ""; } if (30 <= DateNow.Day) { a.Date30 = "<b style='color:Green'>P</b>"; } else { a.Date30 = ""; }
                                    if (31 <= DateNow.Day) { a.Date31 = "<b style='color:Green'>P</b>"; } else { a.Date31 = ""; }
                                }
                                else
                                {
                                    // not this month
                                    a.Date1 = "<b style='color:Green'>P</b>"; a.Date2 = "<b style='color:Green'>P</b>"; a.Date3 = "<b style='color:Green'>P</b>"; a.Date4 = "<b style='color:Green'>P</b>"; a.Date5 = "<b style='color:Green'>P</b>"; a.Date6 = "<b style='color:Green'>P</b>"; a.Date7 = "<b style='color:Green'>P</b>"; a.Date8 = "<b style='color:Green'>P</b>"; a.Date9 = "<b style='color:Green'>P</b>"; a.Date10 = "<b style='color:Green'>P</b>"; a.Date11 = "<b style='color:Green'>P</b>"; a.Date12 = "<b style='color:Green'>P</b>"; a.Date13 = "<b style='color:Green'>P</b>"; a.Date14 = "<b style='color:Green'>P</b>"; a.Date15 = "<b style='color:Green'>P</b>";
                                    a.Date16 = "<b style='color:Green'>P</b>"; a.Date17 = "<b style='color:Green'>P</b>"; a.Date18 = "<b style='color:Green'>P</b>"; a.Date19 = "<b style='color:Green'>P</b>"; a.Date20 = "<b style='color:Green'>P</b>"; a.Date21 = "<b style='color:Green'>P</b>"; a.Date22 = "<b style='color:Green'>P</b>"; a.Date23 = "<b style='color:Green'>P</b>"; a.Date24 = "<b style='color:Green'>P</b>"; a.Date25 = "<b style='color:Green'>P</b>"; a.Date26 = "<b style='color:Green'>P</b>"; a.Date27 = "<b style='color:Green'>P</b>"; a.Date28 = "<b style='color:Green'>P</b>"; a.Date29 = "<b style='color:Green'>P</b>"; a.Date30 = "<b style='color:Green'>P</b>"; a.Date31 = "<b style='color:Green'>P</b>";
                                }
                            }
                        }

                        // Find the Holy Day Leave List
                        List<string> value = getAllSundays(Convert.ToInt32(DateTime.Now.Year.ToString()));
                        var currentMnthHolidays = (from v in value where v.Substring(3, 2) == searchmonth select v);
                        int Hcount = 0;
                        foreach (string s in currentMnthHolidays)
                        {
                            //10/10/2013
                            switch (s.Substring(0, 2))
                            {
                                case "01": { if (ExportType == "Excel")a.Date1 = "H"; else { a.Date1 = "<b style='color:blue'>H</b>"; } } break;
                                case "02": { if (ExportType == "Excel")a.Date2 = "H"; else { a.Date2 = "<b style='color:blue'>H</b>"; } } break;
                                case "03": { if (ExportType == "Excel")a.Date3 = "H"; else { a.Date3 = "<b style='color:blue'>H</b>"; } } break;
                                case "04": { if (ExportType == "Excel")a.Date4 = "H"; else { a.Date4 = "<b style='color:blue'>H</b>"; } } break;
                                case "05": { if (ExportType == "Excel")a.Date5 = "H"; else { a.Date5 = "<b style='color:blue'>H</b>"; } } break;
                                case "06": { if (ExportType == "Excel")a.Date6 = "H"; else { a.Date6 = "<b style='color:blue'>H</b>"; } } break;
                                case "07": { if (ExportType == "Excel")a.Date7 = "H"; else { a.Date7 = "<b style='color:blue'>H</b>"; } } break;
                                case "08": { if (ExportType == "Excel")a.Date8 = "H"; else { a.Date8 = "<b style='color:blue'>H</b>"; } } break;
                                case "09": { if (ExportType == "Excel")a.Date9 = "H"; else { a.Date9 = "<b style='color:blue'>H</b>"; } } break;
                                case "10": { if (ExportType == "Excel")a.Date10 = "H"; else { a.Date10 = "<b style='color:blue'>H</b>"; } } break;
                                case "11": { if (ExportType == "Excel")a.Date11 = "H"; else { a.Date11 = "<b style='color:blue'>H</b>"; } } break;
                                case "12": { if (ExportType == "Excel")a.Date12 = "H"; else { a.Date12 = "<b style='color:blue'>H</b>"; } } break;
                                case "13": { if (ExportType == "Excel")a.Date13 = "H"; else { a.Date13 = "<b style='color:blue'>H</b>"; } } break;
                                case "14": { if (ExportType == "Excel")a.Date14 = "H"; else { a.Date14 = "<b style='color:blue'>H</b>"; } } break;
                                case "15": { if (ExportType == "Excel")a.Date15 = "H"; else { a.Date15 = "<b style='color:blue'>H</b>"; } } break;
                                case "16": { if (ExportType == "Excel")a.Date16 = "H"; else { a.Date16 = "<b style='color:blue'>H</b>"; } } break;
                                case "17": { if (ExportType == "Excel")a.Date17 = "H"; else { a.Date17 = "<b style='color:blue'>H</b>"; } } break;
                                case "18": { if (ExportType == "Excel")a.Date18 = "H"; else { a.Date18 = "<b style='color:blue'>H</b>"; } } break;
                                case "19": { if (ExportType == "Excel")a.Date19 = "H"; else { a.Date19 = "<b style='color:blue'>H</b>"; } } break;
                                case "20": { if (ExportType == "Excel")a.Date20 = "H"; else { a.Date20 = "<b style='color:blue'>H</b>"; } } break;
                                case "21": { if (ExportType == "Excel")a.Date21 = "H"; else { a.Date21 = "<b style='color:blue'>H</b>"; } } break;
                                case "22": { if (ExportType == "Excel")a.Date22 = "H"; else { a.Date22 = "<b style='color:blue'>H</b>"; } } break;
                                case "23": { if (ExportType == "Excel")a.Date23 = "H"; else { a.Date23 = "<b style='color:blue'>H</b>"; } } break;
                                case "24": { if (ExportType == "Excel")a.Date24 = "H"; else { a.Date24 = "<b style='color:blue'>H</b>"; } } break;
                                case "25": { if (ExportType == "Excel")a.Date25 = "H"; else { a.Date25 = "<b style='color:blue'>H</b>"; } } break;
                                case "26": { if (ExportType == "Excel")a.Date26 = "H"; else { a.Date26 = "<b style='color:blue'>H</b>"; } } break;
                                case "27": { if (ExportType == "Excel")a.Date27 = "H"; else { a.Date27 = "<b style='color:blue'>H</b>"; } } break;
                                case "28": { if (ExportType == "Excel")a.Date28 = "H"; else { a.Date28 = "<b style='color:blue'>H</b>"; } } break;
                                case "29": { if (ExportType == "Excel")a.Date29 = "H"; else { a.Date29 = "<b style='color:blue'>H</b>"; } } break;
                                case "30": { if (ExportType == "Excel")a.Date30 = "H"; else { a.Date30 = "<b style='color:blue'>H</b>"; } } break;
                                case "31": { if (ExportType == "Excel")a.Date31 = "H"; else { a.Date31 = "<b style='color:blue'>H</b>"; } } break;
                                default: break;
                            }
                            Hcount = Hcount + 1;
                        }
                        a.HolidayCountList = Hcount;


                        Dictionary<string, object> criteria3 = new Dictionary<string, object>();
                        //  Find the Special Leave list

                        string[] camArr = new string[2];
                        camArr[0] = campus;
                        camArr[1] = "ALL";
                        string[] grdArr = new string[2];
                        grdArr[0] = grade;
                        grdArr[1] = "ALL";
                        criteria3.Add("Campus", camArr);
                        if (grade != "All")
                        {
                            if (!string.IsNullOrEmpty(grade)) { criteria3.Add("Grade", grdArr); }
                        }
                        criteria3.Add("AcademicYear", AcademicYear);
                        Dictionary<long, IList<Holidays>> HolydaysList = attServObj.GetHolidaysListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria3);
                        if (HolydaysList != null && HolydaysList.Count > 0 && HolydaysList.First().Key > 0)
                        {
                            //List<Holidays> HDays = HolydaysList.FirstOrDefault().Value.ToList();
                            List<Holidays> HDays = (from u in HolydaysList.FirstOrDefault().Value
                                                    where u.Holiday <= DateTime.Now
                                                    select u).ToList();
                            IEnumerable<string> hday = from p in HDays
                                                       select p.Holiday.ToString("dd/MM/yyyy");
                            List<string> Hdate = hday.ToList();
                            var SpecialHoliday = (from v in Hdate where v.Substring(3, 2) == searchmonth select v);
                            int SHcount = 0;
                            foreach (string s in SpecialHoliday)
                            {
                                //10/10/2013
                                switch (s.Substring(0, 2))
                                {
                                    case "01": { if (ExportType == "Excel")a.Date1 = "L"; else { a.Date1 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "02": { if (ExportType == "Excel")a.Date2 = "L"; else { a.Date2 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "03": { if (ExportType == "Excel")a.Date3 = "L"; else { a.Date3 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "04": { if (ExportType == "Excel")a.Date4 = "L"; else { a.Date4 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "05": { if (ExportType == "Excel")a.Date5 = "L"; else { a.Date5 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "06": { if (ExportType == "Excel")a.Date6 = "L"; else { a.Date6 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "07": { if (ExportType == "Excel")a.Date7 = "L"; else { a.Date7 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "08": { if (ExportType == "Excel")a.Date8 = "L"; else { a.Date8 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "09": { if (ExportType == "Excel")a.Date9 = "L"; else { a.Date9 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "10": { if (ExportType == "Excel")a.Date10 = "L"; else { a.Date10 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "11": { if (ExportType == "Excel")a.Date11 = "L"; else { a.Date11 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "12": { if (ExportType == "Excel")a.Date12 = "L"; else { a.Date12 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "13": { if (ExportType == "Excel")a.Date13 = "L"; else { a.Date13 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "14": { if (ExportType == "Excel")a.Date14 = "L"; else { a.Date14 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "15": { if (ExportType == "Excel")a.Date15 = "L"; else { a.Date15 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "16": { if (ExportType == "Excel")a.Date16 = "L"; else { a.Date16 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "17": { if (ExportType == "Excel")a.Date17 = "L"; else { a.Date17 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "18": { if (ExportType == "Excel")a.Date18 = "L"; else { a.Date18 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "19": { if (ExportType == "Excel")a.Date19 = "L"; else { a.Date19 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "20": { if (ExportType == "Excel")a.Date20 = "L"; else { a.Date20 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "21": { if (ExportType == "Excel")a.Date21 = "L"; else { a.Date21 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "22": { if (ExportType == "Excel")a.Date22 = "L"; else { a.Date22 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "23": { if (ExportType == "Excel")a.Date23 = "L"; else { a.Date23 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "24": { if (ExportType == "Excel")a.Date24 = "L"; else { a.Date24 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "25": { if (ExportType == "Excel")a.Date25 = "L"; else { a.Date25 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "26": { if (ExportType == "Excel")a.Date26 = "L"; else { a.Date26 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "27": { if (ExportType == "Excel")a.Date27 = "L"; else { a.Date27 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "28": { if (ExportType == "Excel")a.Date28 = "L"; else { a.Date28 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "29": { if (ExportType == "Excel")a.Date29 = "L"; else { a.Date29 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "30": { if (ExportType == "Excel")a.Date30 = "L"; else { a.Date30 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    case "31": { if (ExportType == "Excel")a.Date31 = "L"; else { a.Date31 = "<b style='color:Fuchsia'>L</b>"; } } break;
                                    default: break;
                                }

                                if (Convert.ToInt32(searchmonth) == DateTime.Now.Month)
                                {
                                    if (Convert.ToInt32(s.Substring(0, 2)) == DateTime.Now.Day) { reduceCount = 1; }
                                    if (Convert.ToInt32(s.Substring(0, 2)) < DateTime.Now.Day) { SHcount = SHcount + 1; }
                                }
                                else
                                { SHcount = SHcount + 1; }
                            }
                            a.SpecialHolidayCountList = SHcount;
                        }
                        if (searchmonth == "01" || searchmonth == "03" || searchmonth == "05" || searchmonth == "07" || searchmonth == "08" || searchmonth == "10" || searchmonth == "12")
                        {
                            if (grade == "IX" && AcademicYear == (DateNow.Year - 1).ToString() + "-" + DateNow.Year.ToString())
                            {
                                a.noofpre = (31 - (a.AbsentCountList + (a.HolidayCountList + a.SpecialHolidayCountList)));
                                a.numofworkdays = (31 - (a.HolidayCountList + a.SpecialHolidayCountList));
                            }
                            else
                            {
                                if (DateNow.Month == Convert.ToInt32(searchmonth))
                                {
                                    a.noofpre = (DateNow.Day - (a.AbsentCountList + (CountDays(startDate, endDate) + a.SpecialHolidayCountList)));
                                    a.numofworkdays = (DateNow.Day - (CountDays(startDate, endDate) + a.SpecialHolidayCountList));
                                }
                                else
                                {
                                    a.noofpre = (31 - (a.AbsentCountList + (a.HolidayCountList + a.SpecialHolidayCountList)));
                                    a.numofworkdays = (31 - (a.HolidayCountList + a.SpecialHolidayCountList));
                                }
                            }

                            a.percround = (Convert.ToDecimal(Convert.ToDecimal(a.noofpre) / a.numofworkdays) * 100);
                            a.Percentage = (Math.Round(a.percround, 1));
                            // Total attendance
                            a.totalWorkingday = boughtforWardCount[1] + Convert.ToInt32(a.numofworkdays);
                            a.totalHoliday = a.totalWorkingday - (boughtforWardCount[2] + a.AbsentCountList);
                            a.totalAttendance = Convert.ToInt32(a.noofpre) + boughtforWardCount[0];//Modified By micheal
                            a.totalPercentage = (Convert.ToDouble(a.totalHoliday) / a.totalWorkingday) * 100;
                            a.totalPercentage = (Math.Round(a.totalPercentage, 1));
                            if (DateNow.Month == Convert.ToInt32(searchmonth)) { if (reduceCount == 1) { a.noofpre = a.noofpre - 1; } else { } }
                            //end
                        }
                        else if (searchmonth == "02")
                        {
                            if (grade == "IX" && AcademicYear == (DateNow.Year - 1).ToString() + "-" + DateNow.Year.ToString())
                            {
                                a.noofpre = (28 - (a.AbsentCountList + (a.HolidayCountList + a.SpecialHolidayCountList)));
                                a.numofworkdays = (28 - (a.HolidayCountList + a.SpecialHolidayCountList));
                            }
                            else
                            {
                                if (DateNow.Month == Convert.ToInt32(searchmonth))
                                {
                                    a.noofpre = (DateNow.Day - (a.AbsentCountList + (CountDays(startDate, endDate) + a.SpecialHolidayCountList)));
                                    a.numofworkdays = (DateNow.Day - (CountDays(startDate, endDate) + a.SpecialHolidayCountList));
                                }
                                else
                                {
                                    a.noofpre = (28 - (a.AbsentCountList + (a.HolidayCountList + a.SpecialHolidayCountList)));
                                    a.numofworkdays = (28 - (a.HolidayCountList + a.SpecialHolidayCountList));
                                }
                            }
                            a.noofpre = (28 - (a.AbsentCountList + (a.HolidayCountList + a.SpecialHolidayCountList)));
                            a.numofworkdays = (28 - (a.HolidayCountList + a.SpecialHolidayCountList));
                            a.percround = (Convert.ToDecimal(Convert.ToDecimal(a.noofpre) / a.numofworkdays) * 100);
                            a.Percentage = (Math.Round(a.percround, 1));
                            // Total attendance
                            a.totalWorkingday = boughtforWardCount[1] + Convert.ToInt32(a.numofworkdays);
                            a.totalHoliday = a.totalWorkingday - (boughtforWardCount[2] + a.AbsentCountList);
                            a.totalAttendance = Convert.ToInt32(a.noofpre) + boughtforWardCount[0];//Modified By micheal
                            a.totalPercentage = (Convert.ToDouble(a.totalHoliday) / a.totalWorkingday) * 100;
                            a.totalPercentage = (Math.Round(a.totalPercentage, 1));
                            if (DateNow.Month == Convert.ToInt32(searchmonth)) { if (reduceCount == 1) { a.noofpre = a.noofpre - 1; } else { } }
                            //end
                        }
                        else
                        {
                            if (grade == "IX" && AcademicYear == (DateNow.Year - 1).ToString() + "-" + DateNow.Year.ToString())
                            {
                                a.noofpre = (30 - (a.AbsentCountList + (a.HolidayCountList + a.SpecialHolidayCountList)));
                                a.numofworkdays = (30 - (a.HolidayCountList + a.SpecialHolidayCountList));
                            }
                            else
                            {
                                if (DateNow.Month == Convert.ToInt32(searchmonth))
                                {
                                    a.noofpre = (DateNow.Day - (a.AbsentCountList + (CountDays(startDate, endDate) + a.SpecialHolidayCountList)));
                                    a.numofworkdays = (DateNow.Day - (CountDays(startDate, endDate) + a.SpecialHolidayCountList));
                                }
                                else
                                {
                                    a.noofpre = (30 - (a.AbsentCountList + (a.HolidayCountList + a.SpecialHolidayCountList)));
                                    a.numofworkdays = (30 - (a.HolidayCountList + a.SpecialHolidayCountList));
                                }
                            }
                            a.percround = (Convert.ToDecimal(Convert.ToDecimal(a.noofpre) / a.numofworkdays) * 100);
                            a.Percentage = (Math.Round(a.percround, 1));
                            // Total attendance
                            a.totalWorkingday = boughtforWardCount[1] + Convert.ToInt32(a.numofworkdays);
                            a.totalHoliday = a.totalWorkingday - (boughtforWardCount[2] + a.AbsentCountList);
                            a.totalAttendance = Convert.ToInt32(a.noofpre) + boughtforWardCount[0];//Modified By micheal
                            a.totalPercentage = (Convert.ToDouble(a.totalHoliday) / a.totalWorkingday) * 100;
                            a.totalPercentage = (Math.Round(a.totalPercentage, 1));
                            if (DateNow.Month == Convert.ToInt32(searchmonth)) { if (reduceCount == 1) { a.noofpre = a.noofpre - 1; } else { } }
                        }
                    }
                    if (StudentList != null && StudentList.Count > 0)
                    {
                        if (ExportType == "Excel")
                        {
                            if (searchmonth == "01" || searchmonth == "03" || searchmonth == "05" || searchmonth == "07" || searchmonth == "08" || searchmonth == "10" || searchmonth == "12")
                            {
                                string headerTable = @"<Table border='1px' cellpadding='9' cellspacing='0'><tr><td colspan='2'>Campus - " + campus + "</td><td colspan='38' align='center' style='font-size: large;'>The Indian Public School</td></tr></b></Table>";
                                var studLst = StudentList.First().Value.ToList();

                                var List = (from s in studLst
                                            orderby s.Grade
                                            select s).ToList();
                                ExptToXL_FinalResult(List, "AttendanceReports", (items => new
                                {
                                    items.NewId,
                                    items.Name,
                                    items.Grade,
                                    items.Section,
                                    items.Date1,
                                    items.Date2,
                                    items.Date3,
                                    items.Date4,
                                    items.Date5,
                                    items.Date6,
                                    items.Date7,
                                    items.Date8,
                                    items.Date9,
                                    items.Date10,
                                    items.Date11,
                                    items.Date12,
                                    items.Date13,
                                    items.Date14,
                                    items.Date15,
                                    items.Date16,
                                    items.Date17,
                                    items.Date18,
                                    items.Date19,
                                    items.Date20,
                                    items.Date21,
                                    items.Date22,
                                    items.Date23,
                                    items.Date24,
                                    items.Date25,
                                    items.Date26,
                                    items.Date27,
                                    items.Date28,
                                    items.Date29,
                                    items.Date30,
                                    items.Date31,
                                    items.AbsentCountList,
                                    items.noofpre,
                                    items.Percentage,
                                    items.BroughtForward,
                                    items.totalAttendance,
                                    items.totalPercentage
                                }), headerTable);
                                return new EmptyResult();
                            }
                            else if (searchmonth == "02")
                            {
                                string headerTable = @"<Table border='1px' cellpadding='9' cellspacing='0'><tr><td colspan='2'>Campus - " + campus + "</td><td colspan='35' align='center' style='font-size: large;'>The Indian Public School</td></tr></b></Table>";
                                var studLst = StudentList.First().Value.ToList();

                                var List = (from s in studLst
                                            orderby s.Grade
                                            select s).ToList();
                                ExptToXL_FinalResult(List, "AttendanceReports", (items => new
                                {
                                    items.NewId,
                                    items.Name,
                                    items.Grade,
                                    items.Section,
                                    items.Date1,
                                    items.Date2,
                                    items.Date3,
                                    items.Date4,
                                    items.Date5,
                                    items.Date6,
                                    items.Date7,
                                    items.Date8,
                                    items.Date9,
                                    items.Date10,
                                    items.Date11,
                                    items.Date12,
                                    items.Date13,
                                    items.Date14,
                                    items.Date15,
                                    items.Date16,
                                    items.Date17,
                                    items.Date18,
                                    items.Date19,
                                    items.Date20,
                                    items.Date21,
                                    items.Date22,
                                    items.Date23,
                                    items.Date24,
                                    items.Date25,
                                    items.Date26,
                                    items.Date27,
                                    items.Date28,
                                    items.AbsentCountList,
                                    items.noofpre,
                                    items.Percentage,
                                    items.BroughtForward,
                                    items.totalAttendance,
                                    items.totalPercentage
                                }), headerTable);
                                return new EmptyResult();
                            }
                            else
                            {
                                string headerTable = @"<Table border='1px' cellpadding='9' cellspacing='0'><tr><td colspan='2'>Campus - " + campus + "</td><td colspan='38' align='center' style='font-size: large;'>The Indian Public School</td></tr></b></Table>";
                                var studLst = StudentList.First().Value.ToList();

                                var List = (from s in studLst
                                            orderby s.Grade
                                            select s).ToList();
                                ExptToXL_FinalResult(List, "AttendanceReports", (items => new
                                {
                                    items.NewId,
                                    items.Name,
                                    items.Grade,
                                    items.Section,
                                    items.Date1,
                                    items.Date2,
                                    items.Date3,
                                    items.Date4,
                                    items.Date5,
                                    items.Date6,
                                    items.Date7,
                                    items.Date8,
                                    items.Date9,
                                    items.Date10,
                                    items.Date11,
                                    items.Date12,
                                    items.Date13,
                                    items.Date14,
                                    items.Date15,
                                    items.Date16,
                                    items.Date17,
                                    items.Date18,
                                    items.Date19,
                                    items.Date20,
                                    items.Date21,
                                    items.Date22,
                                    items.Date23,
                                    items.Date24,
                                    items.Date25,
                                    items.Date26,
                                    items.Date27,
                                    items.Date28,
                                    items.Date29,
                                    items.Date30,
                                    items.AbsentCountList,
                                    items.noofpre,
                                    items.Percentage,
                                    items.BroughtForward,
                                    items.totalAttendance,
                                    items.totalPercentage
                                }), headerTable);
                                return new EmptyResult();
                            }


                        }
                        else
                        {
                            long totalrecords = StudentList.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var jsondat = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,

                                rows = (from items in StudentList.First().Value
                                        select new
                                        {
                                            i = items.Id,
                                            cell = new string[] {
                               items.PreRegNum.ToString(),
                               items.Name,
                               items.Date1,items.Date2,items.Date3,items.Date4,items.Date5,items.Date6,items.Date7,items.Date8,items.Date9,items.Date10,items.Date11,items.Date12,items.Date13,items.Date14,items.Date15,items.Date16,items.Date17,items.Date18,items.Date19,
                               items.Date20,items.Date21,items.Date22,items.Date23,items.Date24,items.Date25,items.Date26,items.Date27,items.Date28,items.Date29,items.Date30,items.Date31,items.AbsentCountList.ToString(),items.noofpre.ToString(),items.Percentage.ToString(),items.BroughtForward.ToString(),items.totalAttendance.ToString(),items.totalPercentage.ToString(),
                            }
                                        })
                            };
                            return Json(jsondat, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw ex;
            }
        }

        public Int32[] broughtForwardAttendance(long PreRegNum, int month, string campus, string grade, string academicYear)
        {
            Dictionary<string, object> attCriteria = new Dictionary<string, object>();
            DateTime DateNow = DateTime.Now;
            DateTime[] fromto = new DateTime[2];
            long[] AcademicYear = new long[2];
            Int32 year = 0;
            var splitAcayear = academicYear.Split('-');
            attCriteria.Add("PreRegNum", PreRegNum);
            attCriteria.Add("AcademicYear", academicYear);
            Dictionary<long, IList<Attendance>> AttendanceList = attServObj.GetStudentTemplateForAnAttendanceListWithPagingAndCriteria(0, 999999999, string.Empty, string.Empty, attCriteria);
            attCriteria.Clear();
            attCriteria.Add("Campus", campus);
            attCriteria.Add("Grade", grade);
            attCriteria.Add("AcademicYear", academicYear);
            Dictionary<long, IList<Holidays>> HolydaysListValue = attServObj.GetHolidaysListWithPagingAndCriteria(0, 999999, string.Empty, string.Empty, attCriteria);
            attCriteria.Clear();

            IList<Holidays> HolydaysList = (from u in HolydaysListValue.FirstOrDefault().Value
                                            where u.Holiday <= DateTime.Now
                                            select u).ToList();
            List<int> monthList = new List<int>();
            int countMonth = 0;
            int[] arrMonth = new int[] { 06, 07, 08, 09, 10, 11, 12, 01, 02, 03, 04, 05 };
            int broughtForward = 0, presentdays = 0, absentDayonly = 0;
            int[] precentandAbsent = new int[3];
            foreach (var item in arrMonth)
            {
                if (item == month) { break; } countMonth = item; monthList.Add(countMonth);
            }
            for (int i = 0; i < monthList.Count; i++)
            {
                if (monthList[i] >= 5) { year = Convert.ToInt32(splitAcayear[0]); } else { year = Convert.ToInt32(splitAcayear[1]); }
                int presentCount = GetNumberOfDaysMinusSundays(monthList[i], year);
                DateTime monthStartDate = new DateTime(year, monthList[i], 1);
                DateTime monthEndDate = new DateTime(year, monthList[i], DateTime.DaysInMonth(year, monthList[i]));
                fromto[0] = monthStartDate; fromto[1] = monthEndDate;
                int countAttendance = 0, countHolidays = 0;
                if (AttendanceList != null && AttendanceList.FirstOrDefault().Value.Count > 0 && AttendanceList.FirstOrDefault().Key > 0)
                {
                    var query = (from u in AttendanceList.FirstOrDefault().Value
                                 where u.AbsentDate >= fromto[0] && u.AbsentDate <= fromto[1] && u.PreRegNum == PreRegNum && u.AcademicYear == academicYear
                                 select u).ToList();
                    countAttendance = (query.Count != 0) ? query.Count : 0;
                }
                if (HolydaysList != null && HolydaysList.Count > 0)
                {
                    var holidayquery = (from u in HolydaysList
                                        where u.Holiday >= fromto[0] && u.Holiday <= fromto[1] && u.Campus == campus
                                        select u).ToList();
                    countHolidays = (holidayquery.Count != 0) ? holidayquery.Count : 0;
                }
                broughtForward = broughtForward + (presentCount - (countAttendance + countHolidays));
                presentdays = presentdays + (presentCount - countHolidays);
                absentDayonly = absentDayonly + countAttendance;
            }
            precentandAbsent[0] = broughtForward;
            precentandAbsent[1] = presentdays;
            precentandAbsent[2] = absentDayonly;
            return precentandAbsent;
        }
        public ActionResult GetMonthValbyAcademicYear(string academicYear, string grade)
        {
            try
            {
                DateTime DateNow = DateTime.Now;
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (string.IsNullOrEmpty(academicYear) || academicYear == "Select") { return null; }
                else
                {
                    if (grade == "IX" && academicYear == (DateNow.Year - 1).ToString() + "-" + DateNow.Year.ToString())
                    { }
                    else
                        criteria.Add("Code", DateNow.Month == 10 ? "T" : DateNow.Month == 11 ? "E" : DateNow.Month == 12 ? "W" : DateNow.Month.ToString());
                    //if (academicYear == DateNow.Year.ToString() + "-" + (DateNow.Year + 1).ToString())
                    //{
                    //    criteria.Add("Code", DateNow.Month == 10 ? "T" : DateNow.Month == 11 ? "E" : DateNow.Month == 12 ? "W" : DateNow.Month.ToString());
                    //}
                    Dictionary<long, IList<MonthMasterForAttendance>> monthMaster = attServObj.GetMonthMasterForAttendanceListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                    if (monthMaster != null && monthMaster.First().Key > 0 && monthMaster.First().Value.Count > 0)
                    {
                        var monthMasterLiset = (
                                 from items in monthMaster.First().Value
                                 select new
                                 {
                                     Text = items.MonthName,
                                     Value = items.MonthCode,
                                 }).Distinct().ToList();
                        return Json(monthMasterLiset, JsonRequestBehavior.AllowGet);
                    }
                }
                var jsondat = new { rows = (new { cell = new string[] { } }) };
                return Json(jsondat, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw ex;
            }
        }
        public void ExptToXL_FinalResult<T, TResult>(IList<T> stuList, string filename, Func<T, TResult> selector, string headerTable)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".xls");
            Response.ContentType = "application/vnd.ms-excel";
            System.IO.StringWriter stw = new System.IO.StringWriter();
            HtmlTextWriter htextw = new HtmlTextWriter(stw);
            DataGrid dg = new DataGrid();

            dg.HeaderStyle.BackColor = System.Drawing.Color.FromName("#B6B6B6");
            dg.HeaderStyle.Font.Bold = true;
            dg.HeaderStyle.ForeColor = System.Drawing.Color.White;

            dg.DataSource = stuList.Select(selector);
            dg.DataBind();
            dg.RenderControl(htextw);
            Response.Write(headerTable);
            // Response.Write(stw.ToString().Remove(stw.ToString().IndexOf("<tr "), (stw.ToString().IndexOf("</tr>") - stw.ToString().IndexOf("<tr ")) + 5));
            Response.Write(stw.ToString());
            Response.End();
        }

        #endregion "End"

        #region "Attendance Report Details"

        public ActionResult BulkSaveFunc(string preregnum, string name, string campus, string grade, string section, string fromdate, string todate)
        {
            try
            {
                DateTime DateNow = DateTime.Now;
                string userId = ValidateUser();
                bool checkFlag = false;
                string CheckTestOrlive = ConfigurationManager.AppSettings["SendEmail1"];
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Attendance att = new Attendance();
                    var StartingDate = DateTime.Parse(fromdate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    var EndingDate = DateTime.Parse(todate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    if (DateNow.Month >= 5)
                    {
                        att.AcademicYear = DateNow.Year.ToString() + "-" + (DateNow.Year + 1).ToString();
                    }
                    else { att.AcademicYear = (DateNow.Year - 1).ToString() + "-" + DateNow.Year.ToString(); }
                    criteria.Add("UserId", userId);
                    criteria.Add("AppCode", "ATT");
                    Dictionary<long, IList<UserAppRole>> UserAppRoleList = attServObj.GetAppRoleForAnUserListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    var UserDetails = (from u in UserAppRoleList.First().Value
                                       select new { u.RoleName }).ToList();
                    foreach (DateTime date in GetDateRange(StartingDate, EndingDate))
                    {
                        if (date.DayOfWeek.ToString() != "Sunday")
                        {

                            att.PreRegNum = Convert.ToInt64(preregnum);
                            att.Name = name;
                            att.CreatedBy = userId;
                            att.UserRole = UserDetails.Count == 0 ? "" : UserDetails[0].RoleName;
                            att.AbsentDate = date;
                            att.IsAbsent = true;
                            att.EntryFrom = "BAF";
                            criteria.Clear();
                            criteria.Add("AcademicYear", att.AcademicYear);
                            criteria.Add("PreRegNum", Convert.ToInt64(preregnum));
                            criteria.Add("AbsentDate", date);
                            Dictionary<long, IList<Attendance>> AttendanceList = attServObj.GetStudentTemplateForAnAttendanceListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                            if (AttendanceList.FirstOrDefault().Key == 0)
                            {
                                checkFlag = true;
                                attServObj.CreateAttendanceList(att);
                            }

                        }

                        else { }

                    }
                }
                if (checkFlag)
                {
                    SendBulkMailAndSMSAbsentToParent(preregnum, name, campus, fromdate, todate);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "");
                throw ex;
            }
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }
        private void SendBulkMailAndSMSAbsentToParent(string preregnum, string name, string campus, string srtDate, string endDate)
        {
            string RecipientInfo = "", Subject = "", Body = "", MailBody = ""; bool retValue = false;
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
            MailBody = GetGeneralBodyofMail();
            Subject = "Attendance Information";
            Body = "This is to inform you that your child " + name + " is absent from " + srtDate + " To " + endDate + ".";
            RecipientInfo = "Dear Parent";
            criteria.Clear();
            criteria.Add("PreRegNum", Convert.ToInt64(preregnum));
            Dictionary<long, IList<StudentAttendanceView>> StudentList = attServObj.GetStudentListListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
            if (StudentList != null && StudentList.Count > 0 && StudentList.First().Key > 0)
            {
                EmailLog el = new EmailLog();
                EmailHelper em = new EmailHelper();
                if (!string.IsNullOrEmpty(StudentList.First().Value[0].EmailId))
                {

                    if (ValidEmailOrNot(StudentList.First().Value[0].EmailId))
                    {
                        retValue = em.SendStudentRegistrationMail(null, StudentList.First().Value[0].EmailId, campus, Subject, Body, MailBody, RecipientInfo, "Parent", null);
                    }
                    el.Id = 0;
                    el.EmailFrom = campusemaildet.First().EmailId.ToString();
                    el.NewId = StudentList.First().Value[0].NewId;
                    el.StudName = StudentList.First().Value[0].Name;
                    el.EmailTo = campusemaildet.FirstOrDefault().EmailId;
                    el.EmailBCC = StudentList.First().Value[0].EmailId;
                    el.Subject = Subject;
                    el.IsSent = retValue == true ? true : false;
                    el.Message = Body;
                    el.EmailDateTime = DateTime.Now.ToString();
                    attServObj.CreateOrUpdateEmailLog(el);
                }
            }
            criteria.Clear();
            criteria.Add("PreRegNum", Convert.ToInt64(preregnum));
            Dictionary<long, IList<FamilyDetails>> FamilyDetails = attServObj.GetFamilyDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
            string mobileNum = string.Empty;
            if (FamilyDetails != null && FamilyDetails.Count > 0 && FamilyDetails.First().Key > 0)
            {
                mobileNum = FamilyDetails.First().Value[0].Mobile;
                if (!string.IsNullOrEmpty(mobileNum) && mobileNum.Length < 13)
                {
                    string smsMsg = "your child " + name + " is absent from " + srtDate + " To " + endDate;
                    SendAttendanceBulkAbsent(mobileNum, smsMsg);
                }
            }
        }
        private void SendAttendanceBulkAbsent(string mobileNum, string msg)
        {
            string strUrl; WebRequest request; WebResponse response; Stream s; StreamReader readStream; string dataString = string.Empty;
            if (Regex.IsMatch(mobileNum, @"(?<!\d)\d{10}(?!\d)", RegexOptions.IgnoreCase))
            {
                //Sending SMS

                string template = "Dear Parent, for your information " + msg + " . TIPS Team.";
                // template = template.Replace("MSG", msg);

                strUrl = ConfigurationManager.AppSettings["SMSService"].ToString() + "&senderID=TIPSGB&receipientno=" + mobileNum + "&dcs=0&msgtxt=" + template + "&state=1";
                try
                {
                    request = WebRequest.Create(strUrl);
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
                    ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                }
            }
        }

        #endregion "End"

        #region "Common Methods"

        public int CountDays(DateTime fromDate, DateTime toDate)
        {
            int noOfDays = 0;
            DateTime fDate = Convert.ToDateTime(fromDate);
            DateTime tDate = Convert.ToDateTime(toDate);
            while (DateTime.Compare(fDate, tDate) <= 0)
            {
                if (fDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    noOfDays += 1;
                }
                fDate = fDate.AddDays(1);
            }
            return noOfDays;
        }
        private DateTime convertDateMnthToMonthDate(string DateParam)
        {
            try
            {
                return DateTime.Parse(DateParam, providerObj, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<string> getAllSundays(int Year)
        {
            List<string> strDates = new List<string>();
            for (int month = 1; month <= 12; month++)
            {
                DateTime dt = new DateTime(Year, month, 1);
                int firstSundayOfMonth = (int)dt.DayOfWeek;
                if (firstSundayOfMonth != 0)
                {
                    dt = dt.AddDays((6 - firstSundayOfMonth) + 1);
                }
                while (dt.Month == month)
                {
                    strDates.Add(dt.ToString("dd/MM/yyyy"));
                    dt = dt.AddDays(7);
                }
            }
            return strDates;
        }
        private int GetNumberOfDaysMinusSundays(Int32 Month, Int32 Year)
        {
            //int month = DateTime.Today.Month;
            //int year = DateTime.Today.Year;
            int daysInMonthMinusSunday = 0;
            for (int i = 1; i <= DateTime.DaysInMonth(Year, Month); i++)
            {
                DateTime thisDay = new DateTime(Year, Month, i);
                if (thisDay.DayOfWeek != DayOfWeek.Sunday)
                {
                    daysInMonthMinusSunday += 1;
                }
            }
            return daysInMonthMinusSunday;
        }
        private List<DateTime> GetDateRange(DateTime StartingDate, DateTime EndingDate)
        {
            if (StartingDate > EndingDate)
            {
                return null;
            }
            List<DateTime> rv = new List<DateTime>();
            DateTime tmpDate = StartingDate;
            do
            {
                rv.Add(tmpDate);
                tmpDate = tmpDate.AddDays(1);
            } while (tmpDate <= EndingDate);
            return rv;
        }

        #endregion "End"

        public ActionResult HalfDayAbsent(string preregnum, string name, string campus, string grade, string section, string absdate, string FN)
        {
            try
            {
                DateTime DateNow = DateTime.Now;
                string userId = ValidateUser();
                bool checkFlag = false;
                string CheckTestOrlive = ConfigurationManager.AppSettings["SendEmail1"];
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Attendance att = new Attendance();
                    var StartingDate = DateTime.Parse(absdate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    // var EndingDate = DateTime.Parse(todate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    if (DateNow.Month >= 5)
                    {
                        att.AcademicYear = DateNow.Year.ToString() + "-" + (DateNow.Year + 1).ToString();
                    }
                    else { att.AcademicYear = (DateNow.Year - 1).ToString() + "-" + DateNow.Year.ToString(); }
                    criteria.Add("UserId", userId);
                    criteria.Add("AppCode", "ATT");
                    Dictionary<long, IList<UserAppRole>> UserAppRoleList = attServObj.GetAppRoleForAnUserListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                    var UserDetails = (from u in UserAppRoleList.First().Value
                                       select new { u.RoleName }).ToList();
                    att.PreRegNum = Convert.ToInt64(preregnum);
                    att.Name = name;
                    att.CreatedBy = userId;
                    att.UserRole = UserDetails.Count == 0 ? "" : UserDetails[0].RoleName;
                    att.AbsentDate = StartingDate;
                    att.AttendanceType = "Half Day";
                    att.AbsentCategory = FN;
                    att.IsAbsent = true;
                    att.EntryFrom = "BAF";
                    criteria.Clear();
                    criteria.Add("AcademicYear", att.AcademicYear);
                    criteria.Add("PreRegNum", Convert.ToInt64(preregnum));
                    criteria.Add("AbsentDate", StartingDate);
                    Dictionary<long, IList<Attendance>> AttendanceList = attServObj.GetStudentTemplateForAnAttendanceListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                    if (AttendanceList.FirstOrDefault().Key == 0)
                    {
                        checkFlag = true;
                        attServObj.CreateAttendanceList(att);
                    }
                }
                if (checkFlag)
                {
                    //SendBulkMailAndSMSAbsentToParent(preregnum, name, campus, fromdate, todate);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "");
                throw ex;
            }
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StudentAbsentlist(string stNewID)
        {
            try
            {
                AdmissionManagementService ams = new AdmissionManagementService();
                AttendanceService attsvc = new AttendanceService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                // var stNewID = userId.Substring(1);
                StudentTemplate st = ams.GetStudentDetailsByNewId(stNewID);
                criteria.Add("PreRegNum", st.PreRegNum);
                IList<Attendance> HoldayList = new List<Attendance>();
                Dictionary<long, IList<Attendance>> HolidaysList = attsvc.GetAbsentListForAnAttendanceListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                HoldayList = HolidaysList.FirstOrDefault().Value;

                criteria.Clear();
                string[] camarr = new string[2];
                camarr[0] = st.Campus;
                camarr[1] = "ALL";
                criteria.Add("Campus", camarr);
                IList<Holidays> SchleaveList = new List<Holidays>();
                Dictionary<long, IList<Holidays>> SchoolleaveList = attsvc.GetHolidaysListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                SchleaveList = SchoolleaveList.FirstOrDefault().Value;
                if (HolidaysList != null && HolidaysList.Count > 0 && SchoolleaveList != null && SchoolleaveList.Count > 0)
                {
                    var subjectMstrLst = new
                    {
                        rows = (
                             from items in HoldayList
                             select new
                             {
                                 items.AbsentDate,
                             }).Distinct().ToArray()
                    };
                    var HldyMstrLst = new
                    {
                        rows = (
                             from items in SchleaveList
                             select new
                             {
                                 items.Holiday,
                             }).Distinct().ToArray()
                    };
                    return Json(new { data = subjectMstrLst, item = HldyMstrLst }, "text/html", JsonRequestBehavior.AllowGet);
                }
                else
                { return Json(null, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, "Assess360Policy"); }
            finally { }
        }
        public ActionResult GetAttendanceListForStudentJqGrid(long PreRegNo, string grade, string searchtype, string ExportToXL, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                DateTime DateNow = DateTime.Now;
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                if (!string.IsNullOrEmpty(searchtype))
                {
                    DateTime[] datefromto = getFromTodate(searchtype);
                    criteria.Add("AbsentDate", datefromto);
                }
                criteria.Add("PreRegNum", PreRegNo);
                if (grade == "IX" || grade == "X" || grade == "XI" || grade == "XII")
                {
                    criteria.Add("AcademicYear", DateNow.Year.ToString() + "-" + (DateNow.Year + 1).ToString());
                }
                else
                {
                    if (DateNow.Month >= 5)
                    {
                        criteria.Add("AcademicYear", DateNow.Year.ToString() + "-" + (DateNow.Year + 1).ToString());
                    }
                    else
                    {
                        criteria.Add("AcademicYear", (DateNow.Year - 1).ToString() + "-" + DateNow.Year.ToString());
                    }
                }
                ///get the list from table Attendance for the search criteria--PreRegNo,grade --50
                Dictionary<long, IList<Attendance>> AttendanceList = attServObj.GetStudentTemplateForAnAttendanceListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (AttendanceList != null && AttendanceList.Count > 0 && AttendanceList.FirstOrDefault().Key > 0)
                {
                    if (ExportToXL == "true" || ExportToXL == "True")
                    {
                        base.ExptToXL(AttendanceList.First().Value.ToList(), "AttendanceDetails", (items => new
                        {
                            items.PreRegNum,
                            items.Name,
                            AbsendtDate = items.AbsentDate.Value.ToString("dd'/'MM'/'yyyy"),
                            AbsendtDay = items.AbsentDate.Value.ToString("ddd"),
                            AbsentMarkedBy = items.CreatedBy,
                        }));
                        return new EmptyResult();
                    }
                    else
                    {
                        long totalrecords = AttendanceList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in AttendanceList.First().Value
                                    select new
                                    {
                                        cell = new string[] {
                               items.PreRegNum.ToString(),
                               items.AbsentDate.Value.ToString("dd'/'MM'/'yyyy"),
                               items.AbsentDate.Value.ToString("ddd"),
                               items.CreatedBy,
                            }
                                    })
                        };
                        return Json(jsondat, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw ex;
            }
        }

    }
}
