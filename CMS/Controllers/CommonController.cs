using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TIPS.Entities;
using TIPS.Service;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TIPS.Entities.MenuEntities;
using TIPS.Component;
using TIPS.Entities.AdmissionEntities;
using TIPS.ServiceContract;
using TIPS.Entities.StaffManagementEntities;
using System.Configuration;
using TIPS.Entities.ReportEntities;
using System.Text;
namespace CMS.Controllers
{
    public class CommonController : BaseController
    {
        //
        // GET: /Common/

        string PolicyName = "SystemMgntPolicy";
        AdmissionManagementService ads = new AdmissionManagementService();
        MastersService ms = new MastersService();
        #region Added By Thamizhmani
        #region RoleMasters
        public ActionResult RoleMaster()
        {
            try
            {
                string userId = ValidateUser();
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
        public JsonResult RoleMasterjqgrid(string RoleCode, string RoleName, string Description, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(RoleCode))
                {
                    criteria.Add("RoleCode", RoleCode);
                }
                if (!string.IsNullOrEmpty(RoleName))
                {
                    criteria.Add("RoleName", RoleName);
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    criteria.Add("Description", Description);
                }
                Dictionary<long, IList<Role>> RoleList = ms.GetRoleMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (RoleList != null && RoleList.Count > 0)
                {
                    long totalrecords = RoleList.First().Key;
                    int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalpages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in RoleList.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                       items.Id.ToString(),
                                       items.RoleCode,
                                       items.RoleName,
                                       items.Description,
                                       items.CreatedBy,
                                       items.CreatedDate.ToString(),
                                       items.ModifiedBy,
                                       items.ModifiedDate.ToString()
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
                ExceptionPolicy.HandleException(ex, PolicyName);
                throw ex;
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddRoleMaster(Role rl, string test)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (test == "edit")
                    {
                        MastersService MS = new MastersService();
                        Role UpdatedRole = MS.GetRoleMasterDetailsById(rl.Id);
                        ViewBag.flag = 1;
                        UpdatedRole.RoleCode = rl.RoleCode;
                        UpdatedRole.RoleName = rl.RoleName;
                        UpdatedRole.Description = rl.Description;
                        UpdatedRole.ModifiedBy = Session["UserId"].ToString();
                        UpdatedRole.ModifiedDate = DateTime.Now;
                        MS.CreateOrUpdateRoleTypeMaster(UpdatedRole);
                    }
                    else
                    {
                        if (rl.RoleCode == null)
                        {
                            rl.RoleCode = "*";
                        }
                        rl.CreatedBy = Session["UserId"].ToString();
                        rl.CreatedDate = DateTime.Now;
                        rl.ModifiedBy = Session["UserId"].ToString();
                        rl.ModifiedDate = DateTime.Now;

                        rl.Id = 0;

                        MastersService bs = new MastersService();
                        ViewBag.flag = 1;
                        long id = bs.CreateOrUpdateRoleTypeMaster(rl);
                        rl.Id = id;

                        bs.CreateOrUpdateRoleTypeMaster(rl);
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, PolicyName);
                throw ex;
            }
        }
        #endregion
        #region Application Master
        public ActionResult ApplicationMaster()
        {
            try
            {
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    #region BreadCrumb
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    #endregion
                    return View();
                }
            }
            catch (Exception ex)
            { ExceptionPolicy.HandleException(ex, PolicyName);
            throw ex;
            }
        }
        public JsonResult ApplicationMasterjqGrid(string AppCode, string AppName, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(AppCode))
                {
                    criteria.Add("AppCode", AppCode);
                }
                if (!string.IsNullOrEmpty(AppName))
                {
                    criteria.Add("AppName", AppName);
                }
                sord = sord == "desc" ? "Desc" : "Asc";
                Dictionary<long, IList<Application>> AppList = ms.GetApplicationMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (AppList != null && AppList.Count > 0)
                {
                    long totalrecords = AppList.First().Key;
                    int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalpages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in AppList.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                        items.Id.ToString(),
                                        items.AppCode,
                                        items.AppName,
                                        items.CreatedBy,
                                        items.CreatedDate.ToString(),
                                        items.ModifiedBy,
                                        items.ModifiedDate.ToString()
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
                ExceptionPolicy.HandleException(ex, PolicyName);
                throw ex;
            }
        }
        public ActionResult AddApplicationMaster(Application AppMstr)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (AppMstr.Id > 0)
                    {
                        AppMstr.ModifiedBy = Session["userId"].ToString();
                        AppMstr.ModifiedDate = DateTime.Now;
                        ms.CreateOrUpdateApplicationMaster(AppMstr);
                    }
                    else
                    {
                        AppMstr.CreatedBy = Session["userId"].ToString();
                        AppMstr.CreatedDate = DateTime.Now;
                        AppMstr.ModifiedBy = Session["userId"].ToString();
                        AppMstr.ModifiedDate = DateTime.Now;
                        ms.CreateOrUpdateApplicationMaster(AppMstr);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        #endregion
        #region Academic Year Master
        public ActionResult AcademicMaster()
        {
            try
            {
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    return View();
                }
            }
            catch (Exception ex) {
                ExceptionPolicy.HandleException(ex, PolicyName);
                throw ex;
            }
        }
        public JsonResult AcademicMasterjqgrid(string FormCode, string AcademicYear, string AcademicYeardesc, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(FormCode))
                {
                    criteria.Add("FormCode", FormCode);
                }
                if (!string.IsNullOrEmpty(AcademicYear))
                {
                    criteria.Add("AcademicYear", AcademicYear);
                }
                if (!string.IsNullOrEmpty(AcademicYeardesc))
                {
                    criteria.Add("ACADDESC", AcademicYeardesc);
                }
                Dictionary<long, IList<AcademicyrMaster>> AcademicMstrList = ms.GetAcademicyrMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
                if (AcademicMstrList != null && AcademicMstrList.Count > 0)
                {
                    long totalrecords = AcademicMstrList.First().Key;
                    int totalpages = (int)Math.Ceiling(totalrecords / (float)rows);
                    var jsondat = new
                    {
                        total = totalpages,
                        page = page,
                        records = totalrecords,
                        rows = (from items in AcademicMstrList.First().Value
                                select new
                                {
                                    i = 2,
                                    cell = new string[]{
                                        items.FormId.ToString(),
                                        items.FormCode,
                                        items.AcademicYear,
                                        items.AcademicYeardesc,
                                        items.IsActive==true?"Yes":"No",
                                        items.CreatedBy,
                                        items.CreatedDate.ToString(),
                                        items.UpdateBy,
                                        items.UpdateDate.ToString()
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
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult AddAcademicMaster(AcademicyrMaster AcadMstr, string test)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    if (test == "edit")
                    {
                        ViewBag.flag = 1;
                        AcadMstr.UpdateBy = Session["userId"].ToString();
                        AcadMstr.UpdateDate = DateTime.Now;//.ToString("dd/mm/yyyy");

                        ms.CreateOrUpdateAcademicYearMaster(AcadMstr);
                    }
                    else
                    {
                        AcadMstr.FormId = 0;
                        AcadMstr.FormCode = "nill";

                        long Id = ms.CreateOrUpdateAcademicYearMaster(AcadMstr);

                        AcadMstr.FormId = Id;
                        AcadMstr.FormCode = "ACAD-" + Id.ToString();

                        AcadMstr.CreatedBy = Session["userId"].ToString();
                        AcadMstr.CreatedDate = DateTime.Now;//.ToString("dd/mm/yyyy");
                        AcadMstr.UpdateBy = Session["userId"].ToString();
                        AcadMstr.UpdateDate = DateTime.Now;//.ToString("dd/mm/yyyy");
                        ms.CreateOrUpdateAcademicYearMaster(AcadMstr);
                    }

                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region UserAppRole
        public ActionResult UserAppRole()
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    UserService us = new UserService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    Dictionary<long, IList<Application>> appcode = us.GetApplicationListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    Dictionary<long, IList<Role>> rolecode = us.GetRoleListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    Dictionary<long, IList<Department>> deptcode = us.GetDepartmentListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, criteria);
                    var usrcmp = Session["UserCampus"] as IEnumerable<string>;
                    if (usrcmp.First() != null)            // to check if the usrcmp obj is null or with data
                    {
                        criteria.Add("Name", usrcmp);
                    }
                    MastersService ms = new MastersService();
                    criteria.Clear();

                    ViewBag.appcodeddl = appcode.First().Value;
                    ViewBag.rolecodeddl = rolecode.First().Value;
                    ViewBag.deptcodeddl = deptcode.First().Value;

                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);

                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        public JsonResult UserAppRolejqgrid(UserAppRole role, string id, string txtSearch, string userid, string appcd, string rlcd, string depcd, string brncd, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {

                UserService us = new UserService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();

                if (!string.IsNullOrWhiteSpace(role.AppCode))
                {
                    criteria.Add("AppCode", role.AppCode);
                }
                if (!string.IsNullOrWhiteSpace(role.RoleCode))
                {
                    criteria.Add("RoleCode", role.RoleCode);
                }
                if (!string.IsNullOrWhiteSpace(role.DeptCode))
                {
                    criteria.Add("DeptCode", role.DeptCode);
                }
                if (!string.IsNullOrWhiteSpace(role.BranchCode))
                {
                    criteria.Add("BranchCode", role.BranchCode);
                }
                if (!string.IsNullOrWhiteSpace(role.Email))
                {
                    criteria.Add("Email", role.Email);
                }
                if (!string.IsNullOrWhiteSpace(userid))
                {
                    criteria.Add("UserId", userid);
                }
                if ((!string.IsNullOrWhiteSpace(appcd)))
                {
                    if (appcd.Contains("Select"))
                    {

                    }
                    else
                    {
                        criteria.Add("AppCode", appcd);
                    }
                }
                if (!string.IsNullOrWhiteSpace(rlcd))
                {
                    if (rlcd.Contains("Select"))
                    {

                    }
                    else
                    {
                        criteria.Add("RoleCode", rlcd);
                    }
                }
                if (!string.IsNullOrWhiteSpace(depcd))
                {
                    if (depcd.Contains("Select"))
                    {

                    }
                    else
                    {
                        criteria.Add("DeptCode", depcd);
                    }
                }
                if (!string.IsNullOrWhiteSpace(brncd))
                {
                    if (brncd.Contains("Select"))
                    {

                    }
                    else
                    {
                        criteria.Add("BranchCode", brncd);
                    }
                }
                if (!string.IsNullOrWhiteSpace(sord) && sord == "desc")
                    sord = "Desc";
                else
                    sord = "Asc";
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
                                    i = 2,
                                    cell = new string[] {
                               items.UserId,
                               items.AppCode,
                               items.RoleCode,
                               items.DeptCode,
                               items.BranchCode,
                               items.Email,
                               items.Id.ToString() 
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
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddUserAppRole(UserAppRole apm, string test)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    UserService aps = new UserService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    criteria.Add("UserId", apm.UserId);
                    criteria.Add("AppCode", apm.AppCode);
                    criteria.Add("RoleCode", apm.RoleCode);
                    criteria.Add("DeptCode", apm.DeptCode);
                    criteria.Add("BranchCode", apm.BranchCode);
                    Dictionary<long, IList<UserAppRole>> userapprole = aps.GetAppRoleForAnUserListWithPagingAndCriteria(null, null, null, null, criteria);
                    if (test == "edit")
                    {
                        if (userapprole != null && userapprole.First().Value != null && userapprole.First().Value.Count > 1)
                        {
                            return null;
                        }
                        else
                        {
                            if (apm.RoleCode == "CSE")
                            {
                                apm.DeptCode = null;
                            }

                            ViewBag.flag = 1;
                            aps.CreateOrUpdateUserAppRole(apm);
                            return null;
                        }
                    }
                    else
                    {
                        if (userapprole != null && userapprole.First().Value != null && userapprole.First().Value.Count > 0)
                        {
                            var script1 = @"ErrMsg(""This Combination already exists"");";
                            return JavaScript(script1);
                        }
                        else
                        {
                            if (apm.RoleCode == "CSE")
                            {
                                apm.DeptCode = null;
                            }
                            aps.CreateOrUpdateUserAppRole(apm);
                            var script = @"SucessMsg(""Role mapped Sucessfully"");";
                            return JavaScript(script);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
                throw ex;
            }
        }

        //#region Dropdowns for Appcode,Rolecode, Branchcode and Deptcode
        //public ActionResult AppCodeddl()
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            UserService aps = new UserService();
        //            Dictionary<long, string> appcd = new Dictionary<long, string>();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            Dictionary<long, IList<Application>> application = aps.GetApplicationListWithPagingAndCriteria(0, 9999, null, null, criteria);
        //            foreach (Application app in application.First().Value)
        //            {
        //                appcd.Add(app.Id, app.AppCode);
        //            }
        //            return PartialView("Select", appcd);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
        //        throw ex;
        //    }
        //}

        //public ActionResult RoleCodeddl()
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            UserService rcs = new UserService();
        //            Dictionary<long, string> rlcd = new Dictionary<long, string>();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            Dictionary<long, IList<Role>> role = rcs.GetRoleListWithPagingAndCriteria(0, 9999, null, null, criteria);
        //            foreach (Role rol in role.First().Value)
        //            {
        //                rlcd.Add(rol.Id, rol.RoleCode);
        //            }
        //            return PartialView("Select", rlcd);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
        //        throw ex;
        //    }
        //}

        //public ActionResult DeptCodeddl()
        //{
        //    try
        //    {
        //        string userId = base.ValidateUser();
        //        if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
        //        else
        //        {
        //            UserService dcs = new UserService();
        //            Dictionary<long, string> dptcd = new Dictionary<long, string>();
        //            Dictionary<string, object> criteria = new Dictionary<string, object>();
        //            Dictionary<long, IList<Department>> department = dcs.GetDepartmentListWithPagingAndCriteria(0, 9999, null, null, criteria);
        //            foreach (Department dept in department.First().Value)
        //            {
        //                dptcd.Add(dept.Id, dept.DeptCode);
        //            }
        //            return PartialView("Select", dptcd);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
        //        throw ex;
        //    }
        //}

        //public ActionResult BranchCodeddl()
        //{
        //    try
        //    {

        //        Dictionary<long, string> brncd = new Dictionary<long, string>();
        //        Dictionary<string, object> criteria = new Dictionary<string, object>();

        //        Dictionary<long, IList<CampusMaster>> branch = ms.GetCampusMasterListWithPagingAndCriteria(0, 9999, null, null, criteria);
        //        criteria.Clear();
        //        foreach (CampusMaster brnch in branch.First().Value)
        //        {
        //            brncd.Add(brnch.FormId, brnch.Name);
        //        }
        //        return PartialView("Select", brncd);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, "CallMgmntPolicy");
        //        throw ex;
        //    }
        //}
        //#endregion
        #endregion
        #region MenuMaster Added By Thamizhmani
        public ActionResult Menu()
        {
            try
            {
                string userId = ValidateUser();
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
                ExceptionPolicy.HandleException(ex, "SystemMgntPolicy");
                throw ex;
            }
        }
        public JsonResult Menujqgrid(string RoleCode, string RoleName, string Description, string ParentId, int rows, string sidx, string sord, int? page = 1)
        {
            MastersService ms = new MastersService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(RoleCode))
            {
                criteria.Add("RoleCode", RoleCode);
            }
            if (!string.IsNullOrEmpty(RoleName))
            {
                criteria.Add("RoleName", RoleName);
            }
            if (!string.IsNullOrEmpty(Description))
            {
                criteria.Add("Description", Description);
            }
            Dictionary<long, IList<Menu>> menulist = new Dictionary<long, IList<Menu>>();
            if (string.IsNullOrEmpty(ParentId))
            {
                criteria.Add("ParentORChild", true);
                menulist = ms.GetMenuListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);
            }
            else
            {
                int parentid = Convert.ToInt32(ParentId);
                criteria.Add("ParentRefId", parentid);
                menulist = ms.GetMenuListWithPagingAndCriteria(page - 1, rows, string.Empty, sord, criteria);
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
                    Dictionary<long, IList<Menu>> DeleteMenuList = ms.GetMenuListWithPagingAndCriteria(0, 100, string.Empty, string.Empty, criteria);
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
        #endregion
        #endregion

        #region "Added by Micheal"

        [HttpPost]
        public ActionResult UploadDocuments(HttpPostedFileBase uploadedFile, string docType, string documentFor, long RegNo)
        {
            HttpPostedFileBase theFile = HttpContext.Request.Files["uploadedFile"];
            if (theFile.ContentLength != 0)
            {
                string path = uploadedFile.InputStream.ToString();
                byte[] imageSize = new byte[uploadedFile.ContentLength];

                uploadedFile.InputStream.Read(imageSize, 0, (int)uploadedFile.ContentLength);
                UploadedFiles fu = new UploadedFiles();
                fu.DocumentFor = documentFor;
                fu.DocumentType = docType;
                fu.PreRegNum = RegNo;
                fu.DocumentData = imageSize;
                fu.DocumentName = theFile.FileName;
                //   fu.DocumentType = sd.DocumentType;
                fu.DocumentSize = theFile.ContentLength.ToString();
                fu.UploadedDate = DateTime.Now.Date.ToString("dd/MM/yyyy");

                ads.CreateOrUpdateUploadedFiles(fu);

                return Json(new { success = true, result = "Successfully uploaded the file!" }, "text/html", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, result = "You have uploded the empty file. Please upload the correct file." }, "text/x-json", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        public ActionResult DocumentReport()
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<CampusMaster>> CampusMaster = ms.GetCampusMasterListWithPagingAndCriteria(0, 50, string.Empty, string.Empty, criteria);
            ViewBag.campusddl = CampusMaster.First().Value;
            return View();
        }

        public ActionResult DocumentReportJqGrid(string ddlSearchBy, string Campus, string IdNum, string DocAvl, int? Id, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    DocumentsService ds = new DocumentsService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    if (!string.IsNullOrWhiteSpace(Campus)) { criteria.Add("Campus", Campus); }
                    if (!string.IsNullOrWhiteSpace(ddlSearchBy)) { criteria.Add("DocumentType", ddlSearchBy); }
                    if (!string.IsNullOrWhiteSpace(IdNum)) { criteria.Add("IdNum", IdNum); }
                    // criteria.Add("IsDocumentAvailable", "Yes"); 

                    Dictionary<long, IList<DocumentReport_Vw>> MasterList = ds.GetDocumentsReportListWithPaging(page - 1, rows, sidx, sord, criteria);
                    if (MasterList != null && MasterList.First().Key > 0)
                    {
                        long totalrecords = MasterList.FirstOrDefault().Key;
                        int totalPages = (int)Math.Ceiling(totalrecords / (float)rows);
                        var AssLst = new
                        {
                            total = totalPages,
                            page = page,
                            records = totalrecords,
                            rows = (
                                 from items in MasterList.First().Value
                                 select new
                                 {
                                     cell = new string[] 
                                         {
                                            items.Id.ToString(),
                                            items.PreRegNum.ToString(),
                                            items.Campus,
                                            items.IdNum,
                                            String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '/StaffManagement/ApplicationForm?id="+items.PreRegNum+"'  >{0}</a>",items.Name),
                                            items.Type,
                                            items.IsDocumentAvailable == "Yes" ? "<i class='ace-icon fa fa-check'></i>" : "-",
                                            items.EmailId == null ? "-" : "<i class='ace-icon fa fa-check'></i>",
                                            items.PhoneNo == null ? "-" : "<i class='ace-icon fa fa-check'></i>",
                                            items.PermanantAddress == null ? "-" : "<i class='ace-icon fa fa-check'></i>",
                                            items.PFNo == null ? "-" : "<i class='ace-icon fa fa-check'></i>",
                                            items.ESINo == null ? "-" : "<i class='ace-icon fa fa-check'></i>",
                                            items.BankAccountNumber == null  ? "-" : "<i class='ace-icon fa fa-check'></i>",
                                            items.Designation == null ? "-" : "<i class='ace-icon fa fa-check'></i>",
                                            items.DOB == null ? "-" : "<i class='ace-icon fa fa-check'></i>",
                                            //items.IsDocumentAvailable == "Yes" ?  "<b style='color:green'>Document Avilable</b>" : "<b style='color:red'>Document not uploaded</b>",
                                            //items.EmailId == null ? "<b style='color:Red'>EMail Id not entered</b>" : "<b style='color:green'>EMail Id available</b>",
                                            //items.PhoneNo == null ? "<b style='color:Red'>Phone No not entered</b>" : "<b style='color:green'>Phone No available</b>", 
                                            //items.PermanantAddress == null ?"<b style='color:Red'>Address not entered</b>" : "<b style='color:green'>Address available</b>",
                                            //items.PFNo == null ? "<b style='color:Red'>PF# not entered</b>" : "<b style='color:green'>PF# available</b>",
                                            //items.ESINo == null ? "<b style='color:Red'>ESI# Id not entered</b>" : "<b style='color:green'>ESI# available</b>",
                                            //items.BankAccountNumber == null ? "<b style='color:Red'>Bank Details not entered</b>" : "<b style='color:green'>Bank Details available</b>",
                                            //items.Designation == null ? "<b style='color:Red'>Designation not entered</b>" : "<b style='color:green'>Designation available</b>",
                                            //items.DOB == null ?"<b style='color:Red'>Date of Birth not entered</b>" : "<b style='color:green'>DOB available</b>",

                                            //items.UploadedfilesId != null? String.Format("<img src='/Images/Download.png ' id='ImgHory' onclick=\"ShowComments('" + GetDocumentById(items.PreRegNum) +"' );\" />"):null,
                                      // items.UploadedfilesId != null? String.Format(@"<a style='color:#034af3;text-decoration:underline' href= '/StaffManagement/uploaddisplay?Id="+items.UploadedfilesId+"' target='_Blank'>{0}</a>",items.DocumentName):null,
                                         }
                                 }).ToList()
                        };
                        return Json(AssLst, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "TransportMgmtPolicy");
                throw ex;
            }
        }

        #region Campus Subject Master Added By Prabakaran
        public ActionResult CampusSubjectMaster()
        {
            try
            {
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, PolicyName);
                throw;
            }
        }
        public ActionResult CampusSubjectMasterJqGrid(CampusSubjectMaster campussubjectmaster, string IsAcademicSubject, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<string, object> likecriteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (campussubjectmaster != null)
                {
                    if (!string.IsNullOrEmpty(campussubjectmaster.Campus))
                    { criteria.Add("Campus", campussubjectmaster.Campus); }
                    if (!string.IsNullOrEmpty(campussubjectmaster.Grade))
                    { criteria.Add("Grade", campussubjectmaster.Grade); }
                    if (!string.IsNullOrEmpty(campussubjectmaster.Section))
                    { criteria.Add("Section", campussubjectmaster.Section); }
                    if (!string.IsNullOrEmpty(campussubjectmaster.SubjectName))
                    { criteria.Add("SubjectName", campussubjectmaster.SubjectName); }
                    if (!string.IsNullOrEmpty(campussubjectmaster.AcademicYear))
                    { criteria.Add("AcademicYear", campussubjectmaster.AcademicYear); }
                    if (!string.IsNullOrEmpty(campussubjectmaster.Description))
                    { likecriteria.Add("Description", campussubjectmaster.Description); }
                }
                if (!string.IsNullOrEmpty(IsAcademicSubject))
                {
                    if (IsAcademicSubject == "True" || IsAcademicSubject == "true")
                    { criteria.Add("IsAcademicSubject", true); }
                    if (IsAcademicSubject == "False" || IsAcademicSubject == "false")
                    { criteria.Add("IsAcademicSubject", false); }
                }
                Dictionary<long, IList<CampusSubjectMaster>> CampusSubjectMaster = null;
                CampusSubjectMaster = ms.GetCampusSubjectMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria, likecriteria);

                if (CampusSubjectMaster != null && CampusSubjectMaster.FirstOrDefault().Key > 0)
                {
                    IList<CampusSubjectMaster> campussubmaster = CampusSubjectMaster.FirstOrDefault().Value;
                    long totalRecords = CampusSubjectMaster.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows =
                        (
                        from items in campussubmaster
                        select new
                        {
                            i = items.Id,
                            cell = new string[]
                           {
                                items.Id.ToString(),                                
                                items.Campus,
                                items.Grade,
                                items.Section,
                                items.SubjectName,
                                items.Description,
                                items.AcademicYear,
                                items.IsAcademicSubject==true?"Yes":"No",                                                             
                                items.CreatedBy,
                                items.CreatedDate!=null?items.CreatedDate.Value.ToString("dd/MM/yyyy"):"",
                                items.ModifiedBy,
                                items.ModifiedDate!=null?items.ModifiedDate.Value.ToString("dd/MM/yyyy"):"",
                                
                           }
                        }
                        )
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, PolicyName);
                throw ex;
            }
        }
        public ActionResult AddCampusSubjectMaster(CampusSubjectMaster campussubjectmaster)
        {
            try
            {
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                campussubjectmaster.Description = campussubjectmaster.SubjectName;
                campussubjectmaster.CreatedBy = userId;
                campussubjectmaster.CreatedDate = DateTime.Now;
                ms.CreateOrUpdateCampusSubjectMaster(campussubjectmaster);
                return null;
            }
            catch (Exception ex)
            {

                ExceptionPolicy.HandleException(ex, PolicyName);
                throw ex;
            }
        }
        public ActionResult EditCampusSubjectMaster(CampusSubjectMaster campussubjectmaster)
        {
            try
            {
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                if (campussubjectmaster != null)
                {
                    if (campussubjectmaster.Id > 0)
                    {
                        CampusSubjectMaster csm = ms.GetCampusSubjectMasterById(campussubjectmaster.Id);
                        csm.Campus = campussubjectmaster.Campus;
                        csm.SubjectName = campussubjectmaster.SubjectName;
                        csm.Grade = campussubjectmaster.Grade;
                        csm.Section = campussubjectmaster.Section;
                        csm.Description = campussubjectmaster.SubjectName;
                        csm.AcademicYear = campussubjectmaster.AcademicYear;
                        csm.IsAcademicSubject = campussubjectmaster.IsAcademicSubject;
                        csm.ModifiedBy = userId;
                        csm.ModifiedDate = DateTime.Now;
                        ms.CreateOrUpdateCampusSubjectMaster(csm);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {

                ExceptionPolicy.HandleException(ex, PolicyName);
                throw ex;
            }
        }
        public ActionResult DeleteCampusSubjectMaster(string[] Id)
        {
            try
            {
                var bulkId = Id[0].Split(',');
                List<int> bulkIds = new List<int>();
                foreach (var item in bulkId) { bulkIds.Add(Convert.ToInt32(item)); }
                ms.DeleteCampusSubjectMasterList(bulkIds);
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region Campus Wise Section Master By john naveen
        public ActionResult CampusWiseSectionMaster()
        {
            try
            {
                string userId = ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("LogOff", "Account");
                else
                {
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    string ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.BreadCrumb = GetBreadCrumbDetails(ControllerName, ActionName);
                    return View();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, PolicyName);
                throw;
            }
        }
        public ActionResult CampusWiseSectionMasterJqGrid(CampusWiseSectionMaster campussecmaster, string IsActive, long? AcademicYearId, long? CampusId, long? GradeId, int rows, string sidx, string sord, int? page = 1)
        {
            try
            {
                MastersService ms = new MastersService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                sord = sord == "desc" ? "Desc" : "Asc";
                if (!string.IsNullOrEmpty(IsActive))
                {
                    if (IsActive == "true" || IsActive == "True")
                    {
                        criteria.Add("IsActive", true);
                    }
                    if (IsActive == "false" || IsActive == "False")
                    {
                        criteria.Add("IsActive", false);
                    }
                }
                if (!string.IsNullOrEmpty(campussecmaster.Section)) { criteria.Add("Section", campussecmaster.Section); }
                criteria.Add("AcademicyrMaster.FormId", AcademicYearId);
                criteria.Add("CampusMaster.FormId", CampusId);
                criteria.Add("CampusGradeMaster.Id", GradeId);
                Dictionary<long, IList<CampusWiseSectionMaster>> CampusSectionMaster = null;
                CampusSectionMaster = ms.GetCampusWiseSectionMasterListWithPagingAndCriteria(page - 1, rows, sidx, sord, criteria);

                if (CampusSectionMaster != null && CampusSectionMaster.FirstOrDefault().Key > 0)
                {
                    IList<CampusWiseSectionMaster> campusSecmaster = CampusSectionMaster.FirstOrDefault().Value;
                    long totalRecords = CampusSectionMaster.FirstOrDefault().Key;
                    int totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
                    var jsonData = new
                    {
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows =
                        (
                        from items in campusSecmaster
                        select new
                        {
                            i = items.CampusWiseSectionMasterId,
                            cell = new string[]
                           {
                                items.CampusWiseSectionMasterId.ToString(),                                
                                items.AcademicyrMaster.AcademicYear,
                                items.CampusMaster.Name,
                                items.CampusGradeMaster.gradcod,
                                items.Section,
                                items.IsActive==true?"Yes":"No",
                                items.CreatedBy,
                                items.CreatedDate!=null?items.CreatedDate.ToString("dd/MM/yyyy"):"",
                                items.UpdateBy,
                                items.UpdateDate!=null?items.UpdateDate.ToString("dd/MM/yyyy"):"",
                                
                           }
                        }
                        )
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Empty = new { rows = (new { cell = new string[] { } }) };
                    return Json(Empty, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, PolicyName);
                throw ex;
            }
        }
        public ActionResult SaveOrUpdateCampusWiseSectionMaster(CampusWiseSectionMaster cwsm)
        {
            try
            {
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("logOut", "Account");
                else
                {

                    MastersService ms = new MastersService();
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    CampusWiseSectionMaster csmObj = new CampusWiseSectionMaster();
                    csmObj = ms.GetCampusWiseSectionMasterBySection(cwsm.AcademicyrMaster.FormId, cwsm.CampusMaster.FormId, cwsm.CampusGradeMaster.Id, cwsm.Section);
                    var script = "";
                    if (cwsm.CampusWiseSectionMasterId == 0)
                    {

                        if (csmObj == null)
                        {

                            cwsm.IsActive = true;
                            cwsm.CreatedBy = userId;
                            cwsm.CreatedDate = DateTime.Now;
                            cwsm.UpdateBy = userId;
                            cwsm.UpdateDate = DateTime.Now;
                            ms.CreateOrUpdateCampusWiseSectionMaster(cwsm);
                            return JavaScript(script);
                        }
                        else
                        {
                            script = @"ErrMsg(""Already exist!"");";
                            return JavaScript(script);

                        }
                    }
                    else
                    {
                        if (csmObj == null)
                        {
                            cwsm.IsActive = true;
                            cwsm.CreatedBy = userId;
                            cwsm.CreatedDate = DateTime.Now;
                            cwsm.UpdateBy = userId;
                            cwsm.UpdateDate = DateTime.Now;
                            ms.CreateOrUpdateCampusWiseSectionMaster(cwsm);
                            script = @"SucessMsg(""Updated Sucessfully"");";
                            return JavaScript(script);
                        }
                        else
                        {
                            script = @"ErrMsg(""Already exist!"");";
                            return JavaScript(script);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "MasterPolicy");
                throw ex;
            }
        }
        public ActionResult DeleteCampusWiseSectionMaster(string[] Id)
        {
            try
            {
                MastersBC MBC = new MastersBC();
                string userId = base.ValidateUser();
                if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("Logout", "Account");
                else
                {
                    if (Id == null)
                        return null;
                    string[] CityIdArray = Id[0].Split(',');
                    long[] longCityIdArray = new long[CityIdArray.Length];
                    for (int j = 0; j < CityIdArray.Length; j++)
                        longCityIdArray[j] = Convert.ToInt64(CityIdArray[j]);
                    MBC.DeleteCampusWiseSectionMaster(longCityIdArray);
                    var script = @"SucessMsg(""Deleted Successfully"");";
                    return JavaScript(script);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        #endregion
       
    }
}
