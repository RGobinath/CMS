using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using CustomAuthentication;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TIPS.Entities;
using TIPS.Entities.MenuEntities;
using TIPS.Service;
using TIPS.ServiceContract;
using System.Web;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.IO;
using PersistenceFactory;
using TIPS.Entities.AdmissionEntities;
using TIPS.Entities.StaffManagementEntities;
using CMS.Helpers;
// First Review Completed (by JP and Anbu on 15 Mar 2014-21 MAr 2014)
//comments were added those need to be revisited again
//Search by using key "REVISIT"
namespace CMS.Controllers
{
    public class AccountController : BaseController
    {
        string PolicyName = "CustomAccountPolicy";
        public ActionResult CustomRegister()
        {
            User u = new User();
            u.UserType = "";
            return View(u);
        }

        [HttpPost]
        public ActionResult CustomRegister(TIPS.Entities.User user)
        {
            try
            {
                UserService us = new UserService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("UserId", user.UserId);
                criteria.Add("Campus", user.Campus);
                criteria.Add("UserName", user.UserName);
                Dictionary<long, IList<User>> UserName = us.GetUserListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);

                if (UserName != null && UserName.First().Value != null && UserName.First().Value.Count > 0)
                {
                    var script = @"ErrMsg(""The given User Name already exists!"");";
                    return JavaScript(script);
                }
                else
                {
                    user.CreatedDate = DateTime.Now;
                    user.ModifiedDate = DateTime.Now;
                    PassworAuth PA = new PassworAuth();
                    //encode and save the password
                    var Password = GenerateRandomString(6);
                    user.Password = PA.base64Encode(Password);
                    user.IsActive = true;
                    us.CreateOrUpdateUser(user);
                    TempData["SuccessUserCreation"] = 1;
                    SendMailToNewUser(user.UserName, user.UserId, Password, user.EmailId, user.Campus);
                    var script = @"InfoMsg(""Successfully Registered in ICMS"");";
                    return JavaScript(script);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        public ActionResult LogOn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LogOn(User user)
        {
            Session["Role"] = "";
            Session["AdminRole"] = "";
            Session["userrole"] = "";
            Session["staffapproverrole"] = "";

            try
            {
                if (!string.IsNullOrWhiteSpace(user.UserId) && !string.IsNullOrWhiteSpace(user.Password))
                {
                    TIPS.Service.UserService us = new UserService();
                    TIPS.Entities.User User = us.GetUserByUserId(user.UserId);
                    PassworAuth PA = new PassworAuth();
                    if (User != null)
                    {
                        if (!string.Equals(user.UserId, User.UserId))
                        {
                            ViewBag.User = "Case Sensitive";
                            return View();
                        }
                        if (User.IsActive == false)
                        {
                            ViewBag.User = "Deactivated";
                            return View();
                        }
                        else
                        {
                            if (Request["Password"] == PA.base64Decode2(User.Password))
                            {
                                LoginHistory s = GetLoginDetails(User);
                                us.CreateOrUpdateSession(s);
                                Session["objUser"] = User;
                                Session["UserId"] = User.UserId;
                                Session["UserName"] = User.UserName;
                                Session["UserCampus"] = User.Campus;
                                Dictionary<string, object> criteriamain = new Dictionary<string, object>();
                                criteriamain.Add("UserId", user.UserId);
                                Dictionary<long, IList<UserAppRole>> userappmain = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteriamain);
                                if (userappmain.FirstOrDefault().Value != null && userappmain.FirstOrDefault().Value.Count != 0)
                                {
                                    if (userappmain.First().Value[0].AppCode != null)
                                    {
                                        var appCode = (from u in userappmain.First().Value
                                                       select u.AppCode).Distinct().ToArray();
                                        Session["Role"] = appCode;
                                        var adminRole = (from v in appCode where v == "All" select v);
                                        if (!string.IsNullOrEmpty(adminRole.FirstOrDefault()))
                                        {
                                            Session["AdminRole"] = adminRole.First().ToString();
                                        }
                                        var usercampus = (from u in userappmain.First().Value
                                                          where u.BranchCode != null
                                                          select u.BranchCode).Distinct().ToArray();
                                        Session["UserCampus"] = usercampus;
                                    }
                                }
                                var userrole = (from r in userappmain.First().Value
                                                select r.RoleCode).Distinct().ToArray();
                                Session["userrolesarray"] = userrole;
                                if (userrole.Contains("ADM-APP"))            //to check if user has student approver access
                                {
                                    Session["userrole"] = "ADM-APP";
                                }
                                if (userrole.Contains("STM-APP"))          //to check if user has staff approver access
                                {
                                    Session["staffapproverrole"] = "STM-APP";
                                }
                                //Session["SiteLinks"] = IdHtmlTags();
                                Session["AceSiteLinks"] = AceIdHtmlTags();//For Ace Menu
                                return RedirectToAction("Home", "Home");
                            }
                            else return View();
                        }
                    }
                    else
                    {
                        ViewBag.User = -1;
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
                throw ex;
            }
            return View();
        }

        public string IdHtmlTags()
        {
            try
            {
                if (Session["Role"].ToString() != "")
                {
                    System.Text.StringBuilder html = new System.Text.StringBuilder();
                    MenuService ms = new MenuService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    var rle = Session["Role"] as IEnumerable<string>;
                    if (rle.Contains("All"))
                        return AdminRoleMenuBuilding(ms, html, criteria);
                    else
                        return OtherRolesMenuBuilding(ms, html, criteria, rle);
                }
                else { return string.Empty; }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }
        private string AdminRoleMenuBuilding(MenuService ms, System.Text.StringBuilder html, Dictionary<string, object> criteria)
        {
            Dictionary<long, IList<TIPS.Entities.MenuEntities.Menu>> submenuitems;
            Dictionary<long, IList<TIPS.Entities.MenuEntities.Menu>> thirdlevelmenuitems;
            html.Clear();
            criteria.Add("ParentORChild", true);
            criteria.Add("Website", "StaffPortal");
            Dictionary<long, IList<TIPS.Entities.MenuEntities.Menu>> mainmenu = ms.GetMenuListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
            html.Append("<ul>");
            foreach (var items in mainmenu.First().Value)
            {
                if (items.MenuName == "Home")
                {
                    html.Append("<li><a href='/AdminTemplate/DashBoardIndex'>Home</a></li>");
                }
                else
                {
                    html.Append("<li><a>" + items.MenuName + "</a><ul>");
                    criteria.Clear();
                    criteria.Add("ParentRefId", Convert.ToInt32(items.Id));
                    criteria.Add("MenuLevel", "Level2");
                    criteria.Add("Website", "StaffPortal");
                    submenuitems = ms.GetMenuListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    foreach (var var in submenuitems.First().Value)
                    {
                        html.Append("<li><a href='/" + var.Controller + "/" + var.Action + "'>" + var.MenuName + "</a>");

                        criteria.Clear();
                        criteria.Add("ParentRefId", Convert.ToInt32(var.Id));
                        criteria.Add("MenuLevel", "Level3");
                        criteria.Add("Website", "StaffPortal");
                        thirdlevelmenuitems = ms.GetMenuListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                        if (thirdlevelmenuitems.First().Value.Count > 0)             // if there is a third level menu
                        {
                            int j = 0;
                            foreach (var tid in thirdlevelmenuitems.First().Value)
                            {
                                if (j == 0)   // To add ul tag only for first time
                                {
                                    html.Append("<ul>");
                                }
                                html.Append("<li><a href='/" + tid.Controller + "/" + tid.Action + "'>" + tid.MenuName + "</a> </li>");
                                j++;
                            }
                            if (j != 0)   //  if ul open tag is added then to add the close tag
                            {
                                html.Append("</ul>");
                            }
                        }
                        html.Append("</li>");
                    }
                    html.Append("</ul></li>");
                    criteria.Clear();
                }
            }
            html.Append("</ul>");
            return html.ToString();
        }
        //private string AdminRoleMenuBuilding(MenuService ms, System.Text.StringBuilder html, Dictionary<string, object> criteria)
        //{
        //    Dictionary<long, IList<TIPS.Entities.MenuEntities.Menu>> submenuitems;
        //    Dictionary<long, IList<TIPS.Entities.MenuEntities.Menu>> thirdlevelmenuitems;
        //    html.Clear();
        //    criteria.Add("ParentORChild", true);
        //    Dictionary<long, IList<TIPS.Entities.MenuEntities.Menu>> mainmenu = ms.GetMenuListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);
        //    html.Append("<ul class='nav nav-list'>");
        //    foreach (var items in mainmenu.First().Value)
        //    {
        //        if (items.MenuName == "Home")
        //        {
        //            html.Append("<li class='hover'><a href='/AdminTemplate/DashBoardIndex' class='dropdown-toggle'><span class='menu-text'> Home </span><b class='arrow fa fa-angle-down'></b></a><b class='arrow'></b></li>");
        //            //html.Append("<li class='active hover'><a href='/AdminTemplate/DashBoardIndex'>Home</a></li>");
        //        }
        //        else
        //        {
        //            //html.Append("<li><a>" + items.MenuName + "</a><ul>");
        //            html.Append("<li class='hover'><a href='~//' class='dropdown-toggle'><span class='menu-text'>" + items.MenuName + "</span><b class='arrow fa fa-angle-down'></b></a><b class='arrow'></b><ul class='submenu'>");
        //            //html.Append("<li><a>" + items.MenuName + "</a><ul>");
        //            criteria.Clear();
        //            criteria.Add("ParentRefId", Convert.ToInt32(items.Id));
        //            criteria.Add("MenuLevel", "Level2");
        //            submenuitems = ms.GetMenuListWithPagingAndCriteria(0, 100, string.Empty, string.Empty, criteria);
        //            foreach (var var in submenuitems.First().Value)
        //            {
        //                //html.Append("<li><a href='/" + var.Controller + "/" + var.Action + "'>" + var.MenuName + "</a>");
        //                html.Append("<li class='hover'><a href='/" + var.Controller + "/" + var.Action + "'><i class='menu-icon fa fa-caret-right'></i>" + var.MenuName + "</a><b class='arrow'></b>");

        //                criteria.Clear();
        //                criteria.Add("ParentRefId", Convert.ToInt32(var.Id));
        //                criteria.Add("MenuLevel", "Level3");
        //                thirdlevelmenuitems = ms.GetMenuListWithPagingAndCriteria(0, 100, string.Empty, string.Empty, criteria);
        //                if (thirdlevelmenuitems.First().Value.Count > 0)             // if there is a third level menu
        //                {
        //                    int j = 0;
        //                    foreach (var tid in thirdlevelmenuitems.First().Value)
        //                    {
        //                        if (j == 0)   // To add ul tag only for first time
        //                        {
        //                            html.Append("<ul class='submenu'>");
        //                        }
        //                        html.Append("<li class='hover'><a href='/" + tid.Controller + "/" + tid.Action + "'><i class='menu-icon fa fa-caret-right'></i>" + tid.MenuName + "</a><b class='arrow'></b></li>");
        //                        j++;
        //                    }
        //                    if (j != 0)   //  if ul open tag is added then to add the close tag
        //                    {
        //                        html.Append("</ul>");
        //                    }
        //                }
        //                html.Append("</li>");
        //            }
        //            html.Append("</ul></li>");
        //            criteria.Clear();
        //        }
        //    }
        //    html.Append("</ul>");
        //    return html.ToString();
        //}
        private string OtherRolesMenuBuilding(MenuService ms, System.Text.StringBuilder html, Dictionary<string, object> criteria, IEnumerable<string> rle)
        {
            long[] parentrefid = new long[rle.Count()];
            int i = 0;
            criteria.Clear();
            criteria.Add("Role", rle.ToArray());
            criteria.Add("ParentORChild", false);
            criteria.Add("Website", "StaffPortal");
            Dictionary<long, IList<Menu>> mainmenuparentid = ms.GetMenuListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
            if (mainmenuparentid != null && mainmenuparentid.FirstOrDefault().Value != null && mainmenuparentid.FirstOrDefault().Value.Count > 0)
            {
                foreach (Menu m in mainmenuparentid.First().Value)
                {
                    if (!parentrefid.Contains(m.ParentRefId))
                    {
                        parentrefid[i] = m.ParentRefId;
                        i++;
                    }
                }
            }
            html.Append("<ul>");
            html.Append("<li><a href='/Home/Home'>Home</a></li>");
            //get all the menu items inside the for each loop need to be moved here
            //some other time this need to be concentrated to get all the menu table list, after that the same need to be used for the menu building
            //this is to avoid repeated db read on the same table
            IList<Menu> menuList = ms.GetMenusById(parentrefid.Distinct().ToArray());
            if (menuList != null)
            {
                foreach (long id in parentrefid.Distinct().ToArray())  // to remove repeated parent id's for two or more submenu items
                {
                    if (id != 0)
                    {
                        criteria.Clear();
                        criteria.Add("Id", id);
                        Menu mainmenubyid = menuList.First(s => s.Id == id);
                        if (mainmenubyid.MenuLevel == "Level1")//to take level1 menu--- comments updated by jp,anbu
                            html.Append("<li><a>" + mainmenubyid.MenuName + "</a><ul>");

                        criteria.Clear();
                        criteria.Add("ParentRefId", Convert.ToInt32(id));
                        criteria.Add("MenuLevel", "Level2");
                        criteria.Add("Website", "StaffPortal");
                        Dictionary<long, IList<Menu>> submenubyparentid = ms.GetMenuListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                        foreach (var pid in submenubyparentid.First().Value)
                        {
                            if (rle.Contains(pid.Role))
                            {
                                html.Append("<li><a href='/" + pid.Controller + "/" + pid.Action + "'>" + pid.MenuName + "</a>");
                                criteria.Clear();
                                criteria.Add("ParentRefId", Convert.ToInt32(pid.Id));
                                criteria.Add("MenuLevel", "Level3");
                                criteria.Add("Website", "StaffPortal");
                                Dictionary<long, IList<TIPS.Entities.MenuEntities.Menu>> thirdlevelmenu = ms.GetMenuListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                                if (thirdlevelmenu.First().Value.Count > 0)             // if there is a third level menu
                                {
                                    int j = 0;
                                    foreach (var tid in thirdlevelmenu.First().Value)
                                    {
                                        if (rle.Contains(tid.Role))
                                        {
                                            if (j == 0)   // To add ul tag only for first time
                                            {
                                                html.Append("<ul>");
                                            }
                                            html.Append("<li><a href='/" + tid.Controller + "/" + tid.Action + "'>" + tid.MenuName + "</a> </li>");
                                            j++;
                                        }
                                    }
                                    if (j != 0)   //  if ul open tag is added then to add the close tag
                                    {
                                        html.Append("</ul>");
                                    }
                                }
                                html.Append("</li>");
                            }
                        }
                        html.Append("</ul></li>");
                    }
                }
                html.Append("</ul>");
                return html.ToString();
            }
            else return string.Empty;
        }
        public ActionResult CustomChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CustomChangePassword(TIPS.Entities.User user)
        {
            string oldPassword = Request["Password"];
            try
            {
                //old password should be verified with the stored password in table
                TIPS.Service.UserService us = new UserService();
                TIPS.Entities.User User = us.GetUserByUserId(user.UserId);
                if (User != null)
                {
                    PassworAuth PA = new PassworAuth();
                    if (oldPassword != PA.base64Decode2(User.Password))
                    {
                        //throw new Exception("Old password doesn't match with the stored password for user " + user.UserId + "");
                        var script = @"ErrMsg(""The Password enterd by you does not match our previous records for user"");";
                        return JavaScript(script);
                    }
                    //new and confirm should match
                    else
                    {
                        User.Password = PA.base64Encode(user.NewPassword);
                        us.CreateOrUpdateUser(User);
                        //change the password for the user
                        TempData["SuccessPassChange"] = 1;
                        return RedirectToAction("Login", "Account");
                    }
                }
                else
                {
                    ViewBag.User = -1;
                    return View();
                }

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        public ActionResult CustomForgotPassword()
        {
            return View();
        }
        //Comments JP and Anbu
        //Send email need to be with new task so that ui no need to wait for response --done
        [HttpPost]
        public ActionResult CustomForgotPassword(User user)
        {
            try
            {
                TIPS.Service.UserService us = new UserService();
                TIPS.Entities.User User = us.GetUserByEmailId(user.EmailId);
                if (User != null)
                {
                    if (!User.IsActive)
                    {
                        ViewBag.User = "Deactivated";
                        return View();
                    }
                    else
                    {
                        PassworAuth PA = new PassworAuth();
                        string password = PA.base64Decode2(User.Password);
                        string SendEmail = ConfigurationManager.AppSettings["SendEmail1"];
                        if (SendEmail == "false")
                            return null;
                        else
                        {
                            IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(User.Campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
                            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                            mail.To.Add(user.EmailId);
                            mail.Subject = "Password";

                            string Body = "Dear Sir/Madam, <br/><br/>" +
                                        " Your password is " + password + ". <br/><br/>" +
                                        " Try to login with the given password. <br/><br/>" +
                                        " Regards, <br/>" +
                                        " TIPS Support desk";
                            mail.Body = Body;
                            mail.IsBodyHtml = true;
                            SmtpClient smtp = new SmtpClient("localhost", 25);
                            smtp.Host = "smtp.gmail.com";
                            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtp.EnableSsl = true;
                            try
                            {
                                mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                                smtp.Credentials = new System.Net.NetworkCredential
                                (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());

                                if (ValidEmailOrNot(mail.From.ToString()) && ValidEmailOrNot(mail.To.ToString()))
                                {
                                    new Task(() => { SendEmailWithForNewTask(mail, smtp); }).Start();
                                }
                                ViewBag.PasswordSentMessage = 1;
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("quota"))
                                {
                                    mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                                    smtp.Credentials = new System.Net.NetworkCredential
                                    (campusemaildet.First().AlternateEmailId.ToString(), campusemaildet.First().AlternateEmailIdPassword.ToString());
                                    if (ValidEmailOrNot(mail.From.ToString()) && ValidEmailOrNot(mail.To.ToString()))
                                    {
                                        new Task(() => { SendEmailWithForNewTask(mail, smtp); }).Start();
                                    }
                                }
                                else
                                {
                                    mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                                    smtp.Credentials = new System.Net.NetworkCredential
                                    (campusemaildet.First().AlternateEmailId.ToString(), campusemaildet.First().AlternateEmailIdPassword.ToString());
                                    if (ValidEmailOrNot(mail.From.ToString()) && ValidEmailOrNot(mail.To.ToString()))
                                    {
                                        new Task(() => { SendEmailWithForNewTask(mail, smtp); }).Start();
                                    }
                                }
                            }
                            return View();
                        }
                    }
                }
                else
                {
                    ViewBag.Message = -1;
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
                throw ex;
            }
        }

        public ActionResult LogOff()
        {
            string userId = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
            if (!string.IsNullOrWhiteSpace(userId))
            {
                try
                {
                    new Task(() => { UpdateUserLogoff(userId); }).Start();
                    Session.Remove("UserId");
                    Session.Remove("User");
                    Session.Remove("SiteLinks");
                    Session.RemoveAll();
                    Session["Back"] = "No";
                    Session.Abandon();
                    Session.Clear();
                    Session.Remove(Session.SessionID);
                    Session.Contents.RemoveAll();
                    //Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1)); Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    //Response.Cache.SetNoStore();
                }
                catch (Exception ex)
                {
                    ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
                    throw ex;
                }
            }
            return RedirectToAction("LogIn", "Account");
        }

        public ActionResult FillUserType()
        {
            try
            {
                MastersService ms = new MastersService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                //use the below method without the count value, since this method is not using the value "REVISIT"
                //anbu need to change this logic whenver is possible
                Dictionary<long, IList<UserTypeMaster>> UserTypeList = ms.GetUserTypeMasterListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                if (UserTypeList != null && UserTypeList.First().Value != null && UserTypeList.First().Value.Count > 0)
                {
                    var UserType = (
                             from items in UserTypeList.First().Value
                             select new
                             {
                                 Text = items.UserType,
                                 Value = items.UserType
                             }).Distinct().ToList().OrderBy(x => x.Text);
                    return Json(UserType, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
                throw ex;
            }
        }
        public ActionResult UserList()
        {
            return View();
        }
        public ActionResult UserListJqGrid(string UserId, string UserName, string UserType, string EmailId, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                UserService us = new UserService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(UserId))
                {
                    criteria.Add("UserId", UserId);
                }
                if (!string.IsNullOrEmpty(UserName))
                {
                    criteria.Add("UserName", UserName);
                }
                if (!string.IsNullOrEmpty(UserType))
                {
                    criteria.Add("UserType", UserType);
                }
                if (!string.IsNullOrEmpty(EmailId))
                {
                    criteria.Add("EmailId", EmailId);
                }
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<User>> UserList = us.GetUserListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (UserList != null && UserList.Count > 0)
                {
                    long totalrecords = UserList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var AssLst = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (
                             from items in UserList.First().Value

                             select new
                             {
                                 cell = new string[] 
                                         {
                                            items.UserId,
                                            items.UserName,
                                            items.UserType,
                                            items.EmailId,
                                            items.Password,
                                            items.IsActive.ToString(),
                                         }
                             }).ToList()
                    };
                    return Json(AssLst, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
                throw ex;
            }
        }
        public ActionResult UserModification()
        {//Modified by Thamizhmani
            string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
            return View(new User() { UserType = "" });
        }
        [HttpPost]
        public ActionResult UserModification(User user)
        {
            try
            {
                UserService us = new UserService();
                if (user != null)
                {
                    us.ModifyUser(user);
                    ViewBag.SuccessMsg = "User details changed successfully";
                    return View();
                } return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        public ActionResult LoginHistory()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {//Modified by Thamizhmani
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        public ActionResult LoginListJQGrid(string UserId, string usertyp, string FrmDate, string srchtyp, string ExportType, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(UserId))
                    criteria.Add("UserId", UserId);
                if (!string.IsNullOrEmpty(usertyp))
                    criteria.Add("UserType", usertyp);
                if ((!string.IsNullOrEmpty(FrmDate) && !(string.IsNullOrEmpty(FrmDate.Trim()))))
                {
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    FrmDate = FrmDate.Trim();
                    DateTime FromDate = DateTime.Parse(FrmDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    string To = string.Format("{0:MM/dd/yyyy}", DateTime.Now);
                    DateTime ToDate = Convert.ToDateTime(To + " " + "23:59:59");
                    DateTime[] fromto = new DateTime[2];
                    fromto[0] = Convert.ToDateTime(FromDate);
                    fromto[1] = ToDate;
                    criteria.Add("TimeIn", fromto);
                }
                if (!string.IsNullOrEmpty(srchtyp))
                {
                    DateTime fdate = DateTime.Now;
                    DateTime tdate = DateTime.Now;
                    DateTime[] fromto = new DateTime[2];
                    switch (srchtyp)
                    {
                        case "Today":
                            {
                                string from = string.Format("{0:MM/dd/yyyy}", DateTime.Now);
                                fdate = Convert.ToDateTime(from + " " + "12:00:00 AM");
                                tdate = Convert.ToDateTime(from + " " + "23:59:59");
                                fromto[0] = fdate;
                                fromto[1] = tdate;
                                criteria.Add("TimeIn", fromto);
                                break;
                            }
                        case "Yesterday":
                            {
                                string from = string.Format("{0:MM/dd/yyyy}", DateTime.Now.AddDays(-1));
                                fdate = Convert.ToDateTime(from + " " + "12:00:00 AM");
                                tdate = Convert.ToDateTime(from + " " + "23:59:59");
                                fromto[0] = fdate;
                                fromto[1] = tdate;
                                criteria.Add("TimeIn", fromto);
                                break;
                            }
                        case "This Week":
                            {
                                CultureInfo info = Thread.CurrentThread.CurrentCulture;
                                DayOfWeek firstday = info.DateTimeFormat.FirstDayOfWeek;
                                DayOfWeek today = info.Calendar.GetDayOfWeek(DateTime.Now);

                                int diff = today - firstday;
                                DateTime firstDate = DateTime.Now.AddDays(-diff);
                                DateTime LastDate = firstDate.AddDays(7);

                                string from = string.Format("{0:MM/dd/yyyy}", firstDate);
                                string to = string.Format("{0:MM/dd/yyyy}", LastDate);
                                fdate = Convert.ToDateTime(from + " " + "12:00:00 AM");
                                tdate = Convert.ToDateTime(to + " " + "23:59:59");
                                fromto[0] = fdate;
                                fromto[1] = tdate;
                                criteria.Add("TimeIn", fromto);
                                break;
                            }
                        case "Last Week":
                            {
                                int days = DateTime.Now.DayOfWeek - DayOfWeek.Sunday;
                                DateTime firstDate = DateTime.Now.AddDays(-(days + 7));
                                DateTime LastDate = firstDate.AddDays(6);
                                string from = string.Format("{0:MM/dd/yyyy}", firstDate);
                                string to = string.Format("{0:MM/dd/yyyy}", LastDate);
                                fdate = Convert.ToDateTime(from + " " + "12:00:00 AM");
                                tdate = Convert.ToDateTime(to + " " + "23:59:59");
                                fromto[0] = fdate;
                                fromto[1] = tdate;
                                criteria.Add("TimeIn", fromto);
                                break;
                            }
                        case "This Month":
                            {
                                DateTime firstDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                                DateTime LastDate = firstDate.AddMonths(1).AddDays(-1);
                                string from = string.Format("{0:MM/dd/yyyy}", firstDate);
                                string to = string.Format("{0:MM/dd/yyyy}", LastDate);
                                fdate = Convert.ToDateTime(from + " " + "12:00:00 AM");
                                tdate = Convert.ToDateTime(to + " " + "23:59:59");
                                fromto[0] = fdate;
                                fromto[1] = tdate;
                                criteria.Add("TimeIn", fromto);
                                break;
                            }
                        case "Last Month":
                            {
                                DateTime firstDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
                                DateTime LastDate = firstDate.AddMonths(1).AddDays(-1);
                                string from = string.Format("{0:MM/dd/yyyy}", firstDate);
                                string to = string.Format("{0:MM/dd/yyyy}", LastDate);
                                fdate = Convert.ToDateTime(from + " " + "12:00:00 AM");
                                tdate = Convert.ToDateTime(to + " " + "23:59:59");
                                fromto[0] = fdate;
                                fromto[1] = tdate;
                                criteria.Add("TimeIn", fromto);
                                break;
                            }
                        default: break;
                    }
                }
                sord = sord == "desc" ? "Desc" : "Asc";
                UserService us = new UserService();
                Dictionary<long, IList<LoginHistory>> LoginList = us.GetSessionListWithPaging(page - 1, rows, sord, sidx, criteria);
                if (LoginList != null && LoginList.Count > 0)
                {
                    if (ExportType == "Excel")
                    {
                        var List = LoginList.First().Value.ToList();
                        base.ExptToXL(List, "LoginList", (items => new
                        {
                            items.Id,
                            items.UserId,
                            UserName = items.UserId != null ? us.GetUserNameByUserId(items.UserId) : "",
                            items.UserType,
                            TimeIn = items.TimeIn != null ? items.TimeIn.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : null,
                            TimeOut = items.TimeOut != null ? items.TimeOut.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : null,
                            items.IPAddress,
                            items.BrowserName,
                            items.BrowserVersion,
                            items.BrowserType,
                            items.Platform,
                        }));
                        return new EmptyResult();
                    }
                    else
                    {
                        long totalrecords = LoginList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var LoginLst = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (
                                 from items in LoginList.First().Value
                                 select new
                                 {
                                     cell = new string[] 
                                         {
                                            items.Id.ToString(),
                                            items.UserId,
                                            items.UserId != null ? us.GetUserNameByUserId(items.UserId) : "",
                                            items.UserType,
                                            items.TimeIn!=null?items.TimeIn.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):null,
                                            items.TimeOut!=null?items.TimeOut.Value.ToString("dd/MM/yyyy hh:mm:ss tt"):null,
                                            items.IPAddress,
                                            items.BrowserName,
                                            items.BrowserVersion,
                                            items.BrowserType,
                                            items.Platform,
                                         }
                                 }).ToList()
                        };
                        return Json(LoginLst, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
                ViewBag.Error = ex.Message;
                return View();
            }
        }
        public LoginHistory GetLoginDetails(User u)
        {
            LoginHistory s = new LoginHistory();
            System.Web.HttpBrowserCapabilitiesBase browser = Request.Browser;
            s.UserId = u.UserId;
            s.UserType = u.UserType;
            s.TimeIn = DateTime.Now;
            s.IPAddress = Request.UserHostAddress;
            //s.IPAddress = GetIP();    
            s.BrowserName = browser.Browser;
            s.BrowserVersion = browser.Version;
            s.Platform = browser.Platform;
            s.BrowserType = browser.Type;
            return s;
        }
        public ActionResult GetPassword()
        {//Modified by Thamizh
            string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
            return View();
        }

        public ActionResult GetPasswordJqGrid(string Campus, string Grade, string Section, string UserType, string AcademicYear, string userid, int rows, string sidx, string sord, int? page = 1, long? ExptXl = 0)
        {
            try
            {
                UserService us = new UserService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(userid))
                {
                    criteria.Add("UserId", userid);
                }
                if (!string.IsNullOrEmpty(Campus))
                {
                    criteria.Add("Campus", Campus);
                }
                if (!string.IsNullOrEmpty(Grade))
                {
                    criteria.Add("Grade", Grade);
                }
                if (!string.IsNullOrEmpty(Section))
                {
                    criteria.Add("Section", Section);
                }
                if (!string.IsNullOrEmpty(UserType))
                {
                    criteria.Add("UserType", UserType);
                }
                if (!string.IsNullOrEmpty(AcademicYear))
                {
                    criteria.Add("AcademicYear", AcademicYear);
                }

                sord = sord == "desc" ? "Desc" : "Asc";
                //Dictionary<long, IList<User>> UserList = us.GetUserListWithPagingAndCriteria(0, 9999, sidx, sord, criteria);
                Dictionary<long, IList<Users_vw>> UserList = us.GetUsers_vwListWithPagingAndCriteria(0, 9999, sidx, sord, criteria);
                PassworAuth PA = new PassworAuth();
                //IList<Users_vw> UserListValue = new List<Users_vw>();
                //int count = UserList.First().Value.Count;
                //if (UserList != null && UserList.Count > 0 && UserList.First().Key > 0)
                //{
                //    string[] Password = new string[count];
                //    string[] UserId = new string[count];
                //    var li = (from items in UserList.First().Value
                //              select new { userid = items.UserId, pass = items.Password }).ToArray();
                //    for (int i = 0; i < li.Length; i++)
                //    {
                //        Users_vw u = new Users_vw();
                //        u.Campus
                //        u.UserId = li[i].userid;
                //        u.Password = PA.base64Decode2(li[i].pass);                        
                //        UserListValue.Add(u);
                //    }
                //}
                if (UserList != null && UserList.Count > 0 && UserList.FirstOrDefault().Key > 0)
                {
                    if (ExptXl == 1)
                    {
                        base.ExptToXL(UserList.FirstOrDefault().Value,"UserDetails",(item=>new 
                        {                            
                            item.Campus,
                            item.Grade,
                            item.Section,
                            item.AcademicYear,
                            item.NewId,                               
                            item.PreRegNum,                              
                            item.EmployeeId,
                            item.UserId,
                            item.UserType,
                            item.UserName,                               
                            item.EmailId,                               
                            Password=PA.base64Decode2(item.Password),
                            IsActive=item.IsActive==true?"Yes":"No"
                        }));
                        return new EmptyResult();                        
                    }
                    else
                    {
                        long totalrecords = UserList.First().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var jsondat = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (from items in UserList.FirstOrDefault().Value
                                    select new
                                    {
                                        cell = new string[] {
                               items.Id.ToString(),
                               items.Campus,
                               items.Grade,
                               items.Section,
                               items.AcademicYear,
                               items.NewId,                               
                               items.PreRegNum.ToString(),                               
                               items.EmployeeId,
                               items.UserId,
                               items.UserType,
                               items.UserName,                               
                               items.EmailId,                               
                               items.Password=PA.base64Decode2(items.Password),
                               items.IsActive==true?"Yes":"No"
                            }
                                    }).ToList()
                        };
                        return Json(jsondat, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
                throw ex;
            }
        }
        public JsonResult GetUserIds(string term)
        {
            try
            {
                UserService us = new UserService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                criteria.Add("UserId", term);
                //change the method to not to use the count since it is not being used here "REVISIT"
                Dictionary<long, IList<User>> UserList = us.GetUserListWithPagingAndCriteriaLikeSearch(0, 9999, string.Empty, string.Empty, criteria);
                var UserIds = (from u in UserList.First().Value
                               where u.UserId != null
                               select u.UserId).Distinct().ToList();
                return Json(UserIds, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
                throw ex;
            }
        }
        #region "MenuList Details"

        public ActionResult Menu()
        {
            try
            {
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "SystemMgntPolicy");
                throw ex;
            }
        }

        public JsonResult Menujqgrid(string ParentId, int rows, string sidx, string sord, int? page = 1)
        {
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<Menu>> menulist = new Dictionary<long, IList<Menu>>();
            sord = sord == "asc" ? "Asc" : "Desc";
            if (string.IsNullOrEmpty(ParentId))
            {
                criteria.Add("ParentORChild", true);
                menulist = ms.GetMenuListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
            }
            else
            {
                int parentid = Convert.ToInt32(ParentId);
                criteria.Add("ParentRefId", parentid);
                menulist = ms.GetMenuListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
            }
            if (menulist != null && menulist.Count > 0)
            {
                long totalrecords = menulist.First().Key;
                int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                var jsondat1 = new
                {
                    total = totalPages,
                    page = page,
                    records = totalrecords,
                    rows = (from items in menulist.First().Value
                            select new
                            {
                                i = 2,
                                cell = new string[] {
                               items.Id.ToString(),
                               items.MenuName,
                               items.MenuLevel,
                               items.Role,
                               items.Controller,
                               items.Action,
                            }
                            })
                };
                return Json(jsondat1, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        //felix need to check why he added the edit as parameter "REVISIT"
        public ActionResult AddMenu(Menu m, string edit)
        {
            try
            {
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ms = new MastersService();
                    m.ParentORChild = true;
                    ms.SaveOrUpdateMenuDetails(m);
                }
                return RedirectToAction("Masters", "Home");
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "SystemMgntPolicy");
                throw ex;
            }
        }
        //this method uses for each to remove the child of a parent "REVISIT"
        //this need to be changed bu getting all the items by using long[] array and delete using deleteAll
        public ActionResult DeleteMenu(long id)
        {
            try
            {
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ms = new MastersService();
                    Menu mu = new Menu();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("ParentRefId", (int)id);
                    Dictionary<long, IList<Menu>> DeleteMenuList = ms.GetMenuListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    if (DeleteMenuList != null && DeleteMenuList.Count > 0 && DeleteMenuList.First().Key > 0)
                    {
                        long[] SubMenuIds = (from items in DeleteMenuList.First().Value
                                             select items.Id).ToArray();
                        for (int i = 0; i < SubMenuIds.Length; i++)
                        {
                            mu = ms.GetDeleteMenurowById(SubMenuIds[i]);
                            ms.DeleteMenufunction(mu);
                        }
                    }
                    mu = ms.GetDeleteMenurowById(id);
                    ms.DeleteMenufunction(mu);
                    var script = @"SucessMsg(""Deleted  Successfully"");";
                    return JavaScript(script);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "SystemMgntPolicy");
                throw ex;
            }
        }

        public ActionResult AddSubMenus(Menu Submenu, int ids)
        {
            try
            {
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ms = new MastersService();
                    Submenu.ParentORChild = false;
                    Submenu.MenuLevel = "Level2";
                    Submenu.ParentRefId = ids;
                    ms.SaveOrUpdateSubMenuDetails(Submenu);
                }
                return RedirectToAction("Masters", "Home");
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "SystemMgntPolicy");
                throw ex;
            }
        }

        public ActionResult DeleteSubMenus(long id)
        {
            try
            {
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    MastersService ms = new MastersService();
                    Menu mu = ms.DeleteSubMenurowById(id);
                    ms.DeleteMenufunction(mu);
                    var script = @"SucessMsg(""Deleted  Successfully"");";
                    return JavaScript(script);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "SystemMgntPolicy");
                throw ex;
            }
        }
        #endregion "End MenuList Details"

        public ActionResult BulkUserCreation()
        {//Modified by Thamizhmani
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, PolicyName);
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult BulkUserCreation(HttpPostedFileBase[] uploadedFile)
        {
            StringBuilder retValue = new StringBuilder();
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    UserService us = new UserService();
                    HttpPostedFileBase theFile = HttpContext.Request.Files["uploadedFile"];
                    StringBuilder alreadyExists = new StringBuilder();
                    StringBuilder ErrorFilename = new StringBuilder();
                    StringBuilder UploadedFilename = new StringBuilder();
                    if (theFile != null && theFile.ContentLength > 0)
                    {
                        string fileName = string.Empty;
                        int length = uploadedFile.Length;
                        for (int l = 0; l < length; l++)
                        {
                            try
                            {
                                string path = uploadedFile[l].InputStream.ToString();
                                byte[] imageSize = new byte[uploadedFile[l].ContentLength];
                                uploadedFile[l].InputStream.Read(imageSize, 0, (int)uploadedFile[l].ContentLength);
                                string UploadConnStr = "";
                                fileName = uploadedFile[l].FileName;
                                string fileExtn = Path.GetExtension(uploadedFile[l].FileName);
                                string fileLocation = ConfigurationManager.AppSettings["BulkUserCreationFilePath"].ToString() + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss").Replace(":", ".") + fileName;
                                uploadedFile[l].SaveAs(fileLocation);
                                if (fileExtn == ".xls")
                                {
                                    UploadConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
                                }
                                if (fileExtn == ".xlsx")
                                {
                                    UploadConnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
                                }
                                OleDbConnection itemconn = new OleDbConnection();
                                DataTable DtblXcelItemData = new DataTable();
                                string QeryToGetXcelItemData = "select * from " + string.Format("{0}${1}", "[Sheet1", "A1:AZ]");
                                itemconn.ConnectionString = UploadConnStr;
                                itemconn.Open();
                                OleDbCommand cmd = new OleDbCommand(QeryToGetXcelItemData, itemconn);
                                cmd.CommandType = CommandType.Text;
                                OleDbDataAdapter DtAdptrr = new OleDbDataAdapter();
                                DtAdptrr.SelectCommand = cmd;
                                DtAdptrr.Fill(DtblXcelItemData);
                                string[] strArray = { "EmployeeId", "UserId", "UserName", "Role", "Type", "Campus", "EmailId", "IsActive", "CreatedDate", "ModifiedDate", "CreatedBy", "ModifiedBy", "UserType", "Password", "PasswordQuestion", "PasswordAnswer" };
                                char chrFlag = 'Y';
                                if (DtblXcelItemData.Columns.Count == strArray.Length)
                                {
                                    int j = 0;
                                    string[] strColumnsAray = new string[DtblXcelItemData.Columns.Count];
                                    foreach (DataColumn dtColumn in DtblXcelItemData.Columns)
                                    {
                                        strColumnsAray[j] = dtColumn.ColumnName;
                                        j++;
                                    }
                                    for (int i = 0; i < strArray.Length - 1; i++)
                                    {
                                        if (strArray[i].Trim() != strColumnsAray[i].Trim())
                                        {
                                            chrFlag = 'N';
                                            break;
                                        }
                                    }
                                    if (chrFlag == 'Y')
                                    {
                                        IList<User> UserList = new List<User>();
                                        PassworAuth pa = new PassworAuth();
                                        foreach (DataRow item in DtblXcelItemData.Rows)
                                        {
                                            if (item["UserId"].ToString().Trim() != "")
                                            {
                                                PersistenceServiceFactory PSF = new PersistenceServiceFactory();
                                                User Userexists = PSF.Get<User>("UserId", item["UserId"].ToString().Trim());
                                                if (Userexists == null)
                                                {
                                                    User user = new User();
                                                    user.EmployeeId = item["EmployeeId"] != null ? item["EmployeeId"].ToString().Trim() : "";
                                                    user.UserId = item["UserId"] != null ? item["UserId"].ToString().Trim() : "";
                                                    user.UserName = item["UserName"] != null ? item["UserName"].ToString().Trim() : "";
                                                    user.Role = item["Role"] != null ? item["Role"].ToString().Trim() : "";
                                                    user.Type = item["Type"] != null ? item["Type"].ToString().Trim() : "";
                                                    user.Campus = item["Campus"] != null ? item["Campus"].ToString().Trim() : "";
                                                    user.EmailId = item["EmailId"] != null ? item["EmailId"].ToString().Trim() : "";
                                                    user.IsActive = Convert.ToBoolean(item["IsActive"]);
                                                    user.CreatedDate = DateTime.Now;
                                                    user.ModifiedDate = DateTime.Now;
                                                    user.CreatedBy = userId;
                                                    user.ModifiedBy = userId;
                                                    user.UserType = item["UserType"] != null ? item["UserType"].ToString().Trim() : "";
                                                    user.Password = pa.base64Encode(item["Password"].ToString());
                                                    user.PasswordQuestion = item["PasswordQuestion"] != null ? item["PasswordQuestion"].ToString().Trim() : "";
                                                    user.PasswordAnswer = item["PasswordAnswer"] != null ? item["PasswordAnswer"].ToString().Trim() : "";
                                                    UserList.Add(user);
                                                }
                                            }
                                        }
                                        if (UserList.Count > 0)
                                        {
                                            us.CreateOrUpdateUserList(UserList);
                                            return Json(new { success = true, result = "Users created successfully.." }, "text/html", JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                    else
                                    {
                                        ErrorFilename.Append(fileName + ",");
                                    }
                                }
                                else
                                {
                                    ErrorFilename.Append(fileName + ",");
                                }
                            }
                            catch (Exception ex)
                            {
                                ErrorFilename.Append(fileName + ",");
                                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
                            }
                        }
                    }
                    else
                    {
                        return Json(new { success = false, result = "You have uploaded the empty file. Please upload the correct file." }, "text/html", JsonRequestBehavior.AllowGet);
                    }

                    if (UploadedFilename != null && !string.IsNullOrEmpty(UploadedFilename.ToString()))
                    {
                        retValue.Append("-------files uploaded successfully-----------");
                        retValue.Append("<br />");
                        string[] upfiles = UploadedFilename.ToString().Split(',');
                        if (upfiles != null && upfiles.Count() > 0)
                        {
                            foreach (string s in upfiles)
                            {
                                if (!string.IsNullOrEmpty(s))
                                {
                                    retValue.Append(s + ";");
                                    retValue.Append("<br />");
                                }
                            }

                            retValue.Append("<br />");
                            retValue.Append("Successfully uploaded files" + Convert.ToInt32(UploadedFilename.ToString().Split(',').Count() - 1));
                            retValue.Append("<br />");
                            //retValue.Append("-----------------------------------------------------");
                        }
                    }
                    if (alreadyExists != null && !string.IsNullOrEmpty(alreadyExists.ToString()))
                    {
                        retValue.Append("-----------files already exists--------------");
                        retValue.Append("<br />");
                        string[] existsfiles = alreadyExists.ToString().Split(',');
                        if (existsfiles != null && existsfiles.Count() > 0)
                        {
                            foreach (string s in existsfiles)
                            { if (!string.IsNullOrEmpty(s)) retValue.Append(s + ";"); retValue.Append("<br />"); }
                            //retValue.Append("-------------------------------------------------");
                        }
                    }
                    if (ErrorFilename != null && !string.IsNullOrEmpty(ErrorFilename.ToString()))
                    {
                        retValue.Append("-----------error occured Files--------------");
                        string[] errfiles = ErrorFilename.ToString().Split(',');
                        if (errfiles != null && errfiles.Count() > 0)
                        {
                            foreach (string s in errfiles)
                            { if (!string.IsNullOrEmpty(s))retValue.Append(s + ";"); retValue.Append("<br />"); }
                            //retValue.Append("-------------------------------------------------");
                        }
                    }
                    return Json(new { success = true, result = retValue.ToString().Replace(Environment.NewLine, "<br />") }, "text/html", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
            }
            //MyLabel.Text = sb.ToString().Replace(Environment.NewLine, "<br />");
            return Json(new { success = true, result = retValue.ToString().Replace(Environment.NewLine, "<br />") }, "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult LogOnFromLMS(string PreRegNum)
        {
            Session["Role"] = "";
            Session["AdminRole"] = "";
            Session["userrole"] = "";
            Session["staffapproverrole"] = "";
            try
            {
                if (!string.IsNullOrEmpty(PreRegNum))
                {
                    StaffManagementService staffServObj = new StaffManagementService();
                    StaffDetails staffObj = staffServObj.GetStaffDeatailsByPreRegNum(Convert.ToInt32(PreRegNum));
                    //var UsrPass = UserandPassword.Split(',');
                    if (!string.IsNullOrWhiteSpace(staffObj.StaffUserName))
                    {
                        TIPS.Service.UserService us = new UserService();
                        TIPS.Entities.User User = us.GetUserByUserId(staffObj.StaffUserName);
                        PassworAuth PA = new PassworAuth();
                        if (User != null)
                        {
                            if (!string.Equals(staffObj.StaffUserName, User.UserId))
                            {
                                ViewBag.User = "Case Sensitive";
                                return View();
                            }
                            if (User.IsActive == false)
                            {
                                ViewBag.User = "Deactivated";
                                return View();
                            }
                            else
                            {
                               
                                    LoginHistory s = GetLoginDetails(User);
                                    us.CreateOrUpdateSession(s);
                                    Session["objUser"] = User;
                                    Session["UserId"] = User.UserId;
                                    Dictionary<string, object> criteriamain = new Dictionary<string, object>();
                                    criteriamain.Add("UserId", staffObj.StaffUserName);
                                    Dictionary<long, IList<UserAppRole>> userappmain = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteriamain);
                                    if (userappmain.FirstOrDefault().Value != null && userappmain.FirstOrDefault().Value.Count != 0)
                                    {
                                        if (userappmain.First().Value[0].AppCode != null)
                                        {
                                            var appCode = (from u in userappmain.First().Value
                                                           select u.AppCode).Distinct().ToArray();
                                            Session["Role"] = appCode;
                                            var adminRole = (from v in appCode where v == "All" select v);
                                            if (!string.IsNullOrEmpty(adminRole.FirstOrDefault()))
                                            {
                                                Session["AdminRole"] = adminRole.First().ToString();
                                            }
                                            var usercampus = (from u in userappmain.First().Value
                                                              where u.BranchCode != null
                                                              select u.BranchCode).Distinct().ToArray();
                                            Session["UserCampus"] = usercampus;
                                        }
                                    }
                                    var userrole = (from r in userappmain.First().Value
                                                    select r.RoleCode).Distinct().ToArray();
                                    Session["userrolesarray"] = userrole;
                                    if (userrole.Contains("ADM-APP"))            //to check if user has student approver access
                                    {
                                        Session["userrole"] = "ADM-APP";
                                    }
                                    if (userrole.Contains("STM-APP"))          //to check if user has staff approver access
                                    {
                                        Session["staffapproverrole"] = "STM-APP";
                                    }
                                    Session["SiteLinks"] = IdHtmlTags();
                                    return RedirectToAction("Home", "Home");
                                //}
                                //else return RedirectToAction("LogOff", "Account");
                            }
                        }
                    }
                    else
                    {
                        ViewBag.User = -1;
                        return View();
                    }
                }
                else
                {
                    string userId = base.ValidateUser();
                    return Redirect(" http://117.239.246.89/LMS/Knowa-V1/index.php/login/icms_auth/" + userId);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
                throw ex;
            }
            return RedirectToAction("LogOff", "Account");
        }

        public JsonResult GetStudentListForUserCreationJqGrid(string Campus, string Grade, string Section, string IsHosteller, string AcademicYear, int rows, string sidx, string sord, int? page = 1)
        {
            string userid = (Session["UserId"] == null) ? "" : Session["UserId"].ToString();
            try
            {
                AdmissionManagementService ams = new AdmissionManagementService();
                Dictionary<string, object> Likecriteria = new Dictionary<string, object>();
                Dictionary<string, object> Eqcriteria = new Dictionary<string, object>();
                if (!string.IsNullOrWhiteSpace(Campus)) { Likecriteria.Add("Campus", Campus); }
                if (!string.IsNullOrWhiteSpace(Section)) { Likecriteria.Add("Section", Section); }
                if (!string.IsNullOrWhiteSpace(AcademicYear)) { Likecriteria.Add("AcademicYear", AcademicYear); }
                if (!string.IsNullOrWhiteSpace(IsHosteller))
                {
                    Likecriteria.Add("IsHosteller", IsHosteller == "Yes" ? true : false);
                }
                Likecriteria.Add("AdmissionStatus", "Registered");
                if (!string.IsNullOrWhiteSpace(Grade)) { Eqcriteria.Add("Grade", Grade); }
                Dictionary<long, IList<StudentTemplateView>> StudentList = ams.GetStudentTemplateViewListWithLikeandExactPagingAndCriteria(0, 9999, sord, sidx, Eqcriteria, Likecriteria);
                if (StudentList != null && StudentList.FirstOrDefault().Value != null && StudentList.FirstOrDefault().Value.Count > 0)
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
                                    i = 2,
                                    cell = new string[] {
                                items.Id.ToString(),
                                items.ApplicationNo,
                                items.PreRegNum.ToString(),
                                items.Name,
                                items.Gender,
                                items.Campus,
                                items.Grade,
                                items.Section,
                                items.IsHosteller.ToString(),
                                items.NewId,
                                items.FeeStructYear,
                                items.AdmissionStatus,
                                items.AcademicYear,
                                items.CreatedDate,
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
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }


        public ActionResult UserCreation()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
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
                    Dictionary<long, IList<GradeMaster>> GradeMaster = ms.GetGradeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, null);
                    Dictionary<long, IList<SectionMaster>> SectionMaster = ms.GetSectionMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, null);
                    Dictionary<long, IList<AcademicyrMaster>> AcademicyrMaster = ms.GetAcademicyrMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, null);

                    ViewBag.campusddl = CampusMaster.First().Value;
                    ViewBag.gradeddl = GradeMaster.First().Value;
                    ViewBag.sectionddl = SectionMaster.First().Value;
                    ViewBag.acadddl = AcademicyrMaster.First().Value;
                    //Modified by Thamizhmani
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
                throw ex;
            }
        }

        public JsonResult UserIdCreation(string Campus, string Grade, string Section, string UserType, string IsHosteller, string AcademicYear)
        {
            try
            {
                AdmissionManagementService ams = new AdmissionManagementService();
                Dictionary<string, object> Likecriteria = new Dictionary<string, object>();
                Dictionary<string, object> Eqcriteria = new Dictionary<string, object>();
                if (!string.IsNullOrWhiteSpace(Campus)) { Likecriteria.Add("Campus", Campus); }
                if (!string.IsNullOrWhiteSpace(Section)) { Likecriteria.Add("Section", Section); }
                if (!string.IsNullOrWhiteSpace(AcademicYear)) { Likecriteria.Add("AcademicYear", AcademicYear); }
                if (!string.IsNullOrWhiteSpace(IsHosteller))
                {
                    Likecriteria.Add("IsHosteller", IsHosteller == "Yes" ? true : false);
                }
                Likecriteria.Add("AdmissionStatus", "Registered");
                if (!string.IsNullOrWhiteSpace(Grade)) { Eqcriteria.Add("Grade", Grade); }
                Dictionary<long, IList<StudentTemplateView>> StudentList = ams.GetStudentTemplateViewListWithLikeandExactPagingAndCriteria(0, 9999, "asc", "Name", Eqcriteria, Likecriteria);
                if (StudentList != null && StudentList.FirstOrDefault().Value != null && StudentList.FirstOrDefault().Value.Count > 0)
                {
                    foreach (var item in StudentList.FirstOrDefault().Value)
                    {
                        UserService us = new UserService();
                        User user = new User();
                        if (UserType == "Parent")
                            user = us.GetUserByUserId("P" + item.NewId);
                        else if (UserType == "Student")
                            user = us.GetUserByUserId(item.NewId);
                        if (user == null)
                        {
                            SaveUser(item, new User(), new UserService(), UserType);
                        }
                        if (UserType == "Student")
                        {
                            Dictionary<string, object> criteriaUserAppRole = new Dictionary<string, object>();
                            criteriaUserAppRole.Add("UserId", item.NewId);
                            criteriaUserAppRole.Add("AppCode", "STUIM");
                            criteriaUserAppRole.Add("RoleCode", "STU");
                            criteriaUserAppRole.Add("BranchCode", item.Campus);
                            Dictionary<long, IList<UserAppRole>> userAppRole = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteriaUserAppRole);
                            if (userAppRole != null && userAppRole.FirstOrDefault().Value != null && userAppRole.FirstOrDefault().Value.Count > 0 && userAppRole.First().Key > 0)
                            {
                            }
                            else
                            {
                                UserAppRole uar = new UserAppRole();
                                uar.UserId = item.NewId;
                                uar.AppCode = "STUIM";
                                uar.RoleCode = "STU";
                                uar.AppName = "Issue Management";
                                uar.RoleName = "Student";
                                uar.BranchCode = item.Campus;
                                uar.Email = item.EmailId;
                                us.CreateOrUpdateUserAppRole(uar);
                            }
                        }
                    }
                    return Json("User Ids created successfully", JsonRequestBehavior.AllowGet);
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
                throw ex;
            }

        }

        public void SaveUser(StudentTemplateView item, User user, UserService us, string UserType)
        {
            PassworAuth PA = new PassworAuth();
            string[] dob = new string[3];
            dob = item.DOB.ToString().Contains('/') ? item.DOB.ToString().Split('/') : item.DOB.ToString().Contains('-') ? item.DOB.ToString().Split('-') : item.DOB.ToString().Split('.');
            if (UserType == "Parent")
                user.UserId = "P" + item.NewId;
            else if (UserType == "Student")
            {
                user.UserId = item.NewId;
                user.UserName = item.Name;
            }
            user.Password = dob[0] + dob[1] + dob[2];
            //encode the password
            user.Password = PA.base64Encode(user.Password);
            user.Campus = item.Campus;
            user.UserType = UserType;
            user.CreatedDate = DateTime.Now;
            user.CreatedBy = (Session["UserId"] != null) ? Session["UserId"].ToString() : "";
            user.ModifiedDate = DateTime.Now;
            user.ModifiedBy = (Session["UserId"] != null) ? Session["UserId"].ToString() : "";
            user.EmailId = item.EmailId;
            user.IsActive = true;
            us.CreateOrUpdateUser(user);
        }

        public ActionResult UserDetails()
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
            Dictionary<long, IList<UserTypeMaster>> UserTypeMaster = ms.GetUserTypeMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, null);
            ViewBag.campusddl = CampusMaster.First().Value;
            if (UserTypeMaster != null && UserTypeMaster.FirstOrDefault().Value != null && UserTypeMaster.FirstOrDefault().Value.Count > 0)
                ViewBag.UserType = UserTypeMaster.FirstOrDefault().Value;
            //Modified by Thamizhmani
            string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
            return View();
        }

        public ActionResult UserDetailsListJqGrid(User user, string IsActive1, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                UserService us = new UserService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(user.UserId))
                {
                    criteria.Add("UserId", user.UserId);
                }
                if (!string.IsNullOrEmpty(user.UserName))
                {
                    criteria.Add("UserName", user.UserName);
                }
                if (!string.IsNullOrEmpty(user.Campus))
                {
                    criteria.Add("Campus", user.Campus);
                }
                if (!string.IsNullOrEmpty(user.UserType))
                {
                    criteria.Add("UserType", user.UserType);
                }
                if (!string.IsNullOrEmpty(user.EmailId))
                {
                    criteria.Add("EmailId", user.EmailId);
                }
                if (!string.IsNullOrWhiteSpace(IsActive1))
                {
                    criteria.Add("IsActive", IsActive1 == "Yes" ? true : false);
                }
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<User>> UserList = us.GetUserListWithLikePagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (UserList != null && UserList.Count > 0)
                {
                    long totalrecords = UserList.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var AssLst = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (
                             from items in UserList.First().Value
                             select new
                             {
                                 cell = new string[] 
                                         {
                                            items.Id.ToString(),
                                            items.UserId,
                                            items.UserName,
                                            items.Campus,
                                            items.UserType,
                                            items.EmailId,
                                            items.IsActive.ToString(),
                                         }
                             }).ToList()
                    };
                    return Json(AssLst, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
                throw ex;
            }
        }

        public ActionResult PageHistory()
        {//Modified by Thamizhmani
            string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
            return View();
        }

        public ActionResult PageHistoryListJQGrid(PageHistory ph, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                UserService us = new UserService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<PageHistory>> PageHistory = us.GetPageHistoryWithLikePagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (PageHistory != null && PageHistory.Count > 0)
                {
                    long totalrecords = PageHistory.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var AssLst = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (
                             from items in PageHistory.First().Value
                             select new
                             {
                                 cell = new string[] 
                                         {
                                            items.Id.ToString(),
                                            items.UserId,
                                            items.SessionId,
                                            items.VisitedTime.ToString("dd/MM/yyyy hh:mm:ss tt"),
                                            items.Action,
                                            items.Controller,
                                            items.ExecutionTime.ToString()
                                         }
                             }).ToList()
                    };
                    return Json(AssLst, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
                throw ex;
            }
        }

        public ActionResult ActionHistoryListJQGrid(PageHitCount_Vw ph, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {//Modified By Thamizhmani
                UserService us = new UserService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<PageHitCount_Vw>> PageHistory = us.GetPageHitCountWithLikePagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (PageHistory != null && PageHistory.Count > 0)
                {
                    long totalrecords = PageHistory.First().Key;
                    int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var AssLst = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalrecords,
                        rows = (
                             from items in PageHistory.First().Value
                             select new
                             {
                                 cell = new string[] 
                                         {
                                           items.Id.ToString(),
                                           items.Action,
                                           items.Controller,
                                           items.HitCount.ToString()
                                         }
                             })
                    };
                    return Json(AssLst, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
                throw ex;
            }
        }

        #region Ace Template Login and Menu
        /// <summary>
        /// Modified For New Ace Template
        /// </summary>
        /// <returns></returns>
        public ActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(User user)
        {
            Session["Role"] = "";
            Session["AdminRole"] = "";
            Session["userrole"] = "";
            Session["staffapproverrole"] = "";
            try
            {
                if (!string.IsNullOrWhiteSpace(user.UserId) && !string.IsNullOrWhiteSpace(user.Password))
                {
                    TIPS.Service.UserService us = new UserService();
                    TIPS.Entities.User User = us.GetUserByUserId(user.UserId);
                    PassworAuth PA = new PassworAuth();
                    if (User != null)
                    {
                        if (!string.Equals(user.UserId, User.UserId))
                        {
                            ModelState.AddModelError("e", "Case Sensitive");
                            return View();
                        }
                        if (User.IsActive == false)
                        {
                            ModelState.AddModelError("e", "Deactivated");
                            return View();
                        }
                        else
                        {
                            if (Request["Password"] == PA.base64Decode2(User.Password))
                            {
                                LoginHistory s = GetLoginDetails(User);
                                us.CreateOrUpdateSession(s);
                                Session["objUser"] = User;
                                Session["UserId"] = User.UserId;
                                Session["UserName"] = User.UserName;
                                Dictionary<string, object> criteriamain = new Dictionary<string, object>();
                                criteriamain.Add("UserId", user.UserId);
                                Dictionary<long, IList<UserAppRole>> userappmain = us.GetAppRoleForAnUserListWithPagingAndCriteria(0, 10000, string.Empty, string.Empty, criteriamain);
                                if (userappmain.FirstOrDefault().Value != null && userappmain.FirstOrDefault().Value.Count != 0)
                                {
                                    if (userappmain.First().Value[0].AppCode != null)
                                    {
                                        var appCode = (from u in userappmain.First().Value
                                                       select u.AppCode).Distinct().ToArray();
                                        Session["Role"] = appCode;
                                        var adminRole = (from v in appCode where v == "All" select v);
                                        if (!string.IsNullOrEmpty(adminRole.FirstOrDefault()))
                                        {
                                            Session["AdminRole"] = adminRole.First().ToString();
                                        }
                                        var usercampus = (from u in userappmain.First().Value
                                                          where u.BranchCode != null
                                                          select u.BranchCode).Distinct().ToArray();
                                        Session["UserCampus"] = usercampus;
                                    }
                                }
                                var userrole = (from r in userappmain.First().Value
                                                select r.RoleCode).Distinct().ToArray();
                                Session["userrolesarray"] = userrole;
                                if (userrole.Contains("ADM-APP"))            //to check if user has student approver access
                                {
                                    Session["userrole"] = "ADM-APP";
                                }
                                if (userrole.Contains("STM-APP"))          //to check if user has staff approver access
                                {
                                    Session["staffapproverrole"] = "STM-APP";
                                }
                                if (userrole.Contains("TRNS-APP"))          //to check if user has transport approver access
                                {
                                    Session["transapproverrole"] = "TRNS-APP";
                                }
                                //Session["SiteLinks"] = IdHtmlTags();
                                Session["AceSiteLinks"] = AceIdHtmlTags();//For Ace Menu
                                return RedirectToAction("NewHome", "Home");
                            }
                            else return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("e", "The Email or Password you entered is incorrect.");
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
                throw ex;
            }
            return View();
        }

        public ActionResult Register()
        {
            User u = new User();
            u.UserType = "";
            return View(u);
        }
        [HttpPost]
        public ActionResult Register(TIPS.Entities.User user)
        {
            try
            {
                //check newpassword and confirm password matches or not
                if (user.Password == user.ConfirmPassword)
                {
                    user.CreatedDate = DateTime.Now;
                    user.ModifiedDate = DateTime.Now;
                    TIPS.Service.UserService us = new UserService();
                    PassworAuth PA = new PassworAuth();
                    //encode and save the password
                    user.Password = PA.base64Encode(user.Password);
                    user.IsActive = true;
                    us.CreateOrUpdateUser(user);
                    TempData["SuccessUserCreation"] = 1;
                    return View();
                    //return RedirectToAction("LogIn", "Account");
                }
                else throw new Exception("Password and ConfirmPassword doesn't match.");
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
                ViewBag.Error = ex.Message;
                return View();
            }
        }


        //Ace Template Menu
        public string AceIdHtmlTags()
        {
            try
            {
                if (Session["Role"].ToString() != "")
                {
                    System.Text.StringBuilder html = new System.Text.StringBuilder();
                    MenuService ms = new MenuService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    var rle = Session["Role"] as IEnumerable<string>;
                    if (rle.Contains("All"))
                        return AceAdminRoleMenuBuilding(ms, html, criteria);
                    else
                        return AceOtherRolesMenuBuilding(ms, html, criteria, rle);
                }
                else { return string.Empty; }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                throw ex;
            }
        }

        private string AceAdminRoleMenuBuilding(MenuService ms, System.Text.StringBuilder html, Dictionary<string, object> criteria)
        {
            Dictionary<long, IList<TIPS.Entities.MenuEntities.Menu>> submenuitems;
            Dictionary<long, IList<TIPS.Entities.MenuEntities.Menu>> thirdlevelmenuitems;
            html.Clear();
            string sord = "";
            sord = sord == "desc" ? "Desc" : "Asc";
            string sidx = "OrderNo";
            criteria.Add("ParentORChild", true);
            criteria.Add("Website", "StaffPortal");
            Dictionary<long, IList<TIPS.Entities.MenuEntities.Menu>> mainmenu = ms.GetMenuListWithPagingAndCriteria(0, 99999, sidx, sord, criteria);
            html.Append("<ul class='nav nav-list' style='top: 0px; border-style: solid; border-left-width: 1px; border-right-width: 1px;'>");
            html.Append("<li class='active hover'><a href='/Home/NewHome'><span class='menu-text'>Home</span></a></li>");
            foreach (var items in mainmenu.First().Value)
            {
                html.Append("<li class='hover'><a href='#' class='dropdown-toggle'><span class='menu-text'>" + items.MenuName + "</span><b class='arrow fa fa-angle-down'></b></a><b class='arrow'></b><ul class='submenu'>");
                criteria.Clear();
                criteria.Add("ParentRefId", Convert.ToInt32(items.Id));
                criteria.Add("MenuLevel", "Level2");
                criteria.Add("Website", "StaffPortal");
                submenuitems = ms.GetMenuListWithPagingAndCriteria(0, 9999, sidx, sord, criteria);
                foreach (var var in submenuitems.First().Value)
                {
                    if (var.Controller == null || var.Controller == "")
                    {
                        html.Append("<li class='hover'><a href='#' class='dropdown-toggle'><span class='menu-text'>" + var.MenuName + "</span><b class='arrow fa fa-angle-down'></b></a><b class='arrow'></b>");
                    }
                    else
                    {
                        html.Append("<li class='hover'><a href='/" + var.Controller + "/" + var.Action + "'><i class='menu-icon fa fa-caret-right'></i>" + var.MenuName + "</a><b class='arrow'></b>");
                    }

                    criteria.Clear();
                    criteria.Add("ParentRefId", Convert.ToInt32(var.Id));
                    criteria.Add("MenuLevel", "Level3");
                    criteria.Add("Website", "StaffPortal");
                    thirdlevelmenuitems = ms.GetMenuListWithPagingAndCriteria(0, 99999, sidx, sord, criteria);
                    if (thirdlevelmenuitems.First().Value.Count > 0)             // if there is a third level menu
                    {
                        int j = 0;
                        foreach (var tid in thirdlevelmenuitems.First().Value)
                        {
                            if (j == 0)   // To add ul tag only for first time
                            {
                                html.Append("<ul class='submenu'>");
                            }
                            html.Append("<li class='hover'><a href='/" + tid.Controller + "/" + tid.Action + "'><i class='menu-icon fa fa-caret-right'></i>" + tid.MenuName + "</a><b class='arrow'></b></li>");
                            j++;
                        }
                        if (j != 0)   //  if ul open tag is added then to add the close tag
                        {
                            html.Append("</ul>");
                        }
                    }
                    html.Append("</li>");
                }
                html.Append("</ul></li>");
                criteria.Clear();
            }
            html.Append("</ul>");
            return html.ToString();
        }

        private string AceOtherRolesMenuBuilding(MenuService ms, System.Text.StringBuilder html, Dictionary<string, object> criteria, IEnumerable<string> rle)
        {
            long[] parentrefid = new long[rle.Count()];
            int i = 0;
            criteria.Clear();
            criteria.Add("Role", rle.ToArray());
            criteria.Add("ParentORChild", false);
            criteria.Add("Website", "StaffPortal");
            Dictionary<long, IList<Menu>> mainmenuparentid = ms.GetMenuListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
            if (mainmenuparentid != null && mainmenuparentid.FirstOrDefault().Value != null && mainmenuparentid.FirstOrDefault().Value.Count > 0)
            {
                foreach (Menu m in mainmenuparentid.First().Value)
                {
                    //if (!parentrefid.Contains(m.ParentRefId))
                    if (!parentrefid.Contains(m.ParentRefId) && m.MenuLevel == "Level2")
                    {
                        parentrefid[i] = m.ParentRefId;
                        i++;
                    }
                }
            }
            html.Append("<ul class='nav nav-list' style='top: 0px; border-style: solid; border-left-width: 1px; border-right-width: 1px;'>");
            html.Append("<li class='active hover'><a href='/Home/NewHome'><span class='menu-text'>Home</span></a></li>");
            //get all the menu items inside the for each loop need to be moved here
            //some other time this need to be concentrated to get all the menu table list, after that the same need to be used for the menu building
            //this is to avoid repeated db read on the same table
            IList<Menu> menuList = ms.GetMenusById(parentrefid.Distinct().ToArray());
            if (menuList != null)
            {
                foreach (long id in parentrefid.Distinct().ToArray())  // to remove repeated parent id's for two or more submenu items
                {
                    if (id != 0)
                    {
                        criteria.Clear();
                        criteria.Add("Id", id);
                        Menu mainmenubyid = menuList.First(s => s.Id == id);
                        if (mainmenubyid.MenuLevel == "Level1")//to take level1 menu--- comments updated by jp,anbu
                            html.Append("<li class='hover'><a href='#' class='dropdown-toggle'><span class='menu-text'>" + mainmenubyid.MenuName + "</span><b class='arrow fa fa-angle-down'></b></a><b class='arrow'></b><ul class='submenu'>");

                        criteria.Clear();
                        criteria.Add("ParentRefId", Convert.ToInt32(id));
                        criteria.Add("MenuLevel", "Level2");
                        criteria.Add("Website", "StaffPortal");
                        Dictionary<long, IList<Menu>> submenubyparentid = ms.GetMenuListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                        foreach (var pid in submenubyparentid.First().Value)
                        {
                            if (rle.Contains(pid.Role))
                            {
                                if (pid.Controller == null || pid.Controller == "")
                                {
                                    html.Append("<li class='hover'><a href='#' class='dropdown-toggle'><span class='menu-text'>" + pid.MenuName + "</span><b class='arrow fa fa-angle-down'></b></a><b class='arrow'></b>");
                                }
                                else
                                {
                                    html.Append("<li class='hover'><a href='/" + pid.Controller + "/" + pid.Action + "'><i class='menu-icon fa fa-caret-right'></i>" + pid.MenuName + "</a><b class='arrow'></b>");
                                }
                                criteria.Clear();
                                criteria.Add("ParentRefId", Convert.ToInt32(pid.Id));
                                criteria.Add("MenuLevel", "Level3");
                                criteria.Add("Website", "StaffPortal");
                                Dictionary<long, IList<TIPS.Entities.MenuEntities.Menu>> thirdlevelmenu = ms.GetMenuListWithPagingAndCriteria(0, 99999, string.Empty, string.Empty, criteria);
                                if (thirdlevelmenu.First().Value.Count > 0)             // if there is a third level menu
                                {
                                    int j = 0;
                                    foreach (var tid in thirdlevelmenu.First().Value)
                                    {
                                        if (rle.Contains(tid.Role))
                                        {
                                            if (j == 0)   // To add ul tag only for first time
                                            {
                                                html.Append("<ul class='submenu'>");
                                            }
                                            html.Append("<li class='hover'><a href='/" + tid.Controller + "/" + tid.Action + "'><i class='menu-icon fa fa-caret-right'></i>" + tid.MenuName + "</a><b class='arrow'></b></li>");
                                            j++;
                                        }
                                    }
                                    if (j != 0)   //  if ul open tag is added then to add the close tag
                                    {
                                        html.Append("</ul>");
                                    }
                                }
                                html.Append("</li>");
                            }
                        }
                        html.Append("</ul></li>");
                    }
                }
                html.Append("</ul>");
                return html.ToString();
            }
            else return string.Empty;
        }
        #endregion

        public ActionResult Error()
        { return View(); }

        public ActionResult DeactivateUser(User Us)
        {
            UserService us = new UserService();
            User User = us.GetUserById(Us.Id);
            if (User.UserType == "Parent" || User.UserType == "Student" || User.UserType == "CMSUsers" || User.UserType == "Others")
            {
                User.IsActive = Us.IsActive;
                User.Campus = Us.Campus;
                User.EmailId = Us.EmailId;
                us.CreateOrUpdateUser(User);
            }
            else if (User.UserType == "Staff")
            {
                if (User.EmployeeId != null)
                {
                    StaffManagementService sms = new StaffManagementService();
                    StaffDetails Staff = sms.GetStaffDetailsByIdNumber(User.EmployeeId);
                    if (Staff != null && User.EmployeeId == Staff.IdNumber)
                    {
                        Staff.Status = "Inactive";
                        sms.CreateOrUpdateStaffDetails(Staff);
                        User.IsActive = Us.IsActive;
                        User.Campus = Us.Campus;
                        us.CreateOrUpdateUser(User);
                    }
                }
            }
            return RedirectToAction("UserDetails", "Account");
        }        

        //public void SendMailToNewUser(string Name,string UserId,string password,string Email,string campus)
        //{
        //    try
        //    {
        //        string SendEmail = ConfigurationManager.AppSettings["SendEmail1"];
        //        string From = ConfigurationManager.AppSettings["From"];
        //        if (SendEmail == "false")
        //            return;
        //        else
        //        {
        //            try
        //            {
        //                UserService us = new UserService();
        //                StoreService ss = new StoreService();

        //                string body = "Dear" + Name + ","+"<br/><br/>";
        //                body = body + "Welcome to ICMS...! <br/><br/>";
        //                body = body + "<table style='border-collapse:collapse; border:1px solid black; padding: 5px;'> ";

        //                body = body + "<tr> <td style='font-weight:bold; border:1px solid black; padding: 5px;'><b> Material Name </b></td> <td style='font-weight:bold; border:1px solid black; padding: 5px;'> <b>Requested Qty</b> </td> <td style='font-weight:bold; border:1px solid black; padding: 5px;'> <b>Approved Qty</b> </td>  <tr> ";

        //                body = body + "</table><br/><br/>";

        //                body = body + "<b>Approver Comments:</b><br/><br/>";

        //                body = body + "UserId:"+UserId;

        //               IList<CampusEmailId> campusemaildet = GetEmailIdByCampus(campus, ConfigurationManager.AppSettings["CampusEmailType"].ToString());
        //                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
        //                mail.To.Add(Email);
        //                mail.IsBodyHtml = true;
        //                SmtpClient smtp = new SmtpClient("localhost", 25);
        //                smtp.Host = "smtp.gmail.com";
        //                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //                smtp.EnableSsl = true;
        //                mail.Subject = "Welcome To ICMS ";

        //                mail.Body = body;
        //                if (From == "live")
        //                {
        //                    try
        //                    {
        //                        mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
        //                        smtp.Credentials = new System.Net.NetworkCredential
        //                      (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
        //                        if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
        //                        {
        //                            smtp.Send(mail);
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        if (ex.Message.Contains("quota"))
        //                        {
        //                            mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
        //                            smtp.Credentials = new System.Net.NetworkCredential
        //                            (campusemaildet.First().AlternateEmailId.ToString(), campusemaildet.First().AlternateEmailIdPassword.ToString());
        //                            if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
        //                            {
        //                                smtp.Send(mail);
        //                            }
        //                        }
        //                    }
        //                }
        //                else if (From == "test")
        //                {
        //                    mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
        //                    smtp.Credentials = new System.Net.NetworkCredential
        //                   (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
        //                    if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
        //                    {
        //                        smtp.Send(mail);
        //                    }
        //                }
        //            }
        //            catch (Exception)
        //            {

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "StoreMgmntPolicy");
        //        throw ex;
        //    }
        //}

        public void SendMailToNewUser(string Name, string UserId, string password, string Email, string campus)
        {
            try
            {
                string SendEmail = ConfigurationManager.AppSettings["SendEmail1"];
                if (SendEmail == "false")
                    return;
                else
                {
                    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                    string CCMail = ConfigurationManager.AppSettings["NewStaffCCMail"].ToString();                    
                    string body = "<b>Dear " + Name + "<b><br/><br/>";
                    body = body + "<b>Welcome to ICMS...! </b><br/><br/>";
                    body = body + "<b>URL :</b> <a href='http://myaccess.tipsglobal.net/'>myaccess.tipsglobal.net</a><br/><br/>";
                    body = body + "<table style='border-collapse:collapse; border:1px solid black; padding: 5px;'> ";
                    body = body + "<tr> <td style='font-weight:bold; border:1px solid black; padding: 5px;'><b> User Name </b></td> <td style='border:1px solid black; padding: 5px;'>" + UserId + "  </td> <tr> ";
                    body = body + "<tr> <td style='font-weight:bold; border:1px solid black; padding: 5px;'><b>Password</b></td> <td style='border:1px solid black; padding: 5px;'> " + password + " </td><tr> ";
                    body = body + "</table><br/><br/>Use the above Username and Password in Staff Portal.<br/> <br/>";
                    body = body + "<b>Note:</b><br/>You are Requested to Update your Profile in Staff Portal i.e., EMail,D.O.B,Experience Details,Photo..Etc<br/> <br/>";
                    mail.To.Add(Email);
                    mail.CC.Add(CCMail);
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    mail.Subject = "ICMS-Login credentials ";
                    EmailHelper em = new EmailHelper();
                    em.SendEmailWithEmailTemplate(mail, campus, GetGeneralBodyofMail());
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CustomAccountPolicy");
                throw ex;
            }
        }
        //public string GetIP()
        //{
        //    string Str = "";
        //    Str = System.Net.Dns.GetHostName();
        //    IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(Str);
        //    IPAddress[] addr = ipEntry.AddressList;
        //    return addr[addr.Length - 1].ToString();
        //}
    }
}
// First Review Completed (by JP and Anbu on 15 Mar 2014 - 21 mar 2014)
