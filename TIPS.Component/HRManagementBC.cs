using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TIPS.Entities.HRManagementEntities;
using PersistenceFactory;
using TIPS.Entities.StaffManagementEntities;

namespace TIPS.Component
{
    public class HRManagementBC
    {
        PersistenceServiceFactory PSF = null;
        public HRManagementBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }

        // Leave Request Details...
        public long CreateOrUpdateLeaveRequest(LeaveRequest lrequest)
        {
            try
            {
                if (lrequest != null)
                    PSF.SaveOrUpdate<LeaveRequest>(lrequest);
                else { throw new Exception("HRManagement is required and it cannot be null.."); }
                return lrequest.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public LeaveRequest GetLeaveRequestById(long Id)
        {
            try
            {
                LeaveRequest lrequest = null;
                if (Id > 0)
                    lrequest = PSF.Get<LeaveRequest>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return lrequest;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<LeaveRequest>> GetLeavelistListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<LeaveRequest>> retValue = new Dictionary<long, IList<LeaveRequest>>();
                return PSF.GetListWithExactSearchCriteriaCount<LeaveRequest>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Staff Details...
        public StaffDetailsHRM GetStaffById(string userId)
        {
            try
            {
                return PSF.Get<StaffDetailsHRM>("StaffUserName", userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
       
        // Bank Account Details...
        public BankAccount GetBankAccountById(long Id)
        {
            try
            {
                BankAccount ba = null;
                if (Id > 0)
                    ba = PSF.Get<BankAccount>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return ba;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public long CreateOrUpdateBankAccount(BankAccount bacc)
        {
            try
            {
                if (bacc != null)
                    PSF.SaveOrUpdate<BankAccount>(bacc);
                else { throw new Exception("HRManagement is required and it cannot be null.."); }
                return bacc.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Certificate Details...
        public CertificateRequest GetCertificateRequestById(long Id)
        {
            try
            {
                CertificateRequest cr = null;
                if (Id > 0)
                    cr = PSF.Get<CertificateRequest>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return cr;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public long CreateOrUpdateCertificateRequest(CertificateRequest Crequest)
        {
            try
            {
                if (Crequest != null)
                    PSF.SaveOrUpdate<CertificateRequest>(Crequest);
                else { throw new Exception("HRManagement is required and it cannot be null.."); }
                return Crequest.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
       
        // Salary Advance Details...    
        public SalaryAdvance GetSalaryAdvanceRequestById(long Id)
        {
            try
            {
                SalaryAdvance cr = null;
                if (Id > 0)
                    cr = PSF.Get<SalaryAdvance>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return cr;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public long CreateOrUpdateSalaryAdvance(SalaryAdvance salary)
        {
            try
            {
                if (salary != null)
                    PSF.SaveOrUpdate<SalaryAdvance>(salary);
                else { throw new Exception("HRManagement is required and it cannot be null.."); }
                return salary.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public StaffDetailsHRM GetStaffusername(string ReportMngname)
        {
            try
            {
                return PSF.Get<StaffDetailsHRM>("Name", ReportMngname);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
