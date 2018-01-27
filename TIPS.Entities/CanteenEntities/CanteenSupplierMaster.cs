using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.CanteenEntities
{
   [DataContract]
   public class CanteenSupplierMaster
    {
         [DataMember]
         public virtual long Id { get; set; }
         [DataMember]
         public virtual string FormCode { get; set; }
         [DataMember]
         public virtual string SupplierName { get; set; }
         [DataMember]
         public virtual string CompanyName { get; set; }
         [DataMember]
         public virtual string Address { get; set; }
         [DataMember]
         public virtual string City { get; set; }
         [DataMember]
         public virtual string State { get; set; }
         [DataMember]
         public virtual int? ZipCode { get; set; }
         [DataMember]
         public virtual string Country { get; set; }
         [DataMember]
         public virtual string Type { get; set; }
         [DataMember]
         public virtual string MobileNumber { get; set; }
         [DataMember]
         public virtual string PhoneNumber { get; set; }
         [DataMember]
         public virtual bool IsActive { get; set; }
         [DataMember]
         public virtual string TINNumber { get; set; }
         [DataMember]
         public virtual string PANNumber { get; set; }
         [DataMember]
         public virtual bool IsPreferredSupplier { get; set; }
         [DataMember]
         public virtual string Notes { get; set; }
         [DataMember]
         public virtual string Email { get; set; }
         [DataMember]
         public virtual string CreditTerms { get; set; }
    }
}
