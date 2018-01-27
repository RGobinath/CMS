using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.AssetEntities
{
 public   class AssetDistributionStudent_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string NewId { get; set; }
        //[DataMember]
        //public virtual string IdNum { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string AdmissionStatus { get; set; }
    }
}
