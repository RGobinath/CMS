using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.AdmissionEntities
{
   public  class AcademicYearReport
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string ShowAcademicYear { get; set; }
        [DataMember]
        public virtual string FeeStYear { get; set; }
        [DataMember]   
        public virtual string IBMain { get; set; }
        [DataMember] 
        public virtual string IBKG { get; set; }
        [DataMember]  
        public virtual string TipsSaran { get; set; }
        [DataMember]  
        public virtual string ChennaiMain { get; set; }
        [DataMember]  
        public virtual string ChennaiCity { get; set; }
        [DataMember]
        public virtual string Ernakulam { get; set; }
        [DataMember]   
        public virtual string ErnakulamKG { get; set; }
        [DataMember]   
        public virtual string Karur { get; set; }
        [DataMember]  
        public virtual string KarurKG { get; set; }
        [DataMember]  
        public virtual string Tirupur { get; set; }
        [DataMember]   
       public virtual string TirupurKG { get; set; }
    }                  
}
