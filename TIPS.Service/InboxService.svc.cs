using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using TIPS.Component;
using System.Text;
using TIPS.Entities;
using System.Data;
using TIPS.Entities.InboxEntities;
using TIPS.Entities.StaffManagementEntities;
namespace TIPS.ServiceContract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "InboxService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select InboxService.svc or InboxService.svc.cs at the Solution Explorer and start debugging.
    public class InboxService : IInboxService
    {
        InboxBC IBC = new InboxBC();
        public void DoWork()
        {
        }
        public Dictionary<long, IList<Inbox>> GetInboxDetailsWithPagingAndCriteria(int? page, int? pageSize, string sortType,string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                InboxBC IBC = new InboxBC();
                return IBC.GetInboxDetailsWithPagingAndCriteria(page, pageSize,sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateInbox(Inbox In)
        {
            try
            {
                InboxBC tbc = new InboxBC();
                tbc.CreateOrUpdateInbox(In);
                return In.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }
        public Inbox GetInboxDetailsById(long PreRegNo)
        {
            try
            {
                InboxBC InboxBC = new InboxBC();
                return InboxBC.GetInboxDetailsByPreRegNo(PreRegNo);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<InboxCount_Vw>> GetInboxCountDetails(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                InboxBC ibc = new InboxBC();
                return ibc.GetInboxCountDetails(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<InboxCount_Vw>> GetInboxCountWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                InboxBC IBC = new InboxBC();
                return IBC.GetInboxCountDetailsWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string GetInstanceIdByCallManagementId(string RefId)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(RefId))
                {
                    InboxBC IBC = new InboxBC();

                    return IBC.GetInstanceIdByCallManagementId(RefId);

                }
                else throw new Exception("User Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
      
        public Inbox GetInboxById(long Id)
        {
            try
            {
                InboxBC tbc = new InboxBC();
                return tbc.GetInboxDetailsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Inbox DeleteInboxbyId(Inbox Ib)
        {
            try
            {
                InboxBC bc = new InboxBC();
                return bc.DeleteInboxById(Ib);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<AdmissionStatus_Vw>> GetAdmissionsWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                InboxBC IBC = new InboxBC();
                return IBC.GetAdmissionDetailsWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AdmissionStatus_Vw GetAdmissionDetailsByPreRegNo(long PreRegNo)
        {
            try
            {
                InboxBC InboxBc = new InboxBC();
                return InboxBc.GetAdmissionDetailsByPreRegNo(PreRegNo);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public StaffDetailsView GetStaffDetailsByPreRegNo(long PreRegNo)
        {
            try
            {
                InboxBC InboxBc = new InboxBC();
                return InboxBc.GetStaffDetailsByPreRegNo(PreRegNo);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<Inbox>> InboxListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                InboxBC TBC = new InboxBC();
                return TBC.InboxListWithLikeAndExcactSerachCriteria(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
    }
}
