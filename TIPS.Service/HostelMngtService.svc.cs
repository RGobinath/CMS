using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.Component;
using TIPS.Entities.HostelMgntEntities;

namespace TIPS.ServiceContract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "HostelMngt" in code, svc and config file together.
    public class HostelMngtService : IHostelMgnt
    {
        public long CreateOrUpdateHstMgntAdmissionForm(HstMgntAdmissionForm hmgntObj)
        {
            try
            {
                HostelMgntBC hostelMgnt = new HostelMgntBC();
                hostelMgnt.CreateOrUpdateHstMgntAdmissionForm(hmgntObj);
                return hmgntObj.Id;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public HstMgntAdmissionForm GetHstMgntAdmissionFormById(long Id)
        {
            try
            {
                HostelMgntBC hostelMgnt = new HostelMgntBC();
                return hostelMgnt.GetHstMgntAdmissionFormById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<HstlMgmt_HostelMaster>> GetHstMgntAdmissionFormListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    HostelMgntBC hostelMgnt = new HostelMgntBC();
                    return hostelMgnt.GetHstMgntAdmissionFormListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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

        public Dictionary<long, IList<HstlMgmt_BedMaster>> GetHstlMgmt_BedMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    HostelMgntBC hostelMgnt = new HostelMgntBC();
                    return hostelMgnt.GetHstlMgmt_BedMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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



        #region "Landing Page"
        public HostelMasters_Vw GetHostelMasters_VwById(long Id)
        {
            try
            {
                HostelMgntBC hostelMgnt = new HostelMgntBC();
                return hostelMgnt.GetHostelMasters_VwById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<HostelMasters_Vw>> GetHostelMasters_VwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    HostelMgntBC hostelMgnt = new HostelMgntBC();
                    return hostelMgnt.GetHostelMasters_VwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
        public Dictionary<long, IList<HostelMasters_Vw>> GetHostelMasters_VwLIKEListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    HostelMgntBC hostelMgnt = new HostelMgntBC();
                    return hostelMgnt.GetHostelMasters_VwLIKEListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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

        public Dictionary<long, IList<StudHostelDtls_Vw>> GetStudHostelDtls_VwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    HostelMgntBC hostelMgnt = new HostelMgntBC();
                    return hostelMgnt.GetStudHostelDtls_VwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
        public Dictionary<long, IList<HstlMgmt_RoomMaster>> GetHstlMgmt_RoomMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    HostelMgntBC hostelMgnt = new HostelMgntBC();
                    return hostelMgnt.GetHstlMgmt_RoomMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
        public Dictionary<long, IList<HstMgnt_StudStaffNames_Vw>> GetHstMgnt_StudStaffNames_VwListWithPagingAndCriteriaLikeSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                HostelMgntBC hostelMgnt = new HostelMgntBC();
                return hostelMgnt.GetHstMgnt_StudStaffNames_VwListWithPagingAndCriteriaLikeSearch(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateHstlMgmt_HostelDetails(HstlMgmt_HostelDetails hmgntObj)
        {
            try
            {
                HostelMgntBC hostelMgnt = new HostelMgntBC();
                hostelMgnt.CreateOrUpdateHstlMgmt_HostelDetails(hmgntObj);
                return hmgntObj.HostDts_Id;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long CreateOrUpdateChangeRoomAllotment(ChangeRoomAllotment hmgntObj)
        {
            try
            {
                HostelMgntBC hostelMgnt = new HostelMgntBC();
                hostelMgnt.CreateOrUpdateChangeRoomAllotment(hmgntObj);
                return hmgntObj.Id;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion "End"


        #region "Modify Existing Allocation"

        public HstlMgmt_HostelDetails GetHstlMgmt_HostelDetailsbyId(long Id)
        {
            try
            {
                HostelMgntBC hostelMgnt = new HostelMgntBC();
                return hostelMgnt.GetHstlMgmt_HostelDetailsbyId(Id);

            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long GetHosteIdValuebyHostelNameandType(string HstName, string HstType)
        {
            try
            {
                HostelMgntBC hostelMgnt = new HostelMgntBC();
                return hostelMgnt.GetHosteIdValuebyHostelNameandType(HstName, HstType);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion "End"

        #region "MASTER DETAILS"

        public long CreateOrUpdateHstlMgmt_HostelMaster(HstlMgmt_HostelMaster hmgntObj)
        {
            try
            {
                HostelMgntBC hostelMgnt = new HostelMgntBC();
                hostelMgnt.CreateOrUpdateHstlMgmt_HostelMaster(hmgntObj);
                return hmgntObj.HstlMst_Id;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        #endregion " END "


        public void DeleteRoomMasterAll(List<HstlMgmt_RoomMaster> list)
        {
            HostelMgntBC hostelMgnt = new HostelMgntBC();
            hostelMgnt.DeleteRoomMasterAll(list);
        }

        public void DeleteBedMasterAll(List<HstlMgmt_BedMaster> list)
        {
            HostelMgntBC hostelMgnt = new HostelMgntBC();
            hostelMgnt.DeleteBedMasterAll(list);
        }

        public void DeleteHostalMasterAll(List<HstlMgmt_HostelMaster> list)
        {
            HostelMgntBC hostelMgnt = new HostelMgntBC();
            hostelMgnt.DeleteHostalMasterAll(list);
        }

        public long CreateOrUpdateHstlMgmt_RoomMaster(HstlMgmt_RoomMaster roomObj)
        {
            try
            {
                HostelMgntBC hostelMgnt = new HostelMgntBC();
                hostelMgnt.CreateOrUpdateHstlMgmt_RoomMaster(roomObj);
                return roomObj.HstlMst_Id;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateHstlMgmt_BedMaster(HstlMgmt_BedMaster bedObj)
        {
            try
            {
                HostelMgntBC hostelMgnt = new HostelMgntBC();
                hostelMgnt.CreateOrUpdateHstlMgmt_BedMaster(bedObj);
                return bedObj.BedMst_Id;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public HstlMgmt_BedMaster GetHstlMgmt_BedMasterbyId(long Id)
        {
            try
            {
                HostelMgntBC hostelMgnt = new HostelMgntBC();
                return hostelMgnt.GetHstlMgmt_BedMasterbyId(Id);

            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }


        public HstlMgmt_RoomMaster GetHstlMgmt_RoomMasterbyId(long Id)
        {
            try
            {
                HostelMgntBC hostelMgnt = new HostelMgntBC();
                return hostelMgnt.GetHstlMgmt_RoomMasterbyId(Id);

            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public HstlMgmt_HostelMaster GetHstlMgmt_HostelMasterbyId(long Id)
        {
            try
            {
                HostelMgntBC hostelMgnt = new HostelMgntBC();
                return hostelMgnt.GetHstlMgmt_HostelMasterbyId(Id);

            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public HstlMgmt_HostelDetails GetHstlMgmt_HostelDetailsbyStudentId(long Id)
        {
            try
            {
                HostelMgntBC hostelMgnt = new HostelMgntBC();
                return hostelMgnt.GetHstlMgmt_HostelDetailsbyStudentId(Id);

            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<HstlMgmt_HostelDetails>> GetHstlMgmt_HostelDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    HostelMgntBC hostelMgnt = new HostelMgntBC();
                    return hostelMgnt.GetHstlMgmt_HostelDetailsListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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



        public Dictionary<long, IList<RoomAllotment>> GetRoomAllotmentListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    HostelMgntBC hostelMgnt = new HostelMgntBC();
                    return hostelMgnt.GetRoomAllotmentListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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

        public Dictionary<long, IList<RoomAllotment>> GetRoomAllotmentLIKEListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    HostelMgntBC hostelMgnt = new HostelMgntBC();
                    return hostelMgnt.GetRoomAllotmentLIKEListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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

        public Dictionary<long, IList<HstMgnt_MaterialDamage>> GetHstMgnt_MaterialDamageLIKEListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    HostelMgntBC hostelMgnt = new HostelMgntBC();
                    return hostelMgnt.GetHstMgnt_MaterialDamageLIKEListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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

        public Dictionary<long, IList<HstMgnt_MaterialDamage_Vw>> GetHstMgnt_MaterialDamage_VwLIKEListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    HostelMgntBC hostelMgnt = new HostelMgntBC();
                    return hostelMgnt.GetHstMgnt_MaterialDamage_VwLIKEListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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


        public void RemoveExistingRoomInfo(HstlMgmt_HostelDetails hsRec)
        {
            try
            {
                HostelMgntBC hostelMgnt = new HostelMgntBC();
                hostelMgnt.RemoveExistingRoomInfo(hsRec);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long CreateOrUpdateHstMgnt_MaterialDamage(HstMgnt_MaterialDamage hmgntObj)
        {
            try
            {
                HostelMgntBC hostelMgnt = new HostelMgntBC();
                hostelMgnt.CreateOrUpdateHstMgnt_MaterialDamage(hmgntObj);
                return hmgntObj.Id;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }


        public HstMgnt_MaterialDamage GetHstMgnt_MaterialDamageById(long Id)
        {
            try
            {
                HostelMgntBC hostelMgnt = new HostelMgntBC();
                return hostelMgnt.GetHstMgnt_MaterialDamageById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public void DeleteHstMgnt_MaterialDamage(HstMgnt_MaterialDamage hsObj)
        {
            try
            {
                HostelMgntBC hostelMgnt = new HostelMgntBC();
                hostelMgnt.DeleteHstMgnt_MaterialDamage(hsObj);
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
                try
                {
                    HostelMgntBC hostelMgnt = new HostelMgntBC();
                    return hostelMgnt.GetHstlMgmt_MedicalExpenses_VwLIKEListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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

        public long CreateOrUpdateHstlMgmt_MedicalExpenses(HstlMgmt_MedicalExpenses hmgntObj)
        {
            try
            {
                HostelMgntBC hostelMgnt = new HostelMgntBC();
                hostelMgnt.CreateOrUpdateHstlMgmt_MedicalExpenses(hmgntObj);
                return hmgntObj.Id;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public HstlMgmt_MedicalExpenses GetHstlMgmt_MedicalExpensesById(long Id)
        {
            try
            {
                HostelMgntBC hostelMgnt = new HostelMgntBC();
                return hostelMgnt.GetHstlMgmt_MedicalExpensesById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public void DeleteHstlMgmt_MedicalExpenses(HstlMgmt_MedicalExpenses hsObj)
        {
            try
            {
                HostelMgntBC hostelMgnt = new HostelMgntBC();
                hostelMgnt.DeleteHstlMgmt_MedicalExpenses(hsObj);
            }
            catch (Exception)
            {

                throw;
            }
        }


        #region "Food Expenses"
        public Dictionary<long, IList<HstlMgmt_FoodExpenses_Vw>> GetHstlMgmt_FoodExpenses_VwLIKEListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    HostelMgntBC hostelMgnt = new HostelMgntBC();
                    return hostelMgnt.GetHstlMgmt_FoodExpenses_VwLIKEListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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

        public long CreateOrUpdateHstlMgmt_FoodExpenses(HstlMgmt_FoodExpenses hmgntObj)
        {
            try
            {
                HostelMgntBC hostelMgnt = new HostelMgntBC();
                hostelMgnt.CreateOrUpdateHstlMgmt_FoodExpenses(hmgntObj);
                return hmgntObj.Id;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public HstlMgmt_FoodExpenses GetHstlMgmt_FoodExpensesById(long Id)
        {
            try
            {
                HostelMgntBC hostelMgnt = new HostelMgntBC();
                return hostelMgnt.GetHstlMgmt_FoodExpensesById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }


        public void DeleteHstlMgmt_FoodExpenses(HstlMgmt_FoodExpenses hsObj)
        {
            try
            {
                HostelMgntBC hostelMgnt = new HostelMgntBC();
                hostelMgnt.DeleteHstlMgmt_FoodExpenses(hsObj);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion " END "
    }
}
