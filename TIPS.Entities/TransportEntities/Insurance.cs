using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.TransportEntities
{
    public class Insurance
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual int RefId { get; set; }
        [DataMember]
        public virtual long VehicleId { get; set; }
        [DataMember]
        public virtual string Type { get; set; }
        [DataMember]
        public virtual string VehicleNo { get; set; }
        [DataMember]
        public virtual DateTime? InsuranceDate { get; set; }
        [DataMember]
        public virtual DateTime? NextInsuranceDate { get; set; }
        [DataMember]
        public virtual string InsuranceProvider { get; set; }
        [DataMember]
        public virtual long InsuranceDeclaredValue { get; set; }
        //[DataMember]
        //public virtual DateTime? ValidityFromDate { get; set; }
        //[DataMember]
        //public virtual DateTime? ValidityToDate { get; set; }
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual string ICertificate { get; set; }
        [DataMember]
        public virtual string InsuranceConsultantName { get; set; }

        //public virtual DateTime? InsTaxValidUpto { get; set; }
    }
}
