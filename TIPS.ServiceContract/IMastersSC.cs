using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TIPS.Entities;
using System.ServiceModel;


namespace TIPS.ServiceContract
{
    [ServiceContract()]
    public interface IMastersSC
    {
        long CreateOrUpdateRegionMaster(RegionMaster region);
        bool DeleteRegionMaster(long id);
        bool DeleteRegionMaster(long[] id);
        [OperationContract]
        Dictionary<long, IList<RegionMaster>> GetRegionMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);

        long CreateOrUpdateCountryMaster(CountryMaster country);
        bool DeleteCountryMaster(long id);
        bool DeleteCountryMaster(long[] id);
        [OperationContract]
        Dictionary<long, IList<CountryMaster>> GetCountryMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);

        long CreateOrUpdateGradeMaster(GradeMaster region);
        bool DeleteGradeMaster(long id);
        bool DeleteGradeMaster(long[] id);
        [OperationContract]
        Dictionary<long, IList<GradeMaster>> GetGradeMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);

        long CreateOrUpdateSectionMaster(SectionMaster section);
        bool DeleteSectionMaster(long id);
        bool DeleteSectionMaster(long[] id);
        [OperationContract]
        Dictionary<long, IList<SectionMaster>> GetSectionMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);

        long CreateOrUpdateFeeStructureYearMaster(FeeStructureYearMaster feestructureyear);
        bool DeleteFeeStructureYearMaster(long id);
        bool DeleteFeeStructureYearMaster(long[] id);
        [OperationContract]
        Dictionary<long, IList<FeeStructureYearMaster>> GetFeeStructureYearMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);

        long CreateOrUpdateBloodGroupMaster(BloodGroupMaster bloodgroup);
        bool DeleteBloodGroupMaster(long id);
        bool DeleteBloodGroupMaster(long[] id);
        [OperationContract]
        Dictionary<long, IList<BloodGroupMaster>> GetBloodGroupMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);

        long CreateOrUpdateRelationshipMaster(RelationshipMaster relation);
        bool DeleteRelationshipMaster(long id);
        bool DeleteRelationshipMaster(long[] id);
        [OperationContract]
        Dictionary<long, IList<RelationshipMaster>> GetRelationshipMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);

        long CreateOrUpdateAcademicYearMaster(AcademicyrMaster academic);
        [OperationContract]
        Dictionary<long, IList<AcademicyrMaster>> GetAcademicyrMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);

        long CreateOrUpdateModeOfPaymentMaster(ModeOfPaymentMaster modeofpayment);
        bool DeleteModeOfPaymentMaster(long id);
        bool DeleteModeOfPaymentMaster(long[] id);
        [OperationContract]
        Dictionary<long, IList<ModeOfPaymentMaster>> GetModeOfPaymentMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);

        long CreateOrUpdateDocumentTypeMaster(DocumentTypeMaster documenttype);
        bool DeleteDocumentTypeMaster(long id);
        bool DeleteDocumentTypeMaster(long[] id);
        [OperationContract]
        Dictionary<long, IList<DocumentTypeMaster>> GetDocumentTypeMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);



        long CreateOrUpdateIssueGroupMaster(IssueGroupMaster issuegroup);
        bool DeleteIssueGroupMaster(long id);
        bool DeleteIssueGroupMaster(long[] id);
        [OperationContract]
        Dictionary<long, IList<IssueGroupMaster>> GetIssueGroupMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);

        long CreateOrUpdateIssueTypeMaster(IssueTypeMaster issuetype);
        bool DeleteIssueTypeMaster(long id);
        bool DeleteIssueTypeMaster(long[] id);
        [OperationContract]
        Dictionary<long, IList<IssueTypeMaster>> GetIssueTypeMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);
        Dictionary<long, IList<CampusMaster>> GetCampusMasterNEqListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria);
    }
}
