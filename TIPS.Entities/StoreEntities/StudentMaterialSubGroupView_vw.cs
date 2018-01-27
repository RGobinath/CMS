using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.StoreEntities
{
    public class StudentMaterialSubGroupView_vw
    {
        public virtual long Id { get; set; }
        public virtual string Campus { get; set; }
        public virtual string AcademicYear { get; set; }
        public virtual long MaterialSubGroupId { get; set; }
        public virtual long MaterialId { get; set; }
        public virtual string MaterialSubGroup { get; set; }
        public virtual string Material { get; set; }
        public virtual long Total { get; set; }
    }
}
