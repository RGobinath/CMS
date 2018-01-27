using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.AdmissionEntities
{
    public class PaymentDetails
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long StudentId { get; set; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string ApplicationDate { get; set; }
        [DataMember]
        public virtual string ApplicationNo { get; set; }
        [DataMember]
        public virtual string AppliedCampus { get; set; }
        [DataMember]
        public virtual string AppliedGrade { get; set; }
        [DataMember]
        public virtual string ModeOfPayment { get; set; }
        [DataMember]
        public virtual string Amount { get; set; }
        [DataMember]
        public virtual string FeeType { get; set; }
        [DataMember]
        public virtual string ReferenceNo { get; set; }
        [DataMember]
        public virtual DateTime? ChequeDate { get; set; }
        [DataMember]
        public virtual string BankName { get; set; }
        [DataMember]
        public virtual string Remarks { get; set; }
        [DataMember]
        public virtual DateTime? PaidDate { get; set; }
        [DataMember]
        public virtual DateTime? ClearedDate { get; set; }
        [DataMember]
        public virtual string FeePaidStatus { get; set; }
        [DataMember]
        public virtual DateTime CreatedDate { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime ModifiedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
    }
}
