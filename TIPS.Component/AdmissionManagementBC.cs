using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TIPS.Entities;
using TIPS.Entities.AdmissionEntities;
using PersistenceFactory;
using System.Collections;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using TIPS.Entities.EnquiryEntities;
using TIPS.Entities.StoreEntities;
using TIPS.Entities.CommunictionEntities;
using System.Net.Mail;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;

namespace TIPS.Component
{
    public class AdmissionManagementBC
    {
        PersistenceServiceFactory PSF = null;
        public AdmissionManagementBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }

        public long CreateOrUpdateAdmissionManagement(AdmissionManagement am)
        {
            try
            {
                if (am != null)
                    PSF.SaveOrUpdate<AdmissionManagement>(am);
                else { throw new Exception("AdmissionManagement is required and it cannot be null.."); }
                return am.ApplicationNo;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public AdmissionManagement GetAdmissionManagementById(long ApplicationNo)
        {
            try
            {

                AdmissionManagement AdmissionManagement = null;
                if (ApplicationNo > 0)
                    AdmissionManagement = PSF.Get<AdmissionManagement>(ApplicationNo);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return AdmissionManagement;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<AdmissionManagement>> GetAdmissionManagementListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<AdmissionManagement>> retValue = new Dictionary<long, IList<AdmissionManagement>>();
                return PSF.GetListWithExactSearchCriteriaCount<AdmissionManagement>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long CreateOrUpdateStudentDetails(StudentTemplate st)
        {
            try
            {
                if (st != null)
                    PSF.SaveOrUpdate<StudentTemplate>(st);
                else { throw new Exception("AdmissionManagement is required and it cannot be null.."); }
                return st.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long CreateOrUpdateTransferDetails(TransferDetails td)
        {
            try
            {
                if (td != null)
                    PSF.SaveOrUpdate<TransferDetails>(td);
                else { throw new Exception("AdmissionManagement is required and it cannot be null.."); }
                return td.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateEmailLog(EmailLog el)
        {
            try
            {
                if (el != null)
                    PSF.SaveOrUpdate<EmailLog>(el);
                else { throw new Exception("AdmissionManagement is required and it cannot be null.."); }
                return el.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StudentTemplate>> GetStudentDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<StudentTemplate>> retValue = new Dictionary<long, IList<StudentTemplate>>();
                return PSF.GetListWithExactSearchCriteriaCount<StudentTemplate>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StudentTemplateView>> GetStudentTemplateListWithPagingAndCriteriaWithAlias(int? page, int? pageSize, string sortby, string sortType, string name, string[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCountArray<StudentTemplateView>(page, pageSize, sortby, sortType, name, values, criteria, alias);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Exact Search
        public Dictionary<long, IList<StudentTemplateView>> GetStudentDetailsListExactWithPagingAndCriteriaWithAlias(int? page, int? pageSize, string sortby, string sortType, string name, string[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCountArrayExactSearch<StudentTemplateView>(page, pageSize, sortby, sortType, name, values, criteria, alias);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList GetStudentDetailsListWithQuery(string query)
        {
            try
            {
                //  IList<StudentTemplateView> list = PSF.ExecuteSql(query);
                //if (list != null && list[0] != null)
                //{
                //    return Convert.ToInt64(list[0].ToString()); //list[0] = "0";
                //}
                return PSF.ExecuteSql(query);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public Dictionary<long, IList<StudentTemplate>> GetStudentDetailsListWithEQsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<StudentTemplate>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public Dictionary<long, IList<StudentTemplateView>> GetStudentTemplateViewListWithEQsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<StudentTemplateView>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public Dictionary<long, IList<StudentTemplateView>> GetStudentTemplateViewListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<StudentTemplateView>> retValue = new Dictionary<long, IList<StudentTemplateView>>();
                return PSF.GetListWithExactSearchCriteriaCount<StudentTemplateView>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }


        public StudentTemplate GetStudentDetailsById(long Id)
        {
            try
            {
                StudentTemplate StudentTemplate = null;
                if (Id > 0)
                    StudentTemplate = PSF.Get<StudentTemplate>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return StudentTemplate;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<AddressDetails>> GetAddressDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<AddressDetails>> retValue = new Dictionary<long, IList<AddressDetails>>();
                return PSF.GetListWithExactSearchCriteriaCount<AddressDetails>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateUploadedFiles(UploadedFiles fu)
        {
            try
            {
                if (fu != null)
                    PSF.SaveOrUpdate<UploadedFiles>(fu);
                else { throw new Exception("AdmissionManagement is required and it cannot be null.."); }
                return fu.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<UploadedFiles>> GetUploadedFilesListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<UploadedFiles>> retValue = new Dictionary<long, IList<UploadedFiles>>();
                return PSF.GetListWithExactSearchCriteriaCount<UploadedFiles>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public Dictionary<long, IList<UploadedFilesView>> GetUploadedFilesViewListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<UploadedFilesView>> retValue = new Dictionary<long, IList<UploadedFilesView>>();
                return PSF.GetListWithExactSearchCriteriaCount<UploadedFilesView>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<UploadedFilesView>> GetUploadedFilesViewListWithPagingAndCriteriaWithIn(int? page, int? pageSize, string sortby, string sortType, string name, int[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                return PSF.GetListWithInSearchCriteriaCountArray<UploadedFilesView>(page, pageSize, sortby, sortType, name, values, criteria, alias);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateUploadedFilesView(UploadedFilesView fu)
        {
            try
            {
                if (fu != null)
                    PSF.SaveOrUpdate<UploadedFilesView>(fu);
                else { throw new Exception("AdmissionManagement is required and it cannot be null.."); }
                return fu.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IList<UploadedFiles> GetUploadedFilesByPreRegNum(long PreRegNum, string flag)
        {
            try
            {
                UploadedFiles UploadedFiles = null;
                IList<UploadedFiles> FileUploadList = null;
                if (PreRegNum > 0)
                {
                    DataTable retValue;
                    if (flag == "no")
                    {
                        retValue = PSF.ExecuteSqlUsingSQLCommand("Select *,cast(DocumentData as varbinary(max)) as PictureData from UploadedFiles where PreRegNum=" + PreRegNum + "");
                    }
                    else
                    {
                        retValue = PSF.ExecuteSqlUsingSQLCommand("Select *,cast(DocumentData as varbinary(max)) as PictureData from UploadedFiles where Id=" + PreRegNum + "");
                    }


                    if (retValue != null && retValue.Rows != null && retValue.Rows.Count > 0)
                    {
                        FileUploadList = new List<UploadedFiles>();
                        foreach (DataRow row in retValue.Rows)
                        {
                            UploadedFiles.Id = Convert.ToInt64(row["Id"]);
                            UploadedFiles.DocumentData = ObjectToByteArray(row["PictureData"]);
                            UploadedFiles.DocumentName = row["DocumentName"].ToString();
                            UploadedFiles.DocumentType = row["DocumentType"].ToString();
                            //UploadedFiles.DocumentSize = UploadedFiles.DocumentData.Length.ToString();
                            UploadedFiles.PreRegNum = Convert.ToInt64(row["PreRegNum"]);
                            FileUploadList.Add(UploadedFiles);
                        }
                    }
                }
                else { throw new Exception("PreRegNum is required and it cannot be 0"); }
                //return FileUpload1;
                return FileUploadList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        public UploadedFiles GetUploadedFilesById(long Id)
        {
            try
            {

                UploadedFiles FileUpload1 = null;
                if (Id > 0)
                    FileUpload1 = PSF.Get<UploadedFiles>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return FileUpload1;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public UploadedFiles GetUploadedFilesByIdWithDirectQuery(long Id)
        {
            try
            {
                UploadedFiles UploadedFiles = null;
                UploadedFiles FileUpload1 = null;
                if (Id > 0)
                {
                    DataTable retValue = PSF.ExecuteSqlUsingSQLCommand("Select * from UploadedFiles where Id=" + Id + "");
                    if (retValue != null && retValue.Rows != null && retValue.Rows.Count > 0)
                    {
                        UploadedFiles = new UploadedFiles();
                        UploadedFiles.Id = Id;
                        UploadedFiles.DocumentData = retValue.Rows[0]["DocumentData"] as byte[];
                        UploadedFiles.DocumentName = retValue.Rows[0]["DocumentName"].ToString();
                        UploadedFiles.DocumentType = retValue.Rows[0]["DocumentType"].ToString();
                        UploadedFiles.DocumentSize = UploadedFiles.DocumentData.Length.ToString();
                        UploadedFiles.PreRegNum = Convert.ToInt64(retValue.Rows[0]["PreRegNum"]);
                    }
                    FileUpload1 = PSF.Get<UploadedFiles>(Id);
                }
                else { throw new Exception("Id is required and it cannot be 0"); }
                //return FileUpload1;
                return UploadedFiles;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteUploadedFiles(long id)
        {
            try
            {
                UploadedFiles UploadedFiles = PSF.Get<UploadedFiles>(id);
                PSF.Delete<UploadedFiles>(UploadedFiles);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteUploadedFiles(long[] id)
        {
            try
            {
                IList<UploadedFilesView> UploadedFilesView = PSF.GetListByIds<UploadedFilesView>(id);
                PSF.DeleteAll<UploadedFilesView>(UploadedFilesView);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public long CreateOrUpdatePreRegDetails(PreRegDetails pd)
        {
            try
            {
                if (pd != null)
                    PSF.SaveOrUpdate<PreRegDetails>(pd);
                else { throw new Exception("AdmissionManagement is required and it cannot be null.."); }
                return pd.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<PreRegDetails>> GetPreRegDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<PreRegDetails>> retValue = new Dictionary<long, IList<PreRegDetails>>();
                return PSF.GetListWithExactSearchCriteriaCount<PreRegDetails>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PreRegDetails GetPreRegDetailsById(long Id)
        {
            try
            {

                PreRegDetails PreRegDetails = null;
                if (Id > 0)
                    PreRegDetails = PSF.Get<PreRegDetails>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return PreRegDetails;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long CreateOrUpdateFamilyDetails(FamilyDetails fd)
        {
            try
            {
                if (fd != null)
                    PSF.SaveOrUpdate<FamilyDetails>(fd);
                else { throw new Exception("AdmissionManagement is required and it cannot be null.."); }
                return fd.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long CreateOrUpdateSportsDetails(Sports sp)
        {
            try
            {
                if (sp != null)
                    PSF.SaveOrUpdate<Sports>(sp);
                else { throw new Exception("AdmissionManagement is required and it cannot be null.."); }
                return sp.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<Sports>> GetSportsDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<Sports>> sportsdet = new Dictionary<long, IList<Sports>>();
                return PSF.GetListWithEQSearchCriteriaCount<Sports>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<Sports>> GetSportsDetailsListWithSearchCriteriaCountArray(int? page, int? pageSize, string sortby, string sortType, string name, string[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                Dictionary<long, IList<Sports>> sportsdet = new Dictionary<long, IList<Sports>>();
                return PSF.GetListWithSearchCriteriaCountArray<Sports>(page, pageSize, sortby, sortType, name, values, criteria, alias);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteSportsDetails(long id)
        {
            try
            {
                Sports SportsDetails = PSF.Get<Sports>(id);
                PSF.Delete<Sports>(SportsDetails);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteSportsDetails(long[] id)
        {
            try
            {
                IList<Sports> SportsDetails = PSF.GetListByIds<Sports>(id);
                PSF.DeleteAll<Sports>(SportsDetails);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<FamilyDetails>> GetFamilyDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<FamilyDetails>> familydet = new Dictionary<long, IList<FamilyDetails>>();
                // return PSF.GetListWithExactSearchCriteriaCount<FamilyDetails>(page, pageSize, sortType, sortBy, criteria);
                return PSF.GetListWithEQSearchCriteriaCount<FamilyDetails>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public FamilyDetails GetFamilyDetailsById(long Id)
        {
            try
            {
                FamilyDetails FamilyDetails = null;
                if (Id > 0)
                    FamilyDetails = PSF.Get<FamilyDetails>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return FamilyDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteAttachment(long id)
        {
            try
            {
                EmailAttachment emailattachment = PSF.Get<EmailAttachment>(id);
                PSF.Delete<EmailAttachment>(emailattachment);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteAttachment(long[] id)
        {
            try
            {
                IList<EmailAttachment> emailattachment = PSF.GetListByIds<EmailAttachment>(id);
                PSF.DeleteAll<EmailAttachment>(emailattachment);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteFamilyDetails(long id)
        {
            try
            {
                FamilyDetails FamilyDetails = PSF.Get<FamilyDetails>(id);
                PSF.Delete<FamilyDetails>(FamilyDetails);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteFamilyDetails(long[] id)
        {
            try
            {
                IList<FamilyDetails> FamilyDetails = PSF.GetListByIds<FamilyDetails>(id);
                PSF.DeleteAll<FamilyDetails>(FamilyDetails);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdatePastSchoolDetails(PastSchoolDetails pd)
        {
            try
            {
                if (pd != null)
                    PSF.SaveOrUpdate<PastSchoolDetails>(pd);
                else { throw new Exception("AdmissionManagement is required and it cannot be null.."); }
                return pd.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<PastSchoolDetails>> GetPastSchoolDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<PastSchoolDetails>> pastdet = new Dictionary<long, IList<PastSchoolDetails>>();
                return PSF.GetListWithExactSearchCriteriaCount<PastSchoolDetails>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeletePastSchoolDetails(long id)
        {
            try
            {
                PastSchoolDetails PastSchoolDetails = PSF.Get<PastSchoolDetails>(id);
                PSF.Delete<PastSchoolDetails>(PastSchoolDetails);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeletePastSchoolDetails(long[] id)
        {
            try
            {
                IList<PastSchoolDetails> PastSchoolDetails = PSF.GetListByIds<PastSchoolDetails>(id);
                PSF.DeleteAll<PastSchoolDetails>(PastSchoolDetails);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteTransferDetails(long id)
        {
            try
            {
                TransferDetails TransferDetails = PSF.Get<TransferDetails>(id);
                PSF.Delete<TransferDetails>(TransferDetails);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteTransferDetails(long[] id)
        {
            try
            {
                IList<TransferDetails> TransferDetails = PSF.GetListByIds<TransferDetails>(id);
                PSF.DeleteAll<TransferDetails>(TransferDetails);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdatePaymentDetails(PaymentDetails pd)
        {
            try
            {
                if (pd != null)
                    PSF.SaveOrUpdate<PaymentDetails>(pd);
                else { throw new Exception("AdmissionManagement is required and it cannot be null.."); }
                return pd.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<PaymentDetails>> GetPaymentDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<PaymentDetails>> pmtdet = new Dictionary<long, IList<PaymentDetails>>();
                return PSF.GetListWithExactSearchCriteriaCount<PaymentDetails>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public bool DeletePaymentDetails(long id)
        {
            try
            {
                PaymentDetails PaymentDetails = PSF.Get<PaymentDetails>(id);
                PSF.Delete<PaymentDetails>(PaymentDetails);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeletePaymentDetails(long[] id)
        {
            try
            {
                IList<PaymentDetails> PaymentDetails = PSF.GetListByIds<PaymentDetails>(id);
                PSF.DeleteAll<PaymentDetails>(PaymentDetails);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateApproveAssign(ApproveAssign aa)
        {
            try
            {
                if (aa != null)
                    PSF.SaveOrUpdate<ApproveAssign>(aa);
                else { throw new Exception("AdmissionManagement is required and it cannot be null.."); }
                return aa.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<ApproveAssign>> GetApproveAssignListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<ApproveAssign>> apasign = new Dictionary<long, IList<ApproveAssign>>();
                return PSF.GetListWithExactSearchCriteriaCount<ApproveAssign>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteApproveAssign(long id)
        {
            try
            {
                ApproveAssign ApproveAssign = PSF.Get<ApproveAssign>(id);
                PSF.Delete<ApproveAssign>(ApproveAssign);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteApproveAssign(long[] id)
        {
            try
            {
                IList<ApproveAssign> ApproveAssign = PSF.GetListByIds<ApproveAssign>(id);
                PSF.DeleteAll<ApproveAssign>(ApproveAssign);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateIdGeneration(IdGeneration ig)
        {
            try
            {
                if (ig != null)
                    PSF.SaveOrUpdate<IdGeneration>(ig);
                else { throw new Exception("AdmissionManagement is required and it cannot be null.."); }
                return ig.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<IdGeneration>> GetIdGenerationListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<IdGeneration>> idgen = new Dictionary<long, IList<IdGeneration>>();
                return PSF.GetListWithExactSearchCriteriaCount<IdGeneration>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteIdGeneration(long id)
        {
            try
            {
                IdGeneration IdGeneration = PSF.Get<IdGeneration>(id);
                PSF.Delete<IdGeneration>(IdGeneration);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteIdGeneration(long[] id)
        {
            try
            {
                IList<IdGeneration> IdGeneration = PSF.GetListByIds<IdGeneration>(id);
                PSF.DeleteAll<IdGeneration>(IdGeneration);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long StudentIdCount(string grade, string feeStrutYear, string campus)
        {
            try
            {
                string query = "select MAX(SUBSTRING(NEWID,7,10)) from StudentTemplate  where FeeStructYear='" + feeStrutYear + "' And Campus in (" + campus + ")";
                //Grade='" + grade + "' And
                IList list = PSF.ExecuteSql(query);
                if (list != null && list[0] != null)
                {
                    return Convert.ToInt64(list[0].ToString()); //list[0] = "0";
                }
                else return 0;// Convert.ToInt64(list[0].ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long ErodeStudentIdCount(string grade, string campus, string FeeStructYear)
        {
            try
            {
                string query = "select MAX(CAST(RIGHT(NEWID,4) as bigint)) from StudentTemplate where Campus='" + campus + "' and Grade='" + grade + "'and FeeStructYear='" + FeeStructYear + "' ";
                // and Admissionstatus='Registered' ";  // To avoid duplicate NewId (discussed with JP and Swathi)
                //Grade='" + grade + "' And
                IList list = PSF.ExecuteSql(query);
                if (list != null && list[0] != null)
                {
                    return Convert.ToInt64(list[0].ToString()); //list[0] = "0";
                }
                else return 0;// Convert.ToInt64(list[0].ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public Dictionary<long, IList<GradeMaster>> StudentIdCount(string grade, string feeStrutYear)
        //{
        //    try
        //    {
        //        string query = "select MAX(SUBSTRING(NEWID,7,10)) from StudentTemplate  where Grade='" + grade + "' And FeeStructYear='" + feeStrutYear + "'";
        //        //IList list = PSF.ExecuteSql(query);
        //       // return PSF.ExecuteSql(query);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}

        public Dictionary<long, IList<StudentDetailsExport>> GetStudentExportListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<StudentDetailsExport>> retValue = new Dictionary<long, IList<StudentDetailsExport>>();
                return PSF.GetListWithExactSearchCriteriaCount<StudentDetailsExport>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StudentDetailsExport>> GetStudentExportListWithEQsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<StudentDetailsExport>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        //public Dictionary<long, IList<StudentTemplate1>> GetStudentTemplate1ListWithEQsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        //{
        //    try
        //    {
        //        return PSF.GetListWithEQSearchCriteriaCount<StudentTemplate1>(page, pageSize, sortType, sortBy, criteria);
        //    }
        //    catch (Exception) { throw; }
        //    finally { if (PSF != null) PSF.Dispose(); }
        //}
        //public Dictionary<long, IList<StudentTemplate1>> GetStudentTemplate1ListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        //{
        //    try
        //    {
        //        Dictionary<long, IList<StudentTemplate1>> retValue = new Dictionary<long, IList<StudentTemplate1>>();
        //        return PSF.GetListWithExactSearchCriteriaCount<StudentTemplate1>(page, pageSize, sortBy, sortType, criteria);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public Dictionary<long, IList<TransferDetails>> GetTransferDetailsListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<TransferDetails>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public long CreateOrUpdateEmailAttachment(EmailAttachment ea)
        {
            try
            {
                if (ea != null)
                    PSF.SaveOrUpdate<EmailAttachment>(ea);
                else { throw new Exception("AdmissionManagement is required and it cannot be null.."); }
                return ea.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<EmailAttachment>> GetEmailAttachmentListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<EmailAttachment>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public long GetMaxAttachmentId()
        {
            try
            {
                string query = "select MAX(Id) from EmailAttachment";
                IList list = PSF.ExecuteSql(query);
                if (list != null && list[0] != null)
                {
                    return Convert.ToInt64(list[0].ToString()); //list[0] = "0";
                }
                else return 0;// Convert.ToInt64(list[0].ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public StudentTemplate GetStudentDetailsByAppNo(string appno)
        {
            try
            {
                StudentTemplate StudentTemplate = null;
                if (!string.IsNullOrEmpty(appno))
                    StudentTemplate = PSF.Get<StudentTemplate>(appno);
                else { throw new Exception("appno is required and it cannot be empty"); }
                return StudentTemplate;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long CreateOrUpdateSMSLog(SMS sms)
        {
            try
            {
                if (sms != null)
                    PSF.SaveOrUpdate<SMS>(sms);
                else { throw new Exception("AdmissionManagement is required and it cannot be null.."); }
                return sms.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<SMSTemplate>> GetSMSTemplateListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<SMSTemplate>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public Dictionary<long, IList<AdmissionCountReport_vw>> GetAdmissionCountReport_vwWithEQsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<AdmissionCountReport_vw>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public Dictionary<long, IList<EmailLog>> GetEmailLogListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchWithArrayCriteriaCount<EmailLog>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public Dictionary<long, IList<CampusEmailId>> GetCampusEmailIdListWithPagingAndCriteriaWithAlias(int? page, int? pageSize, string sortby, string sortType, string name, string[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCountArray<CampusEmailId>(page, pageSize, sortby, sortType, name, values, criteria, alias);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<SMS>> GetSMSLogListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<SMS>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public long CreateOrUpdateFollowupDetails(FollowupDetails fw)
        {
            try
            {
                if (fw != null)
                    PSF.SaveOrUpdate<FollowupDetails>(fw);
                else { throw new Exception("AdmissionManagement is required and it cannot be null.."); }
                return fw.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<FollowupDetails>> GetFollowupDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<FollowupDetails>> fwpdet = new Dictionary<long, IList<FollowupDetails>>();
                return PSF.GetListWithExactSearchCriteriaCount<FollowupDetails>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteFollowupDetails(long id)
        {
            try
            {
                FollowupDetails FollowupDetails = PSF.Get<FollowupDetails>(id);
                PSF.Delete<FollowupDetails>(FollowupDetails);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteFollowupDetails(long[] id)
        {
            try
            {
                IList<FollowupDetails> FollowupDetails = PSF.GetListByIds<FollowupDetails>(id);
                PSF.DeleteAll<FollowupDetails>(FollowupDetails);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateStudentStatus(StudentTemplateStatus st)
        {
            try
            {
                if (st != null)
                    PSF.SaveOrUpdate<StudentTemplateStatus>(st);
                else { throw new Exception("AdmissionManagement is required and it cannot be null.."); }
                return st.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetDashBoardStudentList(string StrSql) //Administrative
        {

            try
            {
                return PSF.ExecuteSqlUsingSQLCommand(StrSql);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public StudentTemplate GetStudentDetailsByNewId(string NewId)
        {
            try
            {
                return PSF.Get<StudentTemplate>("NewId", NewId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public StudentTemplate GetStudentDetailsByPreRegNo(long PreRegNo)
        {
            try
            {
                return PSF.Get<StudentTemplate>("PreRegNum", PreRegNo);
            }
            catch (Exception)
            {
                throw;
            }
        }


        #region StudentTemplateAndFamilyDetails_vw
        public Dictionary<long, IList<StudentTemplateAndFamilyDetails_vw>> StudentTemplateAndFamilyDetails_vwListWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<StudentTemplateAndFamilyDetails_vw>(page, pageSize, sortType, sortby, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateFamilyDatails(FamilyDetails FD)
        {
            try
            {
                if (FD != null)
                    PSF.SaveOrUpdate<FamilyDetails>(FD);
                else { throw new Exception("AdmissionManagement is required and it cannot be null.."); }
                return FD.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public long CreateOrUpdateStudentTemplateDetails(StudentTemplate ST)
        {
            try
            {
                if (ST != null)
                    PSF.SaveOrUpdate<StudentTemplate>(ST);
                else { throw new Exception("AdmissionManagement is required and it cannot be null.."); }
                return ST.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public StudentTemplate GetStudentTemplateDetailsById(long Id)
        {
            try
            {

                StudentTemplate StudentTemplateDetails = null;
                if (Id > 0)
                    StudentTemplateDetails = PSF.Get<StudentTemplate>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return StudentTemplateDetails;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region
        public long SaveOrUpdateBulkPromTransferRequestDetails(BulkPromTransferRequestDetails bPTRD)
        {
            try
            {
                if (bPTRD != null)
                    PSF.SaveOrUpdate<BulkPromTransferRequestDetails>(bPTRD);
                else { throw new Exception("Bulk Promotion Id is required and it cannot be null.."); }
                return bPTRD.BulkPromTransferRequestId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<BulkPromTransferRequestDetails>> GetBulkPromTransferRequestDetailsListWithEQsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<BulkPromTransferRequestDetails>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public BulkPromTransferRequestDetails GetBulkPromTransferRequestDetailsById(long Id)
        {
            try
            {

                BulkPromTransferRequestDetails cEmailInfo = null;
                if (Id > 0)
                    cEmailInfo = PSF.Get<BulkPromTransferRequestDetails>("BulkPromTransferRequestId", Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return cEmailInfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IList<BulkPromTransfer> CreateOrUpdateBulkPromTransferList(IList<BulkPromTransfer> recipEmailInfo)
        {
            try
            {
                if (recipEmailInfo != null)
                    PSF.SaveOrUpdate<BulkPromTransfer>(recipEmailInfo);
                else { throw new Exception("Recipients Email is required and it cannot be null.."); }
                return recipEmailInfo;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<BulkPromTransfer>> GetBulkPromTransferListWithEQsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<BulkPromTransfer>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public BulkPromTransfer GetBulkPromTransferByIds(long PreRegNum, long BulkPromTransferRequestId)
        {
            try
            {

                BulkPromTransfer cEmailInfo = null;
                if (PreRegNum > 0 && BulkPromTransferRequestId > 0)
                    cEmailInfo = PSF.Get<BulkPromTransfer>("PreRegNum", PreRegNum, "BulkPromTransferRequestId", BulkPromTransferRequestId);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return cEmailInfo;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public long SaveOrUpdateBulkPromTransfer(BulkPromTransfer bPTRD)
        {
            try
            {
                if (bPTRD != null)
                    PSF.SaveOrUpdate<BulkPromTransfer>(bPTRD);
                else { throw new Exception("Bulk Promotion Id is required and it cannot be null.."); }
                return bPTRD.BulkPromTransferRequestId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public Dictionary<long, IList<StudentTemplateAndFamilyDetails_vw>> StudentTemplateAndFamilyDetails_vwListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<StudentTemplateAndFamilyDetails_vw>(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StudentTemplate>> GetStudentDetailsListWithLikesearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<StudentTemplate>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public StudentTemplate GetStudentDetailsByPreRegNum(long PreRegNum)
        {
            try
            {
                StudentTemplate StudentTemplate = null;
                if (PreRegNum > 0)
                    StudentTemplate = PSF.Get<StudentTemplate>("PreRegNum", PreRegNum);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return StudentTemplate;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region "Windowservice for FollowUpdetails Remaindermail"

        public bool SendEnquiryRemainderMailtoAdmin()
        {
            try
            {
                bool RetValue = false;
                string RecipientInfo = "", Subject = "", MailBody = "", PreRegNums = "";
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                Dictionary<long, IList<CampusMaster>> CampusList = PSF.GetListWithExactSearchCriteriaCount<CampusMaster>(0, 99999, string.Empty, string.Empty, criteria);
                if (CampusList != null && CampusList.Count > 0 && CampusList.FirstOrDefault().Key != 0 && CampusList.FirstOrDefault().Value != null)
                {
                    foreach (var Campusitem in CampusList.FirstOrDefault().Value)
                    {
                        FollowUpRemainderTracker FollowupTracker = PSF.Get<FollowUpRemainderTracker>("Campus", Campusitem.Name, "CheckDate", DateTime.Now.ToString("dd/MM/yyyy"));
                        if (FollowupTracker == null || FollowupTracker != null && FollowupTracker.IsSent == false)
                        {
                            criteria.Clear();
                            string[] dates = new string[2];
                            dates[0] = DateTime.Now.ToString("dd/MM/yyyy");
                            dates[1] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                            criteria.Add("FollowupDate", dates);
                            criteria.Add("Campus", Campusitem.Name);
                            Dictionary<long, IList<FollowupDetailsView_vw>> GetFollowUpDetails = PSF.GetListWithEQSearchCriteriaCount<FollowupDetailsView_vw>(0, 99999, string.Empty, string.Empty, criteria);
                            if (GetFollowUpDetails != null && GetFollowUpDetails.Count > 0 && GetFollowUpDetails.FirstOrDefault().Key != 0 && GetFollowUpDetails.FirstOrDefault().Value != null)
                            {
                                MailBody = GetMailTemplateBody();
                                RecipientInfo = "Dear Team,";
                                Subject = "Reminded you for the following Enquiries";
                                int Tcount = 0, Ncount = 0;
                                string TdayFollowup = "You are Reminded of the following Enquiries today.<br /><br /><table class='tftable' border='1'><tr><th>S.No</th><th>PreRegistration Number</th><th>Student Name</th><th>Campus</th><th>Grade</th><th>Admission Status</th><th>Phone Number</th><th>EmailId</th><th>Followup Date</th></tr>";
                                string NextFollowup = "You are Reminded of the following Enquiries to follow tomorrow.<br /><br /><table class='tftable' border='1'><tr><th>S.No</th><th>PreRegistration Number</th><th>Student Name</th><th>Campus</th><th>Grade</th><th>Admission Status</th><th>Phone Number</th><th>EmailId</th><th>Followup Date</th></tr>";
                                foreach (var Followupitem in GetFollowUpDetails.FirstOrDefault().Value)
                                {
                                    PreRegNums = PreRegNums + Followupitem.PreRegNum + ",";
                                    if (Followupitem.FollowupDate == DateTime.Now.ToString("dd/MM/yyyy")) { Tcount++; TdayFollowup = TdayFollowup + "<tr><td>" + Tcount + "</td><td>" + Followupitem.PreRegNum + "</td><td>" + Followupitem.Name + "</td><td>" + Followupitem.Campus + "</td><td>" + Followupitem.Grade + "</td><td>" + Followupitem.AdmissionStatus + "</td><td>" + Followupitem.PhNumber + "</td><td>" + Followupitem.EmailId + "</td><td>" + Followupitem.FollowupDate + "</td></tr>"; }
                                    else { Ncount++; NextFollowup = NextFollowup + "<tr><td>" + Ncount + "</td><td>" + Followupitem.PreRegNum + "</td><td>" + Followupitem.Name + "</td><td>" + Followupitem.Campus + "</td><td>" + Followupitem.Grade + "</td><td>" + Followupitem.AdmissionStatus + "</td><td>" + Followupitem.PhNumber + "</td><td>" + Followupitem.EmailId + "</td><td>" + Followupitem.FollowupDate + "</td></tr>"; }
                                }
                                TdayFollowup = TdayFollowup + "</table><br/><br/>";
                                NextFollowup = NextFollowup + "</table><br/><br/>";
                                string BodyofMail = "";
                                if (Tcount > 0 && Ncount > 0)
                                {
                                    BodyofMail = BodyofMail + TdayFollowup + NextFollowup;
                                    RetValue = SendMailFollowupdetailstoGRL(BodyofMail, Campusitem.Name, MailBody, Subject, RecipientInfo, "Management");
                                }
                                else if (Tcount == 0 && Ncount > 0)
                                {
                                    BodyofMail = BodyofMail + NextFollowup;
                                    RetValue = SendMailFollowupdetailstoGRL(BodyofMail, Campusitem.Name, MailBody, Subject, RecipientInfo, "Management");
                                }
                                else if (Ncount == 0 && Tcount > 0)
                                {
                                    BodyofMail = BodyofMail + TdayFollowup;
                                    RetValue = SendMailFollowupdetailstoGRL(BodyofMail, Campusitem.Name, MailBody, Subject, RecipientInfo, "Management");
                                }
                                else { }
                                if (RetValue == true)
                                {
                                    FollowupRemainderTrackerUpdate("Daily", PreRegNums, Campusitem.Name, true);
                                }
                            }
                            else
                            {
                                FollowupRemainderTrackerUpdate("Daily", "There are no Enquries to Sent", Campusitem.Name, true);
                            }
                        }
                    }
                }
                return RetValue;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                return true;
            }
        }
        private bool SendMailFollowupdetailstoGRL(string Body, string Campus, string MailBody, string Subject, string RecipientInfo, string SendMailType)
        {
            try
            {
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                string server = ConfigurationManager.AppSettings["CampusEmailType"].ToString();
                string HeadADMNMail = ConfigurationManager.AppSettings["HeadAddmissionMail"].ToString();
                criteria.Add("Campus", Campus);
                criteria.Add("Server", server);
                IList<CampusEmailId> campusemaildet = PSF.GetListWithSearchCriteria<CampusEmailId>(0, 99999, string.Empty, string.Empty, criteria);
                if (SendMailType == "Management")
                {
                    IList<CampusAdminEmailId> campusadmindet = PSF.GetListWithSearchCriteria<CampusAdminEmailId>(0, 99999, string.Empty, string.Empty, criteria);
                    if (campusadmindet != null && campusadmindet.Count > 0)
                    {
                        foreach (var item in campusadmindet)
                        {
                            mail.To.Add(item.EmailId);
                        }
                        mail.CC.Add(HeadADMNMail);
                        if (Campus == "CHENNAI MAIN" || Campus == "CHENNAI CITY")
                        {
                            string ChairmanMail = ConfigurationManager.AppSettings["ChairmanMailId"].ToString();
                            mail.To.Add(ChairmanMail);
                        }
                    }
                    else { mail.To.Add(campusemaildet.First().EmailId); }
                }
                else
                {
                    mail.To.Add(campusemaildet.First().EmailId);
                }
                mail.Subject = Subject; // st.Subject;
                MailBody = MailBody.Replace("{{CampusmailId}}", campusemaildet.First().EmailId);
                MailBody = MailBody.Replace("{{Ph.No}}", campusemaildet.First().PhoneNumber);
                MailBody = MailBody.Replace("{{DateTime}}", DateTime.Now.ToString());
                MailBody = MailBody.Replace("{{Recipients}}", RecipientInfo);
                MailBody = MailBody.Replace("{{Content}}", Body);
                mail.Body = MailBody;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("localhost", 25);
                smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address  
                //Or your Smtp Email ID and Password  
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = true;
                mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                smtp.Credentials = new System.Net.NetworkCredential
               (campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                {
                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("quota"))
                        {
                            mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                            smtp.Credentials = new System.Net.NetworkCredential(campusemaildet.First().AlternateEmailId.ToString(), campusemaildet.First().AlternateEmailIdPassword.ToString());
                            smtp.Send(mail);
                            return true;
                        }
                        else
                        {
                            mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                            smtp.Credentials = new System.Net.NetworkCredential(campusemaildet.First().AlternateEmailId, campusemaildet.First().AlternateEmailIdPassword);
                            smtp.Send(mail);
                            return true;
                        }
                    }
                }
                else { return false; }
            }
            catch (Exception ey)
            {
                ExceptionPolicy.HandleException(ey, "AdmissionPolicy");
                throw ey;
            }
        }
        private string GetMailTemplateBody()
        {
            string MessageBody = System.IO.File.ReadAllText(HttpContext.Current.Request.MapPath("~/Views/Shared/AdmissionEmailBody.html"));
            return MessageBody;
        }

        #endregion

        #region "Windowservice for Past day followup remainder mail"
        public bool SendPastDayEnquiryFollowUpMailtoAdmin()
        {
            try
            {
                bool RetValue = false;
                string RecipientInfo = string.Empty, Subject = string.Empty, MailBody = string.Empty, PreRegNums = string.Empty;
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                FollowUpRemainderTracker FollowupTracker = PSF.Get<FollowUpRemainderTracker>("RemainderType", "PastDay", "CheckDate", DateTime.Now.ToString("dd/MM/yyyy"));
                if (FollowupTracker == null || FollowupTracker != null && FollowupTracker.IsSent == false)
                {
                    MailBody = GetMailTemplateBody();
                    int count = 0;
                    RecipientInfo = "Dear Head-Admin,";
                    DateTime pastDay = DateTime.Now.AddDays(-1);
                    criteria.Add("FollowupDate", pastDay.ToString("dd/MM/yyyy"));
                    Dictionary<long, IList<FollowupDetailsView_vw>> GetFollowUpDetails = PSF.GetListWithEQSearchCriteriaCount<FollowupDetailsView_vw>(0, 99999, string.Empty, string.Empty, criteria);
                    if (GetFollowUpDetails != null && GetFollowUpDetails.Count > 0 && GetFollowUpDetails.FirstOrDefault().Key != 0 && GetFollowUpDetails.FirstOrDefault().Value != null)
                    {
                        Subject = "Not yet followed last day Enquiries";
                        string InCompleteFollowup = "Here the following Enquiries were not done yesterday, so that enquiries follow-ups are moved to next week as same day.<br /><br /><table class='tftable' border='1'><tr><th>S.No</th><th>PreRegistration Number</th><th>Student Name</th><th>Campus</th><th>Grade</th><th>Admission Status</th><th>Phone Number</th><th>EmailId</th><th>Followup Date</th></tr>";
                        foreach (var item in GetFollowUpDetails.FirstOrDefault().Value)
                        {
                            string check = CheckFollowUpCompleted(item);
                            if (check != "FollowUpCompleted")
                            {
                                count++; PreRegNums = PreRegNums + item.PreRegNum + ",";
                                InCompleteFollowup = InCompleteFollowup + "<tr><td>" + count + "</td><td>" + item.PreRegNum + "</td><td>" + item.Name + "</td><td>" + item.Campus + "</td><td>" + item.Grade + "</td><td>" + item.AdmissionStatus + "</td><td>" + item.PhNumber + "</td><td>" + item.EmailId + "</td><td>" + item.FollowupDate + "</td></tr>";
                                UpdateFollowuptoNextWeek(item.PreRegNum);
                            }
                        }
                        InCompleteFollowup = InCompleteFollowup + "</table><br/><br/>";
                        if (count > 0)
                        {
                            RetValue = SendMailPastFollowupbutNotCompleted(InCompleteFollowup, ConfigurationManager.AppSettings["HeadAddmissionMail"].ToString(), MailBody, Subject, RecipientInfo, "Management");
                        }
                        if (RetValue == true)
                        {
                            FollowupRemainderTrackerUpdate("PastDay", PreRegNums, "", true);
                        }
                    }
                    else
                    {
                        FollowupRemainderTrackerUpdate("PastDay", "There are no Enquries at the last day","",true);
                    }
                    return RetValue;
                }
                else
                    return RetValue;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "AdmissionPolicy");
                return true;
            }
        }
        private string CheckFollowUpCompleted(FollowupDetailsView_vw FTracker)
        {
            string retVal = string.Empty;
            DateTime PastDate = new DateTime();
            DateTime ActualDate = new DateTime();
            IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            Dictionary<long, IList<FollowupDetails>> fwpdet = PSF.GetListWithExactSearchCriteriaCount<FollowupDetails>(0, 10000, "Desc", "Id", criteria);
            if (fwpdet != null && fwpdet.Count > 0 && fwpdet.FirstOrDefault().Key != 0 && fwpdet.FirstOrDefault().Value != null)
            {
                PastDate = DateTime.Parse(FTracker.FollowupDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                foreach (var item in fwpdet.FirstOrDefault().Value)
                {
                    ActualDate = DateTime.Parse(item.FollowupDate, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    if (PastDate.Date < ActualDate.Date) { retVal = "FollowUpCompleted"; break; }
                    else { retVal = "FollowUpNotCompleted"; }
                }
                return retVal;
            }
            else
                return retVal;
        }
        private void UpdateFollowuptoNextWeek(long RegNum)
        {
            FollowupDetails fwNew = new FollowupDetails();
            fwNew.PreRegNum = RegNum;
            fwNew.FollowupDate = DateTime.Now.AddDays(7).ToString("dd/MM/yyyy");
            fwNew.Remarks = "followup was not done by admission incharge";
            PSF.SaveOrUpdate<FollowupDetails>(fwNew);
        }
        private bool SendMailPastFollowupbutNotCompleted(string Body, string ReceipientMail, string MailBody, string Subject, string RecipientInfo, string SendMailType)
        {
            try
            {
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                string server = ConfigurationManager.AppSettings["CampusEmailType"].ToString();
                criteria.Add("Campus", "IB-MAIN");
                criteria.Add("Server", server);
                IList<CampusEmailId> campusemaildet = PSF.GetListWithSearchCriteria<CampusEmailId>(0, 99999, string.Empty, string.Empty, criteria);
                if (SendMailType == "Management")
                {
                    mail.To.Add(ReceipientMail);
                }
                else
                {
                    mail.To.Add(campusemaildet.First().EmailId);
                }
                mail.Subject = Subject; // st.Subject;
                MailBody = MailBody.Replace("{{CampusmailId}}", campusemaildet.First().EmailId);
                MailBody = MailBody.Replace("{{Ph.No}}", campusemaildet.First().PhoneNumber);
                MailBody = MailBody.Replace("{{DateTime}}", DateTime.Now.ToString());
                MailBody = MailBody.Replace("{{Recipients}}", RecipientInfo);
                MailBody = MailBody.Replace("{{Content}}", Body);
                mail.Body = MailBody;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("localhost", 25);
                smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address  
                //Or your Smtp Email ID and Password  
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = true;
                mail.From = new MailAddress(campusemaildet.First().EmailId.ToString());
                smtp.Credentials = new System.Net.NetworkCredential(campusemaildet.First().EmailId.ToString(), campusemaildet.First().Password.ToString());
                if (!string.IsNullOrEmpty(mail.From.ToString()) && !string.IsNullOrEmpty(mail.To.ToString()))
                {
                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("quota"))
                        {
                            mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                            smtp.Credentials = new System.Net.NetworkCredential(campusemaildet.First().AlternateEmailId.ToString(), campusemaildet.First().AlternateEmailIdPassword.ToString());
                            smtp.Send(mail);
                            return true;
                        }
                        else
                        {
                            mail.From = new MailAddress(campusemaildet.First().AlternateEmailId.ToString());
                            smtp.Credentials = new System.Net.NetworkCredential(campusemaildet.First().AlternateEmailId.ToString(), campusemaildet.First().AlternateEmailIdPassword.ToString());
                            smtp.Send(mail);
                            return true;
                        }
                    }
                }
                else { return false; }
            }
            catch (Exception ey)
            {
                ExceptionPolicy.HandleException(ey, "AdmissionPolicy");
                throw ey;
            }
        }
        private void FollowupRemainderTrackerUpdate(string Type,string sentList,string Campus,bool IsSent)
        {
            FollowUpRemainderTracker FRem = new FollowUpRemainderTracker();
            FRem.Campus = Campus;
            FRem.RemainderType = Type;
            FRem.SentList = sentList;
            FRem.CheckDate = DateTime.Now.ToString("dd/MM/yyyy");
            FRem.IsSent = IsSent;
            PSF.SaveOrUpdate<FollowUpRemainderTracker>(FRem);
        }
        #endregion

        #region Second Language Master added by Gobi
        public Dictionary<long, IList<SecondLanguageMaster>> GetSecondLanguageMasterListWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<SecondLanguageMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region AgeCutOff
        public Dictionary<long, IList<AgeCutOffMaster>> GetAgeCutOffDetailsListWithCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<AgeCutOffMaster>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        #endregion

        public IList<StudentTemplateView> GetStudentTemplateViewListWithCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteria<StudentTemplateView>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StudentTemplateView>> GetStudentTemplateViewListWithLikeandExactPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria, Dictionary<string, object> Likecriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<StudentTemplateView>(page, pageSize, sortType, sortBy, criteria, Likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateCampusDocument(CampusDocumentMaster ea)
        {
            try
            {
                if (ea != null)
                    PSF.SaveOrUpdate<CampusDocumentMaster>(ea);
                else { throw new Exception("AdmissionManagement is required and it cannot be null.."); }
                return ea.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<CampusDocumentMaster>> GetCampusDocumentListwithCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<CampusDocumentMaster>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        #region Student Route Configuration
        public long CreateOrUpdateStudentLocationMaster(StudentLocationMaster studLocation)
        {
            try
            {
                if (studLocation != null)
                    PSF.SaveOrUpdate<StudentLocationMaster>(studLocation);
                else { throw new Exception("AdmissionManagement is required and it cannot be null.."); }
                return studLocation.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public StudentLocationMaster GetStudentLocationMasterByLocationName(string LocationName)
        {
            try
            {
                StudentLocationMaster StudentLocationMaster = null;
                if (!string.IsNullOrEmpty(LocationName))
                    StudentLocationMaster = PSF.Get<StudentLocationMaster>("LocationName", LocationName);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return StudentLocationMaster;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<StudentLocationMaster>> GetStudentLocationMasterListWithPagingAndCriteriaLikeSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<StudentLocationMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<StudentLocationMaster>> GetStudentLocationMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<StudentLocationMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region "Payment Report"
        public Dictionary<long, IList<PaymentDetailsReport_vw>> GetPaymentReportListWithPagingAndCriteriaWithAlias(int? page, int? pageSize, string sortby, string sortType, string name, string[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCountArray<PaymentDetailsReport_vw>(page, pageSize, sortby, sortType, name, values, criteria, alias);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public PaymentDetails GetPaymentDetailsById(long Id)
        {
            try
            {
                PaymentDetails pay = null;
                if (Id > 0)
                    pay = PSF.Get<PaymentDetails>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return pay;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public StudentLocalityMaster GetStudentLocaLityMasterByLocality(string LocalityName)
        {
            try
            {
                StudentLocalityMaster pay = null;
                pay = PSF.Get<StudentLocalityMaster>("LocalityName", LocalityName);
                return pay;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateStudentSubLocality(StudentSubLocalityMaster Sslc)
        {
            try
            {
                PSF.SaveOrUpdate<StudentSubLocalityMaster>(Sslc);
                return Sslc.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StudentSubLocalityMaster>> GetSublocalityMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCount<StudentSubLocalityMaster>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public Dictionary<long, IList<StudentLocalityMaster>> GetlocalityMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCount<StudentLocalityMaster>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public Dictionary<long, IList<LocationCount_Vw>> GetLocationDetailsListWithEQsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<LocationCount_Vw>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public Dictionary<long, IList<FollowUpReport_vw>> GetFollowupReportList(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> Lkcriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<FollowUpReport_vw>(page, pageSize, sortType, sortBy, criteria, Lkcriteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public FollowupDetails GetFollowupDetailsById(long Id)
        {
            try
            {
                FollowupDetails pay = null;
                if (Id > 0)
                    pay = PSF.Get<FollowupDetails>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return pay;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<AcademicYearReport>> GetAcademicYearWiseDetailsListWithEQsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<AcademicYearReport>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public TCRequestDetails GetTCRequestDetailsById(long Id)
        {
            try
            {
                TCRequestDetails trd = null;
                if (Id > 0)
                    trd = PSF.Get<TCRequestDetails>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return trd;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public long CreateOrUpdateTCRequestDetails(TCRequestDetails td)
        {
            try
            {
                if (td != null)
                    PSF.SaveOrUpdate<TCRequestDetails>(td);
                else { throw new Exception("AdmissionManagement is required and it cannot be null.."); }
                return td.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<TCRequestDetails>> GetTCRequestDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<TCRequestDetails>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<TCRequestDetails>> GetTCRequestDetailsListWithExactPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeSearchCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<TCRequestDetails>(page, pageSize, sortBy, sortType, criteria, likeSearchCriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<RelievingReasonMaster>> GetRelievingReasonMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<RelievingReasonMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<TcRequestReportByCampus_Vw>> GetTCRequestReasonByCampusDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<TcRequestReportByCampus_Vw>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<TcRequestReportByCampusGrade_Vw>> GetTCRequestReportByGradeDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<TcRequestReportByCampusGrade_Vw>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string GetStudentNameByPreRegNum(long PreRegNum)
        {
            try
            {
                StudentTemplateView StudentDetails = PSF.Get<StudentTemplateView>("PreRegNum", PreRegNum);
                return StudentDetails.Name;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #region TcRequestReportByCampus By Prabakaran
        public Dictionary<long, IList<TcRequestReportByCampus_SP>> GetTcRequestReportByCampusListSP(string AcademicYear, string Status,DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                return PSF.ExecuteStoredProcedurewithOptionalParametersByDictonary<TcRequestReportByCampus_SP>("GetTCRequestCampusWiseCount",
                         new[] { new SqlParameter("AcademicYear", AcademicYear),
                             new SqlParameter("Status",Status),
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
        #region TcRequestReportByCampusGrade By Prabakaran
        public Dictionary<long, IList<TcRequestReportByCampusGrade_SP>> GetTcRequestReportByCampusGradeListSP(string AcademicYear, string Status, DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                return PSF.ExecuteStoredProcedurewithOptionalParametersByDictonary<TcRequestReportByCampusGrade_SP>("GetTCRequestCampusGradeWiseCount",
                         new[] { new SqlParameter("AcademicYear", AcademicYear),
                             new SqlParameter("Status",Status),
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
        public TCRequestDetails GetTCRequestDetailsByPreRegNo(long PreRegNum)
        {
            try
            {
                TCRequestDetails trd = null;
                if (PreRegNum > 0)
                    trd = PSF.Get<TCRequestDetails>("PreRegNum",PreRegNum);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return trd;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public UploadedFiles GetUploadedFilesByPreRegNumandDocumentType(long PreRegNum)
        {
            try
            {

                UploadedFiles FileUpload1 = null;
                if (PreRegNum > 0)
                    FileUpload1 = PSF.Get<UploadedFiles>("PreRegNum", PreRegNum, "DocumentType", "Staff Photo");
                else { throw new Exception("Id is required and it cannot be 0"); }
                return FileUpload1;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #region Added for Kiosk
        public Dictionary<long, IList<StudentTemplate>> GetStudentDetailsByNameandCampus(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<StudentTemplate>(page, pageSize, sortby, sortType, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public KioskEnquiryDetails GetEnquiryDetailsById(long Id)
        {
            try
            {
                KioskEnquiryDetails Enqdetails = null;
                if (Id > 0)
                    Enqdetails = PSF.Get<KioskEnquiryDetails>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return Enqdetails;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<KioskEnquiryDetails>> GetKioskEnquiryDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<KioskEnquiryDetails>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
