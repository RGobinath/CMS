using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using TIPS.Component;
using TIPS.Entities.EmployeeEntities;

namespace TIPS.ServiceContract
{
    public class EmployeeService
    {
        #region Employee Attendance

        EmployeeBC ebc = new EmployeeBC();
        public long CreateOrUpdateEmployeeAttendanceList(EmployeeAttendance att)
        {
            try
            {
                EmployeeBC DriverAttendanceBC = new EmployeeBC();
                DriverAttendanceBC.CreateOrUpdateEmployeeAttendanceList(att);
                return att.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<EmployeeAttendance>> GetEmployeeAttendanceDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {

                return ebc.GetEmployeeAttendanceDetailsListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<EmployeeAttendanceReport>> GetEmployeeAttendanceReportDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {

                return ebc.GetEmployeeAttendanceReportDetailsListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateEmployeeOTList(EmployeeOTDetails ot)
        {
            try
            {
                EmployeeBC empBC = new EmployeeBC();
                empBC.CreateOrUpdateEmployeeOTList(ot);
                return ot.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }

        public Dictionary<long, IList<EmployeeOTDetails>> GetEmployeeOTDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {

                return ebc.GetEmployeeOTDetailsListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        public Dictionary<long, IList<EmployeeOTReport>> GetEmployeeOTReportDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {

                return ebc.GetEmployeeOTReportDetailsListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
