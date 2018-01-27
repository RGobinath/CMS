using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TIPS.Entities.TransportEntities;
using PersistenceFactory;
using TIPS.Entities;
using System.Data;
using TIPS.Entities.AdmissionEntities;
using System.Collections;
using System.Data.SqlClient;

namespace TIPS.Component
{
    public class TransportBC
    {
        PersistenceServiceFactory PSF = null;

        public TransportBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }


        public Dictionary<long, IList<VehicleTypeMaster>> GetVehicleTypeMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<VehicleTypeMaster>> retValue = new Dictionary<long, IList<VehicleTypeMaster>>();
                return PSF.GetListWithSearchCriteriaCount<VehicleTypeMaster>(page, pageSize, sortBy, sortType, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<VehicleSubTypeMaster>> GetVehicleSubTypeMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<VehicleSubTypeMaster>> retValue = new Dictionary<long, IList<VehicleSubTypeMaster>>();
                return PSF.GetListWithEQSearchCriteriaCount<VehicleSubTypeMaster>(page, pageSize, sortBy, sortType, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<VehicleTypeAndSubType>> GetVehicleTypeAndSubTypeListWithsearchCriteriaLikeSearch(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCount<VehicleTypeAndSubType>(page, pageSize, sortType, sortBy, criteria, alias);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public long CreateOrUpdateVehicleTypeMaster(VehicleTypeMaster vtm)
        {
            try
            {
                if (vtm != null)
                    PSF.SaveOrUpdate<VehicleTypeMaster>(vtm);
                else { throw new Exception("VehicleTypeMaster is required and it cannot be null.."); }
                return vtm.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long CreateOrUpdateVehicleSubTypeMaster(VehicleSubTypeMaster vstm)
        {
            try
            {
                if (vstm != null)
                    PSF.SaveOrUpdate<VehicleSubTypeMaster>(vstm);
                else { throw new Exception("VehicleSubTypeMaster is required and it cannot be null.."); }
                return vstm.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<VehicleDistanceCovered>> GetVehicleDistanceCoveredListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<VehicleDistanceCovered>> retValue = new Dictionary<long, IList<VehicleDistanceCovered>>();
                return PSF.GetListWithSearchCriteriaCount<VehicleDistanceCovered>(page, pageSize, sortBy, sortType, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public long CreateOrUpdateVehicleDistanceCovered(VehicleDistanceCovered vdc)
        {
            try
            {
                if (vdc != null)
                    PSF.SaveOrUpdate<VehicleDistanceCovered>(vdc);
                else { throw new Exception("VehicleTypeMaster is required and it cannot be null.."); }
                return vdc.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VehicleDistanceCovered GetVehicleDistanceCoveredById(int Id)
        {
            try
            {
                VehicleDistanceCovered vdc = null;
                if (Id > 0)
                    vdc = PSF.Get<VehicleDistanceCovered>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return vdc;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<VehicleFuelManagement>> GetVehicleFuelManagementListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<VehicleFuelManagement>> retValue = new Dictionary<long, IList<VehicleFuelManagement>>();
                return PSF.GetListWithLikeSearchCriteriaCount<VehicleFuelManagement>(page, pageSize, sortBy, sortType, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public long CreateOrUpdateVehicleFuelManagement(VehicleFuelManagement vfm)
        {
            try
            {
                if (vfm != null)
                    PSF.SaveOrUpdate<VehicleFuelManagement>(vfm);
                else { throw new Exception("VehicleTypeMaster is required and it cannot be null.."); }
                return vfm.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public VehicleFuelManagement GetVehicleFuelManagementById(int Id)
        {
            try
            {
                VehicleFuelManagement vfm = null;
                if (Id > 0)
                    vfm = PSF.Get<VehicleFuelManagement>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return vfm;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region  Tranport




        public void SaveOrUpdateMaintenance<Maintenance>(Maintenance main)
        {

            PSF.SaveOrUpdate<Maintenance>(main);
        }
        public void SaveOrUpdateBreakDown<BreakDown>(BreakDown Break)
        {

            PSF.SaveOrUpdate<BreakDown>(Break);
        }

        public void SaveOrUpdateACMaintenance<ACMaintenance>(ACMaintenance acm)
        {

            PSF.SaveOrUpdate<ACMaintenance>(acm);
        }

        public void DeleteMaintenance<Maintenance>(Maintenance main)
        {
            PSF.Delete<Maintenance>(main);
        }

        public void DeleteBreakDown<BreakDown>(BreakDown Break)
        {
            PSF.Delete<BreakDown>(Break);
        }

        #endregion

        public Dictionary<long, IList<FinesAndPenalities>> GetFinesAndPenalitiesIdViewListWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<FinesAndPenalities>(page, pageSize, sortType, sortby, criteria);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public VehicleSubTypeMaster GetVehicleDetailsByVehicleNo(int VehicleId)
        {
            try
            {
                return PSF.Get<VehicleSubTypeMaster>("Id", VehicleId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SaveOrUpdateFinesAndPenalities<FinesAndPenalities>(FinesAndPenalities acm)
        {

            PSF.SaveOrUpdate<FinesAndPenalities>(acm);
        }

        #region FitnessCertificate

        public void SaveOrUpdateFitnessCertificateDetails(FitnessCertificate fc)
        {
            if (fc.Id > 0)
            {
                PSF.Update<FitnessCertificate>(fc);
            }
            else PSF.Save<FitnessCertificate>(fc);
        }

        public Dictionary<long, IList<FitnessCertificate>> GetFitnessCertificateDetailsWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<FitnessCertificate>(page, pageSize, sortType, sortby, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Insurance
        public void SaveOrUpdateInsuranceDetails(Insurance ins)
        {
            if (ins.Id > 0)
            {
                PSF.Update<Insurance>(ins);
            }
            else PSF.Save<Insurance>(ins);
        }

        public Dictionary<long, IList<Insurance>> GetInsuranceDetails(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<Insurance>(page, pageSize, sortType, sortby, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region LocationMaster
        public void SaveOrUpdateLocationMasterDetails(LocationMaster lm)
        {
            if (lm.Id > 0)
            {
                PSF.Update<LocationMaster>(lm);
            }
            else PSF.Save<LocationMaster>(lm);
        }

        public void DeleteLocationMasterDetails(LocationMaster lm)
        {
            if (lm.Id > 0)
            {
                PSF.Delete<LocationMaster>(lm);
            }
        }

        public void DeleteDriverOTDetails(DriverOTDetails DOD)
        {
            if (DOD.Id > 0)
            {
                PSF.Delete<DriverOTDetails>(DOD);
            }
        }

        public Dictionary<long, IList<LocationMaster>> GetLocationMasterDetails(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<LocationMaster>(page, pageSize, sortType, sortby, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region DriverMaster
        public void CreateOrUpdateDriverMaster(DriverMaster dm)
        {
            if (dm.Id > 0)
            {
                PSF.Update<DriverMaster>(dm);
            }
            else PSF.Save<DriverMaster>(dm);
        }

        public void DeleteDriverMasterDetails(DriverMaster lm)
        {
            if (lm.Id > 0)
            {
                PSF.Delete<DriverMaster>(lm);
            }
        }

        public Dictionary<long, IList<DriverMaster>> GetDriverMasterDetails(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<DriverMaster>(page, pageSize, sortby, sortType, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region RouteMaster
        public void SaveOrUpdateRouteMasterDetails(RouteMaster rm)
        {
            if (rm.Id > 0)
            {
                PSF.Update<RouteMaster>(rm);
            }
            else PSF.Save<RouteMaster>(rm);
        }

        public void DeleteRouteMasterDetails(RouteMaster lm)
        {
            if (lm.Id > 0)
            {
                PSF.Delete<RouteMaster>(lm);
            }
        }

        public Dictionary<long, IList<RouteMaster>> GetRouteMasterDetails(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<RouteMaster>(page, pageSize, sortType, sortby, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public VehicleSubTypeMaster GetVehicleSubTypeMasterById(int Id)
        {
            try
            {
                VehicleSubTypeMaster vfm = null;
                if (Id > 0)
                    vfm = PSF.Get<VehicleSubTypeMaster>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return vfm;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateDistanceCoveredDetails(DistanceCoveredDetails vtm)
        {
            try
            {
                if (vtm != null)
                    PSF.SaveOrUpdate<DistanceCoveredDetails>(vtm);
                else { throw new Exception("VehicleTypeMaster is required and it cannot be null.."); }
                return vtm.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DistanceCoveredDetails GetDistanceCoveredDetailsById(int Id)
        {
            try
            {
                DistanceCoveredDetails vfm = null;
                if (Id > 0)
                    vfm = PSF.Get<DistanceCoveredDetails>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return vfm;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<DistanceCoveredDetails>> GetDistanceCoveredDetailsListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<DistanceCoveredDetails>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public IList<VehicleDistanceCovered> CreateOrUpdateVehicleDistanceCoveredList(IList<VehicleDistanceCovered> DLst)
        {
            try
            {
                if (DLst != null)
                    PSF.SaveOrUpdate<VehicleDistanceCovered>(DLst);
                else { throw new Exception("VehicleDistanceCoveredList is required and it cannot be null.."); }
                return DLst;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public VehicleDistanceCovered DeleteVehicleDistanceCoveredbyId(VehicleDistanceCovered sku)
        {
            try
            {
                if (sku != null)
                    PSF.Delete<VehicleDistanceCovered>(sku);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return sku;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateVehicleMaintenance(VehicleMaintenance DLst)
        {
            try
            {
                if (DLst != null)
                    PSF.SaveOrUpdate<VehicleMaintenance>(DLst);
                else { throw new Exception("VehicleMaintenance is required and it cannot be null.."); }
                return DLst.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long CreateOrUpdateVehicleACMaintenance(VehicleACMaintenance DLst)
        {
            try
            {
                if (DLst != null)
                    PSF.SaveOrUpdate<VehicleACMaintenance>(DLst);
                else { throw new Exception("VehicleACMaintenance is required and it cannot be null.."); }
                return DLst.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<VehicleMaintenance>> GetVehicleMaintenanceListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCount<VehicleMaintenance>(page, pageSize, sortBy, sortType, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<VehicleACMaintenance>> GetVehicleACMaintenanceListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCount<VehicleACMaintenance>(page, pageSize, sortBy, sortType, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }


        public long CreateOrUpdateFuelRefillDetails(FuelRefillDetails vtm)
        {
            try
            {
                if (vtm != null)
                    PSF.SaveOrUpdate<FuelRefillDetails>(vtm);
                else { throw new Exception("FuelRefillDetails is required and it cannot be null.."); }
                return vtm.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public FuelRefillDetails GetFuelRefillDetailsById(int Id)
        {
            try
            {
                FuelRefillDetails vfm = null;
                if (Id > 0)
                    vfm = PSF.Get<FuelRefillDetails>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return vfm;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<FuelRefillDetails>> GetFuelRefillDetailsListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<FuelRefillDetails>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public IList<VehicleFuelManagement> CreateOrUpdateVehicleFuelManagementList(IList<VehicleFuelManagement> DLst)
        {
            try
            {
                if (DLst != null)
                    PSF.SaveOrUpdate<VehicleFuelManagement>(DLst);
                else { throw new Exception("VehicleFuelManagement is required and it cannot be null.."); }
                return DLst;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public VehicleFuelManagement DeleteVehicleFuelManagementById(VehicleFuelManagement sku)
        {
            try
            {
                if (sku != null)
                    PSF.Delete<VehicleFuelManagement>(sku);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return sku;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public long CreateOrUpdateFitnessCertificateDetails(FitnessCertificateDetails vtm)
        {
            try
            {
                if (vtm != null)
                    PSF.SaveOrUpdate<FitnessCertificateDetails>(vtm);
                else { throw new Exception("FitnessCertificateDetails is required and it cannot be null.."); }
                return vtm.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public FitnessCertificateDetails GetFitnessCertificateDetailsById(int Id)
        {
            try
            {
                FitnessCertificateDetails vfm = null;
                if (Id > 0)
                    vfm = PSF.Get<FitnessCertificateDetails>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return vfm;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<FitnessCertificateDetails>> GetFitnessCertificateDetailsListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<FitnessCertificateDetails>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public IList<FitnessCertificate> CreateOrUpdateFitnessCertificateList(IList<FitnessCertificate> DLst)
        {
            try
            {
                if (DLst != null)
                    PSF.SaveOrUpdate<FitnessCertificate>(DLst);
                else { throw new Exception("FitnessCertificate is required and it cannot be null.."); }
                return DLst;
            }
            catch (Exception)
            {

                throw;
            }
        }



        public long CreateOrUpdateInsuranceBulkDetails(InsuranceBulkDetails vtm)
        {
            try
            {
                if (vtm != null)
                    PSF.SaveOrUpdate<InsuranceBulkDetails>(vtm);
                else { throw new Exception("InsuranceBulkDetails is required and it cannot be null.."); }
                return vtm.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public InsuranceBulkDetails GetInsuranceBulkDetailsById(int Id)
        {
            try
            {
                InsuranceBulkDetails vfm = null;
                if (Id > 0)
                    vfm = PSF.Get<InsuranceBulkDetails>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return vfm;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<InsuranceBulkDetails>> GetInsuranceBulkDetailsListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<InsuranceBulkDetails>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public IList<Insurance> CreateOrUpdateInsuranceList(IList<Insurance> DLst)
        {
            try
            {
                if (DLst != null)
                    PSF.SaveOrUpdate<Insurance>(DLst);
                else { throw new Exception("Insurance is required and it cannot be null.."); }
                return DLst;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<Insurance>> GetInsuranceDetailsWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<Insurance>(page, pageSize, sortby, sortType, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public Insurance GetInsuranceDetailsById(long Id)
        {
            try
            {
                Insurance vfm = null;
                if (Id > 0)
                    vfm = PSF.Get<Insurance>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return vfm;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Insurance DeleteInsuranceById(Insurance sku)
        {
            try
            {
                if (sku != null)
                    PSF.Delete<Insurance>(sku);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return sku;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public long CreateOrUpdateFinesAndPenalitiesBulkDetails(FinesAndPenalitiesBulkDetails vtm)
        {
            try
            {
                if (vtm != null)
                    PSF.SaveOrUpdate<FinesAndPenalitiesBulkDetails>(vtm);
                else { throw new Exception("InsuranceBulkDetails is required and it cannot be null.."); }
                return vtm.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public FinesAndPenalitiesBulkDetails GetFinesAndPenalitiesBulkDetailsById(int Id)
        {
            try
            {
                FinesAndPenalitiesBulkDetails vfm = null;
                if (Id > 0)
                    vfm = PSF.Get<FinesAndPenalitiesBulkDetails>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return vfm;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<FinesAndPenalitiesBulkDetails>> GetFinesAndPenalitiesBulkDetailsListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<FinesAndPenalitiesBulkDetails>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public IList<FinesAndPenalities> CreateOrUpdateFinesAndPenalitiesList(IList<FinesAndPenalities> DLst)
        {
            try
            {
                if (DLst != null)
                    PSF.SaveOrUpdate<FinesAndPenalities>(DLst);
                else { throw new Exception("Insurance is required and it cannot be null.."); }
                return DLst;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public FinesAndPenalities GetFinesAndPenalitiesById(long Id)
        {
            try
            {
                FinesAndPenalities vfm = null;
                if (Id > 0)
                    vfm = PSF.Get<FinesAndPenalities>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return vfm;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public FinesAndPenalities DeleteFinesAndPenalitiesById(FinesAndPenalities sku)
        {
            try
            {
                if (sku != null)
                    PSF.Delete<FinesAndPenalities>(sku);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return sku;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Vehicle Fuel and Distance covered Report added by micheal
        public Dictionary<long, IList<VehicleDistanceCoveredReport_vw>> GetVehicleDistanceCoveredReportListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCount<VehicleDistanceCoveredReport_vw>(page, pageSize, sortBy, sortType, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<VehicleFuelReport_vw>> GetVehicleFuelReportListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCount<VehicleFuelReport_vw>(page, pageSize, sortBy, sortType, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<VehicleDistanceCoveredChart_vw>> GetVehicleDistanceCoveredchartListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCount<VehicleDistanceCoveredChart_vw>(page, pageSize, sortBy, sortType, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<VehicleFuelQuantityChart_vw>> GetVehicleFuelchartListWithCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCount<VehicleFuelQuantityChart_vw>(page, pageSize, sortBy, sortType, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        public Dictionary<long, IList<VehicleDistanceFuelReport_vw>> GetVehicleDistanceFuelReport_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCount<VehicleDistanceFuelReport_vw>(page, pageSize, sortBy, sortType, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public long CreateOrUpdatePermitDetails(Permit per)
        {
            try
            {
                if (per != null)
                    PSF.SaveOrUpdate<Permit>(per);
                else { throw new Exception("PermitDetails is required and it cannot be null.."); }
                return per.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }



        public Dictionary<long, IList<Permit>> GetPermitDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCount<Permit>(page, pageSize, sortBy, sortType, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public int? CreateOrUpdateDriverOTDetails(DriverOTDetails per)
        {
            try
            {
                if (per != null)
                    PSF.SaveOrUpdate<DriverOTDetails>(per);
                else { throw new Exception("DriverOTDetails is required and it cannot be null.."); }
                return per.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<DriverOTDetails>> GetDriverOTDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCount<DriverOTDetails>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public int? CreateOrUpdateVehicleElectricalMaintenance(VehicleElectricalMaintenance vem)
        {
            try
            {
                if (vem != null)
                    PSF.SaveOrUpdate<VehicleElectricalMaintenance>(vem);
                else { throw new Exception("ElectricalMaintenance is required and it cannot be null.."); }
                return vem.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<VehicleElectricalMaintenance>> GetVehicleElectricalMaintenanceListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<VehicleElectricalMaintenance>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public int? CreateOrUpdateVehicleBodyMaintenance(VehicleBodyMaintenance vbm)
        {
            try
            {
                if (vbm != null)
                    PSF.SaveOrUpdate<VehicleBodyMaintenance>(vbm);
                else { throw new Exception("BodyMaintenance is required and it cannot be null.."); }
                return vbm.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<VehicleBodyMaintenance>> GetVehicleBodyMaintenanceListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<VehicleBodyMaintenance>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public int? CreateOrUpdateVehicleTyreMaintenance(VehicleTyreMaintenance vtm)
        {
            try
            {
                if (vtm != null)
                    PSF.SaveOrUpdate<VehicleTyreMaintenance>(vtm);
                else { throw new Exception("VehicleTyreMaintenance is required and it cannot be null.."); }
                return vtm.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<VehicleTyreMaintenance>> GetVehicleTyreMaintenanceListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<VehicleTyreMaintenance>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public void DeleteVehicleType(VehicleTypeMaster vtm)
        {
            if (vtm.Id > 0)
            {
                PSF.Delete<VehicleTypeMaster>(vtm);
            }
        }

        public IList<VehicleDistanceCovered> DeleteVehicleDistanceCoveredList(IList<VehicleDistanceCovered> DLst)
        {
            try
            {
                if (DLst != null)
                    PSF.DeleteAll<VehicleDistanceCovered>(DLst);
                else { throw new Exception("VehicleDistanceCoveredList is required and it cannot be null.."); }
                return DLst;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeletePermitDetailsById(Permit per)
        {
            if (per.Id > 0)
            {
                PSF.Delete<Permit>(per);
            }
        }

        public void DeleteFitnessCertificateById(FitnessCertificate fc)
        {
            if (fc.Id > 0)
            {
                PSF.Delete<FitnessCertificate>(fc);
            }
        }

        public void DeleteVehicleMaintenanceById(VehicleMaintenance vm)
        {
            if (vm.Id > 0)
            {
                PSF.Delete<VehicleMaintenance>(vm);
            }
        }

        public void DeleteVehicleACMaintenanceById(VehicleACMaintenance vacm)
        {
            if (vacm.Id > 0)
            {
                PSF.Delete<VehicleACMaintenance>(vacm);
            }
        }

        public void DeleteVehicleElectricalMaintenanceById(VehicleElectricalMaintenance vem)
        {
            if (vem.Id > 0)
            {
                PSF.Delete<VehicleElectricalMaintenance>(vem);
            }
        }

        public void DeleteVehicleBodyMaintenanceById(VehicleBodyMaintenance vbm)
        {
            if (vbm.Id > 0)
            {
                PSF.Delete<VehicleBodyMaintenance>(vbm);
            }
        }

        public void DeleteVehicleTyreMaintenanceById(VehicleTyreMaintenance vtm)
        {
            if (vtm.Id > 0)
            {
                PSF.Delete<VehicleTyreMaintenance>(vtm);
            }
        }

        public void DeleteVehicleSubTypeMasterById(VehicleSubTypeMaster vstm)
        {
            if (vstm.Id > 0)
            {
                PSF.Delete<VehicleSubTypeMaster>(vstm);
            }
        }

        #region Newly Added by Micheal
        public Dictionary<long, IList<VehicleTypeAndSubType>> GetVehicleTypeAndSubTypeListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<VehicleTypeAndSubType>> retValue = new Dictionary<long, IList<VehicleTypeAndSubType>>();
                return PSF.GetListWithSearchCriteriaCount<VehicleTypeAndSubType>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region DriverOTAllowance
        public Dictionary<long, IList<DriverOTAllowance_vw>> GetDriverOTAllowanceListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCount<DriverOTAllowance_vw>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        public DataTable GetDriverAllowance(string query) //Administrative
        {

            try
            {
                return PSF.ExecuteSqlUsingSQLCommand(query);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public Dictionary<long, IList<VehicleSubTypeMaster>> GetVehicleSubTypeMasterListWithPagingAndCriteriaLikeSearch(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<VehicleSubTypeMaster>> retValue = new Dictionary<long, IList<VehicleSubTypeMaster>>();
                return PSF.GetListWithLikeSearchCriteriaCount<VehicleSubTypeMaster>(page, pageSize, sortBy, sortType, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<LocationMaster>> GetLocationMasterDetailsWithPagingLikeSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<LocationMaster>(page, pageSize, sortby, sortType, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<TripPurposeMaster>> GetTripPurposeMasterDetailsWithPagingLikeSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<TripPurposeMaster>(page, pageSize, sortby, sortType, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public TripPurposeMaster GetPurposeByPurpose(string Purpose)
        {
            try
            {
                return PSF.Get<TripPurposeMaster>("Purpose", Purpose);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CreateOrUpdateTripPurposeMaster(TripPurposeMaster tpm)
        {
            try
            {
                if (tpm != null)
                    PSF.SaveOrUpdate<TripPurposeMaster>(tpm);
                else { throw new Exception("TripPurposeMaster is required and it cannot be null.."); }
                return tpm.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region For Tool bar search By Gobi
        public Dictionary<long, IList<VehicleMaintenance>> VehicleMaintenanceListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCountWithDecimal<VehicleMaintenance>(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<VehicleACMaintenance>> VehicleACMaintenanceListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCountWithDecimal<VehicleACMaintenance>(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<VehicleElectricalMaintenance>> VehicleElectricalMaintenanceListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCountWithDecimal<VehicleElectricalMaintenance>(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<VehicleBodyMaintenance>> VehicleBodyMaintenanceListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCountWithDecimal<VehicleBodyMaintenance>(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<VehicleTyreMaintenance>> VehicleTyreMaintenanceListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCountWithDecimal<VehicleTyreMaintenance>(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);

            }
            catch (Exception)
            {
                throw;
            }
        }


        public Dictionary<long, IList<VehicleDistanceCovered>> VehicleDistanceCoveredListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCountWithDecimal<VehicleDistanceCovered>(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<VehicleFuelManagement>> VehicleFuelManagementListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCountWithDecimal<VehicleFuelManagement>(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<FinesAndPenalities>> FinesAndPenalitiesListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCountWithDecimal<FinesAndPenalities>(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<FitnessCertificate>> FitnessCertificateListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCountWithDecimal<FitnessCertificate>(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<Insurance>> InsuranceListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCountWithDecimal<Insurance>(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<Permit>> PermitListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCountWithDecimal<Permit>(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);

            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion

        public Dictionary<long, IList<VehicleReport>> GetVehicleReportListWithsearchCriteriaLikeSearch(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCount<VehicleReport>(page, pageSize, sortType, sortBy, criteria, alias);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        #region "Tyre Management"

        public int CreateOrUpdateTyreInvoiceDetails(TyreInvoiceDetails tid)
        {
            try
            {
                if (tid != null)
                    PSF.SaveOrUpdate<TyreInvoiceDetails>(tid);
                else { throw new Exception("TyreInvoiceDetails is required and it cannot be null.."); }
                return tid.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<TyreInvoiceDetails>> GetTyreInvoiceDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<TyreInvoiceDetails>> retValue = new Dictionary<long, IList<TyreInvoiceDetails>>();
                return PSF.GetListWithSearchCriteriaCount<TyreInvoiceDetails>(page, pageSize, sortBy, sortType, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }
        public TyreInvoiceDetails GetTyreInvoiceDetailsById(int Id)
        {
            try
            {
                TyreInvoiceDetails tid = null;
                if (Id > 0)
                    tid = PSF.Get<TyreInvoiceDetails>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return tid;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CreateOrUpdateTyreDetails(TyreDetails td)
        {
            try
            {
                if (td != null)
                    PSF.SaveOrUpdate<TyreDetails>(td);
                else { throw new Exception("TyreDetails is required and it cannot be null.."); }
                return td.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<TyreDetails>> GetTyreDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<TyreDetails>> retValue = new Dictionary<long, IList<TyreDetails>>();
                return PSF.GetListWithSearchCriteriaCount<TyreDetails>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public TyreDetails GetTyreDetailsByTyreNo(string TyreNo)
        {
            try
            {
                return PSF.Get<TyreDetails>("TyreNo", TyreNo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<TyreDetailsAndInvoiceDetails>> GetTyreDetailsAndInvoiceDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<TyreDetailsAndInvoiceDetails>> retValue = new Dictionary<long, IList<TyreDetailsAndInvoiceDetails>>();
                return PSF.GetListWithSearchCriteriaCount<TyreDetailsAndInvoiceDetails>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CreateOrUpdateTyreTyreAssignedList(TyreAssignedList tal)
        {
            try
            {
                if (tal != null)
                    PSF.SaveOrUpdate<TyreAssignedList>(tal);
                else { throw new Exception("TyreInvoiceDetails is required and it cannot be null.."); }
                return tal.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<TyreAssignedList>> GetTyreAssignedListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<TyreAssignedList>> retValue = new Dictionary<long, IList<TyreAssignedList>>();
                return PSF.GetListWithSearchCriteriaCount<TyreAssignedList>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<TyreDetailsAndInvoiceDetails>> GetTyreDetailsAndInvoiceDetailsListWithAliasPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria, string[] criteriaAlias)
        {
            try
            {
                Dictionary<long, IList<TyreDetailsAndInvoiceDetails>> retValue = new Dictionary<long, IList<TyreDetailsAndInvoiceDetails>>();
                return PSF.GetListWithSearchCriteriaCount<TyreDetailsAndInvoiceDetails>(page, pageSize, sortBy, sortType, criteria, criteriaAlias);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<TransportVendorMaster>> GetTransportVendorMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<TransportVendorMaster>> retValue = new Dictionary<long, IList<TransportVendorMaster>>();
                return PSF.GetListWithSearchCriteriaCount<TransportVendorMaster>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CreateOrUpdateTransportVendorMaster(TransportVendorMaster tvm)
        {
            try
            {
                if (tvm != null)
                    PSF.SaveOrUpdate<TransportVendorMaster>(tvm);
                else { throw new Exception("TransportVendorMaster is required and it cannot be null.."); }
                return tvm.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion "Tyre Management"

        #region Added By Gobi
        public LocationMaster GetLocationMasterDetailsById(long Id)
        {
            try
            {
                LocationMaster Loc = null;
                if (Id > 0)
                    Loc = PSF.Get<LocationMaster>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return Loc;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DriverMaster GetDriverMasterDetailsById(long Id)
        {
            try
            {
                DriverMaster Driver = null;
                if (Id > 0)
                    Driver = PSF.Get<DriverMaster>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return Driver;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public RouteMaster GetRouteMasterDetailsById(long Id)
        {
            try
            {
                RouteMaster Route = null;
                if (Id > 0)
                    Route = PSF.Get<RouteMaster>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return Route;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public TransportVendorMaster GetTransportVendorMasterDetailsById(long Id)
        {
            try
            {
                TransportVendorMaster Vendor = null;
                if (Id > 0)
                    Vendor = PSF.Get<TransportVendorMaster>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return Vendor;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Student Route Configuration
        public void CreateOrUpdateRouteConfiguration(RouteConfiguration RouteConfig)
        {
            if (RouteConfig.Id > 0)
            {
                PSF.Update<RouteConfiguration>(RouteConfig);
            }
            else PSF.Save<RouteConfiguration>(RouteConfig);
        }

        public bool DeleteRouteConfiguration(long id)
        {
            try
            {
                RouteConfiguration RouteConfiguration = PSF.Get<RouteConfiguration>(id);
                PSF.Delete<RouteConfiguration>(RouteConfiguration);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteRouteConfiguration(long[] id)
        {
            try
            {
                IList<RouteConfiguration> RouteConfiguration = PSF.GetListByIds<RouteConfiguration>(id);
                PSF.DeleteAll<RouteConfiguration>(RouteConfiguration);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<RouteConfiguration>> GetRouteConfigurationListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<RouteConfiguration>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public RouteConfiguration GetRouteConfigurationByLocationName(long RouteMasterId, string LocationName, string LocationOtherDetails)
        {
            try
            {
                RouteConfiguration RouteConfiguration = null;
                if (!string.IsNullOrEmpty(LocationName) && RouteMasterId > 0)
                    RouteConfiguration = PSF.Get<RouteConfiguration>("RouteMasterId", RouteMasterId, "LocationName", LocationName, "LocationDetails", LocationOtherDetails);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return RouteConfiguration;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public RouteConfiguration GetRouteConfigurationByStopOrderNumber(long RouteMasterId, long StopOrderNumber)
        {
            try
            {
                RouteConfiguration RouteConfiguration = null;
                if (StopOrderNumber>0 && RouteMasterId > 0)
                    RouteConfiguration = PSF.Get<RouteConfiguration>("RouteMasterId", RouteMasterId, "StopOrderNumber", StopOrderNumber);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return RouteConfiguration;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region added by benadict
        public Dictionary<long, IList<StudentTemplate>> GetStudentListListWithsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<StudentTemplate>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public Dictionary<long, IList<RouteMaster>> GetRouteMstrListWithsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<RouteMaster>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public Dictionary<long, IList<RouteConfiguration>> GetRouteConfigListWithsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<RouteConfiguration>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        #endregion
        public Dictionary<long, IList<RouteConfiguration>> GetLocationListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<RouteConfiguration>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool UpdateRouteInStudentTemplate(string selPreRegNum, string RouteId, string notSelPreRegNum, string RouteStudCode, string LocationId, string userId)
        {
            bool retValue = false;
            try
            {
                //string userId = base.ValidateUser();
                //logic to update student template.
                if (!string.IsNullOrEmpty(selPreRegNum))
                {
                    //RouteMasterConfig_vw RouteMasterConfig_vw = GetRouteMasterConfig_vwByLocationName(RouteStudCode);
                    //if (RouteMasterConfig_vw != null && RouteMasterConfig_vw.NoOfStudents < 51)
                    //{

                    //}

                    //string[] arrPreRegNm = selPreRegNum.Split(',');
                    string querySel = "update StudentTemplate set RouteId='" + RouteStudCode + "' where preRegNum in (" + selPreRegNum + ")";
                    PSF.ExecuteSql(querySel);
                    string[] StudId = selPreRegNum.Split(',');
                    for (int i = 0; i < StudId.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(StudId[i]))
                        {
                            RouteStudentListConfig routeStudeConfig = new RouteStudentListConfig();
                            routeStudeConfig = PSF.Get<RouteStudentListConfig>("PreRegNum", Convert.ToInt64(StudId[i]));
                            if (routeStudeConfig == null)
                            {
                                RouteStudentListConfig obj = new RouteStudentListConfig();
                                obj.RouteId = Convert.ToInt64(RouteId);
                                obj.LocationId = Convert.ToInt64(LocationId);
                                obj.PreRegNum = Convert.ToInt64(StudId[i]);
                                obj.RouteStudCode = RouteStudCode;
                                obj.CreatedBy = userId;
                                obj.DateCreated = DateTime.Now;
                                PSF.SaveOrUpdate<RouteStudentListConfig>(obj);
                            }
                        }
                    }

                    //update to new table
                    //get record from existing rows.if no rows then insert otherwise update
                }
                if (!string.IsNullOrEmpty(notSelPreRegNum))
                {
                    string queryNotSel = "update StudentTemplate set RouteId=NULL where preRegNum in (" + notSelPreRegNum + ")";
                    PSF.ExecuteSql(queryNotSel);
                    //remove from new table if exists.
                    //get record from existing rows.if no rows no need to do anythiing otherwise delete
                    string queryDelEntry = "delete from RouteStudentListconfig where PreRegNum in (" + notSelPreRegNum + ")";
                    PSF.ExecuteSql(queryDelEntry);
                }
                retValue = true;
                return retValue;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion
        public Dictionary<long, IList<RouteMasterConfig_vw>> GetRouteMasterConfig_vwListWithsearchCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<RouteMasterConfig_vw>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<RouteStudentConfigurationPDF_vw>> GetRouteStudConfigPDFListWithsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<RouteStudentConfigurationPDF_vw>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        #region RouteStudentListConfiguration
        public void SaveOrUpdateRouteStudConfigDetails(RouteStudConfig rm)
        {
            if (rm.Id > 0)
            {
                PSF.Update<RouteStudConfig>(rm);
            }
            else PSF.Save<RouteStudConfig>(rm);
        }
        public IList<StudentTemplateView> GetStudentNameListByQuery(string LocationName, string Campus)
        {
            try
            {
                string query = "SELECT PreRegNum,Name FROM StudentTemplate WHERE Transport = 1 and RouteId Is Null and LocationName=('" + LocationName + "') and Campus=('" + Campus + "');";
                IList list = PSF.ExecuteSql(query);
                IList<StudentTemplateView> NameList = new List<StudentTemplateView>();
                foreach (var item in list)
                {
                    StudentTemplateView test = new StudentTemplateView();
                    test.PreRegNum = Convert.ToInt64(((object[])(item))[0]);
                    test.Name = Convert.ToString(((object[])(item))[1]);

                    NameList.Add(test);
                }
                return NameList;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public RouteConfiguration GetLocationNameById(long Id)
        {
            try
            {
                RouteConfiguration RouteConfig = null;
                if (Id > 0)
                    RouteConfig = PSF.Get<RouteConfiguration>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return RouteConfig;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region RouteStudentListConfig
        public Dictionary<long, IList<RouteStudentListConfigView>> GetRouteStudentListConfigWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<RouteStudentListConfigView>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public RouteMasterConfig_vw GetRouteMasterConfig_vwByRouteStudCode(string RouteStudCode)
        {
            try
            {
                RouteMasterConfig_vw RouteMasterConfig_vw = null;
                if (!string.IsNullOrEmpty(RouteStudCode))
                    RouteMasterConfig_vw = PSF.Get<RouteMasterConfig_vw>("RouteStudCode", RouteStudCode);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return RouteMasterConfig_vw;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #region Route Configuration Report
        public Dictionary<long, IList<StudentsRouteConfigReport_vw>> GetStudentsRouteConfigReport_vwListWithsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<StudentsRouteConfigReport_vw>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        #endregion

        public long CreateOrUpdateStudentLocationNameMaster(StudentLocationMaster slm)
        {
            try
            {
                if (slm != null)
                    PSF.SaveOrUpdate<StudentLocationMaster>(slm);
                else { throw new Exception("StudentLocationMaster is required and it cannot be null.."); }
                return slm.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<StudentLocationMaster>> GetStudentLocationMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<StudentLocationMaster>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public StudentLocationMaster GetStudentLocationMasterByLocationName(string Campus, string LocationName)
        {
            try
            {
                StudentLocationMaster StudentLocationMaster = null;
                if (!string.IsNullOrEmpty(LocationName))
                    StudentLocationMaster = PSF.Get<StudentLocationMaster>("LocationName", LocationName, "Campus", Campus);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return StudentLocationMaster;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #region ConfiguredStudentList Form
        public StudentTemplateView GetStudentDetailsByPreRegNo(long PreRegNo)
        {
            try
            {
                return PSF.Get<StudentTemplateView>("PreRegNum", PreRegNo);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        #region DriverAttendance

        //public Dictionary<long, IList<DriverAttendance>> GetStudentTemplateForAnAttendanceListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        //{
        //    try
        //    {
        //        return PSF.GetListWithEQSearchCriteriaCount<DriverAttendance>(page, pageSize, sortType, sortby, criteria);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public Dictionary<long, IList<DriverAttendance>> GetAbsentListForAnAttendanceListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        //{
        //    try
        //    {
        //        return PSF.GetListWithEQSearchCriteriaCount<DriverAttendance>(page, pageSize, sortType, sortby, criteria);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}


        public long CreateOrUpdateDriverAttendanceList(DriverAttendance att)
        {
            try
            {
                if (att != null)
                    PSF.SaveOrUpdate<DriverAttendance>(att);
                else { throw new Exception("Value is required and it cannot be null.."); }
                return att.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DriverAttendance GetDriverAttentanceById(long Id)
        {
            try
            {
                DriverAttendance attdel = null;
                if (Id > 0)
                    attdel = PSF.Get<DriverAttendance>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return attdel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long DeleteDriverAttendancevalue(DriverAttendance att)
        {
            try
            {
                if (att != null)
                    PSF.Delete<DriverAttendance>(att);
                else { throw new Exception("Value is required and it cannot be null.."); }
                return 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<DriverAttendance>> GetDriverAttendanceDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCount<DriverAttendance>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<DriverAttendanceReport>> GetDriverListListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<DriverAttendanceReport>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<DriverAttendance>> GetDriverMasterForAnAttendanceListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<DriverAttendance>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<DriverAttendance>> GetAbsentListForAnAttendanceListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<DriverAttendance>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<DriverAttendanceMonthReport>> GetMonthlyAbsentListForAnAttendanceListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<DriverAttendanceMonthReport>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<DriverAttendanceMonthReport>> GetDriverAttendanceMonthReportListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCount<DriverAttendanceMonthReport>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }


        #endregion


        #region Driver ID card
        public Dictionary<long, IList<DriverMaster>> GetDriverListWithEQsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<DriverMaster>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        #endregion
        public DriverMaster GetDriverDetailsByRegNo(long RegNo)
        {
            try
            {
                DriverMaster Driver = null;
                if (RegNo > 0)
                    Driver = PSF.Get<DriverMaster>("DriverRegNo", RegNo);
                else { throw new Exception("RegNo is required and it cannot be 0"); }
                return Driver;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<DriverRegNumDetails>> GetDriverRegNumDetailsListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<DriverRegNumDetails>> retValue = new Dictionary<long, IList<DriverRegNumDetails>>();
                return PSF.GetListWithSearchCriteriaCount<DriverRegNumDetails>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateDriverRegNumDetails(DriverRegNumDetails srn)
        {
            try
            {
                if (srn != null)
                    PSF.SaveOrUpdate<DriverRegNumDetails>(srn);
                else { throw new Exception("Transport Driver RegNo is required and it cannot be null.."); }
                return srn.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Driver family details

        public Dictionary<long, IList<DriverFamilyDetails>> GetDriverFamilyDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<DriverFamilyDetails>> familydet = new Dictionary<long, IList<DriverFamilyDetails>>();
                // return PSF.GetListWithExactSearchCriteriaCount<FamilyDetails>(page, pageSize, sortType, sortBy, criteria);
                return PSF.GetListWithEQSearchCriteriaCount<DriverFamilyDetails>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DriverFamilyDetails GetDriverFamilyDetailsById(long Id)
        {
            try
            {
                DriverFamilyDetails FamilyDetails = null;
                if (Id > 0)
                    FamilyDetails = PSF.Get<DriverFamilyDetails>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return FamilyDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateDriverFamilyDatails(DriverFamilyDetails FD)
        {
            try
            {
                if (FD != null)
                    PSF.SaveOrUpdate<DriverFamilyDetails>(FD);
                else { throw new Exception("Driver management is required and it cannot be null.."); }
                return FD.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteDriverFamilyDetails(long id)
        {
            try
            {
                DriverFamilyDetails FamilyDetails = PSF.Get<DriverFamilyDetails>(id);
                PSF.Delete<DriverFamilyDetails>(FamilyDetails);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteDriverFamilyDetails(long[] id)
        {
            try
            {
                IList<DriverFamilyDetails> FamilyDetails = PSF.GetListByIds<DriverFamilyDetails>(id);
                PSF.DeleteAll<DriverFamilyDetails>(FamilyDetails);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion

        //VehicleElectricalMaintenance_Vw
        #region VehicleElectricalMaintenance_Vw
        public Dictionary<long, IList<VehicleElectricalMaintenance_Vw>> GetVehicleElectricalMaintenance_VwWithPagingLikeSearch(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<VehicleElectricalMaintenance_Vw>(page, pageSize, sortby, sortType, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        //VehicleMaintenance_Vw

        #region VehicleMaintenance_Vw
        public Dictionary<long, IList<VehicleMaintance_Vw>> GetVehicleMaintenance_VwListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCount<VehicleMaintance_Vw>(page, pageSize, sortBy, sortType, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        //VehicleTyreMaintenance_Vw
        #region VehicleTyreMaintenance_Vw
        public Dictionary<long, IList<VehicleTyreMaintenance_Vw>> GetVehicleTyreMaintenance_VwWithPagingLikeSearch(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCountWithDecimal<VehicleTyreMaintenance_Vw>(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        //VehicleACMaintenanceReport_Vw

        #region VehicleACMaintenanceReport_Vw
        public Dictionary<long, IList<VehicleAcMaintanceReport_Vw>> GetVehicleACMaintenanceReport_VwListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCount<VehicleAcMaintanceReport_Vw>(page, pageSize, sortBy, sortType, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        //VehicleBodyMaintainance_Vw
        #region VehicleBodyMaintainance_Vw
        public Dictionary<long, IList<VehicleBodyMaintainance_Vw>> GetVehicleBodyMaintainance_VwWithPagingLikeSearch(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCountWithDecimal<VehicleBodyMaintainance_Vw>(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        //DistanceCovered_Vw
        #region DistanceCovered_Vw
        public Dictionary<long, IList<DistanceCovered_Vw>> GetDistanceCovered_VwWithPagingLikeSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<DistanceCovered_Vw>(page, pageSize, sortby, sortType, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public Dictionary<long, IList<FuelRefilDetails_Vw>> GetFuelTypeDetails(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<FuelRefilDetails_Vw>(page, pageSize, sortby, sortType, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<DriverOTReportDetails>> GetDriverOTReportDetailsWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<DriverOTReportDetails>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable FuelReportDetails(string query) //Administrative
        {

            try
            {
                return PSF.ExecuteSqlUsingSQLCommand(query);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public DriverMaster GetDriverDetailsUsingID(long Id)
        {
            try
            {
                DriverMaster DriverDetails = null;
                if (Id > 0)
                {
                    DriverDetails = PSF.Get<DriverMaster>(Id);
                }
                return DriverDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<VehicleDistanceCovered_vw>> GetVehicleDistanceCovered_vw(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<VehicleDistanceCovered_vw>(page, pageSize, sortby, sortType, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        #region TripMaster
        public void SaveOrUpdateTripMasterDetails(TripMaster trpm)
        {
            if (trpm.TripId > 0)
            {
                PSF.Update<TripMaster>(trpm);
            }
            else PSF.Save<TripMaster>(trpm);
        }
        public bool DeleteTripMasterDetails(long[] Id)
        {
            try
            {
                IList<TripMaster> tasksList = PSF.GetListByIds<TripMaster>(Id);
                PSF.DeleteAll<TripMaster>(tasksList);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<TripMaster>> GetTripMasterDetails(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<TripMaster>(page, pageSize, sortType, sortby, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public TripMaster GetTripMasterDetailsById(long TripId)
        {
            try
            {
                TripMaster trp = null;
                if (TripId > 0)
                    trp = PSF.Get<TripMaster>(TripId);

                else { throw new Exception("Id is required and it cannot be 0"); }
                return trp;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public TripMaster GetTripMasterDetailsByTripName(string TripName)
        {
            try
            {
                TripMaster trp = null;
                if (!string.IsNullOrEmpty(TripName))
                    trp = PSF.Get<TripMaster>("TripName",TripName);

                else { throw new Exception("TripName is required and it cannot be 0"); }
                return trp;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Vehicle cost details added by john
        public void SaveOrUpdateVehicleCostDetails(VehicleCostDetails vcdl)
        {
            if (vcdl.VehicleCostId > 0)
            {
                PSF.Update<VehicleCostDetails>(vcdl);
            }
            else PSF.Save<VehicleCostDetails>(vcdl);
        }
        public bool DeleteVehicleCostDetails(long[] Id)
        {
            try
            {
                IList<VehicleCostDetails> tasksList = PSF.GetListByIds<VehicleCostDetails>(Id);
                PSF.DeleteAll<VehicleCostDetails>(tasksList);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<VehicleCostDetails>> VehicleCostDetailsListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCountWithDecimal<VehicleCostDetails>(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public VehicleCostDetails GetVehicleCostDetailsByTripName(long VehicleId, string TypeOfTrip, DateTime VehicleTravelDate)
        {
            try
            {
                VehicleCostDetails VehicleDEtails = null;
                if (!string.IsNullOrEmpty(TypeOfTrip))
                    VehicleDEtails = PSF.Get<VehicleCostDetails>("VehicleId", VehicleId, "TypeOfTrip", TypeOfTrip, "VehicleTravelDate", VehicleTravelDate);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return VehicleDEtails;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public VehicleCostDetails GetVehicleCostDetailsById(long VehicleCostId)
        {
            try
            {
                VehicleCostDetails vcd = null;
                if (VehicleCostId >= 0)
                    vcd = PSF.Get<VehicleCostDetails>(VehicleCostId);

                else { throw new Exception("Id is required and it cannot be 0"); }
                return vcd;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<VehicleCostDetails_VW>> GetVehicleCostDetails_VWListWithPagingAndCriteriaLikeSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<VehicleCostDetails_VW>(page, pageSize, sortType, sortby, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public VehicleCostDetails GetVehicleCostDetailsByTravelDate(long VehicleId, DateTime VehicleTravelDate)
        {
            try
            {
                VehicleCostDetails VehicleDEtails = null;
                if (VehicleId > 0)
                    VehicleDEtails = PSF.Get<VehicleCostDetails>("VehicleId", VehicleId, "VehicleTravelDate", VehicleTravelDate);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return VehicleDEtails;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<VehicleCostDetailsReport_sp>> GetVehicleCostDetailsReportSP(string Campus, string VehicleNo, DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                return PSF.ExecuteStoredProcedurewithOptionalParametersByDictonary<VehicleCostDetailsReport_sp>("VehicleCostDetailsReport",
                         new[] { new SqlParameter("Campus", Campus),
                             new SqlParameter("VehicleNo",VehicleNo),
                             new SqlParameter("FromDate",FromDate),
                             new SqlParameter("ToDate",ToDate),                             
                    });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        #region by john naveen
        public Dictionary<long, IList<RouteMaster>> GetRouteMasterDetailsListWithLikeSearchCriteriaCount(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<RouteMaster>(page, pageSize, sortType, sortby, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion         

        #region GPS Tracking Device
        public long SaveOrUpdateGPSTrackingDeviceMaster(GPS_TrackingDeviceMaster TrackingDeviceMasterObj)
        {
            try
            {
                if (TrackingDeviceMasterObj != null)
                    PSF.SaveOrUpdate<GPS_TrackingDeviceMaster>(TrackingDeviceMasterObj);
                else { throw new Exception("GPS Tracking Device Master Id is required and it cannot be null.."); }
                return TrackingDeviceMasterObj.GPS_TrackingDeviceMaster_Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteGPSTrackingDeviceMaster(long[] Id)
        {
            try
            {
                IList<GPS_TrackingDeviceMaster> tasksList = PSF.GetListByIds<GPS_TrackingDeviceMaster>(Id);
                PSF.DeleteAll<GPS_TrackingDeviceMaster>(tasksList);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<GPS_TrackingDeviceMaster>> GPSTrackingDeviceMasterListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCountWithDecimal<GPS_TrackingDeviceMaster>(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public GPS_TrackingDeviceMaster GetGPS_TrackingDeviceMasterByName(string Campus, string BrandName, string ModelName, string IMEINmber)
        {
            try
            {
                GPS_TrackingDeviceMaster GPS_TrackingDeviceMaster = null;
                if (!string.IsNullOrEmpty(Campus))
                    if (!string.IsNullOrEmpty(BrandName))
                        if (!string.IsNullOrEmpty(ModelName))
                            if (!string.IsNullOrEmpty(IMEINmber))
                                GPS_TrackingDeviceMaster = PSF.Get<GPS_TrackingDeviceMaster>("Campus", Campus, "BrandName", BrandName, "ModelName", ModelName, "IMEINmber", IMEINmber);
                            else { throw new Exception("Id is required and it cannot be 0"); }
                return GPS_TrackingDeviceMaster;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region Purpose Master
        public long SaveOrUpdatePurposeMasterMaster(PurposeMaster pm)
        {
            try
            {
                if (pm != null)
                    PSF.SaveOrUpdate<PurposeMaster>(pm);
                else { throw new Exception("GPS Tracking Device Master Id is required and it cannot be null.."); }
                return pm.Purpose_Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeletePurposeMaster(long[] Id)
        {
            try
            {
                IList<PurposeMaster> tasksList = PSF.GetListByIds<PurposeMaster>(Id);
                PSF.DeleteAll<PurposeMaster>(tasksList);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<PurposeMaster>> GetPurposeMasterDetailsListWithLikeSearchCriteriaCount(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<PurposeMaster>(page, pageSize, sortType, sortby, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public PurposeMaster GetPurposeMasterDetailsByPurposeName(string PurposeName)
        {
            try
            {
                PurposeMaster pm = null;
                if (!string.IsNullOrEmpty(PurposeName))
                    pm = PSF.Get<PurposeMaster>("PurposeName", PurposeName);

                else { throw new Exception("TripName is required and it cannot be 0"); }
                return pm;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region DailyUsageVehicleMaster
        public DailyUsageVehicleMaster GetDailyUsageVehicleMasterById(int Id)
        {
            try
            {
                DailyUsageVehicleMaster Dvm = null;
                if (Id > 0)
                    Dvm = PSF.Get<DailyUsageVehicleMaster>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return Dvm;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region DailyUsageVehicleMaster view
        public Dictionary<long, IList<DailyUsageVehicleMaster_vw>> GetDailyUsageVehicleMaster_vwListWithsearchCriteriaLikeSearch(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<DailyUsageVehicleMaster_vw>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        #endregion
        #region ForNewVehicleCostDetails
        public Dictionary<long, IList<VehicleCostDetails_Updated>> VehicleCostDetails_UpdatedListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCountWithDecimal<VehicleCostDetails_Updated>(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public VehicleCostDetails_Updated GetVehicleCostDetails_UpdatedByTravelDate(long VehicleId, DateTime VehicleTravelDate)
        {
            try
            {
                VehicleCostDetails_Updated VehicleDEtails = null;
                if (VehicleId > 0)
                    VehicleDEtails = PSF.Get<VehicleCostDetails_Updated>("VehicleId", VehicleId, "VehicleTravelDate", VehicleTravelDate);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return VehicleDEtails;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public VehicleCostDetails_Updated GetVehicleCostDetails_UpdatedByTripName(long VehicleId, string TypeOfTrip, DateTime VehicleTravelDate)
        {
            try
            {
                VehicleCostDetails_Updated VehicleDEtails = null;
                if (!string.IsNullOrEmpty(TypeOfTrip))
                    VehicleDEtails = PSF.Get<VehicleCostDetails_Updated>("VehicleId", VehicleId, "TypeOfTrip", TypeOfTrip, "VehicleTravelDate", VehicleTravelDate);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return VehicleDEtails;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<NewVehicleTypeMaster>> GetNewVehicleTypeMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<NewVehicleTypeMaster>> retValue = new Dictionary<long, IList<NewVehicleTypeMaster>>();
                return PSF.GetListWithSearchCriteriaCount<NewVehicleTypeMaster>(page, pageSize, sortBy, sortType, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public void SaveOrUpdateVehicleCostDetails_Updated(VehicleCostDetails_Updated vcdl)
        {
            if (vcdl.VehicleCostId > 0)
            {
                PSF.Update<VehicleCostDetails_Updated>(vcdl);
            }
            else PSF.Save<VehicleCostDetails_Updated>(vcdl);
        }

        #endregion
        #region DailyUsageVehicleMasterList
        public Dictionary<long, IList<DailyUsageVehicleMaster>> GetDailyUsageVehicleMasterListWithsearchCriteriaLikeSearch(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<DailyUsageVehicleMaster>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        #endregion
        public Dictionary<long, IList<VehicleCostDetails_Updated_VW>> GetVehicleCostDetails_Updated_VWListWithPagingAndCriteriaLikeSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<VehicleCostDetails_Updated_VW>(page, pageSize, sortType, sortby, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public VehicleCostDetails_Updated GetVehicleCostDetails_UpdatedById(long VehicleCostId)
        {
            try
            {
                VehicleCostDetails_Updated vcd = null;
                if (VehicleCostId >= 0)
                    vcd = PSF.Get<VehicleCostDetails_Updated>(VehicleCostId);

                else { throw new Exception("Id is required and it cannot be 0"); }
                return vcd;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region Vehicle Fuel entry
        public Dictionary<long, IList<VehicleFuelRefillEntry>> GetVehicleFuelRefillEntryDetailsWithPagingLikeSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<VehicleFuelRefillEntry>(page, pageSize, sortby, sortType, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateVehicleFuelRefillEntry(VehicleFuelRefillEntry VehicleFuelRefillEntry)
        {
            try
            {
                if (VehicleFuelRefillEntry != null)
                    PSF.SaveOrUpdate<VehicleFuelRefillEntry>(VehicleFuelRefillEntry);
                else { throw new Exception("Id is required and it cannot be null.."); }
                return VehicleFuelRefillEntry.FuelRefillId;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public VehicleFuelRefillEntry GetVehicleFuelRefillEntryById(long Id)
        {
            try
            {
                VehicleFuelRefillEntry vcd = null;
                if (Id >= 0)
                    vcd = PSF.Get<VehicleFuelRefillEntry>(Id);

                else { throw new Exception("Id is required and it cannot be 0"); }
                return vcd;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //Added by prabakaran
        public VehicleFuelRefillEntry GetVehicleFuelRefillEntryByVehicleCostId(long VehicleCostId)
        {
            try
            {
                VehicleFuelRefillEntry vcd = null;
                if (VehicleCostId >= 0)
                    vcd = PSF.Get<VehicleFuelRefillEntry>("VehicleCostId", VehicleCostId);

                else { throw new Exception("Id is required and it cannot be 0"); }
                return vcd;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region supplier master for transport
        public Dictionary<long, IList<TransportSupplierMaster>> GetTransportSupplierMasterWithPagingLikeSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<TransportSupplierMaster>(page, pageSize, sortby, sortType, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateTransportSupplierMaster(TransportSupplierMaster TransportSupplierMaster)
        {
            try
            {
                if (TransportSupplierMaster != null)
                    PSF.SaveOrUpdate<TransportSupplierMaster>(TransportSupplierMaster);
                else { throw new Exception("Id is required and it cannot be null.."); }
                return TransportSupplierMaster.SupplierId;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public TransportSupplierMaster GetTransportSupplierMasterBySupplierName(String SupplierName, string SupplierType)
        {
            try
            {
                return PSF.Get<TransportSupplierMaster>("SupplierName", SupplierName, "SupplierType", SupplierType);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteTransportSupplierMaster(long[] Id)
        {
            try
            {
                IList<TransportSupplierMaster> SupplierMaster = PSF.GetListByIds<TransportSupplierMaster>(Id);
                PSF.DeleteAll<TransportSupplierMaster>(SupplierMaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Vehicle Maintenance Entry
        public Dictionary<long, IList<VehicleMaintenanceEntry>> GetVehicleMaintenanceEntryDetailsWithPagingLikeSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<VehicleMaintenanceEntry>(page, pageSize, sortby, sortType, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateVehicleMaintenanceEntry(VehicleMaintenanceEntry VehicleMaintenanceEntry)
        {
            try
            {
                if (VehicleMaintenanceEntry != null)
                    PSF.SaveOrUpdate<VehicleMaintenanceEntry>(VehicleMaintenanceEntry);
                else { throw new Exception("Id is required and it cannot be null.."); }
                return VehicleMaintenanceEntry.MaintenanceId;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public VehicleMaintenanceEntry GetVehicleMaintenanceEntryByVehicleCostId(long VehicleCostId)
        {
            try
            {
                VehicleMaintenanceEntry vcd = null;
                if (VehicleCostId >= 0)
                    vcd = PSF.Get<VehicleMaintenanceEntry>("VehicleCostId", VehicleCostId);

                else { throw new Exception("Id is required and it cannot be 0"); }
                return vcd;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public VehicleMaintenanceEntry GetVehicleMaintenanceEntryById(long Id)
        {
            try
            {
                VehicleMaintenanceEntry vme = null;
                if (Id >= 0)
                    vme = PSF.Get<VehicleMaintenanceEntry>(Id);

                else { throw new Exception("Id is required and it cannot be 0"); }
                return vme;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Vehicle Service Entry
        public Dictionary<long, IList<VehicleService>> GetVehicleServiceDetailsWithPagingLikeSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<VehicleService>(page, pageSize, sortby, sortType, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateVehicleService(VehicleService VehicleService)
        {
            try
            {
                if (VehicleService != null)
                    PSF.SaveOrUpdate<VehicleService>(VehicleService);
                else { throw new Exception("Id is required and it cannot be null.."); }
                return VehicleService.ServiceId;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public VehicleService GetVehicleServiceByVehicleCostId(long VehicleCostId)
        {
            try
            {
                VehicleService vcd = null;
                if (VehicleCostId >= 0)
                    vcd = PSF.Get<VehicleService>("VehicleCostId", VehicleCostId);

                else { throw new Exception("Id is required and it cannot be 0"); }
                return vcd;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public VehicleService GetVehicleServiceById(long Id)
        {
            try
            {
                VehicleService vehicleservice = null;
                if (Id >= 0)
                    vehicleservice = PSF.Get<VehicleService>(Id);

                else { throw new Exception("Id is required and it cannot be 0"); }
                return vehicleservice;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Vehicle Others Entry
        public Dictionary<long, IList<VehicleOthersEntry>> GetVehicleOthersEntryDetailsWithPagingLikeSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<VehicleOthersEntry>(page, pageSize, sortby, sortType, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateVehicleOthersEntry(VehicleOthersEntry VehicleOthersEntry)
        {
            try
            {
                if (VehicleOthersEntry != null)
                    PSF.SaveOrUpdate<VehicleOthersEntry>(VehicleOthersEntry);
                else { throw new Exception("Id is required and it cannot be null.."); }
                return VehicleOthersEntry.OthersId;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public VehicleOthersEntry GetVehicleOthersEntryByVehicleCostId(long VehicleCostId)
        {
            try
            {
                VehicleOthersEntry voe = null;
                if (VehicleCostId >= 0)
                    voe = PSF.Get<VehicleOthersEntry>("VehicleCostId", VehicleCostId);

                else { throw new Exception("Id is required and it cannot be 0"); }
                return voe;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public VehicleOthersEntry GetVehicleOthersEntryById(long Id)
        {
            try
            {
                VehicleOthersEntry vehicleothersentry = null;
                if (Id >= 0)
                    vehicleothersentry = PSF.Get<VehicleOthersEntry>(Id);

                else { throw new Exception("Id is required and it cannot be 0"); }
                return vehicleothersentry;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Vehicle FC Entry
        public Dictionary<long, IList<VehicleFCEntry>> GetVehicleFCEntryDetailsWithPagingLikeSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithLikeSearchCriteriaCount<VehicleFCEntry>(page, pageSize, sortby, sortType, criteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateVehicleFCEntry(VehicleFCEntry VehicleFCEntry)
        {
            try
            {
                if (VehicleFCEntry != null)
                    PSF.SaveOrUpdate<VehicleFCEntry>(VehicleFCEntry);
                else { throw new Exception("Id is required and it cannot be null.."); }
                return VehicleFCEntry.FCId;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public VehicleFCEntry GetVehicleFCEntryByVehicleCostId(long VehicleCostId)
        {
            try
            {
                VehicleFCEntry vcd = null;
                if (VehicleCostId >= 0)
                    vcd = PSF.Get<VehicleFCEntry>("VehicleCostId", VehicleCostId);

                else { throw new Exception("Id is required and it cannot be 0"); }
                return vcd;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public VehicleFCEntry GetVehicleFCEntryById(long Id)
        {
            try
            {
                VehicleFCEntry vehiclefcentry = null;
                if (Id >= 0)
                    vehiclefcentry = PSF.Get<VehicleFCEntry>(Id);

                else { throw new Exception("Id is required and it cannot be 0"); }
                return vehiclefcentry;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        public Dictionary<long, IList<VehicleOverviewReport_SP>> GetVehicleOverviewReportListbySP(string Campus, string VehicleNo, string VehicleType, DateTime? FromDate, DateTime? ToDate, int? page, int size, string sidx, string sord)
        {
            try
            {
                return PSF.ExecuteStoredProcedurewithOptionalParametersByDictonary<VehicleOverviewReport_SP>("GetVehicleOverviewReport_SPList",
                         new[] { new SqlParameter("Campus", Campus),
                             new SqlParameter("VehicleNo", VehicleNo),
                             new SqlParameter("VehicleType", VehicleType),
                             new SqlParameter("FromDate", FromDate),
                             new SqlParameter("ToDate", ToDate),
                             new SqlParameter("page", page),
                             new SqlParameter("size", size),
                             new SqlParameter("sidx",sidx),                                                       
                             new SqlParameter("sord",sord),   
                    });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region RouteMasterEdit
        public RouteMaster GetRouteDetails(string Campus, string RouteNo)
        {
            try
            {
                RouteMaster objRouteMaster = null;
                if (!string.IsNullOrEmpty(Campus))
                    objRouteMaster = PSF.Get<RouteMaster>("Campus", Campus, "RouteNo", RouteNo);

                else { throw new Exception("Id is required and it cannot be 0"); }
                return objRouteMaster;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void SaveOrUpdateRouteMasterLog(RouteMasterLog objRouteMasterLog)
        {
            if (objRouteMasterLog.RouteLogId > 0)
            {
                PSF.Update<RouteMasterLog>(objRouteMasterLog);
            }
            else PSF.Save<RouteMasterLog>(objRouteMasterLog);
        }
        public Dictionary<long, IList<RouteMasterWithIMEINumber_vw>> GetRouteMasterWithIMEINumber_vwList(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCountWithDecimal<RouteMasterWithIMEINumber_vw>(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
