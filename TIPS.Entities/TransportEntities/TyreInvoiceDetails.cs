using System;
using System.Text;
using System.Collections.Generic;


namespace TIPS.Entities.TransportEntities {
    
    public class TyreInvoiceDetails {
        public virtual int Id { get; set; }
        public virtual string RefNo { get; set; }
        public virtual string Campus { get; set; }
        public virtual DateTime? PurchasedDate { get; set; }
        public virtual string PurchasedFrom { get; set; }
        public virtual string PurchasedBy { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual string PaymentType { get; set; }
        public virtual decimal? TotalCost { get; set; }
        public virtual decimal? TaxPercentage { get; set; }
        public virtual decimal? TaxAmount { get; set; }
        public virtual decimal? OtherExpenses { get; set; }
        public virtual decimal? GrandTotal { get; set; }
        public virtual decimal? RoundedOffCost { get; set; }
        public virtual string Status { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
    }
}
