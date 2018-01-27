using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.Entities.StaffManagementEntities;
using TIPS.Component;
using TIPS.Entities;
using TIPS.Entities.BioMetricsEntities;

namespace TIPS.ServiceContract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "StaffManagementService" in code, svc and config file together.
    public class StaffManagementService : IStaffManagementSC
    {
        #region "Object Declaration"
        StaffManagementBC smsBcObj = new StaffManagementBC();
        #endregion "End"


        public long CreateOrUpdateStaffDetails(StaffDetails sd)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.CreateOrUpdateStaffDetails(sd);
                return sd.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public long CreateOrUpdateStaffDetailsView(StaffDetailsView sd)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.CreateOrUpdateStaffDetailsView(sd);
                return sd.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public long CreateOrUpdateStaffDetailsEdit(StaffDetailsEdit sd)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.CreateOrUpdateStaffDetailsEdit(sd);
                return sd.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<StaffDetails>> GetStaffDetailsListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffDetailsListWithPaging(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<StaffDetailsView>> GetStaffDetailsListWithPagingAndCriteriaWithAlias(int? page, int? pageSize, string sortby, string sortType, string name, string[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                Dictionary<long, IList<StaffDetailsView>> retValue = StaffManagementBC.GetStaffDetailsListWithPagingAndCriteriaWithAlias(page, pageSize, sortby, sortType, name, values, criteria, alias);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<StaffDetailsView>> GetStaffDetailsViewListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffDetailsViewListWithPaging(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<StaffDetailsEdit>> GetStaffDetailsEditListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffDetailsEditListWithPaging(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<StaffDetailsView>> GetStaffDetailsViewListINWithPaging(int? page, int? pageSize, string sortType, string sortBy, string name, int[] values, Dictionary<string, object> searchCriteria, string[] criteriaAlias)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffDetailsViewListINWithPaging(page, pageSize, sortType, sortBy, name, values, searchCriteria, criteriaAlias);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public StaffDetails GetStaffDetailsId(long Id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffDetailsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public StaffIdNumber GetStaffIdnumber(long Id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffIdnumberById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateStaffRequestNumDetails(StaffRequestNumDetails srn)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.CreateOrUpdateStaffRequestNumDetails(srn);
                return srn.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<StaffRequestNumDetails>> GetStaffRequestNumDetailsListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffRequestNumDetailsListWithPaging(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<StaffQualification>> GetStaffQualificationDetailsListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffQualificationDetailsListWithPaging(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateStaffQualificatoinDetails(StaffQualification sq)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.CreateOrUpdateStaffQualificationDetails(sq);
                return sq.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<StaffExperience>> GetStaffExperienceDetailsListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffExperienceDetailsListWithPaging(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateStaffExperienceDetails(StaffExperience se)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.CreateOrUpdateStaffExperienceDetails(se);
                return se.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<StaffTraining>> GetStaffTrainingDetailsListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffTrainingDetailsListWithPaging(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateStaffTrainingDetails(StaffTraining st)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.CreateOrUpdateStaffTrainingDetails(st);
                return st.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public long CreateOrUpdateStaffIdnumber(StaffIdNumber sid)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.CreateOrUpdateStaffIdnumber(sid);
                return sid.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteQualificationDetails(long id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.DeleteQualificationDetails(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool DeleteQualificationDetails(long[] id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.DeleteQualificationDetails(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteExperienceDetails(long id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.DeleteExperienceDetails(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool DeleteExperienceDetails(long[] id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.DeleteExperienceDetails(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteTrainingDetails(long id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.DeleteTrainingDetails(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool DeleteTrainingDetails(long[] id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.DeleteTrainingDetails(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public int CreateOrUpdateEmployeeSalaryDetails(EmployeeSalaryDetails esd)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.CreateOrUpdateEmployeeSalaryDetails(esd);
                return esd.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public IList<EmployeeSalaryDetails> CreateOrUpdateEmployeeSalaryList(IList<EmployeeSalaryDetails> empsalLst)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.CreateOrUpdateEmployeeSalaryList(empsalLst);
                return empsalLst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public StaffDetails GetStaffDetailsByIdNumber(string EmployeeId)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(EmployeeId))
                {
                    StaffManagementBC StaffManagementBC = new StaffManagementBC();
                    return StaffManagementBC.GetStaffDetailsByIdNumber(EmployeeId);
                }
                else throw new Exception("User Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<EmployeeSalaryDetails>> GetEmployeeSalaryDetailsListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetEmployeeSalaryDetailsDetailsListWithPaging(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        #region Staff Birthday Wishes Service
        public bool SendBDayWishes()
        {
            StaffManagementBC SMBC = new StaffManagementBC();
            SMBC.SendBDayWishes();
            return true;
        }
        #endregion
        #region Staff Birthday Wishes List Page
        public Dictionary<long, IList<StaffBDayWishesStatus>> GetStaffBDayWishesStatusListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StaffManagementBC SMBC = new StaffManagementBC();
                return SMBC.GetStaffBDayWishesStatusListWithPaging(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion

        public Sequence GetStaffIdnumberfromSequenceTable(long Id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffIdnumberfromSequenceTable(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long CreateOrUpdateSequence(Sequence sid)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.CreateOrUpdateSequence(sid);
                return sid.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        #region Employee Form
        public long CreateOrUpdateStaffFamilyDetails(StaffFamilyDetails SFD)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.CreateOrUpdateStaffFamilyDetails(SFD);
                return SFD.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<StaffFamilyDetails>> GetStaffFamilyDetailsListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffFamilyDetailsListWithPaging(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public bool DeleteStaffFamilyDetails(long[] id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.DeleteStaffFamilyDetails(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }


        public long CreateOrUpdateStaffReferenceDetails(StaffReferenceDetails Ref)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.CreateOrUpdateStaffReferenceDetails(Ref);
                return Ref.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<StaffReferenceDetails>> GetStaffReferenceDetailsListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffReferenceDetailsListWithPaging(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public bool DeleteStaffReferenceDetails(long[] id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.DeleteStaffReferenceDetails(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #endregion
        public StaffDetails GetStaffDeatailsByPreRegNum(Int32 PreRegNum)
        {
            try
            {
                return smsBcObj.GetStaffDeatailsByPreRegNum(PreRegNum);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long CreateOrUpdateEvents(Event et)
        {
            try
            {
                StaffManagementBC SBC = new StaffManagementBC();
                SBC.CreateOrUpdateEvents(et);
                return et.EventId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public long CreateOrUpdateEventList(EventList el)
        {
            try
            {
                StaffManagementBC SBC = new StaffManagementBC();
                SBC.CreateOrUpdateEventList(el);
                return el.EventListId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Event GetEventsById(long Id)
        {
            try
            {
                StaffManagementBC SBC = new StaffManagementBC();
                return SBC.GetEventsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #region Added By Prabakaran
        public long ExecutePercentageQueryFromStaffDetailsUsingQuery(long PreRegNum)
        {
            try
            {
                StaffManagementBC SBC = new StaffManagementBC();
                return SBC.ExecutePercentageQueryFromStaffDetailsUsingQuery(PreRegNum);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        #region Added By Prabakaran for Staff Promotion
        public long CreateOrUpdateStaffPromotionAndTransferDetails(StaffPromotionAndTransferDetails srn)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.CreateOrUpdateStaffPromotionAndTransferDetails(srn);
                return srn.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #endregion
        #region StaffEvaluationCategoryMaster By Prabakaran
        public Dictionary<long, IList<StaffEvaluationCategoryMaster>> GetStaffEvaluationCategoryListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffEvaluationCategoryListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long SaveOrUpdateStaffEvaluationCateogry(StaffEvaluationCategoryMaster StaffEvaluationCateogry)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.SaveOrUpdateStaffEvaluationCategory(StaffEvaluationCateogry);
                return StaffEvaluationCateogry.StaffEvaluationCategoryId;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public StaffEvaluationCategoryMaster GetStaffEvaluationCategoryById(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    StaffManagementBC StaffManagementBC = new StaffManagementBC();
                    return StaffManagementBC.GetStaffEvaluationCategoryById(Id);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public IList<StaffEvaluationCategoryMaster> SaveOrUpdateStaffEvaluationCateogryByList(IList<StaffEvaluationCategoryMaster> StaffEvaluationCateogry)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.SaveOrUpdateStaffEvaluationCategoryByList(StaffEvaluationCateogry);
                return StaffEvaluationCateogry;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        #region StaffEvaluationParameter By Prabakaran
        public Dictionary<long, IList<StaffEvaluationParameter>> GetStaffEvaluationParameterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffEvaluationParameterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<StaffEvaluationParameter_vw>> GetStaffEvaluationParameter_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffEvaluationParameter_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long SaveOrUpdateStaffEvaluationParameter(StaffEvaluationParameter staffevaluationparameter)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.SaveOrUpdateStaffEvaluationParameter(staffevaluationparameter);
                return staffevaluationparameter.StaffEvaluationParameterId;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public StaffEvaluationParameter GetStaffEvaluationParameterById(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    StaffManagementBC StaffManagementBC = new StaffManagementBC();
                    return StaffManagementBC.GetStaffEvaluationParameterById(Id);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion

        #region Added By Prabakaran Campus Based Staff Details
        public IList<CampusBasedStaffDetails> SaveOrUpdateCampusBasedStaffDetailsByList(IList<CampusBasedStaffDetails> campusbasedstaffdetails)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.SaveOrUpdateCampusBasedStaffDetailsByList(campusbasedstaffdetails);
                return campusbasedstaffdetails;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long DeleteCampusBasedStaffDetailsList(IList<long> Id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.DeleteCampusBasedStaffDetailsList(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public CampusBasedStaffDetails GetCampusBasedStaffDetailsById(long Id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetCampusBasedStaffDetailsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long SaveOrUpdateCampusBasedStaffDetails(CampusBasedStaffDetails CampusBasedStaffDetails)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.SaveOrUpdateCampusBasedStaffDetails(CampusBasedStaffDetails);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public IList<CampusBasedStaffDetails> GetCampusBasedStaffDetailsByStaffsByStaffPreRegNumber(long StaffPreRegNumber)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetCampusBasedStaffDetailsByStaffsByStaffPreRegNumber(StaffPreRegNumber);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        #region Added By Prabakaran StaffWise Score Report
        public Dictionary<long, IList<StaffWiseScoreReport_Vw>> GetStaffWiseScoreReportListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeSearchCriteria)
        {
            try
            {
                try
                {
                    return smsBcObj.GetStaffWiseScoreReportListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria, likeSearchCriteria);
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
        public Dictionary<long, IList<StaffEvaluationCategoryWise_Vw>> GetSStaffEvaluationCategoryWiseListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeSearchCriteria)
        {
            try
            {
                try
                {
                    return smsBcObj.GetSStaffEvaluationCategoryWiseListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria, likeSearchCriteria);
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

        public Dictionary<long, IList<StaffGroupMaster>> GetStaffGroupMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffGroupMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public string GetStaffNameByPreRegNum(long PreRegNum)
        {
            try
            {
                if (PreRegNum > 0)
                {
                    StaffManagementBC StaffManagementBC = new StaffManagementBC();
                    return StaffManagementBC.GetStaffNameByPreRegNum(PreRegNum);
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
        #region Added By Prabakaran
        public StaffDetailsView GetStaffDetailsViewById(long Id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffDetailsViewById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        #region StaffGroupMaster
        public long CreateOrUpdateStaffGroupMaster(StaffGroupMaster sgm)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.CreateOrUpdateStaffGroupMaster(sgm);
                return sgm.StaffGroupId;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public StaffGroupMaster GetStaffGroupMasterByCampusandGroup(string Campus, string GroupName)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffGroupMasterByCampusandGroup(Campus, GroupName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public StaffGroupMaster GetStaffGroupMasterByStaffGroupId(long StaffGroupId)
        {
            try
            {
                if (StaffGroupId > 0)
                {
                    StaffManagementBC StaffManagementBC = new StaffManagementBC();
                    return StaffManagementBC.GetStaffGroupMasterByStaffGroupId(StaffGroupId);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool DeleteStaffGroupMaster(long[] id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.DeleteStaffGroupMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #endregion
        public Dictionary<long, IList<StaffSubGroupMaster>> GetStaffSubGroupMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffSubGroupMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateStaffAttendance(StaffAttendance sa)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.CreateOrUpdateStaffAttendance(sa);
                return sa.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public StaffAttendance GetStaffAttendanceByAttendanceDatewithPreRegNum(long PreRegNum)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffAttendanceByAttendanceDatewithPreRegNum(PreRegNum);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<StaffAttendance_vw>> GetStaffAttendanceListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffAttendance_vwListWithPaging(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public StaffFamilyDetails GetStaffFamilyDetailsById(long Id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffFamilyDetailsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public StaffQualification GetStaffQualificationDetailsById(long Id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffQualificationDetailsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public StaffExperience GetStaffExperienceDetailsById(long Id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffExperienceDetailsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public StaffReferenceDetails GetStaffReferenceDetailsById(long Id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffReferenceDetailsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public StaffDetails GetStaffDeatailsByIdNumber(string IdNumber)
        {
            try
            {
                return smsBcObj.GetStaffDeatailsByIdNumber(IdNumber);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #region Student survey group By john naveen
        public Dictionary<long, IList<StudentSurveyGroupMaster>> GetStudentSurveyGroupListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStudentSurveyGroupListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long SaveOrUpdateStudentSurveyGroup(StudentSurveyGroupMaster StudentSurveyGroupms)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.SaveOrUpdateStudentSurveyGroup(StudentSurveyGroupms);
                return StudentSurveyGroupms.StudentSurveyGroupId;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public StudentSurveyGroupMaster GetStudentSurveyGroupById(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    StaffManagementBC StaffManagementBC = new StaffManagementBC();
                    return StaffManagementBC.GetStudentSurveyGroupById(Id);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public IList<StudentSurveyGroupMaster> SaveOrUpdateStudentSurveyGroupByList(IList<StudentSurveyGroupMaster> StudentSurveyGroupms)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.SaveOrUpdateStudentSurveyGroupByList(StudentSurveyGroupms);
                return StudentSurveyGroupms;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        # region Student Survey Question and answer master by vinoth
        public StudentSurveyQuestionMaster GetStudentSurveyQuestionByStudentSurveyQuestionId(long StudentSurveyQuestionId)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStudentSurveyQuestionByStudentSurveyQuestionId(StudentSurveyQuestionId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public long CreateOrUpdateStudentSurveyQuestion(StudentSurveyQuestionMaster studsurvey)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.CreateOrUpdateStudentSurveyQuestion(studsurvey);
                return studsurvey.StudentSurveyQuestionId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }

        }

        public Dictionary<long, IList<StudentSurveyQuestionMaster>> GetStudentSurveyQuestionListWithExcactAndLikeSearchCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStudentSurveyQuestionListWithExcactAndLikeSearchCriteria(page, pageSize, sortType, sortby, criteria, likecriteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteStudentSurveyQuestion(long[] Ids)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.DeleteStudentSurveyQuestion(Ids);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public StudentSurveyAnswerMaster GetStudentSurveyAnswerMasterByStudentSurveyAnswerMasterId(long StudentSurveyAnswerId)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStudentSurveyAnswerMasterByStudentSurveyAnswerMasterId(StudentSurveyAnswerId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public long CreateOrUpdateStudentSurveyAnswerMaster(StudentSurveyAnswerMaster studsurveyans)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.CreateOrUpdateStudentSurveyAnswerMaster(studsurveyans);
                return studsurveyans.StudentSurveyAnswerId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }

        }

        public Dictionary<long, IList<StudentSurveyAnswerView>> GetStudentSurveyAnswerMasterListWithExcactAndLikeSearchCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStudentSurveyAnswerMasterListWithExcactAndLikeSearchCriteria(page, pageSize, sortType, sortby, criteria, likecriteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteStudentStudentSurveyAnswerMaster(long[] Ids)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.DeleteStudentSurveyAnswerMaster(Ids);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public StudentSurveyQuestionMaster GetStudentSurveyQuestion(long StudentSurveyGroupId, string StudentSurveyQuestion)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStudentSurveyQuestion(StudentSurveyGroupId, StudentSurveyQuestion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public StudentSurveyAnswerMaster GetStudentSurveyAnswer(long StudentSurveyQuestionId, string StudentSurveyAnswer)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStudentSurveyAnswer(StudentSurveyQuestionId, StudentSurveyAnswer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public StudentSurveyAnswerView GetStudentSurveyAnswerView(long StudentSurveyGroupId, long StudentSurveyQuestionId, string StudentSurveyAnswer)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStudentSurveyAnswerView(StudentSurveyGroupId, StudentSurveyQuestionId, StudentSurveyAnswer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Dictionary<long, IList<StudentSurveyQuestionMasterView>> GetStudentSurveyQuestionMasterViewListWithExcactAndLikeSearchCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStudentSurveyQuestionMasterViewListWithExcactAndLikeSearchCriteria(page, pageSize, sortType, sortby, criteria, likecriteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public StudentSurveyGroupMaster GetStudentSurveyAcademicYearCampusGrade(string AcademicYear, string Campus, string Grade)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStudentSurveyGroupMasterByCampusAcdemicYearGrade(AcademicYear, Campus, Grade);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region StudentSurveyReport
        public StaffWiseStudentSurveyNewResult_Vw GetStaffWiseStudentSurveyNewResultById(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    StaffManagementBC SBC = new StaffManagementBC();
                    return SBC.GetStaffWiseStudentSurveyNewResultById(Id);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<StaffWiseStudentSurveyNewResult_Vw>> GetStaffWiseStudentSurverList(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffWiseStudentSurverList(page, pageSize, sortby, sortType, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<StaffWiseStudentSurveyNewResultWOS_Vw>> GetStaffWiseStudentSurverWOSList(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffWiseStudentSurverWOSList(page, pageSize, sortby, sortType, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<StudentSurveyReportNew_Vw>> GetStaffEvaluationStudentCountList(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffEvaluationStudentCountList(page, pageSize, sortby, sortType, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<StudentSurveyReportWOSecNew_Vw>> GetStaffEvaluationStudentCountListWithoutSection(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffEvaluationStudentCountListWithoutSection(page, pageSize, sortby, sortType, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<StaffEvaluationScore_Vw>> GetStaffEvaluationScoreList(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffEvaluationScoreList(page, pageSize, sortby, sortType, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<StaffwiseSurveyQuestionReportWOSec_Vw>> GetStudentSurveyQuestionMarkListWithoutSection(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStudentSurveyQuestionMarkListWithoutSection(page, pageSize, sortby, sortType, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<StaffwiseSurveyQuestionReport_Vw>> GetStudentSurveyQuestionMarkList(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStudentSurveyQuestionMarkList(page, pageSize, sortby, sortType, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public StudentSurveyReportWOSecNew_Vw GetStudentSurveyReportWOSecById(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    StaffManagementBC SBC = new StaffManagementBC();
                    return SBC.GetStudentSurveyReportWOSecById(Id);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public StudentSurveyReportNew_Vw GetStudentSurveyReportById(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    StaffManagementBC SBC = new StaffManagementBC();
                    return SBC.GetStudentSurveyReportById(Id);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public StaffWiseStudentSurveyNewResultWOS_Vw StaffWiseStudentSurveyNewResultWOSById(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    StaffManagementBC SBC = new StaffManagementBC();
                    return SBC.StaffWiseStudentSurveyNewResultWOSById(Id);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        #region SurveyQuestionMaster
        public Dictionary<long, IList<SurveyQuestionMaster>> GetSurveyQuestionMasterWithExactAndLikeSearchCriteriaWithCount(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetSurveyQuestionMasterWithExactAndLikeSearchCriteriaWithCount(page, pageSize, sortby, sortType, criteria, likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateSurveyQuestionMaster(SurveyQuestionMaster sqm)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.CreateOrUpdateSurveyQuestionMaster(sqm);
                return sqm.SurveyQuestionId;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public SurveyQuestionMaster GetSurveyQuestionMasterBySurveyGroupIdandQuestion(long SurveyGroupId, string SurveyQuestion)
        {
            try
            {
                if (SurveyGroupId > 0 && !string.IsNullOrEmpty(SurveyQuestion))
                {
                    StaffManagementBC StaffManagementBC = new StaffManagementBC();
                    return StaffManagementBC.GetSurveyQuestionMasterBySurveyGroupIdandQuestion(SurveyGroupId, SurveyQuestion);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public SurveyQuestionMaster GetSurveyQuestionMasterById(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    StaffManagementBC StaffManagementBC = new StaffManagementBC();
                    return StaffManagementBC.GetSurveyQuestionMasterById(Id);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool DeleteSurveyQuestionMaster(long[] id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.DeleteSurveyQuestionMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #endregion
        #region survey group master added by john naveen
        public long CreateOrUpdateSurveyGroupMaster(SurveyGroupMaster survey)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.CreateOrUpdateSurveyGroupMaster(survey);
                return survey.SurveyGroupId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteSurveyGroupMaster(long id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.DeleteSurveyGroupMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteSurveyGroupMaster(long[] id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.DeleteSurveyGroupMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<SurveyGroupMaster>> GetSurveyGroupMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetSurveyGroupMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public SurveyGroupMaster GetSurveyGroupMasterByGroupName(string SurveyGroup)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetSurveyGroupMasterrByGroupName(SurveyGroup);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        #region Survey Answer Master added by john naveen
        public Dictionary<long, IList<SurveyAnswerMaster>> GetSurveyAnswerMasterWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetSurveyAnswerMasterWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateSurveyAnswerMaster(SurveyAnswerMaster sam)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.CreateOrUpdateSurveyAnswerMaster(sam);
                return sam.SurveyAnswerId;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public SurveyAnswerMaster GetSurveyAnswerMasterBySurveyQuestionandMark(string SurveyAnswer, long SurveyQuestionId, long SurveyMark, bool IsPositive)
        {
            try
            {
                if (!string.IsNullOrEmpty(SurveyAnswer) && SurveyMark >= 0 && SurveyQuestionId > 0)
                {
                    StaffManagementBC StaffManagementBC = new StaffManagementBC();
                    return StaffManagementBC.GetSurveyAnswerMasterBySurveyQuestionandMark(SurveyAnswer, SurveyQuestionId, SurveyMark, IsPositive);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public SurveyAnswerMaster GetSurveyAnswerMasterById(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    StaffManagementBC StaffManagementBC = new StaffManagementBC();
                    return StaffManagementBC.GetSurveyAnswerMasterById(Id);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool DeleteSurveyAnswerMasterMaster(long[] id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.DeleteSurveyAnswerMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #endregion
        #region Staff Attendance Report Configuration
        public long SaveOrUpdateStaffAttendanceReportConfigurationByCampusBasedStaffDetails(CampusBasedStaffDetails CampusBasedStaffDetailsObj, string userId)
        {
            return smsBcObj.SaveOrUpdateStaffAttendanceReportConfigurationByCampusBasedStaffDetails(CampusBasedStaffDetailsObj, userId);
        }
        public Dictionary<long, IList<Staff_AttendanceReportConfiguration>> GetStaff_AttendanceReportConfigurationDetailsListWithPaging(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaff_AttendanceReportConfigurationDetailsListWithPaging(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public StaffDetailsView GetStaffDetailsViewByPreRegNum(Int32 PreRegNum)
        {
            try
            {
                return smsBcObj.GetStaffDetailsViewByPreRegNum(PreRegNum);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<Staff_AttendanceReportConfiguration_Vw>> GetStaff_AttendanceReportConfiguration_VwListWithCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return smsBcObj.GetStaff_AttendanceReportConfiguration_VwListWithCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<CampusBasedStaffDetails_Vw>> GetCampusBasedStaffDetails_VwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetCampusBasedStaffDetails_VwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion

        #region Staff Attendance Configuration
        public bool AddOrUpdateStaff_AttendanceReportConfigurationByStaffs(long StaffPreRegNum, string Campus, string[] ReportingHeadPreRegNums, string userId)
        {
            try
            {
                return smsBcObj.AddOrUpdateStaff_AttendanceReportConfigurationByStaffs(StaffPreRegNum, Campus, ReportingHeadPreRegNums, userId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IList<Staff_AttendanceReportConfigurationByStaffs> GetStaff_AttendanceReportConfigurationsListBasedOnReportingHeadPreRegNums(long[] ReportingHeadPreRegNums)
        {
            try
            {
                return smsBcObj.GetStaff_AttendanceReportConfigurationsListBasedOnReportingHeadPreRegNums(ReportingHeadPreRegNums);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public Staff_AttendanceReportConfigurationByStaffs GetStaff_AttendanceReportConfigurationByStaffsById(long Staff_AttendanceReportConfig_Id)
        {
            try
            {
                return smsBcObj.GetStaff_AttendanceReportConfigurationByStaffsById(Staff_AttendanceReportConfig_Id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public Staff_AttendanceReportConfiguration_Vw GetStaff_AttendanceReportConfiguration_VwById(long Id)
        {
            try
            {
                return smsBcObj.GetStaff_AttendanceReportConfiguration_VwById(Id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public long SaveOrUpdateStaff_AttendanceReportConfigurationByStaffs(Staff_AttendanceReportConfigurationByStaffs ConfigObj)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.SaveOrUpdateStaff_AttendanceReportConfigurationByStaffs(ConfigObj);
                return ConfigObj.Staff_AttendanceReportConfig_Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Staff_AttendanceReportConfigurationByStaffs GetStaff_AttendanceReportConfigurationByStaffsByStaffPreRegNumAndReportPreRegNum(long StaffPreRegNum, long ReportingHeadPreRegNum)
        {
            try
            {
                return smsBcObj.GetStaff_AttendanceReportConfigurationByStaffsByStaffPreRegNumAndReportPreRegNum(StaffPreRegNum, ReportingHeadPreRegNum);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public Dictionary<long, IList<Staff_AttendanceReportConfigurationByStaffs>> GetStaff_AttendanceReportConfigurationByStaffsListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaff_AttendanceReportConfigurationByStaffsListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        #region SurveyConfiguration
        public Dictionary<long, IList<SurveyConfiguration>> GetSurveyConfigurationWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetSurveyConfigurationWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateSurveyQuestionMaster(SurveyConfiguration surveyconfig)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.CreateOrUpdateSurveyConfiguration(surveyconfig);
                return surveyconfig.SurveyConfigurationId;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public Dictionary<long, IList<SurveyConfiguration>> GetSurveyConfigurationListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetSurveyConfigurationListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public IList<SurveyConfiguration> SaveOrUpdateSurveyConfigurationByList(IList<SurveyConfiguration> surveyconfiguration)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.SaveOrUpdateSurveyConfigurationByList(surveyconfiguration);
                return surveyconfiguration;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<SurveyConfiguration_vw>> GetSurveyConfiguration_vwWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetSurveyConfiguration_vwWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Staff Programme Mastser by john naveen
        public Dictionary<long, IList<StaffProgrammeMaster>> GetStaffProgrammeMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffProgrammeMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long CreateOrUpdateStaffProgrammeMaster(StaffProgrammeMaster spm)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.CreateOrUpdateStaffProgrammeMaster(spm);
                return spm.StaffProgrammeMatserId;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public bool DeleteStaffProgrammeMasterMaster(long[] id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.DeleteStaffProgrammeMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public StaffProgrammeMaster GetStaffProgrammeMasterByCampusAndStaffType(string Campus, string StaffType, string ProgrammeName, bool IsActive)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffProgrammeMasterByCampusAndStaffType(Campus, StaffType, ProgrammeName, IsActive);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public StaffProgrammeMaster GetStaffProgrammeMasterByStaffProgrammeMasterId(long Id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffProgrammeMasterByStaffProgrammeMasterId(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }

        }
        public IList<StaffProgrammeMaster> SaveOrUpdateStaffProgrammeMasterByList(IList<StaffProgrammeMaster> StaffProgrammeMaster)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.SaveOrUpdateStaffProgrammeMasterByList(StaffProgrammeMaster);
                return StaffProgrammeMaster;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        #region SurveyMaster Krishna_14062017

        public Dictionary<long, IList<SurveyMaster>> GetSurveyListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                //StaffManagementBC objStaffManagementBC = new StaffManagementBC();
                return smsBcObj.GetSurveyDetailsListWithPaging(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public SurveyMaster GetSurveyName(string SurveyName)
        {
            try
            {
                //StaffManagementBC objStaffManagementBC = new StaffManagementBC();
                if (!string.IsNullOrEmpty(SurveyName))
                {
                    return smsBcObj.GetSurveyName(SurveyName);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public long CreateOrUpdateSurveyMaster(SurveyMaster lobjSurveyMaster)
        {
            try
            {
                //StaffManagementBC objStaffManagementBC = new StaffManagementBC();
                smsBcObj.CreateOrUpdateSurveyMaster(lobjSurveyMaster);
                return lobjSurveyMaster.SurveyId;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public SurveyMaster GetSurveyId(long SurveyId)
        {
            try
            {
                if (SurveyId > 0)
                {
                    return smsBcObj.GetAssetBrandMasterByBrandMasterId(SurveyId);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }


        public bool DeleteSurveyMaster(long[] id)
        {
            try
            {
                smsBcObj.DeleteSurveyMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        #endregion
        #region Survey Result
        public Dictionary<long, IList<StaffWiseSurveyNewResult_Vw>> GetStaffWiseSurveyList(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffWiseSurveyList(page, pageSize, sortby, sortType, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<SurveyReportNew_Vw>> GetSurveyReportNew_VwStudentCountList(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetSurveyReportNew_VwStudentCountList(page, pageSize, sortby, sortType, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public SurveyReportNew_Vw GetSurveyReportById(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    StaffManagementBC SBC = new StaffManagementBC();
                    return SBC.GetSurveyReportById(Id);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<StaffwiseSurveyQuestionReportNew_Vw>> GetStudentSurveyQuestionMarkNewList(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStudentSurveyQuestionMarkNewList(page, pageSize, sortby, sortType, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public StaffWiseSurveyNewResult_Vw GetStaffWiseSurveyNewResultById(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    StaffManagementBC SBC = new StaffManagementBC();
                    return SBC.GetStaffWiseSurveyNewResultById(Id);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<StaffWiseSurveyNewResultWOS_Vw>> GetStaffWiseSurveyWOSList(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffWiseSurveyWOSList(page, pageSize, sortby, sortType, criteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public StaffWiseSurveyNewResultWOS_Vw StaffWiseSurveyNewResultWOSById(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    StaffManagementBC SBC = new StaffManagementBC();
                    return SBC.StaffWiseSurveyNewResultWOSById(Id);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion        
        public Dictionary<long, IList<SurveyReportNew_SP>> GetSurveyReportNew_SPListbySP(string Campus, string Grade, string Section, string AcademicYear, string CategoryName, long StaffEvaluationCategoryId, long StaffPreRegNumber, long CampusBasedStaffDetails_Id)
        {
            try
            {
                StaffManagementBC SBC = new StaffManagementBC();
                return SBC.GetSurveyReportNew_SPListbySP(Campus, Grade, Section, AcademicYear,CategoryName ,StaffEvaluationCategoryId, StaffPreRegNumber, CampusBasedStaffDetails_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region Staff_WorkingDaysMaster
        public Dictionary<long, IList<Staff_WorkingDaysMaster>> GetStaff_WorkingDaysMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaff_WorkingDaysMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateStaff_WorkingDaysMaster(Staff_WorkingDaysMaster workingMasterObj)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.SaveOrUpdateStaff_WorkingDaysMaster(workingMasterObj);
                return workingMasterObj.Staff_WorkingDaysMaster_Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteStaff_WorkingDaysMaster(long[] id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.DeleteStaff_WorkingDaysMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Staff_WorkingDaysMaster GetStaff_WorkingDaysMasterByCampusAndStaffType(string Campus, string StaffType, long Month, long Year)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaff_WorkingDaysMasterByCampusAndStaffType(Campus, StaffType, Month, Year);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Staff_WorkingDaysMaster GetStaff_WorkingDaysMasterByStaff_WorkingDaysMaster_Id(long Staff_WorkingDaysMaster_Id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaff_WorkingDaysMasterByStaff_WorkingDaysMaster_Id(Staff_WorkingDaysMaster_Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public IList<Staff_WorkingDaysMaster> SaveOrUpdateStaff_WorkingDaysMasterByList(IList<Staff_WorkingDaysMaster> StaffWorkingDaysMaster)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.SaveOrUpdateStaff_WorkingDaysMasterByList(StaffWorkingDaysMaster);
                return StaffWorkingDaysMaster;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        #region Staff Attendance Change Details
        public Dictionary<long, IList<Staff_AttendanceChangeDetails>> GetStaff_AttendanceChangeDetailsListWithCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return smsBcObj.GetStaff_AttendanceChangeDetailsListWithCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long SaveOrUpdateStaff_AttendanceChangeDetails(Staff_AttendanceChangeDetails Staff_AttendanceChangeDetails)
        {
            try
            {
                smsBcObj.SaveOrUpdateStaff_AttendanceChangeDetails(Staff_AttendanceChangeDetails);
                return Staff_AttendanceChangeDetails.Staff_AttendanceChangeDetails_Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Staff_AttendanceChangeDetails GetStaff_AttendanceChangeDetailsByPreRegNumAndMonthYear(long PreRegNum, long Month, long Year, bool IsActive)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaff_AttendanceChangeDetailsByPreRegNumAndMonthYear(PreRegNum, Month, Year, IsActive);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        #region Staff Holidays Master
        public Dictionary<long, IList<StaffHolidaysMaster>> GetStaffHolidaysMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffHolidaysMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateStaffHolidaysMaster(StaffHolidaysMaster StaffHolidaysMaster)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.SaveOrUpdateStaffHolidaysMaster(StaffHolidaysMaster);
                return StaffHolidaysMaster.StaffHolidaysMaster_Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteStaffHolidaysMaster(long[] id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.DeleteStaffHolidaysMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public StaffHolidaysMaster GetStaffHolidaysMasterByAcademicYearAndMonth(long Year, long MonthNumber, string HolidayType, string Campus)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffHolidaysMasterByAcademicYearAndMonth(Year, MonthNumber, HolidayType, Campus);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public StaffHolidaysMaster GetStaffHolidaysMasterByAcademicYearAndMonthCampus(long Year, long Month, string Campus)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffHolidaysMasterByAcademicYearAndMonthCampus(Year, Month, Campus);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        #region staff CL Balance Details
        public Dictionary<long, IList<Staff_AttendanceCLDetails>> GetStaff_AttendanceCLDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaff_AttendanceCLDetailsListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Staff_AttendanceCLDetails GetStaff_AttendanceCLDetailsByPreRegNum(long Month, long Year, long PreRegNum, bool IsActive)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaff_AttendanceCLDetailsByPreRegNum(Month, Year, PreRegNum, IsActive);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long SaveOrUpdateStaff_AttendanceCLDetails(Staff_AttendanceCLDetails StaffAttendanceCLDetails)
        {
            try
            {
                smsBcObj.SaveOrUpdateStaff_AttendanceCLDetails(StaffAttendanceCLDetails);
                return StaffAttendanceCLDetails.Staff_AttendanceCLBalance_Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public IList<Staff_AttendanceCLDetails> SaveOrUpdateStaffAttendanceCLDetailsByList(IList<Staff_AttendanceCLDetails> StaffAttendanceCLDetails)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.SaveOrUpdateStaffAttendanceCLDetailsByList(StaffAttendanceCLDetails);
                return StaffAttendanceCLDetails;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        #region Casual Leaves  Master
        public Dictionary<long, IList<CLDetailsMaster>> GetCLDetailsMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetCLDetailsMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public CLDetailsMaster GetCLDetailsMasterByPreRegNum(long Month, long Year, long PreRegNum)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetCLDetailsMasterByPreRegNum(Month, Year, PreRegNum);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long SaveOrUpdateCLDetailsMaster(CLDetailsMaster CLDetailsMaster)
        {
            try
            {
                smsBcObj.SaveOrUpdateStaff_AttendanceCLDetails(CLDetailsMaster);
                return CLDetailsMaster.CLDetails_Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public IList<CLDetailsMaster> SaveOrUpdateCLDetailsMasterByList(IList<CLDetailsMaster> clDetailsMaster)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.SaveOrUpdateCLDetailsMasterByList(clDetailsMaster);
                return clDetailsMaster;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public CLDetailsMaster GetCLDetailsMasterByMonthYear(long Month, long Year)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetCLDetailsMasterByMonthYear(Month, Year);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        #region Staff Attendance Opening Balance Generate Count
        public long SaveOrUpdateStaffAttendanceOpeningBalanceGenerateCount(Staff_AttendanceOpeningBalanceGenerateCount obj)
        {
            try
            {
                smsBcObj.SaveOrUpdateStaffAttendanceOpeningBalanaceGenerateCount(obj);
                return obj.Id;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Staff_AttendanceOpeningBalanceGenerateCount GetStaffAttendanceOpeningBalanceGenerateCountByMonth(long Month, long Year)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffAttendanceOpeningBalanceGenerateCountByMonth(Month, Year);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        public StaffDetails GetStaffDetailsViewByPreRegNumAndStatus(Int32 PreRegNum, string Status)
        {
            try
            {
                return smsBcObj.GetStaffDetailsViewByPreRegNumAndStatus(PreRegNum, Status);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public StaffDetailsView GetStaffDetailsViewByStatus(Int64 PreRegNum)
        {
            try
            {
                return smsBcObj.GetStaffDetailsViewByStatus(PreRegNum);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #region Staff Attendance New Status
        public Dictionary<long, IList<StaffAttendanceNewStatus>> GetStaffAttendanceNewStatusListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaffAttendanceNewStatusListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateStaffAttendanceNewStatus(StaffAttendanceNewStatus obj)
        {
            try
            {
                smsBcObj.SaveOrUpdateStaffAttendanceNewStatus(obj);
                return obj.StaffStatus_Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public StaffAttendanceNewStatus GetStaffAttendanceNewStatusByPreRegNum(long PreRegNum)
        {
            try
            {
                return smsBcObj.GetStaffAttendanceNewStatusByPreRegNum(PreRegNum);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Staff_AttendanceConfigurationsReport
        public Dictionary<long, IList<Staff_AttendanceConfigurationsReport_Vw>> GetStaff_AttendanceConfigurationsReport_VwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> ExactCriteria, Dictionary<string, object> LikeCriteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetStaff_AttendanceConfigurationsReport_VwListWithPagingAndCriteria(page, pageSize, sortby, sortType, ExactCriteria, LikeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public bool UpdateStaffBiometricIdByIdNumberToEmployeeCode(string[] IdNumberToEmployeeCodeArray)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.UpdateStaffBiometricIdByIdNumberToEmployeeCode(IdNumberToEmployeeCodeArray);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion

        #region CandidateDetails
        public Dictionary<long, IList<CandidateDtls>> GetCandidateList_vwStatusListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetCandidateList_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateCandidateList_vw(CandidateDtls obj)
        {
            try
            {
                smsBcObj.SaveOrUpdateCandidateList_vw(obj);
                return obj.PreRegNum;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public CandidateDtls GetCandidateDtlsId(long Id)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                return StaffManagementBC.GetCandidateDtlsId(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateCandidateDtls(CandidateDtls sd)
        {
            try
            {
                StaffManagementBC StaffManagementBC = new StaffManagementBC();
                StaffManagementBC.CreateOrUpdateCandidateDtls(sd);
                return sd.Id;
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
