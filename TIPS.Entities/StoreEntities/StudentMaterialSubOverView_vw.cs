using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.StoreEntities
{
    public class StudentMaterialSubOverView_vw
    {
        public virtual long Id { get; set; }
        public virtual string AcademicYear { get; set; }
        public virtual string MaterialSubGroup { get; set; }
        public virtual long MaterialSubGroupId { get; set; }
        public virtual long MaterialId { get; set; }
        public virtual string Material { get; set; }
        public virtual long IB_MAIN { get; set; }
        public virtual long IB_KG { get; set; }
        public virtual long Karur { get; set; }
        public virtual long Tirupur { get; set; }
        public virtual long Ernakulam { get; set; }
        public virtual long Chennai_Main { get; set; }
        public virtual long Tips_Saran { get; set; }
        public virtual long Chennai_City { get; set; }
        public virtual long Karur_KG { get; set; }
        public virtual long Tirupur_KG { get; set; }
        public virtual long CBSE_Main { get; set; }
        public virtual long Stock { get; set; }
    }
}
