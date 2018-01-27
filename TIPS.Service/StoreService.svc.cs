using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.Entities.StoreEntities;
using TIPS.Component;
using TIPS.Entities;

namespace TIPS.ServiceContract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "StoreService" in code, svc and config file together.
    public class StoreService : IStoreServiceSc
    {
        public long CreateOrUpdateMaterialRequest(MaterialRequest mr)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                StoreBC.CreateOrUpdateMaterialRequest(mr);
                return mr.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public MaterialRequest GetMaterialRequestById(long Id)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialRequestById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }


        public Dictionary<long, IList<StoreMgmntActivity>> GetMaterialRequestListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, string ColumnName, string[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialRequestListWithPagingAndCriteria(page, pageSize, sortType, sortBy, ColumnName, values, criteria, alias);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long CreateOrUpdateMaterialRequestList(MaterialRequestList mrl)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                StoreBC.CreateOrUpdateMaterialRequestList(mrl);
                return mrl.MatReqRefId;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<MaterialRequestList>> GetMaterialRequestListListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialRequestListListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<StoreUnits>> GetStoreUnitsListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    StoreBC storeBC = new StoreBC();
                    return storeBC.GetStoreUnitsListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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

        public MaterialInward GetMaterialInwardById(long Id)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialInwardById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<MaterialInward>> GetMaterialInwardlistWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    StoreBC storeBC = new StoreBC();
                    return storeBC.GetMaterialInwardlistWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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

       

        public long CreateOrUpdateMaterialInward(MaterialInward mi)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                StoreBC.CreateOrUpdateMaterialInward(mi);
                return mi.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateSku(SkuList sl)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                StoreBC.CreateOrUpdateSku(sl);
                return sl.SkuId;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<SkuList>> GetSkulistWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    StoreBC storeBC = new StoreBC();
                    return storeBC.GetSkulistWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
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

        public long StartStoreManagement(MaterialRequest m, string Template, string userId)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                StoreBC.StartStoreManagement(m, Template, userId);
                return m.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }
       
        public bool CompleteActivityStoreManagement(MaterialRequest mr, string Template, string userId, string ActivityName, bool isRejection)
        {
            try
            {
                bool retValue = false;
                StoreBC StoreBC = new StoreBC();
                StoreBC.CompleteActivityStoreManagement(mr, Template, userId, ActivityName, isRejection);
                retValue = true;
                return retValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public SkuList GetSkuListById(long Id)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetSkuListById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateStock(Stock st)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                StoreBC.CreateOrUpdateStock(st);
                return st.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<Stock>> GetStockListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetStockListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public IList<SkuList> CreateOrUpdateSKUList(IList<SkuList> skuLst)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                StoreBC.CreateOrUpdateSKUList(skuLst);
                return skuLst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public IList<StoreMaster> GetStoreByCampus(string Campus)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetStoreByCampus(Campus);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public SkuList DeleteSKUbyId(SkuList sku)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.DeleteSKUbyId(sku);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<Stock_vw>> GetStockListWithPagingAndCriteria_vw(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetStockListWithPagingAndCriteria_vw(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<MaterialInward_vw>> GetMaterialInwardlistWithPagingAndCriteria_vw(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    StoreBC storeBC = new StoreBC();
                    return storeBC.GetMaterialInwardlistWithPagingAndCriteria_vw(page, pageSize, sortby, sortType, criteria);
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

        public IList<MaterialRequestList> CreateOrUpdateMateraialRequestList(IList<MaterialRequestList> MatReqLst)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                StoreBC.CreateOrUpdateSKUList(MatReqLst);
                return MatReqLst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public MaterialRequestList GetMaterialRequestListById(long Id)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialRequestListById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public MaterialRequestList DeleteMaterialRequestListById(MaterialRequestList mrl)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.DeleteMaterialRequestListById(mrl);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateMaterialIssueList(MaterialIssueList mil)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                StoreBC.CreateOrUpdateMaterialIssueList(mil);
                return mil.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public MaterialIssueList GetMaterialIssueListById(long Id)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialIssueListById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }



        public Dictionary<long, IList<MaterialIssueList>> GetMaterialIssuelistWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    StoreBC storeBC = new StoreBC();
                    return storeBC.GetMaterialIssuelistWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
        public long CreateOrUpdateMaterialIssueNote(MaterialIssueNote min)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                StoreBC.CreateOrUpdateMaterialIssueNote(min);
                return min.IssNoteId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public MaterialIssueNote GetMaterialIssueNoteById(long Id)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialIssueNoteById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<MaterialIssueNote>> GetMaterialMaterialIssueNoteListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialMaterialIssueNoteListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<MaterialIssueList_vw>> GetMaterialIssuelist_vwWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    StoreBC storeBC = new StoreBC();
                    return storeBC.GetMaterialIssuelist_vwWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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

        public Dictionary<long, IList<MaterialIssueNote_vw>> GetMaterialMaterialIssueNote_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialMaterialIssueNote_vwListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<MatInward_SkuList_vw>> GetMatInward_SkuList_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMatInward_SkuList_vwListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<MatReq_ReqList_vw>> GetMatReq_ReqList_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMatReq_ReqList_vwListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<MatIssNote_RequestList_vw>> GetMatIssNote_RequestList_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMatIssNote_RequestList_vwListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateStockTransaction(StockTransaction st)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                StoreBC.CreateOrUpdateStockTransaction(st);
                return st.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public MaterialsMaster GetMaterialsMasterByMaterial(string Material)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Material))
                {
                    StoreBC StoreBC = new StoreBC();
                    return StoreBC.GetMaterialsMasterByMaterial(Material);
                }
                else throw new Exception("Material is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public IList<MaterialRequestList> GetMaterialRequestListListWithPagingAndCriteria1(long MatReqRefId)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialRequestListListWithPagingAndCriteria1(MatReqRefId);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<StoreStockBalance>> GetStoreStockBalanceListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetStoreStockBalanceListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<StoreMaster>> GetStoreMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetStoreMasterListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        #region"Store"
        public Dictionary<long, IList<StoreSupplierMaster>> GetStoreSupplierMasterlistWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    StoreBC StoreBC = new StoreBC();
                    return StoreBC.GetStoreSupplierMasterlistWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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


        public long CreateOrUpdateStoreSupplierMaster(StoreSupplierMaster ssm)
        {
            try
            {
                StoreBC sbc = new StoreBC();
                sbc.CreateOrUpdateStoreSupplierMaster(ssm);
                return ssm.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public long CreateOrUpdateMaterialGroupSupplier(MaterialGroupSupplier mgs)
        {
            try
            {
                StoreBC sbc = new StoreBC();
                sbc.CreateOrUpdateMaterialGroupSupplier(mgs);
                return mgs.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<StoreUnits>> GetStoreUnitslistWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    StoreBC StoreBC = new StoreBC();
                    return StoreBC.GetStoreUnitslistWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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

        public long CreateOrUpdateStoreUnitsMaster(StoreUnits su)
        {
            try
            {
                StoreBC sbc = new StoreBC();
                sbc.CreateOrUpdateStoreUnitsMaster(su);
                return su.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public long CreateOrUpdateStoreMaterialGroupMaster(MaterialGroupMaster mgm)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                StoreBC.CreateOrUpdateStoreMaterialGroupMaster(mgm);
                return mgm.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public IList<MaterialSubGroupMaster> GetMaterialSubGroupByMaterialGroup(long MaterialGroupId)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialSubGroupByMaterialGroup(MaterialGroupId);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<MaterialGroupMaster>> GetMaterialGroupListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialGroupListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<MaterialsMaster> GetMaterialByMaterialGroupAndMaterialSubGroup(long MaterialGroupId, long MaterialSubGroupId)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialByMaterialGroupAndMaterialSubGroup(MaterialGroupId, MaterialSubGroupId);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<MaterialGroupSupplier>> GetMaterialGroupSupplierlistWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    StoreBC StoreBC = new StoreBC();
                    return StoreBC.GetMaterialGroupSupplierlistWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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

        public Dictionary<long, IList<MaterialSubGroupMaster>> GetMaterialSubGroupListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialSubGroupListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public long CreateOrUpdateStoreMaterialSubGroupMaster(MaterialSubGroupMaster msgm)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                StoreBC.CreateOrUpdateStoreMaterialSubGroupMaster(msgm);
                return msgm.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<MaterialsMaster>> GetMaterialsMasterlistWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    StoreBC StoreBC = new StoreBC();
                    return StoreBC.GetMaterialsMasterlistWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
        public long CreateOrUpdateMaterialsMaster(MaterialsMaster mm)
        {
            try
            {
                StoreBC sbc = new StoreBC();
                sbc.CreateOrUpdateMaterialsMaster(mm);
                return mm.Id;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public MaterialGroupMaster GetMaterialGroupById(long Id)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialGroupById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<MaterialSubGroupMaster_vw>> GetMaterialSubGroupListWithPagingAndCriteriaUsingView(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialSubGroupListWithPagingAndCriteriaUsingView(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<MaterialsMaster_vw>> GetMaterialsMasterlistWithPagingAndCriteriaUsingView(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    StoreBC StoreBC = new StoreBC();
                    return StoreBC.GetMaterialsMasterlistWithPagingAndCriteriaUsingView(page, pageSize, sortType, sortby, criteria);
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

        public Dictionary<long, IList<MaterialGroupSupplier_vw>> GetMaterialGroupSupplierlistWithPagingAndCriteriaUsingView(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    StoreBC StoreBC = new StoreBC();
                    return StoreBC.GetMaterialGroupSupplierlistWithPagingAndCriteriaUsingView(page, pageSize, sortby, sortType, criteria);
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
        public long CreateOrUpdateUOM_ConversionMatrix(UOM_ConversionMatrix ucm)
        {
            try
            {
                StoreBC sbc = new StoreBC();
                sbc.CreateOrUpdateUOM_ConversionMatrix(ucm);
                return ucm.Id;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<UOM_ConversionMatrix>> GetUOMConversionlistWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    StoreBC StoreBC = new StoreBC();
                    return StoreBC.GetUOMConversionlistWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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

        public Dictionary<long, IList<MaterialsMaster_vw_Stock_vw>> GetMaterialsMasterAndStockBalancelistWithPagingAndCriteriaUsingView(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    StoreBC StoreBC = new StoreBC();
                    return StoreBC.GetMaterialsMasterAndStockBalancelistWithPagingAndCriteriaUsingView(page, pageSize, sortby, sortType, criteria);
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
        #endregion"Store"

        #region StoreReport Added By Micheal
        public Dictionary<long, IList<MaterialInwardOutwardView>> GetMaterialIOListWithPagingAndCriteria_vw(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialIOListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<MonthMaster>> GetMonthMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMonthMasterListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion

        public Dictionary<long, IList<MaterialInwardReport_Vw>> GetMaterialInwardReport_VwListWithPagingAndCriteria_vw(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialInwardReport_VwListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        #region Material Issued report
        public Dictionary<long, IList<MaterialIssueReportView>> GetMaterialIssueListWithPagingAndCriteria_vw(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialIssueListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion

        

        #region Store to Store Material Transfer
        public long CreateOrUpdateMaterialIssue(StoreToStore mi)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                StoreBC.CreateOrUpdateMaterialIssue(mi);
                return mi.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public IList<StoreToStoreIssuedMaterials> CreateOrUpdateStoreToStoreIssuedMaterialsList(IList<StoreToStoreIssuedMaterials> IssueLst)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                StoreBC.CreateOrUpdateStoreToStoreIssuedMaterialsList(IssueLst);
                return IssueLst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<StoreToStoreIssuedMaterials>> GetStoreToStoreIssuedMaterialsListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetStoreToStoreIssuedMaterialsListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public StoreToStore GetMaterialIssueById(int Id)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialIssueById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion

        public Dictionary<long, IList<SkuList_MaterialPrice_vw>> GetSkuList_MaterialPrice_vwWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    StoreBC storeBC = new StoreBC();
                    return storeBC.GetSkuList_MaterialPrice_vwWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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

        public Dictionary<long, IList<StoreToStore>> GetStoreToStoreListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    StoreBC storeBC = new StoreBC();
                    return storeBC.GetStoreToStoreListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
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

        public Dictionary<long, IList<SkuList_MaterialInward>> GetSkuList_MaterialInwardListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, string ColumnName, string[] values, string[] alias)
        {
            try
            {
                StoreBC storeBC = new StoreBC();
                return storeBC.GetSkuList_MaterialInwardListWithPagingAndCriteria(page, pageSize, sortBy, sortType, criteria, ColumnName, values, alias);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public MaterialReturn CreateOrUpdateMaterialReturn(MaterialReturn mr)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                StoreBC.CreateOrUpdateMaterialReturn(mr);
                return mr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public MaterialReturn GetMaterialReturnById(int MatRetId)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialReturnById(MatRetId);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }


        public IList<MaterialReturnList> CreateOrUpdateMaterialReturnList(IList<MaterialReturnList> mrl)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                StoreBC.CreateOrUpdateMaterialReturnList(mrl);
                return mrl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<MaterialReturnList>> GetMaterialReturnListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    StoreBC StoreBC = new StoreBC();
                    return StoreBC.GetMaterialReturnListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
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

        public Dictionary<long, IList<MaterialReturn>> GetMaterialReturnWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    StoreBC StoreBC = new StoreBC();
                    return StoreBC.GetMaterialReturnWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
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

        #region Admin Template Report
        public Dictionary<long, IList<StoreReportForAdminTemplate_vw>> GetStoreReportForAdminTemplate_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetStoreReportForAdminTemplate_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<StoreInwardReportForAdminTemplate_vw>> GetStoreInwardReportForAdminTemplate_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetStoreInwardReportForAdminTemplate_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //Added on 21/10/2014
        public Dictionary<long, IList<MaterialGroupMaster>> GetMaterialGroupMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialGroupMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<MaterialSubGroupMaster>> GetMaterialSubGroupMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialSubGroupMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //public Dictionary<long, IList<MaterialsMaster>> GetMaterialsMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        //{
        //    try
        //    {
        //        StoreBC StoreBC = new StoreBC();
        //        return StoreBC.GetMaterialsMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        #endregion

        public Dictionary<long, IList<MaterialsMaster>> GetAutoCompleteMaterialsMasterlistWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    StoreBC StoreBC = new StoreBC();
                    return StoreBC.GetAutoCompleteMaterialsMasterlistWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
        # region Material Distribution by vinoth
        public long CreateOrUpdateMaterialDistribution(MaterialDistribution md)
        {
            try
            {
                StoreBC sbc = new StoreBC();
                sbc.CreateOrUpdateMaterialDistribution(md);
                return md.MaterialDistributionId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<MaterialDistribution_Vw>> GetMaterialDistributionListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialDistributionListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }


        public Dictionary<long, IList<StudentMaterialDistribution_vw>> GetStudentMaterialDistribution_vwListWithExcactAndLikeSearchCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetStudentMaterialDistribution_vwListWithExcactAndLikeSearchCriteria(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MaterialIssueDetails GetMaterialIssueDetailsById(long IssueId)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialIssueDetailsById(IssueId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public long CreateOrUpdateMaterialIssueDetails(MaterialIssueDetails materialissue)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                StoreBC.CreateOrUpdateMaterialIssueDetails(materialissue);
                return materialissue.IssueId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }

        }


        public Dictionary<long, IList<MaterialIssueDetails>> GetMaterialIssueDetailsListWithExcactAndLikeSearchCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialIssueDetailsListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteMaterialIssueDetails(long[] Ids)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.DeleteIssue(Ids);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Dictionary<long, IList<MaterialDistribution>> GetMaterialDistributionListWithExcactAndLikeSearchCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialDistributionDetailsListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public MaterialDistribution GetMaterialDistributionOnMaterial(string AcademicYear, string Campus, string Grade, string Gender, string IsHosteller, string MaterialSubGroup)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialConfigurationonMaterial(AcademicYear, Campus, Grade, Gender, IsHosteller, MaterialSubGroup);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Dictionary<long, IList<StudentWiseMaterialReport>> GetStudentWiseMaterialReportListWithExcactAndLikeSearchCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetStudentWiseMaterialReportListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public StudentMaterialDistribution_vw GetStudentMaterialDistribution_vwId(long StudId)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetStudentMaterialDistribution_vwStudentId(StudId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region MaterialDistributionReport By Dhanabalan
        public Dictionary<long, IList<MaterialDistributionReport>> GetMaterialdistributionReportListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> Likecriteria)
        {
            try
            {
                try
                {
                    StoreBC storeBC = new StoreBC();
                    return storeBC.GetMaterialdistributionReportListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria, Likecriteria);
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
        public Dictionary<long, IList<MaterialDistributionReportByCampus>> GetMaterialdistributionReportByCampusListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> Likecriteria)
        {
            try
            {
                try
                {
                    StoreBC storeBC = new StoreBC();
                    return storeBC.GetMaterialdistributionReportByCampusListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria, Likecriteria);
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
        public Dictionary<long, IList<MaterialDistributionReportByDate>> GetMaterialDistributionReportByDateListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> Likecriteria)
        {
            try
            {
                try
                {
                    StoreBC storeBC = new StoreBC();
                    return storeBC.GetMaterialDistributionReportByDateListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria, Likecriteria);
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
        #region Student Material Over View By john naveen
        public Dictionary<long, IList<StudentMaterialOverView_vw>> GetStudentMaterialOverView_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetStudentMaterialOverView_vwListWithExcactAndLikeSearchCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public StudentMaterialOverView_vw GetStudentMaterialOverView_vwId(long Id)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetStudentMaterialOverView_vwById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        #region Student Material Sub Group View By john naveen
        public Dictionary<long, IList<StudentMaterialSubGroupView_vw>> GetStudentMaterialSubGroupView_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetStudentMaterialSubGroupView_vwListWithExcactAndLikeSearchCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        #region Material Distribution Sub Over View
        public Dictionary<long, IList<StudentMaterialSubOverView_vw>> GetStudentMaterialSubOverView_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetStudentMaterialSubOverView_vwListWithExcactAndLikeSearchCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion     
        #region Material ditribution Issue Maodify by  john naveen
        public MaterialsMaster GetMaterialsMasterById(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    StoreBC StoreBC = new StoreBC();
                    return StoreBC.GetMaterialsMasterById(Id);
                }
                else throw new Exception("Material is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public StoreStockBalance GetStoreStockBalanceById(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    StoreBC StoreBC = new StoreBC();
                    return StoreBC.GetStoreStockBalanceById(Id);
                }
                else throw new Exception("Material is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public MaterialDistribution GetMaterialDistributionById(long Id)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetMaterialDistributionById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<SkuList_vw>> GetSkuList_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                StoreBC StoreBC = new StoreBC();
                return StoreBC.GetSkuList_vwListWithExcactAndLikeSearchCriteria(page, pageSize, sortType, sortBy, criteria);
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
