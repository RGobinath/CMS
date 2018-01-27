using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.Component;
using TIPS.Entities.CommunictionEntities;
using TIPS.Entities.AdmissionEntities;
using TIPS.Entities;

namespace TIPS.ServiceContract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CommunicationService" in code, svc and config file together.
    public class CommunicationService
    {
        CommunicationBC com = new CommunicationBC();
        #region BulkEmailReg
        public Dictionary<long, IList<ComposeEmailInfo>> GetComposeEmailInfoListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.GetComposeEmailInfoListWithPagingAndCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<BulkEmailRequestWithCount_vw>> GetBulkEmailRequestWithCount_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.GetBulkEmailRequestWithCount_vwListWithPagingAndCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<BulkEmailRequest>> GetBulkEmailRegListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.GetBulkEmailRegListWithPagingAndCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        //Added newly for like and exact search
        public Dictionary<long, IList<BulkEmailRequest>> GetBulkEmailRegListWithLikeandExactPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria, Dictionary<string, object> Likecriteria)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.GetBulkEmailRegListWithLikeandExactPagingAndCriteria(page, pageSize, sortType, sortBy, criteria, Likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<RecipientsEmailInfo>> GetRecipientsEmailListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.GetRecipientsEmailListWithPagingAndCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public ComposeEmailInfo GetComposeEmailInfoById(long Id)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.GetComposeEmailInfoById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long CreateOrUpdateComposeEmailInfo(ComposeEmailInfo compemail)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                com.CreateOrUpdateComposeEmailInfo(compemail);
                return compemail.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }
        public IList<RecipientsEmailInfo> CreateOrUpdateRecipientsEmailList(IList<RecipientsEmailInfo> recipEmailInfo)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                com.CreateOrUpdateRecipientsEmailList(recipEmailInfo);
                return recipEmailInfo;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally { }
        }
        public IList<RecipientsEmailInfo> CreateOrUpdateRecipientsEmailListWithStatus(IList<RecipientsEmailInfo> recipEmailInfo, string EmailStatus)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                if (!string.IsNullOrEmpty(EmailStatus))
                {
                    foreach (RecipientsEmailInfo r in recipEmailInfo)
                    {
                        r.Status = EmailStatus;
                        r.RecipientsModifiedDate = DateTime.Now;
                    }
                    com.CreateOrUpdateRecipientsEmailList(recipEmailInfo);
                }
                return recipEmailInfo;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally { }
        }
        public void ClearSavedListInRecipientsEmailInfo(long ComposeId)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                com.ClearSavedListInRecipientsEmailInfo(ComposeId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public ComposeEmailInfo GetComposeMailInfoById(long Id)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.GetComposeMailInfoById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public RecipientsEmailInfo GetRecipientlistById(long ComposeId, long Id)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.GetRecipientlistById(ComposeId, Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long DeleteRecipientsList(IList<long> IdKeyValue, long ComposeId)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.DeleteRecipientsList(IdKeyValue, ComposeId);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long CreateOrUpdateRecipients(RecipientsEmailInfo rei)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.CreateOrUpdateRecipients(rei);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public IList<EmailAttachment> GetDocumentListByIdandApp(long dId,string AppName)
        {
            try
            {
                CommunicationBC CBc = new CommunicationBC();
                return CBc.GetDocumentListByIdandApp(dId,AppName);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        
        public void DeleteEmailAttachment(long del)
        {
            try
            {
                CommunicationBC ppBC = new CommunicationBC();
                ppBC.DeleteEmailAttachment(del);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public IList<EmailAttachment> CreateOrUpdateEmailAttachmentsList(IList<EmailAttachment> eaList)
        {
            try
            {
                com.CreateOrUpdateEmailAttachmentsList(eaList);
                return eaList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #endregion end

        #region BULK SMS Register Added By Micheal

        public long CreateOrUpdateComposeSMSInfo(ComposeSMSInfo SMSinfo)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                com.CreateOrUpdateComposeSMSInfo(SMSinfo);
                return SMSinfo.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<BulkSMSRequest_vw>> GetBulkSMSReqListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.GetBulkSMSReqListWithPagingAndCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public ComposeSMSInfo GetComposeSMSInfoById(long Id)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.GetComposeSMSInfoById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<ComposeSMSInfo>> GetSMSInfoListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.GetSMSInfoListWithPagingAndCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<SMSRecipientsInfo>> GetBulkSMSRecipListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.GetRecipSMSListWithPagingAndCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        /*newlly added*/
        public Dictionary<long, IList<SMSRecipientsInfo>> GetBulkSMSRecipLikeSearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.GetBulkSMSRecipLikeSearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }


        public IList<SMSRecipientsInfo> CreateOrUpdateSMSRecipientsList(IList<SMSRecipientsInfo> recipEmailInfo)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                com.CreateOrUpdateSMSRecipientsList(recipEmailInfo);
                return recipEmailInfo;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally { }
        }
        public void ClearSavedListInSMSRecipientsInfo(long SMSComposeId)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                com.ClearSavedListInSMSRecipientInfo(SMSComposeId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public SMSRecipientsInfo GetSMSREcipientDetailsById(long Id)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.GetSMSREcipientDetailsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long DeleteSMSRecipientsList(IList<long> PreRegNum, long ComposeId)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.DeleteSMSRecipientsList(PreRegNum, ComposeId);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long UpdateSMSRecipient(SMSRecipientsInfo smsRecip)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                com.UpdateSMSRecipient(smsRecip);
                return smsRecip.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public BulkSMSRequestReport_vw GetSMSReportInfoBySMSComposeId(long ComposeId)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.GetSMSReportInfoBySMSComposeId(ComposeId);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion

        #region Campus EmailId Master
        public Dictionary<long, IList<CampusEmailId>> GetCampusMailIdListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {

                return com.GetCampusMailIdListWithPagingAndCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion

        #region StaffNotification
        public Dictionary<long, IList<StaffRecipientsEmailInfo>> GetStaffEmailListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.GetStaffEmailListWithPagingAndCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public IList<StaffRecipientsEmailInfo> CreateOrUpdateStaffEmailList(IList<StaffRecipientsEmailInfo> StEmailInfo)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                com.CreateOrUpdateStaffEmailList(StEmailInfo);
                return StEmailInfo;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally { }
        }

        public long CreateOrUpdateStaffComposeEmailInfo(StaffComposeMailInfo compemail)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                com.CreateOrUpdateStaffComposeEmailInfo(compemail);
                return compemail.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public StaffComposeMailInfo GetStaffComposeEmailInfoById(long Id)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.GetStaffComposeEmailInfoById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public void DeleteSavedListInStaffmailInfo(long ComposeId)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                com.DeleteSavedListInStaffmailInfo(ComposeId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public IList<StaffRecipientsEmailInfo> CreateOrUpdateStaffEmailListWithStatus(IList<StaffRecipientsEmailInfo> recipEmailInfo, string EmailStatus)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                if (!string.IsNullOrEmpty(EmailStatus))
                {
                    foreach (StaffRecipientsEmailInfo r in recipEmailInfo)
                    {
                        r.Status = EmailStatus;
                        r.RecipientsModifiedDate = DateTime.Now;
                    }
                    com.CreateOrUpdateStaffEmailListWithStatus(recipEmailInfo);
                }
                return recipEmailInfo;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally { }
        }

        public Dictionary<long, IList<StaffComposeMailInfo>> GetStaffComposeMailInfoListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.GetStaffComposeMailInfoListWithPagingAndCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public StaffComposeSMSInfo GetStaffComposeSMSInfoById(long Id)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.GetStaffComposeSMSInfoById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateStaffComposeSMSInfo(StaffComposeSMSInfo SMSinfo)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                com.CreateOrUpdateStaffComposeSMSInfo(SMSinfo);
                return SMSinfo.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<StaffSMSRecipientsInfo>> GetStaffBulkSMSRecipLikeSearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.GetStaffBulkSMSRecipLikeSearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<StaffSMSRecipientsInfo>> GetStaffSMSListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.GetStaffSMSListWithPagingAndCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public IList<StaffSMSRecipientsInfo> CreateOrUpdateSMSStaffList(IList<StaffSMSRecipientsInfo> recipEmailInfo)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                com.CreateOrUpdateSMSStaffList(recipEmailInfo);
                return recipEmailInfo;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally { }
        }

        public void ClearSavedListInStaffSMSInfo(long SMSComposeId)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                com.ClearSavedListInStaffSMSInfo(SMSComposeId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public StaffSMSRecipientsInfo GetStaffSMSDetailsById(long Id)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.GetStaffSMSDetailsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long UpdateStaffSMSStatus(StaffSMSRecipientsInfo smsRecip)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                com.UpdateStaffSMSStatus(smsRecip);
                return smsRecip.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<StaffComposeSMSInfo>> GetStaffComposeSMSListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.GetStaffComposeSMSListWithPagingAndCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public StaffBulkSMSRequestReport_vw GetStaffSMSReportInfoBySMSComposeId(long ComposeId)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.GetStaffSMSReportInfoBySMSComposeId(ComposeId);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long DeleteStaffSMSRecipientsList(IList<long> PreRegNum, long ComposeId)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.DeleteStaffSMSRecipientsList(PreRegNum, ComposeId);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #region Added By Prabakaran
        public StaffRecipientsEmailInfo GetStaffRecipientlistById(long ComposeId, long Id)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.GetStaffRecipientlistById(ComposeId, Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long CreateOrUpdateStaffRecipients(StaffRecipientsEmailInfo rei)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.CreateOrUpdateStaffRecipients(rei);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long DeleteStaffRecipientsList(IList<long> PreRegNum, long ComposeId)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.DeleteStaffRecipientsList(PreRegNum, ComposeId);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        #endregion
        #region SMSCountReport By Prabakaran
        public Dictionary<long, IList<SMSCount_SP>> GetSMSCountDetailsListbySP(string Campus, DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.GetSMSCountDetailsListbySP(Campus, FromDate, ToDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Added By Prabakaran
        public Dictionary<long, IList<RecipientsEmailInfo>> GetRecipientsEmailListWithExactAndLikeSearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                CommunicationBC com = new CommunicationBC();
                return com.GetRecipientsEmailListWithExactAndLikeSearchCriteria(page, pageSize, sortBy, sortType, criteria,likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
    }
}
