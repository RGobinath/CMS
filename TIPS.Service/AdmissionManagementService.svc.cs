using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.ServiceContract;
using TIPS.Entities;
using TIPS.Entities.EnquiryEntities;
using TIPS.Entities.AdmissionEntities;
using TIPS.Component;
using TIPS.Entities.StoreEntities;
using System.Data;
using TIPS.Entities.CommunictionEntities;

namespace TIPS.ServiceContract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AdmissionManagement" in code, svc and config file together.
    public class AdmissionManagementService : IAdmissionManagementSC
    {
        AdmissionManagementBC ABC = new AdmissionManagementBC();
        public long CreateOrUpdateAdmissionManagement(AdmissionManagement am)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                AdmissionManagementBC.CreateOrUpdateAdmissionManagement(am);
                return am.ApplicationNo;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }
        public AdmissionManagement GetAdmissionManagementById(long ApplicationNo)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetAdmissionManagementById(ApplicationNo);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long StudentIdCount(string grade, string feeStrutYear, string campus)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.StudentIdCount(grade, feeStrutYear, campus);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long ErodeStudentIdCount(string grade, string campus, string FeeStructYear)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.ErodeStudentIdCount(grade, campus, FeeStructYear);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<AdmissionManagement>> GetAdmissionManagementListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetAdmissionManagementListWithPaging(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateCallManagement(AdmissionManagement am)
        {
            throw new NotImplementedException();
        }

        public long CreateOrUpdateStudentDetails(StudentTemplate st)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                AdmissionManagementBC.CreateOrUpdateStudentDetails(st);
                return st.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateEmailLog(EmailLog el)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                AdmissionManagementBC.CreateOrUpdateEmailLog(el);
                return el.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateTransferDetails(TransferDetails td)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                AdmissionManagementBC.CreateOrUpdateTransferDetails(td);
                return td.Id;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<StudentTemplate>> GetStudentDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetStudentDetailsListWithPagingAndCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<StudentTemplateView>> GetStudentDetailsListWithPagingAndCriteriaWithAlias(int? page, int? pageSize, string sortby, string sortType, string name, string[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                Dictionary<long, IList<StudentTemplateView>> retValue = AdmissionManagementBC.GetStudentTemplateListWithPagingAndCriteriaWithAlias(page, pageSize, sortby, sortType, name, values, criteria, alias);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        //Exact Search
        public Dictionary<long, IList<StudentTemplateView>> GetStudentDetailsListExactWithPagingAndCriteriaWithAlias(int? page, int? pageSize, string sortby, string sortType, string name, string[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                Dictionary<long, IList<StudentTemplateView>> retValue = AdmissionManagementBC.GetStudentDetailsListExactWithPagingAndCriteriaWithAlias(page, pageSize, sortby, sortType, name, values, criteria, alias);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<StudentTemplate>> GetStudentDetailsListWithEQsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetStudentDetailsListWithEQsearchCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<StudentTemplateView>> GetStudentTemplateViewListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetStudentTemplateViewListWithPagingAndCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<StudentTemplateView>> GetStudentTemplateViewListWithEQsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetStudentTemplateViewListWithEQsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }


        public StudentTemplate GetStudentDetailsById(long Id)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetStudentDetailsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<AddressDetails>> GetAddressDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetAddressDetailsListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }


        public long CreateOrUpdateUploadedFiles(UploadedFiles fu)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                AdmissionManagementBC.CreateOrUpdateUploadedFiles(fu);
                return fu.Id;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateUploadedFilesView(UploadedFilesView fu)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                AdmissionManagementBC.CreateOrUpdateUploadedFilesView(fu);
                return fu.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<UploadedFiles>> GetUploadedFilesListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetUploadedFilesListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<UploadedFilesView>> GetUploadedFilesViewListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetUploadedFilesViewListWithPagingAndCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public UploadedFiles GetUploadedFilesById(long Id)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetUploadedFilesById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public IList<UploadedFiles> GetUploadedFilesByPreRegNum(long PreRegNum, string flag)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetUploadedFilesByPreRegNum(PreRegNum, flag);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public UploadedFiles GetUploadedFilesByIdWithDirectQuery(long Id)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetUploadedFilesByIdWithDirectQuery(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<UploadedFilesView>> GetUploadedFilesViewListWithPagingAndCriteriaWithIn(int? page, int? pageSize, string sortby, string sortType, string name, int[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetUploadedFilesViewListWithPagingAndCriteriaWithIn(page, pageSize, sortby, sortType, name, values, criteria, alias);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public bool DeleteUploadedFiles(long id)
        {
            try
            {
                AdmissionManagementBC AdmissionBC = new AdmissionManagementBC();
                AdmissionBC.DeleteUploadedFiles(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }


        public bool DeleteUploadedFiles(long[] id)
        {
            try
            {
                AdmissionManagementBC AdmissionBC = new AdmissionManagementBC();
                AdmissionBC.DeleteUploadedFiles(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }


        public long CreateOrUpdatePreRegDetails(PreRegDetails pd)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                AdmissionManagementBC.CreateOrUpdatePreRegDetails(pd);
                return pd.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }


        public Dictionary<long, IList<PreRegDetails>> GetPreRegDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetPreRegDetailsListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public PreRegDetails GetPreRegDetailsById(long Id)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetPreRegDetailsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateSportsDetails(Sports sp)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                AdmissionManagementBC.CreateOrUpdateSportsDetails(sp);
                return sp.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }
        public bool DeleteSportsDetails(long id)
        {
            try
            {
                AdmissionManagementBC AdmissionBC = new AdmissionManagementBC();
                AdmissionBC.DeleteTransferDetails(id);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }
        public bool DeleteSportsDetails(long[] id)
        {
            try
            {
                AdmissionManagementBC AdmissionBC = new AdmissionManagementBC();
                AdmissionBC.DeleteSportsDetails(id);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<Sports>> GetSportsDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetSportsDetailsListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<Sports>> GetSportsDetailsListWithSearchCriteriaCountArray(int? page, int? pageSize, string sortby, string sortType, string name, string[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetSportsDetailsListWithSearchCriteriaCountArray(page, pageSize, sortby, sortType, name, values, criteria, alias);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateFamilyDetails(FamilyDetails fd)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                AdmissionManagementBC.CreateOrUpdateFamilyDetails(fd);
                return fd.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<FamilyDetails>> GetFamilyDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetFamilyDetailsListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
                // return AdmissionManagementBC.get.GetFamilyDetailsListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public FamilyDetails GetFamilyDetailsById(long Id)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetFamilyDetailsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public bool DeleteAttachment(long[] id)
        {
            try
            {
                AdmissionManagementBC AdmissionBC = new AdmissionManagementBC();
                AdmissionBC.DeleteAttachment(id);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }
        public bool DeleteAttachment(long id)
        {
            try
            {
                AdmissionManagementBC AdmissionBC = new AdmissionManagementBC();
                AdmissionBC.DeleteAttachment(id);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public bool DeleteFamilyDetails(long id)
        {
            try
            {
                AdmissionManagementBC AdmissionBC = new AdmissionManagementBC();
                AdmissionBC.DeleteFamilyDetails(id);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public bool DeleteFamilyDetails(long[] id)
        {
            try
            {
                AdmissionManagementBC AdmissionBC = new AdmissionManagementBC();
                AdmissionBC.DeleteFamilyDetails(id);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdatePastSchoolDetails(PastSchoolDetails pd)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                AdmissionManagementBC.CreateOrUpdatePastSchoolDetails(pd);
                return pd.Id;
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

        public Dictionary<long, IList<PastSchoolDetails>> GetPastSchoolDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetPastSchoolDetailsListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public bool DeletePastSchoolDetails(long id)
        {
            try
            {
                AdmissionManagementBC AdmissionBC = new AdmissionManagementBC();
                AdmissionBC.DeletePastSchoolDetails(id);
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

        public bool DeletePastSchoolDetails(long[] id)
        {
            try
            {
                AdmissionManagementBC AdmissionBC = new AdmissionManagementBC();
                AdmissionBC.DeletePastSchoolDetails(id);
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

        public bool DeleteTransferDetails(long id)
        {
            try
            {
                AdmissionManagementBC AdmissionBC = new AdmissionManagementBC();
                AdmissionBC.DeleteTransferDetails(id);
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
        public bool DeleteTransferDetails(long[] id)
        {
            try
            {
                AdmissionManagementBC AdmissionBC = new AdmissionManagementBC();
                AdmissionBC.DeleteTransferDetails(id);
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

        public long CreateOrUpdatePaymentDetails(PaymentDetails pd)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                AdmissionManagementBC.CreateOrUpdatePaymentDetails(pd);
                return pd.Id;
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

        public Dictionary<long, IList<PaymentDetails>> GetPaymentDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetPaymentDetailsListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public bool DeletePaymentDetails(long id)
        {
            try
            {
                AdmissionManagementBC AdmissionBC = new AdmissionManagementBC();
                AdmissionBC.DeletePaymentDetails(id);
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

        public bool DeletePaymentDetails(long[] id)
        {
            try
            {
                AdmissionManagementBC AdmissionBC = new AdmissionManagementBC();
                AdmissionBC.DeletePaymentDetails(id);
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

        public long CreateOrUpdateApproveAssign(ApproveAssign aa)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                AdmissionManagementBC.CreateOrUpdateApproveAssign(aa);
                return aa.Id;
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

        public Dictionary<long, IList<ApproveAssign>> GetApproveAssignListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetApproveAssignListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public bool DeleteApproveAssign(long id)
        {
            try
            {
                AdmissionManagementBC AdmissionBC = new AdmissionManagementBC();
                AdmissionBC.DeleteApproveAssign(id);
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

        public bool DeleteApproveAssign(long[] id)
        {
            try
            {
                AdmissionManagementBC AdmissionBC = new AdmissionManagementBC();
                AdmissionBC.DeleteApproveAssign(id);
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

        public long CreateOrUpdateIdGeneration(IdGeneration ig)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                AdmissionManagementBC.CreateOrUpdateIdGeneration(ig);
                return ig.Id;
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

        public Dictionary<long, IList<IdGeneration>> GetIdGenerationListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetIdGenerationListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public bool DeleteIdGeneration(long id)
        {
            try
            {
                AdmissionManagementBC AdmissionBC = new AdmissionManagementBC();
                AdmissionBC.DeleteIdGeneration(id);
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

        public bool DeleteIdGeneration(long[] id)
        {
            try
            {
                AdmissionManagementBC AdmissionBC = new AdmissionManagementBC();
                AdmissionBC.DeleteIdGeneration(id);
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

        public Dictionary<long, IList<StudentDetailsExport>> GetStudentExportListWithEQsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetStudentExportListWithEQsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<StudentDetailsExport>> GetStudentExportListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetStudentExportListWithPagingAndCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        //public Dictionary<long, IList<StudentTemplate1>> GetStudentTemplate1ListWithEQsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        //{
        //    try
        //    {
        //        AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
        //        return AdmissionManagementBC.GetStudentTemplate1ListWithEQsearchCriteria(page, pageSize, sortType, sortBy, criteria);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally { }
        //}
        //public Dictionary<long, IList<StudentTemplate1>> GetStudentTemplate1ListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        //{
        //    try
        //    {
        //        AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
        //        return AdmissionManagementBC.GetStudentTemplate1ListWithPagingAndCriteria(page, pageSize, sortBy, sortType, criteria);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally { }
        //}

        public Dictionary<long, IList<TransferDetails>> GetTransferDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetTransferDetailsListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateEmailAttachment(EmailAttachment ea)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                AdmissionManagementBC.CreateOrUpdateEmailAttachment(ea);
                return ea.Id;
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

        public Dictionary<long, IList<EmailAttachment>> GetEmailAttachmentListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetEmailAttachmentListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long GetMaxAttachmentId()
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetMaxAttachmentId();
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public StudentTemplate GetStudentDetailsByAppNo(string appno)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetStudentDetailsByAppNo(appno);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long CreateOrUpdateSMSLog(SMS sms)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                AdmissionManagementBC.CreateOrUpdateSMSLog(sms);
                return sms.Id;
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

        public Dictionary<long, IList<SMSTemplate>> GetSMSTemplateListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetSMSTemplateListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<AdmissionCountReport_vw>> GetAdmissionCountReport_vwWithEQsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetAdmissionCountReport_vwWithEQsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<EmailLog>> GetEmailLogListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetEmailLogListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<CampusEmailId>> GetCampusEmailIdListWithPagingAndCriteriaWithAlias(int? page, int? pageSize, string sortby, string sortType, string name, string[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                Dictionary<long, IList<CampusEmailId>> retValue = AdmissionManagementBC.GetCampusEmailIdListWithPagingAndCriteriaWithAlias(page, pageSize, sortby, sortType, name, values, criteria, alias);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<SMS>> GetSMSLogListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetSMSLogListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateFollowupDetails(FollowupDetails fwd)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                AdmissionManagementBC.CreateOrUpdateFollowupDetails(fwd);
                return fwd.Id;
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

        public Dictionary<long, IList<FollowupDetails>> GetFollowupDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetFollowupDetailsListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public bool DeleteFollowupDetails(long id)
        {
            try
            {
                AdmissionManagementBC AdmissionBC = new AdmissionManagementBC();
                AdmissionBC.DeleteFollowupDetails(id);
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

        public bool DeleteFollowupDetails(long[] id)
        {
            try
            {
                AdmissionManagementBC AdmissionBC = new AdmissionManagementBC();
                AdmissionBC.DeleteFollowupDetails(id);
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

        public long CreateOrUpdateStudentStatus(StudentTemplateStatus st)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                AdmissionManagementBC.CreateOrUpdateStudentStatus(st);
                return st.Id;
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
        public DataTable GetDashBoardStudentList(string StrSql)
        {
            try
            {
                AdmissionManagementBC BC = new AdmissionManagementBC();
                return BC.GetDashBoardStudentList(StrSql);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public StudentTemplate GetStudentDetailsByNewId(string NewId)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(NewId))
                {
                    return ABC.GetStudentDetailsByNewId(NewId);
                }
                else throw new Exception("NewId is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public StudentTemplate GetStudentDetailsByPreRegNo(long PreRegNo)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetStudentDetailsByPreRegNo(PreRegNo);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        #region Edit or update studtemplate and familydetails

        public Dictionary<long, IList<StudentTemplateAndFamilyDetails_vw>> StudentTemplateAndFamilyDetails_vwListWithCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.StudentTemplateAndFamilyDetails_vwListWithCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateStudentTemplateDetails(StudentTemplate Id)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.CreateOrUpdateStudentTemplateDetails(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public StudentTemplate GetStudentTemplateDetailsById(long Id)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                StudentTemplate StudentTemplateDetails = null;
                if (Id > 0)
                    StudentTemplateDetails = AdmissionManagementBC.GetStudentTemplateDetailsById(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return StudentTemplateDetails;
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
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.StudentTemplateAndFamilyDetails_vwListWithLikeAndExcactSerachCriteria(page, pageSize, sortby, sortType, exctcriteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        #region
        public long SaveOrUpdateBulkPromTransferRequestDetails(BulkPromTransferRequestDetails bPTRD)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                AdmissionManagementBC.SaveOrUpdateBulkPromTransferRequestDetails(bPTRD);
                return bPTRD.BulkPromTransferRequestId;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<BulkPromTransferRequestDetails>> GetBulkPromTransferRequestDetailsListWithEQsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetBulkPromTransferRequestDetailsListWithEQsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public BulkPromTransferRequestDetails GetBulkPromTransferRequestDetailsById(long Id)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetBulkPromTransferRequestDetailsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public IList<BulkPromTransfer> CreateOrUpdateBulkPromTransferList(IList<BulkPromTransfer> recipEmailInfo)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                AdmissionManagementBC.CreateOrUpdateBulkPromTransferList(recipEmailInfo);
                return recipEmailInfo;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally { }
        }
        public Dictionary<long, IList<BulkPromTransfer>> GetBulkPromTransferListWithEQsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetBulkPromTransferListWithEQsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public BulkPromTransfer GetBulkPromTransferByIds(long PreRegNum, long BulkPromTransferRequestId)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetBulkPromTransferByIds(PreRegNum, BulkPromTransferRequestId);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long SaveOrUpdateBulkPromTransfer(BulkPromTransfer bPTRD)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                AdmissionManagementBC.SaveOrUpdateBulkPromTransfer(bPTRD);
                return bPTRD.BulkPromTransferRequestId;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion

        public Dictionary<long, IList<StudentTemplate>> GetStudentDetailsListWithLikesearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetStudentDetailsListWithLikesearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public StudentTemplate GetStudentDetailsByPreRegNum(long PreRegNum)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetStudentDetailsByPreRegNum(PreRegNum);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        #region Second Language Master added by Gobi
        public Dictionary<long, IList<SecondLanguageMaster>> GetSecondLanguageMasterListWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    AdmissionManagementBC Admission = new AdmissionManagementBC();
                    return Admission.GetSecondLanguageMasterListWithCriteria(page, pageSize, sortby, sortType, criteria);
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
        #endregion

        #region AgeCutOff

        public Dictionary<long, IList<AgeCutOffMaster>> GetAgeCutOffDetailsListWithCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmnBC = new AdmissionManagementBC();
                return AdmnBC.GetAgeCutOffDetailsListWithCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        #endregion

        public IList<StudentTemplateView> GetStudentTemplateViewListWithCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetStudentTemplateViewListWithCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        //Added newly for like and exact search
        public Dictionary<long, IList<StudentTemplateView>> GetStudentTemplateViewListWithLikeandExactPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria, Dictionary<string, object> Likecriteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetStudentTemplateViewListWithLikeandExactPagingAndCriteria(page, pageSize, sortType, sortBy, criteria, Likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateCampusDocument(CampusDocumentMaster ea)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                AdmissionManagementBC.CreateOrUpdateCampusDocument(ea);
                return ea.Id;
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

        public Dictionary<long, IList<CampusDocumentMaster>> GetCampusDocumentListwithCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmnBC = new AdmissionManagementBC();
                return AdmnBC.GetCampusDocumentListwithCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #region Student Route Configuration
        public long CreateOrUpdateStudentLocationMaster(StudentLocationMaster studLocation)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                AdmissionManagementBC.CreateOrUpdateStudentLocationMaster(studLocation);
                return studLocation.Id;
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
        public StudentLocationMaster GetStudentLocationMasterByLocationName(string LocationName)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetStudentLocationMasterByLocationName(LocationName);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<StudentLocationMaster>> GetStudentLocationMasterListWithPagingAndCriteriaLikeSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetStudentLocationMasterListWithPagingAndCriteriaLikeSearch(page, pageSize, sortby, sortType, criteria);
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
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetStudentLocationMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                Dictionary<long, IList<PaymentDetailsReport_vw>> retValue = ABC.GetPaymentReportListWithPagingAndCriteriaWithAlias(page, pageSize, sortby, sortType, name, values, criteria, alias);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion

        public PaymentDetails GetPaymentDetailsById(long Id)
        {
            try
            {
                AdmissionManagementBC ABC = new AdmissionManagementBC();
                return ABC.GetPaymentDetailsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public StudentLocalityMaster GetStudentLocaLityMasterByLocality(string LocalityName)
        {
            try
            {
                return ABC.GetStudentLocaLityMasterByLocality(LocalityName);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateStudentSubLocality(StudentSubLocalityMaster Sslc)
        {
            try
            {
                ABC.CreateOrUpdateStudentSubLocality(Sslc);
                return Sslc.Id;
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("Cannot insert duplicate key row in object"))
                {
                    throw new FaultException("Cannot insert duplicate key.");
                }
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<StudentSubLocalityMaster>> GetSublocalityMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return ABC.GetSublocalityMasterListWithPagingAndCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<StudentLocalityMaster>> GetlocalityMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return ABC.GetlocalityMasterListWithPagingAndCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<LocationCount_Vw>> GetLocationListWithEQsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return ABC.GetLocationDetailsListWithEQsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<FollowUpReport_vw>> GetFollowupReportList(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> Lkcriteria)
        {
            try
            {
                return ABC.GetFollowupReportList(page, pageSize, sortBy, sortType, criteria, Lkcriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public FollowupDetails GetFollowupDetailsById(long Id)
        {
            try
            {
                return ABC.GetFollowupDetailsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<AcademicYearReport>> GetAcademicYearWiseDetailsListWithEQsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetAcademicYearWiseDetailsListWithEQsearchCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public TCRequestDetails GetTCRequestDetailsById(long Id)
        {
            try
            {
                return ABC.GetTCRequestDetailsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long CreateOrUpdateTCRequestDetails(TCRequestDetails td)
        {
            try
            {

                ABC.CreateOrUpdateTCRequestDetails(td);
                return td.Id;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<TCRequestDetails>> GetTCRequestDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return ABC.GetTCRequestDetailsListWithPagingAndCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<TCRequestDetails>> GetTCRequestDetailsListWithExactPagingAndExactCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria, Dictionary<string, object> likeSearchCriteria)
        {
            try
            {
                //return ABC.GetTCRequestDetailsListWithPagingAndCriteria(page, pageSize, sortBy, sortType, criteria);
                return ABC.GetTCRequestDetailsListWithExactPagingAndCriteria(page, pageSize, sortType, sortBy, criteria, likeSearchCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<RelievingReasonMaster>> GetRelievingReasonMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return ABC.GetRelievingReasonMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                return ABC.GetTCRequestReasonByCampusDetailsListWithPagingAndCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<TcRequestReportByCampusGrade_Vw>> GetTCRequestReportByGradeDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return ABC.GetTCRequestReportByGradeDetailsListWithPagingAndCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public string GetStudentNameByPreRegNum(long PreRegNum)
        {
            try
            {
                if (PreRegNum > 0)
                {

                    return ABC.GetStudentNameByPreRegNum(PreRegNum);
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #region TcRequestReportByCampus By Prabakaran
        public Dictionary<long, IList<TcRequestReportByCampus_SP>> GetTcRequestReportByCampusListSP(string AcademicYear, string Status, DateTime? FromDate, DateTime? ToDate)
        {
            try
            {

                return ABC.GetTcRequestReportByCampusListSP(AcademicYear, Status, FromDate, ToDate);
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

                return ABC.GetTcRequestReportByCampusGradeListSP(AcademicYear, Status, FromDate, ToDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        public TCRequestDetails GetTCRequestDetailsByPreRegNo(long PreRegNo)
        {
            try
            {
                return ABC.GetTCRequestDetailsByPreRegNo(PreRegNo);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public UploadedFiles GetUploadedFilesByPreRegNumandDocumentType(long PreRegNum)
        {
            try
            {
                AdmissionManagementBC AdmissionManagementBC = new AdmissionManagementBC();
                return AdmissionManagementBC.GetUploadedFilesByPreRegNumandDocumentType(PreRegNum);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #region For Kiosk
        public Dictionary<long, IList<StudentTemplate>> GetStudentDetailsbyNameandCampus(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return ABC.GetStudentDetailsByNameandCampus(page, pageSize, sortby, sortType, criteria);
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
                return ABC.GetEnquiryDetailsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<KioskEnquiryDetails>> GetKioskEnquiryDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {

                return ABC.GetKioskEnquiryDetailsListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
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
