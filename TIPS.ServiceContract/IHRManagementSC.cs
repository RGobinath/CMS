using System.Collections.Generic;
using TIPS.Entities.HRManagementEntities;
using TIPS.Entities.StaffManagementEntities;

namespace TIPS.ServiceContract
{
    public interface IHRManagementSC
    {
        //Leave Request Details...
        long CreateOrUpdateLeaveRequest(LeaveRequest lrequest);
        LeaveRequest GetLeaveRequestById(long Id);
        Dictionary<long, IList<LeaveRequest>> GetLeavelistListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria);

        // BankAccount Details...
        long CreateOrUpdateBankAccount(BankAccount bank);
        BankAccount GetBankAccountById(long Id);
        //Dictionary<long, IList<BankAccount>> GetBankAccountListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria);

        long CreateOrUpdateCertificateRequest(CertificateRequest Crequest);
        CertificateRequest GetCertificateRequestById(long Id);
        //EmploymentRequestEntities GetHRManagementById(long Id);


        SalaryAdvance GetSalaryAdvanceRequestById(long Id);
        long CreateOrUpdateSalaryAdvance(SalaryAdvance Srequest);

        // Staff table Details ....
        StaffDetailsHRM GetStaffById(string userId);
        //Dictionary<long, IList<EmploymentRequestEntities>> GetHRDetailsListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria);

    }

}
