using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersistenceFactory;
using TIPS.Entities.TimeTableEntities;

namespace TIPS.Component
{
    public class TimeTableBC
    {
        PersistenceServiceFactory PSF = null;
        public TimeTableBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }
        //public long SaveOrUpdateTimeTable(TimeTables TimeTable)
        //{
        //    try
        //    {
        //        if (TimeTable != null)
        //            PSF.SaveOrUpdate<TimeTables>(TimeTable);
        //        else { throw new Exception("AcademicMaster is required and it cannot be null.."); }
        //        return TimeTable.TimeTable_Id;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        //public bool DeleteTimeTable(long TimeTable_Id)
        //{
        //    try
        //    {
        //        TimeTables TimeTables = PSF.Get<TimeTables>(TimeTable_Id);
        //        PSF.Delete<TimeTables>(TimeTables);
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //public bool DeleteTimeTable(long[] TimeTable_Id)
        //{
        //    try
        //    {
        //        IList<TimeTables> TimeTables = PSF.GetListByIds<TimeTables>(TimeTable_Id);
        //        PSF.DeleteAll<TimeTables>(TimeTables);
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //public Dictionary<long, IList<TimeTables>> GetTimeTablesListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        //{
        //    try
        //    {
        //        return PSF.GetListWithExactSearchCriteriaCount<TimeTables>(page, pageSize, sortType, sortby, criteria);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //public TimeTables GetClassRoomMasterById(long TimeTable_Id)
        //{
        //    try
        //    {
        //        return PSF.Get<TimeTables>("TimeTable_Id", TimeTable_Id);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public Dictionary<long, IList<tt_timetable>> Get_tt_timetableListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<tt_timetable>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public tt_timetable Get_tt_timetableById(long timetable_id)
        {
            try
            {
                return PSF.Get<tt_timetable>("timetable_id", timetable_id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long SaveOrUpdateTimeTable(tt_timetable att)
        {
            try
            {
                if (att != null)
                    PSF.SaveOrUpdate<tt_timetable>(att);
                else { throw new Exception("Value is required and it cannot be null.."); }
                return att.timetable_id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public PeriodTimeMaster getPeriodTimeMasterByGradeandPeriodNumber(long periodNumber, string Grade)
        {
            try
            {

                PeriodTimeMaster periodTimeMaster = null;
                if (periodNumber > 0)
                    periodTimeMaster = PSF.Get<PeriodTimeMaster>("Period_Number", periodNumber, "Grade", Grade);
                else { throw new Exception("Period_Number is required and it cannot be 0"); }
                return periodTimeMaster;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public tt_SubjectMaster getSubjectMasterById(long subject_id)
        {
            try
            {

                tt_SubjectMaster subjectMaster = null;
                if (subject_id > 0)
                    subjectMaster = PSF.Get<tt_SubjectMaster>("subject_id", subject_id);
                else { throw new Exception("subject_id is required and it cannot be 0"); }
                return subjectMaster;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<tt_days>> Get_tt_days_ListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<tt_days>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public tt_days get_tt_days_By_day_id(long day_id)
        {
            try
            {

                tt_days tt_days = null;
                if (day_id > 0)
                    tt_days = PSF.Get<tt_days>("day_id", day_id);
                else { throw new Exception("tt_days is required and it cannot be 0"); }
                return tt_days;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long SaveOrUpdate_tt_Search(tt_Search search)
        {
            try
            {
                if (search != null)
                    PSF.SaveOrUpdate<tt_Search>(search);
                else { throw new Exception("Null"); }
                return search.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public tt_Search get_tt_SearchById(long Id)
        {
            try
            {

                tt_Search tt_Search = null;
                if (Id > 0)
                    tt_Search = PSF.Get<tt_Search>("Id", Id);
                else { throw new Exception("tt_Search is required and it cannot be 0"); }
                return tt_Search;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public tt_division get_tt_divisionByCampusGrdSec(string Campus, string Grade, string Section)
        {
            try
            {

                tt_division tt_division = null;
                if (!string.IsNullOrEmpty(Campus) && !string.IsNullOrEmpty(Grade) && !string.IsNullOrEmpty(Section))
                    tt_division = PSF.Get<tt_division>("Campus", Campus, "Grade", Grade, "Section", Section);
                else { throw new Exception("tt_division is required and it cannot be 0"); }
                return tt_division;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public tt_allotment get_tt_allotmentByDivision_IdAndCharge(long division_id)
        {
            try
            {

                tt_allotment tt_allotment = null;
                if (division_id > 0)
                    tt_allotment = PSF.Get<tt_allotment>("division_id", division_id, "charge", true);
                else { throw new Exception("tt_allotment is required and it cannot be 0"); }
                return tt_allotment;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public tt_teacher get_tt_teacherByteacher_id(long teacher_id)
        {
            try
            {

                tt_teacher tt_teacher = null;
                if (teacher_id > 0)
                    tt_teacher = PSF.Get<tt_teacher>("teacher_id", teacher_id);
                else { throw new Exception("tt_teacher is required and it cannot be 0"); }
                return tt_teacher;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<tt_allotment>> Get_tt_allotment_ListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<tt_allotment>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public tt_SubjectMaster get_tt_SubjectMasterBysubject_id(long subject_id)
        {
            try
            {

                tt_SubjectMaster subject = null;
                if (subject_id > 0)
                    subject = PSF.Get<tt_SubjectMaster>("subject_id", subject_id);
                else { throw new Exception("tt_SubjectMaster is required and it cannot be 0"); }
                return subject;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public tt_division get_tt_divisionById(long division_id)
        {
            try
            {

                tt_division division = null;
                if (division_id > 0)
                    division = PSF.Get<tt_division>("division_id", division_id);
                else { throw new Exception("division_id is required and it cannot be 0"); }
                return division;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<tt_teacher>> Get_tt_teacherListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<tt_teacher>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<tt_SubjectMaster>> GetSubjectMasterListWithPagingAndCriteriaLikeSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<tt_SubjectMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<tt_SubjectMaster>> GetValidSubjectWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<tt_SubjectMaster>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<tt_teacher>> GetTeacherListWithPagingAndCriteriaLikeSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<tt_teacher>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<tt_teacher>> GetValidTeacherWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<tt_teacher>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tt_division GetDivisionMasterByCampusGradeandSection(string Campus, string Grade, string Section)
        {
            try
            {

                tt_division divisionMaster = null;
                if (!string.IsNullOrEmpty(Campus) && !string.IsNullOrEmpty(Grade) && !string.IsNullOrEmpty(Section))
                    divisionMaster = PSF.Get<tt_division>("Campus", Campus, "Grade", Grade, "Section", Section);
                else { throw new Exception("Period_Number is required and it cannot be 0"); }
                return divisionMaster;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public tt_SubjectMaster GetSubjectMasterBySubject(string Subject)
        {
            try
            {

                tt_SubjectMaster subjectMaster = null;
                if (!string.IsNullOrEmpty(Subject))
                    subjectMaster = PSF.Get<tt_SubjectMaster>("subject_name", Subject);
                else { throw new Exception("subject_name is required and it cannot be 0"); }
                return subjectMaster;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public tt_teacher GetTeacherMasterByStaffName(string staff)
        {
            try
            {

                tt_teacher teacherMstr = null;
                if (!string.IsNullOrEmpty(staff))
                    teacherMstr = PSF.Get<tt_teacher>("teacher_name", staff);
                else { throw new Exception("subject_name is required and it cannot be 0"); }
                return teacherMstr;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<tt_days>> GetDaysMasterListWithPagingAndCriteriaSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<tt_days>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<PeriodTimeMaster>> GetPeriodTimeMasterListWithPagingAndCriteriaSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<PeriodTimeMaster>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tt_allotment GetAllotmentByDivSubjandTecherIds(long div, long sub)
        {
            try
            {

                tt_allotment allotment = null;
                if (div > 0 && sub > 0)
                    allotment = PSF.Get<tt_allotment>("division_id", div, "subject_id", sub);
                else { throw new Exception("division and subject ids are required and it cannot be 0"); }
                return allotment;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<tt_timetable>> GetAllotmentMasterListWithPagingAndCriteriaSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<tt_timetable>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tt_allotment GetAllotmentMasterById(long Id)
        {
            try
            {

                tt_allotment allotment = null;
                if (Id > 0)
                    allotment = PSF.Get<tt_allotment>("allotment_id", Id);
                else { throw new Exception("Allotment Id required and it cannot be 0"); }
                return allotment;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long SaveOrUpdateAllotment(tt_allotment allotment)
        {
            try
            {
                if (allotment != null)
                    PSF.SaveOrUpdate<tt_allotment>(allotment);
                else { throw new Exception("Null"); }
                return allotment.allotment_id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool DeleteTimeTableDetails(long id)
        {
            try
            {
                tt_timetable timetableDetls = PSF.Get<tt_timetable>(id);
                PSF.Delete<tt_timetable>(timetableDetls);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tt_teacher getTeacherDetailsHtmlById(long Id)
        {
            try
            {

                tt_teacher tt_teacher = null;
                if (Id > 0)
                    tt_teacher = PSF.Get<tt_teacher>("teacher_id", Id);
                else { throw new Exception("Teacher Id required and it cannot be 0"); }
                return tt_teacher;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public TimeTable GetTimeTableByCamGraSec(string Campus, string Grade, string Section)
        {
            try
            {

                TimeTable tt_division = null;
                if (!string.IsNullOrEmpty(Campus) && !string.IsNullOrEmpty(Grade) && !string.IsNullOrEmpty(Section))
                    tt_division = PSF.Get<TimeTable>("Campus", Campus, "Grade", Grade, "Section", Section);
                else { throw new Exception("tt_division is required and it cannot be 0"); }
                return tt_division;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<TimeTable>> GetTimeTableListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<TimeTable>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<TTSubjectMaster>> GetTTSubjectWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<TTSubjectMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateTTSubjectMaster(TTSubjectMaster TTS)
        {
            try
            {
                if (TTS != null)
                    PSF.SaveOrUpdate<TTSubjectMaster>(TTS);
                else { throw new Exception("VehicleTypeMaster is required and it cannot be null.."); }
                return TTS.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<TTStaffMaster>> GetTTStaffWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<TTStaffMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateTTStaffMaster(TTStaffMaster TTS)
        {
            try
            {
                if (TTS != null)
                    PSF.SaveOrUpdate<TTStaffMaster>(TTS);
                else { throw new Exception("VehicleTypeMaster is required and it cannot be null.."); }
                return TTS.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long CreateOrUpdateTTStaffConfig(TTStaffConfig TTConfig)
        {
            try
            {
                if (TTConfig != null)
                    PSF.SaveOrUpdate<TTStaffConfig>(TTConfig);
                else { throw new Exception("VehicleTypeMaster is required and it cannot be null.."); }
                return TTConfig.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<TTStaffConfig>> GetTTStaffConfigWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<TTStaffConfig>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

       
    }
}
