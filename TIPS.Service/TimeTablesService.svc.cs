using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.Component;
using TIPS.Entities.TimeTableEntities;

namespace TIPS.ServiceContract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TimeTablesService" in code, svc and config file together.
    public class TimeTablesService : ITimeTablesSC
    {
        TimeTableBC TimeTableBC = new TimeTableBC();
        //public long SaveOrUpdateTimeTable(TimeTables TimeTable)
        //{
        //    try
        //    {
        //        TimeTableBC.SaveOrUpdateTimeTable(TimeTable);
        //        return TimeTable.TimeTable_Id;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally { }

        //}
        //public bool DeleteTimeTable(long TimeTable_Id)
        //{
        //    try
        //    {
        //        TimeTableBC.DeleteTimeTable(TimeTable_Id);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally { }
        //}
        //public bool DeleteTimeTable(long[] TimeTable_Id)
        //{
        //    try
        //    {
        //        TimeTableBC.DeleteTimeTable(TimeTable_Id);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally { }
        //}
        //public Dictionary<long, IList<TimeTables>> GetTimeTablesListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        //{
        //    try
        //    {
        //        return TimeTableBC.GetTimeTablesListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
        //        TimeTables TimeTables = new TimeTables();
        //        TimeTables = TimeTableBC.GetClassRoomMasterById(TimeTable_Id);
        //        return TimeTables;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally { }
        //}

        public Dictionary<long, IList<tt_timetable>> Get_tt_timetableListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return TimeTableBC.Get_tt_timetableListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                tt_timetable tt_timetable = new tt_timetable();
                tt_timetable = TimeTableBC.Get_tt_timetableById(timetable_id);
                return tt_timetable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public long SaveOrUpdateTimeTable(tt_timetable ttable)
        {
            try
            {
                TimeTableBC.SaveOrUpdateTimeTable(ttable);
                return ttable.timetable_id;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public PeriodTimeMaster getPeriodTimeMasterByGradeandPeriodNumber(long periodNumber, string Grade)
        {
            try
            {
                return TimeTableBC.getPeriodTimeMasterByGradeandPeriodNumber(periodNumber, Grade);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public tt_SubjectMaster getSubjectMasterById(long subjectId)
        {
            try
            {
                return TimeTableBC.getSubjectMasterById(subjectId);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<tt_days>> Get_tt_days_ListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return TimeTableBC.Get_tt_days_ListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                return TimeTableBC.get_tt_days_By_day_id(day_id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long SaveOrUpdate_tt_Search(tt_Search search)
        {
            try
            {

                TimeTableBC.SaveOrUpdate_tt_Search(search);
                return search.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public tt_Search get_tt_SearchById(long Id)
        {
            try
            {
                return TimeTableBC.get_tt_SearchById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public tt_division get_tt_divisionByCampusGrdSec(string Campus, string Grade, string Section)
        {
            try
            {
                return TimeTableBC.get_tt_divisionByCampusGrdSec(Campus, Grade, Section);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public tt_allotment get_tt_allotmentByDivision_IdAndCharge(long division_id)
        {
            try
            {
                return TimeTableBC.get_tt_allotmentByDivision_IdAndCharge(division_id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public tt_teacher get_tt_teacherByteacher_id(long teacher_id)
        {
            try
            {
                return TimeTableBC.get_tt_teacherByteacher_id(teacher_id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<tt_allotment>> Get_tt_allotment_ListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return TimeTableBC.Get_tt_allotment_ListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                return TimeTableBC.get_tt_SubjectMasterBysubject_id(subject_id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public tt_division get_tt_divisionById(long division_id)
        {
            try
            {
                return TimeTableBC.get_tt_divisionById(division_id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<tt_teacher>> Get_tt_teacherListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return TimeTableBC.Get_tt_teacherListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                return TimeTableBC.GetSubjectMasterListWithPagingAndCriteriaLikeSearch(page, pageSize, sortby, sortType, criteria);
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
                return TimeTableBC.GetValidSubjectWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<tt_teacher>> GetTeacherMasterListWithPagingAndCriteriaLikeSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return TimeTableBC.GetTeacherListWithPagingAndCriteriaLikeSearch(page, pageSize, sortby, sortType, criteria);
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
                return TimeTableBC.GetValidTeacherWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public tt_division GetDivisionMasterByCampusGradeandSection(string Campus, string Grade, string Section)
        {
            try
            {
                return TimeTableBC.GetDivisionMasterByCampusGradeandSection(Campus, Grade, Section);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public tt_SubjectMaster GetSubjectMasterBySubject(string Subject)
        {
            try
            {
                return TimeTableBC.GetSubjectMasterBySubject(Subject);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public tt_teacher GetTeacherMasterByStaffName(string staff)
        {
            try
            {
                return TimeTableBC.GetTeacherMasterByStaffName(staff);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<tt_days>> GetDaysMasterListWithPagingAndCriteriaSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return TimeTableBC.GetDaysMasterListWithPagingAndCriteriaSearch(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<PeriodTimeMaster>> GetPeriodTimeMasterListWithPagingAndCriteriaSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return TimeTableBC.GetPeriodTimeMasterListWithPagingAndCriteriaSearch(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public tt_allotment GetAllotmentByDivSubjandTecherIds(long div, long sub)
        {
            try
            {
                return TimeTableBC.GetAllotmentByDivSubjandTecherIds(div, sub);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<tt_timetable>> GetAllotmentMasterListWithPagingAndCriteriaSearch(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return TimeTableBC.GetAllotmentMasterListWithPagingAndCriteriaSearch(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public tt_allotment GetAllotmentMasterById(long Id)
        {
            try
            {
                return TimeTableBC.GetAllotmentMasterById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long SaveOrUpdateAllotment(tt_allotment allotment)
        {
            try
            {
                TimeTableBC.SaveOrUpdateAllotment(allotment);
                return allotment.allotment_id;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public bool DeleteTimeTableDetails(long id)
        {
            try
            {
                TimeTableBC.DeleteTimeTableDetails(id);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }
        public tt_teacher getTeacherDetailsHtmlById(long StaffId)
        {
            try
            {
                tt_teacher tt_teacher = new tt_teacher();
                tt_teacher = TimeTableBC.getTeacherDetailsHtmlById(StaffId);
                return tt_teacher;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public TimeTable GetTimeTableByCamGraSec(string Campus, string Grade, string Section)
        {
            try
            {
                return TimeTableBC.GetTimeTableByCamGraSec(Campus, Grade, Section);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<TimeTable>> GetTimeTableListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return TimeTableBC.GetTimeTableListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<TTSubjectMaster>> GetTTSubjectWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return TimeTableBC.GetTTSubjectWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                TimeTableBC tbc = new TimeTableBC();
                tbc.CreateOrUpdateTTSubjectMaster(TTS);
                return TTS.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<TTStaffMaster>> GetTTStaffWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return TimeTableBC.GetTTStaffWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
                TimeTableBC tbc = new TimeTableBC();
                tbc.CreateOrUpdateTTStaffMaster(TTS);
                return TTS.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }
        public long CreateOrUpdateTTStaffConfig(TTStaffConfig TTConfig)
        {
            try
            {
                TimeTableBC tbc = new TimeTableBC();
                tbc.CreateOrUpdateTTStaffConfig(TTConfig);
                return TTConfig.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<TTStaffConfig>> GetTTStaffConfigWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return TimeTableBC.GetTTStaffConfigWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
       
    }
}
