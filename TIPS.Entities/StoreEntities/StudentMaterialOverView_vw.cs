using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.StoreEntities
{
    public class StudentMaterialOverView_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string MaterialSubGroup { get; set; }
        [DataMember]
        public virtual long  IB_MAIN { get; set; }
        [DataMember]  
        public virtual long  IB_KG { get; set; }
        [DataMember]  
        public virtual long  Tips_Saran { get; set; }
        [DataMember]    
        public virtual long  Chennai_Main { get; set; }
        [DataMember]    
        public virtual long  Chennai_City { get; set; }
        [DataMember]  
        public virtual long  Ernakulam { get; set; }
        [DataMember]   
        public virtual long  Karur { get; set; }
        [DataMember]   
        public virtual long  Karur_KG { get; set; }
        [DataMember]   
        public virtual long  Tirupur { get; set; }
        [DataMember]    
        public virtual long  Tirupur_KG { get; set; }
        [DataMember]
        public virtual long CBSE_Main { get; set; }
    }
}
