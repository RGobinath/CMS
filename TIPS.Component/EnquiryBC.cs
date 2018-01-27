using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TIPS.Entities.EnquiryEntities;
using PersistenceFactory;
using System.Collections;

namespace TIPS.Component
{
    public class EnquiryBC
    {
        PersistenceServiceFactory PSF = null;
        public EnquiryBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }
        public Dictionary<long, IList<FollowUpDetails>> GetFollowUpDetailsWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<FollowUpDetails>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public EnquiryDetails GetEnquiryDetailsById(long Id)
        {
            try
            {
                EnquiryDetails EnquiryDetails = null;
                if (Id > 0)
                    EnquiryDetails = PSF.Get<EnquiryDetails>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return EnquiryDetails;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string SaveOrUpdateEnquiryDetails(EnquiryDetails EnquiryDetails)
        {
            try
            {
                //logic to check before saving
                if (EnquiryDetails != null && EnquiryDetails.EnquiryDetailsId > 0)
                { PSF.Update<EnquiryDetails>(EnquiryDetails); }
                else
                {
                    PSF.SaveOrUpdate<EnquiryDetails>(EnquiryDetails);
                    EnquiryDetails.EnquiryDetailsCode = "ENQ-" + EnquiryDetails.EnquiryDetailsId + "";
                    PSF.SaveOrUpdate<EnquiryDetails>(EnquiryDetails);
                }
                return EnquiryDetails.EnquiryDetailsCode;
            }
            catch (Exception)
            { throw; }
        }

        public long SaveOrUpdateFollowUpDetails(FollowUpDetails FollowUpDetails)
        {
            try
            {
                //logic to check before saving
                if (FollowUpDetails != null && FollowUpDetails.EnquiryDetailsId > 0)
                { PSF.SaveOrUpdate<FollowUpDetails>(FollowUpDetails); }
                else
                { PSF.SaveOrUpdate<FollowUpDetails>(FollowUpDetails); }
                return FollowUpDetails.FollowUpDetailsId;
            }
            catch (Exception)
            { throw; }
        }

        public long FollowUpIdCount(long Id)
        {
            try
            {
                if (Id != null)
                {
                    string query = "";
                    query = "select count(EnquiryDetailsId) from EnqFollowUpDetails where EnquiryDetailsId='" + Id + "'";
                    IList list = PSF.ExecuteSql(query);
                    return Convert.ToInt64(list[0].ToString()); //list[0] = "0";
                }
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<EnquiryDetailGridView>> GetEnquiryDetailsGridViewListWithPagingAndCriteriaEQSearch(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchWithArrayCriteriaCount<EnquiryDetailGridView>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<EnquiryDetailGridView>> GetEnquiryDetailsGridViewListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<EnquiryDetailGridView>(page, pageSize,sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<EnquiryDetails>> GetEnquiryDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<EnquiryDetails>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<EnquiryDetails>> GetEnquiryDetailsListWithPagingAndCriteriaEQSearch(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<EnquiryDetails>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<EnquiryCourse>> GetEnquiryCourseDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<EnquiryCourse>> pastdet = new Dictionary<long, IList<EnquiryCourse>>();
                return PSF.GetListWithExactSearchArrayCriteriaCount<EnquiryCourse>(page, pageSize, sortBy,sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string CreateOrUpdateEnquiryCourse(EnquiryCourse Ec, string userid)
        {
            try
            {
                //logic to check before saving
                if (Ec != null && Ec.Id > 0)
                {
                    Ec.ModifiedBy = userid;
                    Ec.ModifiedDate = DateTime.Now;
                    PSF.Update<EnquiryCourse>(Ec);
                }
                else
                {

                    Ec.CreatedBy = userid;
                    Ec.CreatedDate = DateTime.Now;
                    PSF.SaveOrUpdate<EnquiryCourse>(Ec);
                }
                return null ;
            }
            catch (Exception)
            { throw; }
        }
        #region KioskEnquiryDetailsSave
        public string SaveOrUpdateKioskEnquiryDetails(KioskEnquiryDetails KioskEnquiryDetails)
        {
            try
            {
                if (KioskEnquiryDetails != null && KioskEnquiryDetails.Enq_Id > 0)
                { PSF.SaveOrUpdate<KioskEnquiryDetails>(KioskEnquiryDetails); }
                KioskEnquiryDetails.Enq_Number = "ENQ-" + KioskEnquiryDetails.Enq_Id + "";
                PSF.SaveOrUpdate<KioskEnquiryDetails>(KioskEnquiryDetails);

                return Convert.ToString(KioskEnquiryDetails.Enq_Id);
            }
            catch (Exception)
            { throw; }
        }
        #endregion
        #region BasicDetailsSave
        public string SaveOrUpdateKioskBasicDetails(KioskBasicDetails KioskEnquiryDetails)
        {
            try
            {
                if (KioskEnquiryDetails != null && KioskEnquiryDetails.Id > 0)
                { PSF.SaveOrUpdate<KioskBasicDetails>(KioskEnquiryDetails); }
                PSF.SaveOrUpdate<KioskBasicDetails>(KioskEnquiryDetails);

                return Convert.ToString(KioskEnquiryDetails.Id);
            }
            catch (Exception)
            { throw; }
        }
        #endregion
       
    }
}
