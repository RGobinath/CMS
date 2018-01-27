using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.Component;
using TIPS.Entities.TransportEntities;
using TIPS.Entities;
using System.Data;
using TIPS.Entities.AdmissionEntities;

namespace TIPS.ServiceContract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TransportService" in code, svc and config file together.
    public class TransportService : ITransportServiceSc
    {
        TransportBC tbc = new TransportBC();
        public Dictionary<long, IList<VehicleTypeMaster>> GetVehicleTypeMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {

                return tbc.GetVehicleTypeMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<VehicleSubTypeMaster>> GetVehicleSubTypeMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleSubTypeMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleTypeAndSubTypeListWithsearchCriteriaLikeSearch(page, pageSize, sortType, sortBy, criteria, alias);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public long CreateOrUpdateVehicleTypeMaster(VehicleTypeMaster vtm)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdateVehicleTypeMaster(vtm);
                return vtm.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateVehicleSubTypeMaster(VehicleSubTypeMaster vstm)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdateVehicleSubTypeMaster(vstm);
                return vstm.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<VehicleDistanceCovered>> GetVehicleDistanceCoveredListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleDistanceCoveredListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdateVehicleDistanceCovered(vdc);
                return vdc.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public VehicleDistanceCovered GetVehicleDistanceCoveredById(int Id)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleDistanceCoveredById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }


        public Dictionary<long, IList<VehicleFuelManagement>> GetVehicleFuelManagementListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleFuelManagementListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdateVehicleFuelManagement(vfm);
                return vfm.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public VehicleFuelManagement GetVehicleFuelManagementById(int Id)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleFuelManagementById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public VehicleSubTypeMaster GetVehicleDetailsByVehicleNo(int VehicleId)
        {
            try
            {
                if (VehicleId > 0)
                {
                    TransportBC tbc = new TransportBC();
                    return tbc.GetVehicleDetailsByVehicleNo(VehicleId);
                }
                else throw new Exception("User Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<LocationMaster>> GetLocationMasterDetails(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetLocationMasterDetails(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VehicleSubTypeMaster GetVehicleSubTypeMasterById(int Id)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleSubTypeMasterById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<DriverMaster>> GetDriverMasterDetails(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetDriverMasterDetails(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateDriverMaster(DriverMaster dm)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdateDriverMaster(dm);
                return dm.Id;
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("Cannot insert duplicate key row in object 'dbo.QUEUE' with"))
                {
                    throw new FaultException("Cannot insert duplicate key in Queue Name.");
                }
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateDistanceCoveredDetails(DistanceCoveredDetails vtm)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdateDistanceCoveredDetails(vtm);
                return vtm.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public DistanceCoveredDetails GetDistanceCoveredDetailsById(int Id)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetDistanceCoveredDetailsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<DistanceCoveredDetails>> GetDistanceCoveredDetailsListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetDistanceCoveredDetailsListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public IList<VehicleDistanceCovered> CreateOrUpdateVehicleDistanceCoveredList(IList<VehicleDistanceCovered> DLst)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdateVehicleDistanceCoveredList(DLst);
                return DLst;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public VehicleDistanceCovered DeleteVehicleDistanceCoveredbyId(VehicleDistanceCovered sku)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.DeleteVehicleDistanceCoveredbyId(sku);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<RouteMaster>> GetRouteMasterDetails(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetRouteMasterDetails(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateVehicleMaintenance(VehicleMaintenance vtm)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdateVehicleMaintenance(vtm);
                return vtm.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateVehicleACMaintenance(VehicleACMaintenance vtm)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdateVehicleACMaintenance(vtm);
                return vtm.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<VehicleMaintenance>> GetVehicleMaintenanceListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleMaintenanceListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<VehicleACMaintenance>> GetVehicleACMaintenanceListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleACMaintenanceListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdateFuelRefillDetails(vtm);
                return vtm.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public FuelRefillDetails GetFuelRefillDetailsById(int Id)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetFuelRefillDetailsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<FuelRefillDetails>> GetFuelRefillDetailsListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetFuelRefillDetailsListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public IList<VehicleFuelManagement> CreateOrUpdateVehicleFuelManagementList(IList<VehicleFuelManagement> DLst)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdateVehicleFuelManagementList(DLst);
                return DLst;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public VehicleFuelManagement DeleteVehicleFuelManagementbyId(VehicleFuelManagement sku)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.DeleteVehicleFuelManagementById(sku);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }


        public Dictionary<long, IList<FitnessCertificate>> GetFitnessCertificateDetailsWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetFitnessCertificateDetailsWithPagingAndCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public long CreateOrUpdateFitnessCertificateDetails(FitnessCertificateDetails vtm)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdateFitnessCertificateDetails(vtm);
                return vtm.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public FitnessCertificateDetails GetFitnessCertificateDetailsById(int Id)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetFitnessCertificateDetailsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<FitnessCertificateDetails>> GetFitnessCertificateDetailsListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetFitnessCertificateDetailsListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public IList<FitnessCertificate> CreateOrUpdateFitnessCertificateList(IList<FitnessCertificate> DLst)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdateFitnessCertificateList(DLst);
                return DLst;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateInsuranceBulkDetails(InsuranceBulkDetails vtm)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdateInsuranceBulkDetails(vtm);
                return vtm.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public InsuranceBulkDetails GetInsuranceBulkDetailsById(int Id)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetInsuranceBulkDetailsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<InsuranceBulkDetails>> GetInsuranceBulkDetailsListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetInsuranceBulkDetailsListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public IList<Insurance> CreateOrUpdateInsuranceList(IList<Insurance> DLst)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdateInsuranceList(DLst);
                return DLst;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<Insurance>> GetInsuranceDetailsWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetInsuranceDetails(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Insurance DeleteInsurancebyId(Insurance sku)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.DeleteInsuranceById(sku);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Insurance GetInsuranceDetailsById(long Id)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetInsuranceDetailsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }


        public long CreateOrUpdateFinesAndPenalitiesBulkDetails(FinesAndPenalitiesBulkDetails vtm)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdateFinesAndPenalitiesBulkDetails(vtm);
                return vtm.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public FinesAndPenalitiesBulkDetails GetFinesAndPenalitiesBulkDetailsById(int Id)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetFinesAndPenalitiesBulkDetailsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<FinesAndPenalitiesBulkDetails>> GetFinesAndPenalitiesBulkDetailsListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetFinesAndPenalitiesBulkDetailsListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public IList<FinesAndPenalities> CreateOrUpdateFinesAndPenalitiesList(IList<FinesAndPenalities> DLst)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdateFinesAndPenalitiesList(DLst);
                return DLst;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<FinesAndPenalities>> GetFinesAndPenalitiesIdViewListWithCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetFinesAndPenalitiesIdViewListWithCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public FinesAndPenalities GetFinesAndPenalitiesById(long Id)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetFinesAndPenalitiesById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public FinesAndPenalities DeleteFinesAndPenalitiesbyId(FinesAndPenalities sku)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.DeleteFinesAndPenalitiesById(sku);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        #region Vehicle Fuel and Distance covered Report added by micheal

        public Dictionary<long, IList<VehicleDistanceCoveredReport_vw>> GetVehicleDistanceReportListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleDistanceCoveredReportListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<VehicleFuelReport_vw>> GetVehicleFuelListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleFuelReportListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<VehicleDistanceCoveredChart_vw>> GetVehicleDistancechartListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleDistanceCoveredchartListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<VehicleFuelQuantityChart_vw>> GetVehicleFuelchartListWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleFuelchartListWithCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public Dictionary<long, IList<VehicleDistanceFuelReport_vw>> GetVehicleDistanceFuelReport_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleDistanceFuelReport_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdatePermitDetails(per);
                return per.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<Permit>> GetPermitDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetPermitDetailsListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdateDriverOTDetails(per);
                return per.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<DriverOTDetails>> GetDriverOTDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetDriverOTDetailsListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
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
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdateVehicleElectricalMaintenance(vem);
                return vem.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<VehicleElectricalMaintenance>> GetVehicleElectricalMaintenanceListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleElectricalMaintenanceListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public int? CreateOrUpdateVehicleBodyMaintenance(VehicleBodyMaintenance vem)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdateVehicleBodyMaintenance(vem);
                return vem.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<VehicleBodyMaintenance>> GetVehicleBodyMaintenanceListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleBodyMaintenanceListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public int? CreateOrUpdateVehicleTyreMaintenance(VehicleTyreMaintenance vtm)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdateVehicleTyreMaintenance(vtm);
                return vtm.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<VehicleTyreMaintenance>> GetVehicleTyreMaintenanceListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleTyreMaintenanceListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public IList<VehicleDistanceCovered> DeleteVehicleDistanceCoveredList(IList<VehicleDistanceCovered> DLst)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.DeleteVehicleDistanceCoveredList(DLst);
                return DLst;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        #region Newly added by Micheal
        public Dictionary<long, IList<VehicleTypeAndSubType>> GetVehicleTypeAndSubTypeListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleTypeAndSubTypeListWithPaging(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        #endregion

        #region DriverOTAllowance
        public Dictionary<long, IList<DriverOTAllowance_vw>> GetDriverOTAllowanceListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetDriverOTAllowanceListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public DataTable GetDriverAllowance(string query)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetDriverAllowance(query);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<VehicleSubTypeMaster>> GetVehicleSubTypeMasterListWithPagingAndCriteriaLikeSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleSubTypeMasterListWithPagingAndCriteriaLikeSearch(page, pageSize, sortby, sortType, criteria);
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
                TransportBC tbc = new TransportBC();
                return tbc.GetLocationMasterDetailsWithPagingLikeSearch(page, pageSize, sortby, sortType, criteria);
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
                TransportBC tbc = new TransportBC();
                return tbc.GetTripPurposeMasterDetailsWithPagingLikeSearch(page, pageSize, sortby, sortType, criteria);
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
                if (!string.IsNullOrWhiteSpace(Purpose))
                {
                    TransportBC tbc = new TransportBC();
                    return tbc.GetPurposeByPurpose(Purpose);
                }
                else throw new Exception("User Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public int CreateOrUpdateTripPurposeMaster(TripPurposeMaster tpm)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdateTripPurposeMaster(tpm);
                return tpm.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        #region For Tool bar search By Gobi
        public Dictionary<long, IList<VehicleMaintenance>> VehicleMaintenanceListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                TransportBC TBC = new TransportBC();
                return TBC.VehicleMaintenanceListWithLikeAndExcactSerachCriteria(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<VehicleACMaintenance>> VehicleACMaintenanceListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                TransportBC TBC = new TransportBC();
                return TBC.VehicleACMaintenanceListWithLikeAndExcactSerachCriteria(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<VehicleElectricalMaintenance>> VehicleElectricalMaintenanceListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                TransportBC TBC = new TransportBC();
                return TBC.VehicleElectricalMaintenanceListWithLikeAndExcactSerachCriteria(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<VehicleBodyMaintenance>> VehicleBodyMaintenanceListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                TransportBC TBC = new TransportBC();
                return TBC.VehicleBodyMaintenanceListWithLikeAndExcactSerachCriteria(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<VehicleTyreMaintenance>> VehicleTyreMaintenanceListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                TransportBC TBC = new TransportBC();
                return TBC.VehicleTyreMaintenanceListWithLikeAndExcactSerachCriteria(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<VehicleDistanceCovered>> VehicleDistanceCoveredListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                TransportBC TBC = new TransportBC();
                return TBC.VehicleDistanceCoveredListWithLikeAndExcactSerachCriteria(page, pageSize, sortby, sortType, exctcriteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<VehicleFuelManagement>> VehicleFuelManagementListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                TransportBC TBC = new TransportBC();
                return TBC.VehicleFuelManagementListWithLikeAndExcactSerachCriteria(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<FinesAndPenalities>> FinesAndPenalitiesListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                TransportBC TBC = new TransportBC();
                return TBC.FinesAndPenalitiesListWithLikeAndExcactSerachCriteria(page, pageSize, sortby, sortType, exctcriteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<FitnessCertificate>> FitnessCertificateListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                TransportBC TBC = new TransportBC();
                return TBC.FitnessCertificateListWithLikeAndExcactSerachCriteria(page, pageSize, sortby, sortType, exctcriteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<Insurance>> InsuranceListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                TransportBC TBC = new TransportBC();
                return TBC.InsuranceListWithLikeAndExcactSerachCriteria(page, pageSize, sortby, sortType, exctcriteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<Permit>> PermitListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                TransportBC TBC = new TransportBC();
                return TBC.PermitListWithLikeAndExcactSerachCriteria(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion

        public Dictionary<long, IList<VehicleReport>> GetVehicleReportListWithsearchCriteriaLikeSearch(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleReportListWithsearchCriteriaLikeSearch(page, pageSize, sortType, sortBy, criteria, alias);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        #region "Tyre Management"

        public int CreateOrUpdateTyreInvoiceDetails(TyreInvoiceDetails tid)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdateTyreInvoiceDetails(tid);
                return tid.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<TyreInvoiceDetails>> GetTyreInvoiceDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetTyreInvoiceDetailsListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                TransportBC tbc = new TransportBC();
                return tbc.GetTyreInvoiceDetailsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public int CreateOrUpdateTyreDetails(TyreDetails td)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdateTyreDetails(td);
                return td.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<TyreDetails>> GetTyreDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetTyreDetailsListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                if (!string.IsNullOrWhiteSpace(TyreNo))
                {
                    TransportBC tbc = new TransportBC();
                    return tbc.GetTyreDetailsByTyreNo(TyreNo);
                }
                else throw new Exception("TyreNo is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<TyreDetailsAndInvoiceDetails>> GetTyreDetailsAndInvoiceDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetTyreDetailsAndInvoiceDetailsListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CreateOrUpdateTyreTyreAssignedList(TyreAssignedList tid)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdateTyreTyreAssignedList(tid);
                return tid.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<TyreAssignedList>> GetTyreAssignedListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetTyreAssignedListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<TyreDetailsAndInvoiceDetails>> GetTyreDetailsAndInvoiceDetailsListWithAliasPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, string[] criteriaAlias)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetTyreDetailsAndInvoiceDetailsListWithAliasPagingAndCriteria(page, pageSize, sortby, sortType, criteria, criteriaAlias);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<TransportVendorMaster>> GetTransportVendorMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetTransportVendorMasterListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
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
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdateTransportVendorMaster(tvm);
                return tvm.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #endregion "Tyre Management"

        #region Added By Gobi
        public LocationMaster GetLocationMasterDetailsById(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    TransportBC TBC = new TransportBC();
                    return TBC.GetLocationMasterDetailsById(Id);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public long SaveOrUpdateLocationMasterDetails(LocationMaster Loc)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.SaveOrUpdateLocationMasterDetails(Loc);
                return Loc.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public RouteMaster GetRouteMasterDetailsById(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    TransportBC TBC = new TransportBC();
                    return TBC.GetRouteMasterDetailsById(Id);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public long SaveOrUpdateRouteMasterDetails(RouteMaster Route)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.SaveOrUpdateRouteMasterDetails(Route);
                return Route.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public DriverMaster GetDriverMasterDetailsById(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    TransportBC TBC = new TransportBC();
                    return TBC.GetDriverMasterDetailsById(Id);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public TransportVendorMaster GetTransportVendorMasterDetailsById(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    TransportBC TBC = new TransportBC();
                    return TBC.GetTransportVendorMasterDetailsById(Id);
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

        #region Student Route Configuration
        public long CreateOrUpdateRouteConfiguration(RouteConfiguration RouteConfig)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdateRouteConfiguration(RouteConfig);
                return RouteConfig.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool DeleteRouteConfiguration(long id)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.DeleteRouteConfiguration(id);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public bool DeleteRouteConfiguration(long[] id)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.DeleteRouteConfiguration(id);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<RouteConfiguration>> GetRouteConfigurationListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetRouteConfigurationListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public RouteConfiguration GetRouteConfigurationByLocationName(long RouteMasterId, string LocationName, string LocationOtherDetails)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetRouteConfigurationByLocationName(RouteMasterId, LocationName, LocationOtherDetails);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public RouteConfiguration GetRouteConfigurationByStopOrderNumber(long RouteMasterId, long StopOrderNumber)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetRouteConfigurationByStopOrderNumber(RouteMasterId, StopOrderNumber);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<StudentTemplate>> GetStudentListListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetStudentListListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<RouteMaster>> GetRouteMstrListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetRouteMstrListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<RouteConfiguration>> GetRouteConfigListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetRouteConfigListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #region RouteStudent Configuration  Added by Thamizh
        public Dictionary<long, IList<RouteConfiguration>> GetLocationListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetLocationListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public Dictionary<long, IList<RouteMasterConfig_vw>> GetRouteMasterConfig_vwListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetRouteMasterConfig_vwListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<RouteStudentConfigurationPDF_vw>> GetRouteStudConfigPDFListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetRouteStudConfigPDFListWithsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #endregion
        #region RouteStudentConfigurationList
        public long SaveOrUpdateRouteStudConfigDetails(RouteStudConfig Route)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.SaveOrUpdateRouteStudConfigDetails(Route);
                return Route.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public RouteConfiguration GetLocationNameById(long Id)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetLocationNameById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public IList<StudentTemplateView> GetStudentNameListByQuery(string LocationName, string Campus)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetStudentNameListByQuery(LocationName, Campus);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region RouteStudentListConfiguration
        public Dictionary<long, IList<RouteStudentListConfigView>> GetRouteStudentListConfigWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetRouteStudentListConfigWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                TransportBC tbc = new TransportBC();
                RouteMasterConfig_vw RouteMasterConfig_vw = new RouteMasterConfig_vw();
                RouteMasterConfig_vw = tbc.GetRouteMasterConfig_vwByRouteStudCode(RouteStudCode);
                return RouteMasterConfig_vw;

            }
            catch (Exception)
            {
                throw;
            }
        }
        #region Route Configuration Report
        public Dictionary<long, IList<StudentsRouteConfigReport_vw>> GetStudentsRouteConfigReport_vwListWithsearchCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetStudentsRouteConfigReport_vwListWithsearchCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public long CreateOrUpdateStudentLocationNameMaster(StudentLocationMaster slm)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.CreateOrUpdateStudentLocationNameMaster(slm);
                return slm.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }


        public Dictionary<long, IList<StudentLocationMaster>> GetStudentLocationMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetStudentLocationMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                TransportBC TransportBC = new TransportBC();
                return TransportBC.GetStudentLocationMasterByLocationName(Campus, LocationName);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        #region ConfiguredStudentList Form
        public StudentTemplateView GetStudentDetailsByPreRegNo(long PreRegNo)
        {
            try
            {
                TransportBC TransportBC = new TransportBC();
                return TransportBC.GetStudentDetailsByPreRegNo(PreRegNo);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion

        #region DriverAttendance
        public long CreateOrUpdateAttendanceList(DriverAttendance att)
        {
            try
            {
                TransportBC DriverAttendanceBC = new TransportBC();
                DriverAttendanceBC.CreateOrUpdateDriverAttendanceList(att);
                return att.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public DriverAttendance GetDriverAttentanceById(long Id)
        {
            try
            {
                TransportBC DriverAttendanceBC = new TransportBC();
                return DriverAttendanceBC.GetDriverAttentanceById(Id);

            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long DeleteDriverAttendancevalue(DriverAttendance att)
        {
            try
            {
                TransportBC DriverAttendanceBC = new TransportBC();
                DriverAttendanceBC.DeleteDriverAttendancevalue(att);
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<DriverAttendance>> GetDriverAttendanceDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetDriverAttendanceDetailsListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
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
                try
                {
                    TransportBC Driver = new TransportBC();
                    return Driver.GetDriverListListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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

        public Dictionary<long, IList<DriverAttendance>> GetDriverMasterForAnAttendanceListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    TransportBC transportBC = new TransportBC();
                    return transportBC.GetDriverMasterForAnAttendanceListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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

        public Dictionary<long, IList<DriverAttendance>> GetAbsentListForAnAttendanceListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    TransportBC transportBC = new TransportBC();
                    return transportBC.GetAbsentListForAnAttendanceListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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


        public Dictionary<long, IList<DriverAttendanceMonthReport>> GetDriverAttendanceMonthReportListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetDriverAttendanceMonthReportListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
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
                try
                {
                    TransportBC transportBC = new TransportBC();
                    return transportBC.GetMonthlyAbsentListForAnAttendanceListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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

        #region Driver ID card

        public Dictionary<long, IList<DriverMaster>> GetDriverListWithEQsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return tbc.GetDriverListWithEQsearchCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        #endregion

        public DriverMaster GetDriverDetailsByDriverRegNo(long RegNo)
        {
            try
            {
                if (RegNo > 0)
                {
                    return tbc.GetDriverDetailsByRegNo(RegNo);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<DriverRegNumDetails>> GetDriverRegNumDetailsListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return tbc.GetDriverRegNumDetailsListWithPaging(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateDriverRegNumDetails(DriverRegNumDetails srn)
        {
            try
            {
                tbc.CreateOrUpdateDriverRegNumDetails(srn);
                return srn.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        #region Driver family details
        public long CreateOrUpdateDriverFamilyDetails(DriverFamilyDetails fd)
        {
            try
            {
                tbc.CreateOrUpdateDriverFamilyDatails(fd);
                return fd.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<DriverFamilyDetails>> GetDriverFamilyDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return tbc.GetDriverFamilyDetailsListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
                // return AdmissionManagementBC.get.GetFamilyDetailsListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public DriverFamilyDetails GetDriverFamilyDetailsById(long Id)
        {
            try
            {
                return tbc.GetDriverFamilyDetailsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public bool DeleteDriverFamilyDetails(long id)
        {
            try
            {

                tbc.DeleteDriverFamilyDetails(id);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public bool DeleteDriverFamilyDetails(long[] id)
        {
            try
            {

                tbc.DeleteDriverFamilyDetails(id);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }
        #endregion

        //VehicleElectricalMaintenance_Vw

        #region VehicleElectricalMaintenance_Vw
        public Dictionary<long, IList<VehicleElectricalMaintenance_Vw>> GetVehicleElectricalMaintenance_VwWithPagingLikeSearch(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return tbc.GetVehicleElectricalMaintenance_VwWithPagingLikeSearch(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion

        //VehicleMaintenance_Vw
        #region VehicleMaintenance_Vw
        public Dictionary<long, IList<VehicleMaintance_Vw>> GetVehicleMaintenance_VwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleMaintenance_VwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                TransportBC TBC = new TransportBC();
                return TBC.GetVehicleTyreMaintenance_VwWithPagingLikeSearch(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion

        //VehicleACMaintenanceReport
        #region VehicleACMaintenanceReport
        public Dictionary<long, IList<VehicleAcMaintanceReport_Vw>> GetVehicleACMaintenanceReport_VwListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleACMaintenanceReport_VwListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
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
                TransportBC TBC = new TransportBC();
                return TBC.GetVehicleBodyMaintainance_VwWithPagingLikeSearch(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion

        //DistanceCoverd_Vw
        #region DistanceCoverd_Vw
        public Dictionary<long, IList<DistanceCovered_Vw>> GetDistanceCovered_VwWithPagingLikeSearch(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return tbc.GetDistanceCovered_VwWithPagingLikeSearch(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion

        ///TogetFuelTypeDetails
        public Dictionary<long, IList<FuelRefilDetails_Vw>> GetFuelTypeDetails(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetFuelTypeDetails(page, pageSize, sortby, sortType, criteria);
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
                TransportBC tbc = new TransportBC();
                return tbc.GetDriverOTReportDetailsWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataTable FuelReportDetailsNew(string query)
        {
            try
            {
                return tbc.FuelReportDetails(query);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public DriverMaster GetDriverDetailsUsingID(long Id)
        {

            try
            {
                TransportBC TransBC = new TransportBC();
                return TransBC.GetDriverDetailsUsingID(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<VehicleDistanceCovered_vw>> GetVehicleDistanceCovered_vw(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleDistanceCovered_vw(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region Added by john naveen
        public long SaveOrUpdateTripMasterDetails(TripMaster trpm)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.SaveOrUpdateTripMasterDetails(trpm);
                return trpm.TripId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool DeleteTripMasterDetails(long[] Ids)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.DeleteTripMasterDetails(Ids);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<TripMaster>> GetTripMasterDetails(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetTripMasterDetails(page, pageSize, sortType, sortby, criteria);
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
                if (TripId > 0)
                {
                    TransportBC TBC = new TransportBC();
                    return TBC.GetTripMasterDetailsById(TripId);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public TripMaster GetTripMasterDetailsByTripName(string TripName)
        {
            try
            {
                if (!string.IsNullOrEmpty(TripName))
                {
                    TransportBC TBC = new TransportBC();
                    return TBC.GetTripMasterDetailsByTripName(TripName);
                }
                else throw new Exception("TripName is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #endregion
        #region Vehicle Cost Details by naveen

        public long SaveOrUpdateVehicleCostDetails(VehicleCostDetails vcdl)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.SaveOrUpdateVehicleCostDetails(vcdl);
                return vcdl.VehicleCostId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool DeleteVehicleCostDetails(long[] Ids)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.DeleteVehicleCostDetails(Ids);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<VehicleCostDetails>> VehicleCostDetailsListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                TransportBC TBC = new TransportBC();
                return TBC.VehicleCostDetailsListWithLikeAndExcactSerachCriteria(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public VehicleCostDetails GetVehicleCostDetailsByTripName(long VehicleId, string TypeOfTrip, DateTime VehicleTravelDate)
        {
            try
            {
                TransportBC TransportBC = new TransportBC();
                return TransportBC.GetVehicleCostDetailsByTripName(VehicleId, TypeOfTrip, VehicleTravelDate);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public VehicleCostDetails GetVehicleCostDetailsById(long VehicleCostId)
        {
            try
            {
                if (VehicleCostId >= 0)
                {
                    TransportBC tbc = new TransportBC();
                    return tbc.GetVehicleCostDetailsById(VehicleCostId);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<VehicleCostDetails_VW>> GetVehicleCostDetails_VWListWithPagingAndCriteriaLikeSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleCostDetails_VWListWithPagingAndCriteriaLikeSearch(page, pageSize, sortType, sortby, criteria);
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
                TransportBC TransportBC = new TransportBC();
                return TransportBC.GetVehicleCostDetailsByTravelDate(VehicleId, VehicleTravelDate);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<VehicleCostDetailsReport_sp>> GetVehicleCostDetailsReportSP(string AcademicYear, string Status, DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                return tbc.GetVehicleCostDetailsReportSP(AcademicYear, Status, FromDate, ToDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region by John naveen
        public Dictionary<long, IList<RouteMaster>> GetRouteMasterDetailsByListWithLikeSearchCriteriaCount(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetRouteMasterDetailsListWithLikeSearchCriteriaCount(page, pageSize, sortby, sortType, criteria);
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
                TransportBC tbc = new TransportBC();
                tbc.SaveOrUpdateGPSTrackingDeviceMaster(TrackingDeviceMasterObj);
                return TrackingDeviceMasterObj.GPS_TrackingDeviceMaster_Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool DeleteGPSTrackingDeviceMaster(long[] Ids)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.DeleteGPSTrackingDeviceMaster(Ids);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<GPS_TrackingDeviceMaster>> GPSTrackingDeviceMasterListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                TransportBC TBC = new TransportBC();
                return TBC.GPSTrackingDeviceMasterListWithLikeAndExcactSerachCriteria(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public GPS_TrackingDeviceMaster GetGPS_TrackingDeviceMasterByName(string Campus, string BrandName, string ModelName, string IMEINmber)
        {
            try
            {
                TransportBC TransportBC = new TransportBC();
                return TransportBC.GetGPS_TrackingDeviceMasterByName(Campus, BrandName, ModelName, IMEINmber);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        #region purpose Master
        public long SaveOrUpdatePurposeMaster(PurposeMaster pm)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.SaveOrUpdatePurposeMasterMaster(pm);
                return pm.Purpose_Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool DeletePurposeMaster(long[] Ids)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.DeletePurposeMaster(Ids);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<PurposeMaster>> GetPurposeMasterDetailsByListWithLikeSearchCriteriaCount(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetPurposeMasterDetailsListWithLikeSearchCriteriaCount(page, pageSize, sortby, sortType, criteria);
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
                if (!string.IsNullOrEmpty(PurposeName))
                {
                    TransportBC TBC = new TransportBC();
                    return TBC.GetPurposeMasterDetailsByPurposeName(PurposeName);
                }
                else throw new Exception("PurPoseName is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #endregion
        #region ForNewVehicleCostDetails

        public DailyUsageVehicleMaster GetDailyUsageVehicleMasterById(int Id)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetDailyUsageVehicleMasterById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<VehicleCostDetails_Updated>> VehicleCostDetails_UpdatedListWithLikeAndExcactSerachCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                TransportBC TBC = new TransportBC();
                return TBC.VehicleCostDetails_UpdatedListWithLikeAndExcactSerachCriteria(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }


        public VehicleCostDetails_Updated GetVehicleCostDetails_UpdatedByTravelDate(long VehicleId, DateTime VehicleTravelDate)
        {
            try
            {
                TransportBC TransportBC = new TransportBC();
                return TransportBC.GetVehicleCostDetails_UpdatedByTravelDate(VehicleId, VehicleTravelDate);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public VehicleCostDetails_Updated GetVehicleCostDetails_UpdatedByTripName(long VehicleId, string TypeOfTrip, DateTime VehicleTravelDate)
        {
            try
            {
                TransportBC TransportBC = new TransportBC();
                return TransportBC.GetVehicleCostDetails_UpdatedByTripName(VehicleId, TypeOfTrip, VehicleTravelDate);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long SaveOrUpdateVehicleCostDetails_Updated(VehicleCostDetails_Updated vcdl)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.SaveOrUpdateVehicleCostDetails_Updated(vcdl);
                return vcdl.VehicleCostId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<NewVehicleTypeMaster>> GetNewVehicleTypeMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetNewVehicleTypeMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region DailyUsageVehicleMasterList
        public Dictionary<long, IList<DailyUsageVehicleMaster>> GetDailyUsageVehicleMasterListWithsearchCriteriaLikeSearch(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetDailyUsageVehicleMasterListWithsearchCriteriaLikeSearch(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }       
        #endregion       
        #region Daiy usage master View
        public Dictionary<long, IList<DailyUsageVehicleMaster_vw>> GetDailyUsageVehicleMaster_vwListWithsearchCriteriaLikeSearch(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetDailyUsageVehicleMaster_vwListWithsearchCriteriaLikeSearch(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #endregion
        public Dictionary<long, IList<VehicleCostDetails_Updated_VW>> GetVehicleCostDetails_Updated_VWListWithPagingAndCriteriaLikeSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleCostDetails_Updated_VWListWithPagingAndCriteriaLikeSearch(page, pageSize, sortType, sortby, criteria);
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
                if (VehicleCostId >= 0)
                {
                    TransportBC tbc = new TransportBC();
                    return tbc.GetVehicleCostDetails_UpdatedById(VehicleCostId);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #region Vehicle Fuel
        public Dictionary<long, IList<VehicleFuelRefillEntry>> GetVehicleFuelRefillEntryDetailsWithPagingLikeSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleFuelRefillEntryDetailsWithPagingLikeSearch(page, pageSize, sortby, sortType, criteria);
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
                TransportBC tbc = new TransportBC();
                tbc.SaveOrUpdateVehicleFuelRefillEntry(VehicleFuelRefillEntry);
                return VehicleFuelRefillEntry.FuelRefillId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public VehicleFuelRefillEntry GetVehicleFuelRefillEntryById(long Id)
        {
            try
            {
                if (Id >= 0)
                {
                    TransportBC tbc = new TransportBC();
                    return tbc.GetVehicleFuelRefillEntryById(Id);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public VehicleFuelRefillEntry GetVehicleFuelRefillEntryByVehicleCostId(long VehicleCostId)
        {
            try
            {
                if (VehicleCostId >= 0)
                {
                    TransportBC tbc = new TransportBC();
                    return tbc.GetVehicleFuelRefillEntryByVehicleCostId(VehicleCostId);
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
        #region supplier master for transport
        public Dictionary<long, IList<TransportSupplierMaster>> GetTransportSupplierMasterWithPagingLikeSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetTransportSupplierMasterWithPagingLikeSearch(page, pageSize, sortby, sortType, criteria);
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
                TransportBC tbc = new TransportBC();
                tbc.SaveOrUpdateTransportSupplierMaster(TransportSupplierMaster);
                return TransportSupplierMaster.SupplierId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public bool DeleteTransportSupplierMaster(long[] Ids)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.DeleteTransportSupplierMaster(Ids);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public TransportSupplierMaster GetTransportSupplierMasterBySupplierName(string SupplierName, string SupplierType)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(SupplierName))
                {
                    TransportBC tbc = new TransportBC();
                    return tbc.GetTransportSupplierMasterBySupplierName(SupplierName, SupplierType);
                }
                else throw new Exception("The Name is nulll or empty");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }

        }
        #endregion
        
        #region Vehicle Maintenance Entry
        public Dictionary<long, IList<VehicleMaintenanceEntry>> GetVehicleMaintenanceEntryDetailsWithPagingLikeSearch(int? page, int? pageSize, string sordby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleMaintenanceEntryDetailsWithPagingLikeSearch(page, pageSize, sordby, sortType, criteria);
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
                TransportBC tbc = new TransportBC();
                tbc.SaveOrUpdateVehicleMaintenanceEntry(VehicleMaintenanceEntry);
                return VehicleMaintenanceEntry.MaintenanceId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public VehicleMaintenanceEntry GetVehicleMaintenanceEntryById(long Id)
        {
            try
            {
                if (Id >= 0)
                {
                    TransportBC tbc = new TransportBC();
                    return tbc.GetVehicleMaintenanceEntryById(Id);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public VehicleMaintenanceEntry GetVehicleMaintenanceEntryByVehicleCostId(long VehicleCostId)
        {
            try
            {
                if (VehicleCostId >= 0)
                {
                    TransportBC tbc = new TransportBC();
                    return tbc.GetVehicleMaintenanceEntryByVehicleCostId(VehicleCostId);
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
        #region Vehicle Service Entry
        public Dictionary<long, IList<VehicleService>> GetVehicleServiceDetailsWithPagingLikeSearch(int? page, int? pageSize, string sordby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleServiceDetailsWithPagingLikeSearch(page, pageSize, sordby, sortType, criteria);
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
                TransportBC tbc = new TransportBC();
                tbc.SaveOrUpdateVehicleService(VehicleService);
                return VehicleService.ServiceId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public VehicleService GetVehicleServiceByVehicleCostId(long VehicleCostId)
        {
            try
            {
                if (VehicleCostId >= 0)
                {
                    TransportBC tbc = new TransportBC();
                    return tbc.GetVehicleServiceByVehicleCostId(VehicleCostId);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public VehicleService GetVehicleServiceById(long Id)
        {
            try
            {
                if (Id >= 0)
                {
                    TransportBC tbc = new TransportBC();
                    return tbc.GetVehicleServiceById(Id);
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
        #region Vehicle Others Entry
        public Dictionary<long, IList<VehicleOthersEntry>> GetVehicleOthersEntryDetailsWithPagingLikeSearch(int? page, int? pageSize, string sordby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleOthersEntryDetailsWithPagingLikeSearch(page, pageSize, sordby, sortType, criteria);
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
                TransportBC tbc = new TransportBC();
                tbc.SaveOrUpdateVehicleOthersEntry(VehicleOthersEntry);
                return VehicleOthersEntry.OthersId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public VehicleOthersEntry GetVehicleOthersEntryByVehicleCostId(long VehicleCostId)
        {
            try
            {
                if (VehicleCostId >= 0)
                {
                    TransportBC tbc = new TransportBC();
                    return tbc.GetVehicleOthersEntryByVehicleCostId(VehicleCostId);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public VehicleOthersEntry GetVehicleOthersEntryById(long Id)
        {
            try
            {
                if (Id >= 0)
                {
                    TransportBC tbc = new TransportBC();
                    return tbc.GetVehicleOthersEntryById(Id);
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
        #region Vehicle FC Entry
        public Dictionary<long, IList<VehicleFCEntry>> GetVehicleFCEntryDetailsWithPagingLikeSearch(int? page, int? pageSize, string sordby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                return tbc.GetVehicleFCEntryDetailsWithPagingLikeSearch(page, pageSize, sordby, sortType, criteria);
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
                TransportBC tbc = new TransportBC();
                tbc.SaveOrUpdateVehicleFCEntry(VehicleFCEntry);
                return VehicleFCEntry.FCId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public VehicleFCEntry GetVehicleFCEntryByVehicleCostId(long VehicleCostId)
        {
            try
            {
                if (VehicleCostId >= 0)
                {
                    TransportBC tbc = new TransportBC();
                    return tbc.GetVehicleFCEntryByVehicleCostId(VehicleCostId);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public VehicleFCEntry GetVehicleFCEntryById(long Id)
        {
            try
            {
                if (Id >= 0)
                {
                    TransportBC tbc = new TransportBC();
                    return tbc.GetVehicleFCEntryById(Id);
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
        public Dictionary<long, IList<VehicleOverviewReport_SP>> GetVehicleOverviewReportListbySP(string Campus, string VehicleNo, string VehicleType, DateTime? FromDate, DateTime? ToDate, int? page, int size, string sidx, string sord)
        {
            try
            {
                return tbc.GetVehicleOverviewReportListbySP(Campus, VehicleNo, VehicleType, FromDate, ToDate, page, size, sidx, sord);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region RouteMasterEdit
        public RouteMaster GetRouteDetails(string Campus, string Route)
        {
            try
            {
                if (!string.IsNullOrEmpty(Campus))
                {
                    return tbc.GetRouteDetails(Campus, Route);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public long SaveOrUpdateRouteMasterLog(RouteMasterLog RouteMasterLog)
        {
            try
            {
                TransportBC tbc = new TransportBC();
                tbc.SaveOrUpdateRouteMasterLog(RouteMasterLog);
                return RouteMasterLog.RouteLogId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<RouteMasterWithIMEINumber_vw>> GetRouteMasterWithIMEINumber_vwList(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> exctcriteria, Dictionary<string, object> likeCriteria)
        {
            try
            {
                TransportBC TBC = new TransportBC();
                return TBC.GetRouteMasterWithIMEINumber_vwList(page, pageSize, sortType, sortby, exctcriteria, likeCriteria);
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
