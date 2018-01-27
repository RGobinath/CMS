using System;
using System.Collections;
using System.Collections.Generic;
using PersistenceFactory;
using TIPS.Entities;
using TIPS.Entities.AdmissionEntities;
using TIPS.Entities.ParentPortalEntities;
using TIPS.Entities.Assess;

namespace TIPS.Component
{
    public class ParentPortalBC
    {

        PersistenceServiceFactory PSF = null;
        public ParentPortalBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }

        // to fill Parent IssueType DropdownBox

        public Dictionary<long, IList<ParentIssueType>> GetIssueGroupListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<ParentIssueType>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // get ParentDetails from FamilyDetails Table

        public FamilyDetails GetParentByUserId(string userId)
        {
            try
            {
                return PSF.Get<FamilyDetails>("Name", userId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public FamilyDetails GetParentByStudentPreRegNum(long stPreRegNum)
        {
            try
            {
                return PSF.Get<FamilyDetails>("PreRegNum", stPreRegNum);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public FamilyDetails GetParentDetailsByParentName(string parentName)
        {
            try
            {
                return PSF.Get<FamilyDetails>("Name", parentName);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IList<FamilyDetails> GetParentListByUserId(string userId)
        {
            try
            {
                return PSF.GetListByName<FamilyDetails>("from " + typeof(FamilyDetails) + " where Name ='" + userId + "'");
            }
            catch (Exception)
            {

                throw;
            }
        }
        public StudentTemplate GetStudentByParentUserId(long userId)
        {
            try
            {
                return PSF.Get<StudentTemplate>("PreRegNum", userId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public StudentTemplate GetStudentDetailsByStPreRegNum(long stNum)
        {
            try
            {
                return PSF.Get<StudentTemplate>("PreRegNum", stNum);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<StudentTemplate> GetStudentListByParentUserId(long userId) //Administrative
        {
            try
            {
                return PSF.GetListByName<StudentTemplate>("from " + typeof(StudentTemplate) + " where PreRegNum ='" + userId + "'");
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public IList<StudentTemplate> GetStudentDetailsListByStPreRegNum(long stNum) //Administrative
        {
            try
            {
                return PSF.GetListByName<StudentTemplate>("from " + typeof(StudentTemplate) + " where PreRegNum ='" + stNum + "'");
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public Dictionary<long, IList<StudentTemplate>> GetStudentInformationListWithsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCount<StudentTemplate>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<FamilyDetails>> GetFamilyDetailsListWithsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCount<FamilyDetails>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateParentIssueMgt(ParentIssueMgt cm)
        {
            try
            {
                if (cm != null)
                    PSF.SaveOrUpdate<ParentIssueMgt>(cm);
                else { throw new Exception("ParentIssueMgt is required and it cannot be null.."); }
                return cm.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdatePermissionForms(PermissionForms pf)
        {
            try
            {
                if (pf != null)
                    PSF.SaveOrUpdate<PermissionForms>(pf);
                else { throw new Exception("ParentPermissionForms is required and it cannot be null.."); }
                return pf.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<CallManagement>> GetValuesFromParentIssueMgt(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<CallManagement>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<PermissionForms>> GetValuesFromParentPermissionForms(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<PermissionForms>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CallManagement GetParentIssueByViewId(long viewId)
        {
            try
            {
                return PSF.Get<CallManagement>("Id", viewId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public PermissionForms GetParentPermissionByViewId(long viewId)
        {
            try
            {
                return PSF.Get<PermissionForms>("Id", viewId);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<ParentLeaveRequest>> GetValuesFromParentLeaveRequest(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<ParentLeaveRequest>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {

                throw;
            }
        }
        // for Notification 
        public long CreateOrUpdateNotification(Notification n)
        {
            try
            {
                if (n != null)
                    PSF.SaveOrUpdate<Notification>(n);
                else { throw new Exception("Notification is required and it cannot be null.."); }
                return n.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public long CreateShowNotification(ShowNotification sn)
        {
            try
            {
                if (sn != null)
                    PSF.Save<ShowNotification>(sn);
                else { throw new Exception("Notification is required and it cannot be null.."); }
                return sn.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long CreateOrUpdateNoteAttachment(NoteAttachment na)
        {
            try
            {
                if (na != null)
                    PSF.SaveOrUpdate<NoteAttachment>(na);
                else { throw new Exception("NoteAttachment is required and it cannot be null.."); }
                return na.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IList<NoteAttachment> CreateOrUpdateNoteAttachmentsList(IList<NoteAttachment> naList)
        {
            try
            {
                if (naList != null)
                    PSF.SaveOrUpdate<NoteAttachment >(naList);
                else { throw new Exception("Email Attachment is required and it cannot be null.."); }
                return naList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long GetMaxAttachmentId()
        {
            try
            {
                string query = "select MAX(id) from NoteAttachment";
                IList list = PSF.ExecuteSql(query);
                if (list != null && list[0] != null)
                {
                    return Convert.ToInt64(list[0].ToString()); //list[0] = "0";
                }
                else return 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long GetMaxNoteId()
        {
            try
            {
                string query = "select Count from ParentCount where AppName='Notification'";
                IList list = PSF.ExecuteSql(query);
                if (list != null && list[0] != null)
                {
                    return Convert.ToInt64(list[0].ToString()); //list[0] = "0";
                }
                else return 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateNoteCount(long upd)
        {
            try
            {
                string query = "update ParentCount set Count='" + upd + "' where AppName='Notification'";
                PSF.ExecuteSql(query);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeleteAttachment(long del)
        {
            try
            {
                string query = "delete from NoteAttachment where Id='" + del + "'";
                PSF.ExecuteSql(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<NoteAttachment> GetDocListById(long dId)
        {
            try
            {
                return PSF.GetListByName<NoteAttachment>("from " + typeof(NoteAttachment) + " where NotePreId ='" + dId + "'");
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public Dictionary<long, IList<NoteAttachment>> GetDocumentsListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<NoteAttachment>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<NoteAttachmentView>> GetDocumentsListViewWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<NoteAttachmentView>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateNoteEmailLog(NoteEmailLog el)
        {
            try
            {
                if (el != null)
                    PSF.SaveOrUpdate<NoteEmailLog>(el);
                else { throw new Exception("NotificationEmailLog is required and it cannot be null.."); }
                return el.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<Notification>> GetValuesFromNotification(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<Notification>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IList<Notification> GetNotificationListByParent(string PubGrp)
        {
            try
            {
                return PSF.GetListByName<Notification>("from " + typeof(Notification) + " where PublishTo ='" + PubGrp + "'");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IList<ShowNotification> GetShowNotificationListbyNotePreId(long notepreid)
        {
            try
            {
                return PSF.GetListByName<ShowNotification>("from " + typeof(ShowNotification) + " where NotePreId ='" + notepreid + "'");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void SetValidNotificaionByExpireDate(long expID)
        {
            try
            {
                string query = "update Notification set Valid='0', Status='0' where Id='" + expID + "'";
                PSF.ExecuteSqlUsingSQLCommand(query);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public void SetValidShowNotificaionByExpireDate(long npID)
        {
            try
            {
                string query = "update ShowNotification set NoteValid='0' where NotePreId='" + npID + "'";
                PSF.ExecuteSqlUsingSQLCommand(query);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public void SetValidShowNotificationByPublishDate(long npID)
        {
            try
            {
                string query = "update ShowNotification set NoteValid='1' where NotePreId='" + npID + "'";
                PSF.ExecuteSqlUsingSQLCommand(query);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public void SetValidNotificationByPublishDate(long expID)
        {
            try
            {
                string query = "update Notification set Valid='1', Status='1' where Id='" + expID + "'";
                PSF.ExecuteSqlUsingSQLCommand(query);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public void DeleteExistingNewIdFromShowNotification(long notepreid)
        {
            try
            {
                string query = "delete from ShowNotification where NotePreId='" + notepreid + "'";
                PSF.ExecuteSql(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteNotification(long[] id)
        {
            try
            {
                IList<Notification> note = PSF.GetListByIds<Notification>(id);
                PSF.DeleteAll<Notification>(note);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<Notification>> GetNoteTypesearchCriteriaAlias(int? page, int? pageSize, string sortType, string sortBy, string name, string[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCountArray<Notification>(page, pageSize, sortType, sortBy, name, values, criteria, alias);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public IList<Notification> GetNotificationListById(long delId)
        {
            try
            {
                return PSF.GetListByName<Notification>("from " + typeof(Notification) + " where Id ='" + delId + "'");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteNoteAttachmentByNotePreId(long delId)
        {
            try
            {
                string query = "delete from NoteAttachment where NotePreId='" + delId + "'";
                PSF.ExecuteSqlUsingSQLCommand(query);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public void DeleteShowNotificationByNotePreId(long delId)
        {
            try
            {
                string query = "delete from ShowNotification where NotePreId='" + delId + "'";
                PSF.ExecuteSqlUsingSQLCommand(query);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public void SetShowNotificationByValidNote(long notepreid, long nval)
        {
            try
            {
                string query = "update ShowNotification set NoteValid='" + nval + "' where NotePreId='" + notepreid + "'";
                PSF.ExecuteSqlUsingSQLCommand(query);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public Notification getNoteDetailsfromPreId(long nid)
        {
            try
            {
                return PSF.Get<Notification>("NotePreId", nid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<ShowNotification>> GetShowNotificationSearchCriteriaAlias(int? page, int? pageSize, string sortType, string sortBy, string name, string[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCountArray<ShowNotification>(page, pageSize, sortType, sortBy, name, values, criteria, alias);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /* End of Notification */

        /* Start of Photo Gallery */

        public long GetMaxPGId()
        {
            try
            {
                string query = "select Count from ParentCount where AppName='PhotoGallery'";
                IList list = PSF.ExecuteSql(query);
                if (list != null && list[0] != null)
                {
                    return Convert.ToInt64(list[0].ToString()); //list[0] = "0";
                }
                else return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePGCount(long upd)
        {
            try
            {
                string query = "update ParentCount set Count='" + upd + "' where AppName='PhotoGallery'";
                PSF.ExecuteSql(query);

            }
            catch (Exception)
            {

                throw;
            }
        }
        public long GetMaxPGPId()
        {
            try
            {
                string query = "select MAX(id) from PhotoGalleryPhotos";
                IList list = PSF.ExecuteSql(query);
                if (list != null && list[0] != null)
                {
                    return Convert.ToInt64(list[0].ToString()); //list[0] = "0";
                }
                else return 0;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public long CreateOrUpdatePhotoGallery(PhotoGallery pg)
        {
            try
            {
                if (pg != null)
                    PSF.SaveOrUpdate<PhotoGallery>(pg);
                else { throw new Exception("PhotoGallery is required and it cannot be null.."); }
                return pg.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdatePhotoGalleryPhoto(PhotoGalleryPhotos pgp)
        {
            try
            {
                if (pgp != null)
                    PSF.SaveOrUpdate<PhotoGalleryPhotos>(pgp);
                else { throw new Exception("PhotoGalleryPhotos is required and it cannot be null.."); }
                return pgp.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteUploadedFiles(long del)
        {
            try
            {
                string query = "delete from PhotoGalleryPhotos where Id='" + del + "'";
                PSF.ExecuteSql(query);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<PhotoGallery>> GetValuesFromPhotoGallery(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<PhotoGallery>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeletePhotoGallerybyId(long[] id)
        {
            try
            {
                IList<PhotoGallery> note = PSF.GetListByIds<PhotoGallery>(id);
                PSF.DeleteAll<PhotoGallery>(note);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PhotoGallery getPGDetailsfromDelId(long pgid)
        {
            try
            {
                return PSF.Get<PhotoGallery>("Id", pgid);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public PhotoGallery getPGDetailsfromId(long pgid)
        {
            try
            {
                return PSF.Get<PhotoGallery>("PGPreId", pgid);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool DeletePhotoGalleryPhotosbyPGPreId(long[] id)
        {
            try
            {
                IList<PhotoGalleryPhotos> note = PSF.GetListByIds<PhotoGalleryPhotos>(id);
                PSF.DeleteAll<PhotoGalleryPhotos>(note);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PhotoGalleryPhotos> getPGPreIdList(long pgpreId)
        {
            try
            {
                return PSF.GetListByName<PhotoGalleryPhotos>("from " + typeof(PhotoGalleryPhotos) + " where PGPreId ='" + pgpreId + "'");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<PhotoGalleryPhotos>> GetValuesFromPhotoGalleryPhotos(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<PhotoGalleryPhotos>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool DeletePhotosById(long id)
        {
            try
            {
                PhotoGalleryPhotos phid = PSF.Get<PhotoGalleryPhotos>(id);
                PSF.Delete<PhotoGalleryPhotos>(phid);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /* End of Photo Gallery */
        public StudentTemplateView GetStudentTemplateViewByStNewId(string IdNo)
        {
            try
            {
                return PSF.Get<StudentTemplateView>("NewId", IdNo);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<CallManagementView>> GetCallManagementViewParentIssueMgt(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<CallManagementView>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Notification GetParentNotificationByViewId(long viewId)
        {
            try
            {
                return PSF.Get<Notification>("Id", viewId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Notification GetGeneralNotificationByNotePreId(long NotePreId)
        {
            try
            {
                return PSF.Get<Notification>("NotePreId", NotePreId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        
        #region start food menu

        public Dictionary<long, IList<FoodMenuView>> GetValuesFromFoodMenu(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<FoodMenuView>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<FoodNameMaster>> GetValuesFromFoodNameMaster(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<FoodNameMaster>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long CreateOrUpdateFoodNameMater(FoodNameMaster fnm)
        {
            try
            {
                if (fnm != null)
                    PSF.Save<FoodNameMaster>(fnm);
                else { throw new Exception("FoodNameMaster is required and it cannot be null.."); }
                return fnm.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool DeleteFoodNameMaterDetails(long[] id)
        {
            try
            {
                IList<FoodNameMaster> note = PSF.GetListByIds<FoodNameMaster>(id);
                PSF.DeleteAll<FoodNameMaster>(note);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<FoodNameMaster>> GetCampusBreakFastListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<FoodNameMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateFoodMenu(FoodMenuView fm)
        {
            try
            {
                if (fm != null)
                    PSF.Save<FoodMenuView>(fm);
                else { throw new Exception("FoodMenu is required and it cannot be null.."); }
                return fm.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool DeleteFoodMenuDetails(long[] id)
        {
            try
            {
                IList<FoodMenuView> note = PSF.GetListByIds<FoodMenuView>(id);
                PSF.DeleteAll<FoodMenuView>(note);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<FoodMenuView>> GetFoodMenuListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<FoodMenuView>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateFoodMenuByWeeks(FMWeeks fmw1)
        {
            try
            {
                if (fmw1 != null)
                    PSF.SaveOrUpdate<FMWeeks>(fmw1);
                else { throw new Exception("FMWeeks is required and it cannot be null.."); }
                return fmw1.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion end food menu
        public bool UpdatePhotoGalleryNotificationStatus(string campus)
        {
            try
            {
                string sqlQuery = "update PhotoGalleryNotification set Viewstatus=1 where Campus='" + campus + "' and ViewStatus=0";
                PSF.ExecuteSqlUsingSQLCommand(sqlQuery);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region Assess360

        public Dictionary<long, IList<Assess360>> GetAssess360ListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<Assess360>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion Assess360
        #region Added By Prabakaran
        public long DeletePGPhotosByFolder_Id(long Id, long Folder_Id)
        {
            try
            {
                if (Id > 0 && Folder_Id > 0)
                {
                    string sqlQuery = "delete PhotoGalleryPhotos where PGPreId='" + Id + "' and Folder_Id='" + Folder_Id + "'";
                    PSF.ExecuteSqlUsingSQLCommand(sqlQuery);
                }
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteFolder(long Id)
        {
            try
            {
                PhotoGalleryFolder photogalleryfolder = PSF.Get<PhotoGalleryFolder>(Id);
                PSF.Delete<PhotoGalleryFolder>(photogalleryfolder);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public PhotoGallery GetPhotoGalleryById(long Id)
        {
            try
            {
                PhotoGallery pg = new PhotoGallery();
                if (Id > 0)
                {
                    pg = PSF.Get<PhotoGallery>(Id);
                }
                return pg;
            }
            catch (Exception)
            {

                throw;
            }
            finally { }
        }
        public long CreateOrUpdatePhotoGalleryFolder(PhotoGalleryFolder pgf)
        {
            try
            {
                if (pgf != null)
                    PSF.SaveOrUpdate<PhotoGalleryFolder>(pgf);
                else { throw new Exception("Folder Details is required and it cannot be null.."); }
                return pgf.Folder_Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<PhotoGalleryFolder>> GetValuesFromPhotoGalleryFolder(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<PhotoGalleryFolder>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public PhotoGalleryFolder getPGFolderByFolderNamewithPgPreId(long pgid, string FolderName)
        {
            try
            {
                return PSF.Get<PhotoGalleryFolder>("PGPreId", pgid, "FolderName", FolderName);
            }
            catch (Exception)
            {

                throw;
            }
        }        
        public PhotoGallery getPGDetailsByPKId(long Id)
        {
            try
            {
                return PSF.Get<PhotoGallery>(Id);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public PhotoGallery getPGDetailsByParentRefIdandPGPreId(long Id)
        {
            try
            {
                long ParentRefId = 0;
                return PSF.Get<PhotoGallery>("PGPreId", Id, "ParentRefId", ParentRefId);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public PhotoGallery getPGById(long pgid)
        {
            try
            {
                return PSF.Get<PhotoGallery>("PGPreId", pgid);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeletePhotoGallery(PhotoGallery pg)
        {
            try
            {
                if (pg != null)
                {
                    PSF.Delete<PhotoGallery>(pg);
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeletePhotoGalleryPhotosByFolder_IdandId(long Id, long PGPreId)
        {
            try
            {
                string sqlQuery = "delete from PhotoGalleryPhotos where PGPreId='" + PGPreId + "' and Folder_Id='" + Id + "'";
                PSF.ExecuteSqlUsingSQLCommand(sqlQuery);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool UpdatePhotoGalleryDetails(bool IsActive, long PGPreId)
        {
            try
            {
                string sqlQuery = "";
                if (IsActive == true)
                {
                    sqlQuery = "update PhotoGallery set IsActive=1 where PGPreId='" + PGPreId + "'";
                }
                else
                {
                    sqlQuery = "update PhotoGallery set IsActive=0 where PGPreId='" + PGPreId + "'";
                }
                PSF.ExecuteSqlUsingSQLCommand(sqlQuery);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<PhotoGallery> getPGalleryPreIdList(long pgpreId)
        {
            try
            {
                return PSF.GetListByName<PhotoGallery>("from " + typeof(PhotoGallery) + " where PGPreId ='" + pgpreId + "'");
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
