using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.Entities.EnquiryEntities;
using TIPS.Component;

namespace TIPS.ServiceContract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EnquiryService" in code, svc and config file together.
    public class EnquiryService : IEnquiryService
    {
        EnquiryBC EnqBc = new EnquiryBC();
        public Dictionary<long, IList<FollowUpDetails>> GetFollowUpDetailsWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<FollowUpDetails>> retValue = EnqBc.GetFollowUpDetailsWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public EnquiryDetails GetEnquiryDetailsById(long Id)
        {
            try
            {
                return EnqBc.GetEnquiryDetailsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public string SaveOrUpdateEnquiryDetails(EnquiryDetails EnquiryDetails)
        {

            try
            {
                return EnqBc.SaveOrUpdateEnquiryDetails(EnquiryDetails);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long SaveOrUpdateFollowUpDetails(FollowUpDetails FollowUpDetails)
        {

            try
            {
                return EnqBc.SaveOrUpdateFollowUpDetails(FollowUpDetails);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long FollowUpIdCount(long Id)
        {
            try
            {
                return EnqBc.FollowUpIdCount(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<EnquiryDetailGridView>> GetEnquiryDetailsGridViewListWithPagingAndCriteriaEQSearch(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<EnquiryDetailGridView>> retValue = EnqBc.GetEnquiryDetailsGridViewListWithPagingAndCriteriaEQSearch(page, pageSize, sortType, sortby, criteria);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<EnquiryDetailGridView>> GetEnquiryDetailsGridViewListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<EnquiryDetailGridView>> retValue = EnqBc.GetEnquiryDetailsGridViewListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<EnquiryDetails>> GetEnquiryDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<EnquiryDetails>> retValue = EnqBc.GetEnquiryDetailsListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<EnquiryDetails>> GetEnquiryDetailsListWithPagingAndCriteriaEQSearch(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<EnquiryDetails>> retValue = EnqBc.GetEnquiryDetailsListWithPagingAndCriteriaEQSearch(page, pageSize, sortType, sortby, criteria);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<EnquiryCourse>> GetEnquiryCourseDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return EnqBc.GetEnquiryCourseDetailsListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public string CreateOrUpdateEnquiryCourse(EnquiryCourse Ec, string userid)
        {

            try
            {
                return EnqBc.CreateOrUpdateEnquiryCourse(Ec, userid);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #region Kiosk Enquiry Details Save
        public string SaveOrUpdateKioskEnquiryDetails(KioskEnquiryDetails KioskEnquiryDetails)
        {
            try
            {
                return EnqBc.SaveOrUpdateKioskEnquiryDetails(KioskEnquiryDetails);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        #region Basic Details Save
        public string SaveOrUpdateKioskBasicDetails(KioskBasicDetails KioskBasicDetails)
        {

            try
            {
                return EnqBc.SaveOrUpdateKioskBasicDetails(KioskBasicDetails);
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
