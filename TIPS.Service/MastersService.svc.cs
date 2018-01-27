using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.Component;
using TIPS.ServiceContract;
using TIPS.Entities;
using TIPS.Entities.EnquiryEntities;
using TIPS.Entities.StoreEntities;
using TIPS.Entities.MenuEntities;
using TIPS.Entities.Assess;
using TIPS.Entities.TransportEntities;
using TIPS.Entities.ReportEntities;

namespace TIPS.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MasterService" in code, svc and config file together.
    public class MastersService : IMastersSC
    {
        MastersBC Mbc = new MastersBC();
        public long CreateOrUpdateRegionMaster(Entities.RegionMaster region)
        {
            try
            {
                MastersBC RegionBC = new MastersBC();
                RegionBC.CreateOrUpdateRegionMaster(region);
                return region.FormId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteRegionMaster(long id)
        {
            try
            {
                MastersBC RegionBC = new MastersBC();
                RegionBC.DeleteRegionMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteRegionMaster(long[] id)
        {
            try
            {
                MastersBC RegionBC = new MastersBC();
                RegionBC.DeleteRegionMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<Entities.RegionMaster>> GetRegionMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC RegionBC = new MastersBC();
                return RegionBC.GetRegionMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public long CreateOrUpdateCountryMaster(Entities.CountryMaster country)
        {
            try
            {
                MastersBC CountryBC = new MastersBC();
                CountryBC.CreateOrUpdateCountryMaster(country);
                return country.FormId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteCountryMaster(long id)
        {
            try
            {
                MastersBC CountryBC = new MastersBC();
                CountryBC.DeleteCountryMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteCountryMaster(long[] id)
        {
            try
            {
                MastersBC CountryBC = new MastersBC();
                CountryBC.DeleteCountryMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<Entities.CountryMaster>> GetCountryMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC CountryBC = new MastersBC();
                return CountryBC.GetCountryMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public long CreateOrUpdateGradeMaster(Entities.GradeMaster grade)
        {
            try
            {
                MastersBC GradeBC = new MastersBC();
                GradeBC.CreateOrUpdateGradeMaster(grade);
                return grade.FormId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }

        }

        public bool DeleteGradeMaster(long id)
        {
            try
            {
                MastersBC GradeBC = new MastersBC();
                GradeBC.DeleteGradeMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteGradeMaster(long[] id)
        {
            try
            {
                MastersBC GradeBC = new MastersBC();
                GradeBC.DeleteGradeMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }

        }

        public Dictionary<long, IList<Entities.GradeMaster>> GetGradeMasterListWithPagingAndCriteriaWithIn(int? page, int? pageSize, string sortby, string sortType, string name, int[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                MastersBC GradeBC = new MastersBC();
                return GradeBC.GetGradeMasterListWithPagingAndCriteriaWithIn(page, pageSize, sortby, sortType, name, values, criteria, alias);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<Entities.GradeMaster>> GetGradeMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC GradeBC = new MastersBC();
                return GradeBC.GetGradeMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateSectionMaster(Entities.SectionMaster section)
        {
            try
            {
                MastersBC SectionBC = new MastersBC();
                SectionBC.CreateOrUpdateSectionMaster(section);
                return section.FormId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }

        }

        public bool DeleteSectionMaster(long id)
        {
            try
            {
                MastersBC SectionBC = new MastersBC();
                SectionBC.DeleteSectionMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteSectionMaster(long[] id)
        {
            try
            {
                MastersBC SectionBC = new MastersBC();
                SectionBC.DeleteSectionMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }

        }

        public Dictionary<long, IList<Entities.SectionMaster>> GetSectionMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC SectionBC = new MastersBC();
                return SectionBC.GetSectionMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateFeeStructureYearMaster(Entities.FeeStructureYearMaster feestructureyear)
        {
            try
            {
                MastersBC feestructureyearBC = new MastersBC();
                feestructureyearBC.CreateOrUpdateFeeStructureYearMaster(feestructureyear);
                return feestructureyear.FormId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteFeeStructureYearMaster(long id)
        {
            try
            {
                MastersBC feestructyrBC = new MastersBC();
                feestructyrBC.DeleteFeeStructureYearMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }

        }

        public bool DeleteFeeStructureYearMaster(long[] id)
        {
            try
            {
                MastersBC feestructyrBC = new MastersBC();
                feestructyrBC.DeleteFeeStructureYearMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }

        }

        public Dictionary<long, IList<Entities.FeeStructureYearMaster>> GetFeeStructureYearMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC feestructBC = new MastersBC();
                return feestructBC.GetFeeStructureYearMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateFeeTypeMaster(Entities.FeeTypeMaster FeeTypeMaster)
        {
            try
            {
                MastersBC feestructureyearBC = new MastersBC();
                feestructureyearBC.CreateOrUpdateFeeTypeMaster(FeeTypeMaster);
                return FeeTypeMaster.FormId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteFeeTypeMaster(long id)
        {
            try
            {
                MastersBC feetypeBC = new MastersBC();
                feetypeBC.DeleteFeeTypeMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }

        }

        public bool DeleteFeeTypeMaster(long[] id)
        {
            try
            {
                MastersBC feetypeBC = new MastersBC();
                feetypeBC.DeleteFeeTypeMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }

        }

        public Dictionary<long, IList<Entities.FeeTypeMaster>> GetFeeTypeMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC feetypeBC = new MastersBC();
                return feetypeBC.GetFeeTypeMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateCampusMaster(Entities.CampusMaster CampusMaster)
        {
            try
            {
                MastersBC CampusMasterBC = new MastersBC();
                CampusMasterBC.CreateOrUpdateCampusMaster(CampusMaster);
                return CampusMaster.FormId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteCampusMaster(long id)
        {
            try
            {
                MastersBC CampusMasterBC = new MastersBC();
                CampusMasterBC.DeleteCampusMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }

        }

        public bool DeleteCampusMaster(long[] id)
        {
            try
            {
                MastersBC CampusBC = new MastersBC();
                CampusBC.DeleteCampusMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }

        }

        public Dictionary<long, IList<Entities.CampusMaster>> GetCampusMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC CampusBC = new MastersBC();
                return CampusBC.GetCampusMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                MastersBC CampusBC = new MastersBC();
                return CampusBC.GetBankMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateBloodGroupMaster(Entities.BloodGroupMaster bloodgroup)
        {
            try
            {
                MastersBC BloodgrpBC = new MastersBC();
                BloodgrpBC.CreateOrUpdateBloodGroupMaster(bloodgroup);
                return bloodgroup.FormId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteBloodGroupMaster(long id)
        {
            try
            {
                MastersBC bloodgrpBC = new MastersBC();
                bloodgrpBC.DeleteBloodGroupMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteBloodGroupMaster(long[] id)
        {
            try
            {
                MastersBC bloodgrpBC = new MastersBC();
                bloodgrpBC.DeleteBloodGroupMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<Entities.BloodGroupMaster>> GetBloodGroupMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC bloodgrpBC = new MastersBC();
                return bloodgrpBC.GetBloodGroupMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateRelationshipMaster(Entities.RelationshipMaster relation)
        {
            try
            {
                MastersBC RelationBC = new MastersBC();
                RelationBC.CreateOrUpdateRelationshipMaster(relation);
                return relation.FormId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteRelationshipMaster(long id)
        {
            try
            {
                MastersBC RelationshipBC = new MastersBC();
                RelationshipBC.DeleteRelationshipMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }

        }

        public bool DeleteRelationshipMaster(long[] id)
        {
            try
            {
                MastersBC RelationshipBC = new MastersBC();
                RelationshipBC.DeleteRelationshipMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<Entities.RelationshipMaster>> GetRelationshipMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC RelationshipBC = new MastersBC();
                return RelationshipBC.GetRelationshipMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public long CreateOrUpdateAcademicYearMaster(Entities.AcademicyrMaster academic)
        {
            try
            {
                MastersBC AcademicBC = new MastersBC();
                AcademicBC.CreateOrUpdateAcademicYearMaster(academic);
                return academic.FormId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }

        }

        public bool DeleteAcademicyrMaster(long id)
        {
            try
            {
                MastersBC AcademicBC = new MastersBC();
                AcademicBC.DeleteAcademicyrMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteAcademicyrMaster(long[] id)
        {
            try
            {
                MastersBC AcademicBC = new MastersBC();
                AcademicBC.DeleteAcademicyrMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<Entities.AcademicyrMaster>> GetAcademicyrMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC academicBC = new MastersBC();
                return academicBC.GetAcademicyrMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateModeOfPaymentMaster(Entities.ModeOfPaymentMaster modeofpayment)
        {
            try
            {
                MastersBC modeofpmtBC = new MastersBC();
                modeofpmtBC.CreateOrUpdateGradeModeOfPaymentMaster(modeofpayment);
                return modeofpayment.FormId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteModeOfPaymentMaster(long id)
        {
            try
            {
                MastersBC ModeofpmntBC = new MastersBC();
                ModeofpmntBC.DeleteModeOfPaymentMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteModeOfPaymentMaster(long[] id)
        {
            try
            {
                MastersBC ModeofpmntBC = new MastersBC();
                ModeofpmntBC.DeleteModeOfPaymentMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }

        }

        public Dictionary<long, IList<Entities.ModeOfPaymentMaster>> GetModeOfPaymentMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC modeofpmtBC = new MastersBC();
                return modeofpmtBC.GetModeOfPaymentMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public long CreateOrUpdateDocumentTypeMaster(Entities.DocumentTypeMaster document)
        {
            try
            {
                MastersBC documentBC = new MastersBC();
                documentBC.CreateOrUpdateDocumentTypeMaster(document);
                return document.FormId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }



        public bool DeleteDocumentTypeMaster(long id)
        {
            try
            {
                MastersBC DocumentBC = new MastersBC();
                DocumentBC.DeleteDocumentTypeMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteDocumentTypeMaster(long[] id)
        {
            try
            {
                MastersBC DocumentBC = new MastersBC();
                DocumentBC.DeleteDocumentTypeMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }

        }

        public Dictionary<long, IList<Entities.DocumentTypeMaster>> GetDocumentTypeMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC documenttypeBC = new MastersBC();
                return documenttypeBC.GetDocumentTypeMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<Entities.MasterDetails>> GetMasterDetailListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC MasterDetailBC = new MastersBC();
                return MasterDetailBC.GetMasterDetailListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateIssueGroupMaster(Entities.IssueGroupMaster issuegroup)
        {
            try
            {
                MastersBC issuegroupBC = new MastersBC();
                issuegroupBC.CreateOrUpdateIssueGroupMaster(issuegroup);
                return issuegroup.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteIssueGroupMaster(long id)
        {
            try
            {
                MastersBC issuegroupBC = new MastersBC();
                issuegroupBC.DeleteIssueGroupMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteIssueGroupMaster(long[] id)
        {
            try
            {
                MastersBC issuegroupBC = new MastersBC();
                issuegroupBC.DeleteIssueGroupMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }

        }

        public Dictionary<long, IList<Entities.IssueGroupMaster>> GetIssueGroupMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC issuegroupBC = new MastersBC();
                return issuegroupBC.GetIssueGroupMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                MastersBC issuegroupBC = new MastersBC();
                return issuegroupBC.GetStaffDepartmentMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateIssueTypeMaster(Entities.IssueTypeMaster issuetype)
        {
            try
            {
                MastersBC issuetypeBC = new MastersBC();
                issuetypeBC.CreateOrUpdateIssueTypeMaster(issuetype);
                return issuetype.FormId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteIssueTypeMaster(long id)
        {
            try
            {
                MastersBC issuetypeBC = new MastersBC();
                issuetypeBC.DeleteIssueTypeMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public bool DeleteIssueTypeMaster(long[] id)
        {
            try
            {
                MastersBC issuetypeBC = new MastersBC();
                issuetypeBC.DeleteIssueTypeMaster(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<Entities.IssueTypeMaster>> GetIssueTypeMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC issuetypeBC = new MastersBC();
                return issuetypeBC.GetIssueTypeMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<Entities.Role>> GetRoleMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC RoleBC = new MastersBC();
                return RoleBC.GetRoleMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateRoleTypeMaster(Entities.Role role)
        {
            try
            {
                MastersBC roleBC = new MastersBC();
                roleBC.CreateOrUpdateRoleTypeMaster(role);
                return role.Id;
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

        public Dictionary<long, IList<Entities.AssignmentNameMaster>> GetAssignmentNameMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC RoleBC = new MastersBC();
                return RoleBC.GetAssignmentNameMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateAssignmentNameMaster(Entities.AssignmentNameMaster am)
        {
            try
            {
                MastersBC roleBC = new MastersBC();
                roleBC.CreateOrUpdateAssignmentNameMaster(am);
                return am.Id;
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

        public Dictionary<long, IList<Entities.DesignationMaster>> GetDesignationMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC designationBC = new MastersBC();
                return designationBC.GetDesignationMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateDesignationMaster(Entities.DesignationMaster designation)
        {
            try
            {
                MastersBC designationBC = new MastersBC();
                designationBC.CreateOrUpdateDesignationMaster(designation);
                return designation.Id;
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

        public bool DeleteDesignationMaster(long id)
        {
            try
            {
                MastersBC designationBC = new MastersBC();
                designationBC.DeleteDesignationMaster(id);
                return true;
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

        public Dictionary<long, IList<Entities.StaffTypeMaster>> GetStaffTypeMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC stafftypeBC = new MastersBC();
                return stafftypeBC.GetStaffTypeMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateStaffTypeMaster(Entities.StaffTypeMaster StaffType)
        {
            try
            {
                MastersBC stafftypeBC = new MastersBC();
                stafftypeBC.CreateOrUpdateStaffTypeMaster(StaffType);
                return StaffType.Id;
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

        public bool DeleteStaffTypeMaster(long id)
        {
            try
            {
                MastersBC stafftypeBC = new MastersBC();
                stafftypeBC.DeleteStaffTypeMaster(id);
                return true;
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
        public Dictionary<long, IList<UserTypeMaster>> GetUserTypeMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC masterBC = new MastersBC();
                return masterBC.GetUserTypeMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<Entities.CampusMaster>> GetCampusMasterNEqListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC CampusBC = new MastersBC();
                return CampusBC.GetCampusMasterNEqListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                MastersBC MastersBC = new MastersBC();
                MastersBC.DeleteAssignmentName(anm);
                return anm.Id;
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

        public Dictionary<long, IList<Application>> GetApplicationMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC GradeBC = new MastersBC();
                return GradeBC.GetApplicationMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                MastersBC MastersBC = new MastersBC();
                MastersBC.CreateOrUpdateApplicationMaster(app);
                return app.Id;
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

        #region"Menu"
        public Dictionary<long, IList<Menu>> GetMenuListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC menu = new MastersBC();
                return menu.GetMenuListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                MastersBC MastersBC = new MastersBC();
                MastersBC.SaveOrUpdateMenuDetails(mu);
                return mu.Id;
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

        public Menu GetDeleteMenurowById(long Id)
        {
            try
            {
                MastersBC MastersBC = new MastersBC();
                return MastersBC.GetDeleteMenurowById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }


        public long DeleteMenufunction(Menu mu)
        {
            try
            {
                MastersBC MastersBC = new MastersBC();
                MastersBC.DeleteMenufunction(mu);
                return 0;
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
        public long SaveOrUpdateSubMenuDetails(Menu mu)
        {
            try
            {
                MastersBC MastersBC = new MastersBC();
                MastersBC.SaveOrUpdateSubMenuDetails(mu);
                return mu.Id;
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
        public Menu DeleteSubMenurowById(long Id)
        {
            try
            {
                MastersBC MastersBC = new MastersBC();
                return MastersBC.DeleteSubMenurowById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion"Menu"
        #region ForMISReport
        public Dictionary<long, IList<CampusGradeMaster>> GetCampusGradeMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC GradeBC = new MastersBC();
                return GradeBC.GetCampusGradeMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region AgeCutOff
        public long SaveOrUpdateAgeCutOffDetails(AgeCutOffMaster age)
        {
            try
            {
                MastersBC MBC = new MastersBC();
                MBC.SaveOrUpdateAgeCutOffDetails(age);
                return age.Id;
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

        public AgeCutOffMaster GetAgeCutoffMasterDetailsById(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    MastersBC MBC = new MastersBC();
                    return MBC.GetAgeCutoffMasterDetailsById(Id);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public long GetDeleteAgeCutOffMasterById(AgeCutOffMaster ACMaster)
        {
            try
            {
                if (ACMaster.Id > 0)
                {
                    MastersBC MBC = new MastersBC();
                    MBC.GetDeleteAgeCutOffMasterrowById(ACMaster);
                    return ACMaster.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #endregion

        #region ReportCardCBSE
        public Dictionary<long, IList<CampusSubjectMaster>> GetSubjectMasterByCampusListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC Assess360BC = new MastersBC();
                return Assess360BC.GetSubjectMasterByCampusListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<CampusLanguageMaster>> GetLanguageMasterByCampusListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC Assess360BC = new MastersBC();
                return Assess360BC.GetLanguageMasterByCampusListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public CampusMaster GetCampusByCampus(string Campus)
        {
            try
            {
                MastersBC MBC = new MastersBC();
                return MBC.GetCampusByCampus(Campus);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion

        public Dictionary<long, IList<SubjectMaster>> GetSubjectMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC mbc = new MastersBC();
                return mbc.GetSubjectMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Role GetRoleMasterDetailsById(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    MastersBC MBC = new MastersBC();
                    return MBC.GetRoleMasterDetailsById(Id);
                }
                else throw new Exception("Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #region Student Route Configuration
        public Dictionary<long, IList<RouteMaster>> GetRouteMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC RouteBC = new MastersBC();
                return RouteBC.GetRouteMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
        public Dictionary<long, IList<LocationMaster>> GetLocationMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC LocationBC = new MastersBC();
                return LocationBC.GetLocationMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<NationalityMaster>> GetNationalityDetails(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return Mbc.GetNationalityMasterDetails(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region CampusSubjectMaster Added By Prabakaran
        public Dictionary<long, IList<CampusSubjectMaster>> GetCampusSubjectMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {

                return Mbc.GetCampusSubjectMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria, likecriteria);
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
                return Mbc.CreateOrUpdateCampusSubjectMaster(campussubjectmaster);
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
                return Mbc.GetCampusSubjectMasterById(Id);
            }
            catch (Exception)
            {

                throw;
            }
            finally { }
        }
        public long DeleteCampusSubjectMasterList(IList<int> Id)
        {
            try
            {
                return Mbc.DeleteCampusSubjectMasterList(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        public CampusMaster GetCampusById(long Id)
        {
            try
            {
                return Mbc.GetCampusById(Id);
            }
            catch (Exception)
            {

                throw;
            }
            finally { }
        }
        #region Campus Wise Section Master By john naveen
        public Dictionary<long, IList<CampusWiseSectionMaster>> GetCampusWiseSectionMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC MBC = new MastersBC();
                return MBC.GetCampusWiseSectionMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                MastersBC MBC = new MastersBC();
                MBC.CreateOrUpdateCampusWiseSectionMaster(cwsm);
                return cwsm.CampusWiseSectionMasterId;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public bool DeleteCampusWiseSectionMaster(long[] Ids)
        {
            try
            {
                MastersBC MBC = new MastersBC();
                return MBC.DeleteCampusWiseSectionMaster(Ids);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public CampusWiseSectionMaster GetCampusWiseSectionMasterBySection(long AcademicYearId, long CampusId, long GradeId, string Section)
        {
            try
            {
                if (AcademicYearId >= 0 && CampusId >= 0 && GradeId >= 0 && !string.IsNullOrEmpty(Section))
                {
                    MastersBC MBC = new MastersBC();
                    return MBC.GetCampusWiseSectionMasterBySection(AcademicYearId, CampusId, GradeId, Section);
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
        #region Added By Prabakaran
        public Dictionary<long, IList<CampusWiseSectionMaster_vw>> GetCampusWiseSectionMaster_vwListWithExactAndLikeSearchCriteriaWithCount(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                MastersBC MBC = new MastersBC();
                return MBC.GetCampusWiseSectionMaster_vwListWithExactAndLikeSearchCriteriaWithCount(page, pageSize, sortby, sortType, criteria, likecriteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        public Dictionary<long, IList<Entities.NewDepartmentMaster>> GetNewDepartmentMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                MastersBC RelationshipBC = new MastersBC();
                return RelationshipBC.GetNewDepartmentMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }

        }
       
    }
}
