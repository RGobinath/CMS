using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.Entities.HRManagementEntities;
using TIPS.Component;
using TIPS.Entities.StaffManagementEntities;
//using TIPS.ServiceContract;

namespace TIPS.ServiceContract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "HRManagementService" in code, svc and config file together.
    public class HRManagementService : IHRManagementSC
    {
        //HRManagement Components Details...
        HRManagementBC hrbc = new HRManagementBC();

        // Leave Request Details...
        public long CreateOrUpdateLeaveRequest(LeaveRequest lrequest)
        {
            try
            {
                hrbc.CreateOrUpdateLeaveRequest(lrequest);
                return lrequest.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public LeaveRequest GetLeaveRequestById(long Id)
        {
            try
            {
                return hrbc.GetLeaveRequestById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<LeaveRequest>> GetLeavelistListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return hrbc.GetLeavelistListWithPaging(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
          
        // Staff Details...
        public StaffDetailsHRM GetStaffById(string userId)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    return hrbc.GetStaffById(userId);
                }
                else throw new Exception("User Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        // BankAccount Details...
        public BankAccount GetBankAccountById(long Id)
        {
            try
            {
                return hrbc.GetBankAccountById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long CreateOrUpdateBankAccount(BankAccount baccount)
        {
            try
            {
                hrbc.CreateOrUpdateBankAccount(baccount);
                return baccount.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        // Certificate Request Details...
        public CertificateRequest GetCertificateRequestById(long Id)
        {
            try
            {
                return hrbc.GetCertificateRequestById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long CreateOrUpdateCertificateRequest(CertificateRequest lreq)
        {
            try
            {
                hrbc.CreateOrUpdateCertificateRequest(lreq);
                return lreq.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        // Salary Advance Details...
        public SalaryAdvance GetSalaryAdvanceRequestById(long Id)
        {
            try
            {
                return hrbc.GetSalaryAdvanceRequestById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public long CreateOrUpdateSalaryAdvance(SalaryAdvance salary)
        {
            try
            {
                hrbc.CreateOrUpdateSalaryAdvance(salary);
                return salary.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public StaffDetailsHRM GetStaffusername(string ReportMngname)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(ReportMngname))
                {
                    return hrbc.GetStaffusername(ReportMngname);
                }
                else throw new Exception("User Id is required and it cannot be null or empty.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

    }
}
