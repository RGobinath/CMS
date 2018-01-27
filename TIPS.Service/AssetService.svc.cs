using System;
using System.Collections;
using System.Collections.Generic;
using TIPS.Component;
using TIPS.Entities;
using TIPS.Entities.AdmissionEntities;
using TIPS.Entities.AssetEntities;

namespace TIPS.ServiceContract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AdminTemplateService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AdminTemplateService.svc or AdminTemplateService.svc.cs at the Solution Explorer and start debugging.
    public class AssetService : IAssetService
    {
        AssetBC AssBC = new AssetBC();
        public Dictionary<long, IList<AssetMaster>> GetAssetMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AssBC.GetAssetMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<AssetOrganizer>> GetEventListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AssBC.GetEventListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateEvent(AssetOrganizer Ao)
        {
            try
            {
                AssBC.CreateOrUpdateEvent(Ao);
                return Ao.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }
        public void DeleteAssetEvent(long EvntId)
        {
            try
            {
                AssBC.DeleteAssetEvent(EvntId);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public AssetOrganizer GetAssetEventById(long Id)
        {
            try
            {
                return AssBC.GetAssetEventById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<AssetOrganizer_vw>> GetAssetEventviewListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AssBC.GetAssetEventviewListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region IT Asset Management
        public CampusMaster GetAssetDetailsTemplateByFormId(long FormId)
        {
            try
            {
                if (FormId > 0)
                {
                    return AssBC.GetAssetDetailsTemplateByFormId(FormId);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<AssetDetailsTemplate>> GetAssetDetailsTemplateWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AssBC.GetAssetDetailsTemplateWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<AssetDetailsTemplate>> GetAssetTemplateListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AssBC.GetAssetTemplateListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<ITAssetSpecification>> GetAssetSpecificationListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AssBC.GetAssetSpecificationListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateITAsset(AssetDetailsTemplate asset)
        {
            try
            {
                AssBC.CreateOrUpdateITAsset(asset);
                return asset.Asset_Id;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public bool DeleteITAssetTypeMaster(long[] id)
        {
            try
            {
                AssBC.DeleteITAssetTypeMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public AssetDetailsTemplate GetAssetDetailsTemplateByAssetId(long Asset_Id)
        {
            try
            {
                if (Asset_Id > 0)
                {
                    return AssBC.GetAssetDetailsTemplateByAssetId(Asset_Id);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public IList GetITAssetDetailsListbyAssetType(long AssetId)
        {
            try
            {
                return AssBC.GetITAssetDetailsListbyAssetType(AssetId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Dictionary<long, IList<AssetDetails>> GetITAssetDetailsWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AssBC.GetITAssetDetailsWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateITAssetDetails(AssetDetails asset)
        {
            try
            {
                AssBC.CreateOrUpdateITAssetDetails(asset);
                return asset.AssetDet_Id;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public AssetDetails GetAssetDetailsByAssetId(long AssetDet_Id)
        {
            try
            {
                if (AssetDet_Id > 0)
                {
                    return AssBC.GetAssetDetailsByAssetId(AssetDet_Id);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #region Added by prabakaran
        public Dictionary<long, IList<AssetDetails>> GetITAssetDetailsWithPagingAndExactAndLikeCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                return AssBC.GetITAssetDetailsWithPagingAndExactAndLikeCriteria(page, pageSize, sortby, sortType, criteria, likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        public long CreateOrUpdateAssetDetailsTransaction(AssetDetailsTransaction asset)
        {
            try
            {
                AssBC.CreateOrUpdateAssetDetailsTransaction(asset);
                return asset.AssetTrans_Id;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public long CreateOrUpdateITAssetServiceDetails(ITAssetServiceDetails asset)
        {
            try
            {
                AssBC.CreateOrUpdateITAssetServiceDetails(asset);
                return asset.AssetService_Id;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public long CreateOrUpdateITAssetScrapDetails(ITAssetScrapDetails asset)
        {
            try
            {
                AssBC.CreateOrUpdateITAssetScrapDetails(asset);
                return asset.AssetScrap_Id;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public long CreateOrUpdateITAssetDetailsHistory(ITAssetDetailsTransactionHistory asset)
        {
            try
            {
                AssBC.CreateOrUpdateITAssetDetailsHistory(asset);
                return asset.History_Id;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public UploadedFiles GetAssetDocumentsByAssetId(long AssetDet_Id, string DocumentType)
        {
            try
            {
                if (AssetDet_Id > 0)
                {
                    return AssBC.GetAssetDocumentsByAssetId(AssetDet_Id, DocumentType);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<AssetDetailsTemplate>> GetITAssetDetailsTemplateWithPagingAndExactAndLikeCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                return AssBC.GetITAssetDetailsTemplateWithPagingAndExactAndLikeCriteria(page, pageSize, sortby, sortType, criteria, likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<ITAssetSpecification>> GetITAssetSpecificationWithPagingAndExactAndLikeCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                return AssBC.GetITAssetSpecificationWithPagingAndExactAndLikeCriteria(page, pageSize, sortby, sortType, criteria, likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<ITAssetSpecification>> GetITAssetSpecificationWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AssBC.GetITAssetSpecificationWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateITAssetSpecification(ITAssetSpecification spec)
        {
            try
            {
                AssBC.CreateOrUpdateITAssetSpecification(spec);
                return spec.Spec_Id;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public ITAssetSpecification GetITAssetSpecificationBySpec_Id(long Spec_Id)
        {
            try
            {
                if (Spec_Id > 0)
                {
                    return AssBC.GetITAssetSpecificationBySpec_Id(Spec_Id);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool DeleteITAssetSpecification(long[] id)
        {
            try
            {
                AssBC.DeleteITAssetSpecification(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #endregion
        #region AssetLocationMaster Prabakaran
        public Dictionary<long, IList<AssetLocationMaster>> GetAssetLocationMasterWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AssBC.GetAssetLocationMasterWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateAssetLocationMaster(AssetLocationMaster alm)
        {
            try
            {
                AssBC.CreateOrUpdateAssetLocationMaster(alm);
                return alm.LocationId;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public AssetLocationMaster GetAssetLocationMasterByLocationName(string BlockName, string Campus,string LocationName)
        {
            try
            {
                if (!string.IsNullOrEmpty(BlockName))
                {
                    return AssBC.GetAssetLocationMasterByLocationName(BlockName, Campus, LocationName);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public AssetLocationMaster GetAssetLocationMasterByLocationId(long LocationId)
        {
            try
            {
                if (LocationId > 0)
                {
                    return AssBC.GetAssetLocationMasterByBlockId(LocationId);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool DeleteAssetLocationMaster(long[] id)
        {
            try
            {
                AssBC.DeleteAssetLocationMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
#endregion
        #region CampusBlockMaster by Prabakaran
        public Dictionary<long, IList<CampusBlockMaster>> GetCampusBlockMasterWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AssBC.GetCampusBlockMasterWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateCampusBlockMaster(CampusBlockMaster cbm)
        {
            try
            {
                AssBC.CreateOrUpdateCampusBlockMaster(cbm);
                return cbm.BlockId;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public CampusBlockMaster GetCampusBlockMasterByBlockId(long BlockId)
        {
            try
            {
                if (BlockId > 0)
                {
                    return AssBC.GetCampusBlockMasterByBlockId(BlockId);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool DeleteCampusBlockMaster(long[] id)
        {
            try
            {
                AssBC.DeleteCampusBlockMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public CampusBlockMaster GetCampusBlockMasterByBlockName(string BlockName,string Campus)
        {
            try
            {
                if (!string.IsNullOrEmpty(BlockName))
                {
                    return AssBC.GetCampusBlockMasterByBlockName(BlockName,Campus);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #endregion
        #region VendorMaster by Prabakaran
        public Dictionary<long, IList<VendorMaster>> GetVendorMasterWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AssBC.GetVendorMasterWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateVendorMaster(VendorMaster vm)
        {
            try
            {
                AssBC.CreateOrUpdateVendorMaster(vm);
                return vm.VendorId;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public VendorMaster GetVendorMasterByVendorId(long VendorId)
        {
            try
            {
                if (VendorId > 0)
                {
                    return AssBC.GetVendorMasterByVendorId(VendorId);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool DeleteVendorMaster(long[] id)
        {
            try
            {
                AssBC.DeleteVendorMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public VendorMaster GetVendorMasterByVendorName(string VendorName)
        {
            try
            {
                if (!string.IsNullOrEmpty(VendorName))
                {
                    return AssBC.GetVendorMasterByVendorName(VendorName);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #endregion
        public Dictionary<long, IList<AssetDetails_vw>> GetITAssetDetails_vwWithPagingAndExactAndLikeCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                return AssBC.GetITAssetDetails_vwWithPagingAndExactAndLikeCriteria(page, pageSize, sortby, sortType, criteria, likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ITAssetServiceDetails GetITAssetServiceDetailsByAssetService_Id(long AssetService_Id)
        {
            try
            {
                if (AssetService_Id > 0)
                {
                    return AssBC.GetITAssetServiceDetailsByAssetService_Id(AssetService_Id);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<ITAssetServiceDetails>> GetITAssetServiceDetailsWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AssBC.GetITAssetServiceDetailsWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ITAssetDetailsTransactionHistory GetITAssetDetailsTransactionHistoryByTransactionType_IdwithAssetId(long AssetId, long TransactionType_Id)
        {
            try
            {
                if (AssetId > 0 && TransactionType_Id > 0)
                {
                    return AssBC.GetITAssetDetailsTransactionHistoryByTransactionType_IdwithAssetId(AssetId, TransactionType_Id);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #region AssetBrandMaster
        public Dictionary<long, IList<AssetBrandMaster>> GetAssetBrandMasterWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AssBC.GetAssetBrandMasterWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateAssetBrandMaster(AssetBrandMaster abm)
        {
            try
            {
                AssBC.CreateOrUpdateAssetBrandMaster(abm);
                return abm.BrandMasterId;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public AssetBrandMaster GetAssetBrandMasterByBrandMasterId(long BrandMasterId)
        {
            try
            {
                if (BrandMasterId > 0)
                {
                    return AssBC.GetAssetBrandMasterByBrandMasterId(BrandMasterId);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool DeleteAssetBrandMaster(long[] id)
        {
            try
            {
                AssBC.DeleteAssetBrandMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public AssetBrandMaster GetAssetBrandMasterByBrandName(string Brand)
        {
            try
            {
                if (!string.IsNullOrEmpty(Brand))
                {
                    return AssBC.GetAssetBrandMasterByBrandName(Brand);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #endregion
        #region AssetModelMaster
        public Dictionary<long, IList<AssetModelMaster>> GetAssetModelMasterWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AssBC.GetAssetModelMasterWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateAssetModelMaster(AssetModelMaster amm)
        {
            try
            {
                AssBC.CreateOrUpdateAssetModelMaster(amm);
                return amm.ModelMasterId;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public AssetModelMaster GetAssetModelMasterByModelMasterId(long ModelMasterId)
        {
            try
            {
                if (ModelMasterId > 0)
                {
                    return AssBC.GetAssetModelMasterByModelMasterId(ModelMasterId);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool DeleteAssetModelMaster(long[] id)
        {
            try
            {
                AssBC.DeleteAssetModelMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public AssetModelMaster GetAssetModelMasterByBrandandModel(string Brand, string Model)
        {
            try
            {
                if (!string.IsNullOrEmpty(Brand) && !string.IsNullOrEmpty(Model))
                {
                    return AssBC.GetAssetModelMasterByBrandandModel(Brand, Model);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #endregion
        public long CreateOrUpdateErrorLogs(ErrorLogs el)
        {
            try
            {
                AssBC.CreateOrUpdateErrorLogs(el);
                return el.Id;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #region AssetInvoiceDetails
        public Dictionary<long, IList<AssetInvoiceDetails>> GetAssetInvoiceDetailsWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                return AssBC.GetAssetInvoiceDetailsWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria, likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateAssetInvoiceDetails(AssetInvoiceDetails assetinvoicedetails)
        {
            try
            {
                AssBC.CreateOrUpdateAssetInvoiceDetails(assetinvoicedetails);
                return assetinvoicedetails.InvoiceDetailsId;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public AssetInvoiceDetails GetAssetInvoiceDetailsByInvoiceNowithVendorId(string InvoiceNo, long VendorId)
        {
            try
            {
                if (VendorId > 0 && !string.IsNullOrEmpty(InvoiceNo))
                {
                    return AssBC.GetAssetInvoiceDetailsByInvoiceNowithVendorId(InvoiceNo, VendorId);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public AssetInvoiceDetails GetAssetInvoiceDetailsByInvoiceDetailsId(long InvoiceDetailsId)
        {
            try
            {
                if (InvoiceDetailsId > 0)
                {
                    return AssBC.GetAssetInvoiceDetailsByInvoiceDetailsId(InvoiceDetailsId);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<AssetInvoiceDetails_vw>> GetAssetInvoiceDetails_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                
                return AssBC.GetAssetInvoiceDetails_vwListWithPagingAndCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<AssetDetailsReport_vw>> GetAssetDetailsReport_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {

                return AssBC.GetAssetDetailsReport_vwListWithPagingAndCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        public ITAssetServiceDetails GetITAssetServiceDetailsByAssetIdandInvoiceId(long AssetDet_Id,long InvoiceDetailsId)
        {
            try
            {
                if (AssetDet_Id > 0)
                {
                    return AssBC.GetITAssetServiceDetailsByAssetIdandInvoiceId(AssetDet_Id,InvoiceDetailsId);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }        
        #region Sub Asset Details
        public Dictionary<long, IList<SubAssetDetails_vw>> GetITSubAssetDetails_vwWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AssBC.GetITSubAssetDetails_vwWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<SubAssetDetails_vw>> GetITSubAssetDetails_vwWithPagingAndExactAndLikeCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                return AssBC.GetITSubAssetDetails_vwWithPagingAndExactAndLikeCriteria(page, pageSize, sortby, sortType, criteria, likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public SubAssetDetails_vw GetSubAssetDetailsByAssetId(long AssetDet_Id)
        {
            try
            {
                if (AssetDet_Id > 0)
                {
                    return AssBC.GetSubAssetDetailsByAssetId(AssetDet_Id);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #endregion
        public IList<AssetDetails> SaveOrUpdateAssetDetailsList(IList<AssetDetails> assetlist)
        {
            try
            {
                AssBC.SaveOrUpdateAssetDetailsList(assetlist);
                return assetlist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public IList<ITAssetDetailsTransactionHistory> SaveOrUpdateITAssetDetailsTransactionHistoryList(IList<ITAssetDetailsTransactionHistory> historylist)
        {
            try
            {
                AssBC.SaveOrUpdateITAssetDetailsTransactionHistoryList(historylist);
                return historylist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #region AssetProductMaster
        public Dictionary<long, IList<AssetProductMaster>> GetAssetProductMasterWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AssBC.GetAssetProductMasterWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateAssetProductMaster(AssetProductMaster apm)
        {
            try
            {
                AssBC.CreateOrUpdateAssetProductMaster(apm);
                return apm.AssetProductMasterId;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public AssetProductMaster GetAssetProductMasterById(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    return AssBC.GetAssetProductMasterById(Id);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool DeleteAssetProductMaster(long[] id)
        {
            try
            {
                AssBC.DeleteAssetProductMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public AssetProductMaster GetAssetProductMasterByName(string ProductName)
        {
            try
            {
                if (!string.IsNullOrEmpty(ProductName))
                {
                    return AssBC.GetAssetProductMasterByName(ProductName);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        #endregion
        #region AssetModelMaster
        public Dictionary<long, IList<AssetProductTypeMaster>> GetAssetProductTypeMasterWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AssBC.GetAssetProductTypeMasterWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateAssetProductTypeMaster(AssetProductTypeMaster aptm)
        {
            try
            {
                AssBC.CreateOrUpdateAssetProductTypeMaster(aptm);
                return aptm.AssetProductTypeMasterId;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public AssetProductTypeMaster GetAssetProductTypeMasterById(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    return AssBC.GetAssetProductTypeMasterById(Id);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool DeleteAssetProductTypeMaster(long[] id)
        {
            try
            {
                AssBC.DeleteAssetProductTypeMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public AssetProductTypeMaster GetAssetProductTypeMasterByNameandtype(long AssetProductMasterId, string ProductType)
        {
            try
            {
                if (AssetProductMasterId > 0 && !string.IsNullOrEmpty(ProductType))
                {
                    return AssBC.GetAssetProductTypeMasterByNameandtype(AssetProductMasterId, ProductType);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #endregion       
        #region IT Accessories
        public Dictionary<long, IList<ITAccessories>> GetITAccessoriesDetailsWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AssBC.GetITAccessoriesDetailsWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateITAccessories(ITAccessories ITaccess)
        {
            try
            {
                AssBC.CreateOrUpdateITAccessories(ITaccess);
                return ITaccess.Id;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
        #region ITAccessoriesBrandMaster
        public Dictionary<long, IList<ITAccessoriesBrandMaster>> GetITAccessoriesBrandMasterWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AssBC.GetITAccessoriesBrandMasterWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateAssetBrandMaster(ITAccessoriesBrandMaster abm)
        {
            try
            {
                AssBC.CreateOrUpdateITAccessoriesBrandMaster(abm);
                return abm.Id;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public ITAccessoriesBrandMaster GetITAccessoriesBrandMasterById(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    return AssBC.GetITAccessoriesBrandMasterById(Id);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool DeleteITAccessoriesBrandMaster(long[] id)
        {
            try
            {
                AssBC.DeleteITAccessoriesBrandMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public ITAccessoriesBrandMaster GetITAccessoriesBrandMasterByBrandName(string Brand)
        {
            try
            {
                if (!string.IsNullOrEmpty(Brand))
                {
                    return AssBC.GetITAccessoriesBrandMasterByBrandName(Brand);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #endregion
        #region ITAccessoriesModelMaster
        public Dictionary<long, IList<ITAccessoriesModelMaster>> GetITAccessoriesModelMasterWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AssBC.GetITAccessoriesModelMasterWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateITAccessoriesModelMaster(ITAccessoriesModelMaster amm)
        {
            try
            {
                AssBC.CreateOrUpdateITAccessoriesModelMaster(amm);
                return amm.Id;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public ITAccessoriesModelMaster GetITAccessoriesModelMasterById(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    return AssBC.GetITAccessoriesModelMasterById(Id);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool DeleteITAccessoriesModelMaster(long[] id)
        {
            try
            {
                AssBC.DeleteITAccessoriesModelMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public ITAccessoriesModelMaster GetITAccessoriesModelMasterByBrandandModel(long Id, string Model)
        {
            try
            {
                if (Id > 0 && !string.IsNullOrEmpty(Model))
                {
                    return AssBC.GetITAccessoriesModelMasterByBrandandModel(Id, Model);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #endregion
        #region Students Laptop Distribution
        public Dictionary<long, IList<STAssetDetails>> GetSTAssetDetailsWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AssBC.GetSTAssetDetailsWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateSTAssetDetails(STAssetDetails asset)
        {
            try
            {
                AssBC.CreateOrUpdateSTAssetDetails(asset);
                return asset.AssetDet_Id;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public long CreateOrUpdateSTAssetDetailsHistory(STAssetDetailsTransactionHistory asset)
        {
            try
            {
                AssBC.CreateOrUpdateSTAssetDetailsHistory(asset);
                return asset.History_Id;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public Dictionary<long, IList<STAssetDetails>> GetSTAssetDetailsWithPagingAndExactAndLikeCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                return AssBC.GetSTAssetDetailsWithPagingAndExactAndLikeCriteria(page, pageSize, sortby, sortType, criteria, likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<STAssetDetails> SaveOrUpdateSTAssetDetailsList(IList<STAssetDetails> stassetlist)
        {
            try
            {
                AssBC.SaveOrUpdateSTAssetDetailsList(stassetlist);
                return stassetlist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public IList<STAssetDetailsTransactionHistory> SaveOrUpdateSTAssetDetailsTransactionHistoryList(IList<STAssetDetailsTransactionHistory> sthistorylist)
        {
            try
            {
                AssBC.SaveOrUpdateSTAssetDetailsTransactionHistoryList(sthistorylist);
                return sthistorylist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #endregion
        //--Krishna,09062017
        #region LaptopDistribution
        public Dictionary<long, IList<AssetDistributionStudent_vw>> GetStudentListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {            
            try
            {
                return AssBC.GetStudentListWithPagingAndCriteria1(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }        
        public STAssetDetails GetSTAssetDetailsByAssetId(long AssetDet_Id)
        {
            try
            {
                if (AssetDet_Id > 0)
                {
                    return AssBC.GetSTAssetDetailsByAssetId(AssetDet_Id);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }        
        #endregion

        //#region Laptop Entry
        //public AssetDetailsTemplate GetAssetDetailsTemplateByAssetId(long Asset_Id)
        //{
        //    try
        //    {
        //        if (Asset_Id > 0)
        //        {
        //            return AssBC.GetAssetDetailsTemplateByAssetId(Asset_Id);
        //        }
        //        else throw new Exception("Id is required and it cannot be null or empty.");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally { }
        //}

        //public Dictionary<long, IList<STAssetDetails>> GetSTAssetDetailsWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        //{
        //    try
        //    {
        //        return AssBC.GetSTAssetDetailsWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public AssetInvoiceDetails GetAssetInvoiceDetailsByInvoiceDetailsId(long InvoiceDetailsId)
        //{
        //    try
        //    {
        //        if (InvoiceDetailsId > 0)
        //        {
        //            return AssBC.GetAssetInvoiceDetailsByInvoiceDetailsId(InvoiceDetailsId);
        //        }
        //        else throw new Exception("Id is required and it cannot be null or empty.");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally { }
        //}

        //public Dictionary<long, IList<STAssetDetails>> GetSTAssetDetailsWithPagingAndExactAndLikeCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        //{
        //    try
        //    {
        //        return AssBC.GetSTAssetDetailsWithPagingAndExactAndLikeCriteria(page, pageSize, sortby, sortType, criteria, likecriteria);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public IList<STAssetDetails> SaveOrUpdateSTAssetDetailsList(IList<STAssetDetails> stassetlist)
        //{
        //    try
        //    {
        //        AssBC.SaveOrUpdateSTAssetDetailsList(stassetlist);
        //        return stassetlist;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally { }
        //}
        //public long CreateOrUpdateAssetInvoiceDetails(AssetInvoiceDetails assetinvoicedetails)
        //{
        //    try
        //    {
        //        AssBC.CreateOrUpdateAssetInvoiceDetails(assetinvoicedetails);
        //        return assetinvoicedetails.InvoiceDetailsId;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        //public IList<STAssetDetailsTransactionHistory> SaveOrUpdateSTAssetDetailsTransactionHistoryList(IList<STAssetDetailsTransactionHistory> sthistorylist)
        //{
        //    try
        //    {
        //        AssBC.SaveOrUpdateSTAssetDetailsTransactionHistoryList(sthistorylist);
        //        return sthistorylist;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally { }
        //}
        //public Dictionary<long, IList<VendorMaster>> GetVendorMasterWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        //{
        //    try
        //    {
        //        return AssBC.GetVendorMasterWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

       
        //public AssetDetailsTemplate GetAssetDetailsTemplateByAssetId(long Asset_Id)
        //{
        //    try
        //    {
        //        if (Asset_Id > 0)
        //        {
        //            return AssBC.GetAssetDetailsTemplateByAssetId(Asset_Id);
        //        }
        //        else throw new Exception("Id is required and it cannot be null or empty.");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally { }
        //}

        //public Dictionary<long, IList<STAssetDetails>> GetSTAssetDetailsWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        //{
        //    try
        //    {
        //        return AssBC.GetSTAssetDetailsWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //public Dictionary<long, IList<STAssetDetails>> GetSTAssetDetailsWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        //{
        //    try
        //    {
        //        return PSF.GetListWithEQSearchCriteriaCount<STAssetDetails>(page, pageSize, sortType, sortby, criteria);

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public AssetDetailsTemplate GetAssetDetailsTemplateByAssetId(long Asset_Id)
        //{
        //    try
        //    {
        //        if (Asset_Id > 0)
        //        {
        //            return AssBC.GetAssetDetailsTemplateByAssetId(Asset_Id);
        //        }
        //        else throw new Exception("Id is required and it cannot be null or empty.");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally { }
        //}

        //public AssetInvoiceDetails GetAssetInvoiceDetailsByInvoiceDetailsId(long InvoiceDetailsId)
        //{
        //    try
        //    {
        //        if (InvoiceDetailsId > 0)
        //        {
        //            return AssBC.GetAssetInvoiceDetailsByInvoiceDetailsId(InvoiceDetailsId);
        //        }
        //        else throw new Exception("Id is required and it cannot be null or empty.");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally { }
        //}
        //public long CreateOrUpdateAssetInvoiceDetails(AssetInvoiceDetails assetinvoicedetails)
        //{
        //    try
        //    {
        //        AssBC.CreateOrUpdateAssetInvoiceDetails(assetinvoicedetails);
        //        return assetinvoicedetails.InvoiceDetailsId;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public CampusMaster GetAssetDetailsTemplateByFormId(long FormId)
        //{
        //    try
        //    {
        //        if (FormId > 0)
        //        {
        //            return AssBC.GetAssetDetailsTemplateByFormId(FormId);
        //        }
        //        else throw new Exception("Id is required and it cannot be null or empty.");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally { }
        //}
        //public long CreateOrUpdateSTAssetDetailsHistory(STAssetDetailsTransactionHistory asset)
        //{
        //    try
        //    {
        //        AssBC.CreateOrUpdateSTAssetDetailsHistory(asset);
        //        return asset.History_Id;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        //#endregion

        public Dictionary<long, IList<STAssetDetails>> GetAssetListbyInvoiceNo(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AssBC.GetAssetListInvoiceNoWiseWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<StudentLaptopDistribution_vw>> GetStudentLaptopDistributionWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                return AssBC.GetStudentLaptopDistributionWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria, likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<LaptopEntryDtls_vw>> GetLaptopEntryDtls(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return AssBC.GetLaptopEntryDtls(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
