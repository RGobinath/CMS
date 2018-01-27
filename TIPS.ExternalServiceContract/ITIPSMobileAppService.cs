using System.ServiceModel;
using TIPS.Entities;
using TIPS.Entities.AdmissionEntities;
using TIPS.Entities.ParentPortalEntities;
using TIPS.Entities.Assess;
using System.Collections.Generic;

namespace TIPS.ExternalServiceContract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITIPSMobileAppService" in both code and config file together.
    [ServiceContract]
    public interface ITIPSMobileAppService
    {
        [OperationContract]
        bool ValidateWebServiceUser(string uid, string pwd, string roleCode);
        [OperationContract]
        bool Login(string UserId, string Password);
        [OperationContract]
        string[] GetIssueGroupMasterListWithPagingAndCriteria();
        [OperationContract]
        string[] GetIssueTypeMasterListWithPagingAndCriteria(string IssueGroup);
        [OperationContract]
        User LoginAndGetUser(string UserId, string Password);
        [OperationContract]
        CallManagementWrapper IssueManagementList(string userId, int rows, string sidx, string sord, int? page = 1);
        [OperationContract]
        long SaveOrUpdateCallManagement(CallManagement CallManagement, string TemplateName, string userId);
        [OperationContract]
        StudentTemplateView GetStudentDetails(string userId);
        [OperationContract]
        Notification GetParentNotificationByViewId(long viewId);
        [OperationContract]
        IList<NoteAttachment> GetDocListById(long dId);
        [OperationContract]
        Dictionary<long, IList<Notification>> NoteGeneral(string Campus);
        [OperationContract]
        Notification GetGeneralNotificationByNotePreId(long NotePreId);
        [OperationContract]
        IList<Notification> IndividualNotificationList(string UserId, string Campus);
        [OperationContract]
        Assess360StudentMarks Assess360(string UserId);
        [OperationContract]
        Dictionary<long, IList<NoteAttachment>> GetDocumentsListWithPaging( string id);
        [OperationContract]
        Dictionary<long, IList<NoteAttachmentView>> GetDocumentsListViewWithPaging(string notepreid);
    }
    
}
