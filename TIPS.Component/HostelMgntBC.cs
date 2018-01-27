using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TIPS.Entities.HostelMgntEntities;
using PersistenceFactory;

namespace TIPS.Component
{
    public class HostelMgntBC
    {
        PersistenceServiceFactory PSF = null;
        public HostelMgntBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }
        public long CreateOrUpdateHstMgntAdmissionForm(HstMgntAdmissionForm hstmngObj)
        {
            try
            {
                if (hstmngObj != null)
                    PSF.SaveOrUpdate<HstMgntAdmissionForm>(hstmngObj);
                else { throw new Exception("HstMgntAdmissionForm is required and it cannot be null.."); }
                return hstmngObj.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public HstMgntAdmissionForm GetHstMgntAdmissionFormById(long Id)
        {
            try
            {

                HstMgntAdmissionForm hstMgntObj = null;
                if (Id > 0)
                    hstMgntObj = PSF.Get<HstMgntAdmissionForm>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return hstMgntObj;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<HstlMgmt_HostelMaster>> GetHstMgntAdmissionFormListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<HstlMgmt_HostelMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }



        public Dictionary<long, IList<HstlMgmt_BedMaster>> GetHstlMgmt_BedMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<HstlMgmt_BedMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }



        #region "Landing Page"
        public HostelMasters_Vw GetHostelMasters_VwById(long Id)
        {
            try
            {

                HostelMasters_Vw hstMgntObj = null;
                if (Id > 0)
                    hstMgntObj = PSF.Get<HostelMasters_Vw>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return hstMgntObj;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<HostelMasters_Vw>> GetHostelMasters_VwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<HostelMasters_Vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<HostelMasters_Vw>> GetHostelMasters_VwLIKEListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<HostelMasters_Vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<StudHostelDtls_Vw>> GetStudHostelDtls_VwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<StudHostelDtls_Vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<HstlMgmt_RoomMaster>> GetHstlMgmt_RoomMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<HstlMgmt_RoomMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<HstMgnt_StudStaffNames_Vw>> GetHstMgnt_StudStaffNames_VwListWithPagingAndCriteriaLikeSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<HstMgnt_StudStaffNames_Vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateHstlMgmt_HostelDetails(HstlMgmt_HostelDetails hstmngObj)
        {
            try
            {
                if (hstmngObj != null)
                    PSF.SaveOrUpdate<HstlMgmt_HostelDetails>(hstmngObj);
                else { throw new Exception("HstMgntAdmissionForm is required and it cannot be null.."); }
                return hstmngObj.HostDts_Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public long CreateOrUpdateChangeRoomAllotment(ChangeRoomAllotment hstmngObj)
        {
            try
            {
                if (hstmngObj != null)
                    PSF.SaveOrUpdate<ChangeRoomAllotment>(hstmngObj);
                else { throw new Exception("HstMgntAdmissionForm is required and it cannot be null.."); }
                return hstmngObj.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion "End"

        #region "Modify Existing Allocation"
        public HstlMgmt_HostelDetails GetHstlMgmt_HostelDetailsbyId(long Id)
        {
            try
            {
                HstlMgmt_HostelDetails hstObj = null;
                if (Id > 0)
                    hstObj = PSF.Get<HstlMgmt_HostelDetails>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return hstObj;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long GetHosteIdValuebyHostelNameandType(string HstName, string HstType)
        {
            try
            {
                HostelMasters_Vw hsmas = PSF.Get<HostelMasters_Vw>("HostelName", HstName, "HostelType", HstType);
                return hsmas.HstlMst_Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion "End"

        #region "MASTER DETAILS"

        public long CreateOrUpdateHstlMgmt_HostelMaster(HstlMgmt_HostelMaster hstmngObj)
        {
            try
            {
                if (hstmngObj != null)
                    PSF.SaveOrUpdate<HstlMgmt_HostelMaster>(hstmngObj);
                else { throw new Exception("HstlMgmt_HostelMaster is required and it cannot be null.."); }
                return hstmngObj.HstlMst_Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion " END "

        public void DeleteRoomMasterAll(List<HstlMgmt_RoomMaster> list)
        {
            PSF.DeleteAll(list);
        }

        public void DeleteBedMasterAll(List<HstlMgmt_BedMaster> list)
        {
            PSF.DeleteAll(list);
        }

        public void DeleteHostalMasterAll(List<HstlMgmt_HostelMaster> list)
        {
            PSF.DeleteAll(list);
        }

        public void DeleteHstlMgmt_HostelDetailsAll(List<HstlMgmt_HostelDetails> list)
        {
            PSF.DeleteAll(list);
        }

        public long CreateOrUpdateHstlMgmt_RoomMaster(HstlMgmt_RoomMaster roomObj)
        {
            try
            {
                if (roomObj != null)
                    PSF.SaveOrUpdate<HstlMgmt_RoomMaster>(roomObj);
                else { throw new Exception("HstlMgmt_RoomMaster is required and it cannot be null.."); }
                return roomObj.HstlMst_Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long CreateOrUpdateHstlMgmt_BedMaster(HstlMgmt_BedMaster bedObj)
        {
            try
            {
                if (bedObj != null)
                    PSF.SaveOrUpdate<HstlMgmt_BedMaster>(bedObj);
                else { throw new Exception("HstlMgmt_BedMaster is required and it cannot be null.."); }
                return bedObj.BedMst_Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public HstlMgmt_RoomMaster GetHstlMgmt_RoomMasterbyId(long Id)
        {
            try
            {

                HstlMgmt_RoomMaster hstMgntObj = null;
                if (Id > 0)
                    hstMgntObj = PSF.Get<HstlMgmt_RoomMaster>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return hstMgntObj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public HstlMgmt_BedMaster GetHstlMgmt_BedMasterbyId(long Id)
        {
            try
            {

                HstlMgmt_BedMaster hstMgntObj = null;
                if (Id > 0)
                    hstMgntObj = PSF.Get<HstlMgmt_BedMaster>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return hstMgntObj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public HstlMgmt_HostelMaster GetHstlMgmt_HostelMasterbyId(long Id)
        {
            try
            {

                HstlMgmt_HostelMaster hstMgntObj = null;
                if (Id > 0)
                    hstMgntObj = PSF.Get<HstlMgmt_HostelMaster>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return hstMgntObj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public HstlMgmt_HostelDetails GetHstlMgmt_HostelDetailsbyStudentId(long Id)
        {
            try
            {
                HstlMgmt_HostelDetails hstObj = null;
                if (Id > 0)
                    hstObj = PSF.Get<HstlMgmt_HostelDetails>("Stud_Id", Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return hstObj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<HstlMgmt_HostelDetails>> GetHstlMgmt_HostelDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<HstlMgmt_HostelDetails>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public Dictionary<long, IList<RoomAllotment>> GetRoomAllotmentListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<RoomAllotment>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<RoomAllotment>> GetRoomAllotmentLIKEListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<RoomAllotment>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<HstMgnt_MaterialDamage>> GetHstMgnt_MaterialDamageLIKEListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<HstMgnt_MaterialDamage>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<HstMgnt_MaterialDamage_Vw>> GetHstMgnt_MaterialDamage_VwLIKEListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<HstMgnt_MaterialDamage_Vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RemoveExistingRoomInfo(HstlMgmt_HostelDetails hsRec)
        {
            try
            {
                if (hsRec.HostDts_Id > 0)
                    PSF.Delete<HstlMgmt_HostelDetails>(hsRec);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateHstMgnt_MaterialDamage(HstMgnt_MaterialDamage hstmngObj)
        {
            try
            {
                if (hstmngObj != null)
                    PSF.SaveOrUpdate<HstMgnt_MaterialDamage>(hstmngObj);
                else { throw new Exception("HstlMgmt_HostelMaster is required and it cannot be null.."); }
                return hstmngObj.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public HstMgnt_MaterialDamage GetHstMgnt_MaterialDamageById(long Id)
        {
            try
            {
                HstMgnt_MaterialDamage hstObj = null;
                if (Id > 0)
                    hstObj = PSF.Get<HstMgnt_MaterialDamage>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return hstObj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteHstMgnt_MaterialDamage(HstMgnt_MaterialDamage hsObj)
        {
            try
            {
                if (hsObj.Id > 0)
                    PSF.Delete<HstMgnt_MaterialDamage>(hsObj);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<HstlMgmt_MedicalExpenses_Vw>> GetHstlMgmt_MedicalExpenses_VwLIKEListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<HstlMgmt_MedicalExpenses_Vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateHstlMgmt_MedicalExpenses(HstlMgmt_MedicalExpenses hstmngObj)
        {
            try
            {
                if (hstmngObj != null)
                    PSF.SaveOrUpdate<HstlMgmt_MedicalExpenses>(hstmngObj);
                else { throw new Exception("HstlMgmt_HostelMaster is required and it cannot be null.."); }
                return hstmngObj.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public HstlMgmt_MedicalExpenses GetHstlMgmt_MedicalExpensesById(long Id)
        {
            try
            {
                HstlMgmt_MedicalExpenses hstObj = null;
                if (Id > 0)
                    hstObj = PSF.Get<HstlMgmt_MedicalExpenses>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return hstObj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteHstlMgmt_MedicalExpenses(HstlMgmt_MedicalExpenses hsObj)
        {
            try
            {
                if (hsObj.Id > 0)
                    PSF.Delete<HstlMgmt_MedicalExpenses>(hsObj);
            }
            catch (Exception)
            {
                throw;
            }
        }



        #region " Food Expenses"
        public Dictionary<long, IList<HstlMgmt_FoodExpenses_Vw>> GetHstlMgmt_FoodExpenses_VwLIKEListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<HstlMgmt_FoodExpenses_Vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public long CreateOrUpdateHstlMgmt_FoodExpenses(HstlMgmt_FoodExpenses hstmngObj)
        {
            try
            {
                if (hstmngObj != null)
                    PSF.SaveOrUpdate<HstlMgmt_FoodExpenses>(hstmngObj);
                else { throw new Exception("HstlMgmt_FoodExpenses is required and it cannot be null.."); }
                return hstmngObj.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public HstlMgmt_FoodExpenses GetHstlMgmt_FoodExpensesById(long Id)
        {
            try
            {
                HstlMgmt_FoodExpenses hstObj = null;
                if (Id > 0)
                    hstObj = PSF.Get<HstlMgmt_FoodExpenses>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return hstObj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteHstlMgmt_FoodExpenses(HstlMgmt_FoodExpenses hsObj)
        {
            try
            {
                if (hsObj.Id > 0)
                    PSF.Delete<HstlMgmt_FoodExpenses>(hsObj);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion " END "

    }
}
