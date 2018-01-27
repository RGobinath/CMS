using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TIPS.Entities;
using TIPS.Entities.AdmissionEntities;
using TIPS.Entities.Assess;
using TIPS.Service;
using TIPS.ServiceContract;
using System.Globalization;
using TIPS.Component;
using TIPS.Entities.TransportEntities;
using System.Diagnostics;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using TIPS.Entities.StaffManagementEntities;
using TIPS.Entities.Attendance;
using TIPS.Entities.BioMetricsEntities;

namespace CMS.Controllers
{
    public class BaseController : Controller
    {

        private Stopwatch stopWatch = new Stopwatch();
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllerName = filterContext.RouteData.Values["controller"].ToString();
            string actionName = filterContext.RouteData.Values["action"].ToString();
            MastersService ms = new MastersService();
            UserService us = new UserService();
            Dictionary<long, IList<TIPS.Entities.MenuEntities.Menu>> MenuList = ms.GetMenuListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, null);
            if (MenuList != null && MenuList.FirstOrDefault().Value != null && MenuList.FirstOrDefault().Value.Count > 0)
            {
                var ActionArr = (from item in MenuList.FirstOrDefault().Value
                                 select item.Action).Distinct().ToArray();
                if (ActionArr.Count() > 0 && ActionArr.Contains(actionName) && actionName != "PageHistory" && Session["UserId"] != null)
                {
                    PageHistory ph = new PageHistory();
                    ph.UserId = Session["UserId"].ToString();
                    ph.SessionId = Session.SessionID;
                    ph.VisitedTime = DateTime.Now;
                    ph.Action = actionName;
                    ph.Controller = controllerName;
                    us.CreateOrUpdatePageHistory(ph);
                }
            }
            base.OnActionExecuting(filterContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            stopWatch.Stop();
            long executionTime = stopWatch.ElapsedMilliseconds;
            string controllerName = filterContext.RouteData.Values["controller"].ToString();
            string actionName = filterContext.RouteData.Values["action"].ToString();
            if (actionName != "PageHistory" && actionName != "KeepAlive" && Session["UserId"] != null)
            {
                UserService us = new UserService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("UserId", Session["UserId"]);
                criteria.Add("SessionId", Session.SessionID);
                criteria.Add("Action", actionName);
                criteria.Add("Controller", controllerName);
                Dictionary<long, IList<PageHistory>> PageHistory = us.GetPageHistoryWithLikePagingAndCriteria(0, 9999, "Id", "Desc", criteria);
                if (PageHistory != null && PageHistory.FirstOrDefault().Value != null && PageHistory.FirstOrDefault().Value.Count > 0)
                {
                    PageHistory ph = new PageHistory();
                    PageHistory.FirstOrDefault().Value[0].ExecutionTime = executionTime;
                    us.CreateOrUpdatePageHistory(PageHistory.FirstOrDefault().Value[0]);
                }
            }
            base.OnActionExecuted(filterContext);
        }

        //protected override void OnResultExecuted(ResultExecutedContext filterContext)
        //{
        //    stopWatch.Stop();
        //    var executionTime = stopWatch.ElapsedMilliseconds;
        //   // Log(filterContext.RouteData, stopWatch.ElapsedMilliseconds);
        //}

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
        }

        /// <summary>
        /// Return the exception message
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public JsonResult ThrowJSONError(Exception e)
        {
            Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            Response.TrySkipIisCustomErrors = true;
            //Log your exception
            return Json(new { Message = e.Message }, "text/x-json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }

        public void ExptToXL<T, TResult>(IList<T> stuList, string filename, Func<T, TResult> selector)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".xls");
            Response.ContentType = "application/vnd.ms-excel";
            System.IO.StringWriter stw = new System.IO.StringWriter();
            HtmlTextWriter htextw = new HtmlTextWriter(stw);
            DataGrid dg = new DataGrid();
            dg.HeaderStyle.BackColor = System.Drawing.Color.FromName("#5090C1");
            dg.HeaderStyle.Font.Bold = true;
            dg.HeaderStyle.ForeColor = System.Drawing.Color.White;
            dg.DataSource = stuList.Select(selector);
            dg.DataBind();
            dg.RenderControl(htextw);
            Response.Write(stw.ToString());
            Response.End();
        }

        public ActionResult SignOut()
        {
            Session.Abandon();
            Response.Cookies["ASP.NET_SessionID"].Value = null;
            return new EmptyResult();
        }
        public string ValidateUser()
        {
            string userId = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
            if (string.IsNullOrWhiteSpace(userId))
            {
                return "";
            }
            else return userId;
        }
        /// <summary>
        /// Return the exception message
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public JsonResult ThrowJSONErrorNew(Exception e, String exPolicyName)
        {
            Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            Response.TrySkipIisCustomErrors = true;
            //Log your exception
            ExceptionPolicy.HandleException(e, exPolicyName);
            return Json(new { Message = e.Message }, "text/x-json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }
        #region "Email methods"
        public string GetEmailIdForAStudent(long studentId, long PreRegNo, string ParentType)
        {
            AdmissionManagementService ads = new AdmissionManagementService();
            string retValue = "";
            Dictionary<string, object> criteria1 = new Dictionary<string, object>();

            switch (ParentType)
            {
                case "Father":
                    {
                        if (PreRegNo > 0) criteria1.Add("PreRegNum", PreRegNo);
                        criteria1.Add("FamilyDetailType", "Father");
                        break;
                    }
                case "Mother":
                    {
                        if (PreRegNo > 0) criteria1.Add("PreRegNum", PreRegNo);
                        criteria1.Add("FamilyDetailType", "Mother");
                        break;
                    }
            }
            //check this count method and use the list only. "REVISIT"
            Dictionary<long, IList<FamilyDetails>> FamilyDetails = ads.GetFamilyDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria1);
            if (FamilyDetails.First().Value.Count() != 0)
            {
                if ((FamilyDetails.First().Value[0].Email != null) && (FamilyDetails.First().Value[0].Email.Contains("@")))
                {
                    retValue = FamilyDetails.First().Value[0].Email;
                }
            }
            return retValue;
        }
        public string GetContentTypeByFileExtension(string FileExtension)
        {
            string ContentType = "";
            switch (FileExtension)
            {
                case ".bmp":
                    ContentType = "image/bmp";
                    break;
                case ".gif":
                    ContentType = "image/gif";
                    break;
                case ".jpeg":
                    ContentType = "image/jpeg";
                    break;
                case ".jpg":
                    ContentType = "image/jpeg";
                    break;
                case ".png":
                    ContentType = "image/png";
                    break;
                case ".tif":
                    ContentType = "image/tiff";
                    break;
                case ".tiff":
                    ContentType = "image/tiff";
                    break;
                //'Documents'
                case ".doc":
                    ContentType = "application/msword";
                    break;
                case ".docx":
                    ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case ".pdf":
                    ContentType = "application/pdf";
                    break;
                //'Slideshows'
                case ".ppt":
                    ContentType = "application/vnd.ms-powerpoint";
                    break;
                case ".pptx":
                    ContentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                    break;
                //'Data'
                case ".xlsx":
                    ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case ".xls":
                    ContentType = "application/vnd.ms-excel";
                    break;
                case ".csv":
                    ContentType = "text/csv";
                    break;
                case ".xml":
                    ContentType = "text/xml";
                    break;
                case ".txt":
                    ContentType = "text/plain";
                    break;
                //'Compressed Folders'
                case ".zip":
                    ContentType = "application/zip";
                    break;
                //'Audio'
                case ".ogg":
                    ContentType = "application/ogg";
                    break;
                case ".mp3":
                    ContentType = "audio/mpeg";
                    break;
                case ".wma":
                    ContentType = "audio/x-ms-wma";
                    break;
                case ".wav":
                    ContentType = "audio/x-wav";
                    break;
                //'Video'
                case ".wmv":
                    ContentType = "audio/x-ms-wmv";
                    break;
                case ".swf":
                    ContentType = "application/x-shockwave-flash";
                    break;
                case ".avi":
                    ContentType = "video/avi";
                    break;
                case ".mp4":
                    ContentType = "video/mp4";
                    break;
                case ".mpeg":
                    ContentType = "video/mpeg";
                    break;
                case ".mpg":
                    ContentType = "video/mpeg";
                    break;
                case ".qt":
                    ContentType = "video/quicktime";
                    break;
                default:
                    ContentType = "text/plain";
                    break;
            }
            return ContentType;
        }
        #endregion

        public DataTable ListToDataTable<T>(List<T> list)
        {
            DataTable dt = new DataTable();
            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                Type propType = info.PropertyType;
                if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propType = Nullable.GetUnderlyingType(propType);
                }
                dt.Columns.Add(new DataColumn(info.Name, propType));
            }
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    object colVal = info.GetValue(t, null);
                    if (colVal != null)
                    {
                        row[info.Name] = colVal.ToString();
                    }
                    else
                    {
                        row[info.Name] = DBNull.Value;
                    }
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
        public ActionResult FillBranchCode()
        {
            string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
            UserService us = new UserService();
            //pass userid and get the list of roles from user service GetAppRoleForAnUserListWithPagingAndCriteria
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("UserId", userid);
            Dictionary<long, IList<UserAppRole>> userAppRole = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);
            //check this count method "REVISIT" - referred from many places.
            if (userAppRole != null && userAppRole.First().Value != null && userAppRole.First().Value.Count > 0)
            {
                var BranchCodeList = (
                         from items in userAppRole.First().Value
                         where items.UserId == userid && items.BranchCode != null
                         select new
                         {
                             Text = items.BranchCode,
                             Value = items.BranchCode
                         }).Distinct().ToList().OrderBy(x => x.Text);
                return Json(BranchCodeList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult FillAllBranchCode()
        {
            string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
            MastersService ms = new MastersService();
            //check this count method "REVISIT" - referred from many places.
            Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, null);
            if (CampusMaster != null && CampusMaster.First().Value != null && CampusMaster.First().Value.Count > 0)
            {
                var BranchCodeList = (
                         from items in CampusMaster.First().Value
                         select new
                         {
                             Text = items.Name,
                             Value = items.Name
                         }).Distinct().ToList();
                return Json(BranchCodeList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult FillStaffName(string cam)
        {
            Assess360Service a360 = new Assess360Service();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Campus", cam);
            //check this count method "REVISIT" - referred from many places.
            Dictionary<long, IList<StaffMaster>> StaffNames = a360.GetStaffMasterListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);
            if (StaffNames != null && StaffNames.First().Value != null && StaffNames.First().Value.Count > 0)
            {
                var StaffNameList = (
                         from items in StaffNames.First().Value
                         where items.Campus != null
                         select new
                         {
                             Text = items.StaffName,
                             Value = items.StaffName
                         }).Distinct().ToList();
                return Json(StaffNameList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult FillStaffNameByUserRole(string cam)
        {
            UserService us = new UserService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("AppCode", "360");
            criteria.Add("BranchCode", cam);
            Dictionary<long, IList<UserAppRole_Vw>> StaffUserList = us.GetAppRoleOnlyActiveUsersPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
            //check this count method "REVISIT" - referred from many places.
            //Dictionary<long, IList<StaffMaster>> StaffNames = a360.GetStaffMasterListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);
            if (StaffUserList != null && StaffUserList.First().Value != null && StaffUserList.First().Value.Count > 0)
            {
                var StaffNameList = (
                         from items in StaffUserList.First().Value
                         where items.BranchCode != null
                         select new
                         {
                             Text = items.UserName,
                             Value = items.UserId
                         }).Distinct().ToList();
                return Json(StaffNameList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public IList<CampusEmailId> GetEmailIdByCampus(string campus, string server)
        {
            UserService us = new UserService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Campus", campus);
            criteria.Add("Server", server);
            IList<CampusEmailId> EmailId = us.GetCampusEmailListWithPaging(0, 1000, string.Empty, string.Empty, criteria);
            if (EmailId != null && EmailId.Count() > 0)
            {
                return EmailId;
            }
            else
            {
                CampusEmailId ce = new CampusEmailId();
                if (ConfigurationManager.AppSettings["CampusEmailType"].ToString() == "Test")
                { ce.EmailId = "tipscmsupp0rthyderabad247@gmail.com"; ce.Password = "Supp0rt24by7"; }
                else if (ConfigurationManager.AppSettings["CampusEmailType"].ToString() == "Live")
                { ce.EmailId = "supportdesk@tipsglobal.org"; ce.Password = "tips2013"; }
                EmailId.Add(ce);
                return EmailId;
            }
        }
        public IList<CampusAdminEmailId> GetEmailIdByCampusAdmin(string campus, string server)
        {
            UserService us = new UserService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Campus", campus);
            criteria.Add("Server", server);
            IList<CampusAdminEmailId> EmailId = us.GetCampusAdminEmailListWithPaging(0, 1000, string.Empty, string.Empty, criteria);

            if (EmailId != null && EmailId.Count() > 0)
            {
                return EmailId;
            }
            else
            {
                CampusAdminEmailId ce = new CampusAdminEmailId();
                if (ConfigurationManager.AppSettings["CampusEmailType"].ToString() == "Test")
                { ce.EmailId = "tipscmsupp0rthyderabad247@gmail.com"; ce.Password = "Supp0rt24by7"; }
                else if (ConfigurationManager.AppSettings["CampusEmailType"].ToString() == "Live")
                { ce.EmailId = "supportdesk@tipsglobal.org"; ce.Password = "tips2013"; }
                EmailId.Add(ce);
                return EmailId;
            }
        }

        // overall grade list
        public string GetGradeList(string Getvaluevalue)
        {
            string result = "";
            if (!string.IsNullOrEmpty(Getvaluevalue))
            {
                Decimal value = Convert.ToDecimal(Getvaluevalue);
                if (value > 89) { result = "A*"; }
                else if (value <= 89 && value > 79) { result = "A"; }
                else if (value <= 79 && value > 69) { result = "B"; }
                else if (value <= 69 && value > 59) { result = "C"; }
                else if (value <= 59 && value > 49) { result = "D"; }
                else if (value <= 49 && value > 39) { result = "E"; }
                else if (value <= 39 && value > 29) { result = "F"; }
                else if (value <= 29 && value > 19) { result = "G"; }
                else if (value <= 19 && value >= 0) { result = "U"; }
                else { result = ""; }
            }
            return result;
        }

        public string GetSecondLanguangeStudent(string IdNo, string Campus, string Grade)
        {
            string result = string.Empty;
            AdmissionManagementService admsvc = new AdmissionManagementService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Clear();
            criteria.Add("NewId", IdNo);
            criteria.Add("Campus", Campus);
            criteria.Add("Grade", Grade);
            Dictionary<long, IList<StudentTemplate>> StudList = admsvc.GetStudentDetailsListWithEQsearchCriteria(0, 9999, string.Empty, string.Empty, criteria);
            if (StudList != null && StudList.Count > 0 && StudList.FirstOrDefault().Key > 0 && !string.IsNullOrWhiteSpace(StudList.FirstOrDefault().Value[0].SecondLanguage))
            {
                return StudList.FirstOrDefault().Value[0].SecondLanguage;
            }
            else { return null; }
        }

        public JsonResult FillMonth()
        {
            StoreService ss = new StoreService();
            //the below count method need to be modified "REVISIT"
            Dictionary<long, IList<MonthMaster>> monthMaster = ss.GetMonthMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, null);
            if (monthMaster != null && monthMaster.First().Value != null && monthMaster.First().Value.Count > 0)
            {
                var StoreMasterList = (
                         from items in monthMaster.First().Value
                         select new
                         {
                             Text = items.MonthName,
                             Value = items.MonthValue
                         }).Distinct().ToList();
                return Json(StoreMasterList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public void CreateAssess360(StudentTemplate st)
        {
            Assess360Service asrv = new Assess360Service();
            Assess360 a360 = new Assess360();
            a360.DateCreated = DateTime.Now;
            a360.CreatedBy = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
            a360.UserRole = "360";
            a360.StudentId = st.Id;
            a360.Name = st.Name;
            a360.Campus = st.Campus;
            a360.Grade = st.Grade;
            a360.Section = st.Section;
            a360.AcademicYear = st.AcademicYear;
            a360.IdNo = st.NewId;
            a360.ConsolidatedMarks = "100.00 / 100";
            a360.IsActive = true;
            asrv.SaveOrUpdateAssess360(a360);
            a360.RequestNo = "A360-" + a360.IdNo + "-" + a360.Id.ToString();
            asrv.SaveOrUpdateAssess360(a360);
        }
        //added by JP with felix to check whether an email id is valid or not as for as syntax is concerned
        public bool ValidEmailOrNot(string emailId)
        {
            bool valid = false;

            if (!string.IsNullOrEmpty(emailId) && Regex.IsMatch(emailId,
                      @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                      RegexOptions.IgnoreCase))
            {
                valid = true;
            }
            return valid;
        }
        //added by JP with anbu to email with a new task so that the current thread will not wait for the email sending time
        public bool SendEmailWithForNewTask(System.Net.Mail.MailMessage mail, SmtpClient smtp)
        {
            bool retValue = false;
            try
            {
                smtp.Send(mail);
                retValue = true;
            }
            catch (Exception) { return retValue; }
            return retValue;
        }
        //added by JP with anbu to store the user log off while clicking logout link and will be called with new task
        public void UpdateUserLogoff(string userId)
        {
            UserService us = new UserService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("UserId", userId);
            Dictionary<long, IList<LoginHistory>> sessionList = us.GetSessionListWithPaging(0, 100, "Desc", "Id", criteria);
            if (sessionList != null && sessionList.First().Value != null && sessionList.First().Value.Count > 0)
            {
                sessionList.FirstOrDefault().Value[0].TimeOut = DateTime.Now;
                us.UpdateSession(sessionList.FirstOrDefault().Value[0]);
            }
        }

        public static string ConvertDateTimeToDate(string dateTimeString, String langCulture)
        {
            if (!string.IsNullOrEmpty(langCulture))
            {
                CultureInfo culture = new CultureInfo(langCulture);
                DateTime dt = DateTime.MinValue;

                if (DateTime.TryParse(dateTimeString, out dt))
                {
                    return dt.ToString("d", culture);
                }
            }
            else
            {
                langCulture = "en-GB";
                CultureInfo culture = new CultureInfo(langCulture);
                DateTime dt = DateTime.MinValue;

                if (DateTime.TryParse(dateTimeString, out dt))
                {
                    return dt.ToString("d", culture);
                }
            }
            return dateTimeString;
        }

        // written by felix kinoniya
        public JsonResult DesignationByCampus(string campus)
        {
            Assess360Service a360 = new Assess360Service();
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(campus))
                criteria.Add("Campus", campus);
            // Dictionary<long, IList<StaffMaster>> StaffNames = a360.GetStaffMasterListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);
            Dictionary<long, IList<DesignationMaster>> DesignationMaster = ms.GetDesignationMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            if (DesignationMaster != null && DesignationMaster.First().Key > 0 && DesignationMaster.First().Value.Count > 0)
            {
                var designationList = (
                         from items in DesignationMaster.First().Value
                         where items.Campus != null
                         select new
                         {
                             Text = items.Designation,
                             Vadue = items.Designation
                         }).Distinct().ToList();
                return Json(designationList, JsonRequestBehavior.AllowGet);
            }
            var jsondat = new { rows = (new { cell = new string[] { } }) };
            return Json(jsondat, JsonRequestBehavior.AllowGet);
        }

        public JsonResult staffTypeByCampus(string campus)
        {
            Assess360Service a360 = new Assess360Service();
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Campus", campus);
            Dictionary<long, IList<StaffTypeMaster>> StaffTypeMaster = ms.GetStaffTypeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            if (StaffTypeMaster != null && StaffTypeMaster.First().Key > 0 && StaffTypeMaster.First().Value.Count > 0)
            {
                var staffTypeMasterList = (
                         from items in StaffTypeMaster.First().Value
                         where items.Campus != null
                         select new
                         {
                             Text = items.StaffType,
                             Vadue = items.StaffType
                         }).Distinct().ToList();
                return Json(staffTypeMasterList, JsonRequestBehavior.AllowGet);
            }
            var jsondat = new { rows = (new { cell = new string[] { } }) };
            return Json(jsondat, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DepartmentByCampus(string campus)
        {
            Assess360Service a360 = new Assess360Service();
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Campus", campus);
            Dictionary<long, IList<StaffDepartmentMaster>> DepartmentMaster = ms.GetStaffDepartmentMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            if (DepartmentMaster != null && DepartmentMaster.First().Key > 0 && DepartmentMaster.First().Value.Count > 0)
            {
                var departmentMasterList = (
                         from items in DepartmentMaster.First().Value
                         where items.Campus != null
                         select new
                         {
                             Text = items.Department,
                             Vadue = items.Department
                         }).Distinct().ToList();
                return Json(departmentMasterList, JsonRequestBehavior.AllowGet);
            }
            var jsondat = new { rows = (new { cell = new string[] { } }) };
            return Json(jsondat, JsonRequestBehavior.AllowGet);
        }
        // end

        #region ForReportCard
        public ActionResult GetSubjectsByCampusGrade(string Campus, string Grade)
        {
            try
            {
                MastersService MstSrv = new MastersService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                if (!string.IsNullOrWhiteSpace(Grade)) { criteria.Add("Grade", Grade); }
                Dictionary<long, IList<CampusSubjectMaster>> subMaster = MstSrv.GetSubjectMasterByCampusListWithPagingAndCriteria(0, 9999, "Asc", "SubjectName", criteria);
                if (subMaster != null && subMaster.Count > 0)
                {
                    var subjectMstrLst = new
                    {
                        rows = (
                             from items in subMaster.FirstOrDefault().Value
                             select new
                             {
                                 Text = items.SubjectName,
                                 Value = items.SubjectName
                             }).Distinct().ToArray()
                    };

                    return Json(subjectMstrLst, JsonRequestBehavior.AllowGet);
                }
                else
                { return Json(null, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, "Assess360Policy"); }
        }
        public ActionResult GetLanguageByCampus(string Campus)
        {
            try
            {
                MastersService MstSrv = new MastersService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                Dictionary<long, IList<CampusLanguageMaster>> LangMaster = MstSrv.GetLanguageMasterByCampusListWithPagingAndCriteria(0, 9999, "Asc", "LanguageName", criteria);
                if (LangMaster != null && LangMaster.Count > 0)
                {
                    var subjectMstrLst = new
                    {
                        rows = (
                             from items in LangMaster.FirstOrDefault().Value
                             select new
                             {
                                 Text = items.LanguageName,
                                 Value = items.LanguageName
                             }).Distinct().ToArray()
                    };

                    return Json(subjectMstrLst, JsonRequestBehavior.AllowGet);
                }
                else
                { return Json(null, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, "Assess360Policy"); }
        }
        #endregion

        public ActionResult GetCampusWiseGradeVal(string campus)
        {
            try
            {
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    UserService us = new UserService();
                    MastersService Mscv = new MastersService();
                    Dictionary<string, Object> criteria = new Dictionary<string, object>();
                    //if (campus == "TIPSE CBSE")
                    //{
                    // criteria.Add("Code", "1");
                    //}
                    //else if (campus == "TIPS MONTI")
                    //{
                    // criteria.Add("Code", "2");
                    //}
                    //else { }
                    int[] values;
                    var g1 = "";
                    if (campus == "IB KG")
                    {
                        g1 = "2";
                        criteria.Add("Code", g1);
                        values = new int[0];
                    }
                    else if (campus == "IB MAIN")
                    {
                        g1 = "1";
                        criteria.Add("Code", g1);
                        values = new int[0];
                    }
                    else if (campus == "ERNAKULAM")
                    {
                        g1 = "";
                        criteria.Add("Code", g1);
                        values = new int[0];
                    }
                    else if (campus == "TIPS SARAN") // Get using IN condition
                    {
                        values = new int[2];
                        criteria.Clear();
                        values[0] = 11; // get grade with column GRAD
                        values[1] = 12;
                    }
                    else if (campus == "CHENNAI CITY") // Get using IN condition
                    {
                        values = new int[5];
                        criteria.Clear();
                        values[0] = 102; // get grade with column GRAD
                        values[1] = 101;
                        values[2] = 100; values[3] = 1; values[4] = 2;
                    }
                    else if (campus == "CHENNAI MAIN") // Get using IN condition
                    {
                        values = new int[6];
                        criteria.Clear();
                        values[0] = 3; // get grade with column GRAD
                        values[1] = 4;
                        values[2] = 5; values[3] = 6; values[4] = 7; values[5] = 8;
                    }
                    else if (campus == "TIPS CBSE")
                    {
                        values = new int[10];
                        criteria.Clear();
                        values[0] = 1; values[1] = 2; values[2] = 3;
                        values[3] = 4; values[4] = 5; values[5] = 6;
                        values[6] = 7; values[7] = 8; values[8] = 9; values[9] = 10;
                    }
                    else
                    {
                        g1 = "3";
                        criteria.Add("Code1", g1);
                        values = new int[0];
                    }
                    Dictionary<long, IList<GradeMaster>> TIPSGrade = Mscv.GetGradeMasterListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    var Gradname = (
                    from items in TIPSGrade.FirstOrDefault().Value
                    select new
                    {
                        Text = items.gradcod,
                        Value = items.gradcod
                    }).Distinct().ToList();
                    return Json(Gradname, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public string GetCategoryByGrade(string Grade)
        {
            string Category = string.Empty;
            string[] PYPGrades = { "I", "II", "III", "IV", "V" };
            string[] MYPGrades = { "VI", "VII", "VIII", "IX", "X", "XI", "XII" };

            if (PYPGrades.Contains(Grade))
                Category = "PYP";
            else if (MYPGrades.Contains(Grade))
                Category = "MYP";
            return Category;
        }

        public ActionResult GetVanRouteNoByCampus(string Campus)
        {
            if (!string.IsNullOrWhiteSpace(Campus))
            {
                TransportBC samp = new TransportBC();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Campus", Campus);
                Dictionary<long, IList<RouteMaster>> RouteMasterList = samp.GetRouteMasterDetails(0, 99999, string.Empty, string.Empty, criteria);
                if (RouteMasterList != null && RouteMasterList.First().Key > 0 && RouteMasterList.First().Value.Count > 0)
                {
                    var VanNoMasterList = (
                             from items in RouteMasterList.First().Value
                             where items.Campus != null
                             select new
                             {
                                 Text = items.RouteNo,
                                 Value = items.RouteNo
                             }).Distinct().ToList().OrderBy(x => x.Text);
                    return Json(VanNoMasterList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            else
                return Json("", JsonRequestBehavior.AllowGet);
        }

        public IList<CoOrdinatorsContactInfo> GetCoOrdinatorsContactInfo(string campus, string server, string Category)
        {
            UserService us = new UserService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Campus", campus);
            criteria.Add("Server", server);
            criteria.Add("Category", Category);
            IList<CoOrdinatorsContactInfo> CoOrdinatorsContactInfo = us.GetCoOrdinatorsContactInfoListWithPaging(0, 1000, string.Empty, string.Empty, criteria);

            if (CoOrdinatorsContactInfo != null && CoOrdinatorsContactInfo.Count() > 0)
            {
                return CoOrdinatorsContactInfo;
            }
            else
                return null;
        }


        public string GetGeneralBodyofMail()
        {
            string MessageBody = System.IO.File.ReadAllText(HttpContext.Server.MapPath("~/Views/Shared/GeneralEmailBody.html"));
            return MessageBody;
        }

        public Int32 GetGradeStars(string Gradevalue)
        {
            Int32 result = 0;
            if (!string.IsNullOrEmpty(Gradevalue))
            {
                if (Gradevalue == "A*") { result = 5; }
                else if (Gradevalue == "A") { result = 4; }
                else if (Gradevalue == "B") { result = 3; }
                else if (Gradevalue == "C") { result = 2; }
                else if (Gradevalue == "D") { result = 1; }
                else { result = 0; }
            }
            return result;
        }

        #region BreadCrumb
        public string GetBreadCrumbDetails(string ControllerName, string ActionName)
        {
            MenuBC MenuBC = new MenuBC();
            TIPS.Entities.MenuEntities.Menu MenuDetails = new TIPS.Entities.MenuEntities.Menu();
            TIPS.Entities.MenuEntities.Menu Level2Details = new TIPS.Entities.MenuEntities.Menu();
            TIPS.Entities.MenuEntities.Menu Level1Details = new TIPS.Entities.MenuEntities.Menu();
            List<TIPS.Entities.MenuEntities.Menu> MenuList = new List<TIPS.Entities.MenuEntities.Menu>();
            MenuDetails = MenuBC.GetMenuByControllerAndAction(ControllerName, ActionName);
            if (MenuDetails != null)
            {
                TIPS.Entities.MenuEntities.Menu menu = new TIPS.Entities.MenuEntities.Menu();
                menu.MenuName = MenuDetails.MenuName;
                MenuList.Add(menu);
                if (MenuDetails.MenuLevel == "Level3")
                {
                    TIPS.Entities.MenuEntities.Menu Level2 = new TIPS.Entities.MenuEntities.Menu();
                    Level2Details = MenuBC.GetMenuItemsById(MenuDetails.ParentRefId);
                    if (Level2Details != null)
                    {
                        Level2.MenuName = Level2Details.MenuName;
                        MenuList.Add(Level2);
                        if (Level2Details.MenuLevel == "Level2")
                        {
                            TIPS.Entities.MenuEntities.Menu Level1 = new TIPS.Entities.MenuEntities.Menu();
                            Level1Details = MenuBC.GetMenuItemsById(Level2Details.ParentRefId);
                            if (Level1Details != null)
                            {
                                Level1.MenuName = Level1Details.MenuName;
                                MenuList.Add(Level1);
                            }
                        }
                    }
                }
                else if (MenuDetails.MenuLevel == "Level2")
                {
                    TIPS.Entities.MenuEntities.Menu Level1 = new TIPS.Entities.MenuEntities.Menu();
                    Level1Details = MenuBC.GetMenuItemsById(MenuDetails.ParentRefId);
                    if (Level1Details != null)
                    {
                        Level1.MenuName = Level1Details.MenuName;
                        MenuList.Add(Level1);
                    }
                }
                else
                {
                }
            }
            MenuList.Reverse();
            int OrderCount = MenuList.Count;
            System.Text.StringBuilder BredCrumbHtml = new System.Text.StringBuilder();
            BredCrumbHtml.Append("<ul class='breadcrumb'>");
            int i = 1;
            foreach (var item in MenuList)
            {
                if (i == OrderCount)
                    BredCrumbHtml.Append("<li style='font family:verdana;font-size:12px;'><b>" + item.MenuName + "</b></li>");
                else
                    BredCrumbHtml.Append("<li style='font family:verdana;font-size:12px;'>" + item.MenuName + "</li>");
                i++;
            }
            BredCrumbHtml.Append("</ul>");
            return BredCrumbHtml.ToString();
        }
        #endregion

        #region "Search Multiple Criteria"
        public ActionResult FillGradesWithArrayCriteria(string campus)
        {
            try
            {
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ms = new MastersService();
                    Dictionary<string, Object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(campus))
                    {
                        var campusArr = campus.Split(',');
                        criteria.Add("Campus", campusArr);
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

        public IList<AcademicyrMaster> GetAcademicYear()
        {
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
            // ViewBag.acadyrddl = AcademicyrMaster.First().Value;
            return AcademicyrMaster.First().Value;
        }

        public JsonResult GetJsonAcademicYear()
        {
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
            var academicObj = (from items in AcademicyrMaster.First().Value
                               select new
                               {
                                   Text = items.AcademicYear,
                                   Value = items.AcademicYear
                               }).Distinct().ToList();
            return Json(academicObj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Bankmasterddl()
        {
            try
            {
                MastersService ms = new MastersService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<BankMaster>> BankMaster = ms.GetBankMasterListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                if (BankMaster != null && BankMaster.First().Value != null && BankMaster.First().Value.Count > 0)
                {
                    Dictionary<string, string> BankNameList = new Dictionary<string, string>();
                    foreach (var item in BankMaster.FirstOrDefault().Value)
                    {
                        BankNameList.Add(item.BankName, item.BankName);
                    }
                    return PartialView("Dropdown", BankNameList.Distinct());
                }
                else
                    return Json(null, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion "End"
        #region "Get Value from Campus,Grade,AcademicYeaer Master"
        public IList<AcademicyrMaster> AcademicyrMasterFunc()
        {
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            return AcademicyrMaster.First().Value;
        }
        public IList<GradeMaster> GradeMasterFunc()
        {
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            return GradeMaster.First().Value;
        }
        public IList<CampusMaster> CampusMasterFunc()
        {
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            return CampusMaster.First().Value;
        }
        public IList<BankMaster> BankMasterFunc()
        {
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<BankMaster>> BankMaster = ms.GetBankMasterListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
            return BankMaster.First().Value;
        }
        #endregion "End"

        public JsonResult FillAllLocationCode()
        {
            string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
            MastersService ms = new MastersService();
            //check this count method "REVISIT" - referred from many places.
            Dictionary<long, IList<LocationMaster>> LocationMaster = ms.GetLocationMasterListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, null);
            if (LocationMaster != null && LocationMaster.First().Value != null && LocationMaster.First().Value.Count > 0)
            {
                var BranchCodeList = (
                         from items in LocationMaster.First().Value
                         select new
                         {
                             Text = items.LocationName,
                             Value = items.LocationName
                         }).Distinct().ToList();
                return Json(BranchCodeList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public string JoiningYear(string joinYear)
        {
            string retVal = string.Empty;
            if (joinYear != null)
            {
                string[] words = joinYear.Split('/');
                string sub = words[2].Substring(2, 2);
                return sub;
            }
            return retVal;
        }

        public DateTime[] GetLastAndFirstDateTimeinMonth(string mnth, int acaYear)
        {
            DateTime fdate = DateTime.Now;
            DateTime tdate = DateTime.Now;
            DateTime[] fromto = new DateTime[2];
            switch (mnth)
            {
                case "01":
                    {
                        DateTime junfirst = new DateTime(acaYear, 1, 1);
                        DateTime junlast = junfirst.AddMonths(1).AddDays(-1);
                        string from = string.Format("{0:dd/MM/yyyy}", junfirst);
                        string to = string.Format("{0:dd/MM/yyyy}", junlast);
                        fdate = Convert.ToDateTime(from + " " + "12:00:00 AM");
                        tdate = Convert.ToDateTime(to + " " + "23:59:59");
                        fromto[0] = fdate;
                        fromto[1] = tdate;
                        //criteria1.Add("AbsentDate", fromto);
                        break;
                    }
                case "02":
                    {
                        DateTime febfirst = new DateTime(acaYear, 2, 1);
                        DateTime feblast = febfirst.AddMonths(1).AddDays(-1);
                        string from = string.Format("{0:dd/MM/yyyy}", febfirst);
                        string to = string.Format("{0:dd/MM/yyyy}", feblast);
                        fdate = Convert.ToDateTime(from + " " + "12:00:00 AM");
                        tdate = Convert.ToDateTime(to + " " + "23:59:59");
                        fromto[0] = fdate;
                        fromto[1] = tdate;
                        //criteria1.Add("AbsentDate", fromto);
                        break;
                    }
                case "03":
                    {
                        DateTime marfirst = new DateTime(acaYear, 3, 1);
                        DateTime marlast = marfirst.AddMonths(1).AddDays(-1);
                        string from = string.Format("{0:dd/MM/yyyy}", marfirst);
                        string to = string.Format("{0:dd/MM/yyyy}", marlast);
                        fdate = Convert.ToDateTime(from + " " + "12:00:00 AM");
                        tdate = Convert.ToDateTime(to + " " + "23:59:59");
                        fromto[0] = fdate;
                        fromto[1] = tdate;
                        //criteria1.Add("AbsentDate", fromto);
                        break;
                    }
                case "04":
                    {
                        DateTime Aprfirst = new DateTime(acaYear, 4, 1);
                        DateTime Aprlast = Aprfirst.AddMonths(1).AddDays(-1);
                        string from = string.Format("{0:dd/MM/yyyy}", Aprfirst);
                        string to = string.Format("{0:dd/MM/yyyy}", Aprlast);
                        fdate = Convert.ToDateTime(from + " " + "12:00:00 AM");
                        tdate = Convert.ToDateTime(to + " " + "23:59:59");
                        fromto[0] = fdate;
                        fromto[1] = tdate;
                        //criteria1.Add("AbsentDate", fromto);
                        break;
                    }
                case "05":
                    {
                        DateTime mayfirst = new DateTime(acaYear, 5, 1);
                        DateTime maylast = mayfirst.AddMonths(1).AddDays(-1);
                        string from = string.Format("{0:dd/MM/yyyy}", mayfirst);
                        string to = string.Format("{0:dd/MM/yyyy}", maylast);
                        fdate = Convert.ToDateTime(from + " " + "12:00:00 AM");
                        tdate = Convert.ToDateTime(to + " " + "23:59:59");
                        fromto[0] = fdate;
                        fromto[1] = tdate;
                        //criteria1.Add("AbsentDate", fromto);
                        break;
                    }
                case "06":
                    {
                        DateTime junfirst = new DateTime(acaYear, 6, 1);
                        DateTime junlast = junfirst.AddMonths(1).AddDays(-1);
                        string from = string.Format("{0:dd/MM/yyyy}", junfirst);
                        string to = string.Format("{0:dd/MM/yyyy}", junlast);
                        fdate = Convert.ToDateTime(from + " " + "12:00:00 AM");
                        tdate = Convert.ToDateTime(to + " " + "23:59:59");
                        fromto[0] = fdate;
                        fromto[1] = tdate;
                        //criteria1.Add("AbsentDate", fromto);
                        break;
                    }
                case "07":
                    {
                        DateTime jullyfirst = new DateTime(acaYear, 7, 1);
                        DateTime jullylase = jullyfirst.AddMonths(1).AddDays(-1);
                        string from = string.Format("{0:dd/MM/yyyy}", jullyfirst);
                        string to = string.Format("{0:dd/MM/yyyy}", jullylase);
                        fdate = Convert.ToDateTime(from + " " + "12:00:00 AM");
                        tdate = Convert.ToDateTime(to + " " + "23:59:59");
                        fromto[0] = fdate;
                        fromto[1] = tdate;
                        // criteria1.Add("AbsentDate", fromto);
                        break;
                    }
                case "08":
                    {
                        DateTime augfirst = new DateTime(acaYear, 8, 1);
                        DateTime auglast = augfirst.AddMonths(1).AddDays(-1);
                        string from = string.Format("{0:dd/MM/yyyy}", augfirst);
                        string to = string.Format("{0:dd/MM/yyyy}", auglast);
                        fdate = Convert.ToDateTime(from + " " + "12:00:00 AM");
                        tdate = Convert.ToDateTime(to + " " + "23:59:59");
                        fromto[0] = fdate;
                        fromto[1] = tdate;
                        // criteria1.Add("AbsentDate", fromto);
                        break;
                    }
                case "09":
                    {
                        DateTime sepfirst = new DateTime(acaYear, 9, 1);
                        DateTime seplast = sepfirst.AddMonths(1).AddDays(-1);
                        string from = string.Format("{0:dd/MM/yyyy}", sepfirst);
                        string to = string.Format("{0:dd/MM/yyyy}", seplast);
                        fdate = Convert.ToDateTime(from + " " + "12:00:00 AM");
                        tdate = Convert.ToDateTime(to + " " + "23:59:59");
                        fromto[0] = fdate;
                        fromto[1] = tdate;
                        //criteria1.Add("AbsentDate", fromto);
                        break;
                    }
                case "10":
                    {
                        DateTime octfirst = new DateTime(acaYear, 10, 1);
                        DateTime octlast = octfirst.AddMonths(1).AddDays(-1);
                        string from = string.Format("{0:dd/MM/yyyy}", octfirst);
                        string to = string.Format("{0:dd/MM/yyyy}", octlast);
                        fdate = Convert.ToDateTime(from + " " + "12:00:00 AM");
                        tdate = Convert.ToDateTime(to + " " + "23:59:59");
                        fromto[0] = fdate;
                        fromto[1] = tdate;
                        //criteria1.Add("AbsentDate", fromto);
                        break;
                    }
                case "11":
                    {
                        DateTime novfirst = new DateTime(acaYear, 11, 1);
                        DateTime novlast = novfirst.AddMonths(1).AddDays(-1);
                        string from = string.Format("{0:dd/MM/yyyy}", novfirst);
                        string to = string.Format("{0:dd/MM/yyyy}", novlast);
                        fdate = Convert.ToDateTime(from + " " + "12:00:00 AM");
                        tdate = Convert.ToDateTime(to + " " + "23:59:59");
                        fromto[0] = fdate;
                        fromto[1] = tdate;
                        // criteria1.Add("AbsentDate", fromto);
                        break;
                    }
                case "12":
                    {
                        DateTime decfirst = new DateTime(acaYear, 12, 1);
                        DateTime declast = decfirst.AddMonths(1).AddDays(-1);
                        string from = string.Format("{0:dd/MM/yyyy}", decfirst);
                        string to = string.Format("{0:dd/MM/yyyy}", declast);
                        fdate = Convert.ToDateTime(from + " " + "12:00:00 AM");
                        tdate = Convert.ToDateTime(to + " " + "23:59:59");
                        fromto[0] = fdate;
                        fromto[1] = tdate;
                        //criteria1.Add("AbsentDate", fromto);
                        break;
                    }
                default: break;
            }
            return fromto;
        }

        public string[] GetLastAndFirstDateStringinMonth(string mnth, int acaYear)
        {
            string fdate = string.Empty, tdate = string.Empty;
            string[] fromto = new string[2];
            switch (mnth)
            {
                case "01":
                    {
                        DateTime junfirst = new DateTime(acaYear, 1, 1);
                        DateTime junlast = junfirst.AddMonths(1).AddDays(-1);
                        string from = string.Format("{0:dd/MM/yyyy}", junfirst);
                        string to = string.Format("{0:dd/MM/yyyy}", junlast);
                        fromto[0] = from;
                        fromto[1] = to;
                        //criteria1.Add("AbsentDate", fromto);
                        break;
                    }
                case "02":
                    {
                        DateTime febfirst = new DateTime(acaYear, 2, 1);
                        DateTime feblast = febfirst.AddMonths(1).AddDays(-1);
                        string from = string.Format("{0:dd/MM/yyyy}", febfirst);
                        string to = string.Format("{0:dd/MM/yyyy}", feblast);
                        fromto[0] = from;
                        fromto[1] = to;
                        //criteria1.Add("AbsentDate", fromto);
                        break;
                    }
                case "03":
                    {
                        DateTime marfirst = new DateTime(acaYear, 3, 1);
                        DateTime marlast = marfirst.AddMonths(1).AddDays(-1);
                        string from = string.Format("{0:dd/MM/yyyy}", marfirst);
                        string to = string.Format("{0:dd/MM/yyyy}", marlast);
                        fromto[0] = from;
                        fromto[1] = to;
                        //criteria1.Add("AbsentDate", fromto);
                        break;
                    }
                case "04":
                    {
                        DateTime Aprfirst = new DateTime(acaYear, 4, 1);
                        DateTime Aprlast = Aprfirst.AddMonths(1).AddDays(-1);
                        string from = string.Format("{0:dd/MM/yyyy}", Aprfirst);
                        string to = string.Format("{0:dd/MM/yyyy}", Aprlast);
                        fromto[0] = from;
                        fromto[1] = to;
                        //criteria1.Add("AbsentDate", fromto);
                        break;
                    }
                case "05":
                    {
                        DateTime mayfirst = new DateTime(acaYear, 5, 1);
                        DateTime maylast = mayfirst.AddMonths(1).AddDays(-1);
                        string from = string.Format("{0:dd/MM/yyyy}", mayfirst);
                        string to = string.Format("{0:dd/MM/yyyy}", maylast);
                        fromto[0] = from;
                        fromto[1] = to;
                        //criteria1.Add("AbsentDate", fromto);
                        break;
                    }
                case "06":
                    {
                        DateTime junfirst = new DateTime(acaYear, 6, 1);
                        DateTime junlast = junfirst.AddMonths(1).AddDays(-1);
                        string from = string.Format("{0:dd/MM/yyyy}", junfirst);
                        string to = string.Format("{0:dd/MM/yyyy}", junlast);
                        fromto[0] = from;
                        fromto[1] = to;
                        //criteria1.Add("AbsentDate", fromto);
                        break;
                    }
                case "07":
                    {
                        DateTime jullyfirst = new DateTime(acaYear, 7, 1);
                        DateTime jullylase = jullyfirst.AddMonths(1).AddDays(-1);
                        string from = string.Format("{0:dd/MM/yyyy}", jullyfirst);
                        string to = string.Format("{0:dd/MM/yyyy}", jullylase);
                        fromto[0] = from;
                        fromto[1] = to;
                        // criteria1.Add("AbsentDate", fromto);
                        break;
                    }
                case "08":
                    {
                        DateTime augfirst = new DateTime(acaYear, 8, 1);
                        DateTime auglast = augfirst.AddMonths(1).AddDays(-1);
                        string from = string.Format("{0:dd/MM/yyyy}", augfirst);
                        string to = string.Format("{0:dd/MM/yyyy}", auglast);
                        fromto[0] = from;
                        fromto[1] = to;
                        // criteria1.Add("AbsentDate", fromto);
                        break;
                    }
                case "09":
                    {
                        DateTime sepfirst = new DateTime(acaYear, 9, 1);
                        DateTime seplast = sepfirst.AddMonths(1).AddDays(-1);
                        string from = string.Format("{0:dd/MM/yyyy}", sepfirst);
                        string to = string.Format("{0:dd/MM/yyyy}", seplast);
                        fromto[0] = from;
                        fromto[1] = to;
                        //criteria1.Add("AbsentDate", fromto);
                        break;
                    }
                case "10":
                    {
                        DateTime octfirst = new DateTime(acaYear, 10, 1);
                        DateTime octlast = octfirst.AddMonths(1).AddDays(-1);
                        string from = string.Format("{0:dd/MM/yyyy}", octfirst);
                        string to = string.Format("{0:dd/MM/yyyy}", octlast);
                        fromto[0] = from;
                        fromto[1] = to;
                        //criteria1.Add("AbsentDate", fromto);
                        break;
                    }
                case "11":
                    {
                        DateTime novfirst = new DateTime(acaYear, 11, 1);
                        DateTime novlast = novfirst.AddMonths(1).AddDays(-1);
                        string from = string.Format("{0:dd/MM/yyyy}", novfirst);
                        string to = string.Format("{0:dd/MM/yyyy}", novlast);
                        fromto[0] = from;
                        fromto[1] = to;
                        // criteria1.Add("AbsentDate", fromto);
                        break;
                    }
                case "12":
                    {
                        DateTime decfirst = new DateTime(acaYear, 12, 1);
                        DateTime declast = decfirst.AddMonths(1).AddDays(-1);
                        string from = string.Format("{0:dd/MM/yyyy}", decfirst);
                        string to = string.Format("{0:dd/MM/yyyy}", declast);
                        fromto[0] = from;
                        fromto[1] = to;
                        //criteria1.Add("AbsentDate", fromto);
                        break;
                    }
                default: break;
            }
            return fromto;
        }

        public DateTime[] getFromTodate(string SrchCategory)
        {
            DateTime baseDate = DateTime.Today;
            var today = baseDate;
            var yesterday = baseDate.AddDays(-1);
            var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
            var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
            var lastWeekStart = thisWeekStart.AddDays(-7);
            var lastWeekEnd = thisWeekStart.AddSeconds(-1);
            var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
            var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
            var lastMonthStart = thisMonthStart.AddMonths(-1);
            var lastMonthEnd = thisMonthStart.AddSeconds(-1);
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime[] fromto = new DateTime[2];
            switch (SrchCategory)
            {
                case "ThisWeek":
                    fromto[0] = thisWeekStart;
                    fromto[1] = thisWeekEnd;
                    break;
                case "LastWeek":
                    fromto[0] = lastWeekStart;
                    fromto[1] = lastWeekEnd;
                    break;
                case "ThisMonth":
                    fromto[0] = thisMonthStart;
                    fromto[1] = thisMonthEnd;
                    break;
                case "LastMonth":
                    fromto[0] = lastMonthStart;
                    fromto[1] = lastMonthEnd;
                    break;
                case "Today":
                    DateTime todayFirst = new DateTime(DateTime.Now.Year, today.Month, today.Day);
                    DateTime todayLast = new DateTime(DateTime.Now.Year, today.Month, today.Day);
                    string todayfrom = string.Format("{0:MM/dd/yyyy}", todayFirst);
                    string todayto = string.Format("{0:MM/dd/yyyy}", todayLast);
                    fromDate = Convert.ToDateTime(todayfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(todayto + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;
            }
            return fromto;
        }

        public ActionResult IsPhotoUploadedFor(long RefNum, string docType, string docFor)
        {
            TransportBC samp = new TransportBC();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            AdmissionManagementService ads = new AdmissionManagementService();
            criteria.Add("PreRegNum", RefNum);
            criteria.Add("DocumentType", docType);
            criteria.Add("DocumentFor", docFor);
            Dictionary<long, IList<UploadedFiles>> UploadedFiles = ads.GetUploadedFilesListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);
            if (UploadedFiles != null && UploadedFiles.FirstOrDefault().Value != null && UploadedFiles.FirstOrDefault().Value.Count != 0)
            {
                return Json(docFor, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
        }

        public void ExptToXLWithHeader<T, TResult>(IList<T> stuList, string filename, Func<T, TResult> selector, string headerTable)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".xls");
            Response.ContentType = "application/vnd.ms-excel";
            System.IO.StringWriter stw = new System.IO.StringWriter();
            HtmlTextWriter htextw = new HtmlTextWriter(stw);
            DataGrid dg = new DataGrid();
            dg.HeaderStyle.BackColor = System.Drawing.Color.FromName("#206FAE");
            dg.HeaderStyle.Font.Bold = true;
            dg.HeaderStyle.ForeColor = System.Drawing.Color.White;
            dg.DataSource = stuList.Select(selector);
            dg.DataBind();
            dg.RenderControl(htextw);
            Response.Write(headerTable);
            Response.Write(stw.ToString());
            Response.End();
        }

        public string GetCurrentAcademicYear(string Grade)
        {
            try
            {
                string acayear = string.Empty;
                if (Grade == "IX" || Grade == "X" || Grade == "XI" || Grade == "XI")
                {
                    acayear = DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString();
                }
                else
                {
                    if (DateTime.Now.Month > 5)
                    {
                        acayear = DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString();
                    }
                    else
                    {
                        acayear = (DateTime.Now.Year - 1).ToString() + "-" + DateTime.Now.Year.ToString();
                    }
                }
                return acayear;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
                throw ex;
            }
        }

        public string GetSchoolName(string Campus)
        {
            string schlName = "";
            switch (Campus)
            {
                case "IB MAIN":
                    schlName = "THE INDIAN PUBLIC SCHOOL, COIMBATORE";
                    break;
                case "CHENNAI MAIN":
                    schlName = "THE INDIAN PUBLIC SCHOOL, CHENNAI";
                    break;
                default:
                    break;
            }
            return schlName;

        }
        #region added by Thamizhmani for Campus master with Id Name pair
        public JsonResult FillCampus()
        {
            string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
            MastersService ms = new MastersService();
            //check this count method "REVISIT" - referred from many places.
            Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, null);
            if (CampusMaster != null && CampusMaster.First().Value != null && CampusMaster.First().Value.Count > 0)
            {
                var BranchCodeList = (
                         from items in CampusMaster.First().Value
                         select new
                         {
                             Text = items.Name,
                             Value = items.FormId
                         }).Distinct().ToList();
                return Json(BranchCodeList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        //public ActionResult GetSubjectsByCampusGradeSec(string Campus, string Grade, string Section)
        //{
        //    try
        //    {
        //        MastersService MstSrv = new MastersService();
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();
        //        if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
        //        if (!string.IsNullOrWhiteSpace(Grade)) { criteria.Add("Grade", Grade); }
        //        if (!string.IsNullOrWhiteSpace(Section)) { criteria.Add("Section", Section); }
        //        if (Grade == "IX" || Grade == "X")
        //        {
        //            criteria.Add("AcademicYear", DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString());
        //        }
        //        else
        //        {
        //            if (DateTime.Now.Month > 5)
        //            {
        //                criteria.Add("AcademicYear", DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString());
        //            }
        //            else
        //            {
        //                criteria.Add("AcademicYear", (DateTime.Now.Year - 1).ToString() + "-" + DateTime.Now.Year.ToString());
        //            }
        //        }
        //        Dictionary<long, IList<CampusSubjectMaster>> subMaster = MstSrv.GetSubjectMasterByCampusListWithPagingAndCriteria(0, 9999, "Asc", "SubjectName", criteria);
        //        if (subMaster != null && subMaster.Count > 0)
        //        {
        //            var subjectMstrLst =
        //                 (
        //                     from items in subMaster.FirstOrDefault().Value
        //                     select new
        //                     {
        //                         Text = items.SubjectName,
        //                         Value = items.SubjectName
        //                     }).Distinct().ToList();
        //            return Json(subjectMstrLst, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        { return Json(null, JsonRequestBehavior.AllowGet); }
        //    }
        //    catch (Exception ex)
        //    { return ThrowJSONErrorNew(ex, "Assess360Policy"); }
        //}
        public ActionResult GetSubjectsByCampusGradeSec(string Campus, string Grade, string Section, string Board)
        {
            try
            {
                MastersService MstSrv = new MastersService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
                if (!string.IsNullOrWhiteSpace(Grade)) { criteria.Add("Grade", Grade); }
                if (!string.IsNullOrWhiteSpace(Section)) { criteria.Add("Section", Section); }
                if (!string.IsNullOrWhiteSpace(Board)) { criteria.Add("Board", Board); }
                if (Grade == "IX" || Grade == "X")
                {
                    criteria.Add("AcademicYear", DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString());
                }
                else
                {
                    if (DateTime.Now.Month > 5)
                    {
                        criteria.Add("AcademicYear", DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString());
                    }
                    else
                    {
                        criteria.Add("AcademicYear", (DateTime.Now.Year - 1).ToString() + "-" + DateTime.Now.Year.ToString());
                    }
                }
                Dictionary<long, IList<CampusSubjectMaster>> subMaster = MstSrv.GetSubjectMasterByCampusListWithPagingAndCriteria(0, 9999, "Asc", "SubjectName", criteria);
                if (subMaster != null && subMaster.Count > 0)
                {
                    var subjectMstrLst =
                         (
                             from items in subMaster.FirstOrDefault().Value
                             select new
                             {
                                 Text = items.SubjectName,
                                 Value = items.SubjectName
                             }).Distinct().ToList();
                    return Json(subjectMstrLst, JsonRequestBehavior.AllowGet);
                }
                else
                { return Json(null, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, "Assess360Policy"); }
        }

        public void ExptToXL_AssessPointChart<T, TResult>(IList<T> stuList, string filename, Func<T, TResult> selector, string headerTable)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".xls");
            Response.ContentType = "application/vnd.ms-excel";
            System.IO.StringWriter stw = new System.IO.StringWriter();
            HtmlTextWriter htextw = new HtmlTextWriter(stw);
            DataGrid dg = new DataGrid();
            dg.HeaderStyle.BackColor = System.Drawing.Color.FromName("#5090C1");
            dg.HeaderStyle.Font.Bold = true;
            dg.HeaderStyle.ForeColor = System.Drawing.Color.White;
            dg.DataSource = stuList.Select(selector);
            dg.DataBind();
            string style = @"<style> TABLE { border: 7px solid Black; } TD { border: 2px solid #999966; } </style> ";
            Response.Write(style);
            dg.RenderControl(htextw);
            Response.Write(headerTable);
            Response.Write(stw.ToString());
            Response.End();
        }

        public void ExportToPDF_AssessPointChart(string Title, string[] TblHeaders, float[] widths, DataTable dt, string grade, string section)
        {
            Document document = new Document(PageSize.A4, 10.25f, 10.25f, 10.5f, 10.5f);

            iTextSharp.text.Image LogonaceImage;
            string LogonaceImagePath = ConfigurationManager.AppSettings["AppLogos"] + "logonace.jpg";

            LogonaceImage = iTextSharp.text.Image.GetInstance(LogonaceImagePath);
            //  LogonaceImage.ScaleAbsolute(50, 50);

            iTextSharp.text.Image LogoImage;
            string ImagePath = ConfigurationManager.AppSettings["RptCard"] + "logo.jpg";

            LogoImage = iTextSharp.text.Image.GetInstance(ImagePath);
            //  LogoImage.ScaleAbsolute(50, 50);

            // For PDF export we are using the free open-source iTextSharp library.
            string month = DateTime.Now.ToString("MMMM");
            Document pdfDoc = new Document();
            pdfDoc.AddTitle(Title);
            MemoryStream pdfStream = new MemoryStream();
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, pdfStream);

            pdfDoc.Open();//Open Document to write
            pdfDoc.NewPage();

            Font Heading = FontFactory.GetFont("ARIAL", 6, BaseColor.BLACK);
            Font Content = FontFactory.GetFont("ARIAL", 6);

            PdfPTable HeadingTable = new PdfPTable(10);

            PdfPCell Cell1 = new PdfPCell();
            Cell1.Border = 0;
            Cell1.AddElement(LogonaceImage);
            Cell1.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadingTable.AddCell(Cell1);

            PdfPCell Cell2 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 9.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE))));
            Cell2.Border = 0;
            Cell2.Colspan = 8;
            HeadingTable.AddCell(Cell2);

            PdfPCell Cell3 = new PdfPCell();
            Cell3.Border = 0;
            Cell3.AddElement(LogoImage);
            Cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadingTable.AddCell(Cell3);

            HeadingTable.TotalWidth = 570f;
            HeadingTable.LockedWidth = true;
            HeadingTable.SetWidths(new float[] { 60f, 80f, 70f, 125f, 100f, 95f, 110f, 120f, 100f, 60f });

            PdfPTable Table = new PdfPTable(dt.Columns.Count);

            PdfPCell PdfPCell = new PdfPCell(new Phrase("The Indian Public School", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 10.0f, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0)))));
            PdfPCell.Colspan = dt.Columns.Count;
            PdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            Table.AddCell(PdfPCell);

            PdfPCell PdfPCell4 = new PdfPCell(new Phrase("Month : " + month + " ", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 7.0f, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0)))));
            PdfPCell4.Colspan = 2;
            PdfPCell4.HorizontalAlignment = Element.ALIGN_LEFT;
            PdfPCell4.HorizontalAlignment = 1;
            Table.AddCell(PdfPCell4);

            PdfPCell PdfPCell5 = new PdfPCell(new Phrase("Assess 360° Point Chart  ", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 8.0f, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0)))));
            PdfPCell5.Colspan = dt.Columns.Count;
            PdfPCell5.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell5.HorizontalAlignment = 1;
            Table.AddCell(PdfPCell5);

            PdfPCell PdfPCell2 = new PdfPCell(new Phrase("Grade : " + grade + "-" + section + "", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 7.0f, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0)))));
            PdfPCell2.Colspan = 2;
            PdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT;
            PdfPCell2.HorizontalAlignment = 1;
            Table.AddCell(PdfPCell2);

            PdfPCell PdfPCell3 = new PdfPCell(new Phrase("Class Teacher : ", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 7.0f, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0)))));
            PdfPCell3.Colspan = dt.Columns.Count;
            PdfPCell3.HorizontalAlignment = Element.ALIGN_RIGHT;
            PdfPCell3.HorizontalAlignment = 1;
            Table.AddCell(PdfPCell3);

            for (int column = 0; column < dt.Columns.Count; column++)
            {
                PdfPCell = new PdfPCell(new Phrase(new Chunk(TblHeaders[column], Heading)));

                Table.TotalWidth = 570f;
                Table.LockedWidth = true;
                Table.SetWidths(widths);
                Table.AddCell(PdfPCell);
            }
            //How add the data from datatable to pdf table
            for (int rows = 0; rows < dt.Rows.Count; rows++)
            {
                for (int column = 0; column < dt.Columns.Count; column++)
                {
                    PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows][column].ToString(), Content)));
                    Table.AddCell(PdfPCell);
                }
            }

            Table.SpacingBefore = 7f; // Give some space after the text or it may overlap the table  
            pdfDoc.Add(HeadingTable); // add HeadingTable to the document
            pdfDoc.Add(Table); // add Table to the document
            pdfDoc.Close();
            pdfWriter.Close();

            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Title + ".pdf");
            Response.BinaryWrite(pdfStream.ToArray());
            Response.End();

        }

        public void ExportToPDF_Assess360BulkEntry(string Title, string[] TblHeaders, float[] widths, DataTable dt, Assess360BulkInsert abi)
        {
            Document document = new Document(PageSize.A4, 10.25f, 10.25f, 10.5f, 10.5f);

            iTextSharp.text.Image LogonaceImage;
            string LogonaceImagePath = ConfigurationManager.AppSettings["AppLogos"] + "logonace.jpg";

            LogonaceImage = iTextSharp.text.Image.GetInstance(LogonaceImagePath);
            LogonaceImage.ScaleAbsolute(50, 50);
            iTextSharp.text.Image LogoImage;
            string ImagePath = ConfigurationManager.AppSettings["RptCard"] + "logo.jpg";

            LogoImage = iTextSharp.text.Image.GetInstance(ImagePath);
            LogoImage.ScaleAbsolute(50, 50);
            // For PDF export we are using the free open-source iTextSharp library.
            string month = DateTime.Now.ToString("MMMM");
            Document pdfDoc = new Document();
            pdfDoc.AddTitle(Title);
            MemoryStream pdfStream = new MemoryStream();
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, pdfStream);

            pdfDoc.Open();//Open Document to write
            pdfDoc.NewPage();

            Font Heading = FontFactory.GetFont("ARIAL", 6, BaseColor.BLACK);
            Font Content = FontFactory.GetFont("ARIAL", 6);

            PdfPTable HeadingTable = new PdfPTable(11);

            PdfPCell Cell1 = new PdfPCell();
            Cell1.Border = 0;
            Cell1.AddElement(LogonaceImage);
            Cell1.HorizontalAlignment = Element.ALIGN_LEFT;
            Cell1.Colspan = 10;
            HeadingTable.AddCell(Cell1);

            PdfPCell Cell3 = new PdfPCell();
            Cell3.Border = 0;
            Cell3.AddElement(LogoImage);
            Cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadingTable.AddCell(Cell3);

            HeadingTable.TotalWidth = 570f;
            HeadingTable.LockedWidth = true;
            HeadingTable.SetWidths(new float[] { 60f, 80f, 70f, 70f, 60f, 95f, 40f, 50f, 100f, 80f, 110f });

            PdfPTable HeadDetails = new PdfPTable(11);
            PdfPTable MainDetailsTable = new PdfPTable(11);
            PdfPTable Table = new PdfPTable(dt.Columns.Count);
            HeadDetails.TotalWidth = 570f;
            HeadDetails.LockedWidth = true;
            HeadDetails.SetWidths(new float[] { 60f, 80f, 70f, 70f, 60f, 95f, 40f, 50f, 100f, 80f, 110f });

            MainDetailsTable.TotalWidth = 570f;
            MainDetailsTable.LockedWidth = true;
            MainDetailsTable.SetWidths(new float[] { 60f, 80f, 70f, 70f, 60f, 95f, 40f, 50f, 100f, 80f, 110f });

            PdfPCell PdfPCell = new PdfPCell(new Phrase("The Indian Public School", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 10.0f, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0)))));
            PdfPCell.Colspan = dt.Columns.Count;
            PdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            HeadDetails.AddCell(PdfPCell);

            PdfPCell PdfPCell0 = new PdfPCell(new Phrase("Assess 360 Bulk Entry ", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 8.0f, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0)))));
            PdfPCell0.Colspan = dt.Columns.Count;
            PdfPCell0.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell0.HorizontalAlignment = 1;
            HeadDetails.AddCell(PdfPCell0);

            PdfPCell PdfPCell1 = new PdfPCell(new Phrase("Staff ", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 7.0f, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0)))));
            PdfPCell1.Colspan = 2;
            PdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            PdfPCell1.HorizontalAlignment = 1;
            MainDetailsTable.AddCell(PdfPCell1);

            PdfPCell PdfPCell2 = new PdfPCell(new Phrase("Assessment Type ", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 7.0f, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0)))));
            PdfPCell2.Colspan = 2;
            PdfPCell2.HorizontalAlignment = Element.ALIGN_RIGHT;
            PdfPCell2.HorizontalAlignment = 1;
            MainDetailsTable.AddCell(PdfPCell2);

            PdfPCell PdfPCell3 = new PdfPCell(new Phrase("Subject ", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 7.0f, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0)))));
            PdfPCell3.Colspan = 2;
            PdfPCell3.HorizontalAlignment = Element.ALIGN_RIGHT;
            PdfPCell3.HorizontalAlignment = 1;
            MainDetailsTable.AddCell(PdfPCell3);

            PdfPCell PdfPCell4 = new PdfPCell(new Phrase("Assignment Date", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 7.0f, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0)))));
            PdfPCell4.Colspan = 2;
            PdfPCell4.HorizontalAlignment = Element.ALIGN_RIGHT;
            PdfPCell4.HorizontalAlignment = 1;
            MainDetailsTable.AddCell(PdfPCell4);

            PdfPCell PdfPCell5 = new PdfPCell(new Phrase("Assignment Name", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 7.0f, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0)))));
            PdfPCell5.Colspan = 3;
            PdfPCell5.HorizontalAlignment = Element.ALIGN_RIGHT;
            PdfPCell5.HorizontalAlignment = 1;
            MainDetailsTable.AddCell(PdfPCell5);

            PdfPCell PdfPCell6 = new PdfPCell(new Phrase("" + abi.Staff + "", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 7.0f, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0)))));
            PdfPCell6.Colspan = 2;
            PdfPCell6.HorizontalAlignment = Element.ALIGN_RIGHT;
            PdfPCell6.HorizontalAlignment = 1;
            MainDetailsTable.AddCell(PdfPCell6);

            PdfPCell PdfPCell7 = new PdfPCell(new Phrase("" + abi.AssessmentType + "", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 7.0f, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0)))));
            PdfPCell7.Colspan = 2;
            PdfPCell7.HorizontalAlignment = Element.ALIGN_RIGHT;
            PdfPCell7.HorizontalAlignment = 1;
            MainDetailsTable.AddCell(PdfPCell7);

            PdfPCell PdfPCell8 = new PdfPCell(new Phrase("" + abi.Subject + "", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 7.0f, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0)))));
            PdfPCell8.Colspan = 2;
            PdfPCell8.HorizontalAlignment = Element.ALIGN_RIGHT;
            PdfPCell8.HorizontalAlignment = 1;
            MainDetailsTable.AddCell(PdfPCell8);

            PdfPCell PdfPCell9 = new PdfPCell(new Phrase("" + abi.IncidentDateString + "", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 7.0f, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0)))));
            PdfPCell9.Colspan = 2;
            PdfPCell9.HorizontalAlignment = Element.ALIGN_RIGHT;
            PdfPCell9.HorizontalAlignment = 1;
            MainDetailsTable.AddCell(PdfPCell9);

            PdfPCell PdfPCell10 = new PdfPCell(new Phrase("" + abi.AssignmentName + "", new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 7.0f, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0)))));
            PdfPCell10.Colspan = 3;
            PdfPCell10.HorizontalAlignment = Element.ALIGN_RIGHT;
            PdfPCell10.HorizontalAlignment = 1;
            MainDetailsTable.AddCell(PdfPCell10);

            for (int column = 0; column < dt.Columns.Count; column++)
            {
                PdfPCell = new PdfPCell(new Phrase(new Chunk(TblHeaders[column], Heading)));

                Table.TotalWidth = 570f;
                Table.LockedWidth = true;
                Table.SetWidths(widths);
                Table.AddCell(PdfPCell);
            }
            //How add the data from datatable to pdf table
            for (int rows = 0; rows < dt.Rows.Count; rows++)
            {
                for (int column = 0; column < dt.Columns.Count; column++)
                {
                    PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows][column].ToString(), Content)));
                    Table.AddCell(PdfPCell);
                }
            }

            Table.SpacingBefore = 7f; // Give some space after the text or it may overlap the table  
            pdfDoc.Add(HeadingTable); // add HeadingTable to the document
            pdfDoc.Add(HeadDetails);
            pdfDoc.Add(MainDetailsTable);
            pdfDoc.Add(Table); // add Table to the document
            pdfDoc.Close();
            pdfWriter.Close();

            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Title + ".pdf");
            Response.BinaryWrite(pdfStream.ToArray());
            Response.End();

        }

        public ActionResult GetSubjectsByGradeddl(string Campus, string Grade)
        {
            try
            {
                Dictionary<string, string> subject = new Dictionary<string, string>();
                Assess360Service assSrv = new Assess360Service();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrWhiteSpace(Grade) && Grade != "Select") { criteria.Add("Grade", Grade); }
                if (!string.IsNullOrWhiteSpace(Campus)) { criteria.Add("Campus", Campus); }
                Dictionary<long, IList<SubjectMaster>> subMaster = assSrv.GetSubjectMasterListWithPagingAndCriteria(0, 9999, "SubjectName", "Asc", criteria);
                if (subMaster != null && subMaster.Count > 0)
                {
                    foreach (SubjectMaster sub in subMaster.First().Value)
                    {
                        if (subject.Keys.Contains(sub.SubjectName))
                        { subject.Remove(sub.SubjectName); }
                        subject.Add(sub.SubjectName, sub.SubjectName);
                    }
                }
                return PartialView("Dropdown", subject);
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, "Assess360Policy"); }
            finally { }
        }

        public ActionResult GetStaffName(string Campus, string Grade, string Section, string Subject)
        {
            QAService QASrv = new QAService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(Campus)) { criteria.Add("Campus", Campus); }
            if (!string.IsNullOrWhiteSpace(Grade)) { criteria.Add("Grade", Grade); }
            if (!string.IsNullOrWhiteSpace(Section)) { criteria.Add("Section", Section); }
            if (!string.IsNullOrEmpty(Subject)) { criteria.Add("Subject", Subject); }
            Dictionary<long, IList<CampusBasedStaffDetails>> staffnamelist = QASrv.GetCampusBasedStaffDetailsListWithPagingAndCriteria(0, 1, string.Empty, string.Empty, criteria, null);
            if (staffnamelist != null && staffnamelist.Count > 0 && staffnamelist.FirstOrDefault().Key > 0)
            {
                var staffLst = (from items in staffnamelist.FirstOrDefault().Value
                                select new
                                {
                                    Text = items.StaffName,
                                    Value = items.StaffPreRegNumber
                                }).Distinct().ToList();

                return Json(staffLst, JsonRequestBehavior.AllowGet);
            }
            else
            { return Json(null, JsonRequestBehavior.AllowGet); }
        }

        #region Added By Prabakaran
        public ActionResult GetSubjectsByCampusGradeWithMultiplCriteria(string Campus, string Grade)
        {
            try
            {
                MastersService MstSrv = new MastersService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(Campus))
                {
                    var campusArr = Campus.Split(',');
                    criteria.Add("Campus", campusArr);
                }
                if (!string.IsNullOrWhiteSpace(Grade))
                {
                    var GradeArr = Grade.Split(',');
                    criteria.Add("Grade", GradeArr);
                }
                Dictionary<long, IList<CampusSubjectMaster>> subMaster = MstSrv.GetSubjectMasterByCampusListWithPagingAndCriteria(0, 9999, "Asc", "SubjectName", criteria);
                if (subMaster != null && subMaster.Count > 0)
                {
                    var subjectMstrLst = new
                    {
                        rows = (
                             from items in subMaster.FirstOrDefault().Value
                             select new
                             {
                                 Text = items.SubjectName,
                                 Value = items.SubjectName
                             }).Distinct().ToArray()
                    };

                    return Json(subjectMstrLst, JsonRequestBehavior.AllowGet);
                }
                else
                { return Json(null, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, "Assess360Policy"); }
        }
        public JsonResult FillSubjectByCampusAndGrade(string Campus, string Grade)
        {
            MastersService MstSrv = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(Campus))
                criteria.Add("Campus", Campus);
            if (!string.IsNullOrWhiteSpace(Grade))
                criteria.Add("Grade", Grade);
            Dictionary<long, IList<CampusSubjectMaster>> subMaster = MstSrv.GetSubjectMasterByCampusListWithPagingAndCriteria(0, 99999, "Asc", "SubjectName", criteria);
            if (subMaster != null && subMaster.FirstOrDefault().Key > 0)
            {
                var campusGradeObj = (
                                  from items in subMaster.FirstOrDefault().Value
                                  select new
                                  {
                                      Text = items.SubjectName,
                                      Value = items.SubjectName
                                  }).Distinct().ToList();
                return Json(campusGradeObj, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region GetMonthByGrade
        public ActionResult GetMonthValbyAcademicYearandGrade(string academicYear, string grade)
        {
            try
            {
                DateTime DateNow = DateTime.Now;
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                AttendanceService attServObj = new AttendanceService();
                string[] gradearray = grade.Split(',');
                if (gradearray.Contains("X") || gradearray.Contains("IX"))
                {
                    string[] Month = new string[DateNow.Month];
                    for (int i = (DateNow.Month - 1); i >= 0; i--)
                    {
                        if ((i + 1) < 10)
                        {
                            Month[i] = "0" + (i + 1).ToString();
                        }
                        else
                        {
                            Month[i] = (i + 1).ToString();
                        }
                    }
                    criteria.Add("MonthCode", Month);
                }
                else
                {
                    criteria.Add("Code", DateNow.Month == 10 ? "T" : DateNow.Month == 11 ? "E" : DateNow.Month == 12 ? "W" : DateNow.Month.ToString());
                }
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
                var jsondat = new { rows = (new { cell = new string[] { } }) };
                return Json(jsondat, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AttendancePolicy");
                throw ex;
            }
        }
        #endregion
        public JsonResult StaffGroupByCampus(string campus)
        {
            StaffManagementService sms = new StaffManagementService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(campus))
            {
                criteria.Add("Campus", campus);
            }
            criteria.Add("IsActive", true);
            Dictionary<long, IList<StaffGroupMaster>> StaffGroup = sms.GetStaffGroupMasterListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
            if (StaffGroup != null && StaffGroup.First().Key > 0 && StaffGroup.First().Value.Count > 0)
            {
                var StaffGroupList = (
                         from items in StaffGroup.First().Value
                         where items.Campus != null
                         select new
                         {
                             Text = items.GroupName,
                             Value = items.GroupName
                         }).Distinct().ToList();
                return Json(StaffGroupList, JsonRequestBehavior.AllowGet);
            }
            var jsondat = new { rows = (new { cell = new string[] { } }) };
            return Json(jsondat, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DepatmentByCampusandGroup(string campus, string GroupName)
        {
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(campus))
                criteria.Add("Campus", campus);
            if (!string.IsNullOrEmpty(GroupName))
                criteria.Add("GroupName", GroupName);
            Dictionary<long, IList<StaffDepartmentMaster>> StaffDepartmentMaster = ms.GetStaffDepartmentMasterListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
            if (StaffDepartmentMaster != null && StaffDepartmentMaster.First().Key > 0 && StaffDepartmentMaster.First().Value.Count > 0)
            {
                var StaffDepartmentMasterList = (
                         from items in StaffDepartmentMaster.First().Value
                         where items.Campus != null && items.GroupName != null
                         select new
                         {
                             Text = items.Department,
                             Vadue = items.Department
                         }).Distinct().ToList();
                return Json(StaffDepartmentMasterList, JsonRequestBehavior.AllowGet);
            }
            var jsondat = new { rows = (new { cell = new string[] { } }) };
            return Json(jsondat, JsonRequestBehavior.AllowGet);
        }
        public JsonResult StaffNameByCampusWithGroupandDepartment(string campus, string GroupName, string Department)
        {
            StaffManagementService sms = new StaffManagementService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(campus))
                criteria.Add("Campus", campus);
            if (!string.IsNullOrEmpty(GroupName))
                criteria.Add("StaffGroup", GroupName);
            if (!string.IsNullOrEmpty(Department))
                criteria.Add("Department", Department);
            criteria.Add("WorkingType", "Staff");
            Dictionary<long, IList<StaffDetailsView>> StaffDetails = sms.GetStaffDetailsViewListWithPaging(0, 9999, string.Empty, string.Empty, criteria);
            if (StaffDetails != null && StaffDetails.First().Key > 0 && StaffDetails.First().Value.Count > 0)
            {
                var StaffNameList = (
                         from items in StaffDetails.First().Value
                         where items.Campus != null && items.Department != null && items.StaffGroup != null
                         select new
                         {
                             Text = items.Name,
                             Value = items.Id
                         }).Distinct().ToList();
                return Json(StaffNameList, JsonRequestBehavior.AllowGet);
            }
            var jsondat = new { rows = (new { cell = new string[] { } }) };
            return Json(jsondat, JsonRequestBehavior.AllowGet);
        }
        public JsonResult StudentNameByCampusWithGrade(string campus, string Grade)
        {
            AdmissionManagementService ams = new AdmissionManagementService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(campus))
                criteria.Add("Campus", campus);
            if (!string.IsNullOrEmpty(Grade))
            {
                if (Grade == "IX" || Grade == "X" || Grade == "XI" || Grade == "XII")
                {
                    criteria.Add("AcademicYear", DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString());
                }
                else
                {
                    if (DateTime.Now.Month > 5)
                    {
                        criteria.Add("AcademicYear", DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString());
                    }
                    else
                    {
                        criteria.Add("AcademicYear", (DateTime.Now.Year - 1).ToString() + "-" + DateTime.Now.Year.ToString());
                    }
                }
            }
            criteria.Add("AdmissionStatus", "Registered");
            Dictionary<long, IList<StudentTemplateView>> StaffDetails = ams.GetStudentTemplateViewListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
            if (StaffDetails != null && StaffDetails.First().Key > 0 && StaffDetails.First().Value.Count > 0)
            {
                var StaffNameList = (
                         from items in StaffDetails.First().Value
                         where items.Campus != null && items.Grade != null && items.Section != null
                         select new
                         {
                             Text = items.Name,
                             Value = items.Id
                         }).Distinct().ToList();
                return Json(StaffNameList, JsonRequestBehavior.AllowGet);
            }
            var jsondat = new { rows = (new { cell = new string[] { } }) };
            return Json(jsondat, JsonRequestBehavior.AllowGet);
        }
        public JsonResult StaffSubGroupByCampusandGroupName(string campus, string GroupName)
        {
            StaffManagementService sms = new StaffManagementService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(campus))
            {
                criteria.Add("Campus", campus);
            }
            if (!string.IsNullOrEmpty(GroupName))
            {
                criteria.Add("GroupName", GroupName);
            }
            // Dictionary<long, IList<StaffMaster>> StaffNames = a360.GetStaffMasterListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);
            Dictionary<long, IList<StaffSubGroupMaster>> StaffSubGroupMaster = sms.GetStaffSubGroupMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            if (StaffSubGroupMaster != null && StaffSubGroupMaster.First().Key > 0 && StaffSubGroupMaster.First().Value.Count > 0)
            {
                var StaffSubGroupList = (
                         from items in StaffSubGroupMaster.First().Value
                         where items.Campus != null
                         select new
                         {
                             Text = items.SubGroupName,
                             Value = items.SubGroupName
                         }).Distinct().ToList();
                return Json(StaffSubGroupList, JsonRequestBehavior.AllowGet);
            }
            var jsondat = new { rows = (new { cell = new string[] { } }) };
            return Json(jsondat, JsonRequestBehavior.AllowGet);
        }
        public string GenerateRandomString(int length)
        {
            //It will generate string with combination of small,capital letters and numbers
            char[] charArr = "0123456789".ToCharArray();
            string randomString = string.Empty;
            Random objRandom = new Random();
            for (int i = 0; i < length; i++)
            {
                //Don't Allow Repetation of Characters
                int x = objRandom.Next(1, charArr.Length);
                if (!randomString.Contains(charArr.GetValue(x).ToString()))
                    randomString += charArr.GetValue(x);
                else
                    i--;
            }
            return randomString;
        }
        #region "Common SMS method List"
        public string GetSenderIdByCampus(string campus, string server)
        {
            try
            {
                UserService us = new UserService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(campus))
                {
                    criteria.Add("Campus", campus);
                }
                else
                {
                    criteria.Add("Campus", "IB MAIN");
                }
                criteria.Add("Server", server);
                IList<CampusEmailId> senderLst = us.GetCampusEmailListWithPaging(0, 1000, string.Empty, string.Empty, criteria);
                if (senderLst != null && senderLst.Count() > 0)
                    return senderLst[0].SenderID;
                else
                    return string.Empty;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion " END "
        #region added by john naveen

        public JsonResult GetActiveAcademicYear()
        {
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("IsActive", true);
            Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
            var academicObj = (
                              from items in AcademicyrMaster.First().Value
                              select new
                              {
                                  Text = items.AcademicYear,
                                  Value = items.AcademicYear
                              }).Distinct().ToList();
            return Json(academicObj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetActiveAcademicYearddl()
        {
            try
            {
                MastersService ms = new MastersService();
                Dictionary<string, string> aca = new Dictionary<string, string>();

                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("IsActive", true);
                Dictionary<long, IList<AcademicyrMaster>> AcaYear = ms.GetAcademicyrMasterListWithPagingAndCriteria(null, null, null, null, criteria);

                foreach (AcademicyrMaster ay in AcaYear.First().Value)
                {
                    aca.Add(ay.AcademicYear, ay.AcademicYear);
                }
                return PartialView("Dropdown", aca);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "Assess360Policy");
                throw ex;
            }
        }
        public JsonResult GetVehicleType()
        {
            try
            {
                TransportService ts = new TransportService();
                Dictionary<long, string> VehicleTypeList = new Dictionary<long, string>();
                Dictionary<string, object> criteria = new Dictionary<string, object>();

                Dictionary<long, IList<VehicleTypeMaster>> VTypeList = ts.GetVehicleTypeMasterListWithPagingAndCriteria(0, 9999, null, null, criteria);
                var vtypelist = (
                               from items in VTypeList.First().Value
                               select new
                               {
                                   Text = items.VehicleType,
                                   Value = items.Id
                               }).Distinct().ToList();
                return Json(vtypelist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        #endregion

        public JsonResult FillSubDepartment()
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            StaffManagementService smsObj = new StaffManagementService();
            Dictionary<long, IList<Staff_AttendanceReportConfiguration>> Staff_AttendanceReportConfigurationDetails = smsObj.GetStaff_AttendanceReportConfigurationDetailsListWithPaging(null, 99999, string.Empty, string.Empty, criteria);
            if (Staff_AttendanceReportConfigurationDetails != null && Staff_AttendanceReportConfigurationDetails.FirstOrDefault().Key > 0)
            {
                var SubDepartmentList = (
                         from items in Staff_AttendanceReportConfigurationDetails.FirstOrDefault().Value
                         where items.SubDepartment != null && items.SubDepartment != ""
                         select new
                         {
                             Text = items.SubDepartment,
                             Value = items.SubDepartment
                         }).Distinct().ToList();
                return Json(SubDepartmentList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult FillProgramme()
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            StaffManagementService smsObj = new StaffManagementService();
            Dictionary<long, IList<Staff_AttendanceReportConfiguration>> Staff_AttendanceReportConfigurationDetails = smsObj.GetStaff_AttendanceReportConfigurationDetailsListWithPaging(null, 99999, string.Empty, string.Empty, criteria);
            if (Staff_AttendanceReportConfigurationDetails != null && Staff_AttendanceReportConfigurationDetails.FirstOrDefault().Key > 0)
            {
                var ProgrammeList = (
                         from items in Staff_AttendanceReportConfigurationDetails.FirstOrDefault().Value
                         where items.Programme != null && items.Programme != ""
                         select new
                         {
                             Text = items.Programme,
                             Value = items.Programme
                         }).Distinct().ToList();
                return Json(ProgrammeList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        #region added By john naveen
        public JsonResult GetAcademicYearddl()
        {
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
            var academicObj = (
                              from items in AcademicyrMaster.First().Value
                              select new
                              {
                                  Text = items.AcademicYear,
                                  Value = items.FormId
                              }).Distinct().ToList();
            return Json(academicObj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCampusGradeddl(long? FormId)
        {
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("CampusMaster.FormId", FormId);
            Dictionary<long, IList<CampusGradeMaster>> CampusgradeMasterList = ms.GetCampusGradeMasterListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
            var campusGradeObj = (
                              from items in CampusgradeMasterList.First().Value
                              select new
                              {
                                  Text = items.gradcod,
                                  Value = items.Id
                              }).Distinct().ToList();
            return Json(campusGradeObj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FillSectionBasedCampusGrade(string Campus, string Grade)
        {
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<string, object> LikeCriteria = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(Campus))
                criteria.Add("Campus", Campus);
            if (!string.IsNullOrEmpty(Grade))
                criteria.Add("Grade", Grade);
            Dictionary<long, IList<CampusWiseSectionMaster_vw>> CampusWiseSectionMasterList = ms.GetCampusWiseSectionMaster_vwListWithExactAndLikeSearchCriteriaWithCount(0, 999999, null, null, criteria, LikeCriteria);
            if (CampusWiseSectionMasterList != null && CampusWiseSectionMasterList.FirstOrDefault().Key > 0)
            {
                var campusGradeObj = (
                                  from items in CampusWiseSectionMasterList.FirstOrDefault().Value
                                  select new
                                  {
                                      Text = items.Section,
                                      Value = items.Section
                                  }).Distinct().ToList();
                return Json(campusGradeObj, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Campus load based on AppCode By John Naveen
        public JsonResult FillCampusName()
        {
            string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
            MastersService ms = new MastersService();
            UserService us = new UserService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("UserId", userid);
            criteria.Add("AppCode", "STFIOSUMRY");
            string[] BranchList = new string[0] { };
            Dictionary<long, IList<UserAppRole>> listObj = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            if (listObj != null && listObj.FirstOrDefault().Key > 0)
            {
                BranchList = (from item in listObj.FirstOrDefault().Value
                                  where item.DeptCode == "FEES / FINANCE" || item.DeptCode == "HR"
                                  select item.BranchCode).ToArray();
                var Rolearr = (from item in listObj.FirstOrDefault().Value
                               select item.RoleCode).ToArray();
                if (Rolearr != null && Rolearr.Contains("Bio-All"))
                {

                }
                else if (BranchList != null && BranchList.Count() > 0)
                {
                    criteria.Add("Campus", BranchList);
                }
            }
            criteria.Clear();
            if (BranchList.Count() > 0)
            {
                criteria.Add("Name", BranchList);
            }
            Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            if (CampusMaster != null && CampusMaster.First().Value != null && CampusMaster.First().Value.Count > 0)
            {
                var BranchCodeList = (
                         from items in CampusMaster.First().Value
                         select new
                         {
                             Text = items.Name,
                             Value = items.Name
                         }).Distinct().ToList();
                return Json(BranchCodeList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        public JsonResult FillCampusNameByMapping()
        {
            string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
            MastersService ms = new MastersService();
            UserService us = new UserService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("UserId", userid);
            criteria.Add("AppCode", "ATTCNFRPT");
            string[] BranchList = new string[0] { };
            Dictionary<long, IList<UserAppRole>> listObj = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            if (listObj != null && listObj.FirstOrDefault().Key > 0)
            {
                BranchList = (from item in listObj.FirstOrDefault().Value
                              where item.DeptCode == "FEES / FINANCE" || item.DeptCode == "HR"
                              select item.BranchCode).ToArray();
                var Rolearr = (from item in listObj.FirstOrDefault().Value
                               select item.RoleCode).ToArray();
                if (Rolearr != null && Rolearr.Contains("Bio-All"))
                {

                }
                else if (BranchList != null && BranchList.Count() > 0)
                {
                    criteria.Add("Campus", BranchList);
                }
            }
            criteria.Clear();
            if (BranchList.Count() > 0)
            {
                criteria.Add("Name", BranchList);
            }
            Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            if (CampusMaster != null && CampusMaster.First().Value != null && CampusMaster.First().Value.Count > 0)
            {
                var BranchCodeList = (
                         from items in CampusMaster.First().Value
                         select new
                         {
                             Text = items.Name,
                             Value = items.Name
                         }).Distinct().ToList();
                return Json(BranchCodeList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
//Basic code review completed by JP with Anbu 24 Mar 2014
//"REVISIT" need to be visited again