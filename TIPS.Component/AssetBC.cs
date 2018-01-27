using PersistenceFactory;
using System;
using System.Collections;
using System.Collections.Generic;
using TIPS.Entities;
using TIPS.Entities.AdmissionEntities;
using TIPS.Entities.AssetEntities;

namespace TIPS.Component
{
    public class AssetBC
    {
        PersistenceServiceFactory PSF = null;
        public AssetBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }
        public Dictionary<long, IList<AssetMaster>> GetAssetMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<AssetMaster>(page, pageSize, sortType, sortby, criteria);
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
                return PSF.GetListWithExactSearchCriteriaCount<AssetOrganizer>(page, pageSize, sortType, sortby, criteria);
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
                if (Ao != null)
                    PSF.SaveOrUpdate<AssetOrganizer>(Ao);
                else { throw new Exception("Error"); }
                return Ao.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void DeleteAssetEvent(long EvntId)
        {
            try
            {
                string query = "delete from AssetOrganizer where Id=" + EvntId;
                PSF.ExecuteSql(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AssetOrganizer GetAssetEventById(long Id)
        {
            try
            {
                return PSF.Get<AssetOrganizer>(Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<AssetOrganizer_vw>> GetAssetEventviewListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<AssetOrganizer_vw>(page, pageSize, sortType, sortby, criteria);
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
                CampusMaster CampusDetails = null;
                if (FormId > 0)
                    CampusDetails = PSF.Get<CampusMaster>(FormId);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return CampusDetails;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<AssetDetailsTemplate>> GetAssetDetailsTemplateWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                //return PSF.GetListWithExactSearchCriteriaCount<AssetDetailsTemplate>(page, pageSize, sortby, sortType, criteria);
                return PSF.GetListWithEQSearchCriteriaCount<AssetDetailsTemplate>(page, pageSize, sortType, sortby, criteria);
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
                return PSF.GetListWithExactSearchCriteriaCount<AssetDetailsTemplate>(page, pageSize, sortby, sortType,criteria);
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
                return PSF.GetListWithExactSearchCriteriaCount<ITAssetSpecification>(page, pageSize, sortType, sortby, criteria);
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
                if (asset != null)
                    PSF.SaveOrUpdate<AssetDetailsTemplate>(asset);
                else { throw new Exception("Error"); }
                return asset.Asset_Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteITAssetTypeMaster(long[] id)
        {
            try
            {
                IList<AssetDetailsTemplate> assetmaster = PSF.GetListByIds<AssetDetailsTemplate>(id);
                PSF.DeleteAll<AssetDetailsTemplate>(assetmaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public AssetDetailsTemplate GetAssetDetailsTemplateByAssetId(long Asset_Id)
        {
            try
            {
                AssetDetailsTemplate assetDetails = null;
                if (Asset_Id > 0)
                    assetDetails = PSF.Get<AssetDetailsTemplate>(Asset_Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return assetDetails;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IList GetITAssetDetailsListbyAssetType(long AssetId)
        {
            try
            {
                //IList list = PSF.ExecuteSql("EXEC dbo.AssetManagement_sp " + AssetType);
                IList list = PSF.ExecuteSql("EXEC dbo.ITAssetManagement_sp " + AssetId);
                return list;
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
                return PSF.GetListWithEQSearchCriteriaCount<AssetDetails>(page, pageSize, sortType, sortby, criteria);

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
                if (asset != null)
                    PSF.SaveOrUpdate<AssetDetails>(asset);
                else { throw new Exception("Error"); }
                return asset.AssetDet_Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public AssetDetails GetAssetDetailsByAssetId(long AssetDet_Id)
        {
            try
            {
                AssetDetails assetDetails = null;
                if (AssetDet_Id > 0)
                    assetDetails = PSF.Get<AssetDetails>(AssetDet_Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return assetDetails;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ITAssetServiceDetails GetITAssetServiceDetailsByAssetIdandInvoiceId(long AssetDet_Id,long InvoiceDetailsId)
        {
            try
            {
                ITAssetServiceDetails assetserviceDetails = null;
                if (AssetDet_Id > 0 && InvoiceDetailsId > 0)
                    assetserviceDetails = PSF.Get<ITAssetServiceDetails>("AssetDetails.AssetDet_Id", AssetDet_Id, "InvoiceDetailsId", InvoiceDetailsId);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return assetserviceDetails;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #region Added by prabakaran
        public Dictionary<long, IList<AssetDetails>> GetITAssetDetailsWithPagingAndExactAndLikeCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<AssetDetails>(page, pageSize, sortType, sortby, criteria, likecriteria);

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
                if (asset != null)
                    PSF.SaveOrUpdate<AssetDetailsTransaction>(asset);
                else { throw new Exception("Error"); }
                return asset.AssetTrans_Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public long CreateOrUpdateITAssetServiceDetails(ITAssetServiceDetails asset)
        {
            try
            {
                if (asset != null)
                    PSF.SaveOrUpdate<ITAssetServiceDetails>(asset);
                else { throw new Exception("Error"); }
                return asset.AssetService_Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public long CreateOrUpdateITAssetScrapDetails(ITAssetScrapDetails asset)
        {
            try
            {
                if (asset != null)
                    PSF.SaveOrUpdate<ITAssetScrapDetails>(asset);
                else { throw new Exception("Error"); }
                return asset.AssetScrap_Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public long CreateOrUpdateITAssetDetailsHistory(ITAssetDetailsTransactionHistory asset)
        {
            try
            {
                if (asset != null)
                    PSF.SaveOrUpdate<ITAssetDetailsTransactionHistory>(asset);
                else { throw new Exception("Error"); }
                return asset.History_Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public UploadedFiles GetAssetDocumentsByAssetId(long AssetDet_Id, string DocumentType)
        {
            try
            {
                UploadedFiles assetDoc = null;
                if (AssetDet_Id > 0)
                    assetDoc = PSF.Get<UploadedFiles>("PreRegNum", AssetDet_Id, "DocumentType", DocumentType);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return assetDoc;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<AssetDetailsTemplate>> GetITAssetDetailsTemplateWithPagingAndExactAndLikeCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<AssetDetailsTemplate>(page, pageSize, sortType, sortby, criteria, likecriteria);

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
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<ITAssetSpecification>(page, pageSize, sortType, sortby, criteria, likecriteria);

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
                return PSF.GetListWithEQSearchCriteriaCount<ITAssetSpecification>(page, pageSize, sortType, sortby, criteria);
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
                if (spec != null)
                    PSF.SaveOrUpdate<ITAssetSpecification>(spec);
                else { throw new Exception("Error"); }
                return spec.Spec_Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ITAssetSpecification GetITAssetSpecificationBySpec_Id(long Spec_Id)
        {
            try
            {
                ITAssetSpecification spec = null;
                if (Spec_Id > 0)
                    spec = PSF.Get<ITAssetSpecification>(Spec_Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return spec;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteITAssetSpecification(long[] id)
        {
            try
            {
                IList<ITAssetSpecification> spec = PSF.GetListByIds<ITAssetSpecification>(id);
                PSF.DeleteAll<ITAssetSpecification>(spec);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region AssetLocationMaster by Prabakaran
        public Dictionary<long, IList<AssetLocationMaster>> GetAssetLocationMasterWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<AssetLocationMaster>(page, pageSize, sortType, sortby, criteria);
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
                if (alm != null)
                    PSF.SaveOrUpdate<AssetLocationMaster>(alm);
                else { throw new Exception("Error"); }
                return alm.LocationId;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public AssetLocationMaster GetAssetLocationMasterByLocationName(string BlockName, string Campus, string LocationName)
        {
            try
            {
                AssetLocationMaster alm = null;
                if (!string.IsNullOrEmpty(BlockName))
                    alm = PSF.Get<AssetLocationMaster>("BlockName", BlockName, "Campus", Campus, "Location", LocationName);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return alm;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public AssetLocationMaster GetAssetLocationMasterByBlockId(long LocationId)
        {
            try
            {
                AssetLocationMaster alm = null;
                if (LocationId > 0)
                    alm = PSF.Get<AssetLocationMaster>(LocationId);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return alm;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteAssetLocationMaster(long[] id)
        {
            try
            {
                IList<AssetLocationMaster> cbm = PSF.GetListByIds<AssetLocationMaster>(id);
                PSF.DeleteAll<AssetLocationMaster>(cbm);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region CampusBlockMaster by Prabakaran
        public Dictionary<long, IList<CampusBlockMaster>> GetCampusBlockMasterWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<CampusBlockMaster>(page, pageSize, sortType, sortby, criteria);
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
                if (cbm != null)
                    PSF.SaveOrUpdate<CampusBlockMaster>(cbm);
                else { throw new Exception("Error"); }
                return cbm.BlockId;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public CampusBlockMaster GetCampusBlockMasterByBlockId(long BlockId)
        {
            try
            {
                CampusBlockMaster cbm = null;
                if (BlockId > 0)
                    cbm = PSF.Get<CampusBlockMaster>(BlockId);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return cbm;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteCampusBlockMaster(long[] id)
        {
            try
            {
                IList<CampusBlockMaster> cbm = PSF.GetListByIds<CampusBlockMaster>(id);
                PSF.DeleteAll<CampusBlockMaster>(cbm);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public CampusBlockMaster GetCampusBlockMasterByBlockName(string BlockName, string Campus)
        {
            try
            {
                CampusBlockMaster cbm = null;
                if (!string.IsNullOrEmpty(BlockName))
                    cbm = PSF.Get<CampusBlockMaster>("BlockName", BlockName, "Campus", Campus);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return cbm;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region VendorMaster by Prabakaran
        public Dictionary<long, IList<VendorMaster>> GetVendorMasterWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<VendorMaster>(page, pageSize, sortType, sortby, criteria);
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
                if (vm != null)
                    PSF.SaveOrUpdate<VendorMaster>(vm);
                else { throw new Exception("Error"); }
                return vm.VendorId;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public VendorMaster GetVendorMasterByVendorId(long VendorId)
        {
            try
            {
                VendorMaster vm = null;
                if (VendorId > 0)
                    vm = PSF.Get<VendorMaster>(VendorId);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return vm;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteVendorMaster(long[] id)
        {
            try
            {
                IList<VendorMaster> vm = PSF.GetListByIds<VendorMaster>(id);
                PSF.DeleteAll<VendorMaster>(vm);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public VendorMaster GetVendorMasterByVendorName(string VendorName)
        {
            try
            {
                VendorMaster vm = null;
                if (!string.IsNullOrEmpty(VendorName))
                    vm = PSF.Get<VendorMaster>("VendorName", VendorName);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return vm;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        public Dictionary<long, IList<AssetDetails_vw>> GetITAssetDetails_vwWithPagingAndExactAndLikeCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<AssetDetails_vw>(page, pageSize, sortType, sortby, criteria, likecriteria);

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
                ITAssetServiceDetails assetserviceDetails = null;
                if (AssetService_Id > 0)
                    assetserviceDetails = PSF.Get<ITAssetServiceDetails>(AssetService_Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return assetserviceDetails;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<ITAssetServiceDetails>> GetITAssetServiceDetailsWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<ITAssetServiceDetails>(page, pageSize, sortType, sortby, criteria);

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
                ITAssetDetailsTransactionHistory transactiondetails = null;
                if (AssetId > 0 && TransactionType_Id > 0)
                    transactiondetails = PSF.Get<ITAssetDetailsTransactionHistory>("AssetDetails.AssetDet_Id", AssetId, "TransactionType_Id", TransactionType_Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return transactiondetails;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #region AssetBrandMaster
        public Dictionary<long, IList<AssetBrandMaster>> GetAssetBrandMasterWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<AssetBrandMaster>(page, pageSize, sortType, sortby, criteria);
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
                if (abm != null)
                    PSF.SaveOrUpdate<AssetBrandMaster>(abm);
                else { throw new Exception("Error"); }
                return abm.BrandMasterId;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public AssetBrandMaster GetAssetBrandMasterByBrandMasterId(long BrandMasterId)
        {
            try
            {
                AssetBrandMaster abm = null;
                if (BrandMasterId > 0)
                    abm = PSF.Get<AssetBrandMaster>(BrandMasterId);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return abm;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteAssetBrandMaster(long[] id)
        {
            try
            {
                IList<AssetBrandMaster> abm = PSF.GetListByIds<AssetBrandMaster>(id);
                PSF.DeleteAll<AssetBrandMaster>(abm);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public AssetBrandMaster GetAssetBrandMasterByBrandName(string Brand)
        {
            try
            {
                AssetBrandMaster abm = null;
                if (!string.IsNullOrEmpty(Brand))
                    abm = PSF.Get<AssetBrandMaster>("Brand", Brand);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return abm;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region AssetModelMaster
        public Dictionary<long, IList<AssetModelMaster>> GetAssetModelMasterWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<AssetModelMaster>(page, pageSize, sortType, sortby, criteria);
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
                if (amm != null)
                    PSF.SaveOrUpdate<AssetModelMaster>(amm);
                else { throw new Exception("Error"); }
                return amm.ModelMasterId;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public AssetModelMaster GetAssetModelMasterByModelMasterId(long ModelMasterId)
        {
            try
            {
                AssetModelMaster amm = null;
                if (ModelMasterId > 0)
                    amm = PSF.Get<AssetModelMaster>(ModelMasterId);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return amm;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteAssetModelMaster(long[] id)
        {
            try
            {
                IList<AssetModelMaster> amm = PSF.GetListByIds<AssetModelMaster>(id);
                PSF.DeleteAll<AssetModelMaster>(amm);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public AssetModelMaster GetAssetModelMasterByBrandandModel(string Brand, string Model)
        {
            try
            {
                AssetModelMaster amm = null;
                if (!string.IsNullOrEmpty(Brand) && !string.IsNullOrEmpty(Model))
                    amm = PSF.Get<AssetModelMaster>("Brand", Brand, "Model", Model);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return amm;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region AssetInvoiceDetails
        public Dictionary<long, IList<AssetInvoiceDetails>> GetAssetInvoiceDetailsWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCountWithDecimal<AssetInvoiceDetails>(page, pageSize, sortType, sortby, criteria, likecriteria);
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
                if (assetinvoicedetails != null)
                    PSF.SaveOrUpdate<AssetInvoiceDetails>(assetinvoicedetails);
                else { throw new Exception("Error"); }
                return assetinvoicedetails.InvoiceDetailsId;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public AssetInvoiceDetails GetAssetInvoiceDetailsByInvoiceNowithVendorId(string InvoiceNo, long VendorId)
        {
            try
            {
                AssetInvoiceDetails assetinvoicedetails = null;
                if (VendorId > 0 && !string.IsNullOrEmpty(InvoiceNo))
                    assetinvoicedetails = PSF.Get<AssetInvoiceDetails>("VendorMaster.VendorId", VendorId,"InvoiceNo", InvoiceNo);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return assetinvoicedetails;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public AssetInvoiceDetails GetAssetInvoiceDetailsByInvoiceDetailsId(long InvoiceDetailsId)
        {
            try
            {
                AssetInvoiceDetails assetinvoicedetails = null;
                if (InvoiceDetailsId > 0)
                    assetinvoicedetails = PSF.Get<AssetInvoiceDetails>(InvoiceDetailsId);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return assetinvoicedetails;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<AssetInvoiceDetails_vw>> GetAssetInvoiceDetails_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<AssetInvoiceDetails_vw>> retValue = new Dictionary<long, IList<AssetInvoiceDetails_vw>>();
                return PSF.GetListWithExactSearchCriteriaCount<AssetInvoiceDetails_vw>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<AssetDetailsReport_vw>> GetAssetDetailsReport_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<AssetDetailsReport_vw>> retValue = new Dictionary<long, IList<AssetDetailsReport_vw>>();
                return PSF.GetListWithExactSearchCriteriaCount<AssetDetailsReport_vw>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Sub Asset Management
        public Dictionary<long, IList<SubAssetDetails_vw>> GetITSubAssetDetails_vwWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<SubAssetDetails_vw>(page, pageSize, sortType, sortby, criteria);

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
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<SubAssetDetails_vw>(page, pageSize, sortType, sortby, criteria, likecriteria);

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
                SubAssetDetails_vw assetDetails = null;
                if (AssetDet_Id > 0)
                    assetDetails = PSF.Get<SubAssetDetails_vw>(AssetDet_Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return assetDetails;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        public IList<AssetDetails> SaveOrUpdateAssetDetailsList(IList<AssetDetails> assetlist)
        {
            try
            {
                if (assetlist != null)
                {
                    PSF.SaveOrUpdate<AssetDetails>(assetlist);
                }
                else { throw new Exception("User List is required and it cannot be null.."); }
                return assetlist;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IList<ITAssetDetailsTransactionHistory> SaveOrUpdateITAssetDetailsTransactionHistoryList(IList<ITAssetDetailsTransactionHistory> historylist)
        {
            try
            {
                if (historylist != null)
                {
                    PSF.SaveOrUpdate<ITAssetDetailsTransactionHistory>(historylist);
                }
                else { throw new Exception("Asset List is required and it cannot be null.."); }
                return historylist;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #region AssetProductMaster
        public Dictionary<long, IList<AssetProductMaster>> GetAssetProductMasterWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<AssetProductMaster>(page, pageSize, sortType, sortby, criteria);
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
                if (apm != null)
                    PSF.SaveOrUpdate<AssetProductMaster>(apm);
                else { throw new Exception("Error"); }
                return apm.AssetProductMasterId;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public AssetProductMaster GetAssetProductMasterById(long Id)
        {
            try
            {
                AssetProductMaster apm = null;
                if (Id > 0)
                    apm = PSF.Get<AssetProductMaster>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return apm;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteAssetProductMaster(long[] id)
        {
            try
            {
                IList<AssetProductMaster> apm = PSF.GetListByIds<AssetProductMaster>(id);
                PSF.DeleteAll<AssetProductMaster>(apm);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public AssetProductMaster GetAssetProductMasterByName(string ProductName)
        {
            try
            {
                AssetProductMaster apm = null;
                if (!string.IsNullOrEmpty(ProductName))
                    apm = PSF.Get<AssetProductMaster>("ProductName", ProductName);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return apm;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region AssetModelMaster
        public Dictionary<long, IList<AssetProductTypeMaster>> GetAssetProductTypeMasterWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<AssetProductTypeMaster>(page, pageSize, sortType, sortby, criteria);
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
                if (aptm != null)
                    PSF.SaveOrUpdate<AssetProductTypeMaster>(aptm);
                else { throw new Exception("Error"); }
                return aptm.AssetProductTypeMasterId;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public AssetProductTypeMaster GetAssetProductTypeMasterById(long Id)
        {
            try
            {
                AssetProductTypeMaster amm = null;
                if (Id > 0)
                    amm = PSF.Get<AssetProductTypeMaster>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return amm;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteAssetProductTypeMaster(long[] id)
        {
            try
            {
                IList<AssetProductTypeMaster> aptm = PSF.GetListByIds<AssetProductTypeMaster>(id);
                PSF.DeleteAll<AssetProductTypeMaster>(aptm);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public AssetProductTypeMaster GetAssetProductTypeMasterByNameandtype(long AssetProductMasterId, string ProductType)
        {
            try
            {
                AssetProductTypeMaster aptm = null;
                if (AssetProductMasterId > 0 && !string.IsNullOrEmpty(ProductType))
                    aptm = PSF.Get<AssetProductTypeMaster>("AssetProductMaster.AssetProductMasterId", AssetProductMasterId, "ProductType", ProductType);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return aptm;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        public long CreateOrUpdateErrorLogs(ErrorLogs el)
        {
            try
            {
                if (el != null)
                    PSF.SaveOrUpdate<ErrorLogs>(el);
                else { throw new Exception("Error"); }
                return el.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #region IT Accessories
        public Dictionary<long, IList<ITAccessories>> GetITAccessoriesDetailsWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<ITAccessories>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateITAccessories(ITAccessories ITAccess)
        {
            try
            {
                if (ITAccess != null)
                    PSF.SaveOrUpdate<ITAccessories>(ITAccess);
                else { throw new Exception("Error"); }
                return ITAccess.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region IT Accessories BrandMaster
        public Dictionary<long, IList<ITAccessoriesBrandMaster>> GetITAccessoriesBrandMasterWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<ITAccessoriesBrandMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateITAccessoriesBrandMaster(ITAccessoriesBrandMaster abm)
        {
            try
            {
                if (abm != null)
                    PSF.SaveOrUpdate<ITAccessoriesBrandMaster>(abm);
                else { throw new Exception("Error"); }
                return abm.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ITAccessoriesBrandMaster GetITAccessoriesBrandMasterById(long Id)
        {
            try
            {
                ITAccessoriesBrandMaster abm = null;
                if (Id > 0)
                    abm = PSF.Get<ITAccessoriesBrandMaster>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return abm;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteITAccessoriesBrandMaster(long[] id)
        {
            try
            {
                IList<ITAccessoriesBrandMaster> abm = PSF.GetListByIds<ITAccessoriesBrandMaster>(id);
                PSF.DeleteAll<ITAccessoriesBrandMaster>(abm);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ITAccessoriesBrandMaster GetITAccessoriesBrandMasterByBrandName(string Brand)
        {
            try
            {
                ITAccessoriesBrandMaster abm = null;
                if (!string.IsNullOrEmpty(Brand))
                    abm = PSF.Get<ITAccessoriesBrandMaster>("Brand", Brand);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return abm;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region ITAccessoriesModelMaster
        public Dictionary<long, IList<ITAccessoriesModelMaster>> GetITAccessoriesModelMasterWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<ITAccessoriesModelMaster>(page, pageSize, sortType, sortby, criteria);
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
                if (amm != null)
                    PSF.SaveOrUpdate<ITAccessoriesModelMaster>(amm);
                else { throw new Exception("Error"); }
                return amm.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ITAccessoriesModelMaster GetITAccessoriesModelMasterById(long Id)
        {
            try
            {
                ITAccessoriesModelMaster amm = null;
                if (Id > 0)
                    amm = PSF.Get<ITAccessoriesModelMaster>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return amm;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteITAccessoriesModelMaster(long[] id)
        {
            try
            {
                IList<ITAccessoriesModelMaster> amm = PSF.GetListByIds<ITAccessoriesModelMaster>(id);
                PSF.DeleteAll<ITAccessoriesModelMaster>(amm);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ITAccessoriesModelMaster GetITAccessoriesModelMasterByBrandandModel(long Id, string Model)
        {
            try
            {
                ITAccessoriesModelMaster amm = null;
                if (Id > 0 && !string.IsNullOrEmpty(Model))
                    amm = PSF.Get<ITAccessoriesModelMaster>("ITAccessoriesBrandMaster.Id", Id, "Model", Model);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return amm;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region Students Laptop Distribution
        public Dictionary<long, IList<STAssetDetails>> GetSTAssetDetailsWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<STAssetDetails>(page, pageSize, sortType, sortby, criteria);

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
                if (asset != null)
                    PSF.SaveOrUpdate<STAssetDetails>(asset);
                else { throw new Exception("Error"); }
                return asset.AssetDet_Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public long CreateOrUpdateSTAssetDetailsHistory(STAssetDetailsTransactionHistory asset)
        {
            try
            {
                if (asset != null)
                    PSF.SaveOrUpdate<STAssetDetailsTransactionHistory>(asset);
                else { throw new Exception("Error"); }
                return asset.History_Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<STAssetDetails>> GetSTAssetDetailsWithPagingAndExactAndLikeCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<STAssetDetails>(page, pageSize, sortType, sortby, criteria, likecriteria);

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
                if (stassetlist != null)
                {
                    PSF.SaveOrUpdate<STAssetDetails>(stassetlist);
                }
                else { throw new Exception("Asset List is required and it cannot be null.."); }
                return stassetlist;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IList<STAssetDetailsTransactionHistory> SaveOrUpdateSTAssetDetailsTransactionHistoryList(IList<STAssetDetailsTransactionHistory> historylist)
        {
            try
            {
                if (historylist != null)
                {
                    PSF.SaveOrUpdate<STAssetDetailsTransactionHistory>(historylist);
                }
                else { throw new Exception("history List is required and it cannot be null.."); }
                return historylist;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        //--Krishna,09062017
        #region LaptopDistribution
        public Dictionary<long, IList<AssetDistributionStudent_vw>> GetStudentListWithPagingAndCriteria1(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<AssetDistributionStudent_vw>(page, pageSize, sortType, sortby, criteria);
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
                STAssetDetails stassetDetails = null;
                if (AssetDet_Id > 0)
                    stassetDetails = PSF.Get<STAssetDetails>(AssetDet_Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return stassetDetails;
            }
            catch (Exception)
            {

                throw;
            }


        }                
        #endregion

        //#region laptop Entry
        //public AssetDetailsTemplate GetAssetDetailsTemplateByAssetId(long Asset_Id)
        //{
        //    try
        //    {
        //        AssetDetailsTemplate assetDetails = null;
        //        if (Asset_Id > 0)
        //            assetDetails = PSF.Get<AssetDetailsTemplate>(Asset_Id);
        //        else { throw new Exception("Id is required and it cannot be 0"); }
        //        return assetDetails;
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
        //public AssetInvoiceDetails GetAssetInvoiceDetailsByInvoiceDetailsId(long InvoiceDetailsId)
        //{
        //    try
        //    {
        //        AssetInvoiceDetails assetinvoicedetails = null;
        //        if (InvoiceDetailsId > 0)
        //            assetinvoicedetails = PSF.Get<AssetInvoiceDetails>(InvoiceDetailsId);
        //        else { throw new Exception("Id is required and it cannot be 0"); }
        //        return assetinvoicedetails;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        //public Dictionary<long, IList<STAssetDetails>> GetSTAssetDetailsWithPagingAndExactAndLikeCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        //{
        //    try
        //    {
        //        return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<STAssetDetails>(page, pageSize, sortType, sortby, criteria, likecriteria);

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
        //        if (stassetlist != null)
        //        {
        //            PSF.SaveOrUpdate<STAssetDetails>(stassetlist);
        //        }
        //        else { throw new Exception("Asset List is required and it cannot be null.."); }
        //        return stassetlist;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public long CreateOrUpdateAssetInvoiceDetails(AssetInvoiceDetails assetinvoicedetails)
        //{
        //    try
        //    {
        //        if (assetinvoicedetails != null)
        //            PSF.SaveOrUpdate<AssetInvoiceDetails>(assetinvoicedetails);
        //        else { throw new Exception("Error"); }
        //        return assetinvoicedetails.InvoiceDetailsId;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public IList<STAssetDetailsTransactionHistory> SaveOrUpdateSTAssetDetailsTransactionHistoryList(IList<STAssetDetailsTransactionHistory> historylist)
        //{
        //    try
        //    {
        //        if (historylist != null)
        //        {
        //            PSF.SaveOrUpdate<STAssetDetailsTransactionHistory>(historylist);
        //        }
        //        else { throw new Exception("history List is required and it cannot be null.."); }
        //        return historylist;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public Dictionary<long, IList<VendorMaster>> GetVendorMasterWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        //{
        //    try
        //    {
        //        return PSF.GetListWithEQSearchCriteriaCount<VendorMaster>(page, pageSize, sortType, sortby, criteria);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //public CampusMaster GetAssetDetailsTemplateByFormId(long FormId)
        //{
        //    try
        //    {
        //        CampusMaster CampusDetails = null;
        //        if (FormId > 0)
        //            CampusDetails = PSF.Get<CampusMaster>(FormId);
        //        else { throw new Exception("Id is required and it cannot be 0"); }
        //        return CampusDetails;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        //#endregion

        public Dictionary<long, IList<STAssetDetails>> GetAssetListInvoiceNoWiseWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<STAssetDetails>(page, pageSize, sortType, sortby, criteria);
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
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<StudentLaptopDistribution_vw>(page, pageSize, sortType, sortby, criteria, likecriteria);
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
                return PSF.GetListWithEQSearchCriteriaCount<LaptopEntryDtls_vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
