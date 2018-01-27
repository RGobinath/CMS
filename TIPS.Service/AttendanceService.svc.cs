using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.Entities.Attendance;
using TIPS.Component;
using TIPS.ServiceContract;
using TIPS.Entities;
using TIPS.Entities.AdmissionEntities;

namespace TIPS.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AttendanceService" in code, svc and config file together.
    public class AttendanceService : IAttendanceSC
    {
        #region "Component Object Declaration"
        AttendanceBC attBCObj = new AttendanceBC();
        #endregion "End"


        public Dictionary<long, IList<UserAppRole>> GetAppRoleForAnUserListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    return attBCObj.GetAppRoleForAnUserListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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

        public Dictionary<long, IList<StudentAttendanceView>> GetStudentListListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    return attBCObj.GetStudentListListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
        public Dictionary<long, IList<Attendance>> GetStudentTemplateForAnAttendanceListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    return attBCObj.GetStudentTemplateForAnAttendanceListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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

        public Dictionary<long, IList<Attendance>> GetAbsentListForAnAttendanceListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    return attBCObj.GetAbsentListForAnAttendanceListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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

        public Dictionary<long, IList<Holidays>> GetHolidaysListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    return attBCObj.GetHolidaysListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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

        public long CreateOrUpdateAttendanceList(Attendance att)
        {
            try
            {
                attBCObj.CreateOrUpdateAttendanceList(att);
                return att.Id;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Attendance GetAttentanceById(long Id)
        {
            try
            {
                return attBCObj.GetAttentanceById(Id);

            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long DeleteAttendancevalue(Attendance att)
        {
            try
            {
                attBCObj.DeleteAttendancevalue(att);
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long CreateOrUpdateHolyDays(Holidays holyday)
        {
            try
            {
                attBCObj.CreateOrUpdateHolyDays(holyday);
                return holyday.Id;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long CreateOrUpdateEmailLog(EmailLog el)
        {
            try
            {
                attBCObj.CreateOrUpdateEmailLog(el);
                return el.Id;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<FamilyDetails>> GetFamilyDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return attBCObj.GetFamilyDetailsListWithPagingAndCriteria(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long CreateOrUpdateSMSLog(SMS sms)
        {
            try
            {
                attBCObj.CreateOrUpdateSMSLog(sms);
                return sms.Id;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public Holidays GetHolidaysById(long Id)
        {
            try
            {
                return attBCObj.GetHolidaysById(Id);

            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public long DeleteHolidaysList(Holidays hd)
        {
            try
            {
                attBCObj.DeleteHolidaysList(hd);
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<MonthMasterForAttendance>> GetMonthMasterForAttendanceListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    return attBCObj.GetMonthMasterForAttendanceListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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

        #region "Attendance Report Details"
        public long CreateOrUpdateAttendanceMonitor(AttendanceMonitor attMonObj)
        {
            try
            {
                attBCObj.CreateOrUpdateAttendanceMonitor(attMonObj);
                return attMonObj.Id;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<AttendanceMonitor>> GetAttendanceMonitorListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    return attBCObj.GetAttendanceMonitorListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
        public Dictionary<long, IList<EmailConfiguration>> GetEmailConfigurationListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                try
                {
                    return attBCObj.GetEmailConfigurationListWithPagingAndCriteria(page, pageSize, sortby, sortType, criteria);
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
        public long SaveOrUpdateEmailConfiguration(EmailConfiguration mailObj)
        {
            try
            {
                attBCObj.SaveOrUpdateEmailConfiguration(mailObj);
                return mailObj.Id;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long DeleteEmailConfigurationList(string[] Ids)
        {
            try
            {
                attBCObj.DeleteEmailConfigurationList(Ids);
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        public void AttendanceFinishAndNotFinishServiceFunc()
        {
            try
            {
                attBCObj.AttendanceFinishAndNotFinishServiceFunc();
            }
            catch (Exception)
            {
                
                throw;
            }
            finally { }
        }

        public void AttendancePresentAndAbsentServiceFunc()
        {
            try
            {
                attBCObj.AttendancePresentAndAbsentServiceFunc();
            }
            catch (Exception)
            {
                throw;
            }
            finally { }


        }

        public long CreateAttendanceList(Attendance att)
        {
            try
            {
                attBCObj.CreateAttendanceList(att);
                return att.Id;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

        #endregion "End"

        public long SaveOrUpdateHolyDays(Holidays holyday)
        {
            try
            {
                attBCObj.SaveOrUpdateHolyDays(holyday);
                return holyday.Id;
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }

    }
}
