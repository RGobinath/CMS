using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.Entities.Assess;
using TIPS.Component;
using TIPS.Entities.Assess.ReportCardClasses;

namespace TIPS.ServiceContract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ReportCardService" in code, svc and config file together.
    public class ReportCardService : IReportCardService
    {
        #region "MYP"
        public long SaveOrUpdateMYPReportCard(RptCardMYP RptCardMYP)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                return ReportCardBC.SaveOrUpdateMYPReportCard(RptCardMYP);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public RptCardMYP GetMYPReportCard(long Id)
        {

            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                return ReportCardBC.GetMYPReportCard(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }   
        #endregion "MYP"

        #region "PYP"
        public long SaveOrUpdatePYPReportCard(RptCardPYP RptCardPYP)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                return ReportCardBC.SaveOrUpdatePYPReportCard(RptCardPYP);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public RptCardPYP GetPYPReportCard(long Id)
        {

            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                return ReportCardBC.GetPYPReportCard(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion "PYP"

        public long SaveOrUpdateRptCardFocus(RptCardFocus RptCardFocus)
        {

            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                return ReportCardBC.SaveOrUpdateRptCardFocus(RptCardFocus);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        
        public RptCardFocus GetRptCardFocusById(long RptCardFocusId)
        {

            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                return ReportCardBC.GetRptCardFocusById(RptCardFocusId);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public RptCardFocus GetRptCardFocusByGradeCampusSem(string Grade, string Campus, long Semester, string AcademicYear)
        {

            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                return ReportCardBC.GetRptCardFocusByGradeCampusSem(Grade, Campus, Semester, AcademicYear);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<RptCardInBoxView>> GetRepCardInBoxListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                Dictionary<long, IList<RptCardInBoxView>> retValue = ReportCardBC.GetRepCardInBoxListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<StudentDtlsView>> GetStudentDtlsViewListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, string name, string[] values, Dictionary<string, object> criteria)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                Dictionary<long, IList<StudentDtlsView>> retValue = ReportCardBC.GetStudentDtlsViewListWithPagingAndCriteria(page, pageSize, sortby, sortType, name, values, criteria);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<RptCardInBoxView>> GetRepCardInBoxListWithPagingAndCriteriaEQSearch(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                Dictionary<long, IList<RptCardInBoxView>> retValue = ReportCardBC.GetRepCardInBoxListWithPagingAndCriteriaEQSearch(page, pageSize, sortType, sortby, criteria);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<SubjectMarksForRptCard_Vw>> GetSubjectMarksForRptCard_VwWidthSubjectWiseList(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                return ReportCardBC.GetSubjectMarksForRptCard_VwWidthSubjectWiseList(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        #region Start PYP 1 to 5 Grades
        public long SaveOrUpdatePYPFirstGrade(RptCardPYPFirstGrade RptFirstGrade)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                return ReportCardBC.SaveOrUpdatePYPFirstGrade(RptFirstGrade);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public RptCardPYPFirstGrade GetPYPFirstGrade(long Id)
        {

            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                return ReportCardBC.GetPYPFirstGrade(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<RptCardPYPFirstGrade>> GetRepCardFirstGradeListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                Dictionary<long, IList<RptCardPYPFirstGrade>> retValue = ReportCardBC.GetRepCardFirstGradeListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion End PYP 1 - 5 Grades
        public Dictionary<long, IList<RptCardMYP>> GetRptCardMYPListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                Dictionary<long, IList<RptCardMYP>> retValue = ReportCardBC.GetRptCardMYPListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        #region "Bulk Report Card Creation for MYP"
        public Dictionary<long, IList<StudentDtlsView>> GetStudentDtlsViewListforBulkRptCardWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                Dictionary<long, IList<StudentDtlsView>> retValue = ReportCardBC.GetStudentDtlsViewListforBulkRptCardWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        
        public long SaveOrUpdateBulkMYPReportCard(List<StudentDtlsView> rptCardMYP, string semester, string teachName, string rptMypDate)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                return ReportCardBC.SaveOrUpdateBulkMYPReportCard(rptCardMYP, semester, teachName, rptMypDate);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion End.

        #region ReportCardForTirupuandKarur CBSE
        public Dictionary<long, IList<ReportCardRequest>> GetReportCardRequestListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                Dictionary<long, IList<ReportCardRequest>> retValue = ReportCardBC.GetReportCardRequestListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateReportCardRequest(ReportCardRequest RptReq)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                return ReportCardBC.CreateOrUpdateReportCardRequest(RptReq);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public ReportCardRequest GetRepoetCardRequestById(long Id)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                return ReportCardBC.GetRepoetCardRequestById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<RptStudentDtlsView>> GetStudentDtlsViewListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                Dictionary<long, IList<RptStudentDtlsView>> retValue = ReportCardBC.GetStudentDtlsViewListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<ReportCardCBSE>> GetRptCardForCBSEListWithCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                Dictionary<long, IList<ReportCardCBSE>> retValue = ReportCardBC.GetRptCardForCBSEListWithCriteria(page, pageSize, sortType, sortby, criteria);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<ReportCardCBSECommon>> GetRptCardForCBSECommonListWithCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                Dictionary<long, IList<ReportCardCBSECommon>> retValue = ReportCardBC.GetRptCardForCBSECommonListWithCriteria(page, pageSize, sortType, sortby, criteria);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public ReportCardCBSE GetReportCardCBSEById(long Id)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                return ReportCardBC.GetReportCardCBSEById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public ReportCardCBSECommon GetReportCardCBSECommonById(long Id)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                return ReportCardBC.GetReportCardCBSECommonById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public RptCoScholasticCBSE GetReportCardCoScholasticCBSEById(long Id)
        {
            try
            {
                ReportCardBC RptCrdBC = new ReportCardBC();
                return RptCrdBC.GetReportCardCoScholasticCBSEById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long SaveOrUpdateRptCardForCBSE(ReportCardCBSE RptVItoVIII)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                return ReportCardBC.SaveOrUpdateRptCardForCBSE(RptVItoVIII);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long SaveOrUpdateRptCardForCBSECommon(ReportCardCBSECommon RptVItoVIII)
        {
            try
            {
                ReportCardBC RptCardBC = new ReportCardBC();
                return RptCardBC.SaveOrUpdateRptCardForCBSECommon(RptVItoVIII);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long SaveOrUpdateRptCardCoScholasticForCBSE(RptCoScholasticCBSE RptCoSchVItoVIII)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                return ReportCardBC.SaveOrUpdateRptCardCoScholasticForCBSE(RptCoSchVItoVIII);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public ReportCardCBSECommon GetReportCardCBSECommon(long RegNum, string AcYear)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                return ReportCardBC.GetReportCardCBSECommon(RegNum, AcYear);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        /// <summary>
        /// Newly Added
        /// </summary>

        public Dictionary<long, IList<CoScholasticMaster>> GetCoScholictMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                Dictionary<long, IList<CoScholasticMaster>> retValue = ReportCardBC.GetCoScholictMasterListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<CoScholasticItemMaster>> GetCoScholasticItemMasterListWithCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                Dictionary<long, IList<CoScholasticItemMaster>> retValue = ReportCardBC.GetCoScholasticItemMasterListWithCriteria(page, pageSize, sortType, sortby, criteria);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<CoScholasticStudentDetails_Vw>> GetCoScholasticStudentDtlsViewListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                ReportCardBC RptCrdBC = new ReportCardBC();
                Dictionary<long, IList<CoScholasticStudentDetails_Vw>> retValue = RptCrdBC.GetCoScholasticStudentDtlsViewListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<RptCoScholasticCBSE>> GetRptScholasticCBSEListWithCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                ReportCardBC RptCrdBC = new ReportCardBC();
                Dictionary<long, IList<RptCoScholasticCBSE>> retValue = RptCrdBC.GetRptScholasticCBSEListWithCriteria(page, pageSize, sortType, sortby, criteria);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<CoScholasticItemMaster>> GetCoScholasticItemMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                ReportCardBC RptCrdBC = new ReportCardBC();
                Dictionary<long, IList<CoScholasticItemMaster>> retValue = RptCrdBC.GetCoScholasticItemMasterListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<CoSchCriteriaMaster>> GetCoScholasticCriteriaMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                ReportCardBC RptCrdBC = new ReportCardBC();
                Dictionary<long, IList<CoSchCriteriaMaster>> retValue = RptCrdBC.GetCoScholasticCriteriaMasterListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<ReportCardCBSECo_Scholastic>> GetRptCardCoSchWorkandEducationListWithCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                Dictionary<long, IList<ReportCardCBSECo_Scholastic>> retValue = ReportCardBC.GetRptCardCoSchWorkandEducationListWithCriteria(page, pageSize, sortType, sortby, criteria);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public ReportCardCBSECo_Scholastic GetReportCardCBSECo_ScholasticById(long Id)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                return ReportCardBC.GetReportCardCBSECo_ScholasticById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long SaveOrUpdateReportCardCBSECo_Scholastic(ReportCardCBSECo_Scholastic Co_Scholastic)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                return ReportCardBC.SaveOrUpdateReportCardCBSECo_Scholastic(Co_Scholastic);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<CoSch_Item_Vw>> GetRptCardCosch_ItemListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                Dictionary<long, IList<CoSch_Item_Vw>> retValue = ReportCardBC.GetRptCardCosch_ItemListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public string GetCoSchGradeByPreRegNum(long PreRegNum, string CoSchGradeColumn,string Term)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                string retValue = ReportCardBC.GetCoSchGradeByPreRegNum(PreRegNum, CoSchGradeColumn, Term);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        #region "New changes on CBSE ReportCard"
        public Dictionary<long, IList<RptStudentDtlsViewNew>> GetStudentDtlsViewNewListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                Dictionary<long, IList<RptStudentDtlsViewNew>> retValue = ReportCardBC.GetStudentDtlsViewNewListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<ReportCardCBSENew>> GetRptCardForCBSENewListWithCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                Dictionary<long, IList<ReportCardCBSENew>> retValue = ReportCardBC.GetRptCardForCBSENewListWithCriteria(page, pageSize, sortType, sortby, criteria);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public ReportCardCBSENew GetReportCardCBSENewById(long Id)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                return ReportCardBC.GetReportCardCBSENewById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long SaveOrUpdateRptCardForCBSENew(ReportCardCBSENew RptVItoIX)
        {
            try
            {
                ReportCardBC ReportCardBC = new ReportCardBC();
                return ReportCardBC.SaveOrUpdateRptCardForCBSENew(RptVItoIX);
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
