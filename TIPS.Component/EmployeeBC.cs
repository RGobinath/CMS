using System;
using System.Collections.Generic;
using System.Linq;
using PersistenceFactory;
using System.Text;
using TIPS.Entities.EmployeeEntities;
namespace TIPS.Component
{
    public class EmployeeBC
    {
         PersistenceServiceFactory PSF = null;
         public EmployeeBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }
         public long CreateOrUpdateEmployeeAttendanceList(EmployeeAttendance att)
         {
             try
             {
                 if (att != null)
                     PSF.SaveOrUpdate<EmployeeAttendance>(att);
                 else { throw new Exception("Value is required and it cannot be null.."); }
                 return att.Id;
             }
             catch (Exception)
             {
                 throw;
             }
         }

         public Dictionary<long, IList<EmployeeAttendance>> GetEmployeeAttendanceDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
         {
             try
             {
                 return PSF.GetListWithSearchCriteriaCount<EmployeeAttendance>(page, pageSize, sortType, sortBy, criteria);
             }

             catch (Exception)
             {

                 throw;
             }
         }

         public Dictionary<long, IList<EmployeeAttendanceReport>> GetEmployeeAttendanceReportDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
         {
             try
             {
                 return PSF.GetListWithSearchCriteriaCount<EmployeeAttendanceReport>(page, pageSize, sortType, sortBy, criteria);
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
                 if (ot != null)
                     PSF.SaveOrUpdate<EmployeeOTDetails>(ot);
                 else { throw new Exception("Value is required and it cannot be null.."); }
                 return ot.Id;
             }
             catch (Exception)
             {
                 throw;
             }
         }

         public Dictionary<long, IList<EmployeeOTDetails>> GetEmployeeOTDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
         {
             try
             {
                 return PSF.GetListWithSearchCriteriaCount<EmployeeOTDetails>(page, pageSize, sortType, sortBy, criteria);
             }

             catch (Exception)
             {

                 throw;
             }
         }

         public Dictionary<long, IList<EmployeeOTReport>> GetEmployeeOTReportDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
         {
             try
             {
                 return PSF.GetListWithSearchCriteriaCount<EmployeeOTReport>(page, pageSize, sortType, sortBy, criteria);
             }

             catch (Exception)
             {

                 throw;
             }
         }
    }
}
