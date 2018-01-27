using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.StoreEntities
{
    [DataContract]
    public class StudentWiseMaterialReport
    {
        [DataMember]
        public virtual long StudReportId { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual long StudId { get; set; }
        [DataMember]
        public virtual string NewId { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string Gender { get; set; }
        [DataMember]
        public virtual long MaterialSubGroupId { get; set; }
        [DataMember]
        public virtual long MaterialId { get; set; }
        [DataMember]
        public virtual long Tshirts { get; set; }
        [DataMember]
        public virtual long Shirts { get; set; }
        [DataMember]
        public virtual long Pant { get; set; }
        [DataMember]
        public virtual long TotalQty { get; set; }
      
        
        
    }
}
