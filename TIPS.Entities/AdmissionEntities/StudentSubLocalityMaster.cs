using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace TIPS.Entities.AdmissionEntities
{
    [DataContract]
    public class StudentSubLocalityMaster
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long Locality_Id { get; set; }
        [DataMember]
        public virtual string SubLocalityCode { get; set; }
        [DataMember]
        public virtual string SubLocalityName { get; set; }
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
