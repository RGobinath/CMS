using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.TransportEntities
{
    [DataContract]
    public class GPS_TrackingDeviceMaster
    {
        [DataMember]
        public virtual long GPS_TrackingDeviceMaster_Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string BrandName { get; set; }
        [DataMember]
        public virtual string ModelName { get; set; }
        [DataMember]
        public virtual string IMEINmber { get; set; }
        [DataMember]
        public virtual DateTime PurchaseDate { get; set; }
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
