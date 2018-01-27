using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.Component;
using TIPS.Entities.CanteenEntities;

namespace TIPS.ServiceContract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CanteenService" in code, svc and config file together.
    public class CanteenService : ICanteenServiceSC
    {
        public Dictionary<long, IList<CanteenUnits>> GetCanteenUnitslistWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    CanteenBC CanteenBC = new CanteenBC();
                    return CanteenBC.GetCanteenUnitslistWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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

        public long CreateOrUpdateCanteenUnitsMaster(CanteenUnits su)
        {
            try
            {
                CanteenBC cbc = new CanteenBC();
                cbc.CreateOrUpdateCanteenUnitsMaster(su);
                return su.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<CanteenMaterialGroupMaster>> GetCanteenMaterialGroupMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                CanteenBC cbc = new CanteenBC();
                return cbc.GetCanteenMaterialGroupMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                CanteenBC cbc = new CanteenBC();
                cbc.CreateOrUpdateCanteenMaterialGroupMaster(mgm);
                return mgm.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<CanteenMaterialsMaster_vw>> GetCanteenMaterialsMasterlistWithPagingAndCriteriaUsingView(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    CanteenBC cbc = new CanteenBC();
                    return cbc.GetCanteenMaterialsMasterlistWithPagingAndCriteriaUsingView(page, pageSize, sortType, sortby, criteria);
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

        public CanteenMaterialGroupMaster GetCanteenMaterialGroupById(long Id)
        {
            try
            {
                CanteenBC cbc = new CanteenBC();
                return cbc.GetCanteenMaterialGroupById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<CanteenMaterialsMaster>> GetCanteenMaterialsMasterlistWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    CanteenBC cbc = new CanteenBC();
                    return cbc.GetCanteenMaterialsMasterlistWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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

        public long CreateOrUpdateCanteenMaterialsMaster(CanteenMaterialsMaster mm)
        {
            try
            {
                CanteenBC cbc = new CanteenBC();
                cbc.CreateOrUpdateCanteenMaterialsMaster(mm);
                return mm.Id;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<CanteenSupplierMaster>> GetCanteenSupplierMasterlistWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    CanteenBC cbc = new CanteenBC();
                    return cbc.GetCanteenSupplierMasterlistWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
    }
}
