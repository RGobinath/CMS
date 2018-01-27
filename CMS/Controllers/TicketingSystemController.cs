using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TIPS.Entities;
using TIPS.Entities.AdmissionEntities;
using TIPS.Entities.TicketingSystem;
using TIPS.Service;
using TIPS.Service.TicketingSystem;
using TIPS.ServiceContract;
using CMS.Helpers;
using System.Threading.Tasks;

namespace CMS.Controllers
{
    public class TicketingSystemController : BaseController
    {
        string policyName = "TicketingSystem";
        string TemplateName = "ETicket";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult TicketingSystemInbox()
        {
            try
            {
                MastersService ms = new MastersService();
                TicketSystemService tcktSrv = new TicketSystemService();
                UserService us = new UserService();

                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj == null || (Userobj != null && Userobj.UserId == null))
                { return RedirectToAction("LogOff", "Account"); }

                string loggedInUserId = Userobj.UserId;//(Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                Dictionary<string, object> criteria = new Dictionary<string, object>();

                string loggedInUserName = (Userobj.UserName != null) ? Userobj.UserName : "";
                string loggedInUserType = (Userobj.UserType != null) ? Userobj.UserType : "";
                bool isCreator = false;
                Dictionary<string, object> criteriaUserAppRole = new Dictionary<string, object>();
                criteriaUserAppRole.Add("UserId", loggedInUserId);
                criteriaUserAppRole.Add("AppCode", "TKT");
                criteriaUserAppRole.Add("RoleCode", "ETC");
                Dictionary<long, IList<UserAppRole>> userAppRole = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteriaUserAppRole);
                if (userAppRole != null && userAppRole.Count > 0 && userAppRole.First().Key > 0)
                {
                    int count = userAppRole.First().Value.Count;
                    //if it has values then for each concatenate APP+ROLE 
                    string[] userRoles = new string[count];
                    int i = 0;

                    #region "Getting all User Role & App Code"
                    foreach (UserAppRole uar in userAppRole.First().Value)
                    {
                        if (!string.IsNullOrWhiteSpace(uar.AppCode.Trim()) && !string.IsNullOrWhiteSpace(uar.RoleCode.Trim()))
                        {
                            isCreator = true;
                            //userRoles[i] = uar.AppCode.Trim() + uar.RoleCode.Trim();
                        }
                        i++;
                    }
                    #endregion "Getting all User Role & App Code"
                }
                ViewBag.isCreator = isCreator;
                ViewBag.docTypes = string.Empty;
                ViewBag.Module = string.Empty;
                ViewBag.Priority = string.Empty;
                ViewBag.TicketStatus = string.Empty;
                ViewBag.TicketType = string.Empty;
                ViewBag.Severity = string.Empty;
                #region BreadCrumb
                string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                #endregion
                Dictionary<long, IList<TicketType>> tcktTyp = tcktSrv.GetTicketTypeListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                if (tcktTyp != null && tcktTyp.Count > 0)
                {
                    ViewBag.TicketType = (tcktTyp.FirstOrDefault().Value).Select(item => new SelectListItem
                    {
                        Value = item.TicketTypeId.ToString(),
                        Text = item.TicketTypeCode.ToUpper(),
                    });
                }
                
                Dictionary<long, IList<DocumentTypeMaster>> DocumentTypeMaster = ms.GetDocumentTypeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                if (DocumentTypeMaster != null && DocumentTypeMaster.Count > 0)
                { ViewBag.docTypes = DocumentTypeMaster.FirstOrDefault().Value; }

                ViewBag.CreatedDate = DateTime.Now;
                ViewBag.Reporter = loggedInUserId;
                ViewBag.loggedInUserType = loggedInUserType;
                ViewBag.loggedInUserName = string.IsNullOrWhiteSpace(loggedInUserName) ? loggedInUserId : loggedInUserName;
                ViewBag.Status = "New";
                return View();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
            finally
            { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="sidx"></param>
        /// <param name="sord"></param>
        /// <param name="TicketStatus"></param>
        /// <param name="fromDate"></param>
        /// <param name="TicketNo"></param>
        /// <param name="Module"></param>
        /// <param name="TicketType"></param>
        /// <param name="Severity"></param>
        /// <param name="Priority"></param>
        /// <param name="Reporter"></param>
        /// <param name="AssignedTo"></param>
        /// <param name="ActivityFullName"></param>
        /// <param name="Status"></param>
        /// <param name="ExportToXL"></param>
        /// <returns></returns>
        public ActionResult GetTicketingSystemInbox(int page, int rows, string sidx, string sord, string TicketStatus,
            string fromDate, string TicketNo, string Module, string TicketType, string Severity, string Priority,
            string Reporter, string AssignedTo, string ActivityFullName, string Status, string ExportToXL)
        {
            try
            {
                ProcessFlowServices pfs = new ProcessFlowServices(); // TODO: Initialize to an appropriate value

                string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();

                UserService us = new UserService();
                //pass userid and get the list of roles from user service GetAppRoleForAnUserListWithPagingAndCriteria

                Dictionary<long, IList<UserAppRole>> userAppRole = new Dictionary<long, IList<UserAppRole>>();
                Dictionary<string, object> criteriaUserAppRole = new Dictionary<string, object>();
                criteriaUserAppRole.Add("UserId", userid);
                userAppRole = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteriaUserAppRole);
                //if list userAppRole is null then empty grid
                if (userAppRole != null && userAppRole.Count > 0 && userAppRole.First().Key > 0)
                {
                    int count = userAppRole.First().Value.Count;
                    //if it has values then for each concatenate APP+ROLE 
                    string[] userRoles = new string[count];

                    int i = 0;
                    Dictionary<string, object> criteria = new Dictionary<string, object>();

                    #region "Getting all User Role & App Code"
                    foreach (UserAppRole uar in userAppRole.First().Value)
                    {
                        if (!string.IsNullOrWhiteSpace(uar.AppCode.Trim()) && !string.IsNullOrWhiteSpace(uar.RoleCode.Trim()))
                        {
                            userRoles[i] = uar.AppCode.Trim() + uar.RoleCode.Trim();
                        }
                        i++;
                    }
                    #endregion "Getting all User Role & App Code"

                    #region "criteria builder"
                    if ((!string.IsNullOrEmpty(fromDate) && !(string.IsNullOrEmpty(fromDate.Trim()))))
                    {
                        fromDate = fromDate.Trim();
                        string To = string.Format("{0:MM/dd/yyyy}", DateTime.Now);
                        DateTime ToDate = Convert.ToDateTime(To + " " + "23:59:59");
                        DateTime fdate = DateTime.Now;
                        DateTime tdate = DateTime.Now;
                        DateTime[] fromto = new DateTime[2];
                        if (!string.IsNullOrEmpty(fromDate))
                        {
                            fdate = Convert.ToDateTime(fromDate);
                            fromto[0] = fdate;
                        }
                        if (ToDate != null)
                        {
                            fromto[1] = ToDate;
                        }
                        criteria.Add("TicketSystem." + "IssueDate", fromto);
                    }

                    Status = (Status == "" || Status == null) ? "Available" : Status;

                    if (!string.IsNullOrEmpty(Status))
                    {

                        criteria.Add(Status == "Sent" ? "Completed" : Status, true);

                        if (Status == "Assigned")
                        {
                            //criteria.Add("Available", true);
                            criteria.Add("Performer", userid);
                        }
                        else if (Status == "Completed")
                        {
                            criteria.Add("ActivityName", "Complete");
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(TicketNo)) { criteria.Add("TicketSystem.TicketNo", TicketNo); }
                    if (!string.IsNullOrWhiteSpace(Module)) { criteria.Add("TicketSystem.Module", Module); }
                    if (!string.IsNullOrWhiteSpace(TicketType)) { criteria.Add("TicketSystem.TicketType", TicketType); }
                    if (!string.IsNullOrWhiteSpace(Severity)) { criteria.Add("TicketSystem.Severity", Severity); }
                    if (!string.IsNullOrWhiteSpace(Priority)) { criteria.Add("TicketSystem.Priority", Priority); }
                    if (!string.IsNullOrWhiteSpace(TicketStatus)) { criteria.Add("TicketSystem.TicketStatus", TicketStatus); }
                    if (!string.IsNullOrWhiteSpace(Reporter)) { criteria.Add("TicketSystem.Reporter", Reporter); }
                    if (!string.IsNullOrWhiteSpace(AssignedTo)) { criteria.Add("TicketSystem.AssignedTo", AssignedTo); }
                    if (!string.IsNullOrWhiteSpace(ActivityFullName)) { criteria.Add("ActivityFullName", ActivityFullName); }
                    #endregion "criteria builder"

                    string[] alias = new string[1];

                    alias[0] = "TicketSystem";
                    if (sidx == "ActivityFullName")
                    sidx = sidx.ToString(); 
                    else
                     sidx = "TicketSystem." + sidx; 

                    sord = sord == "desc" ? "Desc" : "Asc";
                    criteria.Add("TemplateId", (long)5);
                    Dictionary<long, IList<TicketSystemActivity>> ActivitiesList = pfs.GetTicketSystemActivityListWithsearchCriteria(page - 1, rows, sidx, sord, criteria, "AppRole", userRoles, alias);
                    if (ActivitiesList != null && ActivitiesList.Count > 0)
                    {
                        foreach (TicketSystemActivity pi in ActivitiesList.First().Value)
                        {
                            pi.ProcessInstance.DifferenceInHours = DateTime.Now - pi.ProcessInstance.DateCreated;
                        }
                        if (ExportToXL == "true" || ExportToXL == "True")
                        {
                            base.ExptToXL(ActivitiesList.First().Value.ToList(), "AssessList", (items => new
                            {
                                items.TicketSystem.TicketNo,
                                items.ActivityName,
                                items.ActivityFullName,
                                items.TicketSystem.Module,
                                items.TicketSystem.TicketType,
                                items.TicketSystem.Severity,
                                items.TicketSystem.Priority,
                                items.TicketSystem.TicketStatus,
                                items.TicketSystem.Reporter,
                                CreatedDate = items.TicketSystem.CreatedDate != null ? items.CreatedDate.Value.ToString("dd-MMM-yyyy HH:mm") : "",
                                items.TicketSystem.AssignedTo,
                                Available = items.Available == true ? "Available" : (items.Assigned == true ? "Assigned" : "Completed"),
                                SLA = items.ProcessInstance.Status == "Completed" ? "Completed" : items.ProcessInstance.DifferenceInHours.Value.TotalHours.ToString()  //SLA
                            }));
                            return new EmptyResult();
                        }
                        else
                        {
                            long totalRecords = ActivitiesList.First().Key;
                            int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                            var jsonData = new
                            {
                                total = totalPages,
                                page = page,
                                records = totalRecords,
                                rows = (
                                     from items in ActivitiesList.First().Value
                                     select new
                                     {
                                         i = items.Id,
                                         cell = new string[] 
                                     {      
                                        items.Id.ToString(),
                                        items.TicketSystem.Id.ToString(),
                                        items.TicketSystem.TicketNo,
                                        items.ActivityName,
                                        items.ActivityFullName, 
                                        items.TicketSystem.Module,
                                        items.TicketSystem.TicketType,
                                        items.TicketSystem.Severity, 
                                        items.TicketSystem.Priority,
                                        items.TicketSystem.TicketStatus,
                                        items.TicketSystem.Reporter,
                                        items.TicketSystem.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
                                        items.TicketSystem.AssignedTo,
                                        items.Available==true?"Available":(items.Assigned==true?"Assigned":"Completed"),
                                        "<img src='/Images/History.png ' id='ImgHistory' onclick=\"ShowComments('" + items.ProcessRefId + "');\" />",
                                        items.ProcessInstance.Status=="Completed"?"Completed":items.ProcessInstance.DifferenceInHours.Value.TotalHours.ToString(),//SLA
                                        items.TicketSystem.Summary,
                                     }
                                     })
                            };
                            return Json(jsonData, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                if (ExportToXL == "true" || ExportToXL == "True")
                { return null; }
                else
                { return Json(string.Empty, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception ex)
            {
                return ThrowJSONErrorNew(ex, policyName);
            }
            finally
            { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ActivityId"></param>
        /// <param name="activityName"></param>
        /// <param name="status"></param>
        /// <param name="ActivityFullName"></param>
        /// <returns></returns>
        public ActionResult TicketingSystem(long? Id, long? ActivityId, string activityName, string status, string ActivityFullName)
        {
            try
            {
                MastersService ms = new MastersService();
                TicketSystemService assSrv = new TicketSystemService();
                UserService us = new UserService();
                ProcessFlowServices pfs = new ProcessFlowServices();

                #region "Get the logged in User Details"
                TIPS.Entities.User Userobj = (TIPS.Entities.User)Session["objUser"];
                if (Userobj == null || (Userobj != null && Userobj.UserId == null))
                { return RedirectToAction("LogOff", "Account"); }

                string loggedInUserId = Userobj.UserId;
                string loggedInUserName = (Userobj.UserName != null) ? Userobj.UserName : "";
                string loggedInUserType = (Userobj.UserType != null) ? Userobj.UserType : "";
                #endregion "Get the logged in User Details"

                #region "Assign request to logged in user"
                bool isAssigned = false;
                if (ActivityId > 0 && !string.IsNullOrWhiteSpace(loggedInUserId))
                {
                    try
                    {
                        isAssigned = pfs.AssignActivityCheckBeforeAssigning(ActivityId ?? 0, loggedInUserId);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.ToUpper().IndexOf(loggedInUserId.ToUpper()) <= 0)
                        {
                            TempData["AlrtDskMsg"] = ex.Message;
                            return RedirectToAction("TicketingSystemInbox", "TicketingSystem");
                        }
                    }
                }
                #endregion
                //pass userid and get the list of roles from user service GetAppRoleForAnUserListWithPagingAndCriteria
                #region "Get all the Master details"

                Dictionary<string, object> criteriaUserAppRole = new Dictionary<string, object>();
                criteriaUserAppRole.Add("RoleCode", "ETR");
                Dictionary<long, IList<UserAppRole>> userAppRole = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteriaUserAppRole);
                Dictionary<long, IList<Module>> mdul = assSrv.GetModuleListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, null);
                Dictionary<long, IList<Priority>> prty = assSrv.GetPriorityListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, null);
                Dictionary<long, IList<TicketStatus>> tcktSts = assSrv.GetTicketStatusListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, null);
                Dictionary<long, IList<TicketType>> tcktTyp = assSrv.GetTicketTypeListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, null);
                Dictionary<long, IList<Severity>> svrty = assSrv.GetSeverityListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, null);

                if (mdul != null && mdul.Count > 0)
                {
                    ViewBag.ddModule = (mdul.FirstOrDefault().Value).Select(item => new SelectListItem
                    {
                        Value = item.ModuleCode.ToString(),
                        Text = item.ModuleCode.ToUpper()
                    });
                }
                if (prty != null && prty.Count > 0)
                {
                    ViewBag.ddPriority = (prty.FirstOrDefault().Value).Select(item => new SelectListItem
                    {
                        Value = item.PriorityCode.ToString(),
                        Text = item.PriorityCode.ToUpper()
                    });
                }
                if (tcktSts != null && tcktSts.Count > 0)
                {
                    ViewBag.ddTicketStatus = (tcktSts.FirstOrDefault().Value).Select(item => new SelectListItem
                    {
                        Value = item.StatusCode.ToString(),
                        Text = item.StatusCode.ToUpper()
                    });
                }
                if (tcktTyp != null && tcktTyp.Count > 0)
                {
                    ViewBag.ddTicketType = (tcktTyp.FirstOrDefault().Value).Select(item => new SelectListItem
                    {
                        Value = item.TicketTypeCode.ToString(),
                        Text = item.TicketTypeCode.ToUpper()
                    });
                }
                if (svrty != null && svrty.Count > 0)
                {
                    ViewBag.ddSeverity = (svrty.FirstOrDefault().Value).Select(item => new SelectListItem
                    {
                        Value = item.SeverityCode.ToString(),
                        Text = item.SeverityCode.ToUpper()
                    });
                }
                if (userAppRole != null && userAppRole.Count > 0)
                {
                    ViewBag.ddAssignedTo = (userAppRole.FirstOrDefault().Value).Select(item => new SelectListItem
                    {
                        Value = item.UserId.ToString(),
                        Text = item.UserId.ToUpper()
                    }).Distinct();
                }
                #endregion "Get all the Master details"

                TicketSystem objTcktSys = new TicketSystem();
                #region "To get the Existing Record details"
                if (Id > 0)
                {
                    objTcktSys = assSrv.GetTicketSystemById(Id ?? 0);
                }
                #endregion "To get the Existing Record details"
                #region "For New Record details"
                else
                {
                    objTcktSys.CreatedDate = DateTime.Now;
                    objTcktSys.Reporter = loggedInUserId;
                    activityName = "LogETicket";
                    objTcktSys.Status = "LogETicket";
                    ActivityFullName = "LogETicket";
                }

                #endregion "For New Record details"

                #region "Get the Document Types"
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("DocumentFor", "ETicket");
                Dictionary<long, IList<DocumentTypeMaster>> DocumentTypeMaster = ms.GetDocumentTypeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                if (DocumentTypeMaster != null && DocumentTypeMaster.Count > 0)
                { ViewBag.docTypes = DocumentTypeMaster.FirstOrDefault().Value; }
                else { ViewBag.docTypes = string.Empty; }
                #endregion "Get the Document Types"

                #region "Binding values to Viewbag"
                ViewBag.loggedInUserId = loggedInUserId;
                ViewBag.loggedInUserType = loggedInUserType;
                ViewBag.loggedInUserName = string.IsNullOrWhiteSpace(loggedInUserName) ? loggedInUserId : loggedInUserName;
                ViewBag.activityName = activityName;
                //ViewBag.Status = status;
                ViewBag.ActivityFullName = ActivityFullName;
                ViewBag.ActivityId = ActivityId ?? 0;
                #endregion "Binding values to Viewbag"

                return View(objTcktSys);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
            finally
            { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objTcktSys"></param>
        /// <param name="activityName"></param>
        /// <param name="isrejction"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveTicketSystem(TicketSystem objTcktSys, string activityName, bool? isrejction)
        {
            try
            {
                ProcessFlowServices pfs = new ProcessFlowServices();
                TicketSystemService tcktSrvs = new TicketSystemService();
                TicketComments objTcktCmnts = new TicketComments();
                string loggedInUserId = Request.Form["loggedInUserId"];
                string MailBody;
                activityName = Request.Form["activityName"];
                isrejction = Convert.ToBoolean(Request.Form["isrejction"]);

                objTcktCmnts.CommentedOn = DateTime.Now;
                objTcktCmnts.CommentedBy = loggedInUserId;
                objTcktCmnts.RejectionComments = objTcktSys.Comments;
                string tcktCmnts = objTcktSys.Comments;
                objTcktSys.Comments = string.Empty;
                string tcktId = objTcktSys.TicketNo;
                bool isSucess = false;

                if (activityName == "CloseETicketRejection" || activityName == "ResolveETicket")
                { TempData["AlrtDskMsg"] = tcktId + " has submitted to e-Ticket Creator"; }
                else
                { TempData["AlrtDskMsg"] = tcktId + " has submitted to IT-Team"; }

                if (activityName == "LogETicket" && objTcktSys.Id == 0)
                {
                    tcktId = pfs.StartETicketingSystem(objTcktSys, TemplateName, objTcktSys.Reporter);
                    objTcktSys.Id = Convert.ToInt64(tcktId.Split('-')[1]);
                    objTcktSys.TicketNo = tcktId;

                    if (objTcktSys.Id > 0 && !string.IsNullOrWhiteSpace(tcktCmnts))
                    {
                        objTcktCmnts.TicketId = objTcktSys.Id;
                        if (isrejction == true) { objTcktCmnts.IsRejectionOrResolution = "Rejection"; } else { objTcktCmnts.IsRejectionOrResolution = "Resolution"; }
                        objTcktCmnts.ActivityName = activityName;
                        tcktSrvs.CreateOrUpdateTicketComments(objTcktCmnts);
                    }

                    TempData["AlrtDskMsg"] = tcktId + " has Created Successfully.";

                    return RedirectToAction("TicketingSystem", "TicketingSystem", new { id = objTcktSys.Id, activityName = "LogETicket", status = "LogETicket", ActivityFullName = "LogETicket" });
                }
                else
                {
                    string Ret;
                    if (activityName == "CompleteETicket" && isrejction == true)
                    {
                        Ret = pfs.ReopenActivityTicketSystem(objTcktSys, TemplateName, loggedInUserId, activityName, isrejction ?? false);
                    }
                    else
                    {
                        isSucess = pfs.CompleteActivityTicketSystem(objTcktSys, TemplateName, loggedInUserId, activityName, isrejction ?? false);
                    }

                    if (activityName == "CloseETicket" && isrejction == false)
                    {
                        isSucess = pfs.CompleteActivityTicketSystem(objTcktSys, TemplateName, loggedInUserId, "CompleteETicket", false);
                        TempData["AlrtDskMsg"] = "The " + tcktId + " request has been completed";
                    }

                    if (activityName == "CompleteETicket" && isrejction == true)
                    {
                        TempData["AlrtDskMsg"] = "The " + tcktId + " is Reopened";
                    }

                    if (objTcktSys.Id > 0 && !string.IsNullOrWhiteSpace(tcktCmnts))
                    {
                        objTcktCmnts.TicketId = objTcktSys.Id;
                        if (isrejction == true) { objTcktCmnts.IsRejectionOrResolution = "Rejection"; } else { objTcktCmnts.IsRejectionOrResolution = "Resolution"; }
                        objTcktCmnts.ActivityName = activityName;
                        tcktSrvs.CreateOrUpdateTicketComments(objTcktCmnts);
                    }
                    MailBody = GetBodyofMail();
                    EmailHelper em = new EmailHelper();
                    new Task(() => { em.SendEmailNotification(objTcktSys, loggedInUserId, activityName, isrejction, MailBody); }).Start();
                    return RedirectToAction("TicketingSystemInbox", "TicketingSystem");
                }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, policyName); }
            finally { }
        }

        public void SendEmailNotification(TicketSystem objTcktSys, string userid, string activityName, bool? isrejction)
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
                if (activityName == "LogETicket" || activityName=="ResolveETicketRejection" || activityName == "CloseETicket")
                {
                    criteriaUserAppRole.Add("RoleCode", "ETR");
                }
                else if (activityName == "ResolveETicket" || activityName == "CloseETicketRejection")
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
                                mail.Subject = "e-Ticket Notification " + objTcktSys.TicketNo + " "; string msg = "";

                                foreach (string s in userEmails)
                                {
                                    if (!string.IsNullOrWhiteSpace(s))
                                        mail.To.Add(s);
                                }
                                switch (activityName)
                                {
                                    case "LogETicket":
                                        {
                                            msg += "An e-Ticket " + objTcktSys.TicketNo + " has been Logged for the module " + objTcktSys.Module + " with " + objTcktSys.Priority + " priority. The e-Ticket is Raised by " + userid + ". Please try resolving the ticket based on Priority. The summary of the ticket is <b><i>\"" + objTcktSys.Summary + "\"</i></b> ";
                                            break;
                                        }
                                    case "ResolveETicket":
                                        {
                                            if (rejection) msg += "An e-Ticket " + objTcktSys.TicketNo + " has been Rejected for the module " + objTcktSys.Module + ". The e-Ticket is Rejected for additionnal information by " + userid + ". Please try replying the same based on the comments. ";
                                            else msg += "An e-Ticket " + objTcktSys.TicketNo + " has been Resolved for the module " + objTcktSys.Module + ". The e-Ticket is Resolved by " + userid + " with comments. Please verify the same and complete/Reject based on the results. ";
                                            break;
                                        }
                                    case "ResolveETicketRejection":
                                        {
                                            msg += "An e-Ticket " + objTcktSys.TicketNo + " has been Replied for the module " + objTcktSys.Module + ". Please try resolving the same based on the reply comments. ";
                                            break;
                                        }
                                    case "CloseETicketRejection":
                                        {
                                            if (rejection) msg += ""; 
                                            else msg += "An e-Ticket " + objTcktSys.TicketNo + " has been Rejected for the module " + objTcktSys.Module + ". The e-Ticket is Rejected by " + userid + ". Please try resolving the same based on the comments. ";
                                            break;
                                        }
                                    case "CloseETicket":
                                        {
                                            if (rejection) msg += "An e-Ticket " + objTcktSys.TicketNo + " has been Rejected for the module " + objTcktSys.Module + ". The e-Ticket is Rejected for additionnal information by " + userid + ". Please try replying the same based on the comments. ";
                                            else msg += "An e-Ticket " + objTcktSys.TicketNo + " has been Completed for the module " + objTcktSys.Module + ". The e-Ticket is Completed by " + userid + " with comments <b><i>\"" + objTcktSys.Comments + "\"</i></b> ";
                                            break;
                                        }
                                    default:
                                        return;
                                }
                                string Body = "<b> Dear Recipient, </b> <br/><br/>" +
                                                "" + msg + " <br/><br/>" +
                                                " Regards, <br/>" +
                                                " TIPS team.<br/><br/>" +
                                                "Disclaimer Information: <br/>" +
                                                "This is an automatic email notification. Please do not Reply to this email.";
                                mail.Body = Body;

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
                                    el.Message = Body;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TicketId"></param>
        /// <param name="CommentedBy"></param>
        /// <param name="Note"></param>
        /// <param name="oper"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public long SaveTicketComments(long TicketId, string CommentedBy, string Note, string oper, string id)
        {
            TicketSystemService tcktSrvs = new TicketSystemService();
            TicketComments objtktCmnts = new TicketComments();
            objtktCmnts.CommentedOn = DateTime.Now;
            objtktCmnts.CommentedBy = CommentedBy;
            objtktCmnts.Note = Note;
            objtktCmnts.TicketId = TicketId;
            long tcktCmntsId = tcktSrvs.CreateOrUpdateTicketComments(objtktCmnts);
            return tcktCmntsId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="sidx"></param>
        /// <param name="sord"></param>
        /// <param name="TicketId"></param>
        /// <returns></returns>
        public ActionResult GetTicketCommentDtlsbyTicketId(int page, int rows, string sidx, string sord, long? TicketId)
        {
            try
            {
                if (TicketId == 0)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TicketSystemService tcktSrvc = new TicketSystemService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();

                    sord = sord == "desc" ? "Desc" : "Asc";
                    IList<TicketComments> TcktCmnts = tcktSrvc.GetTicketCommentsByTicketId(TicketId ?? 0);
                    var tcktCmntDtls = TcktCmnts.Where(item => (item.RejectionComments != null));
                    var jsonData = new
                    {
                        rows = (
                        from items in tcktCmntDtls
                        select new
                        {
                            i = items.Id,
                            cell = new string[]
                       {
                            items.Id.ToString(),
                            items.TicketId.ToString(),
                            items.CommentedBy,
                            items.CommentedOn.ToString(),
                            items.RejectionComments, 
                            items.ResolutionComments,
                            items.Note
                       }
                        })
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, policyName); }
            finally
            { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="sidx"></param>
        /// <param name="sord"></param>
        /// <param name="TicketId"></param>
        /// <returns></returns>
        public ActionResult GetTicketNoteDtlsbyTicketId(int page, int rows, string sidx, string sord, long? TicketId)
        {
            try
            {
                if (TicketId == 0)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TicketSystemService tcktSrvc = new TicketSystemService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();

                    sord = sord == "desc" ? "Desc" : "Asc";
                    IList<TicketComments> TcktCmnts = tcktSrvc.GetTicketCommentsByTicketId(TicketId ?? 0);
                    var tcktCmntDtls = TcktCmnts.Where(item => (item.Note != null));
                    var jsonData = new
                    {
                        rows = (
                        from items in tcktCmntDtls
                        select new
                        {
                            i = items.Id,
                            cell = new string[]
                       {
                            items.Id.ToString(),
                            items.TicketId.ToString(),
                            items.CommentedBy,
                            items.CommentedOn.ToString(),
                            items.RejectionComments, 
                            items.ResolutionComments,
                            items.Note
                       }
                        })
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, policyName); }
            finally
            { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ActivityId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public ActionResult AssignActivityToUser(long Id, long ActivityId, string UserId)
        {
            ProcessFlowServices pfs = new ProcessFlowServices();
            try
            {
                bool isAssigned = false;
                if (ActivityId > 0 && !string.IsNullOrWhiteSpace(UserId))
                {
                    isAssigned = pfs.AssignActivity(ActivityId, UserId);
                    TempData["AlrtDskMsg"] = "Eticket-" + Id + " is assigned to " + UserId + "user.";
                }
                TempData["AlrtDskMsg"] = "Eticket-" + Id + " is not assigned. Please try again.";
                return Json(isAssigned, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, policyName); }
            finally
            { pfs = null; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ActivityId"></param>
        /// <returns></returns>
        public ActionResult LoadUserControl(string id, long? ActivityId)
        {
            ViewBag.Id = ActivityId;
            return PartialView(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="rows"></param>
        /// <param name="sidx"></param>
        /// <param name="sord"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult ActivitiesListJqGrid(long? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                ProcessFlowServices pfs = new ProcessFlowServices(); // TODO: Initialize to an appropriate value
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (Id < 0)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                criteria.Add("ProcessRefId", Id);
                criteria.Add("TemplateId", (long)5);
                sord = sord == "desc" ? "Desc" : "Asc";

                Dictionary<long, IList<TicketSystemActivity>> ActivitiesList = pfs.GetTicketSystemActivityListWithsearchCriteria(page - 1, rows, sidx, sord, criteria, string.Empty, null, null);
                if (ActivitiesList != null && ActivitiesList.Count > 0)
                {
                    long totalRecords = ActivitiesList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows = (
                             from items in ActivitiesList.First().Value
                             select new
                             {
                                 i = items.Id,
                                 cell = new string[] { 
                                     items.Id.ToString(), 
                                     items.ActivityFullName, 
                                     items.Available ? "Available" : items.Assigned ? "Assigned" : "Completed", 
                                     items.Performer != null ? items.Performer.ToString() : "", 
                                     items.CreatedDate.ToString(), 
                                     items.AppRole, 
                                     items.TicketSystem.Id.ToString() }
                             })
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        public ActionResult MoveBackToAvailable(long ActivityId)
        {
            ProcessFlowServices pfs = new ProcessFlowServices();
            try
            {
                bool backToAvailable = false;
                if (ActivityId > 0)
                {
                    backToAvailable = pfs.MoveBackToAvailable(ActivityId);
                }
                return Json(backToAvailable, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, policyName); }
            finally
            { pfs = null; }
        }

        #region Ticket Report By Gobi
        public ActionResult TicketReportByPerformer(string PerformerName)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Clear();
                TicketSystemService TktSysSrvc = new TicketSystemService();
                string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
                if (!string.IsNullOrWhiteSpace(PerformerName)) { criteria.Add("Performer", PerformerName); }
                else { if (!string.IsNullOrWhiteSpace(userid)) { criteria.Add("Performer", userid); } }
                Dictionary<long, IList<TicketReport_vw>> TicketReport_vwDetails = TktSysSrvc.GetTicketReport_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                if (TicketReport_vwDetails != null && TicketReport_vwDetails.Count > 0 && TicketReport_vwDetails.First().Key > 0)
                {
                    var TicketDetails = (from u in TicketReport_vwDetails.First().Value
                                         select u).ToList();
                    return Json(TicketDetails, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }

        }
        public ActionResult TicketReport()
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Clear();
                TicketSystemService TktSysSrvc = new TicketSystemService();
                Dictionary<long, IList<TicketReport_vw>> GetTicketReport_vwDetails = TktSysSrvc.GetTicketReport_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                if (GetTicketReport_vwDetails != null && GetTicketReport_vwDetails.Count > 0 && GetTicketReport_vwDetails.First().Key > 0)
                {
                    ViewBag.TicketDetails = GetTicketReport_vwDetails.First().Value.ToList();
                    return View();
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
        }
        public ActionResult TicketDashBoardReport(string PerformerName, string ReportCategory)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Clear();
                TicketSystemService TktSysSrvc = new TicketSystemService();
                if (ReportCategory == "") { ReportCategory = null; }
                DateTime[] datefromto = getFromTodat(ReportCategory);
                if (!string.IsNullOrEmpty(PerformerName)) criteria.Add("Performer", PerformerName);
                criteria.Add("CreatedDate", datefromto);
                Dictionary<long, IList<TicketDashBoardReport_vw>> GetTicketDashBoardReport_vwDetails = TktSysSrvc.GetTicketDashBoardReport_vwListWithPagingAndCriteria(null, 9999, string.Empty, string.Empty, criteria);
                if (GetTicketDashBoardReport_vwDetails != null && GetTicketDashBoardReport_vwDetails.Count > 0 && GetTicketDashBoardReport_vwDetails.First().Key > 0)
                {
                    var TicketDetails = (from u in GetTicketDashBoardReport_vwDetails.First().Value
                                         select u).ToList();
                    return Json(TicketDetails, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
        }
        public DateTime[] getFromTodat(string ReportCategory)
        {

            if (ReportCategory == null)
            {
                ReportCategory = DateTime.Now.Month.ToString("d2");
            }
            DateTime baseDate = DateTime.Now;
            var today = baseDate;
            var yesterday = baseDate.AddDays(-1);
            var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
            var thisWeekEnd = thisWeekStart.AddDays(6).AddSeconds(-1);
            var lastWeekStart = thisWeekStart.AddDays(-7);
            var lastWeekEnd = thisWeekStart.AddDays(-1);
            var thisMonthStart = new DateTime(baseDate.Year, baseDate.Month, 1);
            var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
            var lastMonthStart = thisMonthStart.AddMonths(-1);
            var lastMonthEnd = thisMonthStart.AddSeconds(-1);
            var thisYearStart = new DateTime(baseDate.Year, 1, 1);
            var thisYearEnd = baseDate.AddYears(1).AddSeconds(-1);
            var lastYearStart = new DateTime(baseDate.Year - 1, 1, 1);
            var lastYearEnd = new DateTime(baseDate.Year - 1, 12, 31);
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime[] fromto = new DateTime[2];
            switch (ReportCategory)
            {
                case "01":
                    DateTime janFirst = new DateTime(DateTime.Now.Year, 1, 1);
                    DateTime janLast = new DateTime(DateTime.Now.Year, 1, 31);
                    string janfrom = string.Format("{0:MM/dd/yyyy}", janFirst);
                    string janto = string.Format("{0:MM/dd/yyyy}", janLast);
                    fromDate = Convert.ToDateTime(janfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(janto + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;

                case "02":
                    DateTime febFirst = new DateTime(DateTime.Now.Year, 2, 1);
                    DateTime febLast = new DateTime(DateTime.Now.Year, 2, 28);
                    string febfrom = string.Format("{0:MM/dd/yyyy}", febFirst);
                    string febto = string.Format("{0:MM/dd/yyyy}", febLast);
                    fromDate = Convert.ToDateTime(febfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(febto + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;
                case "03":
                    DateTime marFirst = new DateTime(DateTime.Now.Year, 3, 1);
                    DateTime marLast = new DateTime(DateTime.Now.Year, 3, 31);
                    string marfrom = string.Format("{0:MM/dd/yyyy}", marFirst);
                    string marto = string.Format("{0:MM/dd/yyyy}", marLast);
                    fromDate = Convert.ToDateTime(marfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(marto + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;
                case "04":
                    DateTime aprilFirst = new DateTime(DateTime.Now.Year, 4, 1);
                    DateTime aprilLast = new DateTime(DateTime.Now.Year, 4, 30);
                    string aprilfrom = string.Format("{0:MM/dd/yyyy}", aprilFirst);
                    string aprilto = string.Format("{0:MM/dd/yyyy}", aprilLast);
                    fromDate = Convert.ToDateTime(aprilfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(aprilto + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;
                case "05":
                    DateTime mayFirst = new DateTime(DateTime.Now.Year, 5, 1);
                    DateTime mayLast = new DateTime(DateTime.Now.Year, 5, 31);
                    string mayfrom = string.Format("{0:MM/dd/yyyy}", mayFirst);
                    string mayto = string.Format("{0:MM/dd/yyyy}", mayLast);
                    fromDate = Convert.ToDateTime(mayfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(mayto + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;
                case "06":
                    DateTime juneFirst = new DateTime(DateTime.Now.Year, 6, 1);
                    DateTime juneLast = new DateTime(DateTime.Now.Year, 6, 30);
                    string junefrom = string.Format("{0:MM/dd/yyyy}", juneFirst);
                    string juneto1 = string.Format("{0:MM/dd/yyyy}", juneLast);
                    fromDate = Convert.ToDateTime(junefrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(juneto1 + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;
                case "07":
                    DateTime julyFirst = new DateTime(DateTime.Now.Year, 7, 1);
                    DateTime julyLast = new DateTime(DateTime.Now.Year, 7, 31);
                    string julyfrom = string.Format("{0:MM/dd/yyyy}", julyFirst);
                    string julyto = string.Format("{0:MM/dd/yyyy}", julyLast);
                    fromDate = Convert.ToDateTime(julyfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(julyto + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;
                case "08":
                    DateTime augFirst = new DateTime(DateTime.Now.Year, 8, 1);
                    DateTime augLast = new DateTime(DateTime.Now.Year, 8, 30);
                    string augfrom = string.Format("{0:MM/dd/yyyy}", augFirst);
                    string augto = string.Format("{0:MM/dd/yyyy}", augLast);
                    fromDate = Convert.ToDateTime(augfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(augto + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;
                case "09":
                    DateTime sepFirst = new DateTime(DateTime.Now.Year, 9, 1);
                    DateTime sepLast = new DateTime(DateTime.Now.Year, 9, 31);
                    string sepfrom = string.Format("{0:MM/dd/yyyy}", sepFirst);
                    string septo = string.Format("{0:MM/dd/yyyy}", sepLast);
                    fromDate = Convert.ToDateTime(sepfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(septo + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;
                case "10":
                    DateTime octFirst = new DateTime(DateTime.Now.Year, 10, 1);
                    DateTime octLast = new DateTime(DateTime.Now.Year, 10, 30);
                    string octfrom = string.Format("{0:MM/dd/yyyy}", octFirst);
                    string octto = string.Format("{0:MM/dd/yyyy}", octLast);
                    fromDate = Convert.ToDateTime(octfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(octto + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;
                case "11":
                    DateTime novFirst = new DateTime(DateTime.Now.Year, 11, 1);
                    DateTime novLast = new DateTime(DateTime.Now.Year, 11, 31);
                    string novfrom = string.Format("{0:MM/dd/yyyy}", novFirst);
                    string novto = string.Format("{0:MM/dd/yyyy}", novLast);
                    fromDate = Convert.ToDateTime(novfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(novto + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;
                case "12":
                    DateTime decFirst = new DateTime(DateTime.Now.Year, 6, 1);
                    DateTime decLast = new DateTime(DateTime.Now.Year, 6, 30);
                    string decfrom = string.Format("{0:MM/dd/yyyy}", decFirst);
                    string decto = string.Format("{0:MM/dd/yyyy}", decLast);
                    fromDate = Convert.ToDateTime(decfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(decto + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;
                case "ThisWeek":
                    DateTime thisWeekFirst = new DateTime(thisWeekStart.Year, thisWeekStart.Month, thisWeekStart.Day);
                    DateTime thisWeekLast = new DateTime(thisWeekEnd.Year, thisWeekEnd.Month, thisWeekEnd.Day);
                    string thisWeekfrom = string.Format("{0:MM/dd/yyyy}", thisWeekFirst);
                    string thisWeekto = string.Format("{0:MM/dd/yyyy}", thisWeekLast);
                    fromDate = Convert.ToDateTime(thisWeekfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(thisWeekto + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;
                case "LastWeek":
                    DateTime lastWeekFirst = new DateTime(lastWeekStart.Year, lastWeekStart.Month, lastWeekStart.Day);
                    DateTime lastWeekLast = new DateTime(lastWeekEnd.Year, lastWeekEnd.Month, lastWeekEnd.Day);
                    string lastWeekfrom = string.Format("{0:MM/dd/yyyy}", lastWeekFirst);
                    string lastWeekto = string.Format("{0:MM/dd/yyyy}", lastWeekLast);
                    fromDate = Convert.ToDateTime(lastWeekfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(lastWeekto + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;
                case "ThisMonth":
                    DateTime thisMonthFirst = new DateTime(thisMonthStart.Year, thisMonthStart.Month, thisMonthStart.Day);
                    DateTime thisMonthLast = new DateTime(thisMonthEnd.Year, thisMonthEnd.Month, thisMonthEnd.Day);
                    string thisMonthfrom = string.Format("{0:MM/dd/yyyy}", thisMonthFirst);
                    string thisMonthto = string.Format("{0:MM/dd/yyyy}", thisMonthLast);
                    fromDate = Convert.ToDateTime(thisMonthfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(thisMonthto + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;
                case "LastMonth":
                    DateTime lastMonthFirst = new DateTime(lastMonthStart.Year, lastMonthStart.Month, lastMonthStart.Day);
                    DateTime lastMonthLast = new DateTime(lastMonthEnd.Year, lastMonthEnd.Month, lastMonthEnd.Day);
                    string lastMonthfrom = string.Format("{0:MM/dd/yyyy}", lastMonthFirst);
                    string lastMonthto = string.Format("{0:MM/dd/yyyy}", lastMonthLast);
                    fromDate = Convert.ToDateTime(lastMonthfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(lastMonthto + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
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
                case "Yesterday":
                    DateTime yesFirst = new DateTime(DateTime.Now.Year, yesterday.Month, yesterday.Day);
                    DateTime yesLast = new DateTime(DateTime.Now.Year, yesterday.Month, yesterday.Day);
                    string yesfrom = string.Format("{0:MM/dd/yyyy}", yesFirst);
                    string yesto = string.Format("{0:MM/dd/yyyy}", yesLast);
                    fromDate = Convert.ToDateTime(yesfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(yesto + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;
                case "ThisYear":
                    DateTime thisYearFirst = new DateTime(DateTime.Now.Year, 1, 1);
                    DateTime thisYearLast = new DateTime(DateTime.Now.Year, 12, 31);
                    string thisYearfrom = string.Format("{0:MM/dd/yyyy}", thisYearFirst);
                    string thisYearto = string.Format("{0:MM/dd/yyyy}", thisYearLast);
                    fromDate = Convert.ToDateTime(thisYearfrom + " " + "12:00:00 AM");
                    toDate = Convert.ToDateTime(thisYearto + " " + "23:59:59");
                    fromto[0] = fromDate;
                    fromto[1] = toDate;
                    break;
                case "LastYear":
                    fromto[0] = lastYearStart;
                    fromto[1] = lastYearEnd;
                    break;
            }
            return fromto;
        }

        public ActionResult getTktPerformerList()
        {
            try
            {
                UserService uS = new UserService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("AppCode", "TKT");
                Dictionary<long, IList<UserAppRole>> PerformerList = uS.GetPerformerListWithCriteria(0, 1000, string.Empty, string.Empty, criteria);
                if (PerformerList != null && PerformerList.First().Value != null && PerformerList.First().Value.Count > 0)
                {
                    var PerformerName = (
                             from items in PerformerList.First().Value

                             select new
                             {
                                 Text = items.UserId,
                                 Value = items.UserId
                             }).Distinct().ToList();

                    return Json(PerformerName, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(null, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public string GetBodyofMail()
        {
            string MessageBody = System.IO.File.ReadAllText(HttpContext.Server.MapPath("~/Views/Shared/EmailBody.html"));
            return MessageBody;
        }

        #region Added By Pandiyan
        public ActionResult FillModule()
        {
            try
            {
                TicketSystemService tcktSrvs = new TicketSystemService();
                Dictionary<string, string> modulcod = new Dictionary<string, string>();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<Module>> mdul = tcktSrvs.GetModuleListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                foreach (Module modul in mdul.First().Value)
                {
                    modulcod.Add(modul.ModuleCode, modul.ModuleCode);
                }
                return PartialView("Dropdown", modulcod);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TicketingPolicy");
                throw ex;
            }
        }
        public ActionResult FillTicketType()
        {
            try
            {
                TicketSystemService tcktSrv = new TicketSystemService();
                Dictionary<string, string> ticketcod = new Dictionary<string, string>();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<TicketType>> tcktTyp = tcktSrv.GetTicketTypeListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                foreach (TicketType tickt in tcktTyp.First().Value)
                {

                    ticketcod.Add(tickt.TicketTypeCode, tickt.TicketTypeCode);
                }
                return PartialView("Dropdown", ticketcod);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TicketingPolicy");
                throw ex;
            }
        }
        public ActionResult FillSeverity()
        {
            try
            {
                TicketSystemService tcktSrv = new TicketSystemService();
                Dictionary<string, string> severitycod = new Dictionary<string, string>();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<Severity>> svrty = tcktSrv.GetSeverityListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                foreach (Severity sevrity in svrty.First().Value)
                {

                    severitycod.Add(sevrity.SeverityCode, sevrity.SeverityCode);
                }
                return PartialView("Dropdown", severitycod);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TicketingPolicy");
                throw ex;
            }
        }
        public ActionResult FillPriority()
        {
            try
            {
                TicketSystemService tcktSrv = new TicketSystemService();
                Dictionary<string, string> prioritycod = new Dictionary<string, string>();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<Priority>> prty = tcktSrv.GetPriorityListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                foreach (Priority priorty in prty.First().Value)
                {
                    prioritycod.Add(priorty.PriorityCode, priorty.PriorityCode);
                }
                return PartialView("Dropdown", prioritycod);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TicketingPolicy");
                throw ex;
            }
        }
        public ActionResult FillTicketStatus()
        {
            try
            {
                TicketSystemService tcktSrv = new TicketSystemService();
                Dictionary<string, string> statuscode = new Dictionary<string, string>();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<TicketStatus>> tcktSts = tcktSrv.GetTicketStatusListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                foreach (TicketStatus status in tcktSts.First().Value)
                {
                    statuscode.Add(status.StatusCode, status.StatusCode);
                }
                return PartialView("Dropdown", statuscode);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TicketingPolicy");
                throw ex;
            }
        }
        #endregion

    }
}