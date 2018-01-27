using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TIPS.Entities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TIPS.Entities.StaffManagementEntities;
using TIPS.Entities.AdmissionEntities;
using TIPS.Service;
using TIPS.ServiceContract;
using TIPS.Entities.EmployeeEntities;
using System.Web.UI;
using System.Web.UI.WebControls;
using TIPS.Entities.Attendance;
using System.Configuration;
using System.IO;

namespace CMS.Controllers
{
    public class EmployeeController : BaseController
    {

        public ActionResult Employee(string id)
        {
            try
            {
                FillViewBag();
                Session["status"] = "";
                StaffManagementService sms = new StaffManagementService();
                if (id == null)
                {
                    Dictionary<long, IList<StaffRequestNumDetails>> prd = sms.GetStaffRequestNumDetailsListWithPaging(0, 10000, string.Empty, string.Empty, null);

                    var id1 = prd.First().Value[0].PreRegNum + 1;
                    ViewBag.RequestNum = id1;
                    Session["Reqnum"] = id1;
                    ViewBag.Date = DateTime.Now.ToShortDateString();
                    ViewBag.Time = DateTime.Now.ToShortTimeString();
                    StaffRequestNumDetails srn = new StaffRequestNumDetails();
                    srn.PreRegNum = id1;

                    Session["status"] = "New Registration";
                    ViewBag.admissionstatus = "New Registration";
                    srn.Id = prd.First().Value[0].Id;
                    srn.Date = DateTime.Now.ToShortDateString();
                    srn.Time = DateTime.Now.ToShortTimeString();
                    ViewBag.initialpage = "yes";
                    sms.CreateOrUpdateStaffRequestNumDetails(srn);
                    ViewBag.HideValidation = "True";
                    return View();
                }
                else
                {
                    StaffDetails StaffDetails = new StaffDetails();
                    StaffDetails = sms.GetStaffDetailsId(Convert.ToInt32(id));

                    if (StaffDetails.Status == "SentForApproval")
                    {
                        Session["sentforapproval"] = "yes";
                    }
                    else
                    {
                        Session["sentforapproval"] = "";
                    }

                    StaffQualification staffqual = new StaffQualification();

                    {
                        IList<StaffQualification> staffqualificationList = new List<StaffQualification>();
                        staffqualificationList.Add(staffqual);
                        StaffDetails.StaffQualificationList = staffqualificationList;
                    }

                    StaffExperience staffexp = new StaffExperience();
                    {
                        IList<StaffExperience> staffexpList = new List<StaffExperience>();
                        staffexpList.Add(staffexp);
                        StaffDetails.StaffExperienceList = staffexpList;
                    }

                    StaffTraining stafftraining = new StaffTraining();
                    {
                        IList<StaffTraining> stafftrainingList = new List<StaffTraining>();
                        stafftrainingList.Add(stafftraining);
                        StaffDetails.StaffTrainingList = stafftrainingList;
                    }

                    //  UploadedFiles uploadedfile = new UploadedFiles();
                    UploadedFilesView uploadedfile = new UploadedFilesView();

                    //      if (StaffDetails.UploadedFilesList.Count == 0)
                    {
                        IList<UploadedFilesView> uploadedfileviewList = new List<UploadedFilesView>();
                        uploadedfileviewList.Add(uploadedfile);
                        StaffDetails.UploadedFilesList = uploadedfileviewList;
                    }
                    ViewBag.RequestNum = StaffDetails.PreRegNum;
                    ViewBag.Date = StaffDetails.CreatedDate;// DateTime.Now.ToShortDateString();
                    ViewBag.Time = StaffDetails.CreatedTime;// DateTime.Now.ToShortTimeString();
                    Session["Reqnum"] = StaffDetails.PreRegNum;
                    Session["status"] = StaffDetails.Status;
                    ViewBag.admissionstatus = StaffDetails.Status;
                    ViewBag.nam = StaffDetails.Name;
                    ViewBag.idnum = StaffDetails.IdNumber;
                    ViewBag.desig = StaffDetails.Designation;
                    ViewBag.campus = StaffDetails.Campus;
                    return View(StaffDetails);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
            //   return View();
        }

        [HttpPost]
        public ActionResult Employee(StaffDetailsView sd, string test)
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();

                Dictionary<long, IList<StaffDetails>> staffdetails;
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum", Convert.ToInt32(Session["Reqnum"]));
                staffdetails = sms.GetStaffDetailsListWithPaging(0, 10000, string.Empty, string.Empty, criteria);

                if (staffdetails != null && staffdetails.First().Value != null && staffdetails.First().Value.Count > 0)
                {
                    if (staffdetails.First().Value[0].Status == "Sent For Approval")
                    {
                        Session["sentforapproval"] = "yes";
                    }
                    else
                    {
                        Session["sentforapproval"] = "";
                    }
                }
                if (staffdetails.First().Value.Count != 0)
                {
                    sd.Id = staffdetails.First().Value[0].Id;
                }

                sd.PreRegNum = Convert.ToInt32(Session["Reqnum"]);
                if (Request.Form["btnSave"] == "Save")
                {
                    if (staffdetails != null && staffdetails.First().Value != null && staffdetails.First().Value.Count > 0)
                    {
                        sd.Status = staffdetails.First().Value[0].Status;
                        Session["status"] = staffdetails.First().Value[0].Status;
                        ViewBag.admissionstatus = staffdetails.First().Value[0].Status;
                    }
                    else
                    {
                        sd.Status = "New Registration";
                        sd.WorkingType = "Employee";
                        Session["status"] = sd.Status;// staffdetails.First().Value[0].Status;
                        sd.CreatedDate = DateTime.Now.ToShortDateString();
                        sd.CreatedTime = DateTime.Now.ToShortTimeString();
                        ViewBag.admissionstatus = sd.Status;
                    }
                    ViewBag.save = "yes";
                    if (sd.DocCheck == "yes") { ViewBag.doccheck = "yes"; }
                    else if (sd.QualCheck == "yes") { ViewBag.qualcheck = "yes"; }
                }

                if (Request.Form["btnsentforapproval"] == "Send For Approval")
                {
                    sd.Status = "Sent For Approval";
                    Session["status"] = sd.Status;
                    ViewBag.admissionstatus = sd.Status;
                    ViewBag.sentforappr = "yes";
                }

                if (Request.Form["btnapprove"] == "Approve")
                {
                    MastersService ms = new MastersService();
                    Dictionary<string, object> criteriam = new Dictionary<string, object>();
                    criteriam.Add("Designation", sd.Designation);
                    Dictionary<long, IList<DesignationMaster>> DesignationMaster = ms.GetDesignationMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteriam);
                    TIPS.Service.UserService us = new UserService();

                    if (sd.Status == "Registered")
                    {
                        Dictionary<string, object> criteria2 = new Dictionary<string, object>();
                        criteria2.Add("PreRegNum", Convert.ToInt64(Session["Reqnum"]));   // to check if staff is already registered or not while changing from inactive to registered
                        Dictionary<long, IList<StaffDetailsView>> IdCheck = sms.GetStaffDetailsViewListWithPaging(0, 0, string.Empty, string.Empty, criteria2);

                        if (IdCheck.First().Value[0].IdNumber == null)
                        {
                            StaffIdNumber sid = sms.GetStaffIdnumber(1);
                            sd.IdNumber = "TIPS-" + sid.StaffIdnumber + "";
                            sid.StaffIdnumber = sid.StaffIdnumber + 1;
                            sid.Id = 1;
                            sms.CreateOrUpdateStaffIdnumber(sid);
                        }
                        User usr = new User();
                        if (staffdetails != null && staffdetails.First().Value != null && staffdetails.First().Value.Count > 0)
                        {
                            if (staffdetails.First().Value[0].StaffUserName != null)
                            {
                                usr = us.GetUserByUserId(staffdetails.First().Value[0].StaffUserName);
                                if (usr != null)
                                {
                                    if (staffdetails.First().Value[0].CreatedDate == null)
                                    {
                                        usr.CreatedDate = DateTime.Now;
                                    }
                                    usr.ModifiedDate = DateTime.Now;
                                    usr.IsActive = true;
                                    us.CreateOrUpdateUser(usr);
                                }
                            }
                        }
                        ViewBag.regtrd = "yes";
                        ViewBag.regtrdid = sd.IdNumber;
                    }
                    if (sd.Status == "Inactive")
                    {
                        User usr = new User();
                        if (staffdetails != null && staffdetails.First().Value != null && staffdetails.First().Value.Count > 0)
                        {
                            if (staffdetails.First().Value[0].StaffUserName != null)
                            {
                                usr = us.GetUserByUserId(staffdetails.First().Value[0].StaffUserName);

                                if (usr != null)
                                {
                                    usr.ModifiedDate = DateTime.Now;
                                    usr.IsActive = false;

                                    if (staffdetails.First().Value[0].CreatedDate == null)
                                    {
                                        usr.CreatedDate = DateTime.Now;
                                    }
                                    us.CreateOrUpdateUser(usr);
                                }
                            }
                        }
                    }
                    Session["status"] = sd.Status;
                    ViewBag.admissionstatus = sd.Status;

                }

                ViewBag.Date = sd.CreatedDate;// DateTime.Now.ToShortDateString();
                ViewBag.Time = sd.CreatedTime;// DateTime.Now.ToShortTimeString();

                ViewBag.nam = sd.Name;// StaffDetails.Name;
                ViewBag.idnum = sd.IdNumber;// StaffDetails.IdNumber;
                ViewBag.desig = sd.Designation;// StaffDetails.Designation;
                ViewBag.campus = sd.Campus;// StaffDetails.Campus;
                //  sms.CreateOrUpdateStaffDetails(sd);
                sms.CreateOrUpdateStaffDetailsView(sd);
                ViewBag.RequestNum = Session["Reqnum"];

                FillViewBag();
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public string FillViewBag()
        {
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            var usrcmp = Session["UserCampus"] as IEnumerable<string>;
            if (usrcmp.Count() != 0)
            {
                if (usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
                {
                    criteria.Add("Name", usrcmp);
                }
            }
            Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            criteria.Clear();
            Dictionary<long, IList<BloodGroupMaster>> BloodGroupMaster = ms.GetBloodGroupMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            Dictionary<long, IList<IssueGroupMaster>> IssueGroupMaster = ms.GetIssueGroupMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            Dictionary<long, IList<StaffDepartmentMaster>> DepartmentMaster = ms.GetStaffDepartmentMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            criteria.Clear();
            criteria.Add("DocumentFor", "Employee");
            Dictionary<long, IList<DocumentTypeMaster>> DocumentTypeMaster = ms.GetDocumentTypeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            criteria.Clear();
            Dictionary<long, IList<DesignationMaster>> DesignationMaster = ms.GetDesignationMasterListWithPagingAndCriteria(0, 200, string.Empty, string.Empty, criteria);
            Dictionary<long, IList<StaffTypeMaster>> StaffTypeMaster = ms.GetStaffTypeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.campusddl = CampusMaster.First().Value;
            ViewBag.bloodgrpddl = BloodGroupMaster.First().Value;
            ViewBag.departmentddl = DepartmentMaster.First().Value;
            ViewBag.documentddl = DocumentTypeMaster.First().Value;
            ViewBag.designationddl = DesignationMaster.First().Value;
            ViewBag.stafftypeddl = StaffTypeMaster.First().Value;
            return "";
        }

        public ActionResult EmployeeDetails()
        {
            FillViewBag();
            #region BreadCrumb
            string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
            #endregion
            return View();
        }

        public ActionResult EmployeeDetailsJqGrid(string PreRegNum, string Name, string IdNumber, string campus, string designation, string department, string Gender, string Status, string stat, string appname, string idno, string flag, string type, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                TIPS.Entities.User sessionUser = (TIPS.Entities.User)Session["objUser"];
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                string colName = string.Empty; string[] values = new string[1];
                {
                    var usrcmp = Session["UserCampus"] as IEnumerable<string>;

                    criteria.Add("WorkingType", "Employee");
                    if (!string.IsNullOrWhiteSpace(campus))
                    {
                        criteria.Add("Campus", campus);
                    }
                    else
                    {
                        if (usrcmp != null && usrcmp.Count() != 0)
                        {
                            if (usrcmp.First() != null)   // to check if the usrcmp obj is null or with data
                            {
                                criteria.Add("Campus", usrcmp);
                            }
                        }

                    }
                    if (!string.IsNullOrWhiteSpace(designation)) criteria.Add("Designation", designation);
                    if (!string.IsNullOrWhiteSpace(department)) criteria.Add("Department", department);
                    if (!string.IsNullOrWhiteSpace(appname)) criteria.Add("Name", appname);
                    if (!string.IsNullOrWhiteSpace(idno)) criteria.Add("IdNumber", idno);
                    if (!string.IsNullOrWhiteSpace(PreRegNum)) criteria.Add("PreRegNum", Convert.ToInt32(PreRegNum));//Tool bar search
                    if (!string.IsNullOrWhiteSpace(Name)) criteria.Add("Name", Name);//Tool bar search
                    if (!string.IsNullOrWhiteSpace(IdNumber)) criteria.Add("IdNumber", IdNumber);//Tool bar search
                    if (!string.IsNullOrWhiteSpace(Gender)) criteria.Add("Gender", Gender);//Tool bar search
                    if (!string.IsNullOrWhiteSpace(Status)) criteria.Add("Status", Status);//Tool bar search

                    if (type == "new")
                    {
                        if (Session["staffapproverrole"].ToString() == "EMP-APP")
                        {
                            string[] status = { "Sent For Approval", "On Hold", "Call For Interview" };
                            criteria.Add("Status", status);
                        }
                        else
                        {
                            string[] status = { "New Registration", "Sent For Approval", "On Hold", "Call For Interview" };
                            criteria.Add("Status", status);
                        }
                    }

                    if (type == "old")
                    {
                        if (string.IsNullOrWhiteSpace(stat))
                        {
                            string[] status = { "Registered" };
                            criteria.Add("Status", status);
                        }
                        else
                        {
                            string[] status = { stat.ToString() };
                            criteria.Add("Status", status);
                        }
                    }

                    if (string.Equals(sessionUser.UserType != null ? sessionUser.UserType : string.Empty, "Staff", StringComparison.CurrentCultureIgnoreCase))
                    {
                        colName = "IdNumber";
                        values[0] = sessionUser.EmployeeId != null ? sessionUser.EmployeeId : string.Empty;
                    }
                    Dictionary<long, IList<TIPS.Entities.StaffManagementEntities.StaffDetailsView>> StaffDetails;

                    StaffDetails = sms.GetStaffDetailsListWithPagingAndCriteriaWithAlias(page - 1, rows, sidx, sord, colName, values, criteria, null);

                    if (StaffDetails.Count > 0)
                    {
                        long totalrecords = StaffDetails.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);

                        var jsondat = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,

                            rows = (from items in StaffDetails.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                                    items.PreRegNum.ToString(),
                             
                            String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '/Employee/Employee?id="+items.Id+"'  >{0}</a>",items.Name),
                            items.IdNumber,      
                            items.Campus,
                            items.Designation,
                            items.Department,
                            items.Gender,
                            items.Status,
                            items.Id.ToString()
                            }
                                    })
                        };
                        return Json(jsondat, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var AssLst = new { rows = (new { cell = new string[] { } }) };
                        return Json(AssLst, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        public ActionResult NewEmployeeRegistration()
        {
            FillViewBag();
            #region BreadCrumb
            string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
            #endregion
            return View();
        }

        public ActionResult EmployeeAttendance()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    ViewBag.campusddl = CampusMasterFunc();
                    ViewBag.Success = "";
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
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult EmployeeAttendance(EmployeeAttendance ea)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    EmployeeService es = new EmployeeService();
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    ea.CreatedDate = DateTime.Now;
                    ea.CreatedBy = userId;
                    ea.ModifiedDate = DateTime.Now;
                    ea.ModifiedBy = userId;
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("Campus", ea.Campus);
                    criteria.Add("EmployeeIdNo", ea.EmployeeIdNo);
                    criteria.Add("EmployeeName", ea.EmployeeName);
                    criteria.Add("AbsentDate", ea.AbsentDate);
                    ViewBag.campusddl = CampusMasterFunc();
                    Dictionary<long, IList<EmployeeAttendance>> EmployeeAttendance = es.GetEmployeeAttendanceDetailsListWithPagingAndCriteria(0, 9999, "", "", criteria);
                    if (EmployeeAttendance != null && EmployeeAttendance.FirstOrDefault().Value != null && EmployeeAttendance.FirstOrDefault().Value.Count > 0)
                    {
                        ViewBag.Success = "No";
                        return View();
                    }
                    else
                    {
                        es.CreateOrUpdateEmployeeAttendanceList(ea);
                        ViewBag.Success = "Yes";
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult EmployeeAttendanceDetailsListJqGrid(string AbsentDate, string CreatedDate, EmployeeAttendance ea, int? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    EmployeeService ts = new EmployeeService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (!string.IsNullOrWhiteSpace(ea.Campus)) { criteria.Add("Campus", ea.Campus); }
                    if (!string.IsNullOrWhiteSpace(ea.EmployeeIdNo)) { criteria.Add("EmployeeIdNo", ea.EmployeeIdNo); }
                    if (!string.IsNullOrWhiteSpace(ea.EmployeeName)) { criteria.Add("EmployeeName", ea.EmployeeName); }
                    if (!string.IsNullOrWhiteSpace(ea.AbsentType)) { criteria.Add("AbsentType", ea.AbsentType); }
                    if (!string.IsNullOrWhiteSpace(AbsentDate))
                    {
                        AbsentDate = AbsentDate.Trim();
                        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                        DateTime[] AttDatefromto = new DateTime[2];
                        AttDatefromto[0] = DateTime.Parse(AbsentDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        string To = string.Format("{0:dd/MM/yyyy}", AttDatefromto[0]);
                        AttDatefromto[1] = Convert.ToDateTime(To + " " + "23:59:59");
                        criteria.Add("AbsentDate", AttDatefromto);
                    }
                    if (!string.IsNullOrWhiteSpace(CreatedDate))
                    {
                        CreatedDate = CreatedDate.Trim();
                        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                        DateTime[] CreatedDatefromto = new DateTime[2];
                        CreatedDatefromto[0] = DateTime.Parse(CreatedDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        string ToCreatedDate = string.Format("{0:dd/MM/yyyy}", CreatedDatefromto[0]);
                        CreatedDatefromto[1] = Convert.ToDateTime(ToCreatedDate + " " + "23:59:59");
                        criteria.Add("CreatedDate", CreatedDatefromto);
                    }
                    Dictionary<long, IList<EmployeeAttendance>> AttendanceDetails = ts.GetEmployeeAttendanceDetailsListWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);
                    if (AttendanceDetails != null && AttendanceDetails.Count > 0)
                    {
                        long totalrecords = AttendanceDetails.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var DriverAT = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in AttendanceDetails.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.Id.ToString(),
                               items.Campus,
                               items.EmployeeName, 
                               items.EmployeeIdNo,
                               items.AbsentDate!=null? items.AbsentDate.ToString("dd/MM/yyyy"):"",
                               items.AbsentType.ToString(),
                               items.CreatedDate!=null?items.CreatedDate.ToString("dd/MM/yyyy"):"",
                               //items.CreatedBy!=null?us.GetUserNameByUserId(items.CreatedBy):"",
                               items.CreatedBy
                            }
                                    })
                        };
                        return Json(DriverAT, JsonRequestBehavior.AllowGet);
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
        //public ActionResult DeleteDriverAttendanceDetails(DriverAttendance DAT)
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            TransportBC tbc = new TransportBC();
        //            if (DAT.Id > 0)
        //            {
        //                tbc.DeleteDriverAttendancevalue(DAT);
        //            }
        //            return Json(null, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
        //        throw ex;
        //    }
        //}

        public JsonResult GetAutoCompleteEmployeeNamesByCampus(string Campus, string term)
        {
            try
            {
                TransportService ts = new TransportService();
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Campus", Campus);
                criteria.Add("Name", term);
                criteria.Add("Status", "Registered");
                criteria.Add("WorkingType", "Employee");
                Dictionary<long, IList<StaffDetailsView>> EmployeeList = sms.GetStaffDetailsViewListWithPaging(0, 0, string.Empty, string.Empty, criteria);
                var EmployeeNames = (from u in EmployeeList.First().Value
                                   where u.Name != null && u.Name != ""
                                   select u.Name).Distinct().ToList();
                return Json(EmployeeNames, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult GetEmployeeDetailsByNameAndCampus(string Campus, string EmployeeName)
        {
            try
            {
                TransportService ts = new TransportService();
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("Campus", Campus);
                criteria.Add("Name", EmployeeName);
                criteria.Add("WorkingType", "Employee");
                Dictionary<long, IList<StaffDetailsView>> EmployeeList = sms.GetStaffDetailsViewListWithPaging(0, 0, string.Empty, string.Empty, criteria);
                if (EmployeeList != null && EmployeeList.FirstOrDefault().Value != null && EmployeeList.FirstOrDefault().Key > 0)
                {
                    return Json(EmployeeList.FirstOrDefault().Value[0], JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult EmployeeAttendanceReport()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    DateTime DateNow = DateTime.Now;
                    AttendanceService attServObj = new AttendanceService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<MonthMasterForAttendance>> monthMaster = attServObj.GetMonthMasterForAttendanceListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                    ViewBag.monthMaster = monthMaster.First().Value;
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

        public ActionResult GetEmployeeAttendanceReportsJqGrid(string campus, string employeeid, string employeename, string searchmonth, int year, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                DateTime DateNow = DateTime.Now;
                int reduceCount = 0;
                int acaYear = 0;
                if (string.IsNullOrEmpty(campus))
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    acaYear = year;
                    DateTime startDate = new DateTime(acaYear, Convert.ToInt32(searchmonth), 1);
                    DateTime endDate = new DateTime(acaYear, Convert.ToInt32(searchmonth), DateNow.Day);
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrWhiteSpace(campus)) { criteria.Add("Campus", campus); }
                    criteria.Add("Status", "Registered");
                    criteria.Add("WorkingType", "Employee");
                    sord = sord == "desc" ? "Desc" : "Asc";
                    EmployeeService es = new EmployeeService();
                    StaffManagementService sms = new StaffManagementService();
                    Dictionary<long, IList<EmployeeAttendanceReport>> EmployeeList = es.GetEmployeeAttendanceReportDetailsListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                    criteria.Clear();

                    Dictionary<long, IList<EmployeeAttendance>> AttendanceList = es.GetEmployeeAttendanceDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria);
                    List<EmployeeAttendance> alreadyExists = AttendanceList.FirstOrDefault().Value.ToList();
                    IEnumerable<long> blkLong = from p in alreadyExists
                                                orderby p.PreRegNum ascending
                                                select p.PreRegNum;
                    long[] attids = blkLong.ToArray();

                    foreach (EmployeeAttendanceReport a in EmployeeList.FirstOrDefault().Value)
                    {
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
                            Dictionary<long, IList<EmployeeAttendance>> AbsentList = es.GetEmployeeAttendanceDetailsListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteria1);
                            //a.AbsentCountList = AbsentList.First().Value.Count;
                            List<EmployeeAttendance> Absentdate = AbsentList.FirstOrDefault().Value.ToList();
                            var AbsentLong = (from p in Absentdate
                                              where p.AbsentType == "Absent"
                                              orderby p.PreRegNum ascending
                                              select p.AbsentDate).ToArray();
                            a.AbsentCountList = AbsentLong.Count();

                            var Leave = (from p in Absentdate
                                         where p.AbsentType == "Leave"
                                         orderby p.PreRegNum ascending
                                         select p.AbsentDate).ToArray();
                            a.LeaveCountList = Leave.Count();

                            // Default Prest List
                            a.Date1 = "<b style='color:Green'>P</b>"; a.Date2 = "<b style='color:Green'>P</b>"; a.Date3 = "<b style='color:Green'>P</b>"; a.Date4 = "<b style='color:Green'>P</b>"; a.Date5 = "<b style='color:Green'>P</b>"; a.Date6 = "<b style='color:Green'>P</b>"; a.Date7 = "<b style='color:Green'>P</b>"; a.Date8 = "<b style='color:Green'>P</b>"; a.Date9 = "<b style='color:Green'>P</b>"; a.Date10 = "<b style='color:Green'>P</b>"; a.Date11 = "<b style='color:Green'>P</b>"; a.Date12 = "<b style='color:Green'>P</b>"; a.Date13 = "<b style='color:Green'>P</b>"; a.Date14 = "<b style='color:Green'>P</b>"; a.Date15 = "<b style='color:Green'>P</b>";
                            a.Date16 = "<b style='color:Green'>P</b>"; a.Date17 = "<b style='color:Green'>P</b>"; a.Date18 = "<b style='color:Green'>P</b>"; a.Date19 = "<b style='color:Green'>P</b>"; a.Date20 = "<b style='color:Green'>P</b>"; a.Date21 = "<b style='color:Green'>P</b>"; a.Date22 = "<b style='color:Green'>P</b>"; a.Date23 = "<b style='color:Green'>P</b>"; a.Date24 = "<b style='color:Green'>P</b>"; a.Date25 = "<b style='color:Green'>P</b>"; a.Date26 = "<b style='color:Green'>P</b>"; a.Date27 = "<b style='color:Green'>P</b>"; a.Date28 = "<b style='color:Green'>P</b>"; a.Date29 = "<b style='color:Green'>P</b>"; a.Date30 = "<b style='color:Green'>P</b>";
                            a.Date31 = "<b style='color:Green'>P</b>";
                            // hide default present using this condition only for current month
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

                            // Only for Absent List 
                            for (var i = 0; i < AbsentLong.Length; i++)
                            {
                                string Abvalue = AbsentLong[i].ToString("dd");

                                switch (Abvalue)
                                {
                                    case "01": { if (ExportType == "Excel") { a.Date1 = "<b style='color:Red'>A</b>"; } else { a.Date1 = "<b style='color:Red'>A</b>"; } } break;
                                    case "02": { if (ExportType == "Excel") { a.Date2 = "<b style='color:Red'>A</b>"; } else { a.Date2 = "<b style='color:Red'>A</b>"; } } break;
                                    case "03": { if (ExportType == "Excel") { a.Date3 = "<b style='color:Red'>A</b>"; } else { a.Date3 = "<b style='color:Red'>A</b>"; } } break;
                                    case "04": { if (ExportType == "Excel") { a.Date4 = "<b style='color:Red'>A</b>"; } else { a.Date4 = "<b style='color:Red'>A</b>"; } } break;
                                    case "05": { if (ExportType == "Excel") { a.Date5 = "<b style='color:Red'>A</b>"; } else { a.Date5 = "<b style='color:Red'>A</b>"; } } break;
                                    case "06": { if (ExportType == "Excel") { a.Date6 = "<b style='color:Red'>A</b>"; } else { a.Date6 = "<b style='color:Red'>A</b>"; } } break;
                                    case "07": { if (ExportType == "Excel") { a.Date7 = "<b style='color:Red'>A</b>"; } else { a.Date7 = "<b style='color:Red'>A</b>"; } } break;
                                    case "08": { if (ExportType == "Excel") { a.Date8 = "<b style='color:Red'>A</b>"; } else { a.Date8 = "<b style='color:Red'>A</b>"; } } break;
                                    case "09": { if (ExportType == "Excel") { a.Date9 = "<b style='color:Red'>A</b>"; } else { a.Date9 = "<b style='color:Red'>A</b>"; } } break;
                                    case "10": { if (ExportType == "Excel") { a.Date10 = "<b style='color:Red'>A</b>"; } else { a.Date10 = "<b style='color:Red'>A</b>"; } } break;
                                    case "11": { if (ExportType == "Excel") { a.Date11 = "<b style='color:Red'>A</b>"; } else { a.Date11 = "<b style='color:Red'>A</b>"; } } break;
                                    case "12": { if (ExportType == "Excel") { a.Date12 = "<b style='color:Red'>A</b>"; } else { a.Date12 = "<b style='color:Red'>A</b>"; } } break;
                                    case "13": { if (ExportType == "Excel") { a.Date13 = "<b style='color:Red'>A</b>"; } else { a.Date13 = "<b style='color:Red'>A</b>"; } } break;
                                    case "14": { if (ExportType == "Excel") { a.Date14 = "<b style='color:Red'>A</b>"; } else { a.Date14 = "<b style='color:Red'>A</b>"; } } break;
                                    case "15": { if (ExportType == "Excel") { a.Date15 = "<b style='color:Red'>A</b>"; } else { a.Date15 = "<b style='color:Red'>A</b>"; } } break;
                                    case "16": { if (ExportType == "Excel") { a.Date16 = "<b style='color:Red'>A</b>"; } else { a.Date16 = "<b style='color:Red'>A</b>"; } } break;
                                    case "17": { if (ExportType == "Excel") { a.Date17 = "<b style='color:Red'>A</b>"; } else { a.Date17 = "<b style='color:Red'>A</b>"; } } break;
                                    case "18": { if (ExportType == "Excel") { a.Date18 = "<b style='color:Red'>A</b>"; } else { a.Date18 = "<b style='color:Red'>A</b>"; } } break;
                                    case "19": { if (ExportType == "Excel") { a.Date19 = "<b style='color:Red'>A</b>"; } else { a.Date19 = "<b style='color:Red'>A</b>"; } } break;
                                    case "20": { if (ExportType == "Excel") { a.Date20 = "<b style='color:Red'>A</b>"; } else { a.Date20 = "<b style='color:Red'>A</b>"; } } break;
                                    case "21": { if (ExportType == "Excel") { a.Date21 = "<b style='color:Red'>A</b>"; } else { a.Date21 = "<b style='color:Red'>A</b>"; } } break;
                                    case "22": { if (ExportType == "Excel") { a.Date22 = "<b style='color:Red'>A</b>"; } else { a.Date22 = "<b style='color:Red'>A</b>"; } } break;
                                    case "23": { if (ExportType == "Excel") { a.Date23 = "<b style='color:Red'>A</b>"; } else { a.Date23 = "<b style='color:Red'>A</b>"; } } break;
                                    case "24": { if (ExportType == "Excel") { a.Date24 = "<b style='color:Red'>A</b>"; } else { a.Date24 = "<b style='color:Red'>A</b>"; } } break;
                                    case "25": { if (ExportType == "Excel") { a.Date25 = "<b style='color:Red'>A</b>"; } else { a.Date25 = "<b style='color:Red'>A</b>"; } } break;
                                    case "26": { if (ExportType == "Excel") { a.Date26 = "<b style='color:Red'>A</b>"; } else { a.Date26 = "<b style='color:Red'>A</b>"; } } break;
                                    case "27": { if (ExportType == "Excel") { a.Date27 = "<b style='color:Red'>A</b>"; } else { a.Date27 = "<b style='color:Red'>A</b>"; } } break;
                                    case "28": { if (ExportType == "Excel") { a.Date28 = "<b style='color:Red'>A</b>"; } else { a.Date28 = "<b style='color:Red'>A</b>"; } } break;
                                    case "29": { if (ExportType == "Excel") { a.Date29 = "<b style='color:Red'>A</b>"; } else { a.Date29 = "<b style='color:Red'>A</b>"; } } break;
                                    case "30": { if (ExportType == "Excel") { a.Date30 = "<b style='color:Red'>A</b>"; } else { a.Date30 = "<b style='color:Red'>A</b>"; } } break;
                                    case "31": { if (ExportType == "Excel") { a.Date31 = "<b style='color:Red'>A</b>"; } else { a.Date31 = "<b style='color:Red'>A</b>"; } } break;
                                    default: break;
                                }
                            }
                            for (var i = 0; i < Leave.Length; i++)
                            {
                                string Lvalue = Leave[i].ToString("dd");

                                switch (Lvalue)
                                {
                                    case "01": { if (ExportType == "Excel") { a.Date1 = "<b style='color:orange'>L</b>"; } else { a.Date1 = "<b style='color:orange'>L</b>"; } } break;
                                    case "02": { if (ExportType == "Excel") { a.Date2 = "<b style='color:orange'>L</b>"; } else { a.Date2 = "<b style='color:orange'>L</b>"; } } break;
                                    case "03": { if (ExportType == "Excel") { a.Date3 = "<b style='color:orange'>L</b>"; } else { a.Date3 = "<b style='color:orange'>L</b>"; } } break;
                                    case "04": { if (ExportType == "Excel") { a.Date4 = "<b style='color:orange'>L</b>"; } else { a.Date4 = "<b style='color:orange'>L</b>"; } } break;
                                    case "05": { if (ExportType == "Excel") { a.Date5 = "<b style='color:orange'>L</b>"; } else { a.Date5 = "<b style='color:orange'>L</b>"; } } break;
                                    case "06": { if (ExportType == "Excel") { a.Date6 = "<b style='color:orange'>L</b>"; } else { a.Date6 = "<b style='color:orange'>L</b>"; } } break;
                                    case "07": { if (ExportType == "Excel") { a.Date7 = "<b style='color:orange'>L</b>"; } else { a.Date7 = "<b style='color:orange'>L</b>"; } } break;
                                    case "08": { if (ExportType == "Excel") { a.Date8 = "<b style='color:orange'>L</b>"; } else { a.Date8 = "<b style='color:orange'>L</b>"; } } break;
                                    case "09": { if (ExportType == "Excel") { a.Date9 = "<b style='color: orange'>L</b>"; } else { a.Date9 = "<b style='color:orange'>L</b>"; } } break;
                                    case "10": { if (ExportType == "Excel") { a.Date10 = "<b style='color:orange'>L</b>"; } else { a.Date10 = "<b style='color:orange'>L</b>"; } } break;
                                    case "11": { if (ExportType == "Excel") { a.Date11 = "<b style='color:orange'>L</b>"; } else { a.Date11 = "<b style='color:orange'>L</b>"; } } break;
                                    case "12": { if (ExportType == "Excel") { a.Date12 = "<b style='color:orange'>L</b>"; } else { a.Date12 = "<b style='color:orange'>L</b>"; } } break;
                                    case "13": { if (ExportType == "Excel") { a.Date13 = "<b style='color:orange'>L</b>"; } else { a.Date13 = "<b style='color:orange'>L</b>"; } } break;
                                    case "14": { if (ExportType == "Excel") { a.Date14 = "<b style='color:orange'>L</b>"; } else { a.Date14 = "<b style='color:orange'>L</b>"; } } break;
                                    case "15": { if (ExportType == "Excel") { a.Date15 = "<b style='color:orange'>L</b>"; } else { a.Date15 = "<b style='color:orange'>L</b>"; } } break;
                                    case "16": { if (ExportType == "Excel") { a.Date16 = "<b style='color:orange'>L</b>"; } else { a.Date16 = "<b style='color:orange'>L</b>"; } } break;
                                    case "17": { if (ExportType == "Excel") { a.Date17 = "<b style='color:orange'>L</b>"; } else { a.Date17 = "<b style='color:orange'>L</b>"; } } break;
                                    case "18": { if (ExportType == "Excel") { a.Date18 = "<b style='color:orange'>L</b>"; } else { a.Date18 = "<b style='color:orange'>L</b>"; } } break;
                                    case "19": { if (ExportType == "Excel") { a.Date19 = "<b style='color:orange'>L</b>"; } else { a.Date19 = "<b style='color:orange'>L</b>"; } } break;
                                    case "20": { if (ExportType == "Excel") { a.Date20 = "<b style='color:orange'>L</b>"; } else { a.Date20 = "<b style='color:orange'>L</b>"; } } break;
                                    case "21": { if (ExportType == "Excel") { a.Date21 = "<b style='color:orange'>L</b>"; } else { a.Date21 = "<b style='color:orange'>L</b>"; } } break;
                                    case "22": { if (ExportType == "Excel") { a.Date22 = "<b style='color:orange'>L</b>"; } else { a.Date22 = "<b style='color:orange'>L</b>"; } } break;
                                    case "23": { if (ExportType == "Excel") { a.Date23 = "<b style='color:orange'>L</b>"; } else { a.Date23 = "<b style='color:orange'>L</b>"; } } break;
                                    case "24": { if (ExportType == "Excel") { a.Date24 = "<b style='color:orange'>L</b>"; } else { a.Date24 = "<b style='color:orange'>L</b>"; } } break;
                                    case "25": { if (ExportType == "Excel") { a.Date25 = "<b style='color:orange'>L</b>"; } else { a.Date25 = "<b style='color:orange'>L</b>"; } } break;
                                    case "26": { if (ExportType == "Excel") { a.Date26 = "<b style='color:orange'>L</b>"; } else { a.Date26 = "<b style='color:orange'>L</b>"; } } break;
                                    case "27": { if (ExportType == "Excel") { a.Date27 = "<b style='color:orange'>L</b>"; } else { a.Date27 = "<b style='color:orange'>L</b>"; } } break;
                                    case "28": { if (ExportType == "Excel") { a.Date28 = "<b style='color:orange'>L</b>"; } else { a.Date28 = "<b style='color:orange'>L</b>"; } } break;
                                    case "29": { if (ExportType == "Excel") { a.Date29 = "<b style='color:orange'>L</b>"; } else { a.Date29 = "<b style='color:orange'>L</b>"; } } break;
                                    case "30": { if (ExportType == "Excel") { a.Date30 = "<b style='color:orange'>L</b>"; } else { a.Date30 = "<b style='color:orange'>L</b>"; } } break;
                                    case "31": { if (ExportType == "Excel") { a.Date31 = "<b style='color:orange'>L</b>"; } else { a.Date31 = "<b style='color:orange'>L</b>"; } } break;
                                    default: break;
                                }
                            }
                        }
                        else
                        {
                            a.Date1 = "<b style='color:Green'>P</b>"; a.Date2 = "<b style='color:Green'>P</b>"; a.Date3 = "<b style='color:Green'>P</b>"; a.Date4 = "<b style='color:Green'>P</b>"; a.Date5 = "<b style='color:Green'>P</b>"; a.Date6 = "<b style='color:Green'>P</b>"; a.Date7 = "<b style='color:Green'>P</b>"; a.Date8 = "<b style='color:Green'>P</b>"; a.Date9 = "<b style='color:Green'>P</b>"; a.Date10 = "<b style='color:Green'>P</b>"; a.Date11 = "<b style='color:Green'>P</b>"; a.Date12 = "<b style='color:Green'>P</b>"; a.Date13 = "<b style='color:Green'>P</b>"; a.Date14 = "<b style='color:Green'>P</b>"; a.Date15 = "<b style='color:Green'>P</b>";
                            a.Date16 = "<b style='color:Green'>P</b>"; a.Date17 = "<b style='color:Green'>P</b>"; a.Date18 = "<b style='color:Green'>P</b>"; a.Date19 = "<b style='color:Green'>P</b>"; a.Date20 = "<b style='color:Green'>P</b>"; a.Date21 = "<b style='color:Green'>P</b>"; a.Date22 = "<b style='color:Green'>P</b>"; a.Date23 = "<b style='color:Green'>P</b>"; a.Date24 = "<b style='color:Green'>P</b>"; a.Date25 = "<b style='color:Green'>P</b>"; a.Date26 = "<b style='color:Green'>P</b>"; a.Date27 = "<b style='color:Green'>P</b>"; a.Date28 = "<b style='color:Green'>P</b>"; a.Date29 = "<b style='color:Green'>P</b>"; a.Date30 = "<b style='color:Green'>P</b>"; a.Date31 = "<b style='color:Green'>P</b>";

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
                        if (searchmonth == "01" || searchmonth == "03" || searchmonth == "05" || searchmonth == "07" || searchmonth == "08" || searchmonth == "10" || searchmonth == "12")
                        {
                            if (DateNow.Month == Convert.ToInt32(searchmonth) && DateNow.Year == Convert.ToInt32(year))
                            {
                                a.noofpre = ((DateNow.Day) - (a.AbsentCountList + a.LeaveCountList));
                                a.numofworkdays = 31 - a.HolidayCountList;

                                a.percround = (Convert.ToDecimal(Convert.ToDecimal(a.noofpre) / DateNow.Day) * 100);
                                a.Percentage = (Math.Round(a.percround, 1));
                                // Total attendance
                                a.totalWorkingday = Convert.ToInt32(a.numofworkdays);
                                a.totalAttendance = Convert.ToInt32(a.noofpre);
                                a.totalPercentage = 100;
                                a.totalPercentage = (Math.Round(a.totalPercentage, 1));
                                if (DateNow.Month == Convert.ToInt32(searchmonth)) { if (reduceCount == 1) { a.noofpre = a.noofpre - 1; } else { } }
                            }
                            else
                            {
                                a.noofpre = (31 - a.HolidayCountList) - (a.AbsentCountList + a.LeaveCountList);
                                a.numofworkdays = 31 - a.HolidayCountList;


                                a.percround = (Convert.ToDecimal(Convert.ToDecimal(a.noofpre) / a.numofworkdays) * 100);
                                a.Percentage = (Math.Round(a.percround, 1));
                                // Total attendance
                                a.totalWorkingday = Convert.ToInt32(a.numofworkdays);
                                a.totalAttendance = Convert.ToInt32(a.noofpre);
                                a.totalPercentage = 100;
                                a.totalPercentage = (Math.Round(a.totalPercentage, 1));
                                if (DateNow.Month == Convert.ToInt32(searchmonth)) { if (reduceCount == 1) { a.noofpre = a.noofpre - 1; } else { } }
                            }
                        }
                        else if (searchmonth == "02")
                        {
                            if (DateNow.Month == Convert.ToInt32(searchmonth) && DateNow.Year == Convert.ToInt32(year))
                            {
                                a.noofpre = ((DateNow.Day) - (a.AbsentCountList + a.LeaveCountList));
                                a.numofworkdays = 28 - a.HolidayCountList;

                                a.percround = (Convert.ToDecimal(Convert.ToDecimal(a.noofpre) / DateNow.Day) * 100);
                                a.Percentage = (Math.Round(a.percround, 1));
                                // Total attendance
                                a.totalWorkingday = Convert.ToInt32(a.numofworkdays);
                                a.totalAttendance = Convert.ToInt32(a.noofpre);
                                a.totalPercentage = 100;
                                a.totalPercentage = (Math.Round(a.totalPercentage, 1));
                                if (DateNow.Month == Convert.ToInt32(searchmonth)) { if (reduceCount == 1) { a.noofpre = a.noofpre - 1; } else { } }
                            }
                            else
                            {
                                a.noofpre = (28 - a.HolidayCountList) - (a.AbsentCountList + a.LeaveCountList);
                                a.numofworkdays = 28 - a.HolidayCountList;


                                a.percround = (Convert.ToDecimal(Convert.ToDecimal(a.noofpre) / a.numofworkdays) * 100);
                                a.Percentage = (Math.Round(a.percround, 1));
                                // Total attendance
                                a.totalWorkingday = Convert.ToInt32(a.numofworkdays);
                                a.totalAttendance = Convert.ToInt32(a.noofpre);
                                a.totalPercentage = 100;
                                a.totalPercentage = (Math.Round(a.totalPercentage, 1));
                                if (DateNow.Month == Convert.ToInt32(searchmonth)) { if (reduceCount == 1) { a.noofpre = a.noofpre - 1; } else { } }
                            }
                        }
                        else
                        {

                            if (DateNow.Month == Convert.ToInt32(searchmonth) && DateNow.Year == Convert.ToInt32(year))
                            {
                                a.noofpre = ((DateNow.Day) - (a.AbsentCountList + a.LeaveCountList));
                                a.numofworkdays = 30 - a.HolidayCountList;

                                a.percround = (Convert.ToDecimal(Convert.ToDecimal(a.noofpre) / DateNow.Day) * 100);
                                a.Percentage = (Math.Round(a.percround, 1));
                                // Total attendance
                                a.totalWorkingday = Convert.ToInt32(a.numofworkdays);
                                a.totalAttendance = Convert.ToInt32(a.noofpre);
                                a.totalPercentage = 100;
                                a.totalPercentage = (Math.Round(a.totalPercentage, 1));
                                if (DateNow.Month == Convert.ToInt32(searchmonth)) { if (reduceCount == 1) { a.noofpre = a.noofpre - 1; } else { } }
                            }
                            else
                            {
                                a.noofpre = (30 - a.HolidayCountList) - (a.AbsentCountList + a.LeaveCountList);
                                a.numofworkdays = 30 - a.HolidayCountList;


                                a.percround = (Convert.ToDecimal(Convert.ToDecimal(a.noofpre) / a.numofworkdays) * 100);
                                a.Percentage = (Math.Round(a.percround, 1));
                                // Total attendance
                                a.totalWorkingday = Convert.ToInt32(a.numofworkdays);
                                a.totalAttendance = Convert.ToInt32(a.noofpre);
                                a.totalPercentage = 100;
                                a.totalPercentage = (Math.Round(a.totalPercentage, 1));
                                if (DateNow.Month == Convert.ToInt32(searchmonth)) { if (reduceCount == 1) { a.noofpre = a.noofpre - 1; } else { } }
                            }
                        }
                    }


                    if (EmployeeList != null && EmployeeList.Count > 0)
                    {
                        if (ExportType == "Excel")
                        {
                            if (searchmonth == "01" || searchmonth == "03" || searchmonth == "05" || searchmonth == "07" || searchmonth == "08" || searchmonth == "10" || searchmonth == "12")
                            {
                                string headerTable = @"<Table border='1px' cellpadding='9' cellspacing='0'><tr><td colspan='2'>Campus - " + campus + "</td><td colspan='38' align='center' style='font-size: large;'>The Indian Public School</td></tr></b></Table>";
                                var studLst = EmployeeList.First().Value.ToList();

                                var List = (from s in studLst
                                            orderby s.Id
                                            select s).ToList();
                                ExptToXL_FinalResult(List, "AttendanceReports", (items => new
                                {
                                    items.Campus,
                                    items.Name,
                                    items.IdNumber,
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
                                    items.LeaveCountList,
                                    items.HolidayCountList,
                                    items.noofpre,

                                    items.Percentage,

                                    items.totalWorkingday,
                                    items.totalPercentage
                                }), headerTable);
                                return new EmptyResult();
                            }
                            else if (searchmonth == "02")
                            {
                                string headerTable = @"<Table border='1px' cellpadding='9' cellspacing='0'><tr><td colspan='2'>Campus - " + campus + "</td><td colspan='35' align='center' style='font-size: large;'>The Indian Public School</td></tr></b></Table>";
                                var studLst = EmployeeList.First().Value.ToList();

                                var List = (from s in studLst
                                            orderby s.Id
                                            select s).ToList();
                                ExptToXL_FinalResult(List, "AttendanceReports", (items => new
                                {
                                    items.Campus,
                                    items.Name,
                                    items.IdNumber,
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
                                    items.LeaveCountList,
                                    items.HolidayCountList,
                                    items.noofpre,
                                    items.Percentage,
                                    items.totalWorkingday,
                                    items.totalPercentage
                                }), headerTable);
                                return new EmptyResult();
                            }
                            else
                            {
                                string headerTable = @"<Table border='1px' cellpadding='9' cellspacing='0'><tr><td colspan='2'>Campus - " + campus + "</td><td colspan='38' align='center' style='font-size: large;'>The Indian Public School</td></tr></b></Table>";
                                var studLst = EmployeeList.First().Value.ToList();

                                var List = (from s in studLst
                                            orderby s.Id
                                            select s).ToList();
                                ExptToXL_FinalResult(List, "AttendanceReports", (items => new
                                {
                                    items.Campus,
                                    items.Name,
                                    items.IdNumber,
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
                                    items.LeaveCountList,
                                    items.HolidayCountList,
                                    items.noofpre,
                                    items.Percentage,

                                    items.totalWorkingday,
                                    items.totalPercentage
                                }), headerTable);
                                return new EmptyResult();
                            }


                        }
                        else
                        {
                            long totalrecords = EmployeeList.First().Key;
                            int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                            var jsondat = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalrecords,

                                rows = (from items in EmployeeList.First().Value
                                        select new
                                        {
                                            i = items.Id,
                                            cell = new string[] {
                               
                               items.Campus,items.Name,items.IdNumber,
                               items.Date1,items.Date2,items.Date3,items.Date4,items.Date5,items.Date6,items.Date7,items.Date8,items.Date9,items.Date10,items.Date11,items.Date12,items.Date13,items.Date14,items.Date15,items.Date16,items.Date17,items.Date18,items.Date19,
                               items.Date20,items.Date21,items.Date22,items.Date23,items.Date24,items.Date25,items.Date26,items.Date27,items.Date28,items.Date29,items.Date30,items.Date31,items.AbsentCountList.ToString(),items.LeaveCountList.ToString(),items.HolidayCountList.ToString(),items.noofpre.ToString(),items.Percentage.ToString(),items.totalWorkingday.ToString(),items.totalPercentage.ToString(),
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
        
        public void ExptToXL_FinalResult<T, TResult>(IList<T> stuList, string filename, Func<T, TResult> selector, string headerTable)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".xls");
            Response.ContentType = "application/vnd.ms-excel";
            System.IO.StringWriter stw = new System.IO.StringWriter();
            HtmlTextWriter htextw = new HtmlTextWriter(stw);
            DataGrid dg = new DataGrid();

            dg.HeaderStyle.BackColor = System.Drawing.Color.FromName("#B0B0B0");
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

        public ActionResult AddEmployeeAttendance(EmployeeAttendance ea)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    EmployeeService es = new EmployeeService();
                    StaffManagementService sms = new StaffManagementService();
                    StaffDetails sd = sms.GetStaffDetailsByIdNumber(ea.EmployeeIdNo);
                    ea.PreRegNum = sd.PreRegNum;
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    ea.CreatedDate = DateTime.Now;
                    ea.CreatedBy = userId;
                    ea.ModifiedDate = DateTime.Now;
                    ea.ModifiedBy = userId;

                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("Campus", ea.Campus);
                    criteria.Add("EmployeeIdNo", ea.EmployeeIdNo);
                    criteria.Add("EmployeeName", ea.EmployeeName);
                    criteria.Add("AbsentDate", ea.AbsentDate);
                    ViewBag.campusddl = CampusMasterFunc();
                    Dictionary<long, IList<EmployeeAttendance>> EmployeeAttendance = es.GetEmployeeAttendanceDetailsListWithPagingAndCriteria(0, 9999, "", "", criteria);
                    if (EmployeeAttendance != null && EmployeeAttendance.FirstOrDefault().Value != null && EmployeeAttendance.FirstOrDefault().Value.Count > 0)
                    {
                        ViewBag.Success = "No";
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        es.CreateOrUpdateEmployeeAttendanceList(ea);
                        ViewBag.Success = "Yes";
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult EmployeeOT()
        {
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.campusddl = CampusMaster.First().Value;
            return View();
        }

        public ActionResult AddEmployeeOT(EmployeeOTDetails ea)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    EmployeeService es = new EmployeeService();
                    StaffManagementService sms = new StaffManagementService();
                    StaffDetails sd = sms.GetStaffDetailsByIdNumber(ea.EmployeeIdNo);
                    ea.PreRegNum = sd.PreRegNum;
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    ea.CreatedDate = DateTime.Now;
                    ea.CreatedBy = userId;
                    ea.ModifiedDate = DateTime.Now;
                    ea.ModifiedBy = userId;
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("Campus", ea.Campus);
                    criteria.Add("EmployeeIdNo", ea.EmployeeIdNo);
                    criteria.Add("EmployeeName", ea.EmployeeName);
                    criteria.Add("OTDate", ea.OTDate);
                    ViewBag.campusddl = CampusMasterFunc();
                    Dictionary<long, IList<EmployeeOTDetails>> EmployeeOT = es.GetEmployeeOTDetailsListWithPagingAndCriteria(0, 9999, "", "", criteria);
                    if (EmployeeOT != null && EmployeeOT.FirstOrDefault().Value != null && EmployeeOT.FirstOrDefault().Value.Count > 0)
                    {
                        ViewBag.Success = "No";
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        es.CreateOrUpdateEmployeeOTList(ea);
                        ViewBag.Success = "Yes";
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        public ActionResult EmployeeOTListJqGrid(string AbsentDate, string CreatedDate, EmployeeOTDetails ea, int? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    EmployeeService ts = new EmployeeService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    sord = sord == "desc" ? "Desc" : "Asc";
                    if (!string.IsNullOrWhiteSpace(ea.Campus)) { criteria.Add("Campus", ea.Campus); }
                    if (!string.IsNullOrWhiteSpace(ea.EmployeeIdNo)) { criteria.Add("EmployeeIdNo", ea.EmployeeIdNo); }
                    if (!string.IsNullOrWhiteSpace(ea.EmployeeName)) { criteria.Add("EmployeeName", ea.EmployeeName); }
                    if (!string.IsNullOrWhiteSpace(ea.OTType)) { criteria.Add("OTType", ea.OTType); }
                    if (!string.IsNullOrWhiteSpace(AbsentDate))
                    {
                        AbsentDate = AbsentDate.Trim();
                        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                        DateTime[] AttDatefromto = new DateTime[2];
                        AttDatefromto[0] = DateTime.Parse(AbsentDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        string To = string.Format("{0:dd/MM/yyyy}", AttDatefromto[0]);
                        AttDatefromto[1] = Convert.ToDateTime(To + " " + "23:59:59");
                        criteria.Add("OTDate", AttDatefromto);
                    }
                    if (!string.IsNullOrWhiteSpace(CreatedDate))
                    {
                        CreatedDate = CreatedDate.Trim();
                        IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                        DateTime[] CreatedDatefromto = new DateTime[2];
                        CreatedDatefromto[0] = DateTime.Parse(CreatedDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        string ToCreatedDate = string.Format("{0:dd/MM/yyyy}", CreatedDatefromto[0]);
                        CreatedDatefromto[1] = Convert.ToDateTime(ToCreatedDate + " " + "23:59:59");
                        criteria.Add("CreatedDate", CreatedDatefromto);
                    }
                    Dictionary<long, IList<EmployeeOTDetails>> OTDetails = ts.GetEmployeeOTDetailsListWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);
                    if (OTDetails != null && OTDetails.Count > 0)
                    {
                        long totalrecords = OTDetails.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var EmpOT = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in OTDetails.First().Value
                                    select new
                                    {
                                        i = 2,
                                        cell = new string[] {
                               items.Id.ToString(),
                               items.Campus,
                               items.EmployeeName, 
                               items.EmployeeIdNo,
                               items.OTDate!=null? items.OTDate.ToString("dd/MM/yyyy"):"",
                               items.OTType.ToString(),
                               items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
                               items.CreatedBy
                            }
                                    })
                        };
                        return Json(EmpOT, JsonRequestBehavior.AllowGet);
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

        public ActionResult EmployeeCaptureImage(string PreRegNo)
        {
            ViewBag.PreRegNum = PreRegNo;
            return View();
        }

        public ActionResult uploaddisplayForEmployee(long Id)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("PreRegNum", Id);
                criteria.Add("DocumentType", "Employee Photo");

                Dictionary<long, IList<UploadedFiles>> UploadedFiles = ads.GetUploadedFilesListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);

                //                IList<UploadedFiles> list1 = ads.GetUploadedFilesByPreRegNum(Id,"no");
                if (UploadedFiles != null && UploadedFiles.FirstOrDefault().Value != null && UploadedFiles.FirstOrDefault().Value.Count != 0)
                {
                    if (UploadedFiles.First().Value[0].OldFiles == 1)
                    {
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

        #region Employee OT Details Report
        public ActionResult EmployeeOtReport()
        {
            try
            {
                MastersService ms = new MastersService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                ViewBag.campusddl = CampusMaster.First().Value;
                DateTime daytime = DateTime.Now;
                int CurMonth = daytime.Month;
                ViewBag.CurMonth = CurMonth;

                return View();
            }
            catch (Exception ex)
            {

                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }
        public ActionResult EmployeeOTReportDetailsJqgrid(string Campus, string Name, int Month, string OTType, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                DateTime DateNow = DateTime.Now;
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (string.IsNullOrEmpty(Campus) && string.IsNullOrEmpty(OTType))
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        DateTime today = DateTime.Today;
                        DateTime first = new DateTime(today.Year, Month, 1);
                        DateTime temp = first.AddMonths(1);
                        DateTime last = temp.AddDays(-1);
                        EmployeeService es = new EmployeeService();

                        Dictionary<string, object> criteria = new Dictionary<string, object>();

                        criteria.Add("Campus", Campus);
                        if (!string.IsNullOrWhiteSpace(Name)) { criteria.Add("Name", Name); }
                        Dictionary<long, IList<EmployeeOTReport>> EmpDetails = es.GetEmployeeOTReportDetailsListWithPagingAndCriteria(page - 1, rows, sord, sidx, criteria);
                        criteria.Clear();
                        criteria.Add("Campus", Campus);
                        criteria.Add("OTType", OTType);
                        DateTime[] fromto = new DateTime[2];
                        fromto[0] = first;
                        fromto[1] = last;
                        criteria.Add("OTDate", fromto);
                        Dictionary<long, IList<EmployeeOTDetails>> EmpOT = es.GetEmployeeOTDetailsListWithPagingAndCriteria(page - 1, rows, string.Empty, sord, criteria);
                        IList<EmployeeOTDetails> EmpOTRe = EmpOT.FirstOrDefault().Value.ToList();
                        IEnumerable<string> blkLong = from p in EmpOTRe
                                                      orderby p.PreRegNum ascending
                                                      select p.PreRegNum.ToString();
                        string[] Empid = blkLong.ToArray();
                        foreach (EmployeeOTReport a in EmpDetails.FirstOrDefault().Value)
                        {
                            a.Date1 = "<b style='color:Green'>-</b>"; a.Date2 = "<b style='color:Green'>-</b>"; a.Date3 = "<b style='color:Green'>-</b>"; a.Date4 = "<b style='color:Green'>-</b>"; a.Date5 = "<b style='color:Green'>-</b>"; a.Date6 = "<b style='color:Green'>-</b>"; a.Date7 = "<b style='color:Green'>-</b>"; a.Date8 = "<b style='color:Green'>-</b>"; a.Date9 = "<b style='color:Green'>-</b>"; a.Date10 = "<b style='color:Green'>-</b>"; a.Date11 = "<b style='color:Green'>-</b>"; a.Date12 = "<b style='color:Green'>-</b>"; a.Date13 = "<b style='color:Green'>-</b>"; a.Date14 = "<b style='color:Green'>-</b>"; a.Date15 = "<b style='color:Green'>-</b>";
                            a.Date16 = "<b style='color:Green'>-</b>"; a.Date17 = "<b style='color:Green'>-</b>"; a.Date18 = "<b style='color:Green'>-</b>"; a.Date19 = "<b style='color:Green'>-</b>"; a.Date20 = "<b style='color:Green'>-</b>"; a.Date21 = "<b style='color:Green'>-</b>"; a.Date22 = "<b style='color:Green'>-</b>"; a.Date23 = "<b style='color:Green'>-</b>"; a.Date24 = "<b style='color:Green'>-</b>"; a.Date25 = "<b style='color:Green'>-</b>"; a.Date26 = "<b style='color:Green'>-</b>"; a.Date27 = "<b style='color:Green'>-</b>"; a.Date28 = "<b style='color:Green'>-</b>"; a.Date29 = "<b style='color:Green'>-</b>"; a.Date30 = "<b style='color:Green'>-</b>";
                            a.Date31 = "<b style='color:Green'>-</b>"; a.TotalAllowance = "<b style='color:Green'>0</b>";

                            if (Empid.Contains((a.PreRegNum.ToString())))
                            {
                                //Dictionary<string, object> criteria1 = new Dictionary<string, object>();
                                //criteria1.Add("OTDate", fromto);
                                Dictionary<long, IList<EmployeeOTDetails>> OTReport = es.GetEmployeeOTDetailsListWithPagingAndCriteria(page - 1, rows, string.Empty, sord, criteria);
                                var OTDate = (from p in OTReport.FirstOrDefault().Value
                                              where p.OTType == OTType && p.PreRegNum == a.PreRegNum
                                              orderby p.OTDate ascending
                                              select p.OTDate).ToArray();


                                a.OTCount = OTDate.Length;
                                var allwonce = OTReport.FirstOrDefault().Value
                                    .Where(x => x.OTType == OTType && x.PreRegNum == a.PreRegNum)
                                    .GroupBy(x => new { x.OTType })
                                    .Select(g => new
                                    {
                                        sum = g.Sum(x => x.Allowance) //To Do get total allwonce of ot
                                    });

                                a.TotalAllowance = allwonce.FirstOrDefault().sum.ToString();

                                for (var i = 0; i < OTDate.Length; i++)
                                {
                                    string Abvalue = OTDate[i].ToString("dd");

                                    switch (Abvalue)
                                    {
                                        case "1":
                                        case "01": { if (ExportType == "Excel") { a.Date1 = "<b style='color:Red'>X</b>"; } else { a.Date1 = "<b style='color:Red'>X</b>"; } } break;
                                        case "2":
                                        case "02": { if (ExportType == "Excel") { a.Date2 = "<b style='color:Red'>X</b>"; } else { a.Date2 = "<b style='color:Red'>X</b>"; } } break;
                                        case "3":
                                        case "03": { if (ExportType == "Excel") { a.Date3 = "<b style='color:Red'>X</b>"; } else { a.Date3 = "<b style='color:Red'>X</b>"; } } break;
                                        case "4":
                                        case "04": { if (ExportType == "Excel") { a.Date4 = "<b style='color:Red'>X</b>"; } else { a.Date4 = "<b style='color:Red'>X</b>"; } } break;
                                        case "5":
                                        case "05": { if (ExportType == "Excel") { a.Date5 = "<b style='color:Red'>X</b>"; } else { a.Date5 = "<b style='color:Red'>X</b>"; } } break;
                                        case "6":
                                        case "06": { if (ExportType == "Excel") { a.Date6 = "<b style='color:Red'>X</b>"; } else { a.Date6 = "<b style='color:Red'>X</b>"; } } break;
                                        case "7":
                                        case "07": { if (ExportType == "Excel") { a.Date7 = "<b style='color:Red'>X</b>"; } else { a.Date7 = "<b style='color:Red'>X</b>"; } } break;
                                        case "8":
                                        case "08": { if (ExportType == "Excel") { a.Date8 = "<b style='color:Red'>X</b>"; } else { a.Date8 = "<b style='color:Red'>X</b>"; } } break;
                                        case "9":
                                        case "09": { if (ExportType == "Excel") { a.Date9 = "<b style='color:Red'>X</b>"; } else { a.Date9 = "<b style='color:Red'>X</b>"; } } break;
                                        case "10": { if (ExportType == "Excel") { a.Date10 = "<b style='color:Red'>X</b>"; } else { a.Date10 = "<b style='color:Red'>X</b>"; } } break;
                                        case "11": { if (ExportType == "Excel") { a.Date11 = "<b style='color:Red'>X</b>"; } else { a.Date11 = "<b style='color:Red'>X</b>"; } } break;
                                        case "12": { if (ExportType == "Excel") { a.Date12 = "<b style='color:Red'>X</b>"; } else { a.Date12 = "<b style='color:Red'>X</b>"; } } break;
                                        case "13": { if (ExportType == "Excel") { a.Date13 = "<b style='color:Red'>X</b>"; } else { a.Date13 = "<b style='color:Red'>X</b>"; } } break;
                                        case "14": { if (ExportType == "Excel") { a.Date14 = "<b style='color:Red'>X</b>"; } else { a.Date14 = "<b style='color:Red'>X</b>"; } } break;
                                        case "15": { if (ExportType == "Excel") { a.Date15 = "<b style='color:Red'>X</b>"; } else { a.Date15 = "<b style='color:Red'>X</b>"; } } break;
                                        case "16": { if (ExportType == "Excel") { a.Date16 = "<b style='color:Red'>X</b>"; } else { a.Date16 = "<b style='color:Red'>X</b>"; } } break;
                                        case "17": { if (ExportType == "Excel") { a.Date17 = "<b style='color:Red'>X</b>"; } else { a.Date17 = "<b style='color:Red'>X</b>"; } } break;
                                        case "18": { if (ExportType == "Excel") { a.Date18 = "<b style='color:Red'>X</b>"; } else { a.Date18 = "<b style='color:Red'>X</b>"; } } break;
                                        case "19": { if (ExportType == "Excel") { a.Date19 = "<b style='color:Red'>X</b>"; } else { a.Date19 = "<b style='color:Red'>X</b>"; } } break;
                                        case "20": { if (ExportType == "Excel") { a.Date20 = "<b style='color:Red'>X</b>"; } else { a.Date20 = "<b style='color:Red'>X</b>"; } } break;
                                        case "21": { if (ExportType == "Excel") { a.Date21 = "<b style='color:Red'>X</b>"; } else { a.Date21 = "<b style='color:Red'>X</b>"; } } break;
                                        case "22": { if (ExportType == "Excel") { a.Date22 = "<b style='color:Red'>X</b>"; } else { a.Date22 = "<b style='color:Red'>X</b>"; } } break;
                                        case "23": { if (ExportType == "Excel") { a.Date23 = "<b style='color:Red'>X</b>"; } else { a.Date23 = "<b style='color:Red'>X</b>"; } } break;
                                        case "24": { if (ExportType == "Excel") { a.Date24 = "<b style='color:Red'>X</b>"; } else { a.Date24 = "<b style='color:Red'>X</b>"; } } break;
                                        case "25": { if (ExportType == "Excel") { a.Date25 = "<b style='color:Red'>X</b>"; } else { a.Date25 = "<b style='color:Red'>X</b>"; } } break;
                                        case "26": { if (ExportType == "Excel") { a.Date26 = "<b style='color:Red'>X</b>"; } else { a.Date26 = "<b style='color:Red'>X</b>"; } } break;
                                        case "27": { if (ExportType == "Excel") { a.Date27 = "<b style='color:Red'>X</b>"; } else { a.Date27 = "<b style='color:Red'>X</b>"; } } break;
                                        case "28": { if (ExportType == "Excel") { a.Date28 = "<b style='color:Red'>X</b>"; } else { a.Date28 = "<b style='color:Red'>X</b>"; } } break;
                                        case "29": { if (ExportType == "Excel") { a.Date29 = "<b style='color:Red'>X</b>"; } else { a.Date29 = "<b style='color:Red'>X</b>"; } } break;
                                        case "30": { if (ExportType == "Excel") { a.Date30 = "<b style='color:Red'>X</b>"; } else { a.Date30 = "<b style='color:Red'>X</b>"; } } break;
                                        case "31": { if (ExportType == "Excel") { a.Date31 = "<b style='color:Red'>X</b>"; } else { a.Date31 = "<b style='color:Red'>A</b>"; } } break;
                                        default: break;
                                    }


                                }
                            }
                        }
                        if (EmpDetails != null && EmpDetails.Count > 0)
                        {
                            if (ExportType == "Excel")
                            {
                                var List = EmpDetails.First().Value.ToList();
                                ExptToXL(List, "EmployeeOTDetails", (items => new
                                {
                                    items.Name,
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
                                    items.OTCount,
                                    items.TotalAllowance


                                }));
                                return new EmptyResult();
                            }
                            else
                            {

                                long totalrecords = EmpDetails.First().Key;
                                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                                var DriverOT = new
                                {
                                    total = totalPages,
                                    page = page,
                                    records = totalrecords,
                                    rows = (from items in EmpDetails.First().Value
                                            select new
                                            {
                                                i = 2,
                                                cell = new string[] {
                                            items.Name,
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
                                    items.OTCount.ToString(),
                                    items.TotalAllowance

                        }
                                            })
                                };
                                return Json(DriverOT, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else return Json(null);
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

        public JsonResult EmployeeDocumentsjqgrid(string id, string txtSearch, string idno, string name, string sect, string cname, string grad, string btype, int rows, string sidx, string sord, int? page)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("DocumentFor", "Employee");
                criteria.Add("PreRegNum", Convert.ToInt64(Session["Reqnum"]));
                Dictionary<long, IList<UploadedFilesView>> UploadedFilesview = ads.GetUploadedFilesViewListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                IList<UploadedFilesView> uploadList = new List<UploadedFilesView>();
                foreach (UploadedFilesView item in UploadedFilesview.FirstOrDefault().Value)
                {
                    if (item.DocumentName != "Salary Slip")
                    {
                        uploadList.Add(item);
                    }
                    else
                    {
                        if (item.DocumentName == "Salary Slip" && DateTime.Now.Month == item.MonthOfSalary)
                        {
                            uploadList.Add(item);
                        }
                    }
                }

                //if (UploadedFiles != null && UploadedFiles.Count > 0)
                {
                    long totalrecords = uploadList.Count;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in uploadList
                                select new
                                {
                                    i = 2,
                                    cell = new string[] {
                               items.DocumentType,
                              // String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '/StaffManagement/uploaddisplay?Id="+items.Id+"' >{0}</a>",items.DocumentName),
                              String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '/Employee/uploaddisplayForEmployee?Id="+items.Id+"' target='_Blank'>{0}</a>",items.DocumentName),
                              items.DocumentSize+" Bytes",
                               items.UploadedDate,
                               items.Id.ToString()
                            }
                                })
                    };
                    return Json(jsondat, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
  
    }
}
