using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace TIPS.Entities.AdmissionEntities
{
    [DataContract]
    public class StudentLocalityMaster
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string LocalityCode { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string City { get; set; }
        [DataMember]
        public virtual string LocalityName { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime? ModifiedDate { get; set; }

        
    }
}
