using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersistenceFactory;
using TIPS.Entities.CommunictionEntities;
using TIPS.Entities.AdmissionEntities;
using TIPS.Entities;
using System.Data.SqlClient;

namespace TIPS.Component
{
    public class CommunicationBC
    {
        PersistenceServiceFactory PSF = null;
        public CommunicationBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }
        #region BulkEmailReg
        public Dictionary<long, IList<ComposeEmailInfo>> GetComposeEmailInfoListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                // return PSF.GetListWithEQSearchCriteriaCount<ComposeEmailInfo>(page, pageSize, sortBy, sortType, criteria);
                return PSF.GetListWithExactSearchCriteriaCount<ComposeEmailInfo>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<BulkEmailRequestWithCount_vw>> GetBulkEmailRequestWithCount_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {                
                return PSF.GetListWithExactSearchCriteriaCount<BulkEmailRequestWithCount_vw>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<BulkEmailRequest>> GetBulkEmailRegListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                // return PSF.GetListWithExactSearchCriteriaCount<BulkEmailRequest>(page, pageSize, sortBy, sortType, criteria);
                return PSF.GetListWithEQSearchCriteriaCount<BulkEmailRequest>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<RecipientsEmailInfo>> GetRecipientsEmailListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCountWithInCondition<RecipientsEmailInfo>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ComposeEmailInfo GetComposeEmailInfoById(long Id)
        {
            try
            {

                ComposeEmailInfo cEmailInfo = null;
                if (Id > 0)
                    cEmailInfo = PSF.Get<ComposeEmailInfo>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return cEmailInfo;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public long CreateOrUpdateComposeEmailInfo(ComposeEmailInfo compemail)
        {
            try
            {
                if (compemail != null)
                    PSF.SaveOrUpdate<ComposeEmailInfo>(compemail);
                else { throw new Exception("Id is required and it cannot be null.."); }
                return compemail.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<RecipientsEmailInfo> CreateOrUpdateRecipientsEmailList(IList<RecipientsEmailInfo> recipEmailInfo)
        {
            try
            {
                if (recipEmailInfo != null)
                    PSF.SaveOrUpdate<RecipientsEmailInfo>(recipEmailInfo);
                else { throw new Exception("Recipients Email is required and it cannot be null.."); }
                return recipEmailInfo;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void ClearSavedListInRecipientsEmailInfo(long ComposeId)
        {
            try
            {
                string query = "Delete from RecipientsEmailInfo where ComposeId='" + ComposeId + "'";
                PSF.ExecuteSql(query);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ComposeEmailInfo GetComposeMailInfoById(long Id)
        {
            try
            {

                ComposeEmailInfo compEmailId = null;
                if (Id > 0)
                    compEmailId = PSF.Get<ComposeEmailInfo>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return compEmailId;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public RecipientsEmailInfo GetRecipientlistById(long ComposeId, long Id)
        {
            try
            {

                RecipientsEmailInfo compEmailId = null;
                if (Id > 0)
                    compEmailId = PSF.Get<RecipientsEmailInfo>("ComposeId", ComposeId, "IdKeyValue", Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return compEmailId;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long DeleteRecipientsList(IList<long> IdKeyValue, long ComposeId)
        {
            try
            {
                if (IdKeyValue != null && ComposeId != null)
                {

                    foreach (long item in IdKeyValue)
                    {
                        RecipientsEmailInfo rei = PSF.Get<RecipientsEmailInfo>("IdKeyValue", item, "ComposeId", ComposeId);
                        if (rei != null)
                        {
                            PSF.Delete<RecipientsEmailInfo>(rei);
                        }
                    }
                }
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateRecipients(RecipientsEmailInfo rei)
        {
            try
            {

                if (rei != null)
                    PSF.SaveOrUpdate<RecipientsEmailInfo>(rei);
                else { throw new Exception("Recipients is required and it cannot be null.."); }
                return rei.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<EmailAttachment> GetDocumentListByIdandApp(long dId, string AppName)
        {
            try
            {
                return PSF.GetListByName<EmailAttachment>("from " + typeof(EmailAttachment) + " where PreRegNum ='" + dId + "' and AppName='" + AppName + "'");
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        
        public void DeleteEmailAttachment(long del)
        {
            try
            {
                string query = "delete from EmailAttachment where Id='" + del + "'";
                PSF.ExecuteSql(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<EmailAttachment> CreateOrUpdateEmailAttachmentsList(IList<EmailAttachment> eaList)
        {
            try
            {
                if (eaList != null)
                    PSF.SaveOrUpdate<EmailAttachment>(eaList);
                else { throw new Exception("Email Attachment is required and it cannot be null.."); }
                return eaList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion end

        #region BULK SMS Register
        public long CreateOrUpdateComposeSMSInfo(ComposeSMSInfo SMSinfo)
        {
            try
            {
                if (SMSinfo != null)
                    PSF.SaveOrUpdate<ComposeSMSInfo>(SMSinfo);
                else { throw new Exception("AdmissionManagement is required and it cannot be null.."); }
                return SMSinfo.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<BulkSMSRequest_vw>> GetBulkSMSReqListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<BulkSMSRequest_vw>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //New Mehtod for like and exact search 
        public Dictionary<long, IList<BulkEmailRequest>> GetBulkEmailRegListWithLikeandExactPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria, Dictionary<string, object> Likecriteria)
        {
            try
            {
                // return PSF.GetListWithExactSearchCriteriaCount<BulkEmailRequest>(page, pageSize, sortBy, sortType, criteria);
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<BulkEmailRequest>(page, pageSize, sortType, sortBy, criteria, Likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ComposeSMSInfo GetComposeSMSInfoById(long Id)
        {
            try
            {

                ComposeSMSInfo cEmailInfo = null;
                if (Id > 0)
                    cEmailInfo = PSF.Get<ComposeSMSInfo>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return cEmailInfo;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<ComposeSMSInfo>> GetSMSInfoListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                // return PSF.GetListWithEQSearchCriteriaCount<ComposeEmailInfo>(page, pageSize, sortBy, sortType, criteria);
                return PSF.GetListWithExactSearchCriteriaCount<ComposeSMSInfo>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<SMSRecipientsInfo>> GetRecipSMSListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCountWithInCondition<SMSRecipientsInfo>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public Dictionary<long, IList<SMSRecipientsInfo>> GetBulkSMSRecipLikeSearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<SMSRecipientsInfo>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<SMSRecipientsInfo> CreateOrUpdateSMSRecipientsList(IList<SMSRecipientsInfo> recipEmailInfo)
        {
            try
            {
                if (recipEmailInfo != null)
                    PSF.SaveOrUpdate<SMSRecipientsInfo>(recipEmailInfo);
                else { throw new Exception("Recipients Mobile is required and it cannot be null.."); }
                return recipEmailInfo;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void ClearSavedListInSMSRecipientInfo(long SMSComposeId)
        {
            try
            {
                string query = "Delete from SMSRecipientsInfo where SMSComposeId='" + SMSComposeId + "'";
                PSF.ExecuteSql(query);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public SMSRecipientsInfo GetSMSREcipientDetailsById(long Id)
        {
            try
            {

                SMSRecipientsInfo rsmsInfo = null;
                if (Id > 0)
                    rsmsInfo = PSF.Get<SMSRecipientsInfo>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return rsmsInfo;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public long DeleteSMSRecipientsList(IList<long> PreRegNum, long ComposeId)
        {
            try
            {
                if (PreRegNum != null && ComposeId > 0)
                {

                    foreach (long item in PreRegNum)
                    {
                        SMSRecipientsInfo SMSrei = PSF.Get<SMSRecipientsInfo>("IdKeyValue", item, "SMSComposeId", ComposeId);
                        if (SMSrei != null)
                        {
                            PSF.Delete<SMSRecipientsInfo>(SMSrei);
                            //string query = "Delete from SMSRecipientsInfo where PreRegNum='" + item + "'";
                            //PSF.ExecuteSql(query);
                        }
                    }
                }
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long UpdateSMSRecipient(SMSRecipientsInfo smsRecip)
        {
            try
            {
                if (smsRecip != null)
                    PSF.SaveOrUpdate<SMSRecipientsInfo>(smsRecip);
                else { }
                return smsRecip.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public BulkSMSRequestReport_vw GetSMSReportInfoBySMSComposeId(long ComposeId)
        {
            try
            {

                BulkSMSRequestReport_vw SMSRepInfo = null;
                SMSRepInfo = PSF.Get<BulkSMSRequestReport_vw>("SMSComposeId", ComposeId);
                return SMSRepInfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Campus EmailId Master
        public Dictionary<long, IList<CampusEmailId>> GetCampusMailIdListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                // return PSF.GetListWithEQSearchCriteriaCount<ComposeEmailInfo>(page, pageSize, sortBy, sortType, criteria);
                return PSF.GetListWithExactSearchCriteriaCount<CampusEmailId>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region StaffNotification
        public Dictionary<long, IList<StaffRecipientsEmailInfo>> GetStaffEmailListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCountWithInCondition<StaffRecipientsEmailInfo>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<StaffRecipientsEmailInfo> CreateOrUpdateStaffEmailList(IList<StaffRecipientsEmailInfo> StEmailInfo)
        {
            try
            {
                if (StEmailInfo != null)
                    PSF.SaveOrUpdate<StaffRecipientsEmailInfo>(StEmailInfo);
                else { throw new Exception("Recipients Email is required and it cannot be null.."); }
                return StEmailInfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long CreateOrUpdateStaffComposeEmailInfo(StaffComposeMailInfo compemail)
        {
            try
            {
                if (compemail != null)
                    PSF.SaveOrUpdate<StaffComposeMailInfo>(compemail);
                else { throw new Exception("Id is required and it cannot be null.."); }
                return compemail.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public StaffComposeMailInfo GetStaffComposeEmailInfoById(long Id)
        {
            try
            {

                StaffComposeMailInfo sEmailInfo = null;
                if (Id > 0)
                    sEmailInfo = PSF.Get<StaffComposeMailInfo>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return sEmailInfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeleteSavedListInStaffmailInfo(long ComposeId)
        {
            try
            {
                //string query = "Delete from RecipientsEmailInfo where ComposeId='" + ComposeId + "'";//Commented By Prabakaran
                string query = "Delete from StaffRecipientsEmailInfo where ComposeId='" + ComposeId + "'";
                PSF.ExecuteSql(query);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IList<StaffRecipientsEmailInfo> CreateOrUpdateStaffEmailListWithStatus(IList<StaffRecipientsEmailInfo> recipEmailInfo)
        {
            try
            {
                if (recipEmailInfo != null)
                    PSF.SaveOrUpdate<StaffRecipientsEmailInfo>(recipEmailInfo);
                else { throw new Exception("Recipients Email is required and it cannot be null.."); }
                return recipEmailInfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<StaffComposeMailInfo>> GetStaffComposeMailInfoListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                // return PSF.GetListWithEQSearchCriteriaCount<ComposeEmailInfo>(page, pageSize, sortBy, sortType, criteria);
                return PSF.GetListWithExactSearchCriteriaCount<StaffComposeMailInfo>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public StaffComposeSMSInfo GetStaffComposeSMSInfoById(long Id)
        {
            try
            {

                StaffComposeSMSInfo cEmailInfo = null;
                if (Id > 0)
                    cEmailInfo = PSF.Get<StaffComposeSMSInfo>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return cEmailInfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long CreateOrUpdateStaffComposeSMSInfo(StaffComposeSMSInfo SMSinfo)
        {
            try
            {
                if (SMSinfo != null)
                    PSF.SaveOrUpdate<StaffComposeSMSInfo>(SMSinfo);
                else { throw new Exception("AdmissionManagement is required and it cannot be null.."); }
                return SMSinfo.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StaffSMSRecipientsInfo>> GetStaffBulkSMSRecipLikeSearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<StaffSMSRecipientsInfo>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StaffSMSRecipientsInfo>> GetStaffSMSListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCountWithInCondition<StaffSMSRecipientsInfo>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<StaffSMSRecipientsInfo> CreateOrUpdateSMSStaffList(IList<StaffSMSRecipientsInfo> recipEmailInfo)
        {
            try
            {
                if (recipEmailInfo != null)
                    PSF.SaveOrUpdate<StaffSMSRecipientsInfo>(recipEmailInfo);
                else { throw new Exception("Recipients Mobile is required and it cannot be null.."); }
                return recipEmailInfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void ClearSavedListInStaffSMSInfo(long SMSComposeId)
        {
            try
            {
                string query = "Delete from StaffSMSRecipientsInfo where SMSComposeId='" + SMSComposeId + "'";
                PSF.ExecuteSql(query);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public StaffSMSRecipientsInfo GetStaffSMSDetailsById(long Id)
        {
            try
            {

                StaffSMSRecipientsInfo rsmsInfo = null;
                if (Id > 0)
                    rsmsInfo = PSF.Get<StaffSMSRecipientsInfo>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return rsmsInfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long UpdateStaffSMSStatus(StaffSMSRecipientsInfo smsRecip)
        {
            try
            {
                if (smsRecip != null)
                    PSF.SaveOrUpdate<StaffSMSRecipientsInfo>(smsRecip);
                else { }
                return smsRecip.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StaffComposeSMSInfo>> GetStaffComposeSMSListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCountWithInCondition<StaffComposeSMSInfo>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public StaffBulkSMSRequestReport_vw GetStaffSMSReportInfoBySMSComposeId(long ComposeId)
        {
            try
            {

                StaffBulkSMSRequestReport_vw SMSRepInfo = null;
                SMSRepInfo = PSF.Get<StaffBulkSMSRequestReport_vw>("SMSComposeId", ComposeId);
                return SMSRepInfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long DeleteStaffSMSRecipientsList(IList<long> PreRegNum, long ComposeId)
        {
            try
            {
                if (PreRegNum != null && ComposeId > 0)
                {

                    foreach (long item in PreRegNum)
                    {
                        StaffSMSRecipientsInfo SMSrei = PSF.Get<StaffSMSRecipientsInfo>("Id", item, "SMSComposeId", ComposeId);
                        if (SMSrei != null)
                        {
                            PSF.Delete<StaffSMSRecipientsInfo>(SMSrei);
                            //string query = "Delete from SMSRecipientsInfo where PreRegNum='" + item + "'";
                            //PSF.ExecuteSql(query);
                        }
                    }
                }
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region Added By Prabakaran
        public StaffRecipientsEmailInfo GetStaffRecipientlistById(long ComposeId, long Id)
        {
            try
            {

                StaffRecipientsEmailInfo compEmailId = null;
                if (Id > 0)
                    compEmailId = PSF.Get<StaffRecipientsEmailInfo>("ComposeId", ComposeId, "IdKeyValue", Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return compEmailId;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public long CreateOrUpdateStaffRecipients(StaffRecipientsEmailInfo rei)
        {
            try
            {

                if (rei != null)
                    PSF.SaveOrUpdate<StaffRecipientsEmailInfo>(rei);
                else { throw new Exception("Recipients is required and it cannot be null.."); }
                return rei.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long DeleteStaffRecipientsList(IList<long> PreRegNum, long ComposeId)
        {
            try
            {
                if (PreRegNum != null && ComposeId > 0)
                {

                    foreach (long item in PreRegNum)
                    {
                        StaffRecipientsEmailInfo srei = PSF.Get<StaffRecipientsEmailInfo>("IdKeyValue", item, "ComposeId", ComposeId);
                        if (srei != null)
                        {
                            PSF.Delete<StaffRecipientsEmailInfo>(srei);
                        }
                    }
                }
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #endregion

        #region SMSCountReport By Prabakaran
        public Dictionary<long, IList<SMSCount_SP>> GetSMSCountDetailsListbySP(string Campus, DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                return PSF.ExecuteStoredProcedurewithOptionalParametersByDictonary<SMSCount_SP>("GetSMSCountList",
                         new[] { new SqlParameter("Campus", Campus),
                             new SqlParameter("FromDate",FromDate),
                             new SqlParameter("ToDate",ToDate),                             
                    });                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region AddedByPrabakaran
        public Dictionary<long, IList<RecipientsEmailInfo>> GetRecipientsEmailListWithExactAndLikeSearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<RecipientsEmailInfo>(page, pageSize, sortType, sortBy, criteria,likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
