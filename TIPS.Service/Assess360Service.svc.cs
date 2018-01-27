using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.Entities.Assess;
using TIPS.Component;
using TIPS.ServiceContract;
using System.Data;
using System.Collections;
using TIPS.Entities.AdmissionEntities;
using TIPS.Entities;

namespace TIPS.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Assess360Service" in code, svc and config file together.
    public class Assess360Service : IAssess360SC
    {
        Assess360BC a360BCobj = new Assess360BC();
        public Assess360 GetAssess360ById(long Id)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetAssess360ById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long SaveOrUpdateAssess360Component(Assess360Component Assess360Component)
        {
            try
            {
                long retValue;
                Assess360BC Assess360BC = new Assess360BC();
                retValue = Assess360BC.SaveOrUpdateAssess360Component(Assess360Component);
                this.StoreConsolidatedMark(Assess360Component.Assess360Id);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public bool SaveOrUpdateAssess360ComponentList(IList<Assess360Component> Assess360Component)
        {
            try
            {
                bool retValue;
                Assess360BC Assess360BC = new Assess360BC();
                retValue = Assess360BC.SaveOrUpdateAssess360ComponentList(Assess360Component);
                foreach (Assess360Component a360c in Assess360Component)
                {
                    this.StoreConsolidatedMark(a360c.Assess360Id);
                }
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long SaveOrUpdateAssess360(Assess360 Assess360)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.SaveOrUpdateAssess360(Assess360);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public string SaveOrUpdateAssess360RtnString(Assess360 Assess360)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                Assess360BC.SaveOrUpdateAssess360RtnString(Assess360);
                return Assess360.RequestNo;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Assess360Component GetAssess360Component(long Id)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetAssess360ComponentById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<Assess360Component>> GetAssess360ComponentListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetAssess360ComponentListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<Assess360Component>> GetAssess360ComponentListWithPagingAndInCriteria(int? page, int? pageSize, string sortby, string sortType, string name, int[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetAssess360ComponentListWithPagingAndInCriteria(page, pageSize, sortby, sortType, name, values, criteria, alias);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<Assess360>> GetAssess360ListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                Dictionary<long, IList<Assess360>> retValue = Assess360BC.GetAssess360ListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
                //this.StoreConsolidatedMark(retValue.Values.FirstOrDefault());
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<Assess360>> GetAssess360ListWithPagingAndCriteriaWithAlias(int? page, int? pageSize, string sortby, string sortType, string name, string[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                Dictionary<long, IList<Assess360>> retValue = Assess360BC.GetAssess360ListWithPagingAndCriteriaWithAlias(page, pageSize, sortby, sortType, name, values, criteria, alias);
                this.StoreConsolidatedMark(retValue.Values.FirstOrDefault());
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        private bool StoreConsolidatedMark(IList<Assess360> list)
        {
            bool retValue = false;
            if (list != null && list.Count > 0 && list.FirstOrDefault() != null)
            {
                string consMarks = "100/100";
                foreach (Assess360 a360 in list)
                {
                    consMarks = this.GetConsolidatedMarksForAStudent(a360.Id);
                    a360.ConsolidatedMarks = consMarks;
                    this.SaveOrUpdateAssess360(a360);
                    retValue = true;
                }
            }
            return retValue;
        }
        private bool StoreConsolidatedMark(long studId)
        {
            bool retValue = false;
            Assess360 Assess360 = this.GetAssess360ById(studId);
            if (Assess360 != null && Assess360.Id > 0)
            {
                string consMarks;// = "100/100";
                consMarks = this.GetConsolidatedMarksForAStudent(Assess360.Id);
                Assess360.ConsolidatedMarks = consMarks;
                this.SaveOrUpdateAssess360(Assess360);
                retValue = true;
            }
            return retValue;
        }
        public Dictionary<long, IList<AssessCompMaster>> GetAssess360CompMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetAssess360CompMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<AssessCompMaster>> GetAssess360CompMasterListWithPagingAndCriteriaWithIn(int? page, int? pageSize, string sortby, string sortType, string name, int[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetAssess360CompMasterListWithPagingAndCriteriaWithIn(page, pageSize, sortby, sortType, name, values, criteria, alias);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<StudentDetailsView>> GetStudentDetailsViewListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetStudentDetailsViewListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public IList<AssessCompMaster> GetAssess360CompMasterListByName(string AssessCompGroup)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetAssess360CompMasterListByName(AssessCompGroup);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<StaffMaster>> GetStaffMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetStaffMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<SubjectMaster>> GetSubjectMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetSubjectMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<AssessGroupMaster>> GetAssessGroupMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetAssessGroupMasterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public IList<WeeklyTestForStud> GetStudWeeklyTestForAssess360Id(long Assess360Id)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetStudWeeklyTestForAssess360Id(Assess360Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public IList<StudTermAssessment> GetStudTermAssessmentForAssess360Id(long Assess360Id)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetStudTermAssessmentForAssess360Id(Assess360Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public IList<StudSLCAssessment> GetStudSLCAssessmentForAssess360Id(long Assess360Id)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetStudSLCAssessmentForAssess360Id(Assess360Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public IList<StudHWAccuracy> GetStudHWAccuracyForAssess360Id(long Assess360Id)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetStudHWAccuracyForAssess360Id(Assess360Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public IList<StudCharacterAssessment> GetStudCharacterAssessmentForAssess360Id(long Assess360Id)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetStudCharacterAssessmentForAssess360Id(Assess360Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public IList<StudAttenPuncAssessment> GetStudAttenPuncAssessmentForAssess360Id(long Assess360Id)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetStudAttenPuncAssessmentForAssess360Id(Assess360Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public IList<StudHwCompletion> GetStudHwCompletionForAssess360Id(long Assess360Id)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetStudHwCompletionForAssess360Id(Assess360Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public IList<StudCopiedhomework> GetStudCopiedhomeworkForAssess360Id(long Assess360Id)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetStudCopiedhomeworkForAssess360Id(Assess360Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public IList<Assess360Point> GetAssess360PointForAcademicYear(string grade, string section, string campus, string academicYear)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetAssess360PointForAcademicYear(grade, section, campus, academicYear);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        //public string GetConsolidatedMarksForAStudent(long Assess360Id)
        //{
        //    try
        //    {
        //        Assess360BC Assess360BC = new Assess360BC();
        //        decimal? Consolidation = 0;
        //        //Character calculation part
        //        IList<StudCharacterAssessment> CharAssessment = Assess360BC.GetStudCharacterAssessmentForAssess360Id(Assess360Id);
        //        if (CharAssessment != null && CharAssessment.Count > 0)
        //        {
        //            Consolidation = CharAssessment.FirstOrDefault().FinalMark;
        //        }
        //        else Consolidation += 20;
        //        //Attendence and punctuality part
        //        IList<StudAttenPuncAssessment> AttnPuncAssessment = Assess360BC.GetStudAttenPuncAssessmentForAssess360Id(Assess360Id);
        //        if (AttnPuncAssessment != null && AttnPuncAssessment.Count > 0)
        //        {
        //            Consolidation += AttnPuncAssessment.FirstOrDefault().FinalMark;
        //        }
        //        else Consolidation += 10;
        //        //Homework completion part
        //        IList<StudHwCompletion> HWComplAssessment = Assess360BC.GetStudHwCompletionForAssess360Id(Assess360Id);
        //        if (HWComplAssessment != null && HWComplAssessment.Count > 0)
        //        {
        //            Consolidation += HWComplAssessment.FirstOrDefault().FinalMark;
        //        }
        //        else Consolidation += 5;
        //        //Homework accuracy part
        //        IList<StudHWAccuracy> HWAccuAssessment = Assess360BC.GetStudHWAccuracyForAssess360Id(Assess360Id);
        //        decimal HWAccuracyAvg;
        //        if (HWAccuAssessment != null && HWAccuAssessment.Count > 0)
        //        {
        //            //find average
        //            var a = from e in HWAccuAssessment select e.Mark;
        //            HWAccuracyAvg = Convert.ToDecimal(a.ToArray().Average());
        //            //if (Convert.ToInt16(HWAccuracyAvg) == 0)
        //            //{
        //            //    Consolidation += 15;
        //            //}
        //            //else 
        //            Consolidation += HWAccuracyAvg;
        //        }
        //        else Consolidation += 15;
        //        //Copied homework
        //        IList<StudCopiedhomework> StudCopiedhomework = Assess360BC.GetStudCopiedhomeworkForAssess360Id(Assess360Id);
        //        decimal copy;
        //        if (StudCopiedhomework != null && StudCopiedhomework.Count > 0)
        //        {
        //            //find average
        //            var a = from e in StudCopiedhomework select e.Mark;
        //            copy = Convert.ToDecimal(a.ToArray().Sum());
        //            if (Convert.ToInt16(copy) > 0)
        //            {
        //                Consolidation -= copy;
        //            }
        //        }
        //        //Weekly Chapter test part
        //        IList<WeeklyTestForStud> WeeklyTestAssessment = Assess360BC.GetStudWeeklyTestForAssess360Id(Assess360Id);
        //        if (WeeklyTestAssessment != null && WeeklyTestAssessment.Count > 0)
        //        {
        //            decimal weeklyTest;
        //            if (WeeklyTestAssessment != null && WeeklyTestAssessment.Count > 0)
        //            {
        //                //find average
        //                var a = from e in WeeklyTestAssessment select e.Mark;
        //                weeklyTest = Convert.ToDecimal(a.ToArray().Average());
        //                //if (Convert.ToInt16(weeklyTest) == 0)
        //                //{
        //                //    Consolidation += 20;
        //                //}
        //                //else 
        //                Consolidation += weeklyTest;
        //            }
        //        }
        //        else Consolidation += 20;
        //        //SLC parent Assessment
        //        IList<StudSLCAssessment> SLCTestAssessment = Assess360BC.GetStudSLCAssessmentForAssess360Id(Assess360Id);
        //        if (SLCTestAssessment != null && SLCTestAssessment.Count > 0)
        //        {
        //            Consolidation += SLCTestAssessment.FirstOrDefault().Mark;
        //        }
        //        else Consolidation += 5;
        //        //Term assessment
        //        IList<StudTermAssessment> StudTermAssessment = Assess360BC.GetStudTermAssessmentForAssess360Id(Assess360Id);
        //        if (StudTermAssessment != null && StudTermAssessment.Count > 0)
        //        {
        //            decimal TermAssess;
        //            if (StudTermAssessment != null && StudTermAssessment.Count > 0)
        //            {
        //                //find average
        //                var a = from e in StudTermAssessment select e.Mark;
        //                TermAssess = Convert.ToDecimal(a.ToArray().Average());
        //                //if (Convert.ToInt16(TermAssess) == 0)
        //                //{
        //                //    Consolidation += 25;
        //                //}
        //                //else 
        //                Consolidation += TermAssess;
        //            }
        //        }
        //        else Consolidation += 25;
        //        return "" + Consolidation.Value.ToString("0.00") + " / 100";
        //        //return Assess360BC.GetConsolidatedMarksForAStudent(Assess360Id);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally { }
        //}

        public string GetConsolidatedMarksForAStudent(long Assess360Id)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                decimal? Consolidation = 0;
                IList consVal = Assess360BC.Assess360MarkCalculationForAStudent(Assess360Id);
                if (consVal[0] != null)
                    Consolidation = Convert.ToDecimal(consVal[0]);

                return "" + Consolidation.Value.ToString("0.00") + " / 100";
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public string GetComponentWiseConsolidatedMarksForAStudent(long Assess360Id)
        {
            Assess360BC Assess360BC = new Assess360BC();
            decimal? CharAssess = 0;
            decimal? AttnPunAssess = 0;
            decimal? HWComplAssess = 0;
            decimal? HWAccuracyAssess = 0;
            decimal? WeeklyAssess = 0;
            decimal? SLCAssess = 0;
            decimal? TermAssess = 0;
            decimal? Consolidation = 0;
            //Character calculation part
            IList<StudCharacterAssessment> CharAssessment = Assess360BC.GetStudCharacterAssessmentForAssess360Id(Assess360Id);
            if (CharAssessment != null && CharAssessment.Count > 0)
            {
                CharAssess = CharAssessment.FirstOrDefault().FinalMark;
            }
            else CharAssess = 20;
            Consolidation = CharAssess;
            //Attendence and punctuality part
            IList<StudAttenPuncAssessment> AttnPuncAssessment = Assess360BC.GetStudAttenPuncAssessmentForAssess360Id(Assess360Id);
            if (AttnPuncAssessment != null && AttnPuncAssessment.Count > 0)
            {
                AttnPunAssess = AttnPuncAssessment.FirstOrDefault().FinalMark;
            }
            else AttnPunAssess = 10;
            Consolidation += AttnPunAssess;
            //Homework completion part
            IList<StudHwCompletion> HWComplAssessment = Assess360BC.GetStudHwCompletionForAssess360Id(Assess360Id);
            if (HWComplAssessment != null && HWComplAssessment.Count > 0)
            {
                HWComplAssess = HWComplAssessment.FirstOrDefault().FinalMark;
            }
            else HWComplAssess = 5;
            Consolidation += HWComplAssess;
            //Homework accuracy part
            IList<StudHWAccuracy> HWAccuAssessment = Assess360BC.GetStudHWAccuracyForAssess360Id(Assess360Id);
            decimal HWAccuracyAvg;
            if (HWAccuAssessment != null && HWAccuAssessment.Count > 0)
            {
                //find average
                var a = from e in HWAccuAssessment select e.Mark;
                HWAccuracyAvg = Convert.ToDecimal(a.ToArray().Average());
                //if (Convert.ToInt16(HWAccuracyAvg) == 0)
                //{
                //    HWAccuracyAssess = 15;
                //}
                //else 
                HWAccuracyAssess = HWAccuracyAvg;
            }
            else HWAccuracyAssess = 15;
            //Consolidation += HWAccuracyAssess;
            //data will be assigned in the below copied homework calculation
            //Copied homework
            IList<StudCopiedhomework> StudCopiedhomework = Assess360BC.GetStudCopiedhomeworkForAssess360Id(Assess360Id);
            decimal copy;
            if (StudCopiedhomework != null && StudCopiedhomework.Count > 0)
            {
                //find average
                var a = from e in StudCopiedhomework select e.Mark;
                copy = Convert.ToDecimal(a.ToArray().Sum());
                if (Convert.ToInt16(copy) > 0)
                {
                    HWAccuracyAssess -= copy;
                }
            }
            Consolidation += HWAccuracyAssess;
            //Weekly Chapter test part
            IList<WeeklyTestForStud> WeeklyTestAssessment = Assess360BC.GetStudWeeklyTestForAssess360Id(Assess360Id);
            if (WeeklyTestAssessment != null && WeeklyTestAssessment.Count > 0)
            {
                decimal weeklyTest;
                if (WeeklyTestAssessment != null && WeeklyTestAssessment.Count > 0)
                {
                    //find average
                    var a = from e in WeeklyTestAssessment select e.Mark;
                    weeklyTest = Convert.ToDecimal(a.ToArray().Average());
                    //if (Convert.ToInt16(weeklyTest) == 0)
                    //{
                    //    WeeklyAssess = 20;
                    //}
                    //else 
                    WeeklyAssess = weeklyTest;
                }
            }
            else WeeklyAssess = 20;
            Consolidation += WeeklyAssess;
            //SLC parent Assessment
            IList<StudSLCAssessment> SLCTestAssessment = Assess360BC.GetStudSLCAssessmentForAssess360Id(Assess360Id);
            if (SLCTestAssessment != null && SLCTestAssessment.Count > 0)
            {
                SLCAssess = SLCTestAssessment.FirstOrDefault().Mark;
            }
            else SLCAssess = 5;
            Consolidation += SLCAssess;
            //Term assessment
            IList<StudTermAssessment> StudTermAssessment = Assess360BC.GetStudTermAssessmentForAssess360Id(Assess360Id);
            if (StudTermAssessment != null && StudTermAssessment.Count > 0)
            {
                decimal TermAssessTemp;
                if (StudTermAssessment != null && StudTermAssessment.Count > 0)
                {
                    //find average
                    var a = from e in StudTermAssessment select e.Mark;
                    TermAssessTemp = Convert.ToDecimal(a.ToArray().Average());
                    //if (Convert.ToInt16(TermAssessTemp) == 0)
                    //{
                    //    TermAssess = 25;
                    //}
                    //else 
                    TermAssess = TermAssessTemp;
                }
            }
            else TermAssess = 25;
            Consolidation += TermAssess;
            //return "";
            return "" + Consolidation.Value.ToString("0.00") + " / 100," + CharAssess.Value.ToString("0.00") + "," + AttnPunAssess.Value.ToString("0.00") + "," + HWComplAssess.Value.ToString("0.00") + "," + HWAccuracyAssess.Value.ToString("0.00") + "," + WeeklyAssess.Value.ToString("0.00") + "," + SLCAssess.Value.ToString("0.00") + "," + TermAssess.Value.ToString("0.00") + "";
        }
        public long GetPreRegNumForStudentId(long Id)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetPreRegNumForStudentId(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public string GetCampusForStudentId(long Id)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetCampusForStudentId(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public bool DeleteAssess360Component(long[] Assess360ComponentIds)
        {
            try
            {
                bool retValue = false;
                Assess360BC Assess360BC = new Assess360BC();
                retValue = Assess360BC.DeleteAssess360Component(Assess360ComponentIds);
                foreach (long id in Assess360ComponentIds)
                {
                    this.StoreConsolidatedMark(id);
                }
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }


        public Dictionary<long, IList<Assess360AssessmentSubgrids>> GetAssessListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetAssessListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<Assess360AssessmentStaffNames>> GetStaffNamesBasedOnAssessGroupIdListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetStaffNamesBasedOnAssessGroupIdListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<Assess360Assignment>> GetAssess360AssignmentListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetAssess360AssignmentListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<NotEnteredCount>> GetNotEnteredAssignmentListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetNotEnteredAssignmentListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public DataTable NotEnteredStudentList(string strQry1)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.NotEnteredStudentList(strQry1);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public DataTable EnteredStudentList(string strQry1)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.EnteredStudentList(strQry1);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<Assess360BulkInsert>> GetAssess360BulkInsertListWithPagingAndCriteriaWithAlias(int? page, int? pageSize, string sortby, string sortType, string name, string[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                Dictionary<long, IList<Assess360BulkInsert>> retValue = Assess360BC.GetAssess360BulkInsertListWithPagingAndCriteriaWithAlias(page, pageSize, sortby, sortType, criteria, name, values, alias);
                // Dictionary<long, IList<Assess360BulkInsert>> retValue = Assess360BC.GetAssess360BulkInsertListWithPagingAndCriteriaWithAlias(page, pageSize, sortby, sortType, criteria, name, values, alias);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<Assess360BulkInsert>> GetAssess360BulkInsertListWithPagingAndCriteriaWithAliasNew(string[] columns, object[] values)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                PSFHelperClassBC PSFHelperClassBC = new Component.PSFHelperClassBC();
                return PSFHelperClassBC.GetAssess360BulkInsertListWithHQLQuery(columns, values);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<Assess360BulkInsert>> GetAssess360BulkInsertListWithPagingAndCriteriaWithAliasNewIBMain(string[] columns, object[] values)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                PSFHelperClassBC PSFHelperClassBC = new Component.PSFHelperClassBC();
                return PSFHelperClassBC.GetAssess360BulkInsertListWithHQLQueryforIBMain(columns, values);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        #region start new subject based semester marks

        public Dictionary<long, IList<SubjectStudentTemplate>> GetSubjectMarksViewListWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetSubjectMarksViewListWithCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<SubjectMarks>> GetSubjectMarksViewListWithCriteria1(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetSubjectMarksViewListWithCriteria1(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<Totalsemlist>> GetTotalSemListWithCriteria1(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetTotalSemListWithCriteria1(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public SubjectMarks CheckExistdatainSubjectMarks(long Id)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.CheckExistdatainSubjectMarks(Id);
            }
            catch (Exception ex) { throw; }
            finally { }
        }

        public long CreateOrUpdateSubjectMarks(SubjectMarks fr)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.CreateOrUpdateSubjectMarks(fr);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<StudentFinalResult_vw>> GetStudentFinalResultWidthSubjectWiseList(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetStudentFinalResultWidthSubjectWiseList(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<Vw_FinalResult>> GetFinalResultsListWidthCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {

                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetFinalResultsListWidthCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<StaffsNamesForSubjectMarks>> GetStaffsNamesForSubjectMarksListWithCriteria1(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetStaffsNamesForSubjectMarksListWithCriteria1(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion end new subject based semester marks

        #region test
        public Dictionary<long, IList<ClassforIXAB>> GetClassIXABSubjectListWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetClassIXABSubjectListWithCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<ClassforVItoVIII>> GetClassVItoVIIISubjectListWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetClassVItoVIIISubjectListWithCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<ClassforIXCD>> GetClassIXCDSubjectListWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetClassIXCDSubjectListWithCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion end test

        public Assess360 GetAssess360ByStudentId(long StudentId)
        {
            try
            {
                if (StudentId > 0)
                {
                    Assess360BC aBC = new Assess360BC();
                    return aBC.GetAssess360ByStudentId(StudentId);
                }
                else throw new Exception("IdNo is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        #region Ass360 Request Creation
        public Dictionary<long, IList<StudTempForAss360ReqCreation>> GetStudTemplateListWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetStudTemplateListWithCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public bool CreateOrUpdateAssess360List(Assess360 ass360)
        {
            bool retValue = false;
            if (ass360 != null)
            {
                Assess360BC aBC = new Assess360BC();
                aBC.CreateOrUpdateAssess360List(ass360);
                retValue = true;
            }
            return retValue;
        }
        #endregion
        #region Show Past Year Subject Marks
        public Dictionary<long, IList<ShowPastYearSubjectMarks>> GetMarklistfromSubjectMarksWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetMarklistfromSubjectMarksWithCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<ShowPastYearFinalResults_Vw>> GetShowPastYearFinalResults_VwListWithCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetShowPastYearFinalResults_VwListWithCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        
        #endregion


        #region Newly added by Micheal
        public Dictionary<long, IList<Assess360>> GetAssessDetailsListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC aBC = new Assess360BC();
                return aBC.GetAssessDetailsListWithPaging(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        #endregion

        public Dictionary<long, IList<NotUsedAssignmentList>> GetNotUsedAssignmentListWithPagingAndCriteriaEqSearch(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetNotUsedAssignmentListWithPagingAndCriteriaEqSearch(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<NotUsedAssignmentList>> GetNotUsedAssignmentListWithPagingAndCriteriaLikeSearch(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetNotUsedAssignmentListWithPagingAndCriteriaLikeSearch(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        #region For Admin Template
        public Dictionary<long, IList<Assess360AdminTemplate_vw>> GetAssess360AdminTemplate_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetAssess360AdminTemplate_vwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion

        public Assess360_ARC GetAssess360_ARCById(long Id)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetAssess360_ARCById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<Assess360_ARC>> GetAssess360_ARCListWithPagingAndCriteriaWithAlias(int? page, int? pageSize, string sortby, string sortType, string name, string[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                Dictionary<long, IList<Assess360_ARC>> retValue = Assess360BC.GetAssess360_ARCListWithPagingAndCriteriaWithAlias(page, pageSize, sortby, sortType, name, values, criteria, alias);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        #region "Semester Master"

        public Dictionary<long, IList<SemesterMaster>> GetSemesterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC AssessBc = new Assess360BC();
                return AssessBc.GetSemesterListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long SaveOrUpdateSemester(SemesterMaster semester)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                Assess360BC.SaveOrUpdateSemester(semester);
                return semester.SemesterMasterId;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public SemesterMaster GetSemesterById(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    Assess360BC Assess360BC = new Assess360BC();
                    return Assess360BC.GetSemesterById(Id);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long DeleteSemester(SemesterMaster semester)
        {
            try
            {
                if (semester != null)
                {
                    Assess360BC Assess360BC = new Assess360BC();
                    Assess360BC.DeleteSemester(semester);
                    return semester.SemesterMasterId;
                }
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        #endregion

        public long SaveOrUpdateReportCardAchievement(RptCardAchievement RptAchieve)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.SaveOrUpdateReportCardAchievement(RptAchieve);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<RptCardAchievement>> GetStudentAchievementListWithPagingAndInCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                Dictionary<long, IList<RptCardAchievement>> retValue = Assess360BC.GetStudentAchievementListWithPagingAndInCriteria(page, pageSize, sortby, sortType, criteria);
                //this.StoreConsolidatedMark(retValue.Values.FirstOrDefault());
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public bool DeleteStudentAchievement(long[] studAchieveIds)
        {
            try
            {
                bool retValue = false;
                Assess360BC Assess360BC = new Assess360BC();
                retValue = Assess360BC.DeleteStudentAchievement(studAchieveIds);
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<RptCardMarkWeightingMaster>> GetReportCardMarkWeightingsWihtCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                Dictionary<long, IList<RptCardMarkWeightingMaster>> retValue = Assess360BC.GetReportCardMarkWeightingsWihtCriteria(page, pageSize, sortType, sortby, criteria);
                //this.StoreConsolidatedMark(retValue.Values.FirstOrDefault());
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<WeightingMasterCrossCheck_Vw>> GetWeightingsMasterCrossCheckListWihtCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                Dictionary<long, IList<WeightingMasterCrossCheck_Vw>> retValue = Assess360BC.GetWeightingsMasterCrossCheckListWihtCriteria(page, pageSize, sortby, sortType, criteria);
                //this.StoreConsolidatedMark(retValue.Values.FirstOrDefault());
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }


        public long SaveOrUpdateRptCardMarkWeightingMaster(RptCardMarkWeightingMaster rptcardWeighting)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                Assess360BC.SaveOrUpdateRptCardMarkWeightingMaster(rptcardWeighting);
                return rptcardWeighting.Id;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public RptCardMarkWeightingMaster GetRptCardMarkWeightingMasterById(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    Assess360BC Assess360BC = new Assess360BC();
                    return Assess360BC.GetRptCardMarkWeightingMasterById(Id);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long DeleteRptCardMarkWeightingMaster(RptCardMarkWeightingMaster rptWeighting)
        {
            try
            {
                if (rptWeighting != null)
                {
                    Assess360BC Assess360BC = new Assess360BC();
                    Assess360BC.DeleteRptCardMarkWeightingMaster(rptWeighting);
                    return rptWeighting.Id;
                }
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public decimal GetAttendenceandpunctualityandHomeworkcompletionMarks(long Assess360Id, string Status)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                decimal? AttnPunAssess = 0;
                decimal? HWComplAssess = 0;
                decimal? Consolidation = 0;

                //Attendence and punctuality part
                IList<StudAttenPuncAssessment> AttnPuncAssessment = Assess360BC.GetStudAttenPuncAssessmentForAssess360Id(Assess360Id);
                if (AttnPuncAssessment != null && AttnPuncAssessment.Count > 0)
                {
                    AttnPunAssess = AttnPuncAssessment.FirstOrDefault().FinalMark;
                }
                else AttnPunAssess = 10;
                Consolidation += AttnPunAssess;
                if (Status == "Both")
                {
                    //Homework completion part
                    IList<StudHwCompletion> HWComplAssessment = Assess360BC.GetStudHwCompletionForAssess360Id(Assess360Id);
                    if (HWComplAssessment != null && HWComplAssessment.Count > 0)
                    {
                        HWComplAssess = HWComplAssessment.FirstOrDefault().FinalMark;
                    }
                    else HWComplAssess = 5;
                    Consolidation += HWComplAssess;
                }
                return Consolidation ?? 0;

            }
            catch (Exception)
            {

                throw;
            }
        }

        #region "Rotativa Pdf"
        public Dictionary<long, IList<Assess360MarksCaluctionForAStudent_Vw>> GetAssess360MarksCaluctionForAStudent_VwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetAssess360MarksCaluctionForAStudent_VwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<Assess360MarkcalculationForChart_Vw>> GetAssess360MarkcalculationForChart_VwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetAssess360MarkcalculationForChart_VwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<IXIGCSEWeightingsCalculation_Vw>> GetAssess360WeightingsMarksCaluctionForAStudent_VwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetAssess360WeightingsMarksCaluctionForAStudent_VwListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion "End"

        public StudentTemplate GetStudentTemplateById(long StudentId)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                return Assess360BC.GetStudentTemplateById(StudentId);
            }
            catch (Exception)
            {

                throw;
            }
            throw new NotImplementedException();
        }


        #region "Xth IB ReportCard"
        public Dictionary<long, IList<SummativeAssessment_vw>> GetSummativeAssessmenttestResultList(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return a360BCobj.GetSummativeAssessmenttestResultList(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<SummativeAssessmentVIVII_vw>> GetSummativeAssessmenttestVIVIIResultList(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return a360BCobj.GetSummativeAssessmenttestVIVIIResultList(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<ComparitiveSAReport_vw>> GetComparativeSAReportResultList(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return a360BCobj.GetComparativeSAReportResultList(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<SummativeMarkAnalysisIX_Vw>> GetsummativeMarkAnalysisListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return a360BCobj.GetSummativeMarkAnalysisWithCriteria(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Dictionary<long, IList<SummativeMarkAnalysisVIVIII_Vw>> GetsummativeMarkAnalysisListVIVIII(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return a360BCobj.GetSummativeMarkAnalysisVIVIII(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<SumativeStudentDetails_Vw>> GetSumativeStudentListWithPagingAndInCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                return a360BCobj.GetSumativeStudentListWithPagingAndInCriteria(page, pageSize, sortBy, sortType, criteria, likecriteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<SumativeStudentDetailsIX_Vw>> GetSumativeStudentIXListWithPagingAndInCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                return a360BCobj.GetSumativeStudentListIXWithPagingAndInCriteria(page, pageSize, sortBy, sortType, criteria, likecriteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        #endregion

        public AssessCompMaster GetAssessGroupUsingId(int AssessGroup)
        {
            try
            {
                if (AssessGroup > 0)
                {
                    return a360BCobj.GetAssessGroupUsingId(AssessGroup);
                }
                else throw new Exception("IdNo is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public Dictionary<long, IList<FormattiveAssessment_Vw>> GetFAMarkListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                Dictionary<long, IList<FormattiveAssessment_Vw>> retValue = Assess360BC.GetFAMarkListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
                //this.StoreConsolidatedMark(retValue.Values.FirstOrDefault());
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<ComparitiveFAReport_vw>> GetComparativeFAReportResultList(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return a360BCobj.GetComparativeFAReportResultList(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<FA_HW_Vw>> GetFAHWMarkListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Assess360BC Assess360BC = new Assess360BC();
                Dictionary<long, IList<FA_HW_Vw>> retValue = Assess360BC.GetFAHWMarkListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
                //this.StoreConsolidatedMark(retValue.Values.FirstOrDefault());
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<ComparativeFAHWReport_Vw>> GetComparativeFAHWReportResultList(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return a360BCobj.GetComparativeFAHWReportResultList(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        
    }
}
