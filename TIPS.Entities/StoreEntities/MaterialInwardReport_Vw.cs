using System;
using System.Text;
using System.Collections.Generic;


namespace TIPS.Entities.StoreEntities {
    
    public class MaterialInwardReport_Vw {
        public virtual long? Id { get; set; }
        public virtual string Campus { get; set; }        
        public virtual string MaterialGroup { get; set; }        
        public virtual string MaterialSubGroup { get; set; }
        public virtual string Material { get; set; }
        public virtual string Store { get; set; }
        public virtual int ReceivedQty { get; set; }
        public virtual int DamagedQty { get; set; }
        public virtual int Month { get; set; }
        public virtual int Year { get; set; }
        public virtual decimal? TotalPrice { get; set; }
    }
}
