using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace TIPS.Entities.AdmissionEntities
{
    [DataContract]
    public class AddressDetails
    {
        [DataMember]
        public virtual long Id { get; set; }

        [DataMember]
        public virtual long StudentId { get; set; }
      
        [DataMember]
        public virtual string AddressType { get; set; }     //Primary or secondary

  
        [DataMember]
        public virtual string Add1 { get; set; }

        [DataMember]
        public virtual string Add2 { get; set; }

        [DataMember]
        public virtual string Add3 { get; set; }

        //[Required]
        [DataMember]
        public virtual string City { get; set; }

        [DataMember]
        public virtual string State { get; set; }

        //[Required]
        [DataMember]
        public virtual string Country { get; set; }

        [DataMember]
        public virtual string Pin { get; set; }
  
        [DataMember]
        public virtual string Phone { get; set; }
        
    }
}
