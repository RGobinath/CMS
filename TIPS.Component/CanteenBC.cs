using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersistenceFactory;
using TIPS.Entities.CanteenEntities;

namespace TIPS.Component
{
   public class CanteenBC
    {
       PersistenceServiceFactory PSF = null;
       public CanteenBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }

       public Dictionary<long, IList<CanteenUnits>> GetCanteenUnitslistWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
       {
           try
           {
               Dictionary<long, IList<CanteenUnits>> retValue = new Dictionary<long, IList<CanteenUnits>>();
               return PSF.GetListWithSearchCriteriaCount<CanteenUnits>(page, pageSize, sortType, sortBy, criteria);
           }

           catch (Exception)
           {

               throw;
           }
       }

       public long CreateOrUpdateCanteenUnitsMaster(CanteenUnits su)
       {
           try
           {
               if (su != null)
                   PSF.SaveOrUpdate<CanteenUnits>(su);
               else { throw new Exception("CanteenUnits is required and it cannot be null.."); }
               return su.Id;
           }
           catch (Exception)
           {
               throw;
           }
       }

       public Dictionary<long, IList<CanteenMaterialGroupMaster>> GetCanteenMaterialGroupMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
       {
           try
           {
               Dictionary<long, IList<CanteenMaterialGroupMaster>> retValue = new Dictionary<long, IList<CanteenMaterialGroupMaster>>();
               return PSF.GetListWithSearchCriteriaCount<CanteenMaterialGroupMaster>(page, pageSize, sortType, sortby, criteria);
           }
           catch (Exception)
           {
               throw;
           }
       }

       public long CreateOrUpdateCanteenMaterialGroupMaster(CanteenMaterialGroupMaster mgm)
       {
           try
           {
               if (mgm != null)
                   PSF.SaveOrUpdate<CanteenMaterialGroupMaster>(mgm);
               else { throw new Exception("CanteenMaterialGroupMaster is required and it cannot be null.."); }
               return mgm.Id;
           }
           catch (Exception)
           {

               throw;
           }
       }

       public Dictionary<long, IList<CanteenMaterialsMaster_vw>> GetCanteenMaterialsMasterlistWithPagingAndCriteriaUsingView(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
       {
           try
           {
               Dictionary<long, IList<CanteenMaterialsMaster_vw>> retValue = new Dictionary<long, IList<CanteenMaterialsMaster_vw>>();
               return PSF.GetListWithSearchCriteriaCount<CanteenMaterialsMaster_vw>(page, pageSize, sortType, sortBy, criteria);
           }

           catch (Exception)
           {

               throw;
           }
       }

       public CanteenMaterialGroupMaster GetCanteenMaterialGroupById(long Id)
       {
           try
           {
               CanteenMaterialGroupMaster mg = null;
               if (Id > 0)
                   mg = PSF.Get<CanteenMaterialGroupMaster>(Id);
               else { throw new Exception("Id is required and it cannot be 0"); }
               return mg;
           }
           catch (Exception)
           {
               throw;
           }
       }

       public Dictionary<long, IList<CanteenMaterialsMaster>> GetCanteenMaterialsMasterlistWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
       {
           try
           {
               Dictionary<long, IList<CanteenMaterialsMaster>> retValue = new Dictionary<long, IList<CanteenMaterialsMaster>>();
               return PSF.GetListWithEQSearchCriteriaCount<CanteenMaterialsMaster>(page, pageSize, sortBy, sortType, criteria);
           }

           catch (Exception)
           {

               throw;
           }
       }

       public long CreateOrUpdateCanteenMaterialsMaster(CanteenMaterialsMaster mm)
       {
           try
           {
               if (mm != null)
                   PSF.SaveOrUpdate<CanteenMaterialsMaster>(mm);
               else { throw new Exception("CanteenMaterialsMaster is required and it cannot be null.."); }
               return mm.Id;
           }
           catch (Exception)
           {
               throw;
           }
       }

       public Dictionary<long, IList<CanteenSupplierMaster>> GetCanteenSupplierMasterlistWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
       {
           try
           {
               Dictionary<long, IList<CanteenSupplierMaster>> retValue = new Dictionary<long, IList<CanteenSupplierMaster>>();
               return PSF.GetListWithSearchCriteriaCount<CanteenSupplierMaster>(page, pageSize, sortType, sortBy, criteria);
           }

           catch (Exception)
           {

               throw;
           }
       }

    }
}
