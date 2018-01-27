using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TIPS.Entities;
using TIPS.Entities.AdmissionEntities;
using TIPS.Entities.TaskManagement;
using TIPS.Service;
using TIPS.Service.TaskManagement;
using TIPS.ServiceContract;
using TIPS.Entities.TicketingSystem;

namespace CMS.Controllers
{
    public class TaskSystemController : BaseController
    {
        string policyName = "TaskSystemPolicy";
        string TemplateName = "ETask";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult TaskSystemInbox()
        {
            try
            {
                MastersService ms = new MastersService();
                TaskSystemService tskSrv = new TaskSystemService();
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
                criteriaUserAppRole.Add("AppCode", "TSK");
                criteriaUserAppRole.Add("RoleCode", "CRE");
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
                //   ViewBag.Module = string.Empty;
                ViewBag.Priority = string.Empty;
                ViewBag.TaskStatus = string.Empty;
                ViewBag.TaskType = string.Empty;
                ViewBag.Severity = string.Empty;

                Dictionary<long, IList<Priority>> prty = tskSrv.GetPriorityListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                if (prty != null && prty.Count > 0)
                {
                    ViewBag.Priority = (prty.FirstOrDefault().Value).Select(item => new SelectListItem
                    {
                        Value = item.PriorityId.ToString(),
                        Text = item.PriorityCode.ToUpper(),
                    });
                }
                Dictionary<long, IList<TaskStatus>> tcktSts = tskSrv.GetTaskStatusListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                if (tcktSts != null && tcktSts.Count > 0)
                {
                    ViewBag.TaskStatus = (tcktSts.FirstOrDefault().Value).Select(item => new SelectListItem
                    {
                        Value = item.StatusId.ToString(),
                        Text = item.StatusCode.ToUpper(),
                    });
                }
                Dictionary<long, IList<TaskType>> tcktTyp = tskSrv.GetTaskTypeListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                if (tcktTyp != null && tcktTyp.Count > 0)
                {
                    ViewBag.TaskType = (tcktTyp.FirstOrDefault().Value).Select(item => new SelectListItem
                    {
                        Value = item.TaskTypeId.ToString(),
                        Text = item.TaskTypeCode.ToUpper(),
                    });
                }
                Dictionary<long, IList<Severity>> svrty = tskSrv.GetSeverityListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
                if (svrty != null && svrty.Count > 0)
                {
                    ViewBag.Severity = (svrty.FirstOrDefault().Value).Select(item => new SelectListItem
                    {
                        Value = item.SeverityId.ToString(),
                        Text = item.SeverityCode.ToUpper(),
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
        /// <param name="TaskStatus"></param>
        /// <param name="fromDate"></param>
        /// <param name="TaskNo"></param>
        /// <param name="Module"></param>
        /// <param name="TaskType"></param>
        /// <param name="Severity"></param>
        /// <param name="Priority"></param>
        /// <param name="Reporter"></param>
        /// <param name="AssignedTo"></param>
        /// <param name="ActivityFullName"></param>
        /// <param name="Status"></param>
        /// <param name="ExportToXL"></param>
        /// <returns></returns>
        public ActionResult GetTaskSystemInbox(int page, int rows, string sidx, string sord, string TaskStatus,
            string fromDate, string TaskNo, string TaskType, string Severity, string Priority,
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
                criteriaUserAppRole.Add("AppCode", "TSK");
                userAppRole = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteriaUserAppRole);
                //if list userAppRole is null then empty grid
                if (userAppRole != null && userAppRole.Count > 0 && userAppRole.First().Key > 0)
                {
                    int count = userAppRole.First().Value.Count;
                    //if it has values then for each concatenate APP+ROLE 
                    string[] AppRole = new string[count];
                    string[] Roles = new string[count];

                    int i = 0;
                    Dictionary<string, object> criteria = new Dictionary<string, object>();

                    #region "Getting all User Role & App Code"
                    foreach (UserAppRole uar in userAppRole.First().Value)
                    {
                        if (!string.IsNullOrWhiteSpace(uar.AppCode.Trim()) && !string.IsNullOrWhiteSpace(uar.RoleCode.Trim()))
                        {
                            AppRole[i] = uar.AppCode.Trim() + uar.RoleCode.Trim();
                            Roles[i] = uar.RoleCode.Trim();
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
                        criteria.Add("TaskSystem." + "IssueDate", fromto);
                    }

                    Status = (Status == "" || Status == null) ? "Available" : Status;

                    if (!string.IsNullOrEmpty(Status))
                    {
                        criteria.Add(Status == "Sent" ? "Completed" : Status, true);

                        criteria.Add("Performer", userid);
                        if (Status == "Completed") { criteria.Add("ActivityName", "Complete"); }
                    }

                    // if (Status == "Available" && Roles.Contains("RES"))
                    // criteria.Add("Performer", userid); 

                    if (!string.IsNullOrWhiteSpace(TaskNo)) { criteria.Add("TaskSystem.TaskNo", TaskNo); }
                    if (!string.IsNullOrWhiteSpace(TaskType)) { criteria.Add("TaskSystem.TaskType", TaskType); }
                    if (!string.IsNullOrWhiteSpace(Severity)) { criteria.Add("TaskSystem.Severity", Severity); }
                    if (!string.IsNullOrWhiteSpace(Priority)) { criteria.Add("TaskSystem.Priority", Priority); }
                    if (!string.IsNullOrWhiteSpace(TaskStatus)) { criteria.Add("TaskSystem.TaskStatus", TaskStatus); }
                    if (!string.IsNullOrWhiteSpace(Reporter)) { criteria.Add("TaskSystem.Reporter", Reporter); }
                    if (!string.IsNullOrWhiteSpace(AssignedTo)) { criteria.Add("TaskSystem.AssignedTo", AssignedTo); }
                    if (!string.IsNullOrWhiteSpace(ActivityFullName)) { criteria.Add("ActivityFullName", ActivityFullName); }
                    #endregion "criteria builder"

                    string[] alias = new string[1];

                    alias[0] = "TaskSystem";
                    if (sidx == "ActivityFullName")
                    { sidx = sidx.ToString(); }
                    else
                    { sidx = "TaskSystem." + sidx; }

                    sord = sord == "desc" ? "Desc" : "Asc";
                    criteria.Add("TemplateId", (long)9);
                    Dictionary<long, IList<TaskSystemActivity>> ActivitiesList = pfs.GetTaskSystemActivityListWithsearchCriteria(page - 1, rows, sidx, sord, criteria, "AppRole", AppRole, alias);
                    if (ActivitiesList != null && ActivitiesList.Count > 0)
                    {
                        foreach (TaskSystemActivity pi in ActivitiesList.First().Value)
                        {
                            pi.ProcessInstance.DifferenceInHours = DateTime.Now - pi.ProcessInstance.DateCreated;
                        }
                        if (ExportToXL == "true" || ExportToXL == "True")
                        {
                            base.ExptToXL(ActivitiesList.First().Value.ToList(), "TaskList", (items => new
                            {
                                items.TaskSystem.TaskNo,
                                items.ActivityName,
                                items.ActivityFullName,
                                // items.TaskSystem.Module,
                                items.TaskSystem.TaskType,
                                items.TaskSystem.Severity,
                                items.TaskSystem.Priority,
                                items.TaskSystem.TaskStatus,
                                items.TaskSystem.Reporter,
                                CreatedDate = items.TaskSystem.CreatedDate != null ? items.CreatedDate.Value.ToString("dd-MMM-yyyy hh:mm:ss tt") : "",
                                items.TaskSystem.AssignedTo,
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
                                        items.TaskSystem.Id.ToString(),
                                        items.TaskSystem.TaskNo,
                                        items.ActivityName,
                                        items.ActivityFullName, 
                                       // items.TaskSystem.Module,
                                        items.TaskSystem.TaskType,
                                        items.TaskSystem.Severity, 
                                        items.TaskSystem.Priority,
                                        items.TaskSystem.TaskStatus,
                                        items.TaskSystem.Reporter,
                                        items.TaskSystem.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):"",
                                        items.TaskSystem.AssignedTo,
                                        items.Available==true?"Available":(items.Assigned==true?"Assigned":"Completed"),
                                        "<img src='/Images/History.png ' id='ImgHistory' onclick=\"ShowComments('" + items.ProcessRefId + "');\" />",
                                        items.ProcessInstance.Status=="Completed"?"Completed":items.ProcessInstance.DifferenceInHours.Value.TotalHours.ToString(),//SLA
                                        items.TaskSystem.Summary,
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
        public ActionResult TaskSystem(long? Id, long? ActivityId, string activityName, string status, string ActivityFullName)
        {
            try
            {
                MastersService ms = new MastersService();
                TaskSystemService assSrv = new TaskSystemService();
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
                            return RedirectToAction("TaskSystemInbox", "TaskSystem");
                        }
                    }
                }
                #endregion
                //pass userid and get the list of roles from user service GetAppRoleForAnUserListWithPagingAndCriteria
                #region "Get all the Master details"

                Dictionary<string, object> criteriaUserAppRole = new Dictionary<string, object>();
                criteriaUserAppRole.Add("AppCode", "TSK");
                criteriaUserAppRole.Add("RoleCode", "RES");
                Dictionary<long, IList<UserAppRole>> userAppRole = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteriaUserAppRole);
                //  Dictionary<long, IList<Module>> mdul = assSrv.GetModuleListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, null);
                Dictionary<long, IList<Priority>> prty = assSrv.GetPriorityListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, null);
                Dictionary<long, IList<TaskStatus>> tcktSts = assSrv.GetTaskStatusListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, null);
                Dictionary<long, IList<TaskType>> tcktTyp = assSrv.GetTaskTypeListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, null);
                Dictionary<long, IList<Severity>> svrty = assSrv.GetSeverityListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, null);

                //if (mdul != null && mdul.Count > 0)
                //{
                //    ViewBag.ddModule = (mdul.FirstOrDefault().Value).Select(item => new SelectListItem
                //    {
                //        Value = item.ModuleCode.ToString(),
                //        Text = item.ModuleCode.ToUpper()
                //    });
                //}
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
                    ViewBag.ddTaskStatus = (tcktSts.FirstOrDefault().Value).Select(item => new SelectListItem
                    {
                        Value = item.StatusCode.ToString(),
                        Text = item.StatusCode.ToUpper()
                    });
                }
                if (tcktTyp != null && tcktTyp.Count > 0)
                {
                    ViewBag.ddTaskType = (tcktTyp.FirstOrDefault().Value).Select(item => new SelectListItem
                    {
                        Value = item.TaskTypeCode.ToString(),
                        Text = item.TaskTypeCode.ToUpper()
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

                TaskSystem objTskSys = new TaskSystem();
                #region "To get the Existing Record details"
                if (Id > 0)
                {
                    objTskSys = assSrv.GetTaskSystemById(Id ?? 0);
                }
                #endregion "To get the Existing Record details"
                #region "For New Record details"
                else
                {
                    objTskSys.CreatedDate = DateTime.Now;
                    objTskSys.Reporter = loggedInUserId;
                    objTskSys.BranchCode = (Userobj.Campus != null) ? Userobj.Campus : "";
                    activityName = "LogETask";
                    objTskSys.Status = "LogETask";
                    ActivityFullName = "LogETask";
                }

                #endregion "For New Record details"

                #region "Get the Document Types"
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("DocumentFor", "ETask");
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

                return View(objTskSys);
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
        /// <param name="objTskSys"></param>
        /// <param name="activityName"></param>
        /// <param name="isrejction"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveTaskSystem(TaskSystem objTskSys, string activityName, bool? isrejction)
        {
            try
            {
                ProcessFlowServices pfs = new ProcessFlowServices();
                TaskSystemService tskSrv = new TaskSystemService();
                TaskComments objTcktCmnts = new TaskComments();
                string loggedInUserId = Request.Form["loggedInUserId"];

                activityName = Request.Form["activityName"];
                isrejction = Convert.ToBoolean(Request.Form["isrejction"]);

                objTcktCmnts.CommentedOn = DateTime.Now;
                objTcktCmnts.CommentedBy = loggedInUserId;
                objTcktCmnts.RejectionComments = objTskSys.Comments;
                string tcktCmnts = objTskSys.Comments;
                objTskSys.Comments = string.Empty;
                string tcktId = objTskSys.TaskNo;
                bool isSucess = false;

                if (activityName == "CloseETaskRejection" || activityName == "ResolveETask")
                { TempData["AlrtDskMsg"] = tcktId + " has been submitted to e-Task Creator"; }
                else
                { TempData["AlrtDskMsg"] = tcktId + " has been submitted."; }

                if (activityName == "LogETask" && objTskSys.Id == 0)
                {
                    tcktId = pfs.StartETaskSystem(objTskSys, TemplateName, objTskSys.Reporter);
                    objTskSys.Id = Convert.ToInt64(tcktId.Split('-')[1]);
                    objTskSys.TaskNo = tcktId;

                    if (objTskSys.Id > 0 && !string.IsNullOrWhiteSpace(tcktCmnts))
                    {
                        objTcktCmnts.TaskId = objTskSys.Id;
                        tskSrv.CreateOrUpdateTaskComments(objTcktCmnts);
                    }

                    TempData["AlrtDskMsg"] = tcktId + " has been Created Successfully.";

                    return RedirectToAction("TaskSystem", "TaskSystem", new { id = objTskSys.Id, activityName = "LogETask", status = "LogETask", ActivityFullName = "LogETask" });
                }
                else
                {
                    isSucess = pfs.CompleteActivityTaskSystem(objTskSys, TemplateName, loggedInUserId, activityName, isrejction ?? false);

                    if (activityName == "CloseETask" && isrejction == false)
                    {
                        isSucess = pfs.CompleteActivityTaskSystem(objTskSys, TemplateName, loggedInUserId, "CompleteETask", false);
                        TempData["AlrtDskMsg"] = "The " + tcktId + " request has been completed";
                    }

                    if (objTskSys.Id > 0 && !string.IsNullOrWhiteSpace(tcktCmnts))
                    {
                        objTcktCmnts.TaskId = objTskSys.Id;
                        tskSrv.CreateOrUpdateTaskComments(objTcktCmnts);

                    }
                    SendEmailNotification(objTskSys, loggedInUserId, activityName, isrejction);
                    return RedirectToAction("TaskSystemInbox", "TaskSystem");
                }
            }
            catch (Exception ex)
            { return ThrowJSONErrorNew(ex, policyName); }
            finally { }
        }

        public void SendEmailNotification(TaskSystem objTskSys, string userid, string activityName, bool? isrejction)
        {
            try
            {
                IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(objTskSys.BranchCode, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                bool rejection = isrejction ?? true;// false;
                UserService us = new UserService();
                Dictionary<long, IList<UserAppRole>> userAppRole = new Dictionary<long, IList<UserAppRole>>();
                Dictionary<string, object> criteriaUserAppRole = new Dictionary<string, object>();
                //Based on activity need to assign the role ta get the users
                criteriaUserAppRole.Add("AppCode", "TSK");
                if (activityName == "LogETask" || activityName == "ResolveETaskRejection" || activityName == "CloseETask")
                {
                    criteriaUserAppRole.Add("UserId", objTskSys.AssignedTo);
                }
                else if (activityName == "ResolveETask" || activityName == "CloseETaskRejection")
                { criteriaUserAppRole.Add("UserId", objTskSys.Reporter); }
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
                                mail.Subject = "e-Task Notification "; string msg = "";

                                foreach (string s in userEmails)
                                {
                                    if (!string.IsNullOrWhiteSpace(s))
                                        mail.To.Add(s);
                                }
                                switch (activityName)
                                {
                                    case "LogETask":
                                        {
                                            msg += "An e-Task " + objTskSys.TaskNo + " has been Logged with " + objTskSys.Priority + " priority. The e-Task is Raised by " + userid + ". Please try resolving the Task based on Priority. The summary of the Task is <b><i>\"" + objTskSys.Summary + "\"</i></b> ";
                                            break;
                                        }
                                    case "ResolveETask":
                                        {
                                            if (rejection) msg += "An e-Task " + objTskSys.TaskNo + " has been Rejected. The e-Task is Rejected for additionnal information by " + userid + ". Please try replying the same based on the comments. ";
                                            else msg += "An e-Task " + objTskSys.TaskNo + " has been Resolved. The e-Task is Resolved by " + userid + " with comments. Please verify the same and complete/Reject based on the results. ";
                                            break;
                                        }
                                    case "ResolveETaskRejection":
                                        {
                                            msg += "An e-Task " + objTskSys.TaskNo + " has been Replied. Please try resolving the same based on the reply comments. ";
                                            break;
                                        }
                                    case "CloseETaskRejection":
                                        {
                                            if (rejection) msg += "";
                                            else
                                                msg += "An e-Task " + objTskSys.TaskNo + " has been Replied. Please try resolving the same based on the reply comments. ";
                                            break;
                                        }
                                    case "CloseETask":
                                        {
                                            if (rejection) msg += "An e-Task " + objTskSys.TaskNo + " has been Rejected. The e-Task is Rejected for additional information by " + userid + ". Please try replying the same based on the comments. ";
                                            else msg += "An e-Task " + objTskSys.TaskNo + " has been Completed. The e-Task is Completed by " + userid + " with comments <b><i>\"" + objTskSys.Comments + "\"</i></b> ";
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
                                        mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                                        smtp.Credentials = new System.Net.NetworkCredential
                                      (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                                        smtp.Send(mail);
                                    }

                                    catch (Exception ex)
                                    {
                                        if (ex.Message.Contains("quota"))
                                        {
                                            mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                                            smtp.Credentials = new System.Net.NetworkCredential
                                            (campusemaildet.First().AlternateEmailId.ToString(), campusemaildet.First().AlternateEmailIdPassword.ToString());
                                            smtp.Send(mail);
                                        }
                                    }
                                }
                                else if (From == "test")
                                {
                                    mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                                    smtp.Credentials = new System.Net.NetworkCredential
                                   (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
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
                                el.Module = "ETask";
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
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TaskId"></param>
        /// <param name="CommentedBy"></param>
        /// <param name="Note"></param>
        /// <param name="oper"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public long SaveTaskComments(long TaskId, string CommentedBy, string Note, string oper, string id)
        {
            TaskSystemService tskSrv = new TaskSystemService();
            TaskComments objtktCmnts = new TaskComments();
            objtktCmnts.CommentedBy = CommentedBy;
            objtktCmnts.CommentedOn = DateTime.Now;
            objtktCmnts.Note = Note;
            objtktCmnts.TaskId = TaskId;
            long tcktCmntsId = tskSrv.CreateOrUpdateTaskComments(objtktCmnts);
            return tcktCmntsId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="sidx"></param>
        /// <param name="sord"></param>
        /// <param name="TaskId"></param>
        /// <returns></returns>
        public ActionResult GetTaskCommentDtlsbyTaskId(int page, int rows, string sidx, string sord, long? TaskId)
        {
            try
            {
                if (TaskId == 0)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TaskSystemService tskSrv = new TaskSystemService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();

                    sord = sord == "desc" ? "Desc" : "Asc";
                    IList<TaskComments> TcktCmnts = tskSrv.GetTaskCommentsByTaskId(TaskId ?? 0);
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
                            items.TaskId.ToString(),
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
        /// <param name="TaskId"></param>
        /// <returns></returns>
        public ActionResult GetTaskNoteDtlsbyTaskId(int page, int rows, string sidx, string sord, long? TaskId)
        {
            try
            {
                if (TaskId == 0)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TaskSystemService tskSrv = new TaskSystemService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();

                    sord = sord == "desc" ? "Desc" : "Asc";
                    IList<TaskComments> tskCmnts = tskSrv.GetTaskCommentsByTaskId(TaskId ?? 0);
                    var tcktCmntDtls = tskCmnts.Where(item => (item.Note != null));
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
                            items.TaskId.ToString(),
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
                    TempData["AlrtDskMsg"] = "ETask-" + Id + " is assigned to " + UserId + "user.";
                }
                TempData["AlrtDskMsg"] = "ETask-" + Id + " is not assigned. Please try again.";
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
                criteria.Add("TemplateId", (long)9);
                sord = sord == "desc" ? "Desc" : "Asc";

                Dictionary<long, IList<TaskSystemActivity>> ActivitiesList = pfs.GetTaskSystemActivityListWithsearchCriteria(page - 1, rows, sidx, sord, criteria, string.Empty, null, null);
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
                                     items.TaskSystem.Id.ToString() }
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
                ExceptionPolicy.HandleException(ex, policyName);
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

        public ActionResult TaskUserList(string brncd)
        {
            ViewBag.brncd = brncd;
            return View();
        }

        public JsonResult TaskUserListJqGrid(string brncd, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                UserService us = new UserService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("AppCode", "TSK");
                criteria.Add("RoleCode", "RES");
                if (!string.IsNullOrWhiteSpace(brncd))
                {
                    if (brncd.Contains("Select"))
                    { }
                    else
                    {
                        criteria.Add("BranchCode", brncd);
                    }
                }
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<UserAppRole>> userapprole = us.GetAppRoleForAnUserListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (userapprole != null && userapprole.Count > 0)
                {
                    long totalrecords = userapprole.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,

                        rows = (from items in userapprole.First().Value
                                select new
                                {
                                    cell = new string[] {
                                        items.Id.ToString(),
                                        items.UserId,
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
                ExceptionPolicy.HandleException(ex, policyName);
                throw ex;
            }
        }

    }
}
