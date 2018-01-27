using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.Entities;
using TIPS.ServiceContract;
using TIPS.Component;

namespace TIPS.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserService" in code, svc and config file together.
    public class UserService : IUserServiceSC
    {
        UserBC UBC = new UserBC();
        public Dictionary<long, IList<UserAppRole>> GetAppRoleForAnUserListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                UserBC UserBC = new UserBC();
                return UserBC.GetAppRoleForAnUserListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<UserAppRole>> GetRoleUsersListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                UserBC UserBC = new UserBC();
                return UserBC.GetRoleUsersListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long CreateOrUpdateUserAppRole(Entities.UserAppRole approle)
        {
            try
            {
                UserBC AppRoleBC = new UserBC();
                AppRoleBC.CreateOrUpdateUserAppRole(approle);
                return approle.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateApplication(Entities.Application application)
        {
            try
            {
                UserBC ApplicationBC = new UserBC();
                ApplicationBC.CreateOrUpdateApplication(application);
                return application.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<Entities.Application>> GetApplicationListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                UserBC AppBC = new UserBC();
                return AppBC.GetApplicationListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public long CreateOrUpdateRole(Entities.Role role)
        {
            try
            {
                UserBC RoleBC = new UserBC();
                RoleBC.CreateOrUpdateRole(role);
                return role.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public void CreateOrUpdateUser(User User)
        {
            try
            {
                UserBC UserBC = new UserBC();
                UserBC.CreateOrUpdateUser(User);
                return;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public void ModifyUser(User User)
        {
            try
            {
                UserBC UserBC = new UserBC();
                UserBC.ModifyUser(User);
                return;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }
        public User GetUserByUserId(string userId)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    UserBC UserBC = new UserBC();
                    return UserBC.GetUserByUserId(userId);
                }
                else throw new Exception("User Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public User GetUserByEmailId(string emailId)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(emailId))
                {
                    UserBC UserBC = new UserBC();
                    return UserBC.GetUserByEmailId(emailId);
                }
                else throw new Exception("User EmailId is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool ChangePassword(string userId, string oldPassword, string newPassword)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(userId) && !string.IsNullOrWhiteSpace(oldPassword) && !string.IsNullOrWhiteSpace(newPassword))
                {
                    UserBC UserBC = new UserBC();
                    return UserBC.ChangePassword(userId, oldPassword, newPassword);
                }
                else { throw new Exception("User/oldPassword/newPassword is required and it cannot be null.."); }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<Entities.Role>> GetRoleListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                UserBC RoleBC = new UserBC();
                return RoleBC.GetRoleListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public long CreateOrUpdateDepartment(Entities.Department department)
        {
            try
            {
                UserBC DepartmentBC = new UserBC();
                DepartmentBC.CreateOrUpdateDepartment(department);
                return department.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<Entities.Department>> GetDepartmentListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                UserBC DepartmentBC = new UserBC();
                return DepartmentBC.GetDepartmentListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public long CreateOrUpdateBranch(Entities.Branch branch)
        {
            try
            {
                UserBC BranchBC = new UserBC();
                BranchBC.CreateOrUpdateBranch(branch);
                return branch.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<Entities.Branch>> GetBranchListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                UserBC BranchBC = new UserBC();
                return BranchBC.GetBranchListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                UserBC UserBC = new UserBC();
                return UserBC.CreateOrUpdateSavedSearchTemplate(SavedSearchTemplate);
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
                UserBC UserBC = new UserBC();
                return UserBC.GetSavedSearchTemplateListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                try
                {
                    UserBC UserBC = new UserBC();
                    return UserBC.GetAppRoleForAnUserListWithPagingAndCriteriaWithLikeSearch(page, pageSize, sortby, sortType, criteria);
                }
                catch (Exception)
                {
                    throw;
                }
                finally { }
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
                UserBC UserBC = new UserBC();
                return UserBC.GetUserListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                UserBC UserBC = new UserBC();
                return UserBC.GetUserListWithPagingAndCriteriaLikeSearch(page, pageSize, sortby, sortType, criteria);
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
                UserBC UserBC = new UserBC();
                UserBC.CreateOrUpdateSession(s);
                return;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }
        public void UpdateSession(LoginHistory s)
        {
            try
            {
                UserBC UserBC = new UserBC();
                UserBC.UpdateSession(s);
                return;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<LoginHistory>> GetSessionListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                UserBC UserBC = new UserBC();
                return UserBC.GetSessionListWithPaging(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

       
        public IList<CampusAdminEmailId> GetCampusAdminEmailListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                UserBC UserBC = new UserBC();
                return UserBC.GetCampusAdminEmailListWithPaging(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #region TIPSMaster
        public Dictionary<long, IList<TIPSMaster>> GetTIPSMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                UserBC AppBC = new UserBC();
                return AppBC.GetTIPSMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                UserBC UserBC = new UserBC();
                return UserBC.CreateOrUpdateTIPSMaster(tips, userid);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion TIPSMaster

        public Dictionary<long, IList<SavedSearchTemplate>> GetSavedSearchTemplateListWithEQPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                UserBC UserBC = new UserBC();
                return UserBC.GetSavedSearchTemplateListWithEQPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Monthly wise login count report
        public Dictionary<long, IList<LogInCountReportView>> GetLogInCountListWithPagingAndCriteria_vw(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                UserBC UsrBC = new UserBC();
                return UsrBC.GetLogInCountListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion

        public IList<User> CreateOrUpdateUserList(IList<User> userlist)
        {
            try
            {
                UserBC UserBC = new UserBC();
                UserBC.CreateOrUpdateUserList(userlist);
                return userlist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<UserAppRole>> GetPerformerListWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    UserBC UserBC = new UserBC();
                    return UserBC.GetPerformerListWithCriteria(page, pageSize, sortby, sortType, criteria);
                }
                catch (Exception)
                {
                    throw;
                }
                finally { }
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
                UserBC UserBC = new UserBC();
                return UserBC.GetCoOrdinatorsContactInfoListWithPaging(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public int CreateOrUpdateCoOrdinatorsContactInfo(CoOrdinatorsContactInfo cci)
        {
            try
            {
                UserBC AppRoleBC = new UserBC();
                AppRoleBC.CreateOrUpdateCoOrdinatorsContactInfo(cci);
                return cci.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public string GetUserNameByUserId(string userId)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    UserBC UserBC = new UserBC();
                    return UserBC.GetUserNameByUserId(userId);

                }
                else throw new Exception("User Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<User>> GetUserListWithLikePagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                UserBC UserBC = new UserBC();
                return UserBC.GetUserListWithLikePagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdatePageHistory(PageHistory ph)
        {
            try
            {
                UserBC UserBC = new UserBC();
                UserBC.SaveOrUpdatePageHistory(ph);
                return ph.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<PageHistory>> GetPageHistoryWithLikePagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                UserBC UserBC = new UserBC();
                return UserBC.GetPageHistoryWithLikePagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region CampusEmailId Master    
        public IList<CampusEmailId> GetCampusEmailListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                UserBC UserBC = new UserBC();
                return UserBC.GetCampusEmailListWithPaging(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public CampusEmailId GetCampusEmailIdById(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    return UBC.GetCampusEmailIdById(Id);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public long SaveOrUpdateCampusEmailId(CampusEmailId camEmail)
        {
            try
            {
                UBC.SaveOrUpdateCampusEmailId(camEmail);
                return camEmail.Id;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        //Added by Thamizhmani
        public Dictionary<long, IList<PageHitCount_Vw>> GetPageHitCountWithLikePagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                UserBC UserBC = new UserBC();
                return UserBC.GetPageHitCountWithLikePagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                UserBC UserBC = new UserBC();
                return UserBC.GetAppRoleOnlyActiveUsersPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<UserAppRole_Vw>> GetAppRoleOnlyActiveUsersLikeandEQSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> Exactcriteria, Dictionary<string, object> Likecriteria)
        {
            try
            {
                UserBC UserBC = new UserBC();
                return UserBC.GetAppRoleOnlyActiveUsersLikeandEQSearch(page, pageSize, sortby, sortType, Exactcriteria, Likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion

        public User GetUserById(Int32 Id)
        {
            try
            {
                if (Id > 0)
                {
                    UserBC UserBC = new UserBC();
                    return UserBC.GetUserById(Id);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #region Added By Prabakaran
        public Dictionary<long, IList<Users_vw>> GetUsers_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                UserBC UserBC = new UserBC();
                return UserBC.GetUsers_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        public CampusEmailId GetCampusEmailIdByCampusWithServer(string Campus,string Server)
        {
            try
            {
                if (!string.IsNullOrEmpty(Campus) && !string.IsNullOrEmpty(Server))
                {
                    return UBC.GetCampusEmailIdByCampusWithServer(Campus,Server);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #region Get Employee Id By John Navaeen
        public User GetUserByEmployeeId(string EmployeeId)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(EmployeeId))
                {
                    UserBC UserBC = new UserBC();
                    return UserBC.GetUserByEmployeeId(EmployeeId);
                }
                else throw new Exception("User Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #endregion
    }
}
