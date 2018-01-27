using System.Collections.Generic;
using TIPS.Entities;
using TIPS.Entities.ParentPortalEntities;
using TIPS.Entities.AdmissionEntities;

namespace TIPS.ServiceContract
{
    public interface IParentPortalSC
    {
        FamilyDetails GetParentByUserId(string Id);
        
        IList<FamilyDetails> GetParentListByUserId(string userId);
        
        StudentTemplate GetStudentByParentUserId(long userId);
        
        Dictionary<long, IList<StudentTemplate>> GetStudentInformationListWithsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria);
        
        IList<StudentTemplate> GetStudentListByParentUserId(long userId);
        
        Dictionary<long, IList<ParentIssueType>> GetIssueGroupListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);
        
        long CreateOrUpdateParentIssueMgt(ParentIssueMgt cm);

        StudentTemplate GetStudentDetailsByStPreRegNum(long stNum);
        
        IList<StudentTemplate> GetStudentDetailsListByStPreRegNum(long stNum);

        CallManagement GetParentIssueByViewId(long viewId);

        PermissionForms GetParentPermissionByViewId(long viewId);

        //StudentTemplate GetStudentDetailsByStPreRegNum(string pName);

        Dictionary<long, IList<PermissionForms>> GetValuesFromParentPermissionForms(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria);

        Dictionary<long, IList<FamilyDetails>> GetFamilyDetailsListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria);

        long CreateOrUpdateNotification(Notification cm);

        long CreateOrUpdateNoteAttachment(NoteAttachment na);

        Dictionary<long, IList<Notification>> GetValuesFromNotification(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria);



        long CreateOrUpdatePhotoGalleryPhoto(PhotoGalleryPhotos pgp);
        
        void DeleteUploadedFiles(long del);

        long CreateOrUpdatePhotoGallery(PhotoGallery pg);

        long GetMaxPGPId();

        Dictionary<long, IList<PhotoGallery>> GetValuesFromPhotoGallery(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria);

        bool SetShowNotificationByValidNote(long NotePreId, long nval);

        void SetValidShowNotificationByPublishDate(long npID);

        void SetValidShowNotificationByExpireDate(long npID);

        bool DeleteExistingNewIdFromShowNotification(long NotePreId);

        Notification getNoteDetailsfromPreId(long nid);
        
        Dictionary<long, IList<ShowNotification>> GetShowNotificationSearchCriteriaAlias(int? page, int? pageSize, string sortType, string sortBy, string name, string[] values, Dictionary<string, object> criteria, string[] alias);


        long CreateOrUpdateNoteEmailLog(NoteEmailLog el);

        #region start food menu

        Dictionary<long, IList<FoodMenuView>> GetValuesFromFoodMenu(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria);

        Dictionary<long, IList<FoodNameMaster>> GetValuesFromFoodNameMaster(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria);

        long CreateOrUpdateFoodNameMater(FoodNameMaster fnm);

        long CreateOrUpdateFoodMenuByWeeks(FMWeeks fmw1);
        #endregion end food menu
    }
}
