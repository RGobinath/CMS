using System;
using System.Text;
using System.Collections.Generic;


namespace TIPS.Entities.TransportEntities {
    
    public class TransportVendorMaster {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string DealerType { get; set; }
        public virtual string VendorType { get; set; }
        public virtual string VendorFor { get; set; }
        public virtual string PAN { get; set; }
        public virtual string TIN { get; set; }
        public virtual string FAX { get; set; }
        public virtual string ContactName { get; set; }
        public virtual string ContactNo { get; set; }
        public virtual string Email { get; set; }
        public virtual string Website { get; set; }
        public virtual string ReasonForSelecting { get; set; }
        public virtual string CreditDays { get; set; }
        public virtual bool? ApplicableForTDS { get; set; }
        public virtual string BankName { get; set; }
        public virtual string BankBranch { get; set; }
        public virtual string AccountName { get; set; }
        public virtual string AccountType { get; set; }
        public virtual string AccountNo { get; set; }
        public virtual string PIN { get; set; }
        public virtual string Add1 { get; set; }
        public virtual string Add2 { get; set; }
    }
}
