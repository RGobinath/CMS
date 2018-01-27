using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersistenceFactory;
using TIPS.Entities;
using TIPS.Entities.StoreEntities;
using TIPS.Entities.MenuEntities;
using TIPS.Entities.Assess;
using TIPS.Entities.TransportEntities;
using TIPS.Entities.StudentsReportEntities;
using TIPS.Entities.ReportEntities;

namespace TIPS.Component
{
    public class MastersBC
    {
        PersistenceServiceFactory PSF = null;
        public MastersBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }

        public long CreateOrUpdateRegionMaster(RegionMaster rm)
        {
            try
            {
                if (rm != null)
                    PSF.SaveOrUpdate<RegionMaster>(rm);
                else { throw new Exception("CallManagement is required and it cannot be null.."); }
                return rm.FormId;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<RegionMaster>> GetRegionMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<RegionMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteRegionMaster(long id)
        {
            try
            {
                RegionMaster regionmaster = PSF.Get<RegionMaster>(id);
                PSF.Delete<RegionMaster>(regionmaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteRegionMaster(long[] id)
        {
            try
            {
                IList<RegionMaster> regionmaster = PSF.GetListByIds<RegionMaster>(id);
                PSF.DeleteAll<RegionMaster>(regionmaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateCountryMaster(CountryMaster cm)
        {
            try
            {
                if (cm != null)
                    PSF.SaveOrUpdate<CountryMaster>(cm);
                else { throw new Exception("CallManagement is required and it cannot be null.."); }
                return cm.FormId;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<CountryMaster>> GetCountryMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<CountryMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteCountryMaster(long id)
        {
            try
            {
                CountryMaster countrymaster = PSF.Get<CountryMaster>(id);
                PSF.Delete<CountryMaster>(countrymaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteCountryMaster(long[] id)
        {
            try
            {
                IList<CountryMaster> countrymaster = PSF.GetListByIds<CountryMaster>(id);
                PSF.DeleteAll<CountryMaster>(countrymaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public long CreateOrUpdateGradeMaster(GradeMaster gm)
        {
            try
            {
                if (gm != null)
                    PSF.SaveOrUpdate<GradeMaster>(gm);
                else { throw new Exception("CallManagement is required and it cannot be null.."); }
                return gm.FormId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<GradeMaster>> GetGradeMasterListWithPagingAndCriteriaWithIn(int? page, int? pageSize, string sortby, string sortType, string name, int[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                return PSF.GetListWithInSearchCriteriaCountArray<GradeMaster>(page, pageSize, sortby, sortType, name, values, criteria, alias);                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<GradeMaster>> GetGradeMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<GradeMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteGradeMaster(long id)
        {
            try
            {
                GradeMaster grademaster = PSF.Get<GradeMaster>(id);
                PSF.Delete<GradeMaster>(grademaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteGradeMaster(long[] id)
        {
            try
            {
                IList<GradeMaster> grademaster = PSF.GetListByIds<GradeMaster>(id);
                PSF.DeleteAll<GradeMaster>(grademaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateSectionMaster(SectionMaster sm)
        {
            try
            {
                if (sm != null)
                    PSF.SaveOrUpdate<SectionMaster>(sm);
                else { throw new Exception("CallManagement is required and it cannot be null.."); }
                return sm.FormId;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool DeleteSectionMaster(long id)
        {
            try
            {
                SectionMaster sectionmaster = PSF.Get<SectionMaster>(id);
                PSF.Delete<SectionMaster>(sectionmaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteSectionMaster(long[] id)
        {
            try
            {
                IList<SectionMaster> sectionmaster = PSF.GetListByIds<SectionMaster>(id);
                PSF.DeleteAll<SectionMaster>(sectionmaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<SectionMaster>> GetSectionMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<SectionMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateFeeStructureYearMaster(FeeStructureYearMaster fm)
        {
            try
            {
                if (fm != null)
                    PSF.SaveOrUpdate<FeeStructureYearMaster>(fm);
                else { throw new Exception("CallManagement is required and it cannot be null.."); }
                return fm.FormId;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool DeleteFeeStructureYearMaster(long id)
        {
            try
            {
                FeeStructureYearMaster feestructyrmaster = PSF.Get<FeeStructureYearMaster>(id);
                PSF.Delete<FeeStructureYearMaster>(feestructyrmaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteFeeStructureYearMaster(long[] id)
        {
            try
            {
                IList<FeeStructureYearMaster> feestructyrmaster = PSF.GetListByIds<FeeStructureYearMaster>(id);
                PSF.DeleteAll<FeeStructureYearMaster>(feestructyrmaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<FeeStructureYearMaster>> GetFeeStructureYearMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<FeeStructureYearMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateFeeTypeMaster(FeeTypeMaster fm)
        {
            try
            {
                if (fm != null)
                    PSF.SaveOrUpdate<FeeTypeMaster>(fm);
                else { throw new Exception("CallManagement is required and it cannot be null.."); }
                return fm.FormId;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool DeleteFeeTypeMaster(long id)
        {
            try
            {
                FeeTypeMaster FeeTypeMaster = PSF.Get<FeeTypeMaster>(id);
                PSF.Delete<FeeTypeMaster>(FeeTypeMaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteFeeTypeMaster(long[] id)
        {
            try
            {
                IList<FeeTypeMaster> FeeTypeMaster = PSF.GetListByIds<FeeTypeMaster>(id);
                PSF.DeleteAll<FeeTypeMaster>(FeeTypeMaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<FeeTypeMaster>> GetFeeTypeMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<FeeTypeMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateCampusMaster(CampusMaster cm)
        {
            try
            {
                if (cm != null)
                    PSF.SaveOrUpdate<CampusMaster>(cm);
                else { throw new Exception("CallManagement is required and it cannot be null.."); }
                return cm.FormId;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool DeleteCampusMaster(long id)
        {
            try
            {
                CampusMaster CampusMaster = PSF.Get<CampusMaster>(id);
                PSF.Delete<CampusMaster>(CampusMaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteCampusMaster(long[] id)
        {
            try
            {
                IList<CampusMaster> CampusMaster = PSF.GetListByIds<CampusMaster>(id);
                PSF.DeleteAll<CampusMaster>(CampusMaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<CampusMaster>> GetCampusMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<CampusMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<BankMaster>> GetBankMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<BankMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public long CreateOrUpdateBloodGroupMaster(BloodGroupMaster bm)
        {
            try
            {
                if (bm != null)
                    PSF.SaveOrUpdate<BloodGroupMaster>(bm);
                else { throw new Exception("CallManagement is required and it cannot be null.."); }
                return bm.FormId;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool DeleteBloodGroupMaster(long id)
        {
            try
            {
                BloodGroupMaster bloodgrpmaster = PSF.Get<BloodGroupMaster>(id);
                PSF.Delete<BloodGroupMaster>(bloodgrpmaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteBloodGroupMaster(long[] id)
        {
            try
            {
                IList<BloodGroupMaster> bloodgrpmaster = PSF.GetListByIds<BloodGroupMaster>(id);
                PSF.DeleteAll<BloodGroupMaster>(bloodgrpmaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<BloodGroupMaster>> GetBloodGroupMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<BloodGroupMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateRelationshipMaster(RelationshipMaster rm)
        {
            try
            {
                if (rm != null)
                    PSF.SaveOrUpdate<RelationshipMaster>(rm);
                else { throw new Exception("CallManagement is required and it cannot be null.."); }
                return rm.FormId;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool DeleteRelationshipMaster(long id)
        {
            try
            {
                RelationshipMaster relationmaster = PSF.Get<RelationshipMaster>(id);
                PSF.Delete<RelationshipMaster>(relationmaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteRelationshipMaster(long[] id)
        {
            try
            {
                IList<RelationshipMaster> relationmaster = PSF.GetListByIds<RelationshipMaster>(id);
                PSF.DeleteAll<RelationshipMaster>(relationmaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<RelationshipMaster>> GetRelationshipMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<RelationshipMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateAcademicYearMaster(AcademicyrMaster am)
        {
            try
            {
                if (am != null)
                    PSF.SaveOrUpdate<AcademicyrMaster>(am);
                else { throw new Exception("CallManagement is required and it cannot be null.."); }
                return am.FormId;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool DeleteAcademicyrMaster(long id)
        {
            try
            {
                AcademicyrMaster academicmaster = PSF.Get<AcademicyrMaster>(id);
                PSF.Delete<AcademicyrMaster>(academicmaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteAcademicyrMaster(long[] id)
        {
            try
            {
                IList<AcademicyrMaster> academicyrmaster = PSF.GetListByIds<AcademicyrMaster>(id);
                PSF.DeleteAll<AcademicyrMaster>(academicyrmaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<AcademicyrMaster>> GetAcademicyrMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<AcademicyrMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateGradeModeOfPaymentMaster(ModeOfPaymentMaster mm)
        {
            try
            {
                if (mm != null)
                    PSF.SaveOrUpdate<ModeOfPaymentMaster>(mm);
                else { throw new Exception("CallManagement is required and it cannot be null.."); }
                return mm.FormId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteModeOfPaymentMaster(long id)
        {
            try
            {
                ModeOfPaymentMaster modeofpmntmaster = PSF.Get<ModeOfPaymentMaster>(id);
                PSF.Delete<ModeOfPaymentMaster>(modeofpmntmaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteModeOfPaymentMaster(long[] id)
        {
            try
            {
                IList<ModeOfPaymentMaster> modeofpmntmaster = PSF.GetListByIds<ModeOfPaymentMaster>(id);
                PSF.DeleteAll<ModeOfPaymentMaster>(modeofpmntmaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<ModeOfPaymentMaster>> GetModeOfPaymentMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<ModeOfPaymentMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateDocumentTypeMaster(DocumentTypeMaster dm)
        {
            try
            {
                if (dm != null)
                    PSF.SaveOrUpdate<DocumentTypeMaster>(dm);
                else { throw new Exception("CallManagement is required and it cannot be null.."); }
                return dm.FormId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<DocumentTypeMaster>> GetDocumentTypeMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<DocumentTypeMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteDocumentTypeMaster(long id)
        {
            try
            {
                DocumentTypeMaster documentmaster = PSF.Get<DocumentTypeMaster>(id);
                PSF.Delete<DocumentTypeMaster>(documentmaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteDocumentTypeMaster(long[] id)
        {
            try
            {
                IList<DocumentTypeMaster> documentmaster = PSF.GetListByIds<DocumentTypeMaster>(id);
                PSF.DeleteAll<DocumentTypeMaster>(documentmaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<MasterDetails>> GetMasterDetailListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<MasterDetails>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateIssueGroupMaster(IssueGroupMaster im)
        {
            try
            {
                if (im != null)
                    PSF.SaveOrUpdate<IssueGroupMaster>(im);
                else { throw new Exception("CallManagement is required and it cannot be null.."); }
                return im.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<IssueGroupMaster>> GetIssueGroupMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<IssueGroupMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StaffDepartmentMaster>> GetStaffDepartmentMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<StaffDepartmentMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteIssueGroupMaster(long id)
        {
            try
            {
                IssueGroupMaster issuegroupmaster = PSF.Get<IssueGroupMaster>(id);
                PSF.Delete<IssueGroupMaster>(issuegroupmaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteIssueGroupMaster(long[] id)
        {
            try
            {
                IList<IssueGroupMaster> issuegroupmaster = PSF.GetListByIds<IssueGroupMaster>(id);
                PSF.DeleteAll<IssueGroupMaster>(issuegroupmaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateIssueTypeMaster(IssueTypeMaster itm)
        {
            try
            {
                if (itm != null)
                    PSF.SaveOrUpdate<IssueTypeMaster>(itm);
                else { throw new Exception("CallManagement is required and it cannot be null.."); }
                return itm.FormId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<IssueTypeMaster>> GetIssueTypeMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<IssueTypeMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<Role>> GetRoleMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<Role>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<AssignmentNameMaster>> GetAssignmentNameMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<AssignmentNameMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteIssueTypeMaster(long id)
        {
            try
            {
                IssueTypeMaster issuetypemaster = PSF.Get<IssueTypeMaster>(id);
                PSF.Delete<IssueTypeMaster>(issuetypemaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteIssueTypeMaster(long[] id)
        {
            try
            {
                IList<IssueTypeMaster> issuetypemaster = PSF.GetListByIds<IssueTypeMaster>(id);
                PSF.DeleteAll<IssueTypeMaster>(issuetypemaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateDesignationMaster(DesignationMaster dm)
        {
            try
            {
                if (dm != null)
                    PSF.SaveOrUpdate<DesignationMaster>(dm);
                else { throw new Exception("CallManagement is required and it cannot be null.."); }
                return dm.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<DesignationMaster>> GetDesignationMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<DesignationMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteDesignationMaster(long id)
        {
            try
            {
                DesignationMaster DesignationMaster = PSF.Get<DesignationMaster>(id);
                PSF.Delete<DesignationMaster>(DesignationMaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateStaffTypeMaster(StaffTypeMaster stm)
        {
            try
            {
                if (stm != null)
                    PSF.SaveOrUpdate<StaffTypeMaster>(stm);
                else { throw new Exception("CallManagement is required and it cannot be null.."); }
                return stm.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateRoleTypeMaster(Role rl)
        {
            try
            {
                if (rl != null)
                    PSF.SaveOrUpdate<Role>(rl);
                else { throw new Exception("CallManagement is required and it cannot be null.."); }
                return rl.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateAssignmentNameMaster(AssignmentNameMaster am)
        {
            try
            {
                if (am != null)
                    PSF.SaveOrUpdate<AssignmentNameMaster>(am);
                else { throw new Exception("CallManagement is required and it cannot be null.."); }
                return am.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<StaffTypeMaster>> GetStaffTypeMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<StaffTypeMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteStaffTypeMaster(long id)
        {
            try
            {
                StaffTypeMaster StaffTypeMaster = PSF.Get<StaffTypeMaster>(id);
                PSF.Delete<StaffTypeMaster>(StaffTypeMaster);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<UserTypeMaster>> GetUserTypeMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<UserTypeMaster>> retValue = new Dictionary<long, IList<UserTypeMaster>>();
                return PSF.GetListWithSearchCriteriaCount<UserTypeMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<CampusMaster>> GetCampusMasterNEqListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCountWithNotEqualProperty<CampusMaster>(page, pageSize, sortType, sortby, criteria, "", null);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long DeleteAssignmentName(AssignmentNameMaster anm)
        {
            try
            {
                if (anm != null)
                {
                    PSF.Delete<AssignmentNameMaster>(anm);
                    return anm.Id;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<Application>> GetApplicationMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<Application>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public long CreateOrUpdateApplicationMaster(Application app)
        {
            try
            {
                if (app != null)
                    PSF.SaveOrUpdate<Application>(app);
                else { throw new Exception("Application is required and it cannot be null.."); }
                return app.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #region"Menu"
        public Dictionary<long, IList<Menu>> GetMenuListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<Menu>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long SaveOrUpdateMenuDetails(Menu mu)
        {
            try
            {
                if (mu != null)
                    PSF.SaveOrUpdate<Menu>(mu);
                else { throw new Exception("Webtemplate is required and it cannot be null.."); }
                return mu.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Menu GetDeleteMenurowById(long Id)
        {
            try
            {
                Menu mu = null;

                if (Id > 0)
                    mu = PSF.Get<Menu>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return mu;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long DeleteMenufunction(Menu mu)
        {
            try
            {
                if (mu != null)
                    PSF.Delete<Menu>(mu);
                else { throw new Exception("Value is required and it cannot be null.."); }
                return 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long SaveOrUpdateSubMenuDetails(Menu mu)
        {
            try
            {
                if (mu != null)
                    PSF.SaveOrUpdate<Menu>(mu);
                else { throw new Exception("Webtemplate is required and it cannot be null.."); }
                return mu.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Menu DeleteSubMenurowById(long Id)
        {
            try
            {
                Menu mu = null;
                if (Id > 0)
                    mu = PSF.Get<Menu>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return mu;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion"Menu"
        #region ForMISReport
        public Dictionary<long, IList<CampusGradeMaster>> GetCampusGradeMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<CampusGradeMaster>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region ReportCardCBSE
        public Dictionary<long, IList<CampusSubjectMaster>> GetSubjectMasterByCampusListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<CampusSubjectMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<CampusLanguageMaster>> GetLanguageMasterByCampusListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<CampusLanguageMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public CampusMaster GetCampusByCampus(string Campus)
        {
            try
            {
                CampusMaster cm = null;
                if (!string.IsNullOrEmpty(Campus))
                    cm = PSF.Get<CampusMaster>("Name", Campus);
                else { throw new Exception("Campus is required and it cannot be null"); }
                return cm;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public Dictionary<long, IList<SubjectMaster>> GetSubjectMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<SubjectMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region AgeCutOff
        public long SaveOrUpdateAgeCutOffDetails(AgeCutOffMaster age)
        {
            try
            {
                if (age != null)
                    PSF.SaveOrUpdate<AgeCutOffMaster>(age);
                else { throw new Exception("Id is required and it cannot be null.."); }
                return age.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public AgeCutOffMaster GetAgeCutoffMasterDetailsById(long Id)
        {
            try
            {
                AgeCutOffMaster AgeCutOffDetails = null;

                if (Id > 0)
                    AgeCutOffDetails = PSF.Get<AgeCutOffMaster>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return AgeCutOffDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long GetDeleteAgeCutOffMasterrowById(AgeCutOffMaster ACMaster)
        {
            try
            {
                if (ACMaster != null)
                    PSF.Delete<AgeCutOffMaster>(ACMaster);
                else { throw new Exception("Value is required and it cannot be null.."); }
                return 0;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        public Role GetRoleMasterDetailsById(long Id)
        {
            try
            {
                Role Role = null;

                if (Id > 0)
                    Role = PSF.Get<Role>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return Role;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<RouteMaster>> GetRouteMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<RouteMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<LocationMaster>> GetLocationMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<LocationMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<NationalityMaster>> GetNationalityMasterDetails(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<NationalityMaster>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Campus Subject MasterAdded By Prabakaran
        public Dictionary<long, IList<CampusSubjectMaster>> GetCampusSubjectMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<CampusSubjectMaster>(page, pageSize, sortby, sortType, criteria, likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateCampusSubjectMaster(CampusSubjectMaster campussubjectmaster)
        {
            try
            {
                if (campussubjectmaster != null)
                    PSF.SaveOrUpdate<CampusSubjectMaster>(campussubjectmaster);
                else { throw new Exception("campussubjectmaster is required and it cannot be null.."); }
                return campussubjectmaster.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public CampusSubjectMaster GetCampusSubjectMasterById(int Id)
        {
            try
            {
                if (Id > 0)
                    return PSF.Get<CampusSubjectMaster>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long DeleteCampusSubjectMasterList(IList<int> Id)
        {
            try
            {
                if (Id != null)
                {
                    IList<CampusSubjectMaster> csmlist = new List<CampusSubjectMaster>();
                    foreach (int item in Id)
                    {
                        CampusSubjectMaster csm = PSF.Get<CampusSubjectMaster>("Id", item);
                        if (csm != null)
                        {
                            csmlist.Add(csm);
                        }
                    }
                    if (csmlist.Count > 0)
                    {
                        PSF.DeleteAll<CampusSubjectMaster>(csmlist);
                    }
                }
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion        
        public CampusMaster GetCampusById(long Id)
        {
            try
            {
                if (Id > 0)
                    return PSF.Get<CampusMaster>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region Campus Wise Section Master By john naveen
        public Dictionary<long, IList<CampusWiseSectionMaster>> GetCampusWiseSectionMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<CampusWiseSectionMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateCampusWiseSectionMaster(CampusWiseSectionMaster cwsm)
        {
            try
            {
                if (cwsm != null)
                    PSF.SaveOrUpdate<CampusWiseSectionMaster>(cwsm);
                else { throw new Exception("Error"); }
                return cwsm.CampusWiseSectionMasterId;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteCampusWiseSectionMaster(long[] Id)
        {
            try
            {
                IList<CampusWiseSectionMaster> tasksList = PSF.GetListByIds<CampusWiseSectionMaster>(Id);
                PSF.DeleteAll<CampusWiseSectionMaster>(tasksList);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public CampusWiseSectionMaster GetCampusWiseSectionMasterBySection(long AcademicYearId, long CampusId, long GradeId, string Section)
        {
            try
            {
                CampusWiseSectionMaster cwsm = null;
                cwsm = PSF.Get<CampusWiseSectionMaster>("AcademicyrMaster.FormId", AcademicYearId, "CampusMaster.FormId", CampusId, "CampusGradeMaster.Id", GradeId, "Section", Section);
                return cwsm;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region Added By Prabakaran
        public Dictionary<long, IList<CampusWiseSectionMaster_vw>> GetCampusWiseSectionMaster_vwListWithExactAndLikeSearchCriteriaWithCount(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<CampusWiseSectionMaster_vw>(page, pageSize, sortType, sortby, criteria, likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        public Dictionary<long, IList<NewDepartmentMaster>> GetNewDepartmentMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<NewDepartmentMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
