using System;
using System.Text;
using System.Collections.Generic;


namespace TIPS.Entities.StaffManagementEntities
{
    public class EmployeeSalaryDetails
    {
        public virtual int Id { get; set; }
        public virtual string EmployeeId { get; set; }
        public virtual string StaffUserName { get; set; }
        public virtual decimal? Basic_DA { get; set; }
        public virtual decimal? HRA { get; set; }
        public virtual decimal? MonthlyGross { get; set; }
        public virtual decimal? EmployerPF { get; set; }
        public virtual decimal? TotalCompensation { get; set; }
        public virtual decimal? LOP { get; set; }
        public virtual decimal? LOP_Basic { get; set; }
        public virtual decimal? LOP_HRA { get; set; }
        public virtual decimal? EarnedBasic { get; set; }
        public virtual decimal? Earned_LOP { get; set; }
        public virtual decimal? TotalEarnings { get; set; }
        public virtual decimal? PF { get; set; }
        public virtual decimal? TDS { get; set; }
        public virtual decimal? Mess { get; set; }
        public virtual decimal? EB { get; set; }
        public virtual decimal? Advance { get; set; }
        public virtual decimal? Fine { get; set; }
        public virtual decimal? Others { get; set; }
        public virtual decimal? Total { get; set; }
        public virtual decimal? Reimbursement { get; set; }
        public virtual decimal? TutorAllowance { get; set; }
        public virtual decimal? NetSalary { get; set; }
        public virtual string PaymentMode { get; set; }
        public virtual string BankAccNum { get; set; }
        public virtual string PFNo { get; set; }
       // public virtual Int32 MonthOfSalary { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
    }
}
