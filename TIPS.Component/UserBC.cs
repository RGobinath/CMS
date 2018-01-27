using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TIPS.Entities;
using PersistenceFactory;
using CustomAuthentication;
using System.Collections;
namespace TIPS.Component
{
    public class UserBC
    {
        PersistenceServiceFactory PSF = null;
        public UserBC()
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
        public Dictionary<long, IList<UserAppRole>> GetRoleUsersListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
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
        public bool DeleteUserAppRole(long id)
        {
            try
            {
                UserAppRole UserAppRole = PSF.Get<UserAppRole>(id);
                PSF.Delete<UserAppRole>(UserAppRole);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteUserAppRole(long[] id)
        {
            try
            {

                IList<UserAppRole> UserAppRole = PSF.GetListByIds<UserAppRole>(id);
                PSF.DeleteAll<UserAppRole>(UserAppRole);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateUserAppRole(UserAppRole userapprole)
        {
            try
            {
                if (userapprole != null)
                    PSF.SaveOrUpdate<UserAppRole>(userapprole);
                else { throw new Exception("userapprole is required and it cannot be null.."); }
                return userapprole.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<Application>> GetApplicationListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<Application>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateApplication(Application app)
        {
            try
            {
                if (app != null)
                    PSF.SaveOrUpdate<Application>(app);
                else { throw new Exception("Application is required and it cannot be null.."); }
                return app.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<Role>> GetRoleListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<Role>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateRole(Role role)
        {
            try
            {
                if (role != null)
                    PSF.SaveOrUpdate<Role>(role);
                else { throw new Exception("Application is required and it cannot be null.."); }
                return role.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<Department>> GetDepartmentListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<Department>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateDepartment(Department department)
        {
            try
            {
                if (department != null)
                    PSF.SaveOrUpdate<Department>(department);
                else { throw new Exception("Application is required and it cannot be null.."); }
                return department.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<Branch>> GetBranchListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<Branch>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateBranch(Branch branch)
        {
            try
            {
                if (branch != null)
                    PSF.SaveOrUpdate<Branch>(branch);
                else { throw new Exception("Application is required and it cannot be null.."); }
                return branch.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void CreateOrUpdateUser(User User)
        {
            try
            {
                if (User != null)
                {

                    //check whether the user exists already or not
                    User Userexists = PSF.Get<User>("UserId", User.UserId.Trim());

                    if (Userexists == null && User.Id == 0)
                    {
                        PSF.Save<User>(User);
                    }
                    else if (Userexists == null && User.Id > 0)//changed by prabakaran
                    {
                        PSF.SaveOrUpdate<User>(User);
                    }
                    else if (Userexists != null && Userexists.Id == User.Id)
                    {
                        PSF.SaveOrUpdate<User>(User);
                    }
                    else
                    {
                        throw new Exception("User already exists for " + User.UserId.Trim() + ".");
                    }

                }
                else { PSF.Save<User>(User); }
                return;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void ModifyUser(User User)
        {
            try
            {
                if (User != null)
                {
                    //check whether the user exists already or not
                    User Userexists = PSF.Get<User>("UserId", User.UserId.Trim());

                    if (Userexists == null)
                    {
                        PSF.Save<User>(User);
                    }
                    //else if (Userexists != null && Userexists.Id == User.Id)
                    //{
                    //    PSF.SaveOrUpdate<User>(User);
                    //}
                    else if (Userexists != null)
                    {
                        Userexists.ModifiedDate = DateTime.Now;
                        Userexists.CreatedDate = Userexists.CreatedDate == null ? DateTime.Now : Userexists.CreatedDate;
                        if (!string.IsNullOrEmpty(User.EmailId))
                            Userexists.EmailId = User.EmailId;
                        // if(User.IsActive)
                        if (User.IsActive == true || User.IsActive == false)
                            Userexists.IsActive = User.IsActive;
                        if (!string.IsNullOrEmpty(User.UserName))
                            Userexists.UserName = User.UserName;
                        if (!string.IsNullOrEmpty(User.UserType))
                            Userexists.UserType = User.UserType;

                        PSF.SaveOrUpdate<User>(Userexists);
                    }
                    else
                    {
                        throw new Exception("User already exists for " + User.UserId.Trim() + ".");
                    }
                }
                else { PSF.Save<User>(User); }
                return;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public User GetUserByUserId(string userId)
        {
            try
            {
                return PSF.Get<User>("UserId", userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public User GetUserByEmailId(string emailId)
        {
            try
            {
                return PSF.Get<User>("EmailId", emailId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool ChangePassword(string userId, string oldPassword, string newPassword)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(userId) && !string.IsNullOrWhiteSpace(oldPassword) && !string.IsNullOrWhiteSpace(newPassword))
                {

                    //Get the user 
                    User Userexists = PSF.Get<User>("UserId", userId.Trim());
                    if (Userexists != null)
                    {
                        PassworAuth PA = new PassworAuth();
                        //encode and save the password
                        Userexists.Password = PA.base64Encode(newPassword);
                        PSF.Update<User>(Userexists);
                    }
                    else throw new Exception("No User found for " + userId.Trim() + "");
                }
                else { throw new Exception("User/oldPassword/newPassword is required and it cannot be null.."); }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateSavedSearchTemplate(SavedSearchTemplate SavedSearchTemplate)
        {
            try
            {
                if (SavedSearchTemplate != null)
                {
                    //Check whether any template has been stored for the same user
                    SavedSearchTemplate existsOrNot = PSF.Get<SavedSearchTemplate>("UserId", SavedSearchTemplate.UserId, "Application", SavedSearchTemplate.Application, "SearchName", SavedSearchTemplate.SearchName);
                    if (existsOrNot == null)
                    {
                        PSF.SaveOrUpdate<SavedSearchTemplate>(SavedSearchTemplate);
                    }
                    else throw new Exception("Template already exists in this Name.");
                }
                else throw new Exception("SearchTemplate is required and it cannot be null.");
                return SavedSearchTemplate.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<SavedSearchTemplate>> GetSavedSearchTemplateListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<SavedSearchTemplate>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<UserAppRole>> GetAppRoleForAnUserListWithPagingAndCriteriaWithLikeSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCount<UserAppRole>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<User>> GetUserListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<User>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<User>> GetUserListWithPagingAndCriteriaLikeSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<User>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void CreateOrUpdateSession(LoginHistory s)
        {
            try
            {
                if (s != null)
                {
                    PSF.Save<LoginHistory>(s);

                    //check whether the user exists already or not
                    //Session Userexists = PSF.Get<Session>("UserId", s.UserId.Trim());

                    //if (Userexists == null)
                    //{
                    //    PSF.Save<Session>(s);
                    //}
                    //else if (Userexists != null && Userexists.Id == s.Id)
                    //{

                    //    PSF.SaveOrUpdate<Session>(s);
                    //}

                    //else
                    //{
                    //    throw new Exception("User already exists for " + s.UserId.Trim() + ".");
                    //}

                }
                else { PSF.Save<LoginHistory>(s); }
                return;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateSession(LoginHistory s)
        {
            try
            {
                if (s != null)
                {
                    PSF.SaveOrUpdate(s);
                }
                else
                {
                    return;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<LoginHistory>> GetSessionListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<LoginHistory>> retValue = new Dictionary<long, IList<LoginHistory>>();
                return PSF.GetListWithEQSearchCriteriaCount<LoginHistory>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<CampusAdminEmailId> GetCampusAdminEmailListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteria<CampusAdminEmailId>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region TIPSMaster
        public Dictionary<long, IList<TIPSMaster>> GetTIPSMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<TIPSMaster>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
             
        public string CreateOrUpdateTIPSMaster(TIPSMaster tips, string userid)
        {
            try
            {
                //logic to check before saving
                if (tips != null && tips.Id > 0)
                {
                    tips.ModifiedBy = userid;
                    tips.ModifiedDate = DateTime.Now;
                    PSF.SaveOrUpdate<TIPSMaster>(tips);
                }
                else
                {
                    tips.CreatedBy = userid;
                    tips.CreatedDate = DateTime.Now;
                    PSF.SaveOrUpdate<TIPSMaster>(tips);
                }
                return null;
            }
            catch (Exception)
            { throw; }
        }
        #endregion TIPSMaster

        public Dictionary<long, IList<SavedSearchTemplate>> GetSavedSearchTemplateListWithEQPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<SavedSearchTemplate>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region Monthly wise login count report

        public Dictionary<long, IList<LogInCountReportView>> GetLogInCountListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<LogInCountReportView>> retValue = new Dictionary<long, IList<LogInCountReportView>>();
                return PSF.GetListWithSearchCriteriaCount<LogInCountReportView>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        public IList<User> CreateOrUpdateUserList(IList<User> userlist)
        {
            try
            {
                if (userlist != null)
                {
                    PSF.SaveOrUpdate<User>(userlist);
                }
                else { throw new Exception("User List is required and it cannot be null.."); }
                return userlist;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<UserAppRole>> GetPerformerListWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
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

        public IList<CoOrdinatorsContactInfo> GetCoOrdinatorsContactInfoListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteria<CoOrdinatorsContactInfo>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CreateOrUpdateCoOrdinatorsContactInfo(CoOrdinatorsContactInfo cci)
        {
            try
            {
                if (cci != null)
                    PSF.SaveOrUpdate<CoOrdinatorsContactInfo>(cci);
                else { throw new Exception("CoOrdinatorsContactInfo is required and it cannot be null.."); }
                return cci.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetUserNameByUserId(string userId)
        {
            try
            {
                User user = PSF.Get<User>("UserId", userId);
                if (user != null && !string.IsNullOrWhiteSpace(user.UserName))
                    return user.UserName;
                else
                    return "";
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<User>> GetUserListWithLikePagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<User>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public long CreateOrUpdatePageHistory(PageHistory ph)
        //{
        //    try
        //    {
        //        if (ph != null)
        //            PSF.SaveOrUpdate<PageHistory>(ph);
        //        else { throw new Exception("PageHistory is required and it cannot be null.."); }
        //        return ph.Id;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //public long CreateOrUpdatePageHistory(PageHistory pageHistory)
        //{
        //    try
        //    {
        //        if (pageHistory != null)
        //        {
        //            string query = string.Empty;
        //            string querystr = string.Empty;
        //            PSF.SaveOrUpdate<PageHistory>(pageHistory);
        //            query = query + "if exists (select * from PageHistoryReport where ControllerName='" + pageHistory.Controller + "' and ActionName='" + pageHistory.Action + "' and Campus='" + pageHistory.Campus + "')";
        //            query = query + "begin";
        //            query = query + " update PageHistoryReport set ControllerHit=ControllerHit + 1 , ActionHit=ActionHit + 1,ActionName='" + pageHistory.Action + "' where ControllerName='" + pageHistory.Controller + "' and Campus='" + pageHistory.Campus + "'";
        //            query = query + "end else ";
        //            query = query + "begin";
        //            query = query + " insert into PageHistoryReport values('" + pageHistory.Campus + "','" + pageHistory.Controller + "',1,'" + pageHistory.Action + "',1,null,null,null,null)";
        //            query = query + "end";
        //            querystr = query;
        //            string queryobj = querystr;
        //            IList list = PSF.ExecuteSql(queryobj);
        //            PSF.SaveOrUpdate<PageHistory>(pageHistory);
        //        }
        //        else { throw new Exception("PageHistory is required and it cannot be null.."); }
        //        return pageHistory.PageHistory_Id;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        public long SaveOrUpdatePageHistory(PageHistory pageHistory)
        {
            try
            {
                if (pageHistory != null)
                {
                    string query = string.Empty;
                    string querystr = string.Empty;
                    //menu.ParentRefId if(!string.IsNullOrEmpty(pageHistory.Action)&&!string.IsNullOrEmpty(pageHistory.Controller))
                    MenuDetails_vw menu = PSF.Get<MenuDetails_vw>("Action", pageHistory.Action, "Controller", pageHistory.Controller);
                    if (menu != null)
                    {
                        PSF.SaveOrUpdate<PageHistory>(pageHistory);
                        query = query + "if exists (select * from PageHistoryReport where ControllerName='" + pageHistory.Controller + "' and ActionName='" + pageHistory.Action + "' and Campus='" + pageHistory.Campus + "' and Menu_Id=" + menu.MainMenu_Id + ")";
                        query = query + "begin";
                        query = query + " update PageHistoryReport set ControllerHit=ControllerHit + 1 , ActionHit=ActionHit + 1 where ControllerName='" + pageHistory.Controller + "' and Campus='" + pageHistory.Campus + "' and ActionName='" + pageHistory.Action + "'";
                        query = query + "end else ";
                        query = query + "begin";
                        query = query + " insert into PageHistoryReport values('" + pageHistory.Campus + "','" + pageHistory.Controller + "',1,'" + pageHistory.Action + "',1,null,null,null,null," + menu.MainMenu_Id + ")";
                        query = query + "end";
                        querystr = query;
                        string queryobj = querystr;
                        IList list = PSF.ExecuteSql(queryobj);
                        PSF.SaveOrUpdate<PageHistory>(pageHistory);
                    }
                }
                else { throw new Exception("PageHistory is required and it cannot be null.."); }
                return pageHistory.PageHistory_Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<PageHistory>> GetPageHistoryWithLikePagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<PageHistory>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region CampusEmailId Master
        public CampusEmailId GetCampusEmailIdById(long Id)
        {
            try
            {
                return PSF.Get<CampusEmailId>("Id", Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<CampusEmailId> GetCampusEmailListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteria<CampusEmailId>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateCampusEmailId(CampusEmailId camEmail)
        {
            try
            {
                if (camEmail != null)
                {
                    //check whether the user exists already or not
                    PSF.SaveOrUpdate<CampusEmailId>(camEmail);
                }
                return camEmail.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        //Added by Thamizhmani
        public Dictionary<long, IList<PageHitCount_Vw>> GetPageHitCountWithLikePagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<PageHitCount_Vw>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region UserApproleForonlyActive Users
        public Dictionary<long, IList<UserAppRole_Vw>> GetAppRoleOnlyActiveUsersPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<UserAppRole_Vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<UserAppRole_Vw>> GetAppRoleOnlyActiveUsersLikeandEQSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> Exactcriteria, Dictionary<string, object> Likecriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<UserAppRole_Vw>(page, pageSize, sortType, sortby, Exactcriteria, Likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public User GetUserById(Int32 Id)
        {
            try
            {
                return PSF.Get<User>("Id", Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region Added By Prabakaran
        public Dictionary<long, IList<Users_vw>> GetUsers_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<Users_vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        public CampusEmailId GetCampusEmailIdByCampusWithServer(string Campus, string Server)
        {
            try
            {
                return PSF.Get<CampusEmailId>("Campus", Campus,"Server",Server);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region get Employee Id By John Naveen
        public User GetUserByEmployeeId(string EmployeeId)
        {
            try
            {
                return PSF.Get<User>("EmployeeId", EmployeeId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
