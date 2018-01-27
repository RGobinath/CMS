using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TIPS.Entities.InboxEntities;
using PersistenceFactory;
using TIPS.Entities;
using System.Data;
using TIPS.Entities.AdmissionEntities;
using System.Collections;
using TIPS.Entities.TicketingSystem;
using TIPS.Entities.StaffManagementEntities;

namespace TIPS.Component
{
   public class InboxBC
    {
       PersistenceServiceFactory PSF = null;
       public InboxBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }
       public Dictionary<long, IList<Inbox>> GetInboxDetailsWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
       {
           try
           {
               return PSF.GetListWithSearchCriteriaCount<Inbox>(page, pageSize, sortType,sortBy, criteria);
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
               if (In != null)
                   PSF.SaveOrUpdate<Inbox>(In);
               else { throw new Exception("Inbox Value is required and it cannot be null.."); }
               return In.Id;
           }
           catch (Exception)
           {

               throw;
           }
       }
       public Inbox GetInboxDetailsByPreRegNo(long PreRegNo)
       {
           try
           {
               return PSF.Get<Inbox>("PreRegNum", PreRegNo);
           }
           catch (Exception)
           {
               throw;
           }
       }
       public Dictionary<long, IList<InboxCount_Vw>> GetInboxCountDetails(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
       {
           try
           {
               return PSF.GetListWithExactSearchCriteriaCount<InboxCount_Vw>(page, pageSize, sortby, sortType, criteria);

           }
           catch (Exception)
           {
               throw;
           }
       }
       public Dictionary<long, IList<InboxCount_Vw>> GetInboxCountDetailsWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
       {
           try
           {
               return PSF.GetListWithSearchCriteriaCount<InboxCount_Vw>(page, pageSize, sortType, sortBy, criteria);
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
               CallManagement user = PSF.Get<CallManagement>("IssueNumber", RefId);
               if (user != null)
                   return user.InstanceId.ToString();
               else
                   return "";
           }
           catch (Exception)
           {
               throw;
           }
       }
       public void DeleteInboxDetails(Inbox lm)
       {
           if (lm.Id > 0)
           {
               PSF.Delete<Inbox>(lm);
           }
       }
       public Inbox GetInboxDetailsById(long Id)
       {
           try
           {
               Inbox Ib = null;
               if (Id > 0)
                   Ib = PSF.Get<Inbox>(Id);
               else { throw new Exception("Id is required and it cannot be 0"); }
               return Ib;
           }
           catch (Exception)
           {
               throw;
           }
       }

       public Inbox DeleteInboxById(Inbox Ib)
       {
           try
           {
               if (Ib != null)
                   PSF.Delete<Inbox>(Ib);
               else { throw new Exception("Id is required and it cannot be 0"); }
               return Ib;
           }
           catch (Exception)
           {
               throw;
           }
       }
       public Dictionary<long, IList<AdmissionStatus_Vw>> GetAdmissionDetailsWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
       {
           try
           {
               return PSF.GetListWithSearchCriteriaCount<AdmissionStatus_Vw>(page, pageSize, sortType, sortBy, criteria);
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
               return PSF.Get<AdmissionStatus_Vw>("PreRegNum", PreRegNo);
           }
           catch (Exception)
           {
               throw;
           }
       }
       public StaffDetailsView GetStaffDetailsByPreRegNo(long PreRegNo)
       {
           try
           {
               return PSF.Get<StaffDetailsView>("PreRegNum", PreRegNo);
           }
           catch (Exception)
           {
               throw;
           }
       }

       public Dictionary<long, IList<Inbox>> InboxListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
       {
           try
           {
               return PSF.GetListWithExactAndLikeSearchCriteriaWithCountWithDecimal<Inbox>(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);

           }
           catch (Exception)
           {
               throw;
           }
       }
    }
}
