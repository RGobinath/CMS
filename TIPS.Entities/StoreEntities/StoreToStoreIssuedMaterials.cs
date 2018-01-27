using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace TIPS.Entities.StoreEntities {
    
    public class StoreToStoreIssuedMaterials {
        public virtual int Id { get; set; }
        public virtual int? IssueId { get; set; }
        public virtual string MaterialGroup { get; set; }
        public virtual string MaterialSubGroup { get; set; }
        public virtual string Material { get; set; }
        public virtual string Units { get; set; }
        public virtual int? IssuedQty { get; set; }

        [DataMember]
        public virtual string InwardIds { get; set; }
        [DataMember]
        public virtual string AvailableQtys { get; set; }
        [DataMember]
        public virtual string UnitPrices { get; set; }
        [DataMember]
        public virtual decimal TotalPrice{ get; set; }

    }
}
