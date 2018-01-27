using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.TransportEntities
{
    [DataContract]
    public class DriverFamilyDetails
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long DriverRegNo { get; set; }
        [DataMember]
        public virtual string FName { get; set; }
        [DataMember]
        public virtual DateTime FDob { get; set; }
        [DataMember]
        public virtual long FAge { get; set; }
        [DataMember]
        public virtual string ContactNo { get; set; }
        [DataMember]
        public virtual string FRelationship { get; set; }
        [DataMember]
        public virtual string FOccupation { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime CreatedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime ModifiedDate { get; set; }


    }
}
