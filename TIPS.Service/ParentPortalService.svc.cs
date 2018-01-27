using System;
using System.Collections.Generic;
using System.ServiceModel;
using TIPS.Entities;
using TIPS.Component;
using TIPS.Entities.ParentPortalEntities;
using TIPS.Entities.AdmissionEntities;

namespace TIPS.ServiceContract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ParentIssueTypeService" in code, svc and config file together.
    public class ParentPortalService : IParentPortalSC
    {
        public void DoWork()
        {
        }

        // to fill Parent IssueType DropdownBox

        public Dictionary<long, IList<ParentIssueType>> GetIssueGroupListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                return ppBC.GetIssueGroupListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        // get ParentDetails from FamilyDetails Table

        public FamilyDetails GetParentByUserId(string userId)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                return ppBC.GetParentByUserId(userId);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public FamilyDetails GetParentByStudentPreRegNum(long stPreRegNum)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                return ppBC.GetParentByStudentPreRegNum(stPreRegNum);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public FamilyDetails GetParentDetailsByParentName(string parentName)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                return ppBC.GetParentDetailsByParentName(parentName);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public IList<FamilyDetails> GetParentListByUserId(string userId)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                return ppBC.GetParentListByUserId(userId);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public CallManagement GetParentIssueByViewId(long viewId)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                return ppBC.GetParentIssueByViewId(viewId);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public PermissionForms GetParentPermissionByViewId(long viewId)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                return ppBC.GetParentPermissionByViewId(viewId);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        // get StudentDetails from StudentTemplate Table for a Particular Parent User

        public StudentTemplate GetStudentByParentUserId(long userId)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                return ppBC.GetStudentByParentUserId(userId);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public StudentTemplate GetStudentDetailsByStPreRegNum(long stNum)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                return ppBC.GetStudentDetailsByStPreRegNum(stNum);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public IList<StudentTemplate> GetStudentDetailsListByStPreRegNum(long stNum)
        {
            try
            {
                ParentPortalBC piBC = new ParentPortalBC();
                return piBC.GetStudentDetailsListByStPreRegNum(stNum);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }


        public IList<StudentTemplate> GetStudentListByParentUserId(long userId)
        {
            try
            {
                ParentPortalBC piBC = new ParentPortalBC();
                return piBC.GetStudentListByParentUserId(userId);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<StudentTemplate>> GetStudentInformationListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                ParentPortalBC piBC = new ParentPortalBC();
                return piBC.GetStudentInformationListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<FamilyDetails>> GetFamilyDetailsListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                ParentPortalBC piBC = new ParentPortalBC();
                return piBC.GetFamilyDetailsListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        // save  parent issues 

        public long CreateOrUpdateParentIssueMgt(ParentIssueMgt cm)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                ppBC.CreateOrUpdateParentIssueMgt(cm);
                return cm.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<CallManagement>> GetValuesFromParentIssueMgt(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {

                ParentPortalBC piDetBC = new ParentPortalBC();
                return piDetBC.GetValuesFromParentIssueMgt(page, pageSize, sortBy, sortType, criteria);

            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<PermissionForms>> GetValuesFromParentPermissionForms(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {

                ParentPortalBC piDetBC = new ParentPortalBC();
                return piDetBC.GetValuesFromParentPermissionForms(page, pageSize, sortBy, sortType, criteria);

            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }


        /* Start of Parent Leave Request Forms */

        public Dictionary<long, IList<ParentLeaveRequest>> GetValuesFromParentLeaveRequest(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {

                ParentPortalBC piDetBC = new ParentPortalBC();
                return piDetBC.GetValuesFromParentLeaveRequest(page, pageSize, sortBy, sortType, criteria);

            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        /* Parent Permission Forms */
        public long CreateOrUpdatePermissionForms(PermissionForms pf)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                ppBC.CreateOrUpdatePermissionForms(pf);
                return pf.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }


        /* Start of Notification */

        public long CreateOrUpdateNotification(Notification n)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                ppBC.CreateOrUpdateNotification(n);
                return n.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public long CreateShowNotification(ShowNotification sn)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                ppBC.CreateShowNotification(sn);
                return sn.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }


        public long CreateOrUpdateNoteAttachment(NoteAttachment na)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                ppBC.CreateOrUpdateNoteAttachment(na);
                return na.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public long GetMaxAttachmentId()
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                long Id=ppBC.GetMaxAttachmentId();
                return Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public long GetMaxNoteId()
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                long Id = ppBC.GetMaxNoteId();
                return Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public void UpdateNoteCount(long upd)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                ppBC.UpdateNoteCount(upd);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        
        public IList<NoteAttachment> GetDocListById(long dId)
        {
            try
            {
                ParentPortalBC piBC = new ParentPortalBC();
                return piBC.GetDocListById(dId);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }


        public Dictionary<long, IList<NoteAttachment>> GetDocumentsListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                return ppBC.GetDocumentsListWithPaging(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        

        public long CreateOrUpdateNoteEmailLog(NoteEmailLog el)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                ppBC.CreateOrUpdateNoteEmailLog(el);
                return el.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public void DeleteAttachment(long del)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                ppBC.DeleteAttachment(del);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<Notification>> GetValuesFromNotification(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {

                ParentPortalBC nBC = new ParentPortalBC();
                return nBC.GetValuesFromNotification(page, pageSize, sortBy, sortType, criteria);

            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public IList<Notification> GetNotificationListByParent(string PubGrp)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                return ppBC.GetNotificationListByParent(PubGrp);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public IList<ShowNotification> GetShowNotificationListbyNotePreId(long notepreid)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                return ppBC.GetShowNotificationListbyNotePreId(notepreid);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public bool DeleteExistingNewIdFromShowNotification(long NotePreId)
        {
            try
            {
                ParentPortalBC pBC = new ParentPortalBC();
                pBC.DeleteExistingNewIdFromShowNotification(NotePreId);
                return true;
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("Cannot insert duplicate key row in object 'dbo.QUEUE' with"))
                {
                    throw new FaultException("Cannot insert duplicate key in Queue Name.");
                }
                throw;
            }
            finally { }
        }

        public void SetValidNotificaionByExpireDate(long expID)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                ppBC.SetValidNotificaionByExpireDate(expID);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public void SetValidShowNotificationByExpireDate(long npID)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                ppBC.SetValidShowNotificaionByExpireDate(npID);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public void SetValidShowNotificationByPublishDate(long npID)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                ppBC.SetValidShowNotificationByPublishDate(npID);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }


        public void SetValidNotificationByPublishDate(long expID)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                ppBC.SetValidNotificationByPublishDate(expID);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        
        public bool DeleteNotification(long[] id)
        {
            try
            {
                ParentPortalBC pBC = new ParentPortalBC();
                pBC.DeleteNotification(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<Notification>> GetNoteTypesearchCriteriaAlias(int? page, int? pageSize, string sortBy, string sortType, string name, string[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                ParentPortalBC piBC = new ParentPortalBC();
                return piBC.GetNoteTypesearchCriteriaAlias(page, pageSize, sortBy, sortType, name, values, criteria, alias);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public IList<Notification> GetNotificationListById(long delId)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                return ppBC.GetNotificationListById(delId);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public bool DeleteNoteAttachmentByNotePreId(long NotePreId)
        {
            try
            {
                ParentPortalBC pBC = new ParentPortalBC();
                pBC.DeleteNoteAttachmentByNotePreId(NotePreId);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteShowNotificationByNotePreId(long NotePreId)
        {
            try
            {
                ParentPortalBC pBC = new ParentPortalBC();
                pBC.DeleteShowNotificationByNotePreId(NotePreId);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool SetShowNotificationByValidNote(long NotePreId,long nval)
        {
            try
            {
                ParentPortalBC pBC = new ParentPortalBC();
                pBC.SetShowNotificationByValidNote(NotePreId,nval);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Notification getNoteDetailsfromPreId(long nid)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                return ppBC.getNoteDetailsfromPreId(nid);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<ShowNotification>> GetShowNotificationSearchCriteriaAlias(int? page, int? pageSize, string sortBy, string sortType, string name, string[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                ParentPortalBC piBC = new ParentPortalBC();
                return piBC.GetShowNotificationSearchCriteriaAlias(page, pageSize, sortBy, sortType, name, values, criteria, alias);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }


        /* End of Notification */


        /* Start of PhotoGallery*/
        


        public long GetMaxPGId()
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                long Id = ppBC.GetMaxPGId();
                return Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public void UpdatePGCount(long upd)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                ppBC.UpdatePGCount(upd);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public long GetMaxPGPId()
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                long Id = ppBC.GetMaxPGPId();
                return Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public long CreateOrUpdatePhotoGallery(PhotoGallery pg)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                ppBC.CreateOrUpdatePhotoGallery(pg);
                return pg.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public long CreateOrUpdatePhotoGalleryPhoto(PhotoGalleryPhotos pgp)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                ppBC.CreateOrUpdatePhotoGalleryPhoto(pgp);
                return pgp.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public void DeleteUploadedFiles(long del)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                ppBC.DeleteUploadedFiles(del);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<PhotoGallery>> GetValuesFromPhotoGallery(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {

                ParentPortalBC nBC = new ParentPortalBC();
                return nBC.GetValuesFromPhotoGallery(page, pageSize, sortBy, sortType, criteria);

            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public bool DeletePhotoGallerybyId(long[] id)
        {
            try
            {
                ParentPortalBC pBC = new ParentPortalBC();
                pBC.DeletePhotoGallerybyId(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public PhotoGallery getPGDetailsfromDelId(long pgid)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                return ppBC.getPGDetailsfromDelId(pgid);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public PhotoGallery getPGDetailsfromId(long pgid)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                return ppBC.getPGDetailsfromId(pgid);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public bool DeletePhotoGalleryPhotosbyPGPreId(long[] id)
        {
            try
            {
                ParentPortalBC pBC = new ParentPortalBC();
                pBC.DeletePhotoGalleryPhotosbyPGPreId(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public IList<PhotoGalleryPhotos> getPGPreIdList(long pgpreId)
        {
            try
            {
                ParentPortalBC piBC = new ParentPortalBC();
                return piBC.getPGPreIdList(pgpreId);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<PhotoGalleryPhotos>> GetValuesFromPhotoGalleryPhotos(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {

                ParentPortalBC nBC = new ParentPortalBC();
                return nBC.GetValuesFromPhotoGalleryPhotos(page, pageSize, sortBy, sortType, criteria);

            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public bool DeletePhotosById(long id)
        {
            try
            {
                ParentPortalBC pBC = new ParentPortalBC();
                pBC.DeletePhotosById(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        
        }       
        /* End of PhotoGallery*/

        /* Start of Food Menu*/

        #region start food menu

        public Dictionary<long, IList<FoodMenuView>> GetValuesFromFoodMenu(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {

                ParentPortalBC nBC = new ParentPortalBC();
                return nBC.GetValuesFromFoodMenu(page, pageSize, sortBy, sortType, criteria);

            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<FoodNameMaster>> GetValuesFromFoodNameMaster(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {

                ParentPortalBC nBC = new ParentPortalBC();
                return nBC.GetValuesFromFoodNameMaster(page, pageSize, sortBy, sortType, criteria);

            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateFoodNameMater(FoodNameMaster fnm)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                ppBC.CreateOrUpdateFoodNameMater(fnm);
                return fnm.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteFoodNameMaterDetails(long[] id)
        {
            try
            {
                ParentPortalBC pBC = new ParentPortalBC();
                pBC.DeleteFoodNameMaterDetails(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<FoodNameMaster>> GetCampusBreakFastListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                return ppBC.GetCampusBreakFastListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                ParentPortalBC ppBC = new ParentPortalBC();
                ppBC.CreateOrUpdateFoodMenu(fm);
                return fm.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteFoodMenuDetails(long[] id)
        {
            try
            {
                ParentPortalBC pBC = new ParentPortalBC();
                pBC.DeleteFoodMenuDetails(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<FoodMenuView>> GetFoodMenuListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                return ppBC.GetFoodMenuListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                ParentPortalBC ppBC = new ParentPortalBC();
                ppBC.CreateOrUpdateFoodMenuByWeeks(fmw1);
                return fmw1.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        #endregion end food menu
        public bool UpdatePhotoGalleryNotificationStatus(string campus)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                ppBC.UpdatePhotoGalleryNotificationStatus(campus);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #region Added By Prabakaran
        public long CreateOrUpdatePhotoGalleryFolder(PhotoGalleryFolder pgf)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                ppBC.CreateOrUpdatePhotoGalleryFolder(pgf);
                return pgf.Folder_Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<PhotoGalleryFolder>> GetValuesFromPhotoGalleryFolder(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {

                ParentPortalBC nBC = new ParentPortalBC();
                return nBC.GetValuesFromPhotoGalleryFolder(page, pageSize, sortBy, sortType, criteria);

            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public PhotoGalleryFolder getPGFolderByFolderNamewithPgPreId(long pgid, string FolderName)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                return ppBC.getPGFolderByFolderNamewithPgPreId(pgid, FolderName);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long DeletePGPhotosByFolder_Id(long Id, long Folder_Id)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                return ppBC.DeletePGPhotosByFolder_Id(Id, Folder_Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public bool DeleteFolder(long Id)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                ppBC.DeleteFolder(Id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public PhotoGallery GetPhotoGalleryById(long Id)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                return ppBC.GetPhotoGalleryById(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public PhotoGallery getPGDetailsByPKId(long Id)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                return ppBC.getPGDetailsByPKId(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public PhotoGallery getPGDetailsByParentRefIdandPGPreId(long Id)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                return ppBC.getPGDetailsByParentRefIdandPGPreId(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public PhotoGallery getPGById(long pgid)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                return ppBC.getPGById(pgid);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public bool DeletePhotoGallery(PhotoGallery pg)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                ppBC.DeletePhotoGallery(pg);
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
                ParentPortalBC ppBC = new ParentPortalBC();
                ppBC.DeletePhotoGalleryPhotosByFolder_IdandId(Id, PGPreId);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool UpdatePhotoGalleryDetails(bool IsActive, long PGPreId)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                ppBC.UpdatePhotoGalleryDetails(IsActive, PGPreId);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public IList<PhotoGallery> getPGalleryPreIdList(long pgpreId)
        {
            try
            {
                ParentPortalBC piBC = new ParentPortalBC();
                return piBC.getPGalleryPreIdList(pgpreId);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion

        public IList<NoteAttachment> CreateOrUpdateNoteAttachmentsList(IList<NoteAttachment> naList)
        {
            try
            {
                ParentPortalBC ppBC = new ParentPortalBC();
                ppBC.CreateOrUpdateNoteAttachmentsList(naList);
                return naList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
    }
}
