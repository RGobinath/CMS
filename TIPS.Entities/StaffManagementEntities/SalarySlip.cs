using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.StaffManagementEntities
{
    public class SalarySlip
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string EmployeeCode { get; set; }
        [DataMember]
        public string Branch { get; set; }
        [DataMember]
        public string DateOfJoining { get; set; }
        [DataMember]
        public string Designation { get; set; }
        [DataMember]
        public string PFNumber { get; set; }
        [DataMember]
        public string PaymentBy { get; set; }
        [DataMember]
        public string AcNumber { get; set; }
        [DataMember]
        public decimal BasicPay { get; set; }
        [DataMember]
        public decimal DA { get; set; }
        [DataMember]
        public decimal BasicPay_DA { get; set; }
        [DataMember]
        public decimal HRA { get; set; }
        [DataMember]
        public decimal MonthlyGross { get; set; }
        [DataMember]
        public decimal EmployerPf { get; set; }
        [DataMember]
        public decimal PF { get; set; }
        [DataMember]
        public decimal TDS { get; set; }
        [DataMember]
        public decimal MessBill { get; set; }
        [DataMember]
        public decimal EbBill { get; set; }
        [DataMember]
        public decimal AdvancePayment { get; set; }
        [DataMember]
        public decimal OtherDeduction { get; set; }
        [DataMember]
        public decimal TotalCompensation { get; set; }
        [DataMember]
        public decimal TotalEarnings { get; set; }
        [DataMember]
        public decimal TotalDeduction { get; set; }
        [DataMember]
        public decimal Reimbursement { get; set; }
        [DataMember]
        public decimal TutorAllowance { get; set; }
        [DataMember]
        public decimal NetAmount { get; set; }
        [DataMember]
        public string TipsLogo { get; set; }
        [DataMember]
        public string TipsName { get; set; }
        [DataMember]
        public string TipsAddress { get; set; }
        [DataMember]
        public string NetAmountInWords { get; set; }
        [DataMember]
        public string FirstNetAmountInWords { get; set; }
        [DataMember]
        public string SecondNetAmountInWords { get; set; }
        [DataMember]
        public decimal LOP { get; set; }
        [DataMember]
        public decimal LOP_Basic { get; set; }
        [DataMember]
        public decimal LOP_HRA { get; set; }
        [DataMember]
        public decimal EarnedBasic { get; set; }
        [DataMember]
        public decimal Earned_LOP { get; set; }
        [DataMember]
        public string SalaryMonth { get; set; }
    }
}
