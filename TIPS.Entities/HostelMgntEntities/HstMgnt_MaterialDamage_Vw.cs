using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.HostelMgntEntities
{
    public class HstMgnt_MaterialDamage_Vw
    {
        public virtual long Id { get; set; }
        public virtual long MDId { get; set; }
        public virtual long Stud_Id { get; set; }
        public virtual DateTime DateOfIncident { get; set; }
        public virtual string DetailsOfIncident { get; set; }
        public virtual long Amount { get; set; }
        public virtual string Remarks { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string Name { get; set; }
        public virtual string NewId { get; set; }
        public virtual string DummyRec { get; set; }

    }
}
